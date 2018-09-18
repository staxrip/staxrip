<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TestForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.ListViewEx1 = New StaxRip.UI.ListViewEx()
        Me.LabelEx1 = New StaxRip.UI.LabelEx()
        Me.SuspendLayout()
        '
        'ListViewEx1
        '
        Me.ListViewEx1.Location = New System.Drawing.Point(59, 47)
        Me.ListViewEx1.Name = "ListViewEx1"
        Me.ListViewEx1.Size = New System.Drawing.Size(498, 413)
        Me.ListViewEx1.TabIndex = 0
        Me.ListViewEx1.UseCompatibleStateImageBehavior = False
        '
        'LabelEx1
        '
        Me.LabelEx1.AutoSize = True
        Me.LabelEx1.Location = New System.Drawing.Point(169, 604)
        Me.LabelEx1.Size = New System.Drawing.Size(158, 48)
        Me.LabelEx1.Text = "LabelEx1"
        Me.LabelEx1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'TestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1108, 787)
        Me.Controls.Add(Me.LabelEx1)
        Me.Controls.Add(Me.ListViewEx1)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "TestForm"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Test"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ListViewEx1 As UI.ListViewEx
    Friend WithEvents LabelEx1 As UI.LabelEx
End Class
