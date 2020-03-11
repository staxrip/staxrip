
Namespace UI
    Public Class InputBox
        Property Text As String
        Property Title As String = Application.ProductName
        Property Value As String
        Property CheckBoxText As String
        Property Owner As IWin32Window
        Property Checked As Boolean

        Private StartPositionValue As FormStartPosition = FormStartPosition.CenterParent

        Property StartPosition() As FormStartPosition
            Get
                If Process.GetCurrentProcess.MainWindowHandle = IntPtr.Zero Then
                    StartPositionValue = FormStartPosition.CenterScreen
                End If

                Return StartPositionValue
            End Get
            Set(Value As FormStartPosition)
                StartPositionValue = Value
            End Set
        End Property

        Shared Function Show(text As String) As String
            Return Show(text, Application.ProductName)
        End Function

        Shared Function Show(text As String, title As String) As String
            Return Show(text, title, "", FormStartPosition.CenterParent)
        End Function

        Shared Function Show(text As String, title As String, value As String) As String
            Return Show(text, title, value, FormStartPosition.CenterParent)
        End Function

        Shared Function Show(
            text As String,
            title As String,
            value As String,
            startPos As FormStartPosition) As String

            Dim box As New InputBox
            box.Text = text
            box.Title = title
            box.Value = value
            box.StartPosition = startPos

            If box.Show = DialogResult.OK Then
                Return box.Value
            Else
                Return Nothing
            End If
        End Function

        Function Show() As DialogResult
            Using form As New InputBoxForm
                form.laPrompt.Text = Text
                form.tbInput.Text = Value
                form.Text = Title
                form.StartPosition = StartPosition

                If CheckBoxText <> "" Then
                    form.cb.Checked = Checked
                    form.cb.Text = CheckBoxText
                End If

                form.cb.Visible = CheckBoxText <> ""

                Dim ret As DialogResult

                If Not Owner Is Nothing Then
                    ret = form.ShowDialog(Owner)
                Else
                    ret = form.ShowDialog()
                End If

                Checked = form.cb.Checked
                Value = form.tbInput.Text

                Return ret
            End Using
        End Function
    End Class
End Namespace