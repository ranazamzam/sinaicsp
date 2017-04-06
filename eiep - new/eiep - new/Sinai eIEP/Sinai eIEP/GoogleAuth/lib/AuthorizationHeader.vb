Imports System.Text

Public Class AuthorizeHeader
    Public Property Realm() As String
        Get
            Return m_Realm
        End Get
        Private Set(value As String)
            m_Realm = value
        End Set
    End Property
    Private m_Realm As String
    Public Property ConsumerKey() As String
        Get
            Return m_ConsumerKey
        End Get
        Private Set(value As String)
            m_ConsumerKey = value
        End Set
    End Property
    Private m_ConsumerKey As String
    Public Property SignatureMethod() As String
        Get
            Return m_SignatureMethod
        End Get
        Private Set(value As String)
            m_SignatureMethod = value
        End Set
    End Property
    Private m_SignatureMethod As String
    Public Property Signature() As String
        Get
            Return m_Signature
        End Get
        Private Set(value As String)
            m_Signature = value
        End Set
    End Property
    Private m_Signature As String
    Public Property Timestamp() As String
        Get
            Return m_Timestamp
        End Get
        Private Set(value As String)
            m_Timestamp = value
        End Set
    End Property
    Private m_Timestamp As String
    Public Property Nounce() As String
        Get
            Return m_Nounce
        End Get
        Private Set(value As String)
            m_Nounce = value
        End Set
    End Property
    Private m_Nounce As String
    Public Property Version() As String
        Get
            Return m_Version
        End Get
        Private Set(value As String)
            m_Version = value
        End Set
    End Property
    Private m_Version As String
    Public Property Callback() As String
        Get
            Return m_Callback
        End Get
        Private Set(value As String)
            m_Callback = value
        End Set
    End Property
    Private m_Callback As String
    Public Property Token() As String
        Get
            Return m_Token
        End Get
        Private Set(value As String)
            m_Token = value
        End Set
    End Property
    Private m_Token As String
    Public Property Verifier() As String
        Get
            Return m_Verifier
        End Get
        Private Set(value As String)
            m_Verifier = value
        End Set
    End Property
    Private m_Verifier As String

    Public Sub New(realm__1 As String, consumerKey__2 As String, signatureMethod__3 As String, signature__4 As String, timestamp__5 As String, nounce__6 As String, _
        version__7 As String)
        Realm = realm__1
        ConsumerKey = consumerKey__2
        SignatureMethod = signatureMethod__3
        Signature = signature__4
        Timestamp = timestamp__5
        Nounce = nounce__6
        Version = version__7
    End Sub

    Public Sub New(realm As String, consumerKey As String, signatureMethod As String, signature As String, timestamp As String, nounce As String, _
        version As String, callback__1 As String)
        Me.New(realm, consumerKey, signatureMethod, signature, timestamp, nounce, _
            version)
        Callback = callback__1
    End Sub

    Public Sub New(realm As String, consumerKey As String, signatureMethod As String, signature As String, timestamp As String, nounce As String, _
        version As String, token__1 As String, verifier__2 As String)
        Me.New(realm, consumerKey, signatureMethod, signature, timestamp, nounce, _
            version)
        Token = token__1
        Verifier = verifier__2
    End Sub

    Public Overrides Function ToString() As String
        Dim sb = New StringBuilder()
        sb.Append("OAuth ")
        sb.AppendFormat("realm=""{0}"", ", Realm)
        sb.AppendFormat("{0}=""{1}"", ", OAuthProtocolParameter.ConsumerKey.GetStringValue(), OAuthUtils.UrlEncode(ConsumerKey))
        sb.AppendFormat("{0}=""{1}"", ", OAuthProtocolParameter.SignatureMethod.GetStringValue(), SignatureMethod)
        sb.AppendFormat("{0}=""{1}"", ", OAuthProtocolParameter.Signature.GetStringValue(), OAuthUtils.UrlEncode(Signature))
        sb.AppendFormat("{0}=""{1}"", ", OAuthProtocolParameter.Timestamp.GetStringValue(), Timestamp)
        sb.AppendFormat("{0}=""{1}"", ", OAuthProtocolParameter.Nounce.GetStringValue(), OAuthUtils.UrlEncode(Nounce))
        sb.AppendFormat("{0}=""{1}"", ", OAuthProtocolParameter.Version.GetStringValue(), Version)
        If Not [String].IsNullOrEmpty(Callback) Then
            sb.AppendFormat("{0}=""{1}"", ", OAuthProtocolParameter.Callback.GetStringValue(), OAuthUtils.UrlEncode(Callback))
        End If
        If Not [String].IsNullOrEmpty(Token) Then
            sb.AppendFormat("{0}=""{1}"", ", OAuthProtocolParameter.Token.GetStringValue(), OAuthUtils.UrlEncode(Token))
        End If
        If Not [String].IsNullOrEmpty(Verifier) Then
            sb.AppendFormat("{0}=""{1}"", ", OAuthProtocolParameter.Verifier.GetStringValue(), OAuthUtils.UrlEncode(Verifier))
        End If

        sb = sb.Remove(sb.Length - 2, 2)
        Return sb.ToString()
    End Function
End Class