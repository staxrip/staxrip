
Imports StaxRip.UI

Public Class CommandLineVideoEncoderForm
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
    Friend WithEvents EncodingControl As StaxRip.MacroEditorControl
    Friend WithEvents laType As System.Windows.Forms.Label
    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents CompCheckControl As StaxRip.MacroEditorControl
    Friend WithEvents cbQualityMode As System.Windows.Forms.CheckBox
    Friend WithEvents numPercent As NumEdit
    Friend WithEvents laPercent As System.Windows.Forms.Label
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents tlpMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tbType As StaxRip.UI.TextBoxEx
    Friend WithEvents tlpTop As TableLayoutPanel
    Friend WithEvents flpBottom As FlowLayoutPanel
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.laType = New System.Windows.Forms.Label()
        Me.cbQualityMode = New System.Windows.Forms.CheckBox()
        Me.numPercent = New StaxRip.UI.NumEdit()
        Me.CompCheckControl = New StaxRip.MacroEditorControl()
        Me.EncodingControl = New StaxRip.MacroEditorControl()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.laPercent = New System.Windows.Forms.Label()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.flpBottom = New System.Windows.Forms.FlowLayoutPanel()
        Me.tlpTop = New System.Windows.Forms.TableLayoutPanel()
        Me.tbType = New StaxRip.UI.TextBoxEx()
        Me.tlpMain.SuspendLayout()
        Me.flpBottom.SuspendLayout()
        Me.tlpTop.SuspendLayout()
        Me.SuspendLayout()
        '
        'laType
        '
        Me.laType.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.laType.AutoSize = True
        Me.laType.Location = New System.Drawing.Point(20, 26)
        Me.laType.Margin = New System.Windows.Forms.Padding(20, 0, 3, 0)
        Me.laType.Name = "laType"
        Me.laType.Size = New System.Drawing.Size(289, 48)
        Me.laType.TabIndex = 0
        Me.laType.Text = "Output File Type:"
        '
        'cbQualityMode
        '
        Me.cbQualityMode.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.cbQualityMode.AutoSize = True
        Me.cbQualityMode.Location = New System.Drawing.Point(468, 24)
        Me.cbQualityMode.Margin = New System.Windows.Forms.Padding(50, 3, 3, 3)
        Me.cbQualityMode.Name = "cbQualityMode"
        Me.cbQualityMode.Size = New System.Drawing.Size(281, 52)
        Me.cbQualityMode.TabIndex = 3
        Me.cbQualityMode.Text = "Quality Mode"
        Me.TipProvider.SetTipText(Me.cbQualityMode, "In Quality Mode bitrate related features are disabled.")
        Me.cbQualityMode.UseVisualStyleBackColor = True
        '
        'numPercent
        '
        Me.numPercent.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.numPercent.Increment = 5.0R
        Me.numPercent.Location = New System.Drawing.Point(1122, 20)
        Me.numPercent.Maximum = 200.0R
        Me.numPercent.Name = "numPercent"
        Me.numPercent.Size = New System.Drawing.Size(140, 60)
        Me.numPercent.TabIndex = 5
        '
        'CompCheckControl
        '
        Me.CompCheckControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CompCheckControl.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CompCheckControl.Location = New System.Drawing.Point(15, 422)
        Me.CompCheckControl.Margin = New System.Windows.Forms.Padding(15, 15, 15, 0)
        Me.CompCheckControl.Name = "CompCheckControl"
        Me.CompCheckControl.Size = New System.Drawing.Size(1363, 286)
        Me.CompCheckControl.TabIndex = 1
        Me.CompCheckControl.Text = "Compressibility Check"
        '
        'EncodingControl
        '
        Me.EncodingControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EncodingControl.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EncodingControl.Location = New System.Drawing.Point(15, 106)
        Me.EncodingControl.Margin = New System.Windows.Forms.Padding(15, 0, 15, 0)
        Me.EncodingControl.Name = "EncodingControl"
        Me.EncodingControl.Size = New System.Drawing.Size(1363, 301)
        Me.EncodingControl.TabIndex = 0
        Me.EncodingControl.Text = "Command Line"
        '
        'laPercent
        '
        Me.laPercent.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.laPercent.AutoSize = True
        Me.laPercent.Location = New System.Drawing.Point(802, 26)
        Me.laPercent.Margin = New System.Windows.Forms.Padding(50, 0, 3, 0)
        Me.laPercent.Name = "laPercent"
        Me.laPercent.Size = New System.Drawing.Size(314, 48)
        Me.laPercent.TabIndex = 4
        Me.laPercent.Text = "Aimed Quality (%):"
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(265, 0)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(15, 0, 0, 0)
        Me.bnCancel.Size = New System.Drawing.Size(250, 70)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(0, 0)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(0)
        Me.bnOK.Size = New System.Drawing.Size(250, 70)
        Me.bnOK.Text = "OK"
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 1
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Controls.Add(Me.flpBottom, 0, 3)
        Me.tlpMain.Controls.Add(Me.tlpTop, 0, 0)
        Me.tlpMain.Controls.Add(Me.EncodingControl, 0, 1)
        Me.tlpMain.Controls.Add(Me.CompCheckControl, 0, 2)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 4
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(1393, 809)
        Me.tlpMain.TabIndex = 1
        '
        'flpBottom
        '
        Me.flpBottom.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.flpBottom.AutoSize = True
        Me.flpBottom.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.flpBottom.Controls.Add(Me.bnOK)
        Me.flpBottom.Controls.Add(Me.bnCancel)
        Me.flpBottom.Location = New System.Drawing.Point(863, 723)
        Me.flpBottom.Margin = New System.Windows.Forms.Padding(15)
        Me.flpBottom.Name = "flpBottom"
        Me.flpBottom.Size = New System.Drawing.Size(515, 70)
        Me.flpBottom.TabIndex = 4
        '
        'tlpTop
        '
        Me.tlpTop.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlpTop.ColumnCount = 6
        Me.tlpTop.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpTop.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpTop.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpTop.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpTop.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpTop.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpTop.Controls.Add(Me.laType, 0, 0)
        Me.tlpTop.Controls.Add(Me.tbType, 1, 0)
        Me.tlpTop.Controls.Add(Me.cbQualityMode, 2, 0)
        Me.tlpTop.Controls.Add(Me.laPercent, 3, 0)
        Me.tlpTop.Controls.Add(Me.numPercent, 4, 0)
        Me.tlpTop.Location = New System.Drawing.Point(3, 3)
        Me.tlpTop.Name = "tlpTop"
        Me.tlpTop.RowCount = 1
        Me.tlpTop.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpTop.Size = New System.Drawing.Size(1387, 100)
        Me.tlpTop.TabIndex = 6
        '
        'tbType
        '
        Me.tbType.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.tbType.Location = New System.Drawing.Point(315, 22)
        Me.tbType.Size = New System.Drawing.Size(100, 55)
        Me.tbType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'CommandLineVideoEncoderForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1393, 809)
        Me.Controls.Add(Me.tlpMain)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(6)
        Me.Name = "CommandLineVideoEncoderForm"
        Me.Text = "Command Line Video Encoder"
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.flpBottom.ResumeLayout(False)
        Me.tlpTop.ResumeLayout(False)
        Me.tlpTop.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Encoder As BatchEncoder

    Sub New(encoder As BatchEncoder)
        MyBase.New()
        InitializeComponent()
        RestoreClientSize(50, 22)
        Me.Encoder = encoder

        cbQualityMode.Checked = encoder.QualityMode
        tbType.Text = encoder.OutputExt

        EncodingControl.SetCommandLineDefaults()
        EncodingControl.Value = encoder.CommandLines

        CompCheckControl.SetCommandLineDefaults()
        CompCheckControl.Value = encoder.CompCheckCommandLines

        numPercent.Value = encoder.AutoCompCheckValue

        TipProvider.SetTip("Adjusts the target file size or image size after the compressibility check accordingly.", numPercent, laPercent)
    End Sub

    Protected Overrides Sub OnFormClosed(e As FormClosedEventArgs)
        If DialogResult = DialogResult.OK Then
            Encoder.OutputFileTypeValue = tbType.Text
            Encoder.CommandLines = EncodingControl.Value
            Encoder.CompCheckCommandLines = CompCheckControl.Value
            Encoder.QualityMode = cbQualityMode.Checked
            Encoder.AutoCompCheckValue = CInt(numPercent.Value)
        End If
    End Sub

    Sub cbQualityMode_CheckedChanged(sender As Object, e As EventArgs) Handles cbQualityMode.CheckedChanged
        tlpMain.SuspendLayout()

        If cbQualityMode.Checked Then
            tlpMain.RowStyles(2).Height = 0
        Else
            tlpMain.RowStyles(2).Height = 50
        End If

        CompCheckControl.Visible = Not cbQualityMode.Checked
        tlpMain.ResumeLayout()

        laPercent.Visible = Not cbQualityMode.Checked
        numPercent.Visible = Not cbQualityMode.Checked
    End Sub
End Class
