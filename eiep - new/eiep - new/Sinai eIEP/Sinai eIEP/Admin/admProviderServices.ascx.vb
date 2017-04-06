Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class admProviderServices
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub ddlModel_Load(sender As Object, e As EventArgs)

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

    Protected Sub cmbStudentFilter_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        Dim oStudents As RadComboBox = CType(sender, RadComboBox)

        oStudents.DataSource = GetDataTable("getStudentList '" & System.Web.HttpContext.Current.User.Identity.Name & "', '%" & e.Text & "%', NULL, NULL, '" & cmbYear.SelectedValue & "'")
        oStudents.DataTextField = "student_name"
        oStudents.DataValueField = "student_uuid"
        oStudents.DataBind()
    End Sub

    Protected Sub cmbStudents_ItemsRequested(sender As Object, e As Telerik.Web.UI.RadComboBoxItemsRequestedEventArgs)
        Dim oStudents As RadComboBox = CType(sender, RadComboBox)
        Dim currentSchoolYear As String

        If Month(Now) >= 7 Then
            currentSchoolYear = Year(Now).ToString & " - " & (Year(Now) + 1).ToString
        Else
            currentSchoolYear = (Year(Now) - 1).ToString & " - " & Year(Now).ToString
        End If

        oStudents.DataSource = GetDataTable("getStudentList '" & System.Web.HttpContext.Current.User.Identity.Name & "', '%" & e.Text & "%', NULL, NULL, '" & currentSchoolYear & "'")
        oStudents.DataTextField = "student_name"
        oStudents.DataValueField = "student_uuid"
        oStudents.DataBind()
    End Sub

    Protected Sub btnNewProvider_Click(sender As Object, e As EventArgs)
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        Dim strSql As String
        Dim sqlComm As SqlCommand

        If btnNewProvider.Text = "Add" Then
            sqlConn.Open()
            strSql = "INSERT INTO service_providers VALUES(NEWID(), '" & txtNewProvider.Text & "')"
            sqlComm = New SqlCommand(strSql, sqlConn)
            sqlComm.ExecuteNonQuery()

            cmbProviders.DataSourceID = Nothing
            cmbProviders.DataSourceID = "sqlProviders"
            cmbProviders.DataBind()
            txtNewProvider.Text = ""
        Else
            sqlConn.Open()
            strSql = "UPDATE service_providers SET provider_name = '" & txtNewProvider.Text & "' WHERE providers_uuid = '" & cmbProviders.SelectedValue & "'"
            sqlComm = New SqlCommand(strSql, sqlConn)
            sqlComm.ExecuteNonQuery()

            cmbProviders.SelectedItem.Text = txtNewProvider.Text
        End If

    End Sub

    Protected Sub cmbProviders_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        If e.Value = "00000000-0000-0000-0000-000000000000" Then
            btnNewProvider.Text = "Add"
            txtNewProvider.Text = ""
        Else
            btnNewProvider.Text = "Save"
            txtNewProvider.Text = e.Text
        End If
    End Sub

    Protected Sub rgServices_InsertCommand(sender As Object, e As GridCommandEventArgs)
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        Dim strSql As String = "doInsertService"

        sqlConn.Open()

        Dim sqlComm As New SqlCommand(strSql, sqlConn)
        sqlComm.CommandType = CommandType.StoredProcedure
        sqlComm.Parameters.Add(New SqlParameter("@services_parent", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@services_parent").Value = DBNull.Value
        If CType(e.Item.FindControl("hdnServicesParentUuid"), HiddenField).Value <> "" Then
            sqlComm.Parameters("@services_parent").Value = New Guid(CType(e.Item.FindControl("hdnServicesParentUuid"), HiddenField).Value)
        End If

        sqlComm.Parameters.Add(New SqlParameter("@provider_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@provider_uuid").Value = New Guid(cmbProviders.SelectedValue)

        sqlComm.Parameters.Add(New SqlParameter("@services_name", SqlDbType.NVarChar))
        sqlComm.Parameters("@services_name").Value = CType(e.Item.FindControl("txtService"), TextBox).Text

        sqlComm.Parameters.Add(New SqlParameter("@service_model", SqlDbType.VarChar))
        sqlComm.Parameters("@service_model").Value = CType(e.Item.FindControl("ddlModel"), DropDownList).SelectedValue

        sqlComm.Parameters.Add(New SqlParameter("@num_students", SqlDbType.TinyInt))
        sqlComm.Parameters("@num_students").Value = CType(e.Item.FindControl("txtNumStudents"), RadNumericTextBox).Text

        sqlComm.Parameters.Add(New SqlParameter("@session_length", SqlDbType.VarChar))
        sqlComm.Parameters("@session_length").Value = CType(e.Item.FindControl("txtLength"), TextBox).Text

        sqlComm.Parameters.Add(New SqlParameter("@weekly_sessions", SqlDbType.VarChar))
        sqlComm.Parameters("@weekly_sessions").Value = CType(e.Item.FindControl("txtNumSessions"), TextBox).Text

        sqlComm.Parameters.Add(New SqlParameter("@session_start_date", SqlDbType.VarChar))
        sqlComm.Parameters("@session_start_date").Value = CType(e.Item.FindControl("ddlStart"), DropDownList).SelectedValue

        sqlComm.Parameters.Add(New SqlParameter("@session_end_date", SqlDbType.VarChar))
        sqlComm.Parameters("@session_end_date").Value = CType(e.Item.FindControl("ddlEnd"), DropDownList).SelectedValue

        sqlComm.Parameters.Add(New SqlParameter("@services_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@services_uuid").Direction = ParameterDirection.Output

        sqlComm.CommandText = strSql
        sqlComm.ExecuteNonQuery()

        Dim strServicesUuid As String = sqlComm.Parameters("@services_uuid").Value.ToString

        Dim lvItem As RadListBoxItem

        For Each lvItem In CType(e.Item.FindControl("lvStudents"), RadListBox).Items
            sqlComm = New SqlCommand(strSql, sqlConn)
            strSql = "INSERT INTO services_students VALUES ('" & strServicesUuid & "', '" & lvItem.Value & "')"
            sqlComm.CommandText = strSql
            sqlComm.ExecuteNonQuery()
        Next
        
        sqlConn.Close()
        rgServices.Rebind()

    End Sub

    Protected Sub rgServices_UpdateCommand(sender As Object, e As GridCommandEventArgs)
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        Dim strSql As String = "doUpdateService"

        sqlConn.Open()

        Dim sqlComm As New SqlCommand(strSql, sqlConn)
        sqlComm.CommandType = CommandType.StoredProcedure

        sqlComm.Parameters.Add(New SqlParameter("@services_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@services_uuid").Value = New Guid(CType(e.Item.FindControl("hdnServicesUuid"), HiddenField).Value)

        sqlComm.Parameters.Add(New SqlParameter("@services_name", SqlDbType.NVarChar))
        sqlComm.Parameters("@services_name").Value = CType(e.Item.FindControl("txtService"), TextBox).Text

        sqlComm.Parameters.Add(New SqlParameter("@service_model", SqlDbType.VarChar))
        sqlComm.Parameters("@service_model").Value = CType(e.Item.FindControl("ddlModel"), DropDownList).SelectedValue

        sqlComm.Parameters.Add(New SqlParameter("@num_students", SqlDbType.TinyInt))
        sqlComm.Parameters("@num_students").Value = CType(e.Item.FindControl("txtNumStudents"), RadNumericTextBox).Text

        sqlComm.Parameters.Add(New SqlParameter("@session_length", SqlDbType.VarChar))
        sqlComm.Parameters("@session_length").Value = CType(e.Item.FindControl("txtLength"), TextBox).Text

        sqlComm.Parameters.Add(New SqlParameter("@weekly_sessions", SqlDbType.VarChar))
        sqlComm.Parameters("@weekly_sessions").Value = CType(e.Item.FindControl("txtNumSessions"), TextBox).Text

        sqlComm.Parameters.Add(New SqlParameter("@session_start_date", SqlDbType.VarChar))
        sqlComm.Parameters("@session_start_date").Value = CType(e.Item.FindControl("ddlStart"), DropDownList).SelectedValue

        sqlComm.Parameters.Add(New SqlParameter("@session_end_date", SqlDbType.VarChar))
        sqlComm.Parameters("@session_end_date").Value = CType(e.Item.FindControl("ddlEnd"), DropDownList).SelectedValue

        sqlComm.CommandText = strSql
        sqlComm.ExecuteNonQuery()

        Dim strServicesUuid As String = CType(e.Item.FindControl("hdnServicesUuid"), HiddenField).Value.ToString

        sqlComm = New SqlCommand(strSql, sqlConn)
        strSql = "DELETE FROM services_students WHERE services_uuid = '" & strServicesUuid & "'"
        sqlComm.CommandText = strSql
        sqlComm.ExecuteNonQuery()

        Dim lvItem As RadListBoxItem

        For Each lvItem In CType(e.Item.FindControl("lvStudents"), RadListBox).Items
            sqlComm = New SqlCommand(strSql, sqlConn)
            strSql = "INSERT INTO services_students VALUES ('" & strServicesUuid & "', '" & lvItem.Value & "')"
            sqlComm.CommandText = strSql
            sqlComm.ExecuteNonQuery()
        Next

        sqlConn.Close()
        rgServices.Rebind()

    End Sub

    Protected Sub rgServices_ItemDataBound(sender As Object, e As GridItemEventArgs)

        Dim oItem As GridEditableItem
        'Dim oNumTxt As RadNumericTextBox
        'Dim oTxt As TextBox
        Dim intTmp As Integer
        Dim intTmp1 As Integer

        If TypeOf (e.Item) Is GridEditableItem And e.Item.IsInEditMode Then
            oItem = e.Item
            'oNumTxt = oItem("num_students").Controls(0)
            'oNumTxt.Width = Unit.Pixel(30)
            'oTxt = oItem("service_model").Controls(0)
            'oTxt.Width = Unit.Pixel(50)
            'oTxt = oItem("session_length").Controls(0)
            'oTxt.Width = Unit.Pixel(75)
            'oTxt = oItem("weekly_sessions").Controls(0)
            'oTxt.Width = Unit.Pixel(75)

            If hdnServicesParent.Value <> "" Then
                For intTmp = 0 To rgServices.MasterTableView.Items.Count - 1
                    If rgServices.MasterTableView.Items(intTmp).GetDataKeyValue("services_parent").ToString = hdnServicesParent.Value Then
                        For intTmp1 = intTmp To rgServices.MasterTableView.Items.Count - 1
                            If rgServices.MasterTableView.Items(intTmp1).GetDataKeyValue("services_parent").ToString <> hdnServicesParent.Value Then
                                Exit For
                            End If
                        Next
                        intTmp1 = intTmp1 - 1
                        Exit For
                    End If
                Next

                Dim oDataItem = rgServices.MasterTableView.Items(intTmp1)

                CType(oItem.FindControl("hdnServicesParentUuid"), HiddenField).Value = oDataItem.GetDataKeyValue("services_parent").ToString
                CType(oItem.FindControl("hdnServicesUuid"), HiddenField).Value = oDataItem.GetDataKeyValue("services_uuid").ToString

                CType(oItem.FindControl("txtService"), TextBox).Text = oDataItem.GetDataKeyValue("services_name")
                CType(oItem.FindControl("ddlModel"), DropDownList).SelectedValue = oDataItem.Cells(9).Text
                CType(oItem.FindControl("txtNumStudents"), RadNumericTextBox).Text = oDataItem.Cells(10).Text
                CType(oItem.FindControl("txtLength"), TextBox).Text = oDataItem.Cells(11).Text
                CType(oItem.FindControl("txtNumSessions"), TextBox).Text = oDataItem.Cells(12).Text
                hdnServicesParent.Value = ""
            End If

            'If oItem.FindControl("PerformInsertButton") Is Nothing Then
            '    Dim update As ImageButton = CType(oItem.FindControl("UpdateButton"), ImageButton)
            '    'update.Attributes.Add("onclick", "doValidate('" & oItem("session_start_date").Controls(0).ClientID & "');")
            'Else
            'Dim insert As Button = CType(oItem.FindControl("btnUpdate"), Button)
            'insert.Attributes.Add("onclick", "doValidate('" & oItem.FindControl("ddlStart").ClientID & "');")
            'End If

        End If

    End Sub
End Class