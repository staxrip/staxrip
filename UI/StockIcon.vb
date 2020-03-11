Imports System.Runtime.InteropServices

Namespace UI
    <Flags()>
    Public Enum StockIconOptions As UInteger
        Small = &H1 'Retrieve the small version of the icon, as specified by the SM_CXSMICON and SM_CYSMICON system metrics.
        ShellSize = &H4 'Retrieve the shell-sized icons rather than the sizes specified by the system metrics.
        Handle = &H100 'The hIcon member of the SHSTOCKICONINFO structure receives a handle to the specified icon.
        SystemIndex = &H4000 'The iSysImageImage member of the SHSTOCKICONINFO structure receives the index of the specified icon in the system imagelist.
        LinkOverlay = &H8000 'Add the link overlay to the file's icon.
        Selected = &H10000 'Blend the icon with the system highlight color.
    End Enum

    Public Enum StockIconIdentifier As UInteger
        DocumentNotAssociated = 0 ' document (blank page), no associated program
        DocumentAssociated = 1 ' document with an associated program
        Application = 2 ' generic application with no custom icon
        Folder = 3 ' Folder (closed)
        FolderOpen = 4 ' Folder (open)
        Drive525 = 5 ' 5.25" floppy disk Drive
        Drive35 = 6 '3.5" floppy disk Drive
        DriveRemove = 7 'removable Drive
        DriveFixed = 8 'Fixed (hard disk) Drive
        DriveNetwork = 9 'Network Drive
        DriveNetworkDisabled = 10 'disconnected Network Drive
        DriveCD = 11 'CD Drive
        DriveRAM = 12 'RAM disk Drive
        World = 13 'entire Network
        Server = 15 'a computer on the Network
        Printer = 16 'printer
        MyNetwork = 17 'My Network places
        Find = 22 'Find
        Help = 23 'Help
        Share = 28 'overlay for shared items
        Link = 29 'overlay for shortcuts to items
        SlowFile = 30 'overlay for slow items
        Recycler = 31 'empty recycle bin
        RecyclerFull = 32 'full recycle bin
        MediaCDAudio = 40 'Audio CD Media
        Lock = 47 'Security lock
        AutoList = 49 'AutoList
        PrinterNet = 50 'Network printer
        ServerShare = 51 'Server share
        PrinterFax = 52 'Fax printer
        PrinterFaxNet = 53 'Networked Fax Printer
        PrinterFile = 54 'Print to File
        Stack = 55 'Stack
        MediaSVCD = 56 'SVCD Media
        StuffedFolder = 57 'Folder containing other items
        DriveUnknown = 58 'Unknown Drive
        DriveDVD = 59 'DVD Drive
        MediaDVD = 60 'DVD Media
        MediaDVDRAM = 61 'DVD-RAM Media
        MediaDVDRW = 62 'DVD-RW Media
        MediaDVDR = 63 'DVD-R Media
        MediaDVDROM = 64 'DVD-ROM Media
        MediaCDAudioPlus = 65 'CD+ (Enhanced CD) Media
        MediaCDRW = 66 'CD-RW Media
        MediaCDR = 67 'CD-R Media
        MediaCDBurn = 68 'Burning CD
        MediaBlankCD = 69 'Blank CD Media
        MediaCDROM = 70 'CD-ROM Media
        AudioFiles = 71 'Audio Files
        ImageFiles = 72 'Image Files
        VideoFiles = 73 'Video Files
        MixedFiles = 74 'Mixed Files
        FolderBack = 75 'Folder back
        FolderFront = 76 'Folder front
        Shield = 77 'Security shield. Use for UAC prompts only.
        Warning = 78 'Warning
        Info = 79 'Informational
        [Error] = 80 'Error
        Key = 81 'Key / Secure
        Software = 82 'Software
        Rename = 83 'Rename
        Delete = 84 'Delete
        MediaAudioDVD = 85 'Audio DVD Media
        MediaMovieDVD = 86 'Movie DVD Media
        MediaEnhancedCD = 87 'Enhanced CD Media
        MediaEnhancedDVD = 88 'Enhanced DVD Media
        MediaHDDVD = 89 'HD-DVD Media
        MediaBluRay = 90 'BluRay Media
        MediaVCD = 91 'VCD Media
        MediaDVDPlusR = 92 'DVD+R Media
        MediaDVDPlusRW = 93 'DVD+RW Media
        DesktopPC = 94 'desktop computer
        MobilePC = 95 'mobile computer (laptop/notebook)
        Users = 96 'users
        MediaSmartMedia = 97 'Smart Media
        MediaCompactFlash = 98 'Compact Flash
        DeviceCellPhone = 99 'Cell phone
        DeviceCamera = 100 'Camera
        DeviceVideoCamera = 101 'Video camera
        DeviceAudioPlayer = 102 'Audio player
        NetworkConnect = 103 'Connect to Network
        Internet = 104 'InterNet
        ZipFile = 105 'ZIP File
        Settings = 106 'Settings
    End Enum

    Friend NotInheritable Class StockIcon
        Private Sub New()
        End Sub

        <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
        Private Structure StockIconInfo
            Friend StuctureSize As UInt32
            Friend Handle As IntPtr
            Friend ImageIndex As Int32
            Friend Identifier As Int32
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=260)>
            Friend Path As String
        End Structure

        <DllImport("Shell32.dll")>
        Private Shared Function SHGetStockIconInfo(
            identifier As StockIconIdentifier,
            flags As StockIconOptions,
            ByRef info As StockIconInfo) As Integer
        End Function

        <DllImport("User32.dll")>
        Private Shared Function DestroyIcon(handle As IntPtr) As Boolean
        End Function

        Shared Function GetSmallImage(identifier As StockIconIdentifier) As Bitmap
            Dim ptr = GetIcon(identifier, StockIconOptions.Handle Or StockIconOptions.Small)
            Dim bmp = Icon.FromHandle(ptr).ToBitmap
            DestroyIcon(ptr)
            Return bmp
        End Function

        Shared Function GetImage(identifier As StockIconIdentifier) As Image
            Dim ptr = GetIcon(identifier, StockIconOptions.Handle Or StockIconOptions.ShellSize)
            Dim bmp = Icon.FromHandle(ptr).ToBitmap
            DestroyIcon(ptr)
            Return bmp
        End Function

        Private Shared Function GetIcon(identifier As StockIconIdentifier, flags As StockIconOptions) As IntPtr
            Dim info As New StockIconInfo()
            info.StuctureSize = CType(Marshal.SizeOf(GetType(StockIconInfo)), UInt32)
            Marshal.ThrowExceptionForHR(SHGetStockIconInfo(identifier, flags, info))
            Return info.Handle
        End Function
    End Class
End Namespace
