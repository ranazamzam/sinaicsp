<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Test.aspx.vb" Inherits="Sinai_eIEP.Test" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    <div>
    
    </div>
        <telerik:RadEditor ID="RadEditor1" Runat="server" EditModes="Design" >
<ExportSettings>
<Pdf>
<PageHeader>
<LeftCell Text=""></LeftCell>

<MiddleCell Text=""></MiddleCell>

<RightCell Text=""></RightCell>
</PageHeader>

<PageFooter>
<LeftCell Text=""></LeftCell>

<MiddleCell Text=""></MiddleCell>

<RightCell Text=""></RightCell>
</PageFooter>
</Pdf>
</ExportSettings>
            <Tools>
                <telerik:EditorToolGroup Tag="Formatting">
                    <telerik:EditorTool Name="Bold" />
                    <telerik:EditorTool Name="Italic" />
                    <telerik:EditorTool Name="Underline" />
                    <telerik:EditorTool Name="AjaxSpellCheck" />
                    <telerik:EditorTool Name="RichEditor" Text="Open Advanced Editor"></telerik:EditorTool>
                </telerik:EditorToolGroup>
            </Tools>
<Content>
</Content>

<TrackChangesSettings CanAcceptTrackChanges="False"></TrackChangesSettings>
        </telerik:RadEditor>
        <!--<telerik:RadWindow OnClientShow="SetDialogContent" OnClientPageLoad="SetDialogContent"
          NavigateUrl="Test2.aspx" runat="server" Behaviors="Close"
          ID="DialogWindow" VisibleStatusbar="false" Width="800px" Modal="true" Height="700px">
        </telerik:RadWindow>-->

        <script type="text/javascript">
            //<![CDATA[
            var editorContent = null;

            Telerik.Web.UI.Editor.CommandList["RichEditor"] = function (commandName, editor, args) {
                editorContent = editor.get_html(true); //get RadEditor content
                $find("<%= DialogWindow.ClientID %>").show(); //open RadWindow
          };


           function SetEditorContent(content) {
               //set content to RadEditor on the mane page from RadWindow
               $find("<%= RadEditor1.ClientID %>").set_html(content);
          }


            function SetDialogContent(oWnd) {
              var contentWindow = oWnd.get_contentFrame().contentWindow;
              if (contentWindow && contentWindow.setContent) {
                  window.setTimeout(function () {
                      //pass and set the content from the mane page to RadEditor in RadWindow
                      contentWindow.setContent(editorContent);
                  }, 500);
              }
          }
          //]]>
     </script>

    </form>
</body>
</html>
