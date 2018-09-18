Imports System.Globalization
Imports System.Text
Imports Microsoft.Win32

<Serializable>
Public Class LogBuilder
    Private StartTime As DateTime
    Private Log As New StringBuilder
    Private Last As String

    Sub Append(content As String)
        SyncLock Log
            Log.Append(content)
            If content <> "" Then Last = content
        End SyncLock
    End Sub

    Function EndsWith(value As String) As Boolean
        If Last = "" Then Return False
        Return Last.EndsWith(value)
    End Function

    Private Shared WriteLock As New Object

    Sub Write(title As String, content As String)
        SyncLock WriteLock
            StartTime = DateTime.Now
            If Not EndsWith(BR2) Then Append(BR)
            Append(FormatHeader(title))

            If content <> "" Then
                If content.EndsWith(BR) Then
                    Append(content)
                Else
                    Append(content + BR)
                End If
            End If
        End SyncLock
    End Sub

    Sub WriteLine(value As String)
        If value <> "" Then
            If value.EndsWith(BR) Then
                Append(value)
            Else
                Append(value + BR)
            End If
        End If
    End Sub

    ReadOnly Property Length As Integer
        Get
            SyncLock Log
                Return Log.Length
            End SyncLock
        End Get
    End Property

    Sub WriteHeader(value As String)
        StartTime = DateTime.Now

        If value <> "" Then
            If Not EndsWith(BR2) Then Append(BR)
            Append(FormatHeader(value))
        End If
    End Sub

    Function FormatHeader(value As String) As String
        Dim len = (70 - value.Length) \ 2
        Return "-".Multiply(len) + " " + value + " " + "-".Multiply(len) + BR2
    End Function

    Shared EnvironmentString As String 'cached due to bug report

    Sub WriteEnvironment()
        If ToString.Contains("- System Environment -") Then Exit Sub
        WriteHeader("System Environment")

        If EnvironmentString = "" Then EnvironmentString =
            "StaxRip:" + Application.ProductVersion + BR +
            "Windows:" + Registry.LocalMachine.GetString("SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName") + " " + Registry.LocalMachine.GetString("SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId") + BR +
            "Language:" + CultureInfo.CurrentCulture.EnglishName + BR +
            "CPU:" + Registry.LocalMachine.GetString("HARDWARE\DESCRIPTION\System\CentralProcessor\0", "ProcessorNameString") + BR +
            "GPU:" + String.Join(", ", SystemHelp.VideoControllers) + BR +
            "Resolution:" & Screen.PrimaryScreen.Bounds.Width & " x " & Screen.PrimaryScreen.Bounds.Height & BR +
            "DPI:" & g.MainForm.DeviceDpi

        WriteLine(EnvironmentString.FormatColumn(":"))
    End Sub

    Sub WriteStats()
        WriteStats(StartTime)
    End Sub

    Sub WriteStats(start As DateTime)
        Dim n = DateTime.Now.Subtract(start)
        If Not EndsWith(BR2) Then Append(BR)
        Append("Start: ".PadRight(10) + start.ToLongTimeString + BR)
        Append("End: ".PadRight(10) + DateTime.Now.ToLongTimeString + BR)
        Append("Duration: " + CInt(Math.Floor(n.TotalHours)).ToString("d2") + ":" + n.Minutes.ToString("d2") + ":" + n.Seconds.ToString("d2") + BR2)
    End Sub

    Function IsEmpty() As Boolean
        SyncLock Log
            Return Log.Length = 0
        End SyncLock
    End Function

    Public Overrides Function ToString() As String
        SyncLock Log
            Return Log.ToString
        End SyncLock
    End Function

    Sub Save(Optional proj As Project = Nothing)
        If proj Is Nothing Then proj = p

        SyncLock Log
            Log.ToString.WriteUTF8File(GetPath(proj))
        End SyncLock
    End Sub

    Function GetPath(Optional proj As Project = Nothing) As String
        If proj Is Nothing Then proj = p

        If proj.SourceFile = "" Then
            Return Folder.Temp + "staxrip.log"
        ElseIf proj.TempDir = "" Then
            Dim ret = proj.SourceFile.DirAndBase + "_staxrip.log"
            If ret.Length > 259 Then ret = proj.SourceFile.Dir + proj.SourceFile.Base.Shorten(10) + "_staxrip.log"
            Return ret
        Else
            Dim ret = proj.TempDir + proj.TargetFile.Base + "_staxrip.log"
            If ret.Length > 259 Then ret = proj.TempDir + proj.TargetFile.Base.Shorten(10) + "_staxrip.log"
            Return ret
        End If
    End Function
End Class