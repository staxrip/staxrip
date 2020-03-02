
Imports System.Runtime.InteropServices
Imports System.Text

Public Class Native
    Public Delegate Function CallbackHandler(handle As IntPtr, parameter As Integer) As Boolean

    <DllImport("gdi32.dll")>
    Shared Function ExcludeClipRect(hdc As IntPtr, nLeftRect As Integer, nTopRect As Integer, nRightRect As Integer, nBottomRect As Integer) As Integer
    End Function

#Region "Constants"

    'Friend Const WS_BORDER As Integer = &H800000
    'Friend Const WS_CAPTION As Integer = &HC00000
    'Friend Const WS_CHILD As Integer = &H40000000
    'Friend Const WS_CLIPCHILDREN As Integer = &H2000000
    'Friend Const WS_CLIPSIBLINGS As Integer = &H4000000
    'Friend Const WS_DISABLED As Integer = &H8000000
    'Friend Const WS_DLGFRAME As Integer = &H400000
    'Friend Const WS_EX_APPWINDOW As Integer = &H40000
    'Friend Const WS_EX_CLIENTEDGE As Integer = &H200
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
    'Friend Const WS_VSCROLL As Integer = &H200000

    'Friend Const ASSOCSTR_EXECUTABLE As Integer = 2

    'Friend Const ASSOCF_VERIFY As Integer = &H40

    Friend Const EM_SETCUEBANNER As Integer = &H1501
    Friend Const CB_SETCUEBANNER As Integer = &H1703

    Friend Const SC_CLOSE As Integer = &HF060
    'Friend Const SC_KEYMENU As Integer = &HF100
    'Friend Const SC_MAXIMIZE As Integer = &HF030
    Friend Const SC_MINIMIZE As Integer = &HF020
    'Friend Const SC_MOUSEMOVE As Integer = &HF012
    'Friend Const SC_MOVE As Integer = &HF010
    'Friend Const SC_RESTORE As Integer = &HF120
    'Friend Const SC_SIZE As Integer = &HF000

    'Friend Const SW_ERASE As Integer = 4
    'Friend Const SW_HIDE As Integer = 0
    'Friend Const SW_INVALIDATE As Integer = 2
    'Friend Const SW_MAX As Integer = 10
    'Friend Const SW_MAXIMIZE As Integer = 3
    'Friend Const SW_MINIMIZE As Integer = 6
    'Friend Const SW_NORMAL As Integer = 1
    'Friend Const SW_PARENTCLOSING As Integer = 1
    'Friend Const SW_PARENTOPENING As Integer = 3
    'Friend Const SW_RESTORE As Integer = 9
    'Friend Const SW_SCROLLCHILDREN As Integer = 1
    'Friend Const SW_SHOW As Integer = 5
    'Friend Const SW_SHOWMAXIMIZED As Integer = 3
    'Friend Const SW_SHOWMINIMIZED As Integer = 2
    'Friend Const SW_SHOWMINNOACTIVE As Integer = 7
    'Friend Const SW_SHOWNA As Integer = 8
    'Friend Const SW_SHOWNOACTIVATE As Integer = 4
    'Friend Const SW_SMOOTHSCROLL As Integer = &H10

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

    <DllImport("user32.dll")>
    Shared Function SetForegroundWindow(handle As IntPtr) As Boolean
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
    Shared Function GetWindowRect(hWnd As IntPtr, ByRef lpRect As RECT) As Boolean
    End Function

    <DllImport("user32.dll")>
    Shared Function GetWindowDC(hWnd As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Shared Function ReleaseDC(hWnd As IntPtr, hDC As IntPtr) As Integer
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
    Shared Function FormatMessage(dwFlags As Integer,
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

Public Class Taskbar
    Private Taskbar As ITaskbarList3 = DirectCast(New TaskBarCommunication(), ITaskbarList3)

    Property Handle As IntPtr

    Public Sub New(handle As IntPtr)
        Me.Handle = handle
    End Sub

    <ComImportAttribute>
    <InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)>
    <GuidAttribute("ea1afb91-9e28-4b86-90e9-9e9f8a5eefaf")>
    Private Interface ITaskbarList3
        'ITaskbarList
        <PreserveSig> Sub HrInit()
        <PreserveSig> Sub AddTab(hwnd As IntPtr)
        <PreserveSig> Sub DeleteTab(hwnd As IntPtr)
        <PreserveSig> Sub ActivateTab(hwnd As IntPtr)
        <PreserveSig> Sub SetActiveAlt(hwnd As IntPtr)
        'ITaskbarList2
        <PreserveSig> Sub MarkFullscreenWindow(hwnd As IntPtr, <MarshalAs(UnmanagedType.Bool)> fFullscreen As Boolean)
        'ITaskbarList3
        <PreserveSig> Sub SetProgressValue(hwnd As IntPtr, ullCompleted As UInt64, ullTotal As UInt64)
        <PreserveSig> Sub SetProgressState(hwnd As IntPtr, state As TaskbarStates)
    End Interface

    <ComImportAttribute>
    <ClassInterfaceAttribute(ClassInterfaceType.None)>
    <GuidAttribute("56FDF344-FD6D-11d0-958A-006097C9A090")>
    Private Class TaskBarCommunication
    End Class

    Public Sub SetState(taskbarState As TaskbarStates)
        Taskbar.SetProgressState(Handle, taskbarState)
    End Sub

    Public Sub SetValue(progressValue As Double, progressMax As Double)
        Taskbar.SetProgressValue(Handle, CULng(Math.Truncate(progressValue)), CULng(Math.Truncate(progressMax)))
    End Sub
End Class

Public Enum TaskbarStates
    NoProgress = 0
    Indeterminate = &H1
    Normal = &H2
    [Error] = &H4
    Paused = &H8
End Enum
