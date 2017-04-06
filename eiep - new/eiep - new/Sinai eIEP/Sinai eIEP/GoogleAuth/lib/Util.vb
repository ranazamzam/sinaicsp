Imports System.Runtime.CompilerServices
Imports System.Reflection
Imports System.Security.Cryptography
Imports System.Text

Module Util

    <AttributeUsage(AttributeTargets.Field)> Public Class EnumStringValueAttribute
        Inherits Attribute
        Public Property Value() As String
            Get
                Return m_Value
            End Get
            Private Set(value As String)
                m_Value = value
            End Set
        End Property
        Private m_Value As String

        Public Sub New(value__1 As String)
            Value = value__1
        End Sub
    End Class

    <System.Runtime.CompilerServices.Extension> Public Function GetStringValue(value As [Enum]) As String
        Dim output As String = Nothing
        Dim type As Type = value.[GetType]()
        Dim fieldInfo As FieldInfo = type.GetField(value.ToString())
        Dim attributes As EnumStringValueAttribute() = TryCast(fieldInfo.GetCustomAttributes(GetType(EnumStringValueAttribute), False), EnumStringValueAttribute())
        If attributes.Length > 0 Then
            output = attributes(0).Value
        End If
        Return output
    End Function

End Module
