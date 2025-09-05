
Imports System.Runtime.InteropServices
Imports System.Text

Public Class Native
    Public Const EM_SETCUEBANNER As Integer = &H1501

    Public Const CB_SETCUEBANNER As Integer = &H1703

    Public Const SC_CLOSE As Integer = &HF060
    Public Const SC_MINIMIZE As Integer = &HF020
    Public Const SPI_GETWHEELSCROLLLINES As Integer = 104

    <DllImport("dwmapi.dll")>
    Shared Function DwmSetWindowAttribute(
        hwnd As IntPtr,
        attr As Integer,
        ByRef attrValue As Integer,
        attrSize As Integer) As Integer
    End Function

    <DllImport("gdi32.dll")>
    Shared Function ExcludeClipRect(
        hdc As IntPtr,
        nLeftRect As Integer,
        nTopRect As Integer,
        nRightRect As Integer,
        nBottomRect As Integer) As Integer
    End Function

    <DllImport("user32.dll")>
    Shared Function SetWindowPos(
        hWnd As IntPtr,
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
    Shared Function UnregisterHotKey(hWnd As IntPtr, id As Integer) As Boolean
    End Function

    <DllImport("user32.dll")>
    Shared Function MapVirtualKey(wCode As Integer, wMapType As Integer) As Integer
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Auto, ExactSpelling:=True, CallingConvention:=CallingConvention.Winapi)>
    Shared Function GetKeyState(keyCode As Integer) As Short
    End Function

    <DllImport("user32.dll")>
    Shared Function GetForegroundWindow() As IntPtr
    End Function

    <DllImport("user32.dll")>
    Shared Function SetForegroundWindow(handle As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function GetWindowModuleFileName(
        hwnd As IntPtr,
        lpszFileName As StringBuilder,
        cchFileNameMax As UInteger) As UInteger
    End Function

    <DllImport("user32.dll")>
    Shared Function SendMessage(
        handle As IntPtr,
        message As Int32,
        wParam As IntPtr,
        lParam As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function SendMessage(
        hWnd As IntPtr,
        Msg As Int32,
        wParam As IntPtr,
        lParam As String) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function SendMessage(
        hWnd As IntPtr,
        Msg As Int32,
        wParam As Integer,
        lParam As Integer) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function SendMessage(
        hWnd As IntPtr,
        Msg As Int32,
        wParam As Integer,
        lParam As String) As IntPtr
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function PostMessage(
        hwnd As IntPtr,
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

    <DllImport("kernel32.dll", CharSet:=CharSet.Unicode)>
    Shared Function LoadLibrary(path As String) As IntPtr
    End Function

    <DllImport("kernel32.dll", CharSet:=CharSet.Unicode)>
    Shared Function GetShortPathName(
        longPath As String,
        shortPath As StringBuilder,
        bufSize As Integer) As Integer
    End Function

    <DllImport("kernel32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
    Shared Function GetDiskFreeSpaceEx(lpDirectoryName As String, ByRef lpFreeBytesAvailableToCaller As ULong, ByRef lpTotalNumberOfBytes As ULong, ByRef lpTotalNumberOfFreeBytes As ULong) As Boolean
    End Function

    <DllImport("uxtheme.dll", CharSet:=CharSet.Unicode)>
    Shared Function SetWindowTheme(
        hWnd As IntPtr,
        pszSubAppName As String,
        pszSubIdList As String) As Integer
    End Function

    <DllImport("shlwapi.dll", CharSet:=CharSet.Unicode)>
    Shared Function AssocQueryString(
        flags As UInteger,
        str As UInteger,
        pszAssoc As String,
        pszExtra As String,
        pszOut As StringBuilder,
        ByRef pcchOut As UInteger) As UInteger
    End Function

    <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
    Shared Function SystemParametersInfo(uAction As Integer, uParam As Integer, ByRef lpvParam As Integer, fuWinIni As Integer) As Boolean
    End Function

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

        Sub New(left As Integer, top As Integer, right As Integer, bottom As Integer)
            Me.Left = left
            Me.Top = top
            Me.Right = right
            Me.Bottom = bottom
        End Sub

        Function ToRectangle() As Rectangle
            Return Rectangle.FromLTRB(Left, Top, Right, Bottom)
        End Function
    End Structure

    Public Structure NCCALCSIZE_PARAMS
        Public rgrc0, rgrc1, rgrc2 As RECT
        Public lppos As IntPtr
    End Structure
End Class

Public Class Taskbar
    Private Taskbar As ITaskbarList3 = DirectCast(New TaskBarCommunication(), ITaskbarList3)

    Property Handle As IntPtr

    Sub New(handle As IntPtr)
        Me.Handle = handle
    End Sub

    <ComImport>
    <InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    <Guid("EA1AFB91-9E28-4B86-90E9-9E9F8A5EEFAF")>
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

    <ComImport>
    <ClassInterface(ClassInterfaceType.None)>
    <Guid("56FDF344-FD6D-11d0-958A-006097C9A090")>
    Class TaskBarCommunication
    End Class

    Sub SetState(taskbarState As TaskbarStates)
        Taskbar.SetProgressState(Handle, taskbarState)
    End Sub

    Sub SetValue(progressValue As Double, progressMax As Double)
        Taskbar.SetProgressValue(Handle, CULng(Math.Max(1, Math.Truncate(progressValue))), CULng(Math.Truncate(progressMax)))
    End Sub
End Class

Public Enum TaskbarStates
    NoProgress = 0
    Indeterminate = &H1
    Normal = &H2
    [Error] = &H4
    Paused = &H8
End Enum
