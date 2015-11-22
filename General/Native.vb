Imports System.Runtime.InteropServices
Imports System.Text
Imports System.IO

Public Class Native
    Public Delegate Function CallbackHandler(handle As IntPtr, parameter As Integer) As Boolean

    <DllImport("gdi32.dll")>
    Public Shared Function ExcludeClipRect(hdc As IntPtr, nLeftRect As Integer, nTopRect As Integer, nRightRect As Integer, nBottomRect As Integer) As Integer
    End Function

#Region "Constants"

    'Friend Const WM_ACTIVATE As Integer = 6
    'Friend Const WM_ACTIVATEAPP As Integer = &H1C
    'Friend Const WM_AFXFIRST As Integer = &H360
    'Friend Const WM_AFXLAST As Integer = &H37F
    'Friend Const WM_APP As Integer = &H8000
    'Friend Const WM_APPCOMMAND As Integer = &H319
    'Friend Const WM_ASKCBFORMATNAME As Integer = 780
    'Friend Const WM_CANCELJOURNAL As Integer = &H4B
    'Friend Const WM_CANCELMODE As Integer = &H1F
    'Friend Const WM_CAPTURECHANGED As Integer = &H215
    'Friend Const WM_CHANGECBCHAIN As Integer = &H30D
    'Friend Const WM_CHANGEUISTATE As Integer = &H127
    'Friend Const WM_CHAR As Integer = &H102
    'Friend Const WM_CHARTOITEM As Integer = &H2F
    'Friend Const WM_CHILDACTIVATE As Integer = &H22
    'Friend Const WM_CHOOSEFONT_GETLOGFONT As Integer = &H401
    'Friend Const WM_CLEAR As Integer = &H303
    Friend Const WM_CLOSE As Integer = &H10
    'Friend Const WM_COMMAND As Integer = &H111
    'Friend Const WM_COMMNOTIFY As Integer = &H44
    'Friend Const WM_COMPACTING As Integer = &H41
    'Friend Const WM_COMPAREITEM As Integer = &H39
    'Friend Const WM_CONTEXTMENU As Integer = &H7B
    'Friend Const WM_COPY As Integer = &H301
    'Friend Const WM_COPYDATA As Integer = &H4A
    'Friend Const WM_CREATE As Integer = 1
    'Friend Const WM_CTLCOLOR As Integer = &H19
    'Friend Const WM_CTLCOLORBTN As Integer = &H135
    'Friend Const WM_CTLCOLORDLG As Integer = 310
    'Friend Const WM_CTLCOLOREDIT As Integer = &H133
    'Friend Const WM_CTLCOLORLISTBOX As Integer = &H134
    'Friend Const WM_CTLCOLORMSGBOX As Integer = &H132
    'Friend Const WM_CTLCOLORSCROLLBAR As Integer = &H137
    'Friend Const WM_CTLCOLORSTATIC As Integer = &H138
    'Friend Const WM_CUT As Integer = &H300
    'Friend Const WM_DEADCHAR As Integer = &H103
    'Friend Const WM_DELETEITEM As Integer = &H2D
    'Friend Const WM_DESTROY As Integer = 2
    'Friend Const WM_DESTROYCLIPBOARD As Integer = &H307
    'Friend Const WM_DEVICECHANGE As Integer = &H219
    'Friend Const WM_DEVMODECHANGE As Integer = &H1B
    'Friend Const WM_DISPLAYCHANGE As Integer = &H7E
    'Friend Const WM_DRAWCLIPBOARD As Integer = &H308
    'Friend Const WM_DRAWITEM As Integer = &H2B
    'Friend Const WM_DROPFILES As Integer = &H233
    'Friend Const WM_DWMCOMPOSITIONCHANGED As Integer = &H31E
    'Friend Const WM_ENABLE As Integer = 10
    'Friend Const WM_ENDSESSION As Integer = &H16
    'Friend Const WM_ENTERIDLE As Integer = &H121
    'Friend Const WM_ENTERMENULOOP As Integer = &H211
    'Friend Const WM_ENTERSIZEMOVE As Integer = &H231
    Friend Const WM_ERASEBKGND As Integer = 20
    'Friend Const WM_EXITMENULOOP As Integer = 530
    'Friend Const WM_EXITSIZEMOVE As Integer = &H232
    'Friend Const WM_FLICK As Integer = &H2CB
    'Friend Const WM_FONTCHANGE As Integer = &H1D
    'Friend Const WM_GETDLGCODE As Integer = &H87
    'Friend Const WM_GETFONT As Integer = &H31
    'Friend Const WM_GETHOTKEY As Integer = &H33
    'Friend Const WM_GETICON As Integer = &H7F
    'Friend Const WM_GETMINMAXINFO As Integer = &H24
    'Friend Const WM_GETOBJECT As Integer = &H3D
    Friend Const WM_GETTEXT As Integer = 13
    'Friend Const WM_GETTEXTLENGTH As Integer = 14
    'Friend Const WM_HANDHELDFIRST As Integer = &H358
    'Friend Const WM_HANDHELDLAST As Integer = &H35F
    'Friend Const WM_HELP As Integer = &H53
    'Friend Const WM_HOTKEY As Integer = &H312
    Friend Const WM_HSCROLL As Integer = &H114
    'Friend Const WM_HSCROLLCLIPBOARD As Integer = &H30E
    'Friend Const WM_ICONERASEBKGND As Integer = &H27
    'Friend Const WM_IME_CHAR As Integer = &H286
    'Friend Const WM_IME_COMPOSITION As Integer = &H10F
    'Friend Const WM_IME_COMPOSITIONFULL As Integer = &H284
    'Friend Const WM_IME_CONTROL As Integer = &H283
    'Friend Const WM_IME_ENDCOMPOSITION As Integer = 270
    'Friend Const WM_IME_KEYDOWN As Integer = &H290
    'Friend Const WM_IME_KEYLAST As Integer = &H10F
    'Friend Const WM_IME_KEYUP As Integer = &H291
    'Friend Const WM_IME_NOTIFY As Integer = &H282
    'Friend Const WM_IME_REQUEST As Integer = &H288
    'Friend Const WM_IME_SELECT As Integer = &H285
    'Friend Const WM_IME_SETCONTEXT As Integer = &H281
    'Friend Const WM_IME_STARTCOMPOSITION As Integer = &H10D
    'Friend Const WM_INITDIALOG As Integer = &H110
    'Friend Const WM_INITMENU As Integer = &H116
    'Friend Const WM_INITMENUPOPUP As Integer = &H117
    'Friend Const WM_INPUT As Integer = &HFF
    'Friend Const WM_INPUTLANGCHANGE As Integer = &H51
    'Friend Const WM_INPUTLANGCHANGEREQUEST As Integer = 80
    'Friend Const WM_KEYDOWN As Integer = &H100
    'Friend Const WM_KEYFIRST As Integer = &H100
    'Friend Const WM_KEYLAST As Integer = &H108
    Friend Const WM_KEYUP As Integer = &H101
    'Friend Const WM_KILLFOCUS As Integer = 8
    Friend Const WM_LBUTTONDBLCLK As Integer = &H203
    Friend Const WM_LBUTTONDOWN As Integer = &H201
    Friend Const WM_LBUTTONUP As Integer = &H202
    'Friend Const WM_MBUTTONDBLCLK As Integer = &H209
    'Friend Const WM_MBUTTONDOWN As Integer = &H207
    'Friend Const WM_MBUTTONUP As Integer = 520
    'Friend Const WM_MDIACTIVATE As Integer = &H222
    'Friend Const WM_MDICASCADE As Integer = &H227
    'Friend Const WM_MDICREATE As Integer = &H220
    'Friend Const WM_MDIDESTROY As Integer = &H221
    'Friend Const WM_MDIGETACTIVE As Integer = &H229
    'Friend Const WM_MDIICONARRANGE As Integer = &H228
    'Friend Const WM_MDIMAXIMIZE As Integer = &H225
    'Friend Const WM_MDINEXT As Integer = &H224
    'Friend Const WM_MDIREFRESHMENU As Integer = &H234
    'Friend Const WM_MDIRESTORE As Integer = &H223
    'Friend Const WM_MDISETMENU As Integer = 560
    'Friend Const WM_MDITILE As Integer = 550
    'Friend Const WM_MEASUREITEM As Integer = &H2C
    'Friend Const WM_MENUCHAR As Integer = &H120
    'Friend Const WM_MENUSELECT As Integer = &H11F
    'Friend Const WM_MOUSEACTIVATE As Integer = &H21
    'Friend Const WM_MOUSEFIRST As Integer = &H200
    'Friend Const WM_MOUSEHOVER As Integer = &H2A1
    'Friend Const WM_MOUSELAST As Integer = &H20A
    'Friend Const WM_MOUSELEAVE As Integer = &H2A3
    'Friend Const WM_MOUSEMOVE As Integer = &H200
    'Friend Const WM_MOUSEQUERY As Integer = &H9B
    'Friend Const WM_MOUSEWHEEL As Integer = &H20A
    'Friend Const WM_MOVE As Integer = 3
    'Friend Const WM_MOVING As Integer = &H216
    Friend Const WM_NCACTIVATE As Integer = &H86
    'Friend Const WM_NCCALCSIZE As Integer = &H83
    'Friend Const WM_NCCREATE As Integer = &H81
    'Friend Const WM_NCDESTROY As Integer = 130
    'Friend Const WM_NCHITTEST As Integer = &H84
    'Friend Const WM_NCLBUTTONDBLCLK As Integer = &HA3
    Friend Const WM_NCLBUTTONDOWN As Integer = &HA1
    'Friend Const WM_NCLBUTTONUP As Integer = &HA2
    'Friend Const WM_NCMBUTTONDBLCLK As Integer = &HA9
    'Friend Const WM_NCMBUTTONDOWN As Integer = &HA7
    'Friend Const WM_NCMBUTTONUP As Integer = &HA8
    'Friend Const WM_NCMOUSELEAVE As Integer = &H2A2
    'Friend Const WM_NCMOUSEMOVE As Integer = 160
    Friend Const WM_NCPAINT As Integer = &H85
    'Friend Const WM_NCRBUTTONDBLCLK As Integer = &HA6
    'Friend Const WM_NCRBUTTONDOWN As Integer = &HA4
    'Friend Const WM_NCRBUTTONUP As Integer = &HA5
    'Friend Const WM_NCXBUTTONDBLCLK As Integer = &HAD
    'Friend Const WM_NCXBUTTONDOWN As Integer = &HAB
    'Friend Const WM_NCXBUTTONUP As Integer = &HAC
    'Friend Const WM_NEXTDLGCTL As Integer = 40
    'Friend Const WM_NEXTMENU As Integer = &H213
    Friend Const WM_NOTIFY As Integer = &H4E
    'Friend Const WM_NOTIFYFORMAT As Integer = &H55
    'Friend Const WM_NULL As Integer = 0
    Friend Const WM_PAINT As Integer = 15
    'Friend Const WM_PAINTCLIPBOARD As Integer = &H309
    'Friend Const WM_PAINTICON As Integer = &H26
    'Friend Const WM_PALETTECHANGED As Integer = &H311
    'Friend Const WM_PALETTEISCHANGING As Integer = &H310
    'Friend Const WM_PARENTNOTIFY As Integer = &H210
    'Friend Const WM_PASTE As Integer = 770
    'Friend Const WM_PENWINFIRST As Integer = &H380
    'Friend Const WM_PENWINLAST As Integer = &H38F
    'Friend Const WM_POWER As Integer = &H48
    'Friend Const WM_POWERBROADCAST As Integer = &H218
    'Friend Const WM_PRINT As Integer = &H317
    'Friend Const WM_PRINTCLIENT As Integer = &H318
    'Friend Const WM_QUERYDRAGICON As Integer = &H37
    'Friend Const WM_QUERYENDSESSION As Integer = &H11
    'Friend Const WM_QUERYNEWPALETTE As Integer = &H30F
    'Friend Const WM_QUERYOPEN As Integer = &H13
    'Friend Const WM_QUERYSYSTEMGESTURESTATUS As Integer = &H2CC
    'Friend Const WM_QUERYUISTATE As Integer = &H129
    'Friend Const WM_QUEUESYNC As Integer = &H23
    'Friend Const WM_QUIT As Integer = &H12
    'Friend Const WM_RBUTTONDBLCLK As Integer = &H206
    'Friend Const WM_RBUTTONDOWN As Integer = &H204
    Friend Const WM_RBUTTONUP As Integer = &H205
    Friend Const WM_REFLECT As Integer = &H2000
    'Friend Const WM_RENDERALLFORMATS As Integer = &H306
    'Friend Const WM_RENDERFORMAT As Integer = &H305
    'Friend Const WM_SETCURSOR As Integer = &H20
    'Friend Const WM_SETFOCUS As Integer = 7
    'Friend Const WM_SETFONT As Integer = &H30
    'Friend Const WM_SETHOTKEY As Integer = 50
    'Friend Const WM_SETICON As Integer = &H80
    'Friend Const WM_SETREDRAW As Integer = 11
    'Friend Const WM_SETTEXT As Integer = 12
    Friend Const WM_SETTINGCHANGE As Integer = &H1A
    'Friend Const WM_SHOWWINDOW As Integer = &H18
    'Friend Const WM_SIZE As Integer = 5
    'Friend Const WM_SIZECLIPBOARD As Integer = &H30B
    'Friend Const WM_SIZING As Integer = &H214
    'Friend Const WM_SPOOLERSTATUS As Integer = &H2A
    'Friend Const WM_STYLECHANGED As Integer = &H7D
    'Friend Const WM_STYLECHANGING As Integer = &H7C
    'Friend Const WM_SYSCHAR As Integer = &H106
    'Friend Const WM_SYSCOLORCHANGE As Integer = &H15
    Friend Const WM_SYSCOMMAND As Integer = &H112
    'Friend Const WM_SYSDEADCHAR As Integer = &H107
    'Friend Const WM_SYSKEYDOWN As Integer = 260
    'Friend Const WM_SYSKEYUP As Integer = &H105
    'Friend Const WM_TABLET_ADDED As Integer = &H2C8
    'Friend Const WM_TABLET_REMOVED As Integer = &H2C9
    'Friend Const WM_TCARD As Integer = &H52
    'Friend Const WM_THEMECHANGED As Integer = &H31A
    'Friend Const WM_TIMECHANGE As Integer = 30
    'Friend Const WM_TIMER As Integer = &H113
    'Friend Const WM_UNDO As Integer = &H304
    'Friend Const WM_UNINITMENUPOPUP As Integer = &H125
    'Friend Const WM_UPDATEUISTATE As Integer = &H128
    Friend Const WM_USER As Integer = &H400
    'Friend Const WM_USERCHANGED As Integer = &H54
    'Friend Const WM_VKEYTOITEM As Integer = &H2E
    Friend Const WM_VSCROLL As Integer = &H115
    'Friend Const WM_VSCROLLCLIPBOARD As Integer = &H30A
    'Friend Const WM_WINDOWPOSCHANGED As Integer = &H47
    'Friend Const WM_WINDOWPOSCHANGING As Integer = 70
    'Friend Const WM_WININICHANGE As Integer = &H1A
    'Friend Const WM_WTSSESSION_CHANGE As Integer = &H2B1
    'Friend Const WM_XBUTTONDBLCLK As Integer = &H20D
    'Friend Const WM_XBUTTONDOWN As Integer = &H20B
    'Friend Const WM_XBUTTONUP As Integer = &H20C

    'Friend Const WS_BORDER As Integer = &H800000
    'Friend Const WS_CAPTION As Integer = &HC00000
    'Friend Const WS_CHILD As Integer = &H40000000
    'Friend Const WS_CLIPCHILDREN As Integer = &H2000000
    'Friend Const WS_CLIPSIBLINGS As Integer = &H4000000
    'Friend Const WS_DISABLED As Integer = &H8000000
    'Friend Const WS_DLGFRAME As Integer = &H400000
    'Friend Const WS_EX_APPWINDOW As Integer = &H40000
    Friend Const WS_EX_CLIENTEDGE As Integer = &H200
    'Friend Const WS_EX_COMPOSITED As Integer = &H2000000
    'Friend Const WS_EX_CONTEXTHELP As Integer = &H400
    'Friend Const WS_EX_CONTROLPARENT As Integer = &H10000
    'Friend Const WS_EX_DLGMODALFRAME As Integer = 1
    'Friend Const WS_EX_LAYERED As Integer = &H80000
    'Friend Const WS_EX_LAYOUTRTL As Integer = &H400000
    'Friend Const WS_EX_LEFT As Integer = 0
    'Friend Const WS_EX_LEFTSCROLLBAR As Integer = &H4000
    'Friend Const WS_EX_MDICHILD As Integer = &H40
    'Friend Const WS_EX_NOACTIVATE As Integer = &H8000000
    'Friend Const WS_EX_NOINHERITLAYOUT As Integer = &H100000
    'Friend Const WS_EX_RIGHT As Integer = &H1000
    'Friend Const WS_EX_RTLREADING As Integer = &H2000
    'Friend Const WS_EX_STATICEDGE As Integer = &H20000
    'Friend Const WS_EX_TOOLWINDOW As Integer = &H80
    'Friend Const WS_EX_TOPMOST As Integer = 8
    'Friend Const WS_EX_TRANSPARENT As Integer = &H20
    'Friend Const WS_EX_WINDOWEDGE As Integer = &H100
    'Friend Const WS_HSCROLL As Integer = &H100000
    'Friend Const WS_MAXIMIZE As Integer = &H1000000
    'Friend Const WS_MAXIMIZEBOX As Integer = &H10000
    'Friend Const WS_MINIMIZE As Integer = &H20000000
    'Friend Const WS_MINIMIZEBOX As Integer = &H20000
    'Friend Const WS_OVERLAPPED As Integer = 0
    'Friend Const WS_OVERLAPPEDWINDOW As Integer = &HCF0000
    'Friend Const WS_POPUP As Integer = -2147483648
    'Friend Const WS_SYSMENU As Integer = &H80000
    'Friend Const WS_TABSTOP As Integer = &H10000
    'Friend Const WS_THICKFRAME As Integer = &H40000
    'Friend Const WS_VISIBLE As Integer = &H10000000
    Friend Const WS_VSCROLL As Integer = &H200000

    Friend Const ASSOCSTR_EXECUTABLE As Integer = 2

    Friend Const ASSOCF_VERIFY As Integer = &H40

    Friend Const EM_SETCUEBANNER As Integer = &H1501
    Friend Const CB_SETCUEBANNER As Integer = &H1703

    'Friend Const MAPVK_VK_TO_VSC As Integer = 0
    'Friend Const MAPVK_VSC_TO_VK As Integer = 1
    Friend Const MAPVK_VK_TO_CHAR As Integer = 2
    'Friend Const MAPVK_VSC_TO_VK_EX As Integer = 3
    'Friend Const MAPVK_VK_TO_VSC_EX As Integer = 4

    Friend Const SC_CLOSE As Integer = &HF060
    'Friend Const SC_KEYMENU As Integer = &HF100
    'Friend Const SC_MAXIMIZE As Integer = &HF030
    Friend Const SC_MINIMIZE As Integer = &HF020
    'Friend Const SC_MOUSEMOVE As Integer = &HF012
    'Friend Const SC_MOVE As Integer = &HF010
    'Friend Const SC_RESTORE As Integer = &HF120
    'Friend Const SC_SIZE As Integer = &HF000

    'Friend Const SW_ERASE As Integer = 4
    Friend Const SW_HIDE As Integer = 0
    'Friend Const SW_INVALIDATE As Integer = 2
    'Friend Const SW_MAX As Integer = 10
    'Friend Const SW_MAXIMIZE As Integer = 3
    'Friend Const SW_MINIMIZE As Integer = 6
    'Friend Const SW_NORMAL As Integer = 1
    'Friend Const SW_PARENTCLOSING As Integer = 1
    'Friend Const SW_PARENTOPENING As Integer = 3
    Friend Const SW_RESTORE As Integer = 9
    'Friend Const SW_SCROLLCHILDREN As Integer = 1
    'Friend Const SW_SHOW As Integer = 5
    'Friend Const SW_SHOWMAXIMIZED As Integer = 3
    'Friend Const SW_SHOWMINIMIZED As Integer = 2
    'Friend Const SW_SHOWMINNOACTIVE As Integer = 7
    'Friend Const SW_SHOWNA As Integer = 8
    'Friend Const SW_SHOWNOACTIVATE As Integer = 4
    'Friend Const SW_SMOOTHSCROLL As Integer = &H10

    Friend Const GWL_STYLE As Integer = -16

    Friend Const FORMAT_MESSAGE_ALLOCATE_BUFFER = &H100
    Friend Const FORMAT_MESSAGE_IGNORE_INSERTS = &H200
    Friend Const FORMAT_MESSAGE_FROM_STRING = &H400
    Friend Const FORMAT_MESSAGE_FROM_HMODULE = &H800
    Friend Const FORMAT_MESSAGE_FROM_SYSTEM = &H1000
    Friend Const FORMAT_MESSAGE_ARGUMENT_ARRAY = &H2000

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

    <DllImport("user32.dll")>
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

#Region "kernel32.dll"

    <DllImport("kernel32.dll", CharSet:=CharSet.Unicode)>
    Shared Function LoadLibrary(path As String) As IntPtr
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Shared Function FreeLibrary(hModule As IntPtr) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
    Shared Function FormatMessageW(dwFlags As Integer,
                            lpSource As IntPtr,
                            dwMessageId As Integer,
                            dwLanguageId As Integer,
                            ByRef lpBuffer As String,
                            nSize As Integer,
                            Arguments As IntPtr) As Integer
    End Function

#End Region

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