﻿
Imports System.ComponentModel

Namespace UI
    <ToolboxItem(False)>
    Class CriteriaItemControl
        Sub New(allCriteria As List(Of Criteria))
            InitializeComponent()

            For Each c In allCriteria
                If c.Name?.StartsWith("Source ") Then
                    mbProperties.Add("Source | " + c.Name, c, c.Description)
                ElseIf c.Name?.StartsWith("Target ") Then
                    mbProperties.Add("Target | " + c.Name, c, c.Description)
                ElseIf c.Name?.StartsWith("Audio ") Then
                    mbProperties.Add("Audio | " + c.Name, c, c.Description)
                ElseIf c.Name?.StartsWith("Crop ") Then
                    mbProperties.Add("Crop | " + c.Name, c, c.Description)
                ElseIf c.Name?.StartsWith("Encoder ") Then
                    mbProperties.Add("Encoder | " + c.Name, c, c.Description)
                ElseIf c.Name?.StartsWith("AviSynth/VapourSynth") Then
                    mbProperties.Add("AviSynth/VapourSynth | " + c.Name, c, c.Description)
                ElseIf Not c.Name?.EndsWith(" Directory") AndAlso Not c.Name?.EndsWith(" File") Then
                    mbProperties.Add(c.Name, c, c.Description)
                End If

                If c.Name?.EndsWith(" Directory") Then
                    mbProperties.Add("Directories | " + c.Name, c, c.Description)
                End If

                If c.Name?.EndsWith(" Folder") Then
                    mbProperties.Add("Folders | " + c.Name, c, c.Description)
                End If

                If c.Name?.EndsWith(" File") Then
                    mbProperties.Add("Files | " + c.Name, c, c.Description)
                End If
            Next

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

        Protected Overrides Sub OnLayout(e As LayoutEventArgs)
            Height = bnRemove.Height + 6
            mbProperties.Height = bnRemove.Height
            tePropertiesIdentifier.Height = bnRemove.Height
            mbCondition.Height = bnRemove.Height
            teValue.Height = bnRemove.Height
            MyBase.OnLayout(e)
        End Sub

        Private CriteriaValue As Criteria

        Property Criteria As Criteria
            Get
                Return CriteriaValue
            End Get
            Set(Value As Criteria)
                If Value IsNot Nothing Then
                    CriteriaValue = DirectCast(ObjectHelp.GetCopy(Value), Criteria)

                    For Each c As Criteria In mbProperties.Items
                        If c.Name = Criteria.Name Then
                            mbProperties.Value = c
                            Exit For
                        End If
                    Next

                    tePropertiesIdentifier.Enabled = CriteriaValue.PropertyIdentifierNeeded
                    tePropertiesIdentifier.TextBox.SetTextWithoutTextChangedEvent(CriteriaValue.PropertyIdentifier)
                    tePropertiesIdentifier_TextChanged()

                    mbCondition.Enabled = True
                    mbCondition.Menu.Items.ClearAndDisplose
                    mbCondition.AddRange(CriteriaValue.ConditionNames)
                    mbCondition.Value = CriteriaValue.ConditionName

                    teValue.Enabled = True
                    teValue.TextBox.SetTextWithoutTextChangedEvent(CriteriaValue.ValueString)
                End If
            End Set
        End Property

        Sub bnRemove_Click() Handles bnRemove.Click
            Parent.Controls.Remove(Me)
        End Sub

        Sub teValue_TextChanged() Handles teValue.TextChanged
            Criteria.ValueString = teValue.Text
        End Sub

        Sub tePropertiesIdentifier_TextChanged() Handles tePropertiesIdentifier.TextChanged
            Criteria.PropertyIdentifier = tePropertiesIdentifier.Text

            If String.IsNullOrWhiteSpace(Criteria.PropertyIdentifier) AndAlso Criteria.PropertyIdentifierNeeded Then
                tePropertiesIdentifier.BackColor = ThemeManager.CurrentTheme.General.DangerForeColor
                tePropertiesIdentifier.TextBox.BackColor = ThemeManager.CurrentTheme.General.DangerForeColor
            Else
                tePropertiesIdentifier.BackColor = teValue.BackColor
                tePropertiesIdentifier.TextBox.BackColor = teValue.TextBox.BackColor
            End If
        End Sub

        Sub mbCondition_ValueChangedUser(value As Object) Handles mbCondition.ValueChangedUser
            Criteria.ConditionName = mbCondition.Value.ToString
        End Sub

        Sub mbProperties_ValueChangedUser(value As Object) Handles mbProperties.ValueChangedUser
            Criteria = DirectCast(value, Criteria)
        End Sub
    End Class
End Namespace
