
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
        Me.llConfigCodec = New StaxRip.UI.ButtonLabel()
        Me.llConfigContainer = New StaxRip.UI.ButtonLabel()
        Me.llCompCheck = New StaxRip.UI.ButtonLabel()
        Me.lv = New StaxRip.UI.ListViewEx()
        Me.SuspendLayout()
        '
        'llConfigCodec
        '
        Me.llConfigCodec.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.llConfigCodec.AutoSize = True
        Me.llConfigCodec.BackColor = System.Drawing.SystemColors.Window
        Me.llConfigCodec.ForeColor = System.Drawing.Color.DimGray
        Me.llConfigCodec.LinkColor = System.Drawing.Color.DimGray
        Me.llConfigCodec.Location = New System.Drawing.Point(3, 408)
        Me.llConfigCodec.Margin = New System.Windows.Forms.Padding(3)
        Me.llConfigCodec.Name = "llConfigCodec"
        Me.llConfigCodec.Size = New System.Drawing.Size(128, 37)
        Me.llConfigCodec.TabIndex = 1
        Me.llConfigCodec.TabStop = True
        Me.llConfigCodec.Text = "Options"
        '
        'llConfigContainer
        '
        Me.llConfigContainer.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.llConfigContainer.AutoSize = True
        Me.llConfigContainer.BackColor = System.Drawing.SystemColors.Window
        Me.llConfigContainer.ForeColor = System.Drawing.Color.DimGray
        Me.llConfigContainer.LinkColor = System.Drawing.Color.DimGray
        Me.llConfigContainer.Location = New System.Drawing.Point(346, 408)
        Me.llConfigContainer.Margin = New System.Windows.Forms.Padding(3)
        Me.llConfigContainer.Name = "llConfigContainer"
        Me.llConfigContainer.Size = New System.Drawing.Size(276, 37)
        Me.llConfigContainer.TabIndex = 2
        Me.llConfigContainer.TabStop = True
        Me.llConfigContainer.Text = "Container Options"
        '
        'llCompCheck
        '
        Me.llCompCheck.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.llCompCheck.AutoSize = True
        Me.llCompCheck.BackColor = System.Drawing.SystemColors.Window
        Me.llCompCheck.ForeColor = System.Drawing.Color.DimGray
        Me.llCompCheck.LinkColor = System.Drawing.Color.DimGray
        Me.llCompCheck.Location = New System.Drawing.Point(3, 365)
        Me.llCompCheck.Margin = New System.Windows.Forms.Padding(3)
        Me.llCompCheck.Name = "llCompCheck"
        Me.llCompCheck.Size = New System.Drawing.Size(399, 37)
        Me.llCompCheck.TabIndex = 3
        Me.llCompCheck.TabStop = True
        Me.llCompCheck.Text = "Run Compressibility Check"
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
        Me.Controls.Add(Me.llConfigContainer)
        Me.Controls.Add(Me.llConfigCodec)
        Me.Controls.Add(Me.llCompCheck)
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

        QualityDefinitions = New List(Of QualityItem) From {
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
            New QualityItem(30, "Ultra Low", "Ultra low quality and file size")}

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

        If lv.Columns.Count = 0 Then
            lv.Columns.AddRange({New ColumnHeader, New ColumnHeader})
        End If

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
                    For Each def In QualityDefinitions
                        cms.Items.Add(New ActionMenuItem(def.Value & " - " + def.Text + "      ", Sub() SetQuality(def.Value), def.Tooltip) With {.Font = If(Params.Quant.Value = def.Value, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))})
                    Next
                Case 1 - offset
                    For x = 0 To Params.Preset.Options.Length - 1
                        Dim temp = x
                        Dim presetMenuItem = New ActionMenuItem(Params.Preset.Options(x) + "      ", Sub() SetPreset(temp), "x264 slower compares to x265 medium") With {.Font = If(Params.Preset.Value = x, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))}
                        cms.Items.Add(presetMenuItem)
                    Next
                Case 2 - offset
                    For x = 0 To Params.Tune.Options.Length - 1
                        Dim temp = x
                        cms.Items.Add(New ActionMenuItem(Params.Tune.Options(x) + "      ", Sub() SetTune(temp)) With {.Font = If(Params.Tune.Value = x, New Font(Font.FontFamily, 9 * s.UIScaleFactor, FontStyle.Bold), New Font(Font.FontFamily, 9 * s.UIScaleFactor))})
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

    Function GetQualityCaption(value As Double) As String
        For Each def In QualityDefinitions
            If def.Value = value Then
                Return value & " - " + def.Text
            End If
        Next

        Return value.ToString
    End Function

    Sub UpdateControls()
        If Params.Mode.Value = x265RateMode.SingleCRF AndAlso lv.Items.Count < 4 Then
            lv.Items.Clear()
            lv.Items.Add(New ListViewItem({"Quality", GetQualityCaption(Params.Quant.Value)}))
            lv.Items.Add(New ListViewItem({"Preset", Params.Preset.OptionText}))
            lv.Items.Add(New ListViewItem({"Tune", Params.Tune.OptionText}))
        ElseIf Params.Mode.Value <> 2 AndAlso lv.Items.Count <> 3 Then
            lv.Items.Clear()
            lv.Items.Add(New ListViewItem({"Preset", Params.Preset.OptionText}))
            lv.Items.Add(New ListViewItem({"Tune", Params.Tune.OptionText}))
        End If

        Dim offset = If(Params.Mode.Value = x265RateMode.SingleCRF, 0, 1)
        llCompCheck.Visible = Params.Mode.Value = x265RateMode.TwoPass Or Params.Mode.Value = x265RateMode.ThreePass
    End Sub

    Sub llConfigCodec_Click(sender As Object, e As EventArgs) Handles llConfigCodec.Click
        Encoder.ShowConfigDialog()
    End Sub

    Sub llConfigContainer_Click(sender As Object, e As EventArgs) Handles llConfigContainer.Click
        Encoder.OpenMuxerConfigDialog()
    End Sub

    Sub llCompCheck_Click(sender As Object, e As EventArgs) Handles llCompCheck.Click
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
