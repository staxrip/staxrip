
Imports System.Text
Imports System.Threading

Imports StaxRip.UI

Public Class TaskDialog(Of T)
    Inherits TaskDialogBaseForm

    Property CommandDefinitions As New List(Of CommandDefinition)
    Property ButtonDefinitions As New List(Of ButtonDefinition)
    Property SelectedValue As T
    Property SelectedText As String
    Property Title As String
    Property Content As String
    Property ExpandedContent As String
    Property Timeout As Integer
    Property Symbol As Symbol

    Overloads Property Icon As TaskIcon
    Overloads Property Owner As IntPtr

    Private tbContent As Label
    Private tbExpandedContent As Label

    Sub Init()
        ShowInTaskbar = False
        Text = Application.ProductName
        Font = New Font("Segoe UI", 9)
        Width = FontHeight * 22

        If Content = "" AndAlso Title?.Length > 80 Then
            Content = Title
            Title = ""
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
            Width = FontHeight * 35
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

        TitleLabel.Font = New Font("Segoe UI", 11)
        TitleLabel.Text = Title

        If Content <> "" Then
            tbContent = New Label
            tbContent.BackColor = Theme.General.BackColor
            tbContent.ForeColor = Theme.General.Controls.Label.ForeColor
            tbContent.Margin = New Padding(0)
            tbContent.BorderStyle = BorderStyle.None
            tbContent.Text = Content
            paMain.Controls.Add(tbContent)
        End If

        If ExpandedContent <> "" Then
            tbExpandedContent = New Label
            tbExpandedContent.BackColor = Theme.General.BackColor
            tbExpandedContent.ForeColor = Theme.General.Controls.Label.ForeColor
            tbExpandedContent.Margin = New Padding(0)
            tbExpandedContent.BorderStyle = BorderStyle.None
            tbExpandedContent.Text = ExpandedContent
            tbExpandedContent.Name = "ExpandedInformation"
            blDetails.Visible = True
            paMain.Controls.Add(tbExpandedContent)
        End If

        Dim firstCommandButton As CommandButton

        For Each command In CommandDefinitions
            Dim cb As New CommandButton
            cb.Title = command.Text
            cb.Description = command.Description
            cb.Tag = command
            cb.BackColor = Theme.ProcessingForm.ProcessButtonBackColor
            cb.ForeColor = Theme.ProcessingForm.ProcessButtonForeColor
            cb.FlatAppearance.BorderColor = Theme.General.BackColor

            If TypeOf command.Value Is FontFamily Then
                cb.Font = New Font(command.Text, FontHeight)
            End If

            AddHandler cb.Click, AddressOf CommandClick
            paMain.Controls.Add(cb)

            If firstCommandButton Is Nothing Then
                firstCommandButton = cb
            End If
        Next

        For Each i In ButtonDefinitions
            If Not flpButtons.Visible Then
                flpButtons.Visible = True
                flpButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink
                flpButtons.AutoSize = True
            End If

            Dim b As New ButtonEx
            b.Text = i.Text
            b.Tag = i.Value

            flpButtons.Controls.Add(b)
            i.Button = b
            AddHandler b.Click, AddressOf ButtonClick
        Next

        If Timeout > 0 Then
            g.RunTask(Sub()
                          Thread.Sleep(Timeout * 1000)

                          If Not IsDisposingOrDisposed Then
                              Invoke(Sub() Close())
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

            If Not ok Is Nothing Then
                ActiveControl = ok.Button
            End If

            If ActiveControl Is Nothing Then
                Dim yes = ButtonDefinitions.Where(Function(i) i.Value.Equals(DialogResult.Yes)).FirstOrDefault

                If Not yes Is Nothing Then
                    ActiveControl = yes.Button
                End If
            End If
        End If

        If ActiveControl Is Nothing Then
            If firstCommandButton Is Nothing Then
                ActiveControl = TitleLabel
            Else
                ActiveControl = firstCommandButton
            End If
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

    Sub AddButton(text As String)
        AddButton(text, CType(CObj(text), T))
    End Sub

    Sub AddButton(text As String, value As T)
        ButtonDefinitions.Add(New ButtonDefinition With {.Text = text, .Value = value})
    End Sub

    Sub AddButton(value As T)
        ButtonDefinitions.Add(New ButtonDefinition With {.Text = value.ToString, .Value = value})
    End Sub

    Sub AddButtons(values As IEnumerable(Of T))
        For Each i In values
            AddButton(i)
        Next
    End Sub

    WriteOnly Property Buttons As TaskButton
        Set(value As TaskButton)
            For Each i In {TaskButton.Ok, TaskButton.Yes, TaskButton.No,
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

    Overrides Sub AdjustHeight()
        Dim h = tlpTop.Height

        If paMain.Controls.Count > 0 Then
            Dim last = paMain.Controls(paMain.Controls.Count - 1)
            h += last.Top + last.Height
        End If

        h += spBottom.Height

        If spBottom.Controls.OfType(Of Control).Where(Function(i) i.Visible).Count > 0 Then
            h += spBottom.Margin.Vertical
        End If

        h += CInt(FontHeight * 0.7)

        Dim nonClientHeight = Height - ClientSize.Height
        Dim maxHeight = Screen.FromControl(Me).WorkingArea.Height
        Dim w = ClientSize.Width

        If (h + nonClientHeight) > maxHeight Then
            h = maxHeight - nonClientHeight
            Dim longestLine = GetMaxLineLength()
            Dim predictedWidth = CInt(longestLine * FontHeight * 0.5)

            If predictedWidth > Width Then
                w = predictedWidth
            End If

            Dim max = FontHeight * 40

            If w > max Then
                w = max
            End If
        End If

        If paMain.LineBreaks > 2 Then
            Dim longestLine = GetMaxLineLength()
            Dim predictedWidth = CInt(longestLine * FontHeight * 0.5)

            If predictedWidth > Width Then
                w = predictedWidth
            End If

            Dim max = FontHeight * 40

            If w > max Then
                w = max
            End If
        End If

        ClientSize = New Size(w, h)
        CenterScreen
    End Sub

    Function GetMaxLineLength() As Integer
        Dim ret As Integer

        For Each txt In {Title, Content, ExpandedContent}
            If txt <> "" Then
                For Each line In txt.Split(BR(1))
                    If line.Length > ret Then
                        ret = line.Length
                    End If
                Next
            End If
        Next

        For Each def In CommandDefinitions
            For Each txt In {def.Text, def.Description}
                If txt <> "" Then
                    For Each line In txt.Split(BR(1))
                        If line.Length > ret Then
                            ret = line.Length
                        End If
                    Next
                End If
            Next
        Next

        Return ret
    End Function

    WriteOnly Property ShowCopyButton As Boolean
        Set(value As Boolean)
            If value Then
                Button.Text = "Copy Message"
                Button.Visible = True
                Button.ClickAction = Sub()
                                         Clipboard.SetText(GetText)
                                         MsgInfo("Message was copied to clipboard.")
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
            Case TaskButton.Ok
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
        flpButtons.Margin = New Padding(0, 0, CInt(fh * 0.7), 0)

        AdjustHeight()
        AdjustHeight()

        If Owner <> IntPtr.Zero Then
            Dim GWLP_HWNDPARENT = -8
            SetWindowLongPtr(Handle, GWLP_HWNDPARENT, Owner)
        End If
    End Sub

    Overloads Function Show() As T
        If Application.MessageLoop Then
            Init()

            Using Me
                ShowDialog()
            End Using
        Else
            Init()
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
    Ok = 1
    Yes = 2
    No = 4
    Cancel = 8
    Retry = 16
    Close = 32
    OkCancel = Ok Or Cancel
    YesNo = Yes Or No
    YesNoCancel = YesNo Or Cancel
    RetryCancel = Retry Or Cancel
End Enum
