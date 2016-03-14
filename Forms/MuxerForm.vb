Imports StaxRip.UI

Public Class MuxerForm
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

    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents bnCommandLinePreview As System.Windows.Forms.Button
    Friend WithEvents CmdlControl As StaxRip.CommandLineControl
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents tc As System.Windows.Forms.TabControl
    Friend WithEvents tpCommandLine As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tpOptions As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents SimpleUI As StaxRip.SimpleUI
    Friend WithEvents tpSubtitles As TabPage
    Friend WithEvents SubtitleControl As SubtitleControl
    Friend WithEvents tpAudio As TabPage
    Friend WithEvents bnPlay As ButtonEx
    Friend WithEvents bnDown As ButtonEx
    Friend WithEvents bnUp As ButtonEx
    Friend WithEvents bnRemove As ButtonEx
    Friend WithEvents bnAddAudio As ButtonEx
    Friend WithEvents dgvAudio As DataGridViewEx
    Friend WithEvents bnEdit As ButtonEx
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.CmdlControl = New StaxRip.CommandLineControl()
        Me.bnCommandLinePreview = New System.Windows.Forms.Button()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.tc = New System.Windows.Forms.TabControl()
        Me.tpSubtitles = New System.Windows.Forms.TabPage()
        Me.SubtitleControl = New StaxRip.SubtitleControl()
        Me.tpAudio = New System.Windows.Forms.TabPage()
        Me.bnPlay = New StaxRip.UI.ButtonEx()
        Me.bnDown = New StaxRip.UI.ButtonEx()
        Me.bnUp = New StaxRip.UI.ButtonEx()
        Me.bnRemove = New StaxRip.UI.ButtonEx()
        Me.bnAddAudio = New StaxRip.UI.ButtonEx()
        Me.dgvAudio = New StaxRip.UI.DataGridViewEx()
        Me.tpOptions = New System.Windows.Forms.TabPage()
        Me.SimpleUI = New StaxRip.SimpleUI()
        Me.tpCommandLine = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.bnEdit = New StaxRip.UI.ButtonEx()
        Me.tc.SuspendLayout()
        Me.tpSubtitles.SuspendLayout()
        Me.tpAudio.SuspendLayout()
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpOptions.SuspendLayout()
        Me.tpCommandLine.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'CmdlControl
        '
        Me.CmdlControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CmdlControl.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdlControl.Location = New System.Drawing.Point(3, 28)
        Me.CmdlControl.Name = "CmdlControl"
        Me.CmdlControl.Size = New System.Drawing.Size(1174, 372)
        Me.CmdlControl.TabIndex = 0
        '
        'buCmdlPreview
        '
        Me.bnCommandLinePreview.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCommandLinePreview.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.bnCommandLinePreview.Location = New System.Drawing.Point(767, 470)
        Me.bnCommandLinePreview.Name = "buCmdlPreview"
        Me.bnCommandLinePreview.Size = New System.Drawing.Size(175, 34)
        Me.bnCommandLinePreview.TabIndex = 4
        Me.bnCommandLinePreview.Text = "Command Line..."
        Me.bnCommandLinePreview.UseVisualStyleBackColor = True
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(1116, 470)
        Me.bnCancel.Size = New System.Drawing.Size(100, 34)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(1010, 470)
        Me.bnOK.Size = New System.Drawing.Size(100, 34)
        Me.bnOK.Text = "OK"
        '
        'tc
        '
        Me.tc.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tc.Controls.Add(Me.tpSubtitles)
        Me.tc.Controls.Add(Me.tpAudio)
        Me.tc.Controls.Add(Me.tpOptions)
        Me.tc.Controls.Add(Me.tpCommandLine)
        Me.tc.Location = New System.Drawing.Point(12, 12)
        Me.tc.Name = "tc"
        Me.tc.SelectedIndex = 0
        Me.tc.Size = New System.Drawing.Size(1204, 452)
        Me.tc.TabIndex = 5
        '
        'tpSubtitles
        '
        Me.tpSubtitles.Controls.Add(Me.SubtitleControl)
        Me.tpSubtitles.Location = New System.Drawing.Point(4, 34)
        Me.tpSubtitles.Name = "tpSubtitles"
        Me.tpSubtitles.Padding = New System.Windows.Forms.Padding(3)
        Me.tpSubtitles.Size = New System.Drawing.Size(1196, 414)
        Me.tpSubtitles.TabIndex = 3
        Me.tpSubtitles.Text = "Subtitles"
        Me.tpSubtitles.UseVisualStyleBackColor = True
        '
        'SubtitleControl
        '
        Me.SubtitleControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SubtitleControl.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SubtitleControl.Location = New System.Drawing.Point(3, 3)
        Me.SubtitleControl.Name = "SubtitleControl"
        Me.SubtitleControl.Size = New System.Drawing.Size(1190, 408)
        Me.SubtitleControl.TabIndex = 0
        '
        'tpAudio
        '
        Me.tpAudio.Controls.Add(Me.bnEdit)
        Me.tpAudio.Controls.Add(Me.bnPlay)
        Me.tpAudio.Controls.Add(Me.bnDown)
        Me.tpAudio.Controls.Add(Me.bnUp)
        Me.tpAudio.Controls.Add(Me.bnRemove)
        Me.tpAudio.Controls.Add(Me.bnAddAudio)
        Me.tpAudio.Controls.Add(Me.dgvAudio)
        Me.tpAudio.Location = New System.Drawing.Point(4, 34)
        Me.tpAudio.Name = "tpAudio"
        Me.tpAudio.Padding = New System.Windows.Forms.Padding(3)
        Me.tpAudio.Size = New System.Drawing.Size(1196, 414)
        Me.tpAudio.TabIndex = 4
        Me.tpAudio.Text = "Audio"
        Me.tpAudio.UseVisualStyleBackColor = True
        '
        'bnPlay
        '
        Me.bnPlay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnPlay.Location = New System.Drawing.Point(1090, 174)
        Me.bnPlay.Size = New System.Drawing.Size(100, 36)
        Me.bnPlay.Text = "Play"
        '
        'bnDown
        '
        Me.bnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnDown.Location = New System.Drawing.Point(1090, 132)
        Me.bnDown.Size = New System.Drawing.Size(100, 36)
        Me.bnDown.Text = "Down"
        '
        'bnUp
        '
        Me.bnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnUp.Location = New System.Drawing.Point(1090, 90)
        Me.bnUp.Size = New System.Drawing.Size(100, 36)
        Me.bnUp.Text = "Up"
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRemove.Location = New System.Drawing.Point(1090, 48)
        Me.bnRemove.Size = New System.Drawing.Size(100, 36)
        Me.bnRemove.Text = "Remove"
        '
        'bnAdd
        '
        Me.bnAddAudio.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAddAudio.Location = New System.Drawing.Point(1090, 6)
        Me.bnAddAudio.Size = New System.Drawing.Size(100, 36)
        Me.bnAddAudio.Text = "Add..."
        '
        'dgvAudio
        '
        Me.dgvAudio.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvAudio.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAudio.Location = New System.Drawing.Point(6, 6)
        Me.dgvAudio.Name = "dgvAudio"
        Me.dgvAudio.RowTemplate.Height = 28
        Me.dgvAudio.Size = New System.Drawing.Size(1078, 387)
        Me.dgvAudio.TabIndex = 0
        '
        'tpOptions
        '
        Me.tpOptions.Controls.Add(Me.SimpleUI)
        Me.tpOptions.Location = New System.Drawing.Point(4, 29)
        Me.tpOptions.Name = "tpOptions"
        Me.tpOptions.Padding = New System.Windows.Forms.Padding(3)
        Me.tpOptions.Size = New System.Drawing.Size(1196, 419)
        Me.tpOptions.TabIndex = 2
        Me.tpOptions.Text = "Options"
        Me.tpOptions.UseVisualStyleBackColor = True
        '
        'SimpleUI
        '
        Me.SimpleUI.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SimpleUI.Location = New System.Drawing.Point(3, 3)
        Me.SimpleUI.Name = "SimpleUI"
        Me.SimpleUI.Size = New System.Drawing.Size(1190, 413)
        Me.SimpleUI.TabIndex = 0
        Me.SimpleUI.Text = "SimpleUI1"
        '
        'tpCommandLine
        '
        Me.tpCommandLine.Controls.Add(Me.TableLayoutPanel1)
        Me.tpCommandLine.Location = New System.Drawing.Point(4, 29)
        Me.tpCommandLine.Name = "tpCommandLine"
        Me.tpCommandLine.Padding = New System.Windows.Forms.Padding(8)
        Me.tpCommandLine.Size = New System.Drawing.Size(1196, 419)
        Me.tpCommandLine.TabIndex = 1
        Me.tpCommandLine.Text = "Command Line"
        Me.tpCommandLine.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.CmdlControl, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(8, 8)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1180, 403)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(3, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(233, 25)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Additional custom switches:"
        '
        'bnEdit
        '
        Me.bnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnEdit.Location = New System.Drawing.Point(1090, 216)
        Me.bnEdit.Size = New System.Drawing.Size(100, 36)
        Me.bnEdit.Text = "Edit"
        '
        'MuxerForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1228, 516)
        Me.Controls.Add(Me.tc)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.bnOK)
        Me.Controls.Add(Me.bnCommandLinePreview)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Name = "MuxerForm"
        Me.Text = "Container"
        Me.tc.ResumeLayout(False)
        Me.tpSubtitles.ResumeLayout(False)
        Me.tpAudio.ResumeLayout(False)
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpOptions.ResumeLayout(False)
        Me.tpCommandLine.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Muxer As Muxer
    Private AudioBindingSource As New BindingSource

    Sub New(muxer As Muxer)
        MyBase.New()
        InitializeComponent()

        Text += " - " + muxer.Name
        Me.Muxer = muxer
        SubtitleControl.AddSubtitles(muxer.Subtitles)
        CmdlControl.tb.Text = muxer.AdditionalSwitches
        tc.SelectedIndex = s.Storage.GetInt("last selected muxer tab")

        dgvAudio.MultiSelect = False
        dgvAudio.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvAudio.AllowUserToResizeRows = False
        dgvAudio.RowHeadersVisible = False
        dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvAudio.AutoGenerateColumns = False
        AudioBindingSource.DataSource = ObjectHelp.GetCopy(p.AudioTracks)
        dgvAudio.DataSource = AudioBindingSource

        Dim profileName = dgvAudio.AddTextBoxColumn()
        profileName.DataPropertyName = "Name"
        profileName.HeaderText = "Profile"
        profileName.ReadOnly = True

        Dim pathColumn = dgvAudio.AddTextBoxColumn()
        pathColumn.DataPropertyName = "DisplayName"
        pathColumn.HeaderText = "Track"
        pathColumn.ReadOnly = True

        If dgvAudio.RowCount > 0 Then dgvAudio.Rows(0).Selected = True

        UpdateAudioControls()

        Dim UI = SimpleUI
        UI.BackColor = Color.Transparent

        Dim page = UI.CreateFlowPage("main page")

        If Not TypeOf muxer Is WebMMuxer Then
            UI.AddLabel(page, "Chapters:")

            Dim tbb = UI.AddTextButtonBlock(page)
            tbb.Label.Visible = False
            tbb.Expand(tbb.Edit)
            tbb.Edit.Text = muxer.ChapterFile
            tbb.Edit.SaveAction = Sub(value) muxer.ChapterFile = If(value <> "", value, Nothing)
            tbb.Button.Text = "..."
            tbb.Button.ClickAction = Sub() tbb.Edit.BrowseFile("txt, xml|*.txt;*.xml")
        End If

        If TypeOf muxer Is MkvMuxer Then
            CmdlControl.Presets = s.CmdlPresetsMKV

            Dim offset = 9

            Dim tb = UI.AddTextBlock(page)
            tb.Label.Text = "Title:"
            tb.Label.Tooltip = "Optional title of the output file that may contain macros."
            tb.Label.Offset = offset
            tb.Expand(tb.Edit)
            tb.Edit.Text = DirectCast(muxer, MkvMuxer).Title
            tb.Edit.SaveAction = Sub(value) DirectCast(muxer, MkvMuxer).Title = value

            tb = UI.AddTextBlock(page)
            tb.Label.Text = "Video Stream Name:"
            tb.Label.Tooltip = "Optional name of the video stream that may contain macro."
            tb.Label.Offset = offset
            tb.Expand(tb.Edit)
            tb.Edit.Text = DirectCast(muxer, MkvMuxer).VideoTrackName
            tb.Edit.SaveAction = Sub(value) DirectCast(muxer, MkvMuxer).VideoTrackName = value

            Dim mb = UI.AddMenuButtonBlock(Of Language)(page)
            mb.Label.Text = "Video Stream Language:"
            mb.Label.Tooltip = "Optional language of the video stream."
            mb.Label.Offset = offset
            mb.MenuButton.Value = DirectCast(muxer, MkvMuxer).VideoTrackLanguage
            mb.MenuButton.SaveAction = Sub(value) DirectCast(muxer, MkvMuxer).VideoTrackLanguage = value

            For Each i In Language.Languages
                If i.IsCommon Then
                    mb.MenuButton.Add(i.ToString, i)
                Else
                    mb.MenuButton.Add("More | " + i.ToString.Substring(0, 1) + " | " + i.ToString, i)
                End If
            Next
        ElseIf TypeOf muxer Is MP4Muxer Then
            CmdlControl.Presets = s.CmdlPresetsMP4
        End If

        TipProvider.SetTip("Additional command line switches that may contain macros.", tpCommandLine)
    End Sub

    Private Sub MuxerForm_FormClosed() Handles Me.FormClosed
        If TypeOf Muxer Is MkvMuxer Then
            s.CmdlPresetsMKV = CmdlControl.Presets.ReplaceUnicode
        ElseIf TypeOf Muxer Is MP4Muxer Then
            s.CmdlPresetsMP4 = CmdlControl.Presets.ReplaceUnicode
        End If

        s.Storage.SetInt("last selected muxer tab", tc.SelectedIndex)
        SetValues()

        If DialogResult = DialogResult.OK Then
            p.AudioTracks = DirectCast(AudioBindingSource.DataSource, List(Of AudioProfile))
        End If
    End Sub

    Private Sub SetValues()
        SubtitleControl.SetValues(Muxer)
        SimpleUI.Save()
        Muxer.AdditionalSwitches = CmdlControl.tb.Text.ReplaceUnicode
    End Sub

    Private Sub MuxerForm_HelpRequested() Handles Me.HelpRequested
        Dim f As New HelpForm()
        f.Doc.WriteStart(Text)
        f.Doc.WriteP(Strings.Muxer)
        f.Doc.WriteTips(TipProvider.GetTips, SimpleUI.ActivePage.TipProvider.GetTips)
        f.Doc.WriteTable("Macros", Strings.MacrosHelp, Macro.GetTips())
        f.Show()
    End Sub

    Private Sub buCmdlPreview_Click() Handles bnCommandLinePreview.Click
        SetValues()
        g.ShowCommandLinePreview(Muxer.GetCommandLine)
    End Sub

    Private Sub bnAddAudio_Click(sender As Object, e As EventArgs) Handles bnAddAudio.Click
        Using d As New OpenFileDialog
            d.SetFilter(FileTypes.Audio)
            If Directory.Exists(p.TempDir) Then d.SetInitDir(p.TempDir)

            If d.ShowDialog = DialogResult.OK Then
                For Each i In d.FileNames
                    Dim sb As New SelectionBox(Of AudioProfile)
                    sb.Title = "Audio Profile"
                    sb.Text = "Please select a audio profile."

                    For Each i2 In s.AudioProfiles
                        sb.AddItem(ObjectHelp.GetCopy(Of AudioProfile)(i2))
                    Next

                    If sb.Show = DialogResult.OK Then
                        Dim ap = sb.SelectedItem
                        ap.File = i

                        If Not p.Script.GetFilter("Source").Script.Contains("DirectShowSource") Then
                            ap.Delay = g.ExtractDelay(ap.File)
                        End If

                        If FileTypes.AudioVideo.Contains(ap.File.Ext) Then
                            ap.Streams = MediaInfo.GetAudioStreams(ap.File)
                        End If

                        ap.SetStreamOrLanguage()

                        If Not ap.Stream Is Nothing Then
                            Dim sb2 As New SelectionBox(Of AudioStream)
                            sb2.Title = "Stream Selection"
                            sb2.Text = "Please select a audio stream."

                            For Each i2 In ap.Streams
                                sb2.AddItem(i2)
                            Next

                            If sb2.Show = DialogResult.Cancel Then Return
                            ap.Stream = sb2.SelectedItem
                        End If

                        g.MainForm.UpdateAudio(ap)

                        AudioBindingSource.Add(sb.SelectedItem)
                        AudioBindingSource.Position = AudioBindingSource.Count - 1
                        UpdateAudioControls()
                    End If
                Next
            End If
        End Using
    End Sub

    Private Sub bnRemove_Click(sender As Object, e As EventArgs) Handles bnRemove.Click
        dgvAudio.RemoveSelection
        UpdateAudioControls()
    End Sub

    Private Sub bnUp_Click(sender As Object, e As EventArgs) Handles bnUp.Click
        dgvAudio.MoveSelectionUp
        UpdateAudioControls()
    End Sub

    Private Sub bnDown_Click(sender As Object, e As EventArgs) Handles bnDown.Click
        dgvAudio.MoveSelectionDown
        UpdateAudioControls()
    End Sub

    Private Sub bnPlay_Click(sender As Object, e As EventArgs) Handles bnPlay.Click
        g.Play(DirectCast(AudioBindingSource(dgvAudio.SelectedRows(0).Index), AudioProfile).File)
    End Sub

    Private Sub bnEdit_Click(sender As Object, e As EventArgs) Handles bnEdit.Click
        Dim ap = DirectCast(AudioBindingSource(dgvAudio.SelectedRows(0).Index), AudioProfile)
        ap.EditProject()
        g.MainForm.UpdateAudioMenu()
        g.MainForm.tbSize_TextChanged()
        AudioBindingSource.ResetBindings(False)
    End Sub

    Sub UpdateAudioControls()
        bnRemove.Enabled = dgvAudio.SelectedRows.Count > 0
        bnPlay.Enabled = dgvAudio.SelectedRows.Count > 0
        bnEdit.Enabled = dgvAudio.SelectedRows.Count > 0
        bnUp.Enabled = dgvAudio.CanMoveUp
        bnDown.Enabled = dgvAudio.CanMoveDown
    End Sub

    Private Sub dgvAudio_MouseUp(sender As Object, e As MouseEventArgs) Handles dgvAudio.MouseUp
        UpdateAudioControls()
    End Sub
End Class