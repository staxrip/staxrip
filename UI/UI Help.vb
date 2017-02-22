Imports System.ComponentModel

Namespace UI
    Class KeysHelp
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

        Shared Function GetKeyString(k As Keys) As String
            If k = Keys.None Then Return ""

            Dim s = ""

            If (k And Keys.Control) = Keys.Control Then
                k = k Xor Keys.Control
                s += "Ctrl+"
            End If

            If (k And Keys.Alt) = Keys.Alt Then
                k = k Xor Keys.Alt
                s += "Alt+"
            End If

            If (k And Keys.Shift) = Keys.Shift Then
                k = k Xor Keys.Shift
                s += "Shift+"
            End If

            If KeysTexts.ContainsKey(k) Then
                s += KeysTexts(k)
            Else
                Dim value = Native.MapVirtualKey(CInt(k), Native.MAPVK_VK_TO_CHAR)

                If value = 0 OrElse (value And 1 << 31) = 1 << 31 Then
                    s += k.ToString
                Else
                    s += Convert.ToChar(value)
                End If
            End If

            Return s
        End Function
    End Class

    Class WebBrowserHelp
        Shared Sub ResetTextSize(b As WebBrowser)
            Dim r As New Reflector(b.ActiveXInstance)
            Dim IDM_FONTSIZE = 19, OLECMDEXECOPT_DODEFAULT = 0
            r.Invoke("ExecWB", IDM_FONTSIZE, OLECMDEXECOPT_DODEFAULT, 2)
        End Sub
    End Class
End Namespace