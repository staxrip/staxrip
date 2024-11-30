
Imports System.Web.UI.WebControls.WebParts
Imports StaxRip.UI
Imports StaxRip.VideoEncoderCommandLine

Public Class NVEncControl
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
        Me.blConfigContainer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.blConfigContainer.AutoSize = True
        Me.blConfigContainer.Location = New System.Drawing.Point(346, 408)
        Me.blConfigContainer.Margin = New System.Windows.Forms.Padding(3)
        Me.blConfigContainer.Name = "llConfigContainer"
        Me.blConfigContainer.Size = New System.Drawing.Size(276, 37)
        Me.blConfigContainer.TabIndex = 3
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
        Me.blCompCheck.TabIndex = 4
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
        'NVEncControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.blConfigContainer)
        Me.Controls.Add(Me.tblOverrideName)
        Me.Controls.Add(Me.blConfigCodec)
        Me.Controls.Add(Me.blCompCheck)
        Me.Controls.Add(Me.lv)
        Me.Name = "NVEncControl"
        Me.Size = New System.Drawing.Size(625, 448)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private ReadOnly Encoder As NVEnc
    Private ReadOnly Params As NVEncParams
    Private ReadOnly cms As ContextMenuStripEx
    Private ReadOnly QualityDefinitions As List(Of QualityItem)

    Sub New(enc As NVEnc)
        InitializeComponent()
        components = New ComponentModel.Container()

        QualityDefinitions = If(s.NVEncCQualityDefinitions IsNot Nothing AndAlso s.NVEncCQualityDefinitions.Any(),
            s.NVEncCQualityDefinitions,
            New List(Of QualityItem) From {
                New QualityItem(10, "Super High", "Super high quality and file size"),
                New QualityItem(12, "Very High", "Very high quality and file size"),
                New QualityItem(14, "Higher", "Higher quality and file size"),
                New QualityItem(16, "High", "High quality and file size"),
                New QualityItem(18, "Medium", "Medium quality and file size"),
                New QualityItem(20, "Low", "Low quality and file size"),
                New QualityItem(22, "Lower", "Lower quality and file size"),
                New QualityItem(24, "Very Low", "Very low quality and file size"),
                New QualityItem(26, "Super Low", "Super low quality and file size"),
                New QualityItem(28, "Giga Low", "Giga low quality and file size"),
                New QualityItem(30, "Extreme Low", "Extreme low quality and file size"),
                New QualityItem(33, "Ultra Low", "Ultra low quality and file size")})

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

        If lv.SelectedItems.Count > 0 Then
            Dim selectedIndex = lv.SelectedIndices(0)
            Select Case selectedIndex
                Case 0 - offset
                    Dim param As NumParam
                    If Params.QVBR.Visible Then param = Params.QVBR
                    If Params.QP.Visible Then param = Params.QP
                    If Params.QPAV1.Visible Then param = Params.QPAV1
                    If Params.VbrQuality.Visible Then param = Params.VbrQuality
                    If param IsNot Nothing Then
                        For Each def In QualityDefinitions
                            Dim item = MenuItemEx.Add(cms.Items, def.Value & If(Not String.IsNullOrWhiteSpace(def.Text), $": {def.Text}  ", "  "), Sub() SetQuality(selectedIndex, def.Value), def.Tooltip)
                            item.Font = If(param.Value = def.Value, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))
                        Next
                    End If
                Case 1 - offset
                    Dim param = Params.Mode
                    For x = 0 To param.Options.Length - 1
                        Dim temp = x
                        Dim item = MenuItemEx.Add(cms.Items, param.Options(temp) + "  ", Sub() SetMode(selectedIndex, temp))
                        item.Font = If(param.Value = temp, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))
                    Next
                Case 2 - offset
                    Dim param = Params.Preset
                    For x = 0 To param.Options.Length - 1
                        Dim temp = x
                        Dim item = MenuItemEx.Add(cms.Items, param.Options(temp) + "  ", Sub() SetPreset(selectedIndex, temp))
                        item.Font = If(param.Value = temp, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))
                    Next
                Case 3 - offset
                    Dim param = Params.Tune
                    If Params.TuneH264.Visible Then param = Params.TuneH264
                    For x = 0 To param.Options.Length - 1
                        Dim temp = x
                        Dim item = MenuItemEx.Add(cms.Items, param.Options(temp) + "  ", Sub() SetTune(selectedIndex, temp))
                        item.Font = If(param.Value = temp, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))
                    Next
                Case 4 - offset
                    Dim param = Params.OutputDepth
                    For x = 0 To param.Options.Length - 1
                        Dim temp = x
                        Dim item = MenuItemEx.Add(cms.Items, param.Options(temp) + "  ", Sub() SetOutputDepth(selectedIndex, temp))
                        item.Font = If(param.Value = temp, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))
                    Next
                Case 5 - offset
                    Dim param = If(Params.DolbyVisionProfileAV1.Visible, Params.DolbyVisionProfileAV1, Params.DolbyVisionProfileH265)
                    For x = 0 To param.Options.Length - 1
                        Dim temp = x
                        Dim item = MenuItemEx.Add(cms.Items, param.Options(temp) + "  ", Sub() SetDolbyVisionProfile(param, selectedIndex, temp))
                        item.Font = If(param.Value = temp, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))
                    Next
                Case 6 - offset
                    Dim param = Params.ColorRange
                    For x = 0 To param.Options.Length - 1
                        Dim temp = x
                        Dim item = MenuItemEx.Add(cms.Items, param.Options(temp) + "  ", Sub() SetColorRange(selectedIndex, temp))
                        item.Font = If(param.Value = temp, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))
                    Next
                Case Else
                    Throw New NotSupportedException(NameOf(selectedIndex))
            End Select
        End If
    End Sub

    Sub SetMode(index As Integer, value As Integer)
        Params.Mode.Value = value

        lv.Items(index).SubItems(1).Text = value.ToString()
        lv.Items(index).Selected = False

        UpdateControls()
        Encoder.UpdateTargetFile()
    End Sub

    Sub SetQuality(index As Integer, v As Double)
        If Params.QVBR.Visible Then Params.QVBR.Value = v
        If Params.QP.Visible Then Params.QP.Value = v
        If Params.VbrQuality.Visible Then Params.VbrQuality.Value = v

        lv.Items(index).SubItems(1).Text = GetQualityCaption(v)
        lv.Items(index).Selected = False

        UpdateControls()
        Encoder.UpdateTargetFile()
    End Sub

    Sub SetPreset(index As Integer, value As Integer)
        Params.Preset.Value = value

        lv.Items(index).SubItems(1).Text = value.ToString()
        lv.Items(index).Selected = False

        UpdateControls()
        Encoder.UpdateTargetFile()
    End Sub

    Sub SetTune(index As Integer, value As Integer)
        If Params.Tune.Visible Then Params.Tune.Value = value
        If Params.TuneH264.Visible Then Params.TuneH264.Value = value

        lv.Items(index).SubItems(1).Text = value.ToString()
        lv.Items(index).Selected = False

        UpdateControls()
        Encoder.UpdateTargetFile()
    End Sub

    Sub SetOutputDepth(index As Integer, value As Integer)
        Params.OutputDepth.Value = value

        lv.Items(index).SubItems(1).Text = value.ToString()
        lv.Items(index).Selected = False

        UpdateControls()
        Encoder.UpdateTargetFile()
    End Sub

    Sub SetDolbyVisionProfile(param As OptionParam, index As Integer, value As Integer)
        param.Value = value

        lv.Items(index).SubItems(1).Text = value.ToString
        lv.Items(index).Selected = False

        UpdateControls()
        Encoder.UpdateTargetFile()
    End Sub

    Sub SetColorRange(index As Integer, value As Integer)
        Params.ColorRange.Value = value

        lv.Items(index).SubItems(1).Text = value.ToString
        lv.Items(index).Selected = False

        UpdateControls()
        Encoder.UpdateTargetFile()
    End Sub

    Sub UpdateControls(item As CommandLineParam)
        UpdateControls()
    End Sub

    Sub UpdateControls()
        lv.Items.Clear()
        If Encoder.QualityMode Then
            Dim val = -1.0
            val = If(Params.QVBR.Visible, Params.QVBR.Value, val)
            val = If(Params.QP.Visible, Params.QP.Value, val)
            val = If(Params.QPAV1.Visible, Params.QPAV1.Value, val)
            val = If(Params.VbrQuality.Visible, Params.VbrQuality.Value, val)
            If val >= 0 Then
                lv.Items.Add(New ListViewItem({"Quality", GetQualityCaption(val)}))
            End If
        End If
        lv.Items.Add(New ListViewItem({"Mode", Params.Mode.OptionText}))
        lv.Items.Add(New ListViewItem({"Preset", Params.Preset.OptionText}))
        Dim tuneParam = Params.Tune
        If Params.TuneH264.Visible Then tuneParam = Params.TuneH264
        lv.Items.Add(New ListViewItem({"Tune", tuneParam.OptionText}))
        lv.Items.Add(New ListViewItem({"Output Depth", Params.OutputDepth.OptionText}))
        lv.Items.Add(New ListViewItem({"DV Profile", If(Params.DolbyVisionProfileAV1.Visible, Params.DolbyVisionProfileAV1.OptionText, Params.DolbyVisionProfileH265.OptionText)}))
        lv.Items.Add(New ListViewItem({"Color Range", Params.ColorRange.OptionText}))

        tblOverrideName.State = Encoder.OverridesTargetFileName
        'blCompCheck.Visible = Not Encoder.QualityMode AndAlso Params.Decoder.Value = 0
        blCompCheck.Visible = False

        g.MainForm.UpdateEncoderStateRelatedControls()
    End Sub

    Function GetQualityCaption(value As Double) As String
        For Each def In QualityDefinitions
            If def.Value = value Then
                Return value.ToInvariantString() & If(Not String.IsNullOrWhiteSpace(def.Text), $": {def.Text}", "")
            End If
        Next

        Return value.ToInvariantString()
    End Function

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
