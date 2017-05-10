Imports System.Management.Automation.Runspaces
Imports System.Threading.Tasks
Public Class Scripting
    Shared Sub RunCSharp(code As String)
        MsgError("C# scripting support was removed because it was very heavy requiring 47 nuget packages." + BR2 +
                 "You can port existing C# code to PowerShell or load and execute an C# Assembly with PowerShell." + BR2 +
                 "Visit the support forum.")
    End Sub

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