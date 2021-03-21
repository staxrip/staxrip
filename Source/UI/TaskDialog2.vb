
Imports System.Text
Imports System.Threading

Imports StaxRip.UI

Public Class TaskDialog2(Of T)
    Inherits TaskDialogForm

    Property Commands As New List(Of CommandDefinition)
    Property Buttons As New List(Of ButtonDefinition)
    Property SelectedValue As T
    Property SelectedText As String
    Property Title As String
    Property Content As String
    Property ExpandedContent As String
    Property Timeout As Integer

    Overloads Property Icon As TaskIcon
    Overloads Property Owner As IntPtr

    Private tbContent As TextBox
    Private tbExpandedContent As TextBox

    Shared Function Test() As Boolean
        'Return False

        'Dim nativeTaskDialog As New TaskDialog(Of String)
        'nativeTaskDialog.MainInstruction = "Hallo"
        'nativeTaskDialog.MainIcon = TaskDialogIcon.Info
        'nativeTaskDialog.Content = ("bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb" + BR).Multiply(2)
        ''nativeTaskDialog.Content = ("bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb" + BR).Multiply(40)
        ''nativeTaskDialog.ExpandedInformation = ("bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb" + BR).Multiply(40)
        ''nativeTaskDialog.AddCommand("AAAAA")
        ''nativeTaskDialog.AddCommand("BBBBB", "bbbbbbb bbbbbbbb bbbbbbb", "")
        ''nativeTaskDialog.VerificationText = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa"
        'nativeTaskDialog.Show()
        'Return True

        'Dim ret = MsgQuestion("aaaaa ".Multiply(20), ("bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb" + BR).Multiply(10))
        'Debug.WriteLine(ret)
        'Return True

        'Application.Run(New TestForm)
        'Return True

        Dim td As New TaskDialog2(Of DialogResult)
        td.Icon = TaskIcon.Question

        td.Title = "Hallo " '.Multiply(20)
        td.Content = "Hi there ".Multiply(150)
        td.ExpandedContent = "Hossa ".Multiply(150)
        td.AddButton("OK", DialogResult.Retry)
        td.AddButton("Cancel", DialogResult.Retry)

        For x = 0 To 20
            td.AddCommand(x.ToString)
        Next

        'td.AddCommand("Hallo 1 ".Multiply(5))
        'td.AddCommand("", "Description ".Multiply(5), "")
        'td.AddCommand("Hallo 3".Multiply(5), "Ha Ha Ha ".Multiply(5), "")

        'td.MainInstruction = "Save changed project?"


        'Dim td As New TaskDialog2(Of DialogResult)
        ''td.MainInstruction = "Hallo ".Multiply(20)
        'td.Content = "Hi there"

        'td.AddCommand("Hallo 1 ".Multiply(5))
        'td.AddCommand("", "Description ".Multiply(5), "")
        'td.AddCommand("Hallo 3".Multiply(5), "Ha Ha Ha ".Multiply(5), "")

        'td.MainInstruction = "Save changed project?"

        'td.AddButton("Save", DialogResult.Yes)
        'td.AddButton("Don't Save !!!!!!!!!!", DialogResult.No)
        'td.AddButton("Cancel", DialogResult.Cancel)

        'td.AddCommand("Hi there", "What's up?", "")
        td.Show()
        'MsgInfo(td.SelectedText)

        Return True
    End Function

    Sub Init()
        If Icon <> TaskIcon.None Then
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

        laMainInstruction.Font = New Font("Segoe UI", 12)
        laMainInstruction.Text = Title

        If Content <> "" Then
            tbContent = New TextBox
            tbContent.BackColor = Theme.General.BackColor
            tbContent.ForeColor = Theme.General.Controls.Label.ForeColor
            tbContent.ReadOnly = True
            tbContent.Multiline = True
            tbContent.HideSelection = True
            tbContent.Margin = New Padding(0)
            tbContent.BorderStyle = BorderStyle.None
            tbContent.Text = Content
            paMain.Controls.Add(tbContent)
        End If

        If ExpandedContent <> "" Then
            tbExpandedContent = New TextBox
            tbExpandedContent.BackColor = Theme.General.BackColor
            tbExpandedContent.ForeColor = Theme.General.Controls.Label.ForeColor
            tbExpandedContent.ReadOnly = True
            tbExpandedContent.Multiline = True
            tbExpandedContent.HideSelection = True
            tbExpandedContent.Margin = New Padding(0)
            tbExpandedContent.BorderStyle = BorderStyle.None
            tbExpandedContent.Text = ExpandedContent
            tbExpandedContent.Name = "ExpandedInformation"
            blDetails.Visible = True
            paMain.Controls.Add(tbExpandedContent)
        End If

        For Each command In Commands
            Dim cb As New CommandButton
            cb.Title = command.Text
            cb.Description = command.Description
            cb.Tag = command
            cb.BackColor = Theme.ProcessingForm.ProcessButtonBackColor
            cb.ForeColor = Theme.ProcessingForm.ProcessButtonForeColor
            cb.FlatAppearance.BorderColor = Theme.General.BackColor
            AddHandler cb.Click, AddressOf CommandClick
            paMain.Controls.Add(cb)
        Next

        For Each i In Buttons
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
                If Buttons.Where(Function(i) i.Value.Equals(DialogResult.No)).Any Then
                    SelectedValue = CType(CObj(DialogResult.No), T)
                End If

                If Buttons.Where(Function(i) i.Value.Equals(DialogResult.Cancel)).Any Then
                    SelectedValue = CType(CObj(DialogResult.Cancel), T)
                End If
            End If

            Dim ok = Buttons.Where(Function(i) i.Value.Equals(DialogResult.OK)).FirstOrDefault

            If Not ok Is Nothing Then
                ActiveControl = ok.Button
            End If

            If ActiveControl Is Nothing Then
                Dim yes = Buttons.Where(Function(i) i.Value.Equals(DialogResult.Yes)).FirstOrDefault

                If Not yes Is Nothing Then
                    ActiveControl = yes.Button
                End If
            End If
        End If

        If ActiveControl Is Nothing Then
            ActiveControl = laMainInstruction
        End If

        If Owner = IntPtr.Zero Then
            Owner = GetHandle()
        End If

        ShowInTaskbar = False
        Text = Application.ProductName
        Font = New Font("Segoe UI", 9)
        Width = FontHeight * 22
        ShowIcon = False
        StartPosition = FormStartPosition.CenterScreen
    End Sub

    Sub AddCommand(text As String, Optional value As T = Nothing)
        AddCommand(text, Nothing, value)
    End Sub

    Sub AddCommand(text As String, description As String, value As T)
        If value Is Nothing Then
            value = CType(CObj(text), T)
        End If

        Commands.Add(New CommandDefinition With {.Text = text, .Description = description, .Value = value})
    End Sub

    Sub AddButton(text As String)
        AddButton(text, CType(CObj(text), T))
    End Sub

    Sub AddButton(text As String, value As T)
        Buttons.Add(New ButtonDefinition With {.Text = text, .Value = value})
    End Sub

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
            w = FontHeight * 35
        End If

        ClientSize = New Size(w, h)
        CenterScreen
    End Sub

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
        Dim ret = laMainInstruction.Text

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

        flpButtons.Margin = New Padding(0, 0, CInt(fh * 0.7), 0)

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