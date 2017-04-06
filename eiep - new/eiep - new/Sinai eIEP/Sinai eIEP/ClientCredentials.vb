'
'Copyright 2011 Google Inc
'
'Licensed under the Apache License, Version 2.0(the "License");
'you may not use this file except in compliance with the License.
'You may obtain a copy of the License at
'
'    http://www.apache.org/licenses/LICENSE-2.0
'
'Unless required by applicable law or agreed to in writing, software
'distributed under the License is distributed on an "AS IS" BASIS,
'WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
'See the License for the specific language governing permissions and
'limitations under the License.
'


'Imports Google.Apis.Samples.Helper

Friend NotInheritable Class ClientCredentials
    Private Sub New()
    End Sub
    ''' <summary>
    ''' The OAuth2.0 Client ID of your project.
    ''' </summary>
    Public Shared ReadOnly ClientID As String = "620076569198.apps.googleusercontent.com"

    ''' <summary>
    ''' The OAuth2.0 Client secret of your project.
    ''' </summary>
    Public Shared ReadOnly ClientSecret As String = "gidnpZ443wvWYq7ST6KY03Jd"

    ''' <summary>
    ''' Your Api/Developer key.
    ''' </summary>
    Public Shared ReadOnly ApiKey As String = "<Enter your ApiKey here>"

#Region "Verify Credentials"
    Shared Sub New()
        'ReflectionUtils.VerifyCredentials(GetType(ClientCredentials))
    End Sub
#End Region
End Class