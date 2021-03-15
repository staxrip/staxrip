Imports StaxRip.UI

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class CodeForm
    Inherits FormBase

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub dispose(ByVal disposing As Boolean)
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.rtb = New StaxRip.UI.RichTextBoxEx()
        Me.SuspendLayout()
        '
        'rtb
        '
        Me.rtb.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtb.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtb.Location = New System.Drawing.Point(0, 0)
        Me.rtb.Name = "rtb"
        Me.rtb.Size = New System.Drawing.Size(1281, 1106)
        Me.rtb.TabIndex = 0
        Me.rtb.Text = ""
        '
        'CodeForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1281, 1106)
        Me.Controls.Add(Me.rtb)
        Me.Name = "CodeForm"
        Me.ShowIcon = False
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents rtb As UI.RichTextBoxEx
End Class
