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
    Friend WithEvents blConfigCodec As ButtonLabel
    Friend WithEvents blConfigContainer As ButtonLabel
    Friend WithEvents blCompCheck As ButtonLabel

    Private components As System.ComponentModel.IContainer

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.blConfigCodec = New ButtonLabel()
        Me.blConfigContainer = New ButtonLabel()
        Me.blCompCheck = New ButtonLabel()
        Me.lv = New StaxRip.UI.ListViewEx()
        Me.SuspendLayout()
        '
        'llConfigCodec
        '
        Me.blConfigCodec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.blConfigCodec.AutoSize = True
        Me.blConfigCodec.BackColor = System.Drawing.SystemColors.Window
        Me.blConfigCodec.LinkColor = System.Drawing.Color.DimGray
        Me.blConfigCodec.Location = New System.Drawing.Point(3, 185)
        Me.blConfigCodec.Margin = New System.Windows.Forms.Padding(3)
        Me.blConfigCodec.Name = "llConfigCodec"
        Me.blConfigCodec.Size = New System.Drawing.Size(64, 20)
        Me.blConfigCodec.TabIndex = 1
        Me.blConfigCodec.TabStop = True
        Me.blConfigCodec.Text = "Options"
        '
        'llConfigContainer
        '
        Me.blConfigContainer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.blConfigContainer.AutoSize = True
        Me.blConfigContainer.BackColor = System.Drawing.SystemColors.Window
        Me.blConfigContainer.LinkColor = System.Drawing.Color.DimGray
        Me.blConfigContainer.Location = New System.Drawing.Point(218, 185)
        Me.blConfigContainer.Margin = New System.Windows.Forms.Padding(3)
        Me.blConfigContainer.Name = "llConfigContainer"
        Me.blConfigContainer.Size = New System.Drawing.Size(137, 20)
        Me.blConfigContainer.TabIndex = 2
        Me.blConfigContainer.TabStop = True
        Me.blConfigContainer.Text = "Container Options"
        '
        'llCompCheck
        '
        Me.blCompCheck.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.blCompCheck.AutoSize = True
        Me.blCompCheck.BackColor = System.Drawing.SystemColors.Window
        Me.blCompCheck.LinkColor = System.Drawing.Color.DimGray
        Me.blCompCheck.Location = New System.Drawing.Point(3, 154)
        Me.blCompCheck.Margin = New System.Windows.Forms.Padding(3)
        Me.blCompCheck.Name = "llCompCheck"
        Me.blCompCheck.Size = New System.Drawing.Size(197, 20)
        Me.blCompCheck.TabIndex = 3
        Me.blCompCheck.TabStop = True
        Me.blCompCheck.Text = "Run Compressibility Check"
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
        Me.Controls.Add(Me.blConfigContainer)
        Me.Controls.Add(Me.blConfigCodec)
        Me.Controls.Add(Me.blCompCheck)
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
        blConfigCodec.Left = 5
        blConfigCodec.Top = Height - blConfigCodec.Height - 5

        blCompCheck.Left = 5
        blCompCheck.Top = Height - blConfigCodec.Height - blCompCheck.Height - 10

        blConfigContainer.Left = Width - blConfigContainer.Width - 5
        blConfigContainer.Top = Height - blConfigContainer.Height - 5
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
        blCompCheck.Visible = Encoder.Params.Mode.Value = x264Mode.TwoPass Or Encoder.Params.Mode.Value = x264Mode.ThreePass
    End Sub

    Private Sub llAdvanced_Click(sender As Object, e As EventArgs) Handles blConfigCodec.Click
        Encoder.ShowConfigDialog()
    End Sub

    Private Sub llConfigContainer_Click(sender As Object, e As EventArgs) Handles blConfigContainer.Click
        Encoder.OpenMuxerConfigDialog()
    End Sub

    Private Sub llCompCheck_Click(sender As Object, e As EventArgs) Handles blCompCheck.Click
        Encoder.RunCompCheck()
    End Sub
End Class