
Public Class FrameServerHelp
    Shared WasInitialized As Boolean

    Shared Sub Init()
        If Not WasInitialized Then
            g.AddToPath(Folder.Startup, Package.Python.Directory, Package.AviSynth.Directory,
                        Package.VapourSynth.Directory, Package.FFTW.Directory)

            If IsAviSynthPortableUsed() Then
                DirectoryHelp.Create(Folder.Settings + "Plugins\AviSynth")
            End If

            If IsVapourSynthPortableUsed() Then
                DirectoryHelp.Create(Folder.Settings + "Plugins\VapourSynth")
            End If

            Try
                MakeAviSynthSoftLinks()
                WasInitialized = True
            Catch ex As Exception
                g.ShowException(ex)
            End Try
        End If
    End Sub

    Shared Function IsAviSynthPortableUsed() As Boolean
        Return Package.AviSynth.Directory.StartsWithEx(Folder.Apps)
    End Function

    Shared Function IsVapourSynthPortableUsed() As Boolean
        Return Package.VapourSynth.Directory.StartsWithEx(Folder.Apps)
    End Function

    Shared Function IsPortable() As Boolean
        If (p.Script.Engine = ScriptEngine.AviSynth AndAlso IsAviSynthPortableUsed()) OrElse
            (p.Script.Engine = ScriptEngine.VapourSynth AndAlso IsVapourSynthPortableUsed()) Then

            Return True
        End If
    End Function

    Shared Function IsAviSynthInstalled() As Boolean
        Return (Folder.System + "AviSynth.dll").FileExists
    End Function

    Shared Sub MakeAviSynthSoftLinks()
        Dim packs = {Package.NVEnc, Package.QSVEnc, Package.VCEEnc, Package.avs2pipemod,
                     Package.x264, Package.x265, Package.mpvnet}

        If IsAviSynthPortableUsed() Then
            MakeSoftLink("avisynth to ffmpeg", Package.AviSynth.Path, Package.ffmpeg.Directory + "AviSynth.dll")
            MakeSoftLink("avisynth to staxrip", Package.AviSynth.Path, Folder.Startup + "AviSynth.dll")

            For Each pack In packs
                MakeSoftLink("avisynth to " + pack.Name, Package.AviSynth.Path, pack.Directory + "AviSynth.dll")
            Next
        Else
            DeleteFile(Package.ffmpeg.Directory + "AviSynth.dll")
            DeleteFile(Folder.Startup + "AviSynth.dll")

            For Each pack In packs
                DeleteFile(pack.Directory + "AviSynth.dll")
            Next
        End If
    End Sub

    Shared Sub MakeSoftLink(name As String, source As String, link As String)
        If s.Storage.GetString(name + "symlink") <> source OrElse Not link.FileExists OrElse
            New FileInfo(link).Length > 0 Then

            DeleteFile(link)
            MakeSoftLink(source, link)
            s.Storage.SetString(name + "symlink", source)
        End If
    End Sub

    Shared Sub MakeSoftLink(source As String, link As String)
        Dim cmd = $"mklink {link.Escape} {source.Escape}"

        Using proc As New Process
            proc.StartInfo.FileName = "cmd.exe"
            proc.StartInfo.Arguments = $"/S /C ""{cmd}"""
            proc.StartInfo.UseShellExecute = False
            proc.StartInfo.CreateNoWindow = True
            proc.Start()
            proc.WaitForExit()

            If proc.ExitCode <> 0 Then
                MsgError("Failed to create soft link." + BR2 + link)
            End If
        End Using
    End Sub

    Shared Sub DeleteFile(path As String)
        Try
            If File.Exists(path) Then
                File.Delete(path)
            End If
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Sub
End Class
