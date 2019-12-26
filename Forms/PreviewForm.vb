
Imports System.ComponentModel
Imports System.Drawing.Imaging
Imports StaxRip.UI

Public Class PreviewForm
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

    Friend WithEvents pVideo As System.Windows.Forms.Panel
    Friend WithEvents pnTrack As System.Windows.Forms.Panel
    Private WithEvents bnDeleteRange As System.Windows.Forms.Button
    Private WithEvents bnForward1 As System.Windows.Forms.Button
    Private WithEvents bnRangeStart As System.Windows.Forms.Button
    Private WithEvents bnRangeEnd As System.Windows.Forms.Button
    Private WithEvents bnForward10 As System.Windows.Forms.Button
    Private WithEvents bnBackward10 As System.Windows.Forms.Button
    Private WithEvents bExtras As ButtonEx
    Private WithEvents bnForward100 As System.Windows.Forms.Button
    Private WithEvents bnBackward100 As System.Windows.Forms.Button
    Private WithEvents bnBackward1 As System.Windows.Forms.Button
    Friend WithEvents cmsMain As ContextMenuStripEx
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip

    '<System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PreviewForm))
        Me.bnDeleteRange = New System.Windows.Forms.Button()
        Me.bnForward1 = New System.Windows.Forms.Button()
        Me.bnBackward1 = New System.Windows.Forms.Button()
        Me.bnRangeStart = New System.Windows.Forms.Button()
        Me.bnRangeEnd = New System.Windows.Forms.Button()
        Me.bnForward10 = New System.Windows.Forms.Button()
        Me.bnBackward10 = New System.Windows.Forms.Button()
        Me.bExtras = New StaxRip.UI.ButtonEx()
        Me.bnForward100 = New System.Windows.Forms.Button()
        Me.bnBackward100 = New System.Windows.Forms.Button()
        Me.pVideo = New System.Windows.Forms.Panel()
        Me.cmsMain = New StaxRip.UI.ContextMenuStripEx(Me.components)
        Me.pnTrack = New System.Windows.Forms.Panel()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.pVideo.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnDeleteRange
        '
        Me.bnDeleteRange.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnDeleteRange.BackColor = System.Drawing.Color.White
        Me.bnDeleteRange.BackgroundImage = Global.StaxRip.My.Resources.Resources.X
        Me.bnDeleteRange.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnDeleteRange.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bnDeleteRange.Location = New System.Drawing.Point(332, 432)
        Me.bnDeleteRange.Margin = New System.Windows.Forms.Padding(2)
        Me.bnDeleteRange.Name = "bnDeleteRange"
        Me.bnDeleteRange.Size = New System.Drawing.Size(25, 26)
        Me.bnDeleteRange.TabIndex = 47
        Me.bnDeleteRange.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnDeleteRange, "Deletes the cut selection that encloses the current position.")
        Me.bnDeleteRange.UseVisualStyleBackColor = False
        '
        'bnForward1
        '
        Me.bnForward1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnForward1.BackColor = System.Drawing.Color.White
        Me.bnForward1.BackgroundImage = Global.StaxRip.My.Resources.Resources.Right1
        Me.bnForward1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnForward1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bnForward1.Location = New System.Drawing.Point(251, 432)
        Me.bnForward1.Margin = New System.Windows.Forms.Padding(2)
        Me.bnForward1.Name = "bnForward1"
        Me.bnForward1.Size = New System.Drawing.Size(25, 26)
        Me.bnForward1.TabIndex = 46
        Me.bnForward1.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnForward1, "Forward 1 Frames")
        Me.bnForward1.UseVisualStyleBackColor = False
        '
        'bnBackward1
        '
        Me.bnBackward1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnBackward1.BackColor = System.Drawing.Color.White
        Me.bnBackward1.BackgroundImage = Global.StaxRip.My.Resources.Resources.Left1
        Me.bnBackward1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnBackward1.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bnBackward1.Location = New System.Drawing.Point(170, 432)
        Me.bnBackward1.Margin = New System.Windows.Forms.Padding(2)
        Me.bnBackward1.Name = "bnBackward1"
        Me.bnBackward1.Size = New System.Drawing.Size(25, 26)
        Me.bnBackward1.TabIndex = 45
        Me.bnBackward1.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnBackward1, "Backward 1 Frame")
        Me.bnBackward1.UseVisualStyleBackColor = False
        '
        'bnRangeStart
        '
        Me.bnRangeStart.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnRangeStart.BackColor = System.Drawing.Color.White
        Me.bnRangeStart.BackgroundImage = Global.StaxRip.My.Resources.Resources.BracketOpen
        Me.bnRangeStart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnRangeStart.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bnRangeStart.Location = New System.Drawing.Point(197, 432)
        Me.bnRangeStart.Margin = New System.Windows.Forms.Padding(2)
        Me.bnRangeStart.Name = "bnRangeStart"
        Me.bnRangeStart.Size = New System.Drawing.Size(25, 26)
        Me.bnRangeStart.TabIndex = 44
        Me.bnRangeStart.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnRangeStart, "Sets a start cut point. Press F1 for help about cutting")
        Me.bnRangeStart.UseVisualStyleBackColor = False
        '
        'bnRangeEnd
        '
        Me.bnRangeEnd.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnRangeEnd.BackColor = System.Drawing.Color.White
        Me.bnRangeEnd.BackgroundImage = Global.StaxRip.My.Resources.Resources.BracketClose
        Me.bnRangeEnd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnRangeEnd.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bnRangeEnd.Location = New System.Drawing.Point(224, 432)
        Me.bnRangeEnd.Margin = New System.Windows.Forms.Padding(2)
        Me.bnRangeEnd.Name = "bnRangeEnd"
        Me.bnRangeEnd.Size = New System.Drawing.Size(25, 26)
        Me.bnRangeEnd.TabIndex = 43
        Me.bnRangeEnd.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnRangeEnd, "Sets a end cut point. Press F1 for help about cutting")
        Me.bnRangeEnd.UseVisualStyleBackColor = False
        '
        'bnForward10
        '
        Me.bnForward10.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnForward10.BackColor = System.Drawing.Color.White
        Me.bnForward10.BackgroundImage = Global.StaxRip.My.Resources.Resources.Right2
        Me.bnForward10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnForward10.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bnForward10.Location = New System.Drawing.Point(278, 432)
        Me.bnForward10.Margin = New System.Windows.Forms.Padding(2)
        Me.bnForward10.Name = "bnForward10"
        Me.bnForward10.Size = New System.Drawing.Size(25, 26)
        Me.bnForward10.TabIndex = 42
        Me.bnForward10.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnForward10, "Forward 10 Frames")
        Me.bnForward10.UseVisualStyleBackColor = False
        '
        'bnBackward10
        '
        Me.bnBackward10.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnBackward10.BackColor = System.Drawing.Color.White
        Me.bnBackward10.BackgroundImage = CType(resources.GetObject("bnBackward10.BackgroundImage"), System.Drawing.Image)
        Me.bnBackward10.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnBackward10.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bnBackward10.Location = New System.Drawing.Point(143, 432)
        Me.bnBackward10.Margin = New System.Windows.Forms.Padding(2)
        Me.bnBackward10.Name = "bnBackward10"
        Me.bnBackward10.Size = New System.Drawing.Size(25, 26)
        Me.bnBackward10.TabIndex = 41
        Me.bnBackward10.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnBackward10, "Backward 10 Frames")
        Me.bnBackward10.UseVisualStyleBackColor = False
        '
        'bExtras
        '
        Me.bExtras.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bExtras.BackColor = System.Drawing.Color.White
        Me.bExtras.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bExtras.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bExtras.Location = New System.Drawing.Point(359, 432)
        Me.bExtras.Margin = New System.Windows.Forms.Padding(2)
        Me.bExtras.ShowMenuSymbol = True
        Me.bExtras.Size = New System.Drawing.Size(25, 26)
        Me.bExtras.TabStop = False
        Me.ToolTip.SetToolTip(Me.bExtras, "Shows the menu")
        '
        'bnForward100
        '
        Me.bnForward100.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnForward100.BackColor = System.Drawing.Color.White
        Me.bnForward100.BackgroundImage = Global.StaxRip.My.Resources.Resources.Right3
        Me.bnForward100.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnForward100.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bnForward100.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bnForward100.Location = New System.Drawing.Point(305, 432)
        Me.bnForward100.Margin = New System.Windows.Forms.Padding(2)
        Me.bnForward100.Name = "bnForward100"
        Me.bnForward100.Size = New System.Drawing.Size(25, 26)
        Me.bnForward100.TabIndex = 49
        Me.bnForward100.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnForward100, "Forward 100 Frames")
        Me.bnForward100.UseVisualStyleBackColor = False
        '
        'bnBackward100
        '
        Me.bnBackward100.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnBackward100.BackColor = System.Drawing.Color.White
        Me.bnBackward100.BackgroundImage = CType(resources.GetObject("bnBackward100.BackgroundImage"), System.Drawing.Image)
        Me.bnBackward100.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnBackward100.FlatStyle = System.Windows.Forms.FlatStyle.Flat
        Me.bnBackward100.Font = New System.Drawing.Font("Segoe UI", 4.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bnBackward100.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bnBackward100.Location = New System.Drawing.Point(116, 432)
        Me.bnBackward100.Margin = New System.Windows.Forms.Padding(2)
        Me.bnBackward100.Name = "bnBackward100"
        Me.bnBackward100.Size = New System.Drawing.Size(25, 26)
        Me.bnBackward100.TabIndex = 48
        Me.bnBackward100.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnBackward100, "Backward 100 Frames")
        Me.bnBackward100.UseVisualStyleBackColor = False
        '
        'pVideo
        '
        Me.pVideo.ContextMenuStrip = Me.cmsMain
        Me.pVideo.Controls.Add(Me.bnForward100)
        Me.pVideo.Controls.Add(Me.bnDeleteRange)
        Me.pVideo.Controls.Add(Me.bnForward10)
        Me.pVideo.Controls.Add(Me.bExtras)
        Me.pVideo.Controls.Add(Me.bnForward1)
        Me.pVideo.Controls.Add(Me.bnRangeEnd)
        Me.pVideo.Controls.Add(Me.bnRangeStart)
        Me.pVideo.Controls.Add(Me.bnBackward1)
        Me.pVideo.Controls.Add(Me.bnBackward10)
        Me.pVideo.Controls.Add(Me.bnBackward100)
        Me.pVideo.Controls.Add(Me.pnTrack)
        Me.pVideo.Location = New System.Drawing.Point(64, 25)
        Me.pVideo.Margin = New System.Windows.Forms.Padding(2)
        Me.pVideo.Name = "pVideo"
        Me.pVideo.Size = New System.Drawing.Size(500, 462)
        Me.pVideo.TabIndex = 50
        '
        'cmsMain
        '
        Me.cmsMain.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.cmsMain.Name = "cmsMain"
        Me.cmsMain.Size = New System.Drawing.Size(61, 4)
        '
        'pnTrack
        '
        Me.pnTrack.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnTrack.BackColor = System.Drawing.SystemColors.Control
        Me.pnTrack.Cursor = System.Windows.Forms.Cursors.SizeNS
        Me.pnTrack.Location = New System.Drawing.Point(5, 416)
        Me.pnTrack.Margin = New System.Windows.Forms.Padding(2)
        Me.pnTrack.Name = "pnTrack"
        Me.pnTrack.Size = New System.Drawing.Size(490, 12)
        Me.pnTrack.TabIndex = 51
        '
        'ToolTip
        '
        Me.ToolTip.AutoPopDelay = 5000
        Me.ToolTip.BackColor = System.Drawing.Color.White
        Me.ToolTip.InitialDelay = 100
        Me.ToolTip.ReshowDelay = 100
        '
        'PreviewForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(640, 513)
        Me.Controls.Add(Me.pVideo)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Name = "PreviewForm"
        Me.ShowInTaskbar = True
        Me.Text = "Preview"
        Me.pVideo.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public AVI As AVIFile
    Public RangeStart As Integer = -1

    Private PreviewScript As VideoScript
    Private SizeFactor As Double = 1
    Private TargetFrames As Integer
    Private WithEvents GenericMenu As CustomMenu
    Private CommandManager As New CommandManager
    Private Renderer As VideoRenderer

    Private Const TrackBarBorder As Integer = 1
    Private Const TrackBarGap As Integer = 1
    Private TrackBarPosition As Integer = Control.DefaultFont.Height \ 5

    Private Shared Instances As New List(Of PreviewForm)

    Sub New(script As VideoScript)
        MyBase.New()
        InitializeComponent()
        Icon = g.Icon

        CommandManager.AddCommandsFromObject(Me)
        CommandManager.AddCommandsFromObject(g.DefaultCommands)

        GenericMenu = New CustomMenu(AddressOf GetDefaultMenuPreview,
            s.CustomMenuPreview, CommandManager, cmsMain)

        GenericMenu.AddKeyDownHandler(Me)
        GenericMenu.BuildMenu()

        Instances.Add(Me)

        PreviewScript = script
        NormalRectangle.Size = Size
        NormalRectangle.Location = Location
    End Sub

    Sub RefreshScript()
        TargetFrames = p.Script.GetFrames

        If Not AVI Is Nothing Then
            AVI.Dispose()
        End If

        PreviewScript.Synchronize(True, True, True)
        AVI = New AVIFile(PreviewScript.Path)
        Renderer = New VideoRenderer(pVideo, AVI)
        Renderer.ShowInfo = s.ShowPreviewInfo
        Dim workingArea = Screen.FromControl(Me).WorkingArea

        While GetNormalSize.Width < workingArea.Width * (s.MinPreviewSize / 100) AndAlso
            GetNormalSize.Height < workingArea.Height * (s.MinPreviewSize / 100)

            SizeFactor += 0.05
            SizeFactor = Math.Round(SizeFactor, 2)
        End While

        While GetNormalSize.Width > workingArea.Width * 0.95 OrElse
            GetNormalSize.Height > workingArea.Height * 0.95

            SizeFactor -= 0.05
            SizeFactor = Math.Round(SizeFactor, 2)
        End While

        If s.PreviewFormBorderStyle = FormBorderStyle.None Then
            Fullscreen()
        Else
            NormalScreen()
        End If

        If s.LastPosition < AVI.FrameCount - 1 Then AVI.Position = s.LastPosition
        AfterPositionChanged()
        ShowButtons(Not s.HidePreviewButtons)
        Refresh()
    End Sub

    Public NormalRectangle As Rectangle

    Private Sub Fullscreen()
        Dim trackVisible = pnTrack.Visible
        pnTrack.Visible = False
        FormBorderStyle = FormBorderStyle.None
        s.PreviewFormBorderStyle = FormBorderStyle
        WindowState = FormWindowState.Maximized
        Dim ratio As Double

        If Calc.IsARSignalingRequired Then
            ratio = Calc.GetTargetDAR
        Else
            ratio = AVI.FrameSize.Width / AVI.FrameSize.Height
        End If

        Dim b = Screen.FromControl(Me).Bounds

        pVideo.Dock = DockStyle.None

        If ratio > b.Width / b.Height Then
            Dim h = CInt(b.Width / ratio)
            pVideo.Left = 0
            pVideo.Top = CInt((b.Height - h) / 2)
            pVideo.Width = b.Width
            pVideo.Height = h
        Else
            Dim w = CInt(b.Height * ratio)
            pVideo.Left = CInt((b.Width - w) / 2)
            pVideo.Top = 0
            pVideo.Width = w
            pVideo.Height = b.Height
        End If

        pnTrack.Visible = trackVisible
    End Sub

    Private Sub NormalScreen()
        FormBorderStyle = FormBorderStyle.FixedDialog
        s.PreviewFormBorderStyle = FormBorderStyle
        WindowState = FormWindowState.Normal
        pVideo.Dock = DockStyle.Fill
        ClientSize = GetNormalSize()
        Dim wa = Screen.FromControl(Me).WorkingArea
        If Left + Width > wa.Width OrElse Top + Height > wa.Height Then WindowPositions.CenterScreen(Me)
        If Left < 0 Then Left = 0
        If Top < 0 Then Top = 0
    End Sub

    Function GetNormalSize() As Size
        Dim ret As Size
        Dim frameHeight = AVI.FrameSize.Height

        If Calc.IsARSignalingRequired Then
            ret = New Size(CInt(frameHeight * SizeFactor * Calc.GetTargetDAR), CInt(frameHeight * SizeFactor))
        Else
            ret = New Size(CInt(AVI.FrameSize.Width * SizeFactor), CInt(frameHeight * SizeFactor))
        End If

        Return ret
    End Function

    <Command("Switches the window state between full and normal.")>
    Sub SwitchWindowState()
        If FormBorderStyle = FormBorderStyle.None Then
            NormalScreen()
        Else
            Fullscreen()
        End If

        AfterPositionChanged()
    End Sub

    Sub ShowButtons(vis As Boolean)
        bnForward1.Visible = vis
        bnBackward1.Visible = vis
        bnRangeStart.Visible = vis
        bnRangeEnd.Visible = vis
        bnDeleteRange.Visible = vis
        bnForward10.Visible = vis
        bnBackward10.Visible = vis
        bnForward100.Visible = vis
        bnBackward100.Visible = vis
        bExtras.Visible = vis
    End Sub

    Private Sub bnForward1_Click() Handles bnForward1.Click
        For Each i As PreviewForm In Instances
            SetRelativePos(1)
        Next
    End Sub

    Private Sub bnBackward1_Click() Handles bnBackward1.Click
        For Each i As PreviewForm In Instances
            SetRelativePos(-1)
        Next
    End Sub

    Private Sub bnRangeStart_Click() Handles bnRangeStart.Click
        For Each i As PreviewForm In Instances
            SetRangeStart()
        Next
    End Sub

    Private Sub bnRangeEnd_Click() Handles bnRangeEnd.Click
        For Each i As PreviewForm In Instances
            SetRangeEnd()
        Next
    End Sub

    Private Sub bnBackward10_Click() Handles bnBackward10.Click
        For Each i As PreviewForm In Instances
            SetRelativePos(-10)
        Next
    End Sub

    Private Sub bnForward10_Click() Handles bnForward10.Click
        For Each i As PreviewForm In Instances
            SetRelativePos(10)
        Next
    End Sub

    Private Sub bnDeleteRange_Click() Handles bnDeleteRange.Click
        For Each i As PreviewForm In Instances
            DeleteRange()
        Next
    End Sub

    Private Sub bnBackward100_Click() Handles bnBackward100.Click
        For Each i As PreviewForm In Instances
            i.SetRelativePos(-100)
        Next
    End Sub

    Private Sub bnForward100_Click() Handles bnForward100.Click
        For Each i As PreviewForm In Instances
            SetRelativePos(100)
        Next
    End Sub

    Private Sub Wheel(sender As Object, e As MouseEventArgs) Handles MyBase.MouseWheel
        Dim pos = 1
        If Control.ModifierKeys = Keys.Control Then pos = 10
        If Control.ModifierKeys = Keys.Shift Then pos = 100
        If Control.ModifierKeys = Keys.Alt Then pos = 1000
        If e.Delta < 0 Then pos = pos * -1
        If s.ReverseVideoScrollDirection Then pos = pos * -1
        SetRelativePos(pos)
    End Sub

    Sub DrawTrack()
        Dim g = pnTrack.CreateGraphics()
        g.FillRectangle(Brushes.White, pnTrack.ClientRectangle)

        Dim trackHeight = pnTrack.Height - TrackBarBorder * 2 - TrackBarGap * 2

        Using borderPen As New Pen(Color.Black, TrackBarBorder)
            borderPen.Alignment = Drawing2D.PenAlignment.Inset
            g.DrawRectangle(borderPen, 0, 0, pnTrack.Width - 1, pnTrack.Height - 1)
        End Using

        If p.Ranges.Count > 0 Then
            For x = 0 To p.Ranges.Count - 1
                Dim c As Color

                If (x Mod 2) = 0 Then
                    c = Color.Green
                Else
                    c = Color.LimeGreen
                End If

                Using rangePen As New Pen(c, trackHeight)
                    g.DrawLine(rangePen, GetDrawPos(p.Ranges(x).Start) - CInt(TrackBarPosition / 2),
                        pnTrack.Height \ 2, GetDrawPos(p.Ranges(x).End) + CInt(TrackBarPosition / 2),
                        pnTrack.Height \ 2)
                End Using
            Next
        End If

        Using rangeSetPen As New Pen(Color.DarkOrange, trackHeight)
            If RangeStart > -1 AndAlso RangeStart <= AVI.Position Then
                g.DrawLine(rangeSetPen, GetDrawPos(RangeStart) - CInt(TrackBarPosition / 2),
                    pnTrack.Height \ 2, GetDrawPos(AVI.Position) +
                    CInt(TrackBarPosition / 2), pnTrack.Height \ 2)
            End If
        End Using

        Dim posPen As Pen

        If RangeStart > -1 Then
            posPen = New Pen(Color.DarkOrange, trackHeight)
        Else
            posPen = New Pen(Color.Black, trackHeight)
        End If

        posPen.Alignment = Drawing2D.PenAlignment.Center

        Dim pos = GetDrawPos(AVI.Position)

        g.DrawLine(posPen,
                   pos - CInt(TrackBarPosition / 2),
                   pnTrack.Height \ 2,
                   pos + CInt(TrackBarPosition / 2),
                   pnTrack.Height \ 2)

        posPen.Dispose()
        g.Dispose()
    End Sub

    Private Function GetDrawPos(frame As Integer) As Integer
        Dim values = TrackBarBorder * 2 + TrackBarGap * 2 + TrackBarPosition
        Dim width = CInt(((pnTrack.Width - values) / CInt(TargetFrames - 1)) * frame)
        Return width + CInt(values / 2)
    End Function

    Private Sub pTrack_MouseMove(sender As Object, e As MouseEventArgs) Handles pnTrack.MouseMove
        If e.Button = MouseButtons.Left Then HandleMouseOntrackBar()
    End Sub

    Private Sub pTrack_MouseDown(sender As Object, e As MouseEventArgs) Handles pnTrack.MouseDown
        HandleMouseOntrackBar()
    End Sub

    Private Sub HandleMouseOntrackBar()
        Dim pos = CInt((TargetFrames / pnTrack.Width) * pnTrack.PointToClient(Control.MousePosition).X)
        Dim remainder = pos Mod 4
        If remainder <> 0 Then pos -= remainder

        For Each i In Instances
            i.SetAbsolutePos(pos)
        Next
    End Sub

    Private Sub pTrack_Paint(sender As Object, e As PaintEventArgs) Handles pnTrack.Paint
        DrawTrack()
    End Sub

    Private Sub bExtras_Click() Handles bExtras.Click
        cmsMain.Show(bExtras, New Point(1, bExtras.Height))
    End Sub

    <Command("Jumps to a given frame.")>
    Sub SetAbsolutePos(<DispName("Position")> pos As Integer)
        SetPos(pos)
    End Sub

    <Command("Jumps a given frame count.")>
    Sub SetRelativePos(<DispName("Position"),
        Description("Frames to jump, negative values jump backward.")>
        pos As Integer)

        SetPos(AVI.Position + pos)
    End Sub

    Sub SetPos(pos As Integer)
        AVI.Position = pos
        Renderer.Draw()
        AfterPositionChanged()
    End Sub

    Private Sub AfterPositionChanged()
        s.LastPosition = AVI.Position
        DrawTrack()
        Dim time = TimeSpan.FromSeconds(AVI.Position / AVI.FrameRate).ToString.Shorten(12)
        If time.StartsWith("00") Then time = time.Substring(3)
        Text = "Preview  " & s.LastPosition & "  " + time
    End Sub

    <Command("Dialog to jump to a specific time.")>
    Sub GoToTime()
        Dim d As Date
        d = d.AddSeconds(AVI.Position / AVI.FrameRate)
        Dim value = InputBox.Show("Time:", "Go To Time", d.ToString("HH:mm:ss.fff"))

        If value <> "" Then
            Try
                AVI.Position = CInt((TimeSpan.Parse(value).TotalMilliseconds / 1000) * AVI.FrameRate)
                Renderer.Draw()
                AfterPositionChanged()
            Catch
            End Try
        End If
    End Sub

    <Command("Dialog to jump to a specific frame.")>
    Sub GoToFrame()
        Dim value = InputBox.Show("Frame:", "Go To Frame", AVI.Position.ToString)
        Dim pos As Integer

        If Integer.TryParse(value, pos) Then
            AVI.Position = pos
            Renderer.Draw()
            AfterPositionChanged()
        End If
    End Sub

    <Command("Opens the menu editor.")>
    Sub OpenMenuEditor()
        s.CustomMenuPreview = GenericMenu.Edit()
        g.SaveSettings()
    End Sub

    <Command("Opens the help.")>
    Sub OpenHelp()
        Dim f As New HelpForm
        f.Doc.WriteStart("Preview")
        f.Doc.WriteP("The preview dialog allows to preview and cut the video. Cutting can be achieved by selecting the sections that should be kept. Selected sections appear green in the trackbar. Everything not green will be trimmed off. It's possible to open more than one instance of the preview for filter comparisons.")
        f.Doc.WriteTips(GenericMenu.GetTips)
        f.Doc.WriteTable("Shortcut Keys", GenericMenu.GetKeys, False)
        f.Show()
    End Sub

    <Command("Sets the start cut position.")>
    Sub SetRangeStart()
        Dim r = GetCurrentRange()

        If r Is Nothing OrElse AVI.Position = r.End Then
            RangeStart = AVI.Position
        Else
            r.Start = AVI.Position
        End If

        MergeRanges()
        AfterPositionChanged()
    End Sub

    <Command("Sets the end cut position.")>
    Sub SetRangeEnd()
        If RangeStart > -1 Then
            p.Ranges.Add(New Range(RangeStart, AVI.Position))
            p.Ranges.Sort()
            RangeStart = -1
        Else
            Dim r = GetCurrentRange()

            If Not r Is Nothing Then
                r.End = AVI.Position
                RangeStart = -1
            End If
        End If

        MergeRanges()
        AfterPositionChanged()
    End Sub

    Sub MergeRanges()
        For Each i In p.Ranges.ToArray
            For Each i2 In p.Ranges.ToArray
                If Not i Is i2 Then
                    If (i2.Start >= i.Start AndAlso i2.Start <= i.End) OrElse
                        (i2.End >= i.Start AndAlso i2.End <= i.End) Then

                        p.Ranges.Remove(i)
                        p.Ranges.Remove(i2)
                        p.Ranges.Add(New Range(Math.Min(i.Start, i2.Start), Math.Max(i.End, i2.End)))
                        MergeRanges()
                    End If
                End If
            Next
        Next

        p.Ranges.Sort()
    End Sub

    <Command("Splits the clip or selection into two selections.")>
    Sub SplitRange()
        If p.Ranges.Count = 0 Then
            p.Ranges.Add(New Range(0, AVI.FrameCount - 1))
        End If

        For Each i In p.Ranges.ToArray
            If i.Start < AVI.Position AndAlso i.End > AVI.Position Then
                p.Ranges.Add(New Range(i.Start, AVI.Position))
                p.Ranges.Add(New Range(AVI.Position + 1, i.End))
                p.Ranges.Remove(i)
                Exit For
            End If
        Next

        p.Ranges.Sort()

        AfterPositionChanged()
    End Sub

    <Command("Clears all cuts.")>
    Sub ClearAllRanges()
        p.Ranges.Clear()
        AfterPositionChanged()
    End Sub

    <Command("Deletes the range that encloses the current position.")>
    Sub DeleteRange()
        For Each i As Range In p.Ranges.ToArray
            If AVI.Position >= i.Start AndAlso AVI.Position <= i.End Then
                p.Ranges.Remove(i)
            End If
        Next

        AfterPositionChanged()
    End Sub

    <Command("Shows/hides the buttons.")>
    Sub ShowHideButtons()
        s.HidePreviewButtons = Not s.HidePreviewButtons
        ShowButtons(Not s.HidePreviewButtons)
        AfterPositionChanged()
    End Sub

    <Command("Shows/hides the trackbar.")>
    Sub ShowHideTrackbar()
        pnTrack.Visible = Not pnTrack.Visible
        AfterPositionChanged()
    End Sub

    <Command("Changes the size.")>
    Sub Zoom(<DispName("Factor")> factor As Single)
        SizeFactor += factor
        SizeFactor = Math.Round(SizeFactor, 2)
        NormalScreen()
        Left = (Screen.FromControl(Me).WorkingArea.Width - Width) \ 2
        Top = (Screen.FromControl(Me).WorkingArea.Height - Height) \ 2
        AfterPositionChanged()
    End Sub

    <Command("Shows/hides various infos.")>
    Sub ToggleInfos()
        s.ShowPreviewInfo = Not s.ShowPreviewInfo
        Renderer.ShowInfo = s.ShowPreviewInfo
        Renderer.Draw()
    End Sub

    <Command("Plays the script with a player.")>
    Sub ShowExternalPlayer()
        Dim script = PreviewScript.GetNewScript()
        script.Path = p.TempDir + p.TargetFile.Base + "_play." + script.FileType
        UpdateTrim(script)
        g.PlayScript(script)
    End Sub

    <Command("Plays the script with mpv.net.")>
    Sub PlayWithMpvnet()
        Dim script = PreviewScript.GetNewScript()
        script.Path = p.TempDir + p.TargetFile.Base + "_play." + script.FileType
        UpdateTrim(script)
        g.PlayScriptWithMpvnet(script)
    End Sub

    <Command("Plays the script with MPC.")>
    Sub PlayWithMPC()
        Dim script = PreviewScript.GetNewScript()
        script.Path = p.TempDir + p.TargetFile.Base + "_play." + script.FileType
        UpdateTrim(script)
        g.PlayScriptWithMPC(script)
    End Sub

    <Command("Reloads the script.")>
    Sub Reload()
        RefreshScript()
        AfterPositionChanged()
    End Sub

    <Command("Closes the dialog.")>
    Sub CloseDialog()
        Close()
    End Sub

    Private Sub pVideo_DoubleClick() Handles pVideo.DoubleClick
        SwitchWindowState()
    End Sub

    Private Sub pVideo_MouseMove(sender As Object, e As MouseEventArgs) Handles pVideo.MouseMove
        If Not WindowState = FormWindowState.Maximized AndAlso e.Button = MouseButtons.Left Then
            Native.ReleaseCapture()
            Native.PostMessage(Handle, &HA1, New IntPtr(2), IntPtr.Zero) 'WM_NCLBUTTONDOWN, HTCAPTION
        End If
    End Sub

    <Command("Jumps to the previous cut point.")>
    Sub JumpToThePreviousRangePos()
        Dim list As New List(Of Object)

        For Each i As Range In p.Ranges
            list.Add(i.Start)
            list.Add(i.End)
        Next

        list.Add(AVI.Position - 1)

        list.Sort()

        Dim index = list.IndexOf(AVI.Position - 1) - 1

        If index >= 0 AndAlso index < list.Count Then
            AVI.Position = CInt(list(index))
            Renderer.Draw()
            AfterPositionChanged()
        End If
    End Sub

    <Command("Copies the time of the current position.")>
    Sub CopyTime()
        Dim d As Date
        d = d.AddSeconds(AVI.Position / AVI.FrameRate)
        Clipboard.SetText(d.ToString("HH:mm:ss.fff"))
    End Sub

    <Command("Jumps to the next cut point.")>
    Sub JumpToTheNextRangePos()
        Dim list As New List(Of Object)

        For Each i As Range In p.Ranges
            list.Add(i.Start)
            list.Add(i.End)
        Next

        list.Add(AVI.Position + 1)
        list.Sort()

        Dim index = list.IndexOf(AVI.Position + 1) + 1

        If index >= 0 AndAlso index < list.Count Then
            AVI.Position = CInt(list(index))
            Renderer.Draw()
            AfterPositionChanged()
        End If
    End Sub

    <Command("Saves the current frame as bitmap.")>
    Sub SaveBitmap()
        Using d As New SaveFileDialog
            d.SetFilter({"bmp"})
            d.FileName = p.TargetFile.Base + " - " & AVI.Position

            If d.ShowDialog = DialogResult.OK Then
                AVI.GetBitmap.Save(d.FileName, Imaging.ImageFormat.Bmp)
            End If
        End Using
    End Sub

    <Command("Saves the current frame as bitmap.")>
    Sub SavePng()
        Using d As New SaveFileDialog
            d.SetFilter({"png"})
            d.FileName = p.TargetFile.Base + " - " & AVI.Position

            If d.ShowDialog = DialogResult.OK Then
                AVI.GetBitmap.Save(d.FileName, Imaging.ImageFormat.Png)
            End If
        End Using
    End Sub

    <Command("Saves the current frame as JPG to the given path which can contain macros.")>
    Sub SaveJpgByPath(<DispName("File Path")>
                      <Description("File path which can contain macros.")>
                      path As String)

        path = Macro.Expand(path)
        Dim q = InputBox.Show("Enter the compression quality.", "Compression Quality", s.Storage.GetInt("preview compression quality", 95).ToString)

        If q.IsInt Then
            s.Storage.SetInt("preview compression quality", q.ToInt)
            Dim params = New EncoderParameters(1)
            params.Param(0) = New EncoderParameter(Encoder.Quality, q.ToInt)
            Dim info = ImageCodecInfo.GetImageEncoders.Where(Function(arg) arg.FormatID = ImageFormat.Jpeg.Guid).First
            AVI.GetBitmap.Save(path, info, params)
        End If
    End Sub

    <Command("Saves the current frame as JPG.")>
    Sub SaveJPG()
        Using d As New SaveFileDialog
            d.DefaultExt = "jpg"
            d.FileName = p.TargetFile.Base + " - " & AVI.Position

            If d.ShowDialog = DialogResult.OK Then SaveJpgByPath(d.FileName)
        End Using
    End Sub

    <Command("Shows a dialog to navigate to a chapter.")>
    Sub GoToChapter()
        Dim fp = p.TempDir + p.SourceFile.Base + "_chapters.txt"

        If Not File.Exists(fp) Then
            MsgError("No chapter file found.")
            Exit Sub
        End If

        Using td As New TaskDialog(Of String)
            td.MainInstruction = "Select a chapter"

            For Each i In File.ReadAllLines(fp)
                Dim left = i.Left("=")
                If left.Length <> 9 Then Continue For
                td.AddCommandLink(left.Substring(7) + "   " + i.Right("="), i.Right("="))
            Next

            If td.Show() <> "" Then
                AVI.Position = CInt((TimeSpan.Parse(td.SelectedValue).TotalMilliseconds / 1000) * AVI.FrameRate)
                Renderer.Draw()
                AfterPositionChanged()
            End If
        End Using
    End Sub

    Shared Function GetDefaultMenuPreview() As CustomMenuItem
        Dim ret As New CustomMenuItem("Root")

        ret.Add("Navigation|Go To Start", NameOf(SetAbsolutePos), Keys.Control Or Keys.Left, {0})
        ret.Add("Navigation|Go To End", NameOf(SetAbsolutePos), Keys.Control Or Keys.Right, {1000000000})
        ret.Add("Navigation|-")
        ret.Add("Navigation|Go To Frame...", NameOf(GoToFrame), Keys.G)
        ret.Add("Navigation|Go To Time...", NameOf(GoToTime), Keys.T)
        ret.Add("Navigation|Go To Chapter...", NameOf(GoToChapter), Keys.C)
        ret.Add("Navigation|-")
        ret.Add("Navigation|Go To Previous Cut Point", NameOf(JumpToThePreviousRangePos), Keys.Control Or Keys.Up)
        ret.Add("Navigation|Go To Next Cut Point", NameOf(JumpToTheNextRangePos), Keys.Control Or Keys.Down)
        ret.Add("Navigation|-")
        ret.Add("Navigation|Backward 1 Frame", NameOf(SetRelativePos), Keys.Left, {-1})
        ret.Add("Navigation|Backward 10 Frames", NameOf(SetRelativePos), Keys.Up, {-10})
        ret.Add("Navigation|Backward 100 Frames", NameOf(SetRelativePos), Keys.Prior, {-100})
        ret.Add("Navigation|Backward 1000 Frames", NameOf(SetRelativePos), Keys.Subtract, {-1000})
        ret.Add("Navigation|-")
        ret.Add("Navigation|Forward 1000 Frames", NameOf(SetRelativePos), Keys.Add, {1000})
        ret.Add("Navigation|Forward 100 Frames", NameOf(SetRelativePos), Keys.Next, {100})
        ret.Add("Navigation|Forward 10 Frames", NameOf(SetRelativePos), Keys.Down, {10})
        ret.Add("Navigation|Forward 1 Frames", NameOf(SetRelativePos), Keys.Right, {1})

        ret.Add("Cut|Begin Selection", NameOf(SetRangeStart), Keys.Home)
        ret.Add("Cut|End Selection", NameOf(SetRangeEnd), Keys.End)
        ret.Add("Cut|-")
        ret.Add("Cut|Split", NameOf(SplitRange), Keys.S)
        ret.Add("Cut|-")
        ret.Add("Cut|Delete Selection", NameOf(DeleteRange), Keys.Delete, Symbol.Delete)
        ret.Add("Cut|Delete All Selections", NameOf(ClearAllRanges), Keys.Control Or Keys.Delete)

        ret.Add("View|Infos", NameOf(ToggleInfos), Keys.I, Symbol.Info)
        ret.Add("View|Fullscreen", NameOf(SwitchWindowState), Keys.Enter, Symbol.FullScreen)
        ret.Add("View|-")
        ret.Add("View|Zoom In", NameOf(Zoom), Keys.OemMinus, Symbol.ZoomIn, {-0.25F})
        ret.Add("View|Zoom Out", NameOf(Zoom), Keys.Oemplus, Symbol.ZoomOut, {0.25F})
        ret.Add("View|-")
        ret.Add("View|Buttons", NameOf(ShowHideButtons), Keys.B)
        ret.Add("View|Trackbar", NameOf(ShowHideTrackbar), Keys.Control Or Keys.T)

        ret.Add("Tools|Reload", NameOf(Reload), Keys.R, Symbol.Refresh)
        ret.Add("Tools|Play with mpv.net", NameOf(PlayWithMpvnet), Keys.F9, Symbol.Play)
        ret.Add("Tools|Play with MPC", NameOf(PlayWithMPC), Keys.F10, Symbol.Play)
        ret.Add("Tools|-")
        ret.Add("Tools|Copy Frame Number", NameOf(g.DefaultCommands.CopyToClipboard), {"%pos_frame%"})
        ret.Add("Tools|Copy Time", NameOf(CopyTime))
        ret.Add("Tools|-")
        ret.Add("Tools|Save Png", NameOf(SavePng), Keys.Control Or Keys.P, Symbol.Save)
        ret.Add("Tools|Save Bitmap", NameOf(SaveBitmap), Keys.Control Or Keys.S, Symbol.Save)

        ret.Add("Edit Menu...", NameOf(OpenMenuEditor), Keys.M)
        ret.Add("Help...", NameOf(OpenHelp), Keys.F1, Symbol.Help)
        ret.Add("Exit", NameOf(CloseDialog), Keys.Escape)

        Return ret
    End Function

    Private Sub pVideo_Paint(sender As Object, e As PaintEventArgs) Handles pVideo.Paint
        Renderer.Draw()
    End Sub

    Private Sub GenericMenu_Command(e As CustomMenuItemEventArgs) Handles GenericMenu.Command
        e.Handled = True

        If e.Item.MethodName = "CloseDialog" Then
            ProcessMenu(e.Item)
        Else
            For Each i As PreviewForm In Instances
                i.ProcessMenu(e.Item)
            Next
        End If
    End Sub

    Sub ProcessMenu(item As CustomMenuItem)
        GenericMenu.Process(item)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        RefreshScript()
    End Sub

    Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
        MyBase.OnFormClosing(e)
        Instances.Remove(Me)
        UpdateTrim(p.Script)
        s.LastPosition = AVI.Position
        p.CutFrameCount = AVI.FrameCount
        p.CutFrameRate = AVI.FrameRate
        g.MainForm.UpdateFilters()
        Renderer.Dispose()
        AVI.Dispose()
    End Sub

    Sub UpdateTrim(script As VideoScript)
        script.RemoveFilter("Cutting")

        If p.Ranges.Count > 0 Then
            Dim cutFilter As New VideoFilter
            cutFilter.Path = "Cutting"
            cutFilter.Category = "Cutting"
            cutFilter.Script = GetTrim()
            cutFilter.Active = True
            script.Filters.Add(cutFilter)
        End If
    End Sub

    Function GetTrim() As String
        Dim ret As String

        For Each i In p.Ranges
            If ret <> "" Then
                ret += " + "
            End If

            If PreviewScript.Engine = ScriptEngine.AviSynth Then
                ret += "Trim(" & i.Start & ", " & i.End - 1 & ")"

                If p.TrimCode <> "" Then
                    ret += "." + p.TrimCode.TrimStart("."c)
                End If
            Else
                ret += "clip[" & i.Start & ":" & i.End & "]"
            End If
        Next

        If PreviewScript.Engine = ScriptEngine.AviSynth Then
            Return ret
        Else
            Return "clip = " + ret
        End If
    End Function

    Private Sub Control_Enter() Handles bnBackward100.Enter, bnBackward10.Enter, bnBackward1.Enter, bnForward1.Enter, bnForward10.Enter, bnForward100.Enter, bnRangeEnd.Enter, bnRangeStart.Enter, bnDeleteRange.Enter, bExtras.Enter
        ActiveControl = Nothing
    End Sub

    Function GetCurrentRange() As Range
        For Each i In p.Ranges
            If AVI.Position >= i.Start AndAlso AVI.Position <= i.End Then
                Return i
            End If
        Next
    End Function

    Protected Overrides Sub OnHelpButtonClicked(e As CancelEventArgs)
        e.Cancel = True
        MyBase.OnHelpButtonClicked(e)
        OpenHelp()
    End Sub

    Private Sub pVideo_MouseDown(sender As Object, e As MouseEventArgs) Handles pVideo.MouseDown
        Dim sb = Screen.FromControl(Me).Bounds
        Dim p1 = New Point(sb.Width, 0)
        Dim p2 = PointToScreen(e.Location)
        If Math.Abs(p1.X - p2.X) < 10 AndAlso Math.Abs(p1.Y - p2.Y) < 10 Then Close()
    End Sub

    Private Sub pVideo_MouseClick(sender As Object, e As MouseEventArgs) Handles pVideo.MouseClick
        If pVideo.Width - e.Location.X < 10 AndAlso e.Location.Y < 10 Then
            Close()
        End If
    End Sub

    Private Sub PreviewForm_MouseClick(sender As Object, e As MouseEventArgs) Handles Me.MouseClick
        If Width - e.Location.X < 10 AndAlso e.Location.Y < 10 Then
            Close()
        End If
    End Sub
End Class