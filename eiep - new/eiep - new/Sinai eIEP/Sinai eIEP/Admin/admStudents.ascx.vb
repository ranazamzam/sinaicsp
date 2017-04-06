Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class admStudents
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub cmbStudents_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs)
        cmbStudents.DataSource = GetDataTable("getStudentList '" & System.Web.HttpContext.Current.User.Identity.Name & "', '%" & e.Text & "%', NULL, NULL, '" & cmbYear.SelectedValue & "'")
        cmbStudents.DataTextField = "student_name"
        cmbStudents.DataValueField = "student_uuid"
        cmbStudents.DataBind()
    End Sub

    Private Sub chkDeleted_CheckedChanged(sender As Object, e As EventArgs) Handles chkDeleted.CheckedChanged
        RadGrid1.MasterTableView.Columns(0).Visible = Not chkDeleted.Checked
        If chkDeleted.Checked Then
            CType(RadGrid1.MasterTableView.Columns(1), GridButtonColumn).ConfirmText = "Restore student?"
            CType(RadGrid1.MasterTableView.Columns(1), GridButtonColumn).Text = "Restore"
        Else
            CType(RadGrid1.MasterTableView.Columns(1), GridButtonColumn).ConfirmText = "Delete student?"
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