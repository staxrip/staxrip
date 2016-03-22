Imports StaxRip.UI

Class SimpleSettingsForm
    Inherits DialogBase

#Region " Designer "

    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents SimpleUI As StaxRip.SimpleUI
    Friend WithEvents LineControl1 As StaxRip.UI.LineControl
    Private components As System.ComponentModel.IContainer
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.SimpleUI = New StaxRip.SimpleUI()
        Me.LineControl1 = New StaxRip.UI.LineControl()
        Me.SuspendLayout()
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(692, 506)
        Me.bnCancel.Size = New System.Drawing.Size(100, 34)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(586, 506)
        Me.bnOK.Size = New System.Drawing.Size(100, 34)
        Me.bnOK.Text = "OK"
        '
        'SimpleUI
        '
        Me.SimpleUI.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleUI.Location = New System.Drawing.Point(12, 12)
        Me.SimpleUI.Name = "SimpleUI"
        Me.SimpleUI.Size = New System.Drawing.Size(780, 464)
        Me.SimpleUI.TabIndex = 2
        '
        'LineControl1
        '
        Me.LineControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineControl1.Location = New System.Drawing.Point(13, 481)
        Me.LineControl1.Margin = New System.Windows.Forms.Padding(4, 2, 5, 2)
        Me.LineControl1.Name = "LineControl1"
        Me.LineControl1.Size = New System.Drawing.Size(777, 20)
        Me.LineControl1.TabIndex = 5
        '
        'SimpleSettingsForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(804, 552)
        Me.Controls.Add(Me.LineControl1)
        Me.Controls.Add(Me.SimpleUI)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.bnOK)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Margin = New System.Windows.Forms.Padding(0, 0, 0, 0)
        Me.Name = "SimpleSettingsForm"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private HelpParagraphs As String()

    Sub New(title As String, ParamArray helpParagraphs As String())
        InitializeComponent()
        Text = title
        Me.HelpParagraphs = helpParagraphs
        SimpleUI.Tree.Select()
    End Sub

    Private Sub SimpleSettingsForm_HelpRequested(sender As Object, e As HelpEventArgs) Handles Me.HelpRequested
        Dim f As New HelpForm()
        f.Doc.WriteStart(Text)

        If Not HelpParagraphs Is Nothing Then
            For Each i As String In HelpParagraphs
                f.Doc.WriteP(i)
            Next
        End If

        If Not SimpleUI.ActivePage.TipProvider Is Nothing Then
            f.Doc.WriteTips(SimpleUI.ActivePage.TipProvider.GetTips)
        End If

        f.Show()
    End Sub
End Class