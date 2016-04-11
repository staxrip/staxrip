Imports StaxRip.UI

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class CommandLineForm
    Inherits DialogBase

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
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
        Me.components = New System.ComponentModel.Container()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.rtbCommandLine = New StaxRip.UI.CommandLineRichTextBox()
        Me.SimpleUI = New StaxRip.SimpleUI()
        Me.cbGoTo = New System.Windows.Forms.ComboBox()
        Me.bnMenu = New StaxRip.UI.ButtonEx()
        Me.cms = New ContextMenuStripEx(Me.components)
        Me.SuspendLayout()
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(822, 633)
        Me.bnCancel.Size = New System.Drawing.Size(100, 36)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(716, 633)
        Me.bnOK.Size = New System.Drawing.Size(100, 36)
        Me.bnOK.Text = "OK"
        '
        'rtbCmdl
        '
        Me.rtbCommandLine.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbCommandLine.BlockPaint = False
        Me.rtbCommandLine.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbCommandLine.LastCommandLine = Nothing
        Me.rtbCommandLine.Location = New System.Drawing.Point(12, 583)
        Me.rtbCommandLine.Margin = New System.Windows.Forms.Padding(4)
        Me.rtbCommandLine.Name = "rtbCmdl"
        Me.rtbCommandLine.ReadOnly = True
        Me.rtbCommandLine.Size = New System.Drawing.Size(910, 35)
        Me.rtbCommandLine.TabIndex = 4
        Me.rtbCommandLine.Text = ""
        '
        'SimpleUI
        '
        Me.SimpleUI.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleUI.Location = New System.Drawing.Point(12, 12)
        Me.SimpleUI.Name = "SimpleUI"
        Me.SimpleUI.Size = New System.Drawing.Size(910, 564)
        Me.SimpleUI.TabIndex = 5
        Me.SimpleUI.Text = "SimpleUI"
        '
        'cbGoTo
        '
        Me.cbGoTo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cbGoTo.FormattingEnabled = True
        Me.cbGoTo.Location = New System.Drawing.Point(11, 633)
        Me.cbGoTo.Margin = New System.Windows.Forms.Padding(4)
        Me.cbGoTo.Name = "cbGoTo"
        Me.cbGoTo.Size = New System.Drawing.Size(193, 33)
        Me.cbGoTo.TabIndex = 8
        '
        'bnMenu
        '
        Me.bnMenu.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnMenu.ContextMenuStrip = Me.cms
        Me.bnMenu.Location = New System.Drawing.Point(674, 633)
        Me.bnMenu.ShowMenuSymbol = True
        Me.bnMenu.Size = New System.Drawing.Size(36, 36)
        '
        'cms
        '
        Me.cms.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.cms.Name = "cms"
        Me.cms.Size = New System.Drawing.Size(61, 4)
        '
        'CommandLineForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(934, 680)
        Me.Controls.Add(Me.bnMenu)
        Me.Controls.Add(Me.cbGoTo)
        Me.Controls.Add(Me.SimpleUI)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.bnOK)
        Me.Controls.Add(Me.rtbCommandLine)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "CommandLineForm"
        Me.Text = "x265"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents rtbCommandLine As StaxRip.UI.CommandLineRichTextBox
    Friend WithEvents SimpleUI As StaxRip.SimpleUI
    Friend WithEvents cbGoTo As System.Windows.Forms.ComboBox
    Friend WithEvents bnMenu As StaxRip.UI.ButtonEx
    Friend WithEvents cms As ContextMenuStripEx
End Class
