
Imports System.ComponentModel

Imports StaxRip.UI

Public Class CommandLineControl
    Inherits UserControl

#Region "Designer"
    Friend WithEvents tb As TextBoxEx
    Friend WithEvents tlpMain As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents bn As StaxRip.UI.ButtonEx

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
        Me.bn = New StaxRip.UI.ButtonEx()
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
        Me.tb.Size = New System.Drawing.Size(205, 205)
        '
        'bn
        '
        Me.bn.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.bn.Location = New System.Drawing.Point(215, 0)
        Me.bn.Margin = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.bn.ShowMenuSymbol = True
        Me.bn.Size = New System.Drawing.Size(100, 70)
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 2
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tlpMain.Controls.Add(Me.bn, 1, 0)
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
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
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

    Event PresetsChanged(presets As String)
    Event ValueChanged(value As String)

    Sub New()
        InitializeComponent()
        components = New Container
        AddHandler tb.TextChanged, Sub() RaiseEvent ValueChanged(tb.Text)
    End Sub

    <Browsable(False),
    DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
    Property Presets As String

    Sub MenuItenClick(value As String)
        Dim tup = Macro.ExpandGUI(value)

        If tup.Cancel Then
            Exit Sub
        End If

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
        Using dialog As New MacroEditorDialog
            dialog.SetMacroDefaults()
            dialog.MacroEditorControl.Value = Presets.FormatColumn("=")
            dialog.Text = "Menu Editor"

            If Not RestoreFunc Is Nothing Then
                dialog.bnContext.Text = " Restore Defaults... "
                dialog.bnContext.Visible = True

                dialog.bnContext.AddClickAction(Sub()
                                                    If MsgOK("Restore defaults?") Then
                                                        dialog.MacroEditorControl.Value = RestoreFunc.Invoke
                                                    End If
                                                End Sub)

                dialog.MacroEditorControl.rtbDefaults.Text = RestoreFunc.Invoke
            End If

            If dialog.ShowDialog(FindForm) = DialogResult.OK Then
                Presets = dialog.MacroEditorControl.Value
                RaiseEvent PresetsChanged(Presets)
            End If
        End Using
    End Sub

    Sub bn_Click() Handles bn.Click
        Dim cms = TextCustomMenu.GetMenu(Presets, bn, components, AddressOf MenuItenClick)
        components.Add(cms)
        cms.Items.Add(New ToolStripSeparator)
        cms.Items.Add(New MenuItemEx("Edit Menu...", AddressOf EditPresets))
        cms.Show(bn, 0, bn.Height)
    End Sub

    Sub CommandLineControl_Layout(sender As Object, e As LayoutEventArgs) Handles Me.Layout
        tb.Height = Height
    End Sub

    Sub CommandLineControl_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not DesignHelp.IsDesignMode Then
            Font = New Font("Consolas", 10 * s.UIScaleFactor)
        End If
    End Sub
End Class
