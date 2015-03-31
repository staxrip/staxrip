Imports System.ComponentModel

Namespace UI
    Public Class CriteriaControl
        Inherits PanelEx

        Private buAdd As StaxRip.UI.ButtonEx
        Private ControlList As New List(Of CriteriaItemControl)

        <Browsable(False),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property AllCrieria As List(Of Criteria)

        Sub New()
            BackColor = SystemColors.Window
            ShowNiceBorder = True

            buAdd = New StaxRip.UI.ButtonEx()
            buAdd.Anchor = AnchorStyles.Right Or AnchorStyles.Top
            buAdd.AddClickAction(Sub() AddItem(Nothing))
            buAdd.AutoSize = True
            buAdd.AutoSizeMode = AutoSizeMode.GrowAndShrink
            buAdd.Text = "Add"
            buAdd.Left = ClientSize.Width - buAdd.Size.Width - 6
            SetAddButtonTop()
            Controls.Add(buAdd)
        End Sub

        Private Sub AddItem(criteria As Criteria)
            Dim c As New CriteriaItemControl(AllCrieria)
            ControlList.Add(c)
            c.Criteria = criteria
            c.Anchor = AnchorStyles.Left Or AnchorStyles.Top Or AnchorStyles.Right
            c.Left = 2
            c.Width = ClientSize.Width - 3
            c.Top = GetTop(c)
            Controls.Add(c)
            SetAddButtonTop()
            buAdd.Left = c.Right - c.buRemove.Margin.Right - buAdd.Width - 1
        End Sub

        Private Sub SetAddButtonTop()
            If ControlList.Count = 0 Then
                buAdd.Top = 6
            Else
                Dim c = ControlList(ControlList.Count - 1)
                buAdd.Visible = Not (c.Top + c.Height + 2) + buAdd.Height > Height
                buAdd.Top = c.Top + c.Height + 2
            End If
        End Sub

        Private Function GetTop(c As CriteriaItemControl) As Integer
            Dim ret = c.Margin.Top + Padding.Top + 2

            If ControlList.IndexOf(c) > 0 Then
                Dim above = ControlList(ControlList.IndexOf(c) - 1)
                ret = above.Top + c.Height
            End If

            Return ret
        End Function

        Private Sub ItemsPanel_ControlRemoved(sender As Object, e As ControlEventArgs) Handles Me.ControlRemoved
            Dim c = DirectCast(e.Control, CriteriaItemControl)

            If ControlList.Contains(c) Then
                ControlList.Remove(c)
            End If

            For Each i In ControlList
                i.Top = GetTop(i)
            Next

            SetAddButtonTop()
        End Sub

        <Browsable(False),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property CriteriaList() As List(Of Criteria)
            Get
                Dim ret As New List(Of Criteria)

                For Each i In ControlList
                    If Not i.Criteria Is Nothing Then
                        ret.Add(i.Criteria)
                    End If
                Next

                Return ret
            End Get
            Set(Value As List(Of Criteria))
                While ControlList.Count > 0
                    ControlList(ControlList.Count - 1).buRemove.PerformClick()
                End While

                If Not Value Is Nothing Then
                    For Each i As Criteria In Value
                        AddItem(i)
                    Next
                End If
            End Set
        End Property
    End Class
End Namespace