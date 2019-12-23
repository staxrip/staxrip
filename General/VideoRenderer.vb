
Imports System.Runtime.InteropServices
Imports DirectN

Public Class VideoRenderer
    Implements IDisposable

    Property CropLeft As Integer
    Property CropTop As Integer
    Property CropRight As Integer
    Property CropBottom As Integer

    Private Factory As ComObject(Of ID2D1Factory)
    Private RenderTarget As ID2D1HwndRenderTarget
    Private DeviceContext As ID2D1DeviceContext
    Private AVIFile As AVIFile
    Private Control As Control

    Sub New(ctrl As Control, avi As AVIFile)
        AVIFile = avi
        Control = ctrl

        Factory = D2D1Functions.D2D1CreateFactory(
            D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_SINGLE_THREADED)

        AddHandler ctrl.ClientSizeChanged, AddressOf Resize
    End Sub

    Sub Resize(sender As Object, e As EventArgs)
        Dim bitmapSize = New D2D_SIZE_U With {
                .width = CUInt(Control.ClientSize.Width),
                .height = CUInt(Control.ClientSize.Height)}

        RenderTarget?.Resize(bitmapSize)
    End Sub

    Sub Draw()
        CreateGraphicsResources()

        Try
            Dim dib = AVIFile.GetDIB

            If dib = IntPtr.Zero Then
                Exit Sub
            End If

            DeviceContext.BeginDraw()

            If CropLeft + CropTop + CropRight + CropBottom = 0 Then
                Dim destinationRectangle = New D2D_RECT_F With {
                    .right = Control.ClientSize.Width,
                    .bottom = Control.ClientSize.Height}

                DrawBitmap(CreateBitmapFromDIB(dib), ScaleRectangle(destinationRectangle))
            Else
                Clear(Color.White)

                Dim bitmapHeader = Marshal.PtrToStructure(Of BITMAPINFOHEADER)(dib)
                Dim bitmapSize = New Size(bitmapHeader.biWidth, bitmapHeader.biHeight)

                Dim scaleX = Control.Width / bitmapSize.Width
                Dim scaleY = Control.Height / bitmapSize.Height

                Dim left = CInt(CropLeft * scaleX)
                Dim right = CInt(CropRight * scaleX)
                Dim top = CInt(CropTop * scaleY)
                Dim bottom = CInt(CropBottom * scaleY)

                Dim destinationRectangle As Rectangle
                destinationRectangle.X = left
                destinationRectangle.Y = CInt(CropTop * scaleY)
                destinationRectangle.Width = Control.Width - left - right
                destinationRectangle.Height = Control.Height - top - bottom

                Dim sourceRectangle As Rectangle
                sourceRectangle.X = CropLeft
                sourceRectangle.Y = CropTop
                sourceRectangle.Width = bitmapSize.Width - CropLeft - CropRight
                sourceRectangle.Height = bitmapSize.Height - CropTop - CropBottom

                DrawBitmap(CreateBitmapFromDIB(dib),
                           ConvertRectangle(sourceRectangle),
                           ScaleRectangle(ConvertRectangle(destinationRectangle)))
            End If

            DeviceContext.EndDraw()
        Catch
            ReleaseGraphicsResources()
        End Try
    End Sub

    Sub Clear(color As Color)
        Dim value = ConvertColor(color)
        Dim ptr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(_D3DCOLORVALUE)))
        Marshal.StructureToPtr(Of _D3DCOLORVALUE)(value, ptr, False)
        DeviceContext.Clear(ptr)
        Marshal.FreeHGlobal(ptr)
    End Sub

    Function ConvertColor(color As Color) As _D3DCOLORVALUE
        Dim value As _D3DCOLORVALUE
        value.r = color.R / 255.0!
        value.g = color.G / 255.0!
        value.b = color.B / 255.0!
        value.a = color.A / 255.0!
        Return value
    End Function

    Function ScaleRectangle(rect As D2D_RECT_F) As D2D_RECT_F
        rect.left = (rect.left * 96) / Control.DeviceDpi
        rect.top = (rect.top * 96) / Control.DeviceDpi
        rect.right = (rect.right * 96) / Control.DeviceDpi
        rect.bottom = (rect.bottom * 96) / Control.DeviceDpi
        Return rect
    End Function

    Function ConvertRectangle(inputRect As Rectangle) As D2D_RECT_F
        Dim outputRect As D2D_RECT_F
        outputRect.left = inputRect.Left
        outputRect.top = inputRect.Top
        outputRect.right = inputRect.Left + inputRect.Width
        outputRect.bottom = inputRect.Top + inputRect.Height
        Return outputRect
    End Function

    Sub DrawBitmap(bitmap As ID2D1Bitmap, destinationRectangle As D2D_RECT_F)
        Dim destinationRectanglePtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(D2D_RECT_F)))
        Marshal.StructureToPtr(Of D2D_RECT_F)(destinationRectangle, destinationRectanglePtr, False)

        DeviceContext.DrawBitmap(bitmap, destinationRectanglePtr, 1,
            D2D1_INTERPOLATION_MODE.D2D1_INTERPOLATION_MODE_HIGH_QUALITY_CUBIC,
            IntPtr.Zero, IntPtr.Zero)

        Marshal.FreeHGlobal(destinationRectanglePtr)
        Marshal.ReleaseComObject(bitmap)
    End Sub

    Sub DrawBitmap(bitmap As ID2D1Bitmap, sourceRectangle As D2D_RECT_F, destinationRectangle As D2D_RECT_F)
        Dim sourceRectanglePtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(D2D_RECT_F)))
        Marshal.StructureToPtr(Of D2D_RECT_F)(sourceRectangle, sourceRectanglePtr, False)

        Dim destinationRectanglePtr = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(D2D_RECT_F)))
        Marshal.StructureToPtr(Of D2D_RECT_F)(destinationRectangle, destinationRectanglePtr, False)

        DeviceContext.DrawBitmap(bitmap, destinationRectanglePtr, 1,
            D2D1_INTERPOLATION_MODE.D2D1_INTERPOLATION_MODE_HIGH_QUALITY_CUBIC,
            sourceRectanglePtr, IntPtr.Zero)

        Marshal.FreeHGlobal(sourceRectanglePtr)
        Marshal.FreeHGlobal(destinationRectanglePtr)
        Marshal.ReleaseComObject(bitmap)
    End Sub

    Function CreateBitmapFromDIB(dib As IntPtr) As ID2D1Bitmap
        Dim bitmapHeader = Marshal.PtrToStructure(Of BITMAPINFOHEADER)(dib)
        Dim pixelData = dib + Marshal.SizeOf(GetType(BITMAPINFOHEADER))
        Dim pitch = CUInt((((bitmapHeader.biWidth * bitmapHeader.biBitCount) + 31) And Not 31) >> 3)
        Dim bitmapProperties As D2D1_BITMAP_PROPERTIES

        bitmapProperties.pixelFormat = New D2D1_PIXEL_FORMAT() With {
            .alphaMode = D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_IGNORE,
            .format = DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM}

        bitmapProperties.dpiX = 96
        bitmapProperties.dpiY = 96

        Dim bitmapSize = New D2D_SIZE_U With {
            .width = CUInt(bitmapHeader.biWidth),
            .height = CUInt(bitmapHeader.biHeight)}

        Dim bitmap As ID2D1Bitmap
        DeviceContext.CreateBitmap(bitmapSize, pixelData, pitch, bitmapProperties, bitmap).ThrowOnError()
        Return bitmap
    End Function

    Sub CreateGraphicsResources()
        If RenderTarget Is Nothing Then
            Dim hwndRenderTargetProps = New D2D1_HWND_RENDER_TARGET_PROPERTIES With {
                .hwnd = Control.Handle,
                .pixelSize = New D2D_SIZE_U(Control.ClientSize.Width, Control.ClientSize.Height)}

            Factory.Object.CreateHwndRenderTarget(
                New D2D1_RENDER_TARGET_PROPERTIES,
                hwndRenderTargetProps,
                RenderTarget).ThrowOnError()

            DeviceContext = CType(RenderTarget, ID2D1DeviceContext)
        End If
    End Sub

    Sub ReleaseGraphicsResources()
        If Not DeviceContext Is Nothing Then
            Marshal.ReleaseComObject(DeviceContext)
            DeviceContext = Nothing
        End If

        If Not RenderTarget Is Nothing Then
            Marshal.ReleaseComObject(RenderTarget)
            RenderTarget = Nothing
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        ReleaseGraphicsResources()
        Factory.Dispose()
        RemoveHandler Control.ClientSizeChanged, AddressOf Resize
    End Sub
End Class