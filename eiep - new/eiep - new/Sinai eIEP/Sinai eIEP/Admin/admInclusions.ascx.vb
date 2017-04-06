Imports Telerik.Web.UI

Public Class admInclusions
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim oGrid As RadGrid = CType(Me.Page.FindControl("AdminStudents").FindControl("RadGrid1"), RadGrid)

        If oGrid.EditIndexes.Count > 0 Then
            hdnStudentUUID.Value = oGrid.MasterTableView.DataKeyValues(oGrid.EditIndexes(0)).Item("student_uuid").ToString
        Else
            RadGrid1.Visible = False
        End If

    End Sub

    Private Sub RadGrid1_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles RadGrid1.ItemDataBound

        Dim oItem As GridEditableItem
        Dim oNumTxt As RadNumericTextBox
        Dim oTxt As TextBox

        If TypeOf (e.Item) Is GridEditableItem And e.Item.IsInEditMode Then
            oItem = e.Item
            oTxt = oItem("num_classes").Controls(0)
            oTxt.Width = Unit.Pixel(50)
            oTxt = oItem("subject_name").Controls(0)
            oTxt.Width = Unit.Pixel(250)
            oTxt = oItem("teacher_name").Controls(0)
            oTxt.Width = Unit.Pixel(250)
        End If
 
    End Sub

End Class