
Imports System.Text.RegularExpressions

Imports Microsoft.Win32

Imports StaxRip.UI

Public Class CodeEditor
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
            Case Keys.F9
                PlayScriptWithMPV()
            Case Keys.F10
                PlayScriptWithMPC()
            Case Keys.Control Or Keys.Delete
                If Not ActiveTable Is Nothing Then
                    ActiveTable.RemoveClick()
                End If
            Case Keys.Control Or Keys.Up
                If Not ActiveTable Is Nothing Then
                    ActiveTable.MoveUp()
                End If
            Case Keys.Control Or Keys.Down
                If Not ActiveTable Is Nothing Then
                    ActiveTable.MoveDown()
                End If
            Case Keys.Control Or Keys.I
                ShowInfo()
            Case Keys.Control Or Keys.J
                JoinFilters()
            Case Keys.Control Or Keys.P
                g.MainForm.ShowFilterProfilesDialog()
            Case Keys.Control Or Keys.M
                MacrosForm.ShowDialogForm()
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
        ret.rtbScript.Text = If(filter.Script = "", "", filter.Script + BR)
        ret.SetColor()

        Return ret
    End Function

    Function GetFilters() As List(Of VideoFilter)
        Dim ret As New List(Of VideoFilter)

        For Each table As FilterTable In MainFlowLayoutPanel.Controls
            Dim filter As New VideoFilter()
            filter.Active = table.cbActive.Checked
            filter.Category = table.cbActive.Text
            filter.Path = table.tbName.Text
            filter.Script = table.rtbScript.Text.FixBreak.Trim
            ret.Add(filter)
        Next

        Return ret
    End Function

    Function CreateTempScript() As VideoScript
        Dim script As New VideoScript
        script.Engine = Engine
        script.Path = (p.TempDir + p.TargetFile.Base + $"_temp." + script.FileType).ToShortFilePath
        script.Filters = GetFilters()

        If script.GetError <> "" Then
            MsgError("Script Error", script.GetError)
            Exit Function
        End If

        Return script
    End Function

    Sub PlayScriptWithMPV()
        g.PlayScriptWithMPV(CreateTempScript())
    End Sub

    Sub PlayScriptWithMPC()
        g.PlayScriptWithMPC(CreateTempScript())
    End Sub

    Sub VideoPreview()
        If p.SourceFile = "" Then
            Exit Sub
        End If

        Dim script = CreateTempScript()

        If script Is Nothing Then
            Exit Sub
        End If

        script.RemoveFilter("Cutting")

        Dim form As New PreviewForm(script)
        form.Owner = g.MainForm
        form.Show()
    End Sub

    Sub ShowInfo()
        Dim script = CreateTempScript()

        If Not script Is Nothing Then
            g.ShowScriptInfo(script)
        End If
    End Sub

    Sub ShowAdvancedInfo()
        Dim script = CreateTempScript()

        If script Is Nothing Then
            Exit Sub
        End If

        g.ShowAdvancedScriptInfo(script)
    End Sub

    Sub JoinFilters()
        Dim firstTable = DirectCast(MainFlowLayoutPanel.Controls(0), FilterTable)
        firstTable.tbName.Text = "merged"
        firstTable.rtbScript.Text = MainFlowLayoutPanel.Controls.OfType(Of FilterTable).Select(Function(arg) If(arg.cbActive.Checked, arg.rtbScript.Text.Trim, "#" + arg.rtbScript.Text.Trim.FixBreak.Replace(BR, "# " + BR))).Join(BR) + BR2 + BR2

        For x = MainFlowLayoutPanel.Controls.Count - 1 To 1 Step -1
            MainFlowLayoutPanel.Controls.RemoveAt(x)
        Next
    End Sub

    Sub MainFlowLayoutPanelLayout(sender As Object, e As LayoutEventArgs)
        Dim filterTables = MainFlowLayoutPanel.Controls.OfType(Of FilterTable)

        If filterTables.Count = 0 Then
            Exit Sub
        End If

        Dim maxTextWidth = Aggregate i In filterTables Into Max(i.TrimmedTextSize.Width)

        For Each table As FilterTable In MainFlowLayoutPanel.Controls
            Dim sizeRTB As Size
            sizeRTB.Width = maxTextWidth + FontHeight
            sizeRTB.Height = table.TrimmedTextSize.Height + CInt(FontHeight * 0.3)
            table.rtbScript.Size = sizeRTB
            table.rtbScript.Refresh()
        Next
    End Sub

    Sub CodeEditor_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        Dim form As New HelpForm()
        form.Doc.WriteStart(Text)
        form.Doc.WriteTable("Macros", Macro.GetTips(False, True, False))
        form.Show()
    End Sub

    Public Class FilterTable
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
            tbName.Margin = New Padding(0, 0, 0, 0)

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
                Return TextRenderer.MeasureText(rtbScript.Text, rtbScript.Font, New Size(100000, 100000))
            End Get
        End Property

        ReadOnly Property MaxTextWidth As Integer
            Get
                Return Font.Height * 40
            End Get
        End Property

        ReadOnly Property MaxTextHeight As Integer
            Get
                Return Font.Height * 15
            End Get
        End Property

        ReadOnly Property TrimmedTextSize As Size
            Get
                Dim ret = TextSize

                If ret.Width > MaxTextWidth Then
                    ret.Width = MaxTextWidth
                End If

                If ret.Height > MaxTextHeight Then
                    ret.Height = MaxTextHeight
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
                    If argument.ToLower.RemoveChars(" ").Contains(
                        parameter.Name.ToLower.RemoveChars(" ") + "=") Then

                        skip = True
                    End If
                Next

                If Not skip Then
                    newParameters.Add(argument)
                End If
            Next

            For Each parameter In parameters.Parameters
                Dim value = parameter.Value

                If Editor.Engine = ScriptEngine.VapourSynth Then
                    value = value.Replace("""", "'")
                End If

                newParameters.Add(parameter.Name + "=" + value)
            Next

            rtbScript.Text = Regex.Replace(code, parameters.FunctionName + "\((.+)\)",
                                           parameters.FunctionName + "(" + newParameters.Join(", ") + ")")
        End Sub

        Sub HandleMouseUp(sender As Object, e As MouseEventArgs)
            If e.Button <> MouseButtons.Right Then
                Exit Sub
            End If

            Menu.Items.ClearAndDisplose
            Dim filterProfiles = If(p.Script.Engine = ScriptEngine.AviSynth, s.AviSynthProfiles, s.VapourSynthProfiles)
            Dim code = rtbScript.Text.FixBreak

            For Each i In FilterParameters.Definitions
                If code.Contains(i.FunctionName + "(") Then
                    Dim match = Regex.Match(code, i.FunctionName + "\((.+)\)")

                    If match.Success Then
                        ActionMenuItem.Add(Menu.Items, i.Text, AddressOf SetParameters, i)
                    End If
                End If
            Next

            For Each i In filterProfiles
                If i.Name = cbActive.Text Then
                    Dim cat = i
                    Dim catMenuItem = Menu.Add(i.Name)

                    For Each iFilter In cat.Filters
                        Dim tip = iFilter.Script
                        ActionMenuItem.Add(Menu.Items, If(cat.Filters.Count > 1, iFilter.Category + " | ", "") + iFilter.Path, AddressOf ReplaceClick, iFilter.GetCopy, tip)
                    Next
                End If
            Next

            Dim filterMenuItem = Menu.Add("Add, insert or replace a filter   ")
            filterMenuItem.SetImage(Symbol.Filter)

            ActionMenuItem.Add(filterMenuItem.DropDownItems, "Empty Filter", AddressOf FilterClick, New VideoFilter("Misc", "", ""), "Filter with empty values.")

            For Each filterCategory In filterProfiles
                For Each filter In filterCategory.Filters
                    Dim tip = filter.Script
                    ActionMenuItem.Add(filterMenuItem.DropDownItems, filterCategory.Name + " | " + filter.Path, AddressOf FilterClick, filter.GetCopy, tip)
                Next
            Next

            Dim removeMenuItem = Menu.Add("Remove", AddressOf RemoveClick)
            removeMenuItem.KeyDisplayString = "Ctrl+Delete"
            removeMenuItem.SetImage(Symbol.Remove)

            Dim previewMenuItem = Menu.Add("Preview Video...", AddressOf Editor.VideoPreview, "Previews the script with solved macros.")
            previewMenuItem.Enabled = p.SourceFile <> ""
            previewMenuItem.KeyDisplayString = "F5"
            previewMenuItem.SetImage(Symbol.Photo)

            Dim mpvnetMenuItem = Menu.Add("Play with mpv.net", AddressOf Editor.PlayScriptWithMPV, "Plays the current script with mpv.net.")
            mpvnetMenuItem.Enabled = p.SourceFile <> ""
            mpvnetMenuItem.KeyDisplayString = "F9"
            mpvnetMenuItem.SetImage(Symbol.Play)

            Dim mpcMenuItem = Menu.Add("Play with mpc", AddressOf Editor.PlayScriptWithMPC, "Plays the current script with MPC.")
            mpcMenuItem.Enabled = p.SourceFile <> ""
            mpcMenuItem.KeyDisplayString = "F10"
            mpcMenuItem.SetImage(Symbol.Play)

            Menu.Add("Preview Code...", AddressOf CodePreview, "Previews the script with solved macros.").SetImage(Symbol.Code)

            Dim infoMenuItem = Menu.Add("Info...", AddressOf Editor.ShowInfo, "Previews script parameters such as framecount and colorspace.")
            infoMenuItem.SetImage(Symbol.Info)
            infoMenuItem.KeyDisplayString = "Ctrl+I"
            infoMenuItem.Enabled = p.SourceFile <> ""

            Menu.Add("Advanced Info...", AddressOf Editor.ShowAdvancedInfo, p.SourceFile <> "").SetImage(Symbol.Lightbulb)

            Dim joinMenuItem = Menu.Add("Join Filters", AddressOf Editor.JoinFilters, "Joins all filters into one filter.")
            joinMenuItem.Enabled = DirectCast(Parent, FlowLayoutPanel).Controls.Count > 1
            joinMenuItem.ShortcutKeyDisplayString = "Ctrl+J   "

            Dim profilesMenuItem = Menu.Add("Profiles...", AddressOf g.MainForm.ShowFilterProfilesDialog, "Dialog to edit profiles.")
            profilesMenuItem.KeyDisplayString = "Ctrl+P"
            profilesMenuItem.SetImage(Symbol.FavoriteStar)

            Dim macrosMenuItem = Menu.Add("Macros...", AddressOf MacrosForm.ShowDialogForm, "Dialog to choose macros.")
            macrosMenuItem.KeyDisplayString = "Ctrl+M"
            macrosMenuItem.SetImage(Symbol.CalculatorPercentage)

            Menu.Add("-")

            Dim moveUpMenuItem = Menu.Add("Move Up", AddressOf MoveUp)
            moveUpMenuItem.KeyDisplayString = "Ctrl+Up"
            moveUpMenuItem.SetImage(Symbol.Up)

            Dim moveDownMenuItem = Menu.Add("Move Down", AddressOf MoveDown)
            moveDownMenuItem.KeyDisplayString = "Ctrl+Down"
            moveDownMenuItem.SetImage(Symbol.Down)

            Menu.Add("-")

            Dim cutAction = Sub()
                                Clipboard.SetText(rtbScript.SelectedText)
                                rtbScript.SelectedText = ""
                            End Sub

            Dim copyAction = Sub() Clipboard.SetText(rtbScript.SelectedText)

            Dim pasteAction = Sub()
                                  rtbScript.SelectedText = Clipboard.GetText
                                  rtbScript.ScrollToCaret()
                              End Sub

            Dim cutMenuItem = Menu.Add("Cut", cutAction, rtbScript.SelectionLength > 0 AndAlso Not rtbScript.ReadOnly)
            cutMenuItem.SetImage(Symbol.Cut)
            cutMenuItem.KeyDisplayString = "Ctrl+X"

            Dim copyMenuItem = Menu.Add("Copy", copyAction, rtbScript.SelectionLength > 0)
            copyMenuItem.SetImage(Symbol.Copy)
            copyMenuItem.KeyDisplayString = "Ctrl+C"

            Dim pasteMenuItem = Menu.Add("Paste", pasteAction, Clipboard.GetText <> "" AndAlso Not rtbScript.ReadOnly)
            pasteMenuItem.SetImage(Symbol.Paste)
            pasteMenuItem.KeyDisplayString = "Ctrl+V"

            Menu.Add("-")
            Dim helpMenuItem = Menu.Add("Help")
            helpMenuItem.SetImage(Symbol.Help)
            Dim helpTempMenuItem = Menu.Add("Help | temp")

            Dim helpAction = Sub()
                                 For Each pluginPack In Package.Items.Values.OfType(Of PluginPackage)()
                                     If Not pluginPack.AvsFilterNames Is Nothing Then
                                         For Each avsFilterName In pluginPack.AvsFilterNames
                                             If rtbScript.Text.Contains(avsFilterName) Then
                                                 Dim helpPath = pluginPack.HelpFileOrURL

                                                 If helpPath <> "" Then
                                                     Menu.Add("Help | " + pluginPack.Name, Sub() pluginPack.ShowHelp(), pluginPack.Description)
                                                 End If
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

                                     Menu.Add("Help | AviSynth Help", Sub() g.ShellExecute("http://avisynth.nl"), "http://avisynth.nl")
                                     Menu.Add("Help | -")

                                     For Each pluginPack In Package.Items.Values.OfType(Of PluginPackage)
                                         Dim helpPath = pluginPack.HelpFileOrURL

                                         If helpPath <> "" AndAlso Not pluginPack.AvsFilterNames Is Nothing Then
                                             Menu.Add("Help | " + pluginPack.Name.Substring(0, 1).ToUpper + " | " + pluginPack.Name, Sub() pluginPack.ShowHelp(), pluginPack.Description)
                                             Application.DoEvents()
                                         End If
                                     Next
                                 Else
                                     Menu.Add("Help | VapourSynth Help", Sub() Package.VapourSynth.ShowHelp(), Package.VapourSynth.HelpFileOrURL)
                                     Menu.Add("Help | -")

                                     For Each pluginPack In Package.Items.Values.OfType(Of PluginPackage)
                                         Dim helpPath = pluginPack.HelpFileOrURL

                                         If helpPath <> "" AndAlso Not pluginPack.VSFilterNames Is Nothing Then
                                             Menu.Add("Help | " + pluginPack.Name.Substring(0, 1).ToUpper + " | " + pluginPack.Name, Sub() pluginPack.ShowHelp(), pluginPack.Description)
                                             Application.DoEvents()
                                         End If
                                     Next
                                 End If
                             End Sub

            AddHandler helpMenuItem.DropDownOpened, Sub()
                                                        If helpMenuItem.DropDownItems.Count > 1 Then
                                                            Exit Sub
                                                        End If

                                                        helpTempMenuItem.Visible = False
                                                        helpAction()
                                                    End Sub
            Menu.Show(rtbScript, e.Location)
        End Sub

        Sub MoveUp()
            Dim flow = DirectCast(Parent, FlowLayoutPanel)
            Dim index = flow.Controls.IndexOf(Me)
            index -= 1

            If index < 0 Then
                index = 0
            End If

            flow.Controls.SetChildIndex(Me, index)
        End Sub

        Sub MoveDown()
            Dim flow = DirectCast(Parent, FlowLayoutPanel)
            Dim index = flow.Controls.IndexOf(Me)
            index += 1

            If index >= flow.Controls.Count - 1 Then
                index = flow.Controls.Count - 1
            End If

            flow.Controls.SetChildIndex(Me, index)
        End Sub

        Sub CodePreview()
            Dim script As New VideoScript
            script.Engine = Editor.Engine
            script.Filters = Editor.GetFilters()
            g.CodePreview(script.GetFullScript)
        End Sub

        Sub RemoveClick()
            If MsgQuestion("Remove?") = DialogResult.OK Then
                Dim flow = DirectCast(Parent, FlowLayoutPanel)

                If flow.Controls.Count > 1 Then
                    flow.Controls.Remove(Me)
                    Dispose()
                    Editor.ActiveTable = Nothing
                End If
            End If
        End Sub

        Sub FilterClick(filter As VideoFilter)
            Using td As New TaskDialog(Of String)
                td.MainInstruction = "Choose action"
                td.AddCommand("Replace selection", "Replace")
                td.AddCommand("Insert at selection", "Insert")
                td.AddCommand("Add to end", "Add")

                Select Case td.Show
                    Case "Replace"
                        Dim tup = Macro.ExpandGUI(filter.Script)

                        If tup.Cancel Then
                            Exit Sub
                        End If

                        cbActive.Checked = filter.Active
                        cbActive.Text = filter.Category

                        If tup.Value <> filter.Script AndAlso tup.Caption <> "" Then
                            If filter.Script.StartsWith("$") Then
                                tbName.Text = tup.Caption
                            Else
                                tbName.Text = filter.Name.Replace("...", "") + " " + tup.Caption
                            End If
                        Else
                            tbName.Text = filter.Name
                        End If

                        rtbScript.Text = tup.Value.TrimEnd + BR
                        rtbScript.SelectionStart = rtbScript.Text.Length
                        Application.DoEvents()
                        Menu.Items.ClearAndDisplose
                    Case "Insert"
                        Dim tup = Macro.ExpandGUI(filter.Script)

                        If tup.Cancel Then
                            Exit Sub
                        End If

                        If tup.Value <> filter.Script AndAlso tup.Caption <> "" Then
                            If filter.Script.StartsWith("$") Then
                                filter.Path = tup.Caption
                            Else
                                filter.Path = filter.Path.Replace("...", "") + " " + tup.Caption
                            End If
                        End If

                        filter.Script = tup.Value
                        Dim flow = DirectCast(Parent, FlowLayoutPanel)
                        Dim index = flow.Controls.IndexOf(Me)
                        Dim filterTable = CodeEditor.CreateFilterTable(filter)
                        flow.SuspendLayout()
                        flow.Controls.Add(filterTable)
                        flow.Controls.SetChildIndex(filterTable, index)
                        flow.ResumeLayout()
                        filterTable.rtbScript.SelectionStart = filterTable.rtbScript.Text.Length
                        filterTable.rtbScript.Focus()
                        Application.DoEvents()
                        Menu.Items.ClearAndDisplose
                    Case "Add"
                        Dim tup = Macro.ExpandGUI(filter.Script)

                        If tup.Cancel Then
                            Exit Sub
                        End If

                        If tup.Value <> filter.Script AndAlso tup.Caption <> "" Then
                            If filter.Script.StartsWith("$") Then
                                filter.Path = tup.Caption
                            Else
                                filter.Path = filter.Path.Replace("...", "") + " " + tup.Caption
                            End If
                        End If

                        filter.Script = tup.Value
                        Dim flow = DirectCast(Parent, FlowLayoutPanel)
                        Dim filterTable = CodeEditor.CreateFilterTable(filter)
                        flow.Controls.Add(filterTable)
                        filterTable.rtbScript.SelectionStart = filterTable.rtbScript.Text.Length
                        filterTable.rtbScript.Focus()
                        Application.DoEvents()
                        Menu.Items.ClearAndDisplose
                End Select
            End Using
        End Sub

        Sub ReplaceClick(filter As VideoFilter)
            Dim tup = Macro.ExpandGUI(filter.Script)

            If tup.Cancel Then
                Exit Sub
            End If

            cbActive.Checked = filter.Active
            cbActive.Text = filter.Category

            If tup.Value <> filter.Script AndAlso tup.Caption <> "" Then
                If filter.Script.StartsWith("$") Then
                    tbName.Text = tup.Caption
                Else
                    tbName.Text = filter.Name.Replace("...", "") + " " + tup.Caption
                End If
            Else
                tbName.Text = filter.Name
            End If

            rtbScript.Text = tup.Value.TrimEnd + BR
            rtbScript.SelectionStart = rtbScript.Text.Length
            Application.DoEvents()
            Menu.Items.ClearAndDisplose
        End Sub
    End Class
End Class
