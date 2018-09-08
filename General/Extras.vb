Public Class GifMaker
    Shared Sub Creatator(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.ffmpeg.VerifyOK(True) Then Exit Sub

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        Dim Rate = s.Storage.GetInt("FrameRate")
        Dim cachePath = Folder.Temp + "Palette.png"
        Dim OutPutPath = inputFile + ".gif"
        Dim Seek = s.Storage.GetInt("Time")
        Dim Duration = s.Storage.GetInt("Length")
        Dim Size = s.Storage.GetInt("Scale")
        Dim Mode = s.Storage.GetString("Status")
        Dim Dither = s.Storage.GetString("Dither")


        g.DefaultCommands.ExecuteCommandLine(Package.Items("ffmpeg").Path.Escape + " -ss " & Seek & ".0 -t " & Duration & ".0 -i " + inputFile + " -vf " + """" + "fps=" & Rate & ",scale=" & Size & ":-1:flags=spline,palettegen=stats_mode=" & Mode & """ -loglevel quiet  -y " + cachePath, True, True, False)
        g.DefaultCommands.ExecuteCommandLine(Package.Items("ffmpeg").Path.Escape + " -ss " & Seek & ".0 -t " & Duration & ".0 -i " + inputFile + " -i " + cachePath + " -lavfi " + """" + "fps=" & Rate & ",scale=" & Size & ":-1:flags=spline [x]; [x][1:v] paletteuse=" & Dither & """ -loglevel quiet  -y " + OutPutPath, True, True, False)

        FileHelp.Delete(cachePath)


    End Sub

End Class


Public Class MTN
    Shared Sub Thumbnails(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.MTN.VerifyOK(True) Then Exit Sub

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        Dim Col = s.Storage.GetInt("Column")
        Dim Rows = s.Storage.GetInt("Row")
        Dim SizeWidth = s.Storage.GetInt("Width")
        Dim SizeHeight = s.Storage.GetInt("Height")
        Dim PictureQuality = s.Storage.GetInt("Quality")
        Dim PictureDepth = s.Storage.GetInt("Depth")

        g.DefaultCommands.ExecuteCommandLine(Package.Items("MTN").Path.Escape + """" + inputFile + """" + " -c " & Col & " -r " & Rows & " -w " & SizeWidth & " -h " & SizeHeight & " -D " & PictureDepth & " -j " & PictureQuality & " -Z ", True, True, False)

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

        g.DefaultCommands.ExecuteCommandLine(Package.Items("mkvinfo").Path.Escape + " " + inputFile + BR + "pause", False, False, False)

    End Sub
End Class

Public Class MKVMetaDataHDR
    Shared Sub MetadataHDR(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.mkvmerge.VerifyOK(True) Then Exit Sub

        g.DefaultCommands.ExecuteCommandLine(Package.Items("mkvmerge").Path.Escape + " " + "-o %target_dir%%target_name%_HDR.%muxer_ext%" + " --colour-matrix 0:9 --colour-range 0:1 --colour-transfer-characteristics 0:16 --colour-primaries 0:9 --max-content-light 0:1000 --max-frame-light 0:300 --max-luminance 0:1000 --min-luminance 0:0.01 --chromaticity-coordinates 0:0.68,0.32,0.265,0.690,0.15,0.06 --white-colour-coordinates 0:0.3127,0.3290 " + inputFile, True, True, False)

    End Sub
End Class