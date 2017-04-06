Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class admUsers
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub cmbTeachers_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        cmbTeachers.DataSource = GetDataTable("getTeacherList '" & System.Web.HttpContext.Current.User.Identity.Name & "', '%" & e.Text & "%', NULL")
        cmbTeachers.DataTextField = "teacher_name"
        cmbTeachers.DataValueField = "usr_uuid"
        cmbTeachers.DataBind()
    End Sub

    Private Sub chkDeleted_CheckedChanged(sender As Object, e As EventArgs) Handles chkDeleted.CheckedChanged
        RadGrid1.MasterTableView.Columns(0).Visible = Not chkDeleted.Checked
        If chkDeleted.Checked Then
            CType(RadGrid1.MasterTableView.Columns(1), GridButtonColumn).ConfirmText = "Restore CSP?"
            CType(RadGrid1.MasterTableView.Columns(1), GridButtonColumn).Text = "Restore"
        Else
            CType(RadGrid1.MasterTableView.Columns(1), GridButtonColumn).ConfirmText = "Delete CSP?"
            CType(RadGrid1.MasterTableView.Columns(1), GridButtonColumn).Text = "Delete"
        End If
    End Sub

    Public Shared Function GetDataTable(ByVal query As String) As DataTable

        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        Dim sqlAdapter As New SqlDataAdapter()

        sqlAdapter.SelectCommand = New SqlCommand(query, sqlConn)

        Dim tblData As New DataTable()

        sqlConn.Open()
        sqlAdapter.Fill(tblData)

        sqlConn.Close()

        Return tblData

    End Function
End Class