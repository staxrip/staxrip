Imports System.Text

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

        g.DefaultCommands.ExecuteCommandLine(Package.Items("ffmpeg").Path.Escape + " -ss " & Seek & ".0 -t " & Duration & ".0 -i " + inputFile + " -vf " + """" + "fps=" & Rate & ",scale=" & Size & ":-1:flags=spline,palettegen=stats_mode=" & Mode & """ -loglevel quiet  -y " + cachePath, False, False, False)
        g.DefaultCommands.ExecuteCommandLine(Package.Items("ffmpeg").Path.Escape + " -ss " & Seek & ".0 -t " & Duration & ".0 -i " + inputFile + " -i " + cachePath + " -lavfi " + """" + "fps=" & Rate & ",scale=" & Size & ":-1:flags=spline [x]; [x][1:v] paletteuse=" & Dither & """ -loglevel quiet  -y " + OutPutPath, False, False, False)

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

        g.DefaultCommands.ExecuteCommandLine(Package.Items("MTN").Path.Escape + """" + inputFile + """" + " -c " & Col & " -r " & Rows & " -w " & SizeWidth & " -h " & SizeHeight & " -D " & PictureDepth & " -j " & PictureQuality & " -Z ", False, False, False)

    End Sub
End Class
Public Class VDub2PNG
    <Command("Excute VirtualDub2 Script")>
    Shared Sub PNG(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.vd2.VerifyOK(True) Then Exit Sub

        Dim ScriptCode = ""

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        Dim Path = Folder.Temp + Guid.NewGuid.ToString + ".script"
        Dim OutPath = Folder.Temp + Guid.NewGuid.ToString + ".txt"
        Dim ResizeHeight = s.Storage.GetInt("PNGHeight")
        Dim ResizeWidth = s.Storage.GetInt("PNGWidth")
        Dim OutPutPath = inputFile + ".png"
        Dim FrameStarting = s.Storage.GetInt("FrameStart")
        Dim FrameEnding = s.Storage.GetInt("FrameEnd")
        Dim FrameNum = s.Storage.GetInt("RateDen")

        ScriptCode += "VirtualDub.Open(U" + """" + inputFile + """" + ");" + BR
        ScriptCode += "VirtualDub.video.SetInputFormat(0);" + BR
        ScriptCode += "VirtualDub.video.SetOutputFormat(7);" + BR
        ScriptCode += "VirtualDub.video.SetMode(3);" + BR
        ScriptCode += "VirtualDub.video.filters.Add(""Resize"");" + BR + "VirtualDub.video.filters.instance[0].Config(" & ResizeHeight & "," & ResizeWidth & ",4);" + BR
        ScriptCode += "VirtualDub.video.SetCompression(0x31766568,0,10000,0,""avlib-1.vdplugin"");" + BR
        ScriptCode += "VirtualDub.video.SetCompData(20,""AAAAAAMAAAAIAAAABAAAABUAAAA="");" + BR
        ScriptCode += "VirtualDub.video.SetPreserveEmptyFrames(0);" + BR
        ScriptCode += "VirtualDub.video.SetFrameRate2(0,0,1);" + BR
        ScriptCode += "VirtualDub.video.SetTargetFrameRate(" & FrameNum & "0000" + ",10000);" + BR
        ScriptCode += "VirtualDub.video.SetIVTC(0, 0, 0, 0);" + BR
        ScriptCode += "VirtualDub.video.SetRangeFrames(" & FrameStarting & "," & FrameEnding & ");" + BR
        ScriptCode += "VirtualDub.SaveAnimatedPNG(U" + """" & OutPutPath & """" + ", 0, 1, 0);" + BR
        ScriptCode += "VirtualDub.Close();"

        File.WriteAllText(Path, ScriptCode, Encoding.Default)

        g.DefaultCommands.ExecuteCommandLine(Package.Items("VirtualDub 2").Path.Escape + " /min /x /s " + Path, True, False, False)

        FileHelp.Delete(Path)
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

Public Class UpdaterCaller
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