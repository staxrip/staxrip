Imports StaxRip.UI
Imports System.Globalization

Namespace CommandLine
    MustInherit Class CommandLineParams
        Property Title As String
        Event ValueChanged(item As CommandLineItem)

        MustOverride ReadOnly Property Items As List(Of CommandLineItem)

        MustOverride Function GetArgs(includePaths As Boolean) As String
        MustOverride Function GetPackage() As Package

        Sub Init(store As PrimitiveStore)
            For Each i In Items
                i.Init(store, Me)
            Next
        End Sub

        Sub RaiseValueChanged(item As CommandLineItem)
            OnValueChanged(item)
        End Sub

        Protected Overridable Sub OnValueChanged(item As CommandLineItem)
            RaiseEvent ValueChanged(item)
        End Sub
    End Class

    MustInherit Class CommandLineItem
        Property Name As String
        Property Text As String
        Property Help As String
        Property URL As String
        Property Path As String
        Property Group As String 'TODO: remove?
        Property NoSwitch As String
        Property ArgsFunc As Func(Of String)
        Property LabelMargin As Padding
        Property Switch As String
        Property AlwaysOn As Boolean

        Friend Store As PrimitiveStore
        Friend Params As CommandLineParams

        MustOverride Sub Init(store As PrimitiveStore, params As CommandLineParams)
        MustOverride Function GetControl() As Control

        Overridable Function GetArgs() As String
        End Function

        Property VisibleValue As Boolean = True

        Property Visible As Boolean
            Get
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

    Class BoolParam
        Inherits CommandLineItem

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

                If Not Store Is Nothing Then
                    Store.Bool(GetKey) = value
                End If

                If Not CheckBox Is Nothing Then
                    CheckBox.Checked = value
                End If
            End Set
        End Property

        Public Overrides Function GetControl() As Control
            Return CheckBox
        End Function
    End Class

    Class NumParam
        Inherits CommandLineItem

        Property DefaultValue As Single
        Property NumEdit As NumEdit

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

                If Not Store Is Nothing Then
                    Store.Sng(GetKey) = value
                End If

                If Not NumEdit Is Nothing Then
                    NumEdit.Value = CDec(value)
                End If
            End Set
        End Property

        Overrides Function GetArgs() As String
            If Switch = "" OrElse Not Visible Then Return Nothing

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

    'TODO: remove?
    'Class CheckedNumParam
    '    Inherits CommandLineItem

    '    Property DefaultNumValue As Single
    '    Property DefaultBoolValue As Boolean

    '    Property NumEdit As NumEdit
    '    Property CheckBox As CheckBox

    '    Private MinMaxStepDecValue As Decimal()

    '    Property MinMaxStepDec As Decimal()
    '        Get
    '            If MinMaxStepDecValue Is Nothing Then
    '                Return {Decimal.MinValue, Decimal.MaxValue, 1, 0}
    '            End If

    '            Return MinMaxStepDecValue
    '        End Get
    '        Set(value As Decimal())
    '            MinMaxStepDecValue = value
    '        End Set
    '    End Property

    '    WriteOnly Property MinMaxStep As Integer()
    '        Set(value As Integer())
    '            MinMaxStepDecValue = {value(0), value(1), value(2), 0}
    '        End Set
    '    End Property

    '    Overloads Sub Init(cb As CheckBox, ne As NumEdit)
    '        NumEdit = ne
    '        NumEdit.Value = CDec(NumValue)
    '        AddHandler NumEdit.ValueChanged, AddressOf NumValueChanged
    '        AddHandler NumEdit.Disposed, Sub()
    '                                         RemoveHandler NumEdit.ValueChanged, AddressOf NumValueChanged
    '                                         NumEdit = Nothing
    '                                     End Sub
    '        CheckBox = cb
    '        CheckBox.Checked = BoolValue
    '        AddHandler CheckBox.CheckedChanged, AddressOf CheckedChanged
    '        AddHandler CheckBox.Disposed, Sub()
    '                                          RemoveHandler CheckBox.CheckedChanged, AddressOf CheckedChanged
    '                                          CheckBox = Nothing
    '                                      End Sub
    '    End Sub

    '    Public Overloads Overrides Sub Init(store As PrimitiveStore, params As CommandLineParams)
    '        Me.Store = store
    '        Me.Params = params

    '        If Not store.Sng.ContainsKey(GetKey) Then
    '            store.Sng(GetKey) = NumValueValue
    '        End If

    '        If Not store.Bool.ContainsKey(GetKey) Then
    '            store.Bool(GetKey) = BoolValueValue
    '        End If
    '    End Sub

    '    Sub CheckedChanged(sender As Object, e As EventArgs)
    '        BoolValue = CheckBox.Checked
    '        Params.RaiseValueChanged(Me)
    '    End Sub

    '    Sub NumValueChanged(ne As NumEdit)
    '        If MinMaxStepDec(3) = 0 Then
    '            NumValue = CInt(ne.Value)
    '        Else
    '            NumValue = ne.Value
    '        End If

    '        Params.RaiseValueChanged(Me)
    '    End Sub

    '    Private BoolValueValue As Boolean

    '    Property BoolValue As Boolean
    '        Get
    '            Return Store.Bool(GetKey)
    '        End Get
    '        Set(value As Boolean)
    '            BoolValueValue = value

    '            If Not Store Is Nothing Then
    '                Store.Bool(GetKey) = value
    '            End If

    '            If Not CheckBox Is Nothing Then
    '                CheckBox.Checked = value
    '            End If
    '        End Set
    '    End Property

    '    Private NumValueValue As Single

    '    Property NumValue As Single
    '        Get
    '            Return Store.Sng(GetKey)
    '        End Get
    '        Set(value As Single)
    '            NumValueValue = value

    '            If Not Store Is Nothing Then
    '                Store.Sng(GetKey) = value
    '            End If

    '            If Not NumEdit Is Nothing Then
    '                NumEdit.Value = CDec(value)
    '            End If
    '        End Set
    '    End Property

    '    Overrides Function GetArgs() As String
    '        If Switch = "" OrElse Not Visible Then Return Nothing

    '        If ArgsFunc Is Nothing Then
    '            '.ArgsFunc = Function() If(Not PsyRDOQ.value, "--no-psy-rdoq", If(PsyRDOQValue.Value <> PsyRDOQValue.DefaultValue, "--psy-rdoq " + PsyRDOQValue.Value.ToString(CultureInfo.InvariantCulture), "")),

    '            If BoolValue Then

    '            Else

    '            End If

    '            If NumValue <> DefaultNumValue OrElse AlwaysOn Then
    '                Return Switch + " " + NumValue.ToString(CultureInfo.InvariantCulture)
    '            End If
    '        Else
    '            Return ArgsFunc.Invoke()
    '        End If
    '    End Function

    '    Public Overrides Function GetControl() As Control
    '        Return NumEdit
    '    End Function
    'End Class

    Class OptionParam
        Inherits CommandLineItem

        Property DefaultValue As Integer
        Property Options As String()
        Property Values As String()
        Property Expand As Boolean
        Property MenuButton As MenuButton

        Overloads Sub Init(mb As MenuButton)
            MenuButton = mb
            MenuButton.Value = Value
            AddHandler MenuButton.ValueChangedUser, AddressOf ValueChangedUser
            AddHandler MenuButton.Disposed, Sub()
                                                RemoveHandler MenuButton.ValueChangedUser, AddressOf ValueChangedUser
                                                MenuButton = Nothing
                                            End Sub
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

                If Not Store Is Nothing Then
                    Store.Int(GetKey) = value
                End If

                If Not MenuButton Is Nothing Then
                    MenuButton.Value = ValueValue
                End If
            End Set
        End Property

        Overrides Function GetArgs() As String
            If Switch = "" OrElse Not Visible Then Return Nothing

            If ArgsFunc Is Nothing Then
                If Value <> DefaultValue OrElse AlwaysOn Then
                    If Not Values Is Nothing Then
                        Return Switch + " " & Values(Value)
                    Else
                        Return Switch + " " & Value
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

    Class StringParam
        Inherits CommandLineItem

        Property DefaultValue As String
        Property TextEdit As TextEdit
        Property UseQuotes As Boolean

        Public Overloads Overrides Sub Init(store As PrimitiveStore, params As CommandLineParams)
            Me.Store = store
            Me.Params = params

            If Not store.String.ContainsKey(GetKey) Then
                store.String(GetKey) = ValueValue
            End If
        End Sub

        Overloads Sub Init(te As TextEdit)
            TextEdit = te
            TextEdit.Text = Value
            AddHandler TextEdit.TextChanged, AddressOf TextChanged
            AddHandler TextEdit.Disposed, Sub()
                                              RemoveHandler TextEdit.TextChanged, AddressOf TextChanged
                                              TextEdit = Nothing
                                          End Sub
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
                        If UseQuotes Then
                            Return """" + Value + """"
                        Else
                            Return Value
                        End If
                    Else
                        If UseQuotes Then
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