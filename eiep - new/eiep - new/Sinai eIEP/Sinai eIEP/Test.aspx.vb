Imports System
Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports Telerik.Web.UI
Imports System.Threading

Public Class Test
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If Not Context.User.Identity.IsAuthenticated Then
        '    Response.Redirect("Login.aspx")
        'End If

        'RadEditor1.Modules.Clear()
        'If Not IsPostBack Then
        '    RadEditor1.SpellCheckSettings.AllowAddCustom = True
        '    RadEditor1.SpellCheckSettings.SpellCheckProvider = SpellCheckProvider.PhoneticProvider
        'End If

    End Sub

End Class