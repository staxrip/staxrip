
Imports System.ComponentModel
Imports System.Reflection
Imports System.Text

Imports StaxRip.UI

Public Class Documentation
    Shared Sub GenerateWikiContent()
        Dim outDir = Path.Combine(Folder.Settings, "Wiki Pages")
        FolderHelp.Create(outDir)
        GetCommands(True).WriteFileUTF8(Path.Combine(outDir, "Command-Line-Interface.md"))
        GetCommands(False).WriteFileUTF8(Path.Combine(outDir, "Commands.md"))
        GetMacros.WriteFileUTF8(Path.Combine(outDir, "Macros.md"))
        GetTools.WriteFileUTF8(Path.Combine(outDir, "Tools.md"))
        g.ShellExecute(outDir)
    End Sub

    Shared Function GetCommands(cli As Boolean) As String
        Dim sb As New StringBuilder

        Dim commands As New List(Of Command)(g.MainForm.CommandManager.Commands.Values)
        commands.Sort()

        For Each command In commands
            Dim params = command.MethodInfo.GetParameters
            Dim title = command.MethodInfo.Name

            If cli Then
                title = "-" + title + ":"

                For Each param In params
                    title += param.Name + ","
                Next

                title = title.TrimEnd(",:".ToCharArray)
            End If

            sb.Append("### " + title + BR2)
            sb.Append(command.Attribute.Description + BR2)

            If params.Length > 0 Then
                Dim needsSecondColumn = False

                For Each param In params
                    Dim descAttrib = param.GetCustomAttribute(Of DescriptionAttribute)

                    If (descAttrib IsNot Nothing AndAlso descAttrib.Description <> "") OrElse
                        param.ParameterType.IsEnum Then

                        needsSecondColumn = True
                    End If
                Next

                If needsSecondColumn Then
                    sb.AppendLine("| Parameter | Description |")
                    sb.AppendLine("| --- | --- |")
                Else
                    sb.AppendLine("| Parameter |")
                    sb.AppendLine("| --- |")
                End If

                For Each param In params
                    If needsSecondColumn Then
                        Dim name = param.Name
                        Dim nameAttrib = param.GetCustomAttribute(Of DispNameAttribute)

                        If nameAttrib IsNot Nothing Then
                            name = nameAttrib.DisplayName
                        End If

                        name += $" \<{param.ParameterType.Name.ToLowerInvariant.Replace("int32", "integer")}\>"

                        sb.Append($"| {name} |")

                        If param.ParameterType.IsEnum Then
                            sb.AppendLine(" " + System.Enum.GetNames(param.ParameterType).Join(", ") + " |")
                        Else
                            Dim descAttrib = param.GetCustomAttribute(Of DescriptionAttribute)

                            If descAttrib IsNot Nothing Then
                                sb.AppendLine($" {descAttrib.Description} |")
                                descAttrib.Description.ThrowIfContainsNewLine
                            Else
                                sb.AppendLine($" |")
                            End If
                        End If
                    Else
                        Dim name = param.Name
                        Dim nameAttrib = param.GetCustomAttribute(Of DispNameAttribute)

                        If nameAttrib IsNot Nothing Then
                            name = nameAttrib.DisplayName
                        End If

                        name += $" \<{param.ParameterType.Name.ToLowerInvariant.Replace("int32", "integer")}\>"

                        sb.AppendLine($"| {name} |")
                    End If
                Next

                sb.Append(BR)
            End If
        Next

        Return sb.ToString
    End Function

    Shared Function GetMacros() As String
        Dim sb As New StringBuilder()
        sb.AppendLine("# [Documentation](../README.md) / [Usage](README.md) / Macros")
        sb.AppendLine()
        sb.AppendLine("## Global Macros")
        sb.AppendLine("| Name | Description |")
        sb.AppendLine("|---|---|")

        For Each tip In Macro.GetTips(True, False, False, False)
            sb.AppendLine($"| `{tip.Name}` | {tip.Value} |")
        Next

        sb.AppendLine()
        sb.AppendLine("## Parameter Included Macros")
        sb.AppendLine("| Name | Description |")
        sb.AppendLine("|---|---|")

        For Each tip In Macro.GetTips(False, False, True, False)
            sb.AppendLine($"| `{tip.Name}` | {tip.Value} |")
        Next

        sb.AppendLine()
        sb.AppendLine("## Interactive Macros")
        sb.AppendLine("Interactive macros can be used in certain menus like the filter profiles menu. They give you the ability to pop up dialogs in which you have to put values in or select an option.")
        sb.AppendLine("| Name | Description |")
        sb.AppendLine("|---|---|")

        For Each tip In Macro.GetTips(False, True, False, False)
            sb.AppendLine($"| `{tip.Name}` | {tip.Value} |")
        Next

        sb.AppendLine()
        sb.AppendLine("## Special Macros")

        sb.AppendLine()
        sb.AppendLine("### Encoder Macros")
        sb.AppendLine("When choosing the option to override the target file name via the encoder options, you have the possibility to extend the normal macros with encoder specific ones.  ")
        sb.AppendLine("The syntax is: `%parameter%`  ")
        sb.AppendLine("**Note**: There are some special parameters, that might be not supported (yet). In such cases you can open a feature request.")
        sb.AppendLine()
        sb.AppendLine("Examples:")
        sb.AppendLine("| Encoder | Example | Result |")
        sb.AppendLine("|---|---|---|")
        sb.AppendLine("| x265 | `%source_name%_CRF%--crf%_AQ-Mode%--aq-mode%` | `%source_name%_CRF23_AQ-Mode2` |")
        sb.AppendLine("| SvtAv1EncApp | `%source_name%_CRF%--crf%_Predict%--pred-struct%` | `%source_name%_CRF35_Predict2` |")
        sb.AppendLine("| SvtAv1EncApp | `%source_name%_CRF%--crf%_VBoost%--enable-variance-boost%` | `%source_name%_CRF35_VBoost1` |")

        sb.AppendLine()
        sb.AppendLine("### While Processing Macros")
        sb.AppendLine("These macros are only available while a process is running. They can be used for the `While Processing` event, for example to call an API.")
        sb.AppendLine("| Name | Description |")
        sb.AppendLine("|---|---|")

        For Each tip In Macro.GetTips(False, False, False, True)
            sb.AppendLine($"| `{tip.Name}` | {tip.Value} |")
        Next

        Return sb.ToString
    End Function

    Shared Function GetTools() As String
        Dim sb As New StringBuilder()

        sb.Append("This page is based on the StaxRip version " + Application.ProductVersion + BR2)
        sb.Append("[Console App](Tools#console-app)" + BR2)
        sb.Append("[GUI App](Tools#gui-app)" + BR2)
        sb.Append("[AviSynth Plugin](Tools#avisynth-plugin)" + BR2)
        sb.Append("[AviSynth Script](Tools#avisynth-script)" + BR2)
        sb.Append("[VapourSynth Plugin](Tools#vapoursynth-plugin)" + BR2)
        sb.Append("[VapourSynth Script](Tools#vapoursynth-script)" + BR2)
        sb.Append("| Name | Type | Filename | Version | Modified Date |" + BR +
                  "|------|------|----------|---------|---------------|" + BR)

        For Each pack In Package.Items.Values.OrderBy(Function(i) i.GetTypeName)
            sb.Append("| " + pack.Name + " | " + pack.GetTypeName + " | " + pack.Filename + " | ")

            If pack.IsVersionCorrect Then
                sb.Append(pack.Version)
            End If

            sb.Append(" | ")

            If pack.Path.FileExists Then
                sb.Append(File.GetLastWriteTime(pack.Path).ToString("yyyy-MM-dd"))
            End If

            sb.AppendLine(" |")
        Next

        sb.Append("## Console App" + BR2)

        For Each pack In Package.Items.Values
            If pack.GetTypeName = "Console App" Then
                sb.Append("### " + pack.Name + BR2)

                If pack.Description <> "" Then
                    sb.Append(pack.Description + BR2)
                End If

                If pack.URL <> "" Then
                    sb.Append(pack.URL + BR3)
                End If
            End If
        Next

        sb.Append("## GUI App" + BR2)

        For Each pack In Package.Items.Values
            If pack.GetTypeName = "GUI App" Then
                sb.Append("### " + pack.Name + BR2)

                If pack.Description <> "" Then
                    sb.Append(pack.Description + BR2)
                End If

                If pack.URL <> "" Then
                    sb.Append(pack.URL + BR3)
                End If
            End If
        Next

        sb.Append("## AviSynth Plugin" + BR2)

        For Each pack In Package.Items.Values
            If pack.GetTypeName = "AviSynth Plugin" Then
                sb.Append("### " + pack.ID + BR2)

                If pack.Description <> "" Then
                    sb.Append(pack.Description + BR2)
                End If

                Dim plugin = DirectCast(pack, PluginPackage)

                If Not plugin.AvsFilterNames.NothingOrEmpty Then
                    sb.Append("Filters: " + plugin.AvsFilterNames.Join(", ") + BR2)
                End If

                If pack.URL <> "" Then
                    sb.Append(pack.URL + BR3)
                End If
            End If
        Next

        sb.Append("## AviSynth Script" + BR2)

        For Each pack In Package.Items.Values
            If pack.GetTypeName = "AviSynth Script" Then
                sb.Append("### " + pack.Name + BR2)

                If pack.Description <> "" Then
                    sb.Append(pack.Description + BR2)
                End If

                Dim plugin = DirectCast(pack, PluginPackage)

                If Not plugin.AvsFilterNames.NothingOrEmpty Then
                    sb.Append("Filters: " + plugin.AvsFilterNames.Join(", ") + BR2)
                End If

                If pack.URL <> "" Then
                    sb.Append(pack.URL + BR3)
                End If
            End If
        Next

        sb.Append("## VapourSynth Plugin" + BR2)

        For Each pack In Package.Items.Values
            If pack.GetTypeName = "VapourSynth Plugin" Then
                sb.Append("### " + pack.ID + BR2)

                If pack.Description <> "" Then
                    sb.Append(pack.Description + BR2)
                End If

                Dim plugin = DirectCast(pack, PluginPackage)

                If Not plugin.VsFilterNames.NothingOrEmpty Then
                    sb.Append("Filters: " + plugin.VsFilterNames.Join(", ") + BR2)
                End If

                If pack.URL <> "" Then
                    sb.Append(pack.URL + BR3)
                End If
            End If
        Next

        sb.Append("## VapourSynth Script" + BR2)

        For Each pack In Package.Items.Values
            If pack.GetTypeName = "VapourSynth Script" Then
                sb.Append("### " + pack.Name + BR2)

                If pack.Description <> "" Then
                    sb.Append(pack.Description + BR2)
                End If

                Dim plugin = DirectCast(pack, PluginPackage)

                If Not plugin.VsFilterNames.NothingOrEmpty Then
                    sb.Append("Filters: " + plugin.VsFilterNames.Join(", ") + BR2)
                End If

                If pack.URL <> "" Then
                    sb.Append(pack.URL + BR3)
                End If
            End If
        Next

        Return sb.ToString()
    End Function

    Shared Sub ShowTip(message As String)
        Dim hash = message.MD5Hash

        If Not s.Storage.GetBool(hash) Then
            MsgInfo("Tip", message)
            s.Storage.SetBool(hash, True)
        End If
    End Sub
End Class
