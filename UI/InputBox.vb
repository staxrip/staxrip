Namespace UI
    Class InputBox
        Property Text As String
        Property Title As String = Application.ProductName
        Property Value As String
        Property VerificationText As String
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

        Shared Function Show(text As String,
                             title As String) As String

            Return Show(text, title, "", FormStartPosition.CenterParent)
        End Function

        Shared Function Show(text As String,
                             title As String,
                             value As String) As String

            Return Show(text, title, value, FormStartPosition.CenterParent)
        End Function

        Shared Function Show(text As String,
                             title As String,
                             value As String,
                             startPos As FormStartPosition) As String

            Dim b As New InputBox
            b.Text = text
            b.Title = title
            b.Value = value
            b.StartPosition = startPos

            If b.Show = DialogResult.OK Then
                Return b.Value
            Else
                Return Nothing
            End If
        End Function

        Function Show() As DialogResult
            Using f As New InputBoxForm
                f.laPrompt.Text = Text
                f.tbInput.Text = Value
                f.Text = Title
                f.StartPosition = StartPosition

                If OK(VerificationText) Then
                    f.cb.Checked = Checked
                    f.cb.Text = VerificationText
                End If

                f.cb.Visible = VerificationText <> ""

                Dim ret As DialogResult

                If Not Owner Is Nothing Then
                    ret = f.ShowDialog(Owner)
                Else
                    ret = f.ShowDialog()
                End If

                Checked = f.cb.Checked
                Value = f.tbInput.Text

                Return ret
            End Using
        End Function
    End Class
End Namespace