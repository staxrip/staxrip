Imports StaxRip.UI

Class SimpleSettingsForm
    Inherits DialogBase

#Region " Designer "

    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents SimpleUI As StaxRip.SimpleUI
    Friend WithEvents LineControl1 As StaxRip.UI.LineControl
    Private components As System.ComponentModel.IContainer
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents FlowLayoutPanel1 As FlowLayoutPanel
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.SimpleUI = New StaxRip.SimpleUI()
        Me.LineControl1 = New StaxRip.UI.LineControl()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(106, 0)
        Me.bnCancel.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.bnCancel.Size = New System.Drawing.Size(100, 35)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(0, 0)
        Me.bnOK.Margin = New System.Windows.Forms.Padding(0)
        Me.bnOK.Size = New System.Drawing.Size(100, 35)
        Me.bnOK.Text = "OK"
        '
        'SimpleUI
        '
        Me.SimpleUI.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SimpleUI.Location = New System.Drawing.Point(6, 6)
        Me.SimpleUI.Margin = New System.Windows.Forms.Padding(6)
        Me.SimpleUI.Name = "SimpleUI"
        Me.SimpleUI.Size = New System.Drawing.Size(886, 555)
        Me.SimpleUI.TabIndex = 2
        '
        'LineControl1
        '
        Me.LineControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineControl1.Location = New System.Drawing.Point(6, 567)
        Me.LineControl1.Margin = New System.Windows.Forms.Padding(6, 0, 6, 0)
        Me.LineControl1.Name = "LineControl1"
        Me.LineControl1.Size = New System.Drawing.Size(886, 14)
        Me.LineControl1.TabIndex = 5
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.SimpleUI, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.LineControl1, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel1, 0, 2)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 3
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(898, 622)
        Me.TableLayoutPanel1.TabIndex = 8
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Right
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Controls.Add(Me.bnOK)
        Me.FlowLayoutPanel1.Controls.Add(Me.bnCancel)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(686, 581)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0, 0, 0, 6)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(212, 35)
        Me.FlowLayoutPanel1.TabIndex = 6
        '
        'SimpleSettingsForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(898, 622)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "SimpleSettingsForm"
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
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