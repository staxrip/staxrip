
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

    Shared Function GetJobPath() As String
        Dim name = p.TargetFile.Base

        If name = "" Then
            name = Macro.Expand(p.DefaultTargetName)
        End If

        If name = "" Then
            name = p.SourceFile.Base
        End If

        Return p.TempDir + name + ".srip"
    End Function

    Shared Sub SaveJobs(jobs As List(Of Job))
        Dim formatter As New BinaryFormatter
        Dim counter As Integer

        While True
            Try
                Using stream As New FileStream(Folder.Settings + "Jobs.dat",
                    FileMode.Create, FileAccess.Write, FileShare.None)

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
    End Sub

    Shared Function GetJobs() As List(Of Job)
        Dim formatter As New BinaryFormatter
        Dim jobsPath = Folder.Settings + "Jobs.dat"
        Dim counter As Integer

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
End Class
