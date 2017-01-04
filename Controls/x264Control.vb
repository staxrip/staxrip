Imports StaxRip.UI

Class x264Control
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
    Friend WithEvents llConfigCodec As System.Windows.Forms.LinkLabel
    Friend WithEvents llConfigContainer As System.Windows.Forms.LinkLabel
    Friend WithEvents llCompCheck As System.Windows.Forms.LinkLabel

    Private components As System.ComponentModel.IContainer

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.llConfigCodec = New System.Windows.Forms.LinkLabel()
        Me.llConfigContainer = New System.Windows.Forms.LinkLabel()
        Me.llCompCheck = New System.Windows.Forms.LinkLabel()
        Me.lv = New StaxRip.UI.ListViewEx()
        Me.SuspendLayout()
        '
        'llConfigCodec
        '
        Me.llConfigCodec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.llConfigCodec.AutoSize = True
        Me.llConfigCodec.BackColor = System.Drawing.SystemColors.Window
        Me.llConfigCodec.LinkColor = System.Drawing.Color.DimGray
        Me.llConfigCodec.Location = New System.Drawing.Point(3, 185)
        Me.llConfigCodec.Margin = New System.Windows.Forms.Padding(3)
        Me.llConfigCodec.Name = "llConfigCodec"
        Me.llConfigCodec.Size = New System.Drawing.Size(64, 20)
        Me.llConfigCodec.TabIndex = 1
        Me.llConfigCodec.TabStop = True
        Me.llConfigCodec.Text = "Options"
        '
        'llConfigContainer
        '
        Me.llConfigContainer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llConfigContainer.AutoSize = True
        Me.llConfigContainer.BackColor = System.Drawing.SystemColors.Window
        Me.llConfigContainer.LinkColor = System.Drawing.Color.DimGray
        Me.llConfigContainer.Location = New System.Drawing.Point(218, 185)
        Me.llConfigContainer.Margin = New System.Windows.Forms.Padding(3)
        Me.llConfigContainer.Name = "llConfigContainer"
        Me.llConfigContainer.Size = New System.Drawing.Size(137, 20)
        Me.llConfigContainer.TabIndex = 2
        Me.llConfigContainer.TabStop = True
        Me.llConfigContainer.Text = "Container Options"
        '
        'llCompCheck
        '
        Me.llCompCheck.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.llCompCheck.AutoSize = True
        Me.llCompCheck.BackColor = System.Drawing.SystemColors.Window
        Me.llCompCheck.LinkColor = System.Drawing.Color.DimGray
        Me.llCompCheck.Location = New System.Drawing.Point(3, 154)
        Me.llCompCheck.Margin = New System.Windows.Forms.Padding(3)
        Me.llCompCheck.Name = "llCompCheck"
        Me.llCompCheck.Size = New System.Drawing.Size(197, 20)
        Me.llCompCheck.TabIndex = 3
        Me.llCompCheck.TabStop = True
        Me.llCompCheck.Text = "Run Compressibility Check"
        '
        'lv
        '
        Me.lv.Dock = System.Windows.Forms.DockStyle.Fill
        Me.lv.Location = New System.Drawing.Point(0, 0)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(367, 213)
        Me.lv.TabIndex = 0
        '
        'x264Control
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.Controls.Add(Me.llConfigContainer)
        Me.Controls.Add(Me.llConfigCodec)
        Me.Controls.Add(Me.llCompCheck)
        Me.Controls.Add(Me.lv)
        Me.Name = "x264Control"
        Me.Size = New System.Drawing.Size(367, 213)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Encoder As x264Encoder
    Private cms As ContextMenuStripEx

    Sub New(enc As x264Encoder)
        MyBase.New()
        InitializeComponent()

        components = New System.ComponentModel.Container()

        Encoder = enc
        lv.View = View.Details
        lv.HeaderStyle = ColumnHeaderStyle.None
        lv.FullRowSelect = True
        lv.MultiSelect = False

        cms = New ContextMenuStripEx(components)
        cms.Font = New Font("Segoe UI", 9 * s.UIScaleFactor)

        lv.ContextMenuStrip = cms
        lv.ShowContextMenuOnLeftClick = True

        UpdateControls()
        AddHandler lv.UpdateContextMenu, AddressOf UpdateMenu
    End Sub

    Protected Overrides Sub OnLayout(e As LayoutEventArgs)
        MyBase.OnLayout(e)

        If lv.Columns.Count = 0 Then
            lv.Columns.AddRange({New ColumnHeader, New ColumnHeader})
        End If

        lv.Columns(0).Width = CInt(Width * (32 / 100))
        lv.Columns(1).Width = lv.Width - lv.Columns(0).Width - 4

        'couldn't get scaling to work trying everything
        llConfigCodec.Left = 5
        llConfigCodec.Top = Height - llConfigCodec.Height - 5

        llCompCheck.Left = 5
        llCompCheck.Top = Height - llConfigCodec.Height - llCompCheck.Height - 10

        llConfigContainer.Left = Width - llConfigContainer.Width - 5
        llConfigContainer.Top = Height - llConfigContainer.Height - 5
    End Sub

    Sub UpdateMenu()
        cms.Items.Clear()

        Dim offset = If(Encoder.Params.Mode.Value = x264Mode.SingleCRF, 0, 1)

        If lv.SelectedItems.Count > 0 Then
            Select Case lv.SelectedIndices(0)
                Case 0 - offset
                    Dim fn = Function(value As Integer, text As String, tooltip As String) New ActionMenuItem(value & " - " + text, Sub() SetQuality(value), tooltip) With {.Font = If(Encoder.Params.Quant.Value = value, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))}
                    cms.Items.Add(fn(18, "Super High", "Super high quality and file size (-crf 18)"))
                    cms.Items.Add(fn(19, "Very High", "Very high quality and file size (-crf 19)"))
                    cms.Items.Add(fn(20, "Higher", "Higher quality and file size (-crf 20)"))
                    cms.Items.Add(fn(21, "High", "High quality and file size (-crf 21)"))
                    cms.Items.Add(fn(22, "Medium", "Medium quality and file size (-crf 22)"))
                    cms.Items.Add(fn(23, "Low", "Low quality and file size (-crf 23)"))
                    cms.Items.Add(fn(24, "Lower", "Lower quality and file size (-crf 24)"))
                    cms.Items.Add(fn(25, "Very Low", "Very low quality and file size (-crf 25)"))
                    cms.Items.Add(fn(26, "Super Low", "Super low quality and file size (-rf 26)"))
                Case 1 - offset
                    For Each i In System.Enum.GetValues(GetType(x264PresetMode))
                        Dim a = CType(i, x264PresetMode)

                        cms.Items.Add(New ActionMenuItem(
                                      DispNameAttribute.GetValueForEnum(a), Sub() SetPreset(a),
                                      "Use values between Fast and Slower otherwise the quality and compression will either be poor or the encoding will be painful slow. Slower is three times slower than Medium, Veryslow is 6 times slower than Medium with little gains compared to Slower.") With {.Font = If(Encoder.Params.Preset.Value = a, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))})
                    Next
                Case 2 - offset
                    For Each i In System.Enum.GetValues(GetType(x264TuneMode))
                        Dim a = CType(i, x264TuneMode)
                        cms.Items.Add(New ActionMenuItem(
                                      DispNameAttribute.GetValueForEnum(a), Sub() SetTune(a)) With {.Font = If(Encoder.Params.Tune.Value = a, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))})
                    Next
                Case 3 - offset
                    For Each i In System.Enum.GetValues(GetType(x264DeviceMode))
                        Dim a = CType(i, x264DeviceMode)
                        cms.Items.Add(New ActionMenuItem(DispNameAttribute.GetValueForEnum(a), Sub() SetDevice(a)) With {.Font = If(Encoder.Params.Device.Value = a, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))})
                    Next
            End Select
        End If
    End Sub

    Sub SetQuality(v As Integer)
        Encoder.Params.Quant.Value = v
        lv.Items(0).SubItems(1).Text = GetQualityCaption(v)
        lv.Items(0).Selected = False
        UpdateControls()
    End Sub

    Sub SetPreset(v As x264PresetMode)
        Dim offset = If(Encoder.Params.Mode.Value = x264Mode.SingleCRF, 0, 1)

        Encoder.Params.Preset.Value = v
        Encoder.Params.ApplyDeviceSettings()
        Encoder.Params.ApplyDefaults(Encoder.Params)
        Encoder.Params.ApplyDeviceSettings()

        lv.Items(1 - offset).SubItems(1).Text = v.ToString
        lv.Items(1 - offset).Selected = False

        UpdateControls()
    End Sub

    Sub SetTune(v As x264TuneMode)
        Dim offset = If(Encoder.Params.Mode.Value = x264Mode.SingleCRF, 0, 1)

        Encoder.Params.Tune.Value = v
        Encoder.Params.ApplyDeviceSettings()
        Encoder.Params.ApplyDefaults(Encoder.Params)
        Encoder.Params.ApplyDeviceSettings()

        lv.Items(2 - offset).SubItems(1).Text = v.ToString
        lv.Items(2 - offset).Selected = False

        UpdateControls()
    End Sub

    Sub SetDevice(v As x264DeviceMode)
        Dim offset = If(Encoder.Params.Mode.Value = x264Mode.SingleCRF, 0, 1)

        Encoder.Params.Device.Value = v
        Encoder.Params.ApplyDeviceSettings()
        Encoder.Params.ApplyDefaults(Encoder.Params)
        Encoder.Params.ApplyDeviceSettings()

        lv.Items(3 - offset).SubItems(1).Text = DispNameAttribute.GetValueForEnum(v)
        lv.Items(3 - offset).Selected = False

        UpdateControls()
    End Sub

    Function GetQualityCaption(value As Single) As String
        Select Case value
            Case 18.0!
                Return "18 - Super High"
            Case 19.0!
                Return "19 - Very High"
            Case 20.0!
                Return "20 - Higher"
            Case 21.0!
                Return "21 - High"
            Case 22.0!
                Return "22 - Medium"
            Case 23.0!
                Return "23 - Low"
            Case 24.0!
                Return "24 - Lower"
            Case 25.0!
                Return "25 - Very Low"
            Case 26.0!
                Return "26 - Super Low"
        End Select

        Return value.ToString
    End Function

    Sub UpdateControls()
        If Encoder.Params.Mode.Value = x264Mode.SingleCRF AndAlso lv.Items.Count < 4 Then
            lv.Items.Clear()
            lv.Items.Add(New ListViewItem({"Quality", GetQualityCaption(Encoder.Params.Quant.Value)}))
            lv.Items.Add(New ListViewItem({"Preset", CType(Encoder.Params.Preset.Value, x264PresetMode).ToString}))
            lv.Items.Add(New ListViewItem({"Tune", CType(Encoder.Params.Tune.Value, x264TuneMode).ToString}))
            lv.Items.Add(New ListViewItem({"Device", DispNameAttribute.GetValueForEnum(CType(Encoder.Params.Device.Value, x264DeviceMode))}))
        ElseIf Encoder.Params.Mode.Value <> x264Mode.SingleCRF AndAlso lv.Items.Count <> 3 Then
            lv.Items.Clear()
            lv.Items.Add(New ListViewItem({"Preset", CType(Encoder.Params.Preset.Value, x264PresetMode).ToString}))
            lv.Items.Add(New ListViewItem({"Tune", CType(Encoder.Params.Tune.Value, x264TuneMode).ToString}))
            lv.Items.Add(New ListViewItem({"Device", DispNameAttribute.GetValueForEnum(CType(Encoder.Params.Device.Value, x264DeviceMode))}))
        End If

        Dim offset = If(Encoder.Params.Mode.Value = x264Mode.SingleCRF, 0, 1)
        llCompCheck.Visible = Encoder.Params.Mode.Value = x264Mode.TwoPass Or Encoder.Params.Mode.Value = x264Mode.ThreePass
    End Sub

    Private Sub llAdvanced_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles llConfigCodec.LinkClicked
        Encoder.ShowConfigDialog()
    End Sub

    Private Sub llConfigContainer_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles llConfigContainer.LinkClicked
        Encoder.OpenMuxerConfigDialog()
    End Sub

    Private Sub llCompCheck_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles llCompCheck.LinkClicked
        Encoder.RunCompCheck()
    End Sub
End Class