Imports StaxRip.UI

Public Class UserControlTemplate
    Inherits UserControl

#Region " Designer "
    <DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If

        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.SuspendLayout()
        '
        'UserControlTemplate
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(144.0!, 144.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.Name = "UserControlTemplate"
        Me.Size = New System.Drawing.Size(379, 368)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Sub New()
        MyBase.New()
        InitializeComponent()
    End Sub
End Class