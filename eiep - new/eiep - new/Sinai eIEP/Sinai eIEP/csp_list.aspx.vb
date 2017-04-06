Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class csp_list
    Inherits userSession

    Public timeoutWarningLength As Integer = 300
    Private bvSessionVars As New BitVector32(0)

    Private bitTeacher As Integer = BitVector32.CreateMask()
    Private bitStudents As Integer = BitVector32.CreateMask(bitTeacher)
    Private bitSubjects As Integer = BitVector32.CreateMask(bitStudents)
    Private bitClass As Integer = BitVector32.CreateMask(bitSubjects)
    Private bitSchool As Integer = BitVector32.CreateMask(bitClass)
    Private bitYear As Integer = BitVector32.CreateMask(bitSchool)

    Private currentSchoolYear As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Session.Timeout = 7
        'configure the notification to automatically show 6 min before session expiration.  It will display for a total of 5 min before force terminating the session.
        RadNotification1.ShowInterval = (Session.Timeout - 6) * 60000
        'set the redirect url as a value for an easier and faster extraction in on the client
        RadNotification1.Value = Page.ResolveClientUrl("SessionExpired.aspx")

        If Not Context.User.Identity.IsAuthenticated Or (Session.IsNewSession And Not Session("JustLoggedIn")) Then
            Response.Redirect("Login.aspx")
        End If
        Session("JustLoggedIn") = False

        If Month(Now) >= 7 Then
            currentSchoolYear = Year(Now).ToString & " - " & (Year(Now) + 1).ToString
        Else
            currentSchoolYear = (Year(Now) - 1).ToString & " - " & Year(Now).ToString
        End If

        If Not ViewState.Item("SessionVars") Is Nothing Then
            bvSessionVars = New BitVector32(CType(ViewState("SessionVars"), Integer))
        End If

        If Not IsPostBack Then
            'set the expire timeout for the session
            bvSessionVars = New BitVector32(0)

            If Session.Item("cmbTeachers") Is Nothing Then
                Session.Add("cmbTeachers", "00000000-0000-0000-0000-000000000000")
            End If
            If Session.Item("cmbStudents") Is Nothing Then
                Session.Add("cmbStudents", "00000000-0000-0000-0000-000000000000")
            End If
            If Session.Item("cmbSubjects") Is Nothing Then
                Session.Add("cmbSubjects", "00000000-0000-0000-0000-000000000000")
            End If
            If Session.Item("cmbClass") Is Nothing Then
                Session.Add("cmbClass", "0")
            End If
            If Session.Item("cmbSchools") Is Nothing Then
                Session.Add("cmbSchools", "0")
            End If
            If Session.Item("cmbYear") Is Nothing Then
                Session.Add("cmbYear", currentSchoolYear)
            Else
                cmbYear.SelectedValue = Session("cmbYear").ToString
                bvSessionVars(bitYear) = True
                ViewState("SessionVars") = bvSessionVars.Data
            End If

            If Not Context.User.Identity.IsAuthenticated Then
                Response.Redirect("Login.aspx")
            End If

            If Not IsPostBack() Then
                usr_login.Value = Context.User.Identity.Name
                If Me.roleId = 1 Then admin_link.Style.Item("display") = "block"
                chkDeleted.Visible = (Me.roleId = 1)
                cmbSchools.Visible = ((Me.roleId = 1) Or (Me.roleId = 2))
                lblSchoolFilter.Visible = ((Me.roleId = 1) Or (Me.roleId = 2))
                'iep_list.Style.Item("display") = "block"
                'hdnTest.Value = "00000000-0000-0000-0000-000000000000"
                'AddPageView(tbGoalCatalog.Tabs(0))
            End If
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

    Protected Sub OnCallbackUpdate(sender As Object, e As RadNotificationEventArgs)

    End Sub

    Protected Sub cmbSubjects_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs)
        If cmbSubjects.DataTextField = "subject_text_" Then
            cmbSubjects.DataSource = Nothing
            cmbSubjects.ClearSelection()
        End If
        cmbSubjects.DataSource = GetDataTable("SELECT subject_text FROM vw_list_iep WHERE is_active = 1 AND subject_text LIKE '%" & e.Text & "%' GROUP BY subject_text ORDER BY 1")
        cmbSubjects.DataTextField = "subject_text"
        cmbSubjects.DataValueField = "subject_text"
        cmbSubjects.DataBind()
    End Sub

    Protected Sub cmbTeachers_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs)
        If cmbStudents.DataTextField = "teacher_name_" Then
            cmbStudents.DataSource = Nothing
            cmbStudents.ClearSelection()
        End If
        cmbTeachers.DataSource = GetDataTable("getTeacherList '" & System.Web.HttpContext.Current.User.Identity.Name & "', '%" & e.Text & "%', NULL")
        cmbTeachers.DataTextField = "teacher_name"
        cmbTeachers.DataValueField = "usr_uuid"
        cmbTeachers.DataBind()
    End Sub

    Protected Sub cmbStudents_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs)
        If cmbStudents.DataTextField = "student_name_" Then
            cmbStudents.DataSource = Nothing
            cmbStudents.ClearSelection()
            cmbStudents.Items.Clear()
        End If
        cmbStudents.DataSource = GetDataTable("getStudentList '" & System.Web.HttpContext.Current.User.Identity.Name & "', '%" & e.Text & "%', NULL, NULL, '" & cmbYear.SelectedValue & "'")
        cmbStudents.DataTextField = "student_name"
        cmbStudents.DataValueField = "student_uuid"
        cmbStudents.DataBind()
    End Sub

    Private Sub rgCspList_DataBound(sender As Object, e As EventArgs) Handles rgCspList.DataBound

    End Sub

    Private Sub rgCspList_DeleteCommand(sender As Object, e As GridCommandEventArgs) Handles rgCspList.DeleteCommand
        Dim strSql As String

        strSql = "UPDATE iep SET is_active = " & IIf(chkDeleted.Checked, 1, 0) & " WHERE iep_uuid = '" & e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex).Item("iep_uuid").ToString() & "'"
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        sqlConn.Open()
        Dim sqlCommand As New SqlCommand(strSql, sqlConn)

        sqlCommand.ExecuteNonQuery()

        sqlConn.Close()
    End Sub

    Private Sub rgCspList_InsertCommand(sender As Object, e As GridCommandEventArgs) Handles rgCspList.InsertCommand
        Dim MyUserControl As UserControl = CType(e.Item.FindControl(GridEditFormItem.EditFormUserControlID), UserControl)
        Dim strSql As String
        Dim lbTeachers As RadListBox = CType(MyUserControl.FindControl("lbTeachers"), RadListBox)
        Dim oItem As RadListBoxItem
        Dim intCounter As Integer
        Dim insertedUuid As String

        strSql = "doInsertIEP"
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        sqlConn.Open()

        Dim sqlComm As New SqlCommand(strSql, sqlConn)
        sqlComm.CommandType = CommandType.StoredProcedure
        sqlComm.Parameters.Add(New SqlParameter("@school_id", SqlDbType.TinyInt))
        sqlComm.Parameters("@school_id").Value = CType(MyUserControl.FindControl("cmbSchool"), RadComboBox).SelectedValue

        sqlComm.Parameters.Add(New SqlParameter("@iep_subject", SqlDbType.NVarChar))
        sqlComm.Parameters("@iep_subject").Value = CType(MyUserControl.FindControl("tbSubject"), RadTextBox).Text

        sqlComm.Parameters.Add(New SqlParameter("@iep_subject_uuid", SqlDbType.NVarChar))
        sqlComm.Parameters("@iep_subject_uuid").Value = CType(MyUserControl.FindControl("ddSubjects"), RadDropDownTree).SelectedValue

        If lbTeachers.Items.Count = 0 Then
            oItem = New RadListBoxItem
            oItem.Value = CType(MyUserControl.FindControl("cmbTeacher"), RadComboBox).SelectedValue
            oItem.Text = CType(MyUserControl.FindControl("cmbTeacher"), RadComboBox).SelectedItem.Text
            lbTeachers.Items.Add(oItem)
        End If

        For intCounter = 1 To 5
            If intCounter <= lbTeachers.Items.Count Then
                oItem = lbTeachers.Items(intCounter - 1)
                sqlComm.Parameters.Add(New SqlParameter("@teacher_" & intCounter & "_uuid", SqlDbType.UniqueIdentifier))
                sqlComm.Parameters("@teacher_" & intCounter & "_uuid").Value = New Guid(oItem.Value)
            Else
                sqlComm.Parameters.Add(New SqlParameter("@teacher_" & intCounter & "_uuid", SqlDbType.UniqueIdentifier))
                sqlComm.Parameters("@teacher_" & intCounter & "_uuid").Value = DBNull.Value
            End If
        Next

        sqlComm.Parameters.Add(New SqlParameter("@student_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@student_uuid").Value = New Guid(CType(MyUserControl.FindControl("ddStudent"), RadDropDownTree).SelectedValue)

        sqlComm.Parameters.Add(New SqlParameter("@iep_materials", SqlDbType.NVarChar))
        sqlComm.Parameters("@iep_materials").Value = CType(MyUserControl.FindControl("tbMaterials"), RadTextBox).Text

        sqlComm.Parameters.Add(New SqlParameter("@iep_year", SqlDbType.NChar, 11))
        sqlComm.Parameters("@iep_year").Value = CType(MyUserControl.FindControl("tbYear"), RadTextBox).Text

        sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@iep_uuid").Direction = ParameterDirection.Output

        sqlComm.ExecuteNonQuery()

        insertedUuid = sqlComm.Parameters("@iep_uuid").Value.ToString

        Session.Item("cmbTeachers") = cmbTeachers.SelectedValue
        Session.Item("cmbStudents") = cmbStudents.SelectedValue
        Session.Item("cmbSubjects") = cmbSubjects.SelectedValue
        Session.Item("cmbClass") = cmbClass.SelectedValue
        Session.Item("cmbSchools") = cmbSchools.SelectedValue
        Session.Item("cmbYear") = cmbYear.SelectedValue
        Response.Redirect("csp_details.aspx?iep_uuid=" & insertedUuid)

    End Sub

    Private Sub rgCspList_PreRender(sender As Object, e As EventArgs) Handles rgCspList.PreRender
        If IsNumeric(Session.Item("Page")) Then
            rgCspList.CurrentPageIndex = Session.Item("Page")
            rgCspList.Rebind()
            Session.Item("Page") = ""
        End If
        
    End Sub

    Private Sub rgCspList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rgCspList.SelectedIndexChanged
        Session.Item("cmbTeachers") = cmbTeachers.SelectedValue
        Session.Item("cmbStudents") = cmbStudents.SelectedValue
        Session.Item("cmbSubjects") = cmbSubjects.SelectedValue
        Session.Item("cmbClass") = cmbClass.SelectedValue
        Session.Item("cmbSchools") = cmbSchools.SelectedValue
        Session.Item("cmbYear") = cmbYear.SelectedValue
        Session.Item("Page") = rgCspList.CurrentPageIndex
        Response.Redirect("csp_details.aspx?iep_uuid=" & CType(sender, RadGrid).MasterTableView.DataKeyValues(CType(sender, RadGrid).SelectedItems(0).ItemIndex).Item("iep_uuid").ToString)

    End Sub

    Private Sub rgCspList_UpdateCommand(sender As Object, e As GridCommandEventArgs) Handles rgCspList.UpdateCommand
        Dim editedItem As GridEditableItem = CType(e.Item, GridEditableItem)
        Dim MyUserControl As UserControl = CType(e.Item.FindControl(GridEditFormItem.EditFormUserControlID), UserControl)
        Dim strSql As String
        Dim lbTeachers As RadListBox = CType(MyUserControl.FindControl("lbTeachers"), RadListBox)
        Dim oItem As RadListBoxItem
        Dim intCounter As Integer

        strSql = "UPDATE iep SET teacher_uuid = @teacher_1_uuid, teacher_2_uuid = @teacher_2_uuid, teacher_3_uuid = @teacher_3_uuid, teacher_4_uuid = @teacher_4_uuid, teacher_5_uuid = @teacher_5_uuid, iep_subject = @iep_subject, iep_subject_uuid = @iep_subject_uuid, iep_materials = @iep_materials WHERE iep_uuid = @iep_uuid"
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")

        sqlConn.Open()
        Dim sqlComm As New SqlCommand(strSql, sqlConn)

        sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@iep_uuid").Value = New Guid(editedItem.OwnerTableView.DataKeyValues(editedItem.ItemIndex).Item("iep_uuid").ToString())

        sqlComm.Parameters.Add(New SqlParameter("@iep_subject", SqlDbType.NVarChar))
        sqlComm.Parameters("@iep_subject").Value = CType(MyUserControl.FindControl("tbSubject"), RadTextBox).Text

        sqlComm.Parameters.Add(New SqlParameter("@iep_subject_uuid", SqlDbType.NVarChar))
        sqlComm.Parameters("@iep_subject_uuid").Value = CType(MyUserControl.FindControl("ddSubjects"), RadDropDownTree).SelectedValue

        If lbTeachers.Items.Count = 0 Then
            oItem = New RadListBoxItem
            oItem.Value = CType(MyUserControl.FindControl("cmbTeacher"), RadComboBox).SelectedValue
            oItem.Text = CType(MyUserControl.FindControl("cmbTeacher"), RadComboBox).SelectedItem.Text
            lbTeachers.Items.Add(oItem)
        End If

        For intCounter = 1 To 5
            If intCounter <= lbTeachers.Items.Count Then
                oItem = lbTeachers.Items(intCounter - 1)
                sqlComm.Parameters.Add(New SqlParameter("@teacher_" & intCounter & "_uuid", SqlDbType.UniqueIdentifier))
                sqlComm.Parameters("@teacher_" & intCounter & "_uuid").Value = New Guid(oItem.Value)
            Else
                sqlComm.Parameters.Add(New SqlParameter("@teacher_" & intCounter & "_uuid", SqlDbType.UniqueIdentifier))
                sqlComm.Parameters("@teacher_" & intCounter & "_uuid").Value = DBNull.Value
            End If
        Next

        sqlComm.Parameters.Add(New SqlParameter("@iep_materials", SqlDbType.NVarChar))
        sqlComm.Parameters("@iep_materials").Value = CType(MyUserControl.FindControl("tbMaterials"), RadTextBox).Text

        sqlComm.ExecuteNonQuery()

        sqlConn.Close()

    End Sub

    Private Sub cmbClass_DataBound(sender As Object, e As EventArgs) Handles cmbClass.DataBound
        If Not Session("cmbClass") Is Nothing Then cmbClass.SelectedValue = Session("cmbClass").ToString
        bvSessionVars(bitClass) = True
        ViewState("SessionVars") = bvSessionVars.Data
    End Sub

    Private Sub cmbClass_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs) Handles cmbClass.SelectedIndexChanged
        Session("cmbClass") = cmbClass.SelectedValue
    End Sub

    Private Sub cmbSchools_DataBound(sender As Object, e As EventArgs) Handles cmbSchools.DataBound
        'cmbSchools.SelectedValue = Session("cmbSchools").ToString
        'bvSessionVars(bitSchool) = True
        'ViewState("SessionVars") = bvSessionVars.Data
    End Sub

    Private Sub RefreshCSPList()
        'If 
    End Sub

    Protected Sub cmbTeachers_DataBound(sender As Object, e As EventArgs)
        'cmbTeachers.SelectedValue = Session("cmbTeachers").ToString
        'bvSessionVars(bitTeacher) = True
        'ViewState("SessionVars") = bvSessionVars.Data
    End Sub

    Protected Sub cmbStudents_DataBound(sender As Object, e As EventArgs)
        'cmbStudents.SelectedValue = Session("cmbStudents").ToString
        'bvSessionVars(bitStudents) = True
        'ViewState("SessionVars") = bvSessionVars.Data
    End Sub

    Protected Sub cmbSubjects_DataBound(sender As Object, e As EventArgs)
        'cmbSubjects.SelectedValue = Session("cmbSubjects").ToString
        'bvSessionVars(bitSubjects) = True
        'ViewState("SessionVars") = bvSessionVars.Data
    End Sub

    Protected Sub cmbSubjects_Load(sender As Object, e As EventArgs)
        If Not bvSessionVars(bitSubjects) And Session("cmbSubjects") <> "00000000-0000-0000-0000-000000000000" Then
            cmbSubjects.DataSource = GetDataTable("SELECT '00000000-0000-0000-0000-000000000000' AS subject_text, '' AS subject_text_ UNION ALL SELECT subject_text, subject_text AS subject_text_ FROM vw_list_iep WHERE is_active = 1 AND subject_text LIKE '%" & Session("cmbSubjects").ToString & "%' GROUP BY subject_text ORDER BY 1")
            cmbSubjects.DataTextField = "subject_text_"
            cmbSubjects.DataValueField = "subject_text"
            cmbSubjects.DataBind()
            cmbSubjects.SelectedValue = Session("cmbSubjects").ToString
        End If
        bvSessionVars(bitSubjects) = True
        ViewState("SessionVars") = bvSessionVars.Data
    End Sub

    Protected Sub cmbStudents_Load(sender As Object, e As EventArgs)
        If Not bvSessionVars(bitStudents) And Session("cmbStudents") <> "00000000-0000-0000-0000-000000000000" Then
            cmbStudents.DataSource = GetDataTable("SELECT '00000000-0000-0000-0000-000000000000' AS student_uuid, '' AS student_name_ UNION ALL SELECT student_uuid, student_first_name + N' ' + student_last_name AS student_name_ FROM students WHERE student_uuid = '" & Session("cmbStudents") & "'")
            cmbStudents.DataTextField = "student_name_"
            cmbStudents.DataValueField = "student_uuid"
            cmbStudents.DataBind()
            cmbStudents.SelectedValue = Session("cmbStudents").ToString
        End If
        bvSessionVars(bitStudents) = True
        ViewState("SessionVars") = bvSessionVars.Data
    End Sub

    Protected Sub cmbTeachers_Load(sender As Object, e As EventArgs)
        If Not bvSessionVars(bitTeacher) And Session("cmbTeachers") <> "00000000-0000-0000-0000-000000000000" Then
            cmbTeachers.DataSource = GetDataTable("SELECT '00000000-0000-0000-0000-000000000000' AS usr_uuid, '' AS teacher_name_ UNION ALL SELECT usr_uuid, usr_display_name AS teacher_name_ FROM usrs WHERE usr_uuid = '" & Session("cmbTeachers") & "'")
            cmbTeachers.DataTextField = "teacher_name_"
            cmbTeachers.DataValueField = "usr_uuid"
            cmbTeachers.DataBind()
            cmbTeachers.SelectedValue = Session("cmbTeachers").ToString
        End If
        bvSessionVars(bitTeacher) = True
        ViewState("SessionVars") = bvSessionVars.Data
    End Sub

    Protected Sub cmbSchools_Load(sender As Object, e As EventArgs)
        If Not bvSessionVars(bitSchool) And Session("cmbSchools") <> "0" Then
            'cmbSchools.DataSource = GetDataTable("getSchoolList @usr_login = '" & Context.User.Identity.Name & "'")
            'cmbSchools.DataTextField = "school_name"
            'cmbSchools.DataValueField = "school_id"
            cmbSchools.DataBind()
            cmbSchools.SelectedValue = Session("cmbSchools").ToString
        End If
        bvSessionVars(bitSchool) = True
        ViewState("SessionVars") = bvSessionVars.Data
    End Sub

    Protected Sub cmbYear_SelectedIndexChanged(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)
        If cmbStudents.Text <> "" Then
            Dim strStudent As String = cmbStudents.Text

            If cmbStudents.DataTextField = "student_name_" Then
                cmbStudents.DataSource = Nothing
                cmbStudents.ClearSelection()
                cmbStudents.Items.Clear()
            End If

            cmbStudents.DataSource = GetDataTable("getStudentList '" & System.Web.HttpContext.Current.User.Identity.Name & "', '%" & strStudent & "%', NULL, NULL, '" & e.Value & "', 1")
            cmbStudents.DataTextField = "student_name"
            cmbStudents.DataValueField = "student_uuid"
            cmbStudents.DataBind()
            If cmbStudents.Items.Count > 1 Then cmbStudents.Text = strStudent
            'rgCspList.DataBind()
        End If
    End Sub

End Class