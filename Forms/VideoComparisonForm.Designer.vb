
Imports StaxRip.UI

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class VideoComparisonForm
    Inherits FormBase

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.TabControl = New StaxRip.UI.TabControlEx()
        Me.TrackBar = New StaxRip.UI.TrackBarEx()
        Me.laInfo = New System.Windows.Forms.Label()
        Me.bnMenu = New StaxRip.UI.ButtonEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        CType(Me.TrackBar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl
        '
        Me.TabControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpMain.SetColumnSpan(Me.TabControl, 2)
        Me.TabControl.Location = New System.Drawing.Point(10, 5)
        Me.TabControl.Margin = New System.Windows.Forms.Padding(10, 5, 10, 6)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(1436, 819)
        Me.TabControl.TabIndex = 0
        '
        'TrackBar
        '
        Me.TrackBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TrackBar.AutoSize = False
        Me.tlpMain.SetColumnSpan(Me.TrackBar, 2)
        Me.TrackBar.Location = New System.Drawing.Point(3, 833)
        Me.TrackBar.Name = "TrackBar"
        Me.TrackBar.Size = New System.Drawing.Size(1450, 75)
        Me.TrackBar.TabIndex = 1
        Me.TrackBar.TabStop = False
        Me.TrackBar.TickStyle = System.Windows.Forms.TickStyle.None
        '
        'laInfo
        '
        Me.laInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.laInfo.Location = New System.Drawing.Point(123, 924)
        Me.laInfo.Name = "laInfo"
        Me.laInfo.Size = New System.Drawing.Size(1330, 48)
        Me.laInfo.TabIndex = 4
        Me.laInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'bnMenu
        '
        Me.bnMenu.Location = New System.Drawing.Point(10, 911)
        Me.bnMenu.Margin = New System.Windows.Forms.Padding(10, 0, 10, 10)
        Me.bnMenu.ShowMenuSymbol = True
        Me.bnMenu.Size = New System.Drawing.Size(100, 65)
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 2
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Controls.Add(Me.bnMenu, 0, 2)
        Me.tlpMain.Controls.Add(Me.laInfo, 1, 2)
        Me.tlpMain.Controls.Add(Me.TabControl, 0, 0)
        Me.tlpMain.Controls.Add(Me.TrackBar, 0, 1)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1456, 986)
        Me.tlpMain.TabIndex = 5
        '
        'VideoComparisonForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1456, 986)
        Me.Controls.Add(Me.tlpMain)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(6)
        Me.Name = "VideoComparisonForm"
        Me.ShowIcon = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Video Comparison"
        CType(Me.TrackBar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tlpMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl As TabControlEx
    Friend WithEvents TrackBar As TrackBarEx
    Friend WithEvents laInfo As System.Windows.Forms.Label
    Friend WithEvents bnMenu As StaxRip.UI.ButtonEx
    Friend WithEvents tlpMain As TableLayoutPanel
End Class
