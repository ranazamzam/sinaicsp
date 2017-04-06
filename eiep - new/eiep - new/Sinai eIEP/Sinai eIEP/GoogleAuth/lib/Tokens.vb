Imports System.Collections.Specialized
Imports System.Web

Public Class TokenBase
    Public Property Token() As String
        Get
            Return m_Token
        End Get
        Protected Set(value As String)
            m_Token = value
        End Set
    End Property
    Private m_Token As String
    Public Property TokenSecret() As String
        Get
            Return m_TokenSecret
        End Get
        Protected Set(value As String)
            m_TokenSecret = value
        End Set
    End Property
    Private m_TokenSecret As String
    Public Property AddtionalProperties() As NameValueCollection
        Get
            Return m_AddtionalProperties
        End Get
        Private Set(value As NameValueCollection)
            m_AddtionalProperties = value
        End Set
    End Property
    Private m_AddtionalProperties As NameValueCollection

    Public Sub New()
        AddtionalProperties = New NameValueCollection()
    End Sub

    Public Sub Init(tokenResponse As String)
        Dim parts As String() = tokenResponse.Split("&"c)
        For Each part In parts
            Dim nameValue = part.Split("="c)
            If nameValue.Length = 2 Then
                If nameValue(0) = "oauth_token" Then
                    Token = HttpUtility.UrlDecode(nameValue(1))
                ElseIf nameValue(0) = "oauth_token_secret" Then
                    TokenSecret = HttpUtility.UrlDecode(nameValue(1))
                Else
                    AddtionalProperties.Add(nameValue(0), HttpUtility.UrlDecode(nameValue(1)))
                End If
            End If
        Next
    End Sub
End Class

Public Class RequestToken
    Inherits TokenBase
    Public Property CallbackConfirmed() As Boolean
        Get
            Return m_CallbackConfirmed
        End Get
        Set(value As Boolean)
            m_CallbackConfirmed = value
        End Set
    End Property
    Private m_CallbackConfirmed As Boolean
End Class

Public Class AccessToken
    Inherits TokenBase
    Public Sub New()
    End Sub
End Class