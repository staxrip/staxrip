﻿
Imports StaxRip.UI

Public Class VvencffappControl
    Inherits UserControl

#Region " Designer "
    Friend WithEvents lv As StaxRip.UI.ListViewEx
    Friend WithEvents blConfigCodec As ButtonLabel
    Friend WithEvents blConfigContainer As ButtonLabel
    Friend WithEvents blCompCheck As ButtonLabel

    Private components As System.ComponentModel.IContainer

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.blConfigCodec = New StaxRip.UI.ButtonLabel()
        Me.blConfigContainer = New StaxRip.UI.ButtonLabel()
        Me.blCompCheck = New StaxRip.UI.ButtonLabel()
        Me.lv = New StaxRip.UI.ListViewEx()
        Me.SuspendLayout()
        '
        'llConfigCodec
        '
        Me.blConfigCodec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.blConfigCodec.AutoSize = True
        Me.blConfigCodec.Location = New System.Drawing.Point(3, 408)
        Me.blConfigCodec.Margin = New System.Windows.Forms.Padding(3)
        Me.blConfigCodec.Name = "llConfigCodec"
        Me.blConfigCodec.Size = New System.Drawing.Size(128, 37)
        Me.blConfigCodec.TabIndex = 1
        Me.blConfigCodec.TabStop = True
        Me.blConfigCodec.Text = "Options"
        '
        'llConfigContainer
        '
        Me.blConfigContainer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.blConfigContainer.AutoSize = True
        Me.blConfigContainer.Location = New System.Drawing.Point(346, 408)
        Me.blConfigContainer.Margin = New System.Windows.Forms.Padding(3)
        Me.blConfigContainer.Name = "llConfigContainer"
        Me.blConfigContainer.Size = New System.Drawing.Size(276, 37)
        Me.blConfigContainer.TabIndex = 2
        Me.blConfigContainer.TabStop = True
        Me.blConfigContainer.Text = "Container Options"
        '
        'llCompCheck
        '
        Me.blCompCheck.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.blCompCheck.AutoSize = True
        Me.blCompCheck.Location = New System.Drawing.Point(3, 365)
        Me.blCompCheck.Margin = New System.Windows.Forms.Padding(3)
        Me.blCompCheck.Name = "llCompCheck"
        Me.blCompCheck.Size = New System.Drawing.Size(399, 37)
        Me.blCompCheck.TabIndex = 3
        Me.blCompCheck.TabStop = True
        Me.blCompCheck.Text = "Run Compressibility Check"
        '
        'lv
        '
        Me.lv.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lv.HideSelection = False
        Me.lv.Location = New System.Drawing.Point(0, 0)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(625, 448)
        Me.lv.TabIndex = 0
        Me.lv.UseCompatibleStateImageBehavior = False
        '
        'VvencffappControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.blConfigContainer)
        Me.Controls.Add(Me.blConfigCodec)
        Me.Controls.Add(Me.blCompCheck)
        Me.Controls.Add(Me.lv)
        Me.Name = "VvencffappControl"
        Me.Size = New System.Drawing.Size(625, 448)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Encoder As VvencffappEnc
    Private Params As VvencffappParams
    Private cms As ContextMenuStripEx
    Private QualityDefinitions As List(Of QualityItem)

    Sub New(enc As VvencffappEnc)
        InitializeComponent()
        components = New ComponentModel.Container()

        If Not s.VvencffappQualityDefinitions Is Nothing AndAlso s.VvencffappQualityDefinitions.Any() Then
            QualityDefinitions = s.VvencffappQualityDefinitions
        Else
            QualityDefinitions = New List(Of QualityItem) From {
                New QualityItem(16, "Super High", "Super high quality and file size"),
                New QualityItem(20, "Very High", "Very high quality and file size"),
                New QualityItem(24, "Higher", "Higher quality and file size"),
                New QualityItem(28, "High", "High quality and file size"),
                New QualityItem(32, "Medium (Default)", "Medium quality and file size (Default)"),
                New QualityItem(36, "Low", "Low quality and file size"),
                New QualityItem(40, "Lower", "Lower quality and file size"),
                New QualityItem(44, "Very Low", "Very low quality and file size"),
                New QualityItem(48, "Super Low", "Super low quality and file size"),
                New QualityItem(52, "Extreme Low", "Extreme low quality and file size"),
                New QualityItem(56, "Ultra Low", "Ultra low quality and file size")}
        End If

        Encoder = enc
        Params = Encoder.Params

        cms = New ContextMenuStripEx(components)
        cms.Font = FontManager.GetDefaultFont()

        lv.View = View.Details
        lv.HeaderStyle = ColumnHeaderStyle.None
        lv.FullRowSelect = True
        lv.MultiSelect = False
        lv.ContextMenuStrip = cms
        lv.ShowContextMenuOnLeftClick = True

        UpdateControls()
        AddHandler lv.UpdateContextMenu, AddressOf UpdateMenu

        ApplyTheme()

        AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        RemoveHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
        components?.Dispose()
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

        SuspendLayout()

        For Each i In {blCompCheck, blConfigContainer, blConfigCodec}
            i.ForeColor = theme.General.Controls.ButtonLabel.ForeColor
            i.LinkColor = theme.General.Controls.ButtonLabel.LinkForeColor
            i.LinkHoverColor = theme.General.Controls.ButtonLabel.LinkForeHoverColor
            i.BackColor = theme.General.Controls.ListView.BackColor
        Next

        ResumeLayout()
    End Sub

    Protected Overrides Sub OnLayout(e As LayoutEventArgs)
        MyBase.OnLayout(e)

        If lv.Columns.Count = 0 Then
            lv.Columns.AddRange({New ColumnHeader, New ColumnHeader})
        End If

        lv.Columns(0).Width = CInt(Width * (32 / 100))
        lv.Columns(1).Width = CInt(Width * (66 / 100))

        Dim fh = FontHeight
        blConfigCodec.Left = fh \ 4
        blConfigCodec.Top = Height - blConfigCodec.Height - fh \ 4

        blCompCheck.Left = fh \ 4
        blCompCheck.Top = Height - blConfigCodec.Height - blCompCheck.Height - (fh \ 4) * 2

        blConfigContainer.Left = Width - blConfigContainer.Width - fh \ 4
        blConfigContainer.Top = Height - blConfigContainer.Height - fh \ 4
    End Sub

    Sub UpdateMenu()
        cms.Items.ClearAndDisplose
        Dim offset = If(Params.Mode.Value = VvencffappRateMode.SingleQuant, 0, 1)

        If lv.SelectedItems.Count > 0 Then
            Select Case lv.SelectedIndices(0)
                Case 0 - offset
                    For Each def In QualityDefinitions
                        cms.Items.Add(New MenuItemEx(def.Value & If(Not String.IsNullOrWhiteSpace(def.Text), $" - {def.Text}      ", "      "), Sub() SetQuality(def.Value), def.Tooltip) With {.Font = If(Params.Quant.Value = def.Value, FontManager.GetDefaultFont(9, FontStyle.Bold), FontManager.GetDefaultFont())})
                    Next
                Case 1 - offset
                    For x = 0 To Params.Preset.Options.Length - 1
                        Dim temp = x
                        Dim presetMenuItem = New MenuItemEx(Params.Preset.Options(x) + "      ", Sub() SetPreset(temp), "") With {.Font = If(Params.Preset.Value = x, FontManager.GetDefaultFont(9, FontStyle.Bold), FontManager.GetDefaultFont())}
                        cms.Items.Add(presetMenuItem)
                    Next
                    'Case 2 - offset
                    '    For x = 0 To Params.Tune.Options.Length - 1
                    '        Dim temp = x
                    '        cms.Items.Add(New MenuItemEx(Params.Tune.Options(x) + "      ", Sub() SetTune(temp)) With {.Font = If(Params.Tune.Value = x, FontManager.GetDefaultFont(9, FontStyle.Bold), FontManager.GetDefaultFont())})
                    '    Next
            End Select
        End If
    End Sub

    Sub SetQuality(v As Double)
        Params.Quant.Value = v
        lv.Items(0).SubItems(1).Text = GetQualityCaption(v)
        lv.Items(0).Selected = False
        UpdateControls()
    End Sub

    Sub SetPreset(value As Integer)
        Dim offset = If(Params.Mode.Value = VvencffappRateMode.SingleQuant, 0, 1)

        Params.Preset.Value = value

        Params.ApplyPresetValues()

        lv.Items(1 - offset).SubItems(1).Text = value.ToString
        lv.Items(1 - offset).Selected = False

        UpdateControls()
    End Sub

    Function GetQualityCaption(value As Double) As String
        For Each def In QualityDefinitions
            If def.Value = value Then
                Return value & If(Not String.IsNullOrWhiteSpace(def.Text), $" - {def.Text}", "")
            End If
        Next

        Return value.ToString
    End Function

    Sub UpdateControls()
        If Params.Mode.Value = VvencffappRateMode.SingleQuant AndAlso lv.Items.Count < 2 Then
            lv.Items.Clear()
            lv.Items.Add(New ListViewItem({"Quality", GetQualityCaption(Params.Quant.Value)}))
            lv.Items.Add(New ListViewItem({"Preset", Params.Preset.OptionText}))
        ElseIf Params.Mode.Value <> VvencffappRateMode.TwoPass AndAlso lv.Items.Count <> 1 Then
            lv.Items.Clear()
            lv.Items.Add(New ListViewItem({"Preset", Params.Preset.OptionText}))
        End If

        Dim offset = If(Params.Mode.Value = VvencffappRateMode.SingleQuant, 0, 1)
        blCompCheck.Visible = Params.Mode.Value = VvencffappRateMode.TwoPass
    End Sub

    Sub llConfigCodec_Click(sender As Object, e As EventArgs) Handles blConfigCodec.Click
        Encoder.ShowConfigDialog()
    End Sub

    Sub llConfigContainer_Click(sender As Object, e As EventArgs) Handles blConfigContainer.Click
        Encoder.OpenMuxerConfigDialog()
    End Sub

    Sub llCompCheck_Click(sender As Object, e As EventArgs) Handles blCompCheck.Click
        Encoder.RunCompCheck()
    End Sub

    <Serializable>
    Public Class QualityItem
        Property Value As Double
        Property Text As String
        Property Tooltip As String

        Sub New(value As Double, text As String, tooltip As String)
            Me.Value = value
            Me.Text = text
            Me.Tooltip = tooltip
        End Sub
    End Class
End Class
