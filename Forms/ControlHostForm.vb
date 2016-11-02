Imports StaxRip.UI

Class ControlHostForm
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

    Friend WithEvents pControl As System.Windows.Forms.Panel
    Friend WithEvents ButtonEx1 As StaxRip.UI.ButtonEx

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.pControl = New System.Windows.Forms.Panel()
        Me.ButtonEx1 = New StaxRip.UI.ButtonEx()
        Me.SuspendLayout()
        '
        'pControl
        '
        Me.pControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pControl.Location = New System.Drawing.Point(12, 12)
        Me.pControl.Name = "pControl"
        Me.pControl.Size = New System.Drawing.Size(318, 216)
        Me.pControl.TabIndex = 0
        '
        'ButtonEx1
        '
        Me.ButtonEx1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ButtonEx1.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonEx1.Location = New System.Drawing.Point(230, 234)
        Me.ButtonEx1.Size = New System.Drawing.Size(100, 34)
        Me.ButtonEx1.Text = "Close"
        '
        'ControlHostForm
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.ButtonEx1
        Me.ClientSize = New System.Drawing.Size(342, 280)
        Me.Controls.Add(Me.ButtonEx1)
        Me.Controls.Add(Me.pControl)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(6, 6, 6, 6)
        Me.Name = "ControlHostForm"
        Me.Text = "DialogForm"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private HelpAction As Action

    Sub New(title As String)
        MyBase.New()
        InitializeComponent()
        Text = title
    End Sub

    Sub AddControl(c As Control, helpAction As Action)
        c.Dock = DockStyle.Fill
        pControl.Controls.Add(c)
        Me.HelpAction = helpAction
        Me.HelpButton = Not helpAction Is Nothing
    End Sub

    Private Sub ControlHostForm_HelpRequested(sender As Object, e As HelpEventArgs) Handles Me.HelpRequested
        If Not HelpAction Is Nothing Then
            HelpAction()
        End If
    End Sub
End Class