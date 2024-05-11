Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Web.UI.WebControls



<Serializable>
Public Class RpuPreset
    Public ReadOnly Property Id As Integer = 0
    Public ReadOnly Property Offset As New Padding(0)

    Public Sub New(id As Integer, offset As Padding)
        Me.Id = id
        Me.Offset = offset
    End Sub
End Class

<Serializable>
Public Class RpuEdit
    Public ReadOnly Property Id As Integer = 0
    Public ReadOnly Property StartFrame As Integer
    Public ReadOnly Property EndFrame As Integer

    Public Sub New(id As Integer, startFrame As Integer, endFrame As Integer)
        Me.Id = id
        Me.StartFrame = startFrame
        Me.EndFrame = endFrame
    End Sub
End Class

<Serializable>
Public Class DolbyVisionMetadataFile
    Private _autoCropThresholdBegin As Integer = 1000
    Private _autoCropThresholdEnd As Integer = 1000
    Private _lastWriteTime As DateTime = Nothing
    Private _lastWriteTimeL5 As DateTime = Nothing

    Public ReadOnly Property Path As String = Nothing
    Public ReadOnly Property Edits As New List(Of RpuEdit)
    Public ReadOnly Property Presets As New List(Of RpuPreset)

    Public ReadOnly Property LastWriteTime As DateTime
        Get
            Return _lastWriteTime
        End Get
    End Property

    Public ReadOnly Property LastWriteTimeL5 As DateTime
        Get
            Return _lastWriteTimeL5
        End Get
    End Property


    Public ReadOnly Property Level5JsonFilePath As String
        Get
            Return If(String.IsNullOrWhiteSpace(Path), "", $"{Path.DirAndBase()}_L5.json")
        End Get
    End Property

    Public ReadOnly Property EditorConfigFilePath As String
        Get
            Return If(String.IsNullOrWhiteSpace(Path), "", $"{Path.DirAndBase()}_Config.json")
        End Get
    End Property

    Public ReadOnly Property CroppedRpuFilePath As String
        Get
            Return If(String.IsNullOrWhiteSpace(Path), "", $"{Path.DirAndBase()}_Cropped.rpu")
        End Get
    End Property

    Public ReadOnly Property HasLevel5Changed As Boolean
        Get
            Return Level5JsonFilePath.FileExists() AndAlso _lastWriteTimeL5 <> File.GetLastWriteTimeUtc(Level5JsonFilePath)
        End Get
    End Property

    Public ReadOnly Property Crop As Padding
        Get
            Select Case p.AutoCropDolbyVisionMode
                Case AutoCropDolbyVisionMode.Automatic
                    Return GetAutoCrop(_autoCropThresholdBegin, _autoCropThresholdEnd)
                Case AutoCropDolbyVisionMode.ManualThreshold
                    Return GetCrop(p.AutoCropDolbyVisionThresholdBegin, p.AutoCropDolbyVisionThresholdEnd)
                Case Else
                    Throw New NotImplementedException(NameOf(AutoCropDolbyVisionMode))
            End Select
        End Get
    End Property


    Private Sub New()
    End Sub

    Public Sub New(filePath As String)
        Me.Path = filePath
        Me._lastWriteTime = File.GetLastWriteTimeUtc(filePath)

        WriteLevel5Export(False, False)
        ReadLevel5Export()
    End Sub

    Public Function GetAutoCrop() As Padding
        Return GetAutoCrop(_autoCropThresholdBegin, _autoCropThresholdEnd)
    End Function

    Public Function GetAutoCrop(thresholdBegin As Integer, thresholdEnd As Integer) As Padding
        If Not Edits?.Any() Then Return New Padding(0)
        If Not Presets?.Any() Then Return New Padding(0)

        thresholdBegin = Math.Min(Edits.Max(Function(x) x.EndFrame), thresholdBegin)
        thresholdBegin = Math.Max(0, thresholdBegin)
        thresholdEnd = Math.Min(Edits.Max(Function(x) x.EndFrame), thresholdEnd)
        thresholdEnd = Math.Max(0, thresholdEnd)

        Dim newCrop As New Padding(Integer.MaxValue)
        Dim entries = Edits.Join(Presets, Function(edit) edit.Id, Function(preset) preset.Id, Function(edit, preset) New With {edit.StartFrame, edit.EndFrame, edit.Id, preset.Offset}).OrderBy(Function(x) x.StartFrame)

        For i = 0 To entries.Count() - 1
            Dim entry = entries.ElementAt(i)
            Dim same = entries.Where(Function(x) x.Id = entry.Id)
            Dim sameCount = same.Count()
            Dim take = True

            take = If(take AndAlso i = 0 AndAlso sameCount = 1 AndAlso entry.Offset = Padding.Empty AndAlso entry.EndFrame < thresholdBegin, False, take)
            take = If(take AndAlso i = entries.Count() - 1 AndAlso sameCount = 1 AndAlso entry.Offset = Padding.Empty AndAlso entry.StartFrame > entry.EndFrame - thresholdEnd, False, take)
            take = If(take AndAlso Not same.Where(Function(x) (x.EndFrame - x.StartFrame) > 8).Any() AndAlso entry.Offset = Padding.Empty, False, take)

            If take Then
                newCrop.Left = Math.Min(newCrop.Left, entry.Offset.Left)
                newCrop.Top = Math.Min(newCrop.Top, entry.Offset.Top)
                newCrop.Right = Math.Min(newCrop.Right, entry.Offset.Right)
                newCrop.Bottom = Math.Min(newCrop.Bottom, entry.Offset.Bottom)
            End If
        Next

        Return newCrop
    End Function

    Public Function GetCrop() As Padding
        Return GetCrop(0, 0)
    End Function

    Public Function GetCrop(thresholdBegin As Integer, thresholdEnd As Integer) As Padding
        If Not Edits?.Any() Then Return New Padding(0)
        If Not Presets?.Any() Then Return New Padding(0)

        thresholdBegin = Math.Min(Edits.Max(Function(x) x.EndFrame), thresholdBegin)
        thresholdBegin = Math.Max(0, thresholdBegin)
        thresholdEnd = Math.Min(Edits.Max(Function(x) x.EndFrame), thresholdEnd)
        thresholdEnd = Math.Max(0, thresholdEnd)

        Dim newCrop As New Padding(Integer.MaxValue)
        Dim frames = Edits.OrderByDescending(Function(x) x.EndFrame).First().EndFrame
        Dim entries = Edits.Join(Presets, Function(edit) edit.Id, Function(preset) preset.Id, Function(edit, preset) New With {edit.StartFrame, edit.EndFrame, preset.Offset})

        For Each entry In entries
            If entry.EndFrame >= thresholdBegin AndAlso entry.StartFrame <= (frames - thresholdEnd) Then
                newCrop.Left = Math.Min(newCrop.Left, entry.Offset.Left)
                newCrop.Top = Math.Min(newCrop.Top, entry.Offset.Top)
                newCrop.Right = Math.Min(newCrop.Right, entry.Offset.Right)
                newCrop.Bottom = Math.Min(newCrop.Bottom, entry.Offset.Bottom)
            End If
        Next

        Return newCrop
    End Function

    Public Sub RefreshLevel5Data()
        If Not HasLevel5Changed Then Exit Sub
        If Not (p.AutoCropMode = AutoCropMode.DolbyVisionOnly OrElse p.AutoCropMode = AutoCropMode.Always) Then Exit Sub

        ReadLevel5Export()
    End Sub

    Public Sub ReadLevel5Export(Optional logContent As Boolean = True)
        If Not Path?.FileExists() Then Return
        If Not Level5JsonFilePath.FileExists() Then Return

        Edits.Clear()
        Presets.Clear()

        Try
            Dim jsonContent = Level5JsonFilePath.ReadAllText()
            Dim strippedJsonContent = jsonContent.Right("presets").Left("edits")
            strippedJsonContent = Regex.Replace(strippedJsonContent, "\s", "")
            Dim presetMatches = Regex.Matches(strippedJsonContent, "\{.+?:(?<id>\d+),.+?:(?<left>\d+),.+?:(?<right>\d+),.+?:(?<top>\d+),.+?:(?<bottom>\d+)\}")

            If presetMatches.Count = 0 Then Return

            For Each match As Match In presetMatches
                Dim offset = New Padding(match.Groups("left").Value.ToInt(), match.Groups("top").Value.ToInt(), match.Groups("right").Value.ToInt(), match.Groups("bottom").Value.ToInt())
                Presets.Add(New RpuPreset(match.Groups("id").Value.ToInt(), offset))
            Next

            strippedJsonContent = jsonContent.Right("edits")
            strippedJsonContent = Regex.Replace(strippedJsonContent, "\s", "")
            Dim editMatches = Regex.Matches(strippedJsonContent, """(?<start>\d+)-(?<end>\d+)"":(?<id>\d+),?")

            For Each match As Match In editMatches
                Edits.Add(New RpuEdit(match.Groups("id").Value.ToInt(), match.Groups("start").Value.ToInt(), match.Groups("end").Value.ToInt()))
            Next

            _lastWriteTimeL5 = File.GetLastWriteTimeUtc(Level5JsonFilePath)
        Catch ex As AbortException
            Throw ex
        Catch ex As Exception
            g.ShowException(ex)
            Throw New AbortException
        Finally
            If logContent Then
                Dim exists = File.Exists(Level5JsonFilePath)
                Dim content = If(exists, File.ReadAllText(Level5JsonFilePath), "No file found!")
                
                Log.WriteHeader($"Content of {IO.Path.GetFileName(Level5JsonFilePath)}")
                Log.WriteLine(content)
            End If

            Log.Save()
        End Try
    End Sub

    Public Sub WriteLevel5Export(Optional logContent As Boolean = False, Optional overwrite As Boolean = False)
        If Not Path?.FileExists() Then Return
        If Not overwrite AndAlso Level5JsonFilePath.FileExists() Then Return

        Try
            Dim pd = Path.Dir
            Dim l5 = Level5JsonFilePath.Replace(pd, "." & IO.Path.DirectorySeparatorChar)
            Dim arguments = $"export --data ""level5={l5}"" -i ""{Path}"""
            Using proc As New Proc
                proc.Package = Package.DoViTool
                proc.Project = p
                proc.Header = "Extract Level5 data from RPU metadata file"
                proc.Encoding = Encoding.UTF8
                proc.Arguments = arguments
                proc.WorkingDirectory = pd
                proc.AllowedExitCodes = {0}
                proc.OutputFiles = {Level5JsonFilePath}
                proc.Start()
            End Using
        Catch ex As AbortException
            Throw ex
        Catch ex As Exception
            g.ShowException(ex)
            Throw New AbortException
        Finally
            If logContent Then
                Dim exists = File.Exists(Level5JsonFilePath)
                Dim content = If(exists, File.ReadAllText(Level5JsonFilePath), "No file created!")
                
                Log.WriteHeader($"Content of {IO.Path.GetFileName(Level5JsonFilePath)}")
                Log.WriteLine(content)
            End If

            Log.Save()
        End Try
    End Sub

    Public Sub WriteEditorConfigFile(offset As Padding, Optional logContent As Boolean = True, Optional overwrite As Boolean = True)
        If Not Level5JsonFilePath?.FileExists() Then Return
        If Not overwrite AndAlso EditorConfigFilePath.FileExists() Then Return
        If offset = Padding.Empty Then Return
        If offset.Left < 0 OrElse offset.Top < 0 OrElse offset.Right < 0 OrElse offset.Bottom < 0 Then Throw New ArgumentOutOfRangeException(NameOf(offset))

        Try
            Dim p = ""
            For Each entry In Presets.OrderBy(Function(x) x.Id)
                Dim left = Math.Max(0, entry.Offset.Left - offset.Left)
                Dim top = Math.Max(0, entry.Offset.Top - offset.Top)
                Dim right = Math.Max(0, entry.Offset.Right - offset.Right)
                Dim bottom = Math.Max(0, entry.Offset.Bottom - offset.Bottom)

                p += $"{{"
                p += $"""id"":{entry.Id},"
                p += $"""left"":{left},"
                p += $"""right"":{right},"
                p += $"""top"":{top},"
                p += $"""bottom"":{bottom}"
                p += $"}},"
            Next
            p = p.TrimEnd(","c)
            p = $"""presets"":[{p}]"

            Dim e = ""
            For Each entry In Edits.OrderBy(Function(x) x.StartFrame)
                e += $"""{entry.StartFrame}-{entry.EndFrame}"":{entry.Id},"
            Next
            e = e.TrimEnd(","c)
            e = $"""edits"":{{{e}}}"

            Dim result = $"{{""active_area"":{{""crop"":false,{p},{e}}}}}"
            result.WriteFileUTF8(EditorConfigFilePath)
        Catch ex As AbortException
            Throw ex
        Catch ex As Exception
            g.ShowException(ex)
            Throw New AbortException
        Finally
            If logContent Then
                Dim exists = File.Exists(EditorConfigFilePath)
                Dim content = If(exists, File.ReadAllText(EditorConfigFilePath), "No file created!")
                
                Log.WriteHeader($"Content of {IO.Path.GetFileName(EditorConfigFilePath)}")
                Log.WriteLine(content)
            End If

            Log.Save()
        End Try
    End Sub

    Public Function WriteCroppedRpu(Optional overwrite As Boolean = True) As String
        If Not Path?.FileExists() Then Return Nothing
        If Not EditorConfigFilePath?.FileExists() Then Return Nothing
        If Not overwrite AndAlso CroppedRpuFilePath.FileExists() Then Return Nothing

        Try
            Dim arguments = $"editor -i ""{Path}"" -j ""{EditorConfigFilePath}"" -o ""{CroppedRpuFilePath}"""
            Using proc As New Proc
                proc.Package = Package.DoViTool
                proc.Project = p
                proc.Header = "Creating new RPU metadata file"
                proc.Encoding = Encoding.UTF8
                proc.Arguments = arguments
                proc.AllowedExitCodes = {0}
                proc.OutputFiles = {CroppedRpuFilePath}
                proc.Start()
            End Using
        Catch ex As AbortException
            Throw ex
        Catch ex As Exception
            g.ShowException(ex)
            Throw New AbortException
        Finally
            Log.Save()
        End Try

        Return CroppedRpuFilePath
    End Function
End Class
