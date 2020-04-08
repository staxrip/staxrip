
Imports System.ComponentModel
Imports System.Reflection
Imports StaxRip.UI

Public Class Docs
    Shared Sub GenerateDynamicFiles()
        GenerateMacroTableFile()
        GenerateMiscFiles()
    End Sub

    Shared Sub GenerateMacroTableFile()
        Dim text =
            ".. csv-table::" + BR +
            "    :header: ""Name"", ""Description""" + BR +
            "    :widths: auto" + BR2 +
            "    " + PowerShell.ConvertToCSV(",", Macro.GetTips).Right(BR).Right(BR).Replace(BR, BR + "    ")

        UpdateFile(Folder.Startup + "..\docs\generated\macro-table.rst", text)
    End Sub

    Shared Sub GenerateMiscFiles()
        Dim supportedTools = "Supported Tools" + BR + "===============" + BR2 + "Tools" + BR + "-----" + BR2

        For Each i In Package.Items.Values
            If Not TypeOf i Is PluginPackage Then
                supportedTools += i.Name + BR + "~".Multiply(i.Name.Length) + BR2 + i.Description + BR2
                supportedTools += "Used Version: " + i.Version + BR2 + i.WebURL + BR2 + BR
            End If
        Next

        supportedTools += "AviSynth Plugins" + BR + "----------------" + BR

        For Each i In Package.Items.Values.OfType(Of PluginPackage)
            If Not i.AvsFilterNames.NothingOrEmpty Then
                supportedTools += i.Name + BR + "~".Multiply(i.Name.Length) + BR2 + i.Description + BR2
                supportedTools += "Filters: " + i.AvsFilterNames.Join(", ") + BR2
                supportedTools += "Used Version: " + i.Version + BR2 + i.WebURL + BR2 + BR
            End If
        Next

        supportedTools += "VapourSynth Plugins" + BR + "-------------------" + BR

        For Each i In Package.Items.Values.OfType(Of PluginPackage)
            If Not i.VSFilterNames.NothingOrEmpty Then
                supportedTools += i.Name + BR + "~".Multiply(i.Name.Length) + BR2 + i.Description + BR2
                supportedTools += "Filters: " + i.VSFilterNames.Join(", ") + BR2
                supportedTools += "Used Version: " + i.Version + BR2 + i.WebURL + BR2 + BR
            End If
        Next

        supportedTools.WriteFileUTF8(Folder.Startup + "..\docs\tools.rst")

        Dim screenshots = "Screenshots" + BR + "===========" + BR2 + ".. contents::" + BR2
        Dim screenshotFiles = Directory.GetFiles(Folder.Startup + "..\docs\screenshots").ToList
        screenshotFiles.Sort(New StringLogicalComparer)

        For Each i In screenshotFiles
            Dim name = i.Base.Replace("_", " ").Trim
            screenshots += name + BR + "-".Multiply(name.Length) + BR2 + ".. image:: screenshots/" + i.FileName + BR2
        Next

        screenshots.WriteFileUTF8(Folder.Startup + "..\docs\screenshots.rst")

        Dim powershell = "PowerShell Scripting
====================

StaxRip can be automated via PowerShell scripting.


Events
------

In order to run scripts on certain events the following events are available:

"

        For Each i As ApplicationEvent In System.Enum.GetValues(GetType(ApplicationEvent))
            powershell += "- ``" + i.ToString + "`` " + DispNameAttribute.GetValueForEnum(i) + BR
        Next

        powershell += BR + "Assign to an event by saving a script file in the scripting folder using the event name as file name." + BR2 + "The scripting folder can be opened with:" + BR2 + "Main Menu > Tools > Scripts > Open script folder" + BR2 + "Use one of the following file names:" + BR2

        For Each i In System.Enum.GetNames(GetType(ApplicationEvent))
            powershell += "- " + i.ToString + ".ps1" + BR
        Next

        powershell += BR + "Support
-------

If you have questions feel free to ask here:

https://github.com/stax76/staxrip/issues/200


Default Scripts
---------------

"
        Dim psdir = Folder.Startup + "..\docs\powershell"
        DirectoryHelp.Delete(psdir)
        Directory.CreateDirectory(psdir)

        For Each i In Directory.GetFiles(Folder.Startup + "Apps\Scripts")
            FileHelp.Copy(i, psdir + "\" + i.FileName)
            Dim filename = i.FileName
            powershell += filename + BR + "~".Multiply(filename.Length) + BR2
            powershell += ".. literalinclude:: " + "powershell/" + i.FileName + BR + "   :language: powershell" + BR2
        Next

        powershell.WriteFileUTF8(Folder.Startup + "..\docs\powershell.rst")

        Dim switches = "Command Line Interface
======================

Switches are processed in the order they appear in the command line.

The command line interface, the customizable main menu and Event Command features are built with a shared command system.

There is a special mode where only the MediaInfo window is shown using -mediainfo , this is useful for File Explorer integration with an app like Open++.


Examples
--------

StaxRip C:\\Movie\\project.srip

StaxRip C:\\Movie\\VTS_01_1.VOB C:\\Movie 2\\VTS_01_2.VOB

StaxRip -LoadTemplate:DVB C:\\Movie\\capture.mpg -StartEncoding -Standby

StaxRip -ShowMessageBox:""main text..."",""text ..."",info


Switches
--------

"

        Dim commands As New List(Of Command)(g.MainForm.CommandManager.Commands.Values)
        commands.Sort()

        Dim commandList As New StringPairList

        For Each command In commands
            Dim params = command.MethodInfo.GetParameters
            Dim switch = "-" + command.MethodInfo.Name + ":"

            For Each param In params
                switch += param.Name + ","
            Next

            switch = switch.TrimEnd(",:".ToCharArray)
            switches += switch + BR + "~".Multiply(switch.Length) + BR2

            For Each param In params
                Dim d = param.GetCustomAttribute(Of DescriptionAttribute)

                If Not d Is Nothing Then
                    switches += param.Name + ": " + param.GetCustomAttribute(Of DescriptionAttribute).Description + BR2
                End If
            Next

            Dim enumList As New List(Of String)

            For Each param In params
                If param.ParameterType.IsEnum Then
                    enumList.Add(param.ParameterType.Name + ": " +
                                 System.Enum.GetNames(param.ParameterType).Join(", "))
                End If
            Next

            For Each en In enumList
                switches += en + BR2
            Next

            switches += command.Attribute.Description + BR2 + BR
        Next

        switches.WriteFileUTF8(Folder.Startup + "..\docs\cli.rst")
    End Sub

    Shared Sub UpdateFile(filepath As String, content As String)
        Dim currentContent = filepath.ReadAllText

        If content <> currentContent Then
            content.WriteFileUTF8(filepath)
        End If
    End Sub
End Class
