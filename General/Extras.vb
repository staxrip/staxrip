
Imports System.Text

Public Class Animation
    Shared Sub GIF(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.ffmpeg.VerifyOK(True) Then Exit Sub

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        Dim Rate = s.Storage.GetInt("GifFrameRate", 15)
        Dim cachePath = Folder.Temp + "Palette.png"
        Dim OutPutPath = inputFile.ChangeExt("gif")
        Dim Seek = s.Storage.GetDouble("GifTime", 15.0)
        Dim Duration = s.Storage.GetDouble("GifLength", 4.2)
        Dim Size = s.Storage.GetInt("GifScale", 480)
        Dim Mode = s.Storage.GetString("PaletteGen", "diff")
        Dim SecondMode = s.Storage.GetString("PaletteUse", "rectangle")
        Dim Dither = s.Storage.GetString("GifDither", "dither=floyd_steinberg")
        Dim DirectoryStatus = s.Storage.GetBool("GifOutput", False)
        Dim DirectoryLocation = s.Storage.GetString("GifDirectory", p.DefaultTargetFolder)
        Dim Export = DirectoryLocation + "\" + inputFile.Base.ChangeExt("gif")

        Using Proc As New Proc
            Proc.Header = "Creating Gif"
            Proc.SkipStrings = {"frame=", "size="}
            Proc.Encoding = Encoding.UTF8
            Proc.Package = Package.ffmpeg
            Proc.Arguments = " -ss " & Seek & " -t " & Duration & " -i " + """" + inputFile + """" + " -vf " + """" + "fps=" & Rate & ",scale=" & Size & ":-1:flags=spline,palettegen=stats_mode=" & Mode & """ -loglevel quiet -an -y " + cachePath
            Proc.Start()
        End Using

        If DirectoryStatus = True Then

            Using Proc As New Proc
                Proc.Header = "Creating Gif"
                Proc.SkipStrings = {"frame=", "size="}
                Proc.Encoding = Encoding.UTF8
                Proc.Package = Package.ffmpeg
                Proc.Arguments = " -ss " & Seek & " -t " & Duration & " -i " + """" + inputFile + """" + " -i " + cachePath + " -lavfi " + """" + "fps=" & Rate & ",scale=" & Size & ":-1:flags=spline [x]; [x][1:v] paletteuse=" & Dither & ":diff_mode=" & SecondMode & """ -loglevel quiet -an -y " + """" + Export + """"
                Proc.Start()
            End Using

        Else

            Using Proc As New Proc
                Proc.Header = "Creating Gif"
                Proc.SkipStrings = {"frame=", "size="}
                Proc.Encoding = Encoding.UTF8
                Proc.Package = Package.ffmpeg
                Proc.Arguments = " -ss " & Seek & " -t " & Duration & " -i " + """" + inputFile + """" + " -i " + cachePath + " -lavfi " + """" + "fps=" & Rate & ",scale=" & Size & ":-1:flags=spline [x]; [x][1:v] paletteuse=" & Dither & ":diff_mode=" & SecondMode & """ -loglevel quiet -an -y " + """" + OutPutPath + """"
                Proc.Start()
            End Using

        End If
        Try
            FileHelp.Delete(cachePath)
        Catch ex As Exception
        End Try

    End Sub

    Shared Sub aPNG(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then Exit Sub
        If Not Package.ffmpeg.VerifyOK(True) Then Exit Sub
        If Not Package.PNGopt.VerifyOK(True) Then Exit Sub

        If proj Is Nothing Then
            proj = New Project
            proj.Init()
            proj.SourceFile = inputFile
        End If

        Dim Rate = s.Storage.GetInt("PNGFrameRate", 15)
        Dim Path = inputFile.ChangeExt("apng")
        Dim OptOut = inputFile.ChangeExt(".png").Replace(".png", "_opt.png")
        Dim Seek = s.Storage.GetString("PNGTime", "15.0")
        Dim Duration = s.Storage.GetString("PNGLength", "3.8")
        Dim Size = s.Storage.GetInt("PNGScale", 480)
        Dim OptSettings = s.Storage.GetString("PNGopt", "-z1")
        Dim Opt = s.Storage.GetBool("OptSetting", False)
        Dim DirectoryStatus = s.Storage.GetBool("PNGOutput", False)
        Dim DirectoryLocation = s.Storage.GetString("PNGDirectory", p.DefaultTargetFolder)
        Dim Export = DirectoryLocation + "\" + inputFile.Base.ChangeExt("png")
        Dim NewFile = inputFile.ChangeExt("png")

        Using Proc As New Proc
            Proc.Header = "Encoding PNG"
            Proc.SkipStrings = {"frame=", "size="}
            Proc.Encoding = Encoding.UTF8
            Proc.Package = Package.ffmpeg
            Proc.Arguments = " -ss " & Seek & " -t " & Duration & " -i " + """" + inputFile + """" + " -lavfi " + """" + "fps=" & Rate & ",scale=" & Size & ":-1:flags=spline" + """ -plays 0 -loglevel quiet -an -y " + """" + Path + """"
            Proc.Start()
        End Using

        If File.Exists(NewFile) = True Then
            File.Delete(NewFile)
        End If


        If Opt = True Then
            If File.Exists(OptOut) = True Then
                File.Delete(OptOut)
            End If
            If DirectoryStatus = True Then
                Using Proc As New Proc
                    Proc.Header = "Optimizing PNG"
                    Proc.SkipStrings = {"saving", "Reading", "all done", "APNG"}
                    Proc.Encoding = Encoding.UTF8
                    Proc.Package = Package.PNGopt
                    Proc.Arguments = OptSettings + " " + """" + Path + """" + " " + """" + Export + """"
                    Proc.Start()
                End Using
            Else
                Using Proc As New Proc
                    Proc.Header = "Optimizing PNG"
                    Proc.SkipStrings = {"saving", "Reading", "all done", "APNG"}
                    Proc.Encoding = Encoding.UTF8
                    Proc.Package = Package.PNGopt
                    Proc.Arguments = OptSettings + " " + """" + Path + """" + " " + """" + OptOut + """"
                    Proc.Start()
                End Using
            End If
            Try
                File.Delete(Path)
            Catch ex As Exception
            End Try
        Else
            File.Move(Path, NewFile)
        End If

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
        Dim SizeWidth = s.Storage.GetInt("MTNWidth", 1920)
        Dim SizeHeight = s.Storage.GetInt("MTNHeight", 200)
        Dim PictureQuality = s.Storage.GetInt("MTNQuality", 95)
        Dim PictureDepth = s.Storage.GetInt("MTNDepth", 12)
        Dim DirectoryStatus = s.Storage.GetBool("MTNOutput", False)
        Dim DirectoryLocation = s.Storage.GetString("MTNDirectory", p.DefaultTargetFolder)

        If DirectoryStatus = True Then

            Using Proc As New Proc
                Proc.Header = "Creating Thumbnail"
                Proc.SkipStrings = {"frame=", "size="}
                Proc.Encoding = Encoding.UTF8
                Proc.Package = Package.MTN
                Proc.Arguments = """" + inputFile + """" + " -c " & Col & " -r " & Rows & " -h " & SizeHeight & " -w " & SizeWidth & " -j " & PictureQuality & " -D " & PictureDepth & " -P " & " -O " & """" & DirectoryLocation & """"
                Proc.Start()
            End Using

        Else

            Using Proc As New Proc
                Proc.Header = "Creating Thumbnail"
                Proc.SkipStrings = {"frame=", "size="}
                Proc.Encoding = Encoding.UTF8
                Proc.Package = Package.MTN
                Proc.Arguments = """" + inputFile + """" + " -c " & Col & " -r " & Rows & " -h " & SizeHeight & " -w " & SizeWidth & " -j " & PictureQuality & " -D " & PictureDepth & " -P "
                Proc.Start()
            End Using

        End If

    End Sub
End Class

Public Class MKVInfo
    Shared Sub MetadataHDR(inputFile As String, proj As Project)
        If Not File.Exists(inputFile) Then
            Exit Sub
        End If

        Dim HDR = MediaInfo.GetVideo(inputFile, "transfer_characteristics")

        If HDR = "PQ" Then
            Using Proc As New Proc
                Proc.Header = "Adding HDR Metadata"
                Proc.SkipStrings = {"Progress", "The file", "The cue", "Multiplexing"}
                Proc.Encoding = Encoding.UTF8
                Proc.Package = Package.mkvmerge
                Proc.Arguments = "-o " + """" + inputFile + "_HDR10.mkv" + """" + " --colour-matrix 0:9 --colour-range 0:1 --colour-transfer-characteristics 0:16 --colour-primaries 0:9 --max-content-light 0:1000 --max-frame-light 0:300 --max-luminance 0:1000 --min-luminance 0:0.01 --chromaticity-coordinates 0:0.68,0.32,0.265,0.690,0.15,0.06 --white-colour-coordinates 0:0.3127,0.3290 " + """" + inputFile + """"
                Proc.Start()
            End Using
        ElseIf HDR = "HLG" Then
            Using Proc As New Proc
                Proc.Header = "Adding HDR Metadata"
                Proc.SkipStrings = {"Progress", "The file", "The cue", "Multiplexing"}
                Proc.Encoding = Encoding.UTF8
                Proc.Package = Package.mkvmerge
                Proc.Arguments = "-o " + """" + inputFile + "_HLG.mkv" + """" + " --colour-matrix 0:9 --colour-range 0:1 --colour-transfer-characteristics 0:18 --colour-primaries 0:9 --max-content-light 0:1000 --max-frame-light 0:300 --max-luminance 0:1000 --min-luminance 0:0.01 --chromaticity-coordinates 0:0.68,0.32,0.265,0.690,0.15,0.06 --white-colour-coordinates 0:0.3127,0.3290 " + """" + inputFile + """"
                Proc.Start()
            End Using
        Else
            MsgInfo("No HDR Metadata Key was Found")
        End If
    End Sub
End Class