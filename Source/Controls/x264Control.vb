
Imports StaxRip.UI

Public Class x264Control
    Inherits UserControl

#Region " Designer "
    <DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If

        MyBase.Dispose(disposing)
    End Sub

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
        'blConfigCodec
        '
        Me.blConfigCodec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.blConfigCodec.AutoSize = True
        Me.blConfigCodec.Location = New System.Drawing.Point(3, 223)
        Me.blConfigCodec.Margin = New System.Windows.Forms.Padding(3)
        Me.blConfigCodec.Name = "blConfigCodec"
        Me.blConfigCodec.Size = New System.Drawing.Size(128, 37)
        Me.blConfigCodec.TabIndex = 1
        Me.blConfigCodec.TabStop = True
        Me.blConfigCodec.Text = "Options"
        '
        'llConfigContainer
        '
        Me.blConfigContainer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.blConfigContainer.AutoSize = True
        Me.blConfigContainer.Location = New System.Drawing.Point(280, 223)
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
        Me.blCompCheck.Location = New System.Drawing.Point(3, 180)
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
        Me.lv.Size = New System.Drawing.Size(559, 263)
        Me.lv.TabIndex = 0
        Me.lv.UseCompatibleStateImageBehavior = False
        '
        'x264Control
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.blConfigContainer)
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

        If Not s.X264QualityDefinitions Is Nothing AndAlso s.X264QualityDefinitions.Any() Then
            QualityDefinitions = s.X264QualityDefinitions
        Else
            QualityDefinitions = New List(Of QualityItem) From {
                New QualityItem(14, "Super High", "Super high quality and file size"),
                New QualityItem(16, "Very High", "Very high quality and file size"),
                New QualityItem(18, "Higher", "Higher quality and file size"),
                New QualityItem(20, "High", "High quality and file size"),
                New QualityItem(22, "Medium", "Medium quality and file size"),
                New QualityItem(24, "Low", "Low quality and file size"),
                New QualityItem(26, "Lower", "Lower quality and file size"),
                New QualityItem(28, "Very Low", "Very low quality and file size"),
                New QualityItem(30, "Super Low", "Super low quality and file size")}
        End If

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
        Dim offset = If(Params.Mode.Value = x264RateMode.Quality, 0, 1)

        If lv.SelectedItems.Count > 0 Then
            Select Case lv.SelectedIndices(0)
                Case 0 - offset
                    For Each def In QualityDefinitions
                        cms.Items.Add(New MenuItemEx(def.Value & If(Not String.IsNullOrWhiteSpace(def.Text), $" - {def.Text}      ", "      "), Sub() SetQuality(def.Value), def.Tooltip) With {.Font = If(Params.Quant.Value = def.Value, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))})
                    Next
                Case 1 - offset
                    For x = 0 To Params.Preset.Options.Length - 1
                        Dim temp = x
                        cms.Items.Add(New MenuItemEx(Params.Preset.Options(x) + "      ", Sub() SetPreset(temp), "x264 slower compares to x265 medium") With {.Font = If(Params.Preset.Value = x, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))})
                    Next
                Case 2 - offset
                    For x = 0 To Params.Tune.Options.Length - 1
                        Dim temp = x
                        cms.Items.Add(New MenuItemEx(Params.Tune.Options(x) + "      ", Sub() SetTune(temp)) With {.Font = If(Params.Tune.Value = x, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))})
                    Next
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
        Dim offset = If(Params.Mode.Value = x264RateMode.Quality, 0, 1)

        Params.Preset.Value = value
        Params.ApplyValues(True)
        Params.ApplyValues(False)

        lv.Items(1 - offset).SubItems(1).Text = value.ToString
        lv.Items(1 - offset).Selected = False

        UpdateControls()
    End Sub

    Sub SetTune(value As Integer)
        Dim offset = If(Params.Mode.Value = x264RateMode.Quality, 0, 1)
        Params.Tune.Value = value
        Params.ApplyValues(True)
        Params.ApplyValues(False)
        lv.Items(2 - offset).SubItems(1).Text = value.ToString
        lv.Items(2 - offset).Selected = False
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
        If Params.Mode.Value = x264RateMode.Quality AndAlso lv.Items.Count < 4 Then
            lv.Items.Clear()
            lv.Items.Add(New ListViewItem({"Quality", GetQualityCaption(Params.Quant.Value)}))
            lv.Items.Add(New ListViewItem({"Preset", Params.Preset.OptionText}))
            lv.Items.Add(New ListViewItem({"Tune", Params.Tune.OptionText}))
        ElseIf Params.Mode.Value <> 2 AndAlso lv.Items.Count <> 3 Then
            lv.Items.Clear()
            lv.Items.Add(New ListViewItem({"Preset", Params.Preset.OptionText}))
            lv.Items.Add(New ListViewItem({"Tune", Params.Tune.OptionText}))
        End If

        Dim offset = If(Params.Mode.Value = x264RateMode.Quality, 0, 1)
        blCompCheck.Visible = Params.Mode.Value = x264RateMode.TwoPass Or Params.Mode.Value = x264RateMode.ThreePass
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
