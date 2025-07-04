
Imports System.Web.UI.WebControls.WebParts
Imports StaxRip.UI
Imports StaxRip.VideoEncoderCommandLine

Public Class SvtAv1EncAppControl
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
        'SvtAv1EncAppControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.blConfigContainer)
        Me.Controls.Add(Me.blConfigCodec)
        Me.Controls.Add(Me.blCompCheck)
        Me.Controls.Add(Me.lv)
        Me.Name = "SvtAv1EncAppControl"
        Me.Size = New System.Drawing.Size(625, 448)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private ReadOnly Encoder As SvtAv1Enc
    Private ReadOnly Params As SvtAv1EncParams
    Private ReadOnly cms As ContextMenuStripEx
    Private ReadOnly QualityDefinitions As List(Of QualityItem)

    Sub New(enc As SvtAv1Enc)
        InitializeComponent()
        components = New ComponentModel.Container()

        QualityDefinitions = If(s.SvtAv1EncAppQualityDefinitions IsNot Nothing AndAlso s.SvtAv1EncAppQualityDefinitions.Any(),
            s.SvtAv1EncAppQualityDefinitions,
            New List(Of QualityItem) From {
                New QualityItem(20, "Ultra High", "Ultra high quality and file size"),
                New QualityItem(25, "Extreme High", "Extreme high quality and file size"),
                New QualityItem(30, "Super High", "Super high quality and file size"),
                New QualityItem(33, "Very High", "Very high quality and file size"),
                New QualityItem(35, "Higher (default)", "Higher quality and file size"),
                New QualityItem(38, "High", "High quality and file size"),
                New QualityItem(40, "Medium", "Medium quality and file size"),
                New QualityItem(42, "Low", "Low quality and file size"),
                New QualityItem(45, "Lower", "Lower quality and file size"),
                New QualityItem(47, "Very Low", "Very low quality and file size"),
                New QualityItem(50, "Super Low", "Super low quality and file size"),
                New QualityItem(55, "Extreme Low", "Extreme low quality and file size"),
                New QualityItem(60, "Ultra Low", "Ultra low quality and file size")})

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

        UpdateControls()
        ApplyTheme()

        AddHandler lv.UpdateContextMenu, AddressOf UpdateMenu
        AddHandler Params.ValueChanged, AddressOf UpdateControls
        AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
    End Sub

    Protected Overrides Sub Dispose(disposing As Boolean)
        RemoveHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
        RemoveHandler Params.ValueChanged, AddressOf UpdateControls
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
        Dim offset = If(Params.RateControlMode.Value = SvtAv1EncAppRateMode.Quality, 0, 1)

        If lv.SelectedItems.Count > 0 Then
            Dim selectedIndex = lv.SelectedIndices(0)
            Select Case selectedIndex
                Case 0 - offset
                    Dim param = Params.QuantizationParameter
                    For Each def In QualityDefinitions
                        Dim item = MenuItemEx.Add(cms.Items, def.Value & If(Not String.IsNullOrWhiteSpace(def.Text), $": {def.Text}  ", "  "), Sub() SetQuality(selectedIndex, def.Value), def.Tooltip)
                        item.Font = If(param.Value = def.Value, FontManager.GetDefaultFont(9, FontStyle.Bold), FontManager.GetDefaultFont())
                    Next
                Case 1 - offset
                    Dim param = Params.Preset
                    For x = 0 To param.Options.Length - 1
                        Dim temp = x
                        Dim item = MenuItemEx.Add(cms.Items, param.Options(temp) + "  ", Sub() SetPreset(selectedIndex, temp))
                        item.Font = If(param.Value = temp, FontManager.GetDefaultFont(9, FontStyle.Bold), FontManager.GetDefaultFont())
                    Next
                Case 2 - offset
                    Dim param = Params.Tune
                    For x = 0 To param.Options.Length - 1
                        Dim temp = x
                        Dim item = MenuItemEx.Add(cms.Items, param.Options(temp) + "  ", Sub() SetTune(selectedIndex, temp))
                        item.Font = If(param.Value = temp, FontManager.GetDefaultFont(9, FontStyle.Bold), FontManager.GetDefaultFont())
                    Next
                Case 3 - offset
                    Dim param = Params.FastDecode
                    For x = 0 To param.Options.Length - 1
                        Dim temp = x
                        Dim item = MenuItemEx.Add(cms.Items, param.Options(temp) + "  ", Sub() SetFastDecode(selectedIndex, temp))
                        item.Font = If(param.Value = temp, FontManager.GetDefaultFont(9, FontStyle.Bold), FontManager.GetDefaultFont())
                    Next
                Case 4 - offset
                    Dim param = Params.Lookahead
                    For x = param.Config(0) To param.Config(1) Step param.Config(2)
                        Dim temp = CInt(x)
                        Dim lowerBound = Math.Floor((temp - 1) / 10) * 10 + 1
                        Dim upperBound = lowerBound + 9
                        Dim category = If(temp <= 0, "", $"{lowerBound:00} - {upperBound:00} | ")
                        Dim def = If(temp = CInt(param.InitialValue), "  (default)", "")
                        Dim item = MenuItemEx.Add(cms.Items, category + temp.ToInvariantString() + def + "  ", Sub() SetLookahead(selectedIndex, temp))
                        item.Font = If(param.Value = temp, FontManager.GetDefaultFont(9, FontStyle.Bold), FontManager.GetDefaultFont())
                    Next
                Case 5 - offset
                    Dim param = Params.FilmGrain
                    For x = param.Config(0) To param.Config(1) Step param.Config(2)
                        Dim temp = CInt(x)
                        Dim lowerBound = Math.Floor((temp - 1) / 10) * 10 + 1
                        Dim upperBound = lowerBound + 9
                        Dim category = If(temp = 0, "", $"{lowerBound:00} - {upperBound:00} | ")
                        Dim def = If(temp = CInt(param.InitialValue), "  (default)", "")
                        Dim item = MenuItemEx.Add(cms.Items, category + temp.ToInvariantString() + def + "  ", Sub() SetFilmGrain(selectedIndex, temp))
                        item.Font = If(param.Value = temp, FontManager.GetDefaultFont(9, FontStyle.Bold), FontManager.GetDefaultFont())
                    Next
                Case Else
                    Throw New NotSupportedException(NameOf(selectedIndex))
            End Select
        End If
    End Sub

    Sub SetQuality(index As Integer, v As Double)
        Params.ConstantRateFactor.Value = v
        Params.QuantizationParameter.Value = v
        lv.Items(index).SubItems(1).Text = GetQualityCaption(v)
        lv.Items(index).Selected = False
        UpdateControls()
    End Sub

    Sub SetPreset(index As Integer, value As Integer)
        Params.Preset.Value = value

        Params.ApplyPresetValues()

        lv.Items(index).SubItems(1).Text = value.ToString
        lv.Items(index).Selected = False

        UpdateControls()
    End Sub

    Sub SetTune(index As Integer, value As Integer)
        Params.Tune.Value = value

        lv.Items(index).SubItems(1).Text = value.ToString
        lv.Items(index).Selected = False

        UpdateControls()
    End Sub

    Sub SetFastDecode(index As Integer, value As Integer)
        Params.FastDecode.Value = value

        lv.Items(index).SubItems(1).Text = value.ToString
        lv.Items(index).Selected = False

        UpdateControls()
    End Sub

    Sub SetLookahead(index As Integer, value As Integer)
        Params.Lookahead.Value = value

        lv.Items(index).SubItems(1).Text = value.ToString
        lv.Items(index).Selected = False

        UpdateControls()
    End Sub

    Sub SetFilmGrain(index As Integer, value As Integer)
        Params.FilmGrain.Value = value

        lv.Items(index).SubItems(1).Text = value.ToString
        lv.Items(index).Selected = False

        UpdateControls()
    End Sub

    Function GetQualityCaption(value As Double) As String
        For Each def In QualityDefinitions
            If def.Value = value Then
                Return value & If(Not String.IsNullOrWhiteSpace(def.Text), $": {def.Text}", "")
            End If
        Next

        Return value.ToString
    End Function

    Sub UpdateControls(item As CommandLineParam)
        UpdateControls()
    End Sub

    Sub UpdateControls()
        Dim offset = If(Params.RateControlMode.Value = SvtAv1EncAppRateMode.Quality, 0, 1)

        lv.Items.Clear()
        If Params.RateControlMode.Value = SvtAv1EncAppRateMode.Quality Then
            lv.Items.Add(New ListViewItem({"Quality", GetQualityCaption(Params.QuantizationParameter.Value)}))
        End If
        lv.Items.Add(New ListViewItem({"Preset", Params.Preset.OptionText}))
        lv.Items.Add(New ListViewItem({"Tune", Params.Tune.OptionText}))
        lv.Items.Add(New ListViewItem({"Fast Decode", Params.FastDecode.OptionText}))
        lv.Items.Add(New ListViewItem({"Lookahead", Params.Lookahead.Value.ToInvariantString() + If(Params.Lookahead.Value = Params.Lookahead.InitialValue, " (default)", "")}))
        lv.Items.Add(New ListViewItem({"Film Grain", Params.FilmGrain.Value.ToInvariantString() + If(Params.FilmGrain.Value = Params.FilmGrain.InitialValue, " (default)", "")}))

        blCompCheck.Visible = Params.RateControlMode.Value <> SvtAv1EncAppRateMode.Quality
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
