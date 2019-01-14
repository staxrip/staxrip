Imports StaxRip.UI

Public Class x265Control
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
    Friend WithEvents llConfigCodec As ButtonLabel
    Friend WithEvents llConfigContainer As ButtonLabel
    Friend WithEvents llCompCheck As ButtonLabel

    Private components As System.ComponentModel.IContainer

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.llConfigCodec = New ButtonLabel()
        Me.llConfigContainer = New ButtonLabel()
        Me.llCompCheck = New ButtonLabel()
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
        Me.llConfigCodec.Size = New System.Drawing.Size(120, 25)
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
        Me.llConfigContainer.Size = New System.Drawing.Size(146, 25)
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
        Me.llCompCheck.Size = New System.Drawing.Size(222, 25)
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
        'x265Control
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.Controls.Add(Me.llConfigContainer)
        Me.Controls.Add(Me.llConfigCodec)
        Me.Controls.Add(Me.llCompCheck)
        Me.Controls.Add(Me.lv)
        Me.Name = "x265Control"
        Me.Size = New System.Drawing.Size(367, 213)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region

    Private Encoder As x265Enc
    Private Params As x265Params

    Private cms As ContextMenuStripEx
    Private QualityDefinitions As List(Of QualityItem)

    Sub New(enc As x265Enc)
        MyBase.New()
        InitializeComponent()

        components = New System.ComponentModel.Container()

        QualityDefinitions = New List(Of QualityItem) From {
            New QualityItem(12, "Super High", "Super high quality and file size (-crf 12)"),
            New QualityItem(14, "Very High", "Very high quality and file size (-crf 14)"),
            New QualityItem(16, "Higher", "Higher quality and file size (-crf 16)"),
            New QualityItem(18, "High", "High quality and file size (-crf 18)"),
            New QualityItem(20, "Medium", "Medium quality and file size (-crf 20)"),
            New QualityItem(22, "Low", "Low quality and file size (-crf 22)"),
            New QualityItem(24, "Lower", "Lower quality and file size (-crf 24)"),
            New QualityItem(26, "Very Low", "Very low quality and file size (-crf 26)"),
            New QualityItem(28, "Super Low", "Super low quality and file size (-crf 28)")}

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
    End Sub

    Protected Overrides Sub OnLayout(e As LayoutEventArgs)
        MyBase.OnLayout(e)

        If lv.Columns.Count = 0 Then lv.Columns.AddRange({New ColumnHeader, New ColumnHeader})

        lv.Columns(0).Width = CInt(Width * (32 / 100))
        lv.Columns(1).Width = CInt(Width * (66 / 100))

        'couldn't get scaling to work trying everything
        llConfigCodec.Left = 5
        llConfigCodec.Top = Height - llConfigCodec.Height - 5

        llCompCheck.Left = 5
        llCompCheck.Top = Height - llConfigCodec.Height - llCompCheck.Height - 10

        llConfigContainer.Left = Width - llConfigContainer.Width - 5
        llConfigContainer.Top = Height - llConfigContainer.Height - 5
    End Sub

    Sub UpdateMenu()
        cms.Items.ClearAndDisplose
        Dim offset = If(Params.Mode.Value = x265RateMode.SingleCRF, 0, 1)

        If lv.SelectedItems.Count > 0 Then
            Select Case lv.SelectedIndices(0)
                Case 0 - offset
                    For Each i In QualityDefinitions
                        cms.Items.Add(New ActionMenuItem(i.Value & " - " + i.Text, Sub() SetQuality(i.Value), i.Tooltip) With {.Font = If(Params.Quant.Value = i.Value, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))})
                    Next
                Case 1 - offset
                    For x = 0 To Params.Preset.Options.Length - 1
                        Dim temp = x
                        cms.Items.Add(New ActionMenuItem(
                                      Params.Preset.Options(x), Sub() SetPreset(temp),
                                      "Use values between Fast and Slower otherwise the quality and compression will either be poor or the encoding will be painful slow. Slower is three times slower than Medium, Veryslow is 6 times slower than Medium with little gains compared to Slower.") With {.Font = If(Params.Preset.Value = x, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))})
                    Next
                Case 2 - offset
                    For x = 0 To Params.Tune.Options.Length - 1
                        Dim temp = x
                        cms.Items.Add(New ActionMenuItem(
                                      Params.Tune.Options(x), Sub() SetTune(temp)) With {.Font = If(Params.Tune.Value = x, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))})
                    Next
                Case 3 - offset
                    For x = 0 To Params.OutputDepth.Options.Length - 1
                        Dim temp = x
                        cms.Items.Add(New ActionMenuItem(
                                      Params.OutputDepth.Options(x), Sub() SetDepth(temp)) With {.Font = If(Params.OutputDepth.Value = x, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))})
                    Next
            End Select
        End If
    End Sub

    Sub SetQuality(v As Single)
        Params.Quant.Value = v
        lv.Items(0).SubItems(1).Text = GetQualityCaption(v)
        lv.Items(0).Selected = False
        UpdateControls()
    End Sub

    Sub SetPreset(value As Integer)
        Dim offset = If(Params.Mode.Value = x265RateMode.SingleCRF, 0, 1)

        Params.Preset.Value = value

        Params.ApplyPresetValues()
        Params.ApplyTuneValues()

        lv.Items(1 - offset).SubItems(1).Text = value.ToString
        lv.Items(1 - offset).Selected = False

        UpdateControls()
    End Sub

    Sub SetTune(value As Integer)
        Dim offset = If(Params.Mode.Value = x265RateMode.SingleCRF, 0, 1)

        Params.Tune.Value = value

        Params.ApplyPresetValues()
        Params.ApplyTuneValues()

        lv.Items(2 - offset).SubItems(1).Text = value.ToString
        lv.Items(2 - offset).Selected = False

        UpdateControls()
    End Sub

    Sub SetDepth(value As Integer)
        Dim offset = If(Params.Mode.Value = x265RateMode.SingleCRF, 0, 1)

        Params.OutputDepth.Value = value

        Params.ApplyPresetValues()
        Params.ApplyTuneValues()

        lv.Items(3 - offset).SubItems(1).Text = value.ToString
        lv.Items(3 - offset).Selected = False

        UpdateControls()
    End Sub

    Function GetQualityCaption(value As Double) As String
        For Each i In QualityDefinitions
            If i.Value = value Then
                Return value & " - " + i.Text
            End If
        Next

        Return value.ToString
    End Function

    Sub UpdateControls()
        If Params.Mode.Value = x265RateMode.SingleCRF AndAlso lv.Items.Count < 5 Then
            lv.Items.Clear()
            lv.Items.Add(New ListViewItem({"Quality", GetQualityCaption(Params.Quant.Value)}))
            lv.Items.Add(New ListViewItem({"Preset", Params.Preset.OptionText}))
            lv.Items.Add(New ListViewItem({"Tune", Params.Tune.OptionText}))
            lv.Items.Add(New ListViewItem({"Depth", Params.OutputDepth.OptionText}))
        ElseIf Params.Mode.Value <> 2 AndAlso lv.Items.Count <> 4 Then
            lv.Items.Clear()
            lv.Items.Add(New ListViewItem({"Preset", Params.Preset.OptionText}))
            lv.Items.Add(New ListViewItem({"Tune", Params.Tune.OptionText}))
            lv.Items.Add(New ListViewItem({"Depth", Params.OutputDepth.OptionText}))
        End If

        Dim offset = If(Params.Mode.Value = x265RateMode.SingleCRF, 0, 1)
        llCompCheck.Visible = Params.Mode.Value = x265RateMode.TwoPass Or Params.Mode.Value = x265RateMode.ThreePass
    End Sub

    Private Sub llConfigCodec_Click(sender As Object, e As EventArgs) Handles llConfigCodec.Click
        Encoder.ShowConfigDialog()
    End Sub

    Private Sub llConfigContainer_Click(sender As Object, e As EventArgs) Handles llConfigContainer.Click
        Encoder.OpenMuxerConfigDialog()
    End Sub

    Private Sub llCompCheck_Click(sender As Object, e As EventArgs) Handles llCompCheck.Click
        Encoder.RunCompCheck()
    End Sub

    Public Class QualityItem
        Property Value As Single
        Property Text As String
        Property Tooltip As String

        Sub New(value As Single, text As String, tooltip As String)
            Me.Value = value
            Me.Text = text
            Me.Tooltip = tooltip
        End Sub
    End Class
End Class