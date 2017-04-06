<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SessionExpired.aspx.vb" Inherits="Sinai_eIEP.SessionExpired" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Table ID="tblHeading" runat="server" Width="100%" >
            <asp:TableRow>
                <asp:TableCell Width="50%"><img class="auto-style1" src="images/SINAI%20Schools%20logo.jpeg" height="70px" /></asp:TableCell>
                <asp:TableCell Width="48%" HorizontalAlign="Right"><a runat="server" href="/admin.aspx" target="_blank" style="display:none" id="admin_link">Admin</a></asp:TableCell>
                <asp:TableCell Width="2%">&nbsp;</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <br />
        <h1 style="color:red;text-align:center">Session expired.</h1>
    </div>
    </form>
</body>
</html>
