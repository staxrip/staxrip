Imports StaxRip.UI
Imports System.ComponentModel

Public Class CommandLineControl
    Inherits UserControl

#Region "Designer"
    Friend WithEvents tb As TextBoxEx
    Friend WithEvents tlpMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents bu As StaxRip.UI.ButtonEx

    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.tb = New StaxRip.UI.TextBoxEx()
        Me.bu = New StaxRip.UI.ButtonEx()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'tb
        '
        Me.tb.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tb.Location = New System.Drawing.Point(0, 0)
        Me.tb.Margin = New System.Windows.Forms.Padding(0)
        Me.tb.Multiline = True
        Me.tb.Size = New System.Drawing.Size(235, 205)
        '
        'bu
        '
        Me.bu.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.bu.Location = New System.Drawing.Point(245, 0)
        Me.bu.Margin = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.bu.ShowMenuSymbol = True
        Me.bu.Size = New System.Drawing.Size(70, 70)
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 2
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.Controls.Add(Me.bu, 1, 0)
        Me.tlpMain.Controls.Add(Me.tb, 0, 0)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 1
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Size = New System.Drawing.Size(315, 205)
        Me.tlpMain.TabIndex = 2
        '
        'CommandLineControl
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
        Me.Controls.Add(Me.tlpMain)
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "CommandLineControl"
        Me.Size = New System.Drawing.Size(315, 205)
        Me.tlpMain.ResumeLayout(False)
        Me.tlpMain.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region

    <Browsable(False),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property RestoreFunc As Func(Of String)

    Private HelpFileValue As String
    Private cms As ContextMenuStripEx

    Event PresetsChanged(presets As String)
    Event ValueChanged(value As String)

    Sub New()
        InitializeComponent()
        components = New System.ComponentModel.Container
        AddHandler tb.TextChanged, Sub() RaiseEvent ValueChanged(tb.Text)
    End Sub

    <Browsable(False),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property Presets As String

    Sub MenuItenClick(value As String)
        Dim tup = Macro.ExpandGUI(value)
        If tup.Cancel Then Exit Sub
        value = tup.Value

        If Not value Like "*$*$*" Then
            If tb.Text = "" Then
                tb.Text = value
            Else
                tb.Text = tb.Text.Trim + " " + value
            End If
        End If
    End Sub

    Sub EditPresets()
        Using dia As New MacroEditorDialog
            dia.SetMacroDefaults()
            dia.MacroEditorControl.Value = Presets.FormatColumn("=")
            dia.Text = "Menu Editor"

            If Not RestoreFunc Is Nothing Then
                dia.bnContext.Text = " Restore Defaults... "
                dia.bnContext.Visible = True
                dia.bnContext.AddClickAction(Sub() If MsgOK("Restore defaults?") Then dia.MacroEditorControl.Value = RestoreFunc.Invoke)
                dia.MacroEditorControl.rtbDefaults.Text = RestoreFunc.Invoke
            End If

            If dia.ShowDialog(FindForm) = DialogResult.OK Then
                Presets = dia.MacroEditorControl.Value.ReplaceUnicode
                RaiseEvent PresetsChanged(Presets)
            End If
        End Using
    End Sub

    Private Sub bCmdlAddition_Click() Handles bu.Click
        Dim cms = TextCustomMenu.GetMenu(Presets, bu, components, AddressOf MenuItenClick)
        Me.components.Add(cms)
        cms.Items.Add(New ToolStripSeparator)
        cms.Items.Add(New ActionMenuItem("Edit Menu...", AddressOf EditPresets))
        cms.Show(bu, 0, bu.Height)
    End Sub

    Private Sub CmdlControl_Layout(sender As Object, e As LayoutEventArgs) Handles Me.Layout
        tb.Height = Height
    End Sub

    Private Sub CommandLineControl_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not DesignHelp.IsDesignMode Then Font = New Font("Consolas", 10 * s.UIScaleFactor)
    End Sub
End Class