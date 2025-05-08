### AddAttachments

Adds attachments to the container (works only with mkvmerge).

| Parameter |
| --- |
| File Paths \<string[]\> |

### AddBatchJob

Adds a batch job for a single file.

| Parameter |
| --- |
| sourcefile \<string\> |

### AddBatchJobs

Adds batch jobs for multiple files.

| Parameter |
| --- |
| sourcefiles \<string[]\> |

### AddChaptersFile

Adds a chapters file to the container.

| Parameter |
| --- |
| File Path \<string\> |

### AddFilter

Adds a filter at the end of the script.

| Parameter |
| --- |
| active \<boolean\> |
| name \<string\> |
| category \<string\> |
| script \<string\> |

### AddJob

Adds a job to the job list.

| Parameter | Description |
| --- | --- |
| Show Confirmation \<boolean\> | |
| Template Name \<string\> | Name of the template to be loaded after the job was added. Empty to load no template. |
| Position to insert new job \<integer\> | |

### AddSubtitle

Add a subtitle file to the container.

| Parameter | Description |
| --- | --- |
| Enabled \<boolean\> | Enabled or not |
| Language \<string\> | Language code or name |
| Name \<string\> | Name/Title of the subtitle |
| Default \<boolean\> | Default flag |
| Forced \<boolean\> | Forced flag |
| Commentary \<boolean\> | Commentary flag |
| Hearing-Impaired \<boolean\> | Hearing-Impaired flag |
| File Path \<string\> | Full path of the subtitle file |

### AddSubtitles

Adds subtitles to the container.

| Parameter |
| --- |
| File Paths \<string[]\> |

### AddTagFile

Adds a tag file to the container (works only with mkvmerge).

| Parameter |
| --- |
| File Path \<string\> |

### AddTags

Adds tags to the container (works only with mkvmerge).

| Parameter | Description |
| --- | --- |
| Tags \<string\> | name 1 = value 1; name 2 = value 2; etc. |

### CheckForUpdate

Checks if an update is available.

### ClearJobs

Clears the job list.

### CloseProject

Closes the current project.

### CopyToClipboard

Copies a string to the clipboard.

| Parameter | Description |
| --- | --- |
| Value \<string\> | Copies text to the clipboard. May contain macros. |

### DeleteFiles

Deletes files in a given directory.

| Parameter | Description |
| --- | --- |
| Directory \<string\> | Directory in which to delete files. |
| Filter \<string\> | Example: '.txt .log' |

### DisableEvents

Disables events by name.

| Parameter |
| --- |
| Event Names \<string[]\> |

### DisableFilterNames

Disables filters by name.

| Parameter |
| --- |
| Filter Names \<string[]\> |

### DynamicMenuItem

Placeholder for dynamically updated menu items.

| Parameter | Description |
| --- | --- |
| ID \<dynamicmenuitemid\> | AudioProfiles, EncoderProfiles, FilterSetupProfiles, MuxerProfiles, RecentProjects, TemplateProjects, HelpApplications, Scripts, AddFilters, InsertFilters, ReplaceFilters, FilterCategory |

### EnableEvents

Enables events by name.

| Parameter |
| --- |
| Event Names \<string[]\> |

### EnableFilterNames

Enables filters by name.

| Parameter |
| --- |
| Filter Names \<string[]\> |

### ExecuteCommandLine

Executes a command line. If Shell Execute is disabled then macros are passed in as environment variables.

| Parameter | Description |
| --- | --- |
| Command Line \<string\> | The command line to be executed. Macros are solved. |
| Wait For Exit \<boolean\> | Halt until the command line returns. |
| Show Process Window \<boolean\> | Redirects the output of console apps to StaxRips process window. Disables Shell Execute. |
| Use Shell Execute \<boolean\> | Executes the command line using the shell. Available when the Show Process Window option is disabled. |
| Working Directory \<string\> | Working directory the process will use. |

### ExecutePowerShellCode

Executes PowerShell code.

| Parameter | Description |
| --- | --- |
| Script Code \<string\> | PowerShell script code to be executed. Macros are expanded. |
| Use External Shell \<boolean\> | Execute in StaxRip to automate StaxRip or use external shell. |

### ExecutePowerShellFile

Executes a PowerShell script file.

| Parameter | Description |
| --- | --- |
| File Path \<string\> | Filepath to a PowerShell script file. May contain macros. |
| Arguments \<string\> | Semicolon separated arguments passed to the script host. May contain macros. |

### Exit

Exits StaxRip

### ExitWithoutSaving

Exits StaxRip without saving an unsaved project.

### ExtractHdrMetadata

Extract dynamic HDR metadata from a source file.

| Parameter |
| --- |
| sourcePath \<string\> |

### GenerateWikiContent

Generates various wiki content.

### GetApplicationDetails

Gets application details.

| Parameter |
| --- |
| includeName \<boolean\> |
| includeVersion \<boolean\> |
| includeSettingsVersion \<boolean\> |

### ImportVideoEncoderCommandLine

Changes video encoder settings.

| Parameter |
| --- |
| Command Line \<string\> |

### ImportVideoEncoderCommandLineFromTextFile

Changes video encoder settings from a text file.

| Parameter |
| --- |
| File Path \<string\> |

### LoadProfile

Loads an audio or video profile.

| Parameter |
| --- |
| Video \<string\> |
| Audio 1 \<string\> |
| Audio 2 \<string\> |

### LoadSourceFile

Loads a source file.

| Parameter |
| --- |
| Source File Path \<string\> |

### LoadSourceFiles

Loads multiple source files.

| Parameter |
| --- |
| Source File Paths \<string[]\> |

### LoadSourceFilesWithTemplateSelection

Loads multiple source files after asking for template that shall be used.

| Parameter |
| --- |
| Source File Paths \<string[]\> |

### LoadSourceFileWithTemplateSelection

Loads a source file after asking for template that shall be used.

| Parameter |
| --- |
| Source File Path \<string\> |

### LoadTemplate

Loads a template.

| Parameter |
| --- |
| name \<string\> |

### LoadTemplateWithSelectionDialog

Loads a template that you can choose from via dialog.

| Parameter |
| --- |
| source \<string\> |
| timeout \<integer\> |
| templateFolder \<string\> |

### NoFocus

Opens StaxRip without taking focus

### OpenHelpTopic

Opens a given help topic in the help browser.

| Parameter | Description |
| --- | --- |
| Help Topic \<string\> | Name of the help topic to be opened. |

### PlaySound

Plays audio file.

| Parameter | Description |
| --- | --- |
| FilePath \<string\> | Filepath to a mp3, wav or wmv sound file. |
| Volume (%) \<integer\> | |

### ResetSettings

Shows a dialog allowing to reset specific settings.

### SaveGIF

Shows an Open File dialog to generate a short GIF.

### SaveMKVHDR

Shows an Open File dialog to add the remaining HDR10 Metadata to a MKV file.

### SavePNG

Shows an Open File dialog to create a high quality PNG animation.

### SaveProject

Saves the current project.

### SaveProjectAs

Saves the current project.

### SaveProjectAsTemplate

Saves the current project as template.

### SaveProjectPath

Saves the current project at the specified path.

| Parameter | Description |
| --- | --- |
| path \<string\> | The path may contain macros. |

### SetBitrate

Sets the target video bitrate in Kbps.

| Parameter |
| --- |
| Target Video Bitrate \<integer\> |

### SetCrop

Sets crop values

| Parameter | Description |
| --- | --- |
| Left Crop \<integer\> | Sets the left crop value. |
| Top Crop \<integer\> | Sets the top crop value. |
| Right Crop \<integer\> | Sets the right crop value. |
| Bottom Crop \<integer\> | Sets the bottom crop value. |

### SetFilter

Sets a filter replacing an existing filter of same category.

| Parameter |
| --- |
| name \<string\> |
| category \<string\> |
| script \<string\> |

### SetHideDialogsOption

Sets the project option 'Hide dialogs asking to demux, source filter etc.'

| Parameter |
| --- |
| hide \<boolean\> |

### SetPercent

Sets the bitrate according to the compressibility.

| Parameter |
| --- |
| Percent Value \<integer\> |

### SetSize

Sets the target file size in MB.

| Parameter |
| --- |
| Target File Size \<integer\> |

### SetTargetFile

Sets the file path of the target file.

| Parameter |
| --- |
| Target File Path \<string\> |

### SetTargetImageSize

Sets the target image size.

| Parameter |
| --- |
| width \<integer\> |
| height \<integer\> |

### SetTargetImageSizeByPixel

Sets the target image size by pixels (width x height).

| Parameter |
| --- |
| pixel \<integer\> |

### SetTitle

Sets the container title (works only with mkvmerge).

| Parameter |
| --- |
| Title \<string\> |

### SetVideoTrackLanguage

Sets the video track language (works only with mkvmerge).

| Parameter |
| --- |
| Language \<string\> |

### SetVideoTrackName

Sets the video track name.

| Parameter |
| --- |
| Name \<string\> |

### ShowAdvancedScriptInfo

Shows advanced AviSynth/VapourSynth output script info using various tools.

### ShowAppsDialog

Dialog to manage external tools.

### ShowAudioProfilesDialog

Dialog to manage audio profiles.

| Parameter |
| --- |
| Track Number \<integer\> |

### ShowChangelog

Shows the latest changes.

| Parameter |
| --- |
| force \<boolean\> |

### ShowCodePreview

Dialog to preview script code.

### ShowCropDialog

Shows the crop dialog to crop borders.

### ShowDemuxTool

Allows to use StaxRip's demuxing GUIs independently.

### ShowEncoderProfilesDialog

Shows a dialog to manage video encoder profiles.

### ShowEventCommandsDialog

Shows the Event Command dialog.

### ShowFileBrowserToOpenProject

Shows a file browser to open a project file.

### ShowFilterProfilesDialog

Dialog to configure filter profiles.

### ShowFiltersEditor

Dialog to edit filters.

### ShowFilterSetupProfilesDialog

Dialog to configure filter setup profiles.

### ShowHardcodedSubtitleDialogFromLastSourceDir

Shows a dialog, opening from the last source directory, to add a hardcoded subtitle.

### ShowHardcodedSubtitleDialogFromTempDir

Shows a dialog, opening from the temp folder, to add a hardcoded subtitle.

### ShowJobsDialog

Dialog to manage batch jobs.

### ShowLogFile

Shows the log file with the built-in log file viewer.

### ShowMacrosDialog

Dialog that shows available macros.

### ShowMainMenuEditor

Dialog to configure the main menu.

### ShowMediaInfo

Shows media info on a given file.

| Parameter | Description |
| --- | --- |
| Filepath \<string\> | May contain macros. |

### ShowMediaInfoBrowse

Shows an Open File dialog to show media info.

### ShowMediaInfoFolderViewDialog

Presents MediaInfo of all files in a folder in a grid view.

### ShowMessageBox

Shows a message box.

| Parameter | Description |
| --- | --- |
| Main Instruction \<string\> | Main instruction may contain macros. |
| Content \<string\> | May contain macros. |
| Icon \<taskicon\> | None, Info, Warning, Question, Error, Shield |

### ShowMkvInfo

Shows an Open File dialog to open a file to be shown by the console tool mkvinfo.

### ShowMuxerProfilesDialog

Dialog to manage Muxer profiles.

### ShowOpenSourceBatchFilesDialog

Dialog to open a file batch source.

### ShowOpenSourceBlurayFolderDialog

Dialog to open a Blu-ray folder source.

### ShowOpenSourceBlurayImageDialog

Dialog to open a Blu-ray ISO image.

### ShowOpenSourceDialog

Dialog to open source files.

### ShowOpenSourceMergeFilesDialog

Dialog to open a merged files source.

### ShowOpenSourceMultipleFilesDialog

Dialog to open multiple file sources.

### ShowOpenSourceSingleFileDialog

Dialog to open a single file source.

### ShowOptionsDialog

Dialog to configure project options.

### ShowPreview

Window to preview or cut the video.

### ShowPreviewDialog

Dialog to preview or cut the video.

### ShowRemovedFunctionalityMessage

Shows a message about removed functionality.

### ShowScriptInfo

Shows info about the output AviSynth/VapourSynth script.

### ShowSettingsDialog

Shows the settings dialog.

### ShowSizeMenuEditor

Menu editor for the size menu.

### ShowThumbnailerDialogAsync

Shows a dialog to select files, for those thumbnail sheets are created.

### ShowVideoComparison

Shows a dialog to compare different videos.

### Shutdown

Shut down PC.

### Standby

Standby PC.

### StartAutoCrop

Crops borders automatically.

### StartCompCheck

Starts the compressibility check.

### StartEncoding

Creates a job and runs the job list.

### StartJobs

Runs all active jobs of the job list.

### StartNewInstance

Launches a new instance of StaxRip

### StartSmartCrop

Crops borders automatically until the proper aspect ratio is found.

### StartTool

Starts a tool by name as shown in the app manage dialog.

| Parameter | Description |
| --- | --- |
| Tool Name \<string\> | Tool name as shown in the app manage dialog. |

### Test

Development tests.

### WriteLog

Writes a log message to the log file.

| Parameter | Description |
| --- | --- |
| Header \<string\> | Header is optional and may contain macros. |
| Message \<string\> | Message is optional and may contain macros. |

