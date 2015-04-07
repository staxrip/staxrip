Imports Microsoft.Win32

Imports System.Text.RegularExpressions
Imports System.Threading
Imports System.Runtime.InteropServices

Imports StaxRip.UI

Public Class Packs
    Public Shared AviSynth As New AviSynthPackage
    Public Shared AutoCrop As New AutoCropPackage
    Public Shared BeSweet As New BeSweetPackage
    Public Shared Decomb As New DecombPackage
    Public Shared Deen As New DeenPackage
    Public Shared DGDecode As New DGDecodePackage
    Public Shared DGIndex As New DGIndexPackage
    Public Shared FluxSmooth As New FluxSmoothPackage
    Public Shared Java As New JavaPackage
    Public Shared MediaInfo As New MediaInfoPackage
    Public Shared Mkvmerge As New MKVToolNixPackage
    Public Shared MP4Box As New MP4BoxPackage
    Public Shared NeroAACEnc As New NeroAACEncPackage
    Public Shared NicAudio As New NicAudioPackage
    Public Shared ProjectX As New ProjectXPackage
    Public Shared TomsMoComp As New TomsMoCompPackage
    Public Shared UnDot As New UnDotPackage
    Public Shared VirtualDubMod As New VirtualDubModPackage
    Public Shared VSFilter As New VSFilterPackage
    Public Shared VSRip As New VSRipPackage
    Public Shared x264 As New x264Package
    Public Shared x265 As New x265Package
    Public Shared avs4x26x As New avs4x26xPackage
    Public Shared Yadif As New YadifPackage
    Public Shared ffmpeg As New ffmpegPackage
    Public Shared eac3to As New eac3toPackage
    Public Shared ffms2 As New ffms2Package
    Public Shared BDSup2SubPP As New BDSup2SubPackage
    Public Shared DGDecodeNV As New DGDecodeNVPackage
    Public Shared DGIndexNV As New DGIndexNVPackage
    Public Shared lsmashWorks As New lsmashWorksPackage
    Public Shared dsmux As New dsmuxPackage
    Public Shared qaac As New qaacPackage
    Public Shared NVEncC As New NVEncCPackage
    Public Shared QSVEncC As New QSVEncCPackage
    Public Shared Haali As New HaaliSplitter

    Public Shared Property Packages As New Dictionary(Of String, Package)

    Shared Sub Init()
        AddPackage(Haali)
        AddPackage(QSVEncC)
        AddPackage(AutoCrop)
        AddPackage(AviSynth)
        AddPackage(avs4x26x)
        AddPackage(BDSup2SubPP)
        AddPackage(BeSweet)
        AddPackage(Decomb)
        AddPackage(Deen)
        AddPackage(DGDecode)
        AddPackage(DGDecodeNV)
        AddPackage(DGIndex)
        AddPackage(DGIndexNV)
        AddPackage(dsmux)
        AddPackage(eac3to)
        AddPackage(ffmpeg)
        AddPackage(ffms2)
        AddPackage(FluxSmooth)
        AddPackage(Java)
        AddPackage(lsmashWorks)
        AddPackage(MediaInfo)
        AddPackage(Mkvmerge)
        AddPackage(MP4Box)
        AddPackage(NeroAACEnc)
        AddPackage(NicAudio)
        AddPackage(NVEncC)
        AddPackage(ProjectX)
        AddPackage(qaac)
        AddPackage(TomsMoComp)
        AddPackage(UnDot)
        AddPackage(VirtualDubMod)
        AddPackage(VSFilter)
        AddPackage(VSRip)
        AddPackage(x264)
        AddPackage(x265)
        AddPackage(Yadif)

        Dim fp = CommonDirs.Startup + "Applications\Versions.txt"

        If File.Exists(fp) Then
            For Each i In File.ReadAllLines(CommonDirs.Startup + "Applications\Versions.txt")
                For Each i2 In Packages.Values
                    If i Like "*=*;*" Then
                        Dim name = i.Left("=").Trim

                        If name = i2.Name Then
                            i2.VersionName = i.Right("=").Left(";").Trim
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

Public MustInherit Class Package
    Property Name As String
    Property FileNotFoundMessage As String
    Property SetupAction As Action
    Property VersionName As String
    Property WebURL As String
    Property Description As String
    Property VersionDate As DateTime
    Property TreePath As String = "Tools"
    Property HelpFile As String
    Property HelpDir As String
    Property HelpURL As String
    Property FixedDir As String

    Overridable Property Filename As String

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

    Overridable Function IsRequired() As Boolean
        Return True
    End Function

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

            If IsStatusCritical() Then
                Return False
            End If
        End If

        Return True
    End Function

    Function IsStatusCritical() As Boolean
        Return OK(GetStatusLocation) OrElse OK(GetStatus())
    End Function

    Overridable Function GetStatus() As String
    End Function

    Function GetStatusDisplay() As String
        Dim r = GetStatusLocation()

        If r <> "" Then Return r

        r = GetStatus()

        If r <> "" Then Return r

        r = GetStatusVersion()

        If r <> "" Then Return r

        Return "OK"
    End Function

    Function GetStatusVersion() As String
        If VersionName <> "" AndAlso Not IsCorrectVersion(GetPath) Then
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
        If Not s Is Nothing AndAlso Not s.Storage Is Nothing Then
            Dim fp = s.Storage.GetString(Name + "custom path")

            If fp <> "" Then
                If File.Exists(fp) Then
                    Return fp
                Else
                    s.Storage.SetString(Name + "custom path", Nothing)
                End If
            End If
        End If

        If FixedDir <> "" Then
            If File.Exists(FixedDir + Filename) Then
                Return FixedDir + Filename
            End If

            Return Nothing
        End If

        If TypeOf Me Is AviSynthPluginPackage Then
            For Each i In {
                CommonDirs.Startup + "Applications\AviSynth plugins\" + Filename,
                CommonDirs.Startup + "Applications\AviSynth plugins\" + Name + "\" + Filename}

                If File.Exists(i) Then
                    Return i
                End If
            Next
        End If

        Dim fp2 = CommonDirs.Startup + "Applications\" + Name + "\" + Filename

        If File.Exists(fp2) Then
            Return fp2
        End If

        Dim fp3 = Registry.CurrentUser.GetString("Software\" + Application.ProductName, Name + " location")

        If fp3 <> "" Then
            If File.Exists(fp3) Then
                If Not s Is Nothing AndAlso Not s.Storage Is Nothing Then
                    s.Storage.SetString(Name + "custom path", fp3)
                End If

                Return fp3
            Else
                Registry.CurrentUser.DeleteValue("Software\" + Application.ProductName, Name + " location")
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
        WebURL = "http://avisynth.org/warpenterprises"
        Description = "AviSynth plugin for automatic cropping."
        HelpFile = "AutoCrop.txt"
        FilterNames = {"AutoCrop"}
    End Sub
End Class

Public Class DeenPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "Deen"
        Filename = "Deen.dll"
        WebURL = "http://ziquash.chez.tiscali.fr"
        Description = "AviSynth plugin for denoising."
        FilterNames = {"Deen"}
        HelpFile = "Deen.txt"
    End Sub
End Class

Public Class TomsMoCompPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "TomsMoComp"
        Filename = "TomsMoComp.dll"
        WebURL = "http://www6.impacthosting.com/trbarry"
        Description = "AviSynth plugin for deinterlacing using motion compensation."
        FilterNames = {"TomsMoComp"}
        HelpFile = "TomsMoComp.txt"
    End Sub
End Class

Public Class UnDotPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "UnDot"
        Filename = "UnDot.dll"
        WebURL = "http://www6.impacthosting.com/trbarry"
        Description = "UnDot is a simple median filter for removing dots, that is stray orphan pixels and mosquito noise."
        FilterNames = {"UnDot"}
        HelpFile = "UnDot.txt"
    End Sub
End Class

Public Class NicAudioPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "NicAudio"
        Filename = "NicAudio.dll"
        WebURL = "http://www.codeplex.com/NicAudio"
        Description = "AviSynth audio source filter."
        FilterNames = {"NicAC3Source", "NicDTSSource", "NicMPG123Source", "RaWavSource"}
        HelpFile = "Readme.txt"
    End Sub
End Class

Public Class AviSynthPackage
    Inherits Package

    Sub New()
        Name = "AviSynth"
        Filename = "avisynth.dll"
        WebURL = "http://avisynth.nl"
        Description = "AviSynth is a powerful scripting language used to transform audio and video. The scripts are saved as AVS files which applications like x264 can use as input file like it would be a normal AVI file."
        SetupAction = Sub() g.ShellExecute(CommonDirs.Startup + "Applications\AviSynth 2.6.0 RC1.exe")
        HelpURL = "http://avisynth.nl/index.php/Main_Page"
        FixedDir = CommonDirs.System
    End Sub

    Public Overrides Function GetStatus() As String
        If Not Directory.Exists(Paths.AviSynthPluginsDir) Then
            Return "The AviSynth setup is damaged because the plugins directory doesn't exist. Please click the 'Setup' button above."
        End If
    End Function
End Class

Public MustInherit Class AviSynthPluginPackage
    Inherits Package

    Public Sub New()
        TreePath = "Filters"
    End Sub

    Property FilterNames As String()
    Property IsCPlugin As Boolean
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
        FixedDir = CommonDirs.Startup + "Applications\BeSweet\"
    End Sub
End Class

Public Class DecombPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "Decomb"
        Filename = "Decomb.dll"
        Description = "AviSynth plugin containing filters for deinterlacing and IVTC."
        WebURL = "http://www.neuron2.net/decomb/decombnew.html"
        FilterNames = {"FieldDeinterlace", "Telecide", "Decimate"}
        HelpFile = "DecombReferenceManual.html"
    End Sub
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
        FixedDir = CommonDirs.Startup + "Applications\DGMPGDec\"
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
        FixedDir = CommonDirs.Startup + "Applications\DGMPGDec\"
    End Sub
End Class

Public Class FluxSmoothPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "FluxSmooth"
        Filename = "FluxSmooth.dll"
        Description = "FluxSmooth is a AviSynth plugin that removes noise from poor video sources."
        WebURL = "http://avisynth.org/warpenterprises"
        FilterNames = {"FluxSmoothT", "FluxSmoothST"}
        HelpFile = "readme.html"
    End Sub
End Class

Public Class VSFilterPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "VSFilter"
        Filename = "VSFilter.dll"
        Description = "VSFilter is a AviSynth plugin for adding hardcoded subtitles. Furthermore it's a DirectShow Filter to display optional subtitles."
        WebURL = "http://sourceforge.net/projects/guliverkli"
        FilterNames = {"VobSub", "TextSub"}
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

    Overrides Function IsRequired() As Boolean
        Return Packs.ProjectX.IsRequired
    End Function

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

    Overrides Function IsRequired() As Boolean
        Return CommandLineDemuxer.IsActive("ProjectX")
    End Function

    Overrides ReadOnly Property LaunchAction As Action
        Get
            Return AddressOf LaunchWithJava
        End Get
    End Property
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

    Public Overrides Property Filename As String
        Get
            If s Is Nothing Then
                Return "x264 32-Bit 8-Bit.exe"
            End If

            Return "x264 " + s.x264Build + ".exe"
        End Get
        Set(value As String)
            MyBase.Filename = value
        End Set
    End Property
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

    Public Overrides Function GetPath() As String
        If s Is Nothing Then Return ""
        Return CommonDirs.Startup + "Applications\x265\" + s.x265Build + "\" + Filename
    End Function
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

Public Class VirtualDubModPackage
    Inherits Package

    Sub New()
        Name = "VirtualDubMod"
        Filename = "VirtualDubMod.exe"
        WebURL = "http://virtualdubmod.sourceforge.net"
        Description = "VirtualDubMod is used for tasks like encoding, muxing, demuxing and cutting."
        HelpFile = "VirtualDubMod.chm"
        LaunchName = Filename
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
        LaunchName = "mmg.exe"
    End Sub

    Public Overrides ReadOnly Property LaunchTitle As String
        Get
            Return "mkvmerge GUI"
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

Public Class YadifPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "Yadif"
        Filename = "yadif.dll"
        WebURL = "http://avisynth.org.ru"
        Description = "Port of YADIF (Yet Another DeInterlacing Filter) from MPlayer."
        HelpFile = "yadif.html"
        FilterNames = {"Yadif"}
        IsCPlugin = True
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

Public Class lsmashWorksPackage
    Inherits AviSynthPluginPackage

    Sub New()
        Name = "L-SMASH-Works"
        Filename = "LSMASHSource.dll"
        WebURL = "https://github.com/VFR-maniac/L-SMASH-Works"
        Description = "AviSynth source filter supporting various input formats."
        HelpFile = "README.txt"
        FilterNames = {"LSMASHVideoSource",
                       "LSMASHAudioSource",
                       "LWLibavVideoSource",
                       "LWLibavAudioSource"}
    End Sub

    Public Overrides Function IsRequired() As Boolean
        Return p.AvsDoc.GetFilter("Source").Script.Contains("LSMASHVideoSource") OrElse p.AvsDoc.GetFilter("Source").Script.Contains("LWLibavVideoSource")
    End Function

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
        Filename = "qaac.exe"
        WebURL = "https://sites.google.com/site/qaacpage"
        Description = "qaac is a command line AAC encoder frontend based on Apple encoder."
    End Sub

    Public Overrides Function IsRequired() As Boolean
        Return TypeOf p.Audio0 Is GUIAudioProfile AndAlso
            DirectCast(p.Audio0, GUIAudioProfile).Params.Encoder = GuiAudioEncoder.qaac OrElse
            TypeOf p.Audio1 Is GUIAudioProfile AndAlso
            DirectCast(p.Audio1, GUIAudioProfile).Params.Encoder = GuiAudioEncoder.qaac
    End Function

    Overrides Function GetStatus() As String
        Dim fp = CommonDirs.Programs + "Common Files\Apple\Apple Application Support\CoreAudioToolbox.dll"

        If Not File.Exists(fp) AndAlso Not File.Exists(GetDir() + "QTfiles\CoreAudioToolbox.dll") Then
            Return "Failed to locate Apple Application Support, it must be downloaded and installed manually, qaac requires it, search the internet for a setup guide."
        End If
    End Function
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
        FileNotFoundMessage = "Application not found, please locate it by pressing F11 or disable the DGIndexNV feature under Tools/Settings/Demuxing."
    End Sub

    Overrides Function IsCorrectVersion(path As String) As Boolean
        Return True
    End Function

    Overrides Function IsRequired() As Boolean
        Return CommandLineDemuxer.IsActive("DGIndexNV")
    End Function
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
        FileNotFoundMessage = "Application not found, please locate it by pressing F11 or disable the DGIndexNV feature under Tools/Settings/Demuxing."
    End Sub

    Overrides Function GetStatus() As String
        If Not File.Exists(GetDir() + "License.txt") Then
            Return "DGDecNV is shareware requiring a license file but the file is missing."
        End If
    End Function

    Overrides Function IsCorrectVersion(path As String) As Boolean
        Return True
    End Function

    Overrides Function IsRequired() As Boolean
        Return CommandLineDemuxer.IsActive("DGIndexNV")
    End Function
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
        Description = "nvidia GPU accelerated H.264/H.265 encoder"
        HelpFile = "help.txt"
    End Sub
End Class

Public Class QSVEncCPackage
    Inherits Package

    Sub New()
        Name = "QSVEncC"
        Filename = "QSVEncC.exe"
        Description = "Intel Quick Sync GPU accelerated H.264 encoder"
        HelpFile = "help.txt"
        WebURL = "https://onedrive.live.com/?cid=6bdd4375ac8933c6&id=6BDD4375AC8933C6!482"
    End Sub
End Class

Public Class HaaliSplitter
    Inherits Package

    Sub New()
        Name = "Haali Splitter"
        Filename = "splitter.ax"
        WebURL = "http://haali.su/mkv"
        SetupAction = Sub() g.ShellExecute(CommonDirs.Startup + "Applications\MatroskaSplitter.exe")
        Description = "Haali Splitter is used by eac3to and dsmux to write MKV files. Haali Splitter and LAV Filters overrite each other, most people prefer LAV Filters, therefore it's recommended to install Haali first and LAV Filters last."
    End Sub

    Public Overrides Function GetPath() As String
        Dim ret = Registry.ClassesRoot.GetString("CLSID\" + GUIDS.HaaliMuxer.ToString + "\InprocServer32", Nothing)
        If File.Exists(ret) Then Return ret
    End Function

    Public Overrides Function IsRequired() As Boolean
        Return False
    End Function
End Class

Public Class dsmuxPackage
    Inherits Package

    Sub New()
        Name = "dsmux"
        Filename = "dsmux.exe"
        Description = Strings.dsmux
        WebURL = "http://haali.su/mkv"
        SetupAction = Sub() g.ShellExecute(CommonDirs.Startup + "Applications\MatroskaSplitter.exe")
    End Sub

    Public Overrides Function GetPath() As String
        Dim ret = Registry.ClassesRoot.GetString("CLSID\" + GUIDS.HaaliMuxer.ToString + "\InprocServer32", Nothing)
        ret = Filepath.GetDir(ret) + Filename
        If File.Exists(ret) Then Return ret
    End Function

    Overrides Function IsRequired() As Boolean
        Return False
    End Function
End Class