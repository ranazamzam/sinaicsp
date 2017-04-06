Imports Telerik.Web.UI

Public Class admAccommodations
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim oGrid As RadGrid = CType(Me.Page.FindControl("AdminStudents").FindControl("RadGrid1"), RadGrid)

        If oGrid.EditIndexes.Count > 0 Then
            hdnStudentUUID.Value = oGrid.MasterTableView.DataKeyValues(oGrid.EditIndexes(0)).Item("student_uuid").ToString
        Else
            RadGrid1.Visible = False
        End If

    End Sub

End Class