Imports System.Globalization
Imports Microsoft.Win32

Public Class Log
    Shared StartTime As DateTime

    Shared Event Update(text As String)

    Shared Sub Write(title As String, content As String, Optional proj As Project = Nothing)
        'If proj Is Nothing Then Stop
        If proj Is Nothing Then proj = p
        StartTime = DateTime.Now

        SyncLock proj.Log
            If Not proj.Log.ToString.EndsWith(BR2) Then proj.Log.AppendLine()
            proj.Log.Append(FormatHeader(title))
        End SyncLock

        If content <> "" Then
            If content.EndsWith(BR) Then
                SyncLock proj.Log
                    proj.Log.Append(content)
                End SyncLock
            Else
                SyncLock proj.Log
                    proj.Log.AppendLine(content)
                End SyncLock
            End If
        End If

        RaiseUpdate(proj)
    End Sub

    Shared Sub WriteHeader(value As String, Optional proj As Project = Nothing)
        'If proj Is Nothing Then Stop
        If proj Is Nothing Then proj = p
        StartTime = DateTime.Now

        If value <> "" Then
            SyncLock proj.Log
                If Not proj.Log.ToString.EndsWith(BR2) Then proj.Log.AppendLine()
                proj.Log.Append(FormatHeader(value))
            End SyncLock

            RaiseUpdate(proj)
        End If
    End Sub

    Shared Sub WriteLine(value As String, Optional proj As Project = Nothing)
        'If proj Is Nothing Then Stop
        If proj Is Nothing Then proj = p

        If value <> "" Then
            If value.EndsWith(BR) Then
                SyncLock proj.Log
                    proj.Log.Append(value)
                End SyncLock
            Else
                SyncLock proj.Log
                    proj.Log.AppendLine(value)
                End SyncLock
            End If

            RaiseUpdate(proj)
        End If
    End Sub

    Shared Function FormatHeader(value As String) As String
        Return "-=".Multiply(30) + "-" + BR +
            value.PadLeft(30 + value.Length \ 2) +
            BR + "-=".Multiply(30) + "-" + BR2
    End Function

    Shared Sub WriteEnvironment(Optional proj As Project = Nothing)
        'If proj Is Nothing Then Stop
        If proj Is Nothing Then proj = p
        If proj.Log.ToString.Contains("Environment" + BR + "-=") Then Exit Sub

        Dim staxrip = "-=".Multiply(30) + "-" + BR +
"      _________ __                __________.__        
     /   _____//  |______  ___  __\______   \__|_____  
     \_____  \\   __\__  \ \  \/  /|       _/  \____ \ 
     /        \|  |  / __ \_>    < |    |   \  |  |_> >
    /_______  /|__| (____  /__/\_ \|____|_  /__|   __/ 
            \/           \/      \/       \/   |__|   "

        WriteLine(staxrip, proj)
        WriteHeader("Environment", proj)

        Dim temp =
            "StaxRip:" + Application.ProductVersion + BR +
            "Windows:" + Registry.LocalMachine.GetString("SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ProductName") + " " + Registry.LocalMachine.GetString("SOFTWARE\Microsoft\Windows NT\CurrentVersion", "ReleaseId") + BR +
            "Language:" + CultureInfo.CurrentCulture.EnglishName + BR +
            "CPU:" + Registry.LocalMachine.GetString("HARDWARE\DESCRIPTION\System\CentralProcessor\0", "ProcessorNameString") + BR +
            "GPU:" + String.Join(", ", SystemHelp.VideoControllers) + BR +
            "Resolution:" & Screen.PrimaryScreen.Bounds.Width & " x " & Screen.PrimaryScreen.Bounds.Height & BR +
            "DPI:" & g.MainForm.DeviceDpi

        WriteLine(temp.FormatColumn(":"), proj)
    End Sub

    Shared Function GetPath(Optional proj As Project = Nothing) As String
        If proj Is Nothing Then proj = p

        If p.SourceFile = "" Then
            Return Folder.Temp + "staxrip.log"
        Else
            Return proj.TempDir + proj.Name + "_staxrip.log"
        End If
    End Function

    Shared Sub Save(Optional proj As Project = Nothing)
        'If proj Is Nothing Then Stop
        If proj Is Nothing Then proj = p

        If proj.SourceFile <> "" Then
            SyncLock proj.Log
                If Directory.Exists(proj.TempDir) Then
                    proj.Log.ToString.WriteUTF8File(GetPath(proj))
                End If
            End SyncLock
        End If
    End Sub

    Shared Sub WriteStats(Optional proj As Project = Nothing)
        'If proj Is Nothing Then Stop
        WriteStats(StartTime, proj)
    End Sub

    Shared Sub WriteStats(start As DateTime, Optional proj As Project = Nothing)
        'If proj Is Nothing Then Stop
        If proj Is Nothing Then proj = p

        Dim n = DateTime.Now.Subtract(start)

        SyncLock proj.Log
            If Not proj.Log.ToString.EndsWith(BR2) Then proj.Log.AppendLine()
            proj.Log.Append("Start: ".PadRight(10) + start.ToLongTimeString + BR)
            proj.Log.Append("End: ".PadRight(10) + DateTime.Now.ToLongTimeString + BR)
            proj.Log.Append("Duration: " + CInt(Math.Floor(n.TotalHours)).ToString("d2") + ":" + n.Minutes.ToString("d2") + ":" + n.Seconds.ToString("d2") + BR)
            proj.Log.AppendLine()
        End SyncLock

        RaiseUpdate(proj)
    End Sub

    Private Shared Sub RaiseUpdate(proj As Project)
        SyncLock proj.Log
            RaiseEvent Update(proj.Log.ToString)
        End SyncLock
    End Sub
End Class