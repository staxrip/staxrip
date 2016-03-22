Imports StaxRip.UI

Imports System.ComponentModel

Class ScriptEditor
    Inherits MacroEditor

    Sub New(script As String)
        SetScriptDefaults()
        MacroEditorControl.Value = script
    End Sub
End Class