Imports StaxRip.UI
Imports Microsoft.Win32
Imports System.Text.RegularExpressions

Class CodeEditor
    Property ActiveTable As FilterTable
    Property Engine As ScriptEngine

    Sub New(doc As VideoScript)
        InitializeComponent()

        KeyPreview = True

        MainFlowLayoutPanel.Padding = New Padding(0, 0, 0, 0)
        MainFlowLayoutPanel.SuspendLayout()

        Engine = doc.Engine

        For Each i In doc.Filters
            MainFlowLayoutPanel.Controls.Add(CreateFilterTable(i))
        Next

        MainFlowLayoutPanel.ResumeLayout()
        AddHandler MainFlowLayoutPanel.Layout, AddressOf MainFlowLayoutPanelLayout

        AutoSizeMode = AutoSizeMode.GrowAndShrink
        AutoSize = True
    End Sub

    Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
        Select Case e.KeyData
            Case Keys.F5
                VideoPreview()
            Case Keys.Control Or Keys.Delete
                If Not ActiveTable Is Nothing Then ActiveTable.RemoveClick()
            Case Keys.Control Or Keys.Up
                If Not ActiveTable Is Nothing Then ActiveTable.MoveUp()
            Case Keys.Control Or Keys.Down
                If Not ActiveTable Is Nothing Then ActiveTable.MoveDown()
        End Select

        MyBase.OnKeyDown(e)
    End Sub

    Shared Function CreateFilterTable(filter As VideoFilter) As FilterTable
        Dim ret As New FilterTable

        ret.Margin = New Padding(0)
        ret.Size = New Size(950, 50)
        ret.cbActive.Checked = filter.Active
        ret.cbActive.Text = filter.Category
        ret.tbName.Text = filter.Name
        ret.rtbScript.Text = Macro.SolveInteractive(If(filter.Script = "", "", filter.Script + BR))
        ret.SetColor()

        Return ret
    End Function

    Function GetFilters() As List(Of VideoFilter)
        Dim ret As New List(Of VideoFilter)

        For Each i As FilterTable In MainFlowLayoutPanel.Controls
            Dim f As New VideoFilter()
            f.Active = i.cbActive.Checked
            f.Category = i.cbActive.Text
            f.Path = i.tbName.Text
            f.Script = i.rtbScript.Text.FixBreak.Trim
            ret.Add(f)
        Next

        Return ret
    End Function

    Sub Play()
        If p.SourceFile = "" Then Exit Sub
        Dim doc As New VideoScript
        doc.Engine = Engine
        doc.Path = p.TempDir + p.Name + "_scriptEditor." + doc.FileType
        doc.Filters = GetFilters()
        g.PlayScript(doc)
    End Sub

    Sub VideoPreview()
        If p.SourceFile = "" Then Exit Sub
        Dim doc As New VideoScript
        doc.Engine = Engine
        doc.Path = p.TempDir + p.Name + "_editor." + doc.FileType
        doc.Filters = GetFilters()

        Dim errMsg = doc.GetErrorMessage

        If Not errMsg Is Nothing Then
            MsgError(errMsg)
            Exit Sub
        End If

        doc.Synchronize(True)

        Dim f As New PreviewForm(doc)
        f.Owner = g.MainForm
        f.Show()
    End Sub

    Sub MainFlowLayoutPanelLayout(sender As Object, e As LayoutEventArgs)
        Dim filterTables = MainFlowLayoutPanel.Controls.OfType(Of FilterTable)
        If filterTables.Count = 0 Then Exit Sub
        Dim maxTextWidth = Aggregate i In filterTables Into Max(i.TrimmedTextSize.Width)

        For Each table As FilterTable In MainFlowLayoutPanel.Controls
            Dim rtbSize As Size
            rtbSize.Width = maxTextWidth + FontHeight
            If rtbSize.Width < 300 Then rtbSize.Width = 300
            rtbSize.Height = table.TrimmedTextSize.Height + CInt(FontHeight * 0.3)
            table.rtbScript.Size = rtbSize
            table.rtbScript.Refresh()
        Next
    End Sub

    Class FilterTable
        Inherits TableLayoutPanel

        Property tbName As New TextEdit
        Property rtbScript As RichTextBoxEx
        Property cbActive As New CheckBox
        Property Menu As New ContextMenuStripEx
        Property LastTextSize As Size
        Property Editor As CodeEditor

        Sub New()
            AutoSize = True

            cbActive.AutoSize = True
            cbActive.Anchor = AnchorStyles.Left Or AnchorStyles.Right
            cbActive.Margin = New Padding(0)

            tbName.Dock = DockStyle.Top
            tbName.Margin = New Padding(0, 0, 12, 0)

            rtbScript = New RichTextBoxEx(False)
            rtbScript.EnableAutoDragDrop = True
            rtbScript.Dock = DockStyle.Fill
            rtbScript.WordWrap = False
            rtbScript.ScrollBars = RichTextBoxScrollBars.None
            rtbScript.AcceptsTab = True
            rtbScript.Margin = New Padding(0)
            rtbScript.Font = New Font("Consolas", 10 * s.UIScaleFactor)

            AddHandler Disposed, Sub() Menu.Dispose()
            AddHandler cbActive.CheckedChanged, Sub() SetColor()
            AddHandler rtbScript.MouseUp, AddressOf HandleMouseUp
            AddHandler rtbScript.Enter, Sub() Editor.ActiveTable = Me
            AddHandler rtbScript.TextChanged, Sub()
                                                  If Parent Is Nothing Then Exit Sub
                                                  Dim filterTables = Parent.Controls.OfType(Of FilterTable)
                                                  Dim maxTextWidth = Aggregate i In filterTables Into Max(i.TrimmedTextSize.Width)

                                                  Dim textSizeVar = TrimmedTextSize

                                                  If textSizeVar.Width > maxTextWidth OrElse
                                                                                      (textSizeVar.Width = maxTextWidth AndAlso
                                                                                      textSizeVar.Width <> LastTextSize.Width) OrElse
                                                                                      LastTextSize.Height <> textSizeVar.Height AndAlso
                                                                                      textSizeVar.Height > FontHeight Then

                                                      Parent.PerformLayout()
                                                      LastTextSize = TrimmedTextSize
                                                  End If

                                                  rtbScript.ScrollToCaret()
                                              End Sub
            ColumnCount = 2
            ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
            ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
            RowCount = 1
            RowStyles.Add(New RowStyle(SizeType.AutoSize))

            Dim t As New TableLayoutPanel
            t.AutoSize = True
            t.SuspendLayout()
            t.Dock = DockStyle.Fill
            t.Margin = New Padding(0)
            t.ColumnCount = 1
            t.ColumnStyles.Add(New ColumnStyle(SizeType.AutoSize))
            t.RowCount = 2
            t.RowStyles.Add(New RowStyle(SizeType.AutoSize))
            t.RowStyles.Add(New RowStyle(SizeType.AutoSize))
            t.Controls.Add(cbActive, 0, 0)
            t.Controls.Add(tbName, 0, 1)
            t.ResumeLayout()

            Controls.Add(t, 0, 0)
            Controls.Add(rtbScript, 1, 0)
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            tbName.Height = CInt(FontHeight * 1.2)
            tbName.Width = FontHeight * 7
            MyBase.OnLayout(levent)
        End Sub

        Protected Overrides Sub OnHandleCreated(e As EventArgs)
            Menu.Form = FindForm()
            Editor = DirectCast(Menu.Form, CodeEditor)
            MyBase.OnHandleCreated(e)
        End Sub

        Sub SetColor()
            If cbActive.Checked Then
                rtbScript.ForeColor = Color.Black
                tbName.TextBox.ForeColor = Color.Black
                cbActive.ForeColor = Color.Black
            Else
                rtbScript.ForeColor = Color.Gray
                tbName.TextBox.ForeColor = Color.Gray
                cbActive.ForeColor = Color.Gray
            End If
        End Sub

        ReadOnly Property TextSize As Size
            Get
                Return TextRenderer.MeasureText(rtbScript.Text, rtbScript.Font, New Size(10000, 5000))
            End Get
        End Property

        ReadOnly Property TrimmedTextSize As Size
            Get
                Dim ret = TextSize

                If ret.Width > Screen.GetWorkingArea(Me).Width * 0.7 Then
                    ret.Width = CInt(Screen.GetWorkingArea(Me).Width * 0.7)
                End If

                Return ret
            End Get
        End Property

        Sub SetParameters(parameters As FilterParameters)
            Dim code = rtbScript.Text.FixBreak
            Dim match = Regex.Match(code, parameters.FunctionName + "\((.+)\)")
            Dim args = FilterParameters.SplitCSV(match.Groups(1).Value)
            Dim newParameters As New List(Of String)

            For Each argument In args
                Dim skip = False

                For Each parameter In parameters.Parameters
                    If argument Like "*" + parameter.Name + "*=*" Then
                        skip = True
                    End If
                Next

                If Not skip Then newParameters.Add(argument)
            Next

            For Each parameter In parameters.Parameters
                newParameters.Add(parameter.Name + " = " + parameter.Value)
            Next

            rtbScript.Text = Regex.Replace(code, parameters.FunctionName + "\((.+)\)",
                                           parameters.FunctionName + "(" + newParameters.Join(", ") + ")")
        End Sub

        Sub HandleMouseUp(sender As Object, e As MouseEventArgs)
            If e.Button <> MouseButtons.Right Then Exit Sub
            Menu.Items.Clear()
            Dim filterProfiles As List(Of FilterCategory)

            If p.Script.Engine = ScriptEngine.AviSynth Then
                filterProfiles = s.AviSynthProfiles
            Else
                filterProfiles = s.VapourSynthProfiles
            End If

            Dim code = rtbScript.Text.FixBreak

            For Each i In FilterParameters.Definitions
                If code.Contains(i.FunctionName + "(") Then
                    Dim match = Regex.Match(code, i.FunctionName + "\((.+)\)")
                    If match.Success Then ActionMenuItem.Add(Menu.Items, i.Text, AddressOf SetParameters, i)
                End If
            Next

            If Menu.Items.Count > 0 Then Menu.Items.Add(New ToolStripSeparator)

            For Each i In filterProfiles
                If i.Name = cbActive.Text Then
                    Dim cat = i
                    Dim catMenuItem = Menu.Add(i.Name)
                    Dim catMenuItemTemp = Menu.Add(i.Name + " | a")

                    AddHandler catMenuItem.DropDownOpened, Sub()
                                                               If catMenuItem.DropDownItems.Count > 1 Then Exit Sub
                                                               catMenuItem.DropDownItems.RemoveAt(0)

                                                               For Each iFilter In cat.Filters
                                                                   Dim tip = iFilter.Script
                                                                   ActionMenuItem.Add(Menu.Items, iFilter.Category + " | " + iFilter.Path, AddressOf ReplaceClick, iFilter.GetCopy, tip)
                                                                   Application.DoEvents()
                                                               Next
                                                           End Sub
                End If
            Next

            ActionMenuItem.Add(Menu.Items, "Blank", AddressOf ReplaceClick, New VideoFilter("Misc", "", ""))

            Menu.Items.Add(New ToolStripSeparator)

            Dim replace = Menu.Add("Replace")
            Dim insert = Menu.Add("Insert")

            ActionMenuItem.Add(replace.DropDownItems, "Blank", AddressOf ReplaceClick, New VideoFilter("Misc", "", ""))
            ActionMenuItem.Add(insert.DropDownItems, "Blank", AddressOf InsertClick, New VideoFilter("Misc", "", ""))

            AddHandler replace.DropDownOpened, Sub()
                                                   If replace.DropDownItems.Count > 1 Then Exit Sub

                                                   For Each i In filterProfiles
                                                       For Each i2 In i.Filters
                                                           Dim tip = i2.Script
                                                           ActionMenuItem.Add(replace.DropDownItems, i.Name + " | " + i2.Path, AddressOf ReplaceClick, i2.GetCopy, tip)
                                                           Application.DoEvents()
                                                       Next
                                                   Next
                                               End Sub

            AddHandler insert.DropDownOpened, Sub()
                                                  If insert.DropDownItems.Count > 1 Then Exit Sub

                                                  For Each i In filterProfiles
                                                      For Each i2 In i.Filters
                                                          Dim tip = i2.Script
                                                          ActionMenuItem.Add(insert.DropDownItems, i.Name + " | " + i2.Path, AddressOf InsertClick, i2.GetCopy, tip)
                                                          Application.DoEvents()
                                                      Next
                                                  Next
                                              End Sub

            Dim add As New MenuItemEx("Add")
            Menu.Items.Add(add)
            ActionMenuItem.Add(add.DropDownItems, "Blank", AddressOf AddClick, New VideoFilter("Misc", "", ""))

            AddHandler add.DropDownOpened, Sub()
                                               If add.DropDownItems.Count > 1 Then Exit Sub

                                               For Each i In filterProfiles
                                                   For Each i2 In i.Filters
                                                       Dim tip = i2.Script
                                                       ActionMenuItem.Add(add.DropDownItems, i.Name + " | " + i2.Path, AddressOf AddClick, i2.GetCopy, tip)
                                                       Application.DoEvents()
                                                   Next
                                               Next
                                           End Sub

            Menu.Items.Add(New ToolStripSeparator)

            Menu.Add("Remove", AddressOf RemoveClick).ShortcutKeyDisplayString = KeysHelp.GetKeyString(Keys.Control Or Keys.Delete)
            Menu.Add("Profiles...", AddressOf g.MainForm.ShowFilterProfilesDialog, "Dialog to edit profiles.")
            Menu.Add("Macros...", AddressOf MacrosForm.ShowDialogForm, "Dialog to edit profiles.")
            Menu.Add("Preview Code...", AddressOf CodePreview, "Previews the script with solved macros.")
            Menu.Add("Join Filters", AddressOf JoinFilters, "Joins all filters into one filter.").Enabled = DirectCast(Parent, FlowLayoutPanel).Controls.Count > 1

            Dim mi = Menu.Add("Video Preview...", AddressOf Editor.VideoPreview, "Previews the script with solved macros.")
            mi.Enabled = p.SourceFile <> ""
            mi.ShortcutKeyDisplayString = "F5"

            Menu.Add("Play...", AddressOf Editor.Play, "Plays the current script with MPC.")

            Menu.Items.Add(New ToolStripSeparator)

            Menu.Add("Move Up", AddressOf MoveUp).ShortcutKeyDisplayString = KeysHelp.GetKeyString(Keys.Control Or Keys.Up)
            Menu.Add("Move Down", AddressOf MoveDown).ShortcutKeyDisplayString = KeysHelp.GetKeyString(Keys.Control Or Keys.Down)

            Menu.Items.Add(New ToolStripSeparator)

            Dim cutAction = Sub()
                                Clipboard.SetText(rtbScript.SelectedText)
                                rtbScript.SelectedText = ""
                            End Sub

            Dim copyAction = Sub() Clipboard.SetText(rtbScript.SelectedText)

            Dim pasteAction = Sub()
                                  rtbScript.SelectedText = Clipboard.GetText
                                  rtbScript.ScrollToCaret()
                              End Sub

            Menu.Add("Cut", cutAction).Enabled = rtbScript.SelectionLength > 0 AndAlso Not rtbScript.ReadOnly
            Menu.Add("Copy", copyAction).Enabled = rtbScript.SelectionLength > 0
            Menu.Add("Paste", pasteAction).Enabled = Clipboard.GetText <> "" AndAlso Not rtbScript.ReadOnly

            Menu.Items.Add(New ToolStripSeparator)
            Dim helpMenuItem = Menu.Add("Help")
            Dim helpTempMenuItem = Menu.Add("Help | temp")

            Dim helpAction = Sub()
                                 For Each i In Package.Items.Values.OfType(Of PluginPackage)()
                                     If Not i.AviSynthFilterNames Is Nothing Then
                                         For Each i2 In i.AviSynthFilterNames
                                             If rtbScript.Text.Contains(i2) Then
                                                 Dim path = i.GetHelpPath()
                                                 If path <> "" Then Menu.Add("Help | " + i.Name, Sub() g.ShellExecute(path), path)
                                             End If
                                         Next
                                     End If
                                 Next

                                 If p.Script.Engine = ScriptEngine.AviSynth Then
                                     Dim installDir = Registry.LocalMachine.GetString("SOFTWARE\AviSynth", Nothing)
                                     Dim helpText = rtbScript.Text.Left("(")

                                     If helpText.EndsWith("Resize") Then helpText = "Resize"
                                     If helpText.StartsWith("ConvertTo") Then helpText = "Convert"

                                     Dim filterPath = installDir + "\Docs\English\corefilters\" + helpText + ".htm"

                                     If File.Exists(filterPath) Then
                                         Menu.Add("Help | " + helpText, Sub() g.ShellExecute(filterPath), filterPath)
                                     End If

                                     Dim helpIndex = installDir + "\Docs\English\overview.htm"

                                     If File.Exists(helpIndex) Then
                                         Menu.Add("Help | AviSynth local", Sub() g.ShellExecute(helpIndex), helpIndex)
                                     End If

                                     Menu.Add("Help | AviSynth.nl", Sub() g.ShellExecute("http://avisynth.nl"), "http://avisynth.nl")
                                     Menu.Add("Help | AviSynth+", Sub() g.ShellExecute("http://avisynth.nl/index.php/AviSynth%2B"), "http://avisynth.nl/index.php/AviSynth%2B")
                                     Menu.Add("Help | AviSynth+ plugins", Sub() g.ShellExecute("http://avisynth.nl/index.php/AviSynth%2B#AviSynth.2B_x64_plugins"), "http://avisynth.nl/index.php/AviSynth%2B#AviSynth.2B_x64_plugins")
                                     Menu.Add("Help | -")

                                     For Each i In Package.Items.Values.OfType(Of PluginPackage)
                                         Dim helpPath = i.GetHelpPath

                                         If helpPath <> "" AndAlso Not i.AviSynthFilterNames Is Nothing Then
                                             Menu.Add("Help | " + i.Name.Substring(0, 1).ToUpper + " | " + i.Name, Sub() g.ShellExecute(helpPath), i.Description)
                                             Application.DoEvents()
                                         End If
                                     Next
                                 Else
                                     Menu.Add("Help | vapoursynth.com", Sub() g.ShellExecute("http://www.vapoursynth.com"), "http://www.vapoursynth.com")
                                     Menu.Add("Help | VapourSynth plugins", Sub() g.ShellExecute("http://www.vapoursynth.com/doc/pluginlist.html"), "http://www.vapoursynth.com/doc/pluginlist.html")
                                     Menu.Add("Help | -")

                                     For Each i In Package.Items.Values.OfType(Of PluginPackage)
                                         Dim helpPath = i.GetHelpPath

                                         If helpPath <> "" AndAlso Not i.VapourSynthFilterNames Is Nothing Then
                                             Menu.Add("Help | " + i.Name.Substring(0, 1).ToUpper + " | " + i.Name, Sub() g.ShellExecute(helpPath), i.Description)
                                             Application.DoEvents()
                                         End If
                                     Next
                                 End If
                             End Sub

            AddHandler helpMenuItem.DropDownOpened, Sub()
                                                        If helpMenuItem.DropDownItems.Count > 1 Then Exit Sub
                                                        helpTempMenuItem.Visible = False
                                                        helpAction()
                                                    End Sub
            Menu.Show(rtbScript, e.Location)
        End Sub

        Sub JoinFilters()
            Dim flow = DirectCast(Parent, FlowLayoutPanel)
            Dim firstTable = DirectCast(flow.Controls(0), FilterTable)
            firstTable.tbName.Text = "merged"
            firstTable.rtbScript.Text = flow.Controls.OfType(Of FilterTable).Select(Function(arg) If(arg.cbActive.Checked, arg.rtbScript.Text.Trim, "#" + arg.rtbScript.Text.Trim.FixBreak.Replace(BR, "# " + BR))).Join(BR) + BR2 + BR2 + "#"

            For x = flow.Controls.Count - 1 To 1 Step -1
                flow.Controls.RemoveAt(x)
            Next
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

        Sub CodePreview()
            Dim script As New VideoScript
            script.Engine = Editor.Engine
            script.Filters = Editor.GetFilters()

            Using f As New StringEditorForm
                f.tb.ReadOnly = True
                f.cbWrap.Checked = False
                f.cbWrap.Visible = False
                f.tb.Text = script.GetFullScript
                f.tb.SelectionStart = f.tb.Text.Length
                f.tb.SelectionLength = 0
                f.Text = "Script Preview"
                f.Width = 800
                f.Height = 500
                f.bOK.Visible = False
                f.bCancel.Text = "Close"
                f.ShowDialog()
            End Using
        End Sub

        Sub RemoveClick()
            Dim flow = DirectCast(Parent, FlowLayoutPanel)

            If flow.Controls.Count > 1 Then
                flow.Controls.Remove(Me)
                Dispose()
                Editor.ActiveTable = Nothing
            End If
        End Sub

        Sub ReplaceClick(filter As VideoFilter)
            cbActive.Checked = filter.Active
            cbActive.Text = filter.Category
            tbName.Text = filter.Name
            rtbScript.Text = Macro.SolveInteractive(filter.Script)
        End Sub

        Sub InsertClick(filter As VideoFilter)
            Dim flow = DirectCast(Parent, FlowLayoutPanel)
            Dim index = flow.Controls.IndexOf(Me)
            Dim filterTable = CodeEditor.CreateFilterTable(filter)
            flow.SuspendLayout()
            flow.Controls.Add(filterTable)
            flow.Controls.SetChildIndex(filterTable, index)
            flow.ResumeLayout()
        End Sub

        Sub AddClick(f As VideoFilter)
            Dim flow = DirectCast(Parent, FlowLayoutPanel)
            Dim filterTable = CodeEditor.CreateFilterTable(f)
            flow.Controls.Add(filterTable)
        End Sub
    End Class
End Class