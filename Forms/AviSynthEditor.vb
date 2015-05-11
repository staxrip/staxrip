Imports StaxRip.UI
Imports System.ComponentModel
Imports Microsoft.Win32

Public Class AviSynthEditor
    Sub New(doc As AviSynthDocument)
        InitializeComponent()

        MainFlowLayoutPanel.Margin = New Padding(0)
        MainFlowLayoutPanel.Padding = New Padding(0, 3, 0, 0)
        MainFlowLayoutPanel.SuspendLayout()

        For Each i In doc.Filters
            MainFlowLayoutPanel.Controls.Add(GetFilterTable(i))
        Next

        MainFlowLayoutPanel.ResumeLayout()
        AddHandler MainFlowLayoutPanel.Layout, AddressOf MainFlowLayoutPanelLayout

        AutoSizeMode = Windows.Forms.AutoSizeMode.GrowAndShrink
        AutoSize = True
    End Sub

    Shared Function GetFilterTable(filter As AviSynthFilter) As FilterTable
        Dim ret As New FilterTable

        ret.Margin = New Padding(3, 0, 3, 0)
        ret.Size = New Size(950, 200)
        ret.cbActive.Checked = filter.Active
        ret.cbActive.Text = filter.Category
        ret.tbName.Text = filter.Name
        ret.rtbScript.Text = If(filter.Script = "", "", filter.Script + CrLf)

        If Not filter.Active Then ret.rtbScript.ForeColor = Color.Gray

        Return ret
    End Function

    Sub ShowPreview()
        Dim doc As New AviSynthDocument()
        doc.Filters = GetFilters()
        MsgInfo(Macro.Solve(doc.GetScript))
    End Sub

    Function GetFilters() As List(Of AviSynthFilter)
        Dim ret As New List(Of AviSynthFilter)

        For Each i As FilterTable In MainFlowLayoutPanel.Controls
            Dim f As New AviSynthFilter()
            f.Active = i.cbActive.Checked
            f.Category = i.cbActive.Text
            f.Path = i.tbName.Text
            f.Script = i.rtbScript.Text.FixBreak.Trim
            ret.Add(f)
        Next

        Return ret
    End Function

    Sub MainFlowLayoutPanelLayout(sender As Object, e As LayoutEventArgs)
        For Each i As FilterTable In MainFlowLayoutPanel.Controls
            i.Width = MainFlowLayoutPanel.Width - i.Margin.Horizontal
            i.Height = CInt(FontHeight * 3.3)
            Dim size = TextRenderer.MeasureText(i.rtbScript.Text, i.rtbScript.Font, New Size(2000, 2000))

            If size.Height > FontHeight * 3 Then
                i.Height = size.Height + i.rtbScript.Font.Height
            End If
        Next
    End Sub

    Class FilterTable
        Inherits TableLayoutPanel

        Property tbName As New TextEdit
        Property rtbScript As RichTextBoxEx
        Property cbActive As New CheckBox
        Property Menu As New ContextMenuStrip

        Sub New()
            cbActive.AutoSize = True
            cbActive.Anchor = AnchorStyles.Left Or AnchorStyles.Right
            tbName.Dock = DockStyle.Top
            tbName.Height = 36
            rtbScript = New RichTextBoxEx(Menu)
            rtbScript.EnableAutoDragDrop = True
            rtbScript.Dock = DockStyle.Fill
            rtbScript.WordWrap = False

            AddHandler Disposed, Sub() Menu.Dispose()
            AddHandler Menu.Opening, AddressOf MenuOpening

            AddHandler cbActive.CheckedChanged, Sub(sender As Object, e As EventArgs)
                                                    If cbActive.Checked Then
                                                        rtbScript.ForeColor = Color.Black
                                                    Else
                                                        rtbScript.ForeColor = Color.Gray
                                                    End If
                                                End Sub

            AddHandler rtbScript.KeyUp, Sub(sender As Object, e As KeyEventArgs)
                                            If e.KeyData = Keys.Enter OrElse e.KeyData = Keys.Back Then
                                                Parent.PerformLayout()
                                            End If
                                        End Sub
            ColumnCount = 2
            ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 30.0!))
            ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 70.0!))
            RowCount = 1
            RowStyles.Add(New RowStyle(SizeType.Percent, 100.0!))
            Dim t As New TableLayoutPanel
            t.SuspendLayout()
            t.Dock = DockStyle.Fill
            t.Margin = New Padding(0)
            t.ColumnCount = 1
            t.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))
            t.RowCount = 2
            t.RowStyles.Add(New RowStyle(SizeType.AutoSize))
            t.RowStyles.Add(New RowStyle(SizeType.AutoSize))
            t.Controls.Add(cbActive, 0, 0)
            t.Controls.Add(tbName, 0, 1)
            t.ResumeLayout()
            Controls.Add(t, 0, 0)
            Controls.Add(rtbScript, 1, 0)
        End Sub

        Sub MenuOpening(sender As Object, e As CancelEventArgs)
            Menu.Items.Clear()

            For Each i In s.AviSynthCategories
                If i.Name = cbActive.Text Then
                    For Each i2 In i.Filters
                        Dim tip = i2.Script
                        If Not tip.Contains("%app:") Then tip = Macro.Solve(tip)
                        ActionMenuItem.Add(Menu.Items, i2.Path, AddressOf ReplaceClick, i2.GetCopy, tip)
                    Next
                End If
            Next

            ActionMenuItem.Add(Menu.Items, "Blank", AddressOf ReplaceClick, New AviSynthFilter("Misc", "", "", True))

            Menu.Items.Add(New ToolStripSeparator)

            Dim replace As New MenuItemEx("Replace")
            Dim insert As New MenuItemEx("Insert")

            Menu.Items.Add(replace)
            Menu.Items.Add(insert)

            For Each i In s.AviSynthCategories
                For Each i2 In i.Filters
                    Dim tip = i2.Script

                    If Not tip.Contains("%app:") Then
                        tip = Macro.Solve(tip)
                    End If

                    ActionMenuItem.Add(replace.DropDownItems,
                        i.Name + " | " + i2.Path, AddressOf ReplaceClick, i2.GetCopy, tip)

                    ActionMenuItem.Add(insert.DropDownItems,
                        i.Name + " | " + i2.Path, AddressOf InsertClick, i2.GetCopy, tip)
                Next
            Next

            ActionMenuItem.Add(replace.DropDownItems, "Blank", AddressOf ReplaceClick, New AviSynthFilter("Misc", "", "", True))
            ActionMenuItem.Add(insert.DropDownItems, "Blank", AddressOf InsertClick, New AviSynthFilter("Misc", "", "", True))

            Dim add As New MenuItemEx("Add")
            Menu.Items.Add(add)

            For Each i In s.AviSynthCategories
                For Each i2 In i.Filters
                    Dim tip = i2.Script

                    If Not tip.Contains("%app:") Then
                        tip = Macro.Solve(tip)
                    End If

                    ActionMenuItem.Add(add.DropDownItems,
                        i.Name + " | " + i2.Path, AddressOf AddClick, i2.GetCopy, tip)
                Next
            Next

            ActionMenuItem.Add(add.DropDownItems, "Blank", AddressOf AddClick, New AviSynthFilter("Misc", "", "", True))

            Menu.Items.Add(New ToolStripSeparator)
            Menu.Items.Add(New ActionMenuItem("Remove", AddressOf RemoveClick, "Removes the selected filter."))
            Menu.Items.Add(New ActionMenuItem("Profiles...", AddressOf g.MainForm.OpenAviSynthFilterProfilesDialog, "Dialog to edit profiles."))
            Menu.Items.Add(New ActionMenuItem("Macros...", AddressOf MacrosForm.ShowDialogForm, "Dialog to edit profiles."))
            Menu.Items.Add(New ActionMenuItem("Preview...", AddressOf Preview, "Previews the script with solved macros."))

            Menu.Items.Add(New ToolStripSeparator)

            Menu.Items.Add(New ActionMenuItem("Move Up", AddressOf MoveUp))
            Menu.Items.Add(New ActionMenuItem("Move Down", AddressOf MoveDown))

            Menu.Items.Add(New ToolStripSeparator)

            For Each i In Packs.Packages.Values.OfType(Of AviSynthPluginPackage)()
                For Each i2 In i.FilterNames
                    If rtbScript.Text.Contains(i2) Then
                        Dim path = i.GetHelpPath()

                        If path <> "" Then
                            Menu.Items.Add(New ActionMenuItem(i.Name + " Help", Sub() g.ShellExecute(path), path))
                        End If
                    End If
                Next
            Next

            Dim installDir = Registry.LocalMachine.GetString("SOFTWARE\AviSynth", Nothing)
            Dim helpText = rtbScript.Text.Left("(")

            If helpText.EndsWith("Resize") Then helpText = "Resize"
            If helpText.StartsWith("ConvertTo") Then helpText = "Convert"

            Dim filterPath = installDir + "\Docs\English\corefilters\" + helpText + ".htm"

            If File.Exists(filterPath) Then
                Menu.Items.Add(New ActionMenuItem(helpText + " Help", Sub() g.ShellExecute(filterPath), filterPath))
            End If

            Dim helpIndex = installDir + "\Docs\English\index.htm"

            If File.Exists(helpIndex) Then
                Menu.Items.Add(New ActionMenuItem("AviSynth Help", Sub() g.ShellExecute(helpIndex), helpIndex))
            Else
                Menu.Items.Add(New ActionMenuItem("AviSynth Help", Sub() g.ShellExecute("http://avisynth.nl"), "http://avisynth.nl"))
            End If
        End Sub

        Sub MoveUp()
            Dim flow = DirectCast(Parent, FlowLayoutPanel)
            Dim index = flow.Controls.IndexOf(Me)
            index -= 1
            If index < 0 Then index = 0
            flow.Controls.SetChildIndex(Me, index)
        End Sub

        Sub MoveDown()
            Dim flow = DirectCast(Parent, FlowLayoutPanel)
            Dim index = flow.Controls.IndexOf(Me)
            index += 1
            If index >= flow.Controls.Count - 1 Then index = flow.Controls.Count - 1
            flow.Controls.SetChildIndex(Me, index)
        End Sub

        Sub Preview()
            Dim doc As New AviSynthDocument
            doc.Filters = DirectCast(FindForm(), AviSynthEditor).GetFilters()
            MsgInfo(Macro.Solve(doc.GetScript))
        End Sub

        Sub RemoveClick()
            Dim flow = DirectCast(Parent, FlowLayoutPanel)
            flow.Controls.Remove(Me)
            Dispose()
        End Sub

        Sub ReplaceClick(f As AviSynthFilter)
            cbActive.Checked = f.Active
            cbActive.Text = f.Category
            tbName.Text = f.Path
            rtbScript.Text = f.Script
        End Sub

        Sub InsertClick(f As AviSynthFilter)
            Dim flow = DirectCast(Parent, FlowLayoutPanel)
            Dim index = flow.Controls.IndexOf(Me)
            Dim filterTable = AviSynthEditor.GetFilterTable(f)
            flow.SuspendLayout()
            flow.Controls.Add(filterTable)
            flow.Controls.SetChildIndex(filterTable, index)
            flow.ResumeLayout()
        End Sub

        Sub AddClick(f As AviSynthFilter)
            Dim flow = DirectCast(Parent, FlowLayoutPanel)
            Dim filterTable = AviSynthEditor.GetFilterTable(f)
            flow.Controls.Add(filterTable)
        End Sub
    End Class
End Class