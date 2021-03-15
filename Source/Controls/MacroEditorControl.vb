
Imports System.ComponentModel

Imports StaxRip.UI

Public Class MacroEditorControl
    Inherits UserControl

#Region " Designer "
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If

        MyBase.Dispose(disposing)
    End Sub

    Private WithEvents TabControl As TabControl
    Public WithEvents tpEdit As TabPage
    Friend WithEvents tpPreview As TabPage
    Friend WithEvents gb As GroupBoxEx
    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Public WithEvents rtbEdit As RichTextBoxEx
    Public WithEvents rtbPreview As RichTextBoxEx
    Friend WithEvents tlpMain As TableLayoutPanel
    Friend WithEvents flpButtons As FlowLayoutPanelEx
    Friend WithEvents llMacros As ButtonLabel
    Friend WithEvents llHelp As ButtonLabel
    Public WithEvents tpDefaults As TabPage
    Public WithEvents rtbDefaults As RichTextBoxEx
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TabControl = New TabControl()
        Me.tpEdit = New TabPage()
        Me.tpPreview = New TabPage()
        Me.tpDefaults = New TabPage()
        Me.gb = New GroupBoxEx()
        Me.tlpMain = New TableLayoutPanel()
        Me.flpButtons = New FlowLayoutPanelEx()
        Me.llMacros = New StaxRip.UI.ButtonLabel()
        Me.llHelp = New StaxRip.UI.ButtonLabel()
        Me.rtbEdit = New StaxRip.UI.RichTextBoxEx()
        Me.rtbPreview = New StaxRip.UI.RichTextBoxEx()
        Me.rtbDefaults = New StaxRip.UI.RichTextBoxEx()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.TabControl.SuspendLayout()
        Me.tpEdit.SuspendLayout()
        Me.tpPreview.SuspendLayout()
        Me.tpDefaults.SuspendLayout()
        Me.gb.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.flpButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl
        '
        Me.TabControl.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.TabControl.Controls.Add(Me.tpEdit)
        Me.TabControl.Controls.Add(Me.tpPreview)
        Me.TabControl.Controls.Add(Me.tpDefaults)
        Me.TabControl.Location = New System.Drawing.Point(3, 3)
        Me.TabControl.Margin = New Padding(3, 3, 3, 10)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(609, 453)
        Me.TabControl.TabIndex = 0
        '
        'tpEdit
        '
        Me.tpEdit.Controls.Add(Me.rtbEdit)
        Me.tpEdit.Location = New System.Drawing.Point(12, 58)
        Me.tpEdit.Name = "tpEdit"
        Me.tpEdit.Size = New System.Drawing.Size(585, 383)
        Me.tpEdit.TabIndex = 0
        Me.tpEdit.Text = "   Edit   "
        '
        'tpPreview
        '
        Me.tpPreview.Controls.Add(Me.rtbPreview)
        Me.tpPreview.Location = New System.Drawing.Point(12, 58)
        Me.tpPreview.Name = "tpPreview"
        Me.tpPreview.Size = New System.Drawing.Size(584, 383)
        Me.tpPreview.TabIndex = 1
        Me.tpPreview.Text = " Preview "
        '
        'tpDefaults
        '
        Me.tpDefaults.Controls.Add(Me.rtbDefaults)
        Me.tpDefaults.Location = New System.Drawing.Point(12, 58)
        Me.tpDefaults.Name = "tpDefaults"
        Me.tpDefaults.Size = New System.Drawing.Size(584, 383)
        Me.tpDefaults.TabIndex = 2
        Me.tpDefaults.Text = " Defaults "
        '
        'gb
        '
        Me.gb.Controls.Add(Me.tlpMain)
        Me.gb.Dock = DockStyle.Fill
        Me.gb.Location = New System.Drawing.Point(0, 0)
        Me.gb.Name = "gb"
        Me.gb.Size = New System.Drawing.Size(620, 550)
        Me.gb.TabIndex = 1
        Me.gb.TabStop = False
        '
        'tlpMain
        '
        Me.tlpMain.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.tlpMain.ColumnCount = 1
        Me.tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0!))
        Me.tlpMain.Controls.Add(Me.flpButtons, 0, 1)
        Me.tlpMain.Controls.Add(Me.TabControl, 0, 0)
        Me.tlpMain.Location = New System.Drawing.Point(3, 46)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 2
        Me.tlpMain.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0!))
        Me.tlpMain.RowStyles.Add(New RowStyle())
        Me.tlpMain.Size = New System.Drawing.Size(616, 496)
        Me.tlpMain.TabIndex = 1
        '
        'flpButtons
        '
        Me.flpButtons.AutoSize = True
        Me.flpButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Me.flpButtons.Controls.Add(Me.llMacros)
        Me.flpButtons.Controls.Add(Me.llHelp)
        Me.flpButtons.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.flpButtons.Location = New System.Drawing.Point(0, 466)
        Me.flpButtons.Margin = New Padding(0)
        Me.flpButtons.Name = "flpButtons"
        Me.flpButtons.Size = New System.Drawing.Size(269, 54)
        Me.flpButtons.TabIndex = 1
        '
        'llMacros
        '
        Me.llMacros.AutoSize = True
        Me.llMacros.Location = New System.Drawing.Point(3, 0)
        Me.llMacros.Name = "llMacros"
        Me.llMacros.Size = New System.Drawing.Size(151, 54)
        Me.llMacros.TabIndex = 0
        Me.llMacros.TabStop = True
        Me.llMacros.Text = "Macros"
        '
        'llHelp
        '
        Me.llHelp.AutoSize = True
        Me.llHelp.Location = New System.Drawing.Point(160, 0)
        Me.llHelp.Name = "llHelp"
        Me.llHelp.Size = New System.Drawing.Size(106, 54)
        Me.llHelp.TabIndex = 2
        Me.llHelp.TabStop = True
        Me.llHelp.Text = "Help"
        '
        'rtbEdit
        '
        Me.rtbEdit.AcceptsTab = True
        Me.rtbEdit.BorderStyle = BorderStyle.None
        Me.rtbEdit.Dock = DockStyle.Fill
        Me.rtbEdit.Location = New System.Drawing.Point(0, 0)
        Me.rtbEdit.Margin = New Padding(0)
        Me.rtbEdit.Name = "rtbEdit"
        Me.rtbEdit.Size = New System.Drawing.Size(585, 383)
        Me.rtbEdit.TabIndex = 0
        Me.rtbEdit.Text = ""
        '
        'rtbPreview
        '
        Me.rtbPreview.BorderStyle = BorderStyle.None
        Me.rtbPreview.Dock = DockStyle.Fill
        Me.rtbPreview.Location = New System.Drawing.Point(0, 0)
        Me.rtbPreview.Name = "rtbPreview"
        Me.rtbPreview.ReadOnly = True
        Me.rtbPreview.Size = New System.Drawing.Size(584, 383)
        Me.rtbPreview.TabIndex = 0
        Me.rtbPreview.Text = ""
        '
        'rtbDefaults
        '
        Me.rtbDefaults.BorderStyle = BorderStyle.None
        Me.rtbDefaults.Dock = DockStyle.Fill
        Me.rtbDefaults.Location = New System.Drawing.Point(0, 0)
        Me.rtbDefaults.Name = "rtbDefaults"
        Me.rtbDefaults.ReadOnly = True
        Me.rtbDefaults.Size = New System.Drawing.Size(584, 383)
        Me.rtbDefaults.TabIndex = 0
        Me.rtbDefaults.Text = ""
        '
        'MacroEditorControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = AutoScaleMode.Dpi
        Me.Controls.Add(Me.gb)
        Me.Name = "MacroEditorControl"
        Me.Size = New System.Drawing.Size(620, 550)
        Me.TabControl.ResumeLayout(False)
        Me.tpEdit.ResumeLayout(False)
        Me.tpPreview.ResumeLayout(False)
        Me.tpDefaults.ResumeLayout(False)
        Me.gb.ResumeLayout(False)
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.flpButtons.ResumeLayout(False)
        Me.flpButtons.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private HelpPaths As New List(Of String)

    <Browsable(False),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property SpecialMacrosFunction As Func(Of String, String)

    <Browsable(False),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property AutoWrap As Boolean = True

    Private IsCommandLineMode As Boolean

    Sub New()
        MyBase.New()
        InitializeComponent()
        rtbEdit.EnableAutoDragDrop = True
        rtbEdit.Font = g.GetCodeFont
        rtbPreview.Font = rtbEdit.Font
        rtbDefaults.Font = rtbEdit.Font
        llHelp.Visible = False

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

        BackColor = theme.General.BackColor
    End Sub

    <Category("Appearance"), DefaultValue(""), Bindable(True),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Visible),
    Browsable(True), EditorBrowsable(EditorBrowsableState.Always)>
    Overrides Property Text() As String
        Get
            Return gb.Text
        End Get
        Set(Value As String)
            gb.Text = Value
        End Set
    End Property

    <Browsable(False),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property Value() As String
        Get
            Return rtbEdit.Text.FixBreak
        End Get
        Set(value As String)
            rtbEdit.Text = value

            If IsCommandLineMode Then
                EditTextChanged(Nothing, Nothing)
            End If
        End Set
    End Property

    Sub SetCommandLineDefaults()
        AutoWrap = True
        IsCommandLineMode = True
        AddHandler rtbEdit.TextChanged, AddressOf EditTextChanged
    End Sub

    Sub SetScriptDefaults()
        AutoWrap = False
        rtbEdit.WordWrap = False
        rtbPreview.WordWrap = False
        rtbDefaults.WordWrap = False
    End Sub

    Sub SetMacroDefaults()
        AutoWrap = False
        rtbEdit.WordWrap = False
        rtbPreview.WordWrap = False
        rtbDefaults.WordWrap = False
    End Sub

    Sub UpdateWrapMode(rtb As RichTextBox)
        If AutoWrap Then
            rtb.WordWrap = Not rtb.Text.FixBreak.Contains(BR)
            Dim s = TextRenderer.MeasureText(rtb.Text, rtb.Font)

            If s.Width > (rtb.Width - SystemInformation.VerticalScrollBarWidth) Then
                rtb.ScrollBars = RichTextBoxScrollBars.Both
            Else
                rtb.ScrollBars = RichTextBoxScrollBars.Vertical
            End If
        End If
    End Sub

    Sub tpPreview_Enter() Handles tpPreview.Enter
        UpdatePreview()
    End Sub

    Sub UpdatePreview()
        If Not SpecialMacrosFunction Is Nothing Then
            rtbPreview.Text = SpecialMacrosFunction.Invoke(rtbEdit.Text)
        Else
            rtbPreview.Text = Macro.Expand(rtbEdit.Text)
        End If
    End Sub

    Sub rtbPreview_TextChanged(sender As Object, e As EventArgs) Handles rtbPreview.TextChanged
        UpdateWrapMode(rtbPreview)
    End Sub

    Sub rtbEdit_TextChanged(sender As Object, e As EventArgs) Handles rtbEdit.TextChanged
        UpdateWrapMode(rtbEdit)
    End Sub

    Sub rtbDefaults_TextChanged(sender As Object, e As EventArgs) Handles rtbDefaults.TextChanged
        UpdateWrapMode(rtbDefaults)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)

        UpdateWrapMode(rtbEdit)
        UpdateWrapMode(rtbPreview)

        If rtbDefaults.Text = "" Then
            Controls.Remove(tpDefaults)
            tpDefaults.Dispose()
        End If
    End Sub

    Sub llMacros_Click(sender As Object, e As EventArgs) Handles llMacros.Click
        MacrosForm.ShowDialogForm()
    End Sub

    Sub llHelp_Click(sender As Object, e As EventArgs) Handles llHelp.Click
        For Each path In HelpPaths
            g.ShellExecute(path)
        Next
    End Sub

    Sub EditTextChanged(sender As Object, e As EventArgs)
        Dim editText = Value
        HelpPaths.Clear()
        Dim caption As String

        For Each pack In Package.Items.Values
            If editText.Contains(pack.Name) Then
                llHelp.Visible = True
                caption += ", " + pack.Name
                HelpPaths.Add(pack.HelpFileOrURL)
            End If
        Next

        If caption <> "" Then
            llHelp.Text = "Help (" + caption.Trim(", ".ToCharArray) + ")"
        End If

        llHelp.Visible = HelpPaths.Count > 0
    End Sub
End Class
