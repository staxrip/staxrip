
Imports StaxRip.UI

Public Class TaskDialog2(Of T)
    Inherits TaskDialogForm

    Property Commands As New List(Of Command)
    Property Buttons As New List(Of Button)
    Property tbContent As TextBox
    Property tbExpandedContent As TextBox
    Property SelectedValue As T
    Property SelectedText As String
    Property MainInstruction As String
    Property Content As String
    Property ExpandedInformation As String

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
        'nativeTaskDialog.Footer = "[copymsg: Copy Message]"
        'nativeTaskDialog.Show()
        'Return True

        MsgError("aaaaa ".Multiply(20), ("bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb" + BR).Multiply(10))
        Return True

        'Application.Run(New TestForm)
        'Return True

        Dim td As New TaskDialog2(Of String)
        'td.MainInstruction = "Hallo ".Multiply(20)
        td.Content = "Hi there"

        td.AddCommand("Hallo 1 ".Multiply(5))
        td.AddCommand("", "Description ".Multiply(5), "")
        td.AddCommand("Hallo 3".Multiply(5), "Ha Ha Ha ".Multiply(5), "")

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

        Return True
    End Function

    Sub Init()
        paIcon.Visible = False

        laMainInstruction.Font = New Font("Segoe UI", 12)
        laMainInstruction.Text = MainInstruction

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

        If ExpandedInformation <> "" Then
            tbExpandedContent = New TextBox
            tbExpandedContent.BackColor = Theme.General.BackColor
            tbExpandedContent.ForeColor = Theme.General.Controls.Label.ForeColor
            tbExpandedContent.ReadOnly = True
            tbExpandedContent.Multiline = True
            tbExpandedContent.HideSelection = True
            tbExpandedContent.Margin = New Padding(0)
            tbExpandedContent.BorderStyle = BorderStyle.None
            tbExpandedContent.Text = ExpandedInformation
            tbExpandedContent.Visible = True
            paMain.Controls.Add(tbExpandedContent)
        End If

        For Each command In Commands
            Dim cb As New CommandButton
            cb.Title = command.Text
            cb.Description = command.Description
            cb.Tag = command
            AddHandler cb.Click, AddressOf CommandClick
            paMain.Controls.Add(cb)
        Next

        For Each button In Buttons
            If Not flpButtons.Visible Then
                flpButtons.Visible = True
                flpButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink
                flpButtons.AutoSize = True
            End If

            Dim b As New ButtonEx
            b.Text = button.Text
            b.Tag = button.Value

            flpButtons.Controls.Add(b)
            AddHandler b.Click, AddressOf ButtonClick
        Next

        ActiveControl = laMainInstruction
        ShowIcon = False
        MinimizeBox = False
        MaximizeBox = False
        FormBorderStyle = FormBorderStyle.FixedDialog
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

        Commands.Add(New Command With {.Text = text, .Description = description, .Value = value})
    End Sub

    Sub AddButton(text As String, value As T)
        Buttons.Add(New Button With {.Text = text, .Value = value})
    End Sub

    Sub CommandClick(sender As Object, e As EventArgs)
        Dim tag = DirectCast(sender, CommandButton).Tag
        Dim cmd = DirectCast(tag, Command)
        SelectedText = cmd.Text
        SelectedValue = cmd.Value
        Close()
    End Sub

    Sub ButtonClick(sender As Object, e As EventArgs)
        Dim button = DirectCast(sender, ButtonEx)
        SelectedText = button.Text
        SelectedValue = DirectCast(button.Tag, T)
        Close()
    End Sub

    Sub SetHeight()
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

        If (h + nonClientHeight) > maxHeight Then
            h = maxHeight - nonClientHeight
        End If

        ClientSize = New Size(ClientSize.Width, h)
    End Sub

    Protected Overrides Sub OnLoad(args As EventArgs)
        MyBase.OnLoad(args)

        For Each i As ButtonEx In flpButtons.Controls
            Using g = i.CreateGraphics
                Dim minWidth = CInt(g.MeasureString(i.Text, i.Font).Width + i.Font.Height)

                If i.Width < minWidth Then
                    i.Width = minWidth
                End If
            End Using

            i.Margin = New Padding(CInt(FontHeight * 0.4), 0, 0, 0)
        Next

        flpButtons.Margin = New Padding(0, 0, CInt(FontHeight * 0.7), 0)

        SetHeight()
    End Sub

    Overloads Function Show() As T
        Init()
        ShowDialog()
        Return SelectedValue
    End Function

    Public Class Command
        Property Text As String
        Property Description As String
        Property Value As T
    End Class

    Public Class Button
        Property Text As String
        Property Value As T
    End Class
End Class
