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
    Friend WithEvents lDescription As System.Windows.Forms.Label
    Friend WithEvents lNameTitle As System.Windows.Forms.Label
    Friend WithEvents lValueTitle As System.Windows.Forms.Label
    Friend WithEvents bCopy As System.Windows.Forms.Button

    Private components As System.ComponentModel.IContainer

    <DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.lv = New System.Windows.Forms.ListView()
        Me.stb = New StaxRip.SearchTextBox()
        Me.lName = New System.Windows.Forms.Label()
        Me.lValue = New System.Windows.Forms.Label()
        Me.lDescriptionTitle = New System.Windows.Forms.Label()
        Me.lDescription = New System.Windows.Forms.Label()
        Me.lNameTitle = New System.Windows.Forms.Label()
        Me.lValueTitle = New System.Windows.Forms.Label()
        Me.bCopy = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'lv
        '
        Me.lv.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lv.Location = New System.Drawing.Point(12, 49)
        Me.lv.Name = "lv"
        Me.lv.Size = New System.Drawing.Size(310, 613)
        Me.lv.TabIndex = 2
        Me.lv.UseCompatibleStateImageBehavior = False
        '
        'stb
        '
        Me.stb.BackColor = System.Drawing.Color.Aqua
        Me.stb.Location = New System.Drawing.Point(12, 12)
        Me.stb.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.stb.Name = "stb"
        Me.stb.Size = New System.Drawing.Size(310, 31)
        Me.stb.TabIndex = 4
        '
        'lName
        '
        Me.lName.Location = New System.Drawing.Point(331, 41)
        Me.lName.Name = "lName"
        Me.lName.Size = New System.Drawing.Size(315, 50)
        Me.lName.TabIndex = 5
        Me.lName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lValue
        '
        Me.lValue.Location = New System.Drawing.Point(331, 118)
        Me.lValue.Name = "lValue"
        Me.lValue.Size = New System.Drawing.Size(403, 50)
        Me.lValue.TabIndex = 6
        Me.lValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'lDescriptionTitle
        '
        Me.lDescriptionTitle.AutoSize = True
        Me.lDescriptionTitle.Location = New System.Drawing.Point(331, 169)
        Me.lDescriptionTitle.Name = "lDescriptionTitle"
        Me.lDescriptionTitle.Size = New System.Drawing.Size(106, 25)
        Me.lDescriptionTitle.TabIndex = 7
        Me.lDescriptionTitle.Text = "Description:"
        '
        'lDescription
        '
        Me.lDescription.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.lDescription.Location = New System.Drawing.Point(331, 195)
        Me.lDescription.Name = "lDescription"
        Me.lDescription.Size = New System.Drawing.Size(403, 467)
        Me.lDescription.TabIndex = 8
        '
        'lNameTitle
        '
        Me.lNameTitle.AutoSize = True
        Me.lNameTitle.Location = New System.Drawing.Point(331, 15)
        Me.lNameTitle.Name = "lNameTitle"
        Me.lNameTitle.Size = New System.Drawing.Size(63, 25)
        Me.lNameTitle.TabIndex = 9
        Me.lNameTitle.Text = "Name:"
        '
        'lValueTitle
        '
        Me.lValueTitle.AutoSize = True
        Me.lValueTitle.Location = New System.Drawing.Point(331, 92)
        Me.lValueTitle.Name = "lValueTitle"
        Me.lValueTitle.Size = New System.Drawing.Size(58, 25)
        Me.lValueTitle.TabIndex = 10
        Me.lValueTitle.Text = "Value:"
        '
        'bCopy
        '
        Me.bCopy.AutoSize = True
        Me.bCopy.Location = New System.Drawing.Point(649, 49)
        Me.bCopy.Name = "bCopy"
        Me.bCopy.Size = New System.Drawing.Size(85, 35)
        Me.bCopy.TabIndex = 11
        Me.bCopy.Text = "Copy"
        Me.bCopy.UseVisualStyleBackColor = True
        '
        'MacrosForm
        '
        Me.ClientSize = New System.Drawing.Size(744, 674)
        Me.Controls.Add(Me.bCopy)
        Me.Controls.Add(Me.lValueTitle)
        Me.Controls.Add(Me.lNameTitle)
        Me.Controls.Add(Me.lDescription)
        Me.Controls.Add(Me.lDescriptionTitle)
        Me.Controls.Add(Me.lValue)
        Me.Controls.Add(Me.lName)
        Me.Controls.Add(Me.stb)
        Me.Controls.Add(Me.lv)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!)
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Name = "MacrosForm"
        Me.Text = "Macros"
        Me.ResumeLayout(False)
        Me.PerformLayout()

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

        For Each i In Macro.GetMacros
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
        MsgInfo("Use the keys Up, Down, Enter or double click a list item.")
    End Sub

    Private Sub TaskForm_KeyDown(sender As Object, e As KeyEventArgs) Handles Me.KeyDown
        Select Case e.KeyData
            Case Keys.Enter
                bCopy.PerformClick()
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
        bCopy.PerformClick()
        Close()
    End Sub

    Private Sub lv_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lv.SelectedIndexChanged
        bCopy.Text = "Copy"

        If lv.SelectedItems.Count > 0 Then
            Dim item = lv.SelectedItems(0)

            lName.Text = item.Text
            lValue.Text = Macro.Solve(item.Text, True)
            lDescription.Text = CStr(item.Tag)
        Else
            lName.Text = ""
            lValue.Text = ""
            lDescription.Text = ""
        End If
    End Sub

    Private Sub bCopy_Click(sender As Object, e As EventArgs) Handles bCopy.Click
        Clipboard.SetText(lName.Text)
        bCopy.Font = New Font(bCopy.Font, FontStyle.Bold)
        Application.DoEvents()
        Thread.Sleep(300)
        bCopy.Font = New Font(bCopy.Font, FontStyle.Regular)
    End Sub

    Private Sub MacrosForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Populate(False)
        lDescriptionTitle.SetFontStyle(FontStyle.Bold)
        lNameTitle.SetFontStyle(FontStyle.Bold)
        lValueTitle.SetFontStyle(FontStyle.Bold)
    End Sub
End Class