
Public Class TaskDialog2(Of T)
    Inherits TaskDialogForm

    Property Commands As New List(Of Command)
    Property tbContent As TextBox
    Property tbExpandedContent As TextBox

    Shared Function Test() As Boolean
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

        'MsgInfo("aaaaa ".Multiply(20), ("bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb bbbbb" + BR).Multiply(10))
        'Return True

        'Application.Run(New TestForm)
        'Return True

        Dim td As New TaskDialog2(Of String)
        td.MainInstruction = "Hallo ".Multiply(20)
        td.Content = "Hi there"
        td.AddCommand("Hallo 1 ".Multiply(5))
        td.AddCommand("", "Description ".Multiply(5), "")
        td.AddCommand("Hallo 3".Multiply(5), "Ha Ha Ha ".Multiply(5), "")
        'td.AddCommand("Hi there", "What's up?", "")
        td.Show()

        Return True
    End Function

    Private _MainInstruction As String

    Property MainInstruction As String
        Get
            Return _MainInstruction
        End Get
        Set(value As String)
            _MainInstruction = value
        End Set
    End Property

    Private _Content As String

    Property Content As String
        Get
            Return _Content
        End Get
        Set(Value As String)
            _Content = Value
        End Set
    End Property

    Private _ExpandedInformation As String

    Property ExpandedInformation As String
        Get
            Return _ExpandedInformation
        End Get
        Set(Value As String)
            _ExpandedInformation = Value
        End Set
    End Property

    Sub Init()
        paIcon.Visible = False
        cbVerification.Visible = False
        flpButtons.Visible = False
        llFooter.Visible = False
        mbMenu.Visible = False
        teInput.Visible = False

        laMainInstruction.Font = New Font("Segoe UI", 12)
        laMainInstruction.Text = MainInstruction

        If Content <> "" Then
            tbContent = New TextBox
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
            tbExpandedContent.Text = ExpandedInformation
            paMain.Controls.Add(tbExpandedContent)
        End If

        For Each command In Commands
            Dim cb As New CommandButton
            cb.Title = command.Title
            cb.Description = command.Description
            cb.Tag = command
            paMain.Controls.Add(cb)
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
        Commands.Add(New Command With {.Title = text, .Description = description, .Value = value})
    End Sub

    Sub SetHeight()
        Dim h = tlpTop.Height

        If paMain.Controls.Count > 0 Then
            Dim last = paMain.Controls(paMain.Controls.Count - 1)
            h += last.Top + last.Height
        End If

        h += tlpBottom.Height
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
        SetHeight()
    End Sub

    Overloads Sub Show()
        Init()
        'ShowDialog()
        Application.Run(Me)
    End Sub

    Public Class Command
        Property Title As String
        Property Description As String
        Property Value As T
    End Class
End Class
