Imports StaxRip.UI

<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class TaskDialogForm
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
        Me.components = New System.ComponentModel.Container()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpBottom = New System.Windows.Forms.TableLayoutPanel()
        Me.llFooter = New System.Windows.Forms.LinkLabel()
        Me.cbVerification = New StaxRip.UI.CheckBoxEx()
        Me.flpButtons = New System.Windows.Forms.FlowLayoutPanel()
        Me.teInput = New StaxRip.UI.TextEdit()
        Me.mbMenu = New StaxRip.UI.MenuButton()
        Me.tlpTop = New System.Windows.Forms.TableLayoutPanel()
        Me.paIcon = New System.Windows.Forms.Panel()
        Me.laMainInstruction = New System.Windows.Forms.Label()
        Me.paMain = New StaxRip.TaskDialogForm.TaskDialogPanel()
        Me.tlpMain.SuspendLayout()
        Me.tlpBottom.SuspendLayout()
        Me.tlpTop.SuspendLayout()
        Me.SuspendLayout()
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 1
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Controls.Add(Me.tlpBottom, 0, 2)
        Me.tlpMain.Controls.Add(Me.tlpTop, 0, 0)
        Me.tlpMain.Controls.Add(Me.paMain, 0, 1)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 3
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(804, 586)
        Me.tlpMain.TabIndex = 0
        '
        'tlpBottom
        '
        Me.tlpBottom.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpBottom.AutoSize = True
        Me.tlpBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpBottom.ColumnCount = 1
        Me.tlpBottom.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpBottom.Controls.Add(Me.llFooter, 0, 4)
        Me.tlpBottom.Controls.Add(Me.cbVerification, 0, 3)
        Me.tlpBottom.Controls.Add(Me.flpButtons, 0, 0)
        Me.tlpBottom.Controls.Add(Me.teInput, 0, 1)
        Me.tlpBottom.Controls.Add(Me.mbMenu, 0, 2)
        Me.tlpBottom.Location = New System.Drawing.Point(0, 250)
        Me.tlpBottom.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpBottom.Name = "tlpBottom"
        Me.tlpBottom.RowCount = 5
        Me.tlpBottom.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBottom.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBottom.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBottom.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBottom.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpBottom.Size = New System.Drawing.Size(804, 336)
        Me.tlpBottom.TabIndex = 0
        '
        'llFooter
        '
        Me.llFooter.AutoSize = True
        Me.llFooter.Location = New System.Drawing.Point(3, 288)
        Me.llFooter.Name = "llFooter"
        Me.llFooter.Size = New System.Drawing.Size(142, 48)
        Me.llFooter.TabIndex = 0
        Me.llFooter.TabStop = True
        Me.llFooter.Text = "llFooter"
        '
        'cbVerification
        '
        Me.cbVerification.AutoSize = True
        Me.cbVerification.FlatAppearance.BorderSize = 2
        Me.cbVerification.Location = New System.Drawing.Point(3, 233)
        Me.cbVerification.Size = New System.Drawing.Size(282, 52)
        Me.cbVerification.Text = "cbVerification"
        Me.cbVerification.UseVisualStyleBackColor = False
        '
        'flpButtons
        '
        Me.flpButtons.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.flpButtons.Location = New System.Drawing.Point(604, 0)
        Me.flpButtons.Margin = New System.Windows.Forms.Padding(0)
        Me.flpButtons.Name = "flpButtons"
        Me.flpButtons.Size = New System.Drawing.Size(200, 100)
        Me.flpButtons.TabIndex = 2
        '
        'teInput
        '
        Me.teInput.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.teInput.Location = New System.Drawing.Point(0, 100)
        Me.teInput.Margin = New System.Windows.Forms.Padding(0)
        Me.teInput.Name = "teInput"
        Me.teInput.Size = New System.Drawing.Size(804, 60)
        Me.teInput.Text = "teInput"
        '
        'mbMenu
        '
        Me.mbMenu.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.mbMenu.FlatAppearance.BorderSize = 2
        Me.mbMenu.Location = New System.Drawing.Point(0, 160)
        Me.mbMenu.Margin = New System.Windows.Forms.Padding(0)
        Me.mbMenu.Padding = New System.Windows.Forms.Padding(4, 0, 0, 0)
        Me.mbMenu.ShowMenuSymbol = True
        Me.mbMenu.ShowPath = False
        Me.mbMenu.Size = New System.Drawing.Size(804, 70)
        Me.mbMenu.Text2 = "mbMenu"
        '
        'tlpTop
        '
        Me.tlpTop.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpTop.AutoSize = True
        Me.tlpTop.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpTop.ColumnCount = 2
        Me.tlpTop.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpTop.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpTop.Controls.Add(Me.paIcon, 0, 0)
        Me.tlpTop.Controls.Add(Me.laMainInstruction, 1, 0)
        Me.tlpTop.Location = New System.Drawing.Point(0, 0)
        Me.tlpTop.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpTop.Name = "tlpTop"
        Me.tlpTop.RowCount = 1
        Me.tlpTop.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpTop.Size = New System.Drawing.Size(804, 150)
        Me.tlpTop.TabIndex = 1
        '
        'paIcon
        '
        Me.paIcon.Location = New System.Drawing.Point(0, 0)
        Me.paIcon.Margin = New System.Windows.Forms.Padding(0)
        Me.paIcon.Name = "paIcon"
        Me.paIcon.Size = New System.Drawing.Size(150, 150)
        Me.paIcon.TabIndex = 0
        '
        'laMainInstruction
        '
        Me.laMainInstruction.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.laMainInstruction.AutoSize = True
        Me.laMainInstruction.Location = New System.Drawing.Point(170, 51)
        Me.laMainInstruction.Margin = New System.Windows.Forms.Padding(20)
        Me.laMainInstruction.Name = "laMainInstruction"
        Me.laMainInstruction.Size = New System.Drawing.Size(295, 48)
        Me.laMainInstruction.TabIndex = 1
        Me.laMainInstruction.Text = "laMainInstruction"
        '
        'paMain
        '
        Me.paMain.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.paMain.AutoScroll = True
        Me.paMain.Location = New System.Drawing.Point(0, 150)
        Me.paMain.Margin = New System.Windows.Forms.Padding(0)
        Me.paMain.Name = "paMain"
        Me.paMain.Size = New System.Drawing.Size(804, 100)
        Me.paMain.TabIndex = 2
        '
        'TaskDialogForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(804, 586)
        Me.Controls.Add(Me.tlpMain)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "TaskDialogForm"
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.tlpBottom.ResumeLayout(False)
        Me.tlpBottom.PerformLayout()
        Me.tlpTop.ResumeLayout(False)
        Me.tlpTop.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents tlpBottom As TableLayoutPanel
    Friend WithEvents llFooter As LinkLabel
    Friend WithEvents cbVerification As CheckBoxEx
    Friend WithEvents flpButtons As FlowLayoutPanel
    Friend WithEvents teInput As TextEdit
    Friend WithEvents tlpTop As TableLayoutPanel
    Friend WithEvents paIcon As Panel
    Friend WithEvents laMainInstruction As Label
    Friend WithEvents mbMenu As MenuButton
    Friend WithEvents paMain As TaskDialogPanel
End Class
