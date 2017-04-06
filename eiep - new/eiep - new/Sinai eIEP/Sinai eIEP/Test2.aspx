<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Test2.aspx.vb" Inherits="Sinai_eIEP.Test2" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
</head>
<body onunload="onClientUnload">
    <form id="form1" runat="server">
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        </telerik:RadAjaxManager>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
         <script type="text/javascript">

             var originalContent;
             var focusTimer;

             function GetRadWindow() {
                 var oWindow = null;
                 if (window.radWindow) oWindow = window.radWindow;
                 else if (window.frameElement.radWindow) oWindow = window.frameElement.radWindow;
                 return oWindow;
             }

             function setContent(content, iep_uuid, editMode) {
                 var editor = $find("<%= RadEditor1.ClientID%>");
                 document.getElementById("iep_uuid").value = iep_uuid
                 document.getElementById("client_mode").value = editMode;
                 if (editor) {
                     if (editMode != '') editor.set_html(content);  //set content from the parent page to RadEditor in RadWindow
                     originalContent = content;
                 }
              }

             function isDirty() {
                 var editor = $find("<%= RadEditor1.ClientID%>");
                 return (originalContent != editor.get_html(true));
             }

             function OnClientLoad(editor) {
                 editor.fire("ToggleScreenMode"); //set RadEditor in Full Scree mode                                        
              }

              function onClientUnload(sender, args) {
                  if (confirm("Save changes?")) {
                      var radWindow = GetRadWindow();
                      var browserWindow = radWindow.get_browserWindow();
                      var editor = $find("<%= RadEditor1.ClientID%>");
                      browserWindow.SetEditorContent(editor.get_html(true));     //set the editor content on RadWindow to the editor on the parent page
                      editor.set_html("");
                      radWindow.close(); //close RadWindow
                  }
              }

              function OnClientCommandExecuting(editor, args) {
                  var commandName = args.get_commandName();   //returns the executed command

                  if (commandName == "SaveAndClose") {
                      var radWindow = GetRadWindow();
                      var browserWindow = radWindow.get_browserWindow();
                      browserWindow.SetEditorContent(editor.get_html(true), "", document.getElementById("client_mode").value);     //set the editor content on RadWindow to the editor on the parent page
                      editor.set_html("");
                      originalContent = editor.get_html(true)
                      radWindow.close(); //close RadWindow
                      args.set_cancel(true); //cancel the SaveAndClose command
                  }
              }

              function onLoadEditor(editor, args) {
                  originalContent = "";
                  focusTimer = setTimeout(function () { if (originalContent == "") { $find("<%= RadEditor1.ClientID%>").set_html(""); } $find("<%= RadEditor1.ClientID%>").setFocus(); }, 500)
                  editor.attachEventHandler("onkeydown", function (e) {
                      if (e.keyCode == 13) { alert("Please save before adding a new goal."); }
                  });
              }

        </script>
        <asp:HiddenField ID="iep_uuid" runat="server" />
        <asp:HiddenField ID="client_mode" runat="server" />
        
        <div>
    
    </div>
        <telerik:RadEditor ID="RadEditor1" Runat="server" EditModes="Design" Width="100%" Height="290" OnClientCommandExecuting="OnClientCommandExecuting" OnClientLoad="onLoadEditor" StripFormattingOnPaste="All" Skin="Office2007" >
            
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
            <Tools >
                <telerik:EditorToolGroup Tag="Formatting">
                    <telerik:EditorTool Name="Bold" />
                    <telerik:EditorTool Name="Italic" />
                    <telerik:EditorTool Name="Underline" />
                    <telerik:EditorTool Name="SaveAndClose" />
                </telerik:EditorToolGroup>
            </Tools>
<Content>
</Content>

<TrackChangesSettings CanAcceptTrackChanges="False"></TrackChangesSettings>
        </telerik:RadEditor>
    </form>
</body>
</html>
