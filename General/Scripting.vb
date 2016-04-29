Imports System.Dynamic
Imports System.Management.Automation.Runspaces
Imports System.Threading.Tasks
Imports Microsoft.CodeAnalysis.CSharp.Scripting
Imports Microsoft.CodeAnalysis.Scripting

Public Class Scripting
    Shared Property App As New ScriptingApp

    Shared Sub RunCSharp(code As String)
        Dim t = RunCSharpAsync(code)
        t.Wait()
    End Sub

    Private Shared Async Function RunCSharpAsync(code As String) As Task(Of Object)
        Dim options = ScriptOptions.Default.WithImports(
            "StaxRip", "System.Linq", "System.IO", "System.Text.RegularExpressions").
            WithReferences(GetType(Scripting).Assembly)

        Dim script = CSharpScript.Create(code, options)

        Try
            Await script.RunAsync()
        Catch ex As Exception
            MsgError(ex.Message)
        End Try
    End Function

    Shared Sub RunPowershell(code As String)
        Using runspace = RunspaceFactory.CreateRunspace()
            runspace.Open()
            runspace.SessionStateProxy.SetVariable("app", App)

            Using pipeline = runspace.CreatePipeline()
                pipeline.Commands.AddScript(code)

                Try
                    pipeline.Invoke()
                Catch ex As Exception
                    MsgError(ex.Message)
                End Try
            End Using
        End Using
    End Sub
End Class

Public Class ScriptingApp
    Inherits DynamicObject

    Public Overrides Function TryInvokeMember(
        binder As InvokeMemberBinder,
        args() As Object, ByRef result As Object) As Boolean

        Try
            g.MainForm.CommandManager.Process(binder.Name, args)
            Return True
        Catch ex As Exception
            g.ShowException(ex)
            result = Nothing
            Return False
        End Try
    End Function
End Class