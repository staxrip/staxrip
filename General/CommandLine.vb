
Imports System.Text
Imports StaxRip.UI

Namespace CommandLine
    Public MustInherit Class CommandLineParams
        Property Title As String
        Property Separator As String = " "

        Event ValueChanged(item As CommandLineParam)
        MustOverride ReadOnly Property Items As List(Of CommandLineParam)

        MustOverride Function GetCommandLine(
            includePaths As Boolean,
            includeExecutable As Boolean,
            Optional pass As Integer = 1) As String

        MustOverride Function GetPackage() As Package

        Sub Init(store As PrimitiveStore)
            For Each i In Items
                i.InitParam(store, Me)
            Next
        End Sub

        Protected ItemsValue As List(Of CommandLineParam)

        Protected Sub Add(path As String, ParamArray items As CommandLineParam())
            For Each i In items
                i.Path = path
                ItemsValue.Add(i)
            Next
        End Sub

        Function GetStringParam(switch As String) As StringParam
            Return Items.OfType(Of StringParam).Where(Function(item) item.Switch = switch).FirstOrDefault
        End Function

        Function GetOptionParam(switch As String) As OptionParam
            Return Items.OfType(Of OptionParam).Where(Function(item) item.Switch = switch).FirstOrDefault
        End Function

        Function GetNumParamByName(name As String) As NumParam
            Return Items.OfType(Of NumParam).Where(Function(item) item.Name = name).FirstOrDefault
        End Function

        Sub RaiseValueChanged(item As CommandLineParam)
            OnValueChanged(item)
        End Sub

        Overridable Sub ShowHelp(id As String)
        End Sub

        Protected Overridable Sub OnValueChanged(item As CommandLineParam)
            For Each i In Items
                If Not i.VisibleFunc Is Nothing Then
                    i.Visible = i.Visible
                End If
            Next

            RaiseEvent ValueChanged(item)
        End Sub

        Function GetSAR() As String
            Dim param = GetStringParam("--sar")

            If Not param Is Nothing AndAlso param.Value <> "" Then
                Dim targetPAR = Calc.GetTargetPAR
                Dim val = Calc.ParseCustomAR(param.Value, targetPAR.X, targetPAR.Y)
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

            p.Script.Synchronize()

            If g.IsWindowsTerminalAvailable Then
                Dim cl = "cmd.exe /S /K --% """ + GetCommandLine(True, True) + """"
                Dim base64 = Convert.ToBase64String(Encoding.Unicode.GetBytes(cl)) 'UTF16LE
                g.Execute("wt.exe", "powershell.exe -NoLogo -NoExit -NoProfile -EncodedCommand """ + base64 + """")
            Else
                g.Execute("cmd.exe", "/S /K """ + GetCommandLine(True, True) + """")
            End If
        End Sub
    End Class

    Public MustInherit Class CommandLineParam
        Property AlwaysOn As Boolean
        Property ArgsFunc As Func(Of String)
        Property Help As String
        Property HelpSwitch As String
        Property ImportAction As Action(Of String, String)
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

        Friend Store As PrimitiveStore
        Friend Params As CommandLineParams

        MustOverride Sub InitParam(store As PrimitiveStore, params As CommandLineParams)
        MustOverride Function GetControl() As Control

        Overridable Function GetArgs() As String
        End Function

        Function GetSwitches() As HashSet(Of String)
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

            Return ret
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

    Public Class BoolParam
        Inherits CommandLineParam

        Property DefaultValue As Boolean
        Property CheckBox As CheckBox
        Property IntegerValue As Boolean

        Overrides Sub InitParam(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params

            If Not store.Bool.ContainsKey(GetKey) Then
                store.Bool(GetKey) = ValueValue
            End If
        End Sub

        Overloads Sub InitParam(cb As CheckBox)
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
                Return ArgsFunc.Invoke()
            End If
        End Function

        Private ValueValue As Boolean

        Property Value As Boolean
            Get
                Return Store.Bool(GetKey)
            End Get
            Set(value As Boolean)
                ValueValue = value

                If Not Store Is Nothing Then
                    Store.Bool(GetKey) = value
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

        Property NumEdit As NumEdit
        Property DefaultValue As Double

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

        Overloads Sub InitParam(ne As NumEdit)
            NumEdit = ne
            NumEdit.Value = Value
            AddHandler NumEdit.ValueChanged, AddressOf ValueChanged
            AddHandler NumEdit.Disposed, Sub()
                                             RemoveHandler NumEdit.ValueChanged, AddressOf ValueChanged
                                             NumEdit = Nothing
                                         End Sub
        End Sub

        Overloads Overrides Sub InitParam(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params

            If Not store.Double.ContainsKey(GetKey) Then
                store.Double(GetKey) = ValueValue
            End If
        End Sub

        Sub ValueChanged(ne As NumEdit)
            If Config(3) = 0 Then
                Value = CInt(ne.Value)
            Else
                Value = ne.Value
            End If

            Params.RaiseValueChanged(Me)
        End Sub

        Private ValueValue As Double

        Property Value As Double
            Get
                Return Store.Double(GetKey)
            End Get
            Set(value As Double)
                ValueValue = value
                If Not Store Is Nothing Then Store.Double(GetKey) = value
                If Not NumEdit Is Nothing Then NumEdit.Value = value
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

        Property Options As String()
        Property Values As String()
        Property Expand As Boolean
        Property MenuButton As MenuButton
        Property DefaultValue As Integer
        Property IntegerValue As Boolean

        Sub ShowOption(value As Integer, visible As Boolean)
            If Not MenuButton Is Nothing Then
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

        Public Overloads Overrides Sub InitParam(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params

            If Not store.Int.ContainsKey(GetKey) Then
                store.Int(GetKey) = ValueValue
            End If
        End Sub

        ReadOnly Property OptionText As String
            Get
                Return Options(Value)
            End Get
        End Property

        ReadOnly Property ValueText As String
            Get
                If Values Is Nothing Then
                    Return Options(Value).ToLowerInvariant
                Else
                    Return Values(Value)
                End If
            End Get
        End Property

        Sub ValueChangedUser(obj As Object)
            Value = CInt(obj)
            Params.RaiseValueChanged(Me)
        End Sub

        Private ValueValue As Integer

        Property Value As Integer
            Get
                Dim ret = Store.Int(GetKey)
                If ret > Options.Length - 1 Then ret = Options.Length - 1
                Return ret
            End Get
            Set(value As Integer)
                ValueValue = value
                If Not Store Is Nothing Then
                    Store.Int(GetKey) = value
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
                            Return Switch + Params.Separator & Values(Value)
                        End If
                    ElseIf Switch <> "" Then
                        If IntegerValue Then
                            Return Switch + Params.Separator & Value
                        Else
                            Return Switch + Params.Separator & Options(Value).ToLowerInvariant.Replace(" ", "")
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

        Property DefaultValue As String
        Property TextEdit As TextEdit
        Property Quotes As QuotesMode
        Property RemoveSpace As Boolean
        Property InitAction As Action(Of SimpleUI.TextBlock)
        Property BrowseFileFilter As String
        Property BrowseFolderText As String
        Property Menu As String
        Property Expand As Boolean = True

        WriteOnly Property BrowseFile As Boolean
            Set(value As Boolean)
                BrowseFileFilter = "*.*|*.*"
            End Set
        End Property

        Overloads Overrides Sub InitParam(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params

            If Not store.String.ContainsKey(GetKey) Then
                store.String(GetKey) = ValueValue
            End If
        End Sub

        Overloads Sub InitParam(te As SimpleUI.TextBlock)
            TextEdit = te.Edit
            TextEdit.Text = Value
            AddHandler TextEdit.TextChanged, AddressOf TextChanged
            AddHandler TextEdit.Disposed, Sub()
                                              If Not TextEdit Is Nothing Then
                                                  RemoveHandler TextEdit.TextChanged, AddressOf TextChanged
                                                  TextEdit = Nothing
                                              End If
                                          End Sub

            If Not InitAction Is Nothing Then InitAction.Invoke(te)
        End Sub

        Sub TextChanged()
            Value = TextEdit.Text
            Params.RaiseValueChanged(Me)
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
                Return Store.String(GetKey)
            End Get
            Set(value As String)
                ValueValue = value

                If Not Store Is Nothing Then
                    Store.String(GetKey) = value
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