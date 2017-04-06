Imports System
Imports System.Web
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient

Public Class userSession
    Inherits System.Web.UI.Page

    Private _roleId As Integer
    Private _schoolId As Integer
    Private _usrUuid As String

    Protected ReadOnly Property roleId() As Integer
        Get
            Return _roleId
        End Get
    End Property

    Protected ReadOnly Property schoolId() As Integer
        Get
            Return _schoolId
        End Get
    End Property

    ReadOnly Property usrUuid() As String
        Get
            Return _usrUuid
        End Get
    End Property

    Public Sub New()
        'If HttpContext.Current.Session("userId") Is Nothing Then
        initUserInfo()
        'End If
    End Sub

    Private Sub initUserInfo()

        _roleId = 0
        _schoolId = 0

        Dim sqlLogin = "doLogin"
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        Dim sqlComm As New SqlCommand(sqlLogin, sqlConn)
        Dim sqlAdapter As New SqlDataAdapter()

        sqlComm.CommandType = CommandType.StoredProcedure

        sqlComm.Parameters.Add(New SqlParameter("@usr_login", SqlDbType.NVarChar))
        sqlComm.Parameters("@usr_login").Value = Context.User.Identity.Name


        sqlAdapter.SelectCommand = sqlComm

        Dim tblData As New DataTable()

        sqlConn.Open()
        sqlAdapter.Fill(tblData)

        sqlConn.Close()

        If tblData.Rows.Count = 1 Then
            _roleId = tblData.Rows(0).Item("role_id")
            _usrUuid = tblData.Rows(0).Item("usr_uuid").ToString
            If Not IsDBNull(tblData.Rows(0).Item("school_id")) Then
                _schoolId = tblData.Rows(0).Item("school_id")
            End If
        End If
    End Sub

End Class
