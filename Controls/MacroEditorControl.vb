
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
    Friend WithEvents tpPreview As System.Windows.Forms.TabPage
    Friend WithEvents gb As System.Windows.Forms.GroupBox
    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Public WithEvents rtbEdit As RichTextBoxEx
    Public WithEvents rtbPreview As RichTextBoxEx
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents llMacros As ButtonLabel
    Friend WithEvents llHelp As ButtonLabel
    Public WithEvents tpDefaults As TabPage
    Public WithEvents rtbDefaults As RichTextBoxEx
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TabControl = New System.Windows.Forms.TabControl()
        Me.tpEdit = New System.Windows.Forms.TabPage()
        Me.rtbEdit = New StaxRip.UI.RichTextBoxEx()
        Me.tpPreview = New System.Windows.Forms.TabPage()
        Me.rtbPreview = New StaxRip.UI.RichTextBoxEx()
        Me.tpDefaults = New System.Windows.Forms.TabPage()
        Me.rtbDefaults = New StaxRip.UI.RichTextBoxEx()
        Me.gb = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.llMacros = New StaxRip.UI.ButtonLabel()
        Me.llHelp = New StaxRip.UI.ButtonLabel()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.TabControl.SuspendLayout()
        Me.tpEdit.SuspendLayout()
        Me.tpPreview.SuspendLayout()
        Me.tpDefaults.SuspendLayout()
        Me.gb.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.FlowLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl
        '
        Me.TabControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl.Controls.Add(Me.tpEdit)
        Me.TabControl.Controls.Add(Me.tpPreview)
        Me.TabControl.Controls.Add(Me.tpDefaults)
        Me.TabControl.Location = New System.Drawing.Point(3, 3)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(608, 441)
        Me.TabControl.TabIndex = 0
        '
        'tpEdit
        '
        Me.tpEdit.Controls.Add(Me.rtbEdit)
        Me.tpEdit.Location = New System.Drawing.Point(12, 58)
        Me.tpEdit.Name = "tpEdit"
        Me.tpEdit.Size = New System.Drawing.Size(584, 371)
        Me.tpEdit.TabIndex = 0
        Me.tpEdit.Text = "   Edit   "
        '
        'rtbEdit
        '
        Me.rtbEdit.AcceptsTab = True
        Me.rtbEdit.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbEdit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbEdit.Location = New System.Drawing.Point(0, 0)
        Me.rtbEdit.Name = "rtbEdit"
        Me.rtbEdit.Size = New System.Drawing.Size(584, 371)
        Me.rtbEdit.TabIndex = 0
        Me.rtbEdit.Text = ""
        '
        'tpPreview
        '
        Me.tpPreview.Controls.Add(Me.rtbPreview)
        Me.tpPreview.Location = New System.Drawing.Point(12, 58)
        Me.tpPreview.Name = "tpPreview"
        Me.tpPreview.Size = New System.Drawing.Size(584, 394)
        Me.tpPreview.TabIndex = 1
        Me.tpPreview.Text = " Preview "
        '
        'rtbPreview
        '
        Me.rtbPreview.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbPreview.Location = New System.Drawing.Point(0, 0)
        Me.rtbPreview.Name = "rtbPreview"
        Me.rtbPreview.ReadOnly = True
        Me.rtbPreview.Size = New System.Drawing.Size(584, 394)
        Me.rtbPreview.TabIndex = 0
        Me.rtbPreview.Text = ""
        '
        'tpDefaults
        '
        Me.tpDefaults.Controls.Add(Me.rtbDefaults)
        Me.tpDefaults.Location = New System.Drawing.Point(12, 58)
        Me.tpDefaults.Name = "tpDefaults"
        Me.tpDefaults.Size = New System.Drawing.Size(584, 394)
        Me.tpDefaults.TabIndex = 2
        Me.tpDefaults.Text = " Defaults "
        '
        'rtbDefaults
        '
        Me.rtbDefaults.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbDefaults.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbDefaults.Location = New System.Drawing.Point(0, 0)
        Me.rtbDefaults.Name = "rtbDefaults"
        Me.rtbDefaults.ReadOnly = True
        Me.rtbDefaults.Size = New System.Drawing.Size(584, 394)
        Me.rtbDefaults.TabIndex = 0
        Me.rtbDefaults.Text = ""
        '
        'gb
        '
        Me.gb.Controls.Add(Me.TableLayoutPanel1)
        Me.gb.Dock = System.Windows.Forms.DockStyle.Fill
        Me.gb.Location = New System.Drawing.Point(0, 0)
        Me.gb.Name = "gb"
        Me.gb.Size = New System.Drawing.Size(620, 550)
        Me.gb.TabIndex = 1
        Me.gb.TabStop = False
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 1
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel1, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.TabControl, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 40)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(614, 507)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Controls.Add(Me.llMacros)
        Me.FlowLayoutPanel1.Controls.Add(Me.llHelp)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 447)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(295, 60)
        Me.FlowLayoutPanel1.TabIndex = 1
        '
        'llMacros
        '
        Me.llMacros.AutoSize = True
        Me.llMacros.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llMacros.LinkColor = System.Drawing.Color.Empty
        Me.llMacros.Location = New System.Drawing.Point(3, 0)
        Me.llMacros.Name = "llMacros"
        Me.llMacros.Size = New System.Drawing.Size(167, 60)
        Me.llMacros.TabIndex = 0
        Me.llMacros.TabStop = True
        Me.llMacros.Text = "Macros"
        '
        'llHelp
        '
        Me.llHelp.AutoSize = True
        Me.llHelp.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.llHelp.LinkColor = System.Drawing.Color.Empty
        Me.llHelp.Location = New System.Drawing.Point(176, 0)
        Me.llHelp.Name = "llHelp"
        Me.llHelp.Size = New System.Drawing.Size(116, 60)
        Me.llHelp.TabIndex = 2
        Me.llHelp.TabStop = True
        Me.llHelp.Text = "Help"
        '
        'MacroEditorControl
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.Controls.Add(Me.gb)
        Me.Name = "MacroEditorControl"
        Me.Size = New System.Drawing.Size(620, 550)
        Me.TabControl.ResumeLayout(False)
        Me.tpEdit.ResumeLayout(False)
        Me.tpPreview.ResumeLayout(False)
        Me.tpDefaults.ResumeLayout(False)
        Me.gb.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.FlowLayoutPanel1.ResumeLayout(False)
        Me.FlowLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

#End Region

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
        rtbEdit.Font = New Font("Consolas", 10 * s.UIScaleFactor)
        rtbPreview.Font = rtbEdit.Font
        rtbDefaults.Font = rtbEdit.Font
        Dim c = ControlPaint.Dark(ToolStripRendererEx.ColorBorder, 0)
        llHelp.LinkColor = c
        llMacros.LinkColor = c
        llHelp.Visible = False
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

    Private Sub UpdateWrapMode(rtb As RichTextBox)
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

    Private Sub tpPreview_Enter() Handles tpPreview.Enter
        UpdatePreview()
    End Sub

    Sub UpdatePreview()
        If Not SpecialMacrosFunction Is Nothing Then
            rtbPreview.Text = SpecialMacrosFunction.Invoke(rtbEdit.Text)
        Else
            rtbPreview.Text = Macro.Expand(rtbEdit.Text)
        End If
    End Sub

    Private Sub rtbPreview_TextChanged(sender As Object, e As EventArgs) Handles rtbPreview.TextChanged
        UpdateWrapMode(rtbPreview)
    End Sub

    Private Sub rtbEdit_TextChanged(sender As Object, e As EventArgs) Handles rtbEdit.TextChanged
        UpdateWrapMode(rtbEdit)
    End Sub

    Private Sub rtbDefaults_TextChanged(sender As Object, e As EventArgs) Handles rtbDefaults.TextChanged
        UpdateWrapMode(rtbDefaults)
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        UpdateWrapMode(rtbEdit)
        UpdateWrapMode(rtbPreview)

        If rtbDefaults.Text = "" Then
            Controls.Remove(tpDefaults)
            tpDefaults.Dispose()
        End If

        MyBase.OnLoad(e)
    End Sub

    Private Sub llMacros_Click(sender As Object, e As EventArgs) Handles llMacros.Click
        MacrosForm.ShowDialogForm()
    End Sub

    Private HelpPaths As New List(Of String)

    Private Sub llHelp_Click(sender As Object, e As EventArgs) Handles llHelp.Click
        For Each i In HelpPaths
            g.ShellExecute(i)
        Next
    End Sub

    Sub EditTextChanged(sender As Object, e As EventArgs)
        Dim editText = Value
        HelpPaths.Clear()
        Dim caption As String

        For Each pack In Package.Items.Values
            If editText.Contains(pack.Name) Then
                If pack.HelpFileOrURL <> "" Then
                    llHelp.Visible = True
                    caption += ", " + pack.Name
                    HelpPaths.Add(pack.HelpFileOrURL)
                End If
            End If
        Next

        If caption <> "" Then
            llHelp.Text = "Help (" + caption.Trim(", ".ToCharArray) + ")"
        End If

        llHelp.Visible = HelpPaths.Count > 0
    End Sub
End Class