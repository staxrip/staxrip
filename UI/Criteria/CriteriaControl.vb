Imports System.ComponentModel

Namespace UI
    Public Class CriteriaControl
        Inherits FlowLayoutPanel

        <Browsable(False),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property AllCrieria As List(Of Criteria)

        Sub New()
            FlowDirection = FlowDirection.TopDown
            BackColor = SystemColors.Window
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