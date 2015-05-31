Imports System.Text.RegularExpressions
Imports System.Globalization

Imports StaxRip.UI

Public Class SubtitleControl
    Inherits UserControl

#Region " Designer "
    <DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If

        MyBase.Dispose(disposing)
    End Sub
    Friend WithEvents lv As StaxRip.UI.ListViewEx
    Friend WithEvents bnAdd As System.Windows.Forms.Button
    Friend WithEvents bnDown As System.Windows.Forms.Button
    Friend WithEvents bnRemove As System.Windows.Forms.Button
    Friend WithEvents bnUp As System.Windows.Forms.Button
    Friend WithEvents bnBDSup2SubPP As StaxRip.UI.ButtonEx
    Friend WithEvents bnPlay As StaxRip.UI.ButtonEx
    Friend WithEvents bnEdit As System.Windows.Forms.Button

    Private components As System.ComponentModel.IContainer

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.bnAdd = New System.Windows.Forms.Button()
        Me.bnDown = New System.Windows.Forms.Button()
        Me.bnRemove = New System.Windows.Forms.Button()
        Me.bnUp = New System.Windows.Forms.Button()
        Me.lv = New StaxRip.UI.ListViewEx()
        Me.bnBDSup2SubPP = New StaxRip.UI.ButtonEx()
        Me.bnPlay = New StaxRip.UI.ButtonEx()
        Me.bnEdit = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'bnAdd
        '
        Me.bnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAdd.Location = New System.Drawing.Point(407, 3)
        Me.bnAdd.Name = "bnAdd"
        Me.bnAdd.Size = New System.Drawing.Size(172, 36)
        Me.bnAdd.TabIndex = 12
        Me.bnAdd.Text = "Add..."
        '
        'bnDown
        '
        Me.bnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnDown.Location = New System.Drawing.Point(407, 171)
        Me.bnDown.Name = "bnDown"
        Me.bnDown.Size = New System.Drawing.Size(172, 36)
        Me.bnDown.TabIndex = 10
        Me.bnDown.Text = "Down"
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRemove.Location = New System.Drawing.Point(407, 45)
        Me.bnRemove.Name = "bnRemove"
        Me.bnRemove.Size = New System.Drawing.Size(172, 36)
        Me.bnRemove.TabIndex = 13
        Me.bnRemove.Text = "Remove"
        '
        'bnUp
        '
        Me.bnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnUp.Location = New System.Drawing.Point(407, 129)
        Me.bnUp.Name = "bnUp"
        Me.bnUp.Size = New System.Drawing.Size(172, 36)
        Me.bnUp.TabIndex = 11
        Me.bnUp.Text = "Up"
        '
        'lv
        '
        Me.lv.AllowDrop = True
        Me.lv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lv.DownButton = Me.bnDown
        Me.lv.Editable = True
        Me.lv.FullRowSelect = True
        Me.lv.Location = New System.Drawing.Point(3, 3)
        Me.lv.Name = "lv"
        Me.lv.RemoveButton = Me.bnRemove
        Me.lv.Size = New System.Drawing.Size(398, 406)
        Me.lv.TabIndex = 0
        Me.lv.UpButton = Me.bnUp
        Me.lv.UseCompatibleStateImageBehavior = False
        Me.lv.View = System.Windows.Forms.View.Details
        '
        'bnBDSup2SubPP
        '
        Me.bnBDSup2SubPP.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnBDSup2SubPP.Location = New System.Drawing.Point(407, 255)
        Me.bnBDSup2SubPP.Size = New System.Drawing.Size(172, 36)
        Me.bnBDSup2SubPP.Text = "BDSup2Sub++"
        '
        'bnPreview
        '
        Me.bnPlay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnPlay.Location = New System.Drawing.Point(407, 213)
        Me.bnPlay.Size = New System.Drawing.Size(172, 36)
        Me.bnPlay.Text = "Play"
        '
        'bnEdit
        '
        Me.bnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnEdit.Location = New System.Drawing.Point(407, 87)
        Me.bnEdit.Name = "bnEdit"
        Me.bnEdit.Size = New System.Drawing.Size(172, 36)
        Me.bnEdit.TabIndex = 14
        Me.bnEdit.Text = "Edit..."
        '
        'SubtitleControl
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.Controls.Add(Me.bnEdit)
        Me.Controls.Add(Me.bnPlay)
        Me.Controls.Add(Me.bnBDSup2SubPP)
        Me.Controls.Add(Me.lv)
        Me.Controls.Add(Me.bnAdd)
        Me.Controls.Add(Me.bnDown)
        Me.Controls.Add(Me.bnRemove)
        Me.Controls.Add(Me.bnUp)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "SubtitleControl"
        Me.Size = New System.Drawing.Size(582, 412)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Sub New()
        MyBase.New()
        InitializeComponent()

        Text = "Subtitles"

        lv.SmallImageList = New ImageList() With {.ImageSize = SystemInformation.SmallIconSize}
        lv.MultiSelect = False
        lv.SendMessageHideFocus()

        lv.Columns.Add("Hidden")
        lv.Columns.Add("Language")
        lv.Columns.Add("Name")
        lv.Columns.Add("Option")
        lv.Columns.Add("ID")
        lv.Columns.Add("Type")
        lv.Columns.Add("Size")
        lv.Columns.Add("Filename")
    End Sub

    Private Sub bAdd_Click(sender As Object, e As EventArgs) Handles bnAdd.Click
        Using d As New OpenFileDialog
            d.SetFilter(FileTypes.SubtitleIncludingContainers)
            d.Multiselect = True
            d.SetInitDir(s.LastSourceDir)

            If d.ShowDialog = DialogResult.OK Then
                For Each i In d.FileNames
                    AddSubtitles(Subtitle.Create(i))
                Next

                lv.AutoResizeColumns(False)
            End If
        End Using
    End Sub

    Sub AddSubtitles(subtitles As List(Of Subtitle))
        For Each i In subtitles
            If File.Exists(i.Path) Then
                Dim size As String

                If Not FileTypes.Video.Contains(Filepath.GetExt(i.Path)) Then
                    Dim len = New FileInfo(i.Path).Length

                    If len > 1024 ^ 2 Then
                        size = (len / 1024 ^ 2).ToString("f1") & " MB"
                    ElseIf len > 1024 Then
                        size = CInt(len / 1024).ToString("f1") & " KB"
                    Else
                        size = CInt(len) & " B"
                    End If
                End If

                Dim _option As String

                If i.Default AndAlso i.Forced Then
                    _option = "default, forced"
                ElseIf i.Default Then
                    _option = "default"
                ElseIf i.Forced Then
                    _option = "forced"
                Else
                    _option = ""
                End If

                Dim id As String

                Dim match = Regex.Match(i.Path, " ID(\d+)")

                If match.Success Then
                    id = match.Groups(1).Value
                Else
                    id = (i.StreamOrder + 1).ToString
                End If

                Dim item = New ListViewItem({"", i.Language.ToString, i.Title, _option, id, i.TypeName, size, Filepath.GetName(i.Path)})
                item.Tag = i
                lv.Items.Add(item)
            End If
        Next

        If lv.Items.Count > 0 Then
            lv.Items(0).Selected = True
            lv.EnsureVisible(lv.Items.Count - 1)
        End If
    End Sub

    Sub SetValues(muxer As Muxer)
        muxer.Subtitles.Clear()

        For Each i As ListViewItem In lv.Items
            muxer.Subtitles.Add(DirectCast(i.Tag, Subtitle))
        Next
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        lv.UpdateControls()
    End Sub

    Private Sub lv_ControlsUpdated() Handles lv.ControlsUpdated
        lv.AutoResizeColumns(False)
        Dim selected = lv.SelectedItems.Count > 0
        Dim path = If(selected, DirectCast(lv.SelectedItems(0).Tag, Subtitle).Path, "")
        bnPlay.Enabled = selected AndAlso IsOneOf(Filepath.GetExtFull(path), ".idx", ".srt")
        bnBDSup2SubPP.Enabled = selected AndAlso IsOneOf(Filepath.GetExtFull(path), ".idx", ".sup")
        bnEdit.Enabled = selected
        bnPlay.Enabled = FileTypes.SubtitleExludingContainers.Contains(Filepath.GetExt(path)) AndAlso p.SourceFile <> ""
    End Sub

    Private Sub lv_DoubleClick() Handles lv.DoubleClick
        If lv.SelectedItems.Count = 0 Then
            bnAdd.PerformClick()
        Else
            bnEdit.PerformClick()
        End If
    End Sub

    Private Sub bnBDSup2SubPP_Click() Handles bnBDSup2SubPP.Click
        Try
            Dim item = DirectCast(lv.SelectedItems(0), ListViewItem)
            Dim st = DirectCast(item.Tag, Subtitle)
            Dim fp = st.Path

            If Filepath.GetExtFull(fp) = ".idx" Then
                fp = p.TempDir + Filepath.GetBase(p.TargetFile) + "_Temp.idx"

                Regex.Replace(File.ReadAllText(st.Path), "langidx: \d+", "langidx: " +
                    st.IndexIDX.ToString).WriteFile(fp)

                FileHelp.Copy(Filepath.GetDirAndBase(st.Path) + ".sub", Filepath.GetDirAndBase(fp) + ".sub")
            End If

            g.ShellExecute(Packs.BDSup2SubPP.GetPath, """" + fp + """")
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    Private Sub bnPlay_Click() Handles bnPlay.Click
        If Packs.MPC.VerifyOK(True) Then
            Try
                Dim item = DirectCast(lv.SelectedItems(0), ListViewItem)
                Dim st = DirectCast(item.Tag, Subtitle)
                Dim fp = st.Path

                Dim avs As New AviSynthDocument
                avs.Path = p.TempDir + Filepath.GetBase(p.TargetFile) + "_Play.avs"
                avs.Filters = p.AvsDoc.GetFiltersCopy

                If FileTypes.TextSub.Contains(Filepath.GetExt(fp)) Then
                    Dim insertCat = If(avs.IsFilterActive("Crop"), "Crop", "Source")

                    If Filepath.GetExtFull(st.Path) = ".idx" Then
                        fp = p.TempDir + Filepath.GetBase(p.TargetFile) + "_Play.idx"

                        Regex.Replace(File.ReadAllText(st.Path), "langidx: \d+", "langidx: " +
                            st.IndexIDX.ToString).WriteFile(fp)

                        FileHelp.Copy(Filepath.GetDirAndBase(st.Path) + ".sub", Filepath.GetDirAndBase(fp) + ".sub")

                        avs.InsertAfter(insertCat, New AviSynthFilter("VobSub(""" + fp + """)"))
                    Else
                        avs.InsertAfter(insertCat, New AviSynthFilter("TextSub(""" + fp + """)"))
                    End If
                End If

                Dim par = Calc.GetTargetPAR

                If Not par = New Point(1, 1) Then
                    Dim w = CInt((p.TargetHeight * Calc.GetTargetDAR) / 4) * 4
                    avs.Filters.Add(New AviSynthFilter("LanczosResize(" & w & "," & p.TargetHeight & ")"))
                End If

                Dim ap = p.Audio0

                If Not File.Exists(ap.File) Then ap = p.Audio1

                If File.Exists(ap.File) Then
                    avs.Filters.Add(New AviSynthFilter("KillAudio()"))

                    Dim nic = Audio.GetNicAudioCode(ap)

                    If nic <> "" Then
                        avs.Filters.Add(New AviSynthFilter(nic))
                    Else
                        avs.Filters.Add(New AviSynthFilter("AudioDub(last, DirectShowSource(""" + ap.File + """, video = false))"))
                    End If

                    avs.Filters.Add(New AviSynthFilter("DelayAudio(" & (ap.Delay / 1000).ToString(CultureInfo.InvariantCulture) & ")"))
                End If

                If p.SourceHeight > 576 Then
                    avs.Filters.Add(New AviSynthFilter("ConvertToRGB(matrix=""Rec709"")"))
                Else
                    avs.Filters.Add(New AviSynthFilter("ConvertToRGB(matrix=""Rec601"")"))
                End If

                avs.Synchronize()

                Dim subSwitch As String

                If Not FileTypes.TextSub.Contains(Filepath.GetExt(fp)) AndAlso
                    FileTypes.SubtitleExludingContainers.Contains(Filepath.GetExt(fp)) Then

                    subSwitch = "/sub """ + fp + """"
                End If

                g.Play(avs.Path, subSwitch)
            Catch ex As Exception
                g.ShowException(ex)
            End Try
        End If
    End Sub

    Private Sub bnEdit_Click(sender As Object, e As EventArgs) Handles bnEdit.Click
        Dim listItem = DirectCast(lv.SelectedItems(0), ListViewItem)
        Dim subtitel = DirectCast(listItem.Tag, Subtitle)

        Using form As New SimpleSettingsForm("Subtitle Options")
            form.Size = New Size(500, 300)

            Dim ui = form.SimpleUI
            Dim generalPage = ui.CreateFlowPage("General")
            generalPage.SuspendLayout()

            Dim tb = ui.AddTextBlock(generalPage)
            tb.Label.Text = "Name:"
            tb.Edit.Text = subtitel.Title
            tb.Edit.SaveAction = Sub(value) subtitel.Title = value

            Dim mbi = ui.AddMenuButtonBlock(Of Language)(generalPage)
            mbi.Label.Text = "Language:"
            mbi.Label.Tooltip = "Language of the audio track."
            mbi.MenuButton.Value = subtitel.Language
            mbi.MenuButton.SaveAction = Sub(value) subtitel.Language = value

            Dim cb = ui.AddCheckBox(generalPage)
            cb.Text = "Default"
            cb.Checked = subtitel.Default
            cb.SaveAction = Sub(value) subtitel.Default = value

            cb = ui.AddCheckBox(generalPage)
            cb.Text = "Forced"
            cb.Checked = subtitel.Forced
            cb.SaveAction = Sub(value) subtitel.Forced = value

            For Each i In Language.Languages
                If i.IsCommon Then
                    mbi.MenuButton.Add(i.ToString, i)
                Else
                    mbi.MenuButton.Add("More | " + i.ToString.Substring(0, 1) + " | " + i.ToString, i)
                End If
            Next

            generalPage.ResumeLayout()

            If form.ShowDialog() = DialogResult.OK Then
                ui.Save()

                Dim _option As String

                If subtitel.Default AndAlso subtitel.Forced Then
                    _option = "default, forced"
                ElseIf subtitel.Default Then
                    _option = "default"
                ElseIf subtitel.Forced Then
                    _option = "forced"
                End If

                listItem.SubItems(1).Text = subtitel.Language.ToString
                listItem.SubItems(2).Text = subtitel.Title
                listItem.SubItems(3).Text = _option

                lv.AutoResizeColumns(False)
            End If
        End Using
    End Sub
End Class