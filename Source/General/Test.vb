
Imports System.Text.RegularExpressions

Public Class ConsolAppTester
    Property IgnoredSwitches As String = ""
    Property UndocumentedSwitches As String = ""
    Property Package As Package
    Property CodeFile As String

    Function Test() As String
        Dim log = BR
        log += "ignored switches" + BR
        log += "----------------" + BR
        log += "found in the documentation but not implemented" + BR2
        IgnoredSwitches = IgnoredSwitches.Trim.FixBreak.Replace(BR, " ").ReplaceRecursive("  ", " ")
        Dim ignore = IgnoredSwitches.Split(" "c).Select(Function(x) "--" + x).ToArray
        log += ignore.Join(BR) + BR2
        log += "undocumented switches" + BR
        log += "---------------------" + BR
        log += "implemented even though not documented" + BR2
        UndocumentedSwitches = UndocumentedSwitches.Trim.FixBreak.Replace(BR, " ").ReplaceRecursive("  ", " ")
        Dim undocumented = UndocumentedSwitches.Split(" "c).Select(Function(x) "--" + x).ToArray
        log += undocumented.Join(BR) + BR2
        log += "documented switches" + BR
        log += "-------------------" + BR
        log += "found in the documentation" + BR2
        Dim fullHelp = Package.CreateHelpfile()
        Dim compressedHelp = fullHelp.Replace("--(no-)", "--").Replace("--[no-]", "--").Replace("--no-", "--")
        Dim documented = Regex.Matches(compressedHelp, "--[\w-]+").OfType(Of Match).Select(Function(x) x.Value)
        log += documented.Join(BR) + BR2
        Dim compressedCode = CodeFile.ReadAllText.Replace("--no-", "--")
        Dim implemented = Regex.Matches(compressedCode, "--[\w-]+").OfType(Of Match).Select(Function(x) x.Value)
        log += "implemented switches" + BR
        log += "-------------------" + BR
        log += "implemetation found in StaxRip code" + BR2
        log += implemented.Join(BR) + BR2
        Dim missing = implemented.Where(Function(x) Not documented.Contains(x) AndAlso Not undocumented.Contains(x))
        Dim unknown = documented.Where(Function(x) Not implemented.Contains(x) AndAlso Not ignore.Contains(x)).ToList()
        unknown.Sort()
        Dim unnecessaryIgnore = ignore.Where(Function(x) implemented.Contains(x))
        log += "full documentation" + BR
        log += "------------------" + BR
        log += fullHelp
        'log.WriteUTF8File(Folder.Desktop + Package.Name + ".txt")

        Dim message As String

        If unnecessaryIgnore.Count > 0 Then
            message += BR3 + $"unnecessary on {Package.Name} ignore list:" + BR2 + unnecessaryIgnore.Join(" ")
        End If

        If missing.Count > 0 Then
            message += BR3 + $"removed from {Package.Name}:" + BR2 + missing.Join(" ")
        End If

        If unknown.Count > 0 Then
            message += BR3 + $"{Package.Name} todo:" + BR2 + unknown.Join(" ")
        End If

        Return message
    End Function
End Class
