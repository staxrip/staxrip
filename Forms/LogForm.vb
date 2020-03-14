Imports System.Text.RegularExpressions

Public Class LogForm
    Property Path As String

    Public Sub New()
        InitializeComponent()
        ScaleClientSize(55, 35)
        lb.ItemHeight = FontHeight * 2
        rtb.Font = New Font("Consolas", 10 * s.UIScaleFactor)
        rtb.ReadOnly = True
        rtb.BackColor = Color.White
    End Sub

    Sub Init()
        rtb.Text = Path.ReadAllText

        For Each match As Match In Regex.Matches(rtb.Text, "----- (.+) -----")
            Dim val = match.Groups(1).Value
            Dim match2 = Regex.Match(val, " \d+\.+.+")

            If match2.Success Then
                val = val.Substring(0, val.Length - match2.Value.Length)
            End If

            lb.Items.Add(val)
        Next

        lb.Items.Add("Open with " + g.GetTextEditorPath.Base)
    End Sub

    Private Sub lb_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lb.SelectedIndexChanged
        If lb.SelectedItem Is Nothing Then Exit Sub

        If lb.SelectedItem.ToString.StartsWith("Open with") Then
            g.StartProcess(g.GetTextEditorPath, p.Log.GetPath.Escape)
        Else
            rtb.Find(lb.SelectedItem.ToString)
            rtb.ScrollToCaret()
        End If
    End Sub
End Class