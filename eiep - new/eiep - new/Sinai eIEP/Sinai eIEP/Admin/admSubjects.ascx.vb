Imports System
Imports System.Data
Imports System.Data.Sql
Imports System.Data.SqlClient
Imports Telerik.Web.UI

Public Class admSubjects
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub ddSubjects_NodeEdit(sender As Object, e As Telerik.Web.UI.RadTreeNodeEditEventArgs) Handles ddSubjects.NodeEdit
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        Dim strSql As String
        Dim strParentNodeId As String
        Dim sqlComm As SqlCommand

        sqlConn.Open()

        If e.Node.Value.ToString = "" Then
            If e.Node.Level = 0 Then
                strParentNodeId = "NULL"
            Else
                strParentNodeId = "'" & e.Node.ParentNode.Value & "'"
                strSql = "UPDATE subjects SET subject_order = subject_order + 1 WHERE subject_category = " & strParentNodeId & " AND subject_order > " & e.Node.Index
                sqlComm = New SqlCommand(strSql, sqlConn)
                sqlComm.ExecuteNonQuery()
            End If

            strSql = "SET @subject_uuid = NEWID() INSERT INTO subjects VALUES(@subject_uuid, " & strParentNodeId & ", @subject_name, " & e.Node.Index + 1 & ")"
        Else
            If UCase(e.Text) = "DELETE" Then
                strSql = "DELETE FROM subjects WHERE subject_uuid = '" & e.Node.Value.ToString & "'"
            Else
                strSql = "UPDATE subjects SET subject_name = @subject_name WHERE subject_uuid = '" & e.Node.Value.ToString & "'"
            End If
        End If
        sqlComm = New SqlCommand(strSql, sqlConn)
        sqlComm.Parameters.Add(New SqlParameter("@subject_name", SqlDbType.NVarChar))
        sqlComm.Parameters("@subject_name").Value = e.Text

        sqlComm.Parameters.Add(New SqlParameter("@subject_uuid", SqlDbType.UniqueIdentifier))
        sqlComm.Parameters("@subject_uuid").Direction = ParameterDirection.Output


        sqlComm.ExecuteNonQuery()

        sqlConn.Close()
        'ddSubjects.DataSourceID = Nothing
        'ddSubjects.DataSourceID = "sqlSubjects"
        'ddSubjects.DataBind()
        If UCase(e.Text) <> "DELETE" Then e.Node.Text = e.Text
        If e.Node.Value.ToString = "" Then e.Node.Value = sqlComm.Parameters("@subject_uuid").Value.ToString

    End Sub

    Protected Sub ddSubjects_NodeDrop(sender As Object, e As Telerik.Web.UI.RadTreeNodeDragDropEventArgs)
        Dim sqlConn As New SqlConnection("server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;")
        Dim strSql As String
        Dim sqlComm As SqlCommand
        Dim sourceNode As RadTreeNode = e.SourceDragNode
        Dim destNode As RadTreeNode = e.DestDragNode
        Dim dropPosition As RadTreeViewDropPosition = e.DropPosition

        If destNode IsNot Nothing Then
            If sourceNode.Equals(destNode) OrElse sourceNode.IsAncestorOf(destNode) Then
                Return
            End If
            sourceNode.Owner.Nodes.Remove(sourceNode)

            sqlConn.Open()
            sqlComm = New SqlCommand
            sqlComm.Connection = sqlConn

            strSql = "doMoveSubject @source_uuid, @dest_uuid, @dest_loc"
            sqlComm.Parameters.Add(New SqlParameter("@source_uuid", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters("@source_uuid").Value = New Guid(sourceNode.Value)
            sqlComm.Parameters.Add(New SqlParameter("@dest_uuid", SqlDbType.UniqueIdentifier))
            sqlComm.Parameters("@dest_uuid").Value = New Guid(destNode.Value)
            sqlComm.Parameters.Add(New SqlParameter("@dest_loc", SqlDbType.SmallInt))

            Select Case dropPosition
                Case RadTreeViewDropPosition.Over
                    ' child
                    If destNode.Level = 2 Then
                        destNode.InsertAfter(sourceNode)
                        sqlComm.Parameters("@dest_loc").Value = 0
                    ElseIf Not sourceNode.IsAncestorOf(destNode) Then
                        destNode.Nodes.Add(sourceNode)
                        sqlComm.Parameters("@dest_loc").Value = -1
                    End If
                    Exit Select
                Case RadTreeViewDropPosition.Above
                    ' sibling - above                         
                    destNode.InsertBefore(sourceNode)
                    sqlComm.Parameters("@dest_loc").Value = 1
                    Exit Select

                Case RadTreeViewDropPosition.Below
                    ' sibling - below
                    destNode.InsertAfter(sourceNode)
                    sqlComm.Parameters("@dest_loc").Value = 0
                    Exit Select
            End Select
            'dropped node will be a sibling of the destination node
            sqlComm.CommandText = strSql
            sqlComm.ExecuteNonQuery()
            destNode.Expanded = True
            sourceNode.TreeView.UnselectAllNodes()
        End If
    End Sub

    Protected Sub btnRefresh_Click(sender As Object, e As EventArgs)
        ddSubjects.DataBind()
    End Sub
End Class