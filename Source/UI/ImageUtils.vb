
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Globalization
Imports System.Threading.Tasks

Public Class ImageHelp
    Private Shared Coll As PrivateFontCollection
    Private Shared ReadOnly AwesomePath As String = Folder.Apps + "\Fonts\FontAwesome.ttf"
    Private Shared ReadOnly SegoePath As String = Folder.Apps + "\Fonts\Segoe-MDL2-Assets.ttf"
    Private Shared ReadOnly FontFilesExist As Boolean = File.Exists(AwesomePath) AndAlso File.Exists(SegoePath)

    Shared Async Function GetSymbolImageAsync(symbol As Symbol, Optional color As Color = Nothing) As Task(Of Image)
        Return Await Task.Run(Function() GetSymbolImage(symbol, color))
    End Function

    Shared Function GetSymbolImage(
        symbol As Symbol,
        Optional color As Color = Nothing,
        Optional fontSize As Integer = 12) As Image

        If Not FontFilesExist Then
            Return Nothing
        End If

        Dim legacy = OSVersion.Current < OSVersion.Windows10

        If Coll Is Nothing Then
            Coll = New PrivateFontCollection
            Coll.AddFontFile(AwesomePath)

            If legacy Then
                Coll.AddFontFile(SegoePath)
            End If
        End If

        Dim family As FontFamily = Nothing

        If symbol > 61400 Then
            If Coll.Families.Count > 0 Then
                family = Coll.Families(0)
            End If
        Else
            If legacy Then
                If Coll.Families.Count > 1 Then
                    family = Coll.Families(1)
                End If
            Else
                family = New FontFamily("Segoe MDL2 Assets")
            End If
        End If

        If family Is Nothing Then
            Return Nothing
        End If

        color = If(color = Nothing, ThemeManager.CurrentTheme.General.Controls.ToolStrip.SymbolImageColor.ToColor(), color)

        Dim bitmap As Bitmap

        Using font As New Font(family, fontSize)
            Dim fontHeight = font.Height
            bitmap = New Bitmap(CInt(fontHeight * 1.1F), CInt(fontHeight * 1.1F))

            Using graphics As Graphics = Graphics.FromImage(bitmap)
                graphics.TextRenderingHint = TextRenderingHint.AntiAlias
                Using brush As Brush = New SolidBrush(color)
                    graphics.DrawString(Convert.ToChar(symbol), font, brush, -fontHeight * 0.1F, fontHeight * 0.07F)
                End Using
            End Using
        End Using

        Return bitmap
    End Function
End Class
