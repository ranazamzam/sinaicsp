Imports System
Imports System.Collections.Generic
Imports System.Reflection
Imports System.Security.Cryptography
Imports System.Text

' Reference page on Google: http://code.google.com/apis/accounts/docs/OAuth_ref.html
' Reference page on Twitter: http://dev.twitter.com/pages/auth#at-twitter
' Reference page on Yahoo: http://developer.yahoo.com/oauth/guide/oauth-auth-flow.html
' Reference page on Vimeo: http://vimeo.com/api/docs/oauth


Public Class OAuthUtils
#Region "Private Nested Classes"

    Private Class ProtocolParameter

        Public Property Name() As String
            Get
                Return m_Name
            End Get
            Private Set(value As String)
                m_Name = value
            End Set
        End Property
        Private m_Name As String

        Public Property Value() As String
            Get
                Return m_Value
            End Get
            Private Set(value As String)
                m_Value = value
            End Set
        End Property
        Private m_Value As String

        Public Sub New(name__1 As String, value__2 As String)
            Name = name__1
            Value = value__2
        End Sub

    End Class

    Private Class LexicographicComparer
        Implements IComparer(Of ProtocolParameter)

        Public Function Compare(x As ProtocolParameter, y As ProtocolParameter) As Integer Implements IComparer(Of ProtocolParameter).Compare
            If x.Name = y.Name Then
                Return String.Compare(x.Value, y.Value)
            Else
                Return String.Compare(x.Name, y.Name)
            End If
        End Function

    End Class

#End Region

    'http://en.wikipedia.org/wiki/Percent-encoding
    Private Shared ReadOnly reservedCharacters As String = "!*'();:@&=+$,/?%#[]"
    Private Const OAuthVersion As String = "1.0"
    Private Const OAuthProtocolParameterPrefix As String = "oauth_"

    Private Shared random As New Random()

#Region "Private static methods"

    Private Shared Function GenerateTimeStamp() As String
        Dim ts As TimeSpan = DateTime.UtcNow - New DateTime(1970, 1, 1)
        Return Math.Truncate(ts.TotalSeconds).ToString()
    End Function

    Private Shared Function GenerateNonce(timestamp As String) As String
        Dim buffer = New Byte(255) {}
        random.NextBytes(buffer)
        Dim hmacsha1 = New HMACSHA1()
        hmacsha1.Key = Encoding.ASCII.GetBytes(Encoding.ASCII.GetString(buffer))
        Return ComputeHash(hmacsha1, timestamp)
    End Function

    Private Shared Function ComputeHash(hashAlgorithm As HashAlgorithm, data As String) As String
        If hashAlgorithm Is Nothing Then
            Throw New ArgumentNullException("hashAlgorithm")
        End If
        If String.IsNullOrEmpty(data) Then
            Throw New ArgumentNullException("data")
        End If

        Dim buffer As Byte() = System.Text.Encoding.ASCII.GetBytes(data)
        Dim bytes As Byte() = hashAlgorithm.ComputeHash(buffer)

        Return Convert.ToBase64String(bytes)
    End Function

    Private Shared Function NormalizeProtocolParameters(parameters As IList(Of ProtocolParameter)) As String
        Dim sb = New StringBuilder()
        Dim p As ProtocolParameter = Nothing
        For i As Integer = 0 To parameters.Count - 1
            p = parameters(i)
            sb.AppendFormat("{0}={1}", p.Name, UrlEncode(p.Value))

            If i < parameters.Count - 1 Then
                sb.Append("&")
            End If
        Next

        Return sb.ToString()
    End Function

    Private Shared Function ExtractQueryStrings(url As String) As List(Of ProtocolParameter)
        Dim questionIndex As Integer = url.IndexOf("?"c)
        If questionIndex = -1 Then
            Return New List(Of ProtocolParameter)()
        End If

        Dim parameters = url.Substring(questionIndex + 1)
        Dim result = New List(Of ProtocolParameter)()

        If Not [String].IsNullOrEmpty(parameters) Then
            Dim parts As String() = parameters.Split("&"c)
            For Each part In parts
                If Not String.IsNullOrEmpty(part) AndAlso Not part.StartsWith(OAuthProtocolParameterPrefix) Then
                    If part.IndexOf("="c) <> -1 Then
                        Dim nameValue As String() = part.Split("="c)
                        result.Add(New ProtocolParameter(nameValue(0), nameValue(1)))
                    Else
                        result.Add(New ProtocolParameter(part, [String].Empty))
                    End If
                End If
            Next
        End If
        Return result
    End Function

    Private Function GenerateSignatureBaseString(url As String, httpMethod As String, protocolParameters As List(Of ProtocolParameter)) As String
        protocolParameters.Sort(New LexicographicComparer())

        Dim uri = New Uri(url)
        Dim normalizedUrl = String.Format("{0}://{1}", uri.Scheme, uri.Host)

        If Not ((uri.Scheme = "http" AndAlso uri.Port = 80) OrElse (uri.Scheme = "https" AndAlso uri.Port = 443)) Then
            normalizedUrl += ":" & uri.Port
        End If
        normalizedUrl += uri.AbsolutePath

        Dim normalizedRequestParameters = NormalizeProtocolParameters(protocolParameters)

        Dim signatureBaseSb As New StringBuilder()
        signatureBaseSb.AppendFormat("{0}&", httpMethod)
        signatureBaseSb.AppendFormat("{0}&", UrlEncode(normalizedUrl))
        signatureBaseSb.AppendFormat("{0}", UrlEncode(normalizedRequestParameters))
        Return signatureBaseSb.ToString()
    End Function

    Private Shared Function GenerateSignature(consumerSecret As String, signatureMethod__1 As SignatureMethod, signatureBaseString As String, Optional tokenSecret As String = Nothing) As String
        Select Case signatureMethod__1
            Case SignatureMethod.HMACSHA1
                Dim hmacsha1 = New HMACSHA1()
                hmacsha1.Key = Encoding.ASCII.GetBytes([String].Format("{0}&{1}", UrlEncode(consumerSecret), If([String].IsNullOrEmpty(tokenSecret), "", UrlEncode(tokenSecret))))
                Return ComputeHash(hmacsha1, signatureBaseString)
            Case SignatureMethod.PLAINTEXT
                Throw New NotImplementedException("PLAINTEXT Signature Method type is not yet implemented")
            Case SignatureMethod.RSASHA1
                Throw New NotImplementedException("RSA-SHA1 Signature Method type is not yet implemented")
            Case Else
                Throw New ArgumentException("Unknown Signature Method", "signatureMethod")
        End Select
    End Function

#End Region

#Region "Public Static Methods"

    Public Shared Function UrlEncode(value As String) As String
        If [String].IsNullOrEmpty(value) Then
            Return [String].Empty
        End If

        Dim sb = New StringBuilder()

        For Each [char] As Char In value
            If reservedCharacters.IndexOf([char]) = -1 Then
                sb.Append([char])
            Else
                sb.AppendFormat("%{0:X2}", AscW([char]))
            End If
        Next
        Return sb.ToString()
    End Function

#End Region

    Public Function GetRequestTokenAuthorizationHeader(url As String, realm As String, consumerKey As String, consumerSecret As String, callbackUrl As String, Optional signatureMethod__1 As SignatureMethod = SignatureMethod.HMACSHA1, _
        Optional httpMethod As String = "POST") As AuthorizeHeader
        Dim urlEncodedCallback = UrlEncode(callbackUrl)
        Dim timestamp = GenerateTimeStamp()
        Dim nounce = GenerateNonce(timestamp)

        Dim protocolParameters = ExtractQueryStrings(url)
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.ConsumerKey.GetStringValue(), consumerKey))
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.SignatureMethod.GetStringValue(), signatureMethod__1.GetStringValue()))
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.Timestamp.GetStringValue(), timestamp))
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.Nounce.GetStringValue(), nounce))
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.Version.GetStringValue(), OAuthVersion))
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.Callback.GetStringValue(), callbackUrl))

        Dim signatureBaseString As String = GenerateSignatureBaseString(url, httpMethod, protocolParameters)
        Dim signature = GenerateSignature(consumerSecret, signatureMethod__1, signatureBaseString)
        Return New AuthorizeHeader(realm, consumerKey, signatureMethod__1.GetStringValue(), signature, timestamp, nounce, _
            OAuthVersion, callbackUrl)
    End Function

    Public Function GetAccessTokenAuthorizationHeader(url As String, realm As String, consumerKey As String, consumerSecret As String, token As String, verifier As String, _
        tokenSecret As String, Optional signatureMethod__1 As SignatureMethod = SignatureMethod.HMACSHA1, Optional httpMethod As String = "POST") As AuthorizeHeader
        Dim timestamp = GenerateTimeStamp()
        Dim nounce = GenerateNonce(timestamp)

        Dim protocolParameters = ExtractQueryStrings(url)
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.ConsumerKey.GetStringValue(), consumerKey))
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.SignatureMethod.GetStringValue(), signatureMethod__1.GetStringValue()))
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.Timestamp.GetStringValue(), timestamp))
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.Nounce.GetStringValue(), nounce))
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.Version.GetStringValue(), OAuthVersion))
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.Token.GetStringValue(), token))
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.Verifier.GetStringValue(), verifier))

        Dim signatureBaseString As String = GenerateSignatureBaseString(url, httpMethod, protocolParameters)
        System.Diagnostics.Debug.WriteLine(signatureBaseString)

        Dim signature = GenerateSignature(consumerSecret, signatureMethod__1, signatureBaseString, tokenSecret)
        Return New AuthorizeHeader(realm, consumerKey, signatureMethod__1.GetStringValue(), signature, timestamp, nounce, _
            OAuthVersion, token, verifier)
    End Function

    Public Function GetUserInfoAuthorizationHeader(url As String, realm As String, consumerKey As String, consumerSecret As String, token As String, tokenSecret As String, _
        Optional signatureMethod__1 As SignatureMethod = SignatureMethod.HMACSHA1, Optional httpMethod As String = "POST") As AuthorizeHeader
        System.Diagnostics.Debug.WriteLine("")
        System.Diagnostics.Debug.WriteLine(token)
        System.Diagnostics.Debug.WriteLine(tokenSecret)
        System.Diagnostics.Debug.WriteLine("----")
        Dim timestamp = GenerateTimeStamp()
        Dim nounce = GenerateNonce(timestamp)

        Dim protocolParameters = ExtractQueryStrings(url)
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.ConsumerKey.GetStringValue(), consumerKey))
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.SignatureMethod.GetStringValue(), signatureMethod__1.GetStringValue()))
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.Timestamp.GetStringValue(), timestamp))
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.Nounce.GetStringValue(), nounce))
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.Version.GetStringValue(), OAuthVersion))
        protocolParameters.Add(New ProtocolParameter(OAuthProtocolParameter.Token.GetStringValue(), token))

        Dim signatureBaseString As String = GenerateSignatureBaseString(url, httpMethod, protocolParameters)
        System.Diagnostics.Debug.WriteLine(signatureBaseString)

        Dim signature = GenerateSignature(consumerSecret, signatureMethod__1, signatureBaseString, tokenSecret)
        Return New AuthorizeHeader(realm, consumerKey, signatureMethod__1.GetStringValue(), signature, timestamp, nounce, _
            OAuthVersion, token, Nothing)
    End Function

End Class

Public Enum SignatureMethod
    <EnumStringValueAttribute("HMAC-SHA1")> HMACSHA1
    <EnumStringValueAttribute("RSA-SHA1")> RSASHA1
    <EnumStringValueAttribute("PLAINTEXT")> PLAINTEXT
End Enum

Friend Enum OAuthProtocolParameter
    <EnumStringValueAttribute("oauth_consumer_key")> ConsumerKey
    <EnumStringValueAttribute("oauth_signature_method")> SignatureMethod
    <EnumStringValueAttribute("oauth_signature")> Signature
    <EnumStringValueAttribute("oauth_timestamp")> Timestamp
    <EnumStringValueAttribute("oauth_nonce")> Nounce
    <EnumStringValueAttribute("oauth_version")> Version
    <EnumStringValueAttribute("oauth_callback")> Callback
    <EnumStringValueAttribute("oauth_verifier")> Verifier
    <EnumStringValueAttribute("oauth_token")> Token
    <EnumStringValueAttribute("oauth_token_secret")> TokenSecret
End Enum
