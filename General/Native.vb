Imports System.Runtime.InteropServices
Imports System.Text
Imports System.IO

Public Class Native
    Public Delegate Function CallbackHandler(handle As IntPtr, parameter As Integer) As Boolean

    <DllImport("gdi32.dll")>
    Public Shared Function ExcludeClipRect(hdc As IntPtr, nLeftRect As Integer, nTopRect As Integer, nRightRect As Integer, nBottomRect As Integer) As Integer
    End Function

#Region "Constants"

    'Public Const WM_ACTIVATE As Integer = 6
    'Public Const WM_ACTIVATEAPP As Integer = &H1C
    'Public Const WM_AFXFIRST As Integer = &H360
    'Public Const WM_AFXLAST As Integer = &H37F
    'Public Const WM_APP As Integer = &H8000
    'Public Const WM_APPCOMMAND As Integer = &H319
    'Public Const WM_ASKCBFORMATNAME As Integer = 780
    'Public Const WM_CANCELJOURNAL As Integer = &H4B
    'Public Const WM_CANCELMODE As Integer = &H1F
    'Public Const WM_CAPTURECHANGED As Integer = &H215
    'Public Const WM_CHANGECBCHAIN As Integer = &H30D
    'Public Const WM_CHANGEUISTATE As Integer = &H127
    'Public Const WM_CHAR As Integer = &H102
    'Public Const WM_CHARTOITEM As Integer = &H2F
    'Public Const WM_CHILDACTIVATE As Integer = &H22
    'Public Const WM_CHOOSEFONT_GETLOGFONT As Integer = &H401
    'Public Const WM_CLEAR As Integer = &H303
    Public Const WM_CLOSE As Integer = &H10
    'Public Const WM_COMMAND As Integer = &H111
    'Public Const WM_COMMNOTIFY As Integer = &H44
    'Public Const WM_COMPACTING As Integer = &H41
    'Public Const WM_COMPAREITEM As Integer = &H39
    'Public Const WM_CONTEXTMENU As Integer = &H7B
    'Public Const WM_COPY As Integer = &H301
    'Public Const WM_COPYDATA As Integer = &H4A
    'Public Const WM_CREATE As Integer = 1
    'Public Const WM_CTLCOLOR As Integer = &H19
    'Public Const WM_CTLCOLORBTN As Integer = &H135
    'Public Const WM_CTLCOLORDLG As Integer = 310
    'Public Const WM_CTLCOLOREDIT As Integer = &H133
    'Public Const WM_CTLCOLORLISTBOX As Integer = &H134
    'Public Const WM_CTLCOLORMSGBOX As Integer = &H132
    'Public Const WM_CTLCOLORSCROLLBAR As Integer = &H137
    'Public Const WM_CTLCOLORSTATIC As Integer = &H138
    'Public Const WM_CUT As Integer = &H300
    'Public Const WM_DEADCHAR As Integer = &H103
    'Public Const WM_DELETEITEM As Integer = &H2D
    'Public Const WM_DESTROY As Integer = 2
    'Public Const WM_DESTROYCLIPBOARD As Integer = &H307
    'Public Const WM_DEVICECHANGE As Integer = &H219
    'Public Const WM_DEVMODECHANGE As Integer = &H1B
    'Public Const WM_DISPLAYCHANGE As Integer = &H7E
    'Public Const WM_DRAWCLIPBOARD As Integer = &H308
    'Public Const WM_DRAWITEM As Integer = &H2B
    'Public Const WM_DROPFILES As Integer = &H233
    'Public Const WM_DWMCOMPOSITIONCHANGED As Integer = &H31E
    'Public Const WM_ENABLE As Integer = 10
    'Public Const WM_ENDSESSION As Integer = &H16
    'Public Const WM_ENTERIDLE As Integer = &H121
    'Public Const WM_ENTERMENULOOP As Integer = &H211
    'Public Const WM_ENTERSIZEMOVE As Integer = &H231
    Public Const WM_ERASEBKGND As Integer = 20
    'Public Const WM_EXITMENULOOP As Integer = 530
    'Public Const WM_EXITSIZEMOVE As Integer = &H232
    'Public Const WM_FLICK As Integer = &H2CB
    'Public Const WM_FONTCHANGE As Integer = &H1D
    'Public Const WM_GETDLGCODE As Integer = &H87
    'Public Const WM_GETFONT As Integer = &H31
    'Public Const WM_GETHOTKEY As Integer = &H33
    'Public Const WM_GETICON As Integer = &H7F
    'Public Const WM_GETMINMAXINFO As Integer = &H24
    'Public Const WM_GETOBJECT As Integer = &H3D
    Public Const WM_GETTEXT As Integer = 13
    'Public Const WM_GETTEXTLENGTH As Integer = 14
    'Public Const WM_HANDHELDFIRST As Integer = &H358
    'Public Const WM_HANDHELDLAST As Integer = &H35F
    'Public Const WM_HELP As Integer = &H53
    'Public Const WM_HOTKEY As Integer = &H312
    Public Const WM_HSCROLL As Integer = &H114
    'Public Const WM_HSCROLLCLIPBOARD As Integer = &H30E
    'Public Const WM_ICONERASEBKGND As Integer = &H27
    'Public Const WM_IME_CHAR As Integer = &H286
    'Public Const WM_IME_COMPOSITION As Integer = &H10F
    'Public Const WM_IME_COMPOSITIONFULL As Integer = &H284
    'Public Const WM_IME_CONTROL As Integer = &H283
    'Public Const WM_IME_ENDCOMPOSITION As Integer = 270
    'Public Const WM_IME_KEYDOWN As Integer = &H290
    'Public Const WM_IME_KEYLAST As Integer = &H10F
    'Public Const WM_IME_KEYUP As Integer = &H291
    'Public Const WM_IME_NOTIFY As Integer = &H282
    'Public Const WM_IME_REQUEST As Integer = &H288
    'Public Const WM_IME_SELECT As Integer = &H285
    'Public Const WM_IME_SETCONTEXT As Integer = &H281
    'Public Const WM_IME_STARTCOMPOSITION As Integer = &H10D
    'Public Const WM_INITDIALOG As Integer = &H110
    'Public Const WM_INITMENU As Integer = &H116
    'Public Const WM_INITMENUPOPUP As Integer = &H117
    'Public Const WM_INPUT As Integer = &HFF
    'Public Const WM_INPUTLANGCHANGE As Integer = &H51
    'Public Const WM_INPUTLANGCHANGEREQUEST As Integer = 80
    'Public Const WM_KEYDOWN As Integer = &H100
    'Public Const WM_KEYFIRST As Integer = &H100
    'Public Const WM_KEYLAST As Integer = &H108
    Public Const WM_KEYUP As Integer = &H101
    'Public Const WM_KILLFOCUS As Integer = 8
    Public Const WM_LBUTTONDBLCLK As Integer = &H203
    Public Const WM_LBUTTONDOWN As Integer = &H201
    Public Const WM_LBUTTONUP As Integer = &H202
    'Public Const WM_MBUTTONDBLCLK As Integer = &H209
    'Public Const WM_MBUTTONDOWN As Integer = &H207
    'Public Const WM_MBUTTONUP As Integer = 520
    'Public Const WM_MDIACTIVATE As Integer = &H222
    'Public Const WM_MDICASCADE As Integer = &H227
    'Public Const WM_MDICREATE As Integer = &H220
    'Public Const WM_MDIDESTROY As Integer = &H221
    'Public Const WM_MDIGETACTIVE As Integer = &H229
    'Public Const WM_MDIICONARRANGE As Integer = &H228
    'Public Const WM_MDIMAXIMIZE As Integer = &H225
    'Public Const WM_MDINEXT As Integer = &H224
    'Public Const WM_MDIREFRESHMENU As Integer = &H234
    'Public Const WM_MDIRESTORE As Integer = &H223
    'Public Const WM_MDISETMENU As Integer = 560
    'Public Const WM_MDITILE As Integer = 550
    'Public Const WM_MEASUREITEM As Integer = &H2C
    'Public Const WM_MENUCHAR As Integer = &H120
    'Public Const WM_MENUSELECT As Integer = &H11F
    'Public Const WM_MOUSEACTIVATE As Integer = &H21
    'Public Const WM_MOUSEFIRST As Integer = &H200
    'Public Const WM_MOUSEHOVER As Integer = &H2A1
    'Public Const WM_MOUSELAST As Integer = &H20A
    'Public Const WM_MOUSELEAVE As Integer = &H2A3
    'Public Const WM_MOUSEMOVE As Integer = &H200
    'Public Const WM_MOUSEQUERY As Integer = &H9B
    'Public Const WM_MOUSEWHEEL As Integer = &H20A
    'Public Const WM_MOVE As Integer = 3
    'Public Const WM_MOVING As Integer = &H216
    Public Const WM_NCACTIVATE As Integer = &H86
    'Public Const WM_NCCALCSIZE As Integer = &H83
    'Public Const WM_NCCREATE As Integer = &H81
    'Public Const WM_NCDESTROY As Integer = 130
    'Public Const WM_NCHITTEST As Integer = &H84
    'Public Const WM_NCLBUTTONDBLCLK As Integer = &HA3
    Public Const WM_NCLBUTTONDOWN As Integer = &HA1
    'Public Const WM_NCLBUTTONUP As Integer = &HA2
    'Public Const WM_NCMBUTTONDBLCLK As Integer = &HA9
    'Public Const WM_NCMBUTTONDOWN As Integer = &HA7
    'Public Const WM_NCMBUTTONUP As Integer = &HA8
    'Public Const WM_NCMOUSELEAVE As Integer = &H2A2
    'Public Const WM_NCMOUSEMOVE As Integer = 160
    Public Const WM_NCPAINT As Integer = &H85
    'Public Const WM_NCRBUTTONDBLCLK As Integer = &HA6
    'Public Const WM_NCRBUTTONDOWN As Integer = &HA4
    'Public Const WM_NCRBUTTONUP As Integer = &HA5
    'Public Const WM_NCXBUTTONDBLCLK As Integer = &HAD
    'Public Const WM_NCXBUTTONDOWN As Integer = &HAB
    'Public Const WM_NCXBUTTONUP As Integer = &HAC
    'Public Const WM_NEXTDLGCTL As Integer = 40
    'Public Const WM_NEXTMENU As Integer = &H213
    Public Const WM_NOTIFY As Integer = &H4E
    'Public Const WM_NOTIFYFORMAT As Integer = &H55
    'Public Const WM_NULL As Integer = 0
    Public Const WM_PAINT As Integer = 15
    'Public Const WM_PAINTCLIPBOARD As Integer = &H309
    'Public Const WM_PAINTICON As Integer = &H26
    'Public Const WM_PALETTECHANGED As Integer = &H311
    'Public Const WM_PALETTEISCHANGING As Integer = &H310
    'Public Const WM_PARENTNOTIFY As Integer = &H210
    'Public Const WM_PASTE As Integer = 770
    'Public Const WM_PENWINFIRST As Integer = &H380
    'Public Const WM_PENWINLAST As Integer = &H38F
    'Public Const WM_POWER As Integer = &H48
    'Public Const WM_POWERBROADCAST As Integer = &H218
    'Public Const WM_PRINT As Integer = &H317
    'Public Const WM_PRINTCLIENT As Integer = &H318
    'Public Const WM_QUERYDRAGICON As Integer = &H37
    'Public Const WM_QUERYENDSESSION As Integer = &H11
    'Public Const WM_QUERYNEWPALETTE As Integer = &H30F
    'Public Const WM_QUERYOPEN As Integer = &H13
    'Public Const WM_QUERYSYSTEMGESTURESTATUS As Integer = &H2CC
    'Public Const WM_QUERYUISTATE As Integer = &H129
    'Public Const WM_QUEUESYNC As Integer = &H23
    'Public Const WM_QUIT As Integer = &H12
    'Public Const WM_RBUTTONDBLCLK As Integer = &H206
    'Public Const WM_RBUTTONDOWN As Integer = &H204
    Public Const WM_RBUTTONUP As Integer = &H205
    Public Const WM_REFLECT As Integer = &H2000
    'Public Const WM_RENDERALLFORMATS As Integer = &H306
    'Public Const WM_RENDERFORMAT As Integer = &H305
    'Public Const WM_SETCURSOR As Integer = &H20
    'Public Const WM_SETFOCUS As Integer = 7
    'Public Const WM_SETFONT As Integer = &H30
    'Public Const WM_SETHOTKEY As Integer = 50
    'Public Const WM_SETICON As Integer = &H80
    'Public Const WM_SETREDRAW As Integer = 11
    'Public Const WM_SETTEXT As Integer = 12
    Public Const WM_SETTINGCHANGE As Integer = &H1A
    'Public Const WM_SHOWWINDOW As Integer = &H18
    'Public Const WM_SIZE As Integer = 5
    'Public Const WM_SIZECLIPBOARD As Integer = &H30B
    'Public Const WM_SIZING As Integer = &H214
    'Public Const WM_SPOOLERSTATUS As Integer = &H2A
    'Public Const WM_STYLECHANGED As Integer = &H7D
    'Public Const WM_STYLECHANGING As Integer = &H7C
    'Public Const WM_SYSCHAR As Integer = &H106
    'Public Const WM_SYSCOLORCHANGE As Integer = &H15
    Public Const WM_SYSCOMMAND As Integer = &H112
    'Public Const WM_SYSDEADCHAR As Integer = &H107
    'Public Const WM_SYSKEYDOWN As Integer = 260
    'Public Const WM_SYSKEYUP As Integer = &H105
    'Public Const WM_TABLET_ADDED As Integer = &H2C8
    'Public Const WM_TABLET_REMOVED As Integer = &H2C9
    'Public Const WM_TCARD As Integer = &H52
    'Public Const WM_THEMECHANGED As Integer = &H31A
    'Public Const WM_TIMECHANGE As Integer = 30
    'Public Const WM_TIMER As Integer = &H113
    'Public Const WM_UNDO As Integer = &H304
    'Public Const WM_UNINITMENUPOPUP As Integer = &H125
    'Public Const WM_UPDATEUISTATE As Integer = &H128
    Public Const WM_USER As Integer = &H400
    'Public Const WM_USERCHANGED As Integer = &H54
    'Public Const WM_VKEYTOITEM As Integer = &H2E
    Public Const WM_VSCROLL As Integer = &H115
    'Public Const WM_VSCROLLCLIPBOARD As Integer = &H30A
    'Public Const WM_WINDOWPOSCHANGED As Integer = &H47
    'Public Const WM_WINDOWPOSCHANGING As Integer = 70
    'Public Const WM_WININICHANGE As Integer = &H1A
    'Public Const WM_WTSSESSION_CHANGE As Integer = &H2B1
    'Public Const WM_XBUTTONDBLCLK As Integer = &H20D
    'Public Const WM_XBUTTONDOWN As Integer = &H20B
    'Public Const WM_XBUTTONUP As Integer = &H20C

    'Public Const WS_BORDER As Integer = &H800000
    'Public Const WS_CAPTION As Integer = &HC00000
    'Public Const WS_CHILD As Integer = &H40000000
    'Public Const WS_CLIPCHILDREN As Integer = &H2000000
    'Public Const WS_CLIPSIBLINGS As Integer = &H4000000
    'Public Const WS_DISABLED As Integer = &H8000000
    'Public Const WS_DLGFRAME As Integer = &H400000
    'Public Const WS_EX_APPWINDOW As Integer = &H40000
    Public Const WS_EX_CLIENTEDGE As Integer = &H200
    'Public Const WS_EX_COMPOSITED As Integer = &H2000000
    'Public Const WS_EX_CONTEXTHELP As Integer = &H400
    'Public Const WS_EX_CONTROLPARENT As Integer = &H10000
    'Public Const WS_EX_DLGMODALFRAME As Integer = 1
    'Public Const WS_EX_LAYERED As Integer = &H80000
    'Public Const WS_EX_LAYOUTRTL As Integer = &H400000
    'Public Const WS_EX_LEFT As Integer = 0
    'Public Const WS_EX_LEFTSCROLLBAR As Integer = &H4000
    'Public Const WS_EX_MDICHILD As Integer = &H40
    'Public Const WS_EX_NOACTIVATE As Integer = &H8000000
    'Public Const WS_EX_NOINHERITLAYOUT As Integer = &H100000
    'Public Const WS_EX_RIGHT As Integer = &H1000
    'Public Const WS_EX_RTLREADING As Integer = &H2000
    'Public Const WS_EX_STATICEDGE As Integer = &H20000
    'Public Const WS_EX_TOOLWINDOW As Integer = &H80
    'Public Const WS_EX_TOPMOST As Integer = 8
    'Public Const WS_EX_TRANSPARENT As Integer = &H20
    'Public Const WS_EX_WINDOWEDGE As Integer = &H100
    'Public Const WS_HSCROLL As Integer = &H100000
    'Public Const WS_MAXIMIZE As Integer = &H1000000
    'Public Const WS_MAXIMIZEBOX As Integer = &H10000
    'Public Const WS_MINIMIZE As Integer = &H20000000
    'Public Const WS_MINIMIZEBOX As Integer = &H20000
    'Public Const WS_OVERLAPPED As Integer = 0
    'Public Const WS_OVERLAPPEDWINDOW As Integer = &HCF0000
    'Public Const WS_POPUP As Integer = -2147483648
    'Public Const WS_SYSMENU As Integer = &H80000
    'Public Const WS_TABSTOP As Integer = &H10000
    'Public Const WS_THICKFRAME As Integer = &H40000
    'Public Const WS_VISIBLE As Integer = &H10000000
    Public Const WS_VSCROLL As Integer = &H200000

    Public Const ASSOCSTR_EXECUTABLE As Integer = 2

    Public Const ASSOCF_VERIFY As Integer = &H40

    Public Const EM_SETCUEBANNER As Integer = &H1501
    Public Const CB_SETCUEBANNER As Integer = &H1703

    'Public Const MAPVK_VK_TO_VSC As Integer = 0
    'Public Const MAPVK_VSC_TO_VK As Integer = 1
    Public Const MAPVK_VK_TO_CHAR As Integer = 2
    'Public Const MAPVK_VSC_TO_VK_EX As Integer = 3
    'Public Const MAPVK_VK_TO_VSC_EX As Integer = 4

    Public Const SC_CLOSE As Integer = &HF060
    'Public Const SC_KEYMENU As Integer = &HF100
    'Public Const SC_MAXIMIZE As Integer = &HF030
    Public Const SC_MINIMIZE As Integer = &HF020
    'Public Const SC_MOUSEMOVE As Integer = &HF012
    'Public Const SC_MOVE As Integer = &HF010
    'Public Const SC_RESTORE As Integer = &HF120
    'Public Const SC_SIZE As Integer = &HF000

    'Public Const SW_ERASE As Integer = 4
    Public Const SW_HIDE As Integer = 0
    'Public Const SW_INVALIDATE As Integer = 2
    'Public Const SW_MAX As Integer = 10
    'Public Const SW_MAXIMIZE As Integer = 3
    'Public Const SW_MINIMIZE As Integer = 6
    'Public Const SW_NORMAL As Integer = 1
    'Public Const SW_PARENTCLOSING As Integer = 1
    'Public Const SW_PARENTOPENING As Integer = 3
    Public Const SW_RESTORE As Integer = 9
    'Public Const SW_SCROLLCHILDREN As Integer = 1
    'Public Const SW_SHOW As Integer = 5
    'Public Const SW_SHOWMAXIMIZED As Integer = 3
    'Public Const SW_SHOWMINIMIZED As Integer = 2
    'Public Const SW_SHOWMINNOACTIVE As Integer = 7
    'Public Const SW_SHOWNA As Integer = 8
    'Public Const SW_SHOWNOACTIVATE As Integer = 4
    'Public Const SW_SMOOTHSCROLL As Integer = &H10

    Public Const GWL_STYLE As Integer = -16

#End Region

#Region "Function"

#Region "user32.dll"
    <DllImport("user32.dll", SetLastError:=True)>
    Shared Function SetWindowPos(hWnd As IntPtr,
                                 hWndInsertAfter As IntPtr,
                                 X As Integer,
                                 Y As Integer,
                                 cx As Integer,
                                 cy As Integer,
                                 uFlags As UInteger) As Boolean
    End Function

    <DllImport("user32")>
    Shared Function GetWindowLong(hWnd As IntPtr, nIndex As Integer) As Integer
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function RegisterWindowMessage(id As String) As Integer
    End Function

    <DllImport("user32.dll")>
    Shared Function RegisterHotKey(hWnd As IntPtr, id As Integer, fsModifiers As Integer, vk As Integer) As Boolean
    End Function

    <DllImport("user32.dll")>
    Shared Function MapVirtualKey(wCode As Integer, wMapType As Integer) As Integer
    End Function

    <DllImport("user32.dll")>
    Shared Function UnregisterHotKey(hWnd As IntPtr, id As Integer) As Boolean
    End Function

    <DllImport("user32.dll")>
    Shared Function GetForegroundWindow() As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True)>
    Shared Function GetWindowThreadProcessId(hwnd As IntPtr, ByRef lpdwProcessId As Integer) As Integer
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function SetWindowText(hWnd As IntPtr, lpString As String) As Boolean
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function GetWindowText(hWnd As IntPtr, lpString As StringBuilder, nMaxCount As Integer) As Integer
    End Function

    <DllImport("user32.dll")>
    Shared Function ShowWindow(handle As IntPtr, nCmdShow As Integer) As Integer
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function FindWindow(lpClassName As String, lpWindowName As String) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Shared Function SetForegroundWindow(handle As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll")>
    Shared Function EnumWindows(ewp As CallbackHandler, ByVallParam As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll")>
    Shared Function EnumChildWindows(hwndParent As IntPtr,
                                     ByVallpEnumProc As CallbackHandler,
                                     parameter As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function GetWindowModuleFileName(hwnd As IntPtr,
                                            lpszFileName As StringBuilder,
                                            cchFileNameMax As UInteger) As UInteger
    End Function

    <DllImport("user32.dll")>
    Shared Function SendMessage(handle As IntPtr,
                                message As Int32,
                                wParam As IntPtr,
                                lParam As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function SendMessage(hWnd As IntPtr,
                                Msg As Int32,
                                wParam As IntPtr,
                                lParam As String) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function SendMessage(hWnd As IntPtr,
                                Msg As Int32,
                                wParam As Integer,
                                lParam As Integer) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function SendMessage(hWnd As IntPtr,
                                Msg As Int32,
                                wParam As Integer,
                                lParam As String) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function SendMessage(hWnd As IntPtr,
                                Msg As Int32,
                                ByRef wParam As IntPtr,
                                lParam As StringBuilder) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Shared Function SendMessageTimeout(windowHandle As IntPtr,
                                       msg As Integer,
                                       wParam As IntPtr,
                                       lParam As IntPtr,
                                       flags As Integer,
                                       timeout As Integer,
                                       ByRef result As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function PostMessage(hwnd As IntPtr,
                                wMsg As Integer,
                                wParam As IntPtr,
                                lParam As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Shared Sub ReleaseCapture()
    End Sub

    <DllImport("user32.dll")>
    Public Shared Function GetWindowRect(hWnd As IntPtr, ByRef lpRect As RECT) As Boolean
    End Function

    <DllImport("user32.dll")>
    Public Shared Function GetWindowDC(hWnd As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Public Shared Function ReleaseDC(hWnd As IntPtr, hDC As IntPtr) As Integer
    End Function

#End Region

    <DllImport("kernel32.dll", CharSet:=CharSet.Unicode)>
    Shared Function LoadLibrary(path As String) As IntPtr
    End Function

    <DllImport("uxtheme.dll", CharSet:=CharSet.Unicode)>
    Shared Function SetWindowTheme(hWnd As IntPtr,
                                   pszSubAppName As String,
                                   pszSubIdList As String) As Integer
    End Function

    <DllImport("Shlwapi.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
    Shared Function AssocQueryString(
        flags As UInteger,
        str As UInteger,
        pszAssoc As String,
        pszExtra As String,
        pszOut As StringBuilder,
        ByRef pcchOut As UInteger) As UInteger
    End Function

#End Region

#Region "Structures"

    Public Structure RECT
        Public Left As Integer
        Public Top As Integer
        Public Right As Integer
        Public Bottom As Integer

        Sub New(r As Rectangle)
            Left = r.Left
            Top = r.Top
            Right = r.Right
            Bottom = r.Bottom
        End Sub

        Public Sub New(left As Integer, top As Integer, right As Integer, bottom As Integer)
            Me.Left = left
            Me.Top = top
            Me.Right = right
            Me.Bottom = bottom
        End Sub

        Function ToRectangle() As Rectangle
            Return Rectangle.FromLTRB(Left, Top, Right, Bottom)
        End Function
    End Structure

    Public Structure SHFILEINFO
        Public hIcon As IntPtr
        Public iIcon As Integer
        Public dwAttributes As Integer
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)>
        Public szDisplayName As String
        <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=80)>
        Public szTypeName As String
    End Structure

    Public Structure NMHDR
        Public hwndFrom As Integer
        Public idFrom As Integer
        Public code As Integer
    End Structure

    Public Structure NCCALCSIZE_PARAMS
        Public rgrc0, rgrc1, rgrc2 As RECT
        Public lppos As IntPtr
    End Structure

#End Region

End Class