Imports StaxRip.UI

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class LogForm
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
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.lb = New StaxRip.UI.ListBoxEx()
        Me.rtb = New StaxRip.UI.RichTextBoxEx()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 2
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.0!))
        Me.tlpMain.Controls.Add(Me.lb, 0, 0)
        Me.tlpMain.Controls.Add(Me.rtb, 1, 0)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 1
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Size = New System.Drawing.Size(961, 825)
        Me.tlpMain.TabIndex = 0
        '
        'lb
        '
        Me.lb.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lb.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed
        Me.lb.FormattingEnabled = True
        Me.lb.IntegralHeight = False
        Me.lb.ItemHeight = 67
        Me.lb.Location = New System.Drawing.Point(0, 0)
        Me.lb.Margin = New System.Windows.Forms.Padding(0)
        Me.lb.Name = "lb"
        Me.lb.Size = New System.Drawing.Size(259, 825)
        Me.lb.TabIndex = 1
        '
        'rtb
        '
        Me.rtb.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtb.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtb.Location = New System.Drawing.Point(259, 0)
        Me.rtb.Margin = New System.Windows.Forms.Padding(0)
        Me.rtb.Name = "rtb"
        Me.rtb.Size = New System.Drawing.Size(702, 825)
        Me.rtb.TabIndex = 2
        Me.rtb.Text = ""
        '
        'LogForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(961, 825)
        Me.Controls.Add(Me.tlpMain)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(6, 8, 6, 8)
        Me.Name = "LogForm"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Log File Viewer"
        Me.tlpMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents lb As ListBoxEx
    Friend WithEvents rtb As RichTextBoxEx
End Class
