
Imports StaxRip.UI

Public Class ControlHostForm
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

    Friend WithEvents pControl As System.Windows.Forms.Panel

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.pControl = New System.Windows.Forms.Panel()
        Me.SuspendLayout()
        '
        'pControl
        '
        Me.pControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pControl.Location = New System.Drawing.Point(12, 12)
        Me.pControl.Name = "pControl"
        Me.pControl.Size = New System.Drawing.Size(638, 489)
        Me.pControl.TabIndex = 0
        '
        'ControlHostForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(662, 513)
        Me.Controls.Add(Me.pControl)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Name = "ControlHostForm"
        Me.Text = "DialogForm"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Sub New(title As String)
        InitializeComponent()
        Text = title
        HelpButton = False
    End Sub

    Sub AddControl(ctrl As Control)
        ctrl.Dock = DockStyle.Fill
        pControl.Controls.Add(ctrl)
    End Sub
End Class
