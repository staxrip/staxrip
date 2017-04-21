Imports System.Dynamic
Imports System.Management.Automation.Runspaces
Imports System.Threading.Tasks
Imports Microsoft.CodeAnalysis.CSharp.Scripting
Imports Microsoft.CodeAnalysis.Scripting

Public Class Scripting
    Shared Property Commands As New CommandsClass

    Shared Sub RunCSharp(code As String, Optional hideErrors As Boolean = False)
        RunCSharpAsync(code, hideErrors).Wait()
    End Sub

    Private Shared Async Function RunCSharpAsync(code As String, Optional hideErrors As Boolean = False) As Task(Of Object)
        Try
            If Not s.Storage.GetBool("c# scripting removal2") Then
                MsgWarn("C# scripting support will be removed, use PowerShell scripting instead.")
                s.Storage.SetBool("c# scripting removal2", True)
            End If

            Dim options = ScriptOptions.Default.WithImports(
            "StaxRip", "System.Linq", "System.IO", "System.Text.RegularExpressions").
            WithReferences(GetType(Scripting).Assembly,
                           GetType(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly)

            Await CSharpScript.Create(code, options).RunAsync()
        Catch ex As Exception
            If Not hideErrors Then MsgError(ex.Message)
        End Try
    End Function

    Shared Function RunPowershell(code As String) As Object
        Using runspace = RunspaceFactory.CreateRunspace()
            runspace.ApartmentState = Threading.ApartmentState.STA
            runspace.ThreadOptions = PSThreadOptions.UseCurrentThread
            runspace.Open()
            runspace.SessionStateProxy.SetVariable("commands", Commands)

            Using pipeline = runspace.CreatePipeline()
                pipeline.Commands.AddScript(
"Using namespace StaxRip;
Using namespace StaxRip.UI;
[System.Reflection.Assembly]::LoadWithPartialName(""StaxRip"")")

                pipeline.Commands.AddScript(code)

                Try
                    Dim ret = pipeline.Invoke()
                    If ret.Count > 0 Then Return ret(0)
                Catch ex As Exception
                    g.ShowException(ex)
                End Try
            End Using
        End Using
    End Function

    Public Class CommandsClass
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
End Class