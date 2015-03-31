Imports System.Reflection
Imports System.ComponentModel

Imports StaxRip.UI

Namespace UI
    <Serializable()>
    Public MustInherit Class Criteria
        MustOverride Function Eval() As Boolean
        MustOverride Property ValueString() As String
        MustOverride Property PropertyString() As String
        MustOverride ReadOnly Property ConditionNames() As String()
        MustOverride Property ConditionName() As String

        Private NameValue As String

        Property Name() As String
            Get
                Return NameValue
            End Get
            Set(Value As String)
                NameValue = Value
            End Set
        End Property

        Private MacroValue As String

        Property Macro() As String
            Get
                Return MacroValue
            End Get
            Set(Value As String)
                MacroValue = Value
            End Set
        End Property

        Shared Function Create(t As Type) As Criteria
            If t Is GetType(String) Then
                Return New StringCriteria
            ElseIf t Is GetType(Integer) Then
                Return New IntCriteria
            ElseIf t Is GetType(Boolean) Then
                Return New BooleanCriteria
            End If

            Throw New Exception("failed to create criteria")
        End Function

        Overrides Function ToString() As String
            Return Name
        End Function
    End Class

    <Serializable()>
    Public MustInherit Class GenericCriteria(Of TCondition, TType)
        Inherits Criteria

        Private PropertyValueValue As TType

        Property PropertyValue() As TType
            Get
                Return PropertyValueValue
            End Get
            Set(Value As TType)
                PropertyValueValue = Value
            End Set
        End Property

        Private ValueValue As TType

        Property Value() As TType
            Get
                Return ValueValue
            End Get
            Set(Value As TType)
                ValueValue = Value
            End Set
        End Property

        Private ConditionValue As TCondition

        Property Condition() As TCondition
            Get
                Return ConditionValue
            End Get
            Set(Value As TCondition)
                ConditionValue = Value
            End Set
        End Property

        Overrides ReadOnly Property ConditionNames() As String()
            Get
                Return DispNameAttribute.GetNamesForEnum(Of TCondition)()
            End Get
        End Property

        Overrides Property ConditionName() As String
            Get
                Return DispNameAttribute.GetValueForEnum(Condition)
            End Get
            Set(value As String)
                For Each i As TCondition In System.Enum.GetValues(GetType(TCondition))
                    If DispNameAttribute.GetValueForEnum(i) = value Then
                        Condition = i
                    End If
                Next
            End Set
        End Property
    End Class

    <Serializable()>
    Public Class IntCriteria
        Inherits GenericCriteria(Of IntegerCondition, Integer)

        Overrides Function Eval() As Boolean
            Select Case Condition
                Case IntegerCondition.Is
                    Return PropertyValue = Value
                Case IntegerCondition.IsNot
                    Return PropertyValue <> Value
                Case IntegerCondition.IsHigherThan
                    Return PropertyValue > Value
                Case IntegerCondition.IsLowerThan
                    Return PropertyValue < Value
            End Select
        End Function

        Overrides Property ValueString() As String
            Get
                Return Value.ToString
            End Get
            Set(value As String)
                Me.Value = value.ToInt(Me.Value)
            End Set
        End Property

        Overrides Property PropertyString() As String
            Get
                Return PropertyValue.ToString()
            End Get
            Set(value As String)
                PropertyValue = value.ToInt(PropertyValue)
            End Set
        End Property
    End Class

    <Serializable()>
    Public Class StringCriteria
        Inherits GenericCriteria(Of StringCondition, String)

        Overrides Function Eval() As Boolean
            Select Case Condition
                Case StringCondition.Contains
                    Return PropertyValue.Contains(Value)
                Case StringCondition.DoesntContain
                    Return Not PropertyValue.Contains(Value)
                Case StringCondition.Is
                    Return PropertyValue = Value
                Case StringCondition.IsNot
                    Return PropertyValue <> Value
            End Select
        End Function

        Overrides Property ValueString() As String
            Get
                If Value Is Nothing Then
                    Value = ""
                End If

                Return Value
            End Get
            Set(value As String)
                Me.Value = value
            End Set
        End Property

        Overrides Property PropertyString() As String
            Get
                If PropertyValue Is Nothing Then
                    PropertyValue = ""
                End If

                Return Value
            End Get
            Set(value As String)
                PropertyValue = value
            End Set
        End Property
    End Class

    <Serializable()>
    Public Class BooleanCriteria
        Inherits GenericCriteria(Of BooleanCondition, Boolean)

        Overrides Function Eval() As Boolean
            Select Case Condition
                Case BooleanCondition.Is
                    Return PropertyValue = Value
                Case BooleanCondition.IsNot
                    Return PropertyValue <> Value
                Case Else
                    Throw New NotImplementedException
            End Select
        End Function

        Overrides Property ValueString() As String
            Get
                Return Value.ToString
            End Get
            Set(value As String)
                If Not value Is Nothing AndAlso value.ToUpper = "TRUE" Then
                    Me.Value = True
                Else
                    Me.Value = False
                End If
            End Set
        End Property

        Overrides Property PropertyString() As String
            Get
                Return Value.ToString
            End Get
            Set(value As String)
                If Not value Is Nothing AndAlso value.ToUpper = "TRUE" Then
                    PropertyValue = True
                Else
                    PropertyValue = False
                End If
            End Set
        End Property
    End Class

    Public Enum IntegerCondition
        [Is]
        <DispName("Is Not")> [IsNot]
        <DispName("Is Higher Than")> IsHigherThan
        <DispName("Is Lower Than")> IsLowerThan
    End Enum

    Public Enum StringCondition
        [Is]
        <DispName("Is Not")> [IsNot]
        Contains
        <DispName("Contains Not")> DoesntContain
    End Enum

    Public Enum BooleanCondition
        [Is]
        <DispName("Is Not")> [IsNot]
    End Enum
End Namespace