
Imports StaxRip.CommandLine
Imports StaxRip.UI

Public Class CommandLineForm
    Private Params As CommandLineParams
    Private SearchIndex As Integer
    Private Items As New List(Of Item)
    Private HighlightedControl As Control

    Property HTMLHelp As String

    Event BeforeHelp()

    Sub New(params As CommandLineParams)
        InitializeComponent()
        SimpleUI.ScaleClientSize(37, 26)

        rtbCommandLine.ScrollBars = RichTextBoxScrollBars.None
        rtbCommandLine.ContextMenuStrip.Dispose()
        rtbCommandLine.ContextMenuStrip = cmsCommandLine

        Dim singleList As New List(Of String)

        For Each param In params.Items
            If param.GetKey = "" OrElse singleList.Contains(param.GetKey) Then
                Throw New Exception("key found twice: " + param.GetKey)
            End If

            singleList.Add(param.GetKey)
        Next

        Me.Params = params
        Text = params.Title + " (" & params.Items.Count & " options)"

        InitUI()
        SelectLastPage()
        AddHandler params.ValueChanged, AddressOf ValueChanged
        params.RaiseValueChanged(Nothing)

        cbGoTo.Sorted = True
        cbGoTo.SendMessageCue("Search")
        cbGoTo.Select()

        cms.Add("Execute Command Line", Sub() params.Execute(), p.SourceFile <> "").SetImage(Symbol.fa_terminal)

        cms.Add("Copy Command Line", Sub()
                                         Clipboard.SetText(params.GetCommandLine(True, True))
                                         MsgInfo("Command Line was copied.")
                                     End Sub).SetImage(Symbol.Copy)

        cms.Add("Show Command Line...", Sub() g.ShowCommandLinePreview("Command Line", params.GetCommandLine(True, True)))
        cms.Add("Import Command Line...", Sub() If MsgQuestion("Import command line from clipboard?", Clipboard.GetText) = DialogResult.OK Then BasicVideoEncoder.ImportCommandLine(Clipboard.GetText, params)).SetImage(Symbol.Download)

        cms.Add("Help about this dialog", AddressOf ShowHelp).SetImage(Symbol.Help)
        cms.Add("Help about " + params.GetPackage.Name, Sub() params.GetPackage.ShowHelp()).SetImage(Symbol.Help)
    End Sub

    Sub SelectLastPage()
        SimpleUI.SelectLast(Params.Title + "page selection")
    End Sub

    Sub ValueChanged(item As CommandLineParam)
        rtbCommandLine.SetText(Params.GetCommandLine(False, False))
        rtbCommandLine.SelectionLength = 0
        rtbCommandLine.UpdateHeight()
        UpdateSearchComboBox()
    End Sub

    Sub InitUI()
        Dim flowPanels As New List(Of Control)
        Dim helpControl As Control
        Dim currentFlow As SimpleUI.FlowPage

        For x = 0 To Params.Items.Count - 1
            Dim param = Params.Items(x)
            Dim parent As FlowLayoutPanelEx = SimpleUI.GetFlowPage(param.Path)
            currentFlow = DirectCast(parent, SimpleUI.FlowPage)

            If Not flowPanels.Contains(parent) Then
                flowPanels.Add(parent)
                parent.SuspendLayout()
            End If

            Dim help As String = Nothing

            If param.Switch <> "" Then
                help += param.Switch + BR
            End If

            If param.HelpSwitch <> "" Then
                help += param.HelpSwitch + BR
            End If

            If param.NoSwitch <> "" Then
                help += param.NoSwitch + BR
            End If

            Dim switches = param.Switches

            If Not switches.NothingOrEmpty Then
                help += switches.Join(BR) + BR
            End If

            help += BR

            If TypeOf param Is NumParam Then
                Dim nParam = DirectCast(param, NumParam)

                If nParam.Config(0) > Double.MinValue Then
                    help += "Minimum: " & nParam.Config(0) & BR
                End If

                If nParam.Config(1) < Double.MaxValue Then
                    help += "Maximum: " & nParam.Config(1) & BR
                End If
            End If

            help += BR

            If Not param.URLs.NothingOrEmpty Then
                help += String.Join(BR, param.URLs.Select(Function(val) "[" + val + " " + val + "]"))
            End If

            If param.Help <> "" Then
                help += param.Help
            End If

            If help <> "" Then
                If help.Contains(BR2 + BR) Then
                    help = help.Replace(BR2 + BR, BR2)
                End If

                If help.EndsWith(BR) Then
                    help = help.Trim
                End If
            End If

            If param.Label <> "" Then
                SimpleUI.AddLabel(parent, param.Label).MarginTop = FontHeight \ 2
            End If

            If TypeOf param Is BoolParam Then
                Dim checkBox = SimpleUI.AddBool(parent)
                checkBox.Text = param.Text

                If param.HelpSwitch <> "" Then
                    Dim helpID = param.HelpSwitch
                    checkBox.HelpAction = Sub() Params.ShowHelp(helpID)
                Else
                    checkBox.Help = help
                End If

                checkBox.MarginLeft = param.LeftMargin
                DirectCast(param, BoolParam).InitParam(checkBox)
                helpControl = checkBox
            ElseIf TypeOf param Is NumParam Then
                Dim tempNumParam = DirectCast(param, NumParam)
                Dim nParam = DirectCast(param, NumParam)
                Dim numBlock = SimpleUI.AddNum(parent)
                numBlock.Label.Text = If(param.Text.EndsWith(":"), param.Text, param.Text + ":")

                If param.HelpSwitch <> "" Then
                    Dim helpID = param.HelpSwitch
                    numBlock.Label.HelpAction = Sub() Params.ShowHelp(helpID)
                Else
                    numBlock.Label.Help = help
                End If

                numBlock.NumEdit.Config = nParam.Config
                AddHandler numBlock.Label.MouseDoubleClick, Sub() tempNumParam.Value = tempNumParam.DefaultValue
                DirectCast(param, NumParam).InitParam(numBlock.NumEdit)
                helpControl = numBlock.Label
            ElseIf TypeOf param Is OptionParam Then
                Dim tempOptionParam = DirectCast(param, OptionParam)
                Dim oParam = DirectCast(param, OptionParam)
                Dim menuBlock = SimpleUI.AddMenu(Of Integer)(parent)
                menuBlock.Label.Text = If(param.Text.EndsWith(":"), param.Text, param.Text + ":")

                If param.HelpSwitch <> "" Then
                    Dim helpID = param.HelpSwitch
                    menuBlock.Label.HelpAction = Sub() Params.ShowHelp(helpID)
                    menuBlock.Button.HelpAction = Sub() Params.ShowHelp(helpID)
                Else
                    menuBlock.Help = help
                End If

                helpControl = menuBlock.Label
                AddHandler menuBlock.Label.MouseDoubleClick, Sub() tempOptionParam.ValueChangedUser(tempOptionParam.DefaultValue)

                If oParam.Expand Then
                    menuBlock.Button.Expand = True
                End If

                For x2 = 0 To oParam.Options.Length - 1
                    menuBlock.Button.Add(oParam.Options(x2), x2)
                Next

                oParam.InitParam(menuBlock.Button)
            ElseIf TypeOf param Is StringParam Then
                Dim tempItem = DirectCast(param, StringParam)
                Dim textBlock As SimpleUI.TextBlock

                If tempItem.BrowseFileFilter <> "" Then
                    Dim textButtonBlock = SimpleUI.AddTextButton(parent)
                    textButtonBlock.BrowseFile(tempItem.BrowseFileFilter)
                    textBlock = textButtonBlock
                ElseIf tempItem.Menu <> "" Then
                    Dim textMenuBlock = SimpleUI.AddTextMenu(parent)
                    textMenuBlock.AddMenu(tempItem.Menu)
                    textBlock = textMenuBlock
                Else
                    textBlock = SimpleUI.AddText(parent)
                End If

                textBlock.Label.Text = If(param.Text.EndsWith(":"), param.Text, param.Text + ":")

                If param.HelpSwitch <> "" Then
                    Dim helpID = param.HelpSwitch
                    textBlock.Label.HelpAction = Sub() Params.ShowHelp(helpID)
                Else
                    textBlock.Label.Help = help
                End If

                helpControl = textBlock.Label
                AddHandler textBlock.Label.MouseDoubleClick, Sub() tempItem.Value = tempItem.DefaultValue
                textBlock.Edit.Expand = tempItem.Expand
                tempItem.InitParam(textBlock)
            End If

            If Not helpControl Is Nothing Then
                Dim item As New Item
                item.Control = helpControl
                item.Page = currentFlow
                item.Param = param
                Items.Add(item)
            End If
        Next

        For Each panel In flowPanels
            panel.ResumeLayout()
        Next
    End Sub

    Public Class Item
        Property Page As SimpleUI.FlowPage
        Property Control As Control
        Property Param As CommandLineParam
    End Class

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        SimpleUI.SaveLast(Params.Title + "page selection")
        RemoveHandler Params.ValueChanged, AddressOf ValueChanged
        MyBase.OnFormClosed(e)
    End Sub

    Sub CommandLineForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        ShowHelp()
    End Sub

    Sub ShowHelp()
        RaiseEvent BeforeHelp()

        Dim form As New HelpForm()
        form.Doc.WriteStart(Text)

        form.Doc.WriteH2("How to use the video encoder dialog")

        If cbGoTo.Visible Then
            form.Doc.WriteParagraph("The Search dropdown field at the dialog bottom left lists options and can be used to quickly find options, it searches command line switches, labels and dropdowns. Multiple matches can be cycled by pressing enter.")
        End If

        form.Doc.WriteParagraph("Numeric values and dropdown menu options can be reset to their default value by double clicking on the label.")
        form.Doc.WriteParagraph("The context help is shown with a right-click on a label, dropdown menu or checkbox.")
        form.Doc.WriteParagraph("The command line preview at the bottom of the dialog has a context menu that allows to quickly find and show options.")

        If HTMLHelp <> "" Then
            form.Doc.Writer.WriteRaw(HTMLHelp)
        End If

        form.Doc.WriteTips(SimpleUI.ActivePage.TipProvider.GetTips)
        form.Show()
    End Sub

    Sub cbGoTo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbGoTo.KeyDown
        If e.KeyData = Keys.Enter Then
            SearchIndex += 1
            cbGoTo_TextChanged(Nothing, Nothing)
        Else
            SearchIndex = 0
        End If

        If e.KeyData = Keys.Enter Then
            e.SuppressKeyPress = True
        End If
    End Sub

    Sub cbGoTo_TextChanged(sender As Object, e As EventArgs) Handles cbGoTo.TextChanged
        If Not HighlightedControl Is Nothing Then
            HighlightedControl.Font = New Font(HighlightedControl.Font.FontFamily, HighlightedControl.Font.Size, FontStyle.Regular)
            HighlightedControl = Nothing
        End If

        Dim find = cbGoTo.Text.ToLower
        Dim findNoSpace = find.Replace(" ", "")
        Dim matchedItems As New HashSet(Of Item)

        If find.Length > 1 Then
            For Each item In Items
                If item.Param.Switch = cbGoTo.Text OrElse
                    item.Param.NoSwitch = cbGoTo.Text OrElse
                    item.Param.HelpSwitch = cbGoTo.Text Then

                    matchedItems.Add(item)
                End If

                If Not item.Param.Switches Is Nothing Then
                    For Each switch In item.Param.Switches
                        If switch = cbGoTo.Text Then
                            matchedItems.Add(item)
                        End If
                    Next
                End If
            Next

            For Each item In Items
                If item.Param.Switch.ToLowerEx.Contains(find) OrElse
                    item.Param.NoSwitch.ToLowerEx.Contains(find) OrElse
                    item.Param.HelpSwitch.ToLowerEx.Contains(find) OrElse
                    item.Param.Help.ToLowerEx.Contains(find) OrElse
                    item.Param.Text.ToLowerEx.Contains(find) Then

                    matchedItems.Add(item)
                End If

                If Not item.Param.Switches Is Nothing Then
                    For Each switch In item.Param.Switches
                        If switch.ToLower.Contains(find) Then
                            matchedItems.Add(item)
                        End If
                    Next
                End If

                If TypeOf item.Param Is OptionParam Then
                    Dim param = DirectCast(item.Param, OptionParam)

                    If Not param.Options Is Nothing Then
                        For Each value In param.Options
                            Dim valueNoSpace = value.Replace(" ", "")

                            If value.ToLower.Contains(find) Then
                                matchedItems.Add(item)
                            End If

                            If valueNoSpace.ToLower.Contains(findNoSpace) Then
                                matchedItems.Add(item)
                            End If

                            If value.ToLower.Contains(findNoSpace) Then
                                matchedItems.Add(item)
                            End If

                            If valueNoSpace.ToLower.Contains(find) Then
                                matchedItems.Add(item)
                            End If
                        Next
                    End If

                    If Not param.Values Is Nothing Then
                        For Each value In param.Values
                            Dim valueNoSpace = value.Replace(" ", "")

                            If value.ToLower.Contains(find) Then
                                matchedItems.Add(item)
                            End If

                            If valueNoSpace.ToLower.Contains(findNoSpace) Then
                                matchedItems.Add(item)
                            End If

                            If value.ToLower.Contains(findNoSpace) Then
                                matchedItems.Add(item)
                            End If

                            If valueNoSpace.ToLower.Contains(find) Then
                                matchedItems.Add(item)
                            End If
                        Next
                    End If
                End If
            Next

            Dim visibleItems = matchedItems.Where(Function(arg) arg.Param.Visible)

            If visibleItems.Count > 0 Then
                If SearchIndex >= visibleItems.Count Then
                    SearchIndex = 0
                End If

                Dim control = visibleItems(SearchIndex).Control
                SimpleUI.ShowPage(visibleItems(SearchIndex).Page)
                control.Font = New Font(control.Font.FontFamily, control.Font.Size, FontStyle.Bold)
                HighlightedControl = control
                Exit Sub
            End If
        End If
    End Sub

    Sub UpdateSearchComboBox()
        cbGoTo.Items.Clear()

        For Each i In Items
            If i.Param.Visible Then
                If Not i.Param.Switches Is Nothing Then
                    For Each switch In i.Param.Switches
                        If Not cbGoTo.Items.Contains(switch) Then
                            cbGoTo.Items.Add(switch)
                        End If
                    Next
                End If

                If i.Param.Switch <> "" AndAlso Not cbGoTo.Items.Contains(i.Param.Switch) Then
                    cbGoTo.Items.Add(i.Param.Switch)
                End If

                If i.Param.NoSwitch <> "" AndAlso Not cbGoTo.Items.Contains(i.Param.NoSwitch) Then
                    cbGoTo.Items.Add(i.Param.NoSwitch)
                End If

                If i.Param.HelpSwitch <> "" AndAlso Not cbGoTo.Items.Contains(i.Param.HelpSwitch) Then
                    cbGoTo.Items.Add(i.Param.HelpSwitch)
                End If
            End If
        Next
    End Sub

    Sub CommandLineForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        g.MainForm.PopulateProfileMenu(DynamicMenuItemID.EncoderProfiles)
    End Sub

    Sub rtbCommandLine_MouseUp(sender As Object, e As MouseEventArgs) Handles rtbCommandLine.MouseUp
        If e.Button = MouseButtons.Right Then
            cmsCommandLine.Items.Clear()

            Dim copyItem = cmsCommandLine.Add("Copy Selection", Sub() Clipboard.SetText(rtbCommandLine.SelectedText))
            copyItem.KeyDisplayString = "Ctrl+C"
            copyItem.Visible = rtbCommandLine.SelectionLength > 0

            cmsCommandLine.Add("Copy Command Line", Sub() Clipboard.SetText(Params.GetCommandLine(True, True)))

            Dim find = rtbCommandLine.SelectedText

            If find.Length = 0 Then
                Dim pos = rtbCommandLine.SelectionStart
                Dim leftString = rtbCommandLine.Text.Substring(0, pos)
                Dim left = leftString.LastIndexOf(" ") + 1
                Dim right = rtbCommandLine.Text.Length
                Dim rightString = rtbCommandLine.Text.Substring(pos)
                Dim index = rightString.IndexOf(" ")

                If index > -1 Then
                    right = pos + index
                End If

                If right - left > 0 Then
                    find = rtbCommandLine.Text.Substring(left, right - left)
                End If
            End If

            If find.Length > 0 Then
                cmsCommandLine.Add("Search " + find, Sub()
                                                         cbGoTo.Text = find
                                                         cbGoTo.Focus()
                                                     End Sub)
            End If

            cmsCommandLine.Show(rtbCommandLine, e.Location)
        End If
    End Sub

    Sub rtbCommandLine_MouseDown(sender As Object, e As MouseEventArgs) Handles rtbCommandLine.MouseDown
        If e.Button = MouseButtons.Right AndAlso rtbCommandLine.SelectedText = "" Then
            rtbCommandLine.SelectionStart = rtbCommandLine.GetCharIndexFromPosition(e.Location)
        End If
    End Sub
End Class
