Imports System.ComponentModel

Namespace UI
    Public Class CriteriaControl
        Inherits FlowLayoutPanel

        <Browsable(False),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property AllCrieria As List(Of Criteria)

        Sub New()
            FlowDirection = FlowDirection.TopDown
            ApplyTheme()

            AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
        End Sub

        Sub OnThemeChanged(theme As Theme)
            ApplyTheme(theme)
        End Sub

        Sub ApplyTheme()
            ApplyTheme(ThemeManager.CurrentTheme)
        End Sub

        Sub ApplyTheme(theme As Theme)
            BackColor = theme.General.Controls.CriteriaControl.BackColor
            ForeColor = theme.General.Controls.CriteriaControl.ForeColor
        End Sub

        Sub AddItem(criteria As Criteria)
            Dim c As New CriteriaItemControl(AllCrieria)
            c.Criteria = criteria
            c.Width = ClientSize.Width
            Controls.Add(c)
        End Sub

        <Browsable(False),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property CriteriaList() As List(Of Criteria)
            Get
                Dim ret As New List(Of Criteria)

                For Each i In Controls.OfType(Of CriteriaItemControl)
                    If Not i.Criteria Is Nothing Then
                        ret.Add(i.Criteria)
                    End If
                Next

                Return ret
            End Get
            Set(Value As List(Of Criteria))
                If Not Value Is Nothing Then
                    For Each i As Criteria In Value
                        AddItem(i)
                    Next
                End If
            End Set
        End Property
    End Class
End Namespace