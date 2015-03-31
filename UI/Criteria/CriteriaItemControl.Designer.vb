Namespace UI
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class CriteriaItemControl
        Inherits UserControl

        'UserControl overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
            MyBase.Dispose(disposing)
        End Sub

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.LayoutPanel = New System.Windows.Forms.TableLayoutPanel()
            Me.buRemove = New System.Windows.Forms.Button()
            Me.cbCondition = New System.Windows.Forms.ComboBox()
            Me.cbProperty = New System.Windows.Forms.ComboBox()
            Me.te = New StaxRip.UI.TextEdit()
            Me.LayoutPanel.SuspendLayout()
            Me.SuspendLayout()
            '
            'LayoutPanel
            '
            Me.LayoutPanel.AutoSize = True
            Me.LayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.LayoutPanel.ColumnCount = 4
            Me.LayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
            Me.LayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
            Me.LayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334!))
            Me.LayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
            Me.LayoutPanel.Controls.Add(Me.buRemove, 3, 0)
            Me.LayoutPanel.Controls.Add(Me.te, 2, 0)
            Me.LayoutPanel.Controls.Add(Me.cbCondition, 1, 0)
            Me.LayoutPanel.Controls.Add(Me.cbProperty, 0, 0)
            Me.LayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill
            Me.LayoutPanel.Location = New System.Drawing.Point(0, 0)
            Me.LayoutPanel.Name = "LayoutPanel"
            Me.LayoutPanel.RowCount = 1
            Me.LayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
            Me.LayoutPanel.Size = New System.Drawing.Size(537, 46)
            Me.LayoutPanel.TabIndex = 0
            '
            'buRemove
            '
            Me.buRemove.Anchor = System.Windows.Forms.AnchorStyles.None
            Me.buRemove.AutoSize = True
            Me.buRemove.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.buRemove.Location = New System.Drawing.Point(470, 3)
            Me.buRemove.Name = "buRemove"
            Me.buRemove.Size = New System.Drawing.Size(63, 40)
            Me.buRemove.TabIndex = 3
            Me.buRemove.Text = "Add"
            Me.buRemove.UseVisualStyleBackColor = True
            '
            'cbCondition
            '
            Me.cbCondition.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cbCondition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbCondition.FormattingEnabled = True
            Me.cbCondition.Location = New System.Drawing.Point(158, 2)
            Me.cbCondition.Margin = New System.Windows.Forms.Padding(3, 0, 3, 3)
            Me.cbCondition.Name = "cbCondition"
            Me.cbCondition.Size = New System.Drawing.Size(150, 38)
            Me.cbCondition.TabIndex = 1
            '
            'cbProperty
            '
            Me.cbProperty.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cbProperty.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cbProperty.FormattingEnabled = True
            Me.cbProperty.Location = New System.Drawing.Point(3, 2)
            Me.cbProperty.Margin = New System.Windows.Forms.Padding(3, 0, 3, 3)
            Me.cbProperty.MaxDropDownItems = 30
            Me.cbProperty.Name = "cbProperty"
            Me.cbProperty.Size = New System.Drawing.Size(149, 38)
            Me.cbProperty.Sorted = True
            Me.cbProperty.TabIndex = 0
            '
            'te
            '
            Me.te.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.te.Location = New System.Drawing.Point(314, 5)
            Me.te.Name = "te"
            Me.te.Size = New System.Drawing.Size(150, 36)
            Me.te.TabIndex = 4
            '
            'CriteriaItemControl
            '
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
            Me.AutoSize = True
            Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.BackColor = System.Drawing.SystemColors.Window
            Me.Controls.Add(Me.LayoutPanel)
            Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Margin = New System.Windows.Forms.Padding(0)
            Me.Name = "CriteriaItemControl"
            Me.Size = New System.Drawing.Size(537, 46)
            Me.LayoutPanel.ResumeLayout(False)
            Me.LayoutPanel.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents LayoutPanel As System.Windows.Forms.TableLayoutPanel
        Public WithEvents buRemove As System.Windows.Forms.Button
        Public WithEvents cbProperty As System.Windows.Forms.ComboBox
        Public WithEvents cbCondition As System.Windows.Forms.ComboBox
        Friend WithEvents te As StaxRip.UI.TextEdit

    End Class
End Namespace

