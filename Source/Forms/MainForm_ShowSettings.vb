Imports StaxRip.UI

Partial Public Class MainForm
    Inherits FormBase

    Function GetRestartID() As String
        Return s.AviSynthMode & s.VapourSynthMode
    End Function

    <Command("Shows the settings dialog.")>
    Sub ShowSettingsDialog()
        Dim restartID = GetRestartID()
        Dim oldDefaultFont = FontManager.GetDefaultFont()

        Using form As New SimpleSettingsForm("Settings")
            Dim ui = form.SimpleUI
            ui.Store = s


            '################# General
            ui.CreateFlowPage("General", True)

            Dim b = ui.AddBool
            b.Text = "Save projects automatically"
            b.Field = NameOf(s.AutoSaveProject)

            b = ui.AddBool
            b.Text = "In addition save video encoder profiles separately"
            b.Field = NameOf(s.SaveVideoEncoderProfilesSeparately)

            b = ui.AddBool
            b.Text = "In addition save audio profiles separately"
            b.Field = NameOf(s.SaveAudioProfilesSeparately)

            b = ui.AddBool
            b.Text = "In addition save events separately"
            b.Field = NameOf(s.SaveEventsSeparately)

            b = ui.AddBool()
            b.Text = "Reverse mouse wheel video seek direction"
            b.Field = NameOf(s.ReverseVideoScrollDirection)

            Dim n = ui.AddNum()
            n.Text = "Number of most recently used projects to keep"
            n.Help = "MRU list shown in the main menu under: File > Recent Projects"
            n.Config = {0, 25}
            n.Field = NameOf(s.ProjectsMruNum)

            n = ui.AddNum()
            n.Text = "Maximum number of parallel processes"
            n.Help = "Maximum number of parallel processes used for audio and video processing. Chunk encoding can be enabled in the encoder options."
            n.Config = {1, 16}
            n.Field = NameOf(s.ParallelProcsNum)

            n = ui.AddNum()
            n.Text = "Timeout error messages on job processing"
            n.Help = "Duration of error messages being shown if an error occures and before the next job shall start."
            n.Config = {-1, 3600, 5, 0}
            n.Field = NameOf(s.ErrorMessageTimeout)


            '################# Logs
            ui.CreateFlowPage("Logs", True)

            n = ui.AddNum()
            n.Text = "Number of log files to keep"
            n.Help = "Log files can be found at: Tools > Folders > Log Files"
            n.Config = {1, Integer.MaxValue}
            n.Field = NameOf(s.LogFileNum)

            b = ui.AddBool()
            b.Text = "Write Event Commands to log file"
            b.Field = NameOf(s.LogEventCommand)

            b = ui.AddBool()
            b.Text = "Enable debug logging"
            b.Field = NameOf(s.WriteDebugLog)


            '################# Startup
            ui.CreateFlowPage("Startup", True)

            Dim mb = ui.AddMenu(Of String)()
            mb.Text = "Startup Template:"
            mb.Help = "Template loaded when StaxRip starts."
            mb.Field = NameOf(s.StartupTemplate)
            mb.Expanded = True
            Dim templates = Directory.GetFiles(Folder.Template, "*.srip", SearchOption.AllDirectories).OrderBy(Function(x) x.Count(Function(c) c = Path.DirectorySeparatorChar)).ThenBy(Function(x) x)
            For Each template In templates
                Dim text = template.Replace(Folder.Template, "").Trim(Path.DirectorySeparatorChar).Replace(Path.DirectorySeparatorChar, " | ")
                text = text.Substring(0, text.LastIndexOf(".srip"))
                mb.Add(text)
            Next
            mb.Button.SaveAction = Sub(value)
                                       If value <> s.StartupTemplate AndAlso p.SourceFile = "" Then
                                           Dim templatePath = Path.Combine(Folder.Template, value.Replace(" | ", Path.DirectorySeparatorChar) + ".srip")
                                           If templatePath.FileExists() Then LoadProject(templatePath)
                                       End If
                                       UpdateTemplatesMenuAsync()
                                   End Sub
            mb.Button.ShowPath = True

            b = ui.AddBool
            b.Text = "Start without focus"
            b.Field = NameOf(s.StartWithoutFocus)

            b = ui.AddBool
            b.Text = "Check for Long Path Support"
            b.Field = NameOf(s.CheckForLongPathSupport)

            b = ui.AddBool()
            b.Text = "Check for updates approx. once per day"
            b.Field = NameOf(s.CheckForUpdates)


            '################# Source Opening
            ui.CreateFlowPage("Source Opening", True)

            ui.AddLabel("Template Selection:")

            Dim stsm = ui.AddMenu(Of ShowTemplateSelectionMode)()
            Dim stsd = ui.AddMenu(Of ShowTemplateSelectionDefaultMode)()
            Dim stst = ui.AddNum()

            stsm.Text = "Show when loading via"
            stsm.Field = NameOf(s.ShowTemplateSelection)
            stsm.Expanded = True
            stsm.Button.ValueChangedAction = Sub(value)
                                                 Dim enabled = value <> ShowTemplateSelectionMode.Never
                                                 stsd.Enabled = enabled
                                                 stst.Enabled = enabled
                                             End Sub
            stsm.Button.ValueChangedAction.Invoke(s.ShowTemplateSelection)

            stsd.Text = "Default timeout action"
            stsd.Field = NameOf(s.ShowTemplateSelectionDefault)
            stsd.Expanded = True
            stsd.Button.ValueChangedAction = Sub(value)
                                                 stst.Enabled = value <> ShowTemplateSelectionDefaultMode.None
                                             End Sub
            stsd.Button.ValueChangedAction.Invoke(s.ShowTemplateSelectionDefault)

            stst.Text = "Timeout"
            stst.Help = "Timeout in seconds the Template Selection is shown befor the current template is used"
            stst.Config = {1, Integer.MaxValue}
            stst.Field = NameOf(s.ShowTemplateSelectionTimeout)


            '############# Quality Definitions
            Dim qualityDefinitionsPage = ui.CreateFlowPage("Quality Definitions", True)

            Dim t = ui.AddText()
            t.Text = "x264 quality definitions"
            t.Help = "Create custom quality definitions for x264." + BR2 +
                         "Use this format to create your custom values with optional description:" + BR +
                         "number""text""" + BR +
                         "number: can be used with optional decimal separator (, or .)" + BR +
                         "text: description, optionally empty" + BR2 +
                         "Example:" + BR +
                         "8""Crazy""_19.5""Personal Default"" 21,5""Why not?!"" 22.0 44,3"
            t.Edit.Expand = True
            t.Edit.Text = s.X264QualityDefinitions.ToSeparatedString()
            t.Edit.SaveAction = Sub(value) s.X264QualityDefinitions = value.ToX264QualityItems()?.ToList()

            t = ui.AddText()
            t.Text = "x265 quality definitions"
            t.Help = "Create custom quality definitions for x265." + BR2 +
                         "Use this format to create your custom values with optional description:" + BR +
                         "number""text""" + BR +
                         "number: can be used with optional decimal separator (, or .)" + BR +
                         "text: description, optionally empty" + BR2 +
                         "Example:" + BR +
                         "8""Crazy""_19.5""Personal Default"" 21,5""Why not?!"" 22.0 44,3"
            t.Edit.Expand = True
            t.Edit.Text = s.X265QualityDefinitions.ToSeparatedString()
            t.Edit.SaveAction = Sub(value) s.X265QualityDefinitions = value.ToX265QualityItems()?.ToList()

            t = ui.AddText()
            t.Text = "SvtAv1EncApp quality definitions"
            t.Help = "Create custom quality definitions for SvtAv1EncApp." + BR2 +
                         "Use this format to create your custom values with optional description:" + BR +
                         "number""text""" + BR +
                         "number: can be used with optional decimal separator (, or .)" + BR +
                         "text: description, optionally empty" + BR2 +
                         "Example:" + BR +
                         "8""Crazy""_19.5""Personal Default"" 21,5""Why not?!"" 22.0 44,3"
            t.Edit.Expand = True
            t.Edit.Text = s.SvtAv1EncAppQualityDefinitions.ToSeparatedString()
            t.Edit.SaveAction = Sub(value) s.SvtAv1EncAppQualityDefinitions = value.ToSvtAv1EncAppQualityItems()?.ToList()

            t = ui.AddText()
            t.Text = "vccencFFapp quality definitions"
            t.Help = "Create custom quality definitions for vccencFFapp." + BR2 +
                         "Use this format to create your custom values with optional description:" + BR +
                         "number""text""" + BR +
                         "number: can be used with optional decimal separator (, or .)" + BR +
                         "text: description, optionally empty" + BR2 +
                         "Example:" + BR +
                         "8""Crazy""_19.5""Personal Default"" 21,5""Why not?!"" 22.0 44,3"
            t.Edit.Expand = True
            t.Edit.Text = s.VvencffappQualityDefinitions.ToSeparatedString()
            t.Edit.SaveAction = Sub(value) s.VvencffappQualityDefinitions = value.ToVvencffappQualityItems()?.ToList()


            '############### Generation
            Dim generationPage = ui.CreateFlowPage("Generation", True)

            ui.AddLabel("Position of frame number in file name:")

            Dim previewFrameNumberPosition = ui.AddMenu(Of ImageFrameNumberPosition)()
            previewFrameNumberPosition.Text = "Preview:"
            previewFrameNumberPosition.Field = NameOf(s.SaveImagePreviewFrameNumberPosition)

            Dim videoComparisonFrameNumberPosition = ui.AddMenu(Of ImageFrameNumberPosition)()
            videoComparisonFrameNumberPosition.Text = "Video Comparison:"
            videoComparisonFrameNumberPosition.Field = NameOf(s.SaveImageVideoComparisonFrameNumberPosition)

            ui.AddLine()

            b = ui.AddBool()
            b.Text = "Add line numbers to generated code"
            b.Help = ""
            b.Field = NameOf(s.CommandLinePreviewWithLineNumbers)


            '############# System
            Dim systemPage = ui.CreateFlowPage("System", True)

            Dim procPriority = ui.AddMenu(Of ProcessPriorityClass)
            procPriority.Text = "Process Priority"
            procPriority.Help = "Process priority of the applications StaxRip launches."
            procPriority.Expanded = True
            procPriority.Field = NameOf(s.ProcessPriority)

            Dim cmdline = ui.AddMenu(Of CommandLinePreview)
            cmdline.Text = "Command Line Preview"
            cmdline.Expanded = True
            cmdline.Field = NameOf(s.CommandLinePreview)

            n = ui.AddNum
            n.Text = "Minimum Disk Space"
            n.Help = "Minimum allowed disk space in GB before StaxRip shows an error message."
            n.Config = {0, 10000}
            n.Field = NameOf(s.MinimumDiskSpace)

            n = ui.AddNum
            n.Text = "Shutdown Timeout"
            n.Help = "Timeout in seconds before the shutdown is executed."
            n.Config = {0, 10000}
            n.Field = NameOf(s.ShutdownTimeout)

            n = ui.AddNum
            n.Text = "Focus Steal prevention until"
            n.Help = "StaxRip Main window will not steal focus from other active programs within the given time (in seconds) after a job in StaxRip (in the same instance) has started."
            n.Config = {-1, 1000000}
            n.Field = NameOf(s.PreventFocusStealUntil)

            n = ui.AddNum
            n.Text = "Focus Steal prevention after"
            n.Help = "StaxRip Main window will not steal focus from other active programs, if a job in StaxRip (in the same instance) takes longer than the given time (in seconds)."
            n.Config = {-1, 1000000}
            n.Field = NameOf(s.PreventFocusStealAfter)

            b = ui.AddBool
            b.Text = "Minimize processing dialog to tray"
            b.Field = NameOf(s.MinimizeToTray)

            b = ui.AddBool
            b.Text = "Extend error messages with the help of 'Err'"
            b.Field = NameOf(s.ErrorMessageExtendedByErr)

            b = ui.AddBool
            b.Text = "Force close running apps at shutdown or in hybrid mode"
            b.Field = NameOf(s.ShutdownForce)

            b = ui.AddBool
            b.Text = "Prevent system from entering standby mode while encoding"
            b.Field = NameOf(s.PreventStandby)

            b = ui.AddBool
            b.Text = "Prefer Windows Terminal over Powershell if present"
            b.Field = NameOf(s.PreferWindowsTerminal)


            '################# User Interface
            Dim uiPage = ui.CreateFlowPage("User Interface", True)

            Dim theme = ui.AddMenu(Of String)
            theme.Text = "Theme"
            theme.Expanded = True
            theme.Field = NameOf(s.ThemeName)
            theme.Add(ThemeManager.Themes.Select(Function(x) x.Name))
            theme.Button.ShowPath = True
            theme.Button.SaveAction = Sub(value) ThemeManager.SetCurrentTheme(value)
            theme.Button.ValueChangedAction = Sub(value) ThemeManager.SetCurrentTheme(value)

            Dim uiFallback = ui.AddMenu(Of Boolean)
            uiFallback.Text = "UI Fallback"
            uiFallback.Expanded = True
            uiFallback.Field = NameOf(s.UIFallback)
            uiFallback.Add({False, True})
            uiFallback.Button.ShowPath = True
            uiFallback.Button.SaveAction = Sub(value) s.UIFallback = value
            uiFallback.Button.ValueChangedAction = Sub(value) s.UIFallback = value

            Dim codeFont = ui.AddTextButton()
            codeFont.Text = "Code Font"
            codeFont.Expanded = True
            codeFont.Edit.Text = s.Fonts(FontCategory.Code)
            codeFont.ClickAction = Sub()
                                       Using td As New TaskDialog(Of FontFamily)
                                           td.Title = "Choose a monospaced font"
                                           td.Symbol = Symbol.Font

                                           For Each ff In FontManager.GetFontFamilies(FontCategory.Code, True)
                                               td.AddCommand(ff.Name, ff)
                                           Next

                                           If td.Show IsNot Nothing Then
                                               codeFont.Edit.Text = td.SelectedText
                                           End If
                                       End Using
                                   End Sub
            codeFont.Edit.SaveAction = Sub(value As String)
                                           s.Fonts(FontCategory.Code) = value
                                           FontManager.Reset()
                                       End Sub

            Dim defaultFontfamilies = FontManager.GetFontFamilies(FontCategory.Default, True)
            Dim defaultFont = ui.AddTextButton()
            defaultFont.Text = "Default Font"
            defaultFont.Expanded = True
            defaultFont.Edit.Text = s.Fonts(FontCategory.Default)
            defaultFont.ClickAction = Sub()
                                          Using td As New TaskDialog(Of FontFamily)
                                              td.Title = "Choose a default font"
                                              td.Symbol = Symbol.Font

                                              For Each ff In defaultFontfamilies
                                                  td.AddCommand(ff.Name, ff)
                                              Next

                                              If td.Show IsNot Nothing Then
                                                  defaultFont.Edit.Text = td.SelectedText
                                                  Dim family = defaultFontfamilies.FirstOrDefault(Function(x) x.Name = td.SelectedText)
                                                  If family IsNot Nothing Then
                                                      For Each control As Control In form.GetAllControls()
                                                          control.ReplaceFontFamily(family)
                                                      Next
                                                  End If
                                              End If
                                          End Using
                                      End Sub
            defaultFont.Edit.TextChangedAction = Sub(value As String)
                                                     Dim family = defaultFontfamilies.FirstOrDefault(Function(x) x.Name = value)
                                                     If family IsNot Nothing Then
                                                         For Each control As Control In form.GetAllControls()
                                                             control.ReplaceFontFamily(family)
                                                         Next
                                                     End If
                                                 End Sub
            defaultFont.Edit.SaveAction = Sub(value As String)
                                              s.Fonts(FontCategory.Default) = value
                                              FontManager.Reset()
                                          End Sub

            n = ui.AddNum()
            n.Text = "Scale Factor"
            n.Help = "Requires to restart StaxRip."
            n.Config = {0.3, 3, 0.05, 2}
            n.Field = NameOf(s.UIScaleFactor)

            Dim l = ui.AddLabel("Icon File:")
            l.Help = "The Windows Startmenu uses Windows Links which allow to use custom icon files."

            Dim tb = ui.AddTextButton
            tb.Label.Visible = False
            tb.BrowseFile("ico|*.ico", Folder.Icons)
            tb.Edit.Expand = True
            tb.Edit.Text = s.IconFile
            tb.Edit.SaveAction = Sub(value) s.IconFile = value

            l = ui.AddLabel("Remember Window Positions:")
            l.Help = "Title or beginning of the title of windows of which the location should be remembered. For all windows enter '''all'''."

            t = ui.AddText()
            t.Help = "Title or beginning of the title of windows of which the location should be remembered. For all windows enter '''all'''."
            t.Label.Visible = False
            t.Edit.Expand = True
            t.Edit.Text = s.WindowPositionsRemembered.Join(", ")
            t.Edit.SaveAction = Sub(value) s.WindowPositionsRemembered = value.SplitNoEmptyAndWhiteSpace(",")

            n = ui.AddNum()
            n.Text = "Preview size compared to screen size (percent)"
            n.Label.Offset = 1
            n.Config = {10, 90, 5}
            n.Field = NameOf(s.PreviewSize)

            b = ui.AddBool()
            b.Text = "Expand Preview window automatically depending on its size"
            b.Help = ""
            b.Field = NameOf(s.ExpandPreviewWindow)

            b = ui.AddBool()
            b.Text = "Use binary prefix (MiB) instead of decimal prefix (MB) for sizes"
            b.Help = "Binary: 1 MiB = 1024 KiB" + BR + "Decimal: 1 MB = 1000 KB" + BR2 +
                            "When selected, Staxrip will use binary prefix instead of decimal in the display and calculation of sizes." + BR +
                            "This will not affect external tools behavior nor their displayed information."
            b.Checked = s.BinaryPrefix
            b.SaveAction = Sub(value)
                               s.BinaryPrefix = value
                               UpdateTargetSizeLabel()
                               UpdateSizeOrBitrate()
                           End Sub

            b = ui.AddBool()
            b.Text = "Invert CTRL key behavior on 'Add Job' button"
            b.Help = ""
            b.Field = NameOf(s.InvertCtrlKeyOnNextButton)

            b = ui.AddBool()
            b.Text = "Invert SHIFT key behavior on 'Add Job' button"
            b.Help = ""
            b.Field = NameOf(s.InvertShiftKeyOnNextButton)

            b = ui.AddBool()
            b.Text = "Enable tooltips in menus (restart required)"
            b.Help = "Tooltips can always be shown by right-clicking menu items."
            b.Field = NameOf(s.EnableTooltips)


            '################# Frameserver
            ui.CreateFlowPage("Frameserver", True)

            Dim avsMode = ui.AddMenu(Of FrameServerMode)()
            avsMode.Text = "AviSynth Mode"
            avsMode.Field = NameOf(s.AviSynthMode)

            Dim vsMode = ui.AddMenu(Of FrameServerMode)()
            vsMode.Text = "VapourSynth Mode"
            vsMode.Field = NameOf(s.VapourSynthMode)

            b = ui.AddBool()
            b.Text = "Load AviSynth plugins"
            b.Help = "Detects and adds necessary LoadPlugin calls."
            b.Field = NameOf(s.LoadAviSynthPlugins)

            b = ui.AddBool()
            b.Text = "Load VapourSynth plugins"
            b.Help = "Detects and adds necessary LoadPlugin calls."
            b.Field = NameOf(s.LoadVapourSynthPlugins)


            '############# Preprocessing
            ui.AddControlPage(New PreprocessingControl, "Preprocessing")


            '############# Source Filters
            Dim bsAVS = AddFilterPreferences(ui, "Source Filters | AviSynth", s.AviSynthFilterPreferences, s.AviSynthProfiles)
            Dim bsVS = AddFilterPreferences(ui, "Source Filters | VapourSynth", s.VapourSynthFilterPreferences, s.VapourSynthProfiles)


            '############### Danger Zone
            Dim dangerZonePage = ui.CreateFlowPage("Danger Zone", True)

            l = ui.AddLabel("")

            l = ui.AddLabel("Don't change Danger Zone settings unless you are" + BR +
                                "a power user with debugging experience." + BR)

            l.BackColor = ThemeManager.CurrentTheme.General.DangerBackColor
            l.ForeColor = ThemeManager.CurrentTheme.General.DangerForeColor

            l = ui.AddLabel("")

            b = ui.AddBool
            b.Text = "Allow using tools with unknown version"
            b.Field = NameOf(s.AllowToolsWithWrongVersion)

            b = ui.AddBool
            b.Text = "Allow using custom paths in startup folder"
            b.Field = NameOf(s.AllowCustomPathsInStartupFolder)

            b = ui.AddBool
            b.Text = "Verify tool status"
            b.Field = NameOf(s.VerifyToolStatus)

            ui.SelectLast("last settings page")

            If form.ShowDialog() = DialogResult.OK Then
                s.AviSynthFilterPreferences = DirectCast(bsAVS.DataSource, StringPairList)
                s.AviSynthFilterPreferences.Sort()
                s.VapourSynthFilterPreferences = DirectCast(bsVS.DataSource, StringPairList)
                s.VapourSynthFilterPreferences.Sort()
                ui.Save()
                g.SetRenderer(MenuStrip)
                s.UpdateRecentProjects(Nothing)
                UpdateRecentProjectsMenu()
                UpdateNextButton()

                If Icon IsNot g.Icon Then
                    Icon = g.Icon
                End If

                Dim newDefaultFont = FontManager.GetDefaultFont()
                If Not oldDefaultFont.FontFamily.Equals(newDefaultFont.FontFamily) Then
                    For Each control As Control In GetAllControls()
                        control.ReplaceFontFamily(newDefaultFont.FontFamily)
                    Next
                    SetAudioTracks()
                End If

                FrameServerHelp.AviSynthToolPath()
                g.SaveSettings()
            Else
                If Not ThemeManager.CurrentTheme.Name.Equals(s.ThemeName, StringComparison.OrdinalIgnoreCase) Then
                    ThemeManager.SetCurrentTheme(s.ThemeName)
                End If
            End If

            If restartID <> GetRestartID() Then
                MsgInfo("Please restart StaxRip.")
            End If

            ui.SaveLast("last settings page")
        End Using
    End Sub
End Class
