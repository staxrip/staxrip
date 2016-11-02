Imports StaxRip.UI
Imports System.ComponentModel

Class CommandLineControl
    Inherits UserControl

#Region "Designer"
    Friend WithEvents tb As TextBoxEx
    Friend WithEvents TableLayoutPanel1 As System.Windows.Forms.TableLayoutPanel
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
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel1.SuspendLayout()
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
        Me.tb.Size = New System.Drawing.Size(202, 192)
        '
        'bu
        '
        Me.bu.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.bu.Location = New System.Drawing.Point(208, 0)
        Me.bu.Margin = New System.Windows.Forms.Padding(6, 0, 0, 0)
        Me.bu.ShowMenuSymbol = True
        Me.bu.Size = New System.Drawing.Size(35, 35)
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.Controls.Add(Me.bu, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.tb, 0, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(12)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(243, 192)
        Me.TableLayoutPanel1.TabIndex = 2
        '
        'CommandLineControl
        '
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Margin = New System.Windows.Forms.Padding(0)
        Me.Name = "CommandLineControl"
        Me.Size = New System.Drawing.Size(243, 192)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
#End Region

    <Browsable(False),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property RestoreFunc As Func(Of String)

    Private HelpFileValue As String
    Private cms As ContextMenuStrip

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
        If value.Contains("$") Then
            value = Macro.Solve(value, False)
        End If

        If Not value Like "*$*$*" Then
            If tb.Text = "" Then
                tb.Text = value
            Else
                tb.Text = tb.Text.Trim + " " + value
            End If
        End If
    End Sub

    Sub EditPresets()
        Using f As New MacroEditor
            f.SetMacroDefaults()
            f.MacroEditorControl.Value = Presets.FormatColumn("=")
            f.Text = "Menu Editor"

            If Not RestoreFunc Is Nothing Then
                f.bnContext.Text = " Restore Defaults... "
                f.bnContext.Visible = True
            End If

            Dim t = f

            Dim resetAction = Sub()
                                  If MsgOK("Restore defaults?") Then
                                      t.MacroEditorControl.Value = RestoreFunc.Invoke
                                  End If
                              End Sub

            f.bnContext.AddClickAction(resetAction)

            If f.ShowDialog(FindForm) = DialogResult.OK Then
                Presets = f.MacroEditorControl.Value.ReplaceUnicode
                RaiseEvent PresetsChanged(Presets)
            End If
        End Using
    End Sub

    Private Sub bCmdlAddition_Click() Handles bu.Click
        Dim cms = TextCustomMenu.GetMenu(Presets, bu, components, AddressOf MenuItenClick)
        cms.Items.Add(New ToolStripSeparator)
        cms.Items.Add(New ActionMenuItem("Edit Menu...", AddressOf EditPresets))
        cms.Show(bu, 0, bu.Height)
    End Sub

    Private Sub CmdlControl_Layout(sender As Object, e As LayoutEventArgs) Handles Me.Layout
        tb.Height = Height
    End Sub

    Private Sub CommandLineControl_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not DesignMode Then Font = New Font("Consolas", 10 * s.UIScaleFactor)
    End Sub
End Class