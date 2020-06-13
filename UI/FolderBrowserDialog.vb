
Imports System.Text
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports System.Runtime.CompilerServices

Public Class FolderBrowserDialog
    Inherits CommonDialog

    Private ReadOnly _downlevelDialog As Windows.Forms.FolderBrowserDialog
    Private _description As String
    Private _selectedPath As String
    Private _rootFolder As Environment.SpecialFolder

    Public Sub New()
        Me.New(False)
    End Sub

    Public Sub New(forceDownlevel As Boolean)
        If forceDownlevel Then
            _downlevelDialog = New Windows.Forms.FolderBrowserDialog()
        Else
            Reset()
        End If
    End Sub

    Property Description() As String
        Get
            If _downlevelDialog IsNot Nothing Then Return _downlevelDialog.Description
            Return _description
        End Get
        Set(value As String)
            If _downlevelDialog IsNot Nothing Then
                _downlevelDialog.Description = value
            Else
                _description = If(value, String.Empty)
            End If
        End Set
    End Property

    Property RootFolder() As Environment.SpecialFolder
        Get
            If _downlevelDialog IsNot Nothing Then
                Return _downlevelDialog.RootFolder
            End If

            Return _rootFolder
        End Get
        Set(value As Environment.SpecialFolder)
            If _downlevelDialog IsNot Nothing Then
                _downlevelDialog.RootFolder = value
            Else
                _rootFolder = value
            End If
        End Set
    End Property

    Property SelectedPath() As String
        Get
            If _downlevelDialog IsNot Nothing Then
                Return _downlevelDialog.SelectedPath
            End If

            Return _selectedPath
        End Get
        Set(value As String)
            If _downlevelDialog IsNot Nothing Then
                _downlevelDialog.SelectedPath = value
            Else
                _selectedPath = If(value, "")
            End If
        End Set
    End Property

    Private _showNewFolderButton As Boolean

    Property ShowNewFolderButton() As Boolean
        Get
            If _downlevelDialog IsNot Nothing Then
                Return _downlevelDialog.ShowNewFolderButton
            End If

            Return _showNewFolderButton
        End Get
        Set(value As Boolean)
            If _downlevelDialog IsNot Nothing Then
                _downlevelDialog.ShowNewFolderButton = value
            Else
                _showNewFolderButton = value
            End If
        End Set
    End Property

    Property UseDescriptionForTitle() As Boolean

    Public Overrides Sub Reset()
        _description = ""
        UseDescriptionForTitle = False
        _selectedPath = ""
        _rootFolder = Environment.SpecialFolder.Desktop
        _showNewFolderButton = True
    End Sub

    Protected Overrides Function RunDialog(hwndOwner As IntPtr) As Boolean
        If _downlevelDialog IsNot Nothing Then
            Return _downlevelDialog.ShowDialog(If(hwndOwner = IntPtr.Zero, Nothing, New WindowHandleWrapper(hwndOwner))) = DialogResult.OK
        End If

        Dim dialog As IFileDialog = Nothing

        Try
            dialog = CType(New FileOpenDialogRCW(), IFileDialog)
            SetDialogProperties(dialog)
            Dim result = dialog.Show(hwndOwner)

            If result < 0 Then
                If result = NativeMethods.ERROR_CANCELLED Then
                    Return False
                Else
                    Throw Marshal.GetExceptionForHR(result)
                End If
            End If

            GetResult(dialog)
            Return True
        Finally
            If dialog IsNot Nothing Then
                Dim unused = Marshal.FinalReleaseComObject(dialog)
            End If
        End Try
    End Function

    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso _downlevelDialog IsNot Nothing Then
                _downlevelDialog.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Sub SetDialogProperties(dialog As IFileDialog)
        If _description <> "" Then
            If UseDescriptionForTitle Then
                dialog.SetTitle(_description)
            Else
                DirectCast(dialog, IFileDialogCustomize).AddText(0, _description)
            End If
        End If

        dialog.SetOptions(NativeMethods.FOS.FOS_PICKFOLDERS Or
                          NativeMethods.FOS.FOS_FORCEFILESYSTEM Or
                          NativeMethods.FOS.FOS_FILEMUSTEXIST)

        If _selectedPath <> "" Then
            If Path.GetDirectoryName(_selectedPath) Is Nothing OrElse Not Directory.Exists(Path.GetDirectoryName(_selectedPath)) Then
                dialog.SetFileName(_selectedPath)
            Else
                dialog.SetFolder(NativeMethods.CreateItemFromParsingName(Path.GetDirectoryName(_selectedPath)))
                dialog.SetFileName(Path.GetFileName(_selectedPath))
            End If
        End If
    End Sub

    Sub GetResult(dialog As IFileDialog)
        Dim item As IShellItem = Nothing
        dialog.GetResult(item)
        item.GetDisplayName(NativeMethods.SIGDN.SIGDN_FILESYSPATH, _selectedPath)
    End Sub

    <ComImport(), Guid(IIDGuid.IModalWindow), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IModalWindow
        <MethodImpl(MethodImplOptions.InternalCall, MethodCodeType:=MethodCodeType.Runtime), PreserveSig()>
        Function Show(parent As IntPtr) As Integer
    End Interface

    <ComImport(), Guid(IIDGuid.IFileDialog), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IFileDialog
        Inherits IModalWindow

        <PreserveSig()> Overloads Function Show(parent As IntPtr) As Integer
        Sub SetFileTypes(cFileTypes As UInteger, <[In](), MarshalAs(UnmanagedType.LPArray)> rgFilterSpec As NativeMethods.COMDLG_FILTERSPEC())
        Sub SetFileTypeIndex(iFileType As UInteger)
        Sub GetFileTypeIndex(piFileType As UInteger)
        Sub Advise(<[In](), MarshalAs(UnmanagedType.[Interface])> pfde As IFileDialogEvents, pdwCookie As UInteger)
        Sub Unadvise(dwCookie As UInteger)
        Sub SetOptions(fos As NativeMethods.FOS)
        Sub GetOptions(pfos As NativeMethods.FOS)
        Sub SetDefaultFolder(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)
        Sub SetFolder(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)
        Sub GetFolder(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)
        Sub GetCurrentSelection(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)
        Sub SetFileName(pszName As String)
        Sub GetFileName(<MarshalAs(UnmanagedType.LPWStr)> pszName As String)
        Sub SetTitle(pszTitle As String)
        Sub SetOkButtonLabel(pszText As String)
        Sub SetFileNameLabel(pszLabel As String)
        Sub GetResult(<MarshalAs(UnmanagedType.[Interface])> ByRef ppsi As IShellItem)
        Sub AddPlace(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, fdap As NativeMethods.FDAP)
        Sub SetDefaultExtension(pszDefaultExtension As String)
        Sub Close(<MarshalAs(UnmanagedType.[Error])> hr As Integer)
        Sub SetClientGuid(ByRef guid As Guid)
        Sub ClearClientData()
        Sub SetFilter(<MarshalAs(UnmanagedType.[Interface])> pFilter As IntPtr)
    End Interface

    <ComImport(), Guid(IIDGuid.IFileOpenDialog), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IFileOpenDialog
        Inherits IFileDialog

        <PreserveSig()> Overloads Function Show(parent As IntPtr) As Integer
        Overloads Sub SetFileTypes(cFileTypes As UInteger, ByRef rgFilterSpec As NativeMethods.COMDLG_FILTERSPEC)
        Overloads Sub SetFileTypeIndex(iFileType As UInteger)
        Overloads Sub GetFileTypeIndex(piFileType As UInteger)
        Overloads Sub Advise(<[In](), MarshalAs(UnmanagedType.[Interface])> pfde As IFileDialogEvents, pdwCookie As UInteger)
        Overloads Sub Unadvise(dwCookie As UInteger)
        Overloads Sub SetOptions(fos As NativeMethods.FOS)
        Overloads Sub GetOptions(pfos As NativeMethods.FOS)
        Overloads Sub SetDefaultFolder(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)
        Overloads Sub SetFolder(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)
        Overloads Sub GetFolder(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)
        Overloads Sub GetCurrentSelection(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)
        Overloads Sub SetFileName(pszName As String)
        Overloads Sub GetFileName(<MarshalAs(UnmanagedType.LPWStr)> pszName As String)
        Overloads Sub SetTitle(pszTitle As String)
        Overloads Sub SetOkButtonLabel(pszText As String)
        Overloads Sub SetFileNameLabel(pszLabel As String)
        Overloads Sub GetResult(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)
        Overloads Sub AddPlace(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, fdap As NativeMethods.FDAP)
        Overloads Sub SetDefaultExtension(pszDefaultExtension As String)
        Overloads Sub Close(<MarshalAs(UnmanagedType.[Error])> hr As Integer)
        Overloads Sub SetClientGuid(ByRef guid As Guid)
        Overloads Sub ClearClientData()
        Overloads Sub SetFilter(<MarshalAs(UnmanagedType.[Interface])> pFilter As IntPtr)
        Sub GetResults(<MarshalAs(UnmanagedType.[Interface])> ppenum As IShellItemArray)
        Sub GetSelectedItems(<MarshalAs(UnmanagedType.[Interface])> ppsai As IShellItemArray)
    End Interface

    <ComImport(), Guid(IIDGuid.IFileSaveDialog), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IFileSaveDialog
        Inherits IFileDialog

        <PreserveSig()> Overloads Function Show(parent As IntPtr) As Integer
        Overloads Sub SetFileTypes(cFileTypes As UInteger, ByRef rgFilterSpec As NativeMethods.COMDLG_FILTERSPEC)
        Overloads Sub SetFileTypeIndex(iFileType As UInteger)
        Overloads Sub GetFileTypeIndex(piFileType As UInteger)
        Overloads Sub Advise(<[In](), MarshalAs(UnmanagedType.[Interface])> pfde As IFileDialogEvents, pdwCookie As UInteger)
        Overloads Sub Unadvise(dwCookie As UInteger)
        Overloads Sub SetOptions(fos As NativeMethods.FOS)
        Overloads Sub GetOptions(pfos As NativeMethods.FOS)
        Overloads Sub SetDefaultFolder(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)
        Overloads Sub SetFolder(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)
        Overloads Sub GetFolder(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)
        Overloads Sub GetCurrentSelection(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)
        Overloads Sub SetFileName(pszName As String)
        Overloads Sub GetFileName(<MarshalAs(UnmanagedType.LPWStr)> pszName As String)
        Overloads Sub SetTitle(pszTitle As String)
        Overloads Sub SetOkButtonLabel(pszText As String)
        Overloads Sub SetFileNameLabel(pszLabel As String)
        Overloads Sub GetResult(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)
        Overloads Sub AddPlace(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, fdap As NativeMethods.FDAP)
        Overloads Sub SetDefaultExtension(pszDefaultExtension As String)
        Overloads Sub Close(<MarshalAs(UnmanagedType.[Error])> hr As Integer)
        Overloads Sub SetClientGuid(ByRef guid As Guid)
        Overloads Sub ClearClientData()
        Overloads Sub SetFilter(<MarshalAs(UnmanagedType.[Interface])> pFilter As IntPtr)

        Sub SetSaveAsItem(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem)
        Sub SetProperties(<[In](), MarshalAs(UnmanagedType.[Interface])> pStore As IntPtr)
        Sub SetCollectedProperties(<[In](), MarshalAs(UnmanagedType.[Interface])> pList As IntPtr, fAppendDefault As Integer)
        Sub GetProperties(<MarshalAs(UnmanagedType.[Interface])> ppStore As IntPtr)
        Sub ApplyProperties(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, <[In](), MarshalAs(UnmanagedType.[Interface])> pStore As IntPtr, <[In](), ComAliasName("Interop.wireHWND")> ByRef hwnd As IntPtr, <[In](), MarshalAs(UnmanagedType.[Interface])> pSink As IntPtr)
    End Interface

    <ComImport(), Guid(IIDGuid.IFileDialogEvents), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IFileDialogEvents
        <PreserveSig()> Function OnFileOk(<[In](), MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog) As HRESULT
        <PreserveSig()> Function OnFolderChanging(<[In](), MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog, <[In](), MarshalAs(UnmanagedType.[Interface])> psiFolder As IShellItem) As HRESULT
        Sub OnFolderChange(<[In](), MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog)
        Sub OnSelectionChange(<[In](), MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog)
        Sub OnShareViolation(<[In](), MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog, <[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, pResponse As NativeMethods.FDE_SHAREVIOLATION_RESPONSE)
        Sub OnTypeChange(<[In](), MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog)
        Sub OnOverwrite(<[In](), MarshalAs(UnmanagedType.[Interface])> pfd As IFileDialog, <[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, pResponse As NativeMethods.FDE_OVERWRITE_RESPONSE)
    End Interface

    <ComImport(), Guid(IIDGuid.IShellItem), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Public Interface IShellItem
        Sub BindToHandler(<[In](), MarshalAs(UnmanagedType.[Interface])> pbc As IntPtr, ByRef bhid As Guid, ByRef riid As Guid, ppv As IntPtr)
        Sub GetParent(<MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)
        Sub GetDisplayName(sigdnName As NativeMethods.SIGDN, <MarshalAs(UnmanagedType.LPWStr)> ByRef ppszName As String)
        Sub GetAttributes(sfgaoMask As UInteger, psfgaoAttribs As UInteger)
        Sub Compare(<[In](), MarshalAs(UnmanagedType.[Interface])> psi As IShellItem, hint As UInteger, piOrder As Integer)
    End Interface

    <ComImport(), Guid(IIDGuid.IShellItemArray), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IShellItemArray
        Sub BindToHandler(<[In](), MarshalAs(UnmanagedType.[Interface])> pbc As IntPtr, ByRef rbhid As Guid, ByRef riid As Guid, ppvOut As IntPtr)
        Sub GetPropertyStore(Flags As Integer, ByRef riid As Guid, ppv As IntPtr)
        Sub GetPropertyDescriptionList(ByRef keyType As NativeMethods.PROPERTYKEY, ByRef riid As Guid, ppv As IntPtr)
        Sub GetAttributes(dwAttribFlags As NativeMethods.SIATTRIBFLAGS, sfgaoMask As UInteger, psfgaoAttribs As UInteger)
        Sub GetCount(pdwNumItems As UInteger)
        Sub GetItemAt(dwIndex As UInteger, <MarshalAs(UnmanagedType.[Interface])> ppsi As IShellItem)
        Sub EnumItems(<MarshalAs(UnmanagedType.[Interface])> ppenumShellItems As IntPtr)
    End Interface

    <ComImport(), Guid(IIDGuid.IKnownFolder), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IKnownFolder
        Sub GetId(pkfid As Guid)

        Sub spacer1()
        Sub GetShellItem(dwFlags As UInteger, ByRef riid As Guid, ppv As IShellItem)
        Sub GetPath(dwFlags As UInteger, <MarshalAs(UnmanagedType.LPWStr)> ppszPath As String)
        Sub SetPath(dwFlags As UInteger, pszPath As String)
        Sub GetLocation(dwFlags As UInteger, <Out(), ComAliasName("Interop.wirePIDL")> ppidl As IntPtr)
        Sub GetFolderType(pftid As Guid)
        Sub GetRedirectionCapabilities(pCapabilities As UInteger)

        Sub spacer2()
    End Interface

    <ComImport(), Guid(IIDGuid.IKnownFolderManager), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IKnownFolderManager
        Sub FolderIdFromCsidl(nCsidl As Integer, pfid As Guid)
        Sub FolderIdToCsidl(ByRef rfid As Guid, pnCsidl As Integer)
        Sub GetFolderIds(<Out()> ppKFId As IntPtr, <[In](), Out()> ByRef pCount As UInteger)
        Sub GetFolder(ByRef rfid As Guid, <MarshalAs(UnmanagedType.[Interface])> ppkf As IKnownFolder)
        Sub GetFolderByName(pszCanonicalName As String, <MarshalAs(UnmanagedType.[Interface])> ppkf As IKnownFolder)
        Sub RegisterFolder(ByRef rfid As Guid, ByRef pKFD As NativeMethods.KNOWNFOLDER_DEFINITION)
        Sub UnregisterFolder(ByRef rfid As Guid)
        Sub FindFolderFromPath(pszPath As String, mode As NativeMethods.FFFP_MODE, <MarshalAs(UnmanagedType.[Interface])> ppkf As IKnownFolder)
        Sub FindFolderFromIDList(pidl As IntPtr, <MarshalAs(UnmanagedType.[Interface])> ppkf As IKnownFolder)
        Sub Redirect(ByRef rfid As Guid, hwnd As IntPtr, Flags As UInteger, pszTargetPath As String, cFolders As UInteger, ByRef pExclusion As Guid, <MarshalAs(UnmanagedType.LPWStr)> ppszError As String)
    End Interface

    <ComImport(), Guid(IIDGuid.IFileDialogCustomize), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IFileDialogCustomize
        Sub EnableOpenDropDown(dwIDCtl As Integer)
        Sub AddMenu(dwIDCtl As Integer, pszLabel As String)
        Sub AddPushButton(dwIDCtl As Integer, pszLabel As String)
        Sub AddComboBox(dwIDCtl As Integer)
        Sub AddRadioButtonList(dwIDCtl As Integer)
        Sub AddCheckButton(dwIDCtl As Integer, pszLabel As String, bChecked As Boolean)
        Sub AddEditBox(dwIDCtl As Integer, pszText As String)
        Sub AddSeparator(dwIDCtl As Integer)
        Sub AddText(dwIDCtl As Integer, pszText As String)
        Sub SetControlLabel(dwIDCtl As Integer, pszLabel As String)
        Sub GetControlState(dwIDCtl As Integer, <Out()> pdwState As NativeMethods.CDCONTROLSTATE)
        Sub SetControlState(dwIDCtl As Integer, dwState As NativeMethods.CDCONTROLSTATE)
        Sub GetEditBoxText(dwIDCtl As Integer, <Out()> ppszText As IntPtr)
        Sub SetEditBoxText(dwIDCtl As Integer, pszText As String)
        Sub GetCheckButtonState(dwIDCtl As Integer, <Out()> pbChecked As Boolean)
        Sub SetCheckButtonState(dwIDCtl As Integer, bChecked As Boolean)
        Sub AddControlItem(dwIDCtl As Integer, dwIDItem As Integer, pszLabel As String)
        Sub RemoveControlItem(dwIDCtl As Integer, dwIDItem As Integer)
        Sub RemoveAllControlItems(dwIDCtl As Integer)
        Sub GetControlItemState(dwIDCtl As Integer, dwIDItem As Integer, <Out()> pdwState As NativeMethods.CDCONTROLSTATE)
        Sub SetControlItemState(dwIDCtl As Integer, dwIDItem As Integer, dwState As NativeMethods.CDCONTROLSTATE)
        Sub GetSelectedControlItem(dwIDCtl As Integer, <Out()> pdwIDItem As Integer)
        Sub SetSelectedControlItem(dwIDCtl As Integer, dwIDItem As Integer)
        Sub StartVisualGroup(dwIDCtl As Integer, pszLabel As String)
        Sub EndVisualGroup()
        Sub MakeProminent(dwIDCtl As Integer)
    End Interface

    <ComImport(), Guid(IIDGuid.IFileDialogControlEvents), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IFileDialogControlEvents
        Sub OnItemSelected(<[In](), MarshalAs(UnmanagedType.[Interface])> pfdc As IFileDialogCustomize, dwIDCtl As Integer, dwIDItem As Integer)
        Sub OnButtonClicked(<[In](), MarshalAs(UnmanagedType.[Interface])> pfdc As IFileDialogCustomize, dwIDCtl As Integer)
        Sub OnCheckButtonToggled(<[In](), MarshalAs(UnmanagedType.[Interface])> pfdc As IFileDialogCustomize, dwIDCtl As Integer, bChecked As Boolean)
        Sub OnControlActivating(<[In](), MarshalAs(UnmanagedType.[Interface])> pfdc As IFileDialogCustomize, dwIDCtl As Integer)
    End Interface

    <ComImport(), Guid(IIDGuid.IPropertyStore), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)>
    Friend Interface IPropertyStore
        Sub GetCount(<Out()> cProps As UInteger)
        Sub GetAt(iProp As UInteger, pkey As NativeMethods.PROPERTYKEY)
        Sub GetValue(ByRef key As NativeMethods.PROPERTYKEY, pv As Object)
        Sub SetValue(ByRef key As NativeMethods.PROPERTYKEY, ByRef pv As Object)
        Sub Commit()
    End Interface

    Friend Interface NativeCommonFileDialog
    End Interface

    <ComImport(), Guid(IIDGuid.IFileOpenDialog), CoClass(GetType(FileOpenDialogRCW))>
    Friend Interface NativeFileOpenDialog
        Inherits IFileOpenDialog
    End Interface

    <ComImport(), Guid(IIDGuid.IFileSaveDialog), CoClass(GetType(FileSaveDialogRCW))>
    Friend Interface NativeFileSaveDialog
        Inherits IFileSaveDialog
    End Interface

    <ComImport(), Guid(IIDGuid.IKnownFolderManager), CoClass(GetType(KnownFolderManagerRCW))>
    Friend Interface KnownFolderManager
        Inherits IKnownFolderManager
    End Interface

    <ComImport(),
    ClassInterface(ClassInterfaceType.None),
    TypeLibType(TypeLibTypeFlags.FCanCreate),
    Guid(CLSIDGuid.FileOpenDialog)>
    Friend Class FileOpenDialogRCW
    End Class

    <ComImport(),
    ClassInterface(ClassInterfaceType.None),
    TypeLibType(TypeLibTypeFlags.FCanCreate),
    Guid(CLSIDGuid.FileSaveDialog)>
    Friend Class FileSaveDialogRCW
    End Class

    <ComImport(),
    ClassInterface(ClassInterfaceType.None),
    TypeLibType(TypeLibTypeFlags.FCanCreate),
    Guid(CLSIDGuid.KnownFolderManager)>
    Friend Class KnownFolderManagerRCW
    End Class

    Friend Class WindowHandleWrapper
        Implements IWin32Window

        Private _handle As IntPtr

        Public Sub New(handle As IntPtr)
            _handle = handle
        End Sub

        Public ReadOnly Property Handle() As IntPtr Implements IWin32Window.Handle
            Get
                Return _handle
            End Get
        End Property
    End Class

    Public Class SafeModuleHandle
        Inherits SafeHandle

        Public Sub New()
            MyBase.New(IntPtr.Zero, True)
        End Sub

        Public Overrides ReadOnly Property IsInvalid() As Boolean
            Get
                Return handle = IntPtr.Zero
            End Get
        End Property

        Protected Overrides Function ReleaseHandle() As Boolean
            Return NativeMethods.FreeLibrary(handle)
        End Function
    End Class

    Public NotInheritable Class NativeMethods
        Friend Const BS_COMMANDLINK As Integer = &HE
        Friend Const BCM_SETNOTE As Integer = &H1609
        Friend Const BCM_SETSHIELD As Integer = &H160C
        Friend Const TV_FIRST As Integer = &H1100
        Friend Const TVM_SETEXTENDEDSTYLE As Integer = TV_FIRST + 44
        Friend Const TVM_GETEXTENDEDSTYLE As Integer = TV_FIRST + 45
        Friend Const TVM_SETAUTOSCROLLINFO As Integer = TV_FIRST + 59
        Friend Const TVS_NOHSCROLL As Integer = &H8000
        Friend Const TVS_EX_AUTOHSCROLL As Integer = &H20
        Friend Const TVS_EX_FADEINOUTEXPANDOS As Integer = &H40
        Friend Const GWL_STYLE As Integer = -16

        <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
        Friend Shared Function SendMessage(hWnd As IntPtr, Msg As UInt32, wParam As Integer, lParam As Integer) As Integer
        End Function

        <DllImport("user32", CharSet:=CharSet.Unicode)>
        Friend Shared Function SendMessage(hWnd As IntPtr, Msg As UInt32, wParam As IntPtr, lParam As String) As IntPtr
        End Function

        <DllImport("uxtheme.dll", CharSet:=CharSet.Unicode)>
        Shared Function SetWindowTheme(hWnd As IntPtr, pszSubAppName As String, pszSubIdList As String) As Integer
        End Function

        Friend Const WM_USER As Integer = &H400
        Friend Const WM_ENTERIDLE As Integer = &H121

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)>
        Public Structure COMDLG_FILTERSPEC
            Friend pszName As String
            Friend pszSpec As String
        End Structure

        Public Enum FDAP
            FDAP_BOTTOM = &H0
            FDAP_TOP = &H1
        End Enum

        Public Enum FDE_SHAREVIOLATION_RESPONSE
            FDESVR_DEFAULT = &H0
            FDESVR_ACCEPT = &H1
            FDESVR_REFUSE = &H2
        End Enum

        Public Enum FDE_OVERWRITE_RESPONSE
            FDEOR_DEFAULT = &H0
            FDEOR_ACCEPT = &H1
            FDEOR_REFUSE = &H2
        End Enum

        Friend Enum SIATTRIBFLAGS
            SIATTRIBFLAGS_AND = &H1
            SIATTRIBFLAGS_OR = &H2
            SIATTRIBFLAGS_APPCOMPAT = &H3
        End Enum

        Public Enum SIGDN As UInteger
            SIGDN_NORMALDISPLAY = &H0
            SIGDN_PARENTRELATIVEPARSING = &H80018001UI
            SIGDN_DESKTOPABSOLUTEPARSING = &H80028000UI
            SIGDN_PARENTRELATIVEEDITING = &H80031001UI
            SIGDN_DESKTOPABSOLUTEEDITING = &H8004C000UI
            SIGDN_FILESYSPATH = &H80058000UI
            SIGDN_URL = &H80068000UI
            SIGDN_PARENTRELATIVEFORADDRESSBAR = &H8007C001UI
            SIGDN_PARENTRELATIVE = &H80080001UI
        End Enum

        <Flags()>
        Public Enum FOS As UInteger
            FOS_OVERWRITEPROMPT = &H2
            FOS_STRICTFILETYPES = &H4
            FOS_NOCHANGEDIR = &H8
            FOS_PICKFOLDERS = &H20
            FOS_FORCEFILESYSTEM = &H40
            FOS_ALLNONSTORAGEITEMS = &H80
            FOS_NOVALIDATE = &H100
            FOS_ALLOWMULTISELECT = &H200
            FOS_PATHMUSTEXIST = &H800
            FOS_FILEMUSTEXIST = &H1000
            FOS_CREATEPROMPT = &H2000
            FOS_SHAREAWARE = &H4000
            FOS_NOREADONLYRETURN = &H8000
            FOS_NOTESTFILECREATE = &H10000
            FOS_HIDEMRUPLACES = &H20000
            FOS_HIDEPINNEDPLACES = &H40000
            FOS_NODEREFERENCELINKS = &H100000
            FOS_DONTADDTORECENT = &H2000000
            FOS_FORCESHOWHIDDEN = &H10000000
            FOS_DEFAULTNOMINIMODE = &H20000000
        End Enum

        Friend Enum CDCONTROLSTATE
            CDCS_INACTIVE = &H0
            CDCS_ENABLED = &H1
            CDCS_VISIBLE = &H2
        End Enum

        Friend Enum FFFP_MODE
            FFFP_EXACTMATCH
            FFFP_NEARESTPARENTMATCH
        End Enum

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode, Pack:=4)>
        Friend Structure KNOWNFOLDER_DEFINITION
            Friend category As NativeMethods.KF_CATEGORY
            Friend pszName As String
            Friend pszCreator As String
            Friend pszDescription As String
            Friend fidParent As Guid
            Friend pszRelativePath As String
            Friend pszParsingName As String
            Friend pszToolTip As String
            Friend pszLocalizedName As String
            Friend pszIcon As String
            Friend pszSecurity As String
            Friend dwAttributes As UInteger
            Friend kfdFlags As NativeMethods.KF_DEFINITION_FLAGS
            Friend ftidType As Guid
        End Structure

        Friend Enum KF_CATEGORY
            KF_CATEGORY_VIRTUAL = &H1
            KF_CATEGORY_FIXED = &H2
            KF_CATEGORY_COMMON = &H3
            KF_CATEGORY_PERUSER = &H4
        End Enum

        <Flags()>
        Friend Enum KF_DEFINITION_FLAGS
            KFDF_PERSONALIZE = &H1
            KFDF_LOCAL_REDIRECT_ONLY = &H2
            KFDF_ROAMABLE = &H4
        End Enum

        <StructLayout(LayoutKind.Sequential, Pack:=4)>
        Friend Structure PROPERTYKEY
            Friend fmtid As Guid
            Friend pid As UInteger
        End Structure

        Friend Const ERROR_CANCELLED As Integer = &H800704C7

        <DllImport("kernel32.dll", CharSet:=CharSet.Unicode)>
        Shared Function LoadLibrary(name As String) As SafeModuleHandle
        End Function

        <DllImport("kernel32.dll")>
        Shared Function FreeLibrary(hModule As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
        End Function

        <DllImport("user32.dll", CharSet:=CharSet.Unicode)>
        Shared Function LoadString(hInstance As SafeModuleHandle, uID As UInteger, lpBuffer As StringBuilder, nBufferMax As Integer) As Integer
        End Function

        <DllImport("shell32.dll", CharSet:=CharSet.Unicode)>
        Shared Function SHCreateItemFromParsingName(
            pszPath As String,
            pbc As IntPtr,
            ByRef riid As Guid,
            <MarshalAs(UnmanagedType.IUnknown)> ByRef ppv As Object) As Integer
        End Function

        Shared Function CreateItemFromParsingName(path As String) As IShellItem
            Dim item As Object = Nothing
            Dim guid As New Guid("43826d1e-e718-42ee-bc55-a1e261c37bfe")
            Dim hr = NativeMethods.SHCreateItemFromParsingName(path, IntPtr.Zero, guid, item)

            If hr <> 0 Then
                Throw New Win32Exception(hr)
            End If

            Return DirectCast(item, IShellItem)
        End Function
    End Class

    Class IIDGuid
        Friend Const IModalWindow As String = "b4db1657-70d7-485e-8e3e-6fcb5a5c1802"
        Friend Const IFileDialog As String = "42f85136-db7e-439c-85f1-e4075d135fc8"
        Friend Const IFileOpenDialog As String = "d57c7288-d4ad-4768-be02-9d969532d960"
        Friend Const IFileSaveDialog As String = "84bccd23-5fde-4cdb-aea4-af64b83d78ab"
        Friend Const IFileDialogEvents As String = "973510DB-7D7F-452B-8975-74A85828D354"
        Friend Const IFileDialogControlEvents As String = "36116642-D713-4b97-9B83-7484A9D00433"
        Friend Const IFileDialogCustomize As String = "8016b7b3-3d49-4504-a0aa-2a37494e606f"
        Friend Const IShellItem As String = "43826D1E-E718-42EE-BC55-A1E261C37BFE"
        Friend Const IShellItemArray As String = "B63EA76D-1F85-456F-A19C-48159EFA858B"
        Friend Const IKnownFolder As String = "38521333-6A87-46A7-AE10-0F16706816C3"
        Friend Const IKnownFolderManager As String = "44BEAAEC-24F4-4E90-B3F0-23D258FBB146"
        Friend Const IPropertyStore As String = "886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99"
    End Class

    Class CLSIDGuid
        Friend Const FileOpenDialog As String = "DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7"
        Friend Const FileSaveDialog As String = "C0B4E2F3-BA21-4773-8DBA-335EC946EB8B"
        Friend Const KnownFolderManager As String = "4df0c730-df9d-4ae3-9153-aa6b82e9795a"
    End Class

    Class KFIDGuid
        Friend Const ComputerFolder As String = "0AC0837C-BBF8-452A-850D-79D08E667CA7"
        Friend Const Favorites As String = "1777F761-68AD-4D8A-87BD-30B759FA33DD"
        Friend Const Documents As String = "FDD39AD0-238F-46AF-ADB4-6C85480369C7"
        Friend Const Profile As String = "5E6C858F-0E22-4760-9AFE-EA3317B67173"
    End Class

    Public Enum HRESULT As Long
        S_FALSE = &H1
        S_OK = &H0
        E_INVALIDARG = &H80070057UI
        E_OUTOFMEMORY = &H8007000EUI
    End Enum
End Class
