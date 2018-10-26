Imports System.Text
Public Class GIF
    Shared Sub GIFAnimation(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.ffmpeg.VerifyOK(True) Then Exit Sub

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        Dim Rate = s.Storage.GetInt("GifFrameRate", 15)
        Dim cachePath = Folder.Temp + "Palette.png"
        Dim OutPutPath = inputFile + ".gif"
        Dim Seek = s.Storage.GetString("GifTime", 25)
        Dim Duration = s.Storage.GetString("GifLength", 4)
        Dim Size = s.Storage.GetInt("GifScale", 480)
        Dim Mode = s.Storage.GetString("PaletteGen", "diff")
        Dim SecondMode = s.Storage.GetString("PaletteUse", "rectangle")
        Dim Dither = s.Storage.GetString("GifDither", "dither=bayer:bayer_scale=5")

        Using Proc As New Proc
            Proc.Header = "Creating Gif"
            Proc.SkipStrings = {"frame=", "size="}
            Proc.Encoding = Encoding.UTF8
            Proc.Package = Package.ffmpeg
            Proc.Arguments = " -ss " & Seek & " -t " & Duration & " -i " + """" + inputFile + """" + " -vf " + """" + "fps=" & Rate & ",scale=" & Size & ":-1:flags=spline,palettegen=stats_mode=" & Mode & """ -loglevel quiet -an -y " + cachePath
            Proc.Start()
        End Using

        Using Proc As New Proc
            Proc.Header = "Creating Gif"
            Proc.SkipStrings = {"frame=", "size="}
            Proc.Encoding = Encoding.UTF8
            Proc.Package = Package.ffmpeg
            Proc.Arguments = " -ss " & Seek & " -t " & Duration & " -i " + """" + inputFile + """" + " -i " + cachePath + " -lavfi " + """" + "fps=" & Rate & ",scale=" & Size & ":-1:flags=spline [x]; [x][1:v] paletteuse=" & Dither & ":diff_mode=" & SecondMode & """ -loglevel quiet -an -y " + """" + OutPutPath + """"
            Proc.Start()
        End Using

        FileHelp.Delete(cachePath)

    End Sub
End Class

 Class MTN
    Shared Sub Thumbnails(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.MTN.VerifyOK(True) Then Exit Sub

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        Dim Col = s.Storage.GetInt("MTNColumn", 4)
        Dim Rows = s.Storage.GetInt("MTNRow", 6)
        Dim SizeWidth = s.Storage.GetInt("MTNWidth", 1280)
        Dim SizeHeight = s.Storage.GetInt("MTNHeight", 150)
        Dim PictureQuality = s.Storage.GetInt("MTNQuality", 95)
        Dim PictureDepth = s.Storage.GetInt("MTNDepth", 12)

        Using Proc As New Proc
            Proc.Header = "Creating Thumbnail"
            Proc.SkipStrings = {"frame=", "size="}
            Proc.Encoding = Encoding.UTF8
            Proc.Package = Package.MTN
            Proc.Arguments = """" + inputFile + """" + " -c " & Col & " -r " & Rows & " -w " & SizeWidth & " -h " & SizeHeight & " -D " & PictureDepth & " -j " & PictureQuality & " -P "
            Proc.Start()
        End Using

    End Sub
End Class
Public Class PNG

    Shared Sub aPNGAnimation(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.ffmpeg.VerifyOK(True) Then Exit Sub
        If Not Package.PNGopt.VerifyOK(True) Then Exit Sub

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        Dim Rate = s.Storage.GetInt("PNGFrameRate", 15)
        Dim Path = inputFile + ".apng"
        Dim NewPath = inputFile + ".png"
        Dim OptOut = inputFile + "_opt.png"
        Dim Seek = s.Storage.GetString("PNGTime", 25)
        Dim Duration = s.Storage.GetString("PNGLength", 4)
        Dim Size = s.Storage.GetInt("PNGScale", 480)
        Dim OptSettings = s.Storage.GetString("PNGopt", "-z1")
        Dim Opt = s.Storage.GetBool("OptSetting", False)

        Using Proc As New Proc
            Proc.Header = "Encoding PNG"
            Proc.SkipStrings = {"frame=", "size="}
            Proc.Encoding = Encoding.UTF8
            Proc.Package = Package.ffmpeg
            Proc.Arguments = " -ss " & Seek & " -t " & Duration & " -i " + """" + inputFile + """" + " -lavfi " + """" + "fps=" & Rate & ",scale=" & Size & ":-1:flags=spline" + """ -plays 0 -loglevel quiet -an -y " + """" + Path + """"
            Proc.Start()
        End Using

        File.Move(Path, NewPath)

        If Opt = True Then

            Using Proc As New Proc
                Proc.Header = "Optimizing PNG"
                Proc.SkipStrings = {"saving", "Reading", "all done", "APNG"}
                Proc.Encoding = Encoding.UTF8
                Proc.Package = Package.PNGopt
                Proc.Arguments = OptSettings + " " + """" + NewPath + """" + " " + """" + OptOut + """"
                Proc.Start()
            End Using

            File.Delete(NewPath)

        End If

    End Sub
End Class
Public Class MKVInfoLookup
    Shared Sub MetadataInfo(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.mkvinfo.VerifyOK(True) Then Exit Sub

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        g.DefaultCommands.ExecuteCommandLine(Package.Items("mkvinfo").Path.Escape + " " + """" + inputFile + """" + BR + "pause", False, False, True)

    End Sub
End Class
Public Class MKVMetaDataHDR
    Shared Sub MetadataHDR(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.mkvmerge.VerifyOK(True) Then Exit Sub

        Using Proc As New Proc
            Proc.Header = "Adding HDR Metadata"
            Proc.SkipStrings = {"Progress", "The file", "The cue", "Multiplexing"}
            Proc.Encoding = Encoding.UTF8
            Proc.Package = Package.mkvmerge
            Proc.Arguments = "-o " + """" + inputFile + "_HDR.mkv" + """" + " --colour-matrix 0:9 --colour-range 0:1 --colour-transfer-characteristics 0:16 --colour-primaries 0:9 --max-content-light 0:1000 --max-frame-light 0:300 --max-luminance 0:1000 --min-luminance 0:0.01 --chromaticity-coordinates 0:0.68,0.32,0.265,0.690,0.15,0.06 --white-colour-coordinates 0:0.3127,0.3290 " + """" + inputFile + """"
            Proc.Start()
        End Using

    End Sub
End Class
Public Class UpdateStaxRip
    Shared Sub CheckforUpdate()
        If Not Package.Update.VerifyOK(True) Then Exit Sub

        Dim UpdateCode = ""
        Dim Path = Folder.Startup + "Update" + ".bat"

        UpdateCode += "@echo OFF" + BR
        UpdateCode += "pushd %~dp0" + BR
        UpdateCode += "if exist ""%~dp0\Apps\Update\update.ps1"" (" + BR
        UpdateCode += "set update_script=""Apps\Update\update.ps1""" + BR
        UpdateCode += ") else (" + BR
        UpdateCode += "set update_script=""Apps\update.ps1""" + BR
        UpdateCode += ")" + BR
        UpdateCode += "powershell -noprofile -nologo -noexit -executionpolicy bypass -File %update_script%" + BR
        UpdateCode += "@exit"

        File.WriteAllText(Path, UpdateCode, Encoding.Default)
        g.DefaultCommands.ExecuteCommandLine(Path, False, False, False)

    End Sub
End Class