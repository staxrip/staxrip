Imports System.Text.RegularExpressions
Imports System.Globalization

Imports StaxRip.UI
Imports System.ComponentModel

Public Class SubtitleControl
    Inherits UserControl

    Private BindingSource As New BindingSource

    Friend WithEvents bnSetNames As ButtonEx
    Friend WithEvents flpButtons As FlowLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents bnSubtitleEdit As ButtonEx
    Private Items As New BindingList(Of SubtitleItem)

#Region " Designer "
    <DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If

        MyBase.Dispose(disposing)
    End Sub
    Friend WithEvents bnAdd As System.Windows.Forms.Button
    Friend WithEvents bnDown As System.Windows.Forms.Button
    Friend WithEvents bnRemove As System.Windows.Forms.Button
    Friend WithEvents bnUp As System.Windows.Forms.Button
    Friend WithEvents bnBDSup2SubPP As StaxRip.UI.ButtonEx
    Friend WithEvents bnPlay As StaxRip.UI.ButtonEx
    Friend WithEvents dgv As DataGridView
    Private components As System.ComponentModel.IContainer

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.bnAdd = New System.Windows.Forms.Button()
        Me.bnDown = New System.Windows.Forms.Button()
        Me.bnRemove = New System.Windows.Forms.Button()
        Me.bnUp = New System.Windows.Forms.Button()
        Me.dgv = New System.Windows.Forms.DataGridView()
        Me.flpButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnSetNames = New StaxRip.UI.ButtonEx()
        Me.bnPlay = New StaxRip.UI.ButtonEx()
        Me.bnBDSup2SubPP = New StaxRip.UI.ButtonEx()
        Me.bnSubtitleEdit = New StaxRip.UI.ButtonEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        CType(Me.dgv, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.flpButtons.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnAdd
        '
        Me.bnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAdd.Location = New System.Drawing.Point(10, 0)
        Me.bnAdd.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnAdd.Name = "bnAdd"
        Me.bnAdd.Size = New System.Drawing.Size(280, 80)
        Me.bnAdd.TabIndex = 12
        Me.bnAdd.Text = "Add..."
        '
        'bnDown
        '
        Me.bnDown.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnDown.Location = New System.Drawing.Point(10, 258)
        Me.bnDown.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnDown.Name = "bnDown"
        Me.bnDown.Size = New System.Drawing.Size(280, 80)
        Me.bnDown.TabIndex = 10
        Me.bnDown.Text = "Down"
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRemove.Location = New System.Drawing.Point(10, 86)
        Me.bnRemove.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnRemove.Name = "bnRemove"
        Me.bnRemove.Size = New System.Drawing.Size(280, 80)
        Me.bnRemove.TabIndex = 13
        Me.bnRemove.Text = " Remove"
        '
        'bnUp
        '
        Me.bnUp.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnUp.Location = New System.Drawing.Point(10, 172)
        Me.bnUp.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnUp.Name = "bnUp"
        Me.bnUp.Size = New System.Drawing.Size(280, 80)
        Me.bnUp.TabIndex = 11
        Me.bnUp.Text = "Up"
        '
        'dgv
        '
        Me.dgv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgv.Location = New System.Drawing.Point(0, 0)
        Me.dgv.Margin = New System.Windows.Forms.Padding(0)
        Me.dgv.Name = "dgv"
        Me.dgv.RowTemplate.Height = 28
        Me.dgv.Size = New System.Drawing.Size(249, 731)
        Me.dgv.TabIndex = 17
        '
        'flpButtons
        '
        Me.flpButtons.AutoSize = True
        Me.flpButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpButtons.Controls.Add(Me.bnAdd)
        Me.flpButtons.Controls.Add(Me.bnRemove)
        Me.flpButtons.Controls.Add(Me.bnUp)
        Me.flpButtons.Controls.Add(Me.bnDown)
        Me.flpButtons.Controls.Add(Me.bnSetNames)
        Me.flpButtons.Controls.Add(Me.bnPlay)
        Me.flpButtons.Controls.Add(Me.bnBDSup2SubPP)
        Me.flpButtons.Controls.Add(Me.bnSubtitleEdit)
        Me.flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.flpButtons.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.flpButtons.Location = New System.Drawing.Point(249, 0)
        Me.flpButtons.Margin = New System.Windows.Forms.Padding(0)
        Me.flpButtons.Name = "flpButtons"
        Me.flpButtons.Size = New System.Drawing.Size(290, 688)
        Me.flpButtons.TabIndex = 20
        '
        'bnSetNames
        '
        Me.bnSetNames.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnSetNames.Location = New System.Drawing.Point(10, 344)
        Me.bnSetNames.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnSetNames.Size = New System.Drawing.Size(280, 80)
        Me.bnSetNames.Text = "Set Names"
        '
        'bnPlay
        '
        Me.bnPlay.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnPlay.Location = New System.Drawing.Point(10, 430)
        Me.bnPlay.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnPlay.Size = New System.Drawing.Size(280, 80)
        Me.bnPlay.Text = "Play"
        '
        'bnBDSup2SubPP
        '
        Me.bnBDSup2SubPP.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnBDSup2SubPP.Location = New System.Drawing.Point(10, 516)
        Me.bnBDSup2SubPP.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnBDSup2SubPP.Size = New System.Drawing.Size(280, 80)
        Me.bnBDSup2SubPP.Text = "BDSup2Sub++"
        '
        'bnSubtitleEdit
        '
        Me.bnSubtitleEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnSubtitleEdit.Location = New System.Drawing.Point(10, 602)
        Me.bnSubtitleEdit.Margin = New System.Windows.Forms.Padding(10, 0, 0, 6)
        Me.bnSubtitleEdit.Size = New System.Drawing.Size(280, 80)
        Me.bnSubtitleEdit.Text = "Subtitle Edit"
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 2
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.Controls.Add(Me.flpButtons, 1, 0)
        Me.tlpMain.Controls.Add(Me.dgv, 0, 0)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(1, 2, 1, 2)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 1
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.Size = New System.Drawing.Size(539, 731)
        Me.tlpMain.TabIndex = 21
        '
        'SubtitleControl
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.Controls.Add(Me.tlpMain)
        Me.Margin = New System.Windows.Forms.Padding(1, 2, 1, 2)
        Me.Name = "SubtitleControl"
        Me.Size = New System.Drawing.Size(539, 731)
        CType(Me.dgv, System.ComponentModel.ISupportInitialize).EndInit()
        Me.flpButtons.ResumeLayout(False)
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Sub New()
        MyBase.New()
        InitializeComponent()

        Text = "Subtitles"

        dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
        dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells
        dgv.AutoGenerateColumns = False
        dgv.ShowCellToolTips = False
        dgv.AllowUserToResizeRows = False
        dgv.AllowUserToResizeColumns = False
        dgv.RowHeadersVisible = False
        dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgv.MultiSelect = False
        dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.Disable

        bnAdd.Image = ImageHelp.GetSymbolImage(Symbol.Add)
        bnRemove.Image = ImageHelp.GetSymbolImage(Symbol.Remove)
        bnPlay.Image = ImageHelp.GetSymbolImage(Symbol.Play)
        bnUp.Image = ImageHelp.GetSymbolImage(Symbol.Up)
        bnDown.Image = ImageHelp.GetSymbolImage(Symbol.Down)

        For Each bn In {bnAdd, bnRemove, bnPlay, bnUp, bnDown}
            bn.TextImageRelation = TextImageRelation.Overlay
            bn.ImageAlign = ContentAlignment.MiddleLeft
            Dim pad = bn.Padding
            pad.Left = Control.DefaultFont.Height \ 10
            pad.Right = pad.Left
            bn.Padding = pad
        Next

        Dim enabledColumn As New DataGridViewCheckBoxColumn
        enabledColumn.HeaderText = "Enabled"
        enabledColumn.DataPropertyName = "Enabled"
        dgv.Columns.Add(enabledColumn)

        Dim languageColumn As New DataGridViewComboBoxColumn
        languageColumn.HeaderText = "Language"
        languageColumn.Items.AddRange(Language.Languages.ToArray)
        languageColumn.DataPropertyName = "Language"
        dgv.Columns.Add(languageColumn)

        Dim nameColumn As New DataGridViewTextBoxColumn
        nameColumn.HeaderText = "Name"
        nameColumn.DataPropertyName = "Title"
        dgv.Columns.Add(nameColumn)

        Dim defaultColumn As New DataGridViewCheckBoxColumn
        defaultColumn.HeaderText = "Default"
        defaultColumn.DataPropertyName = "Default"
        dgv.Columns.Add(defaultColumn)

        Dim forcedColumn As New DataGridViewCheckBoxColumn
        forcedColumn.HeaderText = "Forced"
        forcedColumn.DataPropertyName = "Forced"
        dgv.Columns.Add(forcedColumn)

        Dim idColumn As New DataGridViewTextBoxColumn
        idColumn.ReadOnly = True
        idColumn.HeaderText = "ID"
        idColumn.DataPropertyName = "ID"
        dgv.Columns.Add(idColumn)

        Dim typeNameColumn As New DataGridViewTextBoxColumn
        typeNameColumn.ReadOnly = True
        typeNameColumn.HeaderText = "Type"
        typeNameColumn.DataPropertyName = "TypeName"
        dgv.Columns.Add(typeNameColumn)

        Dim sizeColumn As New DataGridViewTextBoxColumn
        sizeColumn.ReadOnly = True
        sizeColumn.HeaderText = "Size"
        sizeColumn.DataPropertyName = "Size"
        dgv.Columns.Add(sizeColumn)

        Dim filenameColumn As New DataGridViewTextBoxColumn
        filenameColumn.ReadOnly = True
        filenameColumn.HeaderText = "Filename"
        filenameColumn.DataPropertyName = "Filename"
        dgv.Columns.Add(filenameColumn)

        BindingSource.AllowNew = False
        BindingSource.DataSource = Items
        dgv.DataSource = BindingSource
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        UpdateControls()
    End Sub

    Private Sub bnAdd_Click(sender As Object, e As EventArgs) Handles bnAdd.Click
        Using d As New OpenFileDialog
            d.SetFilter(FileTypes.SubtitleIncludingContainers)
            d.Multiselect = True
            d.SetInitDir(s.LastSourceDir)

            If d.ShowDialog = DialogResult.OK Then
                For Each iFilename In d.FileNames
                    AddSubtitles(Subtitle.Create(iFilename))
                Next

                UpdateControls()
            End If
        End Using
    End Sub

    Private Sub bnRemove_Click(sender As Object, e As EventArgs) Handles bnRemove.Click
        dgv.RemoveSelection
        UpdateControls()
    End Sub

    Private Sub bnUp_Click(sender As Object, e As EventArgs) Handles bnUp.Click
        dgv.MoveSelectionUp
        UpdateControls()
    End Sub

    Private Sub bnDown_Click(sender As Object, e As EventArgs) Handles bnDown.Click
        dgv.MoveSelectionDown
        UpdateControls()
    End Sub

    Private Sub dgv_CellParsing(sender As Object, e As DataGridViewCellParsingEventArgs) Handles dgv.CellParsing
        If TypeOf dgv.CurrentCell.OwningColumn Is DataGridViewComboBoxColumn Then
            Dim editingControl = DirectCast(dgv.EditingControl, DataGridViewComboBoxEditingControl)
            e.Value = editingControl.SelectedItem
            e.ParsingApplied = True
        End If
    End Sub

    Private Sub dgv_CellEndEdit(sender As Object, e As DataGridViewCellEventArgs) Handles dgv.CellEndEdit
        UpdateControls()
    End Sub

    Sub SetValues(muxer As Muxer)
        muxer.Subtitles.Clear()

        For Each subtitle In Items
            subtitle.Subtitle.Language = subtitle.Language
            subtitle.Subtitle.Enabled = subtitle.Enabled
            subtitle.Subtitle.Title = subtitle.Title
            subtitle.Subtitle.Forced = subtitle.Forced
            subtitle.Subtitle.Default = subtitle.Default

            If subtitle.Subtitle.Title Is Nothing Then
                subtitle.Subtitle.Title = ""
            End If

            muxer.Subtitles.Add(subtitle.Subtitle)
        Next
    End Sub

    Sub UpdateControls()
        Dim selected = dgv.SelectedRows.Count > 0
        Dim path = If(selected AndAlso dgv.CurrentRow.Index < Items.Count, Items(dgv.CurrentRow.Index).Subtitle.Path, "")
        bnBDSup2SubPP.Enabled = selected AndAlso {"idx", "sup"}.Contains(path.Ext)
        bnSubtitleEdit.Enabled = bnBDSup2SubPP.Enabled
        bnPlay.Enabled = p.SourceFile <> "" AndAlso selected
        bnUp.Enabled = dgv.CanMoveUp
        bnDown.Enabled = dgv.CanMoveDown
        bnSetNames.Enabled = selected
        bnRemove.Enabled = selected
    End Sub

    Sub AddSubtitles(subtitles As List(Of Subtitle))
        For Each i In subtitles
            If Items.Where(Function(item) item.Default).Count > 0 Then i.Default = False

            If File.Exists(i.Path) Then
                Dim size As String

                If i.Size > 0 Then
                    If i.Size > 1024 ^ 2 Then
                        size = (i.Size / 1024 ^ 2).ToString("f1") & " MB"
                    Else
                        size = (i.Size / 1024).ToString("f1") & " KB"
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

                Dim id As Integer

                Dim match = Regex.Match(i.Path, " ID(\d+)")

                If match.Success Then
                    id = CInt(match.Groups(1).Value)
                Else
                    id = i.StreamOrder + 1
                End If

                Dim item As New SubtitleItem
                item.Enabled = i.Enabled
                item.Language = i.Language
                item.Title = i.Title
                item.Default = i.Default
                item.Forced = i.Forced
                item.ID = id
                item.TypeName = i.TypeName
                item.Size = size
                item.Filename = i.Path.FileName
                item.Subtitle = i

                Items.Add(item)
            End If
        Next
    End Sub

    Public Class SubtitleItem
        Property Enabled As Boolean
        Property Language As Language
        Property Title As String
        Property Forced As Boolean
        Property [Default] As Boolean
        Property ID As Integer
        Property TypeName As String
        Property Size As String
        Property Filename As String
        Property Subtitle As Subtitle
    End Class

    Private Sub bnPlay_Click() Handles bnPlay.Click
        If Not Package.mpvnet.VerifyOK(True) Then Exit Sub

        Dim st = Items(dgv.CurrentRow.Index).Subtitle
        Dim filepath = st.Path

        If st.Path.Ext = "idx" Then
            filepath = p.TempDir + p.TargetFile.Base + "_play.idx"
            Regex.Replace(st.Path.ReadAllText, "langidx: \d+", "langidx: " +
                          st.IndexIDX.ToString).WriteFileDefault(filepath)
            FileHelp.Copy(st.Path.DirAndBase + ".sub", filepath.DirAndBase + ".sub")
        End If

        If FileTypes.SubtitleExludingContainers.Contains(filepath.Ext) Then
            g.ShellExecute(Package.mpvnet.Path, "--sub-file=" + filepath.Escape + " " + p.FirstOriginalSourceFile.Escape)
        ElseIf p.FirstOriginalSourceFile = filepath Then
            g.ShellExecute(Package.mpvnet.Path, "--sub=" & (st.Index + 1) & " " + filepath.Escape)
        End If
    End Sub

    Private Sub bnSetNames_Click(sender As Object, e As EventArgs) Handles bnSetNames.Click
        Using td As New TaskDialog(Of Integer)
            td.MainInstruction = "Set names for all streams."
            td.AddCommand("Set language in English", 1)

            If CultureInfo.CurrentCulture.NeutralCulture.TwoLetterISOLanguageName <> "en" Then
                td.AddCommand("Set language in " + CultureInfo.CurrentCulture.NeutralCulture.DisplayName, 2)
            End If

            Select Case td.Show
                Case 1
                    For Each i In Items
                        i.Title = i.Language.CultureInfo.EnglishName
                        If i.Forced Then i.Title += " (forced)"
                    Next

                    BindingSource.ResetBindings(False)
                Case 2
                    For Each i In Items
                        i.Title = i.Language.CultureInfo.NeutralCulture.DisplayName
                        If i.Forced Then i.Title += " (forced)"
                    Next

                    BindingSource.ResetBindings(False)
            End Select
        End Using
    End Sub

    Private Sub dgv_MouseUp(sender As Object, e As MouseEventArgs) Handles dgv.MouseUp
        UpdateControls()
    End Sub

    Private Sub bnBDSup2SubPP_Click(sender As Object, e As EventArgs) Handles bnBDSup2SubPP.Click
        Try
            Dim st = Items(dgv.CurrentRow.Index).Subtitle
            Dim fp = st.Path

            If fp.Ext = "idx" Then
                fp = p.TempDir + p.TargetFile.Base + "_temp.idx"
                Regex.Replace(st.Path.ReadAllText, "langidx: \d+", "langidx: " + st.IndexIDX.ToString).WriteFileDefault(fp)
                FileHelp.Copy(st.Path.DirAndBase + ".sub", fp.DirAndBase + ".sub")
            End If

            g.ShellExecute(Package.BDSup2SubPP.Path, """" + fp + """")
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    Private Sub bnSubtitleEdit_Click(sender As Object, e As EventArgs) Handles bnSubtitleEdit.Click
        Try
            Dim st = Items(dgv.CurrentRow.Index).Subtitle
            Dim fp = st.Path

            If fp.ExtFull = ".idx" Then
                fp = p.TempDir + p.TargetFile.Base + "_temp.idx"
                Regex.Replace(st.Path.ReadAllText, "langidx: \d+", "langidx: " + st.IndexIDX.ToString).WriteFileDefault(fp)
                FileHelp.Copy(st.Path.DirAndBase + ".sub", fp.DirAndBase + ".sub")
            End If

            g.ShellExecute(Package.SubtitleEdit.Path, fp.Escape)
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub
End Class