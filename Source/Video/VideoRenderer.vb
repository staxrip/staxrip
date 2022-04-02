
Imports System.Runtime.InteropServices

Imports DirectN
Imports StaxRip.UI

Public Class VideoRenderer
    Implements IDisposable

    Property CropLeft As Integer
    Property CropTop As Integer
    Property CropRight As Integer
    Property CropBottom As Integer
    Property ShowInfo As Boolean
    Property Info As ServerInfo

    Private Direct2dFactory As ComObject(Of ID2D1Factory)
    Private DirectWriteFactory As ComObject(Of IDWriteFactory)
    Private RenderTarget As ID2D1HwndRenderTarget
    Private DeviceContext As ID2D1DeviceContext
    Private Server As IFrameServer
    Private Control As Control

    Private PositionValue As Integer

    Property Position As Integer
        Get
            Return PositionValue
        End Get
        Set(value As Integer)
            PositionValue = value

            If PositionValue < 0 Then
                PositionValue = 0
            End If

            If PositionValue > Server.Info.FrameCount - 1 Then
                PositionValue = Server.Info.FrameCount - 1
            End If
        End Set
    End Property


    Private _backColor As ColorHSL = Color.Empty

    Public Property BackColor As ColorHSL
        Get
            Return _backColor
        End Get
        Set(value As ColorHSL)
            _backColor = value
        End Set
    End Property


    Sub New(control As Control, server As IFrameServer)
        Me.Server = server
        Me.Control = control
        Info = server.Info

        Direct2dFactory = D2D1Functions.D2D1CreateFactory(
            D2D1_FACTORY_TYPE.D2D1_FACTORY_TYPE_SINGLE_THREADED)

        DirectWriteFactory = DWriteFunctions.DWriteCreateFactory(Of IDWriteFactory)(
            DWRITE_FACTORY_TYPE.DWRITE_FACTORY_TYPE_SHARED)

        ApplyTheme()

        AddHandler control.ClientSizeChanged, AddressOf Resize
        AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
    End Sub

    Sub OnThemeChanged(theme As Theme)
        ApplyTheme(theme)
    End Sub

    Sub ApplyTheme()
        ApplyTheme(ThemeManager.CurrentTheme)
    End Sub

    Sub ApplyTheme(theme As Theme)
        If DesignHelp.IsDesignMode Then
            Exit Sub
        End If

        If s.CropColor = Color.Empty Then
            BackColor = theme.General.Controls.VideoRenderer.BackColor
        Else
            BackColor = s.CropColor
        End If
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
            Dim pitch As Integer
            Dim pixelPtr As IntPtr

            If Server.GetFrame(Position, pixelPtr, pitch) <> 0 OrElse pixelPtr = IntPtr.Zero Then
                Exit Sub
            End If

            DeviceContext.BeginDraw()

            If CropLeft + CropTop + CropRight + CropBottom <> 0 Then
                Clear(BackColor)
            End If

            Dim scaleX = Control.Width / Server.Info.Width
            Dim scaleY = Control.Height / Server.Info.Height

            Dim left = CropLeft * scaleX
            Dim right = CropRight * scaleX
            Dim top = CropTop * scaleY
            Dim bottom = CropBottom * scaleY

            Dim destinationRectangle As RectangleF
            destinationRectangle.X = CSng(left)
            destinationRectangle.Y = CSng(CropTop * scaleY)
            destinationRectangle.Width = CSng(Control.Width - left - right)
            destinationRectangle.Height = CSng(Control.Height - top - bottom)

            Dim sourceRectangle As RectangleF
            sourceRectangle.X = CropLeft
            sourceRectangle.Y = CropTop
            sourceRectangle.Width = Server.Info.Width - CropLeft - CropRight
            sourceRectangle.Height = Server.Info.Height - CropTop - CropBottom

            DrawBitmap(CreateBitmap(pixelPtr, pitch),
                       ConvertRectangle(sourceRectangle),
                       ScaleRectangle(ConvertRectangle(destinationRectangle)))

            DrawInfoText()

            DeviceContext.EndDraw()
        Catch
            ReleaseGraphicsResources()
        End Try
    End Sub

    Sub DrawInfoText()
        If Not ShowInfo Then
            Exit Sub
        End If

        Dim text = Info.GetInfoText(Position)
        Dim layout As IDWriteTextLayout = Nothing

        Dim fontName As String

        Using font = g.GetCodeFont
            fontName = font.Name
        End Using

        Dim format = DirectWriteFactory.CreateTextFormat(fontName, 13)

        DirectWriteFactory.Object.CreateTextLayout(
            text,
            CUInt(text.Length),
            format.Object,
            Scale(Control.Width),
            Scale(Control.Height),
            layout)

        Dim backColor As _D3DCOLORVALUE
        backColor.a = 0.7
        Dim backBrush = DeviceContext.CreateSolidColorBrush(Of ID2D1SolidColorBrush)(backColor)
        Dim metrics As DWRITE_TEXT_METRICS
        layout.GetMetrics(metrics)
        Dim padding = Scale(Control.Font.Height) / 3
        Dim rect As D2D_RECT_F
        rect.left = Scale(Control.Font.Height) / 2
        rect.top = rect.left
        rect.right = rect.left + metrics.width + padding * 2
        rect.bottom = rect.top + metrics.height + padding * 2
        DeviceContext.FillRectangle(rect, backBrush.Object)
        Dim foreColor As _D3DCOLORVALUE
        foreColor.r = 1
        foreColor.g = 1
        foreColor.b = 1
        foreColor.a = 1
        Dim foreBrush = DeviceContext.CreateSolidColorBrush(Of ID2D1SolidColorBrush)(foreColor)
        rect.left += padding
        rect.top += padding
        rect.right += padding
        rect.bottom += padding
        DeviceContext.DrawText(text, format.Object, rect, foreBrush.Object)
        foreBrush.Dispose()
        backBrush.Dispose()
        Marshal.ReleaseComObject(layout)
        format.Dispose()
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

    Function Scale(value As Double) As Single
        Return CSng((value * 96) / Control.DeviceDpi)
    End Function

    Function ScaleRectangle(rect As D2D_RECT_F) As D2D_RECT_F
        rect.left = Scale(rect.left)
        rect.top = Scale(rect.top)
        rect.right = Scale(rect.right)
        rect.bottom = Scale(rect.bottom)
        Return rect
    End Function

    Function ConvertRectangle(inputRect As RectangleF) As D2D_RECT_F
        Dim outputRect As D2D_RECT_F
        outputRect.left = inputRect.Left
        outputRect.top = inputRect.Top
        outputRect.right = inputRect.Left + inputRect.Width
        outputRect.bottom = inputRect.Top + inputRect.Height
        Return outputRect
    End Function

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

    Function CreateBitmap(pixelPtr As IntPtr, pitch As Integer) As ID2D1Bitmap
        Dim bitmapProperties As D2D1_BITMAP_PROPERTIES

        bitmapProperties.pixelFormat = New D2D1_PIXEL_FORMAT() With {
            .alphaMode = D2D1_ALPHA_MODE.D2D1_ALPHA_MODE_IGNORE,
            .format = DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM}

        bitmapProperties.dpiX = 96
        bitmapProperties.dpiY = 96

        Dim bitmapSize = New D2D_SIZE_U With {
            .width = CUInt(Server.Info.Width),
            .height = CUInt(Server.Info.Height)}

        Dim bitmap As ID2D1Bitmap = Nothing
        DeviceContext.CreateBitmap(bitmapSize, pixelPtr, CUInt(pitch), bitmapProperties, bitmap).ThrowOnError()
        Return bitmap
    End Function

    Sub CreateGraphicsResources()
        If RenderTarget Is Nothing Then
            Dim hwndRenderTargetProps = New D2D1_HWND_RENDER_TARGET_PROPERTIES With {
                .hwnd = Control.Handle,
                .pixelSize = New D2D_SIZE_U(Control.ClientSize.Width, Control.ClientSize.Height)}

            Direct2dFactory.Object.CreateHwndRenderTarget(
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

    Sub Dispose() Implements IDisposable.Dispose
        ReleaseGraphicsResources()
        Direct2dFactory.Dispose()
        DirectWriteFactory.Dispose()
        RemoveHandler Control.ClientSizeChanged, AddressOf Resize
    End Sub
End Class
