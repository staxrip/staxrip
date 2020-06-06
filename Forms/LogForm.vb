
Imports System.Text.RegularExpressions

Imports StaxRip.UI

Public Class LogForm
    Public Sub New()
        InitializeComponent()
        RestoreClientSize(50, 35)
        Text += " - " + p.Log.GetPath
        lb.BackColor = SystemColors.Control
        lb.ItemHeight = FontHeight * 2
        rtb.Font = New Font("Consolas", 10 * s.UIScaleFactor)
        rtb.ReadOnly = True
        rtb.BackColor = SystemColors.Control
        rtb.Text = p.Log.GetPath.ReadAllText

        For Each match As Match In Regex.Matches(rtb.Text, "^-+ (.+) -+", RegexOptions.Multiline)
            Dim val = match.Groups(1).Value
            Dim match2 = Regex.Match(val, " \d+\.+.+")

            If match2.Success Then
                val = val.Substring(0, val.Length - match2.Value.Length)
            End If

            lb.Items.Add(val)
        Next

        Dim cms = DirectCast(rtb.ContextMenuStrip, ContextMenuStripEx)
        cms.Form = Me

        cms.Add("-")
        cms.Add("Save As...", Sub()
                                  Using dialog As New SaveFileDialog
                                      dialog.FileName = p.Log.GetPath.FileName

                                      If dialog.ShowDialog = DialogResult.OK Then
                                          rtb.Text.FixBreak.WriteFileUTF8(dialog.FileName)
                                      End If
                                  End Using
                              End Sub, Keys.Control Or Keys.S).SetImage(Symbol.Save)
        cms.Add("Open in Text Editor", Sub() g.ShellExecute(g.GetTextEditorPath, p.Log.GetPath.Escape), Keys.Control Or Keys.T).SetImage(Symbol.Edit)
        cms.Add("Show in File Explorer", Sub() g.SelectFileWithExplorer(p.Log.GetPath), Keys.Control Or Keys.E).SetImage(Symbol.FileExplorer)
        cms.Add("Show History", Sub() g.ShellExecute(Folder.Settings + "Log Files"), Keys.Control Or Keys.H).SetImage(Symbol.ClockLegacy)
    End Sub

    Sub lb_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lb.SelectedIndexChanged
        If lb.SelectedItem IsNot Nothing Then
            rtb.Find("- " + lb.SelectedItem.ToString + " -")
            rtb.ScrollToCaret()
        End If
    End Sub
End Class
