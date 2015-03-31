Imports System.Drawing.Design
Imports System.ComponentModel
Imports System.Text.RegularExpressions

Namespace UI
    <ProvideProperty("Pattern", GetType(Control))>
    Public Class ValidationProvider
        Inherits Component
        Implements IExtenderProvider

        Private Patterns As New Dictionary(Of Control, String)
        Private Defaults As New Dictionary(Of Control, String)

        Function CanExtend(obj As Object) As Boolean Implements IExtenderProvider.CanExtend
            Return TypeOf obj Is Control
        End Function

        <DefaultValue("")>
        Function GetPattern(c As Control) As String
            If Patterns.ContainsKey(c) Then
                Return Patterns(c)
            End If

            Return ""
        End Function

        Sub SetPattern(c As Control, value As String)
            If Not Patterns.ContainsKey(c) Then
                AddHandler c.Validating, AddressOf Validating
                AddHandler c.Enter, AddressOf Enter
            End If

            Patterns(c) = value
        End Sub

        Sub Validating(sender As Object, e As CancelEventArgs)
            Dim c = CType(sender, Control)

            If Not Regex.IsMatch(c.Text, Patterns(c)) Then
                MsgWarn("""" + c.Text + """ is no valid input.")
                c.Text = Defaults(c)
            End If
        End Sub

        Sub Enter(sender As Object, e As EventArgs)
            Dim c = DirectCast(sender, Control)
            Defaults(c) = c.Text
        End Sub
    End Class
End Namespace
