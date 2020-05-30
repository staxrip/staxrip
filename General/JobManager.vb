
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
                    FileMode.Create, FileAccess.ReadWrite, FileShare.None)

                    formatter.Serialize(stream, jobs)
                End Using

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

    Shared Sub RemoveJob(jobPath As String)
        Dim jobs = GetJobs()

        For Each i In jobs.ToArray
            If i.Path = jobPath Then
                jobs.Remove(i)
                SaveJobs(jobs)
            End If
        Next
    End Sub

    Shared Sub ActivateJob(jobPath As String, Optional isActive As Boolean = True)
        Dim jobs = GetJobs()

        For Each i In jobs
            If i.Path = jobPath Then
                i.Active = isActive
            End If
        Next

        SaveJobs(jobs)
    End Sub

    Shared Sub AddJob(name As String, path As String)
        Dim jobs = GetJobs()

        For Each job In jobs.ToArray
            If job.Path = path Then
                jobs.Remove(job)
            End If
        Next

        jobs.Add(New Job(name, path))
        SaveJobs(jobs)
    End Sub

    Shared Function GetJobs() As List(Of Job)
        Dim formatter As New BinaryFormatter
        Dim jobsPath = Folder.Settings + "Jobs.dat"
        Dim counter As Integer

        If File.Exists(jobsPath) Then
            While True
                Try
                    Using stream As New FileStream(
                        jobsPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)

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
