Imports StaxRip.UI

Imports System.ComponentModel

Public Class AviSynthScriptEditor
    Inherits MacroEditor

    Sub New(script As String)
        SetScriptDefaults()
        MacroEditorControl.Value = script
    End Sub
End Class