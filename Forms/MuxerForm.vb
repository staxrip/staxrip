Imports System.Threading.Tasks
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
    Friend WithEvents tcMain As System.Windows.Forms.TabControl
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
    Friend WithEvents bnAdd As ButtonEx
    Friend WithEvents dgvAudio As DataGridViewEx
    Friend WithEvents bnEdit As ButtonEx
    Friend WithEvents TableLayoutPanel2 As TableLayoutPanel
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents pnTab As Panel
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.CmdlControl = New StaxRip.CommandLineControl()
        Me.bnCommandLinePreview = New System.Windows.Forms.Button()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.tcMain = New System.Windows.Forms.TabControl()
        Me.tpSubtitles = New System.Windows.Forms.TabPage()
        Me.SubtitleControl = New StaxRip.SubtitleControl()
        Me.tpAudio = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel2 = New System.Windows.Forms.TableLayoutPanel()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnAdd = New StaxRip.UI.ButtonEx()
        Me.bnRemove = New StaxRip.UI.ButtonEx()
        Me.bnUp = New StaxRip.UI.ButtonEx()
        Me.bnDown = New StaxRip.UI.ButtonEx()
        Me.bnPlay = New StaxRip.UI.ButtonEx()
        Me.bnEdit = New StaxRip.UI.ButtonEx()
        Me.dgvAudio = New StaxRip.UI.DataGridViewEx()
        Me.tpOptions = New System.Windows.Forms.TabPage()
        Me.SimpleUI = New StaxRip.SimpleUI()
        Me.tpCommandLine = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.pnTab = New System.Windows.Forms.Panel()
        Me.tcMain.SuspendLayout()
        Me.tpSubtitles.SuspendLayout()
        Me.tpAudio.SuspendLayout()
        Me.TableLayoutPanel2.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tpOptions.SuspendLayout()
        Me.tpCommandLine.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.pnTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'CmdlControl
        '
        Me.CmdlControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.CmdlControl.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CmdlControl.Location = New System.Drawing.Point(0, 48)
        Me.CmdlControl.Margin = New System.Windows.Forms.Padding(0)
        Me.CmdlControl.Name = "CmdlControl"
        Me.CmdlControl.Size = New System.Drawing.Size(1870, 934)
        Me.CmdlControl.TabIndex = 0
        '
        'bnCommandLinePreview
        '
        Me.bnCommandLinePreview.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnCommandLinePreview.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.bnCommandLinePreview.Location = New System.Drawing.Point(365, 766)
        Me.bnCommandLinePreview.Margin = New System.Windows.Forms.Padding(8)
        Me.bnCommandLinePreview.Name = "bnCommandLinePreview"
        Me.bnCommandLinePreview.Size = New System.Drawing.Size(320, 70)
        Me.bnCommandLinePreview.TabIndex = 4
        Me.bnCommandLinePreview.Text = "Command Line..."
        Me.bnCommandLinePreview.UseVisualStyleBackColor = True
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(967, 766)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(8)
        Me.bnCancel.Size = New System.Drawing.Size(250, 70)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(701, 766)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(8)
        Me.bnOK.Size = New System.Drawing.Size(250, 70)
        Me.bnOK.Text = "OK"
        '
        'tcMain
        '
        Me.tcMain.Controls.Add(Me.tpSubtitles)
        Me.tcMain.Controls.Add(Me.tpAudio)
        Me.tcMain.Controls.Add(Me.tpOptions)
        Me.tcMain.Controls.Add(Me.tpCommandLine)
        Me.tcMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tcMain.Location = New System.Drawing.Point(0, 0)
        Me.tcMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tcMain.Name = "tcMain"
        Me.tcMain.SelectedIndex = 0
        Me.tcMain.Size = New System.Drawing.Size(1201, 734)
        Me.tcMain.TabIndex = 5
        '
        'tpSubtitles
        '
        Me.tpSubtitles.Controls.Add(Me.SubtitleControl)
        Me.tpSubtitles.Location = New System.Drawing.Point(12, 69)
        Me.tpSubtitles.Margin = New System.Windows.Forms.Padding(5)
        Me.tpSubtitles.Name = "tpSubtitles"
        Me.tpSubtitles.Padding = New System.Windows.Forms.Padding(5)
        Me.tpSubtitles.Size = New System.Drawing.Size(1177, 653)
        Me.tpSubtitles.TabIndex = 3
        Me.tpSubtitles.Text = " Subtitles "
        Me.tpSubtitles.UseVisualStyleBackColor = True
        '
        'SubtitleControl
        '
        Me.SubtitleControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SubtitleControl.Location = New System.Drawing.Point(5, 5)
        Me.SubtitleControl.Margin = New System.Windows.Forms.Padding(2, 3, 2, 3)
        Me.SubtitleControl.Name = "SubtitleControl"
        Me.SubtitleControl.Size = New System.Drawing.Size(1167, 643)
        Me.SubtitleControl.TabIndex = 0
        '
        'tpAudio
        '
        Me.tpAudio.Controls.Add(Me.TableLayoutPanel2)
        Me.tpAudio.Location = New System.Drawing.Point(12, 69)
        Me.tpAudio.Margin = New System.Windows.Forms.Padding(5)
        Me.tpAudio.Name = "tpAudio"
        Me.tpAudio.Padding = New System.Windows.Forms.Padding(5)
        Me.tpAudio.Size = New System.Drawing.Size(1900, 1010)
        Me.tpAudio.TabIndex = 4
        Me.tpAudio.Text = "   Audio   "
        Me.tpAudio.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel2
        '
        Me.TableLayoutPanel2.ColumnCount = 2
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel2.Controls.Add(Me.FlowLayoutPanel1, 1, 0)
        Me.TableLayoutPanel2.Controls.Add(Me.dgvAudio, 0, 0)
        Me.TableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel2.Location = New System.Drawing.Point(5, 5)
        Me.TableLayoutPanel2.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel2.Name = "TableLayoutPanel2"
        Me.TableLayoutPanel2.RowCount = 1
        Me.TableLayoutPanel2.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel2.Size = New System.Drawing.Size(1890, 1000)
        Me.TableLayoutPanel2.TabIndex = 7
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Controls.Add(Me.bnAdd)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnRemove)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnUp)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnDown)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnPlay)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnEdit)
        Me.FlowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(1599, 0)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(291, 516)
        Me.FlowLayoutPanel1.TabIndex = 6
        '
        'bnAdd
        '
        Me.bnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAdd.Location = New System.Drawing.Point(11, 0)
        Me.bnAdd.Margin = New System.Windows.Forms.Padding(11, 0, 0, 6)
        Me.bnAdd.Size = New System.Drawing.Size(280, 80)
        Me.bnAdd.Text = "Add..."
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRemove.Location = New System.Drawing.Point(11, 86)
        Me.bnRemove.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnRemove.Size = New System.Drawing.Size(280, 80)
        Me.bnRemove.Text = " Remove"
        '
        'bnUp
        '
        Me.bnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnUp.Location = New System.Drawing.Point(11, 172)
        Me.bnUp.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnUp.Size = New System.Drawing.Size(280, 80)
        Me.bnUp.Text = "Up"
        '
        'bnDown
        '
        Me.bnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnDown.Location = New System.Drawing.Point(11, 258)
        Me.bnDown.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnDown.Size = New System.Drawing.Size(280, 80)
        Me.bnDown.Text = "Down"
        '
        'bnPlay
        '
        Me.bnPlay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnPlay.Location = New System.Drawing.Point(11, 344)
        Me.bnPlay.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnPlay.Size = New System.Drawing.Size(280, 80)
        Me.bnPlay.Text = "Play"
        '
        'bnEdit
        '
        Me.bnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnEdit.Location = New System.Drawing.Point(11, 430)
        Me.bnEdit.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnEdit.Size = New System.Drawing.Size(280, 80)
        Me.bnEdit.Text = "Edit..."
        '
        'dgvAudio
        '
        Me.dgvAudio.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvAudio.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvAudio.Location = New System.Drawing.Point(0, 0)
        Me.dgvAudio.Margin = New System.Windows.Forms.Padding(0)
        Me.dgvAudio.Name = "dgvAudio"
        Me.dgvAudio.RowTemplate.Height = 28
        Me.dgvAudio.Size = New System.Drawing.Size(1599, 1000)
        Me.dgvAudio.TabIndex = 0
        '
        'tpOptions
        '
        Me.tpOptions.Controls.Add(Me.SimpleUI)
        Me.tpOptions.Location = New System.Drawing.Point(12, 69)
        Me.tpOptions.Margin = New System.Windows.Forms.Padding(5)
        Me.tpOptions.Name = "tpOptions"
        Me.tpOptions.Padding = New System.Windows.Forms.Padding(5)
        Me.tpOptions.Size = New System.Drawing.Size(1900, 1010)
        Me.tpOptions.TabIndex = 2
        Me.tpOptions.Text = "  Options  "
        Me.tpOptions.UseVisualStyleBackColor = True
        '
        'SimpleUI
        '
        Me.SimpleUI.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SimpleUI.Location = New System.Drawing.Point(5, 5)
        Me.SimpleUI.Margin = New System.Windows.Forms.Padding(5)
        Me.SimpleUI.Name = "SimpleUI"
        Me.SimpleUI.Size = New System.Drawing.Size(1890, 1000)
        Me.SimpleUI.TabIndex = 0
        Me.SimpleUI.Text = "SimpleUI1"
        '
        'tpCommandLine
        '
        Me.tpCommandLine.Controls.Add(Me.TableLayoutPanel1)
        Me.tpCommandLine.Location = New System.Drawing.Point(12, 69)
        Me.tpCommandLine.Margin = New System.Windows.Forms.Padding(5)
        Me.tpCommandLine.Name = "tpCommandLine"
        Me.tpCommandLine.Padding = New System.Windows.Forms.Padding(15, 14, 15, 14)
        Me.tpCommandLine.Size = New System.Drawing.Size(1900, 1010)
        Me.tpCommandLine.TabIndex = 1
        Me.tpCommandLine.Text = " Command Line "
        Me.tpCommandLine.UseVisualStyleBackColor = True
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 36.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.Label1, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.CmdlControl, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(15, 14)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(5)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1870, 982)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(5, 0)
        Me.Label1.Margin = New System.Windows.Forms.Padding(5, 0, 5, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(460, 48)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Additional custom switches:"
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 3
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.Controls.Add(Me.bnCancel, 2, 1)
        Me.tlpMain.Controls.Add(Me.bnOK, 1, 1)
        Me.tlpMain.Controls.Add(Me.bnCommandLinePreview, 0, 1)
        Me.tlpMain.Controls.Add(Me.pnTab, 0, 0)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.Padding = New System.Windows.Forms.Padding(8)
        Me.tlpMain.RowCount = 2
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1233, 852)
        Me.tlpMain.TabIndex = 8
        '
        'pnTab
        '
        Me.tlpMain.SetColumnSpan(Me.pnTab, 3)
        Me.pnTab.Controls.Add(Me.tcMain)
        Me.pnTab.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnTab.Location = New System.Drawing.Point(16, 16)
        Me.pnTab.Margin = New System.Windows.Forms.Padding(8)
        Me.pnTab.Name = "pnTab"
        Me.pnTab.Size = New System.Drawing.Size(1201, 734)
        Me.pnTab.TabIndex = 8
        '
        'MuxerForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1233, 852)
        Me.Controls.Add(Me.tlpMain)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(11, 10, 11, 10)
        Me.Name = "MuxerForm"
        Me.Text = "Container"
        Me.tcMain.ResumeLayout(False)
        Me.tpSubtitles.ResumeLayout(False)
        Me.tpAudio.ResumeLayout(False)
        Me.TableLayoutPanel2.ResumeLayout(False)
        Me.TableLayoutPanel2.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        CType(Me.dgvAudio, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tpOptions.ResumeLayout(False)
        Me.tpCommandLine.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.tlpMain.ResumeLayout(False)
        Me.pnTab.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Muxer As Muxer
    Private AudioBindingSource As New BindingSource

    Sub New(muxer As Muxer)
        MyBase.New()
        InitializeComponent()
        ScaleClientSize(40, 20)
        Text += " - " + muxer.Name
        Me.Muxer = muxer
        SubtitleControl.AddSubtitles(muxer.Subtitles)
        CmdlControl.tb.Text = muxer.AdditionalSwitches
        tcMain.SelectedIndex = s.Storage.GetInt("last selected muxer tab")

        dgvAudio.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgvAudio.MultiSelect = False
        dgvAudio.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvAudio.AllowUserToResizeRows = False
        dgvAudio.RowHeadersVisible = False
        dgvAudio.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgvAudio.AutoGenerateColumns = False
        AudioBindingSource.DataSource = ObjectHelp.GetCopy(p.AudioTracks)
        dgvAudio.DataSource = AudioBindingSource

        bnAdd.Image = ImageHelp.GetSymbolImage(Symbol.Add)
        bnRemove.Image = ImageHelp.GetSymbolImage(Symbol.Remove)
        bnPlay.Image = ImageHelp.GetSymbolImage(Symbol.Play)
        bnUp.Image = ImageHelp.GetSymbolImage(Symbol.Up)
        bnDown.Image = ImageHelp.GetSymbolImage(Symbol.Down)
        bnEdit.Image = ImageHelp.GetSymbolImage(Symbol.Repair)

        For Each bn In {bnAdd, bnRemove, bnPlay, bnUp, bnDown, bnEdit}
            bn.TextImageRelation = TextImageRelation.Overlay
            bn.ImageAlign = ContentAlignment.MiddleLeft
            Dim pad = bn.Padding
            pad.Left = Control.DefaultFont.Height \ 10
            pad.Right = pad.Left
            bn.Padding = pad
        Next

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
        TipProvider.SetTip("Additional command line switches that may contain macros.", tpCommandLine)
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        If TypeOf Muxer Is MkvMuxer Then
            s.CmdlPresetsMKV = CmdlControl.Presets.ReplaceUnicode
        ElseIf TypeOf Muxer Is MP4Muxer Then
            s.CmdlPresetsMP4 = CmdlControl.Presets.ReplaceUnicode
        End If

        s.Storage.SetInt("last selected muxer tab", tcMain.SelectedIndex)
        SetValues()

        If DialogResult = DialogResult.OK Then
            p.AudioTracks = DirectCast(AudioBindingSource.DataSource, List(Of AudioProfile))
        End If

        MyBase.OnFormClosed(e)
    End Sub

    Private Sub SetValues()
        SubtitleControl.SetValues(Muxer)
        SimpleUI.Save()
        Muxer.AdditionalSwitches = CmdlControl.tb.Text.ReplaceUnicode
    End Sub

    Private Sub buCmdlPreview_Click() Handles bnCommandLinePreview.Click
        SetValues()
        g.ShowCommandLinePreview("Command Line", Muxer.GetCommandLine)
    End Sub

    Private Sub bnAddAudio_Click(sender As Object, e As EventArgs) Handles bnAdd.Click
        Using d As New System.Windows.Forms.OpenFileDialog
            d.SetFilter(FileTypes.Audio.Union(FileTypes.VideoAudio))
            d.Multiselect = True
            d.SetInitDir(p.TempDir)

            If d.ShowDialog = DialogResult.OK Then
                Dim sb As New SelectionBox(Of AudioProfile)
                sb.Title = "Audio Profile"
                sb.Text = "Please select a audio profile."

                For Each audioProfile In s.AudioProfiles
                    sb.AddItem(audioProfile)
                Next

                If sb.Show = DialogResult.OK Then
                    For Each path In d.FileNames
                        Dim ap = ObjectHelp.GetCopy(sb.SelectedValue)
                        ap.File = path

                        If Not p.Script.GetFilter("Source").Script.Contains("DirectShowSource") Then
                            ap.Delay = g.ExtractDelay(ap.File)
                        End If

                        If FileTypes.VideoAudio.Contains(ap.File.Ext) Then
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
                            ap.Stream = sb2.SelectedValue
                        End If

                        g.MainForm.UpdateSizeOrBitrate()
                        AudioBindingSource.Add(ap)
                        AudioBindingSource.Position = AudioBindingSource.Count - 1
                        UpdateAudioControls()
                    Next
                End If
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
        g.MainForm.UpdateSizeOrBitrate()
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

    Protected Overrides Sub OnShown(e As EventArgs)
        Dim lastAction As Action

        Dim UI = SimpleUI
        UI.Store = Muxer
        UI.BackColor = Color.Transparent

        Dim page = UI.CreateFlowPage("main page")
        page.SuspendLayout()

        Dim tb = UI.AddTextButton()
        tb.Text = "Cover:"
        tb.Expandet = True
        tb.Property = NameOf(Muxer.CoverFile)
        tb.BrowseFile("jpg, png|*.jpg;*.png")

        If Not TypeOf Muxer Is WebMMuxer Then
            tb = UI.AddTextButton()
            tb.Text = "Chapters:"
            tb.Expandet = True
            tb.Property = NameOf(Muxer.ChapterFile)
            tb.BrowseFile("txt, xml|*.txt;*.xml")
        End If

        If TypeOf Muxer Is MkvMuxer Then
            tb = UI.AddTextButton()
            tb.Text = "timestamps:"
            tb.Help = "txt or mkv file"
            tb.Expandet = True
            tb.Property = NameOf(Muxer.TimestampsFile)
            tb.BrowseFile("txt, mkv|*.txt;*.mkv")
        End If

        If TypeOf Muxer Is MkvMuxer Then
            CmdlControl.Presets = s.CmdlPresetsMKV

            Dim tags = UI.AddTextButton()
            tags.Text = "Tags"
            tags.Help = "Tags added to the MKV file." + BR2 + "Syntax: name1: value1; name2: value2"
            tags.Expandet = True
            tags.Property = NameOf(Muxer.Tags)
            tags.MacroDialog()

            tb = UI.AddTextButton()
            tb.Text = "Title:"
            tb.Expandet = True
            tb.Property = NameOf(MkvMuxer.Title)
            tb.MacroDialog()

            Dim t = UI.AddText()
            t.Text = "Video Track Name:"
            t.Help = "Optional name of the video stream that may contain macro."
            t.Expandet = True
            t.Property = NameOf(MkvMuxer.VideoTrackName)

            Dim tm = UI.AddTextMenu()
            tm.Text = "Display Aspect Ratio:"
            tm.Help = "Display Aspect Ratio to be applied by mkvmerge. By default and best practice the aspect ratio should be signalled to the encoder and not to the muxer, use this setting at your own risk."
            tm.Property = NameOf(MkvMuxer.DAR)
            tm.AddMenu(s.DarMenu)

            Dim ml = UI.AddMenu(Of Language)()
            ml.Text = "Video Track Language:"
            ml.Help = "Optional language of the video stream."
            ml.Property = NameOf(MkvMuxer.VideoTrackLanguage)

            lastAction = Sub()
                             For Each i In Language.Languages
                                 If i.IsCommon Then
                                     ml.Button.Add(i.ToString + " (" + i.TwoLetterCode + ", " + i.ThreeLetterCode + ")", i)
                                 Else
                                     ml.Button.Add("More | " + i.ToString.Substring(0, 1).ToUpper + " | " + i.ToString + " (" + i.TwoLetterCode + ", " + i.ThreeLetterCode + ")", i)
                                 End If

                                 Application.DoEvents()
                             Next
                         End Sub

        ElseIf TypeOf Muxer Is MP4Muxer Then
            Dim tags = UI.AddTextButton()
            tags.Text = "Tags"
            Task.Run(Sub()
                         Dim stdOut = ProcessHelp.GetErrOut(Package.MP4Box.Path, "-tag-list")
                         BeginInvoke(Sub() tags.Help = "Tags added to the MP4 file." + BR2 + "Syntax: prop1=val1:prop2=val2" + BR2 + stdOut)
                     End Sub)
            tags.Expandet = True
            tags.Property = NameOf(Muxer.Tags)
            tags.MacroDialog()

            CmdlControl.Presets = s.CmdlPresetsMP4

            Dim tm = UI.AddTextMenu()
            tm.Text = "Pixel Aspect Ratio:"
            tm.Help = "Display Aspect Ratio to be applied by MP4Box. By default and best practice the aspect ratio should be signalled to the encoder and not to the muxer, use this setting at your own risk."
            tm.Property = NameOf(MP4Muxer.PAR)
            tm.AddMenu(s.ParMenu)
        End If

        page.ResumeLayout()
        lastAction?.Invoke

        MyBase.OnShown(e)
    End Sub

    Private Sub MuxerForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        Dim f As New HelpForm()
        f.Doc.WriteStart(Text)
        f.Doc.WriteP(Strings.Muxer)
        f.Doc.WriteTips(TipProvider.GetTips, SimpleUI.ActivePage.TipProvider.GetTips)
        f.Doc.WriteTable("Macros", Strings.MacrosHelp, Macro.GetTips())
        f.Show()
    End Sub
End Class