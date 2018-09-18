Imports StaxRip.UI

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class VideoComparisonForm
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
        Me.TabControl = New StaxRip.UI.TabControlEx()
        Me.TrackBar = New StaxRip.UI.TrackBarEx()
        Me.lInfo = New System.Windows.Forms.Label()
        Me.bnMenu = New StaxRip.UI.ButtonEx()
        CType(Me.TrackBar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TabControl
        '
        Me.TabControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl.Location = New System.Drawing.Point(12, 9)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(953, 602)
        Me.TabControl.TabIndex = 0
        '
        'TrackBar
        '
        Me.TrackBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TrackBar.AutoSize = False
        Me.TrackBar.Location = New System.Drawing.Point(12, 617)
        Me.TrackBar.Name = "TrackBar"
        Me.TrackBar.Size = New System.Drawing.Size(950, 35)
        Me.TrackBar.TabIndex = 1
        Me.TrackBar.TabStop = False
        Me.TrackBar.TickStyle = System.Windows.Forms.TickStyle.None
        '
        'lInfo
        '
        Me.lInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lInfo.Location = New System.Drawing.Point(53, 658)
        Me.lInfo.Name = "lInfo"
        Me.lInfo.Size = New System.Drawing.Size(909, 35)
        Me.lInfo.TabIndex = 4
        Me.lInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'bnMenu
        '
        Me.bnMenu.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnMenu.Location = New System.Drawing.Point(12, 658)
        Me.bnMenu.ShowMenuSymbol = True
        Me.bnMenu.Size = New System.Drawing.Size(35, 35)
        '
        'VideoComparisonForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(974, 705)
        Me.Controls.Add(Me.bnMenu)
        Me.Controls.Add(Me.lInfo)
        Me.Controls.Add(Me.TrackBar)
        Me.Controls.Add(Me.TabControl)
        Me.DoubleBuffered = True
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Name = "VideoComparisonForm"
        Me.ShowIcon = False
        Me.Text = "Video Comparison"
        CType(Me.TrackBar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl As TabControlEx
    Friend WithEvents TrackBar As TrackBarEx
    Friend WithEvents lInfo As System.Windows.Forms.Label
    Friend WithEvents bnMenu As StaxRip.UI.ButtonEx
End Class
