
Imports System.Collections.ObjectModel
Imports System.Management.Automation
Imports System.Management.Automation.Runspaces

Public Class PowerShell
    Shared References As New List(Of Object)
    Shared Property InitCode As String

    Shared Function Invoke(
        code As String,
        Optional variableName As String = Nothing,
        Optional variableValue As Object = Nothing,
        Optional args As String() = Nothing) As Collection(Of PSObject)

        Dim runspace = RunspaceFactory.CreateRunspace()
        runspace.ApartmentState = Threading.ApartmentState.STA
        runspace.ThreadOptions = PSThreadOptions.UseCurrentThread
        runspace.Open()

        Dim pipeline = runspace.CreatePipeline()
        Dim policyCommand = New Runspaces.Command("Set-ExecutionPolicy")
        policyCommand.Parameters.Add("ExecutionPolicy", "Unrestricted")
        policyCommand.Parameters.Add("Scope", "Process")
        pipeline.Commands.Add(policyCommand)

        If InitCode <> "" Then
            pipeline.Commands.AddScript(InitCode)
        End If

        pipeline.Commands.AddScript(code)

        If Not args.NothingOrEmpty Then
            Dim scriptCommand = pipeline.Commands(pipeline.Commands.Count - 1)

            For Each arg In args
                scriptCommand.Parameters.Add(Nothing, arg)
            Next
        End If

        If variableName <> "" Then
            runspace.SessionStateProxy.SetVariable(variableName, variableValue)
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
