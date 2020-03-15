Command Line Interface
======================

Switches are processed in the order they appear in the command line.

The command line interface, the customizable main menu and Event Command features are built with a shared command system.

There is a special mode where only the MediaInfo window is shown using -mediainfo , this is useful for File Explorer integration with an app like Open++.


Examples
--------

StaxRip C:\\Movie\\project.srip

StaxRip C:\\Movie\\VTS_01_1.VOB C:\\Movie 2\\VTS_01_2.VOB

StaxRip -LoadTemplate:DVB C:\\Movie\\capture.mpg -StartEncoding -Standby

StaxRip -ShowMessageBox:"main text...","text ...",info


Switches
--------

-AddFilter:active,name,category,script
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Adds a filter at the end of the script.


-AddJob:showConfirmation,templateName
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

templateName: Name of the template to be loaded after the job was added. Empty to load no template.

Adds a job to the job list.


-ClearJobs
~~~~~~~~~~

Clears the job list.


-CopyToClipboard:value
~~~~~~~~~~~~~~~~~~~~~~

value: Copies the text to the clipboard. The text may contain macros.

Copies a string to the clipboard.


-DeleteFiles:dir,filter
~~~~~~~~~~~~~~~~~~~~~~~

dir: Directory in which to delete files.

filter: Example: '.txt .log'

Deletes files in a given directory.


-DynamicMenuItem:id
~~~~~~~~~~~~~~~~~~~

DynamicMenuItemID: Audio1Profiles, Audio2Profiles, EncoderProfiles, FilterSetupProfiles, MuxerProfiles, RecentProjects, TemplateProjects, HelpApplications, Scripts

Placeholder for dynamically updated menu items.


-ExecuteBatchScript:batchScript,interpretOutput
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

batchScript: Batch script code to be executed. Macros are solved as well as passed in as environment variables.

interpretOutput: Interprets each output line as StaxRip command.

Saves a batch script as bat file and executes it. Macros are solved as well as passed in as environment variables.


-ExecuteCommandLine:commandLines,waitForExit,showProcessWindow,asBatch
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

commandLines: One or more command lines to be executed or if batch mode is used content of the batch file. Macros are solved as well as passed in as environment variables.

waitForExit: This will halt the main thread until the command line returns.

showProcessWindow: Redirects the output of console apps to the process window.

asBatch: Alternative mode that creats a BAT file to execute.

Executes command lines separated by a line break line by line. Macros are solved and passed as environment variables.


-ExecutePowerShellScript:code,externalShell
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

code: PowerShell script code to be executed.

externalShell: Execute in StaxRip to automate StaxRip or in external Shell.

Executes PowerShell script code.


-ExecuteScriptFile:filepath
~~~~~~~~~~~~~~~~~~~~~~~~~~~

filepath: Filepath to a PowerShell script, the path may contain macros.

Executes a PowerShell script.


-Exit
~~~~~

Exits StaxRip


-ImportVideoEncoderCommandLine:commandLine
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Changes the video encoders settings.


-LoadProfile:videoProfile,audioProfile1,audioProfile2
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Loads a audio or video profile.


-LoadSourceFile:path
~~~~~~~~~~~~~~~~~~~~

Loads the source file.


-LoadTemplate:name
~~~~~~~~~~~~~~~~~~

Loads a template.


-MediainfoMKV
~~~~~~~~~~~~~

Shows the Metadata Information for MKV file including HDR10 data.


-MediaInfoShowMedia
~~~~~~~~~~~~~~~~~~~

View the Metadata of any Selected File


-OpenHelpTopic:topic
~~~~~~~~~~~~~~~~~~~~

topic: Name Of the help topic To be opened.

Opens a given help topic In the help browser.


-PlaySound:Filepath,Volume
~~~~~~~~~~~~~~~~~~~~~~~~~~

Filepath: Filepath To a mp3, wav Or wmv sound file.

Plays a mp3, wav Or wmv sound file.


-ResetSettings
~~~~~~~~~~~~~~

Shows a dialog allowing to reset various settings.


-SaveGif
~~~~~~~~

Generates a Short Gif Based on Input data.


-SaveMKVHDR
~~~~~~~~~~~

Adds the Remaining HDR10 Metadata to MKV file.


-SaveMTN
~~~~~~~~

Generate Thumbnails Using MTN Engine


-SavePNG
~~~~~~~~

Creates Very High Quality Animations in the Form of PNG.


-SaveProject
~~~~~~~~~~~~

Saves the current project.


-SaveProjectAs
~~~~~~~~~~~~~~

Saves the current project.


-SaveProjectAsTemplate
~~~~~~~~~~~~~~~~~~~~~~

Saves the current project as template.


-SaveProjectPath:path
~~~~~~~~~~~~~~~~~~~~~

path: The path may contain macros.

Saves the current project at the specified path.


-SetBitrate:bitrate
~~~~~~~~~~~~~~~~~~~

Sets the target video bitrate in Kbps.


-SetFilter:name,category,script
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Sets a filter replacing a existing filter of same category.


-SetHideDialogsOption:hide
~~~~~~~~~~~~~~~~~~~~~~~~~~

Sets the project option 'Hide dialogs asking to demux, source filter etc.'


-SetPercent:value
~~~~~~~~~~~~~~~~~

Sets the bitrate according to the compressibility.


-SetSize:targetSize
~~~~~~~~~~~~~~~~~~~

Sets the target file size in MB.


-SetTargetFile:path
~~~~~~~~~~~~~~~~~~~

Sets the file path of the target file.


-SetTargetImageSize:width,height
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Sets the target image size.


-SetTargetImageSizeByPixel:pixel
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Sets the target image size by pixels (width x height).


-ShowAppsDialog
~~~~~~~~~~~~~~~

Dialog to manage external applications.


-ShowAudioProfilesDialog:number
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Dialog to manage audio profiles.


-ShowBatchGenerateThumbnailsDialog
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Shows a dialog to generate thumbnails.


-ShowCommandPrompt
~~~~~~~~~~~~~~~~~~

Shows a command prompt with the temp directory of the current project.


-ShowCropDialog
~~~~~~~~~~~~~~~

Dialog to crop borders.


-ShowDemuxTool
~~~~~~~~~~~~~~

Allows to use StaxRip's demuxing GUIs independently.


-ShowEncoderProfilesDialog
~~~~~~~~~~~~~~~~~~~~~~~~~~

Dialog to manage encoder profiles.


-ShowEventCommandsDialog
~~~~~~~~~~~~~~~~~~~~~~~~

A Event Command allows to define a command to be executed on a defined event. Furthermore criteria can be defined to execute the command only if certain criteria is matched.


-ShowFileBrowserToOpenProject
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Shows a file browser to open a project file.


-ShowFilterProfilesDialog
~~~~~~~~~~~~~~~~~~~~~~~~~

Dialog to configure AviSynth filter profiles.


-ShowFiltersEditor
~~~~~~~~~~~~~~~~~~

Dialog to edit filters.


-ShowFilterSetupProfilesDialog
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Dialog to configure filter setup profiles.


-ShowHardcodedSubtitleDialog
~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Shows a dialog to add a hardcoded subtitle.


-ShowHelpURL:url
~~~~~~~~~~~~~~~~

url: URL or local file to be shown in the internet explorer powered help browser.

Opens a given URL or local file in the help browser.


-ShowJobsDialog
~~~~~~~~~~~~~~~

Dialog to manage batch jobs.


-ShowLAVFiltersConfigDialog
~~~~~~~~~~~~~~~~~~~~~~~~~~~

Shows LAV Filters video decoder configuration


-ShowLogFile
~~~~~~~~~~~~

Shows the log file with the built in log file viewer.


-ShowMainMenuEditor
~~~~~~~~~~~~~~~~~~~

Dialog to configure the main menu.


-ShowMediaInfo:filepath
~~~~~~~~~~~~~~~~~~~~~~~

filepath: The filepath may contain macros.

Shows media info on a given file.


-ShowMediaInfoFolderViewDialog
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

Presents MediaInfo of all files in a folder in a list view.


-ShowMessageBox:mainInstruction,content,icon
~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~

mainInstruction: Main instruction may contain macros.

content: Content may contain macros.

MsgIcon: None, Error, Question, Warning, Info

Shows a message box.


-ShowMuxerProfilesDialog
~~~~~~~~~~~~~~~~~~~~~~~~

Dialog to manage Muxer profiles.


-ShowOpenSourceDialog
~~~~~~~~~~~~~~~~~~~~~

Dialog to open source files.


-ShowOptionsDialog
~~~~~~~~~~~~~~~~~~

Dialog to configure project options.


-ShowPowerShell
~~~~~~~~~~~~~~~

Shows the powershell with aliases for all tools staxrip includes.


-ShowPreview
~~~~~~~~~~~~

Dialog to preview or cut the video.


-ShowScriptInfo
~~~~~~~~~~~~~~~

Shows script info using various console tools.


-ShowSettingsDialog
~~~~~~~~~~~~~~~~~~~

Shows the settings dialog.


-ShowSizeMenuEditor
~~~~~~~~~~~~~~~~~~~

Menu editor for the size menu.


-ShowVideoComparison
~~~~~~~~~~~~~~~~~~~~

Compare and extract images for video comparisons.


-Shutdown
~~~~~~~~~

Shuts PC down.


-Standby
~~~~~~~~

Puts PC in standby mode.


-StartAutoCrop
~~~~~~~~~~~~~~

Crops borders automatically.


-StartCompCheck
~~~~~~~~~~~~~~~

Starts the compressibility check.


-StartEncoding
~~~~~~~~~~~~~~

Creates a job and runs the job list.


-StartJobs
~~~~~~~~~~

Runs all active jobs of the job list.


-StartSmartCrop
~~~~~~~~~~~~~~~

Crops borders automatically until the proper aspect ratio is found.


-StartTool:name
~~~~~~~~~~~~~~~

name: Tool name as shown in the app manage dialog.

Starts a tool by name as shown in the app manage dialog.


-Test
~~~~~

Test


-WriteLog:header,message
~~~~~~~~~~~~~~~~~~~~~~~~

header: Header is optional.

message: Message is optional and may contain macros.

Writes a log message to the process window.


