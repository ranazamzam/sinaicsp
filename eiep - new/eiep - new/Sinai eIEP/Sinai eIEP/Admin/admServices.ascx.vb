Imports Telerik.Web.UI

Public Class admServices
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim oGrid As RadGrid = CType(Me.Page.FindControl("AdminStudents").FindControl("RadGrid1"), RadGrid)

        If oGrid.EditIndexes.Count > 0 Then
            hdnStudentUUID.Value = oGrid.MasterTableView.DataKeyValues(oGrid.EditIndexes(0)).Item("student_uuid").ToString
        Else
            RadGrid1.Visible = False
        End If

    End Sub

    Private Sub RadGrid1_CancelCommand(sender As Object, e As GridCommandEventArgs) Handles RadGrid1.CancelCommand
        hdnServicesParent.Value = ""
    End Sub

    Private Sub RadGrid1_ItemDataBound(sender As Object, e As GridItemEventArgs) Handles RadGrid1.ItemDataBound

        Dim oItem As GridEditableItem
        Dim oNumTxt As RadNumericTextBox
        Dim oTxt As TextBox
        Dim intTmp As Integer
        Dim intTmp1 As Integer

        If TypeOf (e.Item) Is GridEditableItem And e.Item.IsInEditMode Then
            oItem = e.Item
            oNumTxt = oItem("num_students").Controls(0)
            oNumTxt.Width = Unit.Pixel(30)
            oTxt = oItem("service_model").Controls(0)
            oTxt.Width = Unit.Pixel(50)
            oTxt = oItem("session_length").Controls(0)
            oTxt.Width = Unit.Pixel(75)
            oTxt = oItem("weekly_sessions").Controls(0)
            oTxt.Width = Unit.Pixel(75)

            If hdnServicesParent.Value <> "" Then
                For intTmp = 0 To RadGrid1.MasterTableView.Items.Count - 1
                    If RadGrid1.MasterTableView.Items(intTmp).GetDataKeyValue("services_parent").ToString = hdnServicesParent.Value Then
                        For intTmp1 = intTmp To RadGrid1.MasterTableView.Items.Count - 1
                            If RadGrid1.MasterTableView.Items(intTmp1).GetDataKeyValue("services_parent").ToString <> hdnServicesParent.Value Then
                                Exit For
                            End If
                        Next
                        intTmp1 = intTmp1 - 1
                        Exit For
                    End If
                Next
                CType(oItem("services_name").Controls(0), TextBox).Text = RadGrid1.MasterTableView.Items(intTmp).Cells(7).Text 'RadGrid1.MasterTableView.Items(intTmp1).GetDataKeyValue("services_name")
                CType(oItem("provider_name").Controls(0), TextBox).Text = RadGrid1.MasterTableView.Items(intTmp).Cells(8).Text 'RadGrid1.MasterTableView.Items(intTmp1).GetDataKeyValue("provider_name")
                CType(oItem("service_model").Controls(0), TextBox).Text = RadGrid1.MasterTableView.Items(intTmp).Cells(9).Text 'RadGrid1.MasterTableView.Items(intTmp1).GetDataKeyValue("service_model")
                CType(oItem("num_students").Controls(0), RadNumericTextBox).Text = RadGrid1.MasterTableView.Items(intTmp).Cells(10).Text 'RadGrid1.MasterTableView.Items(intTmp1).GetDataKeyValue("num_students")
                CType(oItem("session_length").Controls(0), TextBox).Text = RadGrid1.MasterTableView.Items(intTmp).Cells(11).Text 'RadGrid1.MasterTableView.Items(intTmp1).GetDataKeyValue("session_length")
                CType(oItem("weekly_sessions").Controls(0), TextBox).Text = RadGrid1.MasterTableView.Items(intTmp).Cells(12).Text 'RadGrid1.MasterTableView.Items(intTmp1).GetDataKeyValue("weekly_sessions")

            End If

            If oItem.FindControl("PerformInsertButton") Is Nothing Then
                Dim update As ImageButton = CType(oItem.FindControl("UpdateButton"), ImageButton)
                update.Attributes.Add("onclick", "doValidate('" & oItem("session_start_date").Controls(0).ClientID & "');")
            Else
                Dim insert As ImageButton = CType(oItem.FindControl("PerformInsertButton"), ImageButton)
                insert.Attributes.Add("onclick", "doValidate('" & oItem("session_start_date").Controls(0).ClientID & "');")
            End If
            
        End If

    End Sub

    Private Sub RadGrid1_ItemInserted(sender As Object, e As GridInsertedEventArgs) Handles RadGrid1.ItemInserted
        hdnServicesParent.Value = ""
    End Sub

    Protected Sub SqlDataSource1_Updating(sender As Object, e As SqlDataSourceCommandEventArgs)

    End Sub
End Class