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
        Me.cms = New StaxRip.UI.ContextMenuStripEx(Me.components)
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(1652, 1163)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(8, 8, 16, 16)
        Me.bnCancel.Size = New System.Drawing.Size(200, 72)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(1436, 1163)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(8, 8, 8, 16)
        Me.bnOK.Size = New System.Drawing.Size(200, 72)
        Me.bnOK.Text = "OK"
        '
        'rtbCommandLine
        '
        Me.rtbCommandLine.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbCommandLine.BlockPaint = False
        Me.rtbCommandLine.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TableLayoutPanel1.SetColumnSpan(Me.rtbCommandLine, 4)
        Me.rtbCommandLine.Font = New System.Drawing.Font("Consolas", 10.0!)
        Me.rtbCommandLine.LastCommandLine = Nothing
        Me.rtbCommandLine.Location = New System.Drawing.Point(16, 1085)
        Me.rtbCommandLine.Margin = New System.Windows.Forms.Padding(16, 0, 16, 8)
        Me.rtbCommandLine.Name = "rtbCommandLine"
        Me.rtbCommandLine.ReadOnly = True
        Me.rtbCommandLine.Size = New System.Drawing.Size(1836, 59)
        Me.rtbCommandLine.TabIndex = 4
        Me.rtbCommandLine.Text = ""
        '
        'SimpleUI
        '
        Me.TableLayoutPanel1.SetColumnSpan(Me.SimpleUI, 4)
        Me.SimpleUI.Location = New System.Drawing.Point(16, 16)
        Me.SimpleUI.Margin = New System.Windows.Forms.Padding(16)
        Me.SimpleUI.Name = "SimpleUI"
        Me.SimpleUI.Size = New System.Drawing.Size(1836, 1053)
        Me.SimpleUI.TabIndex = 5
        Me.SimpleUI.Text = "SimpleUI"
        '
        'cbGoTo
        '
        Me.cbGoTo.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.cbGoTo.FormattingEnabled = True
        Me.cbGoTo.Location = New System.Drawing.Point(16, 1171)
        Me.cbGoTo.Margin = New System.Windows.Forms.Padding(16, 8, 8, 16)
        Me.cbGoTo.Name = "cbGoTo"
        Me.cbGoTo.Size = New System.Drawing.Size(476, 56)
        Me.cbGoTo.TabIndex = 8
        '
        'bnMenu
        '
        Me.bnMenu.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.bnMenu.ContextMenuStrip = Me.cms
        Me.bnMenu.Location = New System.Drawing.Point(1348, 1163)
        Me.bnMenu.Margin = New System.Windows.Forms.Padding(8, 8, 8, 16)
        Me.bnMenu.ShowMenuSymbol = True
        Me.bnMenu.Size = New System.Drawing.Size(72, 72)
        '
        'cms
        '
        Me.cms.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.cms.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.cms.Name = "cms"
        Me.cms.Size = New System.Drawing.Size(111, 4)
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel1.ColumnCount = 4
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.Controls.Add(Me.SimpleUI, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.rtbCommandLine, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.bnCancel, 3, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.bnMenu, 1, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.bnOK, 2, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.cbGoTo, 0, 2)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(1868, 1255)
        Me.TableLayoutPanel1.TabIndex = 11
        '
        'CommandLineForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(1868, 1255)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Margin = New System.Windows.Forms.Padding(8)
        Me.Name = "CommandLineForm"
        Me.Text = "x265"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents rtbCommandLine As StaxRip.UI.CommandLineRichTextBox
    Friend WithEvents SimpleUI As StaxRip.SimpleUI
    Friend WithEvents cbGoTo As System.Windows.Forms.ComboBox
    Friend WithEvents bnMenu As StaxRip.UI.ButtonEx
    Friend WithEvents cms As ContextMenuStripEx
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
End Class
