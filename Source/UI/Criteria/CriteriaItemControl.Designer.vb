Namespace UI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class CriteriaItemControl
        Inherits UserControl

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.components = New System.ComponentModel.Container()
            Me.LayoutPanel = New System.Windows.Forms.TableLayoutPanel()
            Me.mbProperties = New StaxRip.UI.MenuButton()
            Me.tePropertiesIdentifier = New StaxRip.UI.TextEdit()
            Me.mbCondition = New StaxRip.UI.MenuButton()
            Me.teValue = New StaxRip.UI.TextEdit()
            Me.bnRemove = New ButtonEx()
            Me.LayoutPanel.SuspendLayout()
            Me.SuspendLayout()
            '
            'LayoutPanel
            '
            Me.LayoutPanel.AutoScroll = True
            Me.LayoutPanel.AutoSize = True
            Me.LayoutPanel.AutoSizeMode = AutoSizeMode.GrowAndShrink
            Me.LayoutPanel.ColumnCount = 5
            Me.LayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0!))
            Me.LayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.0!))
            Me.LayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20.0!))
            Me.LayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35.0!))
            Me.LayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
            Me.LayoutPanel.Controls.Add(Me.mbProperties, 0, 0)
            Me.LayoutPanel.Controls.Add(Me.tePropertiesIdentifier, 1, 0)
            Me.LayoutPanel.Controls.Add(Me.mbCondition, 2, 0)
            Me.LayoutPanel.Controls.Add(Me.teValue, 3, 0)
            Me.LayoutPanel.Controls.Add(Me.bnRemove, 4, 0)
            Me.LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LayoutPanel.Location = New System.Drawing.Point(0, 0)
            Me.LayoutPanel.Name = "LayoutPanel"
            Me.LayoutPanel.RowCount = 1
            Me.LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.LayoutPanel.Size = New System.Drawing.Size(911, 175)
            Me.LayoutPanel.TabIndex = 0
            '
            'mbProperties
            '
            Me.mbProperties.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.mbProperties.Location = New System.Drawing.Point(3, 70)
            Me.mbProperties.ShowMenuSymbol = True
            Me.mbProperties.Size = New System.Drawing.Size(292, 35)
            Me.mbProperties.TabIndex = 1
            '
            'tePropertiesValue
            '
            Me.tePropertiesIdentifier.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tePropertiesIdentifier.Enabled = False
            Me.tePropertiesIdentifier.Location = New System.Drawing.Point(525, 70)
            Me.tePropertiesIdentifier.Name = "tePropertiesIdentifier"
            Me.tePropertiesIdentifier.Size = New System.Drawing.Size(218, 35)
            Me.tePropertiesIdentifier.TabIndex = 2
            '
            'mbCondition
            '
            Me.mbCondition.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.mbCondition.Enabled = False
            Me.mbCondition.Location = New System.Drawing.Point(301, 70)
            Me.mbCondition.ShowMenuSymbol = True
            Me.mbCondition.Size = New System.Drawing.Size(218, 35)
            Me.mbCondition.TabIndex = 3
            '
            'teValue
            '
            Me.teValue.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.teValue.Enabled = False
            Me.teValue.Location = New System.Drawing.Point(525, 70)
            Me.teValue.Name = "teValue"
            Me.teValue.Size = New System.Drawing.Size(218, 35)
            Me.teValue.TabIndex = 4
            '
            'bnRemove
            '
            Me.bnRemove.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.bnRemove.AutoSize = False
            Me.bnRemove.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.bnRemove.Location = New System.Drawing.Point(749, 58)
            Me.bnRemove.Name = "bnRemove"
            Me.bnRemove.Size = New System.Drawing.Size(158, 58)
            Me.bnRemove.TabIndex = 5
            Me.bnRemove.Text = "Remove"
            '
            'CriteriaItemControl
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
            Me.AutoScroll = True
            Me.Controls.Add(Me.LayoutPanel)
            Me.Font = FontManager.GetDefaultFont()
            Me.Margin = New System.Windows.Forms.Padding(0)
            Me.Name = "CriteriaItemControl"
            Me.Size = New System.Drawing.Size(911, 175)
            Me.LayoutPanel.ResumeLayout(False)
            Me.LayoutPanel.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents LayoutPanel As TableLayoutPanel
        Friend WithEvents mbProperties As MenuButton
        Friend WithEvents tePropertiesIdentifier As TextEdit
        Friend WithEvents mbCondition As MenuButton
        Friend WithEvents teValue As TextEdit
        Public WithEvents bnRemove As ButtonEx
    End Class
End Namespace

