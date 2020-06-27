
Imports System.Runtime.InteropServices

Public Class FrameServerHelp
    Shared WasInitialized As Boolean
    Shared WasAviSynthInitialized As Boolean
    Shared WasVapourSynthInitialized As Boolean

    Shared Sub Init()
        If ffmpegMightUseAviSynth() Then
            If IsAviSynthPortableUsed() Then
                Dim msg = "Soft link creation is required to use AviSynth+ in portable mode due to " +
                          "design limitations of AviSynth+ and ffmpeg." + BR2 + GetAviSynthOptions()

                MakeSoftLink("avs to ffmpeg", Package.AviSynth.Path,
                             Package.ffmpeg.Directory + "AviSynth.dll", msg)
            Else
                DeleteSoftLink(Package.ffmpeg.Directory + "AviSynth.dll")
            End If
        End If

        If Not WasInitialized Then
            g.AddToPath(Folder.Startup, Package.Python.Directory, Package.AviSynth.Directory,
                        Package.VapourSynth.Directory, Package.FFTW.Directory)

            WasInitialized = True
        End If

        If IsAviSynthUsed() AndAlso Not WasAviSynthInitialized Then
            If IsAviSynthPortableUsed() Then
                DirectoryHelp.Create(Folder.Settings + "Plugins\AviSynth")
            End If

            CreateAviSynthSoftLinks()
            WasAviSynthInitialized = True
        End If

        If IsVapourSynthUsed() AndAlso Not WasVapourSynthInitialized Then
            If IsVapourSynthPortableUsed() Then
                DirectoryHelp.Create(Folder.Settings + "Plugins\VapourSynth")
            End If

            WasVapourSynthInitialized = True
        End If
    End Sub

    Shared Function GetAviSynthOptions() As String
        Return "Option one is installing a compatible AviSynth+ version and disabling AviSynth+ " +
               "portable mode in the StaxRip settings (Tools > Settings > General)." + BR2 +
               "Option two is running StaxRip with administrative privileges until soft link " +
               "creation completes, this has to be done only once, after the links were created, " +
               "regular privileges are sufficient." + BR2 +
               "Option three is enabling Developer Mode in the Windows 10 settings, this allows " +
               "soft link creation without administrative privileges."
    End Function

    Shared Function ffmpegMightUseAviSynth() As Boolean
        Return (p.Script.Engine = ScriptEngine.AviSynth AndAlso TypeOf p.VideoEncoder Is ffmpegEnc) OrElse
            p.Audio0.File.Ext = "avs" OrElse p.Audio1.File.Ext = "avs"
    End Function

    Shared Function IsAviSynthPortableUsed() As Boolean
        Return Package.AviSynth.Directory.StartsWithEx(Folder.Apps)
    End Function

    Shared Function IsVapourSynthPortableUsed() As Boolean
        Return Package.VapourSynth.Directory.StartsWithEx(Folder.Apps)
    End Function

    Shared Function IsPortable() As Boolean
        If (IsAviSynthUsed() AndAlso IsAviSynthPortableUsed()) OrElse
            (IsVapourSynthUsed() AndAlso IsVapourSynthPortableUsed()) Then

            Return True
        End If
    End Function

    Shared Function IsAviSynthInstalled() As Boolean
        Return (Folder.System + "AviSynth.dll").FileExists
    End Function

    Shared Function IsAviSynthUsed() As Boolean
        Return p.Script.Engine = ScriptEngine.AviSynth
    End Function

    Shared Function IsVapourSynthUsed() As Boolean
        Return p.Script.Engine = ScriptEngine.VapourSynth
    End Function

    Shared Sub CreateAviSynthSoftLinks()
        Dim packs = {Package.x265, Package.NVEnc, Package.QSVEnc, Package.VCEEnc, Package.x264, Package.mpvnet}

        If IsAviSynthPortableUsed() Then
            If IsAviSynthInstalled() Then
                Dim msg = "When AviSynth+ is installed then portable mode requires " +
                          "soft link creation due to limitations of AviSynth+ and " +
                          "most AviSynth reading tools." + BR2 + GetAviSynthOptions()

                For Each pack In packs
                    MakeSoftLink("avs to " + pack.Name, Package.AviSynth.Path,
                                 pack.Directory + "AviSynth.dll", msg)
                Next
            Else
                For Each pack In packs
                    DeleteSoftLink(pack.Directory + "AviSynth.dll")
                Next
            End If
        Else
            For Each pack In packs
                DeleteSoftLink(pack.Directory + "AviSynth.dll")
            Next
        End If
    End Sub

    Shared Sub MakeSoftLink(name As String, target As String, link As String, msg As String)
        If s.Storage.GetString(name + "softlink") <> target OrElse Not link.FileExists OrElse
            New FileInfo(link).Length > 0 Then

            DeleteSoftLink(link)
            MakeSoftLink(target, link, msg)
            s.Storage.SetString(name + "softlink", target)
        End If
    End Sub

    Shared Sub MakeSoftLink(target As String, link As String, msg As String)
        'return value not working, known Windows bug
        CreateSymbolicLink(link, target, 2)

        If Not File.Exists(link) Then
            MsgError("Failed to create soft link", link + BR2 + msg)
            Throw New AbortException()
        End If
    End Sub

    Shared Sub DeleteSoftLink(path As String)
        Try
            If File.Exists(path) Then
                File.Delete(path)
            End If
        Catch ex As Exception
            MsgError("Failed to delete soft link", path + BR2 + ex.Message)
            Throw New AbortException()
        End Try
    End Sub

    <DllImport("kernel32.dll", CharSet:=CharSet.Unicode)>
    Shared Function CreateSymbolicLink(link As String, target As String, flags As Integer) As Boolean
    End Function
End Class
