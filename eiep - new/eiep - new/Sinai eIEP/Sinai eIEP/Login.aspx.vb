Imports Telerik.Web.UI

Public Class Login
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If IsPostBack And Context.User.Identity.IsAuthenticated Then
            If Session("JustLoggedIn") Is Nothing Then
                Session.Add("JustLoggedIn", True)
            End If

            Session("JustLoggedIn") = True
            'Response.Redirect("csp_list.aspx")
        End If

    End Sub

    Protected Sub RadAjaxManager1_AjaxRequest(sender As Object, e As Telerik.Web.UI.AjaxRequestEventArgs)
        If Session("JustLoggedIn") Is Nothing Then
            Session.Add("JustLoggedIn", True)
        End If
        Session("JustLoggedIn") = True
        FormsAuthentication.RedirectFromLoginPage(e.Argument.ToString, False)
        '        Response.Redirect("csp_list.aspx")
    End Sub

End Class