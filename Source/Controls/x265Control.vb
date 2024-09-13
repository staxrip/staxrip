
Imports StaxRip.UI

Public Class x265Control
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
        'x265Control
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.blConfigContainer)
        Me.Controls.Add(Me.blConfigCodec)
        Me.Controls.Add(Me.blCompCheck)
        Me.Controls.Add(Me.lv)
        Me.Name = "x265Control"
        Me.Size = New System.Drawing.Size(625, 448)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Encoder As x265Enc
    Private Params As x265Params
    Private cms As ContextMenuStripEx
    Private QualityDefinitions As List(Of QualityItem)

    Sub New(enc As x265Enc)
        InitializeComponent()
        components = New ComponentModel.Container()

        QualityDefinitions = If(s.X265QualityDefinitions IsNot Nothing AndAlso s.X265QualityDefinitions.Any(),
            s.X265QualityDefinitions,
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
                New QualityItem(28, "Extreme Low", "Extreme low quality and file size"),
                New QualityItem(30, "Ultra Low", "Ultra low quality and file size")})

        Encoder = enc
        Params = Encoder.Params

        cms = New ContextMenuStripEx(components)
        cms.Font = New Font("Segoe UI", 9 * s.UIScaleFactor)

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
        Dim offset = If(Encoder.QualityMode, 0, 1)

        If lv.SelectedItems.Count > 0 Then
            Dim selectedIndex = lv.SelectedIndices(0)            
            Select Case selectedIndex
                Case 0 - offset
                    Dim param = Params.Quant
                    For Each def In QualityDefinitions
                        Dim item = MenuItemEx.Add(cms.Items, def.Value.ToInvariantString() & If(Not String.IsNullOrWhiteSpace(def.Text), $": {def.Text}  ", "  "), Sub() SetQuality(selectedIndex, def.Value), def.Tooltip)
                        item.Font = If(param.Value = def.Value, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))
                    Next
                Case 1 - offset
                    Dim param = Params.Preset
                    For x = 0 To param.Options.Length - 1
                        Dim temp = x
                        Dim item = MenuItemEx.Add(cms.Items, param.Options(temp) + "  ", Sub() SetPreset(selectedIndex, temp))
                        item.Font = If(param.Value = temp, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))
                    Next
                Case 2 - offset
                    Dim param = Params.Tune
                    For x = 0 To param.Options.Length - 1
                        Dim temp = x
                        Dim item = MenuItemEx.Add(cms.Items, param.Options(temp) + "  ", Sub() SetTune(selectedIndex, temp))
                        item.Font = If(param.Value = temp, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))
                    Next
                Case 3 - offset
                    Dim param = Params.AQmode
                    For x = 0 To param.Options.Length - 1
                        Dim temp = x
                        Dim item = MenuItemEx.Add(cms.Items, param.Options(temp) + "  ", Sub() SetAqMode(selectedIndex, temp))
                        item.Font = If(param.Value = temp, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))
                    Next
                Case 4 - offset
                    Dim param = Params.DolbyVisionProfile
                    For x = 0 To param.Options.Length - 1
                        Dim temp = x
                        Dim item = MenuItemEx.Add(cms.Items, param.Options(temp) + "  ", Sub() SetDolbyVisionProfile(selectedIndex, temp))
                        item.Font = If(param.Value = temp, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))
                    Next
                Case 5 - offset
                    Dim param = Params.Range
                    For x = 0 To param.Options.Length - 1
                        Dim temp = x
                        Dim item = MenuItemEx.Add(cms.Items, param.Options(temp) + "  ", Sub() SetRange(selectedIndex, temp))
                        item.Font = If(param.Value = temp, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))
                    Next
            End Select
        End If
    End Sub

    Sub SetQuality(index As Integer, v As Double)
        Params.Quant.Value = v
        lv.Items(index).SubItems(1).Text = GetQualityCaption(v)
        lv.Items(index).Selected = False
        UpdateControls()
    End Sub

    Sub SetPreset(index As Integer, value As Integer)
        Params.Preset.Value = value

        Params.ApplyPresetValues()
        Params.ApplyTuneValues()

        lv.Items(index).SubItems(1).Text = value.ToString
        lv.Items(index).Selected = False

        UpdateControls()
    End Sub

    Sub SetTune(index As Integer, value As Integer)
        Params.Tune.Value = value

        Params.ApplyPresetValues()
        Params.ApplyTuneValues()

        lv.Items(index).SubItems(1).Text = value.ToString
        lv.Items(index).Selected = False

        UpdateControls()
    End Sub

    Sub SetAqMode(index As Integer, value As Integer)
        Params.AQmode.Value = value

        lv.Items(index).SubItems(1).Text = value.ToString
        lv.Items(index).Selected = False

        UpdateControls()
    End Sub

    Sub SetDolbyVisionProfile(index As Integer, value As Integer)
        Params.DolbyVisionProfile.Value = value

        lv.Items(index).SubItems(1).Text = value.ToString
        lv.Items(index).Selected = False

        UpdateControls()
    End Sub

    Sub SetRange(index As Integer, value As Integer)
        Params.Range.Value = value

        lv.Items(index).SubItems(1).Text = value.ToString
        lv.Items(index).Selected = False

        UpdateControls()
    End Sub

    Function GetQualityCaption(value As Double) As String
        For Each def In QualityDefinitions
            If def.Value = value Then
                Return value.ToInvariantString() & If(Not String.IsNullOrWhiteSpace(def.Text), $" - {def.Text}", "")
            End If
        Next

        Return value.ToString
    End Function

    Sub UpdateControls()
        lv.Items.Clear()
        If Encoder.QualityMode Then
            lv.Items.Add(New ListViewItem({"Quality", GetQualityCaption(Params.Quant.Value)}))
        End If
        lv.Items.Add(New ListViewItem({"Preset", Params.Preset.OptionText}))
        lv.Items.Add(New ListViewItem({"Tune", Params.Tune.OptionText}))
        lv.Items.Add(New ListViewItem({"AQ-Mode", Params.AQmode.OptionText}))
        lv.Items.Add(New ListViewItem({"DV Profile", Params.DolbyVisionProfile.OptionText}))
        lv.Items.Add(New ListViewItem({"Range", Params.Range.OptionText}))

        blCompCheck.Visible = Params.Mode.Value = x265RateMode.TwoPass Or Params.Mode.Value = x265RateMode.ThreePass
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
