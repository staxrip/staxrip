
Imports System.Runtime.InteropServices
Imports DirectN

Public Class VideoRenderer
    Implements IDisposable

    Private Factory As ComObject(Of ID2D1Factory)
    Private RenderTarget As ID2D1HwndRenderTarget
    Private DeviceContext As ID2D1DeviceContext
    Private AVIFile As AVIFile
    Private Control As Control

    Sub New(ctrl As Control, avi As AVIFile)
        AVIFile = avi
        Control = ctrl
        Factory = D2D1Functions.D2D1CreateFactory(D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_SINGLE_THREADED)
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
            DeviceContext.BeginDraw()

            Dim dibPointer = AVIFile.GetDIB

            If dibPointer = IntPtr.Zero Then
                Exit Sub
            End If

            Dim bitmapHeader = Marshal.PtrToStructure(Of BITMAPINFOHEADER)(dibPointer)
            Dim pixelPointer = dibPointer + Marshal.SizeOf(GetType(BITMAPINFOHEADER))
            Dim pitch = CUInt((((bitmapHeader.biWidth * bitmapHeader.biBitCount) + 31) And Not 31) >> 3)
            Dim bitmapProperties As D2D1_BITMAP_PROPERTIES

            bitmapProperties.pixelFormat = New D2D1_PIXEL_FORMAT() With {
                .alphaMode = D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_IGNORE,
                .format = DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM}

            Dim bitmapSize = New D2D_SIZE_U With {
                .width = CUInt(bitmapHeader.biWidth),
                .height = CUInt(bitmapHeader.biHeight)}

            Dim bitmap As ID2D1Bitmap
            DeviceContext.CreateBitmap(bitmapSize, pixelPointer, pitch, bitmapProperties, bitmap).ThrowOnError()

            Dim rectDestination = New D2D_RECT_F With {
                .right = CSng((Control.ClientSize.Width * 96) / Control.DeviceDpi),
                .bottom = CSng((Control.ClientSize.Height * 96) / Control.DeviceDpi)}

            Dim rectDestinationPointer = Marshal.AllocHGlobal(Marshal.SizeOf(GetType(D2D_RECT_F)))
            Marshal.StructureToPtr(Of D2D_RECT_F)(rectDestination, rectDestinationPointer, False)

            DeviceContext.DrawBitmap(
                bitmap,
                rectDestinationPointer,
                1,
                D2D1_INTERPOLATION_MODE.D2D1_INTERPOLATION_MODE_HIGH_QUALITY_CUBIC,
                IntPtr.Zero,
                IntPtr.Zero)

            Marshal.FreeHGlobal(rectDestinationPointer)
            Marshal.ReleaseComObject(bitmap)

            DeviceContext.EndDraw()
        Catch
            ReleaseGraphicsResources()
        End Try
    End Sub

    Sub CreateGraphicsResources()
        If RenderTarget Is Nothing Then
            Dim hwndRenderTargetProps = New D2D1_HWND_RENDER_TARGET_PROPERTIES With {
                .hwnd = Control.Handle,
                .pixelSize = New D2D_SIZE_U(Control.ClientSize.Width, Control.ClientSize.Height)}

            Factory.Object.CreateHwndRenderTarget(New D2D1_RENDER_TARGET_PROPERTIES,
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