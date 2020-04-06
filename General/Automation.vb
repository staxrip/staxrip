
Imports System.Management.Automation.Runspaces

Imports StaxRip.UI

Public Class Scripting
    Shared Function RunPowershell(code As String) As Object
        Try
            Using runspace = RunspaceFactory.CreateRunspace()
                runspace.ApartmentState = Threading.ApartmentState.STA
                runspace.ThreadOptions = PSThreadOptions.UseCurrentThread
                runspace.Open()

                Using pipeline = runspace.CreatePipeline()
                    pipeline.Commands.AddScript(
                        "Using namespace StaxRip;" + BR +
                        "Using namespace StaxRip.UI;" + BR +
                        "Using namespace System;" + BR +
                        "Using namespace System.Windows.Forms;" + BR +
                        "[Reflection.Assembly]::LoadWithPartialName(""StaxRip"")")

                    pipeline.Commands.AddScript(code)

                    Try
                        Return pipeline.Invoke()
                    Catch ex As Exception
                        Try
                            Using pipeline2 = runspace.CreatePipeline()
                                pipeline2.Commands.AddScript("$PSVersionTable.PSVersion.Major * 10 + $PSVersionTable.PSVersion.Minor")

                                If pipeline2.Invoke()(0).ToString.ToInt < 51 Then
                                    Throw New Exception()
                                End If
                            End Using
                        Catch
                            MsgError("PowerShell Setup Problem", "Ensure you have at least PowerShell 5.1 installed.")
                            Exit Function
                        End Try

                        g.ShowException(ex)
                    End Try
                End Using
            End Using
        Catch ex As Exception
            g.ShowException(ex, "Failed to execute PowerShell script", "Install PowerShell 5.1 or higher.")
        End Try
    End Function
End Class

Public Enum ApplicationEvent
    <DispName("After Project Loaded")> ProjectLoaded
    <DispName("After Job Processed")> JobProcessed
    <DispName("After Video Encoded")> VideoEncoded
    <DispName("Before Job Processed")> BeforeJobProcessed
    <DispName("After Source Loaded")> AfterSourceLoaded
    <DispName("Application Exit")> ApplicationExit
    <DispName("After Project Or Source Loaded")> ProjectOrSourceLoaded
    <DispName("After Jobs Processed")> JobsProcessed
    <DispName("After Job Muxed")> JobMuxed
End Enum
