
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

    Friend WithEvents pnVideo As System.Windows.Forms.Panel
    Friend WithEvents pnTrack As System.Windows.Forms.Panel
    Private WithEvents bnDelete As ButtonEx
    Private WithEvents bnRight1 As ButtonEx
    Private WithEvents bnStartCutRange As ButtonEx
    Private WithEvents bnEndCutRange As ButtonEx
    Private WithEvents bnRight2 As ButtonEx
    Private WithEvents bnLeft2 As ButtonEx
    Private WithEvents bnMenu As ButtonEx
    Private WithEvents bnRight3 As ButtonEx
    Private WithEvents bnLeft3 As ButtonEx
    Private WithEvents bnLeft1 As ButtonEx
    Friend WithEvents cmsMain As ContextMenuStripEx
    Friend WithEvents ToolTip As System.Windows.Forms.ToolTip

    '<System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.bnDelete = New StaxRip.UI.ButtonEx()
        Me.bnRight1 = New StaxRip.UI.ButtonEx()
        Me.bnLeft1 = New StaxRip.UI.ButtonEx()
        Me.bnStartCutRange = New StaxRip.UI.ButtonEx()
        Me.bnEndCutRange = New StaxRip.UI.ButtonEx()
        Me.bnRight2 = New StaxRip.UI.ButtonEx()
        Me.bnLeft2 = New StaxRip.UI.ButtonEx()
        Me.bnMenu = New StaxRip.UI.ButtonEx()
        Me.bnRight3 = New StaxRip.UI.ButtonEx()
        Me.bnLeft3 = New StaxRip.UI.ButtonEx()
        Me.pnVideo = New System.Windows.Forms.Panel()
        Me.cmsMain = New StaxRip.UI.ContextMenuStripEx(Me.components)
        Me.pnTrack = New System.Windows.Forms.Panel()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.pnVideo.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnDelete
        '
        Me.bnDelete.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnDelete.BackColor = System.Drawing.Color.WhiteSmoke
        Me.bnDelete.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnDelete.Location = New System.Drawing.Point(671, 629)
        Me.bnDelete.Margin = New System.Windows.Forms.Padding(6)
        Me.bnDelete.Size = New System.Drawing.Size(70, 70)
        Me.bnDelete.Symbol = StaxRip.UI.ButtonEx.ButtonSymbol.Delete
        Me.bnDelete.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnDelete, "Deletes the cut selection that encloses the current position.")
        '
        'bnRight1
        '
        Me.bnRight1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnRight1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.bnRight1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnRight1.Location = New System.Drawing.Point(425, 629)
        Me.bnRight1.Margin = New System.Windows.Forms.Padding(6)
        Me.bnRight1.Size = New System.Drawing.Size(70, 70)
        Me.bnRight1.Symbol = StaxRip.UI.ButtonEx.ButtonSymbol.Right1
        Me.bnRight1.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnRight1, "Forward 1 Frames")
        '
        'bnLeft1
        '
        Me.bnLeft1.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnLeft1.BackColor = System.Drawing.Color.WhiteSmoke
        Me.bnLeft1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnLeft1.Location = New System.Drawing.Point(179, 629)
        Me.bnLeft1.Margin = New System.Windows.Forms.Padding(6)
        Me.bnLeft1.Size = New System.Drawing.Size(70, 70)
        Me.bnLeft1.Symbol = StaxRip.UI.ButtonEx.ButtonSymbol.Left1
        Me.bnLeft1.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnLeft1, "Backward 1 Frame")
        '
        'bnOpen
        '
        Me.bnStartCutRange.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnStartCutRange.BackColor = System.Drawing.Color.WhiteSmoke
        Me.bnStartCutRange.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnStartCutRange.Location = New System.Drawing.Point(261, 629)
        Me.bnStartCutRange.Margin = New System.Windows.Forms.Padding(6)
        Me.bnStartCutRange.Size = New System.Drawing.Size(70, 70)
        Me.bnStartCutRange.Symbol = StaxRip.UI.ButtonEx.ButtonSymbol.Open
        Me.bnStartCutRange.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnStartCutRange, "Sets a start cut point. Press F1 for help about cutting")
        '
        'bnClose
        '
        Me.bnEndCutRange.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnEndCutRange.BackColor = System.Drawing.Color.WhiteSmoke
        Me.bnEndCutRange.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnEndCutRange.Location = New System.Drawing.Point(343, 629)
        Me.bnEndCutRange.Margin = New System.Windows.Forms.Padding(6)
        Me.bnEndCutRange.Size = New System.Drawing.Size(70, 70)
        Me.bnEndCutRange.Symbol = StaxRip.UI.ButtonEx.ButtonSymbol.Close
        Me.bnEndCutRange.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnEndCutRange, "Sets a end cut point. Press F1 for help about cutting")
        '
        'bnRight2
        '
        Me.bnRight2.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnRight2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.bnRight2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnRight2.Location = New System.Drawing.Point(507, 629)
        Me.bnRight2.Margin = New System.Windows.Forms.Padding(6)
        Me.bnRight2.Size = New System.Drawing.Size(70, 70)
        Me.bnRight2.Symbol = StaxRip.UI.ButtonEx.ButtonSymbol.Right2
        Me.bnRight2.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnRight2, "Forward 10 Frames")
        '
        'bnLeft2
        '
        Me.bnLeft2.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnLeft2.BackColor = System.Drawing.Color.WhiteSmoke
        Me.bnLeft2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnLeft2.Location = New System.Drawing.Point(97, 629)
        Me.bnLeft2.Margin = New System.Windows.Forms.Padding(6)
        Me.bnLeft2.Size = New System.Drawing.Size(70, 70)
        Me.bnLeft2.Symbol = StaxRip.UI.ButtonEx.ButtonSymbol.Left2
        Me.bnLeft2.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnLeft2, "Backward 10 Frames")
        '
        'bnMenu
        '
        Me.bnMenu.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnMenu.BackColor = System.Drawing.Color.WhiteSmoke
        Me.bnMenu.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bnMenu.Location = New System.Drawing.Point(753, 629)
        Me.bnMenu.Margin = New System.Windows.Forms.Padding(6)
        Me.bnMenu.Size = New System.Drawing.Size(70, 70)
        Me.bnMenu.Symbol = StaxRip.UI.ButtonEx.ButtonSymbol.Menu
        Me.bnMenu.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnMenu, "Shows the menu")
        '
        'bnRight3
        '
        Me.bnRight3.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnRight3.BackColor = System.Drawing.Color.WhiteSmoke
        Me.bnRight3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnRight3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bnRight3.Location = New System.Drawing.Point(589, 629)
        Me.bnRight3.Margin = New System.Windows.Forms.Padding(6)
        Me.bnRight3.Size = New System.Drawing.Size(70, 70)
        Me.bnRight3.Symbol = StaxRip.UI.ButtonEx.ButtonSymbol.Right3
        Me.bnRight3.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnRight3, "Forward 100 Frames")
        '
        'bnLeft3
        '
        Me.bnLeft3.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.bnLeft3.BackColor = System.Drawing.Color.WhiteSmoke
        Me.bnLeft3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.bnLeft3.Font = New System.Drawing.Font("Segoe UI", 4.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bnLeft3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.bnLeft3.Location = New System.Drawing.Point(15, 629)
        Me.bnLeft3.Margin = New System.Windows.Forms.Padding(6)
        Me.bnLeft3.Size = New System.Drawing.Size(70, 70)
        Me.bnLeft3.Symbol = StaxRip.UI.ButtonEx.ButtonSymbol.Left3
        Me.bnLeft3.TabStop = False
        Me.ToolTip.SetToolTip(Me.bnLeft3, "Backward 100 Frames")
        '
        'pnVideo
        '
        Me.pnVideo.ContextMenuStrip = Me.cmsMain
        Me.pnVideo.Controls.Add(Me.bnMenu)
        Me.pnVideo.Controls.Add(Me.bnDelete)
        Me.pnVideo.Controls.Add(Me.bnRight3)
        Me.pnVideo.Controls.Add(Me.bnRight2)
        Me.pnVideo.Controls.Add(Me.bnRight1)
        Me.pnVideo.Controls.Add(Me.bnEndCutRange)
        Me.pnVideo.Controls.Add(Me.bnStartCutRange)
        Me.pnVideo.Controls.Add(Me.bnLeft1)
        Me.pnVideo.Controls.Add(Me.bnLeft2)
        Me.pnVideo.Controls.Add(Me.bnLeft3)
        Me.pnVideo.Controls.Add(Me.pnTrack)
        Me.pnVideo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnVideo.Location = New System.Drawing.Point(0, 0)
        Me.pnVideo.Margin = New System.Windows.Forms.Padding(2)
        Me.pnVideo.Name = "pnVideo"
        Me.pnVideo.Size = New System.Drawing.Size(837, 714)
        Me.pnVideo.TabIndex = 50
        '
        'cmsMain
        '
        Me.cmsMain.Font = New System.Drawing.Font("Segoe UI", 9.0!)
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
        Me.pnTrack.Location = New System.Drawing.Point(15, 587)
        Me.pnTrack.Margin = New System.Windows.Forms.Padding(2)
        Me.pnTrack.Name = "pnTrack"
        Me.pnTrack.Size = New System.Drawing.Size(807, 25)
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
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.Black
        Me.ClientSize = New System.Drawing.Size(837, 714)
        Me.Controls.Add(Me.pnVideo)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Name = "PreviewForm"
        Me.ShowInTaskbar = True
        Me.Text = "Preview"
        Me.pnVideo.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private FrameServer As FrameServer
    Private Renderer As VideoRenderer
    Private StartRange As Integer = -1
    Private EndRange As Integer = -1
    Private PreviewScript As VideoScript
    Private SizeFactor As Double = 1
    Private WithEvents GenericMenu As CustomMenu
    Private CommandManager As New CommandManager

    Private Const TrackBarBorder As Integer = 1
    Private Const TrackBarGap As Integer = 1
    Private TrackBarPosition As Integer = CInt(Control.DefaultFont.Height / 4) - 1

    Private Shared Instances As New List(Of PreviewForm)

    Sub New(script As VideoScript)
        MyBase.New()
        InitializeComponent()
        Icon = g.Icon

        CommandManager.AddCommandsFromObject(Me)
        CommandManager.AddCommandsFromObject(g.DefaultCommands)

        GenericMenu = New CustomMenu(AddressOf GetDefaultMenu,
            s.CustomMenuPreview, CommandManager, cmsMain)

        GenericMenu.AddKeyDownHandler(Me)
        GenericMenu.BuildMenu()

        Instances.Add(Me)

        PreviewScript = script
        NormalRectangle.Size = Size
        NormalRectangle.Location = Location
    End Sub

    Sub RefreshScript()
        If Not Renderer Is Nothing Then
            Renderer.Dispose()
        End If

        If Not FrameServer Is Nothing Then
            FrameServer.Dispose()
        End If

        PreviewScript.Synchronize()
        PreviewScript.Synchronize(True, True, True)
        FrameServer = New FrameServer(PreviewScript.Path)
        Renderer = New VideoRenderer(pnVideo, FrameServer)
        Renderer.Info = PreviewScript.OriginalInfo
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

        If s.LastPosition < FrameServer.Info.FrameCount - 1 Then
            Renderer.Position = s.LastPosition
        End If

        AfterPositionChanged()
        ShowButtons(Not s.HidePreviewButtons)
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
            ratio = FrameServer.Info.Width / FrameServer.Info.Height
        End If

        Dim b = Screen.FromControl(Me).Bounds

        pnVideo.Dock = DockStyle.None

        If ratio > b.Width / b.Height Then
            Dim h = CInt(b.Width / ratio)
            pnVideo.Left = 0
            pnVideo.Top = CInt((b.Height - h) / 2)
            pnVideo.Width = b.Width
            pnVideo.Height = h
        Else
            Dim w = CInt(b.Height * ratio)
            pnVideo.Left = CInt((b.Width - w) / 2)
            pnVideo.Top = 0
            pnVideo.Width = w
            pnVideo.Height = b.Height
        End If

        pnTrack.Visible = trackVisible
    End Sub

    Private Sub NormalScreen()
        FormBorderStyle = FormBorderStyle.FixedDialog
        s.PreviewFormBorderStyle = FormBorderStyle
        WindowState = FormWindowState.Normal
        pnVideo.Dock = DockStyle.Fill
        ClientSize = GetNormalSize()
        Dim wa = Screen.FromControl(Me).WorkingArea

        If Left + Width > wa.Width OrElse Top + Height > wa.Height Then
            WindowPositions.CenterScreen(Me)
        End If

        If Left < 0 Then Left = 0
        If Top < 0 Then Top = 0
    End Sub

    Function GetNormalSize() As Size
        Dim ret As Size
        Dim frameHeight = FrameServer.Info.Height

        If Calc.IsARSignalingRequired Then
            ret = New Size(CInt(frameHeight * SizeFactor * Calc.GetTargetDAR), CInt(frameHeight * SizeFactor))
        Else
            ret = New Size(CInt(FrameServer.Info.Width * SizeFactor), CInt(frameHeight * SizeFactor))
        End If

        Return ret
    End Function

    <Command("Switches the window state between full and normal.")>
    Sub SwitchWindowState()
        ShowButtons(False)

        If FormBorderStyle = FormBorderStyle.None Then
            NormalScreen()
        Else
            Fullscreen()
        End If

        ShowButtons(Not s.HidePreviewButtons)
        AfterPositionChanged()
    End Sub

    Sub ShowButtons(vis As Boolean)
        bnRight1.Visible = vis
        bnLeft1.Visible = vis
        bnStartCutRange.Visible = vis
        bnEndCutRange.Visible = vis
        bnDelete.Visible = vis
        bnRight2.Visible = vis
        bnLeft2.Visible = vis
        bnRight3.Visible = vis
        bnLeft3.Visible = vis
        bnMenu.Visible = vis
    End Sub

    Private Sub bnRight1_Click() Handles bnRight1.Click
        For Each i As PreviewForm In Instances
            SetRelativePos(1)
        Next
    End Sub

    Private Sub bnLeft1_Click() Handles bnLeft1.Click
        For Each i As PreviewForm In Instances
            SetRelativePos(-1)
        Next
    End Sub

    Private Sub bnStartCutRange_Click() Handles bnStartCutRange.Click
        For Each i As PreviewForm In Instances
            SetRangeStart()
        Next
    End Sub

    Private Sub bnEndCutRange_Click() Handles bnEndCutRange.Click
        For Each i As PreviewForm In Instances
            SetRangeEnd()
        Next
    End Sub

    Private Sub bnLeft2_Click() Handles bnLeft2.Click
        For Each i As PreviewForm In Instances
            SetRelativePos(-10)
        Next
    End Sub

    Private Sub bnRight2_Click() Handles bnRight2.Click
        For Each i As PreviewForm In Instances
            SetRelativePos(10)
        Next
    End Sub

    Private Sub bnDelete_Click() Handles bnDelete.Click
        For Each i As PreviewForm In Instances
            DeleteRange()
        Next
    End Sub

    Private Sub bnLeft3_Click() Handles bnLeft3.Click
        For Each i As PreviewForm In Instances
            i.SetRelativePos(-100)
        Next
    End Sub

    Private Sub bnRight3_Click() Handles bnRight3.Click
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
            If StartRange > -1 AndAlso StartRange <= Renderer.Position Then
                g.DrawLine(rangeSetPen, GetDrawPos(StartRange) - CInt(TrackBarPosition / 2),
                    pnTrack.Height \ 2, GetDrawPos(Renderer.Position) +
                    CInt(TrackBarPosition / 2), pnTrack.Height \ 2)
            End If
        End Using

        Using rangeSetPen As New Pen(Color.DarkOrange, trackHeight)
            If EndRange > -1 AndAlso EndRange >= Renderer.Position Then
                g.DrawLine(rangeSetPen, GetDrawPos(EndRange) - CInt(TrackBarPosition / 2),
                    pnTrack.Height \ 2, GetDrawPos(Renderer.Position) +
                    CInt(TrackBarPosition / 2), pnTrack.Height \ 2)
            End If
        End Using

        Dim posPen As Pen

        If StartRange > -1 OrElse EndRange > -1 Then
            posPen = New Pen(Color.DarkOrange, trackHeight)
        Else
            posPen = New Pen(Color.Black, trackHeight)
        End If

        posPen.Alignment = Drawing2D.PenAlignment.Center

        Dim pos = GetDrawPos(Renderer.Position)

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
        Dim width = CInt(((pnTrack.Width - values) / CInt(PreviewScript.Info.FrameCount - 1)) * frame)
        Return width + CInt(values / 2)
    End Function

    Private Sub pTrack_MouseMove(sender As Object, e As MouseEventArgs) Handles pnTrack.MouseMove
        If e.Button = MouseButtons.Left Then HandleMouseOntrackBar()
    End Sub

    Private Sub pTrack_MouseDown(sender As Object, e As MouseEventArgs) Handles pnTrack.MouseDown
        HandleMouseOntrackBar()
    End Sub

    Private Sub HandleMouseOntrackBar()
        Dim pos = CInt((PreviewScript.Info.FrameCount / pnTrack.Width) * pnTrack.PointToClient(Control.MousePosition).X)
        Dim remainder = pos Mod 4

        If remainder <> 0 Then
            pos -= remainder
        End If

        For Each i In Instances
            i.SetAbsolutePos(pos)
        Next
    End Sub

    Private Sub pnTrack_Paint(sender As Object, e As PaintEventArgs) Handles pnTrack.Paint
        DrawTrack()
    End Sub

    Private Sub bnExtras_Click() Handles bnMenu.Click
        cmsMain.Show(bnMenu, New Point(1, bnMenu.Height))
    End Sub

    <Command("Jumps to a given frame.")>
    Sub SetAbsolutePos(<DispName("Position")> pos As Integer)
        SetPos(pos)
    End Sub

    <Command("Jumps a given frame count.")>
    Sub SetRelativePos(<DispName("Position"),
        Description("Frames to jump, negative values jump backward.")>
        pos As Integer)

        SetPos(Renderer.Position + pos)
    End Sub

    Sub SetPos(pos As Integer)
        Renderer.Position = pos
        Renderer.Draw()
        AfterPositionChanged()
    End Sub

    Private Sub AfterPositionChanged()
        s.LastPosition = Renderer.Position
        DrawTrack()
        Dim time = TimeSpan.FromSeconds(Renderer.Position / FrameServer.FrameRate).ToString.Shorten(12)
        If time.StartsWith("00") Then time = time.Substring(3)
        Text = "Preview  " & s.LastPosition & "  " + time
    End Sub

    <Command("Dialog to jump to a specific time.")>
    Sub GoToTime()
        Dim d As Date
        d = d.AddSeconds(Renderer.Position / FrameServer.FrameRate)
        Dim value = InputBox.Show("Time:", "Go To Time", d.ToString("HH:mm:ss.fff"))

        If value <> "" Then
            Try
                Renderer.Position = CInt((TimeSpan.Parse(value).TotalMilliseconds / 1000) * FrameServer.FrameRate)
                Renderer.Draw()
                AfterPositionChanged()
            Catch
            End Try
        End If
    End Sub

    <Command("Dialog to jump to a specific frame.")>
    Sub GoToFrame()
        Dim value = InputBox.Show("Frame:", "Go To Frame", Renderer.Position.ToString)
        Dim pos As Integer

        If Integer.TryParse(value, pos) Then
            Renderer.Position = pos
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
        Dim form As New HelpForm
        form.Doc.WriteStart("Preview")
        form.Doc.WriteP("The preview dialog allows to preview and cut the video. It's possible to open more than one instance of the preview for filter comparisons.")
        form.Doc.WriteH2("Cutting")
        form.Doc.WriteP("Cutting can be achieved by selecting the sections that should be kept. Selected sections appear green in the trackbar. Cutting in muxing mode without video encoding works only with mkv muxer (using the nearest key frames).")
        form.Doc.WriteTips(GenericMenu.GetTips)
        form.Doc.WriteTable("Shortcut Keys", GenericMenu.GetKeys, False)
        form.Show()
    End Sub

    <Command("Sets the start cut position.")>
    Sub SetRangeStart()
        If EndRange > -1 Then
            p.Ranges.Add(New Range(Renderer.Position, EndRange))
            p.Ranges.Sort()
            EndRange = -1
        Else
            Dim currentRange = GetCurrentRange()

            If currentRange Is Nothing OrElse Renderer.Position = currentRange.End Then
                StartRange = Renderer.Position
            Else
                currentRange.Start = Renderer.Position
                EndRange = -1
            End If
        End If

        MergeRanges()
        AfterPositionChanged()
    End Sub

    <Command("Sets the end cut position.")>
    Sub SetRangeEnd()
        If StartRange > -1 Then
            p.Ranges.Add(New Range(StartRange, Renderer.Position))
            p.Ranges.Sort()
            StartRange = -1
        Else
            Dim currentRange = GetCurrentRange()

            If currentRange Is Nothing Then
                EndRange = Renderer.Position
            Else
                currentRange.End = Renderer.Position
                StartRange = -1
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
            p.Ranges.Add(New Range(0, FrameServer.Info.FrameCount - 1))
        End If

        For Each i In p.Ranges.ToArray
            If i.Start < Renderer.Position AndAlso i.End > Renderer.Position Then
                p.Ranges.Add(New Range(i.Start, Renderer.Position))
                p.Ranges.Add(New Range(Renderer.Position + 1, i.End))
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
            If Renderer.Position >= i.Start AndAlso Renderer.Position <= i.End Then
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
        g.PlayScriptWithMPV(script, "--start=" + GetPlayPosition.ToString.TrimEnd("0"c))
    End Sub

    <Command("Plays the script with MPC.")>
    Sub PlayWithMPC()
        Dim script = PreviewScript.GetNewScript()
        script.Path = p.TempDir + p.TargetFile.Base + "_play." + script.FileType
        UpdateTrim(script)
        g.PlayScriptWithMPC(script, "/start " & GetPlayPosition.TotalMilliseconds)
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

    <Command("Jumps to the previous cut point.")>
    Sub JumpToThePreviousRangePos()
        Dim list As New List(Of Object)

        For Each i As Range In p.Ranges
            list.Add(i.Start)
            list.Add(i.End)
        Next

        list.Add(Renderer.Position - 1)

        list.Sort()

        Dim index = list.IndexOf(Renderer.Position - 1) - 1

        If index >= 0 AndAlso index < list.Count Then
            Renderer.Position = CInt(list(index))
            Renderer.Draw()
            AfterPositionChanged()
        End If
    End Sub

    <Command("Copies the time of the current position.")>
    Sub CopyTime()
        Dim d As Date
        d = d.AddSeconds(Renderer.Position / FrameServer.FrameRate)
        Clipboard.SetText(d.ToString("HH:mm:ss.fff"))
    End Sub

    <Command("Jumps to the next cut point.")>
    Sub JumpToTheNextRangePos()
        Dim list As New List(Of Object)

        For Each i As Range In p.Ranges
            list.Add(i.Start)
            list.Add(i.End)
        Next

        list.Add(Renderer.Position + 1)
        list.Sort()

        Dim index = list.IndexOf(Renderer.Position + 1) + 1

        If index >= 0 AndAlso index < list.Count Then
            Renderer.Position = CInt(list(index))
            Renderer.Draw()
            AfterPositionChanged()
        End If
    End Sub

    <Command("Saves the current frame as bitmap.")>
    Sub SaveBitmap()
        Using d As New SaveFileDialog
            d.SetFilter({"bmp"})
            d.FileName = p.TargetFile.Base + " - " & Renderer.Position

            If d.ShowDialog = DialogResult.OK Then
                BitmapUtil.CreateBitmap(FrameServer, Renderer.Position).Save(d.FileName, ImageFormat.Bmp)
            End If
        End Using
    End Sub

    <Command("Saves the current frame as bitmap.")>
    Sub SavePng()
        Using d As New SaveFileDialog
            d.SetFilter({"png"})
            d.FileName = p.TargetFile.Base + " - " & Renderer.Position

            If d.ShowDialog = DialogResult.OK Then
                BitmapUtil.CreateBitmap(FrameServer, Renderer.Position).Save(d.FileName, ImageFormat.Png)
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
            BitmapUtil.CreateBitmap(FrameServer, Renderer.Position).Save(path, info, params)
        End If
    End Sub

    <Command("Saves the current frame as JPG.")>
    Sub SaveJPG()
        Using d As New SaveFileDialog
            d.DefaultExt = "jpg"
            d.FileName = p.TargetFile.Base + " - " & Renderer.Position

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
                td.AddCommand(left.Substring(7) + "   " + i.Right("="), i.Right("="))
            Next

            If td.Show() <> "" Then
                Renderer.Position = CInt((TimeSpan.Parse(td.SelectedValue).TotalMilliseconds / 1000) * FrameServer.FrameRate)
                Renderer.Draw()
                AfterPositionChanged()
            End If
        End Using
    End Sub

    Shared Function GetDefaultMenu() As CustomMenuItem
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

        ret.Add("View|Info", NameOf(ToggleInfos), Keys.I, Symbol.Info)
        ret.Add("View|Fullscreen", NameOf(SwitchWindowState), Keys.Enter, Symbol.FullScreen)
        ret.Add("View|-")
        ret.Add("View|Zoom In", NameOf(Zoom), Keys.Oemplus, Symbol.ZoomIn, {0.25F})
        ret.Add("View|Zoom Out", NameOf(Zoom), Keys.OemMinus, Symbol.ZoomOut, {-0.25F})
        ret.Add("View|-")
        ret.Add("View|Buttons", NameOf(ShowHideButtons), Keys.B)
        ret.Add("View|Trackbar", NameOf(ShowHideTrackbar), Keys.Control Or Keys.T)

        ret.Add("Tools|Reload", NameOf(Reload), Keys.R, Symbol.Refresh)
        ret.Add("Tools|Play with mpv.net", NameOf(PlayWithMpvnet), Keys.F9, Symbol.Play)
        ret.Add("Tools|Play with mpc", NameOf(PlayWithMPC), Keys.F10, Symbol.Play)
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

    Sub RefreshControls()
        bnLeft1.Refresh()
        bnLeft2.Refresh()
        bnLeft3.Refresh()
        bnStartCutRange.Refresh()
        bnEndCutRange.Refresh()
        bnRight1.Refresh()
        bnRight2.Refresh()
        bnRight3.Refresh()
        bnDelete.Refresh()
        bnMenu.Refresh()
        pnTrack.Refresh()
    End Sub

    Private Sub pVideo_DoubleClick() Handles pnVideo.DoubleClick
        SwitchWindowState()
    End Sub

    Private Sub pVideo_MouseMove(sender As Object, e As MouseEventArgs) Handles pnVideo.MouseMove
        If Not WindowState = FormWindowState.Maximized AndAlso e.Button = MouseButtons.Left Then
            Native.ReleaseCapture()
            Native.PostMessage(Handle, &HA1, New IntPtr(2), IntPtr.Zero) 'WM_NCLBUTTONDOWN, HTCAPTION
        End If
    End Sub

    Private Sub pVideo_Paint(sender As Object, e As PaintEventArgs) Handles pnVideo.Paint
        RefreshControls()
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
        s.LastPosition = Renderer.Position
        p.CutFrameCount = FrameServer.Info.FrameCount
        p.CutFrameRate = FrameServer.FrameRate
        g.MainForm.UpdateFilters()
        Renderer.Dispose()
        FrameServer.Dispose()
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

    Private Sub Control_Enter() Handles bnLeft3.Enter, bnLeft2.Enter, bnLeft1.Enter, bnRight1.Enter, bnRight2.Enter, bnRight3.Enter, bnEndCutRange.Enter, bnStartCutRange.Enter, bnDelete.Enter, bnMenu.Enter
        ActiveControl = Nothing
    End Sub

    Function GetCurrentRange() As Range
        For Each i In p.Ranges
            If Renderer.Position >= i.Start AndAlso Renderer.Position <= i.End Then
                Return i
            End If
        Next
    End Function

    Function GetPlayPosition() As TimeSpan
        If p.Ranges.Count = 0 Then
            Return TimeSpan.FromSeconds(Renderer.Position / FrameServer.FrameRate)
        Else
            If GetCurrentRange() Is Nothing Then
                Return TimeSpan.Zero
            Else
                Dim frames As Integer

                For x = 0 To p.Ranges.Count - 1
                    Dim current = p.Ranges(x)

                    If current.End < Renderer.Position Then
                        frames += current.End - current.Start
                    Else
                        Dim pos = Renderer.Position

                        If pos = current.End AndAlso x = p.Ranges.Count - 1 Then
                            pos -= 2
                        End If

                        Return TimeSpan.FromSeconds((frames + (pos - current.Start)) / FrameServer.FrameRate)
                    End If
                Next
            End If
        End If
    End Function

    Protected Overrides Sub OnHelpButtonClicked(e As CancelEventArgs)
        e.Cancel = True
        MyBase.OnHelpButtonClicked(e)
        OpenHelp()
    End Sub

    Private Sub pVideo_MouseDown(sender As Object, e As MouseEventArgs) Handles pnVideo.MouseDown
        Dim sb = Screen.FromControl(Me).Bounds
        Dim p1 = New Point(sb.Width, 0)
        Dim p2 = PointToScreen(e.Location)

        If Math.Abs(p1.X - p2.X) < 10 AndAlso Math.Abs(p1.Y - p2.Y) < 10 Then
            Close()
        End If
    End Sub

    Private Sub pVideo_MouseClick(sender As Object, e As MouseEventArgs) Handles pnVideo.MouseClick
        If pnVideo.Width - e.Location.X < 10 AndAlso e.Location.Y < 10 Then
            Close()
        End If
    End Sub

    Private Sub PreviewForm_MouseClick(sender As Object, e As MouseEventArgs) Handles Me.MouseClick
        If Width - e.Location.X < 10 AndAlso e.Location.Y < 10 Then
            Close()
        End If
    End Sub
End Class