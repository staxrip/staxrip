
Imports System.Text.RegularExpressions

Public Class ConsolAppTester
    Property IgnoredSwitches As String = ""
    Property UndocumentedSwitches As String = ""
    Property Package As Package
    Property HelpSwitch As String
    Property CodeFile As String

    Public Function Test() As String
        IgnoredSwitches = IgnoredSwitches.Trim.FixBreak.Replace(BR, " ").ReplaceRecursive("  ", " ")
        Dim ignore = IgnoredSwitches.Split(" "c).Select(Function(x) "--" + x).ToArray
        UndocumentedSwitches = UndocumentedSwitches.Trim.FixBreak.Replace(BR, " ").ReplaceRecursive("  ", " ")
        Dim undocumented = UndocumentedSwitches.Split(" "c).Select(Function(x) "--" + x).ToArray
        Dim help = Package.CreateHelpfile(HelpSwitch)
        help = help.Replace("--(no-)", "--").Replace("--[no-]", "--").Replace("--no-", "--")
        Dim switches = Regex.Matches(help, "--[\w-]+").OfType(Of Match).Select(Function(x) x.Value)
        Dim code = File.ReadAllText(CodeFile).Replace("--no-", "--")
        Dim present = Regex.Matches(code, "--[\w-]+").OfType(Of Match).Select(Function(x) x.Value)
        Dim missing = present.Where(Function(x) Not switches.Contains(x) AndAlso Not undocumented.Contains(x))
        Dim unknown = switches.Where(Function(x) Not present.Contains(x) AndAlso Not ignore.Contains(x)).ToList()
        unknown.Sort()
        Dim unnecessaryIgnore = ignore.Where(Function(x) present.Contains(x))
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
