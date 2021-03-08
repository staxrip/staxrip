Imports System.ComponentModel

Namespace UI
    Public Class KeysHelp
        Private Shared Converter As TypeConverter = TypeDescriptor.GetConverter(GetType(Keys))
        Private Shared KeysTexts As Dictionary(Of Keys, String)

        Shared Sub New()
            KeysTexts = New Dictionary(Of Keys, String)
            KeysTexts(Keys.Add) = "+ (Numpad)"
            KeysTexts(Keys.Back) = "Back"
            KeysTexts(Keys.Decimal) = "Decimal"
            KeysTexts(Keys.Delete) = "Delete"
            KeysTexts(Keys.Divide) = "Divide"
            KeysTexts(Keys.Down) = "Down"
            KeysTexts(Keys.End) = "End"
            KeysTexts(Keys.Enter) = "Enter"
            KeysTexts(Keys.Escape) = "Escape"
            KeysTexts(Keys.Home) = "Home"
            KeysTexts(Keys.Insert) = "Insert"
            KeysTexts(Keys.Left) = "Left"
            KeysTexts(Keys.Multiply) = "Multiply"
            KeysTexts(Keys.Next) = "Page Down"
            KeysTexts(Keys.Prior) = "Page Up"
            KeysTexts(Keys.Right) = "Right"
            KeysTexts(Keys.Space) = "Space"
            KeysTexts(Keys.Subtract) = "- (Numpad)"
            KeysTexts(Keys.Up) = "Up"
            KeysTexts(Keys.Control) = "Control"
            KeysTexts(Keys.Alt) = "Alt"
            KeysTexts(Keys.Shift) = "Shift"

            KeysTexts(Keys.D0) = "0"
            KeysTexts(Keys.D1) = "1"
            KeysTexts(Keys.D2) = "2"
            KeysTexts(Keys.D3) = "3"
            KeysTexts(Keys.D4) = "4"
            KeysTexts(Keys.D5) = "5"
            KeysTexts(Keys.D6) = "6"
            KeysTexts(Keys.D7) = "7"
            KeysTexts(Keys.D8) = "8"
            KeysTexts(Keys.D9) = "9"

            KeysTexts(Keys.NumPad0) = "0 (Numpad)"
            KeysTexts(Keys.NumPad1) = "1 (Numpad)"
            KeysTexts(Keys.NumPad2) = "2 (Numpad)"
            KeysTexts(Keys.NumPad3) = "3 (Numpad)"
            KeysTexts(Keys.NumPad4) = "4 (Numpad)"
            KeysTexts(Keys.NumPad5) = "5 (Numpad)"
            KeysTexts(Keys.NumPad6) = "6 (Numpad)"
            KeysTexts(Keys.NumPad7) = "7 (Numpad)"
            KeysTexts(Keys.NumPad8) = "8 (Numpad)"
            KeysTexts(Keys.NumPad9) = "9 (Numpad)"
        End Sub

        Shared Function GetKeyString(key As Keys) As String
            If key = Keys.None Then
                Return ""
            End If

            Dim ret = ""

            If (key And Keys.Control) = Keys.Control Then
                key = key Xor Keys.Control
                ret += "Ctrl+"
            End If

            If (key And Keys.Alt) = Keys.Alt Then
                key = key Xor Keys.Alt
                ret += "Alt+"
            End If

            If (key And Keys.Shift) = Keys.Shift Then
                key = key Xor Keys.Shift
                ret += "Shift+"
            End If

            If KeysTexts.ContainsKey(key) Then
                ret += KeysTexts(key)
            Else
                Dim value = Native.MapVirtualKey(CInt(key), 2) 'MAPVK_VK_TO_CHAR

                If value = 0 OrElse (value And 1 << 31) = 1 << 31 Then
                    ret += key.ToString
                Else
                    ret += Convert.ToChar(value)
                End If
            End If

            Return ret
        End Function
    End Class

    Public Class WebBrowserHelp
        Shared Sub ResetTextSize(b As WebBrowser)
            Dim r As New Reflector(b.ActiveXInstance)
            Dim IDM_FONTSIZE = 19, OLECMDEXECOPT_DODEFAULT = 0
            r.Invoke("ExecWB", IDM_FONTSIZE, OLECMDEXECOPT_DODEFAULT, 2)
        End Sub
    End Class
End Namespace
