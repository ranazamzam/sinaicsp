Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Web
Imports System.Web.SessionState
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.HtmlControls

Imports Telerik.Web.UI
Imports System.Configuration

Public Class eiep_main
    Inherits userSession

    Private _intSelected As Integer

    Public Property intSelected As Integer
        Get
            Return Me._intSelected
        End Get
        Set(ByVal value As Integer)
            Me._intSelected = value
        End Set
    End Property


    Private hTest As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not Context.User.Identity.IsAuthenticated Then
            Response.Redirect("Login.aspx")
        End If

        If Not IsPostBack() Then
            usr_login.Value = Context.User.Identity.Name
            If Me.roleId = 1 Then admin_link.Style.Item("display") = "block"
            chkDeleted.Visible = (Me.roleId = 1)
            cmbSchools.Visible = ((Me.roleId = 1) Or (Me.roleId = 2))
            lblSchoolFilter.Visible = ((Me.roleId = 1) Or (Me.roleId = 2))
            iep_list.Style.Item("display") = "block"
            hdnTest.Value = "00000000-0000-0000-0000-000000000000"
            AddPageView(tbGoalCatalogue.Tabs(0))
        End If

        iep_selected.Value = intSelected
    End Sub

    Protected Sub RadGrid1_InsertCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles RadGrid1.InsertCommand

        Dim MyUserControl As UserControl = CType(e.Item.FindControl(GridEditFormItem.EditFormUserControlID), UserControl)
        Dim strSql As String
        Dim lbTeachers As RadListBox = CType(MyUserControl.FindControl("lbTeachers"), RadListBox)
        Dim oItem As RadListBoxItem
        Dim intCounter As Integer

        'strSql = "INSERT INTO [dbo].[iep] ([school_id], [iep_subject], [teacher_uuid], [student_uuid], [iep_materials], [iep_year]) " _
        '       & "VALUES(@school_id, @iep_subject, @teacher_uuid, @student_uuid, @iep_materials, @iep_year)"
        strSql = "doInsertIEP" ' @school_id, @iep_subject, @teacher_uuid, @student_uuid, @iep_materials, @iep_year, @iep_uuid OUT"
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        sqlConn.Open()

        Dim sqlComm As New SqlCommand(strSql, sqlConn)
        sqlComm.CommandType = CommandType.StoredProcedure
        sqlComm.Parameters.Add(New SqlParameter("@school_id", SqlDbType.TinyInt))
        sqlComm.Parameters("@school_id").Value = CType(MyUserControl.FindControl("cmbSchool"), RadComboBox).SelectedValue

        sqlComm.Parameters.Add(New SqlParameter("@iep_subject", SqlDbType.NVarChar))
        sqlComm.Parameters("@iep_subject").Value = CType(MyUserControl.FindControl("txtSubject"), TextBox).Text

        sqlComm.Parameters.Add(New SqlParameter("@iep_subject_uuid", SqlDbType.NVarChar))
        sqlComm.Parameters("@iep_subject_uuid").Value = CType(MyUserControl.FindControl("ddSubjects"), RadDropDownTree).SelectedValue

        'sqlComm.Parameters.Add(New SqlParameter("@teacher_uuid", SqlDbType.UniqueIdentifier))
        'sqlComm.Parameters("@teacher_uuid").Value = New Guid(CType(MyUserControl.FindControl("cmbTeacher"), RadComboBox).SelectedValue)

        If lbTeachers.FindItemByValue(CType(MyUserControl.FindControl("cmbTeacher"), RadComboBox).SelectedValue) Is Nothing Then
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
        sqlComm.Parameters("@iep_materials").Value = CType(MyUserControl.FindControl("txtMaterials"), TextBox).Text

        sqlComm.Parameters.Add(New SqlParameter("@iep_year", SqlDbType.NChar, 11))
        sqlComm.Parameters("@iep_year").Value = CType(MyUserControl.FindControl("txtYear"), TextBox).Text

        sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@iep_uuid").Direction = ParameterDirection.Output

        sqlComm.ExecuteNonQuery()

        hdnTest.Value = sqlComm.Parameters("@iep_uuid").Value.ToString

        strSql = "getPageNum"
        sqlComm = New SqlCommand(strSql, sqlConn)
        sqlComm.CommandType = CommandType.StoredProcedure

        sqlComm.Parameters.Add(New SqlParameter("@usr_login", SqlDbType.NVarChar))
        sqlComm.Parameters("@usr_login").Value = Context.User.Identity.Name

        sqlComm.Parameters.Add(New SqlParameter("@student_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@student_uuid").Value = New Guid(cmbStudents.SelectedValue)

        sqlComm.Parameters.Add(New SqlParameter("@teacher_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@teacher_uuid").Value = New Guid(cmbTeachers.SelectedValue)

        sqlComm.Parameters.Add(New SqlParameter("@school_id", SqlDbType.TinyInt))
        sqlComm.Parameters("@school_id").Value = cmbSchools.SelectedValue

        sqlComm.Parameters.Add(New SqlParameter("@subject_text", SqlDbType.NVarChar))
        sqlComm.Parameters("@subject_text").Value = cmbSubjects.SelectedValue

        sqlComm.Parameters.Add(New SqlParameter("@current_placement", SqlDbType.NVarChar))
        sqlComm.Parameters("@current_placement").Value = cmbClass.SelectedValue

        sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@iep_uuid").Value = New Guid(hdnTest.Value)

        sqlComm.Parameters.Add(New SqlParameter("@items_per_page", SqlDbType.TinyInt))
        sqlComm.Parameters("@items_per_page").Value = 20

        sqlComm.Parameters.Add(New SqlParameter("@page_num", SqlDbType.TinyInt))
        sqlComm.Parameters("@page_num").Direction = ParameterDirection.Output

        sqlComm.ExecuteNonQuery()

        hdnPage.Value = sqlComm.Parameters("@page_num").Value
        
        ViewState("hdnTest") = sqlComm.Parameters("@iep_uuid").Value.ToString
        ViewState("lblStudent") = CType(MyUserControl.FindControl("ddStudent"), RadDropDownTree).SelectedValue
        ViewState("lblSchool") = CType(MyUserControl.FindControl("cmbSchool"), RadComboBox).SelectedValue
        ViewState("lblTeacher") = CType(MyUserControl.FindControl("cmbTeacher"), RadComboBox).SelectedValue
        ViewState("lblSubject") = CType(MyUserControl.FindControl("txtSubject"), TextBox).Text
        ViewState("lblMats") = CType(MyUserControl.FindControl("txtMaterials"), TextBox).Text
        ViewState("lblYr") = CType(MyUserControl.FindControl("txtYear"), TextBox).Text

        RadGrid2.Rebind()
        rgComments.Rebind()
        
        sqlConn.Close()

    End Sub

    Private Sub RadGrid1_ItemCreated(sender As Object, e As GridItemEventArgs) Handles RadGrid1.ItemCreated
        'Dim item As Telerik.Web.UI.GridDataItem
        Dim grid As RadGrid = TryCast(RadGrid1, RadGrid)

        If ViewState("hdnTest") <> "" Then
            lblStudent.Text = ViewState("lblStudent")
            lblSchool.Text = ViewState("lblSchool")
            lblTeacher.Text = ViewState("lblTeacher")
            lblSubject.Text = ViewState("lblSubject")
            lblMats.Text = ViewState("lblMats")
            lblYr.Text = ViewState("lblYear")
            RadGrid2.Rebind()
            ViewState("hdnTest") = ""
        End If

        'If e.Item.ItemType = GridItemType.Item Or e.Item.ItemType = GridItemType.AlternatingItem Then
        'item = e.Item
        'Debug.Print("Hidden1: " & ViewState("hdnTest"))
        'Debug.Print("Hidden2: " & hdnTest.Value)
        'End If
    End Sub

    Private Sub RadGrid1_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles RadGrid1.ItemDataBound

    End Sub

    Protected Sub RadGrid1_UpdateCommand(ByVal source As Object, ByVal e As Telerik.Web.UI.GridCommandEventArgs) Handles RadGrid1.UpdateCommand

        Dim editedItem As GridEditableItem = CType(e.Item, GridEditableItem)
        Dim MyUserControl As UserControl = CType(e.Item.FindControl(GridEditFormItem.EditFormUserControlID), UserControl)
        Dim strSql As String
        Dim lbTeachers As RadListBox = CType(MyUserControl.FindControl("lbTeachers"), RadListBox)
        Dim oItem As RadListBoxItem
        Dim intCounter As Integer

        'If editedItem.OwnerTableView.DataKeyValues(editedItem.ItemIndex).Item("usr_uuid").ToString() <> CType(MyUserControl.FindControl("ddlTeacher"), DropDownList).SelectedItem.Value Then
        'strSql = "UPDATE iep SET teacher_uuid = '" & CType(MyUserControl.FindControl("ddlTeacher"), DropDownList).SelectedItem.Value & "' WHERE iep_uuid = '" & editedItem.OwnerTableView.DataKeyValues(editedItem.ItemIndex).Item("iep_uuid").ToString() & "'"
        strSql = "UPDATE iep SET teacher_uuid = @teacher_1_uuid, teacher_2_uuid = @teacher_2_uuid, teacher_3_uuid = @teacher_3_uuid, teacher_4_uuid = @teacher_4_uuid, teacher_5_uuid = @teacher_5_uuid, iep_subject = @iep_subject, iep_subject_uuid = @iep_subject_uuid, iep_materials = @iep_materials WHERE iep_uuid = @iep_uuid"
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")

        sqlConn.Open()
        Dim sqlComm As New SqlCommand(strSql, sqlConn)

        sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@iep_uuid").Value = New Guid(editedItem.OwnerTableView.DataKeyValues(editedItem.ItemIndex).Item("iep_uuid").ToString())

        sqlComm.Parameters.Add(New SqlParameter("@iep_subject", SqlDbType.NVarChar))
        sqlComm.Parameters("@iep_subject").Value = CType(MyUserControl.FindControl("txtSubject"), TextBox).Text

        sqlComm.Parameters.Add(New SqlParameter("@iep_subject_uuid", SqlDbType.NVarChar))
        sqlComm.Parameters("@iep_subject_uuid").Value = CType(MyUserControl.FindControl("ddSubjects"), RadDropDownTree).SelectedValue

        'sqlComm.Parameters.Add(New SqlParameter("@teacher_uuid", SqlDbType.UniqueIdentifier))
        'sqlComm.Parameters("@teacher_uuid").Value = New Guid(CType(MyUserControl.FindControl("cmbTeacher"), RadComboBox).SelectedValue)

        If lbTeachers.FindItemByValue(CType(MyUserControl.FindControl("cmbTeacher"), RadComboBox).SelectedValue) Is Nothing Then
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
        sqlComm.Parameters("@iep_materials").Value = CType(MyUserControl.FindControl("txtMaterials"), TextBox).Text

        sqlComm.ExecuteNonQuery()

        sqlConn.Close()
        'End If

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

    Protected Sub RadGrid2_NeedDataSource(ByVal source As Object, ByVal e As GridNeedDataSourceEventArgs)
        'Dim grid As RadGrid = TryCast(source, RadGrid)
        'If iep_uuid.Value <> "" Then
        'Grid.DataSource = GetDataTable("SELECT * FROM vw_get_iep_data WHERE iep_uuid = '" + iep_uuid.Value + "' ORDER BY iep_data_sequence, iep_sub_data_sequence")
        'Else
        'Dim dt As New DataTable()
        'Grid.DataSource = dt
        'End If
    End Sub

    Protected Sub RadAjaxManager1_AjaxRequest(ByVal sender As Object, ByVal e As AjaxRequestEventArgs)
        Dim grid As RadGrid = TryCast(RadGrid2, RadGrid)
        Dim strSql As String

        If e.Argument.IndexOf("deleteSelected") <> -1 Then
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            sqlConn.Open()

            doSnapShot(RadGrid2.MasterTableView.DataKeyValues(0).Item("iep_uuid").ToString)

            For Each oSelected As GridDataItem In grid.SelectedItems
                strSql = "deleteGoal '" & oSelected.GetDataKeyValue("iep_data_uuid").ToString() & "'"
                Dim sqlComm As New SqlCommand(strSql, sqlConn)

                sqlComm.ExecuteNonQuery()
            Next

            sqlConn.Close()

            RadGrid2.Rebind()
        ElseIf e.Argument.IndexOf("Undo") = 0 Then
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            sqlConn.Open()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            strSql = "iepUndo @iep_uuid"

            sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters("@iep_uuid").Value = New Guid(hdnTest.Value)
            sqlComm.CommandText = strSql
            sqlComm.ExecuteNonQuery()
            sqlConn.Close()

            RadGrid2.MasterTableView.Rebind()

        ElseIf Left(e.Argument, 4) = "iep_" Then
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            sqlConn.Open()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            strSql = "UPDATE iep SET " & e.Argument & " = @text WHERE iep_uuid = @iep_uuid"

            sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters("@iep_uuid").Value = New Guid(iep_uuid.Value)

            sqlComm.Parameters.Add(New SqlParameter("@text", SqlDbType.NVarChar))
            Select Case e.Argument
                Case "iep_comments"
                    sqlComm.Parameters("@text").Value = reComments.GetHtml(EditorStripHtmlOptions.None)
                Case "iep_feb_notes"
                    sqlComm.Parameters("@text").Value = reFNotes.GetHtml(EditorStripHtmlOptions.None)
                Case "iep_june_notes"
                    sqlComm.Parameters("@text").Value = reJNotes.GetHtml(EditorStripHtmlOptions.None)
            End Select

            sqlComm.CommandText = strSql
            sqlComm.ExecuteNonQuery()

            sqlConn.Close()
            rgComments.Rebind()
        ElseIf e.Argument.IndexOf("HandleSubItem") = 0 Then
            doSnapShot(hdnTest.Value)

            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            sqlConn.Open()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            strSql = "doSubGoal @iep_data_uuid"

            sqlComm.Parameters.Add(New SqlParameter("@iep_data_uuid", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters("@iep_data_uuid").Value = New Guid(Replace(e.Argument, "HandleSubItem|", ""))

            sqlComm.CommandText = strSql
            sqlComm.ExecuteNonQuery()

            sqlConn.Close()
            RadGrid2.Rebind()
        ElseIf e.Argument.IndexOf("LockIEP") = 0 Then
            If rgComments.Items.Count > 0 Then
                doSnapShot(hdnTest.Value, 1)
                Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
                sqlConn.Open()
                Dim sqlComm As New SqlCommand
                sqlComm.Connection = sqlConn
                strSql = "doLockIep @iep_uuid, @usr_uuid, 1"

                sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
                sqlComm.Parameters("@iep_uuid").Value = New Guid(rgComments.Items(0).GetDataKeyValue("iep_uuid").ToString)

                sqlComm.Parameters.Add(New SqlParameter("@usr_uuid", SqlDbType.UniqueIdentifier))
                sqlComm.Parameters("@usr_uuid").Value = New Guid(usrUuid)

                sqlComm.CommandText = strSql
                sqlComm.ExecuteNonQuery()

                sqlConn.Close()
            End If
        ElseIf e.Argument.IndexOf("UnLockIEP") = 0 Then
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")

            sqlConn.Open()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            strSql = "doLockIep @iep_uuid, @usr_uuid, 0"

            sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters("@iep_uuid").Value = DBNull.Value

            sqlComm.Parameters.Add(New SqlParameter("@usr_uuid", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters("@usr_uuid").Value = New Guid(usrUuid)

            sqlComm.CommandText = strSql
            sqlComm.ExecuteNonQuery()

            sqlConn.Close()
        ElseIf e.Argument.IndexOf("ClipboardPaste") = 0 Then
            doSnapShot(hdnTest.Value)
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            Dim strDataText As String = RadEditor1.GetHtml(EditorStripHtmlOptions.None)
            Dim strGoals() As String = strDataText.Split("~")
            Dim intCounter As Integer
            Dim sqlComm As SqlCommand

            sqlConn.Open()

            For intCounter = UBound(strGoals) To 1 Step -1
                sqlComm = New SqlCommand
                sqlComm.Connection = sqlConn

                If UCase(strGoals(intCounter)) Like "[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F]" Then
                    strSql = "doCloneIEP @iep_src_uuid, @iep_dest_uuid"
                    sqlComm.Parameters.Add(New SqlParameter("@iep_dest_uuid", SqlDbType.UniqueIdentifier))
                    sqlComm.Parameters("@iep_dest_uuid").Value = New Guid(iep_uuid.Value)

                    sqlComm.Parameters.Add(New SqlParameter("@iep_src_uuid", SqlDbType.UniqueIdentifier))
                    sqlComm.Parameters("@iep_src_uuid").Value = New Guid(strGoals(intCounter))
                Else
                    If Left(strGoals(intCounter), Len("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")) = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Then
                        strGoals(intCounter) = strGoals(intCounter).Replace("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "")
                        strSql = "doInsertSubGoal @iep_uuid, @iep_data_text, @iep_data_dest_uuid"
                    Else
                        strSql = "doInsertGoal @iep_uuid, @iep_data_text, @iep_data_dest_uuid"
                    End If
                    sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
                    sqlComm.Parameters("@iep_uuid").Value = New Guid(iep_uuid.Value)

                    sqlComm.Parameters.Add(New SqlParameter("@iep_data_dest_uuid", SqlDbType.UniqueIdentifier))
                    If RadGrid2.SelectedItems.Count = 1 Then
                        sqlComm.Parameters("@iep_data_dest_uuid").Value = New Guid(CType(RadGrid2.SelectedItems(0), GridDataItem).GetDataKeyValue("iep_data_uuid").ToString)
                    Else
                        sqlComm.Parameters("@iep_data_dest_uuid").Value = New Guid(CType(RadGrid2.Items(RadGrid2.Items.Count - 1), GridDataItem).GetDataKeyValue("iep_data_uuid").ToString)
                    End If
                    'sqlComm.Parameters("@iep_data_dest_uuid").Value = DBNull.Value

                    sqlComm.Parameters.Add(New SqlParameter("@iep_data_text", SqlDbType.NVarChar))
                    sqlComm.Parameters("@iep_data_text").Value = strGoals(intCounter)
                End If

                sqlComm.CommandText = strSql
                sqlComm.ExecuteNonQuery()
            Next
            sqlConn.Close()
        ElseIf e.Argument.IndexOf("doBulkSubGoal") = 0 Then
            doSnapShot(hdnTest.Value)
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            Dim oItem As GridItem
            Dim sqlComm As SqlCommand

            sqlConn.Open()

            For Each oItem In RadGrid2.SelectedItems ' intCounter = 1 To UBound(strGoals)

                If CType(oItem.Cells(4).Controls(0), ImageButton).ImageUrl = "images/arrow_right.gif" Then
                    sqlComm = New SqlCommand
                    sqlComm.Connection = sqlConn

                    strSql = "doSubGoal @iep_data_uuid"
                    'oItem.Cells(2)
                    sqlComm.Parameters.Add(New SqlParameter("@iep_data_uuid", SqlDbType.UniqueIdentifier))
                    sqlComm.Parameters("@iep_data_uuid").Value = New Guid(RadGrid2.MasterTableView.DataKeyValues(oItem.DataSetIndex).Item("iep_data_uuid").ToString)

                    sqlComm.CommandText = strSql
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            sqlConn.Close()
        ElseIf e.Argument.IndexOf("doBulkGoal") = 0 Then
            doSnapShot(hdnTest.Value)
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            Dim oItem As GridItem
            Dim sqlComm As SqlCommand

            sqlConn.Open()

            For Each oItem In RadGrid2.SelectedItems ' intCounter = 1 To UBound(strGoals)

                If CType(oItem.Cells(4).Controls(0), ImageButton).ImageUrl = "images/arrow_left.gif" Then
                    sqlComm = New SqlCommand
                    sqlComm.Connection = sqlConn

                    strSql = "doSubGoal @iep_data_uuid"
                    'oItem.Cells(2)
                    sqlComm.Parameters.Add(New SqlParameter("@iep_data_uuid", SqlDbType.UniqueIdentifier))
                    sqlComm.Parameters("@iep_data_uuid").Value = New Guid(RadGrid2.MasterTableView.DataKeyValues(oItem.DataSetIndex).Item("iep_data_uuid").ToString)

                    sqlComm.CommandText = strSql
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            sqlConn.Close()
        ElseIf e.Argument.IndexOf("") <> -1 Then
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            sqlConn.Open()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn

            Dim strDataText As String = RadEditor1.GetHtml(EditorStripHtmlOptions.None)
            If UCase(strDataText) Like "GC_[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F]" Then
                doSnapShot(hdnTest.Value)

                strSql = "doCloneIEP_New @iep_src_uuid, @iep_dest_uuid, 1"
                'strSql = "INSERT INTO [dbo].[iep_data] ([iep_uuid], [iep_data_sequence], [iep_data_text], [iep_date_initiated]) " _
                '            & "SELECT @iep_uuid, iep_data_sequence + (SELECT ISNULL(MAX(iep_data_sequence), 0) + 1 FROM iep_data WHERE iep_uuid = @iep_uuid), iep_data_text, iep_date_initiated FROM iep_data WHERE iep_data.iep_uuid = @iep_src_uuid"
                '& "VALUES(@iep_uuid, (SELECT ISNULL(MAX(iep_data_sequence), 0) + 1 FROM iep_data WHERE iep_uuid = @iep_uuid), @iep_data_text, CONVERT(NVARCHAR, MONTH(GETDATE())) + N'/' + RIGHT(CONVERT(NVARCHAR, YEAR(GETDATE())), 2))"

                sqlComm.Parameters.Add(New SqlParameter("@iep_src_uuid", SqlDbType.NVarChar))
                sqlComm.Parameters("@iep_src_uuid").Value = Replace(strDataText, "gc_", "")

                sqlComm.Parameters.Add(New SqlParameter("@iep_dest_uuid", SqlDbType.UniqueIdentifier))
                sqlComm.Parameters("@iep_dest_uuid").Value = New Guid(iep_uuid.Value)
                '8-4-4-4-12
            ElseIf UCase(strDataText) Like "[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F]" And Len(strDataText) Then
                doSnapShot(hdnTest.Value)

                strSql = "doCloneIEP @iep_src_uuid, @iep_dest_uuid"
                'strSql = "INSERT INTO [dbo].[iep_data] ([iep_uuid], [iep_data_sequence], [iep_data_text], [iep_date_initiated]) " _
                '            & "SELECT @iep_uuid, iep_data_sequence + (SELECT ISNULL(MAX(iep_data_sequence), 0) + 1 FROM iep_data WHERE iep_uuid = @iep_uuid), iep_data_text, iep_date_initiated FROM iep_data WHERE iep_data.iep_uuid = @iep_src_uuid"
                '& "VALUES(@iep_uuid, (SELECT ISNULL(MAX(iep_data_sequence), 0) + 1 FROM iep_data WHERE iep_uuid = @iep_uuid), @iep_data_text, CONVERT(NVARCHAR, MONTH(GETDATE())) + N'/' + RIGHT(CONVERT(NVARCHAR, YEAR(GETDATE())), 2))"

                sqlComm.Parameters.Add(New SqlParameter("@iep_src_uuid", SqlDbType.NVarChar))
                sqlComm.Parameters("@iep_src_uuid").Value = strDataText

                sqlComm.Parameters.Add(New SqlParameter("@iep_dest_uuid", SqlDbType.UniqueIdentifier))
                sqlComm.Parameters("@iep_dest_uuid").Value = New Guid(iep_uuid.Value)
            ElseIf e.Argument.IndexOf("doSubGoal") = 0 Then
                doSnapShot(hdnTest.Value)

                strSql = "doInsertSubGoal @iep_uuid, @iep_data_text, @iep_data_dest_uuid"

                sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
                sqlComm.Parameters("@iep_uuid").Value = New Guid(iep_uuid.Value)

                sqlComm.Parameters.Add(New SqlParameter("@iep_data_text", SqlDbType.NVarChar))
                sqlComm.Parameters("@iep_data_text").Value = SentenceCase(strDataText)

                sqlComm.Parameters.Add(New SqlParameter("@iep_data_dest_uuid", SqlDbType.UniqueIdentifier))
                If RadGrid2.SelectedItems.Count = 1 Then
                    sqlComm.Parameters("@iep_data_dest_uuid").Value = New Guid(CType(RadGrid2.SelectedItems(0), GridDataItem).GetDataKeyValue("iep_data_uuid").ToString)
                Else
                    sqlComm.Parameters("@iep_data_dest_uuid").Value = DBNull.Value
                End If
            ElseIf iep_data_uuid.Value.ToString = "" Then
                doSnapShot(hdnTest.Value)

                strSql = "doInsertGoal @iep_uuid, @iep_data_text, @iep_data_dest_uuid"
                'strSql = "INSERT INTO [dbo].[iep_data] ([iep_uuid], [iep_data_sequence], [iep_data_text]) " _
                '           & "VALUES(@iep_uuid, (SELECT ISNULL(MAX(iep_data_sequence), 0) + 1 FROM iep_data WHERE iep_uuid = @iep_uuid), @iep_data_text)"

                sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
                sqlComm.Parameters("@iep_uuid").Value = New Guid(iep_uuid.Value)

                sqlComm.Parameters.Add(New SqlParameter("@iep_data_text", SqlDbType.NVarChar))
                sqlComm.Parameters("@iep_data_text").Value = SentenceCase(strDataText)

                sqlComm.Parameters.Add(New SqlParameter("@iep_data_dest_uuid", SqlDbType.UniqueIdentifier))
                If RadGrid2.SelectedItems.Count = 1 Then
                    sqlComm.Parameters("@iep_data_dest_uuid").Value = New Guid(CType(RadGrid2.SelectedItems(0), GridDataItem).GetDataKeyValue("iep_data_uuid").ToString)
                Else
                    sqlComm.Parameters("@iep_data_dest_uuid").Value = DBNull.Value
                End If
            Else
                doSnapShot(hdnTest.Value)

                strSql = "UPDATE iep_data SET iep_data_text = @iep_data_text WHERE iep_data_uuid = @iep_data_uuid"

                sqlComm.Parameters.Add(New SqlParameter("@iep_data_uuid", SqlDbType.UniqueIdentifier))
                sqlComm.Parameters("@iep_data_uuid").Value = New Guid(iep_data_uuid.Value)

                sqlComm.Parameters.Add(New SqlParameter("@iep_data_text", SqlDbType.NVarChar))
                sqlComm.Parameters("@iep_data_text").Value = SentenceCase(strDataText)
            End If


            sqlComm.CommandText = strSql
            sqlComm.ExecuteNonQuery()

            sqlConn.Close()
            hdnAddNew.Value = 1
            RadGrid2.MasterTableView.Rebind()
        End If
    End Sub

    Private Sub RadGrid2_DataBound(sender As Object, e As EventArgs) Handles RadGrid2.DataBound
        'If RadGrid1.SelectedItems.Count >= 1 Then
        '    iep_list.Attributes.Add("style", "display:none")
        'End If
    End Sub

    Protected Sub RadGrid2_ItemCreated(ByVal sender As Object, ByVal e As Telerik.Web.UI.GridItemEventArgs) Handles RadGrid2.ItemCreated
        Dim item As Telerik.Web.UI.GridDataItem
        If e.Item.ItemType = GridItemType.Item Or e.Item.ItemType = GridItemType.AlternatingItem Then
            item = e.Item
            If Not IsDBNull(item.GetDataKeyValue("iep_data_parent_uuid")) Then
                CType(item("SubGoal").Controls(0), ImageButton).ImageUrl = "images/arrow_left.gif"
                'item("iep_data_text").ForeColor = Color.Red
            End If
        End If
    End Sub

    Protected Sub RadGrid1_DeleteCommand(sender As Object, e As GridCommandEventArgs)

        Dim strSql As String

        strSql = "UPDATE iep SET is_active = " & IIf(chkDeleted.Checked, 1, 0) & " WHERE iep_uuid = '" & e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex).Item("iep_uuid").ToString() & "'"
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        sqlConn.Open()
        Dim sqlCommand As New SqlCommand(strSql, sqlConn)

        sqlCommand.ExecuteNonQuery()

        sqlConn.Close()

    End Sub

    Protected Sub RadGrid2_DeleteCommand(sender As Object, e As GridCommandEventArgs)
        Dim strSql As String

        doSnapShot(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex).Item("iep_uuid").ToString)

        strSql = "deleteGoal '" & e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex).Item("iep_data_uuid").ToString() & "'"
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        sqlConn.Open()
        Dim sqlComm As New SqlCommand(strSql, sqlConn)

        sqlComm.ExecuteNonQuery()

        sqlConn.Close()

        RadGrid2.Rebind()

    End Sub

    Protected Sub RadGrid2_UpdateCommand(sender As Object, e As GridCommandEventArgs)

        Dim editedItems As GridItemCollection = CType(e.Item.OwnerTableView.ChildEditItems, GridItemCollection)
        Dim editedItem As GridEditableItem = CType(e.Item, GridEditableItem)
        Dim newValues As New Hashtable()
        Dim newValue As DictionaryEntry
        Dim strSql As String
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        sqlConn.Open()
        Dim sqlComm As New SqlCommand
        sqlComm.Connection = sqlConn

        sqlComm.Parameters.Add(New SqlParameter("@iep_data_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters.Add(New SqlParameter("@iep_data_grade_1_id", SqlDbType.TinyInt))
        sqlComm.Parameters.Add(New SqlParameter("@iep_data_grade_2_id", SqlDbType.TinyInt))
        sqlComm.Parameters.Add(New SqlParameter("@iep_data_grade_3_id", SqlDbType.TinyInt))
        sqlComm.Parameters.Add(New SqlParameter("@iep_date_initiated", SqlDbType.NVarChar))

        If editedItem.ItemIndex = 0 Then
            doSnapShot(editedItem.GetDataKeyValue("iep_uuid").ToString)
        End If

        'For Each editedItem In editedItems
        editedItem.OwnerTableView.ExtractValuesFromItem(newValues, editedItem)
        For Each newValue In newValues
            'If Not IsNothing(newValue.Value) Then
            If newValue.Value <> editedItem.SavedOldValues(newValue.Key) Then
                strSql = "UPDATE [dbo].[iep_data] SET [iep_date_initiated] = @iep_date_initiated, [iep_data_grade_1_id] = @iep_data_grade_1_id, [iep_data_grade_2_id] = @iep_data_grade_2_id, [iep_data_grade_3_id] = @iep_data_grade_3_id WHERE [iep_data_uuid] = @iep_data_uuid"
                sqlComm.CommandText = strSql

                sqlComm.Parameters("@iep_data_uuid").Value = New Guid(editedItem.GetDataKeyValue("iep_data_uuid").ToString)

                sqlComm.Parameters("@iep_data_grade_1_id").Value = DBNull.Value
                If Not IsNothing(newValues("iep_data_grade_1_id")) Then
                    sqlComm.Parameters("@iep_data_grade_1_id").Value = newValues("iep_data_grade_1_id")
                End If

                sqlComm.Parameters("@iep_data_grade_2_id").Value = DBNull.Value
                If Not IsNothing(newValues("iep_data_grade_2_id")) Then
                    sqlComm.Parameters("@iep_data_grade_2_id").Value = newValues("iep_data_grade_2_id")
                End If

                sqlComm.Parameters("@iep_data_grade_3_id").Value = DBNull.Value
                If Not IsNothing(newValues("iep_data_grade_3_id")) Then
                    sqlComm.Parameters("@iep_data_grade_3_id").Value = newValues("iep_data_grade_3_id")
                End If

                sqlComm.Parameters("@iep_date_initiated").Value = IIf(newValues("iep_date_initiated") Is Nothing, DBNull.Value, newValues("iep_date_initiated"))

                sqlComm.ExecuteNonQuery()

                Exit For
            End If
            'End If
        Next
        'Next


        sqlConn.Close()

        'RadGrid2.MasterTableView.Rebind()

    End Sub

    Private Sub DoLog(strLogInfo As String)
        Dim strSql As String
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        sqlConn.Open()
        Dim sqlComm As New SqlCommand
        sqlComm.Connection = sqlConn

        strSql = "INSERT INTO [dbo].[tmp_log] ([log_ts], [log_data]) " _
                        & "VALUES(GETDATE(), @log_data)"

        sqlComm.CommandText = strSql
        sqlComm.Parameters.Add(New SqlParameter("@log_data", SqlDbType.NVarChar))
        sqlComm.Parameters("@log_data").Value = strLogInfo

        sqlComm.ExecuteNonQuery()
        sqlConn.Close()
        sqlConn = Nothing
        sqlComm = Nothing

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

    Private Sub SqlDataSource1_Selecting(sender As Object, e As SqlDataSourceSelectingEventArgs) Handles SqlDataSource1.Selecting
        'SqlDataSource1.SelectParameters.Item("@usr_login").DefaultValue = Context.User.Identity.Name
    End Sub

    Protected Sub cmbTeachers_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs)
        cmbTeachers.DataSource = GetDataTable("getTeacherList '" & System.Web.HttpContext.Current.User.Identity.Name & "', '%" & e.Text & "%', NULL")
        cmbTeachers.DataTextField = "teacher_name"
        cmbTeachers.DataValueField = "usr_uuid"
        cmbTeachers.DataBind()
    End Sub

    Protected Sub cmbStudents_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs)
        cmbStudents.DataSource = GetDataTable("getStudentList '" & System.Web.HttpContext.Current.User.Identity.Name & "', '%" & e.Text & "%', NULL")
        cmbStudents.DataTextField = "student_name"
        cmbStudents.DataValueField = "student_uuid"
        cmbStudents.DataBind()
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

    Protected Sub RadGrid2_RowDrop(sender As Object, e As GridDragDropEventArgs)
        'For Each draggedItem As GridDataItem In e.DraggedItems
        If e.DraggedItems.Count = 0 Then Exit Sub

        Dim oDataItem As GridDataItem
        Dim intCounter As Integer
        
        doSnapShot(e.DraggedItems(0).GetDataKeyValue("iep_uuid").ToString)

        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        sqlConn.Open()
        Dim strSql As String = "doMoveGoal @iep_data_uuid_src, @iep_data_uuid_dest, @move_up"

        Dim draggedItems = e.DraggedItems.OrderBy(Function(a) a.ItemIndexHierarchical).ToArray()

        For intCounter = draggedItems.Count - 1 To 0 Step -1
            oDataItem = draggedItems(intCounter)
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn

            sqlComm.Parameters.Add(New SqlParameter("@iep_data_uuid_src", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters("@iep_data_uuid_src").Value = New Guid(oDataItem.GetDataKeyValue("iep_data_uuid").ToString)

            sqlComm.Parameters.Add(New SqlParameter("@move_up", SqlDbType.Bit))
            sqlComm.Parameters("@move_up").Value = Math.Abs(e.DropPosition - 1) 'IIf(draggedItems(0).ItemIndex > e.DestDataItem.ItemIndex, 1, 0)

            sqlComm.Parameters.Add(New SqlParameter("@iep_data_uuid_dest", SqlDbType.UniqueIdentifier))
            If e.DropPosition = GridItemDropPosition.Above And e.DestDataItem.ItemIndex <> 0 Then
                sqlComm.Parameters("@iep_data_uuid_dest").Value = New Guid(RadGrid2.Items(e.DestDataItem.ItemIndex - 1).GetDataKeyValue("iep_data_uuid").ToString)
                sqlComm.Parameters("@move_up").Value = 0
            Else
                sqlComm.Parameters("@iep_data_uuid_dest").Value = New Guid(e.DestDataItem.GetDataKeyValue("iep_data_uuid").ToString)
            End If

            sqlComm.CommandText = strSql
            sqlComm.ExecuteNonQuery()
        Next
        sqlConn.Close()
        RadGrid2.Rebind()
        'Next
    End Sub

    Protected Sub Unnamed_Click(sender As Object, e As EventArgs)

    End Sub

    Protected Sub RadGrid1_ItemInserted(sender As Object, e As GridInsertedEventArgs)

    End Sub

    Function SentenceCase(strDataText As String) As String
        Dim strTemp As String
        Dim intSPos As Integer

        strTemp = strDataText
        While strTemp.StartsWith("<")
            strTemp = Right(strTemp, strTemp.Length - strTemp.IndexOf(">") - 1)
        End While

        intSPos = (strDataText.Length - strTemp.Length)
        strDataText = Left(strDataText, intSPos) & strDataText.Substring(intSPos, 1).ToUpper & strDataText.Substring(intSPos + 1)

        strTemp = strDataText
        'While strTemp.EndsWith(">")
        '    strTemp = Left(strTemp, strTemp.LastIndexOf("<"))
        'End While

        'intSPos = strTemp.Length
        'If strDataText.LastIndexOf(".") <> intSPos - 1 Then
        '    strDataText = Left(strDataText, intSPos) & "." & Right(strDataText, strDataText.Length - intSPos)
        'End If

        SentenceCase = strDataText

    End Function

    Protected Sub RadMultiPage1_PageViewCreated(sender As Object, e As RadMultiPageEventArgs)
        Dim userControlName As String = "gcCategory.ascx"
        Dim userControl As Control = Page.LoadControl(userControlName)
        userControl.ID = e.PageView.ID & "_userControl"
        e.PageView.Controls.Add(userControl)
    End Sub

    Protected Sub tbGoalCatalogue_TabClick(sender As Object, e As RadTabStripEventArgs)
        AddPageView(e.Tab)
        e.Tab.PageView.Selected = True
    End Sub

    Private Sub AddPageView(ByVal tab As RadTab)
        Dim pageView As RadPageView = New RadPageView
        pageView.ID = tab.PageViewID
        RadMultiPage1.PageViews.Add(pageView)
        tab.PageViewID = pageView.ID
    End Sub

    Protected Sub cmbSubjects_ItemsRequested(sender As Object, e As RadComboBoxItemsRequestedEventArgs)
        cmbSubjects.DataSource = GetDataTable("SELECT subject_text FROM vw_list_iep WHERE is_active = 1 AND subject_text LIKE '%" & e.Text & "%' GROUP BY subject_text ORDER BY 1")
        cmbSubjects.DataTextField = "subject_text"
        cmbSubjects.DataValueField = "subject_text"
        cmbSubjects.DataBind()
    End Sub

    Private Sub RadGrid2_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles RadGrid2.ItemDataBound
        Dim oItem As RadComboBox
        If TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode Then
            oItem = CType(e.Item, GridEditableItem)("iep_data_grade_1_id").Controls(0)
            oItem.Width = Unit.Pixel(50)
            oItem.DropDownAutoWidth = RadComboBoxDropDownAutoWidth.Enabled
            oItem = CType(e.Item, GridEditableItem)("iep_data_grade_2_id").Controls(0)
            oItem.Width = Unit.Pixel(50)
            oItem.DropDownAutoWidth = RadComboBoxDropDownAutoWidth.Enabled
            oItem = CType(e.Item, GridEditableItem)("iep_data_grade_3_id").Controls(0)
            oItem.Width = Unit.Pixel(50)
            oItem.DropDownAutoWidth = RadComboBoxDropDownAutoWidth.Enabled
        End If
    End Sub
End Class