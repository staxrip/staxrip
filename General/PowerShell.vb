
Imports System.Collections.ObjectModel
Imports System.Management.Automation
Imports System.Management.Automation.Runspaces

Public Class PowerShell
    Shared References As New List(Of Object)
    Shared Property InitCode As String

    Shared Function Invoke(code As String) As Object
        Invoke(code, Nothing, Nothing)
    End Function

    Shared Function Invoke(code As String, varName As String, varValue As Object) As Collection(Of PSObject)
        Dim runspace = RunspaceFactory.CreateRunspace()
        runspace.ApartmentState = Threading.ApartmentState.STA
        runspace.Open()

        Dim pipeline = runspace.CreatePipeline()
        Dim cmd = New Runspaces.Command("Set-ExecutionPolicy")
        cmd.Parameters.Add("ExecutionPolicy", "Unrestricted")
        cmd.Parameters.Add("Scope", "Process")
        pipeline.Commands.Add(cmd)

        If InitCode <> "" Then
            pipeline.Commands.AddScript(InitCode)
        End If

        pipeline.Commands.AddScript(code)

        If varName <> "" Then
            runspace.SessionStateProxy.SetVariable(varName, varValue)
        End If

        If code.Contains("Register-ObjectEvent") Then
            References.Add(runspace)
            References.Add(pipeline)
            Return pipeline.Invoke()
        Else
            Dim ret = pipeline.Invoke()
            pipeline.Dispose()
            runspace.Dispose()
            Return ret
        End If
    End Function

    Shared Function InvokeAndConvert(code As String) As String
        Return InvokeAndConvert(code, Nothing, Nothing)
    End Function

    Shared Function InvokeAndConvert(code As String, varName As String, varValue As Object) As String
        Dim objects = Invoke(code, varName, varValue)

        If objects Is Nothing Then
            Return ""
        End If

        Dim lines As New List(Of String)

        For Each obj In objects
            If Not obj Is Nothing Then
                lines.Add(obj.ToString)
            End If
        Next

        Return String.Join(Environment.NewLine, lines)
    End Function
End Class
