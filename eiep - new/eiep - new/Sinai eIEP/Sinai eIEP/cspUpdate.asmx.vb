Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
 Public Class cspUpdate
    Inherits System.Web.Services.WebService

    <WebMethod()> _
    Public Sub setDateInitiated(cspDataUuid As String, newValue As String)
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        sqlConn.Open()
        Dim sqlComm As New SqlCommand
        sqlComm.Connection = sqlConn

        sqlComm.Parameters.Add(New SqlParameter("@iep_data_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters.Add(New SqlParameter("@iep_date_initiated", SqlDbType.NVarChar))

        Dim strSql = "UPDATE [dbo].[iep_data] SET [iep_date_initiated] = @iep_date_initiated WHERE [iep_data_uuid] = @iep_data_uuid"
        sqlComm.CommandText = strSql

        sqlComm.Parameters("@iep_data_uuid").Value = New Guid(cspDataUuid)

        sqlComm.Parameters("@iep_date_initiated").Value = DBNull.Value
        If newValue <> "" Then sqlComm.Parameters("@iep_date_initiated").Value = newValue

        sqlComm.ExecuteNonQuery()

        sqlConn.Close()

        sqlComm = Nothing
        sqlConn = Nothing
    End Sub

    <WebMethod()> _
    Public Sub setGradeValue(controlId As String, cspDataUuid As String, newValue As String)
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        sqlConn.Open()
        Dim sqlComm As New SqlCommand
        sqlComm.Connection = sqlConn

        sqlComm.Parameters.Add(New SqlParameter("@iep_data_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters.Add(New SqlParameter("@iep_data_grade_id", SqlDbType.TinyInt))

        Dim strSql = "UPDATE [dbo].[iep_data] SET [" & Right(controlId, 19) & "] = @iep_data_grade_id WHERE [iep_data_uuid] = @iep_data_uuid"
        sqlComm.CommandText = strSql

        sqlComm.Parameters("@iep_data_uuid").Value = New Guid(cspDataUuid)

        sqlComm.Parameters("@iep_data_grade_id").Value = DBNull.Value
        If IsNumeric(newValue) Then sqlComm.Parameters("@iep_data_grade_id").Value = newValue

        sqlComm.ExecuteNonQuery()

        sqlConn.Close()

        sqlComm = Nothing
        sqlConn = Nothing
    End Sub

    <WebMethod()> _
    Public Sub updateCommentsNotes(cspUuid As String, fieldName As String, newValue As String)
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        sqlConn.Open()
        Dim sqlComm As New SqlCommand
        sqlComm.Connection = sqlConn

        Dim strSql As String = "UPDATE iep SET " & fieldName & " = @text WHERE iep_uuid = @iep_uuid"


        sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@iep_uuid").Value = New Guid(cspUuid)

        sqlComm.Parameters.Add(New SqlParameter("@text", SqlDbType.NVarChar))
        sqlComm.Parameters("@text").Value = newValue

        sqlComm.CommandText = strSql
        sqlComm.ExecuteNonQuery()

        sqlConn.Close()

        sqlComm = Nothing
        sqlConn = Nothing
    End Sub

    <WebMethod()> _
    Public Sub doLock(cspUuid As String)
        doSnapShot(cspUuid, 1)

        Dim uSession As New userSession

        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        sqlConn.Open()
        Dim sqlComm As New SqlCommand
        sqlComm.Connection = sqlConn
        Dim strSql = "doLockIep @iep_uuid, @usr_uuid, 1"

        sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@iep_uuid").Value = New Guid(cspUuid)

        sqlComm.Parameters.Add(New SqlParameter("@usr_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@usr_uuid").Value = New Guid(uSession.usrUuid)

        sqlComm.CommandText = strSql
        sqlComm.ExecuteNonQuery()

        sqlConn.Close()

        sqlComm = Nothing
        sqlConn = Nothing

    End Sub

    Private Sub doSnapShot(strIepUuid As String, Optional bitInit As Integer = 0)
        Dim strSql As String
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        sqlConn.Open()
        Dim sqlComm As New SqlCommand
        sqlComm.Connection = sqlConn

        strSql = "iepSnapshot @iep_uuid, @init"

        sqlComm.CommandText = strSql
        sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@iep_uuid").Value = New Guid(strIepUuid)

        sqlComm.Parameters.Add(New SqlParameter("@init", SqlDbType.Bit))
        sqlComm.Parameters("@init").Value = bitInit

        sqlComm.ExecuteNonQuery()
        sqlConn.Close()
        sqlConn = Nothing
        sqlComm = Nothing

    End Sub
End Class