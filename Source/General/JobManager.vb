
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Threading

<Serializable>
Public Class Job
    Property Name As String
    Property Path As String
    Property Active As Boolean

    Sub New(name As String, path As String)
        Me.Name = name
        Me.Path = path
        Active = True
    End Sub

    Public Overrides Function ToString() As String
        Return Name
    End Function
End Class

Public Class JobManager
    Shared ReadOnly Property ActiveJobs As IEnumerable(Of Job)
        Get
            Return From i In GetJobs() Where i.Active = True
        End Get
    End Property

    Shared ReadOnly Property JobPath As String
        Get
            Dim name = p.TargetFile.Base

            If name = "" Then
                Dim enc = TryCast(p.VideoEncoder, BasicVideoEncoder)
                Dim temp = enc?.OverridingTargetFileName
                If enc?.OverridesTargetFileName AndAlso temp <> "" AndAlso temp.IsValidFileSystemName() Then
                    name = temp
                End If
            End If

            If name = "" Then
                Dim temp = Macro.Expand(p.DefaultTargetName)
                If temp.IsValidFileSystemName() Then
                    name = temp
                End If
            End If

            If name = "" Then
                name = p.SourceFile.Base
            End If

            Return Path.Combine(p.TempDir, name & ".srip")
        End Get
    End Property

    Shared Sub RemoveJob(path As String)
        Dim jobs = GetJobs()

        For Each job In jobs.ToArray
            If job.Path = path Then
                jobs.Remove(job)
                SaveJobs(jobs)
            End If
        Next
    End Sub

    Shared Sub ActivateJob(path As String, Optional isActive As Boolean = True)
        Dim jobs = GetJobs()

        For Each job In jobs
            If job.Path = path Then
                job.Active = isActive
            End If
        Next

        SaveJobs(jobs)
    End Sub

    Shared Sub AddJob(name As String, path As String, Optional position As Integer = -1)
        Dim jobs = GetJobs()

        For Each job In jobs.ToArray
            If job.Path.ToLowerEx() = path.ToLowerEx() Then
                jobs.Remove(job)
            End If
        Next

        If position = -1 Then
            jobs.Add(New Job(name, path))
        ElseIf position >= 0 Then
            If jobs.Count > position Then
                jobs.Insert(position, New Job(name, path))
            Else
                jobs.Add(New Job(name, path))
            End If
        Else
            If -jobs.Count - 1 <= position Then
                jobs.Insert(jobs.Count + position + 1, New Job(name, path))
            Else
                jobs.Add(New Job(name, path))
            End If
        End If

        SaveJobs(jobs)
        g.RaiseAppEvent(ApplicationEvent.AfterJobAdded)
    End Sub

    Shared Function GetJobs() As List(Of Job)
        Dim counter As Integer = 0
        Dim formatter As New BinaryFormatter
        Dim jobsPath As String = Path.Combine(Folder.Settings, "Jobs.dat")

        If File.Exists(jobsPath) Then
            While True
                Try
                    Using stream As New FileStream(jobsPath, FileMode.Open, FileAccess.Read, FileShare.None)
                        Try
                            Return DirectCast(formatter.Deserialize(stream), List(Of Job))
                        Catch ex As Exception
                            Return New List(Of Job)
                        End Try
                    End Using

                    Exit While
                Catch ex As Exception
                    Thread.Sleep(500)
                    counter += 1

                    If counter > 9 Then
                        g.ShowException(ex, "Failed to load job file", jobsPath)
                        FileHelp.Delete(jobsPath)
                        Exit While
                    End If
                End Try
            End While
        End If

        Return New List(Of Job)
    End Function

    Shared Sub SaveJobs(jobs As List(Of Job))
        Dim counter As Integer = 0
        Dim formatter As New BinaryFormatter
        Dim jobsDir As String = Folder.Settings
        Dim jobsPath As String = Path.Combine(jobsDir, "Jobs.dat")
        Dim availableNumberOfBytes As ULong = 0
        Dim totalNumberOfBytes As ULong = 0
        Dim totalNumberOfFreeBytes As ULong = 0

        While True
            Try
                If Native.GetDiskFreeSpaceEx(jobsDir, availableNumberOfBytes, totalNumberOfBytes, totalNumberOfFreeBytes) Then
                    If availableNumberOfBytes < 600000 Then Throw New AbortException()
                End If

                Using stream As New FileStream(jobsPath, FileMode.Create, FileAccess.Write, FileShare.None)
                    formatter.Serialize(stream, jobs)
                End Using

                'otherwise exceptions, better solution not found
                Thread.Sleep(100)

                Exit While
            Catch ex As Exception
                Thread.Sleep(500)
                counter += 1

                If counter > 9 Then
                    Throw ex
                End If
            End Try
        End While
    End Sub
End Class
