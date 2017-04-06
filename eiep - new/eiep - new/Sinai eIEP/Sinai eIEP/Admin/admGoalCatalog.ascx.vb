Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class admGoalCatalog
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            If Session.Item("reClipboardTransfer") Is Nothing Then
                Session.Add("reClipboardTransfer", "")
            End If
            reClipboardTransfer.Content = Session("reClipboardTransfer").ToString
        End If

    End Sub

    Protected Sub cmbCategories_SelectedIndexChanged(sender As Object, e As Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs)
        If e.Value = "00000000-0000-0000-0000-000000000000" Then
            rbtNewCat.Text = "Add"
            rtbNewCategory.Text = ""
        Else
            rbtNewCat.Text = "Save"
            rtbNewCategory.Text = e.Text
        End If
    End Sub

    Protected Sub rbtNewCat_Click(sender As Object, e As EventArgs)
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        Dim strSql As String
        Dim sqlComm As SqlCommand

        If rbtNewCat.Text = "Add" Then
            sqlConn.Open()
            strSql = "INSERT INTO gc_categories VALUES(NEWID(), N'" & rtbNewCategory.Text & "', " & cmbCategories.Items.Count & ")"
            sqlComm = New SqlCommand(strSql, sqlConn)
            sqlComm.ExecuteNonQuery()

            cmbCategories.DataSourceID = Nothing
            cmbCategories.DataSourceID = "sqlGCCategories"
            cmbCategories.DataBind()
            rtbNewCategory.Text = ""
        Else
            sqlConn.Open()
            strSql = "UPDATE gc_categories SET gc_category_name = N'" & rtbNewCategory.Text & "' WHERE gc_category_uuid = '" & cmbCategories.SelectedValue & "'"
            sqlComm = New SqlCommand(strSql, sqlConn)
            sqlComm.ExecuteNonQuery()

            cmbCategories.SelectedItem.Text = rtbNewCategory.Text
        End If

    End Sub

    Protected Sub rgGCGoals_CancelCommand(sender As Object, e As GridCommandEventArgs)
        Dim strSql As String

        If e.CommandArgument = "doSubGoal" Then
            Dim gdiSubGoal As GridDataItem = CType(e.Item, GridDataItem)
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            sqlConn.Open()
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn
            strSql = "doGCSubGoal @gc_data_uuid"

            sqlComm.Parameters.Add(New SqlParameter("@gc_data_uuid", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters("@gc_data_uuid").Value = New Guid(gdiSubGoal.GetDataKeyValue("gc_data_uuid").ToString)

            sqlComm.CommandText = strSql
            sqlComm.ExecuteNonQuery()

            sqlConn.Close()

        End If

    End Sub

    Protected Sub rgGCGoals_RowDrop(sender As Object, e As GridDragDropEventArgs)
        'For Each draggedItem As GridDataItem In e.DraggedItems
        If e.DraggedItems.Count = 0 Then Exit Sub

        Dim oDataItem As GridDataItem
        Dim intCounter As Integer

        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        sqlConn.Open()
        Dim strSql As String = "doMoveGCGoal @gc_data_uuid_src, @gc_data_uuid_dest, @move_up"

        Dim draggedItems = e.DraggedItems.OrderBy(Function(a) a.ItemIndexHierarchical).ToArray()

        For intCounter = draggedItems.Count - 1 To 0 Step -1
            oDataItem = draggedItems(intCounter)
            Dim sqlComm As New SqlCommand
            sqlComm.Connection = sqlConn

            sqlComm.Parameters.Add(New SqlParameter("@gc_data_uuid_src", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters("@gc_data_uuid_src").Value = New Guid(oDataItem.GetDataKeyValue("gc_data_uuid").ToString)

            sqlComm.Parameters.Add(New SqlParameter("@move_up", SqlDbType.Bit))
            sqlComm.Parameters("@move_up").Value = Math.Abs(e.DropPosition - 1) 'IIf(draggedItems(0).ItemIndex > e.DestDataItem.ItemIndex, 1, 0)

            sqlComm.Parameters.Add(New SqlParameter("@gc_data_uuid_dest", SqlDbType.UniqueIdentifier))
            If e.DropPosition = GridItemDropPosition.Above And e.DestDataItem.ItemIndex <> 0 Then
                sqlComm.Parameters("@gc_data_uuid_dest").Value = New Guid(rgGCGoals.Items(e.DestDataItem.ItemIndex - 1).GetDataKeyValue("gc_data_uuid").ToString)
                sqlComm.Parameters("@move_up").Value = 0
            Else
                sqlComm.Parameters("@gc_data_uuid_dest").Value = New Guid(e.DestDataItem.GetDataKeyValue("gc_data_uuid").ToString)
            End If

            sqlComm.CommandText = strSql
            sqlComm.ExecuteNonQuery()
        Next
        sqlConn.Close()
        rgGCGoals.Rebind()
    End Sub

    Protected Sub RadAjaxManager1_AjaxRequest(sender As Object, e As AjaxRequestEventArgs)

        If e.Argument.IndexOf("ClipboardPaste") = 0 Then
            Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
            Dim strDataText As String = reClipboardTransfer.GetHtml(EditorStripHtmlOptions.None)
            Dim strGoals() As String = strDataText.Split("~")
            Dim intCounter As Integer
            Dim sqlComm As SqlCommand
            Dim strSql As String

            sqlConn.Open()

            For intCounter = UBound(strGoals) To 1 Step -1
                sqlComm = New SqlCommand
                sqlComm.Connection = sqlConn
                If UCase(strGoals(intCounter)) Like "[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F]-[0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F][0-9A-F]" Then
                    strSql = "doGCCloneIEP @gc_src_uuid, @gc_subject_uuid"
                    sqlComm.Parameters.Add(New SqlParameter("@gc_subject_uuid", SqlDbType.UniqueIdentifier))
                    sqlComm.Parameters("@gc_subject_uuid").Value = New Guid(cmbCategories.SelectedValue)

                    sqlComm.Parameters.Add(New SqlParameter("@gc_src_uuid", SqlDbType.UniqueIdentifier))
                    sqlComm.Parameters("@gc_src_uuid").Value = New Guid(strGoals(intCounter))
                Else
                    If Left(strGoals(intCounter), Len("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;")) = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;" Then
                        strGoals(intCounter) = strGoals(intCounter).Replace("&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;", "")
                        strSql = "doInsertGCSubGoal @gc_subject_uuid, @gc_data_text, @gc_dest_uuid"
                    Else
                        strSql = "doInsertGCGoal @gc_subject_uuid, @gc_data_text, @gc_dest_uuid"
                    End If
                    sqlComm.Parameters.Add(New SqlParameter("@gc_subject_uuid", SqlDbType.UniqueIdentifier))
                    sqlComm.Parameters("@gc_subject_uuid").Value = New Guid(cmbCategories.SelectedValue)

                    sqlComm.Parameters.Add(New SqlParameter("@gc_dest_uuid", SqlDbType.UniqueIdentifier))
                    If rgGCGoals.SelectedItems.Count = 1 Then
                        sqlComm.Parameters("@gc_dest_uuid").Value = New Guid(CType(rgGCGoals.SelectedItems(0), GridDataItem).GetDataKeyValue("gc_data_uuid").ToString)
                    Else
                        If rgGCGoals.Items.Count = 0 Then
                            sqlComm.Parameters("@gc_dest_uuid").Value = DBNull.Value
                        Else
                            sqlComm.Parameters("@gc_dest_uuid").Value = New Guid(CType(rgGCGoals.Items(rgGCGoals.Items.Count - 1), GridDataItem).GetDataKeyValue("gc_data_uuid").ToString)
                        End If
                    End If

                    sqlComm.Parameters.Add(New SqlParameter("@gc_data_text", SqlDbType.NVarChar))
                    sqlComm.Parameters("@gc_data_text").Value = strGoals(intCounter)
                End If

                sqlComm.CommandText = strSql
                sqlComm.ExecuteNonQuery()
            Next
            sqlConn.Close()
        End If

    End Sub
End Class