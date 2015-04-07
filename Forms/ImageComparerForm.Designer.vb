Imports StaxRip.UI

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ImageComparerForm
    Inherits FormBase

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
        Me.TabControl = New System.Windows.Forms.TabControl()
        Me.bnAdd = New System.Windows.Forms.Button()
        Me.TrackBar = New System.Windows.Forms.TrackBar()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.bnRemove = New System.Windows.Forms.Button()
        Me.bnSave = New System.Windows.Forms.Button()
        Me.lInfo = New System.Windows.Forms.Label()
        Me.bnHelp = New System.Windows.Forms.Button()
        CType(Me.TrackBar, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl
        '
        Me.TabControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl.Location = New System.Drawing.Point(0, 0)
        Me.TabControl.Margin = New System.Windows.Forms.Padding(0)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(656, 473)
        Me.TabControl.TabIndex = 0
        '
        'bnAdd
        '
        Me.bnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnAdd.Location = New System.Drawing.Point(3, 3)
        Me.bnAdd.Name = "bnAdd"
        Me.bnAdd.Size = New System.Drawing.Size(120, 36)
        Me.bnAdd.TabIndex = 0
        Me.bnAdd.Text = "Add"
        Me.bnAdd.UseVisualStyleBackColor = True
        '
        'TrackBar
        '
        Me.TrackBar.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TrackBar.AutoSize = False
        Me.TrackBar.Location = New System.Drawing.Point(3, 476)
        Me.TrackBar.Name = "TrackBar"
        Me.TrackBar.Size = New System.Drawing.Size(650, 40)
        Me.TrackBar.TabIndex = 1
        Me.TrackBar.TabStop = False
        Me.TrackBar.TickStyle = System.Windows.Forms.TickStyle.None
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.Controls.Add(Me.TabControl, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel1, 0, 2)
        Me.TableLayoutPanel1.Controls.Add(Me.TrackBar, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.lInfo, 0, 3)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 4
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(656, 617)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Controls.Add(Me.bnAdd)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnRemove)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnSave)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnHelp)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(3, 522)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(504, 42)
        Me.FlowLayoutPanel1.TabIndex = 2
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnRemove.Location = New System.Drawing.Point(129, 3)
        Me.bnRemove.Name = "bnRemove"
        Me.bnRemove.Size = New System.Drawing.Size(120, 36)
        Me.bnRemove.TabIndex = 1
        Me.bnRemove.Text = "Remove"
        Me.bnRemove.UseVisualStyleBackColor = True
        '
        'bnSave
        '
        Me.bnSave.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnSave.Location = New System.Drawing.Point(255, 3)
        Me.bnSave.Name = "bnSave"
        Me.bnSave.Size = New System.Drawing.Size(120, 36)
        Me.bnSave.TabIndex = 2
        Me.bnSave.Text = "Save"
        Me.bnSave.UseVisualStyleBackColor = True
        '
        'lInfo
        '
        Me.lInfo.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lInfo.Location = New System.Drawing.Point(3, 567)
        Me.lInfo.Name = "lInfo"
        Me.lInfo.Size = New System.Drawing.Size(650, 50)
        Me.lInfo.TabIndex = 3
        '
        'bnHelp
        '
        Me.bnHelp.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnHelp.Location = New System.Drawing.Point(381, 3)
        Me.bnHelp.Name = "bnHelp"
        Me.bnHelp.Size = New System.Drawing.Size(120, 36)
        Me.bnHelp.TabIndex = 3
        Me.bnHelp.Text = "Help"
        Me.bnHelp.UseVisualStyleBackColor = True
        '
        'ImageComparerForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(656, 617)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Name = "ImageComparerForm"
        Me.ShowIcon = False
        Me.Text = "Image Comparer"
        Me.WindowState = System.Windows.Forms.FormWindowState.Maximized
        CType(Me.TrackBar, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents TabControl As System.Windows.Forms.TabControl
    Friend WithEvents bnAdd As System.Windows.Forms.Button
    Friend WithEvents TrackBar As System.Windows.Forms.TrackBar
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents bnRemove As System.Windows.Forms.Button
    Friend WithEvents bnSave As System.Windows.Forms.Button
    Friend WithEvents lInfo As System.Windows.Forms.Label
    Friend WithEvents bnHelp As System.Windows.Forms.Button
End Class
