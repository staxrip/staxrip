
Imports System.ComponentModel
Imports System.Drawing.Imaging
Imports System.Reflection
Imports System.Runtime.InteropServices
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
        'bnStartCutRange
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
        'bnEndCutRange
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

    Shared Property Instances As New List(Of PreviewForm)

    Private FrameServer As IFrameServer
    Private Renderer As VideoRenderer
    Private StartRange As Integer = -1
    Private EndRange As Integer = -1
    Private PreviewScript As VideoScript
    Private CommandManager As New CommandManager
    Private TrackBarBorder As Integer = 1
    Private TrackBarGap As Integer = 1
    Private TrackBarPosition As Integer = CInt(Control.DefaultFont.Height / 4) - 1
    Private VideoSize As Size
    Private ShowPreviewInfo As Boolean
    Private HidePreviewButtons As Boolean

    Private WithEvents GenericMenu As CustomMenu

    Sub New(script As VideoScript)
        InitializeComponent()

        GetType(Panel).InvokeMember("DoubleBuffered", BindingFlags.SetProperty Or
            BindingFlags.Instance Or BindingFlags.NonPublic, Nothing,
            pnTrack, New Object() {True})

        Icon = g.Icon

        CommandManager.AddCommandsFromObject(Me)
        CommandManager.AddCommandsFromObject(g.DefaultCommands)

        GenericMenu = New CustomMenu(AddressOf GetDefaultMenu,
            s.CustomMenuPreview, CommandManager, cmsMain)

        GenericMenu.AddKeyDownHandler(Me)
        GenericMenu.BuildMenu()

        Instances.Add(Me)
        ShowPreviewInfo = s.ShowPreviewInfo
        HidePreviewButtons = s.HidePreviewButtons

        PreviewScript = script
    End Sub

    Sub Fullscreen()
        BlockVideoPaint = True
        FormBorderStyle = FormBorderStyle.None
        s.PreviewFormBorderStyle = FormBorderStyle
        WindowState = FormWindowState.Maximized
        Dim screenBounds = Screen.FromControl(Me).Bounds
        pnVideo.Dock = DockStyle.None
        Dim ratio = VideoSize.Width / VideoSize.Height

        If ratio > screenBounds.Width / screenBounds.Height Then
            Dim h = CInt(screenBounds.Width / ratio)
            pnVideo.Left = 0
            pnVideo.Top = CInt((screenBounds.Height - h) / 2)
            pnVideo.Width = screenBounds.Width
            pnVideo.Height = h
        Else
            Dim w = CInt(screenBounds.Height * ratio)
            pnVideo.Left = CInt((screenBounds.Width - w) / 2)
            pnVideo.Top = 0
            pnVideo.Width = w
            pnVideo.Height = screenBounds.Height
        End If

        BlockVideoPaint = False
        pnVideo.Refresh()
    End Sub

    Sub NormalScreen()
        FormBorderStyle = FormBorderStyle.Sizable
        s.PreviewFormBorderStyle = FormBorderStyle
        WindowState = FormWindowState.Normal
        pnVideo.Dock = DockStyle.Fill
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

    Sub bnRight1_Click() Handles bnRight1.Click
        For Each i In Instances
            SetRelativePos(1)
        Next
    End Sub

    Sub bnLeft1_Click() Handles bnLeft1.Click
        For Each i In Instances
            SetRelativePos(-1)
        Next
    End Sub

    Sub bnStartCutRange_Click() Handles bnStartCutRange.Click
        For Each i In Instances
            SetRangeStart()
        Next
    End Sub

    Sub bnEndCutRange_Click() Handles bnEndCutRange.Click
        For Each i In Instances
            SetRangeEnd()
        Next
    End Sub

    Sub bnLeft2_Click() Handles bnLeft2.Click
        For Each i In Instances
            SetRelativePos(-10)
        Next
    End Sub

    Sub bnRight2_Click() Handles bnRight2.Click
        For Each i In Instances
            SetRelativePos(10)
        Next
    End Sub

    Sub bnDelete_Click() Handles bnDelete.Click
        For Each i In Instances
            DeleteRange()
        Next
    End Sub

    Sub bnLeft3_Click() Handles bnLeft3.Click
        For Each i In Instances
            i.SetRelativePos(-100)
        Next
    End Sub

    Sub bnRight3_Click() Handles bnRight3.Click
        For Each i In Instances
            SetRelativePos(100)
        Next
    End Sub

    Function GetDrawPos(frame As Integer) As Integer
        Dim values = TrackBarBorder * 2 + TrackBarGap * 2 + TrackBarPosition
        Dim width = CInt(((pnTrack.Width - values) / CInt(PreviewScript.Info.FrameCount - 1)) * frame)
        Return width + CInt(values / 2)
    End Function

    Sub pnTrack_MouseMove(sender As Object, e As MouseEventArgs) Handles pnTrack.MouseMove
        If e.Button = MouseButtons.Left Then
            HandleMouseOntrackBar()
        End If
    End Sub

    Sub pnTrack_MouseDown(sender As Object, e As MouseEventArgs) Handles pnTrack.MouseDown
        HandleMouseOntrackBar()
    End Sub

    Sub HandleMouseOntrackBar()
        Dim pos = CInt((PreviewScript.Info.FrameCount / pnTrack.Width) * pnTrack.PointToClient(Control.MousePosition).X)
        Dim remainder = pos Mod 4

        If remainder <> 0 Then
            pos -= remainder
        End If

        For Each i In Instances
            i.SetAbsolutePos(pos)
        Next
    End Sub

    Sub pnTrack_Paint(sender As Object, e As PaintEventArgs) Handles pnTrack.Paint
        Dim gx = e.Graphics
        gx.FillRectangle(Brushes.White, pnTrack.ClientRectangle)

        Dim trackHeight = pnTrack.Height - TrackBarBorder * 2 - TrackBarGap * 2

        Using borderPen As New Pen(Color.Black, TrackBarBorder)
            borderPen.Alignment = Drawing2D.PenAlignment.Inset
            gx.DrawRectangle(borderPen, 0, 0, pnTrack.Width - 1, pnTrack.Height - 1)
        End Using

        If p.Ranges.Count > 0 Then
            For x = 0 To p.Ranges.Count - 1
                Dim col As Color

                If (x Mod 2) = 0 Then
                    col = Color.Green
                Else
                    col = Color.LimeGreen
                End If

                Using rangePen As New Pen(col, trackHeight)
                    gx.DrawLine(rangePen, GetDrawPos(p.Ranges(x).Start) - CInt(TrackBarPosition / 2),
                        pnTrack.Height \ 2, GetDrawPos(p.Ranges(x).End) + CInt(TrackBarPosition / 2),
                        pnTrack.Height \ 2)
                End Using
            Next
        End If

        Using rangeSetPen As New Pen(Color.DarkOrange, trackHeight)
            If StartRange > -1 AndAlso StartRange <= Renderer.Position Then
                gx.DrawLine(rangeSetPen, GetDrawPos(StartRange) - CInt(TrackBarPosition / 2),
                    pnTrack.Height \ 2, GetDrawPos(Renderer.Position) +
                    CInt(TrackBarPosition / 2), pnTrack.Height \ 2)
            End If
        End Using

        Using rangeSetPen As New Pen(Color.DarkOrange, trackHeight)
            If EndRange > -1 AndAlso EndRange >= Renderer.Position Then
                gx.DrawLine(rangeSetPen, GetDrawPos(EndRange) - CInt(TrackBarPosition / 2),
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

        gx.DrawLine(
            posPen,
            pos - CInt(TrackBarPosition / 2),
            pnTrack.Height \ 2,
            pos + CInt(TrackBarPosition / 2),
            pnTrack.Height \ 2)

        posPen.Dispose()
    End Sub

    Sub bnExtras_Click() Handles bnMenu.Click
        cmsMain.Show(bnMenu, New Point(1, bnMenu.Height))
    End Sub

    Sub SetPos(pos As Integer)
        Try
            Renderer.Position = pos
            Renderer.Draw()
            AfterPositionChanged()
        Catch
        End Try
    End Sub

    Sub AfterPositionChanged()
        s.LastPosition = Renderer.Position
        pnTrack.Refresh()
        Dim time = TimeSpan.FromSeconds(Renderer.Position / FrameServer.FrameRate).ToString.Shorten(12)

        If time.StartsWith("00") Then
            time = time.Substring(3)
        End If

        Text = "Preview  " & s.LastPosition & "  " + time
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

    <Command("Dialog to jump to a specific time.")>
    Sub GoToTime()
        Dim d As Date
        d = d.AddSeconds(Renderer.Position / FrameServer.FrameRate)
        Dim value = InputBox.Show("Time:", "Go To Time", d.ToString("HH:mm:ss.fff"))

        If value <> "" Then
            SetPos(CInt((TimeSpan.Parse(value).TotalMilliseconds / 1000) * FrameServer.FrameRate))
        End If
    End Sub

    <Command("Dialog to jump to a specific frame.")>
    Sub GoToFrame()
        Dim value = InputBox.Show("Frame:", "Go To Frame", Renderer.Position.ToString)
        Dim pos As Integer

        If Integer.TryParse(value, pos) Then
            SetPos(pos)
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
        form.Doc.WriteParagraph("The preview dialog allows to preview and cut the video. It's possible to open more than one instance of the preview for filter comparisons.")
        form.Doc.WriteH2("Cutting")
        form.Doc.WriteParagraph("Cutting can be achieved by selecting the sections that should be kept. Selected sections appear green in the trackbar. Cutting in muxing mode without video encoding works only with mkv muxer (using the nearest key frames).")
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
        MergeRanges()
        AfterPositionChanged()
    End Sub

    <Command("Creates a job for each selection.")>
    Sub CreateJobForEachSelection()
        If p.SourceFile = "" OrElse p.Ranges.Count = 0 Then
            Exit Sub
        End If

        If Not g.MainForm.AssistantPassed Then
            MsgError("Follow assistant message in main dialog.")
            Exit Sub
        End If

        If MsgQuestion("Save current project?") = DialogResult.OK Then
            g.MainForm.IsSaveCanceled()
        End If

        Dim ranges = p.Ranges.ToArray
        Dim targetFile = p.TargetFile

        For x = 0 To ranges.Length - 1
            p.Ranges.Clear()
            p.Ranges.Add(ranges(x))
            g.UpdateTrim(p.Script)
            g.MainForm.UpdateFilters()
            g.MainForm.tbTargetFile.Text = targetFile.DirAndBase + "_" & (x + 1) & targetFile.ExtFull
            g.MainForm.AddJob(False)
        Next

        g.MainForm.tbTargetFile.Text = targetFile
        p.Ranges = ranges.ToList
        g.UpdateTrim(p.Script)
        g.MainForm.UpdateFilters()
        g.MainForm.ShowJobsDialog()
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
        HidePreviewButtons = Not HidePreviewButtons
        ShowButtons(Not HidePreviewButtons)
        AfterPositionChanged()
    End Sub

    <Command("Shows/hides the trackbar.")>
    Sub ShowHideTrackbar()
        pnTrack.Visible = Not pnTrack.Visible
        AfterPositionChanged()
    End Sub

    <Command("Changes the size.")>
    Sub Zoom(<DispName("Factor")> factor As Single)
        SetSize(CInt(Height * factor))
    End Sub

    <Command("Shows/hides various infos.")>
    Sub ToggleInfos()
        ShowPreviewInfo = Not ShowPreviewInfo
        Renderer.ShowInfo = ShowPreviewInfo
        Renderer.Draw()
    End Sub

    <Command("Plays the script with a player.")>
    Sub ShowExternalPlayer()
        Dim script = PreviewScript.GetNewScript()
        script.Path = p.TempDir + p.TargetFile.Base + "_play." + script.FileType
        g.UpdateTrim(script)
        g.PlayScript(script)
    End Sub

    <Command("Plays the script with mpv.net.")>
    Sub PlayWithMpvnet()
        Dim script = PreviewScript.GetNewScript()
        script.Path = p.TempDir + p.TargetFile.Base + "_play." + script.FileType
        g.UpdateTrim(script)
        g.PlayScriptWithMPV(script, "--start=" + GetPlayPosition.ToString)
    End Sub

    <Command("Plays the script with MPC.")>
    Sub PlayWithMPC()
        Dim script = PreviewScript.GetNewScript()
        script.Path = p.TempDir + p.TargetFile.Base + "_play." + script.FileType
        g.UpdateTrim(script)
        g.PlayScriptWithMPC(script, "/start " & GetPlayPosition.TotalMilliseconds)
    End Sub

    <Command("Closes the dialog.")>
    Sub CloseDialog()
        Close()
    End Sub

    <Command("Jumps to the previous cut point.")>
    Sub JumpToThePreviousRangePos()
        Dim list As New List(Of Object)

        For Each range As Range In p.Ranges
            list.Add(range.Start)
            list.Add(range.End)
        Next

        list.Add(Renderer.Position - 1)

        list.Sort()

        Dim index = list.IndexOf(Renderer.Position - 1) - 1

        If index >= 0 AndAlso index < list.Count Then
            SetPos(CInt(list(index)))
        End If
    End Sub

    <Command("Copies the time of the current position.")>
    Sub CopyTime()
        Clipboard.SetText(Date.Today.AddSeconds(Renderer.Position / FrameServer.FrameRate).ToString("HH:mm:ss.fff"))
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
            SetPos(CInt(list(index)))
        End If
    End Sub

    <Command("Saves the current frame as bitmap.")>
    Sub SaveBitmap()
        Using dialog As New SaveFileDialog
            dialog.SetFilter({"bmp"})
            dialog.FileName = p.TargetFile.Base + " - " & Renderer.Position

            If dialog.ShowDialog = DialogResult.OK Then
                BitmapUtil.CreateBitmap(FrameServer, Renderer.Position).Save(dialog.FileName, ImageFormat.Bmp)
            End If
        End Using
    End Sub

    <Command("Saves the current frame as bitmap.")>
    Sub SavePng()
        Using dialog As New SaveFileDialog
            dialog.SetFilter({"png"})
            dialog.FileName = p.TargetFile.Base + " - " & Renderer.Position

            If dialog.ShowDialog = DialogResult.OK Then
                BitmapUtil.CreateBitmap(FrameServer, Renderer.Position).Save(dialog.FileName, ImageFormat.Png)
            End If
        End Using
    End Sub

    <Command("Saves the current frame as JPG to the given path which can contain macros.")>
    Sub SaveJpgByPath(
        <DispName("File Path")>
        <Description("File path which can contain macros.")>
        path As String)

        path = Macro.Expand(path)
        Dim result = InputBox.Show("Enter the compression quality.", "Compression Quality", s.Storage.GetInt("preview compression quality", 95).ToString)

        If result.IsInt Then
            s.Storage.SetInt("preview compression quality", result.ToInt)
            Dim params = New EncoderParameters(1)
            params.Param(0) = New EncoderParameter(Encoder.Quality, result.ToInt)
            Dim info = ImageCodecInfo.GetImageEncoders.Where(Function(arg) arg.FormatID = ImageFormat.Jpeg.Guid).First
            BitmapUtil.CreateBitmap(FrameServer, Renderer.Position).Save(path, info, params)
        End If
    End Sub

    <Command("Saves the current frame as JPG.")>
    Sub SaveJPG()
        Using dialog As New SaveFileDialog
            dialog.DefaultExt = "jpg"
            dialog.FileName = p.TargetFile.Base + " - " & Renderer.Position

            If dialog.ShowDialog = DialogResult.OK Then
                SaveJpgByPath(dialog.FileName)
            End If
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

            For Each line In File.ReadAllLines(fp)
                Dim left = line.Left("=")

                If left.Length = 9 Then
                    td.AddCommand(left.Substring(7) + "   " + line.Right("="), line.Right("="))
                End If
            Next

            If td.Show() <> "" Then
                SetPos(CInt((TimeSpan.Parse(td.SelectedValue).TotalMilliseconds / 1000) * FrameServer.FrameRate))
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
        ret.Add("Cut|Split Selection", NameOf(SplitRange), Keys.S)
        ret.Add("Cut|-")
        ret.Add("Cut|Delete Selection", NameOf(DeleteRange), Keys.Delete, Symbol.Delete)
        ret.Add("Cut|Delete All Selections", NameOf(ClearAllRanges), Keys.Control Or Keys.Delete)
        ret.Add("Cut|-")
        ret.Add("Cut|Create job for each selection", NameOf(CreateJobForEachSelection))

        ret.Add("View|Info", NameOf(ToggleInfos), Keys.I, Symbol.Info)
        ret.Add("View|Fullscreen", NameOf(SwitchWindowState), Keys.Enter, Symbol.FullScreen)
        ret.Add("View|-")
        ret.Add("View|Zoom In", NameOf(Zoom), Keys.Oemplus, Symbol.ZoomIn, {1.1F})
        ret.Add("View|Zoom Out", NameOf(Zoom), Keys.OemMinus, Symbol.ZoomOut, {0.9F})
        ret.Add("View|-")
        ret.Add("View|Buttons", NameOf(ShowHideButtons), Keys.B)
        ret.Add("View|Trackbar", NameOf(ShowHideTrackbar), Keys.Control Or Keys.T)

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

    Sub MergeRanges()
        For Each r1 In p.Ranges.ToArray
            For Each r2 In p.Ranges.ToArray
                If Not r1 Is r2 Then
                    If (r2.Start >= r1.Start AndAlso r2.Start <= r1.End) OrElse
                        (r2.End >= r1.Start AndAlso r2.End <= r1.End) Then

                        p.Ranges.Remove(r1)
                        p.Ranges.Remove(r2)
                        p.Ranges.Add(New Range(Math.Min(r1.Start, r2.Start), Math.Max(r1.End, r2.End)))
                        MergeRanges()
                    End If
                End If
            Next
        Next

        p.Ranges.Sort()
    End Sub

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

    Sub GenericMenu_Command(e As CustomMenuItemEventArgs) Handles GenericMenu.Command
        e.Handled = True

        Select Case e.Item.MethodName
            Case NameOf(CloseDialog), NameOf(PlayWithMPC), NameOf(PlayWithMpvnet)
                ProcessMenu(e.Item)
            Case Else
                For Each i In Instances
                    i.ProcessMenu(e.Item)
                Next
        End Select
    End Sub

    Sub ProcessMenu(item As CustomMenuItem)
        GenericMenu.Process(item)
    End Sub

    Sub Controls_Enter() Handles bnLeft3.Enter, bnLeft2.Enter, bnLeft1.Enter, bnRight1.Enter, bnRight2.Enter,
        bnRight3.Enter, bnEndCutRange.Enter, bnStartCutRange.Enter, bnDelete.Enter, bnMenu.Enter

        ActiveControl = Nothing
    End Sub

    Function GetCurrentRange() As Range
        For Each range In p.Ranges
            If Renderer.Position >= range.Start AndAlso Renderer.Position <= range.End Then
                Return range
            End If
        Next
    End Function

    Function GetPlayPosition() As TimeSpan
        If p.Ranges.Count = 0 Then
            Dim pos = Renderer.Position

            If pos = FrameServer.Info.FrameCount - 1 Then
                pos -= 2
            End If

            Return TimeSpan.FromSeconds(pos / FrameServer.FrameRate)
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
                        Return TimeSpan.FromSeconds((frames + (pos - current.Start)) / FrameServer.FrameRate)
                    End If
                Next
            End If
        End If
    End Function

    Protected Overrides Sub OnHelpButtonClicked(e As CancelEventArgs)
        e.Cancel = True
        OpenHelp()
    End Sub

    Protected Overrides Sub OnLoad(args As EventArgs)
        MyBase.OnLoad(args)

        PreviewScript.Synchronize(True, True, True)
        FrameServer = FrameServerFactory.Create(PreviewScript.Path)
        Renderer = New VideoRenderer(pnVideo, FrameServer)
        Renderer.Info = PreviewScript.OriginalInfo
        Renderer.ShowInfo = s.ShowPreviewInfo

        If s.LastPosition < FrameServer.Info.FrameCount - 1 Then
            Renderer.Position = s.LastPosition
        End If

        Dim info = FrameServer.Info

        If Calc.IsARSignalingRequired Then
            VideoSize = New Size(CInt(info.Height * Calc.GetTargetDAR), CInt(info.Height))
        Else
            VideoSize = New Size(CInt(info.Width), CInt(info.Height))
        End If

        p.CutFrameCount = info.FrameCount
        p.CutFrameRate = FrameServer.FrameRate

        Dim workingArea = Screen.FromControl(Me).WorkingArea
        Dim initHeight = CInt((workingArea.Height / 100) * s.PreviewSize)

        SetSize(initHeight)

        If s.PreviewFormBorderStyle = FormBorderStyle.None Then
            Fullscreen()
        Else
            NormalScreen()
        End If

        AfterPositionChanged()
        ShowButtons(Not s.HidePreviewButtons)
    End Sub

    Sub SetSize(newHeight As Integer)
        Dim workingArea = Screen.FromControl(Me).WorkingArea
        Dim bordersHeight = Height - ClientSize.Height
        Dim clientHeight = newHeight - bordersHeight

        If clientHeight > workingArea.Height * 0.9 Then
            clientHeight = CInt(workingArea.Height * 0.9)
        End If

        Dim clientWidth = CInt((clientHeight / VideoSize.Height) * VideoSize.Width)

        If clientWidth > workingArea.Width * 0.9 Then
            clientWidth = CInt(workingArea.Width * 0.9)
            clientHeight = CInt((clientWidth / VideoSize.Width) * VideoSize.Height)
        End If

        Width = clientWidth + (Width - ClientSize.Width)
        Height = clientHeight + bordersHeight
    End Sub

    Protected Overrides Sub OnMouseWheel(e As MouseEventArgs)
        MyBase.OnMouseWheel(e)

        Dim pos = 1

        If Control.ModifierKeys = Keys.Control Then pos = 10
        If Control.ModifierKeys = Keys.Shift Then pos = 100
        If Control.ModifierKeys = Keys.Alt Then pos = 1000

        If e.Delta < 0 Then
            pos = pos * -1
        End If

        If s.ReverseVideoScrollDirection Then
            pos = pos * -1
        End If

        SetRelativePos(pos)
    End Sub

    Protected Overrides Sub OnMouseClick(e As MouseEventArgs)
        MyBase.OnMouseClick(e)

        If Width - e.Location.X < 10 AndAlso e.Location.Y < 10 Then
            Close()
        End If
    End Sub

    Protected Overrides Sub OnFormClosing(args As FormClosingEventArgs)
        MyBase.OnFormClosing(args)
        Instances.Remove(Me)
        GenericMenu.RemoveKeyDownHandler(Me)
        g.UpdateTrim(p.Script)
        s.ShowPreviewInfo = ShowPreviewInfo
        s.HidePreviewButtons = HidePreviewButtons
        s.LastPosition = Renderer.Position
        g.MainForm.UpdateFilters()
        Renderer.Dispose()
        FrameServer.Dispose()
    End Sub

    Protected Overrides Sub WndProc(ByRef m As Message)
        If m.Msg = &H214 Then 'WM_SIZING
            Dim rc = Marshal.PtrToStructure(Of Native.RECT)(m.LParam)
            Dim r = rc
            SubtractWindowBorders(Handle, r)
            Dim c_w = r.Right - r.Left, c_h = r.Bottom - r.Top
            Dim aspect = VideoSize.Width / VideoSize.Height
            Dim d_w = CInt(c_h * aspect - c_w)
            Dim d_h = CInt(c_w / aspect - c_h)
            Dim d_w2 = CInt(c_h * aspect - c_w)
            Dim d_h2 = CInt(c_w / aspect - c_h)
            Dim d_corners = {d_w, d_h, -d_w, -d_h}
            Dim corners = {rc.Left, rc.Top, rc.Right, rc.Bottom}
            Dim corner = GetResizeBorder(m.WParam.ToInt32())

            If corner >= 0 Then
                corners(corner) -= d_corners(corner)
            End If

            Marshal.StructureToPtr(Of Native.RECT)(New Native.RECT(corners(0), corners(1), corners(2), corners(3)), m.LParam, False)
            m.Result = New IntPtr(1)
            Exit Sub
        End If

        MyBase.WndProc(m)
    End Sub

    Shared Function GetResizeBorder(v As Integer) As Integer
        Select Case v
            Case 1
                Return 3
            Case 3
                Return 2
            Case 2
                Return 3
            Case 6
                Return 2
            Case 4
                Return 1
            Case 5
                Return 1
            Case 7
                Return 3
            Case 8
                Return 3
            Case Else
                Return -1
        End Select
    End Function

    Shared Sub SubtractWindowBorders(hwnd As IntPtr, ByRef rect As Native.RECT)
        Dim rect2 = New Native.RECT(0, 0, 0, 0)
        AddWindowBorders(hwnd, rect2)
        rect.Left -= rect2.Left
        rect.Top -= rect2.Top
        rect.Right -= rect2.Right
        rect.Bottom -= rect2.Bottom
    End Sub

    Shared Sub AddWindowBorders(hwnd As IntPtr, ByRef rc As Native.RECT)
        AdjustWindowRect(rc, CUInt(GetWindowLongPtr(hwnd, -16)), False)
    End Sub

    <DllImport("user32.dll")>
    Shared Function AdjustWindowRect(ByRef lpRect As Native.RECT, dwStyle As UInteger, bMenu As Boolean) As Boolean
    End Function

    <DllImport("user32.dll")>
    Shared Function GetWindowLongPtr(hWnd As IntPtr, nIndex As Integer) As IntPtr
    End Function

    Sub pnVideo_MouseDown(sender As Object, e As MouseEventArgs) Handles pnVideo.MouseDown
        Dim sb = Screen.FromControl(Me).Bounds
        Dim p1 = New Point(sb.Width, 0)
        Dim p2 = PointToScreen(e.Location)

        If Math.Abs(p1.X - p2.X) <10 AndAlso Math.Abs(p1.Y - p2.Y) <10 Then
            Close()
        End If
    End Sub

    Sub pnVideo_MouseClick(sender As Object, e As MouseEventArgs) Handles pnVideo.MouseClick
        If pnVideo.Width - e.Location.X < 10 AndAlso e.Location.Y < 10 Then
            Close()
        End If
    End Sub

    Sub pnVideo_DoubleClick() Handles pnVideo.DoubleClick
        SwitchWindowState()
    End Sub

    Sub pnVideo_MouseMove(sender As Object, e As MouseEventArgs) Handles pnVideo.MouseMove
        If Not WindowState = FormWindowState.Maximized AndAlso e.Button = MouseButtons.Left Then
            Native.ReleaseCapture()
            Native.PostMessage(Handle, &HA1, New IntPtr(2), IntPtr.Zero) 'WM_NCLBUTTONDOWN, HTCAPTION
        End If
    End Sub

    Private BlockVideoPaint As Boolean

    Sub pnVideo_Paint(sender As Object, e As PaintEventArgs) Handles pnVideo.Paint
        If Not BlockVideoPaint Then
            RefreshControls()
            Renderer.Draw()
        End If
    End Sub
End Class
