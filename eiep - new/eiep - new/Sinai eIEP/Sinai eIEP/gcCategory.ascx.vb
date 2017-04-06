Imports Telerik.Web.UI

Public Class gcCategory
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        hdnGCCatId.Value = Me.ID
        
    End Sub

End Class