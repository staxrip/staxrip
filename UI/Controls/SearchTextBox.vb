Imports System.Drawing.Drawing2D

Imports StaxRip.UI

Class SearchTextBox
    Inherits UserControl

    Private tb As TextBoxEx
    Private bn As Button

    Public Sub New()
        InitializeComponent()
        tb.SendMessageCue("Search", False)
        AddHandler tb.TextChanged, Sub() OnTextChanged(New EventArgs)
        AddHandler bn.Click, Sub() tb.Text = ""
    End Sub

    Private Sub InitializeComponent()
        Me.tb = New StaxRip.UI.TextBoxEx()
        Me.bn = New StaxRip.SearchTextBox.Button()
        Me.SuspendLayout()
        '
        'tb
        '
        Me.tb.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tb.Location = New System.Drawing.Point(0, 0)
        Me.tb.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.tb.Size = New System.Drawing.Size(236, 31)
        '
        'bn
        '
        Me.bn.Location = New System.Drawing.Point(170, 10)
        Me.bn.Name = "bn"
        Me.bn.Size = New System.Drawing.Size(75, 23)
        Me.bn.TabIndex = 2
        Me.bn.Text = "Button1"
        Me.bn.Visible = False
        '
        'SearchTextBox
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(144.0!, 144.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.Controls.Add(Me.bn)
        Me.Controls.Add(Me.tb)
        Me.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.Name = "SearchTextBox"
        Me.Size = New System.Drawing.Size(236, 31)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        bn.Visible = tb.Text <> ""
        MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnLayout(e As LayoutEventArgs)
        MyBase.OnLayout(e)

        bn.Top = 3
        bn.Height = Height - 6
        bn.Width = bn.Height
        bn.Left = Width - bn.Width - bn.Top

        If Height <> tb.Height Then Height = tb.Height

        tb.Width = Width
    End Sub

    Overrides Property Text As String
        Get
            Return tb.Text
        End Get
        Set(value As String)
            tb.Text = value
        End Set
    End Property

    Private Class Button
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