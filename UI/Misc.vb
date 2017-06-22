Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Runtime.InteropServices

Namespace UI
    Public Class FormBase
        Inherits Form

        Event FilesDropped(files As String())

        Private FileDropValue As Boolean

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Shadows Property FontHeight As Integer

        <DefaultValue(False)>
        Property FileDrop As Boolean
            Get
                Return FileDropValue
            End Get
            Set(value As Boolean)
                FileDropValue = value
                AllowDrop = value
            End Set
        End Property

        Public Sub New()
            Font = New Font("Segoe UI", 9)
            FontHeight = Font.Height
        End Sub

        Protected Overrides Sub OnDragEnter(e As DragEventArgs)
            If FileDrop Then
                Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())
                If Not files.NothingOrEmpty Then e.Effect = DragDropEffects.Copy
            End If

            MyBase.OnDragEnter(e)
        End Sub

        Protected Overrides Sub OnDragDrop(e As DragEventArgs)
            If FileDrop Then
                Dim files = TryCast(e.Data.GetData(DataFormats.FileDrop), String())
                If Not files.NothingOrEmpty Then RaiseEvent FilesDropped(files)
            End If

            MyBase.OnDragDrop(e)
        End Sub

        Protected Overrides Sub OnLoad(e As EventArgs)
            KeyPreview = True
            SetTabIndexes(Me)

            If AutoScaleMode = AutoScaleMode.Dpi Then
                If s.UIScaleFactor <> 1 Then
                    Font = New Font("Segoe UI", 9 * s.UIScaleFactor)
                    Scale(New SizeF(1 * s.UIScaleFactor, 1 * s.UIScaleFactor))
                End If
            Else
                Dim designDimension = 144
                If s.UIScaleFactor <> 1 Then Font = New Font("Segoe UI", 9 * s.UIScaleFactor)

                If designDimension <> DeviceDpi OrElse s.UIScaleFactor <> 1 Then
                    Scale(New SizeF(CSng(DeviceDpi / designDimension * s.UIScaleFactor),
                                    CSng(DeviceDpi / designDimension * s.UIScaleFactor)))
                End If
            End If

            MyBase.OnLoad(e)

            If Not DesignHelp.IsDesignMode Then s.WindowPositions?.RestorePosition(Me)
        End Sub

        'Private ChangeService As IComponentChangeService

        'Public Overrides Property Site() As ISite
        '    Get
        '        Return MyBase.Site
        '    End Get
        '    Set(ByVal Value As ISite)
        '        MyBase.Site = Value

        '        If ChangeService Is Nothing Then
        '            ChangeService = DirectCast(GetService(GetType(IComponentChangeService)), IComponentChangeService)
        '            AddHandler ChangeService.ComponentChanged, AddressOf OnComponentChanged
        '        End If
        '    End Set
        'End Property

        'Sub OnComponentChanged(sender As Object, e As ComponentChangedEventArgs)
        '    If e.Component Is Me AndAlso (e.Member.Name = "Size" OrElse e.Member.Name = "Height") Then
        '        DesignClientSize = ClientSize
        '        DesignDPI = CurrentDPIDimension
        '    End If
        'End Sub

        'Protected Overrides Sub Dispose(disposing As Boolean)
        '    MyBase.Dispose(disposing)
        '    If Not ChangeService Is Nothing Then RemoveHandler ChangeService.ComponentChanged, AddressOf OnComponentChanged
        'End Sub

        Protected Overrides Sub OnFormClosing(e As FormClosingEventArgs)
            If Not s.WindowPositions Is Nothing Then s.WindowPositions.Save(Me)
            MyBase.OnFormClosing(e)
        End Sub

        Sub SetTabIndexes(c As Control)
            Dim index = 0

            Dim controls = From i In c.Controls.OfType(Of Control)()
                           Order By Math.Sqrt(i.Top * i.Top + i.Left * i.Left)

            For Each i In controls
                i.TabIndex = index
                index += 1
                SetTabIndexes(i)
            Next
        End Sub

        Protected Overrides Sub WndProc(ByRef m As Message)
#Region "WM"
            'If m.Msg = 6 Then Debug.WriteLine("WM_ACTIVATE")
            'If m.Msg = &H1C Then Debug.WriteLine("WM_ACTIVATEAPP")
            'If m.Msg = &H360 Then Debug.WriteLine("WM_AFXFIRST")
            'If m.Msg = &H37F Then Debug.WriteLine("WM_AFXLAST")
            'If m.Msg = &H8000 Then Debug.WriteLine("WM_APP")
            'If m.Msg = &H319 Then Debug.WriteLine("WM_APPCOMMAND")
            'If m.Msg = 780 Then Debug.WriteLine("WM_ASKCBFORMATNAME")
            'If m.Msg = &H4B Then Debug.WriteLine("WM_CANCELJOURNAL")
            'If m.Msg = &H1F Then Debug.WriteLine("WM_CANCELMODE")
            'If m.Msg = &H215 Then Debug.WriteLine("WM_CAPTURECHANGED")
            'If m.Msg = &H30D Then Debug.WriteLine("WM_CHANGECBCHAIN")
            'If m.Msg = &H127 Then Debug.WriteLine("WM_CHANGEUISTATE")
            'If m.Msg = &H102 Then Debug.WriteLine("WM_CHAR")
            'If m.Msg = &H2F Then Debug.WriteLine("WM_CHARTOITEM")
            'If m.Msg = &H22 Then Debug.WriteLine("WM_CHILDACTIVATE")
            'If m.Msg = &H401 Then Debug.WriteLine("WM_CHOOSEFONT_GETLOGFONT")
            'If m.Msg = &H303 Then Debug.WriteLine("WM_CLEAR")
            'If m.Msg = &H10 Then Debug.WriteLine("WM_CLOSE")
            'If m.Msg = &H111 Then Debug.WriteLine("WM_COMMAND")
            'If m.Msg = &H44 Then Debug.WriteLine("WM_COMMNOTIFY")
            'If m.Msg = &H41 Then Debug.WriteLine("WM_COMPACTING")
            'If m.Msg = &H39 Then Debug.WriteLine("WM_COMPAREITEM")
            'If m.Msg = &H7B Then Debug.WriteLine("WM_CONTEXTMENU")
            'If m.Msg = &H301 Then Debug.WriteLine("WM_COPY")
            'If m.Msg = &H4A Then Debug.WriteLine("WM_COPYDATA")
            'If m.Msg = 1 Then Debug.WriteLine("WM_CREATE")
            'If m.Msg = &H19 Then Debug.WriteLine("WM_CTLCOLOR")
            'If m.Msg = &H135 Then Debug.WriteLine("WM_CTLCOLORBTN")
            'If m.Msg = 310 Then Debug.WriteLine("WM_CTLCOLORDLG")
            'If m.Msg = &H133 Then Debug.WriteLine("WM_CTLCOLOREDIT")
            'If m.Msg = &H134 Then Debug.WriteLine("WM_CTLCOLORLISTBOX")
            'If m.Msg = &H132 Then Debug.WriteLine("WM_CTLCOLORMSGBOX")
            'If m.Msg = &H137 Then Debug.WriteLine("WM_CTLCOLORSCROLLBAR")
            'If m.Msg = &H138 Then Debug.WriteLine("WM_CTLCOLORSTATIC")
            'If m.Msg = &H300 Then Debug.WriteLine("WM_CUT")
            'If m.Msg = &H103 Then Debug.WriteLine("WM_DEADCHAR")
            'If m.Msg = &H2D Then Debug.WriteLine("WM_DELETEITEM")
            'If m.Msg = 2 Then Debug.WriteLine("WM_DESTROY")
            'If m.Msg = &H307 Then Debug.WriteLine("WM_DESTROYCLIPBOARD")
            'If m.Msg = &H219 Then Debug.WriteLine("WM_DEVICECHANGE")
            'If m.Msg = &H1B Then Debug.WriteLine("WM_DEVMODECHANGE")
            'If m.Msg = &H7E Then Debug.WriteLine("WM_DISPLAYCHANGE")
            'If m.Msg = &H308 Then Debug.WriteLine("WM_DRAWCLIPBOARD")
            'If m.Msg = &H2B Then Debug.WriteLine("WM_DRAWITEM")
            'If m.Msg = &H233 Then Debug.WriteLine("WM_DROPFILES")
            'If m.Msg = &H31E Then Debug.WriteLine("WM_DWMCOMPOSITIONCHANGED")
            'If m.Msg = 10 Then Debug.WriteLine("WM_ENABLE")
            'If m.Msg = &H16 Then Debug.WriteLine("WM_ENDSESSION")
            'If m.Msg = &H121 Then Debug.WriteLine("WM_ENTERIDLE")
            'If m.Msg = &H211 Then Debug.WriteLine("WM_ENTERMENULOOP")
            'If m.Msg = &H231 Then Debug.WriteLine("WM_ENTERSIZEMOVE")
            'If m.Msg = 20 Then Debug.WriteLine("WM_ERASEBKGND")
            'If m.Msg = 530 Then Debug.WriteLine("WM_EXITMENULOOP")
            'If m.Msg = &H232 Then Debug.WriteLine("WM_EXITSIZEMOVE")
            'If m.Msg = &H2CB Then Debug.WriteLine("WM_FLICK")
            'If m.Msg = &H1D Then Debug.WriteLine("WM_FONTCHANGE")
            'If m.Msg = &H87 Then Debug.WriteLine("WM_GETDLGCODE")
            'If m.Msg = &H31 Then Debug.WriteLine("WM_GETFONT")
            'If m.Msg = &H33 Then Debug.WriteLine("WM_GETHOTKEY")
            'If m.Msg = &H7F Then Debug.WriteLine("WM_GETICON")
            'If m.Msg = &H24 Then Debug.WriteLine("WM_GETMINMAXINFO")
            'If m.Msg = &H3D Then Debug.WriteLine("WM_GETOBJECT")
            'If m.Msg = 13 Then Debug.WriteLine("WM_GETTEXT")
            'If m.Msg = 14 Then Debug.WriteLine("WM_GETTEXTLENGTH")
            'If m.Msg = &H358 Then Debug.WriteLine("WM_HANDHELDFIRST")
            'If m.Msg = &H35F Then Debug.WriteLine("WM_HANDHELDLAST")
            'If m.Msg = &H53 Then Debug.WriteLine("WM_HELP")
            'If m.Msg = &H312 Then Debug.WriteLine("WM_HOTKEY")
            'If m.Msg = &H114 Then Debug.WriteLine("WM_HSCROLL")
            'If m.Msg = &H30E Then Debug.WriteLine("WM_HSCROLLCLIPBOARD")
            'If m.Msg = &H27 Then Debug.WriteLine("WM_ICONERASEBKGND")
            'If m.Msg = &H286 Then Debug.WriteLine("WM_IME_CHAR")
            'If m.Msg = &H10F Then Debug.WriteLine("WM_IME_COMPOSITION")
            'If m.Msg = &H284 Then Debug.WriteLine("WM_IME_COMPOSITIONFULL")
            'If m.Msg = &H283 Then Debug.WriteLine("WM_IME_CONTROL")
            'If m.Msg = 270 Then Debug.WriteLine("WM_IME_ENDCOMPOSITION")
            'If m.Msg = &H290 Then Debug.WriteLine("WM_IME_KEYDOWN")
            'If m.Msg = &H10F Then Debug.WriteLine("WM_IME_KEYLAST")
            'If m.Msg = &H291 Then Debug.WriteLine("WM_IME_KEYUP")
            'If m.Msg = &H282 Then Debug.WriteLine("WM_IME_NOTIFY")
            'If m.Msg = &H288 Then Debug.WriteLine("WM_IME_REQUEST")
            'If m.Msg = &H285 Then Debug.WriteLine("WM_IME_SELECT")
            'If m.Msg = &H281 Then Debug.WriteLine("WM_IME_SETCONTEXT")
            'If m.Msg = &H10D Then Debug.WriteLine("WM_IME_STARTCOMPOSITION")
            'If m.Msg = &H110 Then Debug.WriteLine("WM_INITDIALOG")
            'If m.Msg = &H116 Then Debug.WriteLine("WM_INITMENU")
            'If m.Msg = &H117 Then Debug.WriteLine("WM_INITMENUPOPUP")
            'If m.Msg = &HFF Then Debug.WriteLine("WM_INPUT")
            'If m.Msg = &H51 Then Debug.WriteLine("WM_INPUTLANGCHANGE")
            'If m.Msg = 80 Then Debug.WriteLine("WM_INPUTLANGCHANGEREQUEST")
            'If m.Msg = &H100 Then Debug.WriteLine("WM_KEYDOWN")
            'If m.Msg = &H100 Then Debug.WriteLine("WM_KEYFIRST")
            'If m.Msg = &H108 Then Debug.WriteLine("WM_KEYLAST")
            'If m.Msg = &H101 Then Debug.WriteLine("WM_KEYUP")
            'If m.Msg = 8 Then Debug.WriteLine("WM_KILLFOCUS")
            'If m.Msg = &H203 Then Debug.WriteLine("WM_LBUTTONDBLCLK")
            'If m.Msg = &H201 Then Debug.WriteLine("WM_LBUTTONDOWN")
            'If m.Msg = &H202 Then Debug.WriteLine("WM_LBUTTONUP")
            'If m.Msg = &H209 Then Debug.WriteLine("WM_MBUTTONDBLCLK")
            'If m.Msg = &H207 Then Debug.WriteLine("WM_MBUTTONDOWN")
            'If m.Msg = 520 Then Debug.WriteLine("WM_MBUTTONUP")
            'If m.Msg = &H222 Then Debug.WriteLine("WM_MDIACTIVATE")
            'If m.Msg = &H227 Then Debug.WriteLine("WM_MDICASCADE")
            'If m.Msg = &H220 Then Debug.WriteLine("WM_MDICREATE")
            'If m.Msg = &H221 Then Debug.WriteLine("WM_MDIDESTROY")
            'If m.Msg = &H229 Then Debug.WriteLine("WM_MDIGETACTIVE")
            'If m.Msg = &H228 Then Debug.WriteLine("WM_MDIICONARRANGE")
            'If m.Msg = &H225 Then Debug.WriteLine("WM_MDIMAXIMIZE")
            'If m.Msg = &H224 Then Debug.WriteLine("WM_MDINEXT")
            'If m.Msg = &H234 Then Debug.WriteLine("WM_MDIREFRESHMENU")
            'If m.Msg = &H223 Then Debug.WriteLine("WM_MDIRESTORE")
            'If m.Msg = 560 Then Debug.WriteLine("WM_MDISETMENU")
            'If m.Msg = 550 Then Debug.WriteLine("WM_MDITILE")
            'If m.Msg = &H2C Then Debug.WriteLine("WM_MEASUREITEM")
            'If m.Msg = &H120 Then Debug.WriteLine("WM_MENUCHAR")
            'If m.Msg = &H11F Then Debug.WriteLine("WM_MENUSELECT")
            'If m.Msg = &H21 Then Debug.WriteLine("WM_MOUSEACTIVATE")
            'If m.Msg = &H200 Then Debug.WriteLine("WM_MOUSEFIRST")
            'If m.Msg = &H2A1 Then Debug.WriteLine("WM_MOUSEHOVER")
            'If m.Msg = &H20A Then Debug.WriteLine("WM_MOUSELAST")
            'If m.Msg = &H2A3 Then Debug.WriteLine("WM_MOUSELEAVE")
            'If m.Msg = &H200 Then Debug.WriteLine("WM_MOUSEMOVE")
            'If m.Msg = &H9B Then Debug.WriteLine("WM_MOUSEQUERY")
            'If m.Msg = &H20A Then Debug.WriteLine("WM_MOUSEWHEEL")
            'If m.Msg = 3 Then Debug.WriteLine("WM_MOVE")
            'If m.Msg = &H216 Then Debug.WriteLine("WM_MOVING")
            'If m.Msg = &H86 Then Debug.WriteLine("WM_NCACTIVATE")
            'If m.Msg = &H83 Then Debug.WriteLine("WM_NCCALCSIZE")
            'If m.Msg = &H81 Then Debug.WriteLine("WM_NCCREATE")
            'If m.Msg = 130 Then Debug.WriteLine("WM_NCDESTROY")
            'If m.Msg = &H84 Then Debug.WriteLine("WM_NCHITTEST")
            'If m.Msg = &HA3 Then Debug.WriteLine("WM_NCLBUTTONDBLCLK")
            'If m.Msg = &HA1 Then Debug.WriteLine("WM_NCLBUTTONDOWN")
            'If m.Msg = &HA2 Then Debug.WriteLine("WM_NCLBUTTONUP")
            'If m.Msg = &HA9 Then Debug.WriteLine("WM_NCMBUTTONDBLCLK")
            'If m.Msg = &HA7 Then Debug.WriteLine("WM_NCMBUTTONDOWN")
            'If m.Msg = &HA8 Then Debug.WriteLine("WM_NCMBUTTONUP")
            'If m.Msg = &H2A2 Then Debug.WriteLine("WM_NCMOUSELEAVE")
            'If m.Msg = 160 Then Debug.WriteLine("WM_NCMOUSEMOVE")
            'If m.Msg = &H85 Then Debug.WriteLine("WM_NCPAINT")
            'If m.Msg = &HA6 Then Debug.WriteLine("WM_NCRBUTTONDBLCLK")
            'If m.Msg = &HA4 Then Debug.WriteLine("WM_NCRBUTTONDOWN")
            'If m.Msg = &HA5 Then Debug.WriteLine("WM_NCRBUTTONUP")
            'If m.Msg = &HAD Then Debug.WriteLine("WM_NCXBUTTONDBLCLK")
            'If m.Msg = &HAB Then Debug.WriteLine("WM_NCXBUTTONDOWN")
            'If m.Msg = &HAC Then Debug.WriteLine("WM_NCXBUTTONUP")
            'If m.Msg = 40 Then Debug.WriteLine("WM_NEXTDLGCTL")
            'If m.Msg = &H213 Then Debug.WriteLine("WM_NEXTMENU")
            'If m.Msg = &H4E Then Debug.WriteLine("WM_NOTIFY")
            'If m.Msg = &H55 Then Debug.WriteLine("WM_NOTIFYFORMAT")
            'If m.Msg = 0 Then Debug.WriteLine("WM_NULL")
            'If m.Msg = 15 Then Debug.WriteLine("WM_PAINT")
            'If m.Msg = &H309 Then Debug.WriteLine("WM_PAINTCLIPBOARD")
            'If m.Msg = &H26 Then Debug.WriteLine("WM_PAINTICON")
            'If m.Msg = &H311 Then Debug.WriteLine("WM_PALETTECHANGED")
            'If m.Msg = &H310 Then Debug.WriteLine("WM_PALETTEISCHANGING")
            'If m.Msg = &H210 Then Debug.WriteLine("WM_PARENTNOTIFY")
            'If m.Msg = 770 Then Debug.WriteLine("WM_PASTE")
            'If m.Msg = &H380 Then Debug.WriteLine("WM_PENWINFIRST")
            'If m.Msg = &H38F Then Debug.WriteLine("WM_PENWINLAST")
            'If m.Msg = &H48 Then Debug.WriteLine("WM_POWER")
            'If m.Msg = &H218 Then Debug.WriteLine("WM_POWERBROADCAST")
            'If m.Msg = &H317 Then Debug.WriteLine("WM_PRINT")
            'If m.Msg = &H318 Then Debug.WriteLine("WM_PRINTCLIENT")
            'If m.Msg = &H37 Then Debug.WriteLine("WM_QUERYDRAGICON")
            'If m.Msg = &H11 Then Debug.WriteLine("WM_QUERYENDSESSION")
            'If m.Msg = &H30F Then Debug.WriteLine("WM_QUERYNEWPALETTE")
            'If m.Msg = &H13 Then Debug.WriteLine("WM_QUERYOPEN")
            'If m.Msg = &H2CC Then Debug.WriteLine("WM_QUERYSYSTEMGESTURESTATUS")
            'If m.Msg = &H129 Then Debug.WriteLine("WM_QUERYUISTATE")
            'If m.Msg = &H23 Then Debug.WriteLine("WM_QUEUESYNC")
            'If m.Msg = &H12 Then Debug.WriteLine("WM_QUIT")
            'If m.Msg = &H206 Then Debug.WriteLine("WM_RBUTTONDBLCLK")
            'If m.Msg = &H204 Then Debug.WriteLine("WM_RBUTTONDOWN")
            'If m.Msg = &H205 Then Debug.WriteLine("WM_RBUTTONUP")
            'If m.Msg = &H2000 Then Debug.WriteLine("WM_REFLECT")
            'If m.Msg = &H306 Then Debug.WriteLine("WM_RENDERALLFORMATS")
            'If m.Msg = &H305 Then Debug.WriteLine("WM_RENDERFORMAT")
            'If m.Msg = &H20 Then Debug.WriteLine("WM_SETCURSOR")
            'If m.Msg = 7 Then Debug.WriteLine("WM_SETFOCUS")
            'If m.Msg = &H30 Then Debug.WriteLine("WM_SETFONT")
            'If m.Msg = 50 Then Debug.WriteLine("WM_SETHOTKEY")
            'If m.Msg = &H80 Then Debug.WriteLine("WM_SETICON")
            'If m.Msg = 11 Then Debug.WriteLine("WM_SETREDRAW")
            'If m.Msg = 12 Then Debug.WriteLine("WM_SETTEXT")
            'If m.Msg = &H1A Then Debug.WriteLine("WM_SETTINGCHANGE")
            'If m.Msg = &H18 Then Debug.WriteLine("WM_SHOWWINDOW")
            'If m.Msg = 5 Then Debug.WriteLine("WM_SIZE")
            'If m.Msg = &H30B Then Debug.WriteLine("WM_SIZECLIPBOARD")
            'If m.Msg = &H214 Then Debug.WriteLine("WM_SIZING")
            'If m.Msg = &H2A Then Debug.WriteLine("WM_SPOOLERSTATUS")
            'If m.Msg = &H7D Then Debug.WriteLine("WM_STYLECHANGED")
            'If m.Msg = &H7C Then Debug.WriteLine("WM_STYLECHANGING")
            'If m.Msg = &H106 Then Debug.WriteLine("WM_SYSCHAR")
            'If m.Msg = &H15 Then Debug.WriteLine("WM_SYSCOLORCHANGE")
            'If m.Msg = &H112 Then Debug.WriteLine("WM_SYSCOMMAND")
            'If m.Msg = &H107 Then Debug.WriteLine("WM_SYSDEADCHAR")
            'If m.Msg = 260 Then Debug.WriteLine("WM_SYSKEYDOWN")
            'If m.Msg = &H105 Then Debug.WriteLine("WM_SYSKEYUP")
            'If m.Msg = &H2C8 Then Debug.WriteLine("WM_TABLET_ADDED")
            'If m.Msg = &H2C9 Then Debug.WriteLine("WM_TABLET_REMOVED")
            'If m.Msg = &H52 Then Debug.WriteLine("WM_TCARD")
            'If m.Msg = &H31A Then Debug.WriteLine("WM_THEMECHANGED")
            'If m.Msg = 30 Then Debug.WriteLine("WM_TIMECHANGE")
            'If m.Msg = &H113 Then Debug.WriteLine("WM_TIMER")
            'If m.Msg = &H304 Then Debug.WriteLine("WM_UNDO")
            'If m.Msg = &H125 Then Debug.WriteLine("WM_UNINITMENUPOPUP")
            'If m.Msg = &H128 Then Debug.WriteLine("WM_UPDATEUISTATE")
            'If m.Msg = &H400 Then Debug.WriteLine("WM_USER")
            'If m.Msg = &H54 Then Debug.WriteLine("WM_USERCHANGED")
            'If m.Msg = &H2E Then Debug.WriteLine("WM_VKEYTOITEM")
            'If m.Msg = &H115 Then Debug.WriteLine("WM_VSCROLL")
            'If m.Msg = &H30A Then Debug.WriteLine("WM_VSCROLLCLIPBOARD")
            'If m.Msg = &H47 Then Debug.WriteLine("WM_WINDOWPOSCHANGED")
            'If m.Msg = 70 Then Debug.WriteLine("WM_WINDOWPOSCHANGING")
            'If m.Msg = &H1A Then Debug.WriteLine("WM_WININICHANGE")
            'If m.Msg = &H2B1 Then Debug.WriteLine("WM_WTSSESSION_CHANGE")
            'If m.Msg = &H20D Then Debug.WriteLine("WM_XBUTTONDBLCLK")
            'If m.Msg = &H20B Then Debug.WriteLine("WM_XBUTTONDOWN")
            'If m.Msg = &H20C Then Debug.WriteLine("WM_XBUTTONUP")
#End Region
            Snap(m)
            MyBase.WndProc(m)
        End Sub

        Private IsResizing As Boolean

        Sub Snap(ByRef m As Message)
            Select Case m.Msg
                Case &H214 'WM_SIZING
                    IsResizing = True
                Case &H232 'WM_EXITSIZEMOVE
                    IsResizing = False
                Case &H46 'WM_WINDOWPOSCHANGING
                    If Not IsResizing Then Snap(m.LParam)
            End Select
        End Sub

        Sub Snap(handle As IntPtr)
            If Not s?.SnapToDesktopEdges Then Exit Sub

            Dim workingArea = Screen.FromControl(Me).WorkingArea
            Dim newPos = DirectCast(Marshal.PtrToStructure(handle, GetType(WindowPos)), WindowPos)
            Dim snapMargin = Control.DefaultFont.Height
            Dim border As Integer

            If OSVersion.Current >= OSVersion.Windows8 Then
                border = (Width - ClientSize.Width) \ 2 - 1
            End If

            If newPos.Y <> 0 Then
                If Math.Abs(newPos.Y - workingArea.Y) < snapMargin AndAlso Top > newPos.Y Then
                    newPos.Y = workingArea.Y
                ElseIf Math.Abs(newPos.Y + Height - (workingArea.Bottom + border)) < snapMargin AndAlso Top < newPos.Y Then
                    newPos.Y = (workingArea.Bottom + border) - Height
                End If
            End If

            If newPos.X <> 0 Then
                If Math.Abs(newPos.X - (workingArea.X - border)) < snapMargin AndAlso Left > newPos.X Then
                    newPos.X = workingArea.X - border
                ElseIf Math.Abs(newPos.X + Width - (workingArea.Right + border)) < snapMargin AndAlso Left < newPos.X Then
                    newPos.X = (workingArea.Right + border) - Width
                End If
            End If

            Marshal.StructureToPtr(newPos, handle, True)
        End Sub

        <StructLayout(LayoutKind.Sequential)>
        Structure WindowPos
            Public Hwnd As IntPtr
            Public HwndInsertAfter As IntPtr
            Public X As Integer
            Public Y As Integer
            Public Width As Integer
            Public Height As Integer
            Public Flags As Integer
        End Structure
    End Class

    Public Class DialogBase
        Inherits FormBase

        Sub New()
            FormBorderStyle = FormBorderStyle.FixedDialog
            HelpButton = True
            MaximizeBox = False
            MinimizeBox = False
            ShowIcon = False
            ShowInTaskbar = False
            StartPosition = FormStartPosition.CenterParent
        End Sub

        Protected Overrides Sub OnHelpButtonClicked(e As CancelEventArgs)
            e.Cancel = True
            MyBase.OnHelpButtonClicked(e)
            OnHelpRequested(New HelpEventArgs(MousePosition))
        End Sub
    End Class

    Class ListBag(Of T)
        Implements IComparable(Of ListBag(Of T))

        Property Text As String
        Property Value As T

        Sub New(text As String, value As T)
            Me.Text = text
            Me.Value = value
        End Sub

        Shared Sub SelectItem(cb As ComboBox, value As T)
            Dim selectItem As Object = Nothing

            For Each i As ListBag(Of T) In cb.Items
                If i.Value.Equals(value) Then selectItem = i
            Next

            If Not selectItem Is Nothing Then cb.SelectedItem = selectItem
        End Sub

        Shared Function GetValue(cb As ComboBox) As T
            Return DirectCast(DirectCast(cb.SelectedItem, ListBag(Of T)).Value, T)
        End Function

        Shared Function GetBagsForEnumType() As ListBag(Of T)()
            Dim ret As New List(Of ListBag(Of T))

            For Each i As T In System.Enum.GetValues(GetType(T))
                ret.Add(New ListBag(Of T)(DispNameAttribute.GetValueForEnum(i), i))
            Next

            Return ret.ToArray
        End Function

        Overrides Function ToString() As String
            Return Text
        End Function

        Function CompareTo(other As ListBag(Of T)) As Integer Implements IComparable(Of ListBag(Of T)).CompareTo
            Return Text.CompareTo(other.Text)
        End Function
    End Class

    <Serializable()>
    Public Class WindowPositions
        Private Positions As New Dictionary(Of String, Point)
        Private WindowStates As New Dictionary(Of String, FormWindowState)

        Sub Save(f As Form)
            SavePosition(f)
            SaveWindowState(f)
        End Sub

        Private Sub SavePosition(f As Form)
            If f.WindowState = FormWindowState.Normal Then
                Positions(GetKey(f)) = f.Location
            End If
        End Sub

        Private Sub SaveWindowState(f As Form)
            WindowStates(GetKey(f)) = f.WindowState
        End Sub

        Private Sub RestorePositionInternal(form As Form)
            If Positions.ContainsKey(GetKey(form)) Then
                form.StartPosition = FormStartPosition.Manual
                Dim pos = Positions(GetKey(form))
                Dim workingArea = Screen.FromControl(form).WorkingArea
                If pos.X < workingArea.X Then pos.X = workingArea.X
                If pos.Y < workingArea.Y Then pos.Y = workingArea.Y
                If pos.X + form.Width > workingArea.Right Then pos.X = workingArea.Right - form.Width
                If pos.Y + form.Height > workingArea.Bottom Then pos.Y = workingArea.Bottom - form.Height
                form.Location = pos
            End If
        End Sub

        Private Sub RestoreWindowState(f As Form)
            If WindowStates.ContainsKey(GetKey(f)) Then f.WindowState = WindowStates(GetKey(f))
        End Sub

        Sub CenterScreen(form As Form)
            form.StartPosition = FormStartPosition.Manual
            Dim wa = Screen.FromControl(form).WorkingArea
            form.Left = (wa.Width - form.Width) \ 2
            form.Top = (wa.Height - form.Height) \ 2
        End Sub

        Sub RestorePosition(form As Form)
            Dim v = GetText(form)

            If Not s.WindowPositionsCenterScreen.NothingOrEmpty AndAlso
                Not TypeOf form Is InputBoxForm Then

                For Each i In s.WindowPositionsCenterScreen
                    If v.StartsWith(i) OrElse i = "all" Then
                        CenterScreen(form)
                        Exit For
                    End If
                Next
            End If

            If Not s.WindowPositionsRemembered.NothingOrEmpty AndAlso
                Not TypeOf form Is InputBoxForm Then

                For Each i In s.WindowPositionsRemembered
                    If v.StartsWith(i) OrElse i = "all" Then
                        RestorePositionInternal(form)
                        Exit For
                    End If
                Next
            End If
        End Sub

        Private Function GetKey(f As Form) As String
            Return f.Name + f.GetType().FullName + GetText(f)
        End Function

        Function GetText(f As Form) As String
            Return If(TypeOf f Is HelpForm, "Help", f.Text)
        End Function
    End Class

    Class OpenFileDialogEditor
        Inherits UITypeEditor

        Overloads Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
            Using f As New OpenFileDialog
                If f.ShowDialog = DialogResult.OK Then
                    Return f.FileName
                Else
                    Return value
                End If
            End Using
        End Function

        Overloads Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.Modal
        End Function
    End Class

    Class StringEditor
        Inherits UITypeEditor

        Sub New()
        End Sub

        Overloads Overrides Function EditValue(context As ITypeDescriptorContext, provider As IServiceProvider, value As Object) As Object
            Dim form As New StringEditorForm
            form.rtb.Text = DirectCast(value, String)

            If form.ShowDialog() = DialogResult.OK Then
                Return form.rtb.Text
            Else
                Return value
            End If
        End Function

        Overloads Overrides Function GetEditStyle(context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.Modal
        End Function
    End Class

    Class DesignHelp
        Private Shared IsDesignModeValue As Boolean?

        Shared ReadOnly Property IsDesignMode As Boolean
            Get
                If Not IsDesignModeValue.HasValue Then
                    IsDesignModeValue = Process.GetCurrentProcess.ProcessName = "devenv"
                End If

                Return IsDesignModeValue.Value
            End Get
        End Property
    End Class
End Namespace