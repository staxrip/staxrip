Imports StaxRip.UI
Imports System.Globalization
Imports System.Text

Namespace CommandLine
    Public MustInherit Class CommandLineParams
        Property Title As String

        Event ValueChanged(item As CommandLineParam)
        MustOverride ReadOnly Property Items As List(Of CommandLineParam)

        MustOverride Function GetCommandLine(includePaths As Boolean,
                                             includeExecutable As Boolean,
                                             Optional pass As Integer = 1) As String

        MustOverride Function GetPackage() As Package

        Sub Init(store As PrimitiveStore)
            For Each i In Items
                i.Init(store, Me)
            Next
        End Sub

        Sub RaiseValueChanged(item As CommandLineParam)
            OnValueChanged(item)
        End Sub

        Protected Overridable Sub OnValueChanged(item As CommandLineParam)
            For Each i In Items
                If Not i.VisibleFunc Is Nothing Then
                    i.Visible = i.Visible
                End If
            Next

            RaiseEvent ValueChanged(item)
        End Sub

        Sub Execute()
            Dim batchPath = p.TempDir + p.TargetFile.Base + "_vexe.bat"
            Dim batchCode = "@echo off" + BR + "CHCP 65001" + BR + GetCommandLine(True, True)
            File.WriteAllText(batchPath, batchCode, New UTF8Encoding(False))

            Dim batchProc As New Process
            batchProc.StartInfo.FileName = "cmd.exe"
            batchProc.StartInfo.Arguments = "/k """ + batchPath + """"
            batchProc.StartInfo.WorkingDirectory = p.TempDir
            batchProc.Start()
        End Sub
    End Class

    Public MustInherit Class CommandLineParam
        Property AlwaysOn As Boolean
        Property ArgsFunc As Func(Of String)
        Property Help As String
        Property LabelMargin As Padding
        Property Name As String
        Property NoSwitch As String
        Property Path As String
        Property Switch As String
        Property Switches As IEnumerable(Of String)
        Property Text As String
        Property URL As String
        Property VisibleFunc As Func(Of Boolean)
        Property ImportAction As Action(Of String)

        Friend Store As PrimitiveStore
        Friend Params As CommandLineParams

        MustOverride Sub Init(store As PrimitiveStore, params As CommandLineParams)
        MustOverride Function GetControl() As Control

        Overridable Function GetArgs() As String
        End Function

        Function GetSwitches() As HashSet(Of String)
            Dim ret As New HashSet(Of String)

            If Switch <> "" Then ret.Add(Switch)

            If Not Switches.NothingOrEmpty Then
                For Each i In Switches
                    ret.Add(i)
                Next
            End If

            Return ret
        End Function

        Property VisibleValue As Boolean = True

        Property Visible As Boolean
            Get
                If Not VisibleFunc Is Nothing Then Return VisibleFunc.Invoke
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
            If Name <> "" Then Return Name
            If Switch <> "" Then Return Switch
            If Text <> "" Then Return Text
        End Function
    End Class

    Public Class BoolParam
        Inherits CommandLineParam

        Property DefaultValue As Boolean
        Property CheckBox As CheckBox

        Public Overloads Overrides Sub Init(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params

            If Not store.Bool.ContainsKey(GetKey) Then
                store.Bool(GetKey) = ValueValue
            End If
        End Sub

        Overloads Sub Init(cb As CheckBox)
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
            If Switch = "" AndAlso NoSwitch = "" AndAlso
                ArgsFunc Is Nothing Then Return Nothing

            If Not Visible Then Return Nothing

            If ArgsFunc Is Nothing Then
                If Value AndAlso DefaultValue = False Then
                    Return Switch
                ElseIf Not Value AndAlso DefaultValue Then
                    Return NoSwitch
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
                If Not Store Is Nothing Then Store.Bool(GetKey) = value
                If Not CheckBox Is Nothing Then CheckBox.Checked = value
            End Set
        End Property

        WriteOnly Property InitValue As Boolean
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
        Property DefaultValue As Single

        Private MinMaxStepDecValue As Decimal()

        Property MinMaxStepDec As Decimal()
            Get
                If MinMaxStepDecValue Is Nothing Then
                    Return {Decimal.MinValue, Decimal.MaxValue, 1, 0}
                End If

                Return MinMaxStepDecValue
            End Get
            Set(value As Decimal())
                MinMaxStepDecValue = value
            End Set
        End Property

        WriteOnly Property MinMaxStep As Integer()
            Set(value As Integer())
                MinMaxStepDecValue = {value(0), value(1), value(2), 0}
            End Set
        End Property

        Overloads Sub Init(ne As NumEdit)
            NumEdit = ne
            NumEdit.Value = CDec(Value)
            AddHandler NumEdit.ValueChanged, AddressOf ValueChanged
            AddHandler NumEdit.Disposed, Sub()
                                             RemoveHandler NumEdit.ValueChanged, AddressOf ValueChanged
                                             NumEdit = Nothing
                                         End Sub
        End Sub

        Public Overloads Overrides Sub Init(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params

            If Not store.Sng.ContainsKey(GetKey) Then
                store.Sng(GetKey) = ValueValue
            End If
        End Sub

        Sub ValueChanged(ne As NumEdit)
            If MinMaxStepDec(3) = 0 Then
                Value = CInt(ne.Value)
            Else
                Value = ne.Value
            End If

            Params.RaiseValueChanged(Me)
        End Sub

        Private ValueValue As Single

        Property Value As Single
            Get
                Return Store.Sng(GetKey)
            End Get
            Set(value As Single)
                ValueValue = value
                If Not Store Is Nothing Then Store.Sng(GetKey) = value
                If Not NumEdit Is Nothing Then NumEdit.Value = CDec(value)
            End Set
        End Property

        WriteOnly Property InitValue As Single
            Set(value As Single)
                Me.Value = value
                DefaultValue = value
            End Set
        End Property

        Overrides Function GetArgs() As String
            If Not Visible Then Return Nothing
            If Switch = "" AndAlso ArgsFunc Is Nothing Then Return Nothing

            If ArgsFunc Is Nothing Then
                If Value <> DefaultValue OrElse AlwaysOn Then
                    Return Switch + " " + Value.ToString(CultureInfo.InvariantCulture)
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

        Overloads Sub Init(mb As MenuButton)
            MenuButton = mb
            MenuButton.Value = Value
            AddHandler MenuButton.ValueChangedUser, AddressOf ValueChangedUser
            AddHandler MenuButton.Disposed, Sub()
                                                RemoveHandler MenuButton.ValueChangedUser, AddressOf ValueChangedUser
                                                MenuButton = Nothing
                                            End Sub
        End Sub

        Sub ShowOption(value As Integer, visible As Boolean)
            If Not MenuButton Is Nothing Then
                For Each i In MenuButton.Menu.Items.OfType(Of ToolStripMenuItem)
                    For Each i2 In Values
                        If value.Equals(i.Tag) Then i.Visible = visible
                    Next
                Next
            End If
        End Sub

        Public Overloads Overrides Sub Init(store As PrimitiveStore, params As CommandLineParams)
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
                Return Values(Value)
            End Get
        End Property

        Sub ValueChangedUser(obj As Object)
            Value = CInt(obj)
            Params.RaiseValueChanged(Me)
        End Sub

        Private ValueValue As Integer

        Property Value As Integer
            Get
                Return Store.Int(GetKey)
            End Get
            Set(value As Integer)
                ValueValue = value
                If Not Store Is Nothing Then Store.Int(GetKey) = value
                If Not MenuButton Is Nothing Then MenuButton.Value = ValueValue
            End Set
        End Property

        WriteOnly Property InitValue As Integer
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
                            Return Switch + " " & Values(Value)
                        End If
                    ElseIf Switch <> "" Then
                        If IntegerValue Then
                            Return Switch + " " & Value
                        Else
                            Return Switch + " " & Options(Value)
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
        Property Quotes As Boolean
        Property InitAction As Action(Of SimpleUI.TextBlock)
        Property BrowseFileFilter As String
        Property BrowseFolderText As String

        Public Overloads Overrides Sub Init(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params

            If Not store.String.ContainsKey(GetKey) Then
                store.String(GetKey) = ValueValue
            End If
        End Sub

        Overloads Sub Init(te As SimpleUI.TextBlock)
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
            If Not Visible Then Return Nothing

            If Not ArgsFunc Is Nothing Then
                Return ArgsFunc.Invoke
            Else
                If Value <> DefaultValue Then
                    If Switch = "" Then
                        If Quotes Then
                            Return """" + Value + """"
                        Else
                            Return Value
                        End If
                    Else
                        If Quotes Then
                            Return Switch + " """ + Value + """"
                        Else
                            Return Switch + " " + Value
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

        Public Overrides Function GetControl() As Control
            Return TextEdit
        End Function
    End Class
End Namespace