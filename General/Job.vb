
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Threading

Public Class Job
    Shared ReadOnly Property ActiveJobs As IEnumerable(Of StringBooleanPair)
        Get
            Return From i In GetJobs() Where i.Value = True
        End Get
    End Property

    Shared Sub SaveJobs(jobs As List(Of StringBooleanPair))
        Dim formatter As New BinaryFormatter
        Dim counter As Integer

        While True
            Try
                Using stream As New FileStream(Folder.Settings + "Jobs.dat",
                                               FileMode.Create,
                                               FileAccess.ReadWrite,
                                               FileShare.None)

                    formatter.Serialize(stream, jobs)
                End Using

                Exit While
            Catch ex As Exception
                Thread.Sleep(500)
                counter += 1
                If counter > 9 Then Throw ex
            End Try
        End While
    End Sub

    Shared Sub RemoveJob(jobPath As String)
        Dim jobs = GetJobs()

        For Each i In jobs.ToArray
            If i.Key = jobPath Then
                jobs.Remove(i)
                SaveJobs(jobs)
            End If
        Next
    End Sub

    Shared Sub ActivateJob(jobPath As String, Optional isActive As Boolean = True)
        Dim jobs = GetJobs()

        For Each i In jobs
            If i.Key = jobPath Then i.Value = isActive
        Next

        SaveJobs(jobs)
    End Sub

    Shared Sub AddJob(jobPath As String, Optional isActive As Boolean = True)
        Dim jobs = GetJobs()

        For Each i In jobs.ToArray
            If i.Key = jobPath Then jobs.Remove(i)
        Next

        jobs.Add(New StringBooleanPair(jobPath, isActive))
        SaveJobs(jobs)
    End Sub

    Shared Function GetJobs() As List(Of StringBooleanPair)
        Dim formatter As New BinaryFormatter
        Dim jobsPath = Folder.Settings + "Jobs.dat"
        Dim counter As Integer

        If File.Exists(jobsPath) Then
            While True
                Try
                    Using stream As New FileStream(jobsPath,
                                                   FileMode.Open,
                                                   FileAccess.ReadWrite,
                                                   FileShare.None)

                        Return DirectCast(formatter.Deserialize(stream), List(Of StringBooleanPair))
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

        Return New List(Of StringBooleanPair)
    End Function
End Class