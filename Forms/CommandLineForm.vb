Imports System.Threading
Imports System.Threading.Tasks
Imports StaxRip.CommandLine
Imports StaxRip.UI

Public Class CommandLineForm
    Private Params As CommandLineParams

    Public HTMLHelp As String

    Private SearchIndex As Integer
    Private Items As New List(Of Item)

    Public Sub New(params As CommandLineParams)
        InitializeComponent()
        SimpleUI.ScaleClientSize(37, 24)
        rtbCommandLine.ScrollBars = RichTextBoxScrollBars.None
        Dim singleList As New List(Of String)

        For Each i In params.Items
            If i.GetKey = "" OrElse singleList.Contains(i.GetKey) Then
                Throw New Exception("key found twice: " + i.GetKey)
            End If

            singleList.Add(i.GetKey)
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

        cms.Add("Execute Command Line", Sub() params.Execute(), Nothing, p.SourceFile <> "").SetImage(Symbol.fa_terminal)
        cms.Add("Copy Command Line", Sub() Clipboard.SetText(params.GetCommandLine(True, True)))
        cms.Add("Show Command Line...", Sub() g.ShowCommandLinePreview("Command Line", params.GetCommandLine(True, True)))
        cms.Add("Import Command Line...", Sub() If MsgQuestion("Import command line from clipboard?") = DialogResult.OK Then BasicVideoEncoder.ImportCommandLine(Clipboard.GetText, params))

        cms.Add("Help", AddressOf ShowHelp).SetImage(Symbol.Help)
        cms.Add(params.GetPackage.Name + " Help", Sub() g.StartProcess(params.GetPackage.GetHelpPath))
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
            Dim item = Params.Items(x)
            Dim parent As FlowLayoutPanelEx = SimpleUI.GetFlowPage(item.Path)
            currentFlow = DirectCast(parent, SimpleUI.FlowPage)

            If Not flowPanels.Contains(parent) Then
                flowPanels.Add(parent)
                parent.SuspendLayout()
            End If

            Dim help As String = Nothing
            Dim switches = item.Switches

            If item.Switch <> "" Then help += item.Switch + BR
            If item.NoSwitch <> "" Then help += item.NoSwitch + BR
            If Not switches.NothingOrEmpty Then help += switches.Join(BR) + BR

            help += BR

            If TypeOf item Is NumParam Then
                Dim param = DirectCast(item, NumParam)
                If param.Config(0) > Double.MinValue Then help += "Minimum: " & param.Config(0) & BR
                If param.Config(1) < Double.MaxValue Then help += "Maximum: " & param.Config(1) & BR
            End If

            help += BR

            If Not item.URLs.NothingOrEmpty Then help += String.Join(BR, item.URLs.Select(Function(val) "[" + val + " " + val + "]"))
            If item.Help <> "" Then help += item.Help

            If help <> "" Then
                If help.Contains(BR2 + BR) Then help = help.Replace(BR2 + BR, BR2)
                If help.EndsWith(BR) Then help = help.Trim
            End If

            If item.Label <> "" Then SimpleUI.AddLabel(parent, item.Label).MarginTop = FontHeight \ 2

            If TypeOf item Is BoolParam Then
                Dim cb = SimpleUI.AddBool(parent)
                cb.Text = item.Text

                If item.HelpSwitch <> "" Then
                    Dim helpID = item.HelpSwitch
                    cb.HelpAction = Sub() Params.ShowHelp(helpID)
                Else
                    cb.Help = help
                End If

                cb.MarginLeft = item.LeftMargin
                DirectCast(item, BoolParam).InitParam(cb)
                helpControl = cb
            ElseIf TypeOf item Is NumParam Then
                Dim tempItem = DirectCast(item, NumParam)
                Dim param = DirectCast(item, NumParam)
                Dim nb = SimpleUI.AddNum(parent)
                nb.Label.Text = If(item.Text.EndsWith(":"), item.Text, item.Text + ":")

                If item.HelpSwitch <> "" Then
                    Dim helpID = item.HelpSwitch
                    nb.Label.HelpAction = Sub() Params.ShowHelp(helpID)
                Else
                    nb.Label.Help = help
                End If

                nb.NumEdit.Config = param.Config
                AddHandler nb.Label.MouseDoubleClick, Sub() tempItem.Value = tempItem.DefaultValue
                DirectCast(item, NumParam).InitParam(nb.NumEdit)
                helpControl = nb.Label
            ElseIf TypeOf item Is OptionParam Then
                Dim tempItem = DirectCast(item, OptionParam)
                Dim os = DirectCast(item, OptionParam)
                Dim mb = SimpleUI.AddMenu(Of Integer)(parent)
                mb.Label.Text = If(item.Text.EndsWith(":"), item.Text, item.Text + ":")

                If item.HelpSwitch <> "" Then
                    Dim helpID = item.HelpSwitch
                    mb.Label.HelpAction = Sub() Params.ShowHelp(helpID)
                    mb.Button.HelpAction = Sub() Params.ShowHelp(helpID)
                Else
                    mb.Help = help
                End If

                helpControl = mb.Label
                AddHandler mb.Label.MouseDoubleClick, Sub() tempItem.ValueChangedUser(tempItem.DefaultValue)
                If os.Expand Then mb.Button.Expand = True

                For x2 = 0 To os.Options.Length - 1
                    mb.Button.Add(os.Options(x2), x2)
                Next

                os.Init2(mb.Button)
            ElseIf TypeOf item Is StringParam Then
                Dim tempItem = DirectCast(item, StringParam)
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

                textBlock.Label.Text = If(item.Text.EndsWith(":"), item.Text, item.Text + ":")

                If item.HelpSwitch <> "" Then
                    Dim helpID = item.HelpSwitch
                    textBlock.Label.HelpAction = Sub() Params.ShowHelp(helpID)
                Else
                    textBlock.Label.Help = help
                End If

                helpControl = textBlock.Label
                AddHandler textBlock.Label.MouseDoubleClick, Sub() tempItem.Value = tempItem.DefaultValue
                textBlock.Edit.Expand = True
                Dim sp = DirectCast(item, StringParam)
                sp.Init(textBlock)
            End If

            If Not helpControl Is Nothing Then
                Dim item2 As New Item
                item2.Control = helpControl
                item2.Page = currentFlow
                item2.Param = item
                Items.Add(item2)
            End If
        Next

        For Each i In flowPanels
            i.ResumeLayout()
        Next
    End Sub

    Public Class Item
        Property Page As SimpleUI.FlowPage
        Property Control As Control
        Property Param As CommandLineParam
    End Class

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        SimpleUI.SaveLast(Params.Title + "page selection")
        MyBase.OnFormClosed(e)
    End Sub

    Private Sub CommandLineForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        ShowHelp()
    End Sub

    Sub ShowHelp()
        Dim f As New HelpForm()
        f.Doc.WriteStart(Text)
        If cbGoTo.Visible Then f.Doc.WriteP("The Search input field can be used to search for options, it searches the switch and the label. Multiple matches can be cycled by pressing enter.")
        f.Doc.WriteP("Numeric values and options can be reset to their default value by double clicking on the label.")
        f.Doc.WriteP("The context help is shown with a right-click on a label, menu or checkbox.")
        If HTMLHelp <> "" Then f.Doc.Writer.WriteRaw(HTMLHelp)
        f.Doc.WriteTips(SimpleUI.ActivePage.TipProvider.GetTips)
        f.Show()
    End Sub

    Private Sub cbGoTo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbGoTo.KeyDown
        If e.KeyData = Keys.Enter Then
            SearchIndex += 1
            cbGoTo_TextChanged(Nothing, Nothing)
        Else
            SearchIndex = 0
        End If
    End Sub

    Private Sub cbGoTo_TextChanged(sender As Object, e As EventArgs) Handles cbGoTo.TextChanged
        Dim find = cbGoTo.Text.ToLower
        Dim matchedItems As New HashSet(Of Item)

        If find.Length > 1 Then
            For Each item In Items
                If item.Param.NoSwitch?.ToLower?.Contains(find) Then matchedItems.Add(item)
                If item.Param.Switch?.ToLower?.Contains(find) Then matchedItems.Add(item)
                If item.Param.Help?.ToLower?.Contains(find) Then matchedItems.Add(item)
                If item.Param.Text?.ToLower?.Contains(find) Then matchedItems.Add(item)

                If Not item.Param.Switches Is Nothing Then
                    For Each switch In item.Param.Switches
                        If switch.ToLower.Contains(find) Then matchedItems.Add(item)
                    Next
                End If
            Next

            Dim visibleItems = matchedItems.Where(Function(arg) arg.Param.Visible)

            If visibleItems.Count > 0 Then
                If SearchIndex >= visibleItems.Count Then SearchIndex = 0
                Dim control = visibleItems(SearchIndex).Control
                SimpleUI.ShowPage(visibleItems(SearchIndex).Page)
                Highlight(control)
            End If
        End If
    End Sub

    Sub Highlight(c As Control)
        Task.Run(Sub() HighlightAsync(c))
    End Sub

    Private BlockHighlight As Boolean
    Private EarlyExit As Boolean

    Sub HighlightAsync(c As Control)
        While BlockHighlight
            EarlyExit = True
            Thread.Sleep(1)
        End While

        EarlyExit = False
        BlockHighlight = True

        Try
            Dim size As Double = 9

            For x = 0 To 30
                If EarlyExit Then Exit For
                Thread.Sleep(1)
                size += 0.1
                Dim tempSize = size
                Invoke(Sub()
                           c.Font = New Font(c.Font.FontFamily, CSng(tempSize * s.UIScaleFactor), FontStyle.Bold)
                           c.ForeColor = ControlPaint.Dark(ToolStripRendererEx.ColorBorder, 0.1)
                       End Sub)
            Next

            For x = 0 To 30
                If EarlyExit Then Exit For
                Thread.Sleep(1)
                size -= 0.1
                Dim tempSize = size
                Invoke(Sub()
                           c.Font = New Font(c.Font.FontFamily, CSng(tempSize * s.UIScaleFactor), FontStyle.Bold)
                           c.ForeColor = ControlPaint.Dark(ToolStripRendererEx.ColorBorder, 0.1)
                       End Sub)
            Next
        Finally
            Try
                Invoke(Sub()
                           c.Font = New Font(c.Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Regular)
                           c.ForeColor = Color.Black
                       End Sub)
            Catch
            End Try
        End Try

        BlockHighlight = False
    End Sub

    Sub UpdateSearchComboBox()
        cbGoTo.Items.Clear()

        For Each i In Items
            If i.Param.Visible Then
                If Not i.Param.Switches Is Nothing Then
                    For Each switch In i.Param.Switches
                        If Not cbGoTo.Items.Contains(switch) Then cbGoTo.Items.Add(switch)
                    Next
                End If

                If i.Param.Switch <> "" AndAlso Not cbGoTo.Items.Contains(i.Param.Switch) Then
                    cbGoTo.Items.Add(i.Param.Switch)
                End If

                If i.Param.NoSwitch <> "" AndAlso Not cbGoTo.Items.Contains(i.Param.NoSwitch) Then
                    cbGoTo.Items.Add(i.Param.NoSwitch)
                End If
            End If
        Next
    End Sub

    Private Sub CommandLineForm_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        g.MainForm.PopulateProfileMenu(DynamicMenuItemID.EncoderProfiles)
    End Sub
End Class