Imports StaxRip.UI

Public Class EventCommandsEditor
    Inherits DialogBase

#Region " Designer "

    Friend WithEvents lb As StaxRip.UI.ListBoxEx
    Friend WithEvents bnAdd As ButtonEx
    Friend WithEvents bnRemove As ButtonEx
    Friend WithEvents bnClone As ButtonEx
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents LineControl1 As StaxRip.UI.LineControl
    Friend WithEvents bnEdit As ButtonEx

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.bnEdit = New StaxRip.UI.ButtonEx()
        Me.bnClone = New StaxRip.UI.ButtonEx()
        Me.bnRemove = New StaxRip.UI.ButtonEx()
        Me.bnAdd = New StaxRip.UI.ButtonEx()
        Me.lb = New StaxRip.UI.ListBoxEx()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.LineControl1 = New StaxRip.UI.LineControl()
        Me.SuspendLayout()
        '
        'bnEdit
        '
        Me.bnEdit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnEdit.Location = New System.Drawing.Point(421, 92)
        Me.bnEdit.Size = New System.Drawing.Size(100, 34)
        Me.bnEdit.Text = "Edit..."
        '
        'bnClone
        '
        Me.bnClone.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnClone.Location = New System.Drawing.Point(421, 132)
        Me.bnClone.Size = New System.Drawing.Size(100, 34)
        Me.bnClone.Text = "Clone"
        '
        'bnRemove
        '
        Me.bnRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnRemove.Location = New System.Drawing.Point(421, 52)
        Me.bnRemove.Size = New System.Drawing.Size(100, 34)
        Me.bnRemove.Text = "Remove"
        '
        'bnAdd
        '
        Me.bnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnAdd.Location = New System.Drawing.Point(421, 12)
        Me.bnAdd.Size = New System.Drawing.Size(100, 34)
        Me.bnAdd.Text = "Add..."
        '
        'lb
        '
        Me.lb.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lb.Button1 = Me.bnEdit
        Me.lb.Button2 = Me.bnClone
        Me.lb.FormattingEnabled = True
        Me.lb.IntegralHeight = False
        Me.lb.ItemHeight = 30
        Me.lb.Location = New System.Drawing.Point(12, 12)
        Me.lb.Name = "lb"
        Me.lb.RemoveButton = Me.bnRemove
        Me.lb.Size = New System.Drawing.Size(403, 378)
        Me.lb.TabIndex = 0
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(421, 419)
        Me.bnCancel.Size = New System.Drawing.Size(100, 34)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(315, 419)
        Me.bnOK.Size = New System.Drawing.Size(100, 34)
        Me.bnOK.Text = "OK"
        '
        'LineControl1
        '
        Me.LineControl1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LineControl1.Location = New System.Drawing.Point(12, 395)
        Me.LineControl1.Margin = New System.Windows.Forms.Padding(4, 2, 5, 2)
        Me.LineControl1.Name = "LineControl1"
        Me.LineControl1.Size = New System.Drawing.Size(509, 17)
        Me.LineControl1.TabIndex = 7
        '
        'EventCommandsEditor
        '
        Me.AcceptButton = Me.bnOK
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(533, 465)
        Me.Controls.Add(Me.LineControl1)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.bnOK)
        Me.Controls.Add(Me.lb)
        Me.Controls.Add(Me.bnEdit)
        Me.Controls.Add(Me.bnClone)
        Me.Controls.Add(Me.bnRemove)
        Me.Controls.Add(Me.bnAdd)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.MinimumSize = New System.Drawing.Size(239, 219)
        Me.Name = "EventCommandsEditor"
        Me.Text = "Event Commands"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Sub New(actions As List(Of EventCommand))
        MyBase.New()
        InitializeComponent()

        lb.Items.AddRange(DirectCast(ObjectHelp.GetCopy(actions), List(Of EventCommand)).ToArray)

        If lb.Items.Count > 0 Then
            lb.SelectedIndex = 0
        End If

        lb.UpdateControls()
    End Sub

    Private Sub EventCommandsEditor_HelpRequested() Handles Me.HelpRequested
        Dim f As New HelpForm
        f.Doc.WriteStart(Text)
        f.Doc.WriteP(Strings.EventCommands)
        f.Show()
    End Sub

    Private Sub bAdd_Click() Handles bnAdd.Click
        Dim ec As New EventCommand

        Using f As New EventCommandEditor(ec)
            If f.ShowDialog = DialogResult.OK Then
                lb.SelectedIndex = lb.Items.Add(ec)
            End If
        End Using
    End Sub

    Private Sub bEdit_Click() Handles bnEdit.Click
        Using f As New EventCommandEditor(DirectCast(lb.SelectedItem, EventCommand))
            If f.ShowDialog = DialogResult.OK Then
                lb.UpdateSelection()
            End If
        End Using
    End Sub

    Private Sub bClone_Click() Handles bnClone.Click
        lb.SelectedIndex = lb.Items.Add(ObjectHelp.GetCopy(lb.SelectedItem))
    End Sub
End Class