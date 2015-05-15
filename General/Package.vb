Imports Microsoft.Win32

Public Class Packs
    Public Shared autocrop As New AutoCropPackage
    Public Shared AviSynth As New AviSynthPackage
    Public Shared avs4x26x As New avs4x26xPackage
    Public Shared AVSMeter As New AVSMeterPackage
    Public Shared BDSup2SubPP As New BDSup2SubPackage
    Public Shared BeSweet As New BeSweetPackage
    Public Shared checkmate As New checkmatePackage
    'Public Shared DGDecode As New DGDecodePackage
    Public Shared DGDecodeIM As New DGDecodeIMPackage
    Public Shared DGDecodeNV As New DGDecodeNVPackage
    Public Shared DGIndex As New DGIndexPackage
    Public Shared DGIndexIM As New DGIndexIMPackage
    Public Shared DGIndexNV As New DGIndexNVPackage
    Public Shared DivX265 As New DivX265Package
    Public Shared dsmux As New dsmuxPackage
    Public Shared DSS2 As New DSS2Package
    Public Shared eac3to As New eac3toPackage
    Public Shared ffmpeg As New ffmpegPackage
    Public Shared ffms2 As New ffms2Package
    Public Shared Haali As New HaaliSplitter
    Public Shared Java As New JavaPackage
    Public Shared lsmashWorks As New LSmashWorksPackage
    Public Shared masktools2 As New masktools2Package
    Public Shared MediaInfo As New MediaInfoPackage
    Public Shared Mkvmerge As New MKVToolNixPackage
    Public Shared MP4Box As New MP4BoxPackage
    Public Shared MPC As New MPCPackage
    Public Shared mvtools2 As New mvtools2Package
    Public Shared NeroAACEnc As New NeroAACEncPackage
    Public Shared NicAudio As New NicAudioPackage
    Public Shared nnedi3 As New nnedi3Package
    Public Shared NVEncC As New NVEncCPackage
    Public Shared ProjectX As New ProjectXPackage
    Public Shared qaac As New qaacPackage
    Public Shared QSVEncC As New QSVEncCPackage
    Public Shared QTGMC As New QTGMCPackage
    Public Shared RgTools As New RgToolsPackage
    Public Shared SangNom2 As New SangNom2Package
    Public Shared TDeint As New TDeintPackage
    Public Shared UnDot As New UnDotPackage
    Public Shared VSFilter As New VSFilterPackage
    Public Shared VSRip As New VSRipPackage
    Public Shared x264 As New x264Package
    Public Shared x265 As New x265Package
    Public Shared xvid_encraw As New xvid_encrawPackage
    Public Shared DGAVCIndex As New DGAVCIndexPackage
    Public Shared flash3kyuu_deband As New flash3kyuu_debandPackage

    Public Shared Property Packages As New Dictionary(Of String, Package)

    Shared Sub Init()
        AddPackage(flash3kyuu_deband)
        AddPackage(DGAVCIndex)
        AddPackage(autocrop)
        AddPackage(AviSynth)
        AddPackage(avs4x26x)
        AddPackage(AVSMeter)
        AddPackage(BDSup2SubPP)
        AddPackage(BeSweet)
        AddPackage(checkmate)
        'AddPackage(DGDecode)
        AddPackage(DGDecodeIM)
        AddPackage(DGDecodeNV)
        AddPackage(DGIndex)
        AddPackage(DGIndexIM)
        AddPackage(DGIndexNV)
        AddPackage(DivX265)
        AddPackage(dsmux)
        AddPackage(DSS2)
        AddPackage(eac3to)
        AddPackage(ffmpeg)
        AddPackage(ffms2)
        AddPackage(Haali)
        AddPackage(Java)
        AddPackage(lsmashWorks)
        AddPackage(masktools2)
        AddPackage(MediaInfo)
        AddPackage(Mkvmerge)
        AddPackage(MP4Box)
        AddPackage(MPC)
        AddPackage(mvtools2)
        AddPackage(NeroAACEnc)
        AddPackage(NicAudio)
        AddPackage(nnedi3)
        AddPackage(NVEncC)
        AddPackage(ProjectX)
        AddPackage(qaac)
        AddPackage(QSVEncC)
        AddPackage(QTGMC)
        AddPackage(RgTools)
        AddPackage(SangNom2)
        AddPackage(TDeint)
        AddPackage(UnDot)
        AddPackage(VSFilter)
        AddPackage(VSRip)
        AddPackage(x264)
        AddPackage(x265)
        AddPackage(xvid_encraw)

        Dim fp = CommonDirs.Startup + "Tools\Versions.txt"

        If File.Exists(fp) Then
            For Each i In File.ReadAllLines(CommonDirs.Startup + "Tools\Versions.txt")
                For Each i2 In Packages.Values
                    If i Like "*=*;*" Then
                        Dim name = i.Left("=").Trim

                        If name = i2.Name Then
                            i2.Version = i.Right("=").Left(";").Trim
                            Dim a = i.Right("=").Right(";").Trim.Split("-"c)
                            i2.VersionDate = New DateTime(CInt(a(0)), CInt(a(1)), CInt(a(2)))
                        End If
                    End If
                Next
            Next
        End If
    End Sub

    Shared Sub AddPackage(package As Package)
        Packages.Add(package.Name, package)
    End Sub
End Class

Public Class Package
    Property Name As String
    Property FileNotFoundMessage As String
    Property SetupAction As Action
    Property Version As String
    Property WebURL As String
    Property Description As String
    Property VersionDate As DateTime
    Property TreePath As String = "Tools"
    Property HelpFile As String
    Property HelpDir As String
    Property HelpURL As String
    Property FixedDir As String
    Property Filenames As String()

    Private FilenameValue As String

    Overridable Property Filename As String
        Get
            If FilenameValue = "" AndAlso OK(Filenames) Then
                FilenameValue = Filenames(0)
            End If

            Return FilenameValue
        End Get
        Set(value As String)
            FilenameValue = value
        End Set
    End Property

    Protected LaunchName As String

    Overridable ReadOnly Property LaunchAction As Action
        Get
            If LaunchName <> "" Then
                Return Sub() g.ShellExecute(GetDir() + LaunchName)
            End If
        End Get
    End Property

    Overridable ReadOnly Property LaunchTitle As String
        Get
            Return Name
        End Get
    End Property

    Protected IsRequiredValue As Boolean = True

    Overridable ReadOnly Property IsRequired() As Boolean
        Get
            Return IsRequiredValue
        End Get
    End Property

    Sub LaunchWithJava()
        Try
            Dim p As New Process
            p.StartInfo.FileName = Packs.Java.GetDir + "javaw.exe"
            p.StartInfo.Arguments = "-jar """ + GetPath() + """"
            p.StartInfo.WorkingDirectory = GetDir()
            p.Start()
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub

    Function GetHelpPath() As String
        If HelpFile <> "" Then
            Return GetDir() + HelpFile
        ElseIf HelpDir <> "" Then
            Return GetDir() + HelpDir
        ElseIf HelpURL <> "" Then
            Return HelpURL
        End If
    End Function

    Function VerifyOK(Optional showAnyway As Boolean = False) As Boolean
        If (IsRequired() OrElse showAnyway) AndAlso IsStatusCritical() Then
            Using f As New ApplicationsForm
                f.ShowPackage(Me)
                f.ShowDialog()
                g.MainForm.Refresh()
            End Using

            If IsStatusCritical() Then Return False
        End If

        Return True
    End Function

    Function IsStatusCritical() As Boolean
        Return GetStatusLocation() <> "" OrElse GetStatus() <> ""
    End Function

    Overridable Function GetStatus() As String
    End Function

    Function GetStatusDisplay() As String
        Dim ret = GetStatusLocation()

        If ret <> "" Then Return ret

        ret = GetStatus()

        If ret <> "" Then Return ret

        ret = GetStatusVersion()

        If ret <> "" Then Return ret

        Return "OK"
    End Function

    Function GetStatusVersion() As String
        If Version <> "" AndAlso Not IsCorrectVersion(GetPath) Then
            Dim text = "Unknown version, use at your own risk, press F12 to edit the version."

            If SetupAction Is Nothing Then
                Return text
            Else
                Return text + " In case of problems click the 'Setup' button above to install the recommended version."
            End If
        End If
    End Function

    Function GetStatusLocation() As String
        Dim path = GetPath()

        If path Is Nothing Then
            If FileNotFoundMessage <> "" Then
                Return FileNotFoundMessage
            ElseIf Not SetupAction Is Nothing Then
                Return "Please install " + Name + "."
            End If

            Return "Application not found, please redownload StaxRip at SourceForge."
        End If

        If FixedDir <> "" AndAlso path <> "" AndAlso Not path.ToLower.StartsWith(FixedDir.ToLower) Then
            Return "The application has to be located at: " + FixedDir
        End If
    End Function

    Overridable Function IsCorrectVersion(path As String) As Boolean
        If path <> "" Then
            Dim dt = File.GetLastWriteTimeUtc(path)
            Return dt.AddDays(-2) < VersionDate AndAlso dt.AddDays(2) > VersionDate
        End If
    End Function

    Function GetDir() As String
        Return Filepath.GetDir(GetPath)
    End Function

    Overridable Function GetPath() As String
        Dim ret As String

        If Not s Is Nothing AndAlso Not s.Storage Is Nothing Then
            ret = s.Storage.GetString(Name + "custom path")

            If ret <> "" Then
                If File.Exists(ret) Then
                    Return ret
                Else
                    s.Storage.SetString(Name + "custom path", Nothing)
                End If
            End If
        End If

        If FixedDir <> "" Then
            If File.Exists(FixedDir + Filename) Then Return FixedDir + Filename
            Return Nothing
        End If

        If TypeOf Me Is AviSynthPluginPackage Then
            For Each i In {
                CommonDirs.Startup + "Tools\Plugins\" + Filename,
                CommonDirs.Startup + "Tools\Plugins\" + Name + "\" + Filename}

                If File.Exists(i) Then Return i
            Next
        End If

        ret = CommonDirs.Startup + "Tools\" + Name + "\" + Filename
        If File.Exists(ret) Then Return ret

        ret = Registry.CurrentUser.GetString("Software\" + Application.ProductName + "\Tool Paths", Name)

        If ret <> "" Then
            If File.Exists(ret) Then
                If Not s Is Nothing AndAlso Not s.Storage Is Nothing Then
                    s.Storage.SetString(Name + "custom path", ret)
                End If

                Return ret
            Else
                Registry.CurrentUser.DeleteValue("Software\" + Application.ProductName + "\Tool Paths", Name)
            End If
        End If
    End Function

    Overrides Function ToString() As String
        Return Name
    End Function
End Class

Public Class AutoCropPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "AutoCrop"
        Filename = "AutoCrop.dll"
        WebURL = "http://avisynth.org.ru/docs/english/externalfilters/autocrop.htm"
        HelpURL = "http://avisynth.org.ru/docs/english/externalfilters/autocrop.htm"
        Description = "AutoCrop is an AviSynth filter that automatically crops the black borders from a clip. It operates in either preview mode where it overlays the recommended cropping information on the existing clip, or cropping mode where it really crops the clip."
        FilterNames = {"AutoCrop"}
    End Sub
End Class

Public Class VSFilterPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "VSFilter"
        Filename = "VSFilter.dll"
        Description = "AviSynth subtitle plugin. The format of the subtitles can be *.sub, *.srt, *.ssa, *.ass, etc. (ssa = Sub Station Alpha)."
        WebURL = "http://avisynth.org.ru/docs/english/externalfilters/vsfilter.htm"
        HelpURL = "http://avisynth.org.ru/docs/english/externalfilters/vsfilter.htm"
        FilterNames = {"VobSub", "TextSub"}
    End Sub
End Class

Public Class UnDotPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "UnDot"
        Filename = "UnDot.dll"
        WebURL = "http://avisynth.nl/index.php/UnDot"
        HelpURL = "http://avisynth.nl/index.php/UnDot"
        Description = "UnDot is a simple median filter for removing dots, that is stray orphan pixels and mosquito noise."
        FilterNames = {"UnDot"}
    End Sub
End Class

Public Class NicAudioPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "NicAudio"
        Filename = "NicAudio.dll"
        WebURL = "http://www.codeplex.com/NicAudio"
        HelpURL = "http://avisynth.org.ru/docs/english/externalfilters/nicaudio.htm"
        Description = "AviSynth audio source filter."
        FilterNames = {"NicAC3Source", "NicDTSSource", "NicMPASource", "RaWavSource"}
    End Sub
End Class

Public Class AviSynthPackage
    Inherits Package

    Sub New()
        Name = "AviSynth+"
        Filename = "avisynth.dll"
        WebURL = "http://avisynth.nl"
        Description = "AviSynth+ is a powerful scripting language used to transform audio and video. The scripts are saved as AVS files which applications like x264 can use as input file like it would be a normal AVI file."
        HelpURL = "http://avisynth.nl/index.php/Main_Page"
        FixedDir = CommonDirs.System
        SetupAction = Sub() g.ShellExecute(CommonDirs.Startup + "Tools\AviSynth+_v0.1.0_r1825-MT.exe")
    End Sub

    Public Overrides Function GetStatus() As String
        If Not Directory.Exists(Paths.AviSynthPluginsDir) Then
            Return "The AviSynth setup is damaged because the plugins directory doesn't exist. Please click the 'Setup' button above."
        End If
    End Function
End Class

Public MustInherit Class AviSynthPluginPackage
    Inherits Package

    Property FilterNames As String()
    Property Dependencies As String()

    Public Sub New()
        TreePath = "Filters"
    End Sub
End Class

Public Class BeSweetPackage
    Inherits Package

    Sub New()
        Name = "BeSweet"
        Filename = "BeSweet.exe"
        WebURL = "http://dspguru.doom9.net"
        Description = "Alternative audio converter, for most formats StaxRip uses now eac3to by default."
        HelpDir = "help"
    End Sub

    Overrides Function GetStatus() As String
        For Each i In {p.Audio0, p.Audio1}
            If TypeOf i Is BatchAudioProfile Then
                Dim profile = DirectCast(i, BatchAudioProfile)

                If profile.CommandLines.Contains("-lame(") Then
                    If Not CheckLib(GetDir() + "lame_enc.dll", New DateTime(2005, 12, 22)) Then
                        Return "There is a problem with the lame library."
                    End If
                End If

                If profile.CommandLines.Contains("-bsn(") Then
                    If Not CheckLib(GetDir() + "bsn.dll", New DateTime(2006, 5, 22)) Then
                        Return "There is a problem with the bsn library."
                    End If
                End If
            End If
        Next
    End Function

    Private Function CheckLib(filename As String, dt As DateTime) As Boolean
        Return File.Exists(filename) AndAlso File.GetLastWriteTime(filename) > dt.AddDays(-2)
    End Function
End Class

Public Class VSRipPackage
    Inherits Package

    Sub New()
        Name = "VSRip"
        Filename = "VSRip.exe"
        Description = "VSRip rips VobSub subtitles."
        WebURL = "http://sourceforge.net/projects/guliverkli"
        LaunchName = Filename
    End Sub
End Class

Public Class NeroAACEncPackage
    Inherits Package

    Sub New()
        Name = "Nero AAC Encoder"
        Filename = "neroAacEnc.exe"
        Description = "Free AAC encoder"
        WebURL = "http://www.nero.com/enu/downloads-nerodigital-nero-aac-codec.php"
        HelpFile = "nero readme.txt"
        FixedDir = CommonDirs.Startup + "Tools\BeSweet\"
    End Sub
End Class

Public Class JavaPackage
    Inherits Package

    Sub New()
        Name = "Java"
        Filename = "Java.exe"
        WebURL = "http://java.com"
        Description = "Java is required by ProjectX. " + Strings.ProjectX
        FileNotFoundMessage = Strings.InstallManually
    End Sub

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return Packs.ProjectX.IsRequired
        End Get
    End Property

    Public Overrides Function GetPath() As String
        Dim ret = MyBase.GetPath()
        If ret <> "" Then Return ret

        ret = "C:\ProgramData\Oracle\Java\javapath\" + Filename
        If File.Exists(ret) Then Return ret

        ret = "C:\Windows\Sysnative\" + Filename
        If File.Exists(ret) Then Return ret

        ret = "C:\Windows\System32\" + Filename
        If File.Exists(ret) Then Return ret
    End Function
End Class

Public Class ProjectXPackage
    Inherits Package

    Sub New()
        Name = "ProjectX"
        Filename = "ProjectX.jar"
        WebURL = "http://project-x.sourceforge.net"
        Description = Strings.ProjectX
    End Sub

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return CommandLineDemuxer.IsActive("ProjectX")
        End Get
    End Property

    Overrides ReadOnly Property LaunchAction As Action
        Get
            Return AddressOf LaunchWithJava
        End Get
    End Property

    Public Overrides Function GetStatus() As String
        If Packs.Java.GetPath = "" Then Return "Failed to locate Java, ProjectX requires Java."
        Return MyBase.GetStatus()
    End Function
End Class

Public Class x264Package
    Inherits Package

    Sub New()
        Name = "x264"
        Filename = "x264.exe"
        WebURL = "http://www.videolan.org/developers/x264.html"
        Description = "H.264 video encoding command line application."
        HelpFile = "Help.txt"
    End Sub
End Class

Public Class x265Package
    Inherits Package

    Sub New()
        Name = "x265"
        Filename = "x265.exe"
        WebURL = "http://x265.org"
        Description = "H.265 video encoding command line application."
        HelpURL = "http://x265.readthedocs.org"
    End Sub
End Class

Public Class avs4x26xPackage
    Inherits Package

    Sub New()
        Name = "avs4x26x"
        Filename = "avs4x26x.exe"
        WebURL = "http://forum.doom9.org/showthread.php?t=162656"
        Description = "Pipes video from AviSynth to x264/x265."
    End Sub
End Class

Public Class MP4BoxPackage
    Inherits Package

    Sub New()
        Name = "MP4Box"
        Filename = "MP4Box.exe"
        WebURL = "http://gpac.wp.mines-telecom.fr/"
        Description = "MP4Box is a MP4 Muxer."
        HelpURL = "http://gpac.wp.mines-telecom.fr/mp4box/mp4box-documentation"
    End Sub
End Class

Public Class MKVToolNixPackage
    Inherits Package

    Sub New()
        Name = "MKVToolNix"
        Filename = "mkvmerge.exe"
        WebURL = "http://www.bunkus.org/videotools/mkvtoolnix"
        Description = "MKVtoolnix contains mkvmerge and mkvextract to mux and demux Matroska (MKV) files."
        HelpFile = "doc\command_line_references_and_guide.html"
        LaunchName = "mkvtoolnix-gui.exe"
    End Sub

    Public Overrides ReadOnly Property LaunchTitle As String
        Get
            Return "MKVToolNix GUI"
        End Get
    End Property
End Class

Public Class MediaInfoPackage
    Inherits Package

    Sub New()
        Name = "MediaInfo"
        Filename = "MediaInfo.dll"
        WebURL = "http://mediainfo.sourceforge.net"
        Description = "MediaInfo is used by StaxRip to read infos from media files."
    End Sub
End Class

Public Class ffmpegPackage
    Inherits Package

    Sub New()
        Name = "ffmpeg"
        Filename = "ffmpeg.exe"
        WebURL = "http://ffmpeg.org"
        Description = "Versatile audio video converter."
        HelpURL = "https://www.ffmpeg.org/ffmpeg-all.html"
    End Sub
End Class

Public Class eac3toPackage
    Inherits Package

    Sub New()
        Name = "eac3to"
        Filename = "eac3to.exe"
        WebURL = "http://forum.doom9.org/showthread.php?t=125966"
        Description = "Audio conversion command line application."
        HelpURL = "http://en.wikibooks.org/wiki/Eac3to/How_to_Use"
    End Sub
End Class

Public Class ffms2Package
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "ffms2"
        Filename = "ffms2.dll"
        WebURL = "https://github.com/FFMS/ffms2"
        Description = "AviSynth source filter supporting various input formats."
        FilterNames = {"FFVideoSource", "FFAudioSource"}
        HelpDir = "doc"
    End Sub
End Class

Public Class LSmashWorksPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "L-SMASH-Works"
        Filename = "LSMASHSource.dll"
        WebURL = "http://avisynth.nl/index.php/LSMASHSource"
        Description = "AviSynth source filter supporting various input formats."
        HelpFile = "README.txt"
        FilterNames = {"LSMASHVideoSource",
                       "LSMASHAudioSource",
                       "LWLibavVideoSource",
                       "LWLibavAudioSource"}
    End Sub

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return p.AvsDoc.GetFilter("Source").Script.Contains("LSMASHVideoSource") OrElse p.AvsDoc.GetFilter("Source").Script.Contains("LWLibavVideoSource")
        End Get
    End Property

    Public Overrides Function GetStatus() As String
        If Not File.Exists(CommonDirs.System + "MSVCR120.dll") Then
            Return "Failed to locate the required runtime 'Visual C++ Redistributable Packages for Visual Studio 2013', please download and install it manually."
        End If
    End Function
End Class

Public Class qaacPackage
    Inherits Package

    Sub New()
        Name = "qaac"
        Filename = "qaac64.exe"
        WebURL = "https://sites.google.com/site/qaacpage"
        Description = "qaac is a command line AAC encoder frontend based on the Apple AAC encoder. qaac requires libflac which StaxRip includes and it requires AppleApplicationSupport64.msi which can be extracted from the x64 iTunes installer using 7-Zip. The makeportable script found on the qaac website can also be used."
    End Sub

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return TypeOf p.Audio0 Is GUIAudioProfile AndAlso
                DirectCast(p.Audio0, GUIAudioProfile).Params.Encoder = GuiAudioEncoder.qaac OrElse
                TypeOf p.Audio1 Is GUIAudioProfile AndAlso
                DirectCast(p.Audio1, GUIAudioProfile).Params.Encoder = GuiAudioEncoder.qaac
        End Get
    End Property

    Overrides Function GetStatus() As String
        Dim path = CommonDirs.Programs + "Common Files\Apple\Apple Application Support\CoreAudioToolbox.dll"

        If Not File.Exists(path) AndAlso Not File.Exists(GetDir() + "QTfiles64\CoreAudioToolbox.dll") Then
            Return "Failed to locate CoreAudioToolbox, read the description below. Expected paths:" +
                CrLf2 + path + CrLf2 + GetDir() + "QTfiles64\CoreAudioToolbox.dll"
        End If
    End Function
End Class

Public Class checkmatePackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "checkmate"
        Filename = "checkmate.dll"
        WebURL = "http://github.com/tp7/checkmate"
        HelpURL = "http://github.com/tp7/checkmate"
        Description = "Spatial and temporal dot crawl reducer. Checkmate is most effective in static or low motion scenes. When using in high motion scenes (or areas) be careful, it's known to cause artifacts with its default values."
        FilterNames = {"checkmate"}
    End Sub
End Class

Public Class SangNom2Package
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "SangNom2"
        Filename = "SangNom2.dll"
        WebURL = "http://avisynth.nl/index.php/SangNom2"
        HelpURL = "http://avisynth.nl/index.php/SangNom2"
        Description = "SangNom2 is a reimplementation of MarcFD's old SangNom filter. Originally it's a single field deinterlacer using edge-directed interpolation but nowadays it's mainly used in anti-aliasing scripts. The output is not completely but mostly identical to the original SangNom."
        FilterNames = {"SangNom2"}
    End Sub
End Class

Public Class DSS2Package
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "DSS2"
        Filename = "DSS2.dll"
        WebURL = "http://code.google.com/p/xvid4psp/downloads/detail?name=DSS2%20mod%20%2B%20LAVFilters.7z&can=2&q="
        Description = "Direct Show source filter"
        FilterNames = {"DSS2"}
    End Sub
End Class

Public Class DGIndexNVPackage
    Inherits Package

    Sub New()
        Name = "DGIndexNV"
        Filename = "DGIndexNV.exe"
        WebURL = "http://neuron2.net/dgdecnv/dgdecnv.html"
        Description = Strings.DGDecNV
        HelpFile = "DGIndexNVManual.html"
        LaunchName = Filename
        FileNotFoundMessage = "Application not found, please locate it by pressing F11 or disable the DGIndexNV feature under Tools/Settings/Demux."
    End Sub

    Overrides Function GetStatus() As String
        If Not File.Exists(GetDir() + "License.txt") Then
            Return "DGDecNV is shareware requiring a license file but the file is missing."
        End If
    End Function

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return CommandLineDemuxer.IsActive("DGIndexNV")
        End Get
    End Property
End Class

Public Class DGDecodeNVPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "DGDecodeNV"
        Filename = "DGDecodeNV.dll"
        WebURL = "http://neuron2.net/dgdecnv/dgdecnv.html"
        Description = Strings.DGDecNV
        HelpFile = "DGDecodeNVManual.html"
        FilterNames = {"DGSource", "DGMultiSource"}
        FileNotFoundMessage = "Application not found, please locate it by pressing F11 or disable the DGIndexNV feature under Tools/Settings/Demux."
    End Sub

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return CommandLineDemuxer.IsActive("DGIndexNV")
        End Get
    End Property
End Class

Public Class DGIndexIMPackage
    Inherits Package

    Sub New()
        Name = "DGIndexIM"
        Filename = "DGIndexIM.exe"
        WebURL = "http://rationalqm.us/mine.html"
        Description = Strings.DGDecIM
        HelpFile = "Notes.txt"
        FileNotFoundMessage = "Application not found, please locate it by pressing F11 or disable the DGIndexIM feature under Tools/Settings/Demux."
    End Sub

    Overrides Function GetStatus() As String
        If Not File.Exists(GetDir() + "License.txt") Then
            Return "DGIndexIM is shareware requiring a license file but the file is missing."
        End If
    End Function

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return CommandLineDemuxer.IsActive("DGIndexIM")
        End Get
    End Property
End Class

Public Class DGDecodeIMPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "DGDecodeIM"
        Filename = "DGDecodeIM.dll"
        WebURL = "http://rationalqm.us/mine.html"
        Description = Strings.DGDecIM
        HelpFile = "Notes.txt"
        FilterNames = {"DGSourceIM"}
        FileNotFoundMessage = "Application not found, please locate it by pressing F11 or disable the DGIndexIM feature under Tools/Settings/Demux."
    End Sub

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return CommandLineDemuxer.IsActive("DGIndexIM")
        End Get
    End Property
End Class

Public Class BDSup2SubPackage
    Inherits Package

    Sub New()
        Name = "BDSup2Sub++"
        Filename = "bdsup2sub++.exe"
        LaunchName = Filename
        WebURL = "http://forum.doom9.org/showthread.php?p=1613303"
        Description = "Converts Blu-ray subtitles to other formats like VobSub."
    End Sub
End Class

Public Class NVEncCPackage
    Inherits Package

    Sub New()
        Name = "NVEncC"
        Filename = "NVEncC.exe"
        WebURL = "http://forum.videohelp.com/threads/370223-NVEncC-by-rigaya-NVIDIA-GPU-encoding"
        Description = "nvidia GPU accelerated H.264/H.265 encoder."
        HelpFile = "help.txt"
    End Sub
End Class

Public Class QSVEncCPackage
    Inherits Package

    Sub New()
        Name = "QSVEncC"
        Filename = "QSVEncC64.exe"
        Description = "Intel Quick Sync GPU accelerated H.264 encoder."
        HelpFile = "help.txt"
        WebURL = "https://onedrive.live.com/?cid=6bdd4375ac8933c6&id=6BDD4375AC8933C6!482"
    End Sub
End Class

Public Class AVSMeterPackage
    Inherits Package

    Sub New()
        Name = "AVSMeter"
        Filename = "AVSMeter64.exe"
        Description = "AVSMeter runs an Avisynth script with virtually no overhead, displays clip info, CPU and memory usage and the minimum, maximum and average frames processed per second. It measures how fast Avisynth can serve frames to a client application like x264 and comes in handy when testing filters/plugins to evaluate their performance and memory requirements."
        HelpFile = "doc\AVSMeter.html"
        WebURL = "http://forum.doom9.org/showthread.php?t=165528"
    End Sub
End Class

Public Class DivX265Package
    Inherits Package

    Sub New()
        Name = "DivX265"
        Filename = "DivX265.exe"
        Description = "DivX H265 command line encoder"
        HelpFile = "help.txt"
        WebURL = "http://labs.divx.com/term/HEVC"
    End Sub
End Class

Public Class xvid_encrawPackage
    Inherits Package

    Sub New()
        Name = "xvid_encraw"
        Filename = "xvid_encraw.exe"
        Description = "XviD command line encoder"
        HelpFile = "help.txt"
    End Sub
End Class

Public Class HaaliSplitter
    Inherits Package

    Sub New()
        Name = "Haali Splitter"
        Filename = "splitter.ax"
        WebURL = "http://haali.su/mkv"
        SetupAction = Sub() g.ShellExecute(CommonDirs.Startup + "Tools\MatroskaSplitter.exe")
        Description = "Haali Splitter is used by eac3to and dsmux to write MKV files. Haali Splitter and LAV Filters overrite each other, most people prefer LAV Filters, therefore it's recommended to install Haali first and LAV Filters last."
    End Sub

    Public Overrides Function GetPath() As String
        Dim ret = Registry.ClassesRoot.GetString("CLSID\" + GUIDS.HaaliMuxer.ToString + "\InprocServer32", Nothing)
        If File.Exists(ret) Then Return ret
    End Function

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return CommandLineDemuxer.IsActive("dsmux")
        End Get
    End Property
End Class

Public Class dsmuxPackage
    Inherits Package

    Sub New()
        Name = "dsmux"
        Filename = "dsmux.x64.exe"
        Description = Strings.dsmux
        WebURL = "http://haali.su/mkv"
        SetupAction = Sub() g.ShellExecute(CommonDirs.Startup + "Tools\MatroskaSplitter.exe")
    End Sub

    Public Overrides Function GetPath() As String
        Dim ret = Registry.ClassesRoot.GetString("CLSID\" + GUIDS.HaaliMuxer.ToString + "\InprocServer32", Nothing)
        ret = Filepath.GetDir(ret) + Filename
        If File.Exists(ret) Then Return ret
    End Function

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return CommandLineDemuxer.IsActive("dsmux")
        End Get
    End Property
End Class

Public Class DGDecodePackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "DGDecode"
        Filename = "DGDecode.dll"
        WebURL = "http://www.neuron2.net/dgmpgdec/dgmpgdec.html"
        Description = Strings.DGMPGDec
        FilterNames = {"MPEG2Source", "Deblock"}
        HelpFile = "DGDecodeManual.html"
        FixedDir = CommonDirs.Startup + "Tools\DGMPGDec\"
    End Sub
End Class

Public Class DGIndexPackage
    Inherits Package

    Sub New()
        Name = "DGIndex"
        Filename = "DGIndex.exe"
        WebURL = "http://www.neuron2.net/dgmpgdec/dgmpgdec.html"
        Description = Strings.DGMPGDec
        HelpFile = "DGIndexManual.html"
        LaunchName = Filename
        FixedDir = CommonDirs.Startup + "Tools\DGMPGDec\"
    End Sub
End Class

Public Class MPCPackage
    Inherits Package

    Sub New()
        Name = "MPC Player"
        Filenames = {"mpc-be64.exe", "mpc-hc64.exe"}
        Description = "MPC is a open source media player with built in playback support for all common media formats. MPC-HC or MPC-BE can be used, x64 is absolutely required because StaxRip supports only AviSynth+ x64. StaxRip uses MPC's /dub and /sub CLI switches."
        WebURL = "http://mpc-hc.org"
        HelpURL = "http://forum.doom9.org/showthread.php?p=1719479&goto=newpost"
        IsRequiredValue = False
        FileNotFoundMessage = "MPC player could not be found, please locate it by pressing F11."
    End Sub

    Public Overrides ReadOnly Property LaunchAction As Action
        Get
            Return Sub() g.ShellExecute(GetPath)
        End Get
    End Property
End Class

Public Class TDeintPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "TDeint"
        Filename = "TDeint.dll"
        WebURL = "http://avisynth.nl/index.php/TDeint"
        HelpURL = "http://avisynth.nl/index.php/TDeint"
        Description = "TDeint is a bi-directionally, motion adaptive, sharp deinterlacer. It can adaptively choose between using per-field and per-pixel motion adaptivity, and can use cubic interpolation, kernel interpolation (with temporal direction switching), or one of two forms of modified ELA interpolation which help to reduce ""jaggy"" edges in moving areas where interpolation must be used."
        FilterNames = {"TDeint"}
    End Sub
End Class

Public Class nnedi3Package
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "nnedi3"
        Filename = "nnedi3.dll"
        WebURL = "http://forum.doom9.org/showthread.php?t=170083"
        HelpFile = "Readme.txt"
        Description = "nnedi3 is an intra-field only deinterlacer. It takes in a frame, throws away one field, and then interpolates the missing pixels using only information from the kept field."
        FilterNames = {"nnedi3"}
    End Sub
End Class

Public Class mvtools2Package
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "mvtools2"
        Filename = "mvtools2.dll"
        WebURL = "http://avisynth.org.ru/mvtools/mvtools2.html"
        HelpFile = "mvtools2.html"
        Description = "MVTools plugin for AviSynth 2.5 is collection of functions for estimation and compensation of objects motion in video clips. Motion compensation may be used for strong temporal denoising, advanced framerate conversions, image restoration and other tasks."
        FilterNames = {"MSuper", "MAnalyse", "MCompensate", "MMask", "MDeGrain1", "MDeGrain2", "MDegrain3"}
    End Sub
End Class

Public Class masktools2Package
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "masktools2"
        Filename = "masktools2.dll"
        WebURL = "http://avisynth.nl/index.php/MaskTools2"
        HelpURL = "http://avisynth.nl/index.php/MaskTools2"
        Description = "MaskTools2 contain a set of filters designed to create, manipulate and use masks. Masks, in video processing, are a way to give a relative importance to each pixel. You can, for example, create a mask that selects only the green parts of the video, and then replace those parts with another video."
        FilterNames = {"Mt_edge", "Mt_motion"}
    End Sub
End Class

Public Class RgToolsPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "RgTools"
        Filename = "RgTools.dll"
        WebURL = "http://avisynth.nl/index.php/RgTools"
        HelpURL = "http://avisynth.nl/index.php/RgTools"
        Description = "RgTools is a modern rewrite of RemoveGrain, Repair, BackwardClense, Clense, ForwardClense and VerticalCleaner all in a single plugin."
        FilterNames = {"RemoveGrain", "Clense", "ForwardClense", "BackwardClense", "Repair", "VerticalCleaner"}
    End Sub
End Class

Public Class flash3kyuu_debandPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "flash3kyuu_deband"
        Filename = "flash3kyuu_deband.dll"
        WebURL = "http://forum.doom9.org/showthread.php?t=161411"
        HelpFile = "flash3kyuu_deband.txt"
        Description = "Simple debanding filter that can be quite effective for some anime sources."
        FilterNames = {"f3kdb"}
    End Sub
End Class

Public Class QTGMCPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "QTGMC"
        Filename = "QTGMC.avsi"
        WebURL = "http://avisynth.nl/index.php/QTGMC"
        HelpFile = "QTGMC.html"
        Description = "A very high quality deinterlacer with a range of features for both quality and convenience. These include a simple presets system, extensive noise processing capabilities, support for repair of progressive material, precision source matching, shutter speed simulation, etc. Originally based on TempGaussMC by Didée."
        FilterNames = {"QTGMC"}
        Dependencies = {"masktools2", "mvtools2", "nnedi3", "RgTools"}
    End Sub
End Class

Public Class DGAVCIndexPackage
    Inherits Package

    Sub New()
        Name = "DGAVCIndex"
        Filename = "DGAVCIndex.exe"
        Description = Strings.DGAVCIndex
        HelpFile = "DGAVCIndexManual.html"
        LaunchName = Filename
    End Sub
End Class