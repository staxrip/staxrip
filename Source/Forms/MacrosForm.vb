
Imports System.Threading

Imports StaxRip.UI

Public Class MacrosForm
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

    Friend WithEvents lv As ListViewEx
    Friend WithEvents stb As StaxRip.SearchTextBox
    Friend WithEvents lName As LabelEx
    Friend WithEvents lValue As LabelEx
    Friend WithEvents lDescriptionTitle As LabelEx
    Friend WithEvents lNameTitle As LabelEx
    Friend WithEvents lValueTitle As LabelEx
    Friend WithEvents bnCopy As ButtonEx
    Friend WithEvents tlpRight As TableLayoutPanel
    Friend WithEvents lDescription As LabelEx
    Friend WithEvents tlpLeft As TableLayoutPanel
    Friend WithEvents tlpMain As TableLayoutPanel
    Private components As System.ComponentModel.IContainer

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lv = New ListViewEx()
        Me.stb = New StaxRip.SearchTextBox()
        Me.lName = New LabelEx()
        Me.lValue = New LabelEx()
        Me.lDescriptionTitle = New LabelEx()
        Me.lNameTitle = New LabelEx()
        Me.lValueTitle = New LabelEx()
        Me.bnCopy = New ButtonEx()
        Me.tlpRight = New TableLayoutPanel()
        Me.lDescription = New LabelEx()
        Me.tlpLeft = New TableLayoutPanel()
        Me.tlpMain = New TableLayoutPanel()
        Me.tlpRight.SuspendLayout()
        Me.tlpLeft.SuspendLayout()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'lv
        '
        Me.lv.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.lv.Location = New System.Drawing.Point(10, 82)
        Me.lv.Margin = New Padding(10, 0, 0, 10)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(294, 461)
        Me.lv.TabIndex = 2
        Me.lv.UseCompatibleStateImageBehavior = False
        '
        'stb
        '
        Me.stb.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.stb.Location = New System.Drawing.Point(10, 10)
        Me.stb.Margin = New Padding(10, 10, 0, 10)
        Me.stb.Name = "stb"
        Me.stb.Size = New System.Drawing.Size(294, 62)
        Me.stb.TabIndex = 4
        '
        'lName
        '
        Me.lName.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.lName.Location = New System.Drawing.Point(4, 50)
        Me.lName.Margin = New Padding(4, 0, 4, 0)
        Me.lName.Name = "lName"
        Me.lName.Size = New System.Drawing.Size(364, 50)
        Me.lName.TabIndex = 5
        Me.lName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lValue
        '
        Me.lValue.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.lValue.Location = New System.Drawing.Point(4, 218)
        Me.lValue.Margin = New Padding(4, 0, 4, 0)
        Me.lValue.Name = "lValue"
        Me.lValue.Size = New System.Drawing.Size(364, 100)
        Me.lValue.TabIndex = 6
        Me.lValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lDescriptionTitle
        '
        Me.lDescriptionTitle.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.lDescriptionTitle.Location = New System.Drawing.Point(4, 318)
        Me.lDescriptionTitle.Margin = New Padding(4, 0, 4, 0)
        Me.lDescriptionTitle.Name = "lDescriptionTitle"
        Me.lDescriptionTitle.Size = New System.Drawing.Size(364, 53)
        Me.lDescriptionTitle.TabIndex = 7
        Me.lDescriptionTitle.Text = "Description:"
        '
        'lNameTitle
        '
        Me.lNameTitle.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.lNameTitle.Location = New System.Drawing.Point(4, 0)
        Me.lNameTitle.Margin = New Padding(4, 0, 4, 0)
        Me.lNameTitle.Name = "lNameTitle"
        Me.lNameTitle.Size = New System.Drawing.Size(364, 50)
        Me.lNameTitle.TabIndex = 9
        Me.lNameTitle.Text = "Name:"
        Me.lNameTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lValueTitle
        '
        Me.lValueTitle.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.lValueTitle.Location = New System.Drawing.Point(4, 168)
        Me.lValueTitle.Margin = New Padding(4, 0, 4, 0)
        Me.lValueTitle.Name = "lValueTitle"
        Me.lValueTitle.Size = New System.Drawing.Size(364, 50)
        Me.lValueTitle.TabIndex = 10
        Me.lValueTitle.Text = "Value:"
        Me.lValueTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'bnCopy
        '
        Me.bnCopy.Location = New System.Drawing.Point(4, 103)
        Me.bnCopy.Margin = New Padding(4, 3, 4, 3)
        Me.bnCopy.Name = "bnCopy"
        Me.bnCopy.Size = New System.Drawing.Size(273, 62)
        Me.bnCopy.TabIndex = 11
        Me.bnCopy.Text = "Copy"
        Me.bnCopy.UseVisualStyleBackColor = True
        '
        'tlpRight
        '
        Me.tlpRight.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.tlpRight.ColumnCount = 1
        Me.tlpRight.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100.0!))
        Me.tlpRight.Controls.Add(Me.lNameTitle, 0, 0)
        Me.tlpRight.Controls.Add(Me.lValue, 0, 4)
        Me.tlpRight.Controls.Add(Me.lDescription, 0, 6)
        Me.tlpRight.Controls.Add(Me.lDescriptionTitle, 0, 5)
        Me.tlpRight.Controls.Add(Me.bnCopy, 0, 2)
        Me.tlpRight.Controls.Add(Me.lName, 0, 1)
        Me.tlpRight.Controls.Add(Me.lValueTitle, 0, 3)
        Me.tlpRight.Location = New System.Drawing.Point(314, 3)
        Me.tlpRight.Margin = New Padding(4, 3, 4, 3)
        Me.tlpRight.Name = "tlpRight"
        Me.tlpRight.RowCount = 7
        Me.tlpRight.RowStyles.Add(New RowStyle())
        Me.tlpRight.RowStyles.Add(New RowStyle())
        Me.tlpRight.RowStyles.Add(New RowStyle())
        Me.tlpRight.RowStyles.Add(New RowStyle())
        Me.tlpRight.RowStyles.Add(New RowStyle())
        Me.tlpRight.RowStyles.Add(New RowStyle())
        Me.tlpRight.RowStyles.Add(New RowStyle(SizeType.Absolute, 45.0!))
        Me.tlpRight.Size = New System.Drawing.Size(372, 553)
        Me.tlpRight.TabIndex = 12
        '
        'lDescription
        '
        Me.lDescription.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.lDescription.Location = New System.Drawing.Point(4, 371)
        Me.lDescription.Margin = New Padding(4, 0, 4, 0)
        Me.lDescription.Name = "lDescription"
        Me.lDescription.Size = New System.Drawing.Size(364, 182)
        Me.lDescription.TabIndex = 12
        '
        'tlpLeft
        '
        Me.tlpLeft.Anchor = CType((((AnchorStyles.Top Or AnchorStyles.Bottom) _
            Or AnchorStyles.Left) _
            Or AnchorStyles.Right), AnchorStyles)
        Me.tlpLeft.ColumnCount = 1
        Me.tlpLeft.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 50.0!))
        Me.tlpLeft.Controls.Add(Me.stb, 0, 0)
        Me.tlpLeft.Controls.Add(Me.lv, 0, 1)
        Me.tlpLeft.Location = New System.Drawing.Point(3, 3)
        Me.tlpLeft.Name = "tlpLeft"
        Me.tlpLeft.RowCount = 2
        Me.tlpLeft.RowStyles.Add(New RowStyle())
        Me.tlpLeft.RowStyles.Add(New RowStyle(SizeType.Percent, 50.0!))
        Me.tlpLeft.Size = New System.Drawing.Size(304, 553)
        Me.tlpLeft.TabIndex = 13
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 2
        Me.tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 45.0!))
        Me.tlpMain.ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 55.0!))
        Me.tlpMain.Controls.Add(Me.tlpLeft, 0, 0)
        Me.tlpMain.Controls.Add(Me.tlpRight, 1, 0)
        Me.tlpMain.Dock = DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 1
        Me.tlpMain.RowStyles.Add(New RowStyle(SizeType.Percent, 100.0!))
        Me.tlpMain.Size = New System.Drawing.Size(690, 559)
        Me.tlpMain.TabIndex = 14
        '
        'MacrosForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(690, 559)
        Me.Controls.Add(Me.tlpMain)
        Me.KeyPreview = True
        Me.Margin = New Padding(11, 9, 11, 9)
        Me.Name = "MacrosForm"
        Me.Text = "Macros"
        Me.tlpRight.ResumeLayout(False)
        Me.tlpLeft.ResumeLayout(False)
        Me.tlpMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Sub New()
        InitializeComponent()
        ScaleClientSize(30, 25)
        lv.View = View.Tile
        lv.FullRowSelect = True
        lv.MultiSelect = False
        lv.Columns.Add(New ColumnHeader())
        Native.SetWindowTheme(lv.Handle, "explorer", Nothing)
        ActiveControl = stb

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

        stb.BackColor = theme.General.BackColor.AddLuminance(0.25)
    End Sub

    Shared Sub ShowDialogForm()
        Using form As New MacrosForm
            form.ShowDialog()
        End Using
    End Sub

    Function Match(search As String, ParamArray values As String()) As Boolean
        For Each i In values
            If i <> "" AndAlso i.ToLowerInvariant.Contains(search.ToLowerInvariant) Then
                Return True
            End If
        Next
    End Function

    Sub Populate(Optional sort As Boolean = True)
        lv.BeginUpdate()
        lv.Items.Clear()

        Dim macros As New StringPairList

        For Each mac In Macro.GetMacros(True, True)
            macros.Add(mac.Name, mac.Description)
        Next

        For Each i In macros
            If stb.Text = "" OrElse Match(stb.Text, i.Name, i.Value) Then
                Dim item As New ListViewItem
                item.Text = i.Name
                item.Tag = i.Value
                lv.Items.Add(item)
            End If
        Next

        If lv.Items.Count > 0 Then
            lv.Items(0).Selected = True
            lv.Items(0).EnsureVisible()
            lv.TileSize = New Size(lv.Width - SystemInformation.VerticalScrollBarWidth - 5, CInt(Font.Height * 1.2))
            lv.Scrollable = True
            Native.SetWindowTheme(lv.Handle, "explorer", Nothing)
        End If

        lv.EndUpdate()
        lv.Refresh()
    End Sub

    Sub tbFilter_TextChanged() Handles stb.TextChanged
        Populate()
    End Sub

    Sub MacrosForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        g.ShowWikiPage("Macros")
    End Sub

    Sub TaskForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyData
            Case Keys.Enter
                bnCopy.PerformClick()
                Close()
        End Select

        If lv.Items.Count > 0 Then
            If lv.SelectedIndices.Count = 0 Then
                lv.Items(0).Selected = True
            End If

            Dim sel As Integer

            Select Case e.KeyData
                Case Keys.Up
                    e.Handled = True
                    sel = -1
                Case Keys.Down
                    e.Handled = True
                    sel = 1
            End Select

            If sel <> 0 Then
                sel = lv.SelectedItems(0).Index + sel

                If sel < 0 Then
                    sel = 0
                End If

                If sel >= lv.Items.Count Then
                    sel = lv.Items.Count - 1
                End If

                lv.Items(sel).Selected = True
                lv.Items(sel).EnsureVisible()
            End If
        End If
    End Sub

    Sub lv_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lv.MouseDoubleClick
        bnCopy.PerformClick()
        Close()
    End Sub

    Sub lv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lv.SelectedIndexChanged
        bnCopy.Text = "Copy"

        If lv.SelectedItems.Count > 0 Then
            Dim item = lv.SelectedItems(0)
            lName.Text = item.Text
            lValue.Text = Macro.Expand(item.Text)
            lDescription.Text = CStr(item.Tag)
        Else
            lName.Text = ""
            lValue.Text = ""
            lDescription.Text = ""
        End If
    End Sub

    Sub bnCopy_Click(sender As Object, e As EventArgs) Handles bnCopy.Click
        Clipboard.SetText(lName.Text)
        bnCopy.SetFontStyle(FontStyle.Bold)
        Application.DoEvents()
        Thread.Sleep(300)
        bnCopy.SetFontStyle(FontStyle.Regular)
    End Sub

    Sub MacrosForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Populate(False)
        lDescriptionTitle.SetFontStyle(FontStyle.Bold)
        lNameTitle.SetFontStyle(FontStyle.Bold)
        lValueTitle.SetFontStyle(FontStyle.Bold)
    End Sub
End Class
