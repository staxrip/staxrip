
Imports System.Text
Imports System.Threading
Imports System.Threading.Tasks
Imports StaxRip.UI

Public Class TaskDialog(Of T)
    Inherits TaskDialogBaseForm

    Property CommandDefinitions As New List(Of CommandDefinition)
    Property ButtonDefinitions As New List(Of ButtonDefinition)
    Property SelectedValue As T
    Property SelectedText As String
    Property Title As String
    Property Timeout As Integer
    Property TimeoutButton As ButtonEx
    Property Symbol As Symbol
    Property Content As String
    Property ContentLabel As LabelEx
    Property ExpandedContent As String
    Property ExpandedContentLabel As LabelEx

    Overloads Property Icon As TaskIcon
    Overloads Property Owner As IntPtr


    Public Sub New()
        MyBase.New()
    End Sub

    Public Sub New(defaultValue As T)
        MyClass.New()
        SelectedValue = defaultValue
    End Sub

    Public Sub New(defaultText As String, defaultValue As T)
        MyClass.New(defaultValue)
        SelectedText = defaultText
    End Sub


    Sub Init()
        ShowInTaskbar = False
        Width = FontHeight * 35

        Content = If(Content = "", " ", Content)
        Title = If(Title = "", " ", Title)

        If Content = " " AndAlso Title?.Length > 80 Then
            Content = Title
            Title = " "
        End If

        If MenuButton.Items.Count > 0 Then
            MenuButton.Visible = True

            For Each i In MenuButton.Items
                Dim textWidth = TextRenderer.MeasureText(i.ToString, Font).Width + FontHeight * 3

                If MenuButton.Width < textWidth Then
                    Width += textWidth - MenuButton.Width
                End If
            Next
        End If

        If Content?.Length > 1000 OrElse ExpandedContent?.Length > 1000 Then
            Width = FontHeight * 46
        End If

        ShowIcon = False
        StartPosition = FormStartPosition.CenterScreen

        If Icon <> TaskIcon.None OrElse Symbol <> Symbol.None Then
            pbIcon.Visible = True
        End If

        Select Case Icon
            Case TaskIcon.Warning
                pbIcon.Image = StockIcon.GetImage(StockIconIdentifier.Warning)
            Case TaskIcon.Error
                pbIcon.Image = StockIcon.GetImage(StockIconIdentifier.Error)
            Case TaskIcon.Info
                pbIcon.Image = StockIcon.GetImage(StockIconIdentifier.Info)
            Case TaskIcon.Shield
                pbIcon.Image = StockIcon.GetImage(StockIconIdentifier.Shield)
            Case TaskIcon.Question
                pbIcon.Image = StockIcon.GetImage(StockIconIdentifier.Help)
        End Select

        If Symbol <> Symbol.None Then
            pbIcon.Image = ImageHelp.GetSymbolImage(Symbol, Nothing, 20)
        End If

        TitleLabel.Font = FontManager.GetDefaultFont(11)
        TitleLabel.Text = Title

        Dim firstCommandButton As CommandButton = Nothing

        Try
            paMain.SuspendLayout()

            If Content <> "" Then
                ContentLabel = New LabelEx With {
                    .Margin = New Padding(0),
                    .BorderStyle = BorderStyle.None,
                    .Text = Content,
                    .Name = "Information"
                }
                paMain.Controls.Add(ContentLabel)
            End If

            If ExpandedContent <> "" Then
                ExpandedContentLabel = New LabelEx With {
                    .Margin = New Padding(0),
                    .BorderStyle = BorderStyle.None,
                    .Text = ExpandedContent,
                    .Name = "ExpandedInformation"
                }
                blDetails.Visible = True
                paMain.Controls.Add(ExpandedContentLabel)
            End If

            For Each command In CommandDefinitions
                Dim cb As New CommandButton With {
                    .Title = command.Text,
                    .Description = command.Description,
                    .Tag = command
                }

                If TypeOf command.Value Is FontFamily Then
                    cb.ReplaceFontFamily(FontManager.GetFontFamily(FontCategory.All, command.Text))
                End If

                AddHandler cb.Click, AddressOf CommandClick
                paMain.Controls.Add(cb)

                If firstCommandButton Is Nothing Then
                    firstCommandButton = cb
                End If
            Next

            For i = 0 To ButtonDefinitions.Count - 1
                Dim bd = ButtonDefinitions(i)

                If Not flpButtons.Visible Then
                    flpButtons.Visible = True
                    flpButtons.AutoSize = True
                End If

                Dim b As New ButtonEx With {
                    .Text = bd.Text,
                    .Tag = bd.Value,
                    .TabStop = True,
                    .TabIndex = i
                }

                If AcceptButton Is Nothing AndAlso bd.Text = "OK" Then
                    AcceptButton = b
                End If

                If bd.TimeoutButton AndAlso TimeoutButton Is Nothing Then
                    TimeoutButton = b
                End If

                flpButtons.Controls.Add(b)
                bd.Button = b
                AddHandler b.Click, AddressOf ButtonClick
            Next
        Finally
            paMain.ResumeLayout()
        End Try

        If Timeout > 0 Then
            Dim originalWindowTitle = Text
            Dim originalButtonText = ""
            Dim button = If(TimeoutButton, TryCast(AcceptButton, ButtonEx))

            If button IsNot Nothing Then
                originalButtonText = button.Text
            End If

            g.RunTask(Sub()
                          Dim buttonText = originalButtonText
                          Dim windowTitle = originalWindowTitle
                          Dim counter = Timeout

                          While counter > 0 AndAlso Not IsDisposingOrDisposed
                              If button IsNot Nothing AndAlso Not button.IsDisposed Then
                                  If button.InvokeRequired Then
                                      button.Invoke(Sub()
                                                 If button IsNot Nothing AndAlso Not button.IsDisposed Then
                                                     button.Text = $"{buttonText} ({counter})"
                                                 End If
                                             End Sub)
                                  Else
                                      button.Text = $"{buttonText} ({counter})"
                                  End If
                              Else
                                  If InvokeRequired Then
                                      Invoke(Sub() Text = $"{windowTitle} ({counter})")
                                  Else
                                      Text = $"{windowTitle} ({counter})"
                                  End If
                              End If
                              counter -= 1
                              Thread.Sleep(1000)
                          End While

                          If Not IsDisposingOrDisposed Then
                              Invoke(Sub()
                                         SelectedText = button.Text
                                         SelectedValue = DirectCast(button.Tag, T)
                                         Close()
                                     End Sub)
                          End If
                      End Sub)
        End If

        If TypeOf SelectedValue Is DialogResult Then
            If SelectedValue.Equals(DialogResult.None) Then
                If ButtonDefinitions.Where(Function(i) i.Value.Equals(DialogResult.No)).Any Then
                    SelectedValue = CType(CObj(DialogResult.No), T)
                End If

                If ButtonDefinitions.Where(Function(i) i.Value.Equals(DialogResult.Cancel)).Any Then
                    SelectedValue = CType(CObj(DialogResult.Cancel), T)
                End If
            End If

            Dim ok = ButtonDefinitions.Where(Function(i) i.Value.Equals(DialogResult.OK)).FirstOrDefault

            If ok IsNot Nothing Then
                ActiveControl = ok.Button
            End If

            If ActiveControl Is Nothing Then
                Dim yes = ButtonDefinitions.Where(Function(i) i.Value.Equals(DialogResult.Yes)).FirstOrDefault

                If yes IsNot Nothing Then
                    ActiveControl = yes.Button
                End If
            End If

            If TimeoutButton IsNot Nothing Then
                ActiveControl = TimeoutButton
            End If
        End If

        If ActiveControl Is Nothing Then
            ActiveControl = If(firstCommandButton Is Nothing, TitleLabel, DirectCast(firstCommandButton, Control))
        End If

        If Owner = IntPtr.Zero Then
            Owner = GetHandle()
        End If
    End Sub

    Sub AddCommand(value As T)
        AddCommand(value.ToString, Nothing, value)
    End Sub

    Sub AddCommand(text As String, Optional value As T = Nothing)
        AddCommand(text, Nothing, value)
    End Sub

    Sub AddCommand(text As String, description As String, value As T)
        If value Is Nothing Then
            value = CType(CObj(text), T)
        End If

        CommandDefinitions.Add(New CommandDefinition With {.Text = text, .Description = description, .Value = value})
    End Sub

    Sub AddCommands(values As IEnumerable(Of T))
        For Each i In values
            AddCommand(i)
        Next
    End Sub

    Sub AddButton(text As String, Optional timeoutButton As Boolean = False)
        AddButton(text, CType(CObj(text), T), timeoutButton)
    End Sub

    Sub AddButton(value As T, Optional timeoutButton As Boolean = False)
        AddButton(value.ToString, value, timeoutButton)
    End Sub

    Sub AddButton(text As String, value As T, Optional timeoutButton As Boolean = False)
        ButtonDefinitions.Add(New ButtonDefinition With {.Text = text, .Value = value, .TimeoutButton = timeoutButton})
    End Sub

    Sub AddButtons(values As IEnumerable(Of T))
        For Each i In values
            AddButton(i)
        Next
    End Sub

    WriteOnly Property Buttons As TaskButton
        Set(value As TaskButton)
            For Each i In {TaskButton.OK, TaskButton.Yes, TaskButton.No,
                TaskButton.Cancel, TaskButton.Retry, TaskButton.Close}

                If value.HasFlag(i) Then
                    AddButton(i.ToString, CType(CObj(GetDialogResultFromButton(i)), T))
                End If
            Next
        End Set
    End Property

    Sub CommandClick(sender As Object, e As EventArgs)
        Dim tag = DirectCast(sender, CommandButton).Tag
        Dim cmd = DirectCast(tag, CommandDefinition)
        SelectedText = If(cmd.Text = "", cmd.Description, cmd.Text)
        SelectedValue = cmd.Value
        Close()
    End Sub

    Sub ButtonClick(sender As Object, e As EventArgs)
        Dim button = DirectCast(sender, ButtonEx)
        SelectedText = button.Text
        SelectedValue = DirectCast(button.Tag, T)
        Close()
    End Sub

    Overrides Sub AdjustSize()
        Dim h = tlpTop.Height + tlpTop.Margin.Vertical

        If paMain.Controls.Count > 0 Then
            Dim last = paMain.Controls(paMain.Controls.Count - 1)
            h += last.Top + last.Height + last.Margin.Vertical
        End If

        h += spBottom.Height

        If spBottom.Controls.OfType(Of Control).Where(Function(i) i.Visible).Count > 0 Then
            h += spBottom.Margin.Vertical
        End If

        Dim fh = FontHeight
        h += CInt(fh * 0.7)

        Dim nonClientHeight = Height - ClientSize.Height
        Dim workingArea = Screen.FromControl(Me).WorkingArea
        Dim maxHeight = workingArea.Height
        Dim w = ClientSize.Width
        Dim secondLongestLine = GetSecondLongestLineLength()
        Dim predictedWidth = CInt(secondLongestLine * fh * 0.45)

        If predictedWidth > Width Then
            w = predictedWidth
        End If

        If w > fh * 40 Then
            w = fh * 40
        End If

        Dim ncx = Width - ClientSize.Width
        Dim ncy = Height - ClientSize.Height

        w += ncx
        h += ncy

        If h > maxHeight Then
            h = maxHeight
        End If

        Dim l = (workingArea.Width - w) \ 2
        Dim t = (workingArea.Height - h) \ 2

        Native.SetWindowPos(Handle, IntPtr.Zero, l, t, w, h, 64)
    End Sub

    Function GetSecondLongestLineLength() As Integer
        Dim list As New List(Of Integer)({51, 52})

        If Content <> "" Then
            For Each line In Content.Split(BR(1))
                list.Add(line.Length)
            Next
        End If

        If ExpandedContent <> "" AndAlso ExpandedContentLabel?.Height > 0 Then
            For Each line In ExpandedContent.Split(BR(1))
                list.Add(line.Length)
            Next
        End If

        For Each def In CommandDefinitions
            If def.Description <> "" Then
                For Each line In def.Description.Split(BR(1))
                    list.Add(line.Length)
                Next
            End If
        Next

        For Each def In CommandDefinitions
            If def.Text <> "" Then
                For Each line In def.Text.Split(BR(1))
                    list.Add(CInt(line.Length / 11 * 9))
                Next
            End If
        Next

        list.Sort()
        list.Reverse()
        Return list(1)
    End Function

    WriteOnly Property ShowCopyButton As Boolean
        Set(value As Boolean)
            If value Then
                blCopyMessage.Text = "Copy Message"
                blCopyMessage.Visible = True
                blCopyMessage.ClickAction = Sub()
                                                g.RunSTATask(Sub()
                                                                 Clipboard.SetText(GetText)
                                                                 MsgInfo("Message was copied to clipboard.")
                                                             End Sub)
                                            End Sub
            End If
        End Set
    End Property

    Function GetHandle() As IntPtr
        Dim sb As New StringBuilder(500)
        Dim handle = Native.GetForegroundWindow
        Native.GetWindowModuleFileName(handle, sb, CUInt(sb.Capacity))

        If sb.ToString.Replace(".vshost", "").Base = Application.ExecutablePath.Base Then
            Return handle
        End If
    End Function

    Function GetText() As String
        Dim ret = TitleLabel.Text

        If Content <> "" Then
            ret += BR2 + Content
        End If

        If ExpandedContent <> "" Then
            ret += BR2 + ExpandedContent
        End If

        Return ret
    End Function

    Shared Function GetDialogResultFromButton(button As TaskButton) As DialogResult
        Select Case button
            Case TaskButton.OK
                Return DialogResult.OK
            Case TaskButton.Cancel, TaskButton.Close
                Return DialogResult.Cancel
            Case TaskButton.Yes
                Return DialogResult.Yes
            Case TaskButton.No
                Return DialogResult.No
            Case TaskButton.None
                Return DialogResult.None
            Case TaskButton.Retry
                Return DialogResult.Retry
        End Select
    End Function

    Protected Overrides Sub OnLoad(args As EventArgs)
        MyBase.OnLoad(args)
        Font = FontManager.GetDefaultFont()
        Dim fh = FontHeight

        For Each i As ButtonEx In flpButtons.Controls
            i.Height = CInt(fh * 1.5)
            i.Width = fh * 5

            Using g = i.CreateGraphics
                Dim minWidth = CInt(g.MeasureString(i.Text, i.Font).Width + fh)

                If i.Width < minWidth Then
                    i.Width = minWidth
                End If
            End Using

            i.Margin = New Padding(CInt(fh * 0.4), 0, 0, 0)
        Next

        MenuButton.Margin = New Padding(CInt(fh * 0.7), MenuButton.Margin.Top, CInt(fh * 0.7), MenuButton.Margin.Bottom)
        InputTextEdit.Margin = MenuButton.Margin
        flpButtons.Margin = New Padding(0, 0, CInt(fh * 0.7), 0)

        If InputTextEdit.Visible Then
            ActiveControl = InputTextEdit
            InputTextEdit.TextBox.SelectAll()
        End If

        AdjustSize()
        AdjustSize()

        If Owner <> IntPtr.Zero Then
            Dim GWLP_HWNDPARENT = -8
            SetWindowLongPtr(Handle, GWLP_HWNDPARENT, Owner)
        End If
    End Sub

    Overloads Function Show() As T
        Init()

        If Application.MessageLoop Then
            Using Me
                ShowDialog()
            End Using
        Else
            Application.Run(Me)
        End If

        Return SelectedValue
    End Function

    Public Class CommandDefinition
        Property Text As String
        Property Description As String
        Property Value As T
    End Class

    Public Class ButtonDefinition
        Property Text As String
        Property Value As T
        Property TimeoutButton As Boolean
        Property Button As ButtonEx
    End Class
End Class

Public Enum TaskIcon
    None
    Info
    Warning
    Question
    [Error]
    Shield
End Enum

Public Enum TaskButton
    None = 0
    OK = 1
    Yes = 2
    No = 4
    Cancel = 8
    Retry = 16
    Close = 32
    OkCancel = OK Or Cancel
    YesNo = Yes Or No
    YesNoCancel = YesNo Or Cancel
    RetryCancel = Retry Or Cancel
End Enum
