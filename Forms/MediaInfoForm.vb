Imports StaxRip.UI
Imports System.Text
Imports Microsoft.Win32

Public Class MediaInfoForm
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

    Friend WithEvents tv As TreeViewEx
    Friend WithEvents rtb As StaxRip.UI.RichTextBoxEx
    Friend WithEvents stb As StaxRip.SearchTextBox
    Friend WithEvents tlpMain As TableLayoutPanel
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.tv = New StaxRip.UI.TreeViewEx()
        Me.rtb = New StaxRip.UI.RichTextBoxEx()
        Me.stb = New StaxRip.SearchTextBox()
        Me.tlpMain = New System.Windows.Forms.TableLayoutPanel()
        Me.tlpMain.SuspendLayout()
        Me.SuspendLayout()
        '
        'tv
        '
        Me.tv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tv.Location = New System.Drawing.Point(0, 80)
        Me.tv.Margin = New System.Windows.Forms.Padding(0)
        Me.tv.Name = "tv"
        Me.tv.Size = New System.Drawing.Size(350, 632)
        Me.tv.TabIndex = 2
        '
        'rtb
        '
        Me.rtb.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtb.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtb.Location = New System.Drawing.Point(350, 0)
        Me.rtb.Margin = New System.Windows.Forms.Padding(0)
        Me.rtb.Name = "rtb"
        Me.tlpMain.SetRowSpan(Me.rtb, 2)
        Me.rtb.Size = New System.Drawing.Size(580, 712)
        Me.rtb.TabIndex = 4
        Me.rtb.Text = ""
        '
        'stb
        '
        Me.stb.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.stb.Location = New System.Drawing.Point(0, 0)
        Me.stb.Margin = New System.Windows.Forms.Padding(0)
        Me.stb.Name = "stb"
        Me.stb.Size = New System.Drawing.Size(350, 80)
        Me.stb.TabIndex = 5
        '
        'tlpMain
        '
        Me.tlpMain.ColumnCount = 2
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 350.0!))
        Me.tlpMain.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Controls.Add(Me.stb, 0, 0)
        Me.tlpMain.Controls.Add(Me.tv, 0, 1)
        Me.tlpMain.Controls.Add(Me.rtb, 1, 0)
        Me.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tlpMain.Location = New System.Drawing.Point(0, 0)
        Me.tlpMain.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.tlpMain.Name = "tlpMain"
        Me.tlpMain.RowCount = 2
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.tlpMain.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpMain.Size = New System.Drawing.Size(930, 712)
        Me.tlpMain.TabIndex = 6
        '
        'MediaInfoForm
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(930, 712)
        Me.Controls.Add(Me.tlpMain)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.HelpButton = False
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.MaximizeBox = True
        Me.MinimizeBox = True
        Me.Name = "MediaInfoForm"
        Me.Text = "MediaInfo"
        Me.tlpMain.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private SourcePath As String
    Private ActiveGroup As String
    Private Items As New List(Of Item)

    Sub New(fp As String)
        MyBase.New()
        InitializeComponent()

        tv.ItemHeight = FontHeight * 2
        ScaleClientSize(39, 31)

        rtb.WordWrap = False
        rtb.ReadOnly = True
        rtb.BackColor = Color.White
        rtb.Font = New Font("Consolas", 10 * s.UIScaleFactor)

        Dim devModeCaption = If(Registry.CurrentUser.GetBoolean("Software\" + Application.ProductName, "DevMode"), "User Mode", "Developer Mode")
        ActionMenuItem.Add(rtb.ContextMenuStrip.Items, devModeCaption, AddressOf ToggleDevMode)

        tv.SelectOnMouseDown = True
        tv.ShowLines = False
        tv.HideSelection = False
        tv.FullRowSelect = True
        tv.ShowPlusMinus = False
        tv.AutoCollaps = True
        tv.ExpandMode = TreeNodeExpandMode.InclusiveChilds

        SourcePath = fp
        Text = "MediaInfo - " + fp
        Parse()
        ActiveControl = stb

        AddHandler stb.TextChanged, Sub() If tv.SelectedNode Is tv.Nodes(1) Then UpdateItems() Else tv.SelectedNode = tv.Nodes(1)
    End Sub

    Sub ToggleDevMode()
        Registry.CurrentUser.Write("Software\" + Application.ProductName, "DevMode", Not Registry.CurrentUser.GetBoolean("Software\" + Application.ProductName, "DevMode"))
        Close()
        g.DefaultCommands.ShowMediaInfo(SourcePath)
    End Sub

    Sub UpdateItems()
        Dim newText As New StringBuilder
        Dim items As IEnumerable(Of Item)

        If ActiveGroup = "Advanced" Then
            items = Me.Items.Where(Function(item) item.IsComplete)
        ElseIf ActiveGroup = "Basic" Then
            items = Me.Items.Where(Function(item) Not item.IsComplete)
        Else
            Dim l As New List(Of Item)
            l.AddRange(Me.Items.Where(Function(item) Not item.IsComplete AndAlso item.Group = ActiveGroup))
            l.Add(New Item With {.Name = "", .Value = "", .Group = ActiveGroup})
            l.AddRange(Me.Items.Where(Function(item) item.IsComplete AndAlso item.Group = ActiveGroup))
            items = l
        End If

        Dim search = stb.Text.ToLower

        If search <> "" Then items = items.Where(Function(item) item.Name.ToLower.Contains(search) OrElse item.Value.ToLower.Contains(search))
        Dim groups As New List(Of String)

        For Each i In items
            If i.Group <> "" AndAlso Not groups.Contains(i.Group) Then
                groups.Add(i.Group)
            End If
        Next

        For Each group In groups
            If newText.Length = 0 Then
                newText.Append(group + BR2)
            Else
                newText.Append(BR + group + BR2)
            End If

            Dim itemsInGroup = items.Where(Function(item) item.Group = group)

            For Each i3 In itemsInGroup
                If i3.Name <> "" Then
                    newText.Append(i3.Name.PadRight(25))
                    newText.Append(": ")
                End If

                newText.Append(i3.Value)
                newText.Append(BR)
            Next
        Next

        rtb.Text = newText.ToString
    End Sub

    Protected Overrides Sub OnDragDrop(drgevent As DragEventArgs)
        MyBase.OnDragDrop(drgevent)

        Dim files = TryCast(drgevent.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            SourcePath = files(0)
            Text = "MediaInfo - " + SourcePath
            Parse()
        End If
    End Sub

    Protected Overrides Sub OnDragEnter(e As DragEventArgs)
        MyBase.OnDragEnter(e)

        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())
        If Not files.NothingOrEmpty Then e.Effect = DragDropEffects.Copy
    End Sub

    Public Class Item
        Property Name As String
        Property Value As String
        Property Group As String
        Property IsComplete As Boolean
    End Class

    Sub Parse()
        tv.Nodes.Clear()
        Items.Clear()

        Dim output = MediaInfo.GetCompleteSummary(SourcePath).FixBreak
        Dim group As String

        tv.Nodes.Add("Basic")
        tv.Nodes.Add("Advanced")

        For Each line In output.SplitLinesNoEmpty
            If line.Contains(":") Then
                Dim item As New Item
                item.Name = line.Left(":").Trim
                item.Value = line.Right(":").Trim
                item.Group = group
                item.IsComplete = True

                If item.Name Is Nothing Then item.Name = ""
                If item.Value Is Nothing Then item.Value = ""

                Items.Add(item)
            Else
                group = line.Trim
                tv.Nodes.Add(line.Trim)
            End If
        Next

        output = MediaInfo.GetSummary(SourcePath)

        For Each line In output.SplitLinesNoEmpty
            If line.Contains(":") Then
                Dim item As New Item
                item.Name = line.Left(":").Trim
                item.Value = line.Right(":").Trim
                item.Group = group

                If item.Name Is Nothing Then item.Name = ""
                If item.Value Is Nothing Then item.Value = ""

                If item.Name = "File size" AndAlso item.Value.EndsWith("GiB") Then
                    item.Value += " (" + CInt(CLng(MediaInfo.GetGeneral(SourcePath, "FileSize")) / 1024 / 1024).ToString + " MB)"
                End If

                If item.Name = "Unique ID" Then Continue For

                Items.Add(item)
            Else
                group = line.Trim
            End If
        Next

        For Each node As TreeNode In tv.Nodes
            For Each item In Items
                If item.Group = node.Text AndAlso item.Name = "Format" Then
                    node.Text += " (" + item.Value + ")"
                End If
            Next
        Next

        If stb.Text = "" Then
            tv.SelectedNode = tv.Nodes(0)
        Else
            tv.SelectedNode = tv.Nodes(1)
        End If
    End Sub

    Private Sub tv_AfterSelect(sender As Object, e As TreeViewEventArgs) Handles tv.AfterSelect
        Application.DoEvents()
        ActiveGroup = e.Node.Text
        UpdateItems()
    End Sub

    Private Sub rtb_KeyDown(sender As Object, e As KeyEventArgs) Handles rtb.KeyDown
        If e.KeyData = (Keys.Control Or Keys.C) Then
            Clipboard.SetText(rtb.SelectedText)
            e.Handled = True
        End If
    End Sub
End Class