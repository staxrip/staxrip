.. option:: AddBatchJob

    Adds a batch job.

.. list-table::
    :widths: auto

    * - sourcefile <string>

.. option:: AddFilter

    Adds a filter at the end of the script.

.. list-table::
    :widths: auto

    * - active <boolean>
    * - name <string>
    * - category <string>
    * - script <string>

.. option:: AddJob

    Adds a job to the job list.

.. list-table::
    :widths: auto

    * - showConfirmation <boolean>
      - Show Confirmation
    * - templateName <string>
      - Template Name: Name of the template to be loaded after the job was added. Empty to load no template.
    * - position <integer>
      - Position to insert new job

.. option:: CheckForUpdate

    Checks if a update is available.

.. option:: ClearJobs

    Clears the job list.

.. option:: CopyToClipboard

    Copies a string to the clipboard.

.. list-table::
    :widths: auto

    * - value <string>
      - Copies text to the clipboard. May contain macros.

.. option:: DeleteFiles

    Deletes files in a given directory.

.. list-table::
    :widths: auto

    * - dir <string>
      - Directory: Directory in which to delete files.
    * - filter <string>
      - Example: '.txt .log'

.. option:: DynamicMenuItem

    Placeholder for dynamically updated menu items.

.. list-table::
    :widths: auto

    * - id <dynamicmenuitemid>
      -  Audio1Profiles, Audio2Profiles, EncoderProfiles, FilterSetupProfiles, MuxerProfiles, RecentProjects, TemplateProjects, HelpApplications, Scripts

.. option:: ExecuteCommandLine

    Executes a command line. If Shell Execute is disabled then macros are passed in as environment variables.

.. list-table::
    :widths: auto

    * - commandLine <string>
      - Command Line: The command line to be executed. Macros are solved.
    * - waitForExit <boolean>
      - Wait For Exit: Halt until the command line returns.
    * - showProcessWindow <boolean>
      - Show Process Window: Redirects the output of console apps to StaxRips process window. Disables Shell Execute.
    * - useShellExecute <boolean>
      - Use Shell Execute: Executes the command line using the shell. Available when the Show Process Window option is disabled.
    * - workingDirectory <string>
      - Working Directory: Working directory the process will use.

.. option:: ExecutePowerShellScript

    Executes PowerShell script code.

.. list-table::
    :widths: auto

    * - code <string>
      - Script Code: PowerShell script code to be executed. Macros are expanded.
    * - externalShell <boolean>
      - Use External Shell: Execute in StaxRip to automate StaxRip or use external shell.

.. option:: ExecuteScriptFile

    Executes a PowerShell PS1 script file.

.. list-table::
    :widths: auto

    * - filepath <string>
      - File Path: Filepath to a PowerShell PS1 script file. May contain macros.

.. option:: Exit

    Exits StaxRip

.. option:: ImportVideoEncoderCommandLine

    Changes video encoder settings.

.. list-table::
    :widths: auto

    * - commandLine <string>
      - Command Line

.. option:: LoadProfile

    Loads a audio or video profile.

.. list-table::
    :widths: auto

    * - videoProfile <string>
      - Video
    * - audioProfile1 <string>
      - Audio 1
    * - audioProfile2 <string>
      - Audio 2

.. option:: LoadSourceFile

    Loads a source file.

.. list-table::
    :widths: auto

    * - path <string>
      - Source File Path

.. option:: LoadTemplate

    Loads a template.

.. list-table::
    :widths: auto

    * - name <string>

.. option:: OpenHelpTopic

    Opens a given help topic In the help browser.

.. list-table::
    :widths: auto

    * - topic <string>
      - Help Topic: Name Of the help topic To be opened.

.. option:: PlaySound

    Plays audio file.

.. list-table::
    :widths: auto

    * - FilePath <string>
      - Filepath to a mp3, wav or wmv sound file.
    * - Volume <integer>
      - Volume (%)

.. option:: ResetSettings

    Shows a dialog allowing to reset specific settings.

.. option:: SaveGIF

    Shows a Open File dialog to generate a short GIF.

.. option:: SaveMKVHDR

    Shows a Open File dialog to add the remaining HDR10 Metadata to a MKV file.

.. option:: SaveMTN

    Shows a Open File dialog to generate thumbnails using mtn engine

.. option:: SavePNG

    Shows a open file dialog to create a high quality PNG animation.

.. option:: SaveProject

    Saves the current project.

.. option:: SaveProjectAs

    Saves the current project.

.. option:: SaveProjectAsTemplate

    Saves the current project as template.

.. option:: SaveProjectPath

    Saves the current project at the specified path.

.. list-table::
    :widths: auto

    * - path <string>
      - The path may contain macros.

.. option:: SetBitrate

    Sets the target video bitrate in Kbps.

.. list-table::
    :widths: auto

    * - bitrate <integer>
      - Target Video Bitrate

.. option:: SetFilter

    Sets a filter replacing a existing filter of same category.

.. list-table::
    :widths: auto

    * - name <string>
    * - category <string>
    * - script <string>

.. option:: SetHideDialogsOption

    Sets the project option 'Hide dialogs asking to demux, source filter etc.'

.. list-table::
    :widths: auto

    * - hide <boolean>

.. option:: SetPercent

    Sets the bitrate according to the compressibility.

.. list-table::
    :widths: auto

    * - value <integer>
      - Percent Value

.. option:: SetSize

    Sets the target file size in MB.

.. list-table::
    :widths: auto

    * - targetSize <integer>
      - Target File Size

.. option:: SetTargetFile

    Sets the file path of the target file.

.. list-table::
    :widths: auto

    * - path <string>
      - Target File Path

.. option:: SetTargetImageSize

    Sets the target image size.

.. list-table::
    :widths: auto

    * - width <integer>
    * - height <integer>

.. option:: SetTargetImageSizeByPixel

    Sets the target image size by pixels (width x height).

.. list-table::
    :widths: auto

    * - pixel <integer>

.. option:: ShowAppsDialog

    Dialog to manage external tools.

.. option:: ShowAudioProfilesDialog

    Dialog to manage audio profiles.

.. list-table::
    :widths: auto

    * - number <integer>
      - Track Number (0 or 1)

.. option:: ShowBatchGenerateThumbnailsDialog

    Shows a dialog to generate thumbnails.

.. option:: ShowCropDialog

    Shows the crop dialog to crop borders.

.. option:: ShowDemuxTool

    Allows to use StaxRip's demuxing GUIs independently.

.. option:: ShowEncoderProfilesDialog

    Shows a dialog to manage video encoder profiles.

.. option:: ShowEventCommandsDialog

    Shows the Event Command dialog.

.. option:: ShowFileBrowserToOpenProject

    Shows a file browser to open a project file.

.. option:: ShowFilterProfilesDialog

    Dialog to configure AviSynth filter profiles.

.. option:: ShowFiltersEditor

    Dialog to edit filters.

.. option:: ShowFilterSetupProfilesDialog

    Dialog to configure filter setup profiles.

.. option:: ShowHardcodedSubtitleDialog

    Shows a dialog to add a hardcoded subtitle.

.. option:: ShowJobsDialog

    Dialog to manage batch jobs.

.. option:: ShowLogFile

    Shows the log file with the built-in log file viewer.

.. option:: ShowMainMenuEditor

    Dialog to configure the main menu.

.. option:: ShowMediaInfo

    Shows media info on a given file.

.. list-table::
    :widths: auto

    * - filepath <string>
      - May contain macros.

.. option:: ShowMediaInfoBrowse

    Shows a Open File dialog to show media info.

.. option:: ShowMediaInfoFolderViewDialog

    Presents MediaInfo of all files in a folder in a grid view.

.. option:: ShowMessageBox

    Shows a message box.

.. list-table::
    :widths: auto

    * - mainInstruction <string>
      - Main Instruction: Main instruction may contain macros.
    * - content <string>
      - May contain macros.
    * - icon <msgicon>
      -  None, Error, Question, Warning, Info

.. option:: ShowMkvInfo

    Shows a Open File dialog to open a file to be shown by the console tool mkvinfo.

.. option:: ShowMuxerProfilesDialog

    Dialog to manage Muxer profiles.

.. option:: ShowOpenSourceBatchFilesDialog

    Dialog to open a file batch source.

.. option:: ShowOpenSourceBlurayFolderDialog

    Dialog to open a Blu-ray folder source.

.. option:: ShowOpenSourceDialog

    Dialog to open source files.

.. option:: ShowOpenSourceMergeFilesDialog

    Dialog to open a merged files source.

.. option:: ShowOpenSourceSingleFileDialog

    Dialog to open a single file source.

.. option:: ShowOptionsDialog

    Dialog to configure project options.

.. option:: ShowPreview

    Dialog to preview or cut the video.

.. option:: ShowScriptInfo

    Shows script info using various console tools.

.. option:: ShowSettingsDialog

    Shows the settings dialog.

.. option:: ShowSizeMenuEditor

    Menu editor for the size menu.

.. option:: ShowVideoComparison

    Shows a dialog to compare different videos.

.. option:: Shutdown

    Shut down PC.

.. option:: Standby

    Standby PC.

.. option:: StartAutoCrop

    Crops borders automatically.

.. option:: StartCompCheck

    Starts the compressibility check.

.. option:: StartEncoding

    Creates a job and runs the job list.

.. option:: StartJobs

    Runs all active jobs of the job list.

.. option:: StartSmartCrop

    Crops borders automatically until the proper aspect ratio is found.

.. option:: StartTool

    Starts a tool by name as shown in the app manage dialog.

.. list-table::
    :widths: auto

    * - name <string>
      - Tool Name: Tool name as shown in the app manage dialog.

.. option:: TestAndDynamicFileCreation

    Development tests and creation of doc files.

.. option:: WriteLog

    Writes a log message to the log file.

.. list-table::
    :widths: auto

    * - header <string>
      - Header is optional and may contain macros.
    * - message <string>
      - Message is optional and may contain macros.

