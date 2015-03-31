Imports StaxRip.UI
Imports System.ComponentModel

Namespace UI
    <ToolboxItem(False)>
    Public Class CriteriaItemControl
        Private IsLoading As Boolean

        Sub New(allCriteria As List(Of Criteria))
            InitializeComponent()
            cbProperty.Items.AddRange(allCriteria.ToArray)
            buRemove.Text = "Remove"
        End Sub

        Private CriteriaValue As Criteria

        Property Criteria() As Criteria
            Get
                Return CriteriaValue
            End Get
            Set(Value As Criteria)
                If Not Value Is Nothing Then
                    CriteriaValue = DirectCast(ObjectHelp.GetCopy(Value), Criteria)
                    IsLoading = True

                    For i = 0 To cbProperty.Items.Count - 1
                        Dim c As Criteria = DirectCast(cbProperty.Items(i), Criteria)

                        If c.Name = CriteriaValue.Name Then
                            cbProperty.SelectedIndex = i
                            Exit For
                        End If
                    Next

                    cbCondition.Items.Clear()
                    cbCondition.Items.AddRange(CriteriaValue.ConditionNames)
                    cbCondition.SelectedItem = CriteriaValue.ConditionName
                    te.Text = CriteriaValue.ValueString

                    IsLoading = False
                End If
            End Set
        End Property

        Private Sub bRemove_Click() Handles buRemove.Click
            Parent.Controls.Remove(Me)
        End Sub

        Private Sub cbProperty_SelectedIndexChanged() Handles cbProperty.SelectedIndexChanged
            If Not IsLoading AndAlso Not cbProperty.SelectedItem Is Nothing Then
                Criteria = DirectCast(cbProperty.SelectedItem, Criteria)
            End If
        End Sub

        Private Sub cbCondition_SelectedIndexChanged() Handles cbCondition.SelectedIndexChanged
            If Not IsLoading AndAlso Not cbCondition.SelectedItem Is Nothing Then
                Criteria.ConditionName = cbCondition.SelectedItem.ToString
            End If
        End Sub

        Private Sub tbValue_TextChanged() Handles te.TextChanged
            If Not IsLoading Then
                Criteria.ValueString = te.Text
            End If
        End Sub

        Public Overrides Function GetPreferredSize(proposedSize As Size) As Size
            Dim ret = MyBase.GetPreferredSize(proposedSize)

            If Not Parent Is Nothing Then
                ret.Width = Parent.ClientSize.Width - 3
            End If

            Return ret
        End Function

        Private Sub te_Layout(sender As Object, e As LayoutEventArgs) Handles te.Layout
            te.Height = buRemove.Height
        End Sub
    End Class
End Namespace