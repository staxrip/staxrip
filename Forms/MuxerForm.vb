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
    Friend WithEvents buCmdlPreview As System.Windows.Forms.Button
    Friend WithEvents CmdlControl As StaxRip.CommandLineControl
    Friend WithEvents SubtitleControl As StaxRip.SubtitleControl
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents tc As System.Windows.Forms.TabControl
    Friend WithEvents tpSubtitles As System.Windows.Forms.TabPage
    Friend WithEvents tpCommandLine As System.Windows.Forms.TabPage
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents tpOptions As System.Windows.Forms.TabPage
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents SimpleUI As StaxRip.SimpleUI

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.CmdlControl = New StaxRip.CommandLineControl()
        Me.SubtitleControl = New StaxRip.SubtitleControl()
        Me.buCmdlPreview = New System.Windows.Forms.Button()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.tc = New System.Windows.Forms.TabControl()
        Me.tpSubtitles = New System.Windows.Forms.TabPage()
        Me.tpOptions = New System.Windows.Forms.TabPage()
        Me.SimpleUI = New StaxRip.SimpleUI()
        Me.tpCommandLine = New System.Windows.Forms.TabPage()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.tc.SuspendLayout()
        Me.tpSubtitles.SuspendLayout()
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
        Me.CmdlControl.Size = New System.Drawing.Size(912, 372)
        Me.CmdlControl.TabIndex = 0
        '
        'SubtitleControl
        '
        Me.SubtitleControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SubtitleControl.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SubtitleControl.Location = New System.Drawing.Point(3, 3)
        Me.SubtitleControl.Name = "SubtitleControl"
        Me.SubtitleControl.Size = New System.Drawing.Size(939, 408)
        Me.SubtitleControl.TabIndex = 0
        '
        'buCmdlPreview
        '
        Me.buCmdlPreview.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.buCmdlPreview.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.buCmdlPreview.Location = New System.Drawing.Point(516, 470)
        Me.buCmdlPreview.Name = "buCmdlPreview"
        Me.buCmdlPreview.Size = New System.Drawing.Size(175, 34)
        Me.buCmdlPreview.TabIndex = 4
        Me.buCmdlPreview.Text = "Command Line..."
        Me.buCmdlPreview.UseVisualStyleBackColor = True
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(865, 470)
        Me.bnCancel.Size = New System.Drawing.Size(100, 34)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(759, 470)
        Me.bnOK.Size = New System.Drawing.Size(100, 34)
        Me.bnOK.Text = "OK"
        '
        'tc
        '
        Me.tc.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tc.Controls.Add(Me.tpSubtitles)
        Me.tc.Controls.Add(Me.tpOptions)
        Me.tc.Controls.Add(Me.tpCommandLine)
        Me.tc.Location = New System.Drawing.Point(12, 12)
        Me.tc.Name = "tc"
        Me.tc.SelectedIndex = 0
        Me.tc.Size = New System.Drawing.Size(953, 452)
        Me.tc.TabIndex = 5
        '
        'tpSubtitles
        '
        Me.tpSubtitles.Controls.Add(Me.SubtitleControl)
        Me.tpSubtitles.Location = New System.Drawing.Point(4, 34)
        Me.tpSubtitles.Name = "tpSubtitles"
        Me.tpSubtitles.Padding = New System.Windows.Forms.Padding(3)
        Me.tpSubtitles.Size = New System.Drawing.Size(945, 414)
        Me.tpSubtitles.TabIndex = 0
        Me.tpSubtitles.Text = "Subtitles"
        Me.tpSubtitles.UseVisualStyleBackColor = True
        '
        'tpOptions
        '
        Me.tpOptions.Controls.Add(Me.SimpleUI)
        Me.tpOptions.Location = New System.Drawing.Point(4, 29)
        Me.tpOptions.Name = "tpOptions"
        Me.tpOptions.Padding = New System.Windows.Forms.Padding(3)
        Me.tpOptions.Size = New System.Drawing.Size(934, 419)
        Me.tpOptions.TabIndex = 2
        Me.tpOptions.Text = "Options"
        Me.tpOptions.UseVisualStyleBackColor = True
        '
        'SimpleUI
        '
        Me.SimpleUI.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SimpleUI.Location = New System.Drawing.Point(3, 3)
        Me.SimpleUI.Name = "SimpleUI"
        Me.SimpleUI.Size = New System.Drawing.Size(928, 413)
        Me.SimpleUI.TabIndex = 0
        Me.SimpleUI.Text = "SimpleUI1"
        '
        'tpCommandLine
        '
        Me.tpCommandLine.Controls.Add(Me.TableLayoutPanel1)
        Me.tpCommandLine.Location = New System.Drawing.Point(4, 29)
        Me.tpCommandLine.Name = "tpCommandLine"
        Me.tpCommandLine.Padding = New System.Windows.Forms.Padding(8)
        Me.tpCommandLine.Size = New System.Drawing.Size(934, 419)
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
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(918, 403)
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
        'MuxerForm
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(977, 516)
        Me.Controls.Add(Me.tc)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.bnOK)
        Me.Controls.Add(Me.buCmdlPreview)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Name = "MuxerForm"
        Me.Text = "Container"
        Me.tc.ResumeLayout(False)
        Me.tpSubtitles.ResumeLayout(False)
        Me.tpOptions.ResumeLayout(False)
        Me.tpCommandLine.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Muxer As Muxer

    Sub New(muxer As Muxer)
        MyBase.New()
        InitializeComponent()

        Text += " - " + muxer.Name
        Me.Muxer = muxer
        SubtitleControl.AddSubtitles(ObjectHelp.GetCopy(Of List(Of Subtitle))(muxer.Subtitles))

        CmdlControl.tb.Text = muxer.AdditionalSwitches
        tc.SelectedIndex = s.Storage.GetInt("last selected muxer tab")

        Dim ui = SimpleUI
        ui.BackColor = Color.Transparent

        Dim page = ui.CreateFlowPage("main page")

        If Not TypeOf muxer Is WebMMuxer Then
            ui.AddLabel(page, "Chapters:")

            Dim tbb = ui.AddTextButtonBlock(page)
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

            Dim tb = ui.AddTextBlock(page)
            tb.Label.Text = "Title:"
            tb.Label.Tooltip = "Optional title of the output file that may contain macros."
            tb.Label.Offset = offset
            tb.Expand(tb.Edit)
            tb.Edit.Text = DirectCast(muxer, MkvMuxer).Title
            tb.Edit.SaveAction = Sub(value) DirectCast(muxer, MkvMuxer).Title = value

            tb = ui.AddTextBlock(page)
            tb.Label.Text = "Video Stream Name:"
            tb.Label.Tooltip = "Optional name of the video stream that may contain macro."
            tb.Label.Offset = offset
            tb.Expand(tb.Edit)
            tb.Edit.Text = DirectCast(muxer, MkvMuxer).VideoTrackName
            tb.Edit.SaveAction = Sub(value) DirectCast(muxer, MkvMuxer).VideoTrackName = value

            Dim mb = ui.AddMenuButtonBlock(Of Language)(page)
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

    Private Sub buCmdlPreview_Click() Handles buCmdlPreview.Click
        SetValues()
        g.ShowCommandLinePreview(Muxer.GetCommandLine)
    End Sub

    Private Sub MuxerForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        SubtitleControl.lv.SetColumnWidths()
    End Sub
End Class