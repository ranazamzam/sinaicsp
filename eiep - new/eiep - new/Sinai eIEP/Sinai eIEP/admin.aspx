<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="admin.aspx.vb" Inherits="Sinai_eIEP.admin" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register Src="~/admin/admUsers.ascx" TagName="AdminUsers" TagPrefix="uc" %>
<%@ Register Src="~/admin/admStudents.ascx" TagName="AdminStudents" TagPrefix="uc" %>
<%@ Register Src="~/admin/admSubjects.ascx" TagName="AdminSubjects" TagPrefix="uc" %>
<%@ Register Src="~/admin/admLocks.ascx" TagName="AdminLocks" TagPrefix="uc" %>
<%@ Register Src="~/admin/admGoalCatalog.ascx" TagName="AdminGoalCatalog" TagPrefix="uc" %>
<%@ Register Src="~/admin/admProviderServices.ascx" TagName="AdminProviderServices" TagPrefix="uc" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 189px;
            height: 98px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        
        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">
                var hoveredItem;

                function cmcSubjects(asender, eventArgs) {
                    console.log("menu")
                    sender = eventArgs.get_menu()
                    if (hoveredItem != null && hoveredItem != undefined) {
                        hoveredItem.select();
                        switch (hoveredItem.get_level()) {
                            case 0:
                                item = sender.findItemByText("New Subject");
                                if (item) { item.show(); }
                                item = sender.findItemByText("New Sub-Subject");
                                if (item) { item.hide(); }
                                break;
                            case 1:
                                item = sender.findItemByText("New Subject");
                                if (item) { item.show(); }
                                item = sender.findItemByText("New Sub-Subject");
                                if (item) { item.show(); }
                                break;
                            case 2:
                                item = sender.findItemByText("New Subject");
                                if (item) { item.hide(); }
                                item = sender.findItemByText("New Sub-Subject");
                                if (item) { item.show(); }
                                break;
                        }
                    }
                }

                function getHover(sender, args) {
                    hoveredItem = args._node;
                }

                function editItem(asender, args) {
                    var tree = hoveredItem.get_treeView();
                    var menuItem = args.get_menuItem();
                    var treeNode = args.get_node();
                    menuItem.get_menu().hide();

                    switch (menuItem.get_value()) {
                        case 'newSubCategory':
                            tree.trackChanges();
                            var node = new Telerik.Web.UI.RadTreeNode();
                            node.set_text("New Category");
                            tree.get_nodes().add(node);
                            tree.commitChanges();
                            tree.trackChanges();
                            node.startEdit();
                            break;
                        case 'newSubject':
                            // Only allow when level 1 or 2 item is selected
                            tree.trackChanges();
                            var node = new Telerik.Web.UI.RadTreeNode();
                            node.set_text("New Subject");
                            if (hoveredItem.get_level() == 0) {
                                hoveredItem.expand();
                                hoveredItem.get_nodes().insert(0, node);
                            } else {
                                hoveredItem.get_parent().get_nodes().insert(hoveredItem.get_index() + 1, node);
                            }
                            tree.commitChanges();
                            tree.trackChanges();
                            node.startEdit();
                            break;
                        case 'newSubSubject':
                            // Only allow when level 2 or 3 item is selected
                            tree.trackChanges();
                            var node = new Telerik.Web.UI.RadTreeNode();
                            node.set_text("New Sub-Subject");
                            if (hoveredItem.get_level() == 1) {
                                hoveredItem.expand();
                                hoveredItem.get_nodes().insert(0, node);
                            } else {
                                hoveredItem.get_parent().get_nodes().insert(hoveredItem.get_index() + 1, node);
                            }
                            tree.commitChanges();
                            tree.trackChanges();
                            node.startEdit();
                            break;
                        case 'editSubject':
                            if (hoveredItem != null && hoveredItem != undefined) {
                                tree.trackChanges();
                                hoveredItem.startEdit();
                            }
                            break;
                    }
                }

                function tvRebind(sender, args) {
                    /*console.log(args)
                    if (args._node._text != 'New Category' && args._node._text != 'New Subject' && args._node._text != 'New Sub-Subject') {
                        $find("AdminSubjects_RadAjaxLoadingPanel1").show('AdminSubjects_ddSubjects');
                    }*/
                }

            </script>
        </telerik:RadCodeBlock>

        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false" Skin="Outlook">
            <TargetControls>
                <telerik:TargetControl ControlID="tbAdmin" Skin="Outlook" />
            </TargetControls>
        </telerik:RadSkinManager>

        <img class="auto-style1" src="images/SINAI%20Schools%20logo.jpeg" /><br /><br />

        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" RequestQueueSize="5"></telerik:RadAjaxManager>
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js">
                </asp:ScriptReference>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js">
                </asp:ScriptReference>
            </Scripts>
        </telerik:RadScriptManager>
    <div>
    <telerik:RadTabStrip runat="server" Width="1000px"
            ID="tbAdmin"
            Align="Justify"
            AutoPostBack="true"
            MultiPageID="RadMultiPage1"
            SelectedIndex="0">
            <Tabs>
                <telerik:RadTab Width="144px" Text="Users / Teachers"></telerik:RadTab>
                <telerik:RadTab Width="142px" Text="Students"></telerik:RadTab>
                <telerik:RadTab Width="142px" Text="Subjects"></telerik:RadTab>
                <telerik:RadTab Width="142px" Text="Locks"></telerik:RadTab>
                <telerik:RadTab Width="144px" Text="Goal Catalog"></telerik:RadTab>
                <telerik:RadTab Width="142px" Text="Services"></telerik:RadTab>
                <telerik:RadTab Width="144px" Text="End Of Year" Enabled="false"></telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0">
            <telerik:RadPageView runat="server" Width="1000px" Height="700px" ID="RadPageView1">
                <div class="contentWrapper">
                    <uc:AdminUsers runat="server" ID="AdminUsers" />
                </div>
            </telerik:RadPageView>
            <telerik:RadPageView runat="server" Width="1000px" Height="700px" ID="RadPageView2">
                <div class="contentWrapper">
                    <uc:AdminStudents runat="server" ID="AdminStudents" />
                </div>
            </telerik:RadPageView>
 
            <telerik:RadPageView runat="server" Width="1000px" Height="700px" ID="RadPageView3">
                <div class="contentWrapper">
                    <uc:AdminSubjects runat="server" ID="AdminSubjects" />
                </div>
            </telerik:RadPageView>
            
            <telerik:RadPageView runat="server" Width="1000px" Height="700px" ID="RadPageView4">
                <div class="contentWrapper">
                    <uc:AdminLocks runat="server" ID="AdminLocks" />
                </div>
            </telerik:RadPageView>
            <telerik:RadPageView runat="server" Width="1000px" Height="700px" ID="RadPageView5">
                <div class="contentWrapper">
                    <uc:AdminGoalCatalog runat="server" ID="AdminGoalCatalog" />
                </div>
            </telerik:RadPageView>
            
            <telerik:RadPageView runat="server" Width="1000px" Height="700px" ID="RadPageView6">
                <div class="contentWrapper">
                    <uc:AdminProviderServices runat="server" ID="AdminProviderServices" />
                </div>
            </telerik:RadPageView>
        </telerik:RadMultiPage>
    </div>
    </form>
</body>
</html>
