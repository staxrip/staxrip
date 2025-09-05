﻿
Imports System.Text
Imports StaxRip.SimpleUI

Imports StaxRip.UI

Namespace VideoEncoderCommandLine
    Public MustInherit Class CommandLineParams
        Property Title As String
        Property Separator As String = " "

        Event ValueChanged(item As CommandLineParam)
        MustOverride ReadOnly Property Items As List(Of CommandLineParam)

        MustOverride Function GetCommandLine(
            includePaths As Boolean,
            includeExecutable As Boolean,
            Optional pass As Integer = 1) As String

        MustOverride ReadOnly Property Package As Package

        Overridable Function GetCommandLinePreview() As String
            Return GetCommandLine(True, True)
        End Function

        Sub Init(store As PrimitiveStore)
            For Each i In Items
                i.InitParam(store, Me)
            Next
        End Sub

        Protected ItemsValue As List(Of CommandLineParam)

        Protected Sub Add(path As String, ParamArray items As CommandLineParam())
            For Each i In items
                i.WasInitialValueSet = True
                i.Path = path

                If i.HelpSwitch = "" Then
                    Dim switches = i.GetSwitches

                    If Not switches.NothingOrEmpty Then
                        i.HelpSwitch = switches(0)
                    End If
                End If

                ItemsValue.Add(i)
            Next
        End Sub

        Function GetStringParam(switch As String) As StringParam
            Return Items.OfType(Of StringParam).Where(Function(item) item.Switch = switch).FirstOrDefault
        End Function

        Function GetOptionParam(switch As String) As OptionParam
            Return Items.OfType(Of OptionParam).Where(Function(item) item.Switch = switch).FirstOrDefault
        End Function

        Function GetNumParam(switch As String) As NumParam
            Return Items.OfType(Of NumParam).Where(Function(item) item.Switch = switch).FirstOrDefault
        End Function

        Function GetNumParamByName(name As String) As NumParam
            Return Items.OfType(Of NumParam).Where(Function(item) item.Name = name).FirstOrDefault
        End Function

        Sub RaiseValueChanged(item As CommandLineParam)
            OnValueChanged(item)
        End Sub

        Overridable Sub ShowHelp(options As String())
        End Sub

        Shared Sub ShowConsoleHelp(package As Package, options As String())
            Dim opt = GetHelpOption(options)

            If opt = "" Then
                Exit Sub
            End If

            Dim content = package.CreateHelpfile()
            Dim find = ""

            If content.Contains(opt.Replace("--", "--(no-)") + " ") Then
                find = opt.Replace("--", "--(no-)") + " "
            ElseIf content.Contains(opt.Replace("--", "--(no-)")) Then
                find = opt.Replace("--", "--(no-)")
            ElseIf content.Contains(opt.Replace("--", "--[no-]") + " ") Then
                find = opt.Replace("--", "--[no-]") + " "
            ElseIf content.Contains(opt.Replace("--", "--[no-]")) Then
                find = opt.Replace("--", "--[no-]")
            ElseIf content.Contains(", " + opt + " ") Then
                find = ", " + opt + " "
            ElseIf content.Contains(",  " + opt + " ") Then
                find = ",  " + opt + " "
            ElseIf content.Contains("  " + opt + " ") Then
                find = "  " + opt + " "
            ElseIf content.Contains(opt + " ") Then
                find = opt + " "
            ElseIf content.Contains(opt) Then
                find = opt
            End If

            If find = "" Then
                MsgError($"{opt} not found", "This option is either undocumented or deprecated.")
                Exit Sub
            End If

            g.ShowCode(package.Name + " Help", content, find)
        End Sub

        Shared Function GetHelpOption(options As String()) As String
            If options.NothingOrEmpty Then
                Return Nothing
            End If

            options = options.Where(Function(i) i.Length > 3).ToArray

            If options.Length = 1 Then
                Return options(0)
            ElseIf options.Length > 1 Then
                Using td As New TaskDialog(Of String)
                    td.Title = "Choose option"

                    For Each i In options
                        td.AddCommand(i)
                    Next

                    Return td.Show()
                End Using
            End If
        End Function

        Protected Overridable Sub OnValueChanged(item As CommandLineParam)
            For Each i In Items
                If i.VisibleFunc IsNot Nothing Then
                    i.Visible = i.VisibleFunc.Invoke()
                End If
            Next

            RaiseEvent ValueChanged(item)
        End Sub

        Function GetSAR() As String
            Dim param = GetStringParam("--sar")

            If param?.Value <> "" Then
                If param.Value.Trim.EqualsAny("1:1", "1/1", "1") Then
                    Return "--sar 1:1"
                End If

                Dim targetPAR = Calc.GetTargetPAR
                Dim val = Calc.ParseCustomAR(param.Value, targetPAR.X, targetPAR.Y)

                If param?.Value = "auto" AndAlso val = New Point(1, 1) AndAlso
                    (p.CustomTargetPAR.TrimEx.EqualsAny("1:1", "1/1", "1") OrElse
                    p.CustomTargetPAR = "force" OrElse p.CustomSourcePAR = "force") Then

                    Return "--sar 1:1"
                End If

                If param?.Value = "force" AndAlso val = New Point(1, 1) Then
                    Return "--sar 1:1"
                End If

                Dim isInTolerance = val = targetPAR AndAlso Not Calc.IsARSignalingRequired

                If val.X <> 0 AndAlso val <> New Point(1, 1) AndAlso Not isInTolerance Then
                    Return "--sar " & val.X & ":" & val.Y
                End If
            End If
        End Function

        Sub Execute()
            If Not g.VerifyRequirements Then
                Exit Sub
            End If

            FrameServerHelp.AviSynthToolPath()

            Dim cl = GetCommandLine(True, True)

            p.Script.Synchronize()

            If s.PreferWindowsTerminal AndAlso g.IsWindowsTerminalAvailable Then
                cl = "cmd.exe /S /K --% """ + cl + """"
                Dim base64 = Convert.ToBase64String(Encoding.Unicode.GetBytes(cl)) 'UTF16LE
                g.Execute("wt.exe", "powershell.exe -NoLogo -NoExit -NoProfile -EncodedCommand """ + base64 + """")
            Else
                g.Execute("cmd.exe", "/S /K """ + cl + """")
            End If
        End Sub
    End Class

    Public MustInherit Class CommandLineParam
        Property AlwaysOn As Boolean
        Property ArgsFunc As Func(Of String)
        Property Help As String
        Property HelpSwitch As String
        Property HintText As String
        Property ImportAction As Action(Of String, String)
        Property WasInitialValueSet As Boolean
        Property Label As String
        Property LeftMargin As Double
        Property Name As String
        Property NoSwitch As String
        Property Path As String
        Property Switch As String
        Property Switches As IEnumerable(Of String)
        Property Text As String
        Property URLs As List(Of String)
        Property VisibleFunc As Func(Of Boolean)
        Property Weight As Integer

        Friend Store As PrimitiveStore
        Friend Params As CommandLineParams

        MustOverride Sub InitParam(store As PrimitiveStore, params As CommandLineParams)
        MustOverride Function GetControl() As Control

        Overridable Function GetArgs() As String
        End Function

        Function GetSwitches() As String()
            Dim ret As New HashSet(Of String)

            If Switch <> "" Then ret.Add(Switch)
            If NoSwitch <> "" Then ret.Add(NoSwitch)
            If HelpSwitch <> "" Then ret.Add(HelpSwitch)

            If Not Switches.NothingOrEmpty Then
                For Each i In Switches
                    If i <> "" Then
                        ret.Add(i)
                    End If
                Next
            End If

            Return ret.ToArray
        End Function

        Property VisibleValue As Boolean = True

        Property Visible As Boolean
            Get
                If Not VisibleFunc Is Nothing Then
                    Return VisibleFunc.Invoke
                End If

                Return VisibleValue
            End Get
            Set(value As Boolean)
                If value <> VisibleValue Then
                    VisibleValue = value

                    Dim c = GetControl()

                    If Not c Is Nothing Then
                        If TypeOf c.Parent Is SimpleUI.EmptyBlock Then
                            c = c.Parent
                        End If

                        c.Visible = value
                    End If
                End If
            End Set
        End Property

        Function GetKey() As String
            If Name <> "" Then
                Return Name
            End If

            If Switch <> "" Then
                Return Switch
            End If

            If HelpSwitch <> "" Then
                Return Text + HelpSwitch
            End If

            Return Text
        End Function
    End Class

    Public Class LineParam
        Inherits CommandLineParam

        Property Line As LineControl


        Public Overrides Sub InitParam(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params
        End Sub

        Public Overloads Sub InitParam(line As SimpleUILineControl)
            Me.Line = line
        End Sub

        Public Overrides Function GetControl() As Control
            Return Line
        End Function
    End Class

    Public Class BoolParam
        Inherits CommandLineParam

        Property CheckBox As CheckBoxEx
        Property DefaultValue As Boolean
        Property InitialValue As Boolean
        Property IntegerValue As Boolean
        Property ValueChangedAction As Action(Of Boolean)

        ReadOnly Property IsDefaultValue As Boolean
            Get
                Return Value = DefaultValue
            End Get
        End Property

        Sub SetDefaultValue()
            Value = DefaultValue
        End Sub

        Overrides Sub InitParam(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params
        End Sub

        Overloads Sub InitParam(cb As CheckBoxEx)
            CheckBox = cb
            CheckBox.Checked = Value
            AddHandler CheckBox.CheckedChanged, AddressOf CheckedChanged
            AddHandler CheckBox.Disposed, Sub()
                                              RemoveHandler CheckBox.CheckedChanged, AddressOf CheckedChanged
                                              CheckBox = Nothing
                                          End Sub
        End Sub

        Sub CheckedChanged(sender As Object, e As EventArgs)
            Value = CheckBox.Checked
            Params.RaiseValueChanged(Me)
            ValueChangedAction?.Invoke(Value)
        End Sub

        Overrides Function GetArgs() As String
            If Switch = "" AndAlso NoSwitch = "" AndAlso ArgsFunc Is Nothing Then
                Return Nothing
            End If

            If Not Visible Then
                Return Nothing
            End If

            If ArgsFunc Is Nothing Then
                If Value AndAlso DefaultValue = False Then
                    If IntegerValue Then
                        Return Switch + Params.Separator + "1"
                    Else
                        Return Switch
                    End If
                ElseIf Not Value AndAlso DefaultValue Then
                    If IntegerValue Then
                        Return Switch + Params.Separator + "0"
                    Else
                        Return NoSwitch
                    End If
                End If
            Else
                Return ArgsFunc.Invoke()?.Trim()
            End If
        End Function

        Private ValueValue As Boolean

        Property Value As Boolean
            Get
                If Store.Bool.ContainsKey(GetKey) Then
                    Return Store.Bool(GetKey)
                Else
                    Return InitialValue
                End If
            End Get
            Set(value As Boolean)
                If Not WasInitialValueSet Then
                    InitialValue = value
                    WasInitialValueSet = True
                End If

                ValueValue = value

                If Not Store Is Nothing Then
                    If value = InitialValue Then
                        If Store.Bool.ContainsKey(GetKey) Then
                            Store.Bool.Remove(GetKey)
                        End If
                    Else
                        Store.Bool(GetKey) = value
                    End If
                End If

                If Not CheckBox Is Nothing Then
                    CheckBox.Checked = value
                End If
            End Set
        End Property

        WriteOnly Property Init As Boolean
            Set(value As Boolean)
                Me.Value = value
                DefaultValue = value
            End Set
        End Property

        Public Overrides Function GetControl() As Control
            Return CheckBox
        End Function
    End Class

    Public Class NumParam
        Inherits CommandLineParam

        Property DefaultValue As Double
        Property InitialValue As Double
        Property NumEdit As NumEdit
        Property ValueChangedAction As Action(Of Double)

        Private ConfigValue As Double()

        Property Config As Double() 'min, max, step, decimal places
            Get
                If ConfigValue Is Nothing Then
                    Return {Double.MinValue, Double.MaxValue, 1, 0}
                End If

                Return ConfigValue
            End Get
            Set(value As Double())
                ConfigValue = {value(0), value(1), 1, 0}

                If value.Length > 2 Then ConfigValue(2) = value(2)
                If value.Length > 3 Then ConfigValue(3) = value(3)

                If ConfigValue(0) = 0 AndAlso ConfigValue(1) = 0 Then
                    ConfigValue(0) = Double.MinValue
                    ConfigValue(1) = Double.MaxValue
                End If
            End Set
        End Property

        ReadOnly Property IsDefaultValue As Boolean
            Get
                Return Value = DefaultValue
            End Get
        End Property

        Sub SetDefaultValue()
            Value = DefaultValue
        End Sub

        Overloads Sub InitParam(ne As NumEdit)
            NumEdit = ne
            NumEdit.Value = Value
            AddHandler NumEdit.ValueChanged, AddressOf ValueChanged
            AddHandler NumEdit.Disposed, Sub()
                                             RemoveHandler NumEdit.ValueChanged, AddressOf ValueChanged
                                             NumEdit = Nothing
                                         End Sub
        End Sub

        Overrides Sub InitParam(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params
        End Sub

        Sub ValueChanged(Optional ne As NumEdit = Nothing)
            If ne IsNot Nothing Then
                Value = If(Config(3) = 0, CInt(ne.Value), ne.Value)
            End If

            Params.RaiseValueChanged(Me)
            ValueChangedAction?.Invoke(Value)
        End Sub

        Private ValueValue As Double

        Property Value As Double
            Get
                If Store.Double.ContainsKey(GetKey) Then
                    Return Store.Double(GetKey)
                Else
                    Return InitialValue
                End If
            End Get
            Set(value As Double)
                If Not WasInitialValueSet Then
                    InitialValue = value
                    WasInitialValueSet = True
                End If

                ValueValue = value

                If Not Store Is Nothing Then
                    If value = InitialValue Then
                        If Store.Double.ContainsKey(GetKey) Then
                            Store.Double.Remove(GetKey)
                        End If
                    Else
                        Store.Double(GetKey) = value
                    End If
                End If

                If Not NumEdit Is Nothing Then
                    NumEdit.Value = value
                End If
            End Set
        End Property

        WriteOnly Property Init As Double
            Set(value As Double)
                Me.Value = value
                DefaultValue = value
            End Set
        End Property

        Overrides Function GetArgs() As String
            If Not Visible Then Return Nothing
            If Switch = "" AndAlso ArgsFunc Is Nothing Then Return Nothing

            If ArgsFunc Is Nothing Then
                If Value <> DefaultValue OrElse AlwaysOn Then
                    Return Switch + Params.Separator + Value.ToInvariantString
                End If
            Else
                Return ArgsFunc.Invoke()
            End If
        End Function

        Public Overrides Function GetControl() As Control
            Return NumEdit
        End Function
    End Class

    Public Class OptionParam
        Inherits CommandLineParam

        Property DefaultValue As Integer
        Property InitialValue As Integer
        Property IntegerValue As Boolean
        Property MenuButton As MenuButton
        Property Options As String()
        Property Values As String()
        Property ValueChangedAction As Action(Of Integer)

        ReadOnly Property IsDefaultValue As Boolean
            Get
                Return Value = DefaultValue
            End Get
        End Property

        Sub SetDefaultValue()
            Value = DefaultValue
        End Sub

        Sub ShowOption(value As Integer, visible As Boolean)
            If MenuButton IsNot Nothing Then
                For Each i In MenuButton.Menu.Items.OfType(Of ToolStripMenuItem)
                    If value.Equals(i.Tag) Then i.Visible = visible
                Next
            End If
        End Sub

        Overloads Sub InitParam(mb As MenuButton)
            MenuButton = mb
            MenuButton.Value = Value
            AddHandler MenuButton.ValueChangedUser, AddressOf ValueChangedUser
            AddHandler MenuButton.Disposed, Sub()
                                                RemoveHandler MenuButton.ValueChangedUser, AddressOf ValueChangedUser
                                                MenuButton = Nothing
                                            End Sub
        End Sub

        Overrides Sub InitParam(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params
        End Sub

        Property Expanded As Boolean

        ReadOnly Property OptionText As String
            Get
                Return Options(Value)
            End Get
        End Property

        ReadOnly Property ValueText As String
            Get
                If Values Is Nothing Then
                    Return Options(Value).ToLowerInvariant.Replace(" ", "")
                Else
                    Return Values(Value)
                End If
            End Get
        End Property

        Sub ValueChangedUser(obj As Object)
            Value = CInt(obj)
            Params.RaiseValueChanged(Me)
            ValueChangedAction?.Invoke(Value)
        End Sub

        Private ValueValue As Integer

        Property Value As Integer
            Get
                If Store.Int.ContainsKey(GetKey) Then
                    Dim ret = Store.Int(GetKey)

                    If ret > Options.Length - 1 Then
                        ret = Options.Length - 1
                    End If

                    Return ret
                Else
                    Return InitialValue
                End If
            End Get
            Set(value As Integer)
                If Not WasInitialValueSet Then
                    InitialValue = value
                    WasInitialValueSet = True
                End If

                ValueValue = value

                If Not Store Is Nothing Then
                    If value = InitialValue Then
                        If Store.Int.ContainsKey(GetKey) Then
                            Store.Int.Remove(GetKey)
                        End If
                    Else
                        Store.Int(GetKey) = value
                    End If
                End If

                If Not MenuButton Is Nothing Then
                    MenuButton.Value = ValueValue
                End If
            End Set
        End Property

        WriteOnly Property Init As Integer
            Set(value As Integer)
                Me.Value = value
                DefaultValue = value
            End Set
        End Property

        Overrides Function GetArgs() As String
            If Not Visible Then Return Nothing

            If ArgsFunc Is Nothing Then
                If Value <> DefaultValue OrElse AlwaysOn Then
                    If Not Values Is Nothing Then
                        If Values(Value).StartsWith("--") Then
                            Return Values(Value)
                        ElseIf Switch <> "" Then
                            Dim v = Values(Value)
                            Return Switch + If(String.IsNullOrWhiteSpace(v), "", Params.Separator & v)
                        End If
                    ElseIf Switch <> "" Then
                        If IntegerValue Then
                            Return Switch + Params.Separator & Value
                        Else
                            Dim v = Options(Value).ToLowerInvariant.Replace(" ", "")
                            Return Switch + If(String.IsNullOrWhiteSpace(v), "", Params.Separator & v)
                        End If
                    End If
                End If
            Else
                Return ArgsFunc.Invoke
            End If
        End Function

        Public Overrides Function GetControl() As Control
            Return MenuButton
        End Function
    End Class

    Public Class StringParam
        Inherits CommandLineParam

        Property BrowseFileFilter As String
        Property BrowseFolderText As String
        Property DefaultValue As String
        Property Expand As Boolean = True
        Property InitAction As Action(Of SimpleUI.TextBlock)
        Property InitialValue As String
        Property Menu As String
        Property Quotes As QuotesMode
        Property RemoveSpace As Boolean
        Property TextEdit As TextEdit
        Property TextChangedAction As Action(Of String)

        WriteOnly Property BrowseFile As Boolean
            Set(value As Boolean)
                BrowseFileFilter = "*.*|*.*"
            End Set
        End Property

        ReadOnly Property IsDefaultValue As Boolean
            Get
                Return Value = DefaultValue
            End Get
        End Property

        Sub SetDefaultValue()
            Value = DefaultValue
        End Sub

        Overrides Sub InitParam(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params
        End Sub

        Overloads Sub InitParam(te As SimpleUI.TextBlock)
            TextEdit = te.Edit
            TextEdit.Text = Value
            AddHandler TextEdit.TextBox.TextChanged, AddressOf TextChanged
            AddHandler TextEdit.Disposed, Sub()
                                              If Not TextEdit Is Nothing Then
                                                  RemoveHandler TextEdit.TextChanged, AddressOf TextChanged
                                                  TextEdit = Nothing
                                              End If
                                          End Sub

            If Not InitAction Is Nothing Then InitAction.Invoke(te)
        End Sub

        Sub TextChanged(sender As Object, e As EventArgs)
            Value = TextEdit.Text
            Params.RaiseValueChanged(Me)
            TextChangedAction?.Invoke(Value)
        End Sub

        Overrides Function GetArgs() As String
            If Not Visible Then
                Return Nothing
            End If

            If Not ArgsFunc Is Nothing Then
                Return ArgsFunc.Invoke
            Else
                Dim val = Value

                If RemoveSpace AndAlso val?.Contains(" ") Then
                    val = val.Replace(" ", "")
                End If

                If val <> DefaultValue AndAlso val <> "" Then
                    If Switch = "" Then
                        If AlwaysOn Then
                            If Quotes = QuotesMode.Always Then
                                Return """" + val + """"
                            ElseIf Quotes = QuotesMode.Auto Then
                                Return val.Escape
                            Else
                                Return val
                            End If
                        End If
                    Else
                        If Quotes = QuotesMode.Always Then
                            Return Switch + Params.Separator + """" + val + """"
                        ElseIf Quotes = QuotesMode.Auto Then
                            Return Switch + Params.Separator + val.Escape
                        Else
                            Return Switch + Params.Separator + val
                        End If
                    End If
                End If
            End If
        End Function

        Private ValueValue As String

        Property Value As String
            Get
                If Store.String.ContainsKey(GetKey) Then
                    Return Store.String(GetKey)
                Else
                    Return InitialValue
                End If
            End Get
            Set(value As String)
                If Not WasInitialValueSet Then
                    InitialValue = value
                    WasInitialValueSet = True
                End If

                ValueValue = value

                If Not Store Is Nothing Then
                    If value = InitialValue Then
                        If Store.String.ContainsKey(GetKey) Then
                            Store.String.Remove(GetKey)
                        End If
                    Else
                        Store.String(GetKey) = value
                    End If
                End If

                If Not TextEdit Is Nothing Then
                    TextEdit.Text = value
                End If
            End Set
        End Property

        WriteOnly Property Init As String
            Set(value As String)
                Me.Value = value
                DefaultValue = value
            End Set
        End Property

        Public Overrides Function GetControl() As Control
            Return TextEdit
        End Function
    End Class
End Namespace