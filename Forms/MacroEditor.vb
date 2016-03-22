Imports System.ComponentModel

Imports StaxRip.UI

Class MacroEditor
    Inherits DialogBase

#Region " Designer "

    Protected Overloads Overrides Sub Dispose(disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    Friend WithEvents MacroEditorControl As StaxRip.MacroEditorControl
    Friend WithEvents bnContext As System.Windows.Forms.Button
    Friend WithEvents bnCancel As StaxRip.UI.ButtonEx
    Friend WithEvents bnOK As StaxRip.UI.ButtonEx
    Friend WithEvents tlp As TableLayoutPanel
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.MacroEditorControl = New StaxRip.MacroEditorControl()
        Me.bnContext = New System.Windows.Forms.Button()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.tlp = New System.Windows.Forms.TableLayoutPanel()
        Me.tlp.SuspendLayout()
        Me.SuspendLayout()
        '
        'MacroEditorControl
        '
        Me.MacroEditorControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlp.SetColumnSpan(Me.MacroEditorControl, 3)
        Me.MacroEditorControl.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.MacroEditorControl.Location = New System.Drawing.Point(3, 3)
        Me.MacroEditorControl.Name = "MacroEditorControl"
        Me.MacroEditorControl.Size = New System.Drawing.Size(646, 393)
        Me.MacroEditorControl.TabIndex = 0
        '
        'bnContext
        '
        Me.bnContext.Anchor = System.Windows.Forms.AnchorStyles.Left
        Me.bnContext.Location = New System.Drawing.Point(3, 402)
        Me.bnContext.Name = "bnContext"
        Me.bnContext.Size = New System.Drawing.Size(100, 36)
        Me.bnContext.TabIndex = 2
        Me.bnContext.UseVisualStyleBackColor = True
        Me.bnContext.Visible = False
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(549, 402)
        Me.bnCancel.Size = New System.Drawing.Size(100, 36)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(443, 402)
        Me.bnOK.Size = New System.Drawing.Size(100, 36)
        Me.bnOK.Text = "OK"
        '
        'tlp
        '
        Me.tlp.ColumnCount = 3
        Me.tlp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlp.Controls.Add(Me.bnCancel, 2, 1)
        Me.tlp.Controls.Add(Me.MacroEditorControl, 0, 0)
        Me.tlp.Controls.Add(Me.bnContext, 0, 1)
        Me.tlp.Controls.Add(Me.bnOK, 1, 1)
        Me.tlp.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlp.Location = New System.Drawing.Point(0, 0)
        Me.tlp.Name = "tlp"
        Me.tlp.RowCount = 3
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5.0!))
        Me.tlp.Size = New System.Drawing.Size(652, 446)
        Me.tlp.TabIndex = 3
        '
        'MacroEditor
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(652, 446)
        Me.Controls.Add(Me.tlp)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Name = "MacroEditor"
        Me.Text = "Text"
        Me.tlp.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private HelpText As String

    Sub New()
        MyBase.New()
        InitializeComponent()
        MinimumSize = New Size(950, 600)
    End Sub

    Sub SetBatchDefaults()
        Text = "Batch Editor"
        HelpText = "Editor for batch script editing."
        MacroEditorControl.SetCommandLineDefaults()
        Size = New Size(800, 250)
    End Sub

    Sub SetMacroDefaults()
        Text = "Macro String Editor"
        HelpText = "Editor for strings that may contain macros."
        MacroEditorControl.SetMacroDefaults()
        Size = New Size(500, 400)
    End Sub

    Sub SetScriptDefaults()
        Text = "Script Editor"
        HelpText = "Editor for scripts that may contain macros."
        MacroEditorControl.SetScriptDefaults()
        Size = New Size(600, 400)
    End Sub

    Private Sub UniversalEditor_HelpRequested(sender As Object, e As HelpEventArgs) Handles Me.HelpRequested
        Dim f As New HelpForm()
        f.Doc.WriteStart(Text)
        f.Doc.WriteP(HelpText)
        f.Doc.WriteTips(MacroEditorControl.TipProvider.GetTips)
        f.Doc.WriteTable("Macros", Strings.MacrosHelp, Macro.GetTips())
        f.Show()
    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)
        Dim c = MacroEditorControl
        Dim s = TextRenderer.MeasureText(c.rtbEdit.Text, c.rtbEdit.Font)
        Dim w = c.TabControl.Width - s.Width - 150
        Dim h = c.TabControl.Height - s.Height - 150

        Width -= w
        Height -= h

        Dim b = Screen.FromControl(Me).WorkingArea

        If Width > b.Width * 0.7 Then Width = CInt(b.Width * 0.7)
        If Height > b.Height * 0.7 Then Height = CInt(b.Height * 0.7)

        MyBase.OnLoad(e)
    End Sub

    Private Sub MacroEditor_Load(sender As Object, e As EventArgs) Handles Me.Load
        bnContext.AutoSize = True
    End Sub
End Class