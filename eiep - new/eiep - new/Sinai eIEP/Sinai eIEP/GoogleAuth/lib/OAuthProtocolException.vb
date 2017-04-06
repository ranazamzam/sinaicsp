Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Runtime.Serialization

Public Class OAuthProtocolException
    Inherits Exception
    Public Sub New(message As String)
        MyBase.New(message)
    End Sub

    Public Sub New(message As String, innerException As Exception)
        MyBase.New(message, innerException)
    End Sub

    Public Sub New(info As SerializationInfo, context As StreamingContext)
        MyBase.New(info, context)
    End Sub
End Class