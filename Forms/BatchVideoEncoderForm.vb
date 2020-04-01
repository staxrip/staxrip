Imports StaxRip.UI

Public Class BatchVideoEncoderForm
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
    Friend WithEvents EncodingCliControl As StaxRip.MacroEditorControl
    Friend WithEvents lType As System.Windows.Forms.Label
    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents CompCheckCliControl As StaxRip.MacroEditorControl
    Friend WithEvents cbQualityMode As System.Windows.Forms.CheckBox
    Friend WithEvents numPercent As NumEdit
    Friend WithEvents lPercent As System.Windows.Forms.Label
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents tlp As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tbType As StaxRip.UI.TextBoxEx

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.lType = New System.Windows.Forms.Label()
        Me.cbQualityMode = New System.Windows.Forms.CheckBox()
        Me.numPercent = New StaxRip.UI.NumEdit()
        Me.CompCheckCliControl = New StaxRip.MacroEditorControl()
        Me.EncodingCliControl = New StaxRip.MacroEditorControl()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.lPercent = New System.Windows.Forms.Label()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.tlp = New System.Windows.Forms.TableLayoutPanel()
        Me.tbType = New StaxRip.UI.TextBoxEx()
        Me.tlp.SuspendLayout()
        Me.SuspendLayout()
        '
        'lType
        '
        Me.lType.AutoSize = True
        Me.lType.Location = New System.Drawing.Point(12, 20)
        Me.lType.Name = "lType"
        Me.lType.Size = New System.Drawing.Size(147, 25)
        Me.lType.TabIndex = 0
        Me.lType.Text = "Output File Type:"
        '
        'cbQualityMode
        '
        Me.cbQualityMode.AutoSize = True
        Me.cbQualityMode.Location = New System.Drawing.Point(296, 20)
        Me.cbQualityMode.Name = "cbQualityMode"
        Me.cbQualityMode.Size = New System.Drawing.Size(146, 29)
        Me.cbQualityMode.TabIndex = 3
        Me.cbQualityMode.Text = "Quality Mode"
        Me.TipProvider.SetTipText(Me.cbQualityMode, "In Quality Mode bitrate related features are disabled.")
        Me.cbQualityMode.UseVisualStyleBackColor = True
        '
        'numPercent
        '
        Me.numPercent.Increment = New Decimal(New Integer() {5, 0, 0, 0})
        Me.numPercent.Location = New System.Drawing.Point(648, 18)
        Me.numPercent.Maximum = New Decimal(New Integer() {200, 0, 0, 0})
        Me.numPercent.Name = "numPercent"
        Me.numPercent.Size = New System.Drawing.Size(75, 31)
        Me.numPercent.TabIndex = 5
        '
        'CompCheckCliControl
        '
        Me.CompCheckCliControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.CompCheckCliControl.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CompCheckCliControl.Location = New System.Drawing.Point(3, 260)
        Me.CompCheckCliControl.Name = "CompCheckCliControl"
        Me.CompCheckCliControl.Size = New System.Drawing.Size(1234, 252)
        Me.CompCheckCliControl.TabIndex = 1
        Me.CompCheckCliControl.Text = "Compressibility Check Batch"
        '
        'EncodingCliControl
        '
        Me.EncodingCliControl.Dock = System.Windows.Forms.DockStyle.Fill
        Me.EncodingCliControl.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EncodingCliControl.Location = New System.Drawing.Point(3, 3)
        Me.EncodingCliControl.Name = "EncodingCliControl"
        Me.EncodingCliControl.Size = New System.Drawing.Size(1234, 251)
        Me.EncodingCliControl.TabIndex = 0
        Me.EncodingCliControl.Text = "Command Line"
        '
        'lPercent
        '
        Me.lPercent.AutoSize = True
        Me.lPercent.Location = New System.Drawing.Point(471, 21)
        Me.lPercent.Name = "lPercent"
        Me.lPercent.Size = New System.Drawing.Size(159, 25)
        Me.lPercent.TabIndex = 4
        Me.lPercent.Text = "Aimed Quality (%):"
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(1146, 577)
        Me.bnCancel.Size = New System.Drawing.Size(100, 36)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(1040, 577)
        Me.bnOK.Size = New System.Drawing.Size(100, 36)
        Me.bnOK.Text = "OK"
        '
        'tlp
        '
        Me.tlp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlp.ColumnCount = 1
        Me.tlp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlp.Controls.Add(Me.EncodingCliControl, 0, 0)
        Me.tlp.Controls.Add(Me.CompCheckCliControl, 0, 1)
        Me.tlp.Location = New System.Drawing.Point(9, 56)
        Me.tlp.Margin = New System.Windows.Forms.Padding(0, 3, 0, 3)
        Me.tlp.Name = "tlp"
        Me.tlp.RowCount = 2
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tlp.Size = New System.Drawing.Size(1240, 515)
        Me.tlp.TabIndex = 1
        '
        'tbType
        '
        Me.tbType.Location = New System.Drawing.Point(165, 17)
        Me.tbType.Size = New System.Drawing.Size(100, 31)
        '
        'BatchEncoderForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(1258, 623)
        Me.Controls.Add(Me.tbType)
        Me.Controls.Add(Me.tlp)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.bnOK)
        Me.Controls.Add(Me.lPercent)
        Me.Controls.Add(Me.numPercent)
        Me.Controls.Add(Me.cbQualityMode)
        Me.Controls.Add(Me.lType)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Name = "BatchEncoderForm"
        Me.Text = "Batch Encoder"
        Me.tlp.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Encoder As BatchEncoder

    Sub New(encoder As BatchEncoder)
        MyBase.New()
        InitializeComponent()
        Me.Encoder = encoder

        cbQualityMode.Checked = encoder.QualityMode
        tbType.Text = encoder.OutputExt

        EncodingCliControl.SetCommandLineDefaults()
        EncodingCliControl.Value = encoder.CommandLines

        CompCheckCliControl.SetCommandLineDefaults()
        CompCheckCliControl.Value = encoder.CompCheckCommandLines

        numPercent.Value = encoder.AutoCompCheckValue

        TipProvider.SetTip("Adjusts the target file size or image size after the compressibility check accordingly.", numPercent, lPercent)
    End Sub

    Private Sub CmdlEncoderForm_FormClosed() Handles Me.FormClosed
        If DialogResult = DialogResult.OK Then
            Encoder.OutputFileTypeValue = tbType.Text
            Encoder.CommandLines = EncodingCliControl.Value
            Encoder.CompCheckCommandLines = CompCheckCliControl.Value
            Encoder.QualityMode = cbQualityMode.Checked
            Encoder.AutoCompCheckValue = CInt(numPercent.Value)
        End If
    End Sub

    Private Sub BatchEncoderForm_HelpRequested() Handles Me.HelpRequested
        Dim form As New HelpForm
        form.Doc.WriteStart(Text)
        form.Doc.WriteP("The batch encoder allows executing a command line. If there is a piping symbol or line break then it's executed as batch file.")
        form.Doc.WriteTips(TipProvider.GetTips, EncodingCliControl.TipProvider.GetTips)
        form.Doc.WriteTable("Macros", Strings.MacrosHelp, Macro.GetTips())
        form.Show()
    End Sub

    Private Sub cbQualityMode_CheckedChanged(sender As Object, e As EventArgs) Handles cbQualityMode.CheckedChanged
        tlp.SuspendLayout()

        If cbQualityMode.Checked Then
            tlp.RowStyles(1).Height = 0
            Height -= CInt(tlp.Height / 2)
        Else
            tlp.RowStyles(1).Height = 50
            Height += CInt(tlp.Height)
        End If

        tlp.ResumeLayout()

        lPercent.Visible = Not cbQualityMode.Checked
        numPercent.Visible = Not cbQualityMode.Checked
    End Sub
End Class