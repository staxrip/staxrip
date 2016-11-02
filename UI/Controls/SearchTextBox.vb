Imports System.Drawing.Drawing2D
Imports StaxRip.UI

Class SearchTextBox
    Inherits UserControl

#Region "Designer"

    Private Sub InitializeComponent()
        Me.Edit = New StaxRip.UI.TextEdit()
        Me.Button = New StaxRip.SearchTextBox.SearchTextBoxButton()
        Me.SuspendLayout()
        '
        'Edit
        '
        Me.Edit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Edit.Location = New System.Drawing.Point(0, 0)
        Me.Edit.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Edit.Name = "Edit"
        Me.Edit.Size = New System.Drawing.Size(200, 70)
        Me.Edit.TabIndex = 3
        '
        'Button
        '
        Me.Button.Location = New System.Drawing.Point(90, 24)
        Me.Button.Name = "Button"
        Me.Button.Size = New System.Drawing.Size(27, 23)
        Me.Button.TabIndex = 2
        Me.Button.Text = "Button1"
        Me.Button.Visible = False
        '
        'SearchTextBox
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.Controls.Add(Me.Button)
        Me.Controls.Add(Me.Edit)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "SearchTextBox"
        Me.Size = New System.Drawing.Size(200, 70)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Edit As TextEdit
    Private Button As SearchTextBoxButton

    Public Sub New()
        InitializeComponent()
        Edit.TextBox.SendMessageCue("Search", False)
        AddHandler Edit.TextChanged, Sub() OnTextChanged(New EventArgs)
        AddHandler Button.Click, Sub() Edit.Text = ""
    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        Button.Visible = Edit.Text <> ""
        MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnLayout(e As LayoutEventArgs)
        MyBase.OnLayout(e)

        Button.Top = 3
        Button.Height = Height - 6
        Button.Width = Button.Height
        Button.Left = Width - Button.Width - Button.Top

        If Height <> Edit.Height Then Height = Edit.Height

        Edit.Width = Width
    End Sub

    Overrides Property Text As String
        Get
            Return Edit.Text
        End Get
        Set(value As String)
            Edit.Text = value
        End Set
    End Property

    Private Class SearchTextBoxButton
        Inherits Control

        Private MouseIsOver As Boolean

        Protected Overrides Sub OnMouseEnter(eventargs As EventArgs)
            MouseIsOver = True
            Refresh()
            MyBase.OnMouseEnter(eventargs)
        End Sub

        Protected Overrides Sub OnMouseLeave(eventargs As EventArgs)
            MouseIsOver = False
            Refresh()
            MyBase.OnMouseLeave(eventargs)
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Using p = New Pen(Color.DarkGray, 2)
                Dim offset = CSng(Width / 3.5)
                e.Graphics.DrawLine(p, offset, offset, Width - offset, Height - offset)
                e.Graphics.DrawLine(p, Width - offset, offset, offset, Height - offset)
            End Using
        End Sub

        Protected Overrides Sub OnPaintBackground(e As PaintEventArgs)
            If MouseIsOver Then
                Dim r = New Rectangle(Point.Empty, Size)

                Using path = ToolStripRendererEx.CreateRoundRectangle(New Rectangle(r.X, r.Y, r.Width - 1, r.Height - 1), 3)
                    Using b As New LinearGradientBrush(New Point(0, 0), New Point(0, r.Height), Color.White, Color.LightGray)
                        e.Graphics.FillPath(b, path)
                    End Using

                    Using p As New Pen(Brushes.LightGray)
                        e.Graphics.DrawPath(p, path)
                    End Using
                End Using
            Else
                e.Graphics.Clear(Color.White)
            End If
        End Sub
    End Class
End Class