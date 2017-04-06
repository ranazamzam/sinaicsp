Imports System
Imports System.Web.SessionState
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class csp_details
    Inherits userSession

    Public timeoutWarningLength As Integer = 300
    Public enableEdit As Boolean = True

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dvSql As DataView

        If Request.QueryString("iep_uuid") = "" Then
            Response.Redirect("csp_list.aspx")
        End If

        RadScriptManager.GetCurrent(Me).Services.Add(New ServiceReference(ResolveUrl("cspUpdate.asmx")))

        Dim currentSchoolYear As String

        If Month(Now) >= 7 Then
            currentSchoolYear = Year(Now).ToString & " - " & (Year(Now) + 1).ToString
        Else
            currentSchoolYear = (Year(Now) - 1).ToString & " - " & Year(Now).ToString
        End If

        dvSql = DirectCast(sqlCspHeader.Select(DataSourceSelectArguments.Empty), DataView)

        'configure the notification to automatically show 6 min before session expiration.  It will display for a total of 5 min before force terminating the session.
        RadNotification1.ShowInterval = (Session.Timeout - 6) * 60000
        'set the redirect url as a value for an easier and faster extraction in on the client
        RadNotification1.Value = Page.ResolveClientUrl("SessionExpired.aspx")

        If Not Context.User.Identity.IsAuthenticated Or Session.IsNewSession Then
            Response.Redirect("Login.aspx")
        End If

        If Not IsPostBack Then
            If Session.Item("reClipboardTransfer") Is Nothing Then
                Session.Add("reClipboardTransfer", "")
            End If
            reClipboardTransfer.Content = Session("reClipboardTransfer").ToString
        End If

        If Not dvSql Is Nothing Then
            If dvSql.Count = 1 Then
                For Each drvSql As DataRowView In dvSql
                    lblStudent.Text = drvSql("display_name").ToString()
                    lblSchool.Text = drvSql("school_name").ToString()
                    lblTeacher.Text = drvSql("usr_display_name").ToString()
                    lblYr.Text = drvSql("iep_year").ToString()
                    If drvSql("iep_year").ToString() <> currentSchoolYear Then
                        enableEdit = False
                        ctxMenu.FindItemByValue("deleteSelected").Enabled = False
                        ctxMenu.FindItemByValue("doSubGoal").Enabled = False
                        ctxMenu.FindItemByValue("doGoal").Enabled = False
                        rgCspGoals.ClientSettings.AllowRowsDragDrop = False
                        rslpGoalCatalog.Enabled = False
                        ctxClipboard.Enabled = False
                        reComments.EditModes = EditModes.Preview
                        reFNotes.EditModes = EditModes.Preview
                        reJNotes.EditModes = EditModes.Preview
                    Else
                        enableEdit = True
                    End If
                    lblSubject.Text = drvSql("subject_text").ToString()
                    lblMats.Text = drvSql("iep_materials").ToString()
                    hdnSchoolId.Value = drvSql("school_id").ToString()
                Next
            End If
        End If

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
        End If

        usr_login.Value = Context.User.Identity.Name

    End Sub

    Protected Sub OnCallbackUpdate(sender As Object, e As RadNotificationEventArgs)

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

    Protected Sub RadAjaxManager1_AjaxRequest(sender As Object, e As Telerik.Web.UI.AjaxRequestEventArgs)
        Dim grid As RadGrid = TryCast(rgCspGoals, RadGrid)
        Dim strSql As String

        If e.Argument.IndexOf("deleteSelected") <> -1 Then
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            sqlConn.Open()

            doSnapShot(Request.QueryString("iep_uuid"))

            For Each oSelected As GridDataItem In grid.SelectedItems
                strSql = "deleteGoal '" & oSelected.GetDataKeyValue("iep_data_uuid").ToString() & "'"
                Dim sqlComm As New SqlCommand(strSql, sqlConn)

                sqlComm.ExecuteNonQuery()
            Next

            sqlConn.Close()

            rgCspGoals.Rebind()
        ElseIf e.Argument.IndexOf("Undo") = 0 Then
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            sqlConn.Open()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            strSql = "iepUndo @iep_uuid"

            sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters("@iep_uuid").Value = New Guid(Request.QueryString("iep_uuid"))
            sqlComm.CommandText = strSql
            sqlComm.ExecuteNonQuery()
            sqlConn.Close()

            rgCspGoals.MasterTableView.Rebind()
        ElseIf e.Argument.IndexOf("doClose") = 0 Then
            If Session.Item("reClipboardTransfer") Is Nothing Then
                Session.Add("reClipboardTransfer", "")
            End If
            Session.Item("reClipboardTransfer") = reClipboardTransfer.GetHtml(EditorStripHtmlOptions.None)
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
            Response.Redirect("csp_list.aspx")
        ElseIf Left(e.Argument, 4) = "iep_" Then
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            sqlConn.Open()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            strSql = "UPDATE iep SET " & e.Argument & " = @text WHERE iep_uuid = @iep_uuid"

            sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters("@iep_uuid").Value = New Guid(Request.QueryString("iep_uuid"))

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
            doSnapShot(Request.QueryString("iep_uuid"))

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
            rgCspGoals.Rebind()
        ElseIf e.Argument.IndexOf("LockIEP") = 0 Then
            doSnapShot(Request.QueryString("iep_uuid"), 1)
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            sqlConn.Open()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            strSql = "doLockIep @iep_uuid, @usr_uuid, 1"

            sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters("@iep_uuid").Value = New Guid(Request.QueryString("iep_uuid"))

            sqlComm.Parameters.Add(New SqlParameter("@usr_uuid", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters("@usr_uuid").Value = New Guid(usrUuid)

            sqlComm.CommandText = strSql
            sqlComm.ExecuteNonQuery()

            sqlConn.Close()

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
            doSnapShot(Request.QueryString("iep_uuid"))
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            Dim strDataText As String = reTransfer.GetHtml(EditorStripHtmlOptions.None)
            Dim strGoals() As String = strDataText.Split("~")
            Dim intCounter As Integer
            Dim sqlComm As SqlCommand

            sqlConn.Open()

            For intCounter = UBound(strGoals) To 1 Step -1
                sqlComm = New SqlCommand
                sqlComm.Connection = sqlConn
                If UCase(strGoals(intCounter)) Like "GC_[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F]" Then
                    doSnapShot(Request.QueryString("iep_uuid"))

                    strSql = "doCloneIEP_New @iep_src_uuid, @iep_dest_uuid, 1"
                    'strSql = "INSERT INTO [dbo].[iep_data] ([iep_uuid], [iep_data_sequence], [iep_data_text], [iep_date_initiated]) " _
                    '            & "SELECT @iep_uuid, iep_data_sequence + (SELECT ISNULL(MAX(iep_data_sequence), 0) + 1 FROM iep_data WHERE iep_uuid = @iep_uuid), iep_data_text, iep_date_initiated FROM iep_data WHERE iep_data.iep_uuid = @iep_src_uuid"
                    '& "VALUES(@iep_uuid, (SELECT ISNULL(MAX(iep_data_sequence), 0) + 1 FROM iep_data WHERE iep_uuid = @iep_uuid), @iep_data_text, CONVERT(NVARCHAR, MONTH(GETDATE())) + N'/' + RIGHT(CONVERT(NVARCHAR, YEAR(GETDATE())), 2))"

                    sqlComm.Parameters.Add(New SqlParameter("@iep_src_uuid", SqlDbType.NVarChar))
                    sqlComm.Parameters("@iep_src_uuid").Value = Replace(strGoals(intCounter), "gc_", "")

                    sqlComm.Parameters.Add(New SqlParameter("@iep_dest_uuid", SqlDbType.UniqueIdentifier))
                    sqlComm.Parameters("@iep_dest_uuid").Value = New Guid(Request.QueryString("iep_uuid"))
                ElseIf UCase(strGoals(intCounter)) Like "[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F]" Then
                    strSql = "doCloneIEP @iep_src_uuid, @iep_dest_uuid"
                    sqlComm.Parameters.Add(New SqlParameter("@iep_dest_uuid", SqlDbType.UniqueIdentifier))
                    sqlComm.Parameters("@iep_dest_uuid").Value = New Guid(Request.QueryString("iep_uuid"))

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
                    sqlComm.Parameters("@iep_uuid").Value = New Guid(Request.QueryString("iep_uuid"))

                    sqlComm.Parameters.Add(New SqlParameter("@iep_data_dest_uuid", SqlDbType.UniqueIdentifier))
                    If rgCspGoals.SelectedItems.Count = 1 Then
                        sqlComm.Parameters("@iep_data_dest_uuid").Value = New Guid(CType(rgCspGoals.SelectedItems(0), GridDataItem).GetDataKeyValue("iep_data_uuid").ToString)
                    Else
                        If rgCspGoals.Items.Count = 0 Then
                            sqlComm.Parameters("@iep_data_dest_uuid").Value = DBNull.Value
                        Else
                            sqlComm.Parameters("@iep_data_dest_uuid").Value = New Guid(CType(rgCspGoals.Items(rgCspGoals.Items.Count - 1), GridDataItem).GetDataKeyValue("iep_data_uuid").ToString)
                        End If
                    End If

                    sqlComm.Parameters.Add(New SqlParameter("@iep_data_text", SqlDbType.NVarChar))
                    sqlComm.Parameters("@iep_data_text").Value = strGoals(intCounter)
                End If

                sqlComm.CommandText = strSql
                sqlComm.ExecuteNonQuery()
            Next
            sqlConn.Close()
        ElseIf e.Argument.IndexOf("doBulkSubGoal") = 0 Then
            doSnapShot(Request.QueryString("iep_uuid"))
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            Dim oItem As GridItem
            Dim sqlComm As SqlCommand

            sqlConn.Open()

            For Each oItem In rgCspGoals.SelectedItems ' intCounter = 1 To UBound(strGoals)

                If CType(oItem.Cells(4).Controls(0), ImageButton).ImageUrl = "images/arrow_right.gif" Then
                    sqlComm = New SqlCommand
                    sqlComm.Connection = sqlConn

                    strSql = "doSubGoal @iep_data_uuid"
                    'oItem.Cells(2)
                    sqlComm.Parameters.Add(New SqlParameter("@iep_data_uuid", SqlDbType.UniqueIdentifier))
                    sqlComm.Parameters("@iep_data_uuid").Value = New Guid(rgCspGoals.MasterTableView.DataKeyValues(oItem.DataSetIndex).Item("iep_data_uuid").ToString)

                    sqlComm.CommandText = strSql
                    sqlComm.ExecuteNonQuery()
                End If
            Next
            sqlConn.Close()
        ElseIf e.Argument.IndexOf("doBulkGoal") = 0 Then
            doSnapShot(Request.QueryString("iep_uuid"))
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            Dim oItem As GridItem
            Dim sqlComm As SqlCommand

            sqlConn.Open()

            For Each oItem In rgCspGoals.SelectedItems ' intCounter = 1 To UBound(strGoals)

                If CType(oItem.Cells(4).Controls(0), ImageButton).ImageUrl = "images/arrow_left.gif" Then
                    sqlComm = New SqlCommand
                    sqlComm.Connection = sqlConn

                    strSql = "doSubGoal @iep_data_uuid"
                    'oItem.Cells(2)
                    sqlComm.Parameters.Add(New SqlParameter("@iep_data_uuid", SqlDbType.UniqueIdentifier))
                    sqlComm.Parameters("@iep_data_uuid").Value = New Guid(rgCspGoals.MasterTableView.DataKeyValues(oItem.DataSetIndex).Item("iep_data_uuid").ToString)

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

            Dim strDataText As String = reTransfer.GetHtml(EditorStripHtmlOptions.None)
            If UCase(strDataText) Like "GC_[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F]" Then
                doSnapShot(Request.QueryString("iep_uuid"))

                strSql = "doCloneIEP_New @iep_src_uuid, @iep_dest_uuid, 1"
                'strSql = "INSERT INTO [dbo].[iep_data] ([iep_uuid], [iep_data_sequence], [iep_data_text], [iep_date_initiated]) " _
                '            & "SELECT @iep_uuid, iep_data_sequence + (SELECT ISNULL(MAX(iep_data_sequence), 0) + 1 FROM iep_data WHERE iep_uuid = @iep_uuid), iep_data_text, iep_date_initiated FROM iep_data WHERE iep_data.iep_uuid = @iep_src_uuid"
                '& "VALUES(@iep_uuid, (SELECT ISNULL(MAX(iep_data_sequence), 0) + 1 FROM iep_data WHERE iep_uuid = @iep_uuid), @iep_data_text, CONVERT(NVARCHAR, MONTH(GETDATE())) + N'/' + RIGHT(CONVERT(NVARCHAR, YEAR(GETDATE())), 2))"

                sqlComm.Parameters.Add(New SqlParameter("@iep_src_uuid", SqlDbType.NVarChar))
                sqlComm.Parameters("@iep_src_uuid").Value = Replace(strDataText, "gc_", "")

                sqlComm.Parameters.Add(New SqlParameter("@iep_dest_uuid", SqlDbType.UniqueIdentifier))
                sqlComm.Parameters("@iep_dest_uuid").Value = New Guid(Request.QueryString("iep_uuid"))
                '8-4-4-4-12
            ElseIf UCase(strDataText) Like "[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F]" And Len(strDataText) Then
                doSnapShot(Request.QueryString("iep_uuid"))

                strSql = "doCloneIEP @iep_src_uuid, @iep_dest_uuid"
                'strSql = "INSERT INTO [dbo].[iep_data] ([iep_uuid], [iep_data_sequence], [iep_data_text], [iep_date_initiated]) " _
                '            & "SELECT @iep_uuid, iep_data_sequence + (SELECT ISNULL(MAX(iep_data_sequence), 0) + 1 FROM iep_data WHERE iep_uuid = @iep_uuid), iep_data_text, iep_date_initiated FROM iep_data WHERE iep_data.iep_uuid = @iep_src_uuid"
                '& "VALUES(@iep_uuid, (SELECT ISNULL(MAX(iep_data_sequence), 0) + 1 FROM iep_data WHERE iep_uuid = @iep_uuid), @iep_data_text, CONVERT(NVARCHAR, MONTH(GETDATE())) + N'/' + RIGHT(CONVERT(NVARCHAR, YEAR(GETDATE())), 2))"

                sqlComm.Parameters.Add(New SqlParameter("@iep_src_uuid", SqlDbType.NVarChar))
                sqlComm.Parameters("@iep_src_uuid").Value = strDataText

                sqlComm.Parameters.Add(New SqlParameter("@iep_dest_uuid", SqlDbType.UniqueIdentifier))
                sqlComm.Parameters("@iep_dest_uuid").Value = New Guid(Request.QueryString("iep_uuid"))
            ElseIf e.Argument.IndexOf("doSubGoal") = 0 Then
                doSnapShot(Request.QueryString("iep_uuid"))

                strSql = "doInsertSubGoal @iep_uuid, @iep_data_text, @iep_data_dest_uuid"

                sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
                sqlComm.Parameters("@iep_uuid").Value = New Guid(Request.QueryString("iep_uuid"))

                sqlComm.Parameters.Add(New SqlParameter("@iep_data_text", SqlDbType.NVarChar))
                sqlComm.Parameters("@iep_data_text").Value = SentenceCase(strDataText)

                sqlComm.Parameters.Add(New SqlParameter("@iep_data_dest_uuid", SqlDbType.UniqueIdentifier))
                If rgCspGoals.SelectedItems.Count = 1 Then
                    sqlComm.Parameters("@iep_data_dest_uuid").Value = New Guid(CType(rgCspGoals.SelectedItems(0), GridDataItem).GetDataKeyValue("iep_data_uuid").ToString)
                Else
                    sqlComm.Parameters("@iep_data_dest_uuid").Value = DBNull.Value
                End If

            ElseIf iep_data_uuid.Value.ToString = "" Then
                doSnapShot(Request.QueryString("iep_uuid"))

                strSql = "doInsertGoal @iep_uuid, @iep_data_text, @iep_data_dest_uuid"
                'strSql = "INSERT INTO [dbo].[iep_data] ([iep_uuid], [iep_data_sequence], [iep_data_text]) " _
                '           & "VALUES(@iep_uuid, (SELECT ISNULL(MAX(iep_data_sequence), 0) + 1 FROM iep_data WHERE iep_uuid = @iep_uuid), @iep_data_text)"

                sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
                sqlComm.Parameters("@iep_uuid").Value = New Guid(Request.QueryString("iep_uuid"))

                sqlComm.Parameters.Add(New SqlParameter("@iep_data_text", SqlDbType.NVarChar))
                sqlComm.Parameters("@iep_data_text").Value = SentenceCase(strDataText)

                sqlComm.Parameters.Add(New SqlParameter("@iep_data_dest_uuid", SqlDbType.UniqueIdentifier))
                If rgCspGoals.SelectedItems.Count = 1 Then
                    sqlComm.Parameters("@iep_data_dest_uuid").Value = New Guid(CType(rgCspGoals.SelectedItems(0), GridDataItem).GetDataKeyValue("iep_data_uuid").ToString)
                Else
                    sqlComm.Parameters("@iep_data_dest_uuid").Value = DBNull.Value
                End If
            Else
                doSnapShot(Request.QueryString("iep_uuid"))

                strSql = "UPDATE iep_data SET iep_data_text = @iep_data_text WHERE iep_data_uuid = @iep_data_uuid"

                sqlComm.Parameters.Add(New SqlParameter("@iep_data_uuid", SqlDbType.UniqueIdentifier))
                sqlComm.Parameters("@iep_data_uuid").Value = New Guid(iep_data_uuid.Value)

                sqlComm.Parameters.Add(New SqlParameter("@iep_data_text", SqlDbType.NVarChar))
                sqlComm.Parameters("@iep_data_text").Value = SentenceCase(strDataText)
            End If


            sqlComm.CommandText = strSql
            sqlComm.ExecuteNonQuery()

            sqlConn.Close()
            rgCspGoals.MasterTableView.Rebind()
        End If
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

    Private Sub rgCspGoals_DeleteCommand(sender As Object, e As GridCommandEventArgs) Handles rgCspGoals.DeleteCommand
        Dim strSql As String

        doSnapShot(e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex).Item("iep_uuid").ToString)

        strSql = "deleteGoal '" & e.Item.OwnerTableView.DataKeyValues(e.Item.ItemIndex).Item("iep_data_uuid").ToString() & "'"
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        sqlConn.Open()
        Dim sqlComm As New SqlCommand(strSql, sqlConn)

        sqlComm.ExecuteNonQuery()

        sqlConn.Close()

    End Sub

    Private Sub grade_SelectedIndexChange(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)

        If e.Value <> e.OldValue Then
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            sqlConn.Open()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn

            sqlComm.Parameters.Add(New SqlParameter("@iep_data_uuid", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters.Add(New SqlParameter("@iep_data_grade_id", SqlDbType.TinyInt))

            Dim strSql = "UPDATE [dbo].[iep_data] SET [" & Right(sender.parent.CellIndexHierarchical, 19) & "] = @iep_data_grade_id WHERE [iep_data_uuid] = @iep_data_uuid"
            sqlComm.CommandText = strSql

            sqlComm.Parameters("@iep_data_uuid").Value = New Guid(sender.parent.parent.GetDataKeyValue("iep_data_uuid").ToString)

            sqlComm.Parameters("@iep_data_grade_id").Value = DBNull.Value
            If IsNumeric(e.Value) Then sqlComm.Parameters("@iep_data_grade_id").Value = e.Value

            sqlComm.ExecuteNonQuery()

            sqlConn.Close()

            sqlComm = Nothing
            sqlConn = Nothing
        End If

    End Sub

    Private Sub dateInitiated_SelectedIndexChange(sender As Object, e As RadComboBoxSelectedIndexChangedEventArgs)

        If e.Value <> e.OldValue Then
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            sqlConn.Open()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn

            sqlComm.Parameters.Add(New SqlParameter("@iep_data_uuid", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters.Add(New SqlParameter("@iep_date_initiated", SqlDbType.NVarChar))

            Dim strSql = "UPDATE [dbo].[iep_data] SET [iep_date_initiated] = @iep_date_initiated WHERE [iep_data_uuid] = @iep_data_uuid"
            sqlComm.CommandText = strSql

            sqlComm.Parameters("@iep_data_uuid").Value = New Guid(sender.parent.parent.GetDataKeyValue("iep_data_uuid").ToString)

            sqlComm.Parameters("@iep_date_initiated").Value = DBNull.Value
            If e.Value <> "" Then sqlComm.Parameters("@iep_date_initiated").Value = e.Value

            sqlComm.ExecuteNonQuery()

            sqlConn.Close()

            sqlComm = Nothing
            sqlConn = Nothing
        End If

    End Sub

    Private Sub rgCspGoals_ItemCreated(sender As Object, e As GridItemEventArgs) Handles rgCspGoals.ItemCreated
        Dim item As Telerik.Web.UI.GridDataItem
        Dim editItem As Telerik.Web.UI.GridEditableItem

        If e.Item.ItemType = GridItemType.Item Or e.Item.ItemType = GridItemType.AlternatingItem Then
            item = e.Item
            If Not IsDBNull(item.GetDataKeyValue("iep_data_parent_uuid")) Then
                CType(item("SubGoal").Controls(0), ImageButton).ImageUrl = "images/arrow_left.gif"
            End If
            If enableEdit = False Then
                CType(item("DeleteColumn1").Controls(0), ImageButton).Enabled = False
                CType(item("SubGoal").Controls(0), ImageButton).Enabled = False
                CType(item("EditCommandColumn1").Controls(0), ImageButton).Enabled = False
            End If
        End If

        If TypeOf e.Item Is GridEditableItem And e.Item.IsInEditMode = True Then
            editItem = CType(e.Item, GridEditableItem)

            Dim oDropDown = CType(editItem("iep_date_initiated").Controls(0), RadComboBox)
            If Not oDropDown Is Nothing Then
                'oDropDown.AutoPostBack = True
                'AddHandler oDropDown.SelectedIndexChanged, AddressOf dateInitiated_SelectedIndexChange
                oDropDown.OnClientSelectedIndexChanged = "setDateInitiated"
            End If

            oDropDown = CType(editItem("iep_data_grade_1_id").Controls(0), RadComboBox)
            If Not oDropDown Is Nothing Then
                '    oDropDown.AutoPostBack = True
                '    AddHandler oDropDown.SelectedIndexChanged, AddressOf grade_SelectedIndexChange
                oDropDown.OnClientSelectedIndexChanged = "setGradeValue"
            End If

            oDropDown = CType(editItem("iep_data_grade_2_id").Controls(0), RadComboBox)
            If Not oDropDown Is Nothing Then
                '    oDropDown.AutoPostBack = True
                '    AddHandler oDropDown.SelectedIndexChanged, AddressOf grade_SelectedIndexChange
                oDropDown.OnClientSelectedIndexChanged = "setGradeValue"
            End If

            oDropDown = CType(editItem("iep_data_grade_3_id").Controls(0), RadComboBox)
            If Not oDropDown Is Nothing Then
                '    oDropDown.AutoPostBack = True
                '    AddHandler oDropDown.SelectedIndexChanged, AddressOf grade_SelectedIndexChange
                oDropDown.OnClientSelectedIndexChanged = "setGradeValue"
            End If
        End If

    End Sub

    Private Sub rgCspGoals_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles rgCspGoals.ItemDataBound
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

    Private Sub rgCspGoals_RowDrop(sender As Object, e As GridDragDropEventArgs) Handles rgCspGoals.RowDrop
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
                sqlComm.Parameters("@iep_data_uuid_dest").Value = New Guid(rgCspGoals.Items(e.DestDataItem.ItemIndex - 1).GetDataKeyValue("iep_data_uuid").ToString)
                sqlComm.Parameters("@move_up").Value = 0
            Else
                sqlComm.Parameters("@iep_data_uuid_dest").Value = New Guid(e.DestDataItem.GetDataKeyValue("iep_data_uuid").ToString)
            End If

            sqlComm.CommandText = strSql
            sqlComm.ExecuteNonQuery()
        Next
        sqlConn.Close()
        rgCspGoals.DataBind()

    End Sub

    Private Sub rgCspGoals_UpdateCommand(sender As Object, e As GridCommandEventArgs) Handles rgCspGoals.UpdateCommand

        'Dim editedItems As GridItemCollection = CType(e.Item.OwnerTableView.ChildEditItems, GridItemCollection)
        'Dim editedItem As GridEditableItem = CType(e.Item, GridEditableItem)
        'Dim newValues As New Hashtable()
        'Dim newValue As DictionaryEntry
        'Dim strSql As String
        'Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        'sqlConn.Open()
        'Dim sqlComm As New SqlCommand
        'sqlComm.Connection = sqlConn

        'sqlComm.Parameters.Add(New SqlParameter("@iep_data_uuid", SqlDbType.UniqueIdentifier))
        'sqlComm.Parameters.Add(New SqlParameter("@iep_data_grade_1_id", SqlDbType.TinyInt))
        'sqlComm.Parameters.Add(New SqlParameter("@iep_data_grade_2_id", SqlDbType.TinyInt))
        'sqlComm.Parameters.Add(New SqlParameter("@iep_data_grade_3_id", SqlDbType.TinyInt))
        'sqlComm.Parameters.Add(New SqlParameter("@iep_date_initiated", SqlDbType.NVarChar))

        'If editedItem.ItemIndex = 0 Then
        '    doSnapShot(editedItem.GetDataKeyValue("iep_uuid").ToString)
        'End If

        'For Each editedItem In editedItems
        '    editedItem.OwnerTableView.ExtractValuesFromItem(newValues, editedItem)
        '    For Each newValue In newValues
        '        'If Not IsNothing(newValue.Value) Then
        '        If newValue.Value <> editedItem.SavedOldValues(newValue.Key) Then
        '            strSql = "UPDATE [dbo].[iep_data] SET [iep_date_initiated] = @iep_date_initiated, [iep_data_grade_1_id] = @iep_data_grade_1_id, [iep_data_grade_2_id] = @iep_data_grade_2_id, [iep_data_grade_3_id] = @iep_data_grade_3_id WHERE [iep_data_uuid] = @iep_data_uuid"
        '            sqlComm.CommandText = strSql

        '            sqlComm.Parameters("@iep_data_uuid").Value = New Guid(editedItem.GetDataKeyValue("iep_data_uuid").ToString)

        '            sqlComm.Parameters("@iep_data_grade_1_id").Value = DBNull.Value
        '            If Not IsNothing(newValues("iep_data_grade_1_id")) Then
        '                sqlComm.Parameters("@iep_data_grade_1_id").Value = newValues("iep_data_grade_1_id")
        '            End If

        '            sqlComm.Parameters("@iep_data_grade_2_id").Value = DBNull.Value
        '            If Not IsNothing(newValues("iep_data_grade_2_id")) Then
        '                sqlComm.Parameters("@iep_data_grade_2_id").Value = newValues("iep_data_grade_2_id")
        '            End If

        '            sqlComm.Parameters("@iep_data_grade_3_id").Value = DBNull.Value
        '            If Not IsNothing(newValues("iep_data_grade_3_id")) Then
        '                sqlComm.Parameters("@iep_data_grade_3_id").Value = newValues("iep_data_grade_3_id")
        '            End If

        '            sqlComm.Parameters("@iep_date_initiated").Value = IIf(newValues("iep_date_initiated") Is Nothing, DBNull.Value, newValues("iep_date_initiated"))

        '            sqlComm.ExecuteNonQuery()

        '            Exit For
        '        End If
        '        'End If
        '    Next
        'Next

        ''rgCspGoals.DataBind()

        'sqlConn.Close()

    End Sub

    Private Function doPrevNext(intAdjust As Integer) As String
        Dim strSql As String

        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        sqlConn.Open()

        Dim sqlComm As New SqlCommand(strSql, sqlConn)

        strSql = "getNextPrevCsp"
        sqlComm = New SqlCommand(strSql, sqlConn)
        sqlComm.CommandType = CommandType.StoredProcedure

        sqlComm.Parameters.Add(New SqlParameter("@usr_login", SqlDbType.NVarChar))
        sqlComm.Parameters("@usr_login").Value = Context.User.Identity.Name

        sqlComm.Parameters.Add(New SqlParameter("@student_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@student_uuid").Value = New Guid(Session("cmbStudents").ToString)

        sqlComm.Parameters.Add(New SqlParameter("@teacher_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@teacher_uuid").Value = New Guid(Session("cmbTeachers").ToString)

        sqlComm.Parameters.Add(New SqlParameter("@school_id", SqlDbType.TinyInt))
        sqlComm.Parameters("@school_id").Value = Session("cmbSchools").ToString

        sqlComm.Parameters.Add(New SqlParameter("@subject_text", SqlDbType.NVarChar))
        sqlComm.Parameters("@subject_text").Value = Session("cmbSubjects").ToString

        sqlComm.Parameters.Add(New SqlParameter("@current_placement", SqlDbType.NVarChar))
        sqlComm.Parameters("@current_placement").Value = Session("cmbClass").ToString

        sqlComm.Parameters.Add(New SqlParameter("@school_year", SqlDbType.NVarChar))
        sqlComm.Parameters("@school_year").Value = Session("cmbYear").ToString

        sqlComm.Parameters.Add(New SqlParameter("@iep_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@iep_uuid").Value = New Guid(Request.QueryString("iep_uuid"))

        sqlComm.Parameters.Add(New SqlParameter("@adj", SqlDbType.SmallInt))
        sqlComm.Parameters("@adj").Value = intAdjust

        sqlComm.Parameters.Add(New SqlParameter("@next_iep_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@next_iep_uuid").Direction = ParameterDirection.Output

        sqlComm.ExecuteNonQuery()
        sqlConn.Close()

        If Session.Item("reClipboardTransfer") Is Nothing Then
            Session.Add("reClipboardTransfer", "")
        End If
        Session.Item("reClipboardTransfer") = reClipboardTransfer.GetHtml(EditorStripHtmlOptions.None)

        Return sqlComm.Parameters("@next_iep_uuid").Value.ToString

    End Function

    Private Sub rbtnNext_Click(sender As Object, e As EventArgs) Handles rbtnNext.Click
        
        Dim strNextUuid As String = doPrevNext(1)

        Response.Redirect("csp_details.aspx?iep_uuid=" & strNextUuid)
        
    End Sub

    Private Sub rbtnPrev_Click(sender As Object, e As EventArgs) Handles rbtnPrev.Click
        Dim strNextUuid As String = doPrevNext(-1)

        Response.Redirect("csp_details.aspx?iep_uuid=" & strNextUuid)
    End Sub

    Protected Sub RadAjaxManager2_AjaxRequest(sender As Object, e As AjaxRequestEventArgs)

    End Sub
End Class