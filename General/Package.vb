Imports Microsoft.Win32
Imports StaxRip

Public Class Packs
    Public Shared autocrop As New AutoCropPackage
    Public Shared AviSynth As New AviSynthPackage
    Public Shared AVSMeter As New AVSMeterPackage
    Public Shared BDSup2SubPP As New BDSup2SubPackage
    Public Shared BeSweet As New BeSweetPackage
    Public Shared checkmate As New checkmatePackage
    Public Shared DGDecodeIM As New DGDecodeIMPackage
    Public Shared DGDecodeNV As New DGDecodeNVPackage
    Public Shared DGIndex As New DGIndexPackage
    Public Shared DGIndexIM As New DGIndexIMPackage
    Public Shared DGIndexNV As New DGIndexNVPackage
    Public Shared DivX265 As New DivX265Package
    Public Shared dsmux As New dsmuxPackage
    Public Shared DSS2mod As New DSS2modPackage
    Public Shared eac3to As New eac3toPackage
    Public Shared ffmpeg As New ffmpegPackage
    Public Shared ffms2 As New ffms2Package
    Public Shared Haali As New HaaliSplitter
    Public Shared Java As New JavaPackage
    Public Shared lsmashWorks As New LSmashWorksAviSynthPackage
    Public Shared masktools2 As New masktools2Package
    Public Shared MediaInfo As New MediaInfoPackage
    Public Shared Mkvmerge As New MKVToolNixPackage
    Public Shared MP4Box As New MP4BoxPackage
    Public Shared MPC As New MPCPackage
    Public Shared mvtools As New mvtoolsPackage
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
    Public Shared flash3kyuu_deband As New flash3kyuu_debandPackage
    Public Shared Decomb As New DecombPackage
    Public Shared vinverse As New vinversePackage
    Public Shared vspipe As New vspipePackage
    Public Shared VapourSynth As New VapourSynthPackage
    Public Shared Python As New PythonPackage
    Public Shared vscpp2013 As New vscpp2013Package
    Public Shared vscpp2010 As New vscpp2010Package
    Public Shared fmtconv As New fmtconvPackage
    Public Shared scenechange As New scenechangePackage
    Public Shared temporalsoften As New temporalsoftenPackage
    Public Shared havsfunc As New havsfuncPackage
    Public Shared LSmashWorksVapourSynth As New vslsmashsourcePackage

    Public Shared Property Packages As New List(Of Package)

    Shared Sub Init()
        Packages.Add(LSmashWorksVapourSynth)
        Packages.Add(havsfunc)
        Packages.Add(temporalsoften)
        Packages.Add(scenechange)
        Packages.Add(fmtconv)
        Packages.Add(vscpp2010)
        Packages.Add(vscpp2013)
        Packages.Add(Python)
        Packages.Add(VapourSynth)
        Packages.Add(vspipe)
        Packages.Add(vinverse)
        Packages.Add(Decomb)
        Packages.Add(flash3kyuu_deband)
        Packages.Add(autocrop)
        Packages.Add(AviSynth)
        Packages.Add(AVSMeter)
        Packages.Add(BDSup2SubPP)
        Packages.Add(BeSweet)
        Packages.Add(checkmate)
        Packages.Add(DGDecodeIM)
        Packages.Add(DGDecodeNV)
        Packages.Add(DGIndex)
        Packages.Add(DGIndexIM)
        Packages.Add(DGIndexNV)
        Packages.Add(DivX265)
        Packages.Add(dsmux)
        Packages.Add(DSS2mod)
        Packages.Add(eac3to)
        Packages.Add(ffmpeg)
        Packages.Add(ffms2)
        Packages.Add(Haali)
        Packages.Add(Java)
        Packages.Add(lsmashWorks)
        Packages.Add(masktools2)
        Packages.Add(MediaInfo)
        Packages.Add(Mkvmerge)
        Packages.Add(MP4Box)
        Packages.Add(MPC)
        Packages.Add(mvtools)
        Packages.Add(NeroAACEnc)
        Packages.Add(NicAudio)
        Packages.Add(nnedi3)
        Packages.Add(NVEncC)
        Packages.Add(ProjectX)
        Packages.Add(qaac)
        Packages.Add(QSVEncC)
        Packages.Add(QTGMC)
        Packages.Add(RgTools)
        Packages.Add(SangNom2)
        Packages.Add(TDeint)
        Packages.Add(UnDot)
        Packages.Add(VSFilter)
        Packages.Add(VSRip)
        Packages.Add(x264)
        Packages.Add(x265)
        Packages.Add(xvid_encraw)

        Packages.Sort()

        Dim fp = CommonDirs.Startup + "Apps\Versions.txt"

        If File.Exists(fp) Then
            For Each i In File.ReadAllLines(CommonDirs.Startup + "Apps\Versions.txt")
                For Each i2 In Packages
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
End Class

Public Class Package
    Implements IComparable(Of Package)

    Property Name As String
    Property FileNotFoundMessage As String
    Property SetupAction As Action
    Property Version As String
    Property WebURL As String
    Property Description As String
    Property VersionDate As DateTime
    Property HelpFile As String
    Property HelpDir As String
    Property HelpURL As String
    Property DownloadURL As String
    Property Filenames As String()

    Protected FixedDirValue As String

    Overridable ReadOnly Property FixedDir As String
        Get
            Return FixedDirValue
        End Get
    End Property

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

            Return "App not found"
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

        Dim plugin = TryCast(Me, PluginPackage)

        If Not plugin Is Nothing Then
            If Not plugin.VapourSynthFilterNames Is Nothing AndAlso Not plugin.AviSynthFilterNames Is Nothing Then
                ret = CommonDirs.Startup + "Apps\Plugins\both\" + Name + "\" + Filename
                If File.Exists(ret) Then Return ret
            Else
                If plugin.VapourSynthFilterNames Is Nothing Then
                    ret = CommonDirs.Startup + "Apps\Plugins\avs\" + Name + "\" + Filename
                    If File.Exists(ret) Then Return ret
                Else
                    ret = CommonDirs.Startup + "Apps\Plugins\vs\" + Name + "\" + Filename
                    If File.Exists(ret) Then Return ret
                End If
            End If
        End If

        ret = CommonDirs.Startup + "Apps\" + Name + "\" + Filename
        If File.Exists(ret) Then Return ret
    End Function

    Overrides Function ToString() As String
        Return Name
    End Function

    Function CompareTo(other As Package) As Integer Implements System.IComparable(Of Package).CompareTo
        Return Name.CompareTo(other.Name)
    End Function
End Class

Public Class AutoCropPackage
    Inherits PluginPackage

    Sub New()
        Name = "AutoCrop"
        Filename = "AutoCrop.dll"
        WebURL = "http://avisynth.org.ru/docs/english/externalfilters/autocrop.htm"
        HelpURL = "http://avisynth.org.ru/docs/english/externalfilters/autocrop.htm"
        Description = "AutoCrop is an AviSynth filter that automatically crops the black borders from a clip. It operates in either preview mode where it overlays the recommended cropping information on the existing clip, or cropping mode where it really crops the clip."
        AviSynthFilterNames = {"AutoCrop"}
    End Sub
End Class

Public Class VSFilterPackage
    Inherits PluginPackage

    Sub New()
        Name = "VSFilter"
        Filename = "VSFilter.dll"
        Description = "AviSynth subtitle plugin. The format of the subtitles can be *.sub, *.srt, *.ssa, *.ass, etc. (ssa = Sub Station Alpha)."
        WebURL = "http://avisynth.org.ru/docs/english/externalfilters/vsfilter.htm"
        HelpURL = "http://avisynth.org.ru/docs/english/externalfilters/vsfilter.htm"
        AviSynthFilterNames = {"VobSub", "TextSub"}
    End Sub
End Class

Public Class UnDotPackage
    Inherits PluginPackage

    Sub New()
        Name = "UnDot"
        Filename = "UnDot.dll"
        WebURL = "http://avisynth.nl/index.php/UnDot"
        HelpURL = "http://avisynth.nl/index.php/UnDot"
        Description = "UnDot is a simple median filter for removing dots, that is stray orphan pixels and mosquito noise."
        AviSynthFilterNames = {"UnDot"}
    End Sub
End Class

Public Class NicAudioPackage
    Inherits PluginPackage

    Sub New()
        Name = "NicAudio"
        Filename = "NicAudio.dll"
        WebURL = "http://www.codeplex.com/NicAudio"
        HelpURL = "http://avisynth.org.ru/docs/english/externalfilters/nicaudio.htm"
        Description = "AviSynth audio source filter."
        AviSynthFilterNames = {"NicAC3Source", "NicDTSSource", "NicMPASource", "RaWavSource"}
    End Sub
End Class

Public Class AviSynthPackage
    Inherits Package

    Sub New()
        Name = "AviSynth+"
        Filename = "avisynth.dll"
        WebURL = "http://avisynth.nl"
        Description = "StaxRip support both AviSynth+ x64 and VapourSynth x64 as scripting based video processing tool."
        HelpURL = "http://avisynth.nl/index.php/Main_Page"
        FixedDirValue = CommonDirs.System
        SetupAction = Sub() g.ShellExecute(CommonDirs.Startup + "Apps\AviSynth+_r1825.exe")
    End Sub

    Public Overrides Function GetStatus() As String
        If Not Directory.Exists(Paths.AviSynthPluginsDir) Then
            Return "The AviSynth setup is damaged because the plugins directory doesn't exist. Please click the 'Setup' button above."
        End If
    End Function
End Class

Public Class PythonPackage
    Inherits Package

    Sub New()
        Name = "Python"
        Filename = "py.exe"
        WebURL = "http://www.python.org"
        Description = "Python x64 is required by VapourSynth x64. StaxRip x64 supports both AviSynth+ x64 and VapourSynth x64 as scripting based video processing tool."
        HelpURL = "http://docs.python.org/3"
        FixedDirValue = CommonDirs.Windows
        DownloadURL = "http://www.python.org/downloads/windows"
    End Sub

    Public Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return p.VideoScript.Engine = ScriptingEngine.VapourSynth
        End Get
    End Property
End Class

Public Class VapourSynthPackage
    Inherits Package

    Sub New()
        Name = "VapourSynth"
        Filename = "vapoursynth.dll"
        WebURL = "http://www.vapoursynth.com"
        Description = "StaxRip x64 supports both AviSynth+ x64 and VapourSynth x64 as scripting based video processing tool."
        HelpURL = "http://www.vapoursynth.com/doc"
        DownloadURL = "http://github.com/vapoursynth/vapoursynth/releases"
    End Sub

    Public Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return p.VideoScript.Engine = ScriptingEngine.VapourSynth
        End Get
    End Property

    Public Overrides ReadOnly Property FixedDir As String
        Get
            Return Registry.LocalMachine.GetString("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\VapourSynth_is1", "Inno Setup: App Path") + "\core64\"
        End Get
    End Property
End Class

Public Class vspipePackage
    Inherits Package

    Sub New()
        Name = "vspipe"
        Filename = "vspipe.exe"
        Description = "vspipe is installed by VapourSynth and used to pipe VapourSynth scripts to encoding apps."
        WebURL = "http://www.vapoursynth.com/doc/vspipe.html"
        HelpURL = "http://www.vapoursynth.com/doc/vspipe.html"
        DownloadURL = "http://github.com/vapoursynth/vapoursynth/releases"
    End Sub

    Public Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return p.VideoScript.Engine = ScriptingEngine.VapourSynth
        End Get
    End Property

    Public Overrides ReadOnly Property FixedDir As String
        Get
            Return Registry.LocalMachine.GetString("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\VapourSynth_is1", "Inno Setup: App Path") + "\core64\"
        End Get
    End Property
End Class

Public Class vscpp2013Package
    Inherits Package

    Sub New()
        Name = "Visual C++ 2013"
        Filename = "msvcr120.dll"
        Description = "Visual C++ 2013 Redistributable Packages which is required by various tools used by StaxRip."
        DownloadURL = "https://www.microsoft.com/en-US/download/details.aspx?id=40784"
        FixedDirValue = CommonDirs.System
    End Sub
End Class

Public Class vscpp2010Package
    Inherits Package

    Sub New()
        Name = "Visual C++ 2010"
        Filename = "msvcr100.dll"
        Description = "Visual C++ 2010 Redistributable Packages. Only MP4Box requires this outdated runtime. MP4Box is used to demux and mux MP4."
        DownloadURL = "https://www.microsoft.com/en-us/download/details.aspx?id=13523"
        FixedDirValue = CommonDirs.System
    End Sub
End Class

Public MustInherit Class PluginPackage
    Inherits Package

    Property AviSynthFilterNames As String()
    Property VapourSynthFilterNames As String()
    Property Dependencies As String()
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
        FixedDirValue = CommonDirs.Startup + "Apps\BeSweet\"
    End Sub
End Class

Public Class JavaPackage
    Inherits Package

    Sub New()
        Name = "Java"
        Filename = "Java.exe"
        WebURL = "http://java.com"
        Description = "Java is required by ProjectX. " + Strings.ProjectX
        DownloadURL = "http://java.com/en/download"
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
        Description = "H.264 video encoding command line app."
        HelpFile = "Help.txt"
    End Sub
End Class

Public Class x265Package
    Inherits Package

    Sub New()
        Name = "x265"
        Filename = "x265.exe"
        WebURL = "http://x265.org"
        Description = "H.265 video encoding command line app."
        HelpURL = "http://x265.readthedocs.org"
    End Sub
End Class

Public Class MP4BoxPackage
    Inherits Package

    Sub New()
        Name = "MP4Box"
        Filename = "MP4Box.exe"
        WebURL = "http://gpac.wp.mines-telecom.fr/"
        Description = "MP4Box is a MP4 muxing and demuxing command line app."
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
        HelpURL = "http://www.bunkus.org/videotools/mkvtoolnix/docs.html"
    End Sub
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
        Description = "Audio conversion command line app."
        HelpURL = "http://en.wikibooks.org/wiki/Eac3to/How_to_Use"
    End Sub
End Class

Public Class ffms2Package
    Inherits PluginPackage

    Sub New()
        Name = "ffms2"
        Filename = "ffms2.dll"
        WebURL = "https://github.com/FFMS/ffms2"
        Description = "AviSynth+ and VapourSynth source filter supporting various input formats."
        AviSynthFilterNames = {"FFVideoSource", "FFAudioSource"}
        VapourSynthFilterNames = {"ffms2"}
        HelpDir = "doc"
    End Sub
End Class

Public Class LSmashWorksAviSynthPackage
    Inherits PluginPackage

    Sub New()
        Name = "L-SMASH-Works"
        Filename = "LSMASHSource.dll"
        WebURL = "http://avisynth.nl/index.php/LSMASHSource"
        Description = "AviSynth and VapourSynth source filter based on Libav supporting a wide range of input formats."
        HelpFile = "README.txt"
        AviSynthFilterNames = {"LSMASHVideoSource", "LSMASHAudioSource", "LWLibavVideoSource", "LWLibavAudioSource"}
    End Sub

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return p.VideoScript.GetFilter("Source").Script.Contains("LSMASHVideoSource") OrElse
                p.VideoScript.GetFilter("Source").Script.Contains("LWLibavVideoSource")
        End Get
    End Property
End Class

Public Class vslsmashsourcePackage
    Inherits PluginPackage

    Sub New()
        Name = "vslsmashsource"
        Filename = "vslsmashsource.dll"
        Description = "VapourSynth source filter based on Libav supporting a wide range of input formats."
        VapourSynthFilterNames = {"lsmas.LibavSMASHSource", "lsmas.LWLibavSource"}
    End Sub

    Overrides ReadOnly Property IsRequired As Boolean
        Get
            Return p.VideoScript.GetFilter("Source").Script.Contains("lsmas.LibavSMASHSource") OrElse
                p.VideoScript.GetFilter("Source").Script.Contains("lsmas.LWLibavSource")
        End Get
    End Property
End Class

Public Class qaacPackage
    Inherits Package

    Sub New()
        Name = "qaac"
        Filename = "qaac64.exe"
        WebURL = "http://github.com/nu774/qaac"
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
    Inherits PluginPackage

    Sub New()
        Name = "checkmate"
        Filename = "checkmate.dll"
        WebURL = "http://github.com/tp7/checkmate"
        HelpURL = "http://github.com/tp7/checkmate"
        Description = "Spatial and temporal dot crawl reducer. Checkmate is most effective in static or low motion scenes. When using in high motion scenes (or areas) be careful, it's known to cause artifacts with its default values."
        AviSynthFilterNames = {"checkmate"}
    End Sub
End Class

Public Class SangNom2Package
    Inherits PluginPackage

    Sub New()
        Name = "SangNom2"
        Filename = "SangNom2.dll"
        WebURL = "http://avisynth.nl/index.php/SangNom2"
        HelpURL = "http://avisynth.nl/index.php/SangNom2"
        Description = "SangNom2 is a reimplementation of MarcFD's old SangNom filter. Originally it's a single field deinterlacer using edge-directed interpolation but nowadays it's mainly used in anti-aliasing scripts. The output is not completely but mostly identical to the original SangNom."
        AviSynthFilterNames = {"SangNom2"}
    End Sub
End Class

Public Class DSS2modPackage
    Inherits PluginPackage

    Sub New()
        Name = "DSS2mod"
        Filename = "DSS2.dll"
        WebURL = "http://code.google.com/p/xvid4psp/downloads/detail?name=DSS2%20mod%20%2B%20LAVFilters.7z&can=2&q="
        Description = "Direct Show source filter"
        AviSynthFilterNames = {"DSS2"}
    End Sub
End Class

Public Class vinversePackage
    Inherits PluginPackage

    Sub New()
        Name = "vinverse"
        Filename = "vinverse.dll"
        WebURL = "http://avisynth.nl/index.php/Vinverse"
        HelpURL = "http://avisynth.nl/index.php/Vinverse"
        Description = "A modern rewrite of a simple but effective plugin to remove residual combing originally based on an AviSynth script by Didée and then written as a plugin by tritical."
        AviSynthFilterNames = {"vinverse", "vinverse2"}
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
    Inherits PluginPackage

    Sub New()
        Name = "DGDecodeNV"
        Filename = "DGDecodeNV.dll"
        WebURL = "http://neuron2.net/dgdecnv/dgdecnv.html"
        Description = Strings.DGDecNV
        HelpFile = "DGDecodeNVManual.html"
        AviSynthFilterNames = {"DGSource"}
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
    Inherits PluginPackage

    Sub New()
        Name = "DGDecodeIM"
        Filename = "DGDecodeIM.dll"
        WebURL = "http://rationalqm.us/mine.html"
        Description = Strings.DGDecIM
        HelpFile = "Notes.txt"
        AviSynthFilterNames = {"DGSourceIM"}
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

Public Class dsmuxPackage
    Inherits Package

    Sub New()
        Name = "dsmux"
        Filename = "dsmux.x64.exe"
        Description = Strings.dsmux
        WebURL = "http://haali.su/mkv"
        SetupAction = Sub() g.ShellExecute(CommonDirs.Startup + "Apps\MatroskaSplitter.exe")
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

Public Class HaaliSplitter
    Inherits Package

    Sub New()
        Name = "Haali Splitter"
        Filename = "splitter.ax"
        WebURL = "http://haali.su/mkv"
        SetupAction = Sub() g.ShellExecute(CommonDirs.Startup + "Apps\MatroskaSplitter.exe")
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

Public Class DGDecodePackage
    Inherits PluginPackage

    Sub New()
        Name = "DGDecode"
        Filename = "DGDecode.dll"
        WebURL = "http://www.neuron2.net/dgmpgdec/dgmpgdec.html"
        Description = Strings.DGMPGDec
        AviSynthFilterNames = {"MPEG2Source", "Deblock"}
        HelpFile = "DGDecodeManual.html"
        FixedDirValue = CommonDirs.Startup + "Apps\DGMPGDec\"
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
        FixedDirValue = CommonDirs.Startup + "Apps\DGMPGDec\"
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
    Inherits PluginPackage

    Sub New()
        Name = "TDeint"
        Filename = "TDeint.dll"
        WebURL = "http://avisynth.nl/index.php/TDeint"
        HelpURL = "http://avisynth.nl/index.php/TDeint"
        Description = "TDeint is a bi-directionally, motion adaptive, sharp deinterlacer. It can adaptively choose between using per-field and per-pixel motion adaptivity, and can use cubic interpolation, kernel interpolation (with temporal direction switching), or one of two forms of modified ELA interpolation which help to reduce ""jaggy"" edges in moving areas where interpolation must be used."
        AviSynthFilterNames = {"TDeint"}
    End Sub
End Class

Public Class nnedi3Package
    Inherits PluginPackage

    Sub New()
        Name = "nnedi3"
        Filename = "libnnedi3.dll"
        WebURL = "http://github.com/dubhater/vapoursynth-nnedi3"
        HelpURL = "http://github.com/dubhater/vapoursynth-nnedi3"
        Description = "nnedi3 is an intra-field only deinterlacer. It takes in a frame, throws away one field, and then interpolates the missing pixels using only information from the kept field."
        VapourSynthFilterNames = {"nnedi3.nnedi3", "nnedi3.nnedi3_rpow2"}
    End Sub
End Class

Public Class scenechangePackage
    Inherits PluginPackage

    Sub New()
        Name = "scenechange"
        Filename = "scenechange.dll"
        VapourSynthFilterNames = {"scenechange"}
    End Sub
End Class

Public Class temporalsoftenPackage
    Inherits PluginPackage

    Sub New()
        Name = "temporalsoften"
        Filename = "temporalsoften.dll"
        VapourSynthFilterNames = {"temporalsoften"}
    End Sub
End Class

Public Class havsfuncPackage
    Inherits PluginPackage

    Sub New()
        Name = "havsfunc"
        Filename = "havsfunc.py"
        VapourSynthFilterNames = {"havsfunc.QTGMC", "havsfunc.ediaa", "havsfunc.daa", "havsfunc.maa",
                                  "havsfunc.SharpAAMCmod", "havsfunc.Deblock_QED", "havsfunc.DeHalo_alpha",
                                  "havsfunc.YAHR", "havsfunc.HQDeringmod", "havsfunc.ivtc_txt60mc",
                                  "havsfunc.Vinverse", "havsfunc.Vinverse2", "havsfunc.logoNR",
                                  "havsfunc.LUTDeCrawl", "havsfunc.LUTDeRainbow", "havsfunc.GSMC",
                                  "havsfunc.SMDegrain", "havsfunc.SmoothLevels", "havsfunc.FastLineDarkenMOD",
                                  "havsfunc.LSFmod", "havsfunc.GrainFactory3"}

        Dependencies = {"fmtconv", "mvtools", "nnedi3", "scenechange", "temporalsoften"}
        Description = "Various popular AviSynth scripts ported to VapourSynth."
        HelpURL = "http://forum.doom9.org/showthread.php?t=166582"
        WebURL = "http://forum.doom9.org/showthread.php?t=166582"
    End Sub
End Class

Public Class mvtoolsPackage
    Inherits PluginPackage

    Sub New()
        Name = "mvtools"
        Filename = "libmvtools.dll"
        WebURL = "http://github.com/dubhater/vapoursynth-mvtools"
        HelpURL = "http://github.com/dubhater/vapoursynth-mvtools"
        Description = "MVTools is a set of filters for motion estimation and compensation."
        VapourSynthFilterNames = {"mv.Super", "mv.Analyse", "mv.Recalculate", "mv.Compensate", "mv.Degrain1", "mv.Degrain2",
            "mv.Degrain3", "mv.Mask", "mv.Finest", "mv.FlowBlur", "mv.FlowInter", "mv.FlowFPS", "mv.BlockFPS", "mv.SCDetection"}
    End Sub
End Class

Public Class masktools2Package
    Inherits PluginPackage

    Sub New()
        Name = "masktools2"
        Filename = "masktools2.dll"
        WebURL = "http://avisynth.nl/index.php/MaskTools2"
        HelpURL = "http://avisynth.nl/index.php/MaskTools2"
        Description = "MaskTools2 contain a set of filters designed to create, manipulate and use masks. Masks, in video processing, are a way to give a relative importance to each pixel. You can, for example, create a mask that selects only the green parts of the video, and then replace those parts with another video."
        AviSynthFilterNames = {"Mt_edge", "Mt_motion"}
    End Sub
End Class

Public Class RgToolsPackage
    Inherits PluginPackage

    Sub New()
        Name = "RgTools"
        Filename = "RgTools.dll"
        WebURL = "http://avisynth.nl/index.php/RgTools"
        HelpURL = "http://avisynth.nl/index.php/RgTools"
        Description = "RgTools is a modern rewrite of RemoveGrain, Repair, BackwardClense, Clense, ForwardClense and VerticalCleaner all in a single plugin."
        AviSynthFilterNames = {"RemoveGrain", "Clense", "ForwardClense", "BackwardClense", "Repair", "VerticalCleaner"}
    End Sub
End Class

Public Class DecombPackage
    Inherits PluginPackage

    Sub New()
        Name = "Decomb"
        Filename = "Decomb.dll"
        WebURL = "http://neuron2.net/decomb/decombnew.html"
        HelpFile = "DecombReferenceManual.html"
        Description = "This package of plugin functions for Avisynth provides the means for removing combing artifacts from telecined progressive streams, interlaced streams, and mixtures thereof. Functions can be combined to implement inverse telecine (IVTC) for both NTSC and PAL streams."
        AviSynthFilterNames = {"Telecide", "FieldDeinterlace", "Decimate", "IsCombed"}
    End Sub
End Class

Public Class flash3kyuu_debandPackage
    Inherits PluginPackage

    Sub New()
        Name = "flash3kyuu_deband"
        Filename = "flash3kyuu_deband.dll"
        WebURL = "http://forum.doom9.org/showthread.php?t=161411"
        HelpFile = "flash3kyuu_deband.txt"
        Description = "Simple debanding filter that can be quite effective for some anime sources."
        AviSynthFilterNames = {"f3kdb"}
    End Sub
End Class

Public Class QTGMCPackage
    Inherits PluginPackage

    Sub New()
        Name = "QTGMC"
        Filename = "QTGMC.avsi"
        WebURL = "http://avisynth.nl/index.php/QTGMC"
        HelpFile = "QTGMC.html"
        Description = "A very high quality deinterlacer with a range of features for both quality and convenience. These include a simple presets system, extensive noise processing capabilities, support for repair of progressive material, precision source matching, shutter speed simulation, etc. Originally based on TempGaussMC by Didée."
        AviSynthFilterNames = {"QTGMC"}
        Dependencies = {"masktools2", "mvtools2", "nnedi3", "RgTools"}
    End Sub
End Class

Public Class fmtconvPackage
    Inherits PluginPackage

    Sub New()
        Name = "fmtconv"
        Filename = "fmtconv.dll"
        WebURL = "http://github.com/EleonoreMizo/fmtconv"
        HelpFile = "fmtconv.html"
        Description = "Fmtconv is a format-conversion plug-in for the Vapoursynth video processing engine. It does resizing, bitdepth conversion with dithering and colorspace conversion."
        VapourSynthFilterNames = {"fmtc.bitdepth", "fmtc.convert", "fmtc.matrix", "fmtc.resample", "fmtc.transfer"}
    End Sub
End Class