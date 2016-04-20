Imports StaxRip.UI
Imports System.Text

Class MediaInfoForm
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
    Friend WithEvents CommandLink1 As StaxRip.UI.CommandLink
    Friend WithEvents tv As StaxRip.UI.TreeViewEx
    Friend WithEvents cms As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents CopyValueToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyRowsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents CopyEverythingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents rtb As StaxRip.UI.RichTextBoxEx
    Friend WithEvents stb As StaxRip.SearchTextBox

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.cms = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.CopyValueToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyRowsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyEverythingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tv = New StaxRip.UI.TreeViewEx()
        Me.rtb = New StaxRip.UI.RichTextBoxEx()
        Me.stb = New StaxRip.SearchTextBox()
        Me.cms.SuspendLayout()
        Me.SuspendLayout()
        '
        'cms
        '
        Me.cms.ImageScalingSize = New System.Drawing.Size(24, 24)
        Me.cms.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CopyValueToolStripMenuItem, Me.CopyRowsToolStripMenuItem, Me.CopyEverythingToolStripMenuItem})
        Me.cms.Name = "cms"
        Me.cms.Size = New System.Drawing.Size(215, 94)
        '
        'CopyValueToolStripMenuItem
        '
        Me.CopyValueToolStripMenuItem.Name = "CopyValueToolStripMenuItem"
        Me.CopyValueToolStripMenuItem.Size = New System.Drawing.Size(214, 30)
        Me.CopyValueToolStripMenuItem.Text = "Copy Value"
        '
        'CopyRowsToolStripMenuItem
        '
        Me.CopyRowsToolStripMenuItem.Name = "CopyRowsToolStripMenuItem"
        Me.CopyRowsToolStripMenuItem.Size = New System.Drawing.Size(214, 30)
        Me.CopyRowsToolStripMenuItem.Text = "Copy Rows"
        '
        'CopyEverythingToolStripMenuItem
        '
        Me.CopyEverythingToolStripMenuItem.Name = "CopyEverythingToolStripMenuItem"
        Me.CopyEverythingToolStripMenuItem.Size = New System.Drawing.Size(214, 30)
        Me.CopyEverythingToolStripMenuItem.Text = "Copy Everything"
        '
        'tv
        '
        Me.tv.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tv.Location = New System.Drawing.Point(6, 43)
        Me.tv.Name = "tv"
        Me.tv.Size = New System.Drawing.Size(138, 624)
        Me.tv.TabIndex = 2
        '
        'rtb
        '
        Me.rtb.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtb.BlockPaint = False
        Me.rtb.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtb.Location = New System.Drawing.Point(147, 8)
        Me.rtb.Name = "rtb"
        Me.rtb.Size = New System.Drawing.Size(797, 659)
        Me.rtb.TabIndex = 4
        Me.rtb.Text = ""
        '
        'stb
        '
        Me.stb.Location = New System.Drawing.Point(6, 8)
        Me.stb.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
        Me.stb.Name = "stb"
        Me.stb.Size = New System.Drawing.Size(138, 31)
        Me.stb.TabIndex = 5
        '
        'MediaInfoForm
        '
        Me.AllowDrop = True
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None
        Me.ClientSize = New System.Drawing.Size(950, 673)
        Me.Controls.Add(Me.stb)
        Me.Controls.Add(Me.rtb)
        Me.Controls.Add(Me.tv)
        Me.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.HelpButton = False
        Me.KeyPreview = True
        Me.Location = New System.Drawing.Point(0, 0)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "MediaInfoForm"
        Me.Text = "MediaInfo"
        Me.cms.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private SourcePath As String
    Private ActiveGroup As String
    Private Items As New List(Of Item)

    Sub New(fp As String)
        MyBase.New()
        InitializeComponent()

        rtb.WordWrap = False
        rtb.ReadOnly = True
        rtb.BackColor = Color.White
        rtb.Font = New Font("Consolas", 10 * s.UIScaleFactor)

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

    Sub UpdateItems()
        Dim newText As New StringBuilder

        Dim items As IEnumerable(Of Item)

        If ActiveGroup = "Advanced" Then
            items = Me.Items.Where(Function(i) i.IsComplete)
        ElseIf ActiveGroup = "Basic" Then
            items = Me.Items.Where(Function(i) Not i.IsComplete)
        Else
            Dim l As New List(Of Item)
            l.AddRange(Me.Items.Where(Function(i) Not i.IsComplete AndAlso i.Group = ActiveGroup))
            l.Add(New Item With {.Name = "", .Value = "", .Group = ActiveGroup})
            l.AddRange(Me.Items.Where(Function(i) i.IsComplete AndAlso i.Group = ActiveGroup))
            items = l
        End If

        Dim search = stb.Text.ToLower

        If search <> "" Then
            items = items.Where(Function(i) i.Name.ToLower.Contains(search) OrElse i.Value.ToLower.Contains(search))
        End If

        Dim groups As New List(Of String)

        For Each i In items
            If i.Group <> "" AndAlso Not groups.Contains(i.Group) Then
                groups.Add(i.Group)
            End If
        Next

        For Each i In groups
            If newText.Length = 0 Then
                newText.Append(i + CrLf2)
            Else
                newText.Append(CrLf + i + CrLf2)
            End If

            Dim itemsInGroup = items.Where(Function(v) v.Group = i)

            For Each i3 In itemsInGroup
                If i3.Name <> "" Then
                    newText.Append(i3.Name.PadRight(26))
                    newText.Append(": ")
                End If

                newText.Append(i3.Value)
                newText.Append(CrLf)
            Next
        Next

        rtb.BlockPaint = True
        rtb.Text = ""
        rtb.SelectionFont = New Font("Consolas", 10 * s.UIScaleFactor)
        rtb.SelectionColor = Color.Black
        rtb.Text = newText.ToString

        Dim lines = rtb.Lines

        For x = 0 To lines.Length - 1
            If groups.Contains(lines(x)) Then
                rtb.Select(rtb.GetFirstCharIndexFromLine(x), lines(x).Length)
                rtb.SelectionFont = New Font("Consolas", 10 * s.UIScaleFactor, FontStyle.Bold)
                rtb.SelectionColor = ControlPaint.Dark(ToolStripRendererEx.ColorBorder, 0)
            End If
        Next

        rtb.SelectionStart = 0
        rtb.ScrollToCaret()
        rtb.BlockPaint = False
        rtb.Refresh()
    End Sub

    Private Sub MediaInfoForm_DragDrop(sender As Object, e As DragEventArgs) Handles Me.DragDrop
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())

        If Not files.NothingOrEmpty Then
            SourcePath = files(0)
            Text = "MediaInfo - " + SourcePath
            Parse()
        End If
    End Sub

    Private Sub MediaInfoForm_DragEnter(sender As Object, e As DragEventArgs) Handles Me.DragEnter
        Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())
        If Not files.NothingOrEmpty Then e.Effect = DragDropEffects.Copy
    End Sub

    Class Item
        Property Name As String
        Property Value As String
        Property Group As String
        Property IsComplete As Boolean
    End Class

    Sub Parse()
        tv.Nodes.Clear()
        Items.Clear()

        Dim output = MediaInfo.GetCompleteSummary(SourcePath)
        Dim group As String

        tv.Nodes.Add("Basic")
        tv.Nodes.Add("Advanced")

        For Each i In output.SplitLinesNoEmpty
            If i.Contains(":") Then
                Dim item As New Item
                item.Name = i.Left(":").Trim
                item.Value = i.Right(":").Trim
                item.Group = group
                item.IsComplete = True

                If item.Name Is Nothing Then item.Name = ""
                If item.Value Is Nothing Then item.Value = ""

                Items.Add(item)
            Else
                group = i.Trim
                tv.Nodes.Add(i.Trim)
            End If
        Next

        output = MediaInfo.GetSummary(SourcePath)

        For Each i In output.SplitLinesNoEmpty
            If i.Contains(":") Then
                Dim item As New Item
                item.Name = i.Left(":").Trim
                item.Value = i.Right(":").Trim
                item.Group = group

                If item.Name Is Nothing Then item.Name = ""
                If item.Value Is Nothing Then item.Value = ""

                If item.Name = "File size" AndAlso item.Value.EndsWith("GiB") Then
                    item.Value += " (" + CInt(CLng(MediaInfo.GetGeneral(SourcePath, "FileSize")) / 1024 / 1024).ToString + " MB)"
                End If

                If item.Name = "Unique ID" Then Continue For

                Items.Add(item)
            Else
                group = i.Trim
            End If
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

    Private Sub MediaInfoForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        tv.ItemHeight = FontHeight * 2
    End Sub
End Class