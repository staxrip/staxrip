Imports StaxRip.UI

Public Class DialogTemplate
    Inherits DialogBase

#Region " Designer "

    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub
    Friend WithEvents CommandLink1 As StaxRip.UI.CommandLink

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'DialogTemplate
        '
        Me.ClientSize = New System.Drawing.Size(306, 271)
        Me.Name = "DialogTemplate"
        Me.Text = "DialogTemplate"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Sub New()
        MyBase.New()
        InitializeComponent()
    End Sub
End Class