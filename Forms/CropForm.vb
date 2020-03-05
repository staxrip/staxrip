
Imports System.ComponentModel

Imports StaxRip.UI

Public Class CropForm
    Inherits DialogBase

#Region " Designer "
    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    Friend WithEvents pLeftActive As System.Windows.Forms.Panel
    Friend WithEvents pTopActive As System.Windows.Forms.Panel
    Friend WithEvents pBottomActive As System.Windows.Forms.Panel
    Friend WithEvents pRightActive As System.Windows.Forms.Panel
    Friend WithEvents tbPosition As TrackBar
    Friend WithEvents pVideo As System.Windows.Forms.Panel
    Friend WithEvents StatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents tsbMenu As System.Windows.Forms.ToolStripDropDownButton
    Friend WithEvents laStatus As System.Windows.Forms.ToolStripStatusLabel

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.pLeftActive = New System.Windows.Forms.Panel()
        Me.pTopActive = New System.Windows.Forms.Panel()
        Me.pBottomActive = New System.Windows.Forms.Panel()
        Me.pRightActive = New System.Windows.Forms.Panel()
        Me.pVideo = New System.Windows.Forms.Panel()
        Me.tbPosition = New System.Windows.Forms.TrackBar()
        Me.StatusStrip = New System.Windows.Forms.StatusStrip()
        Me.laStatus = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tsbMenu = New System.Windows.Forms.ToolStripDropDownButton()
        CType(Me.tbPosition, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.StatusStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'pLeftActive
        '
        Me.pLeftActive.BackColor = System.Drawing.SystemColors.InfoText
        Me.pLeftActive.Location = New System.Drawing.Point(24, 56)
        Me.pLeftActive.Name = "pLeftActive"
        Me.pLeftActive.Size = New System.Drawing.Size(8, 81)
        Me.pLeftActive.TabIndex = 0
        '
        'pTopActive
        '
        Me.pTopActive.BackColor = System.Drawing.SystemColors.InfoText
        Me.pTopActive.Location = New System.Drawing.Point(64, 15)
        Me.pTopActive.Name = "pTopActive"
        Me.pTopActive.Size = New System.Drawing.Size(152, 8)
        Me.pTopActive.TabIndex = 1
        '
        'pBottomActive
        '
        Me.pBottomActive.BackColor = System.Drawing.SystemColors.InfoText
        Me.pBottomActive.Location = New System.Drawing.Point(64, 184)
        Me.pBottomActive.Name = "pBottomActive"
        Me.pBottomActive.Size = New System.Drawing.Size(152, 8)
        Me.pBottomActive.TabIndex = 3
        '
        'pRightActive
        '
        Me.pRightActive.BackColor = System.Drawing.SystemColors.InfoText
        Me.pRightActive.Location = New System.Drawing.Point(256, 56)
        Me.pRightActive.Name = "pRightActive"
        Me.pRightActive.Size = New System.Drawing.Size(8, 88)
        Me.pRightActive.TabIndex = 4
        '
        'pVideo
        '
        Me.pVideo.BackColor = System.Drawing.Color.Black
        Me.pVideo.Location = New System.Drawing.Point(64, 55)
        Me.pVideo.Name = "pVideo"
        Me.pVideo.Size = New System.Drawing.Size(149, 89)
        Me.pVideo.TabIndex = 2
        '
        'tbPosition
        '
        Me.tbPosition.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tbPosition.AutoSize = False
        Me.tbPosition.Location = New System.Drawing.Point(12, 326)
        Me.tbPosition.Name = "tbPosition"
        Me.tbPosition.Size = New System.Drawing.Size(711, 33)
        Me.tbPosition.TabIndex = 5
        Me.tbPosition.TabStop = False
        Me.tbPosition.TickStyle = System.Windows.Forms.TickStyle.None
        '
        'StatusStrip
        '
        Me.StatusStrip.ImageScalingSize = New System.Drawing.Size(48, 48)
        Me.StatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.laStatus, Me.tsbMenu})
        Me.StatusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.StatusStrip.Location = New System.Drawing.Point(0, 336)
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(735, 57)
        Me.StatusStrip.TabIndex = 6
        Me.StatusStrip.Text = "StatusStrip"
        '
        'laStatus
        '
        Me.laStatus.Name = "laStatus"
        Me.laStatus.Size = New System.Drawing.Size(34, 52)
        Me.laStatus.Text = "-"
        '
        'tsbMenu
        '
        Me.tsbMenu.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.tsbMenu.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsbMenu.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbMenu.Name = "tsbMenu"
        Me.tsbMenu.Size = New System.Drawing.Size(142, 55)
        Me.tsbMenu.Text = "Menu"
        '
        'CropForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(735, 393)
        Me.Controls.Add(Me.StatusStrip)
        Me.Controls.Add(Me.tbPosition)
        Me.Controls.Add(Me.pVideo)
        Me.Controls.Add(Me.pRightActive)
        Me.Controls.Add(Me.pBottomActive)
        Me.Controls.Add(Me.pTopActive)
        Me.Controls.Add(Me.pLeftActive)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.MinimumSize = New System.Drawing.Size(200, 200)
        Me.Name = "CropForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.Text = "Crop"
        CType(Me.tbPosition, System.ComponentModel.ISupportInitialize).EndInit()
        Me.StatusStrip.ResumeLayout(False)
        Me.StatusStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private FrameServer As FrameServer
    Private Renderer As VideoRenderer
    Private SelectedBorderColor As Color = ToolStripRendererEx.ColorBorder
    Private Side As AnchorStyles
    Private ActiveCropSide As AnchorStyles
    Private CommandManager As New CommandManager
    Private WithEvents CustomMenu As CustomMenu

    Sub New()
        MyBase.New()
        InitializeComponent()

        MinimumSize = New Size(CInt(Font.Size * 90), CInt(Font.Size * 70))

        CommandManager.AddCommandsFromObject(Me)
        CommandManager.AddCommandsFromObject(g.DefaultCommands)

        ContextMenuStrip = New ContextMenuStripEx

        CustomMenu = New CustomMenu(AddressOf GetDefaultMenuCrop,
            s.CustomMenuCrop, CommandManager, ContextMenuStrip)

        CustomMenu.AddKeyDownHandler(Me)
        CustomMenu.BuildMenu()

        StatusStrip.Font = New Font("Segoe UI", 9 * s.UIScaleFactor)

        pVideo.Left = 10
        pVideo.Top = 10
        pVideo.Width = ClientSize.Width - 20
        pVideo.Height = tbPosition.Top - 20
        pVideo.Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Bottom

        pLeftActive.Top = 10
        pLeftActive.Left = 0
        pLeftActive.Width = 10
        pLeftActive.Height = pVideo.Height
        pLeftActive.Anchor = AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Top

        pTopActive.Left = 10
        pTopActive.Top = 0
        pTopActive.Width = pVideo.Width
        pTopActive.Height = 10
        pTopActive.Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right

        pRightActive.Left = 10 + pVideo.Width
        pRightActive.Top = 10
        pRightActive.Width = 10
        pRightActive.Height = pVideo.Height
        pRightActive.Anchor = AnchorStyles.Top Or AnchorStyles.Right Or AnchorStyles.Bottom

        pBottomActive.Left = 10
        pBottomActive.Top = 10 + pVideo.Height
        pBottomActive.Width = pVideo.Width
        pBottomActive.Height = 10
        pBottomActive.Anchor = AnchorStyles.Right Or AnchorStyles.Bottom Or AnchorStyles.Left

        FormBorderStyle = FormBorderStyle.Sizable

        DeactivateActiveColor()

        tbPosition.Maximum = p.SourceFrames

        pTopActive.BackColor = SelectedBorderColor
        Side = AnchorStyles.Top
    End Sub

    Private Sub TrackLength_Scroll() Handles tbPosition.Scroll
        Renderer.Position = tbPosition.Value
        Renderer.Draw()
    End Sub

    Private Sub DeactivateActiveColor()
        pLeftActive.BackColor = Drawing.SystemColors.Control
        pTopActive.BackColor = Drawing.SystemColors.Control
        pRightActive.BackColor = Drawing.SystemColors.Control
        pBottomActive.BackColor = Drawing.SystemColors.Control
    End Sub

    Private Sub CropForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim zoom = 0.0
        Dim workingArea = Screen.FromControl(Me).WorkingArea

        While p.SourceWidth * zoom < 0.7 * workingArea.Width AndAlso
            p.SourceHeight * zoom < 0.7 * workingArea.Height

            zoom += 0.01
        End While

        SetDialogSize(CInt(p.SourceWidth * zoom), CInt(p.SourceHeight * zoom))
    End Sub

    Private Sub Wheel(sender As Object, e As MouseEventArgs) Handles MyBase.MouseWheel
        Dim value = 2
        value = If((Control.ModifierKeys And Keys.Shift) = Keys.Shift, 8, value)
        value = If(e.Delta > 0, value, value * -1)
        CropActiveSideInternal(value, (Control.ModifierKeys And Keys.Control) = Keys.Control)
    End Sub

    Private Sub CropActiveSideInternal(x As Integer, opposite As Boolean)
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

    Private Function GetSide(e As MouseEventArgs) As AnchorStyles
        Dim factorX = p.SourceWidth / pVideo.Width
        Dim factorY = p.SourceHeight / pVideo.Height

        Dim leftSide = CInt(e.X * factorX)
        Dim topSide = CInt(e.Y * factorY)
        Dim rightSide = CInt((pVideo.Width - e.X) * factorX)
        Dim bottomSide = CInt((pVideo.Height - e.Y) * factorY)

        Dim sides As Integer() = {leftSide, topSide, rightSide, bottomSide}
        Array.Sort(sides)

        Select Case CInt(sides(0))
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

    Private Sub MouseCrop(e As MouseEventArgs)
        Dim scaleX = p.SourceWidth / pVideo.Width
        Dim scaleY = p.SourceHeight / pVideo.Height

        Dim leftSide = CInt(e.X * scaleX)
        Dim topSide = CInt(e.Y * scaleY)
        Dim rightSide = CInt((pVideo.Width - e.X) * scaleX)
        Dim bottomSide = CInt((pVideo.Height - e.Y) * scaleY)

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

    Private Sub MouseSelectBorder(e As MouseEventArgs)
        Select Case GetSide(e)
            Case AnchorStyles.Left
                DeactivateActiveColor()
                pLeftActive.BackColor = SelectedBorderColor
                Side = AnchorStyles.Left
            Case AnchorStyles.Top
                DeactivateActiveColor()
                pTopActive.BackColor = SelectedBorderColor
                Side = AnchorStyles.Top
            Case AnchorStyles.Right
                DeactivateActiveColor()
                pRightActive.BackColor = SelectedBorderColor
                Side = AnchorStyles.Right
            Case AnchorStyles.Bottom
                DeactivateActiveColor()
                pBottomActive.BackColor = SelectedBorderColor
                Side = AnchorStyles.Bottom
        End Select
    End Sub

    Private Function FixMod(value As Integer) As Integer
        If p.AutoCorrectCropValues AndAlso Not value Mod 2 = 0 Then
            value += 1
        End If

        Return value
    End Function

    Sub UpdateAll()
        If p.CropLeft > p.SourceWidth * 0.4 Then
            p.CropLeft = FixMod(CInt(p.SourceWidth * 0.4))
        End If

        If p.CropTop > p.SourceHeight * 0.4 Then
            p.CropTop = FixMod(CInt(p.SourceHeight * 0.4))
        End If

        If p.CropRight > p.SourceWidth * 0.4 Then
            p.CropRight = FixMod(CInt(p.SourceWidth * 0.4))
        End If

        If p.CropBottom > p.SourceHeight * 0.4 Then
            p.CropBottom = FixMod(CInt(p.SourceHeight * 0.4))
        End If

        Renderer.CropLeft = p.CropLeft
        Renderer.CropTop = p.CropTop
        Renderer.CropRight = p.CropRight
        Renderer.CropBottom = p.CropBottom
        Renderer.Draw()

        Dim cropw = p.SourceWidth - p.CropLeft - p.CropRight
        Dim croph = p.SourceHeight - p.CropTop - p.CropBottom

        Dim isResized = p.Script.IsFilterActive("Resize")
        Dim isValidAnamorphicSize = (p.TargetWidth = 1440 AndAlso p.TargetHeight = 1080) OrElse (p.TargetWidth = 960 AndAlso p.TargetHeight = 720)
        Dim err = If(isResized AndAlso Not isValidAnamorphicSize, Calc.GetAspectRatioError.ToString("f2") + "%", "n/a")

        laStatus.Text =
            "  Size: " & cropw & "/" & croph &
            "  X: " & p.CropLeft & "/" & p.CropRight &
            "  Y: " & p.CropTop & "/" & p.CropBottom &
            "  Mod: " + Calc.GetMod(cropw, croph, False) +
            "  Error: " + err +
            "  DAR: " + Calc.GetTargetDAR().ToString("f6")
    End Sub

    Private Sub pVideo_MouseMove(sender As Object, e As MouseEventArgs) Handles pVideo.MouseMove
        If Control.MouseButtons = Windows.Forms.MouseButtons.Left Then
            MouseCrop(e)
        Else
            MouseSelectBorder(e)
        End If
    End Sub

    Private Sub pVideo_MouseDown(sender As Object, e As MouseEventArgs) Handles pVideo.MouseDown
        If Control.MouseButtons = Windows.Forms.MouseButtons.Left Then
            ActiveCropSide = GetSide(e)
            MouseCrop(e)
        End If
    End Sub

    Private Sub pVideo_Paint(sender As Object, e As PaintEventArgs) Handles pVideo.Paint
        Renderer?.Draw()
    End Sub

    Private Sub CropForm_SizeChanged() Handles MyBase.SizeChanged
        Renderer?.Draw()
    End Sub

    Private Sub CropForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim err = p.Script.GetError

        If err <> "" Then
            Using td As New TaskDialog(Of String)
                td.MainInstruction = "Script Error"
                td.Content = err
                td.AddButton("OK", "OK")
                td.AddButton("Exit", "Exit")

                If td.Show() = "OK" Then
                    e.Cancel = True
                    Exit Sub
                End If
            End Using
        End If

        p.RemindToCrop = False
        s.LastPosition = Renderer.Position
        Renderer.Dispose()
        FrameServer.Dispose()
    End Sub

    Private Sub tbPosition_Enter() Handles tbPosition.Enter
        ActiveControl = Nothing
    End Sub

    Private Sub SetDialogSize(w As Integer, h As Integer)
        ClientSize = New Size(ClientSize.Width + w - pVideo.Width, ClientSize.Height + h - pVideo.Height)
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

        ret.Add("No Crop", NameOf(SetCropValues), Keys.N, {0, 0, 0, 0})
        ret.Add("Auto Crop", NameOf(RunAutoCrop), Keys.A)
        ret.Add("Smart Crop", NameOf(RunSmartCrop), Keys.S)
        ret.Add("-")
        ret.Add("Increase Active Side", NameOf(CropActiveSide), Keys.Add, {2, 2})
        ret.Add("Decrease Active Side", NameOf(CropActiveSide), Keys.Subtract, {-2, -2})
        ret.Add("-")
        ret.Add("Increase Active Side Large", NameOf(CropActiveSide), Keys.Add Or Keys.Shift, {8, 8})
        ret.Add("Decrease Active Side Large", NameOf(CropActiveSide), Keys.Subtract Or Keys.Shift, {-8, -8})
        ret.Add("-")
        ret.Add("Increase Active And Opposite Side", NameOf(CropActiveAndOppositeSide), Keys.Add Or Keys.Control, {2, 2})
        ret.Add("Decrease Active And Opposite Side", NameOf(CropActiveAndOppositeSide), Keys.Subtract Or Keys.Control, {-2, -2})
        ret.Add("-")
        ret.Add("Increase Active And Opposite Side Large", NameOf(CropActiveAndOppositeSide), Keys.Add Or Keys.Control Or Keys.Shift, {8, 8})
        ret.Add("Decrease Active And Opposite Side Large", NameOf(CropActiveAndOppositeSide), Keys.Subtract Or Keys.Control Or Keys.Shift, {-8, -8})
        ret.Add("-")
        ret.Add("Navigate 100 Frames Backward", NameOf(SetRelativePosition), Keys.PageUp, {-100})
        ret.Add("Navigate 1000 Frames Backward", NameOf(SetRelativePosition), Keys.PageUp Or Keys.Control, {-1000})
        ret.Add("Navigate 1000 Frames Forward", NameOf(SetRelativePosition), Keys.PageDown Or Keys.Control, {1000})
        ret.Add("Navigate 100 Frames Forward", NameOf(SetRelativePosition), Keys.PageDown, {100})
        ret.Add("-")
        ret.Add("Crop Options...", NameOf(ShowOptions), Keys.O, Symbol.Settings)
        ret.Add("Edit Menu...", NameOf(ShowMenuEditor), Keys.M)
        ret.Add("Help...", NameOf(ShowHelpDialog), Keys.F1, Symbol.Help)
        ret.Add("Exit", NameOf(CloseDialog), Keys.Escape)

        Return ret
    End Function

    <Command("Sets the four crop values.")>
    Private Sub SetCropValues(
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
    Private Sub CropActiveSide(
        <DispName("Pixel (corrected)"),
        Description("Pixels to crop when 'Auto correct crop values' is enabled.")>
        valueSafe As Integer,
        <DispName("Pixel (uncorrected)"),
        Description("Pixels to crop when 'Auto correct crop values' is disabled.")>
        valueUnsafe As Integer)

        CropActiveSideInternal(FixMod(If(p.AutoCorrectCropValues, valueSafe, valueUnsafe)), False)
    End Sub

    <Command("Crops the active and the opposite side of the active side")>
    Private Sub CropActiveAndOppositeSide(
        <DispName("Pixel (corrected)"),
        Description("Pixels to crop when 'Auto correct crop values' is enabled.")>
        valueSafe As Integer,
        <DispName("Pixel (uncorrected)"),
        Description("Pixels to crop when 'Auto correct crop values' is disabled.")>
        valueUnsafe As Integer)

        CropActiveSideInternal(FixMod(If(p.AutoCorrectCropValues, valueSafe, valueUnsafe)), True)
    End Sub

    <Command("Detects the crop values automatically.")>
    Private Sub RunAutoCrop()
        p.CropLeft = 0
        p.CropTop = 0
        p.CropRight = 0
        p.CropBottom = 0
        UpdateAll()
        g.RunAutoCrop(Sub(progress As Double)
                          tbPosition.Value = CInt(tbPosition.Maximum / 100 * progress)
                          TrackLength_Scroll()
                      End Sub)
        UpdateAll()
    End Sub

    <Command("Crops until the proper aspect ratio is found.")>
    Private Sub RunSmartCrop()
        g.SmartCrop()
        UpdateAll()
    End Sub

    <Command("Dialog to configure the menu.")>
    Private Sub ShowMenuEditor()
        s.CustomMenuCrop = CustomMenu.Edit()
        g.SaveSettings()
    End Sub

    <Command("Exits the dialog.")>
    Private Sub CloseDialog()
        Close()
    End Sub

    <Command("Shows a dialog with crop options.")>
    Private Sub ShowOptions()
        g.MainForm.ShowOptionsDialog("Image|Crop")
        UpdateAll()
    End Sub

    <Command("Jumps a given frame count.")>
    Private Sub SetRelativePosition(
        <DispName("Offset"), Description("Frames to jump, negative values jump backward.")>
        offset As Integer)

        Renderer.Position += offset
        tbPosition.Value = Renderer.Position
        Renderer.Draw()
    End Sub

    <Command("Opens the help of the crop dialog.")>
    Private Sub ShowHelpDialog()
        Refresh()

        Dim form As New HelpForm()
        form.Doc.WriteStart("Crop Dialog")
        form.Doc.WriteP("The crop dialog allows to crop borders. Right-click to show a '''context menu''' with available features. By default StaxRip detects the crop values automatically. It's recommended to check the detected values visually. A crop value can be changed roughly by moving the mouse while the left mouse button is pressed. Alternative methods are using the '''mousewheel''' or keyboard shortcuts. The Ctrl key crops the active and opposite side, the Shift key crops 8 instead of 2 pixel.")
        form.Doc.WriteTips(CustomMenu.GetTips)
        form.Doc.WriteTable("Shortcut Keys", CustomMenu.GetKeys, False)
        form.Show()
    End Sub

    Protected Overrides Sub OnHelpButtonClicked(e As CancelEventArgs)
        e.Cancel = True
        MyBase.OnHelpButtonClicked(e)
        ShowHelpDialog()
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        Dim script As New VideoScript
        script.Engine = p.Script.Engine
        script.Path = p.TempDir + p.TargetFile.Base + "_crop." + script.FileType
        script.Filters.Add(p.Script.GetFilter("Source").GetCopy)
        script.Synchronize(True, True, True)

        FrameServer = New FrameServer(script.Path)
        Renderer = New VideoRenderer(pVideo, FrameServer)

        If s.LastPosition < (FrameServer.Info.FrameCount - 1) Then
            Renderer.Position = s.LastPosition
        End If

        tbPosition.Value = Renderer.Position
        UpdateAll()
    End Sub

    Private Sub tsbMenu_Click(sender As Object, e As EventArgs) Handles tsbMenu.Click
        ContextMenuStrip.Show(MousePosition)
    End Sub
End Class