
Public Class FrameServerHelp
    Shared WasInitialized As Boolean

    Shared Sub Init()
        If Not WasInitialized Then
            g.AddToPath(Folder.Startup, Package.Python.Directory, Package.AviSynth.Directory,
                        Package.VapourSynth.Directory, Package.FFTW.Directory)

            DirectoryHelp.Create(Folder.Settings + "Plugins\AviSynth")
            DirectoryHelp.Create(Folder.Settings + "Plugins\VapourSynth")
            MakeSymLinks()
            WasInitialized = True
        End If
    End Sub

    Shared Function IsPortable() As Boolean
        If (p.Script.Engine = ScriptEngine.AviSynth AndAlso
            Package.AviSynth.Directory.StartsWithEx(Folder.Apps)) OrElse
            (p.Script.Engine = ScriptEngine.VapourSynth AndAlso
            Package.VapourSynth.Directory.StartsWithEx(Folder.Apps)) Then

            Return True
        End If
    End Function

    Shared Sub MakeSymLinks()
        Dim pack = Package.ffmpeg
        Dim path = pack.Directory + "AviSynth.dll"

        If Package.AviSynth.Path.StartsWith(Folder.Startup) Then
            If s.Storage.GetString(pack.Name + "symlink") <> path OrElse Not path.FileExists Then
                FileHelp.Delete(path)
                Dim cmd = $"mklink {path.Escape} {Package.AviSynth.Path.Escape}"

                Using proc As New Process
                    proc.StartInfo.FileName = "cmd.exe"
                    proc.StartInfo.Arguments = $"/S /C ""{cmd}"""
                    proc.StartInfo.UseShellExecute = False
                    proc.StartInfo.CreateNoWindow = True
                    proc.Start()
                End Using

                s.Storage.SetString(pack.Name + "symlink", path)
            End If
        Else
            FileHelp.Delete(path)
        End If
    End Sub
End Class
