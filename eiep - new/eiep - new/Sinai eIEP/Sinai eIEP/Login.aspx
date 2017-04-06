<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Login.aspx.vb" Inherits="Sinai_eIEP.Login" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html>
<head>
  <link href="https://fonts.googleapis.com/css?family=Roboto" rel="stylesheet" type="text/css">
  <script src="https://apis.google.com/js/api:client.js"></script>
  <script>
      var googleUser = {};
      var startApp = function () {
          gapi.load('auth2', function () {
              // Retrieve the singleton for the GoogleAuth library and set up the client.
              auth2 = gapi.auth2.init({
                  client_id: '620076569198.apps.googleusercontent.com',
                  cookiepolicy: 'single_host_origin',
                  // Request scopes in addition to 'profile' and 'email'
                  //scope: 'additional_scope'
              });
              attachSignin(document.getElementById('customBtn'));
          });
      };
      function attachSignin(element) {
          console.log(element.id);
          auth2.attachClickHandler(element, {},
              function (googleUser) {
                  //document.getElementById('name').innerText = "Signed in: " + googleUser.getBasicProfile().getEmail();
                  $find("<%= RadAjaxManager1.ClientID%>").ajaxRequest(googleUser.getBasicProfile().getEmail());
              }, function (error) {
                  alert(JSON.stringify(error, undefined, 2));
              });
          }
  </script>
  <style type="text/css">
    #customBtn {
      display: inline-block;
      background: #4285f4;
      color: white;
      width: 190px;
      border-radius: 5px;
      white-space: nowrap;
    }
    #customBtn:hover {
      cursor: pointer;
    }
    span.label {
      font-weight: bold;
    }
    span.icon {
      background: url('images/g-normal.png') transparent 5px 50% no-repeat;
      display: inline-block;
      vertical-align: middle;
      width: 42px;
      height: 42px;
      border-right: #2265d4 1px solid;
    }
    span.buttonText {
      display: inline-block;
      vertical-align: middle;
      padding-left: 42px;
      padding-right: 42px;
      font-size: 14px;
      font-weight: bold;
      /* Use the Roboto font that is loaded in the <head> */
      font-family: 'Roboto', sans-serif;
    }
  </style>
  </head>
  <body>

  <!-- In the callback, you would hide the gSignInWrapper element on a
  successful sign in -->
      <form id="Form2" runat="server">
      <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="30" ></asp:ScriptManager>
      <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            
        </telerik:RadAjaxManager>
    
        <asp:Table ID="tblHeading" runat="server" Width="100%" >
            <asp:TableRow>
                <asp:TableCell Width="100%" HorizontalAlign="Center"><p>&nbsp;</p></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Width="100%" HorizontalAlign="Center"><img class="auto-style1" src="images/SINAI%20Schools%20logo.jpeg" height="100px" /></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Width="100%" HorizontalAlign="Center"><p>&nbsp;</p></asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell Width="100%" HorizontalAlign="Center"><div id="gSignInWrapper">
     
    <div id="customBtn" class="customGPlusSignIn" style="align-self:center">
      <span class="icon"></span>
      <span class="buttonText">Sign In</span>
    </div>
  </div>
  <div id="name"></div>
  <script>startApp();</script>
  </asp:TableCell>
            </asp:TableRow>
        </asp:Table>
      
  
          </form>
</body>
</html>