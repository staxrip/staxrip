Imports System.Threading
Imports System.Threading.Tasks
Imports System.Globalization

Imports StaxRip.CommandLine
Imports StaxRip.UI

Class CommandLineForm
    Private Params As CommandLineParams

    Public HTMLHelp As String

    Public Sub New(params As CommandLineParams)
        InitializeComponent()
        Me.Params = params
        Text = params.Title
        InitUI()
        SimpleUI.SelectLast(params.Title + "page selection")
        AddHandler params.ValueChanged, AddressOf ValueChanged
        params.RaiseValueChanged(Nothing)

        cbGoTo.Sorted = True
        cbGoTo.SendMessageCue("Search")
        cbGoTo.Select()

        cms.Items.Add(New ActionMenuItem("Execute Command Line", Sub() g.ShellExecute(params.GetPackage.GetPath, params.GetArgs(True)), Nothing, p.SourceFile <> ""))
        cms.Items.Add(New ActionMenuItem("Copy Command Line", Sub() Clipboard.SetText("""" + params.GetPackage.GetPath + """ " + params.GetArgs(True))))
        cms.Items.Add(New ActionMenuItem("Help", Sub() ShowHelp()))
        cms.Items.Add(New ActionMenuItem(params.GetPackage.Name + " Help", Sub() g.ShellExecute(params.GetPackage.GetHelpPath)))

        UpdateSearchComboBox()
    End Sub

    Sub ValueChanged(item As CommandLineItem)
        rtbCmdl.SetText(Params.GetArgs(False))
        rtbCmdl.SelectionLength = 0
        UpdateHeight()
    End Sub

    Sub UpdateHeight()
        Dim s = TextRenderer.MeasureText(rtbCmdl.Text, rtbCmdl.Font,
                                         New Size(rtbCmdl.ClientSize.Width, Integer.MaxValue),
                                         TextFormatFlags.WordBreak)
        Height += CInt(s.Height * 1.2) - rtbCmdl.Height
        rtbCmdl.Refresh()
    End Sub

    Sub InitUI()
        Dim groupPanel As FlowLayoutPanelEx
        Dim lastGroup As String
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

            If item.Group <> "" Then
                If item.Group <> lastGroup Then
                    groupPanel = SimpleUI.AddEmptyBlock(parent)
                    groupPanel.Margin = New Padding With {.Top = 3, .Bottom = 3}
                End If

                lastGroup = item.Group
                parent = groupPanel
            Else
                lastGroup = Nothing
                groupPanel = Nothing
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
                    help += CrLf2 + "Minimum: " & param.MinMaxStepDec(0) & CrLf + "Maximum: " & param.MinMaxStepDec(1)
                ElseIf TypeOf item Is OptionParam Then
                    Dim param = DirectCast(item, OptionParam)
                    help += CrLf2 + "Default: " + param.Options(param.DefaultValue)
                ElseIf TypeOf item Is StringParam Then
                    help += CrLf2 + "Default: " & DirectCast(item, StringParam).DefaultValue
                End If

                If item.Help <> "" Then help += CrLf2 + item.Help
            End If

            If TypeOf item Is BoolParam Then
                Dim cb = SimpleUI.AddCheckBox(parent)
                cb.Margin = New Padding(3) With {.Left = 9}
                cb.Text = item.Text
                cb.Tooltip = help
                DirectCast(item, BoolParam).Init(cb)
                helpControl = cb
            ElseIf TypeOf item Is NumParam Then
                Dim tempItem = DirectCast(item, NumParam)

                'TODO: remove?
                If item.Group <> "" Then
                    'Dim param = DirectCast(item, NumParam)
                    'Dim l = SimpleUI.AddLabel(parent, item.Text)
                    'AddHandler l.MouseDoubleClick, Sub() tempItem.Value = tempItem.DefaultValue
                    'l.Margin = item.LabelMargin
                    'l.Tooltip = help
                    'helpControl = l
                    'Dim num = SimpleUI.AddNumeric(parent)
                    'num.Margin = New Padding With {.Right = FontHeight}
                    'num.Init(param.MinMaxStepDec)
                    'param.Init(num)
                Else
                    Dim param = DirectCast(item, NumParam)
                    Dim nb = SimpleUI.AddNumericBlock(parent)
                    nb.Label.Text = item.Text
                    nb.Label.Tooltip = help
                    nb.NumEdit.Init(param.MinMaxStepDec)
                    AddHandler nb.Label.MouseDoubleClick, Sub() tempItem.Value = tempItem.DefaultValue
                    DirectCast(item, NumParam).Init(nb.NumEdit)
                    helpControl = nb.Label
                End If
                'ElseIf TypeOf item Is CheckedNumParam Then
                '    Dim tempItem = DirectCast(item, CheckedNumParam)

                '    Dim nb = SimpleUI.AddCheckedNumericBlock(parent)
                '    nb.CheckBox.Margin = New Padding(3) With {.Left = 9}
                '    nb.CheckBox.Text = item.Text
                '    nb.CheckBox.Tooltip = help
                '    nb.NumEdit.Init(tempItem.MinMaxStepDec)
                '    DirectCast(item, CheckedNumParam).Init(nb.CheckBox, nb.NumEdit)
                '    helpControl = nb.CheckBox
            ElseIf TypeOf item Is OptionParam Then
                Dim tempItem = DirectCast(item, OptionParam)
                Dim os = DirectCast(item, OptionParam)
                Dim mb = SimpleUI.AddMenuButtonBlock(Of Integer)(parent)
                mb.Label.Text = item.Text
                mb.Tooltip = help
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
                sp.Init(tb.Edit)
            End If

            If item.Switch <> "" AndAlso Not helpControl Is Nothing Then
                SwitchControlDic(item.Switch) = helpControl
                ControlFlowDic(helpControl) = currentFlow
                ControlHelpDic(helpControl) = item.Help
            End If
        Next

        For Each i In flowPanels
            i.ResumeLayout()
        Next
    End Sub

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

    Private SearchIndex As Integer
    Private SwitchControlDic As New Dictionary(Of String, Control)
    Private ControlFlowDic As New Dictionary(Of Control, SimpleUI.FlowPage)
    Private ControlHelpDic As New Dictionary(Of Control, String)

    Private Sub cbGoTo_KeyDown(sender As Object, e As KeyEventArgs) Handles cbGoTo.KeyDown
        If e.KeyData = Keys.Enter Then
            SearchIndex += 1
            cbGoTo_TextChanged(Nothing, Nothing)
        Else
            SearchIndex = 0
        End If
    End Sub

    Private Sub cbGoTo_TextChanged(sender As Object, e As EventArgs) Handles cbGoTo.TextChanged
        Dim q = cbGoTo.Text.ToLower

        Dim matchedControls As New List(Of Control)

        If q.Length > 1 Then
            For Each i In SwitchControlDic
                If i.Key.ToLower.Contains(q) Then
                    matchedControls.Add(i.Value)
                End If
            Next

            For Each i In ControlHelpDic
                If i.Value <> "" AndAlso i.Value.ToLower.Contains(q) AndAlso
                    Not matchedControls.Contains(i.Key) Then

                    matchedControls.Add(i.Key)
                End If
            Next

            If matchedControls.Count > 0 Then
                If SearchIndex >= matchedControls.Count Then
                    SearchIndex = 0
                End If

                ShowControl(matchedControls(SearchIndex))
            End If
        End If
    End Sub

    Sub ShowControl(c As Control)
        Dim flow = ControlFlowDic(c)
        SimpleUI.ShowPage(flow)
        c.Font = New Font(c.Font, FontStyle.Bold)
        c.ForeColor = ControlPaint.Dark(ToolStripRendererEx.ColorBorder, 0.1)
        ResetFontAsync(c)
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

        For Each i In SwitchControlDic
            If cbGoTo.Text = "" OrElse SwitchControlDic.ContainsKey(cbGoTo.Text) OrElse
                i.Key.ToLower.Contains(cbGoTo.Text.ToLower) Then

                cbGoTo.Items.Add(i.Key)
            End If
        Next

        If cbGoTo.Items.Count < 20 Then cbGoTo.Visible = False
    End Sub

    Private Sub CommandLineForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        UpdateHeight()
    End Sub
End Class