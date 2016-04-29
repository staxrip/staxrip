Imports System.Threading
Imports System.Threading.Tasks
Imports System.Globalization

Imports StaxRip.CommandLine
Imports StaxRip.UI

Class CommandLineForm
    Private Params As CommandLineParams

    Public HTMLHelp As String

    Private SearchIndex As Integer
    Private Items As New List(Of Item)

    Public Sub New(params As CommandLineParams)
        InitializeComponent()

        Dim singleList As New List(Of String)

        For Each i In params.Items
            If i.GetKey = "" OrElse singleList.Contains(i.GetKey) Then
                Throw New Exception("key found twice: " + i.GetKey)
            End If

            singleList.Add(i.GetKey)
        Next

        Me.Params = params

        Dim allSwitches As New HashSet(Of String)

        For Each param In params.Items
            If param.Switch <> "" Then allSwitches.Add(param.Switch)

            If Not param.Switches.NothingOrEmpty Then
                For Each switch In param.Switches
                    allSwitches.Add(switch)
                Next
            End If
        Next

        Text = params.Title + " (" & allSwitches.Count & " options)"
        InitUI()
        SelectLastPage()
        AddHandler params.ValueChanged, AddressOf ValueChanged
        params.RaiseValueChanged(Nothing)

        cbGoTo.Sorted = True
        cbGoTo.SendMessageCue("Search")
        cbGoTo.Select()

        cms.Items.Add(New ActionMenuItem("Execute Command Line", Sub() params.Execute(), Nothing, p.SourceFile <> ""))
        cms.Items.Add(New ActionMenuItem("Copy Command Line", Sub() Clipboard.SetText(params.GetCommandLine(True, True))))
        cms.Items.Add(New ActionMenuItem("Show Command Line", Sub() g.ShowCommandLinePreview(params.GetCommandLine(True, True))))
        cms.Items.Add(New ActionMenuItem("Help", Sub() ShowHelp()))
        cms.Items.Add(New ActionMenuItem(params.GetPackage.Name + " Help", Sub() g.ShellExecute(params.GetPackage.GetHelpPath)))
    End Sub

    Sub SelectLastPage()
        SimpleUI.SelectLast(Params.Title + "page selection")
    End Sub

    Sub ValueChanged(item As CommandLineParam)
        rtbCommandLine.SetText(Params.GetCommandLine(False, False))
        rtbCommandLine.SelectionLength = 0
        UpdateHeight()
        UpdateSearchComboBox()
    End Sub

    Sub UpdateHeight()
        Dim s = TextRenderer.MeasureText(rtbCommandLine.Text, rtbCommandLine.Font,
                                         New Size(rtbCommandLine.ClientSize.Width, Integer.MaxValue),
                                         TextFormatFlags.WordBreak)
        Height += CInt(s.Height * 1.2) - rtbCommandLine.Height
        rtbCommandLine.Refresh()
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

            Dim help As String

            If item.Help <> "" Then
                help = item.Help
            Else
                help = Nothing 'compiler bug?
            End If

            If item.Switch <> "" Then
                If item.NoSwitch <> "" Then
                    help = item.Switch + BR2 + item.NoSwitch
                Else
                    help = item.Switch
                End If

                If TypeOf item Is NumParam Then
                    Dim param = DirectCast(item, NumParam)
                    help += BR
                    If param.MinMaxStepDec(0) > Integer.MinValue Then help += BR + "Minimum: " & param.MinMaxStepDec(0)
                    If param.MinMaxStepDec(1) < Integer.MaxValue Then help += BR + "Maximum: " & param.MinMaxStepDec(1)
                End If

                If item.Help <> "" Then help += BR2 + item.Help
                If item.URL <> "" Then help += BR2 + "[" + item.URL + " " + item.URL + "]"
            End If

            If TypeOf item Is BoolParam Then
                Dim cb = SimpleUI.AddCheckBox(parent)
                cb.Margin = New Padding(3) With {.Left = 9}
                cb.Text = item.Text
                cb.Tooltip = help
                If item.URL <> "" Then currentFlow.TipProvider.SetURL(item.URL, cb)
                DirectCast(item, BoolParam).Init(cb)
                helpControl = cb
            ElseIf TypeOf item Is NumParam Then
                Dim tempItem = DirectCast(item, NumParam)
                Dim param = DirectCast(item, NumParam)
                Dim nb = SimpleUI.AddNumericBlock(parent)
                nb.Label.Text = item.Text
                nb.Label.Tooltip = help
                If item.URL <> "" Then currentFlow.TipProvider.SetURL(item.URL, nb.Label)
                nb.NumEdit.Init(param.MinMaxStepDec)
                AddHandler nb.Label.MouseDoubleClick, Sub() tempItem.Value = tempItem.DefaultValue
                DirectCast(item, NumParam).Init(nb.NumEdit)
                helpControl = nb.Label
            ElseIf TypeOf item Is OptionParam Then
                Dim tempItem = DirectCast(item, OptionParam)
                Dim os = DirectCast(item, OptionParam)
                Dim mb = SimpleUI.AddMenuButtonBlock(Of Integer)(parent)
                mb.Label.Text = item.Text
                mb.Tooltip = help
                If item.URL <> "" Then currentFlow.TipProvider.SetURL(item.URL, mb.Label, mb.MenuButton)
                helpControl = mb.Label
                AddHandler mb.Label.MouseDoubleClick, Sub() tempItem.ValueChangedUser(tempItem.DefaultValue)
                If os.Expand Then mb.Expand(mb.MenuButton)

                For x2 = 0 To os.Options.Length - 1
                    mb.MenuButton.Add(os.Options(x2), x2)
                Next

                os.Init(mb.MenuButton)
            ElseIf TypeOf item Is StringParam Then
                Dim tempItem = DirectCast(item, StringParam)
                Dim textBlock As SimpleUI.TextBlock

                If tempItem.BrowseFileFilter <> "" Then
                    Dim textButtonBlock = SimpleUI.AddTextButtonBlock(parent)
                    textButtonBlock.BrowseFile(tempItem.BrowseFileFilter)
                    textBlock = textButtonBlock
                Else
                    textBlock = SimpleUI.AddTextBlock(parent)
                End If

                textBlock.Label.Text = item.Text
                textBlock.Label.Tooltip = help
                helpControl = textBlock.Label
                AddHandler textBlock.Label.MouseDoubleClick, Sub() tempItem.Value = tempItem.DefaultValue
                textBlock.Expand(textBlock.Edit)
                Dim sp = DirectCast(item, StringParam)
                sp.Init(textBlock)
            End If

            If Not helpControl Is Nothing Then
                Dim item2 As New Item
                item2.Control = helpControl
                item2.Page = currentFlow
                item2.Help = item.Help
                item2.Switch = item.Switch
                item2.Param = item
                item2.Text = item.Text
                Items.Add(item2)
            End If
        Next

        For Each i In flowPanels
            i.ResumeLayout()
        Next
    End Sub

    Class Item
        Property Page As SimpleUI.FlowPage
        Property Control As Control
        Property Switch As String = ""
        Property Help As String = ""
        Property Text As String = ""
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
        If cbGoTo.Visible Then f.Doc.WriteP("The Search input field can be used to search for options, it searches in the switch, the label and the help. Multiple matches can be cycled by pressing enter.")
        f.Doc.WriteP("Numeric values and options can easily be reset to their default value by double clicking on the label. The default value for boolean and any other value can be found in the tooltip which can be shown by right-clicking the label.")
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
                If item.Switch?.ToLower?.Contains(find) Then matchedItems.Add(item)
                If item.Help?.ToLower?.Contains(find) Then matchedItems.Add(item)
                If item.Text?.ToLower?.Contains(find) Then matchedItems.Add(item)

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

    Private LastHighlightedControl As Control

    Sub Highlight(c As Control)
        If Not LastHighlightedControl Is Nothing Then
            LastHighlightedControl.Font = New Font(c.Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Regular)
            LastHighlightedControl.ForeColor = Color.Black
        End If

        c.Font = New Font(c.Font.FontFamily, 11 * s.UIScaleFactor, FontStyle.Bold)
        c.ForeColor = ControlPaint.Dark(ToolStripRendererEx.ColorBorder, 0.1)
        LastHighlightedControl = c
        ResetFontAsync(c)
    End Sub

    Async Sub ResetFontAsync(c As Control)
        Await Task.Run(Sub() Thread.Sleep(1000))

        If Not c.IsDisposed AndAlso Not c.Disposing Then
            c.Font = New Font(c.Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Regular)
            c.ForeColor = Color.Black
        End If
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

                If i.Switch <> "" AndAlso Not cbGoTo.Items.Contains(i.Switch) Then
                    cbGoTo.Items.Add(i.Switch)
                End If
            End If
        Next
    End Sub

    Private Sub CommandLineForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        UpdateHeight()
    End Sub
End Class