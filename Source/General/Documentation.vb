
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
        GetMacros.WriteFileUTF8BOM(Path.Combine(outDir, "Macros.md"))
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
        sb.AppendLine("Macros can be used almost everywhere in StaxRip and give you the ability to to set dynamic values where needed or wanted,")
        sb.AppendLine("for example in scripts or settings. Most of them are also used for [events](Events.md) as criteria.")
        sb.AppendLine()
        sb.AppendLine("- [Global Macros](Macros.md#global-macros)")
        sb.AppendLine("- [Parameter Included Macros](Macros.md#parameter-included-macros)")
        sb.AppendLine("- [Interactive Macros](Macros.md#interactive-macros)")
        sb.AppendLine("- [Special Macros](Macros.md#special-macros)")
        sb.AppendLine("    - [Encoder Macros](Macros.md#encoder-macros)")
        sb.AppendLine("    - [While Processing Macros](Macros.md#while-processing-macros)")
        sb.AppendLine("    - [Audio Track Macros](Macros.md#audio-track-macros)")
        sb.AppendLine("- [Function Macros](Macros.md#function-macros)")
        sb.AppendLine("- [Appendix](Macros.md#appendix)")
        sb.AppendLine("    - [Appendix A: DataColumn Expression Syntax](Macros.md#appendix-a-datacolumn-expression-syntax)")
        sb.AppendLine()
        sb.AppendLine("-----")
        sb.AppendLine()
        sb.AppendLine("## Global Macros")
        sb.AppendLine()
        sb.AppendLine("| Name | Description |")
        sb.AppendLine("|---|---|")

        For Each tip In Macro.GetTips(True, False, False, False, False)
            sb.AppendLine($"| `{tip.Name}` | {tip.Value} |")
        Next

        sb.AppendLine()
        sb.AppendLine("## Parameter Included Macros")
        sb.AppendLine()
        sb.AppendLine("| Name | Description |")
        sb.AppendLine("|---|---|")

        For Each tip In Macro.GetTips(False, False, True, False, False)
            sb.AppendLine($"| `{tip.Name}` | {tip.Value} |")
        Next

        sb.AppendLine()
        sb.AppendLine("## Interactive Macros")
        sb.AppendLine("Interactive macros can be used in certain menus like the filter profiles menu. They give you the ability to pop up dialogs in which you have to put values in or select an option.")
        sb.AppendLine()
        sb.AppendLine("| Name | Description |")
        sb.AppendLine("|---|---|")

        For Each tip In Macro.GetTips(False, True, False, False, False)
            sb.AppendLine($"| `{tip.Name}` | {tip.Value} |")
        Next

        sb.AppendLine()
        sb.AppendLine("## Special Macros")

        sb.AppendLine()
        sb.AppendLine("### Encoder Macros")
        sb.AppendLine("When choosing the option to override the target file name via the encoder options, you have the possibility to extend the normal macros with encoder specific ones.  ")
        sb.AppendLine("The syntax is: `%parameter%`  ")
        sb.AppendLine("> :information_source: There are some special parameters, that might be not supported (yet). In such cases you can open a [feature request](https://github.com/staxrip/staxrip/issues/new?template=request-a-feature.md).")
        sb.AppendLine()
        sb.AppendLine("Examples:")
        sb.AppendLine()
        sb.AppendLine("| Encoder | Example | Result |")
        sb.AppendLine("|---|---|---|")
        sb.AppendLine("| x265 | `%source_name%_CRF%--crf%_AQ-Mode%--aq-mode%` | `%source_name%_CRF23_AQ-Mode2` |")
        sb.AppendLine("| SvtAv1EncApp | `%source_name%_CRF%--crf%_Predict%--pred-struct%` | `%source_name%_CRF35_Predict2` |")
        sb.AppendLine("| SvtAv1EncApp | `%source_name%_CRF%--crf%_VBoost%--enable-variance-boost%` | `%source_name%_CRF35_VBoost1` |")
        sb.AppendLine()
        sb.AppendLine("Encoder related parameters can be extended with some modifiers to modify the output of the values.")
        sb.AppendLine()
        sb.AppendLine("| Modifier | Syntax | Example | Result | Note |")
        sb.AppendLine("|---|---|---|---|---|")
        sb.AppendLine("| ` ` | `%parameter%`  | `%--preset%`  | `VeryFast` | No modifier, normal value without spaces |")
        sb.AppendLine("| `D` | `%parameter_D%` | `%--preset_D%` | `True`     | `True` if the default value is set, otherwise `False` |")
        sb.AppendLine("| `L` | `%parameter_L%` | `%--preset_L%` | `veryfast` | Value in lowercase without spaces |")
        sb.AppendLine("| `T` | `%parameter_T%` | `%--preset_T%` | `Veryfast` | Value in Title-case (Only first letter in uppercase) without spaces |")
        sb.AppendLine("| `U` | `%parameter_U%` | `%--preset_U%` | `VERYFAST` | Value in uppercase without spaces |")
        sb.AppendLine("| `V` | `%parameter_V%` | `%--preset_V%` | `2`        | Returns the numeric value of the option list,<br>`1` for checked checkboxes, otherwise `0`<br>[Only available for checkboxes and option lists] |")
        sb.AppendLine("| `Z` | `%parameter_Z%` | `%--preset_Z%` | `0`        | `1` in case it is active/visible, otherwise `0` |")
        sb.AppendLine()
        sb.AppendLine("> :warning: All modifier letters are **case sensitive**!")

        sb.AppendLine()
        sb.AppendLine("### While Processing Macros")
        sb.AppendLine("These macros are only available while a process is running. They can be used for the `While Processing` event, for example to call an API.")
        sb.AppendLine()
        sb.AppendLine("| Name | Description |")
        sb.AppendLine("|---|---|")

        For Each tip In Macro.GetTips(False, False, False, True, False)
            sb.AppendLine($"| `{tip.Name}` | {tip.Value} |")
        Next

        sb.AppendLine()
        sb.AppendLine("### Audio Track Macros")
        sb.AppendLine("These macros are only available in audio processing, for example inside the Audio Options.")
        sb.AppendLine()
        sb.AppendLine("| Name | Description |")
        sb.AppendLine("|---|---|")

        For Each tip In Macro.GetTips(False, False, False, False, True)
            sb.AppendLine($"| `{tip.Name}` | {tip.Value} |")
        Next

        sb.AppendLine()
        sb.AppendLine("## Function Macros")
        sb.AppendLine("Function macros are similar to *normal* macros, but they don't just replace a text with a specific value; they can perform an action like a condition.")
        sb.AppendLine("This makes it possible to return different strings depending on the expression, especially when cascaded/nested. The following table and the examples below should give you enough explanation and ideas.")
        sb.AppendLine()
        sb.AppendLine("- `<lower(<input>)>`")
        sb.AppendLine("  - **\<input\>** is any text you want to have in lower case")
        sb.AppendLine("  - Examples:")
        sb.AppendLine("    - `<lower(StaxRip is COOL)>` => `staxrip is cool`")
        sb.AppendLine("- `<if(<expression>;<true>;<false>)>`")
        sb.AppendLine("  - **\<expression\>** corresponds to the [*Expression Syntax*](Macros.md#appendix-a-datacolumn-expression-syntax)")
        sb.AppendLine("    - Can also include another (nested) function")
        sb.AppendLine("  - **\<true\>** or **\<false\>** is returned depending on whether the expression is true or false")
        sb.AppendLine("    - Can be another (nested) function, a macro or any text")
        sb.AppendLine("    - First whitespace after `;` will be ignored")
        sb.AppendLine("  - Examples with `--preset VeryFast`:")
        sb.AppendLine("    - `<if('%--preset%' = 'Medium'; Default; %--preset%)>`")
        sb.AppendLine("    - `<if('%--preset_V%' = 2; Default; %--preset%)>`")
        sb.AppendLine("    - `<if('%--preset_V%' <> 0; %--preset%; 'PresetIs0')>`")
        sb.AppendLine("    - `<if('%--preset_V%' IN (0,1,2); Faster than Faster; Faster or slower)>`")
        sb.AppendLine("    - `<if('%--preset%' LIKE '*Fast*'; Includes 'Fast'; SomethingElse)>`")
        sb.AppendLine("    - `<if(%target_height% > 1080; 4K; <if(%target_height% > 720; FullHD; SD)>)>`")
        sb.AppendLine("    - `<if('%source_mi_g:Title%' <> ''; Original Title: %source_mi_g:Title%; <if('%source_mi_g:Movie%' <> ''; Original Title: %source_mi_g:Movie%; '')>)>`")
        sb.AppendLine("    - `<if(%--sao%; SAO; NoSAO)>`")
        sb.AppendLine("    - `<if(%--sao% AND NOT %--limit-sao%; FullSAO; <if(%--sao% AND %--limit-sao%; RestrictedSAO; NoSAO)>)>`")
        sb.AppendLine("    - `<if('%source_mi_v:Framerate%' >= 48; HiFPS; '')>`")
        sb.AppendLine("    - `<if(CONVERT(%source_mi_v:FrameRate%, 'System.Decimal') >= 48; HighFPS; '')>`")
        sb.AppendLine("- `<replace(<input>;<oldText>;<newText>)>`")
        sb.AppendLine("  - **\<input\>** is any text you want to replace parts of")
        sb.AppendLine("  - **\<oldText\>** is the text or macro that shall be replaced")
        sb.AppendLine("    - First whitespace after `;` will be ignored")
        sb.AppendLine("  - **\<newText\>** is the text or macro that shall be used for the replacement")
        sb.AppendLine("    - First whitespace after `;` will be ignored")
        sb.AppendLine("  - Examples:")
        sb.AppendLine("    - `<replace(This is any text you like; you; I)>`")
        sb.AppendLine("    - `<replace(%source_name%; 1080p; 720p)>`")
        sb.AppendLine("- `<title(<input>)>`")
        sb.AppendLine("  - **\<input\>** is any text you want to have in title case")
        sb.AppendLine("  - Examples:")
        sb.AppendLine("    - `<title(StaxRip is COOL)>` => `Staxrip Is Cool`")
        sb.AppendLine("- `<upper(<input>)>`")
        sb.AppendLine("  - **\<input\>** is any text you want to have in upper case")
        sb.AppendLine("  - Examples:")
        sb.AppendLine("    - `<upper(StaxRip is COOL)>` => `STAXRIP IS COOL`")
        sb.AppendLine()

        sb.AppendLine()
        sb.AppendLine("## Appendix")

        sb.AppendLine()
        sb.AppendLine("### Appendix A: DataColumn Expression Syntax")
        sb.AppendLine("Generally the whole *ADO.NET DataColumn Expression Syntax* is supported. Here are the most important operands and functions:")
        sb.AppendLine("- `+`, `-`, `*`, `/`, `%`")
        sb.AppendLine("- `<`, `<=`, `<>`, `=`, `>=`, `>`, `IN`, `LIKE`")
        sb.AppendLine("- `AND`, `OR`, `NOT`")
        sb.AppendLine("- `CONVERT(expression, .NET Data Type)`, `LEN(expression)`, `ISNULL(expression, replacementvalue)`, `SUBSTRING(expression, start, length)`, `TRIM(expression)`")
        sb.AppendLine("- For more information: https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-data-datacolumn-expression")

        Return sb.ToString()
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
