
Imports System.Web.UI.WebControls.WebParts
Imports StaxRip.UI
Imports StaxRip.VideoEncoderCommandLine

Public Class x264Control
    Inherits UserControl

#Region " Designer "
    Friend WithEvents lv As StaxRip.UI.ListViewEx
    Friend WithEvents blConfigCodec As ButtonLabel
    Friend WithEvents blConfigContainer As ButtonLabel
    Friend WithEvents blCompCheck As ButtonLabel
    Friend WithEvents tblOverrideName As ToggleButtonLabel

    Private components As System.ComponentModel.IContainer

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.blConfigCodec = New StaxRip.UI.ButtonLabel()
        Me.blConfigContainer = New StaxRip.UI.ButtonLabel()
        Me.blCompCheck = New StaxRip.UI.ButtonLabel()
        Me.tblOverrideName = New StaxRip.UI.ToggleButtonLabel()
        Me.lv = New StaxRip.UI.ListViewEx()
        Me.SuspendLayout()
        '
        'blConfigCodec
        '
        Me.blConfigCodec.Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Left), AnchorStyles)
        Me.blConfigCodec.AutoSize = True
        Me.blConfigCodec.Location = New System.Drawing.Point(3, 223)
        Me.blConfigCodec.Margin = New Padding(3)
        Me.blConfigCodec.Name = "blConfigCodec"
        Me.blConfigCodec.Size = New System.Drawing.Size(128, 37)
        Me.blConfigCodec.TabIndex = 1
        Me.blConfigCodec.TabStop = True
        Me.blConfigCodec.Text = "Options"
        '
        'blOverrideName
        '
        Me.tblOverrideName.Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right), AnchorStyles)
        Me.tblOverrideName.AutoSize = True
        Me.tblOverrideName.Location = New System.Drawing.Point(3, 223)
        Me.tblOverrideName.Margin = New Padding(3)
        Me.tblOverrideName.Name = "blOverrideName"
        Me.tblOverrideName.Size = New System.Drawing.Size(128, 37)
        Me.tblOverrideName.TabIndex = 2
        Me.tblOverrideName.TabStop = True
        Me.tblOverrideName.Text = "Name Override"
        '
        'llConfigContainer
        '
        Me.blConfigContainer.Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Right), AnchorStyles)
        Me.blConfigContainer.AutoSize = True
        Me.blConfigContainer.Location = New System.Drawing.Point(280, 223)
        Me.blConfigContainer.Margin = New Padding(3)
        Me.blConfigContainer.Name = "llConfigContainer"
        Me.blConfigContainer.Size = New System.Drawing.Size(276, 37)
        Me.blConfigContainer.TabIndex = 3
        Me.blConfigContainer.TabStop = True
        Me.blConfigContainer.Text = "Container Options"
        '
        'llCompCheck
        '
        Me.blCompCheck.Anchor = CType((AnchorStyles.Bottom Or AnchorStyles.Left), AnchorStyles)
        Me.blCompCheck.AutoSize = True
        Me.blCompCheck.Location = New System.Drawing.Point(3, 180)
        Me.blCompCheck.Margin = New Padding(3)
        Me.blCompCheck.Name = "llCompCheck"
        Me.blCompCheck.Size = New System.Drawing.Size(399, 37)
        Me.blCompCheck.TabIndex = 4
        Me.blCompCheck.TabStop = True
        Me.blCompCheck.Text = "Run Compressibility Check"
        '
        'lv
        '
        Me.lv.Dock = DockStyle.Fill
        Me.lv.HideSelection = False
        Me.lv.Location = New System.Drawing.Point(0, 0)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(559, 263)
        Me.lv.TabIndex = 0
        Me.lv.UseCompatibleStateImageBehavior = False
        '
        'x264Control
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = AutoScaleMode.Dpi
        Me.Controls.Add(Me.blConfigContainer)
        Me.Controls.Add(Me.tblOverrideName)
        Me.Controls.Add(Me.blConfigCodec)
        Me.Controls.Add(Me.blCompCheck)
        Me.Controls.Add(Me.lv)
        Me.Name = "x264Control"
        Me.Size = New System.Drawing.Size(559, 263)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Encoder As x264Enc
    Private Params As x264Params

    Private cms As ContextMenuStripEx
    Private QualityDefinitions As List(Of QualityItem)

    Sub New(enc As x264Enc)
        MyBase.New()
        InitializeComponent()

        components = New System.ComponentModel.Container()

        QualityDefinitions = If(s.X264QualityDefinitions IsNot Nothing AndAlso s.X264QualityDefinitions.Any(),
            s.X264QualityDefinitions,
            New List(Of QualityItem) From {
                New QualityItem(0, "Lossless", "Lossless quality but huge file size"),
                New QualityItem(14, "Super High", "Super high quality and file size"),
                New QualityItem(16, "Very High", "Very high quality and file size"),
                New QualityItem(18, "Higher", "Higher quality and file size"),
                New QualityItem(20, "High", "High quality and file size"),
                New QualityItem(22, "Medium", "Medium quality and file size"),
                New QualityItem(24, "Low", "Low quality and file size"),
                New QualityItem(26, "Lower", "Lower quality and file size"),
                New QualityItem(28, "Very Low", "Very low quality and file size"),
                New QualityItem(30, "Super Low", "Super low quality and file size")})

        Encoder = enc
        Params = Encoder.Params

        cms = New ContextMenuStripEx(components) With {
            .Font = FontManager.GetDefaultFont()
        }

        lv.View = View.Details
        lv.HeaderStyle = ColumnHeaderStyle.None
        lv.FullRowSelect = True
        lv.MultiSelect = False
        lv.ContextMenuStrip = cms
        lv.ShowContextMenuOnLeftClick = True

        tblOverrideName.ClickAction = Sub(value)
                                          Params.OverrideTargetFileName.Value = value
                                          If value Then Encoder.UpdateTargetFile()
                                          UpdateControls()
                                      End Sub

        AddHandler Params.ValueChanged, AddressOf ParamsValueChanged
        AddHandler lv.UpdateContextMenu, AddressOf UpdateMenu

        UpdateControls()
        ApplyTheme()

        AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        RemoveHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
        RemoveHandler Params.ValueChanged, AddressOf ParamsValueChanged
        RemoveHandler lv.UpdateContextMenu, AddressOf UpdateMenu
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
        If DesignHelp.IsDesignMode Then Exit Sub

        SuspendLayout()

        For Each i In {blCompCheck, blConfigContainer, blConfigCodec}
            i.BackColor = theme.General.Controls.ListView.BackColor
        Next
        For Each i In {tblOverrideName}
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

        Dim fh = FontHeight \ 4

        blConfigCodec.Left = fh
        blConfigCodec.Top = Height - blConfigCodec.Height - fh

        blCompCheck.Left = fh
        blCompCheck.Top = Height - blConfigCodec.Height - blCompCheck.Height - fh * 2

        blConfigContainer.Left = Width - blConfigContainer.Width - fh
        blConfigContainer.Top = Height - blConfigContainer.Height - fh

        Dim right = blConfigContainer.Left - blConfigCodec.Left
        Dim left = blConfigCodec.Left + blConfigCodec.Width + blConfigCodec.Left
        Dim adjustedText = "Target Name Override"

        If (right - left) < 125 Then
            adjustedText = "Override"
        ElseIf (right - left) < 185 Then
            adjustedText = "Name Override"
        End If

        tblOverrideName.Text = adjustedText
        tblOverrideName.Left = (right - left - tblOverrideName.Width) \ 2 + left
        tblOverrideName.Top = Height - tblOverrideName.Height - fh
    End Sub

    Sub ParamsValueChanged(item As CommandLineParam)
        If item Is Params.OverrideTargetFileName Then
            tblOverrideName.State = DirectCast(item, BoolParam).Value
        End If
    End Sub

    Sub UpdateMenu()
        cms.Items.ClearAndDisplose
        Dim offset = If(Encoder.QualityMode, 0, 1)
        Dim add = Sub(path As String, action As Action, isSelected As Boolean, help As String)
                      Dim item = MenuItemEx.Add(cms.Items, path & "  ", action, help)
                      'item.Font = New Font(item.Font, If(isSelected, FontStyle.Bold, FontStyle.Regular))
                      item.CheckState = If(isSelected, CheckState.Checked, CheckState.Unchecked)
                  End Sub

        If lv.SelectedItems.Count > 0 Then
            Dim selectedIndex = lv.SelectedIndices(0)
            Select Case selectedIndex
                Case 0 - offset
                    Dim param = Params.Quant
                    For Each def In QualityDefinitions
                        Dim p = def.Value & If(Not String.IsNullOrWhiteSpace(def.Text), $": {def.Text}", "")
                        add(p, Sub() SetQuality(selectedIndex, def.Value), param.Value = def.Value, def.Tooltip)
                    Next
                Case 1 - offset
                    Dim param = Params.Preset
                    For x = 0 To param.Options.Length - 1
                        Dim temp = x
                        add(param.Options(temp), Sub() SetPreset(selectedIndex, temp), param.Value = temp, "x264 slower compares to x265 medium")
                    Next
                Case 2 - offset
                    Dim param = Params.Tune
                    For x = 0 To param.Options.Length - 1
                        Dim temp = x
                        add(param.Options(temp), Sub() SetTune(selectedIndex, temp), param.Value = temp, "")
                    Next
            End Select
        End If
    End Sub

    Sub SetQuality(index As Integer, value As Double)
        Params.Quant.Value = value

        lv.Items(index).SubItems(1).Tag = GetQualityCaption(value)
        lv.Items(index).SubItems(1).Text = GetQualityCaption(value)
        lv.Items(index).Selected = False

        UpdateControls()
        Encoder.UpdateTargetFile()
    End Sub

    Sub SetPreset(index As Integer, value As Integer)
        Params.Preset.Value = value

        Params.ApplyValues(True)
        Params.ApplyValues(False)

        lv.Items(index).SubItems(1).Tag = value.ToString
        lv.Items(index).SubItems(1).Text = value.ToString
        lv.Items(index).Selected = False

        UpdateControls()
        Encoder.UpdateTargetFile()
    End Sub

    Sub SetTune(index As Integer, value As Integer)
        Params.Tune.Value = value

        Params.ApplyValues(True)
        Params.ApplyValues(False)

        lv.Items(index).SubItems(1).Tag = value.ToString
        lv.Items(index).SubItems(1).Text = value.ToString
        lv.Items(index).Selected = False

        UpdateControls()
        Encoder.UpdateTargetFile()
    End Sub

    Function GetQualityCaption(value As Double) As String
        For Each def In QualityDefinitions
            If def.Value = value Then
                Return value.ToInvariantString() & If(Not String.IsNullOrWhiteSpace(def.Text), $" - {def.Text}", "")
            End If
        Next

        Return value.ToInvariantString()
    End Function

    Sub UpdateControls()
        If Encoder.QualityMode AndAlso lv.Items.Count < 4 Then
            lv.Items.Clear()
            lv.Items.Add(New ListViewItem({"Quality", GetQualityCaption(Params.Quant.Value)}))
            lv.Items.Add(New ListViewItem({"Preset", Params.Preset.OptionText}))
            lv.Items.Add(New ListViewItem({"Tune", Params.Tune.OptionText}))
        ElseIf Params.Mode.Value <> 2 AndAlso lv.Items.Count <> 3 Then
            lv.Items.Clear()
            lv.Items.Add(New ListViewItem({"Preset", Params.Preset.OptionText}))
            lv.Items.Add(New ListViewItem({"Tune", Params.Tune.OptionText}))
        End If

        tblOverrideName.State = Encoder.OverridesTargetFileName
        blCompCheck.Visible = Params.Mode.Value = x264RateMode.TwoPass Or Params.Mode.Value = x264RateMode.ThreePass

        g.MainForm.UpdateEncoderStateRelatedControls()
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
