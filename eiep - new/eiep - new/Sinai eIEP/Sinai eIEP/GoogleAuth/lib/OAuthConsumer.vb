Imports System.IO
Imports System.Net
Imports System.Text

Public Class OAuthConsumer
#Region "Private methods"

    Private Function NormalizeUrl(url As String) As String
        Dim questionIndex As Integer = url.IndexOf("?"c)
        If questionIndex = -1 Then
            Return url
        End If

        Dim parameters = url.Substring(questionIndex + 1)
        Dim result = New StringBuilder()
        result.Append(url.Substring(0, questionIndex + 1))

        Dim hasQueryParameters As Boolean = False
        If Not [String].IsNullOrEmpty(parameters) Then
            Dim parts As String() = parameters.Split("&"c)
            hasQueryParameters = parts.Length > 0
            For Each part In parts
                Dim nameValue = part.Split("="c)
                result.Append(nameValue(0) & "=")
                If nameValue.Length = 2 Then
                    result.Append(OAuthUtils.UrlEncode(nameValue(1)))
                End If
                result.Append("&")
            Next
            If hasQueryParameters Then
                result = result.Remove(result.Length - 1, 1)
            End If
        End If
        Return result.ToString()
    End Function

    Private Function MakeRequest(Of T As {TokenBase, New})(endPoint As String, authorizationHeader As AuthorizeHeader) As T
        Dim normalizedEndpoint = NormalizeUrl(endPoint)
        Dim request = WebRequest.Create(normalizedEndpoint)
        request.Headers.Add("Authorization", authorizationHeader.ToString())
        request.Method = "POST"
        Try
            Dim requestStream = request.GetRequestStream()
            Dim response = request.GetResponse()
            Using responseStream = response.GetResponseStream()
                Dim reader = New StreamReader(responseStream)
                Dim responseText = reader.ReadToEnd()
                reader.Close()
                Dim instanceOfT = New T()
                instanceOfT.Init(responseText)
                Return instanceOfT
            End Using
        Catch e As WebException
            Using resp = e.Response
                Using sr As New StreamReader(resp.GetResponseStream())
                    Dim errorMessage = sr.ReadToEnd()
                    Throw New OAuthProtocolException(errorMessage, e)
                End Using
            End Using
        End Try
    End Function

#End Region

    ''' <summary>
    ''' Step 1 of the oAuth protocol process
    ''' Make a request with the provider for a <see cref="RequestToken"/>
    ''' </summary>
    ''' <param name="requestTokenEndpoint">The url (as per the provider) to use for making a requet for a token</param>
    ''' <param name="realm">Typically the url of Your "application" or website</param>
    ''' <param name="consumerKey">The Consumer Key given to you by the provider</param>
    ''' <param name="consumerSecret">The Consumer Secret given to you by the provider</param>
    ''' <param name="callback">The url you'd like the provider to call you back on</param>
    ''' <param name="signatureMethod__1">defaults to HMAC-SHA1 - the only signature method currently supported</param>
    ''' <returns>An instance of a <see cref="RequestToken" /> class</returns>
    Public Function GetOAuthRequestToken(requestTokenEndpoint As String, realm As String, consumerKey As String, consumerSecret As String, callback As String, Optional signatureMethod__1 As SignatureMethod = SignatureMethod.HMACSHA1) As RequestToken
        Dim oAuthUtils = New OAuthUtils()
        Dim authorizationHeader = oAuthUtils.GetRequestTokenAuthorizationHeader(requestTokenEndpoint, realm, consumerKey, consumerSecret, callback, signatureMethod__1)
        Return MakeRequest(Of RequestToken)(requestTokenEndpoint, authorizationHeader)
    End Function

    ''' <summary>
    ''' Step 3 of the oAuth protocol process
    ''' Make a request on the provider to Exchange a <see cref="RequestToken"/> for an <see cref="AccessToken"/>
    ''' (Step 2 is a simple redirect and so there is no method for it in this class)
    ''' </summary>
    ''' <param name="accessTokenEndpoint">The url (as per the provider) to use for making a requet to Exchange a request token for an access token</param>
    ''' <param name="realm">Typically the url of Your "application" or website</param>
    ''' <param name="consumerKey">The Consumer Key given to you by the provider</param>
    ''' <param name="consumerSecret">The Consumer Secret given to you by the provider</param>
    ''' <param name="token">The token you got at the end of Step 1 or Step 2</param>
    ''' <param name="verifier">The verifier you got at the end of step 2</param>
    ''' <param name="tokenSecret">The tokenSecret you got at the end of step 1</param>
    ''' <param name="signatureMethod__1">defaults to HMAC-SHA1 - the only signature method currently supported</param>
    ''' <returns>An instance of a <see cref="AccessToken"/> class</returns>
    Public Function GetOAuthAccessToken(accessTokenEndpoint As String, realm As String, consumerKey As String, consumerSecret As String, token As String, verifier As String, _
        tokenSecret As String, Optional signatureMethod__1 As SignatureMethod = SignatureMethod.HMACSHA1) As AccessToken
        Dim oAuthUtils = New OAuthUtils()
        Dim authorizationHeader = oAuthUtils.GetAccessTokenAuthorizationHeader(accessTokenEndpoint, realm, consumerKey, consumerSecret, token, verifier, _
            tokenSecret, signatureMethod__1)
        Return MakeRequest(Of AccessToken)(accessTokenEndpoint, authorizationHeader)
    End Function


    Public Function GetUserInfo(userInfoEndpoint As String, realm As String, consumerKey As String, consumerSecret As String, token As String, tokenSecret As String, _
        Optional signatureMethod__1 As SignatureMethod = SignatureMethod.HMACSHA1) As String
        Dim oAuthUtils = New OAuthUtils()
        Dim authorizationHeader = oAuthUtils.GetUserInfoAuthorizationHeader(userInfoEndpoint, realm, consumerKey, consumerSecret, token, tokenSecret, _
            signatureMethod__1, "GET")

        Dim request = WebRequest.Create(userInfoEndpoint)
        request.Headers.Add("Authorization", authorizationHeader.ToString())
        request.Method = "GET"
        Try
            Dim response = request.GetResponse()
            Using responseStream = response.GetResponseStream()
                Dim reader = New StreamReader(responseStream)
                Dim responseText = reader.ReadToEnd()
                reader.Close()
                Return responseText
            End Using
        Catch e As WebException
            Using resp = e.Response
                Using sr As New StreamReader(resp.GetResponseStream())
                    Dim errorMessage = sr.ReadToEnd()
                    Throw New OAuthProtocolException(errorMessage, e)
                End Using
            End Using
        End Try
    End Function
End Class
