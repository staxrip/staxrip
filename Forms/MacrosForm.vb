Imports StaxRip.UI
Imports System.Threading

Class MacrosForm
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

    Friend WithEvents lv As ListView
    Friend WithEvents stb As StaxRip.SearchTextBox
    Friend WithEvents lName As System.Windows.Forms.Label
    Friend WithEvents lValue As System.Windows.Forms.Label
    Friend WithEvents lDescriptionTitle As System.Windows.Forms.Label
    Friend WithEvents lNameTitle As System.Windows.Forms.Label
    Friend WithEvents lValueTitle As System.Windows.Forms.Label
    Friend WithEvents bnCopy As System.Windows.Forms.Button
    Friend WithEvents tlp As TableLayoutPanel
    Friend WithEvents lDescription As Label
    Private components As System.ComponentModel.IContainer

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lv = New System.Windows.Forms.ListView()
        Me.stb = New StaxRip.SearchTextBox()
        Me.lName = New System.Windows.Forms.Label()
        Me.lValue = New System.Windows.Forms.Label()
        Me.lDescriptionTitle = New System.Windows.Forms.Label()
        Me.lNameTitle = New System.Windows.Forms.Label()
        Me.lValueTitle = New System.Windows.Forms.Label()
        Me.bnCopy = New System.Windows.Forms.Button()
        Me.tlp = New System.Windows.Forms.TableLayoutPanel()
        Me.lDescription = New System.Windows.Forms.Label()
        Me.tlp.SuspendLayout()
        Me.SuspendLayout()
        '
        'lv
        '
        Me.lv.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lv.Location = New System.Drawing.Point(14, 94)
        Me.lv.Margin = New System.Windows.Forms.Padding(5)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(568, 1096)
        Me.lv.TabIndex = 2
        Me.lv.UseCompatibleStateImageBehavior = False
        '
        'stb
        '
        Me.stb.BackColor = System.Drawing.Color.Aqua
        Me.stb.Location = New System.Drawing.Point(14, 12)
        Me.stb.Margin = New System.Windows.Forms.Padding(5, 7, 5, 7)
        Me.stb.Name = "stb"
        Me.stb.Size = New System.Drawing.Size(568, 70)
        Me.stb.TabIndex = 4
        '
        'lName
        '
        Me.lName.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lName.Location = New System.Drawing.Point(3, 60)
        Me.lName.Name = "lName"
        Me.lName.Size = New System.Drawing.Size(745, 60)
        Me.lName.TabIndex = 5
        Me.lName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lValue
        '
        Me.lValue.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lValue.Location = New System.Drawing.Point(3, 256)
        Me.lValue.Name = "lValue"
        Me.lValue.Size = New System.Drawing.Size(745, 60)
        Me.lValue.TabIndex = 6
        Me.lValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lDescriptionTitle
        '
        Me.lDescriptionTitle.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lDescriptionTitle.Location = New System.Drawing.Point(3, 316)
        Me.lDescriptionTitle.Name = "lDescriptionTitle"
        Me.lDescriptionTitle.Size = New System.Drawing.Size(745, 60)
        Me.lDescriptionTitle.TabIndex = 7
        Me.lDescriptionTitle.Text = "Description:"
        '
        'lNameTitle
        '
        Me.lNameTitle.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lNameTitle.Location = New System.Drawing.Point(3, 0)
        Me.lNameTitle.Name = "lNameTitle"
        Me.lNameTitle.Size = New System.Drawing.Size(745, 60)
        Me.lNameTitle.TabIndex = 9
        Me.lNameTitle.Text = "Name:"
        Me.lNameTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lValueTitle
        '
        Me.lValueTitle.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lValueTitle.Location = New System.Drawing.Point(3, 196)
        Me.lValueTitle.Name = "lValueTitle"
        Me.lValueTitle.Size = New System.Drawing.Size(745, 60)
        Me.lValueTitle.TabIndex = 10
        Me.lValueTitle.Text = "Value:"
        Me.lValueTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'bnCopy
        '
        Me.bnCopy.Location = New System.Drawing.Point(3, 123)
        Me.bnCopy.Name = "bnCopy"
        Me.bnCopy.Size = New System.Drawing.Size(300, 70)
        Me.bnCopy.TabIndex = 11
        Me.bnCopy.Text = "Copy"
        Me.bnCopy.UseVisualStyleBackColor = True
        '
        'tlp
        '
        Me.tlp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlp.ColumnCount = 1
        Me.tlp.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlp.Controls.Add(Me.lNameTitle, 0, 0)
        Me.tlp.Controls.Add(Me.lValue, 0, 4)
        Me.tlp.Controls.Add(Me.lDescription, 0, 6)
        Me.tlp.Controls.Add(Me.lDescriptionTitle, 0, 5)
        Me.tlp.Controls.Add(Me.bnCopy, 0, 2)
        Me.tlp.Controls.Add(Me.lName, 0, 1)
        Me.tlp.Controls.Add(Me.lValueTitle, 0, 3)
        Me.tlp.Location = New System.Drawing.Point(590, 12)
        Me.tlp.Name = "tlp"
        Me.tlp.RowCount = 7
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlp.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50.0!))
        Me.tlp.Size = New System.Drawing.Size(751, 1180)
        Me.tlp.TabIndex = 12
        '
        'lDescription
        '
        Me.lDescription.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lDescription.Location = New System.Drawing.Point(3, 376)
        Me.lDescription.Name = "lDescription"
        Me.lDescription.Size = New System.Drawing.Size(745, 804)
        Me.lDescription.TabIndex = 12
        '
        'MacrosForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(1353, 1204)
        Me.Controls.Add(Me.tlp)
        Me.Controls.Add(Me.stb)
        Me.Controls.Add(Me.lv)
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(11, 10, 11, 10)
        Me.Name = "MacrosForm"
        Me.Text = "Macros"
        Me.tlp.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Public Sub New()
        InitializeComponent()

        lv.View = View.Tile
        lv.FullRowSelect = True
        lv.MultiSelect = False
        lv.Columns.Add(New ColumnHeader())
        Native.SetWindowTheme(lv.Handle, "explorer", Nothing)
        ActiveControl = stb
    End Sub

    Public Shared Sub ShowDialogForm()
        Using f As New MacrosForm
            f.ShowDialog()
        End Using
    End Sub

    Function Match(search As String, ParamArray values As String()) As Boolean
        For Each i In values
            If i <> "" AndAlso i.ToLower.Contains(search.ToLower) Then
                Return True
            End If
        Next
    End Function

    Sub Populate(Optional sort As Boolean = True)
        lv.BeginUpdate()
        lv.Items.Clear()

        Dim macros As New StringPairList

        For Each i In Macro.GetMacros(True)
            macros.Add(i.Name, i.Description)
        Next

        For Each i In Package.Items.Values
            macros.Add("%app:" + i.Name + "%", "")
        Next

        For Each i In Package.Items.Values
            macros.Add("%app_dir:" + i.Name + "%", "")
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

            If 31 * lv.Items.Count > lv.Height Then
                lv.TileSize = New Size(lv.Width - SystemInformation.VerticalScrollBarWidth - 5, CInt(Font.Height * 1.5))
                lv.Scrollable = True
            Else
                lv.Scrollable = False
            End If

            Native.SetWindowTheme(lv.Handle, "explorer", Nothing)
        End If

        lv.EndUpdate()
        lv.Refresh()
    End Sub

    Private Sub tbFilter_TextChanged() Handles stb.TextChanged
        Populate()
    End Sub

    Private Sub MacrosForm_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
        Dim f As New HelpForm()
        f.Doc.WriteStart(Text)
        f.Doc.WriteTable("Macros", Strings.MacrosHelp, Macro.GetTips())
        f.Show()
    End Sub

    Private Sub TaskForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
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
                If sel < 0 Then sel = 0
                If sel >= lv.Items.Count Then sel = lv.Items.Count - 1
                lv.Items(sel).Selected = True
                lv.Items(sel).EnsureVisible()
            End If
        End If
    End Sub

    Private Sub lv_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles lv.MouseDoubleClick
        bnCopy.PerformClick()
        Close()
    End Sub

    Private Sub lv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lv.SelectedIndexChanged
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

    Private Sub bCopy_Click(sender As Object, e As EventArgs) Handles bnCopy.Click
        Clipboard.SetText(lName.Text)
        bnCopy.Font = New Font(bnCopy.Font, FontStyle.Bold)
        Application.DoEvents()
        Thread.Sleep(300)
        bnCopy.Font = New Font(bnCopy.Font, FontStyle.Regular)
    End Sub

    Private Sub MacrosForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Populate(False)
        lDescriptionTitle.SetFontStyle(FontStyle.Bold)
        lNameTitle.SetFontStyle(FontStyle.Bold)
        lValueTitle.SetFontStyle(FontStyle.Bold)
    End Sub
End Class