Imports System.Management.Automation.Runspaces
Imports System.Threading.Tasks
Imports Microsoft.CodeAnalysis.CSharp.Scripting
Imports Microsoft.CodeAnalysis.Scripting

Public Class Scripting
    Shared Sub RunCSharp(code As String)
        RunCSharpAsync(code).Wait()
    End Sub

    Private Shared Async Function RunCSharpAsync(code As String) As Task(Of Object)
        Try
            MsgWarn("C# scripting support will be removed, use PowerShell scripting instead.")
            Dim options = ScriptOptions.Default.WithImports(
            "StaxRip", "System.Linq", "System.IO", "System.Text.RegularExpressions").
            WithReferences(GetType(Scripting).Assembly,
                           GetType(Microsoft.CSharp.RuntimeBinder.CSharpArgumentInfo).Assembly)
            Await CSharpScript.Create(code, options).RunAsync()
        Catch ex As Exception
            g.ShowException(ex)
        End Try
    End Function

    Shared Function RunPowershell(code As String) As Object
        Using runspace = RunspaceFactory.CreateRunspace()
            runspace.ApartmentState = Threading.ApartmentState.STA
            runspace.ThreadOptions = PSThreadOptions.UseCurrentThread
            runspace.Open()

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
End Class