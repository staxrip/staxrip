Imports System.ComponentModel

Imports StaxRip.UI
Imports System.Text

Class MacroEditorControl
    Inherits UserControl

#Region " Designer "
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If

        MyBase.Dispose(disposing)
    End Sub
    Friend WithEvents TabControl As System.Windows.Forms.TabControl
    Friend WithEvents tpEdit As System.Windows.Forms.TabPage
    Friend WithEvents tpPreview As System.Windows.Forms.TabPage
    Friend WithEvents gb As System.Windows.Forms.GroupBox
    Friend WithEvents TipProvider As StaxRip.UI.TipProvider
    Friend WithEvents rtbEdit As RichTextBoxEx
    Friend WithEvents rtbPreview As System.Windows.Forms.RichTextBox
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents FlowLayoutPanel1 As System.Windows.Forms.FlowLayoutPanel
    Friend WithEvents llMacros As System.Windows.Forms.LinkLabel
    Friend WithEvents llExecute As System.Windows.Forms.LinkLabel
    Friend WithEvents llHelp As System.Windows.Forms.LinkLabel

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TabControl = New System.Windows.Forms.TabControl()
        Me.tpEdit = New System.Windows.Forms.TabPage()
        Me.rtbEdit = New StaxRip.UI.RichTextBoxEx()
        Me.tpPreview = New System.Windows.Forms.TabPage()
        Me.rtbPreview = New System.Windows.Forms.RichTextBox()
        Me.gb = New System.Windows.Forms.GroupBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.FlowLayoutPanel1 = New System.Windows.Forms.FlowLayoutPanel()
        Me.llMacros = New System.Windows.Forms.LinkLabel()
        Me.llExecute = New System.Windows.Forms.LinkLabel()
        Me.llHelp = New System.Windows.Forms.LinkLabel()
        Me.TipProvider = New StaxRip.UI.TipProvider(Me.components)
        Me.TabControl.SuspendLayout()
        Me.tpEdit.SuspendLayout()
        Me.tpPreview.SuspendLayout()
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
        Me.TabControl.Location = New System.Drawing.Point(3, 3)
        Me.TabControl.Name = "TabControl"
        Me.TabControl.SelectedIndex = 0
        Me.TabControl.Size = New System.Drawing.Size(608, 489)
        Me.TabControl.TabIndex = 0
        '
        'tpEdit
        '
        Me.tpEdit.Controls.Add(Me.rtbEdit)
        Me.tpEdit.Location = New System.Drawing.Point(4, 34)
        Me.tpEdit.Name = "tpEdit"
        Me.tpEdit.Padding = New System.Windows.Forms.Padding(3)
        Me.tpEdit.Size = New System.Drawing.Size(600, 451)
        Me.tpEdit.TabIndex = 0
        Me.tpEdit.Text = "Edit"
        Me.tpEdit.UseVisualStyleBackColor = True
        '
        'rtbEdit
        '
        Me.rtbEdit.AcceptsTab = True
        Me.rtbEdit.BlockPaint = False
        Me.rtbEdit.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtbEdit.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbEdit.Location = New System.Drawing.Point(3, 3)
        Me.rtbEdit.Name = "rtbEdit"
        Me.rtbEdit.Size = New System.Drawing.Size(594, 445)
        Me.rtbEdit.TabIndex = 0
        Me.rtbEdit.Text = ""
        '
        'tpPreview
        '
        Me.tpPreview.Controls.Add(Me.rtbPreview)
        Me.tpPreview.Location = New System.Drawing.Point(4, 29)
        Me.tpPreview.Name = "tpPreview"
        Me.tpPreview.Padding = New System.Windows.Forms.Padding(3)
        Me.tpPreview.Size = New System.Drawing.Size(600, 461)
        Me.tpPreview.TabIndex = 1
        Me.tpPreview.Text = "Preview"
        Me.tpPreview.UseVisualStyleBackColor = True
        '
        'rtbPreview
        '
        Me.rtbPreview.Dock = System.Windows.Forms.DockStyle.Fill
        Me.rtbPreview.Location = New System.Drawing.Point(3, 3)
        Me.rtbPreview.Name = "rtbPreview"
        Me.rtbPreview.ReadOnly = True
        Me.rtbPreview.Size = New System.Drawing.Size(594, 455)
        Me.rtbPreview.TabIndex = 0
        Me.rtbPreview.Text = ""
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
        Me.TableLayoutPanel1.Controls.Add(Me.TabControl, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.FlowLayoutPanel1, 0, 1)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(3, 27)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(614, 520)
        Me.TableLayoutPanel1.TabIndex = 1
        '
        'FlowLayoutPanel1
        '
        Me.FlowLayoutPanel1.AutoSize = True
        Me.FlowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.FlowLayoutPanel1.Controls.Add(Me.llMacros)
        Me.FlowLayoutPanel1.Controls.Add(Me.llExecute)
        Me.FlowLayoutPanel1.Controls.Add(Me.llHelp)
        Me.FlowLayoutPanel1.Location = New System.Drawing.Point(0, 495)
        Me.FlowLayoutPanel1.Margin = New System.Windows.Forms.Padding(0)
        Me.FlowLayoutPanel1.Name = "FlowLayoutPanel1"
        Me.FlowLayoutPanel1.Size = New System.Drawing.Size(208, 25)
        Me.FlowLayoutPanel1.TabIndex = 1
        '
        'llMacros
        '
        Me.llMacros.AutoSize = True
        Me.llMacros.Location = New System.Drawing.Point(3, 0)
        Me.llMacros.Name = "llMacros"
        Me.llMacros.Size = New System.Drawing.Size(70, 25)
        Me.llMacros.TabIndex = 0
        Me.llMacros.TabStop = True
        Me.llMacros.Text = "Macros"
        '
        'llExecute
        '
        Me.llExecute.AutoSize = True
        Me.llExecute.Location = New System.Drawing.Point(79, 0)
        Me.llExecute.Name = "llExecute"
        Me.llExecute.Size = New System.Drawing.Size(71, 25)
        Me.llExecute.TabIndex = 1
        Me.llExecute.TabStop = True
        Me.llExecute.Text = "Execute"
        '
        'llHelp
        '
        Me.llHelp.AutoSize = True
        Me.llHelp.Location = New System.Drawing.Point(156, 0)
        Me.llHelp.Name = "llHelp"
        Me.llHelp.Size = New System.Drawing.Size(49, 25)
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

        Dim c = ControlPaint.Dark(ToolStripRendererEx.ColorBorder, 0)

        llExecute.LinkColor = c
        llHelp.LinkColor = c
        llMacros.LinkColor = c

        llExecute.Visible = False
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
    End Sub

    Sub SetMacroDefaults()
        AutoWrap = False
        rtbEdit.WordWrap = False
        rtbPreview.WordWrap = False
    End Sub

    Private Sub UpdateWrapMode(rtb As RichTextBox)
        If AutoWrap Then
            rtb.WordWrap = Not rtb.Text.FixBreak.Contains(CrLf)

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
            rtbPreview.Text = Macro.Solve(rtbEdit.Text, True)
        End If
    End Sub

    Private Sub rtbPreview_TextChanged(sender As Object, e As EventArgs) Handles rtbPreview.TextChanged
        UpdateWrapMode(rtbPreview)
    End Sub

    Private Sub rtbEdit_TextChanged(sender As Object, e As EventArgs) Handles rtbEdit.TextChanged
        UpdateWrapMode(rtbEdit)
    End Sub

    Protected Overrides Sub OnHandleCreated(e As EventArgs)
        UpdateWrapMode(rtbEdit)
        UpdateWrapMode(rtbPreview)
        MyBase.OnHandleCreated(e)
    End Sub

    Private Sub llMacros_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles llMacros.LinkClicked
        MacrosForm.ShowDialogForm()
    End Sub

    Private Sub llExecute_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles llExecute.LinkClicked
        UpdatePreview()
        Dim batchPath = p.TempDir + Filepath.GetBase(p.TargetFile) + "_execute.bat"
        File.WriteAllText(batchPath, rtbPreview.Text, Encoding.GetEncoding(850))
        g.ShellExecute(batchPath)
    End Sub

    Private HelpPaths As New List(Of String)

    Private Sub llHelp_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles llHelp.LinkClicked
        For Each i In HelpPaths
            g.ShellExecute(i)
        Next
    End Sub

    Sub EditTextChanged(sender As Object, e As EventArgs)
        Dim editText = Value
        HelpPaths.Clear()
        Dim caption As String

        For Each i In Packs.Packages
            If editText.Contains(i.Name) Then
                If i.GetHelpPath() <> "" Then
                    llHelp.Visible = True
                    caption += ", " + i.Name
                    HelpPaths.Add(i.GetHelpPath())
                End If
            End If
        Next

        If caption <> "" Then
            llHelp.Text = "Help (" + caption.Trim(", ".ToCharArray) + ")"
        End If

        llHelp.Visible = HelpPaths.Count > 0
    End Sub
End Class