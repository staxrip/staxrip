Imports StaxRip.UI

Public Class SelectionBoxForm
    Inherits FormBase

#Region " Designer "

    Sub New()
        MyBase.New()
        InitializeComponent()
    End Sub

    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    Friend WithEvents bnCancel As System.Windows.Forms.Button
    Public WithEvents bnOK As System.Windows.Forms.Button
    Public WithEvents lText As System.Windows.Forms.Label
    Public WithEvents cbItems As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.lText = New System.Windows.Forms.Label()
        Me.bnCancel = New System.Windows.Forms.Button()
        Me.bnOK = New System.Windows.Forms.Button()
        Me.cbItems = New System.Windows.Forms.ComboBox()
        Me.SuspendLayout()
        '
        'lText
        '
        Me.lText.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lText.Location = New System.Drawing.Point(13, 6)
        Me.lText.Name = "lText"
        Me.lText.Size = New System.Drawing.Size(408, 40)
        Me.lText.TabIndex = 0
        Me.lText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bnCancel.Location = New System.Drawing.Point(321, 90)
        Me.bnCancel.Name = "bnCancel"
        Me.bnCancel.Size = New System.Drawing.Size(100, 34)
        Me.bnCancel.TabIndex = 3
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.bnOK.Location = New System.Drawing.Point(215, 90)
        Me.bnOK.Name = "bnOK"
        Me.bnOK.Size = New System.Drawing.Size(100, 34)
        Me.bnOK.TabIndex = 2
        Me.bnOK.Text = "OK"
        '
        'cbItems
        '
        Me.cbItems.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cbItems.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cbItems.FormattingEnabled = True
        Me.cbItems.Location = New System.Drawing.Point(12, 51)
        Me.cbItems.MaxDropDownItems = 20
        Me.cbItems.Name = "cbItems"
        Me.cbItems.Size = New System.Drawing.Size(409, 33)
        Me.cbItems.TabIndex = 1
        '
        'SelectionBoxForm
        '
        Me.AcceptButton = Me.bnOK
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(433, 136)
        Me.Controls.Add(Me.cbItems)
        Me.Controls.Add(Me.bnOK)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.lText)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SelectionBoxForm"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private ReturnValueValue As Object

    Property ReturnValue() As Object
        Get
            Return ReturnValueValue
        End Get
        Set(value As Object)
            ReturnValueValue = value
        End Set
    End Property

    Private Sub lText_TextChanged() Handles lText.TextChanged
        Dim g As Graphics = lText.CreateGraphics
        Dim s As SizeF = g.MeasureString(lText.Text, lText.Font, lText.Width)
        g.Dispose()

        If s.Height > lText.Height Then
            Height += CInt(s.Height - lText.Height)
        End If
    End Sub
End Class