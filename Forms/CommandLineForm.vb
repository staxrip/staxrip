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
        Me.Params = params
        Text = params.Title
        InitUI()

        If SimpleUI.Tree.Nodes.Count > 10 Then
            SimpleUI.Tree.ItemHeight = CInt(SimpleUI.Tree.Height / SimpleUI.Tree.Nodes.Count) - 3
        End If

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

    Sub ValueChanged(item As CommandLineItem)
        rtbCmdl.SetText(Params.GetCommandLine(False, False))
        rtbCmdl.SelectionLength = 0
        UpdateHeight()
        UpdateSearchComboBox()
    End Sub

    Sub UpdateHeight()
        Dim s = TextRenderer.MeasureText(rtbCmdl.Text, rtbCmdl.Font,
                                         New Size(rtbCmdl.ClientSize.Width, Integer.MaxValue),
                                         TextFormatFlags.WordBreak)
        Height += CInt(s.Height * 1.2) - rtbCmdl.Height
        rtbCmdl.Refresh()
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
                    help = item.Switch + CrLf2 + item.NoSwitch
                Else
                    help = item.Switch
                End If

                If TypeOf item Is BoolParam Then
                    If DirectCast(item, BoolParam).DefaultValue Then
                        help += CrLf2 + "Default: enabled"
                    Else
                        help += CrLf2 + "Default: disabled"
                    End If
                ElseIf TypeOf item Is NumParam Then
                    Dim param = DirectCast(item, NumParam)
                    help += CrLf2 + "Default: " & param.DefaultValue.ToString(CultureInfo.InvariantCulture)
                    If param.MinMaxStepDec(0) <> Decimal.MinValue Then help += CrLf + "Minimum: " & param.MinMaxStepDec(0)
                    If param.MinMaxStepDec(1) <> Decimal.MaxValue Then help += CrLf + "Maximum: " & param.MinMaxStepDec(1)
                ElseIf TypeOf item Is OptionParam Then
                    Dim param = DirectCast(item, OptionParam)

                    If param.DefaultValue >= 0 AndAlso param.DefaultValue < param.Options.Length Then
                        help += CrLf2 + "Default: " + param.Options(param.DefaultValue)
                    End If
                ElseIf TypeOf item Is StringParam Then
                        help += CrLf2 + "Default: " & DirectCast(item, StringParam).DefaultValue
                End If

                If item.Help <> "" Then help += CrLf2 + item.Help
                If item.URL <> "" Then help += CrLf2 + "[" + item.URL + " " + item.URL + "]"
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

                If os.Expand Then
                    mb.Expand(mb.MenuButton)
                End If

                For x2 = 0 To os.Options.Length - 1
                    mb.MenuButton.Add(os.Options(x2), x2)
                Next

                os.Init(mb.MenuButton)
            ElseIf TypeOf item Is StringParam Then
                Dim tempItem = DirectCast(item, StringParam)
                Dim tb = SimpleUI.AddTextBlock(parent)
                tb.Label.Text = item.Text
                tb.Label.Tooltip = help
                helpControl = tb.Label
                AddHandler tb.Label.MouseDoubleClick, Sub() tempItem.Value = tempItem.DefaultValue
                tb.Expand(tb.Edit)
                Dim sp = DirectCast(item, StringParam)
                sp.Init(tb)
            End If

            Dim listText = item.Switch
            If listText = "" AndAlso item.Text <> "" Then listText = item.Text.Trim(" "c, ":"c)

            If listText <> "" AndAlso Not helpControl Is Nothing Then
                Dim item2 As New Item
                item2.Control = helpControl
                item2.Page = currentFlow
                If item.Help <> "" Then item2.Help = item.Help
                If listText <> "" Then item2.Text = listText
                item2.Item = item
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
        Property Text As String = ""
        Property Help As String = ""
        Property Item As CommandLineItem
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

        Dim matchedItems As New List(Of Item)

        If find.Length > 1 Then
            For Each i In Items
                If i.Text.ToLower.Contains(find) Then
                    matchedItems.Add(i)
                End If
            Next

            For Each i In Items
                If i.Help <> "" AndAlso i.Help.ToLower.Contains(find) AndAlso
                    Not matchedItems.Contains(i) Then

                    matchedItems.Add(i)
                End If
            Next

            Dim visibleItems = matchedItems.Where(Function(arg) arg.Item.Visible)

            If visibleItems.Count > 0 Then
                If SearchIndex >= visibleItems.Count Then SearchIndex = 0
                Dim control = visibleItems(SearchIndex).Control
                SimpleUI.ShowPage(visibleItems(SearchIndex).Page)

                control.Font = New Font(control.Font, FontStyle.Bold)
                control.ForeColor = ControlPaint.Dark(ToolStripRendererEx.ColorBorder, 0.1)
                ResetFontAsync(control)
            End If
        End If
    End Sub

    Async Sub ResetFontAsync(c As Control)
        Await Task.Run(Sub() Thread.Sleep(1000))

        If Not c.IsDisposed AndAlso Not c.Disposing Then
            c.Font = New Font(c.Font, FontStyle.Regular)
            c.ForeColor = Color.Black
        End If
    End Sub

    Sub UpdateSearchComboBox()
        cbGoTo.Items.Clear()

        For Each i In Items
            If i.Item.Visible AndAlso Not cbGoTo.Items.Contains(i.Text) Then
                cbGoTo.Items.Add(i.Text)
            End If
        Next
    End Sub

    Private Sub CommandLineForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        UpdateHeight()
    End Sub
End Class