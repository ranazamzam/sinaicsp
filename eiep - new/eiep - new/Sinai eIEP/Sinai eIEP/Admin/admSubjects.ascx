<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="admSubjects.ascx.vb" Inherits="Sinai_eIEP.admSubjects" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:SqlDataSource ID="sqlSubjects" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT * FROM subjects ORDER BY subject_order">
</asp:SqlDataSource>

    <telerik:RadAjaxLoadingPanel runat="server" ID="lpSubjects" Skin="Outlook" />

<telerik:RadAjaxPanel runat="server" ID="rpnlSubjects" LoadingPanelID="lpSubjects">
    <asp:table ID="Table1" runat="server" CellPadding="2" CellSpacing="2" Width="1000px" BorderStyle="Solid" BorderWidth="1">
        <asp:TableRow>
            <asp:TableCell Width="2%"></asp:TableCell>
            <asp:TableCell Width="98%">
                <telerik:RadButton runat="server" ID="btnRefresh" OnClick="btnRefresh_Click">
                    <Icon PrimaryIconCssClass="rbRefresh"></Icon>
                </telerik:RadButton>
    
                <telerik:RadTreeView ID="ddSubjects" onClientContextMenuShowing="cmcSubjects" OnClientContextMenuItemClicking="editItem" runat="server" Width="475" TextMode="FullPath" Height="375" DataSourceID="sqlSubjects" DataFieldParentID="subject_category" DataFieldID="subject_uuid" DataTextField="subject_name" Enabled="true" DataValueField="subject_uuid" EnableDragAndDrop="true" OnClientMouseOver="getHover" AllowNodeEditing="true" OnNodeDrop="ddSubjects_NodeDrop" EnableDragAndDropBetweenNodes="true" >
                    <ContextMenus>
                        <telerik:RadtreeviewContextMenu runat="server" ID="ctxMenu" EnableRoundedCorners="true" EnableShadows="true" >
                            <Items>
                                <telerik:RadMenuItem Text="New Category" ImageUrl="../images/AddRecord.gif" Value="newSubCategory" />
                                <telerik:RadMenuItem Text="New Subject" ImageUrl="../images/AddRecord.gif" Value="newSubject" />
                                <telerik:RadMenuItem Text="New Sub-Subject" ImageUrl="../images/AddRecord.gif" Value="newSubSubject" />
                                <telerik:RadMenuItem Text="Modify Subject" ImageUrl="../images/Edit.gif" Value="editSubject"/>
                            </Items>
                        </telerik:RadtreeviewContextMenu>
                    </ContextMenus>
                </telerik:RadTreeView>
            </asp:TableCell>
        </asp:TableRow>
    </asp:table>
</telerik:RadAjaxPanel>