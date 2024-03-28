Imports System.ComponentModel

Namespace UI
    Public Class CriteriaControl
        Inherits FlowLayoutPanel

        <Browsable(False),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property AllCriteria As List(Of Criteria)

        Sub New()
            FlowDirection = FlowDirection.TopDown
            Dock = DockStyle.Fill
            AutoSize = False
            AutoScroll = False
            AutoScrollMinSize = New Size(0, 800)
            AutoSizeMode = AutoSizeMode.GrowAndShrink
            HorizontalScroll.Enabled = False
            HorizontalScroll.Visible = False
            HorizontalScroll.Maximum = 0
            VerticalScroll.Visible = True
            AutoScroll = True

            ApplyTheme()

            AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            RemoveHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
            MyBase.Dispose(disposing)
        End Sub

        Sub OnThemeChanged(theme As Theme)
            ApplyTheme(theme)
        End Sub

        Sub ApplyTheme()
            ApplyTheme(ThemeManager.CurrentTheme)
        End Sub

        Sub ApplyTheme(theme As Theme)
            If DesignHelp.IsDesignMode Then
                Exit Sub
            End If

            BackColor = theme.General.Controls.CriteriaControl.BackColor
            ForeColor = theme.General.Controls.CriteriaControl.ForeColor
        End Sub

        Sub AddItem(criteria As Criteria)
            Dim c As New CriteriaItemControl(AllCriteria) With {
                .Criteria = criteria,
                .Width = ClientSize.Width
            }
            Controls.Add(c)
        End Sub

        <Browsable(False),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property CriteriaList() As List(Of Criteria)
            Get
                Dim ret As New List(Of Criteria)

                For Each i In Controls.OfType(Of CriteriaItemControl)
                    If i.Criteria IsNot Nothing Then
                        ret.Add(i.Criteria)
                    End If
                Next

                Return ret
            End Get
            Set(Value As List(Of Criteria))
                If Value IsNot Nothing Then
                    For Each i As Criteria In Value
                        AddItem(i)
                    Next
                End If
            End Set
        End Property
    End Class
End Namespace