﻿
Imports System.ComponentModel

Imports StaxRip.UI

Public Class CropForm
    Inherits DialogBase

#Region " Designer "
    Private components As System.ComponentModel.IContainer

    Friend WithEvents pnLeftActive As PanelEx
    Friend WithEvents pnTopActive As PanelEx
    Friend WithEvents pnBottomActive As PanelEx
    Friend WithEvents pnRightActive As PanelEx
    Friend WithEvents tbPosition As TrackBar
    Friend WithEvents pnVideo As PanelEx
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents tsbMenu As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents laStatus As System.Windows.Forms.ToolStripStatusLabel

    <System.Diagnostics.DebuggerStepThrough()> Sub InitializeComponent()
        Me.pnLeftActive = New PanelEx()
        Me.pnTopActive = New PanelEx()
        Me.pnBottomActive = New PanelEx()
        Me.pnRightActive = New PanelEx()
        Me.pnVideo = New PanelEx()
        Me.tbPosition = New System.Windows.Forms.TrackBar()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.laStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsbMenu = New System.Windows.Forms.ToolStripDropDownButton()
        CType(Me.tbPosition, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnLeftActive
        '
        Me.pnLeftActive.BackColor = System.Drawing.SystemColors.InfoText
        Me.pnLeftActive.Location = New System.Drawing.Point(24, 56)
        Me.pnLeftActive.Name = "pnLeftActive"
        Me.pnLeftActive.Size = New System.Drawing.Size(20, 81)
        Me.pnLeftActive.TabIndex = 0
        '
        'pnTopActive
        '
        Me.pnTopActive.BackColor = System.Drawing.SystemColors.InfoText
        Me.pnTopActive.Location = New System.Drawing.Point(64, 15)
        Me.pnTopActive.Name = "pnTopActive"
        Me.pnTopActive.Size = New System.Drawing.Size(152, 20)
        Me.pnTopActive.TabIndex = 1
        '
        'pnBottomActive
        '
        Me.pnBottomActive.BackColor = System.Drawing.SystemColors.InfoText
        Me.pnBottomActive.Location = New System.Drawing.Point(64, 184)
        Me.pnBottomActive.Name = "pnBottomActive"
        Me.pnBottomActive.Size = New System.Drawing.Size(152, 20)
        Me.pnBottomActive.TabIndex = 3
        '
        'pnRightActive
        '
        Me.pnRightActive.BackColor = System.Drawing.SystemColors.InfoText
        Me.pnRightActive.Location = New System.Drawing.Point(256, 56)
        Me.pnRightActive.Name = "pnRightActive"
        Me.pnRightActive.Size = New System.Drawing.Size(20, 88)
        Me.pnRightActive.TabIndex = 4
        '
        'pVideo
        '
        Me.pnVideo.BackColor = System.Drawing.Color.Black
        Me.pnVideo.Location = New System.Drawing.Point(64, 55)
        Me.pnVideo.Name = "pVideo"
        Me.pnVideo.Size = New System.Drawing.Size(149, 89)
        Me.pnVideo.TabIndex = 2
        '
        'tbPosition
        '
        Me.tbPosition.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbPosition.AutoSize = False
        Me.tbPosition.Location = New System.Drawing.Point(12, 266)
        Me.tbPosition.Name = "tbPosition"
        Me.tbPosition.Size = New System.Drawing.Size(711, 61)
        Me.tbPosition.TabIndex = 5
        Me.tbPosition.TabStop = False
        Me.tbPosition.TickStyle = System.Windows.Forms.TickStyle.None
        '
        'StatusStrip
        '
        Me.StatusStrip.ImageScalingSize = New System.Drawing.Size(48, 48)
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.laStatus, Me.tsbMenu})
        Me.StatusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.StatusStrip.Location = New System.Drawing.Point(0, 330)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(735, 63)
        Me.StatusStrip.TabIndex = 6
        Me.StatusStrip.Text = "StatusStrip"
        '
        'laStatus
        '
        Me.laStatus.Name = "laStatus"
        Me.laStatus.Size = New System.Drawing.Size(34, 48)
        Me.laStatus.Text = "-"
        '
        'tsbMenu
        '
        Me.tsbMenu.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbMenu.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbMenu.Name = "tsbMenu"
        Me.tsbMenu.Size = New System.Drawing.Size(142, 57)
        Me.tsbMenu.Text = "Menu"
        '
        'CropForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(735, 393)
        Me.Controls.Add(Me.StatusStrip)
        Me.Controls.Add(Me.tbPosition)
        Me.Controls.Add(Me.pnVideo)
        Me.Controls.Add(Me.pnRightActive)
        Me.Controls.Add(Me.pnBottomActive)
        Me.Controls.Add(Me.pnTopActive)
        Me.Controls.Add(Me.pnLeftActive)
        Me.Font = FontManager.GetDefaultFont()
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MinimumSize = New System.Drawing.Size(200, 200)
        Me.Name = "CropForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = $"Crop - {g.DefaultCommands.GetApplicationDetails()}"
        CType(Me.tbPosition, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private FrameServer As IFrameServer
    Private Renderer As VideoRenderer
    Private Side As AnchorStyles
    Private ActiveCropSide As AnchorStyles
    Private CommandManager As New CommandManager
    Private WithEvents CustomMenu As CustomMenu

    Public Property BorderColor As ColorHSL = Color.Empty
    Public Property BorderSelectedColor As ColorHSL = Color.Empty
    Public Property RendererBackColor As ColorHSL = Color.Empty

    Sub New()
        InitializeComponent()
        IsComposited = False

        MinimumSize = New Size(CInt(Font.Size * 60), CInt(Font.Size * 60))

        CommandManager.AddCommandsFromObject(Me)
        CommandManager.AddCommandsFromObject(g.DefaultCommands)

        ContextMenuStrip = New ContextMenuStripEx

        CustomMenu = New CustomMenu(AddressOf GetDefaultMenuCrop,
            s.CustomMenuCrop, CommandManager, ContextMenuStrip)

        CustomMenu.AddKeyDownHandler(Me)
        CustomMenu.BuildMenu()

        StatusStrip.Font = FontManager.GetDefaultFont()

        Dim offset = CInt(FontHeight * 0.6)

        pnVideo.Left = offset
        pnVideo.Top = offset
        pnVideo.Width = ClientSize.Width - offset * 2
        pnVideo.Height = tbPosition.Top - offset * 2
        pnVideo.Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Bottom

        pnLeftActive.Top = offset
        pnLeftActive.Left = 0
        pnLeftActive.Width = offset
        pnLeftActive.Height = pnVideo.Height
        pnLeftActive.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Top

        pnTopActive.Left = offset
        pnTopActive.Top = 0
        pnTopActive.Width = pnVideo.Width
        pnTopActive.Height = offset
        pnTopActive.Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right

        pnRightActive.Left = offset + pnVideo.Width
        pnRightActive.Top = offset
        pnRightActive.Width = offset
        pnRightActive.Height = pnVideo.Height
        pnRightActive.Anchor = AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Bottom

        pnBottomActive.Left = offset
        pnBottomActive.Top = offset + pnVideo.Height
        pnBottomActive.Width = pnVideo.Width
        pnBottomActive.Height = offset
        pnBottomActive.Anchor = AnchorStyles.Right Or AnchorStyles.Bottom Or AnchorStyles.Left

        FormBorderStyle = FormBorderStyle.Sizable

        tbPosition.Maximum = p.SourceFrames

        ApplyTheme()

        AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        RemoveHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
        components?.Dispose()
        MyBase.Dispose(disposing)
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

        BackColor = theme.General.BackColor

        BorderColor = theme.CropForm.BorderColor
        BorderSelectedColor = theme.CropForm.BorderSelectedColor

        StatusStrip.BackColor = theme.General.BackColor
        StatusStrip.ForeColor = theme.General.ForeColor
    End Sub

    Protected Overrides Sub OnLoad(args As EventArgs)
        MyBase.OnLoad(args)

        Dim zoom = 0.0
        Dim workingArea = Screen.FromControl(Me).WorkingArea

        While p.SourceWidth * zoom < 0.77 * workingArea.Width AndAlso
            p.SourceHeight * zoom < 0.77 * workingArea.Height

            zoom += 0.01
        End While

        SetDialogSize(CInt(p.SourceWidth * zoom), CInt(p.SourceHeight * zoom))

        Dim script As New VideoScript
        script.Engine = p.Script.Engine
        script.Path = Path.Combine(p.TempDir, p.TargetFile.Base + "_crop." + script.FileType)
        script.Filters.Add(p.Script.GetFilter("Source").GetCopy())

        If p.Script.GetFilter("Rotation") IsNot Nothing Then
            script.Filters.Add(p.Script.GetFilter("Rotation").GetCopy())
        End If

        If p.CropWithTonemapping Then
            If p.SourceVideoBitDepth > 8 AndAlso Not p.SourceVideoHdrFormat?.EqualsAny("", "SDR") Then
                If p.Script.Engine = ScriptEngine.AviSynth AndAlso Package.AVSLibPlacebo.RequirementsFulfilled Then
                    script.Filters.Add(New VideoFilter("Color", "Tonemap", "ConvertBits(16)" + BR + "libplacebo_Tonemap()" + BR + "ConvertToYUV420()" + BR + "ConvertBits(8)"))
                ElseIf p.Script.Engine = ScriptEngine.VapourSynth AndAlso Package.VSLibPlacebo.RequirementsFulfilled Then
                    script.Filters.Add(New VideoFilter("Color", "Tonemap", "clip = core.fmtc.bitdepth(clip, bits=16)" + BR + "clip = core.placebo.Tonemap(clip, src_csp=1, dst_csp=0, dynamic_peak_detection=0, tone_mapping_function=7)" + BR + $"clip = clip.resize.Bicubic(format = vs.YUV420P8)"))
                End If
            End If
        End If

        If p.CropWithHighContrast Then
            If p.Script.Engine = ScriptEngine.AviSynth Then
                script.Filters.Add(New VideoFilter("Levels", "Levels", "Levels(0, 2.2, 255, 0, 255, coring=true)"))
            Else
                script.Filters.Add(New VideoFilter("Levels", "Levels", "clip = adjust.Tweak(clip, hue=0, sat=1.75, bright=10, cont=1.75)"))
            End If
        End If

        script.Synchronize(True, True, True)

        FrameServer = FrameServerFactory.Create(script.Path)
        Renderer = New VideoRenderer(pnVideo, FrameServer) With {
            .Info = script.Info
        }

        If s.LastPosition < (FrameServer.Info.FrameCount - 1) Then
            Renderer.Position = s.LastPosition
        End If

        tbPosition.Value = Renderer.Position
        SelectBorder(AnchorStyles.Top)
        UpdateAll()

        If FrameServer.Error <> "" Then
            MsgError("CROP", FrameServer.Error)
        End If
    End Sub



    Protected Overrides Sub OnFormClosing(args As FormClosingEventArgs)
        MyBase.OnFormClosing(args)

        Dim err = p.Script.GetError

        If err <> "" Then
            Using td As New TaskDialog(Of String)
                td.Title = "Script Error"
                td.Icon = TaskIcon.Error
                td.Content = err
                td.AddButton("OK")
                td.AddButton("Exit")

                If td.Show() = "OK" Then
                    args.Cancel = True
                    Exit Sub
                End If
            End Using
        End If

        p.RemindToCrop = False
        s.LastPosition = Renderer.Position
        Renderer.Dispose()
        FrameServer.Dispose()
    End Sub

    Sub TrackLength_Scroll() Handles tbPosition.Scroll
        SetPos(tbPosition.Value)
    End Sub

    Sub SetPos(pos As Integer)
        Try
            Renderer.Position = pos
            tbPosition.Value = Renderer.Position
            Renderer.Draw()
            UpdateAll()
        Catch
        End Try
    End Sub

    Protected Overrides Sub OnSizeChanged(args As EventArgs)
        MyBase.OnSizeChanged(args)
        Renderer?.Draw()
    End Sub

    Sub CropActiveSideInternal(x As Integer, opposite As Boolean)
        Select Case Side
            Case AnchorStyles.Left
                p.CropLeft += x

                If opposite Then
                    p.CropRight += x
                End If
            Case AnchorStyles.Top
                p.CropTop += x

                If opposite Then
                    p.CropBottom += x
                End If
            Case AnchorStyles.Right
                p.CropRight += x

                If opposite Then
                    p.CropLeft += x
                End If
            Case AnchorStyles.Bottom
                p.CropBottom += x

                If opposite Then
                    p.CropTop += x
                End If
        End Select

        UpdateAll()
    End Sub

    Function GetSide(e As MouseEventArgs) As AnchorStyles
        Dim factorX = p.SourceWidth / pnVideo.Width
        Dim factorY = p.SourceHeight / pnVideo.Height

        Dim leftSide = CInt(e.X * factorX)
        Dim topSide = CInt(e.Y * factorY)
        Dim rightSide = CInt((pnVideo.Width - e.X) * factorX)
        Dim bottomSide = CInt((pnVideo.Height - e.Y) * factorY)

        Dim sides As Integer() = {leftSide, topSide, rightSide, bottomSide}
        Array.Sort(sides)

        Select Case sides(0)
            Case leftSide
                Return AnchorStyles.Left
            Case topSide
                Return AnchorStyles.Top
            Case rightSide
                Return AnchorStyles.Right
            Case bottomSide
                Return AnchorStyles.Bottom
        End Select

        Return AnchorStyles.None
    End Function

    Sub MouseCrop(e As MouseEventArgs)
        Dim scaleX = p.SourceWidth / pnVideo.Width
        Dim scaleY = p.SourceHeight / pnVideo.Height

        Dim leftSide = CInt(e.X * scaleX)
        Dim topSide = CInt(e.Y * scaleY)
        Dim rightSide = CInt((pnVideo.Width - e.X) * scaleX)
        Dim bottomSide = CInt((pnVideo.Height - e.Y) * scaleY)

        Select Case ActiveCropSide
            Case AnchorStyles.Left
                p.CropLeft = FixMod(leftSide)
            Case AnchorStyles.Top
                p.CropTop = FixMod(topSide)
            Case AnchorStyles.Right
                p.CropRight = FixMod(rightSide)
            Case AnchorStyles.Bottom
                p.CropBottom = FixMod(bottomSide)
        End Select

        UpdateAll()
    End Sub

    Sub MouseSelectBorder(e As MouseEventArgs)
        SelectBorder(GetSide(e))
    End Sub

    Sub SelectBorder(borderSide As AnchorStyles)
        Dim color = BorderSelectedColor

        DeactivateActiveColor()
        Select Case borderSide
            Case AnchorStyles.Left
                pnLeftActive.BackColor = color
            Case AnchorStyles.Top
                pnTopActive.BackColor = color
            Case AnchorStyles.Right
                pnRightActive.BackColor = color
            Case AnchorStyles.Bottom
                pnBottomActive.BackColor = color
        End Select
        Side = borderSide
    End Sub

    Sub DeactivateActiveColor()
        Dim color = BorderColor
        pnLeftActive.BackColor = color
        pnTopActive.BackColor = color
        pnRightActive.BackColor = color
        pnBottomActive.BackColor = color
    End Sub


    Function FixMod(value As Integer) As Integer
        If p.AutoCorrectCropValues AndAlso Not value Mod 2 = 0 Then
            value += 1
        End If

        Return value
    End Function

    Sub UpdateAll()
        Dim maxWidth = CInt(p.SourceWidth * 0.8)
        Dim maxHeight = CInt(p.SourceHeight * 0.8)

        If p.CropLeft > maxWidth Then
            p.CropLeft = FixMod(maxWidth)
        End If

        If p.CropTop > maxHeight Then
            p.CropTop = FixMod(maxHeight)
        End If

        If p.CropRight > maxWidth Then
            p.CropRight = FixMod(maxWidth)
        End If

        If p.CropBottom > maxHeight Then
            p.CropBottom = FixMod(maxHeight)
        End If

        Renderer.CropLeft = p.CropLeft
        Renderer.CropTop = p.CropTop
        Renderer.CropRight = p.CropRight
        Renderer.CropBottom = p.CropBottom
        Renderer.Draw()

        Dim cropw = p.SourceWidth - p.CropLeft - p.CropRight
        Dim croph = p.SourceHeight - p.CropTop - p.CropBottom

        Dim time = "n/a"
        If Renderer.Info.FrameRate > 0 Then
            Dim lengthDate = Date.Today.AddSeconds(Renderer.Position / Renderer.Info.FrameRate)
            time = lengthDate.ToString(If(lengthDate.Hour = 0, "mm:ss.fff", "HH:mm:ss.fff"))
        End If

        Dim isResized = p.Script.IsFilterActive("Resize")
        Dim isValidAnamorphicSize = (p.TargetWidth = 1440 AndAlso p.TargetHeight = 1080) OrElse (p.TargetWidth = 960 AndAlso p.TargetHeight = 720)
        Dim err = If(isResized AndAlso Not isValidAnamorphicSize, Calc.GetAspectRatioError.ToString("f2") + "%", "n/a")

        laStatus.Text =
            "  Frame: " & Renderer.Position.ToString().PadLeft(6) &
            "  |  Time: " & time &
            "  |  Size: " & cropw & "/" & croph &
            "  |  X: " & p.CropLeft & "/" & p.CropRight &
            "  |  Y: " & p.CropTop & "/" & p.CropBottom &
            "  |  Mod: " + Calc.GetMod(cropw, croph, False) +
            "  |  Error: " + err +
            "  |  DAR: " + Calc.GetTargetDAR().ToString("f6")
    End Sub

    Sub pVideo_MouseMove(sender As Object, e As MouseEventArgs) Handles pnVideo.MouseMove
        If Control.MouseButtons = Windows.Forms.MouseButtons.Left Then
            MouseCrop(e)
        Else
            MouseSelectBorder(e)
        End If
    End Sub

    Sub pVideo_MouseDown(sender As Object, e As MouseEventArgs) Handles pnVideo.MouseDown
        If Control.MouseButtons = Windows.Forms.MouseButtons.Left Then
            ActiveCropSide = GetSide(e)
            MouseCrop(e)
        End If
    End Sub

    Sub pVideo_Paint(sender As Object, e As PaintEventArgs) Handles pnVideo.Paint
        Renderer?.Draw()
    End Sub

    Sub tbPosition_Enter() Handles tbPosition.Enter
        ActiveControl = Nothing
    End Sub

    Sub SetDialogSize(w As Integer, h As Integer)
        ClientSize = New Size(ClientSize.Width + w - pnVideo.Width, ClientSize.Height + h - pnVideo.Height)
        Renderer?.Draw()
    End Sub

    Protected Overrides Function IsInputKey(keyData As Keys) As Boolean
        Select Case keyData
            Case Keys.Left, Keys.Up, Keys.Right, Keys.Down
                Return True
            Case Else
                Return MyBase.IsInputKey(keyData)
        End Select
    End Function

    Shared Function GetDefaultMenuCrop() As CustomMenuItem
        Dim ret As New CustomMenuItem("Root")
        Dim minusKey = If(Not KeysHelp.IsNumPadInstalled, Keys.Subtract, Keys.OemMinus)
        Dim plusKey = If(Not KeysHelp.IsNumPadInstalled, Keys.Add, Keys.Oemplus)
        
        ret.Add("No Crop", NameOf(SetCropValues), Keys.N, {0, 0, 0, 0})
        ret.Add("Auto Crop", NameOf(RunAutoCrop), Keys.A)
        ret.Add("Smart Crop", NameOf(RunSmartCrop), Keys.S)
        ret.Add("-")
        ret.Add("Increase Active Side", NameOf(CropActiveSide), plusKey, {2, 2})
        ret.Add("Decrease Active Side", NameOf(CropActiveSide), minusKey, {-2, -2})
        ret.Add("-")
        ret.Add("Increase Active Side Large", NameOf(CropActiveSide), plusKey Or Keys.Shift, {8, 8})
        ret.Add("Decrease Active Side Large", NameOf(CropActiveSide), minusKey Or Keys.Shift, {-8, -8})
        ret.Add("-")
        ret.Add("Increase Active And Opposite Side", NameOf(CropActiveAndOppositeSide), plusKey Or Keys.Control, {2, 2})
        ret.Add("Decrease Active And Opposite Side", NameOf(CropActiveAndOppositeSide), minusKey Or Keys.Control, {-2, -2})
        ret.Add("-")
        ret.Add("Increase Active And Opposite Side Large", NameOf(CropActiveAndOppositeSide), plusKey Or Keys.Control Or Keys.Shift, {8, 8})
        ret.Add("Decrease Active And Opposite Side Large", NameOf(CropActiveAndOppositeSide), minusKey Or Keys.Control Or Keys.Shift, {-8, -8})
        ret.Add("-")
        ret.Add("Navigation|Go To Frame...", NameOf(GoToFrame), Keys.F)
        ret.Add("Navigation|Go To Time...", NameOf(GoToTime), Keys.T)
        ret.Add("Navigation|-")
        ret.Add("Navigation|Go To Start", NameOf(SetAbsolutePosition), Keys.Control Or Keys.Left, {0})
        ret.Add("Navigation|100 Frames Backward", NameOf(SetRelativePosition), Keys.PageUp, {-100})
        ret.Add("Navigation|1000 Frames Backward", NameOf(SetRelativePosition), Keys.PageUp Or Keys.Control, {-1000})
        ret.Add("Navigation|-")
        ret.Add("Navigation|100 Frames Forward", NameOf(SetRelativePosition), Keys.PageDown, {100})
        ret.Add("Navigation|1000 Frames Forward", NameOf(SetRelativePosition), Keys.PageDown Or Keys.Control, {1000})
        ret.Add("Navigation|Go To End", NameOf(SetAbsolutePosition), Keys.Control Or Keys.Right, {1000000000})
        ret.Add("-")
        ret.Add("Crop Color | Theme", NameOf(SetCropColor), {Color.Empty})
        ret.Add("Crop Color | White", NameOf(SetCropColor), {Color.White})
        ret.Add("Crop Color | Black", NameOf(SetCropColor), {Color.Black})
        ret.Add("Crop Color | Light Gray", NameOf(SetCropColor), {Color.LightGray})
        ret.Add("Crop Options...", NameOf(ShowOptions), Keys.O, Symbol.Settings)
        ret.Add("Edit Menu...", NameOf(ShowMenuEditor), Keys.M)
        ret.Add("Help...", NameOf(ShowHelpDialog), Keys.F1, Symbol.Help)
        ret.Add("Exit", NameOf(CloseDialog), Keys.Escape)

        Return ret
    End Function

    <Command("Sets the four crop values.")>
    Sub SetCropValues(
        left As Integer,
        top As Integer,
        right As Integer,
        bottom As Integer)

        p.CropLeft = left
        p.CropTop = top
        p.CropRight = right
        p.CropBottom = bottom

        UpdateAll()
    End Sub

    <Command("Crops the active side.")>
    Sub CropActiveSide(
        <DispName("Pixel (corrected)"),
        Description("Pixels to crop when 'Auto correct crop values' is enabled.")>
        valueSafe As Integer,
        <DispName("Pixel (uncorrected)"),
        Description("Pixels to crop when 'Auto correct crop values' is disabled.")>
        valueUnsafe As Integer)

        CropActiveSideInternal(FixMod(If(p.AutoCorrectCropValues, valueSafe, valueUnsafe)), False)
    End Sub

    <Command("Crops the active and the opposite side of the active side")>
    Sub CropActiveAndOppositeSide(
        <DispName("Pixel (corrected)"),
        Description("Pixels to crop when 'Auto correct crop values' is enabled.")>
        valueSafe As Integer,
        <DispName("Pixel (uncorrected)"),
        Description("Pixels to crop when 'Auto correct crop values' is disabled.")>
        valueUnsafe As Integer)

        CropActiveSideInternal(FixMod(If(p.AutoCorrectCropValues, valueSafe, valueUnsafe)), True)
    End Sub

    <Command("Detects the crop values automatically.")>
    Sub RunAutoCrop()
        p.CropLeft = 0
        p.CropTop = 0
        p.CropRight = 0
        p.CropBottom = 0
        UpdateAll()

        g.RunAutoCrop(Sub(progress As Double)
                          tbPosition.Value = CInt(tbPosition.Maximum / 100 * progress)
                          TrackLength_Scroll()
                          Application.DoEvents()
                      End Sub)

        tbPosition.Value = 0
        TrackLength_Scroll()
    End Sub

    <Command("Crops until the proper aspect ratio is found.")>
    Sub RunSmartCrop()
        g.SmartCrop()
        UpdateAll()
    End Sub

    <Command("Dialog to configure the menu.")>
    Sub ShowMenuEditor()
        s.CustomMenuCrop = CustomMenu.Edit()
        g.SaveSettings()
    End Sub

    <Command("Exits the dialog.")>
    Sub CloseDialog()
        Close()
    End Sub

    <Command("Shows a dialog with crop options.")>
    Sub ShowOptions()
        g.MainForm.ShowOptionsDialog("Image | Crop")
        UpdateAll()
    End Sub

    <Command("Sets the color used for cropping.")>
    Sub SetCropColor(Color As Color)
        s.CropColor = Color
        Renderer.ApplyTheme()
        Renderer.Draw()
    End Sub

    <Command("Dialog to jump to a specific time.")>
    Sub GoToTime()
        Dim d As Date
        d = d.AddSeconds(Renderer.Position / FrameServer.FrameRate)
        Dim value = InputBox.Show("Go To Time", d.ToString("HH:mm:ss.fff"))

        If value <> "" Then
            SetPos(CInt((TimeSpan.Parse(value).TotalMilliseconds / 1000) * FrameServer.FrameRate))
        End If
    End Sub

    <Command("Dialog to jump to a specific frame.")>
    Sub GoToFrame()
        Dim value = InputBox.Show("Go To Frame", Renderer.Position.ToString)
        Dim pos As Integer

        If Integer.TryParse(value, pos) Then
            SetPos(pos)
        End If
    End Sub

    <Command("Jumps to a given frame.")>
    Sub SetAbsolutePosition(<DispName("Position")> pos As Integer)
        SetPos(pos)
    End Sub

    <Command("Jumps a given frame count.")>
    Sub SetRelativePosition(<DispName("Offset"), Description("Frames to jump, negative values jump backward.")> offset As Integer)
        Renderer.Position += offset
        tbPosition.Value = Renderer.Position
        Renderer.Draw()
        UpdateAll()
    End Sub

    <Command("Opens the help of the crop dialog.")>
    Sub ShowHelpDialog()
        Dim form As New HelpForm()
        form.Doc.WriteStart("Crop Dialog")
        form.Doc.WriteParagraph("The crop dialog allows to crop borders. Right-click to show a '''context menu''' with available features. By default StaxRip detects the crop values automatically. It's recommended to check the detected values visually. A crop value can be changed roughly by moving the mouse while the left mouse button is pressed. Alternative methods are using the '''mousewheel''' or keyboard shortcuts. The Ctrl key crops the active and opposite side, the Shift key crops 8 instead of 2 pixel.")
        form.Doc.WriteTips(CustomMenu.GetTips)
        form.Doc.WriteTable("Shortcut Keys", CustomMenu.GetKeys, False)
        form.Show()
    End Sub

    Protected Overrides Sub OnHelpButtonClicked(e As CancelEventArgs)
        e.Cancel = True
        ShowHelpDialog()
    End Sub

    Protected Overrides Sub OnMouseWheel(args As MouseEventArgs)
        MyBase.OnMouseWheel(args)

        Dim value = 2
        value = If((Control.ModifierKeys And Keys.Shift) = Keys.Shift, 8, value)
        value = If(args.Delta > 0, value, value * -1)
        CropActiveSideInternal(value, (Control.ModifierKeys And Keys.Control) = Keys.Control)
    End Sub

    Sub tsbMenu_Click(sender As Object, e As EventArgs) Handles tsbMenu.Click
        ContextMenuStrip.Show(MousePosition)
    End Sub

End Class
