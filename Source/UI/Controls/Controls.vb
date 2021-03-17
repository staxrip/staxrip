
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.VisualStyles
Imports System.Threading
Imports System.Threading.Tasks
Imports System.Drawing.Drawing2D
Imports System.Text.RegularExpressions

Namespace UI
    Public Class TreeViewEx
        Inherits TreeView

        Private _backAlternateColor As Color
        Private _backExpandedColor As Color
        Private _backHighlightColor As Color
        Private _backSelectedColor As Color
        Private _foreExpandedColor As Color
        Private _foreHighlightColor As Color
        Private _foreSelectedColor As Color

        <DefaultValue(False)>
        Property AutoCollaps As Boolean

        <DefaultValue(GetType(TreeNodeExpandMode), "Disabled")>
        Property ExpandMode As TreeNodeExpandMode

        <DefaultValue(False)>
        Property SelectOnMouseDown() As Boolean

        Public Property BackAlternateColor As Color
            Get
                Return _backAlternateColor
            End Get
            Set(value As Color)
                _backAlternateColor = value
            End Set
        End Property

        Private Function ShouldSerializeBackAlternateColor() As Boolean
            Return BackAlternateColor <> Color.Empty
        End Function

        Public Property BackExpandedColor As Color
            Get
                Return _backExpandedColor
            End Get
            Set(value As Color)
                _backExpandedColor = value
            End Set
        End Property

        Private Function ShouldSerializeBackExpandedColor() As Boolean
            Return BackExpandedColor <> Color.Empty
        End Function

        Public Property BackHighlightColor As Color
            Get
                Return _backHighlightColor
            End Get
            Set(value As Color)
                _backHighlightColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeBackHighlightColor() As Boolean
            Return BackHighlightColor <> Color.Empty
        End Function

        Public Property BackSelectedColor As Color
            Get
                Return _backSelectedColor
            End Get
            Set(value As Color)
                _backSelectedColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeBackSelectedColor() As Boolean
            Return BackSelectedColor <> Color.Empty
        End Function

        Public Property ForeExpandedColor As Color
            Get
                Return _foreExpandedColor
            End Get
            Set(value As Color)
                _foreExpandedColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeForeExpandedColor() As Boolean
            Return ForeExpandedColor <> Color.Empty
        End Function

        Public Property ForeHighlightColor As Color
            Get
                Return _foreHighlightColor
            End Get
            Set(value As Color)
                _foreHighlightColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeForeHighlightColor() As Boolean
            Return ForeHighlightColor <> Color.Empty
        End Function

        Public Property ForeSelectedColor As Color
            Get
                Return _foreSelectedColor
            End Get
            Set(value As Color)
                _foreSelectedColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeForeSelectedColor() As Boolean
            Return ForeSelectedColor <> Color.Empty
        End Function

        Sub New()
            MyBase.New()
            DrawMode = TreeViewDrawMode.OwnerDrawAll
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

            SuspendLayout()
            BackColor = theme.General.Controls.TreeView.BackColor
            BackAlternateColor = theme.General.Controls.TreeView.BackAlternateColor
            BackExpandedColor = theme.General.Controls.TreeView.BackExpandedColor
            BackHighlightColor = theme.General.Controls.TreeView.BackHighlightColor
            BackSelectedColor = theme.General.Controls.TreeView.BackSelectedColor
            ForeColor = theme.General.Controls.TreeView.ForeColor
            ForeExpandedColor = theme.General.Controls.TreeView.ForeExpandedColor
            ForeHighlightColor = theme.General.Controls.TreeView.ForeHighlightColor
            ForeSelectedColor = theme.General.Controls.TreeView.ForeSelectedColor
            LineColor = theme.General.Controls.TreeView.LineColor
            ResumeLayout()
        End Sub

        Protected Overrides Sub OnDrawNode(e As DrawTreeNodeEventArgs)
            'MyBase.OnDrawNode(e)

            e.DrawDefault = False
            Dim font = If(e.Node.NodeFont, e.Node.TreeView.Font)
            Dim state = e.State
            Dim text = New String(" "c, e.Node.Level * 4) + e.Node.Text
            Dim bounds = e.Bounds
            Dim textFlags = TextFormatFlags.GlyphOverhangPadding Or TextFormatFlags.VerticalCenter Or TextFormatFlags.SingleLine

            If e.Node.IsSelected Then
                e.Graphics.FillRectangle(New SolidBrush(BackSelectedColor), bounds)
                'ControlPaint.DrawFocusRectangle(e.Graphics, bounds, ForeSelectedColor, BackSelectedColor)
                TextRenderer.DrawText(e.Graphics, text, font, bounds, ForeSelectedColor, BackSelectedColor, textFlags)
            ElseIf e.Node.IsExpanded Then
                e.Graphics.FillRectangle(New SolidBrush(BackExpandedColor), bounds)
                TextRenderer.DrawText(e.Graphics, text, font, bounds, ForeExpandedColor, BackExpandedColor, textFlags)
            Else
                e.Graphics.FillRectangle(New SolidBrush(BackColor), bounds)
                TextRenderer.DrawText(e.Graphics, text, font, bounds, ForeColor, BackColor, textFlags)
            End If
            MyBase.OnDrawNode(e)
        End Sub

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
            If SelectOnMouseDown AndAlso m.Msg = &H201 Then 'WM_LBUTTONDOWN
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

        Function GetParentParentNodes(n As TreeNode) As TreeNodeCollection
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

        Function GetParentNodes(n As TreeNode) As TreeNodeCollection
            Dim parent As TreeNode = n.Parent

            If parent Is Nothing Then
                Return Nodes
            Else
                Return parent.Nodes
            End If
        End Function

        Protected Overrides Sub OnHandleCreated(e As EventArgs)
            MyBase.OnHandleCreated(e)
            'Native.SetWindowTheme(Handle, "explorer", Nothing)
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
                    If iNode.Text = " " + iNodeName Then
                        ret = iNode
                        currentNodeList = iNode.Nodes
                        found = True
                    End If
                Next

                If Not found Then
                    ret = New TreeNode
                    ret.Text = " " + iNodeName
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
                    ret.ExStyle = ret.ExStyle Or &H200 'WS_EX_CLIENTEDGE
                End If

                Return ret
            End Get
        End Property
    End Class


    Public Class ToolStripButtonEx
        Inherits ToolStripButton

        Sub New()
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

            BackColor = theme.General.Controls.ToolStripButton.BackColor
            ForeColor = theme.General.Controls.ToolStripButton.ForeColor
        End Sub
    End Class


    Public Class LineControl
        Inherits Control

        Sub New()
            Margin = New Padding(4, 2, 5, 2)
            Anchor = AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right
            SetStyle(ControlStyles.SupportsTransparentBackColor, True)
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            e.Graphics.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

            Dim textOffset As Integer
            Dim lineHeight = CInt(Height / 2)

            If Text <> "" Then
                Dim textSize = e.Graphics.MeasureString(Text, Font)
                textOffset = CInt(textSize.Width)

                Using brush = New SolidBrush(If(Enabled, ForeColor, SystemColors.GrayText))
                    e.Graphics.DrawString(Text, Font, brush, 0, CInt((Height - textSize.Height) / 2) - 1)
                End Using
            End If

            If Enabled Then
                e.Graphics.DrawLine(Pens.Silver, textOffset, lineHeight, Width, lineHeight)
                e.Graphics.DrawLine(Pens.White, textOffset, lineHeight + 1, Width, lineHeight + 1)
            Else
                e.Graphics.DrawLine(SystemPens.InactiveBorder, textOffset, lineHeight, Width, lineHeight)
            End If
        End Sub
    End Class

    Public Class CommandLink
        Inherits Button

        Const BS_COMMANDLINK As Integer = &HE

        Const BCM_SETNOTE As Integer = &H1609

        Sub New()
            FlatStyle = FlatStyle.System
        End Sub

        Protected Overloads Overrides ReadOnly Property CreateParams() As CreateParams
            Get
                Dim params = MyBase.CreateParams
                params.Style = params.Style Or BS_COMMANDLINK
                Return params
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
            MyBase.OnCreateControl()

            If Note <> "" Then
                Text += BR2 + Note
            End If
        End Sub
    End Class

    Public Class TextBoxEx
        Inherits TextBox

        'Private Const WM_NCPAINT As UInteger = &H85
        'Private Const RDW_INVALIDATE As UInteger = &H1
        'Private Const RDW_IUPDATENOW As UInteger = &H100
        'Private Const RDW_FRAME = &H400

        Private _blockOnTextChanged As Boolean = False
        Private _borderColor As Color
        Private _borderFocusedColor As Color

        Public Property BorderColor As Color
            Get
                Return _borderColor
            End Get
            Set(value As Color)
                _borderColor = value
                'Native.RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, RDW_FRAME Or RDW_IUPDATENOW Or RDW_INVALIDATE)
            End Set
        End Property

        Private Function ShouldSerializeBorderColor() As Boolean
            Return BorderColor <> Color.Empty
        End Function

        Public Property BorderFocusedColor As Color
            Get
                Return _borderFocusedColor
            End Get
            Set(value As Color)
                _borderFocusedColor = value
                'Native.RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, RDW_FRAME Or RDW_IUPDATENOW Or RDW_INVALIDATE)
            End Set
        End Property

        Private Function ShouldSerializeBorderFocusedColor() As Boolean
            Return BorderFocusedColor <> Color.Empty
        End Function

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
        Shadows Property TabIndex As Integer
            Get
                Return MyBase.TabIndex
            End Get
            Set(value As Integer)
                MyBase.TabIndex = value
            End Set
        End Property

        Sub New()
            ApplyTheme()
            BorderStyle = BorderStyle.None

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

            SuspendLayout()
            BackColor = theme.General.Controls.TextBox.BackColor
            ForeColor = theme.General.Controls.TextBox.ForeColor
            BorderColor = theme.General.Controls.TextBox.BorderColor
            BorderFocusedColor = theme.General.Controls.TextBox.BorderFocusedColor
            ResumeLayout()
        End Sub

        Public Sub SetTextWithoutTextChangedEvent(text As String)
            _blockOnTextChanged = True
            Me.Text = text
            _blockOnTextChanged = False
        End Sub

        Protected Overrides Sub OnTextChanged(e As EventArgs)
            If Not _blockOnTextChanged Then
                MyBase.OnTextChanged(e)
            End If
        End Sub

        'Protected Overrides Sub WndProc(ByRef m As Message)
        '    MyBase.WndProc(m)

        '    If BorderColor <> Color.Empty AndAlso BorderColor <> Color.Transparent AndAlso BorderStyle = BorderStyle.Fixed3D Then
        '        If m.Msg = WM_NCPAINT Then
        '            Dim hDC = Native.GetWindowDC(Handle)
        '            If Me.Focused Then
        '                Using g = Graphics.FromHdc(hDC)
        '                    Using p = New Pen(BorderFocusedColor, 2)
        '                        Dim rect = New Rectangle(1, 1, Me.Width - 2, Me.Height - 2)
        '                        g.DrawRectangle(p, rect)
        '                    End Using
        '                End Using
        '            Else
        '                Using g = Graphics.FromHdc(hDC)
        '                    Using p = New Pen(BorderColor, 2)
        '                        Dim rect = New Rectangle(1, 1, Me.Width - 2, Me.Height - 2)
        '                        g.DrawRectangle(p, rect)
        '                    End Using
        '                End Using
        '            End If
        '            Native.ReleaseDC(Handle, hDC)
        '        End If
        '    End If
        'End Sub

        'Protected Overrides Sub OnSizeChanged(e As EventArgs)
        '    MyBase.OnSizeChanged(e)
        '    Native.RedrawWindow(Handle, IntPtr.Zero, IntPtr.Zero, RDW_FRAME Or RDW_IUPDATENOW Or RDW_INVALIDATE)
        'End Sub
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

        Private _theme As Theme

        Public ReadOnly Property Theme As Theme
            Get
                Return _theme
            End Get
        End Property

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
        Shadows Property TabIndex As Integer
            Get
                Return MyBase.TabIndex
            End Get
            Set(value As Integer)
                MyBase.TabIndex = value
            End Set
        End Property

        Sub New()
            SetStyle(ControlStyles.ResizeRedraw, True)
            SetStyle(ControlStyles.UserPaint, True)
            UseVisualStyleBackColor = False
            FlatStyle = FlatStyle.Standard
            FlatAppearance.BorderSize = 2

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
            _theme = theme

            If DesignHelp.IsDesignMode Then
                Exit Sub
            End If

            SuspendLayout()
            If Appearance.HasFlag(Appearance.Normal) Then
                FlatStyle = FlatStyle.Standard
                FlatAppearance.BorderSize = 0

                BackColor = theme.General.Controls.CheckBox.BackColor
                ForeColor = theme.General.Controls.CheckBox.ForeColor
                FlatAppearance.BorderColor = theme.General.Controls.CheckBox.BorderColor
                FlatAppearance.CheckedBackColor = theme.General.Controls.CheckBox.BackColor
                FlatAppearance.MouseOverBackColor = theme.General.Controls.CheckBox.BackHighlightColor
            Else
                FlatStyle = FlatStyle.Flat
                FlatAppearance.BorderSize = 0

                BackColor = theme.General.Controls.CheckBox.BackColor
                ForeColor = theme.General.Controls.Button.ForeColor
                FlatAppearance.BorderColor = theme.General.Controls.Button.BorderColor
                FlatAppearance.CheckedBackColor = theme.General.Controls.Button.BackColor
                FlatAppearance.MouseOverBackColor = theme.General.Controls.CheckBox.BackHoverColor
            End If
            ResumeLayout()
        End Sub

        Protected Overrides Sub OnPaint(pevent As PaintEventArgs)
            InvokePaintBackground(Me, pevent)

            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias
            Dim state As CheckBoxState = If(CheckState = CheckState.Checked, CheckBoxState.CheckedNormal, CheckBoxState.UncheckedNormal)
            Dim glyphSize As Size = CheckBoxRendererEx.GetGlyphSize(pevent.Graphics, state)
            Dim vPos As Integer = (Height - glyphSize.Height) \ 2
            Dim hPos As Integer = 1
            Dim glyphLocation As Point = New Point(hPos, vPos)
            Dim textLocation As Point = New Point(hPos + glyphSize.Width + hPos, 1 + Height - (Height - CInt(pevent.Graphics.MeasureString(Text, Font).Height)) \ 3)
            Dim textFlags As TextFormatFlags = TextFormatFlags.SingleLine Or TextFormatFlags.VerticalCenter
            Dim fColor As ColorHSL = If(Checked, _theme.General.Controls.CheckBox.ForeCheckedColor, _theme.General.Controls.CheckBox.ForeColor)

            CheckBoxRendererEx.DrawCheckBox(pevent.Graphics, glyphLocation, state)
            TextRenderer.DrawText(pevent.Graphics, Text, Font, textLocation, fColor, textFlags)
        End Sub

        Protected Overrides Sub OnPaintBackground(pevent As PaintEventArgs)
            MyBase.OnPaintBackground(pevent)
        End Sub

        Protected Overrides Sub OnCheckedChanged(e As EventArgs)
            MyBase.OnCheckedChanged(e)
        End Sub
    End Class

    Public Class ComboBoxEx
        Inherits ComboBox

        Sub New()
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

            SuspendLayout()
            BackColor = theme.General.Controls.ComboBox.BackColor
            FlatStyle = theme.General.Controls.ComboBox.FlatStyle
            ForeColor = theme.General.Controls.ComboBox.ForeColor
            ResumeLayout()
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
        Shadows Property TabIndex As Integer
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

        Private BorderRect As Native.RECT

        Private _backReadonlyColor As Color
        Private _borderColor As Color
        Private _borderFocusedColor As Color
        Private _borderHoverColor As Color

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property BlockPaint As Boolean

        Public Property BackReadonlyColor As Color
            Get
                Return _backReadonlyColor
            End Get
            Set(value As Color)
                _backReadonlyColor = value
            End Set
        End Property

        Private Function ShouldSerializeBackReadonlyColor() As Boolean
            Return BackReadonlyColor <> Color.Empty
        End Function

        Public Property BorderColor As Color
            Get
                Return _borderColor
            End Get
            Set(value As Color)
                _borderColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeBorderColor() As Boolean
            Return BorderColor <> Color.Empty
        End Function

        Public Property BorderFocusedColor As Color
            Get
                Return _borderFocusedColor
            End Get
            Set(value As Color)
                _borderFocusedColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeBorderFocusedColor() As Boolean
            Return BorderFocusedColor <> Color.Empty
        End Function

        Public Property BorderHoverColor As Color
            Get
                Return _borderHoverColor
            End Get
            Set(value As Color)
                _borderHoverColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeBorderHoverColor() As Boolean
            Return BorderHoverColor <> Color.Empty
        End Function

        Public Event AfterThemeApplied(theme As Theme)

        Sub New()
            MyClass.New(True)
        End Sub

        Sub New(createMenu As Boolean)
            If createMenu Then
                InitMenu()
            End If

            If VisualStyleInformation.IsEnabledByUser Then
                BorderStyle = BorderStyle.None
            End If

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

            SuspendLayout()
            BackColor = theme.General.Controls.RichTextBox.BackColor
            BackReadonlyColor = theme.General.Controls.RichTextBox.BackReadonlyColor
            ForeColor = theme.General.Controls.RichTextBox.ForeColor
            BorderColor = theme.General.Controls.RichTextBox.BorderColor
            BorderFocusedColor = theme.General.Controls.RichTextBox.BorderFocusedColor
            ResumeLayout()
            RaiseEvent AfterThemeApplied(theme)
        End Sub

        Sub InitMenu()
            If DesignHelp.IsDesignMode Then
                Exit Sub
            End If

            Dim cms As New ContextMenuStripEx()

            Dim cutItem = cms.Add("Cut")
            cutItem.SetImage(Symbol.Cut)
            cutItem.ShortcutKeyDisplayString = "Ctrl+X"

            Dim copyItem = cms.Add("Copy", Sub() Clipboard.SetText(SelectedText))
            copyItem.SetImage(Symbol.Copy)
            copyItem.ShortcutKeyDisplayString = "Ctrl+C"

            Dim pasteItem = cms.Add("Paste")
            pasteItem.SetImage(Symbol.Paste)
            pasteItem.ShortcutKeyDisplayString = "Ctrl+V"

            cms.Add("Copy Everything", Sub() Clipboard.SetText(Text))

            AddHandler cutItem.Click, Sub()
                                          Clipboard.SetText(SelectedText)
                                          SelectedText = ""
                                      End Sub

            AddHandler pasteItem.Click, Sub()
                                            SelectedText = Clipboard.GetText
                                            ScrollToCaret()
                                        End Sub

            AddHandler cms.Opening, Sub()
                                        cutItem.Visible = SelectionLength > 0 AndAlso Not Me.ReadOnly
                                        copyItem.Visible = SelectionLength > 0
                                        pasteItem.Visible = Clipboard.GetText <> "" AndAlso Not Me.ReadOnly
                                    End Sub

            ContextMenuStrip = cms
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            RemoveHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
            MyBase.Dispose(disposing)
            ContextMenuStrip?.Dispose()
        End Sub

        Protected Overrides Sub OnKeyDown(e As KeyEventArgs)
            If e.KeyData = (Keys.Control Or Keys.V) Then
                e.SuppressKeyPress = True
                SelectedText = Clipboard.GetText
            End If

            MyBase.OnKeyDown(e)
        End Sub

        Protected Overrides Sub WndProc(ByRef m As Message)
            Const WM_NCPAINT = &H85
            Const WM_NCCALCSIZE = &H83
            Const WM_THEMECHANGED = &H31A

            Select Case m.Msg
                Case 15, 20 'WM_PAINT, WM_ERASEBKGND
                    If BlockPaint Then
                        Exit Sub
                    End If
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

        Sub WmNccalcsize(ByRef m As Message)
            If Not VisualStyleInformation.IsEnabledByUser Then
                Return
            End If

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

        Sub WmNcpaint(ByRef m As Message)
            If Not VisualStyleInformation.IsEnabledByUser Then
                Return
            End If

            Dim rect As Native.RECT
            Native.GetWindowRect(Handle, rect)

            rect.Right -= rect.Left
            rect.Bottom -= rect.Top
            rect.Top = 0
            rect.Left = 0

            rect.Left += BorderRect.Left
            rect.Top += BorderRect.Top
            rect.Right -= BorderRect.Right
            rect.Bottom -= BorderRect.Bottom

            Dim hDC = Native.GetWindowDC(Handle)
            Native.ExcludeClipRect(hDC, rect.Left, rect.Top, rect.Right, rect.Bottom)

            Using g = Graphics.FromHdc(hDC)
                g.Clear(BorderColor)
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

        <DefaultValue(False)>
        Property NoMouseWheelEvent As Boolean

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

            SuspendLayout()
            BackColor = theme.General.Controls.Label.BackColor
            ForeColor = theme.General.Controls.Label.ForeColor
            ResumeLayout()
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
        Shadows Property TabIndex As Integer
            Get
                Return MyBase.TabIndex
            End Get
            Set(value As Integer)
                MyBase.TabIndex = value
            End Set
        End Property
    End Class

    Public Class PropertyGridEx
        Inherits PropertyGrid

        Private Description As String

        Sub New()
            ToolbarVisible = False 'GridView is not ready when PropertySortChanged happens so Init fails

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

            SuspendLayout()
            BackColor = theme.General.Controls.PropertyGrid.BackColor
            CategoryForeColor = theme.General.Controls.PropertyGrid.CategoryForeColor
            CategorySplitterColor = theme.General.Controls.PropertyGrid.CategorySplitterColor
            CommandsActiveLinkColor = theme.General.Controls.PropertyGrid.CommandsActiveLinkColor
            CommandsBackColor = theme.General.Controls.PropertyGrid.CommandsBackColor
            CommandsBorderColor = theme.General.Controls.PropertyGrid.CommandsBorderColor
            CommandsDisabledLinkColor = theme.General.Controls.PropertyGrid.CommandsDisabledLinkColor
            CommandsForeColor = theme.General.Controls.PropertyGrid.CommandsForeColor
            CommandsLinkColor = theme.General.Controls.PropertyGrid.CommandsLinkColor
            DisabledItemForeColor = theme.General.Controls.PropertyGrid.DisabledItemForeColor
            ForeColor = theme.General.Controls.PropertyGrid.ForeColor
            HelpBackColor = theme.General.Controls.PropertyGrid.HelpBackColor
            HelpBorderColor = theme.General.Controls.PropertyGrid.HelpBorderColor
            HelpForeColor = theme.General.Controls.PropertyGrid.HelpForeColor
            LineColor = theme.General.Controls.PropertyGrid.LineColor
            SelectedItemWithFocusBackColor = theme.General.Controls.PropertyGrid.SelectedItemWithFocusBackColor
            SelectedItemWithFocusForeColor = theme.General.Controls.PropertyGrid.SelectedItemWithFocusForeColor
            ViewBackColor = theme.General.Controls.PropertyGrid.ViewBackColor
            ViewBorderColor = theme.General.Controls.PropertyGrid.ViewBorderColor
            ViewForeColor = theme.General.Controls.PropertyGrid.ViewForeColor

            ResumeLayout()
        End Sub

        Protected Overrides Sub OnSelectedGridItemChanged(e As SelectedGridItemChangedEventArgs)
            MyBase.OnSelectedGridItemChanged(e)
            Description = e.NewSelection.PropertyDescriptor.Description
            SetHelpHeight()
        End Sub

        Protected Overrides Sub OnLayout(e As LayoutEventArgs)
            SetHelpHeight()
            MyBase.OnLayout(e)
        End Sub

        Sub SetHelpHeight()
            If Description <> "" Then
                HelpVisible = True

                Dim lines = CInt(Math.Ceiling(CreateGraphics.MeasureString(
                    Description, Font, Width).Height / Font.Height))

                Dim grid As New Reflector(Me, GetType(PropertyGrid))
                Dim doc = grid.Invoke("doccomment")
                doc.Invoke("Lines", lines + 1)
                doc.Invoke("userSized", True)
                grid.Invoke("OnLayoutInternal", False)
            Else
                HelpVisible = False
            End If
        End Sub

        Sub MoveSplitter(x As Integer)
            Dim grid As New Reflector(Me, GetType(PropertyGrid))
            grid.Invoke("gridView").Invoke("MoveSplitterTo", x)
        End Sub
    End Class

    Public Class ButtonLabel
        Inherits Label

        Property LinkColor As Color
        Property LinkHoverColor As Color

        Sub New()
            ApplyTheme()

            AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
        End Sub

        Private Function ShouldSerializeLinkColor() As Boolean
            Return LinkColor <> Color.Empty
        End Function

        Private Function ShouldSerializeLinkHoverColor() As Boolean
            Return LinkHoverColor <> Color.Empty
        End Function

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

            SuspendLayout()
            BackColor = theme.General.Controls.ButtonLabel.BackColor
            ForeColor = theme.General.Controls.ButtonLabel.ForeColor
            LinkColor = theme.General.Controls.ButtonLabel.LinkForeColor
            LinkHoverColor = theme.General.Controls.ButtonLabel.LinkForeHoverColor
            ResumeLayout()
        End Sub

        Protected Overrides Sub OnMouseEnter(e As EventArgs)
            ForeColor = LinkHoverColor
            MyBase.OnMouseEnter(e)
        End Sub

        Protected Overrides Sub OnMouseLeave(e As EventArgs)
            ForeColor = LinkColor
            MyBase.OnMouseLeave(e)
        End Sub
    End Class

    <DefaultEvent("LinkClick")>
    Public Class LinkGroupBox
        Inherits GroupBoxEx

        Public WithEvents Label As New ButtonLabel
        Event LinkClick()

        Sub New()
            Label.Left = 14
            Label.AutoSize = True
            Controls.Add(Label)
        End Sub

        Property Color As Color

        Private Function ShouldSerializeColor() As Boolean
            Return Color <> Color.Empty
        End Function

        Overrides Property Text() As String
            Get
                Return Label.Text
            End Get
            Set(value As String)
                Label.Text = value.TrimEx
            End Set
        End Property

        Sub Label_Click() Handles Label.Click
            ShowContext()
            RaiseEvent LinkClick()
        End Sub

        Sub ShowContext()
            If Not Label.ContextMenuStrip Is Nothing Then
                Label.ContextMenuStrip.Show(Label, 0, 16)
            End If
        End Sub
    End Class

    Public Class GroupBoxEx
        Inherits GroupBox

        Sub New()
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

            SuspendLayout()
            BackColor = theme.General.Controls.GroupBox.BackColor
            ForeColor = theme.General.Controls.GroupBox.ForeColor
            ResumeLayout()
        End Sub
    End Class

    Public Class MenuButton
        Inherits ButtonEx

        Event ValueChangedUser(value As Object)

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property Items As New List(Of Object)
        Property Menu As New ContextMenuStripEx
        Property ShowPath As Boolean

        Sub New()
            Menu.ShowImageMargin = False
            ShowMenuSymbol = True
            Padding = New Padding(4, 0, 0, 0)
            AddHandler Menu.Opening, AddressOf MenuOpening
        End Sub

        Sub MenuOpening(sender As Object, e As CancelEventArgs)
            Menu.MinimumSize = New Size(Width, 0)

            For Each mi As MenuItemEx In Menu.Items
                mi.Font = New Font("Segoe UI", 9 * s.UIScaleFactor, If(Not Value Is Nothing AndAlso Value.Equals(mi.Tag), FontStyle.Bold, FontStyle.Regular))

                If (Menu.Width - mi.Width) > 2 Then
                    mi.AutoSize = False
                    mi.Width = Menu.Width - 1
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

                            MenuItemEx.Add(Menu.Items, text, Sub(o As Object) OnAction(text, o), temp, Nothing).Tag = temp
                        Next
                    End If
                End If

                If Not value Is Nothing Then
                    For Each i In Menu.Items.OfType(Of MenuItemEx)()
                        If value.Equals(i.Tag) Then Text = i.Text

                        If i.DropDownItems.Count > 0 Then
                            For Each i2 In i.DropDownItems.OfType(Of MenuItemEx)()
                                If value.Equals(i2.Tag) Then
                                    If ShowPath Then
                                        Text = i2.Path
                                    Else
                                        Text = i2.Text
                                    End If
                                End If
                            Next
                        End If
                    Next
                End If

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

        Sub Add(ParamArray items As Object())
            For Each i In items
                Add(i.ToString, i, Nothing)
            Next
        End Sub

        Function Add(path As String, obj As Object, Optional tip As String = Nothing) As MenuItemEx
            Items.Add(obj)
            Dim name = path

            If path.Contains("|") Then
                name = path.RightLast("|").Trim
            End If

            Dim ret = MenuItemEx.Add(Menu.Items, path, Sub(o As Object) OnAction(name, o), obj, tip)
            ret.Tag = obj
            ret.Path = path
            Return ret
        End Function

        Sub Clear()
            Items.Clear()
            Menu.Items.ClearAndDisplose
        End Sub

        Sub OnAction(text As String, value As Object)
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

    Public Class CommandLineRichTextBox
        Inherits RichTextBoxEx

        Property LastCommandLine As String

        Sub SetText(commandLine As String)
            If commandLine = LastCommandLine Then
                Exit Sub
            End If

            If commandLine = "" Then
                Text = ""
                LastCommandLine = ""
                Exit Sub
            End If

            Format(commandLine)
        End Sub

        Sub Format(commandLine As String)
            BlockPaint = True
            Text = commandLine

            ClearAllFormatting()

            If LastCommandLine <> "" Then
                Dim selStart = GetCompareIndex(commandLine, LastCommandLine)
                Dim selEnd = commandLine.Length - GetCompareIndex(commandLine.Reverse(), LastCommandLine.Reverse())

                If selEnd > selStart AndAlso selEnd - selStart < commandLine.Length - 1 Then
                    While selStart > 0 AndAlso selStart + 1 < commandLine.Length AndAlso
                        Not commandLine(selStart - 1) + commandLine(selStart) = " -"

                        selStart -= 1
                    End While

                    If selEnd - selStart < 25 Then
                        SelectionStart = selStart

                        If selEnd - selStart = commandLine.Length Then
                            SelectionLength = 0
                        Else
                            SelectionLength = selEnd - selStart
                        End If

                        SelectionFont = New Font(Font, FontStyle.Bold)
                    End If
                End If
            End If

            If s.CommandLineHighlighting Then
                Dim th = ThemeManager.CurrentTheme.General.Controls.CommandLineRichTextBox
                Dim matches = Regex.Matches(Me.Text, "(?<=\s|^)(--\w[^\s=]*)([\s=]((?!--)[^""\s]+|""[^""\n]*"")?)?", RegexOptions.IgnoreCase)
                For Each m As Match In matches
                    Me.SelectionFormat(m.Groups(1).Index, m.Groups(1).Length, th.ParameterBackColor, th.ParameterForeColor, th.ParameterFontStyles)

                    If m.Groups.Count > 3 Then
                        Me.SelectionFormat(m.Groups(3).Index, m.Groups(3).Length, th.ParameterValueBackColor, th.ParameterValueForeColor, th.ParameterValueFontStyles)
                    End If
                Next
            End If

            SelectionStart = commandLine.Length
            BlockPaint = False
            Refresh()
            LastCommandLine = commandLine
        End Sub

        Function GetCompareIndex(a As String, b As String) As Integer
            For x = 0 To a.Length - 1
                If x > b.Length - 1 OrElse x > a.Length - 1 OrElse a(x) <> b(x) Then
                    Return x
                End If
            Next

            Return 0
        End Function

        Sub CommandLineRichTextBox_HandleCreated(sender As Object, e As EventArgs) Handles Me.HandleCreated
            If Not DesignMode Then
                Font = g.GetCodeFont
            End If
        End Sub

        Sub UpdateHeight()
            Using graphics = CreateGraphics()
                Dim stringSize = graphics.MeasureString(Text, Font, Size.Width)
                Dim h = CInt(stringSize.Height) + 1

                If h > Font.Height * 9.1 Then
                    h = CInt(Font.Height * 9.1)
                    ScrollBars = RichTextBoxScrollBars.Vertical
                End If

                Size = New Size(Size.Width, h)
            End Using
        End Sub
    End Class

    <ProvideProperty("Expand", GetType(Control))>
    Public Class FlowLayoutPanelEx
        Inherits FlowLayoutPanel

        <DefaultValue(False)>
        Property UseParentWidth As Boolean

        Property AutomaticOffset As Boolean

        Sub New()
            WrapContents = False
        End Sub

        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            MyBase.OnLayout(levent)

            If Not WrapContents AndAlso FlowDirection = FlowDirection.LeftToRight Then
                Dim nextPos As Integer

                For Each ctrl As Control In Controls
                    If ctrl.Visible Then
                        nextPos += ctrl.Margin.Left + ctrl.Width + ctrl.Margin.Right

                        Dim expandetControl = TryCast(ctrl, SimpleUI.SimpleUIControl)

                        If Not expandetControl Is Nothing AndAlso expandetControl.Expand Then
                            Dim diff = Aggregate i2 In Controls.OfType(Of Control)() Into Sum(If(i2.Visible, i2.Width + i2.Margin.Left + i2.Margin.Right, 0))

                            Dim hostWidth = Width - 1

                            If ctrl.AutoSize Then
                                nextPos += hostWidth - diff
                            Else
                                ctrl.Width += hostWidth - diff
                                nextPos += hostWidth - diff
                            End If
                        End If
                    End If

                    Dim index = Controls.IndexOf(ctrl)

                    If index < Controls.Count - 1 Then
                        Dim ctrl2 = Controls(index + 1)
                        ctrl2.Left = nextPos
                    End If
                Next
            End If

            'vertical align middles
            If Not WrapContents AndAlso
                (FlowDirection = FlowDirection.LeftToRight OrElse
                FlowDirection = FlowDirection.RightToLeft) Then

                For Each ctrl As Control In Controls
                    Dim offset = 0

                    If TypeOf ctrl Is CheckBox Then
                        offset = 1
                    End If

                    ctrl.Top = CInt((Height - ctrl.Height) / 2) + offset
                Next
            End If

            Dim labelBlocks = From block In Controls.OfType(Of SimpleUI.LabelBlock)() Where block.Label.Offset = 0

            If labelBlocks.Count > 0 Then
                Dim hMax = Aggregate i In labelBlocks Into Max(TextRenderer.MeasureText(i.Label.Text, i.Label.Font).Width)

                For Each lb In labelBlocks
                    lb.Label.Offset = hMax / lb.Label.Font.Height
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

            If UseParentWidth Then
                ret.Width = Parent.Width
            End If

            Return ret
        End Function
    End Class

    Public Class ButtonEx
        Inherits Button

        Private _backDisabledColor As Color
        Private _foreDisabledColor As Color
        Private _text As String = ""

        <DefaultValue(ButtonSymbol.None)>
        Property Symbol As ButtonSymbol

        <DefaultValue(False)>
        Property ShowMenuSymbol As Boolean

        <Browsable(False)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property ClickAction As Action

        Public Property BackDisabledColor As Color
            Get
                Return _backDisabledColor
            End Get
            Set(value As Color)
                _backDisabledColor = value
            End Set
        End Property

        Private Function ShouldSerializeBackDisabledColor() As Boolean
            Return BackDisabledColor <> Color.Empty
        End Function

        Public Property ForeDisabledColor As Color
            Get
                Return _foreDisabledColor
            End Get
            Set(value As Color)
                _foreDisabledColor = value
            End Set
        End Property

        Private Function ShouldSerializeForeDisabledColor() As Boolean
            Return ForeDisabledColor <> Color.Empty
        End Function

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property Name As String
            Get
                Return MyBase.Name
            End Get
            Set(value As String)
                MyBase.Name = value
            End Set
        End Property

        <DefaultValue(0), Browsable(False),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property TabIndex As Integer
            Get
                Return MyBase.TabIndex
            End Get
            Set(value As Integer)
                MyBase.TabIndex = value
            End Set
        End Property

        'The designer is not able to serialize the Text property probably
        'because it shadows the base Text property, Text2 is used as workaround
        Property Text2 As String
            Get
                Return _text
            End Get
            Set(value As String)
                _text = value
            End Set
        End Property

        Shadows Property Text As String
            Get
                Return _text
            End Get
            Set(value As String)
                _text = value

                If AutoSize Then
                    Dim textSize = TextRenderer.MeasureText(_text, Font)
                    Dim fh = Font.Height
                    MinimumSize = New Size(CInt(fh * 2.2), fh)
                    AutoSizeMode = AutoSizeMode.GrowOnly
                    Size = New Size(textSize.Width + Padding.Horizontal + fh, textSize.Height + CInt(fh * 0.45))
                End If

                Invalidate(True)
            End Set
        End Property

        <DefaultValue(True), Browsable(False),
        DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property UseVisualStyleBackColor As Boolean
            Get
                Return True
            End Get
            Set(value As Boolean)
            End Set
        End Property

        Protected Overrides ReadOnly Property DefaultSize As Size
            Get
                Return New Size(250, 70)
            End Get
        End Property

        Sub New()
            SetStyle(ControlStyles.ResizeRedraw, True)

            Padding = New Padding(0)

            If Not DesignHelp.IsDesignMode Then
                MinimumSize = New Size(20, 20)
                UseCompatibleTextRendering = True
                MyBase.UseVisualStyleBackColor = False
                FlatStyle = FlatStyle.Flat
            End If

            FlatAppearance.BorderSize = 2

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

            SuspendLayout()
            BackColor = theme.General.Controls.Button.BackColor
            BackDisabledColor = theme.General.Controls.Button.BackDisabledColor
            ForeColor = theme.General.Controls.Button.ForeColor
            ForeDisabledColor = theme.General.Controls.Button.ForeDisabledColor
            FlatAppearance.BorderColor = theme.General.Controls.Button.BorderColor
            ResumeLayout()
        End Sub

        Protected Overrides Sub OnClick(e As EventArgs)
            ClickAction?.Invoke()
            ContextMenuStrip?.Show(Me, 0, Height)
            MyBase.OnClick(e)
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias

            Dim fore = If(Enabled, ForeColor, ForeDisabledColor)
            Dim flags As TextFormatFlags

            Select Case TextAlign
                Case Drawing.ContentAlignment.BottomCenter
                    flags = TextFormatFlags.Bottom Or TextFormatFlags.HorizontalCenter
                Case Drawing.ContentAlignment.BottomLeft
                    flags = TextFormatFlags.Bottom Or TextFormatFlags.Left
                Case Drawing.ContentAlignment.BottomRight
                    flags = TextFormatFlags.Bottom Or TextFormatFlags.Right
                Case Drawing.ContentAlignment.MiddleCenter
                    flags = TextFormatFlags.VerticalCenter Or TextFormatFlags.HorizontalCenter
                Case Drawing.ContentAlignment.MiddleLeft
                    flags = TextFormatFlags.VerticalCenter Or TextFormatFlags.Left
                Case Drawing.ContentAlignment.MiddleRight
                    flags = TextFormatFlags.VerticalCenter Or TextFormatFlags.Right
                Case Drawing.ContentAlignment.TopCenter
                    flags = TextFormatFlags.Top Or TextFormatFlags.HorizontalCenter
                Case Drawing.ContentAlignment.TopLeft
                    flags = TextFormatFlags.Top Or TextFormatFlags.Left
                Case Drawing.ContentAlignment.TopRight
                    flags = TextFormatFlags.Top Or TextFormatFlags.Right
            End Select

            Using foreBrush = New SolidBrush(fore)
                Dim rect = ClientRectangle
                rect.Offset(0, Padding.Top - If(FlatStyle.HasFlag(FlatStyle.Flat), FlatAppearance.BorderSize \ 2, 0))

                TextRenderer.DrawText(e.Graphics, Text, Font, rect, fore, flags)
            End Using

            If ShowMenuSymbol Then
                Dim h = CInt(Font.Height * 0.3)
                Dim w = h * 2

                Dim x1 = If(Text = "", Width \ 2 - w \ 2, Width - w - CInt(w * 0.7))
                Dim y1 = CInt(Height / 2 - h / 2)

                Dim x2 = CInt(x1 + w / 2)
                Dim y2 = y1 + h

                Dim x3 = x1 + w
                Dim y3 = y1

                Using pen = New Pen(fore, Font.Height / 16.0F)
                    e.Graphics.DrawLine(pen, x1, y1, x2, y2)
                    e.Graphics.DrawLine(pen, x2, y2, x3, y3)
                End Using
            End If

            If Symbol <> ButtonSymbol.None Then
                Dim p = New Pen(ForeColor)
                p.Alignment = PenAlignment.Center
                p.EndCap = LineCap.Round
                p.StartCap = LineCap.Round
                p.Width = CInt(Height / 14)

                Dim d As New SymbolDrawer()
                d.Graphics = e.Graphics
                d.Pen = p

                d.Point1.Width = ClientSize.Width
                d.Point2.Width = ClientSize.Width
                d.Point1.Height = ClientSize.Height
                d.Point2.Height = ClientSize.Height

                Select Case Symbol
                    Case ButtonSymbol.Open
                        d.Point1.MoveRight(0.6)
                        d.Point1.MoveDown(0.3)
                        d.Point2.MoveDown(0.3)
                        d.Point2.MoveRight(0.4)
                        d.Draw()
                        d.Point1.MoveDown(0.4)
                        d.Point2.MoveDown(0.4)
                        d.Draw()
                        d.Point1.MoveLeft(0.2)
                        d.Point1.MoveUp(0.4)
                        d.Draw()
                    Case ButtonSymbol.Close
                        d.Point1.MoveRight(0.6)
                        d.Point1.MoveDown(0.3)
                        d.Point2.MoveDown(0.3)
                        d.Point2.MoveRight(0.4)
                        d.Draw()
                        d.Point1.MoveDown(0.4)
                        d.Point2.MoveDown(0.4)
                        d.Draw()
                        d.Point2.MoveRight(0.2)
                        d.Point2.MoveUp(0.4)
                        d.Draw()
                    Case ButtonSymbol.Left3
                        d.Point1.MoveRight(0.2)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.3)
                        d.Point2.MoveRight(0.4)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Reset()
                        d.Point1.MoveRight(0.2)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.7)
                        d.Point2.MoveRight(0.4)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                    Case ButtonSymbol.Left2
                        d.Point1.MoveRight(0.3)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.3)
                        d.Point2.MoveRight(0.5)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Reset()
                        d.Point1.MoveRight(0.3)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.7)
                        d.Point2.MoveRight(0.5)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                    Case ButtonSymbol.Left1
                        d.Point1.MoveRight(0.4)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.3)
                        d.Point2.MoveRight(0.6)
                        d.Draw()
                        d.Point2.MoveDown(0.4)
                        d.Draw()
                    Case ButtonSymbol.Right1
                        d.Point1.MoveRight(0.6)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.3)
                        d.Point2.MoveRight(0.4)
                        d.Draw()
                        d.Point2.MoveDown(0.4)
                        d.Draw()
                    Case ButtonSymbol.Right2
                        d.Point1.MoveRight(0.5)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.3)
                        d.Point2.MoveRight(0.3)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Reset()
                        d.Point1.MoveRight(0.5)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.7)
                        d.Point2.MoveRight(0.3)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                    Case ButtonSymbol.Right3
                        d.Point1.MoveRight(0.4)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.3)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Reset()
                        d.Point1.MoveRight(0.4)
                        d.Point1.MoveDown(0.5)
                        d.Point2.MoveDown(0.7)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                        d.Point1.MoveRight(0.2)
                        d.Point2.MoveRight(0.2)
                        d.Draw()
                    Case ButtonSymbol.Delete
                        d.Point1.MoveRight(0.3)
                        d.Point1.MoveDown(0.3)
                        d.Point2.MoveRight(0.7)
                        d.Point2.MoveDown(0.7)
                        d.Draw()
                        d.Point1.MoveRight(0.4)
                        d.Point2.MoveLeft(0.4)
                        d.Draw()
                    Case ButtonSymbol.Menu
                        d.Point1.MoveRight(0.2)
                        d.Point1.MoveDown(0.35)
                        d.Point2.MoveRight(0.5)
                        d.Point2.MoveDown(0.65)
                        d.Draw()
                        d.Point1.MoveRight(0.6)
                        d.Draw()
                End Select
            End If
        End Sub

        Sub ShowBold()
            SetFontStyle(FontStyle.Bold)

            For i = 0 To 20
                Application.DoEvents()
                Thread.Sleep(10)
            Next

            SetFontStyle(FontStyle.Regular)
        End Sub

        Enum ButtonSymbol
            None
            Left1
            Left2
            Left3
            Right1
            Right2
            Right3
            Open
            Close
            Delete
            Menu
        End Enum

        Class SymbolDrawer
            Property Point1 As New Point
            Property Point2 As New Point
            Property Pen As Pen = New Pen(Color.OrangeRed, 4)
            Property Graphics As Graphics

            Sub Draw()
                Graphics.DrawLine(Pen, Point1.X, Point1.Y, Point2.X, Point2.Y)
            End Sub

            Sub Reset()
                Point1.X = 0
                Point2.X = 0
                Point1.Y = 0
                Point2.Y = 0
            End Sub

            Class Point
                Property Width As Single
                Property Height As Single
                Property X As Single
                Property Y As Single

                Sub MoveLeft(value As Single)
                    X -= Width * value
                End Sub

                Sub MoveRight(value As Single)
                    X += Width * value
                End Sub

                Sub MoveUp(value As Single)
                    Y -= Height * value
                End Sub

                Sub MoveDown(value As Single)
                    Y += Height * value
                End Sub
            End Class
        End Class
    End Class

    Public Class ListBoxEx
        Inherits ListBox

        Private _backAlternateColor As Color
        Private _backHighlightColor As Color
        Private _backSelectedColor As Color
        Private _foreHighlightColor As Color
        Private _foreSelectedColor As Color
        Private _symbolImageColor As Color

        Private LastTick As Long
        Private KeyText As String = ""
        Private BlockOnSelectedIndexChanged As Boolean

        <DefaultValue(GetType(Button), Nothing)> Property UpButton As Button
        <DefaultValue(GetType(Button), Nothing)> Property DownButton As Button
        <DefaultValue(GetType(Button), Nothing)> Property RemoveButton As Button
        <DefaultValue(GetType(Button), Nothing)> Property Button1 As Button
        <DefaultValue(GetType(Button), Nothing)> Property Button2 As Button

        Public Property BackAlternateColor As Color
            Get
                Return _backAlternateColor
            End Get
            Set(value As Color)
                _backAlternateColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeBackAlternateColor() As Boolean
            Return BackAlternateColor <> Color.Empty
        End Function

        Public Property BackHighlightColor As Color
            Get
                Return _backHighlightColor
            End Get
            Set(value As Color)
                _backHighlightColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeBackHighlightColor() As Boolean
            Return BackHighlightColor <> Color.Empty
        End Function

        Public Property BackSelectedColor As Color
            Get
                Return _backSelectedColor
            End Get
            Set(value As Color)
                _backSelectedColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeBackSelectedColor() As Boolean
            Return BackSelectedColor <> Color.Empty
        End Function

        Public Property ForeHighlightColor As Color
            Get
                Return _foreHighlightColor
            End Get
            Set(value As Color)
                _foreHighlightColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeForeHighlightColor() As Boolean
            Return ForeHighlightColor <> Color.Empty
        End Function

        Public Property ForeSelectedColor As Color
            Get
                Return _foreSelectedColor
            End Get
            Set(value As Color)
                _foreSelectedColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeForeSelectedColor() As Boolean
            Return ForeSelectedColor <> Color.Empty
        End Function

        Public Property SymbolImageColor As Color
            Get
                Return _symbolImageColor
            End Get
            Set(value As Color)
                _symbolImageColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeSymbolImageColor() As Boolean
            Return SymbolImageColor <> Color.Empty
        End Function

        Sub New()
            MyBase.New()
            DrawMode = DrawMode.OwnerDrawFixed
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

            SuspendLayout()
            BackColor = theme.General.Controls.ListBox.BackColor
            BackAlternateColor = theme.General.Controls.ListBox.BackAlternateColor
            BackHighlightColor = theme.General.Controls.ListBox.BackHighlightColor
            BackSelectedColor = theme.General.Controls.ListBox.BackSelectedColor
            ForeColor = theme.General.Controls.ListBox.ForeColor
            ForeHighlightColor = theme.General.Controls.ListBox.ForeHighlightColor
            ForeSelectedColor = theme.General.Controls.ListBox.ForeSelectedColor
            SymbolImageColor = theme.General.Controls.ListBox.SymbolImageColor
            ResumeLayout()
        End Sub

        Protected Overrides Sub OnFontChanged(e As EventArgs)
            ItemHeight = CInt(Font.Height * 1.4)
            MyBase.OnFontChanged(e)
        End Sub

        Protected Overrides Sub OnDrawItem(e As DrawItemEventArgs)
            If Items.Count = 0 OrElse e.Index < 0 Then Exit Sub

            Dim back As Color = BackColor
            Dim fore As Color = ForeColor
            Dim r = e.Bounds
            Dim g = e.Graphics
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

            If (e.State And DrawItemState.Selected) = DrawItemState.Selected Then
                fore = ForeSelectedColor
                back = BackSelectedColor

                r.Width -= 1
                r.Height -= 1
                r.Inflate(-1, -1)
            End If

            Using backBrush As New SolidBrush(back)
                g.FillRectangle(backBrush, r)
            End Using

            Dim sf As New StringFormat
            sf.FormatFlags = StringFormatFlags.NoWrap
            sf.LineAlignment = StringAlignment.Center

            Dim r2 = e.Bounds
            r2.X = 2
            r2.Width = e.Bounds.Width

            Dim caption As String

            If DisplayMember <> "" Then
                Try
                    caption = Items(e.Index).GetType.GetProperty(DisplayMember).GetValue(Items(e.Index), Nothing).ToString
                Catch ex As Exception
                    caption = Items(e.Index).ToString()
                End Try
            Else
                caption = Items(e.Index).ToString()
            End If

            Using foreBrush = New SolidBrush(fore)
                e.Graphics.DrawString(caption, Font, foreBrush, r2, sf)
            End Using
        End Sub

        Protected Overrides Sub OnSelectedValueChanged(e As EventArgs)
            MyBase.OnSelectedValueChanged(e)
            UpdateControls()
        End Sub

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
            If FindStringExact(text) > -1 Then Items.RemoveAt(FindStringExact(text))
        End Sub

        Sub RemoveSelection()
            If SelectedIndex > -1 Then
                If SelectionMode = Windows.Forms.SelectionMode.One Then
                    Dim index = SelectedIndex

                    If Items.Count - 1 > SelectedIndex Then
                        SelectedIndex += 1
                    Else
                        SelectedIndex -= 1
                    End If

                    Items.RemoveAt(index)
                Else
                    Dim iFirst = Items.IndexOf(SelectedItems(0))

                    For i = SelectedItems.Count - 1 To 0 Step -1
                        Items.Remove(SelectedItems(i))
                    Next

                    SelectedIndex = If(iFirst > Items.Count - 1, Items.Count - 1, iFirst)
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

        Protected Overrides Sub OnCreateControl()
            MyBase.OnCreateControl()

            If Not DesignMode Then
                If Not UpButton Is Nothing Then UpButton.AddClickAction(AddressOf MoveSelectionUp)
                If Not DownButton Is Nothing Then DownButton.AddClickAction(AddressOf MoveSelectionDown)
                If Not RemoveButton Is Nothing Then RemoveButton.AddClickAction(AddressOf RemoveSelection)
                UpdateControls()
            End If
        End Sub

        Protected Overrides Sub OnSelectedIndexChanged(e As EventArgs)
            If Not BlockOnSelectedIndexChanged Then
                MyBase.OnSelectedIndexChanged(e)
                'UpdateControls()
            End If
        End Sub

        Sub UpdateControls()
            If Not RemoveButton Is Nothing Then
                RemoveButton.Enabled = Not SelectedItem Is Nothing
            End If

            If Not UpButton Is Nothing Then
                UpButton.Enabled = SelectedIndex > 0
            End If

            If Not DownButton Is Nothing Then
                DownButton.Enabled = SelectedIndex > -1 AndAlso SelectedIndex < Items.Count - 1
            End If

            If Not Button1 Is Nothing Then
                Button1.Enabled = Not SelectedItem Is Nothing
            End If

            If Not Button2 Is Nothing Then
                Button2.Enabled = Not SelectedItem Is Nothing
            End If

            If SelectedIndex = -1 AndAlso Items.Count > 0 Then
                SelectedIndex = 0
            End If
        End Sub
    End Class

    Public Class NumEdit
        Inherits UserControl

        WithEvents TextEdit As New TextEdit

        Private UpControl As New UpDownButton(True)
        Private DownControl As New UpDownButton(False)
        Private TipProvider As TipProvider

        'Private _borderNormalColor As ColorHSL = Color.Empty
        'Private _borderSelectedColor As ColorHSL = Color.Empty

        'Public Property BorderNormalColor As ColorHSL
        '    Get
        '        Return _borderNormalColor
        '    End Get
        '    Set(value As ColorHSL)
        '        _borderNormalColor = value
        '    End Set
        'End Property

        'Public Property BorderSelectedColor As ColorHSL
        '    Get
        '        Return _borderSelectedColor
        '    End Get
        '    Set(value As ColorHSL)
        '        _borderSelectedColor = value
        '    End Set
        'End Property


        Event ValueChanged(numEdit As NumEdit)

        Sub New()
            SetStyle(ControlStyles.ResizeRedraw, True)
            BorderStyle = BorderStyle.FixedSingle

            TextEdit.BorderStyle = BorderStyle.None
            TextEdit.TextBox.BorderStyle = BorderStyle.None
            TextEdit.TextBox.TextAlign = HorizontalAlignment.Center
            TextEdit.Text = "0"

            Controls.Add(TextEdit)
            Controls.Add(UpControl)
            Controls.Add(DownControl)

            UpControl.ClickAction = Sub()
                                        Value += Increment
                                        Value = Math.Round(Value, DecimalPlaces)
                                    End Sub

            DownControl.ClickAction = Sub()
                                          Value -= Increment
                                          Value = Math.Round(Value, DecimalPlaces)
                                      End Sub

            ApplyTheme()

            AddHandler ThemeManager.CurrentThemeChanged, AddressOf OnThemeChanged
            AddHandler UpControl.MouseDown, Sub() Focus()
            AddHandler DownControl.MouseDown, Sub() Focus()
            AddHandler TextEdit.LostFocus, Sub() UpdateText()
            AddHandler TextEdit.GotFocus, AddressOf SetActive
            AddHandler TextEdit.LostFocus, AddressOf SetNormal
            AddHandler TextEdit.MouseWheel, AddressOf Wheel
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

            SuspendLayout()
            BackColor = theme.General.Controls.NumEdit.BackColor
            TextEdit.BackColor = theme.General.Controls.NumEdit.BackColor
            TextEdit.TextBox.BackColor = theme.General.Controls.NumEdit.BackColor
            TextEdit.TextBox.BorderColor = theme.General.Controls.NumEdit.BorderColor
            'BorderColor = theme.General.Controls.NumEdit.BorderColor
            'BorderNormalColor = theme.General.Controls.NumEdit.BorderColor
            'BorderSelectedColor = theme.General.Controls.NumEdit.BorderSelectedColor
            ResumeLayout()
        End Sub

        WriteOnly Property Help As String
            Set(value As String)
                If TipProvider Is Nothing Then
                    TipProvider = New TipProvider()
                End If

                TipProvider.SetTip(value, TextEdit, UpControl, DownControl)
            End Set
        End Property

        <Category("Data")>
        <DefaultValue(GetType(Double), "-9000000000")>
        Property Minimum As Double = -9000000000

        <Category("Data")>
        <DefaultValue(GetType(Double), "9000000000")>
        Property Maximum As Double = 9000000000

        <Category("Data")>
        <DefaultValue(GetType(Double), "1")>
        Property Increment As Double = 1

        Private ValueValue As Double

        <Category("Data")>
        <DefaultValue(GetType(Double), "0")>
        Property Value As Double
            Get
                Return ValueValue
            End Get
            Set(value As Double)
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

        Protected Overrides Sub Dispose(disposing As Boolean)
            TipProvider?.Dispose()
            MyBase.Dispose(disposing)
        End Sub

        Sub Wheel(sender As Object, e As MouseEventArgs)
            If e.Delta > 0 Then
                Value += Increment
            Else
                Value -= Increment
            End If
        End Sub

        'Sub SetColor(c As Color)
        '    BorderColor = c
        '    Invalidate()
        'End Sub

        Protected Overridable Sub OnValueChanged(numEdit As NumEdit)
            RaiseEvent ValueChanged(Me)
        End Sub


        Protected Overrides Sub OnLayout(levent As LayoutEventArgs)
            Dim h = (ClientSize.Height \ 2) - 1
            h -= h Mod 2

            If h > 20 Then
                h -= 1
            End If

            UpControl.Width = CInt(Height * 0.8)
            UpControl.Height = h
            UpControl.Top = 1
            UpControl.Left = ClientSize.Width - UpControl.Width - 1

            DownControl.Width = UpControl.Width
            DownControl.Left = UpControl.Left
            DownControl.Top = ClientSize.Height - h - 1
            DownControl.Height = h

            TextEdit.Top = (ClientSize.Height - TextEdit.Height) \ 2 + 1
            TextEdit.Left = 2
            TextEdit.Width = DownControl.Left - 3
            TextEdit.Height = TextRenderer.MeasureText("gG", TextEdit.Font).Height

            MyBase.OnLayout(levent)
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)
            'Dim r = ClientRectangle
            'r.Inflate(-1, -1)
            'e.Graphics.FillRectangle(If(Enabled, Brushes.White, SystemBrushes.Control), r)
            'ControlPaint.DrawBorder(e.Graphics, ClientRectangle, BorderColor, ButtonBorderStyle.Solid)
        End Sub

        Protected Overrides ReadOnly Property DefaultSize() As Size
            Get
                Return New Size(250, 70)
            End Get
        End Property

        Sub SetValue(value As Double, updateText As Boolean)
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
            TextEdit.TextBox.SetTextWithoutTextChangedEvent(ValueValue.ToString("F" & DecimalPlaces))
        End Sub

        Sub TextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles TextEdit.KeyDown
            If e.KeyData = Keys.Up Then
                Value += Increment
            ElseIf e.KeyData = Keys.Down Then
                Value -= Increment
            End If
        End Sub

        Sub TextBox_TextChanged(sender As Object, e As EventArgs) Handles TextEdit.TextChanged
            If TextEdit.Text.IsDouble Then
                SetValue(TextEdit.Text.ToDouble, False)
            End If
        End Sub

        Sub SetNormal(sender As Object, e As EventArgs)
            'DownControl.SetNormal()
            'UpControl.SetNormal()
        End Sub

        Sub SetActive(sender As Object, e As EventArgs)
            'DownControl.SetActive()
            'UpControl.SetActive()
        End Sub

        'Class Edit
        '    Inherits TextEdit

        '    Private BlockTextChanged As Boolean

        '    Sub SetTextWithoutTextChanged(val As String)
        '        BlockTextChanged = True
        '        Text = val
        '        BlockTextChanged = False
        '    End Sub

        '    Protected Overrides Sub OnTextChanged(e As EventArgs)
        '        If Not BlockTextChanged Then
        '            MyBase.OnTextChanged(e)
        '        End If
        '    End Sub
        'End Class

        Class UpDownButton
            Inherits Control

            Private IsUp As Boolean
            Private IsHot As Boolean
            Private IsPressed As Boolean
            Private LastMouseDownTick As Integer

            Property ClickAction As Action

            Sub New(isUp As Boolean)
                Me.IsUp = isUp
                TabStop = False

                SetStyle(ControlStyles.ResizeRedraw, True)
                SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
            End Sub

            Protected Overrides Sub OnMouseEnter(e As EventArgs)
                MyBase.OnMouseEnter(e)
                IsHot = True
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseLeave(e As EventArgs)
                MyBase.OnMouseLeave(e)
                IsHot = False
                Invalidate()
            End Sub

            Protected Overrides Sub OnMouseDown(e As MouseEventArgs)
                MyBase.OnMouseDown(e)
                IsPressed = True
                Invalidate()
                ClickAction.Invoke()
                LastMouseDownTick = Environment.TickCount
                MouseDownClicks(750, LastMouseDownTick)
            End Sub

            Protected Overrides Sub OnMouseUp(e As MouseEventArgs)
                MyBase.OnMouseUp(e)
                IsPressed = False
                Invalidate()
            End Sub

            Protected Overrides Sub OnPaint(e As PaintEventArgs)
                MyBase.OnPaint(e)

                Dim gx = e.Graphics
                gx.SmoothingMode = SmoothingMode.HighQuality

                If IsPressed Then
                    gx.Clear(ThemeManager.CurrentTheme.General.Controls.NumEdit.UpDownButton.BackPressedColor)
                ElseIf IsHot Then
                    gx.Clear(ThemeManager.CurrentTheme.General.Controls.NumEdit.UpDownButton.BackHotColor)
                Else
                    gx.Clear(ThemeManager.CurrentTheme.General.Controls.NumEdit.UpDownButton.BackColor)
                End If

                'ControlPaint.DrawBorder(gx, ClientRectangle, ThemeManager.CurrentTheme.General.Controls.NumEdit.UpDownButton.BorderColor, ButtonBorderStyle.Solid)

                Dim h = CInt(Font.Height * 0.2)
                Dim w = h * 2

                Dim x1 = Width \ 2 - w \ 2
                Dim y1 = CInt(Height / 2 - h / 2)

                Dim x2 = CInt(x1 + w / 2)
                Dim y2 = y1 + h

                Dim x3 = x1 + w
                Dim y3 = y1

                If IsUp Then
                    y1 = y2
                    y2 = y3
                    y3 = y1
                End If

                Using pen = New Pen(If(Enabled, ThemeManager.CurrentTheme.General.Controls.NumEdit.UpDownButton.ForeColor, ThemeManager.CurrentTheme.General.Controls.NumEdit.UpDownButton.ForeDisabledColor), Font.Height / 20.0F)
                    gx.DrawLine(pen, x1, y1, x2, y2)
                    gx.DrawLine(pen, x2, y2, x3, y3)
                End Using
            End Sub

            Async Sub MouseDownClicks(sleep As Integer, tick As Integer)
                Await Task.Run(Sub() Thread.Sleep(sleep))

                If IsPressed AndAlso LastMouseDownTick = tick Then
                    ClickAction.Invoke()
                    MouseDownClicks(20, tick)
                End If
            End Sub

            Public Sub SetNormal()
                Invalidate()
            End Sub

            Public Sub SetActive()
                Invalidate()
            End Sub
        End Class
    End Class

    Public Class TextEdit
        Inherits UserControl

        Private _backReadonlyColor As Color
        Private _borderColor As Color
        Private _borderFocusedColor As Color
        Private _borderHoverColor As Color
        Private _drawBorder As Integer = -1

        Public WithEvents TextBox As New TextBoxEx

        Public Shadows Event DoubleClick(sender As Object, e As EventArgs)
        Public Shadows Event KeyDown(sender As Object, e As KeyEventArgs)
        Public Shadows Event MouseDown(sender As Object, e As MouseEventArgs)
        Public Shadows Event MouseWheel(sender As Object, e As MouseEventArgs)
        Public Shadows Event TextChanged(sender As Object, e As EventArgs)

        Public Property BackReadonlyColor As Color
            Get
                Return _backReadonlyColor
            End Get
            Set(value As Color)
                _backReadonlyColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeBackReadonlyColor() As Boolean
            Return BackReadonlyColor <> Color.Empty
        End Function

        Public Property BorderColor As Color
            Get
                Return _borderColor
            End Get
            Set(value As Color)
                _borderColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeBorderColor() As Boolean
            Return BorderColor <> Color.Empty
        End Function

        Public Property BorderFocusedColor As Color
            Get
                Return _borderFocusedColor
            End Get
            Set(value As Color)
                _borderFocusedColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeBorderFocusedColor() As Boolean
            Return BorderFocusedColor <> Color.Empty
        End Function

        Public Property BorderHoverColor As Color
            Get
                Return _borderHoverColor
            End Get
            Set(value As Color)
                _borderHoverColor = value
                Invalidate()
            End Set
        End Property

        Private Function ShouldSerializeBorderHoverColor() As Boolean
            Return BorderHoverColor <> Color.Empty
        End Function

        ReadOnly Property DrawBorder As Integer
            Get
                If _drawBorder < 0 Then
                    If Parent IsNot Nothing Then
                        _drawBorder = If(TypeOf Parent Is NumEdit, 0, 1)
                    Else
                        _drawBorder = 1
                    End If
                End If
                Return _drawBorder
            End Get
        End Property

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Property [ReadOnly] As Boolean
            Get
                Return TextBox.ReadOnly
            End Get
            Set(value As Boolean)
                TextBox.ReadOnly = value
                Invalidate()
            End Set
        End Property

        <Browsable(True)>
        <DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)>
        Overrides Property Text As String
            Get
                Return TextBox.Text
            End Get
            Set(value As String)
                TextBox.Text = value
            End Set
        End Property

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property TabIndex As Integer
            Get
                Return MyBase.TabIndex
            End Get
            Set(value As Integer)
                MyBase.TabIndex = value
            End Set
        End Property

        Sub New()
            SetStyle(ControlStyles.ResizeRedraw, True)
            TextBox.BorderStyle = BorderStyle.None
            Controls.Add(TextBox)

            AddHandler TextBox.DoubleClick, Sub(sender As Object, e As EventArgs) RaiseEvent DoubleClick(sender, e)
            AddHandler TextBox.KeyDown, Sub(sender As Object, e As KeyEventArgs) RaiseEvent KeyDown(sender, e)
            AddHandler TextBox.MouseDown, Sub(sender As Object, e As MouseEventArgs) RaiseEvent MouseDown(sender, e)
            AddHandler TextBox.MouseWheel, Sub(sender As Object, e As MouseEventArgs) RaiseEvent MouseWheel(sender, e)
            AddHandler TextBox.TextChanged, Sub(sender As Object, e As EventArgs) RaiseEvent TextChanged(sender, e)
            AddHandler TextBox.GotFocus, AddressOf TextBox_GotFocus
            AddHandler TextBox.LostFocus, AddressOf TextBox_LostFocus
            AddHandler TextBox.MouseEnter, AddressOf TextBox_GotFocus
            AddHandler TextBox.MouseLeave, AddressOf TextBox_LostFocus

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

            SuspendLayout()
            BackColor = theme.General.Controls.TextEdit.BackColor
            BackReadonlyColor = theme.General.Controls.TextEdit.BackReadonlyColor
            ForeColor = theme.General.Controls.TextEdit.ForeColor
            BorderColor = theme.General.Controls.TextEdit.BorderColor
            BorderFocusedColor = theme.General.Controls.TextEdit.BorderFocusedColor
            BorderHoverColor = theme.General.Controls.TextEdit.BorderHoverColor
            ResumeLayout()
        End Sub

        Sub TextBox_GotFocus(sender As Object, e As EventArgs)
            Invalidate()
        End Sub

        Sub TextBox_LostFocus(sender As Object, e As EventArgs)
            Invalidate()
        End Sub

        Protected Overrides Sub OnLayout(args As LayoutEventArgs)
            MyBase.OnLayout(args)

            If TextBox.Multiline Then
                TextBox.Top = 2
                TextBox.Left = 2
                TextBox.Width = ClientSize.Width - 4
                TextBox.Height = ClientSize.Height - 4
            Else
                TextBox.Top = ((ClientSize.Height - TextBox.Height) \ 2)
                TextBox.Left = 2
                TextBox.Width = ClientSize.Width - 4
                Dim h = TextRenderer.MeasureText("gG", TextBox.Font).Height

                If TextBox.Height < h Then
                    TextBox.Multiline = True
                    TextBox.MinimumSize = New Size(0, h)
                    TextBox.Multiline = False
                End If
            End If
        End Sub

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            MyBase.OnPaint(e)

            'Dim col = If(Enabled AndAlso Not TextBox.ReadOnly, BackColor, BackReadonlyColor)
            'Dim cr = ClientRectangle
            'cr.Inflate(-1, -1)

            'Using brush As New SolidBrush(col)
            '    e.Graphics.FillRectangle(brush, cr)
            'End Using

            If Not [ReadOnly] AndAlso DrawBorder > 0 Then
                Dim borderCol = If(TextBox.Focused OrElse Not GetChildAtPoint(PointToClient(Cursor.Position)) Is Nothing, BorderFocusedColor, BorderColor)
                borderCol = If(GetChildAtPoint(PointToClient(Cursor.Position)) IsNot Nothing, BorderHoverColor, borderCol)
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle, borderCol, ButtonBorderStyle.Solid)
            End If
        End Sub
    End Class

    Public Interface IPage
        Property Node As TreeNode
        Property Path As String
        Property TipProvider As TipProvider
        Property FormSizeScaleFactor As SizeF
    End Interface

    Public Class DataGridViewEx
        Inherits DataGridView

        Sub New()
            MyBase.New()
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

            If Not Disposing AndAlso Not IsDisposed Then
                SuspendLayout()
                BackgroundColor = theme.General.Controls.GridView.BackColor
                ForeColor = theme.General.Controls.GridView.ForeColor
                GridColor = theme.General.Controls.GridView.GridColor
                ResumeLayout()
            End If
        End Sub

        Function AddTextBoxColumn() As DataGridViewTextBoxColumn
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

        Function HoverTab() As TabPage
            For index = 0 To TabCount - 1
                If GetTabRect(index).Contains(PointToClient(Cursor.Position)) Then
                    Return TabPages(index)
                End If
            Next
        End Function

        Sub SwapTabPages(ByVal tp1 As TabPage, ByVal tp2 As TabPage)
            Dim index1 = TabPages.IndexOf(tp1)
            Dim index2 = TabPages.IndexOf(tp2)
            TabPages(index1) = tp2
            TabPages(index2) = tp1
        End Sub
    End Class

    Public Class LabelProgressBar
        Inherits Control

        Property ProgressColor As Color

        Sub New()
            SetStyle(ControlStyles.ResizeRedraw, True)
            SetStyle(ControlStyles.Selectable, False)
            SetStyle(ControlStyles.OptimizedDoubleBuffer, True)

            'If BackColor.GetBrightness > 0.5 Then
            '    ForeColor = Color.FromArgb(10, 10, 10)
            '    BackColor = Color.FromArgb(240, 240, 240)
            '    ProgressColor = Color.FromArgb(180, 180, 180)
            'Else
            '    ForeColor = Color.FromArgb(240, 240, 240)
            '    BackColor = Color.FromArgb(10, 10, 10)
            '    ProgressColor = Color.FromArgb(100, 100, 100)
            'End If

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

            SuspendLayout()
            BackColor = theme.General.Controls.LabelProgressBar.BackColor
            ForeColor = theme.General.Controls.LabelProgressBar.ForeColor
            ProgressColor = theme.General.Controls.LabelProgressBar.ProgressColor
            ResumeLayout()
        End Sub

        Private Function ShouldSerializeProgressColor() As Boolean
            Return ProgressColor <> Color.Empty
        End Function

        Private _Minimum As Double

        Public Property Minimum() As Double
            Get
                Return _Minimum
            End Get
            Set
                If _Minimum <> Value Then
                    _Minimum = Value
                    Invalidate()
                End If
            End Set
        End Property

        Private _Maximum As Double = 100

        Public Property Maximum() As Double
            Get
                Return _Maximum
            End Get
            Set
                If _Maximum <> Value Then
                    _Maximum = Value
                    Invalidate()
                End If
            End Set
        End Property

        Private _Value As Double

        Public Property Value() As Double
            Get
                Return _Value
            End Get
            Set
                If _Value <> Value Then
                    _Value = Value
                    Invalidate()
                End If
            End Set
        End Property

        Public Overrides Property Text As String
            Get
                Return MyBase.Text
            End Get
            Set
                MyBase.Text = Value
                Invalidate()
            End Set
        End Property

        Protected Overrides Sub OnPaint(e As PaintEventArgs)
            Dim g = e.Graphics
            g.TextRenderingHint = Drawing.Text.TextRenderingHint.AntiAlias

            Using foreBrush = New SolidBrush(ForeColor)
                Using progressBrush = New SolidBrush(ProgressColor)
                    g.FillRectangle(progressBrush, New RectangleF(0, 0, CSng(Width * (Value - Minimum) / Maximum), Height))
                    g.DrawString(Text, Font, foreBrush, 0, CInt((Height - FontHeight) / 2))
                End Using
            End Using

            MyBase.OnPaint(e)
        End Sub
    End Class
End Namespace