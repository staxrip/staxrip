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
        Dim options = ScriptOptions.Default.WithImports("StaxRip").
            WithReferences(GetType(Scripting).Assembly)

        Dim script = CSharpScript.Create(code, options, GetType(Globals))

        Try
            Await script.RunAsync(New Globals With {.app = App})
        Catch ex As Exception
            MsgError(ex.Message)
        End Try
    End Function

    Public Class Globals
        Public app As ScriptingApp
    End Class

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
    ' the currently active project
    ReadOnly Property Project As Project
        Get
            Return p
        End Get
    End Property

    ' the currently active script
    ReadOnly Property Script As VideoScript
        Get
            Return p.Script
        End Get
    End Property

    ' the currently active video encoder
    ReadOnly Property VideoEncoder As VideoEncoder
        Get
            Return p.VideoEncoder
        End Get
    End Property

    ' show message box
    Sub Msg(text As String)
        StaxRip.Msg(text, MsgIcon.None)
    End Sub

    ' load x265 defaults
    Function Loadx265Defaults() As x265Encoder
        Dim ret = New x265Encoder
        ret.Params.ApplyPresetDefaultValues()
        ret.Params.ApplyPresetValues()
        Return DirectCast(g.MainForm.LoadVideoEncoder(ret), x265Encoder)
    End Function

    ' ensure the current encoder is x265
    Sub Ensurex265()
        If Not TypeOf p.VideoEncoder Is x265Encoder Then Loadx265Defaults()
    End Sub

    ' load VapourSynth defaults
    Sub LoadVapourSynthDefaults()
        g.MainForm.LoadScriptProfile(VideoScript.GetDefaults()(1))
    End Sub

    ' ensure the current scripting engine is VapourSynth
    Sub EnsureVapourSynth()
        If Not p.Script.Engine = ScriptingEngine.VapourSynth Then
            LoadVapourSynthDefaults()
        End If
    End Sub
End Class