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

    Friend WithEvents tv As TreeViewEx
    Friend WithEvents rtb As StaxRip.UI.RichTextBoxEx
    Friend WithEvents stb As StaxRip.SearchTextBox
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.tv = New StaxRip.UI.TreeViewEx()
        Me.rtb = New StaxRip.UI.RichTextBoxEx()
        Me.stb = New StaxRip.SearchTextBox()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.TableLayoutPanel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'tv
        '
        Me.tv.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tv.Location = New System.Drawing.Point(15, 101)
        Me.tv.Margin = New System.Windows.Forms.Padding(15, 15, 0, 15)
        Me.tv.Name = "tv"
        Me.tv.Size = New System.Drawing.Size(285, 1379)
        Me.tv.TabIndex = 2
        '
        'rtb
        '
        Me.rtb.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtb.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.rtb.Location = New System.Drawing.Point(315, 15)
        Me.rtb.Margin = New System.Windows.Forms.Padding(15)
        Me.rtb.Name = "rtb"
        Me.TableLayoutPanel1.SetRowSpan(Me.rtb, 2)
        Me.rtb.Size = New System.Drawing.Size(1679, 1465)
        Me.rtb.TabIndex = 4
        Me.rtb.Text = ""
        '
        'stb
        '
        Me.stb.Anchor = CType((System.Windows.Forms.AnchorStyles.Left Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.stb.Location = New System.Drawing.Point(15, 15)
        Me.stb.Margin = New System.Windows.Forms.Padding(15, 15, 0, 0)
        Me.stb.Name = "stb"
        Me.stb.Size = New System.Drawing.Size(285, 71)
        Me.stb.TabIndex = 5
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Controls.Add(Me.stb, 0, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.tv, 0, 1)
        Me.TableLayoutPanel1.Controls.Add(Me.rtb, 1, 0)
        Me.TableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(0, 0)
        Me.TableLayoutPanel1.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 2
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(2009, 1495)
        Me.TableLayoutPanel1.TabIndex = 6
        '
        'MediaInfoForm
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(288.0!, 288.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.ClientSize = New System.Drawing.Size(2009, 1495)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable
        Me.HelpButton = False
        Me.KeyPreview = True
        Me.Margin = New System.Windows.Forms.Padding(4, 5, 4, 5)
        Me.Name = "MediaInfoForm"
        Me.Text = "MediaInfo"
        Me.TableLayoutPanel1.ResumeLayout(False)
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
                newText.Append(i + BR2)
            Else
                newText.Append(BR + i + BR2)
            End If

            Dim itemsInGroup = items.Where(Function(v) v.Group = i)

            For Each i3 In itemsInGroup
                If i3.Name <> "" Then
                    newText.Append(i3.Name.PadRight(26))
                    newText.Append(": ")
                End If

                newText.Append(i3.Value)
                newText.Append(BR)
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

        rtb.SelectionLength = 0
        rtb.SelectionStart = 0
        rtb.ScrollToCaret()
        rtb.BlockPaint = False
        rtb.Refresh()
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

    Protected Overrides Sub OnLoad(e As EventArgs)
        MyBase.OnLoad(e)
        tv.ItemHeight = CInt(FontHeight * 1.5)
    End Sub
End Class