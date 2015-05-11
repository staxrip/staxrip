Imports System.ComponentModel

Imports StaxRip.UI

Public Class MacroEditor
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

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.MacroEditorControl = New StaxRip.MacroEditorControl()
        Me.bnContext = New System.Windows.Forms.Button()
        Me.bnCancel = New StaxRip.UI.ButtonEx()
        Me.bnOK = New StaxRip.UI.ButtonEx()
        Me.SuspendLayout()
        '
        'MacroEditorControl
        '
        Me.MacroEditorControl.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MacroEditorControl.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.MacroEditorControl.Location = New System.Drawing.Point(12, 12)
        Me.MacroEditorControl.Name = "MacroEditorControl"
        Me.MacroEditorControl.Size = New System.Drawing.Size(461, 137)
        Me.MacroEditorControl.TabIndex = 0
        '
        'bnContext
        '
        Me.bnContext.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.bnContext.Location = New System.Drawing.Point(11, 157)
        Me.bnContext.Name = "bnContext"
        Me.bnContext.Size = New System.Drawing.Size(100, 36)
        Me.bnContext.TabIndex = 2
        Me.bnContext.UseVisualStyleBackColor = True
        Me.bnContext.Visible = False
        '
        'bnCancel
        '
        Me.bnCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.bnCancel.Location = New System.Drawing.Point(373, 157)
        Me.bnCancel.Size = New System.Drawing.Size(100, 36)
        Me.bnCancel.Text = "Cancel"
        '
        'bnOK
        '
        Me.bnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bnOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.bnOK.Location = New System.Drawing.Point(267, 157)
        Me.bnOK.Size = New System.Drawing.Size(100, 36)
        Me.bnOK.Text = "OK"
        '
        'MacroEditor
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.CancelButton = Me.bnCancel
        Me.ClientSize = New System.Drawing.Size(485, 203)
        Me.Controls.Add(Me.bnCancel)
        Me.Controls.Add(Me.bnOK)
        Me.Controls.Add(Me.bnContext)
        Me.Controls.Add(Me.MacroEditorControl)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Name = "MacroEditor"
        Me.Text = "Text"
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private HelpText As String

    Sub New()
        MyBase.New()
        InitializeComponent()
        MinimumSize = New Size(950, 600)
    End Sub

    Sub SetCommandlineDefaults()
        Text = "Command Line Editor"
        HelpText = "Editor for command lines that may contain macros."
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