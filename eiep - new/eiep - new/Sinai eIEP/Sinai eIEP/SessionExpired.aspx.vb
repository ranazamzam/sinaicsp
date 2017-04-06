Public Class SessionExpired
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("expired") Is Nothing Then Session.Add("expired", 0)

        Session.Clear()
        Session.Abandon()
        Response.AppendHeader("Cache-Control", "no-store")
        Response.Cookies.Add(New HttpCookie("ASP.NET_SessionId", ""))
    End Sub

End Class