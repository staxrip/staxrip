Imports System.Reflection
Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Drawing.Drawing2D
Imports System.Windows.Forms.VisualStyles
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Drawing.Design
Imports System.Threading
Imports System.Threading.Tasks

Namespace UI
    Public Class TreeViewEx
        Inherits TreeView

        Private AutoCollapsValue As Boolean

        <DefaultValue(False)>
        Property AutoCollaps As Boolean

        <DefaultValue(GetType(TreeNodeExpandMode), "Disabled")>
        Property ExpandMode As TreeNodeExpandMode

        <DefaultValue(False)>
        Property SelectOnMouseDown() As Boolean

        Protected Overrides Sub OnAfterSelect(e As TreeViewEventArgs)
            If AutoCollaps Then
                For Each i As TreeNode In If(e.Node.Parent Is Nothing, Nodes, e.Node.Parent.Nodes)
                    If Not i Is e.Node Then
                        i.Collapse()
                    End If
                Next
            End If

            Select Case ExpandMode
                Case TreeNodeExpandMode.Normal
                    e.Node.Expand()
                Case TreeNodeExpandMode.InclusiveChilds
                    e.Node.ExpandAll()
            End Select

            MyBase.OnAfterSelect(e)
        End Sub

        Protected Overrides Sub WndProc(ByRef m As Message)
            If SelectOnMouseDown AndAlso m.Msg = Native.WM_LBUTTONDOWN Then
                Dim n = GetNodeAt(ClientMousePos)

                If Not n Is Nothing AndAlso n.Nodes.Count = 0 Then
                    SelectedNode = n
                    Focus()
                    Exit Sub
                End If
            End If

            MyBase.WndProc(m)
        End Sub

        Sub MoveSelectionLeft()
            Dim n As TreeNode = SelectedNode

            If Not n Is Nothing AndAlso Not n.Parent Is Nothing Then
                Dim parentParentNodes As TreeNodeCollection = GetParentParentNodes(n)
                Dim parentIndex As Integer = n.Parent.Index

                If Not parentParentNodes Is Nothing Then
                    n.Remove()
                    parentParentNodes.Insert(parentIndex + 1, n)
                    SelectedNode = n
                End If
            End If
        End Sub

        Sub MoveSelectionUp()
            Dim n As TreeNode = SelectedNode

            If Not n Is Nothing Then
                If n.Index = 0 Then
                    Dim parentParentNodes As TreeNodeCollection = GetParentParentNodes(n)

                    If Not parentParentNodes Is Nothing Then
                        Dim index As Integer = n.Parent.Index
                        n.Remove()
                        parentParentNodes.Insert(index, n)
                    End If
                Else
                    Dim index As Integer = n.Index
                    Dim parentNodes As TreeNodeCollection = GetParentNodes(n)
                    n.Remove()
                    parentNodes.Insert(index - 1, n)
                End If

                SelectedNode = n
            End If
        End Sub

        Sub MoveSelectionRight()
            Dim n As TreeNode = SelectedNode

            If Not n Is Nothing Then
                If n.Index > 0 Then
                    Dim previousNode As TreeNode = n.PrevNode
                    n.Remove()
                    previousNode.Nodes.Add(n)
                    SelectedNode = n
                End If
            End If
        End Sub

        Sub MoveSelectionDown()
            Dim n As TreeNode = SelectedNode

            If Not n Is Nothing Then
                If n.NextNode Is Nothing Then
                    Dim parentParentNodes As TreeNodeCollection = GetParentParentNodes(n)

                    If Not parentParentNodes Is Nothing Then
                        Dim index As Integer = n.Parent.Index
                        n.Remove()
                        parentParentNodes.Insert(index + 1, n)
                    End If
                Else
                    Dim index As Integer = n.Index
                    Dim parentNodes As TreeNodeCollection = GetParentNodes(n)
                    n.Remove()
                    parentNodes.Insert(index + 1, n)
                End If

                SelectedNode = n
            End If
        End Sub

        Private Function GetParentParentNodes(n As TreeNode) As TreeNodeCollection
            Dim parent = n.Parent

            If parent Is Nothing Then
                Return Nothing
            End If

            If parent.Parent Is Nothing Then
                Return Nodes
            Else
                Return parent.Parent.Nodes
            End If
        End Function

        Private Function GetParentNodes(n As TreeNode) As TreeNodeCollection
            Dim parent As TreeNode = n.Parent

            If parent Is Nothing Then
                Return Nodes
            Else
                Return parent.Nodes
            End If
        End Function

        Protected Overrides Sub OnHandleCreated(e As EventArgs)
            MyBase.OnHandleCreated(e)
            Native.SetWindowTheme(Handle, "explorer", Nothing)
        End Sub

        Function AddNode(path As String) As TreeNode
            Dim pathElements = path.SplitNoEmptyAndWhiteSpace("|")
            Dim currentNodeList = Nodes
            Dim currentPath = ""
            Dim ret As TreeNode = Nothing

            For Each iNodeName In pathElements
                If currentPath <> "" Then
                    currentPath += "|"
                End If

                currentPath += iNodeName
                Dim found = False

                For Each iNode As TreeNode In currentNodeList
                    If iNode.Text = iNodeName Then
                        ret = iNode
                        currentNodeList = iNode.Nodes
                        found = True
                    End If
                Next

                If Not found Then
                    ret = New TreeNode
                    ret.Text = iNodeName
                    currentNodeList.Add(ret)
                    currentNodeList = ret.Nodes
                End If
            Next

            Return ret
        End Function

        Function GetNodes() As List(Of TreeNode)
            Dim ret As New List(Of TreeNode)
            AddNodesRecursive(Nodes, ret)
            Return ret
        End Function

        Shared Sub AddNodesRecursive(searchList As TreeNodeCollection, returnList As List(Of TreeNode))
            For Each i As TreeNode In searchList
                returnList.Add(i)
                AddNodesRecursive(i.Nodes, returnList)
            Next
        End Sub
    End Class

    Public Enum TreeNodeExpandMode
        Disabled
        Normal
        InclusiveChilds
    End Enum

    Public Class ToolStripEx
        Inherits ToolStrip

        Sub New()
            MyBase.New()
        End Sub

        Sub New(ParamArray items As ToolStripItem())
            MyBase.New(items)
        End Sub

        <DefaultValue(False), Description("Overdraws a weird grey line visible at the bottom in some configurations.")>
        Property OverdrawLine As Boolean

        <DefaultValue(False), Description("Only one button can be checked at the same time.")>
        Property SingleAutoChecked As Boolean

        <DefaultValue(False), Description("Shows a themed control border.")>
        Property ShowControlBorder As Boolean

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            If OverdrawLine Then
                Dim b As New SolidBrush(BackColor)
                e.Graphics.FillRectangle(b, 0, Height - 2, Width, 2)
                b.Dispose()
            End If

            If ShowControlBorder AndAlso VisualStyleInformation.IsEnabledByUser Then
                ControlPaint.DrawBorder(e.Graphics,
                                        ClientRectangle,
                                        VisualStyleInformation.TextControlBorder,
                                        ButtonBorderStyle.Solid)
            End If
        End Sub

        Protected Overrides Sub OnItemClicked(e As ToolStripItemClickedEventArgs)
            If SingleAutoChecked Then
                For Each i In Items.OfType(Of ToolStripButton)()
                    If i Is e.ClickedItem Then
                        i.Checked = True
                    Else
                        i.Checked = False
                    End If
                Next
            End If

            MyBase.OnItemClicked(e)
        End Sub

        Protected Overrides ReadOnly Property CreateParams() As CreateParams
            Get
                Dim ret = MyBase.CreateParams

                If ShowControlBorder AndAlso Not VisualStyleInformation.IsEnabledByUser Then
                    ret.ExStyle = ret.ExStyle Or Native.WS_EX_CLIENTEDGE
                End If

                Return ret
            End Get
        End Property
    End Class

    Public Class LineControl
        Inherits Control

        Public Sub New()
            Margin = New Padding(4, 2, 5, 2)
            Anchor = AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right
            SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim textOffset As Integer
            Dim lineHeight = CInt(Height / 2)

            If Text <> "" Then
                Dim textSize = e.Graphics.MeasureString(Text, Font)
                textOffset = CInt(textSize.Width)

                Using b = New SolidBrush(If(Enabled, ForeColor, SystemColors.GrayText))
                    e.Graphics.DrawString(Text, Font, b, 0, CInt((Height - textSize.Height) / 2) - 1)
                End Using
            End If

            If Enabled Then
                e.Graphics.DrawLine(Pens.Silver, textOffset, lineHeight, Width, lineHeight)
                e.Graphics.DrawLine(Pens.White, textOffset, lineHeight + 1, Width, lineHeight + 1)
            Else
                e.Graphics.DrawLine(SystemPens.InactiveBorder, textOffset, lineHeight, Width, lineHeight)
            End If

            MyBase.OnPaint(e)
        End Sub
    End Class

    Public Class CommandLink
        Inherits Button

        Const BS_COMMANDLINK As Integer = &HE

        Const BCM_SETSHIELD As Integer = &H160C
        Const BCM_SETNOTE As Integer = &H1609
        Const BCM_GETNOTE As Integer = &H160A
        Const BCM_GETNOTELENGTH As Integer = &H160B

        Sub New()
            Me.FlatStyle = FlatStyle.System
        End Sub

        Protected Overloads Overrides ReadOnly Property CreateParams() As CreateParams
            Get
                Dim r = MyBase.CreateParams
                r.Style = r.Style Or BS_COMMANDLINK
                Return r
            End Get
        End Property

        Private NoteValue As String = ""

        <DefaultValue("")>
        Property Note() As String
            Get
                Return NoteValue
            End Get
            Set(value As String)
                Native.SendMessage(Handle, BCM_SETNOTE, 0, value)
                NoteValue = value
            End Set
        End Property

        Protected Overrides Sub OnCreateControl()
            If OK(Note) Then
                Text += CrLf2 + Note
            End If

            MyBase.OnCreateControl()
        End Sub
    End Class

    Public Class TextBoxEx
        Inherits TextBox

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property ValidationFunc As Func(Of String, Boolean)

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(value As String)
                MyBase.Name = value
            End Set
        End Property

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property TabIndex() As Integer
            Get
                Return MyBase.TabIndex
            End Get
            Set(value As Integer)
                MyBase.TabIndex = value
            End Set
        End Property

        Protected Overrides Sub OnValidating(e As CancelEventArgs)
            If Not ValidationFunc Is Nothing Then
                e.Cancel = Not ValidationFunc.Invoke(Text)
            End If

            MyBase.OnValidating(e)
        End Sub
    End Class

    Public Class PanelEx
        Inherits Panel

        Private ShowNiceBorderValue As Boolean

        Sub New()
            SetStyle(ControlStyles.ResizeRedraw, True)
        End Sub

        <DefaultValue(False), Description("Nicer border if themes are enabled.")>
        Property ShowNiceBorder() As Boolean
            Get
                Return ShowNiceBorderValue
            End Get
            Set(Value As Boolean)
                ShowNiceBorderValue = Value
                Invalidate()
            End Set
        End Property

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            If ShowNiceBorder AndAlso VisualStyleInformation.IsEnabledByUser Then
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle,
                    VisualStyleInformation.TextControlBorder, ButtonBorderStyle.Solid)
            End If
        End Sub
    End Class

    Public Class CheckBoxEx
        Inherits CheckBox

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(value As String)
                MyBase.Name = value
            End Set
        End Property

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property TabIndex() As Integer
            Get
                Return MyBase.TabIndex
            End Get
            Set(value As Integer)
                MyBase.TabIndex = value
            End Set
        End Property
    End Class

    Public Class RichTextBoxEx
        Inherits RichTextBox

        Property BlockPaint As Boolean

        Private BorderRect As Native.RECT

        Sub New(Optional cms As ContextMenuStrip = Nothing)
            If VisualStyleInformation.IsEnabledByUser Then
                BorderStyle = BorderStyle.None
            End If

            If cms Is Nothing Then
                ContextMenuStrip = New ContextMenuStrip
            Else
                ContextMenuStrip = cms
            End If

            AddHandler Disposed, Sub() If Not ContextMenuStrip Is Nothing Then ContextMenuStrip.Dispose()

            Dim cutItem = ContextMenuStrip.Items.Add("Cut")
            Dim copyItem = ContextMenuStrip.Items.Add("Copy")
            Dim pasteItem = ContextMenuStrip.Items.Add("Paste")

            AddHandler cutItem.Click, Sub()
                                          Clipboard.SetText(SelectedText)
                                          SelectedText = ""
                                      End Sub

            AddHandler copyItem.Click, Sub() Clipboard.SetText(SelectedText)

            AddHandler pasteItem.Click, Sub() Paste()

            AddHandler ContextMenuStrip.Opening, Sub()
                                                     cutItem.Enabled = SelectionLength > 0 AndAlso Not [ReadOnly]
                                                     copyItem.Enabled = SelectionLength > 0
                                                     pasteItem.Enabled = Clipboard.GetText <> "" AndAlso Not [ReadOnly]
                                                 End Sub
        End Sub

        Protected Overrides Sub WndProc(ByRef m As Message)
            Const WM_NCPAINT = &H85
            Const WM_NCCALCSIZE = &H83
            Const WM_THEMECHANGED = &H31A

            Select Case m.Msg
                Case Native.WM_PAINT, Native.WM_ERASEBKGND
                    If BlockPaint Then Exit Sub
                Case Else
            End Select

            MyBase.WndProc(m)

            Select Case m.Msg
                Case WM_NCPAINT
                    WmNcpaint(m)
                Case WM_NCCALCSIZE
                    WmNccalcsize(m)
                Case WM_THEMECHANGED
                    UpdateStyles()
            End Select
        End Sub

        Private Sub WmNccalcsize(ByRef m As Message)
            If Not VisualStyleInformation.IsEnabledByUser Then Return

            Dim par As New Native.NCCALCSIZE_PARAMS()
            Dim windowRect As Native.RECT

            If m.WParam <> IntPtr.Zero Then
                par = CType(Marshal.PtrToStructure(m.LParam, GetType(Native.NCCALCSIZE_PARAMS)), Native.NCCALCSIZE_PARAMS)
                windowRect = par.rgrc0
            End If

            Dim clientRect = windowRect

            clientRect.Left += 1
            clientRect.Top += 1
            clientRect.Right -= 1
            clientRect.Bottom -= 1

            BorderRect = New Native.RECT(clientRect.Left - windowRect.Left,
                                         clientRect.Top - windowRect.Top,
                                         windowRect.Right - clientRect.Right,
                                         windowRect.Bottom - clientRect.Bottom)

            If m.WParam = IntPtr.Zero Then
                Marshal.StructureToPtr(clientRect, m.LParam, False)
            Else
                par.rgrc0 = clientRect
                Marshal.StructureToPtr(par, m.LParam, False)
            End If

            Const WVR_HREDRAW = &H100
            Const WVR_VREDRAW = &H200
            Const WVR_REDRAW = (WVR_HREDRAW Or WVR_VREDRAW)

            m.Result = New IntPtr(WVR_REDRAW)
        End Sub

        Private Sub WmNcpaint(ByRef m As Message)
            If Not VisualStyleInformation.IsEnabledByUser Then Return

            Dim r As Native.RECT
            Native.GetWindowRect(Handle, r)

            r.Right -= r.Left
            r.Bottom -= r.Top
            r.Top = 0
            r.Left = 0

            r.Left += BorderRect.Left
            r.Top += BorderRect.Top
            r.Right -= BorderRect.Right
            r.Bottom -= BorderRect.Bottom

            Dim hDC = Native.GetWindowDC(Handle)
            Native.ExcludeClipRect(hDC, r.Left, r.Top, r.Right, r.Bottom)

            Using g = Graphics.FromHdc(hDC)
                g.Clear(Color.CadetBlue)
            End Using

            Native.ReleaseDC(Handle, hDC)
            m.Result = IntPtr.Zero
        End Sub

        Protected Overrides Sub OnHandleCreated(e As EventArgs)
            MyBase.OnHandleCreated(e)
            AutoWordSelection = False
        End Sub
    End Class

    Public Class TrackBarEx
        Inherits TrackBar

        Shadows Property Value As Integer
            Get
                Return MyBase.Value
            End Get
            Set(value As Integer)
                If value > Maximum Then value = Maximum
                If value < Minimum Then value = Minimum
                If value <> MyBase.Value Then MyBase.Value = value
            End Set
        End Property
    End Class

    Public Class LabelEx
        Inherits Label

        Sub New()
            TextAlign = Drawing.ContentAlignment.MiddleLeft
        End Sub

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(value As String)
                MyBase.Name = value
            End Set
        End Property

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property TabIndex() As Integer
            Get
                Return MyBase.TabIndex
            End Get
            Set(value As Integer)
                MyBase.TabIndex = value
            End Set
        End Property
    End Class

    Public Class MultiFolderTree
        Inherits TreeViewEx

        Property Paths As List(Of String) = New List(Of String)

        Sub New()
            AutoCollaps = True
            ExpandMode = TreeNodeExpandMode.Normal
        End Sub

        Sub Init()
            CheckBoxes = True
            Dim drives As New List(Of String)
            Dim volumes As New List(Of String)

            If Not DesignMode Then
                For Each i As DriveInfo In DriveInfo.GetDrives
                    If i.DriveType = DriveType.Fixed Then
                        drives.Add(i.RootDirectory.Name)

                        Try 'user reported crash
                            volumes.Add(i.VolumeLabel)
                        Catch ex As Exception
                            volumes.Add(i.Name)
                        End Try
                    End If
                Next

                Init(Nodes, drives.ToArray, volumes)

                For Each i As FolderTreeNode In Nodes
                    Init(i.Nodes, GetDirs(i.Path))
                Next
            End If
        End Sub

        Sub Init(nodes As TreeNodeCollection,
                 directories As String(),
                 Optional volumes As List(Of String) = Nothing)

            For i = 0 To directories.Length - 1
                Dim directory = directories(i)

                Try
                    Dim di As New DirectoryInfo(directory)

                    If directory.Length = 3 OrElse (di.Attributes And FileAttributes.Hidden) = 0 Then
                        Dim n As New FolderTreeNode
                        n.Path = directory

                        If volumes Is Nothing Then
                            n.Text = DirPath.GetName(directory)
                        Else
                            n.Text = volumes(i) + " (" + directory.Substring(0, 2) + ")"
                        End If

                        If n.Text = "" Then
                            n.Text = directory
                        End If

                        nodes.Add(n)
                    End If
                Catch
                End Try
            Next
        End Sub

        <DebuggerNonUserCode()>
        Private Function GetDirs(path As String) As String()
            Try
                Return Directory.GetDirectories(path)
            Catch ex As Exception
                Return New String() {}
            End Try
        End Function

        Protected Overrides Sub OnBeforeExpand(e As TreeViewCancelEventArgs)
            MyBase.OnBeforeExpand(e)

            Dim n = DirectCast(e.Node, FolderTreeNode)

            For Each i As FolderTreeNode In n.Nodes
                If Not i.WasInitialized Then
                    Init(i.Nodes, GetDirs(i.Path))
                    i.WasInitialized = True
                End If
            Next
        End Sub

        Protected Overrides Sub OnAfterCheck(e As TreeViewEventArgs)
            Dim n = DirectCast(e.Node, FolderTreeNode)

            If e.Node.Checked Then
                Paths.Add(n.Path)
            Else
                Paths.Remove(n.Path)
            End If

            MyBase.OnAfterCheck(e)
        End Sub

        Protected Overrides Sub OnClick(e As EventArgs)
            MyBase.OnClick(e)

            Dim n = GetNodeAt(ClientMousePos)

            If Not n Is Nothing AndAlso (n.Nodes.Count = 0 OrElse
                n.IsExpanded) AndAlso n.Bounds.Contains(ClientMousePos) Then

                n.Checked = Not n.Checked
            End If
        End Sub
    End Class

    Public Class FolderTreeNode
        Inherits TreeNode

        Public Path As String
        Public WasInitialized As Boolean
    End Class

    Public Class BeforeShowControlEventArgs
        Inherits EventArgs

        Public Control As Control
        Public Position As Point
        Public Cancel As Boolean
    End Class

    Public Class PropertyGridEx
        Inherits PropertyGrid

        Sub New()
            ToolbarVisible = False 'GridView is not ready when PropertySortChanged happens so Init fails
        End Sub

        Protected Overrides Sub OnSelectedGridItemChanged(e As SelectedGridItemChangedEventArgs)
            MyBase.OnSelectedGridItemChanged(e)

            Dim help = e.NewSelection.PropertyDescriptor.Description

            If help <> "" Then
                HelpVisible = True

                Dim lines = CInt(Math.Ceiling(CreateGraphics.MeasureString(
                    help, Font, Width).Height / Font.Height))

                Dim r As New Reflector(Me, GetType(PropertyGrid))
                Dim doc = r.Invoke("doccomment")
                doc.Invoke("Lines", lines + 1)
                doc.Invoke("userSized", True)
                r.Invoke("OnLayoutInternal", False)
            Else
                HelpVisible = False
            End If
        End Sub

        Sub MoveSplitter(x As Integer)
            Dim grid As New Reflector(Me, GetType(PropertyGrid))
            grid.Invoke("gridView").Invoke("MoveSplitterTo", x)
        End Sub
    End Class

    <DefaultEvent("LinkClick")>
    Public Class LinkGroupBox
        Inherits GroupBox

        Public WithEvents ll As New LinkLabel
        Event LinkClick()

        Sub New()
            ll.Left = 8
            ll.AutoSize = True
            Controls.Add(ll)
        End Sub

        Overrides Property Text() As String
            Get
                Return ll.Text
            End Get
            Set(value As String)
                ll.Text = value
            End Set
        End Property

        Private Sub Label_Click() Handles ll.Click
            ShowContext()
            RaiseEvent LinkClick()
        End Sub

        Private Sub ShowContext()
            If Not ll.ContextMenuStrip Is Nothing Then
                ll.ContextMenuStrip.Show(ll, 0, 16)
            End If
        End Sub
    End Class

    Public Class CheckedListBoxEx
        Inherits CheckedListBox

        <DefaultValue(CStr(Nothing))> Property UpButton As Button
        <DefaultValue(CStr(Nothing))> Property DownButton As Button
        <DefaultValue(CStr(Nothing))> Property RemoveButton As Button

        Event ItemsChanged()

        Sub OnItemsChanged()
            RaiseEvent ItemsChanged()
        End Sub

        Sub MoveSelectedItemUp()
            Dim i = SelectedIndex

            If i > 0 Then
                Dim state = GetItemChecked(i)
                Items.Insert(i - 1, SelectedItem)
                Items.RemoveAt(i + 1)
                SelectedIndex = i - 1
                SetItemChecked(SelectedIndex, state)
                OnItemsChanged()
            End If
        End Sub

        Sub MoveSelectedItemDown()
            Dim i = SelectedIndex

            If i < Items.Count - 1 Then
                Dim state = GetItemChecked(i)
                Items.Insert(i + 2, SelectedItem)
                Items.RemoveAt(i)
                SelectedIndex = i + 1
                SetItemChecked(SelectedIndex, state)
                OnItemsChanged()
            End If
        End Sub

        Sub RemoveSelection()
            Dim i = SelectedIndex
            Items.Remove(SelectedItem)
            OnItemsChanged()

            If Items.Count > i Then
                SelectedIndex = i
            Else
                If Items.Count > 0 Then
                    SelectedIndex = Items.Count - 1
                End If
            End If
        End Sub

        Protected Overrides Sub OnSelectedIndexChanged(e As EventArgs)
            MyBase.OnSelectedIndexChanged(e)
            UpdateControls()
        End Sub

        Sub UpdateControls()
            If Not RemoveButton Is Nothing Then RemoveButton.Enabled = Not SelectedItem Is Nothing
            If Not UpButton Is Nothing Then UpButton.Enabled = SelectedIndex > 0
            If Not DownButton Is Nothing Then DownButton.Enabled = SelectedIndex < Items.Count - 1 AndAlso SelectedIndex >= 0
        End Sub

        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()

            If Not DesignMode Then
                If Not UpButton Is Nothing Then UpButton.AddClickAction(AddressOf MoveSelectedItemUp)
                If Not DownButton Is Nothing Then DownButton.AddClickAction(AddressOf MoveSelectedItemDown)
                If Not RemoveButton Is Nothing Then RemoveButton.AddClickAction(AddressOf RemoveSelection)
            End If
        End Sub
    End Class

    Public Class MenuButton
        Inherits ButtonEx

        Event ValueChangedUser(value As Object)

        Property Menu As New ContextMenuStrip

        Public Sub New()
            Menu.ShowImageMargin = False
            ShowMenuSymbol = True

            AddHandler Menu.Opening, AddressOf MenuOpening
        End Sub

        Sub MenuOpening(sender As Object, e As CancelEventArgs)
            Menu.MinimumSize = New Size(Width, 0)

            For Each i As ActionMenuItem In Menu.Items
                If Not Value Is Nothing AndAlso Value.Equals(i.Tag) Then
                    i.Font = New Font(i.Font, FontStyle.Bold)
                Else
                    i.Font = New Font(i.Font, FontStyle.Regular)
                End If

                If (Menu.Width - i.Width) > 2 Then
                    i.AutoSize = False
                    i.Width = Menu.Width - 1
                End If
            Next
        End Sub

        Private ValueValue As Object

        <DefaultValue(CStr(Nothing))>
        Property Value As Object
            Get
                Return ValueValue
            End Get
            Set(value As Object)
                If Menu.Items.Count = 0 Then
                    If TypeOf value Is System.Enum Then
                        For Each i In System.Enum.GetValues(value.GetType)
                            Dim text = DispNameAttribute.GetValueForEnum(i)
                            Dim temp = i

                            ActionMenuItem.Add(Menu.Items, text, Sub(o As Object) OnAction(text, o), temp, Nothing).Tag = temp
                        Next
                    End If
                End If

                For Each i In Menu.Items.OfType(Of ActionMenuItem)()
                    If value.Equals(i.Tag) Then Text = i.Text
                Next

                If Text = "" AndAlso Not value Is Nothing Then
                    Text = value.ToString
                End If

                ValueValue = value
            End Set
        End Property

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            If e.Button = MouseButtons.Left Then
                Menu.Show(Me, 0, Height)
            Else
                MyBase.OnMouseDown(e)
            End If
        End Sub

        Protected Overridable Sub OnValueChanged(value As Object)
            RaiseEvent ValueChangedUser(value)
        End Sub

        Sub Add(items As IEnumerable(Of Object))
            For Each i In items
                Add(i.ToString, i, Nothing)
            Next
        End Sub

        Sub Add(path As String, obj As Object, Optional tip As String = Nothing)
            Dim name = path

            If path.Contains("|") Then
                name = path.RightLast("|").Trim
            End If

            ActionMenuItem.Add(Menu.Items, path, Sub(o As Object) OnAction(name, o), obj, tip).Tag = obj
        End Sub

        Private Sub OnAction(text As String, value As Object)
            Me.Text = text
            Me.Value = value
            OnValueChanged(value)
        End Sub

        Function GetValue(Of T)() As T
            Return DirectCast(Value, T)
        End Function

        Function GetInt() As Integer
            Return DirectCast(Value, Integer)
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)
            Menu.Dispose()
            MyBase.Dispose(disposing)
        End Sub
    End Class

    Public Class CmdlRichTextBox
        Inherits RichTextBoxEx

        Public Sub New()
            Font = New Font("Tahoma", 9, FontStyle.Regular)
        End Sub

        Property LastCmdl As String

        Sub SetText(cmdl As String)
            If cmdl = LastCmdl Then
                Exit Sub
            End If

            If Not OK(cmdl) Then
                Text = ""
                LastCmdl = ""
                Exit Sub
            End If

            BlockPaint = True
            Text = cmdl
            SelectAll()
            SelectionColor = ForeColor
            SelectionFont = New Font(Font, FontStyle.Regular)

            If LastCmdl <> "" Then
                Dim selStart = GetCompareIndex(cmdl, LastCmdl)
                Dim selEnd = cmdl.Length - GetCompareIndex(ReverseString(cmdl), ReverseString(LastCmdl))

                If selEnd > selStart AndAlso selEnd - selStart < cmdl.Length - 1 Then
                    While selStart > 0 AndAlso selStart + 1 < cmdl.Length AndAlso
                        Not cmdl(selStart - 1) + cmdl(selStart) = " -"

                        selStart = selStart - 1
                    End While

                    If selEnd - selStart < 25 Then
                        SelectionStart = selStart

                        If selEnd - selStart = cmdl.Length Then
                            SelectionLength = 0
                        Else
                            SelectionLength = selEnd - selStart
                        End If

                        SelectionFont = New Font(Font, FontStyle.Bold)
                    End If
                End If
            End If

            SelectionStart = cmdl.Length
            BlockPaint = False
            Refresh()
            LastCmdl = cmdl
        End Sub

        Function GetCompareIndex(a As String, b As String) As Integer
            For x = 0 To a.Length - 1
                If x > b.Length - 1 OrElse x > a.Length - 1 OrElse a(x) <> b(x) Then
                    Return x
                End If
            Next

            Return 0
        End Function

        Function ReverseString(value As String) As String
            Dim a = value.ToCharArray
            Array.Reverse(a)
            Return New String(a)
        End Function
    End Class

    Public Class StockIconLinkLabel
        Inherits LinkLabel

        Private Img As Image

        Public Sub New()
            Padding = New Padding(18, 0, 0, 0)
            MinimumSize = New Size(18, 18)
        End Sub

        <DefaultValue(GetType(StockIconIdentifier), "Info")>
        Property Icon As StockIconIdentifier = StockIconIdentifier.Info

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            If Img Is Nothing Then
                Img = StockIcon.GetSmallImage(Icon)
            End If

            If Not Img Is Nothing Then
                e.Graphics.DrawImage(Img, 0, 0)
            End If
        End Sub
    End Class

    Public Class WikiLinkLabel
        Inherits LinkLabel

        Private MarkupValue As String

        <Editor(GetType(StringEditor), GetType(UITypeEditor))>
        Public Property Markup As String
            Get
                Return MarkupValue
            End Get
            Set(value As String)
                MarkupValue = value

                Links.Clear()

                If value.Contains("[") Then
                    Dim re As New Regex("\[(.+?) (.+?)\]")

                    While True
                        Dim m = re.Match(value)

                        If m.Success Then
                            Links.Add(m.Index, m.Groups(2).Value.Length, m.Groups(1).Value)
                            value = value.Replace(m.Value, m.Groups(2).Value)
                        Else
                            Exit While
                        End If
                    End While
                End If

                Text = value
            End Set
        End Property

        <Browsable(False),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Overrides Property Text As String
            Get
                Return MyBase.Text
            End Get
            Set(value As String)
                MyBase.Text = value
            End Set
        End Property

        Protected Overrides Sub OnLinkClicked(e As LinkLabelLinkClickedEventArgs)
            Dim t = e.Link.LinkData.ToString

            If t.StartsWith("mailto:") OrElse t.StartsWith("http://") Then
                Process.Start(t)
            End If

            MyBase.OnLinkClicked(e)
        End Sub
    End Class

    <ProvideProperty("Expand", GetType(Control))>
    Class FlowLayoutPanelEx
        Inherits FlowLayoutPanel
        Implements IExtenderProvider

        <DefaultValue(False)>
        Property UseParenWidth As Boolean

        Property AutomaticOffset As Boolean

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property ExpandedControls As New Dictionary(Of Control, Boolean)

        Sub New()
            WrapContents = False
        End Sub

        <DefaultValue(False)>
        <DisplayName("Expand")>
        Function GetExpand(c As Control) As Boolean
            Return ExpandedControls.ContainsKey(c) AndAlso ExpandedControls(c)
        End Function

        <DisplayName("Expand")>
        Sub SetExpand(c As Control, value As Boolean)
            ExpandedControls(c) = value
            PerformLayout()
        End Sub

        Sub Expand(c As Control)
            SetExpand(c, True)
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            MyBase.OnLayout(levent)

            If Not WrapContents AndAlso FlowDirection = FlowDirection.LeftToRight Then
                Dim nextPos As Integer

                For Each i As Control In Controls
                    If nextPos = 0 Then
                        nextPos = 3
                    End If

                    If i.Visible Then
                        nextPos += i.Margin.Left + i.Width + i.Margin.Right

                        If ExpandedControls.Keys.Contains(i) AndAlso
                            ExpandedControls(i) Then

                            Dim diff = Aggregate i2 In Controls.OfType(Of Control)() Into Sum(If(i2.Visible, i2.Width + i2.Margin.Left + i2.Margin.Right, 0))

                            If i.AutoSize Then
                                nextPos += Width - diff
                            Else
                                i.Width += Width - diff
                                nextPos += Width - diff
                            End If
                        End If
                    End If

                    Dim index = Controls.IndexOf(i)

                    If index < Controls.Count - 1 Then
                        Dim c = Controls(index + 1)
                        c.Left = nextPos
                    End If
                Next
            End If

            'vertical align middles
            If Not WrapContents AndAlso
                (FlowDirection = FlowDirection.LeftToRight OrElse
                FlowDirection = FlowDirection.RightToLeft) Then

                For Each i As Control In Controls
                    Dim offset = 0

                    If TypeOf i Is CheckBox Then
                        offset = 1
                    End If

                    i.Top = CInt((Height - i.Height) / 2) + offset
                Next
            End If

            Dim labelBlocks = From block In Controls.OfType(Of SimpleUI.LabelBlock)() Where block.Label.Offset = 0

            If labelBlocks.Count > 0 Then
                Dim hMax = Aggregate i In labelBlocks Into Max(TextRenderer.MeasureText(i.Label.Text, i.Label.Font).Width)

                For Each i In labelBlocks
                    i.Label.Offset = CInt(Math.Ceiling(hMax / i.Label.Font.Height))
                Next
            End If
        End Sub

        Protected Overrides ReadOnly Property DefaultMargin As Padding
            Get
                Return New Padding(0)
            End Get
        End Property

        Public Overrides Function GetPreferredSize(proposedSize As Size) As Size
            Dim ret = MyBase.GetPreferredSize(proposedSize)

            If UseParenWidth Then
                ret.Width = Parent.Width
            End If

            Return ret
        End Function
    End Class

    Public Class ButtonEx
        Inherits Button

        <DefaultValue(False)>
        Property ShowMenuSymbol As Boolean

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property ClickAction As Action

        Public Sub New()
            MyBase.UseVisualStyleBackColor = True
        End Sub

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property Name() As String
            Get
                Return MyBase.Name
            End Get
            Set(value As String)
                MyBase.Name = value
            End Set
        End Property

        <DefaultValue(0), Browsable(False),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property TabIndex() As Integer
            Get
                Return MyBase.TabIndex
            End Get
            Set(value As Integer)
                MyBase.TabIndex = value
            End Set
        End Property

        <DefaultValue(True), Browsable(False),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property UseVisualStyleBackColor() As Boolean
            Get
                Return True
            End Get
            Set(value As Boolean)
            End Set
        End Property

        Protected Overrides ReadOnly Property DefaultSize As Size
            Get
                Return New Size(100, 36)
            End Get
        End Property

        Protected Overrides Sub OnClick(e As EventArgs)
            If Not ClickAction Is Nothing Then
                ClickAction.Invoke()
            End If

            If Not ContextMenuStrip Is Nothing Then
                ContextMenuStrip.Show(Me, 0, Height)
            End If

            MyBase.OnClick(e)
        End Sub

        <DefaultValue(GetType(Image), Nothing)>
        Property ZoomImage As Image

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            If ShowMenuSymbol Then
                Dim _text = "6"
                Dim _font = New Font("Marlett", Font.Size)
                Dim textSize = e.Graphics.MeasureString(_text, _font)
                Dim x = If(Text <> "", Width - textSize.Width, Math.Ceiling((Width - textSize.Width) / 2))
                Dim y = Math.Ceiling((Height - textSize.Height) / 2)
                Dim brush = If(Enabled, Brushes.Black, SystemBrushes.GrayText)
                e.Graphics.DrawString(_text, _font, brush, CSng(x), CSng(y))
            End If

            If Not ZoomImage Is Nothing Then
                Dim rect As New Rectangle(Padding.Left,
                                          Padding.Top,
                                          Width - Padding.Horizontal,
                                          Height - Padding.Vertical)
                Dim targetPoint As Point
                Dim targetSize As Size
                Dim sizeToFit = ZoomImage.Size

                Dim ar1 = rect.Width / rect.Height
                Dim ar2 = sizeToFit.Width / sizeToFit.Height

                If ar2 < ar1 Then
                    targetSize.Height = rect.Height
                    targetSize.Width = CInt(sizeToFit.Width / (sizeToFit.Height / rect.Height))
                    targetPoint.X = CInt((rect.Width - targetSize.Width) / 2) + Padding.Left
                    targetPoint.Y = Padding.Top
                Else
                    targetSize.Width = rect.Width
                    targetSize.Height = CInt(sizeToFit.Height / (sizeToFit.Width / rect.Width))
                    targetPoint.Y = CInt((rect.Height - targetSize.Height) / 2) + Padding.Top
                    targetPoint.X = Padding.Left
                End If

                If Enabled Then
                    e.Graphics.DrawImage(ZoomImage, New Rectangle(targetPoint, targetSize))
                Else
                    GetType(ControlPaint).InvokeMember("DrawImageDisabled",
                                                       BindingFlags.Static Or
                                                       BindingFlags.NonPublic Or
                                                       BindingFlags.InvokeMethod,
                                                       Nothing,
                                                       Nothing,
                                                       {e.Graphics,
                                                        ZoomImage,
                                                        New Rectangle(targetPoint, targetSize),
                                                        SystemColors.Control,
                                                        False})
                End If
            End If
        End Sub

        Sub ShowBold()
            Font = New Font(Font, FontStyle.Bold)

            For i = 0 To 20
                Application.DoEvents()
                Thread.Sleep(10)
            Next

            Font = New Font(Font, FontStyle.Regular)
        End Sub
    End Class

    Public Class ListBoxEx
        Inherits ListBox

        <DefaultValue(GetType(Button), Nothing)> Property UpButton As Button
        <DefaultValue(GetType(Button), Nothing)> Property DownButton As Button
        <DefaultValue(GetType(Button), Nothing)> Property RemoveButton As Button
        <DefaultValue(GetType(Button), Nothing)> Property Button1 As Button
        <DefaultValue(GetType(Button), Nothing)> Property Button2 As Button

        Private LastTick As Long
        Private KeyText As String = ""
        Private BlockOnSelectedIndexChanged As Boolean

        Sub UpdateSelection()
            If SelectedIndex > -1 Then
                BlockOnSelectedIndexChanged = True
                Items(SelectedIndex) = SelectedItem
                BlockOnSelectedIndexChanged = False

                If Sorted Then
                    Sorted = False
                    Sorted = True
                End If
            End If
        End Sub

        Private SavedSelection As New List(Of Integer)

        Sub SaveSelection()
            SavedSelection.Clear()

            For Each i As Integer In SelectedIndices
                SavedSelection.Add(i)
            Next
        End Sub

        Sub RestoreSelection()
            For Each i In SavedSelection
                SetSelected(i, True)
            Next
        End Sub

        Sub DeleteItem(text As String)
            If FindStringExact(text) > -1 Then
                Items.RemoveAt(FindStringExact(text))
            End If
        End Sub

        Sub RemoveSelection()
            If SelectedIndex > -1 Then
                If SelectionMode = Windows.Forms.SelectionMode.One Then
                    Dim index As Integer = SelectedIndex

                    If Items.Count - 1 > SelectedIndex Then
                        SelectedIndex += 1
                    Else
                        SelectedIndex -= 1
                    End If

                    Items.RemoveAt(index)
                Else
                    Dim iFirst As Integer = SelectedIndex

                    Dim indices(SelectedIndices.Count - 1) As Integer
                    SelectedIndices.CopyTo(indices, 0)

                    SelectedIndex = -1

                    For i As Integer = indices.Length - 1 To 0 Step -1
                        Items.RemoveAt(indices(i))
                    Next

                    If iFirst > Items.Count - 1 Then
                        SelectedIndex = Items.Count - 1
                    Else
                        SelectedIndex = iFirst
                    End If
                End If
            End If
        End Sub

        Function CanMoveUp() As Boolean
            Return SelectedIndices.Count > 0 AndAlso SelectedIndices(0) > 0
        End Function

        Function CanMoveDown() As Boolean
            Return SelectedIndices.Count > 0 AndAlso SelectedIndices(SelectedIndices.Count - 1) < Items.Count - 1
        End Function

        Sub MoveSelectionUp()
            If CanMoveUp() Then
                Dim iAbove = SelectedIndices(0) - 1

                If iAbove = -1 Then
                    Exit Sub
                End If

                Dim itemAbove = Items(iAbove)
                Items.RemoveAt(iAbove)
                Dim iLastItem = SelectedIndices(SelectedIndices.Count - 1)
                Items.Insert(iLastItem + 1, itemAbove)
                SetSelected(SelectedIndex, True)
            End If
        End Sub

        Sub MoveSelectionDown()
            If CanMoveDown() Then
                Dim iBelow = SelectedIndices(SelectedIndices.Count - 1) + 1

                If iBelow >= Items.Count Then
                    Exit Sub
                End If

                Dim itemBelow = Items(iBelow)
                Items.RemoveAt(iBelow)
                Dim iAbove = SelectedIndices(0) - 1
                Items.Insert(iAbove + 1, itemBelow)
                SetSelected(SelectedIndex, True)
            End If
        End Sub

        Sub KeyDownSequence(e As KeyEventArgs)
            If Environment.TickCount - LastTick > 1000 Then
                KeyText = Convert.ToChar(e.KeyValue).ToString()
            Else
                KeyText += Convert.ToChar(e.KeyValue).ToString()
            End If

            For i As Integer = 0 To Items.Count - 1
                If Items(i).ToString().ToLower().StartsWith(KeyText.ToLower) Then
                    Application.DoEvents()

                    SelectedIndex = -1
                    SelectedIndex = i

                    Exit For
                End If
            Next

            LastTick = Environment.TickCount
        End Sub

        Sub DrawNumbered(e As DrawItemEventArgs)
            If e.Index > -1 Then
                e.DrawBackground()
                e.DrawFocusRectangle()

                Dim sb As SolidBrush = New SolidBrush(ForeColor)
                Dim pf As PointF

                If e.Index > 998 Then
                    pf = New PointF(e.Bounds.Left + 40, e.Bounds.Top)
                Else
                    If e.Index > 98 Then
                        pf = New PointF(e.Bounds.Left + 30, e.Bounds.Top)
                    Else
                        pf = New PointF(e.Bounds.Left + 20, e.Bounds.Top)
                    End If
                End If

                e.Graphics.DrawString(Items(e.Index).ToString, Font, sb, pf)
                e.Graphics.DrawString((e.Index + 1).ToString, Font, sb, e.Bounds.Left, e.Bounds.Top)
            End If
        End Sub

        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()

            If Not DesignMode Then
                If Not UpButton Is Nothing Then UpButton.AddClickAction(AddressOf MoveSelectionUp)
                If Not DownButton Is Nothing Then DownButton.AddClickAction(AddressOf MoveSelectionDown)
                If Not RemoveButton Is Nothing Then RemoveButton.AddClickAction(AddressOf RemoveSelection)
            End If
        End Sub

        Protected Overrides Sub OnSelectedIndexChanged(e As EventArgs)
            If Not BlockOnSelectedIndexChanged Then
                MyBase.OnSelectedIndexChanged(e)
                UpdateControls()
            End If
        End Sub

        Sub UpdateControls()
            If Not RemoveButton Is Nothing Then RemoveButton.Enabled = Not SelectedItem Is Nothing
            If Not UpButton Is Nothing Then UpButton.Enabled = SelectedIndex > 0
            If Not DownButton Is Nothing Then DownButton.Enabled = SelectedIndex < Items.Count - 1
            If Not Button1 Is Nothing Then Button1.Enabled = Not SelectedItem Is Nothing
            If Not Button2 Is Nothing Then Button2.Enabled = Not SelectedItem Is Nothing
        End Sub
    End Class

    Class NumEdit
        Inherits UserControl

        WithEvents TextBox As New Edit

        Private UpControl As New Button(True)
        Private DownControl As New Button(False)
        Private BorderColor As Color = Color.CadetBlue

        Event ValueChanged(numEdit As NumEdit)

        Sub New()
            SetStyle(ControlStyles.Opaque Or ControlStyles.ResizeRedraw, True)

            TextBox.BorderStyle = BorderStyle.None
            TextBox.TextAlign = HorizontalAlignment.Center
            TextBox.Text = "0"

            Controls.Add(TextBox)
            Controls.Add(UpControl)
            Controls.Add(DownControl)

            UpControl.ClickAction = Sub() Value += Increment
            DownControl.ClickAction = Sub() Value -= Increment

            AddHandler UpControl.MouseDown, Sub() Focus()
            AddHandler DownControl.MouseDown, Sub() Focus()
            AddHandler TextBox.LostFocus, Sub() UpdateText()
            AddHandler TextBox.GotFocus, Sub() SetColor(Color.CornflowerBlue)
            AddHandler TextBox.LostFocus, Sub() SetColor(Color.CadetBlue)
            AddHandler TextBox.MouseWheel, AddressOf Wheel
        End Sub

        Sub Wheel(sender As Object, e As MouseEventArgs)
            If e.Delta > 0 Then
                Value += Increment
            Else
                Value -= Increment
            End If
        End Sub

        Private Sub SetColor(c As Color)
            BorderColor = c
            Invalidate()
        End Sub

        Protected Overridable Sub OnValueChanged(numEdit As NumEdit)
            RaiseEvent ValueChanged(Me)
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            UpControl.Width = CInt(ClientSize.Height * 0.7)
            UpControl.Height = (ClientSize.Height \ 2) - 2
            UpControl.Top = 2
            UpControl.Left = ClientSize.Width - UpControl.Width - 2

            DownControl.Width = UpControl.Width
            DownControl.Left = UpControl.Left
            DownControl.Top = UpControl.Height + 3
            DownControl.Height = ClientSize.Height - UpControl.Height - 5

            TextBox.Top = (ClientSize.Height - TextBox.Height) \ 2
            TextBox.Left = 2
            TextBox.Width = DownControl.Left - 3
            TextBox.Height = TextRenderer.MeasureText("gG", TextBox.Font).Height

            MyBase.OnLayout(levent)
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim r = ClientRectangle
            r.Inflate(-1, -1)
            e.Graphics.FillRectangle(If(Enabled, Brushes.White, SystemBrushes.Control), r)

            If VisualStyleInformation.IsEnabledByUser Then
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle, BorderColor, ButtonBorderStyle.Solid)
            Else
                ControlPaint.DrawBorder3D(e.Graphics, ClientRectangle, Border3DStyle.Sunken)
            End If

            MyBase.OnPaint(e)
        End Sub

        Protected Overrides ReadOnly Property DefaultSize() As Size
            Get
                Return New Size(100, 36)
            End Get
        End Property

        <Category("Data")>
        <DefaultValue(GetType(Decimal), "-2147483648")>
        Public Property Minimum As Decimal = -2147483648

        <Category("Data")>
        <DefaultValue(GetType(Decimal), "2147483647")>
        Public Property Maximum As Decimal = 2147483647

        <Category("Data")>
        <DefaultValue(GetType(Decimal), "1")>
        Property Increment As Decimal = 1

        Private ValueValue As Decimal

        <Category("Data")>
        <DefaultValue(GetType(Decimal), "0")>
        Property Value As Decimal
            Get
                Return ValueValue
            End Get
            Set(value As Decimal)
                SetValue(value, True)
            End Set
        End Property

        Private DecimalPlacesValue As Integer

        <Category("Data")>
        <DefaultValue(0)>
        Property DecimalPlaces As Integer
            Get
                Return DecimalPlacesValue
            End Get
            Set(value As Integer)
                DecimalPlacesValue = value
                UpdateText()
            End Set
        End Property

        Private Sub SetValue(value As Decimal, updateText As Boolean)
            If value <> ValueValue Then
                If value > Maximum Then
                    value = Maximum
                ElseIf value < Minimum Then
                    value = Minimum
                End If

                ValueValue = value

                If updateText Then
                    Me.UpdateText()
                End If

                OnValueChanged(Me)
            End If
        End Sub

        Sub UpdateText()
            TextBox.SetTextWithoutTextChanged(ValueValue.ToString("F" & DecimalPlaces))
        End Sub

        Private Sub TextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles TextBox.KeyDown
            If e.KeyData = Keys.Up Then
                Value += Increment
            ElseIf e.KeyData = Keys.Down Then
                Value -= Increment
            End If
        End Sub

        Private Sub TextBox_TextChanged(sender As Object, e As EventArgs) Handles TextBox.TextChanged
            Dim ret As Decimal

            If Decimal.TryParse(TextBox.Text, ret) Then
                SetValue(ret, False)
            End If
        End Sub

        Private Class Edit
            Inherits TextBox

            Private BlockTextChanged As Boolean

            Sub SetTextWithoutTextChanged(val As String)
                BlockTextChanged = True
                Text = val
                BlockTextChanged = False
            End Sub

            Protected Overrides Sub OnTextChanged(e As EventArgs)
                If Not BlockTextChanged Then
                    MyBase.OnTextChanged(e)
                End If
            End Sub
        End Class

        Private Class Button
            Inherits Control

            Private IsUp As Boolean
            Private IsHot As Boolean
            Private IsPressed As Boolean
            Private Renderer As VisualStyleRenderer
            Private LastMouseDownTick As Integer

            Property ClickAction As Action

            Sub New(isUp As Boolean)
                Me.IsUp = isUp
                TabStop = False
                SetStyle(ControlStyles.Opaque Or ControlStyles.ResizeRedraw, True)
            End Sub

            Protected Overrides Sub OnMouseEnter(e As EventArgs)
                IsHot = True
                Invalidate()
                MyBase.OnMouseEnter(e)
            End Sub

            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                IsHot = False
                Invalidate()
                MyBase.OnMouseLeave(e)
            End Sub

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                IsPressed = True
                Invalidate()
                ClickAction.Invoke()
                LastMouseDownTick = Environment.TickCount
                MouseDownClicks(1000, LastMouseDownTick)
                MyBase.OnMouseDown(e)
            End Sub

            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                IsPressed = False
                Invalidate()
                MyBase.OnMouseUp(e)
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                If VisualStyleInformation.IsEnabledByUser Then
                    Dim element As VisualStyleElement
                    Dim disabled, hot, normal, pressed As VisualStyleElement

                    If IsUp Then
                        disabled = VisualStyleElement.Spin.Up.Disabled
                        hot = VisualStyleElement.Spin.Up.Hot
                        normal = VisualStyleElement.Spin.Up.Normal
                        pressed = VisualStyleElement.Spin.Up.Pressed
                    Else
                        disabled = VisualStyleElement.Spin.Down.Disabled
                        hot = VisualStyleElement.Spin.Down.Hot
                        normal = VisualStyleElement.Spin.Down.Normal
                        pressed = VisualStyleElement.Spin.Down.Pressed
                    End If

                    If Enabled Then
                        If IsPressed Then
                            element = pressed
                        ElseIf IsHot Then
                            element = hot
                        Else
                            element = normal
                        End If
                    Else
                        element = disabled
                    End If

                    If Renderer Is Nothing Then
                        Renderer = New VisualStyleRenderer(element)
                    Else
                        Renderer.SetParameters(element)
                    End If

                    Renderer.DrawBackground(e.Graphics, ClientRectangle)
                Else
                    ControlPaint.DrawButton(e.Graphics,
                                            ClientRectangle,
                                            If(IsPressed, ButtonState.Pushed, ButtonState.Normal))

                    Dim text = If(IsUp, "5", "6")
                    Dim font = New Font("Marlett", Me.Font.Size - 1)
                    Dim textSize = e.Graphics.MeasureString(text, font)
                    Dim x = Math.Ceiling((Width - textSize.Width) / 2)
                    Dim y = Math.Ceiling((Height - textSize.Height) / 2)
                    Dim brush = If(Enabled, Brushes.Black, SystemBrushes.GrayText)
                    e.Graphics.DrawString(text, font, brush, CInt(x), CInt(y))
                End If

                MyBase.OnPaint(e)
            End Sub

            Async Sub MouseDownClicks(sleep As Integer, tick As Integer)
                Await Task.Run(Sub() Thread.Sleep(sleep))

                If IsPressed AndAlso LastMouseDownTick = tick Then
                    ClickAction.Invoke()
                    MouseDownClicks(20, tick)
                End If
            End Sub
        End Class
    End Class

    Public Class TextEdit
        Inherits UserControl

        Public WithEvents TextBox As New TextBoxEx
        Public Shadows Event TextChanged()
        Private BorderColor As Color = Color.CadetBlue

        Sub New()
            SetStyle(ControlStyles.Opaque Or ControlStyles.ResizeRedraw, True)
            TextBox.BorderStyle = BorderStyle.None
            Controls.Add(TextBox)
            AddHandler TextBox.GotFocus, Sub() SetColor(Color.CornflowerBlue)
            AddHandler TextBox.LostFocus, Sub() SetColor(Color.CadetBlue)
            AddHandler TextBox.TextChanged, Sub() RaiseEvent TextChanged()
        End Sub

        Private Sub SetColor(c As Color)
            BorderColor = c
            Invalidate()
        End Sub

        Private TextValue As String

        <BrowsableAttribute(True)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
        Overrides Property Text As String
            Get
                Return TextBox.Text
            End Get
            Set(value As String)
                TextBox.Text = value
            End Set
        End Property

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            If TextBox.Multiline Then
                TextBox.Top = 2
                TextBox.Left = 2
                TextBox.Width = ClientSize.Width - 4
                TextBox.Height = ClientSize.Height - 4
            Else
                TextBox.Top = (ClientSize.Height - TextBox.Height) \ 2
                TextBox.Left = 2
                TextBox.Width = ClientSize.Width - 4
                TextBox.Height = TextRenderer.MeasureText("gG", TextBox.Font).Height
            End If

            MyBase.OnLayout(levent)
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim r = ClientRectangle
            r.Inflate(-1, -1)
            e.Graphics.FillRectangle(If(Enabled, Brushes.White, SystemBrushes.Control), r)
            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, BorderColor, ButtonBorderStyle.Solid)
            MyBase.OnPaint(e)
        End Sub
    End Class

    Interface IPage
        Property Node As TreeNode
        Property Path As String
        Property TipProvider As TipProvider
    End Interface

    Class DataGridViewEx
        Inherits DataGridView

        Function AddTextColumn() As DataGridViewTextBoxColumn
            Dim ret As New DataGridViewTextBoxColumn
            Columns.Add(ret)
            Return ret
        End Function

        Function AddComboBoxColumn() As DataGridViewComboBoxColumn
            Dim ret As New DataGridViewComboBoxColumn
            Columns.Add(ret)
            Return ret
        End Function
    End Class

    Public Class TabControlEx
        Inherits TabControl

        Private DragStartPosition As Point = Point.Empty
        Private TabType As Type

        Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
            DragStartPosition = New Point(e.X, e.Y)
            MyBase.OnMouseDown(e)
        End Sub

        Protected Overrides Sub OnMouseMove(e As MouseEventArgs)
            If e.Button <> MouseButtons.Left Then Return

            Dim rect = New Rectangle(DragStartPosition, Size.Empty)
            rect.Inflate(SystemInformation.DragSize)

            Dim page = HoverTab()

            If Not page Is Nothing AndAlso Not rect.Contains(e.X, e.Y) Then
                TabType = page.GetType
                DoDragDrop(page, DragDropEffects.All)
            End If

            DragStartPosition = Point.Empty
            MyBase.OnMouseMove(e)
        End Sub

        Protected Overrides Sub OnDragOver(e As DragEventArgs)
            Dim hoverTab = Me.HoverTab()

            If hoverTab Is Nothing Then
                e.Effect = DragDropEffects.None
            Else
                If e.Data.GetDataPresent(TabType) Then
                    e.Effect = DragDropEffects.Move
                    Dim dragTab = DirectCast(e.Data.GetData(TabType), TabPage)

                    If hoverTab Is dragTab Then Return

                    Dim tabRect = GetTabRect(TabPages.IndexOf(hoverTab))
                    tabRect.Inflate(-3, -3)

                    If tabRect.Contains(PointToClient(New Point(e.X, e.Y))) Then
                        SwapTabPages(dragTab, hoverTab)
                        SelectedTab = dragTab
                    End If
                End If
            End If

            MyBase.OnDragOver(e)
        End Sub

        Private Function HoverTab() As TabPage
            For index = 0 To TabCount - 1
                If GetTabRect(index).Contains(PointToClient(Cursor.Position)) Then
                    Return TabPages(index)
                End If
            Next
        End Function

        Private Sub SwapTabPages(ByVal tp1 As TabPage, ByVal tp2 As TabPage)
            Dim index1 = TabPages.IndexOf(tp1)
            Dim index2 = TabPages.IndexOf(tp2)
            TabPages(index1) = tp2
            TabPages(index2) = tp1
        End Sub
    End Class
End Namespace