<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="admGoalCatalog.ascx.vb" Inherits="Sinai_eIEP.admGoalCatalog" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

    <script type="text/javascript">
        function onSelecting(sender, args) {
            
            if (sender.get_selectedItems().length == 1) {
                document.getElementById("AdminGoalCatalog_selectedGoal").value = sender.get_selectedItems()[0].getDataKeyValue("gc_data_uuid")
            } else {
                document.getElementById("AdminGoalCatalog_selectedGoal").value = "00000000-0000-0000-0000-000000000000"
            }
        }
        function onRowCreated(sender, args) {
            if (args.get_gridDataItem().getDataKeyValue("gc_data_parent_uuid") != "") {
                args.get_gridDataItem().get_element().cells[7].innerHTML = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + args.get_gridDataItem().get_element().cells[7].innerHTML
                args.get_gridDataItem().get_element().cells[2].innerHTML = args.get_gridDataItem().get_element().cells[2].innerHTML.replace("_right", "_left")
            } else if (args.get_itemIndexHierarchical() == "0") {
                args.get_gridDataItem().get_element().cells[2].innerHTML = "&nbsp;"
            }
        }
        function goalRowClick(sender, args) {
            var intItemNum = parseInt(args.get_itemIndexHierarchical()) + 1;

            if (sender.get_masterTableView().get_dataItems()[intItemNum] != null) {
                if (sender.get_masterTableView().get_dataItems()[intItemNum - 1].getDataKeyValue("gc_data_parent_uuid") == "" && sender.get_masterTableView().get_dataItems()[intItemNum].getDataKeyValue("gc_data_parent_uuid") != "") {
                    while (sender.get_masterTableView().get_dataItems()[intItemNum].getDataKeyValue("gc_data_parent_uuid") != "") {
                        sender.get_masterTableView().get_dataItems()[intItemNum].set_selected(true);
                        intItemNum += 1;
                        if (sender.get_masterTableView().get_dataItems()[intItemNum] == null) {
                            return;
                        }
                    }
                }
            }
        }

        function doClipboardInit(sender, args) {
            var list = $find("<%= lbClipboard.ClientID%>")
                var items = list.get_items();
                var item;
                var transferItems = $find("<%= reClipboardTransfer.ClientID%>").get_html().split("~")
                var i

                for (i = 1; i < transferItems.length; i++) {
                    item = new Telerik.Web.UI.RadListBoxItem();
                    item.set_value(transferItems[i].split('|||')[0]);
                    item.set_text(transferItems[i].split('|||')[1])
                    items.add(item);
                    list.commitChanges();
                }

        }

        function clipboardContextMenu(sender, eventArgs) {
            var menu = $find("<%=ctxClipboard.ClientID%>");
            var evt = eventArgs.get_domEvent();
            myList = $find(sender._element.id);
            if (myList.get_id().slice(-3) == 'Cat') {
                var item = menu.findItemByText("Clear Clipboard");
                if (item) {
                    item.hide();
                }
            } else {
                var item = menu.findItemByText("Clear Clipboard");
                if (item) {
                    item.show();
                }
            }
            menu.show(evt);
        }

        function cpMenuClick(sender, args) {
            switch (args.get_item().get_value()) {
                case "pasteSelected":
                    pasteItems();
                    break;
                case "selectAll":
                    myList.get_items().forEach(function (itm) { itm.check(); });
                    break;
                case "unSelectAll":
                    myList.get_items().forEach(function (itm) { itm.uncheck(); });
                    break;
                case "clearList":
                    myList.get_items().clear();
                    break;
            }
        }

        function doCheck(sender, args) {
            var item = args.get_item();

            if (item.get_selected()) {
                if (item.get_checked()) {
                    item.uncheck();
                }
                else {
                    item.check();
                }
                item.unselect();
            }
        }

        function pasteItems() {
            var checkedItems = myList.get_checkedItems();
            var i;
            var strItems = '';
            var sPrefix = '';

            if (myList.get_id().slice(-3) == 'Cat') {
                sPrefix = 'gc_';
            }

            if (checkedItems.length == 0) {
                return
            }

            for (i = 0; i < checkedItems.length; i++) {
                strItems = strItems + '~' + sPrefix + checkedItems[i].get_value();
            }

            console.log(strItems)

            $find("<%= reClipboardTransfer.ClientID%>").set_html(strItems);
            $find("<%= RadAjaxLoadingPanel1.ClientID%>").show("<%= rgGCGoals.ClientID%>")
            $find("<%= RadAjaxManager1.ClientID%>").ajaxRequest("ClipboardPaste");
            goalTimer = setTimeout(checkStatus, 2500);
        }

        function checkStatus() {
            $find("<%= rgGCGoals.ClientID%>").get_masterTableView().rebind();
            $find("<%= RadAjaxLoadingPanel1.ClientID%>").hide("<%= rgGCGoals.ClientID%>")
        }
    </script>

    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" ></telerik:RadAjaxLoadingPanel>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" RequestQueueSize="30" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="rgGCGoals" >
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="rgGCGoals" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>

<telerik:RadSplitter ID="rspCspDetails" runat="server" BorderStyle="None" Height="800px" Width="1000px" BorderSize="0" PanesBorderSize="0" >
            <telerik:RadPane runat="server" ID="rpMain" >
                <asp:Panel ID="mainPanel" runat="server" style="width:98%;margin-left:auto;margin-right:auto" Enabled="true">
    <asp:SqlDataSource ID="sqlGCCategories" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT * FROM gc_categories UNION SELECT '00000000-0000-0000-0000-000000000000', '', -1 ORDER BY gc_category_order ">
    </asp:SqlDataSource>
    <!--<asp:SqlDataSource ID="sqlGCSubjects" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT * FROM gc_subjects WHERE gc_category_uuid = @gc_category_uuid UNION SELECT '00000000-0000-0000-0000-000000000000', NULL, '', -1 ORDER BY gc_subject_order ">
        <SelectParameters>
            <asp:ControlParameter ControlID="cmbCategories" Name="gc_category_uuid" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>-->
    <asp:SqlDataSource ID="sqlGCGoals" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT * FROM gc_data WHERE gc_subject_uuid = @gc_subject_uuid ORDER BY gc_data_sequence, gc_sub_data_sequence"
        UpdateCommand="UPDATE gc_data SET gc_data_text = @gc_data_text WHERE gc_data_uuid = @gc_data_uuid"
        InsertCommand="doInsertGCGoal @gc_subject_uuid, @gc_data_text, @gc_dest_uuid"
        DeleteCommand="deleteGCGoal @gc_data_uuid">
        <SelectParameters>
            <asp:ControlParameter ControlID="cmbCategories" Name="gc_subject_uuid" PropertyName="SelectedValue" />
        </SelectParameters>
        <InsertParameters>
            <asp:ControlParameter ControlID="selectedGoal" Name="gc_dest_uuid" PropertyName="Value" />
            <asp:ControlParameter ControlID="cmbCategories" Name="gc_subject_uuid" PropertyName="SelectedValue" />
        </InsertParameters>
    </asp:SqlDataSource>

    <asp:HiddenField runat="server" ID="selectedGoal" Value="00000000-0000-0000-0000-000000000000" />
    <div style="display:none">
        <telerik:RadEditor ID="reClipboardTransfer" Runat="server" EditModes="Design" Width="100%" Height="100" Visible="true" OnClientLoad="doClipboardInit" />
    </div>

    <asp:table ID="Table1" runat="server" CellPadding="2" CellSpacing="2" Width="98%" BorderStyle="Solid" BorderWidth="1">
        <asp:TableRow>
            <asp:TableCell Width="2%"></asp:TableCell>
            <asp:TableCell>Subject: 
                <telerik:RadComboBox runat="server" ID="cmbCategories" AutoPostBack="true" Height="100px" DataSourceID="sqlGCCategories" DataTextField="gc_category_name" DataValueField="gc_category_uuid" OnSelectedIndexChanged="cmbCategories_SelectedIndexChanged" >
                </telerik:RadComboBox>
                &nbsp;<telerik:RadTextBox runat="server" ID="rtbNewCategory" />
                &nbsp;<telerik:RadButton runat="server" ID="rbtNewCat" OnClick="rbtNewCat_Click" Text="Add" />
            </asp:TableCell>
           
        </asp:TableRow>
    </asp:table>

    <telerik:RadGrid ID="rgGCGoals" runat="server" AutoGenerateColumns="False" AllowAutomaticUpdates="true" AllowAutomaticDeletes="true" AllowAutomaticInserts="true" CellSpacing="0" DataSourceID="sqlGCGoals" GridLines="None" AllowMultiRowSelection="True" ClientSettings-Selecting-AllowRowSelect="true"  OnCancelCommand="rgGCGoals_CancelCommand" OnRowDrop="rgGCGoals_RowDrop" >
        <MasterTableView DataKeyNames="gc_data_uuid" DataSourceID="sqlGCGoals" CommandItemDisplay="Top" ClientDataKeyNames="gc_data_uuid, gc_data_parent_uuid" >
            <Columns>
                <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn">
                    <HeaderStyle Width="15px"></HeaderStyle>
                </telerik:GridClientSelectColumn>
                <telerik:GridButtonColumn ConfirmText="Delete this goal?" ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn1" ShowInEditForm="false">
                    <HeaderStyle Width="15px"></HeaderStyle>
                    <ItemStyle CssClass="MyImageButton"></ItemStyle>
                </telerik:GridButtonColumn>
                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="Cancel" CommandArgument="doSubGoal" UniqueName="SubGoal" ImageUrl="~/images/arrow_right.gif" Visible="true" ShowInEditForm="false">
                    <HeaderStyle Width="15px"></HeaderStyle>
                    <ItemStyle CssClass="MyImageButton"></ItemStyle>
                </telerik:GridButtonColumn>
                <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="EditCommandColumn1" CommandName="Edit" Text="Edit goal text" ShowInEditForm="false" ImageUrl="~/images/Edit.gif" >
                    <HeaderStyle Width="15px"></HeaderStyle>
                    <ItemStyle CssClass="MyImageButton"></ItemStyle>
                </telerik:GridButtonColumn>
                <telerik:GridBoundColumn DataField="gc_data_uuid" DataType="System.Guid" FilterControlAltText="Filter gc_goal_uuid column" HeaderText="gc_goal_uuid" ReadOnly="True" SortExpression="gc_goal_uuid" UniqueName="gc_goal_uuid" Display="False" >
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="gc_data_parent_uuid" DataType="System.Guid" FilterControlAltText="Filter gc_parent_uuid column" HeaderText="gc_parent_uuid" ReadOnly="True" SortExpression="gc_parent_uuid" UniqueName="gc_parent_uuid" Display="False" Visible="false">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="gc_data_sequence" DataType="System.Byte" FilterControlAltText="Filter gc_goal_sequence column" HeaderText="gc_goal_sequence" ReadOnly="True" SortExpression="gc_goal_sequence" UniqueName="gc_goal_sequence" Display="False">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="gc_sub_data_sequence" DataType="System.Byte" FilterControlAltText="Filter gc_goal_sub_sequence column" HeaderText="gc_goal_sub_sequence" ReadOnly="True" SortExpression="gc_goal_sub_sequence" UniqueName="gc_goal_sub_sequence" Display="False">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridBoundColumn>
                <telerik:GridTemplateColumn UniqueName="gc_data_text" HeaderText="Goal" EditFormHeaderTextFormat="" >
                         <ItemTemplate>
                              <asp:Label ID="lblField1" CssClass="text" runat="server" Text='<%# Eval("gc_data_text")%>'></asp:Label>
                         </ItemTemplate>
                         <EditItemTemplate>
                              <telerik:RadEditor ID="RadEditor1" runat="server" content='<%# Bind("gc_data_text")%>' EditModes="Design" Width="100%" Height="150" StripFormattingOnPaste="All" >
                                   <Tools >
                                        <telerik:EditorToolGroup Tag="Formatting">
                                            <telerik:EditorTool Name="Bold" />
                                            <telerik:EditorTool Name="Italic" />
                                            <telerik:EditorTool Name="Underline" />
                                        </telerik:EditorToolGroup>
                                    </Tools>
                              </telerik:RadEditor>
                         </EditItemTemplate>
                         <InsertItemTemplate>
                              <telerik:RadEditor ID="RadEditor1" runat="server" content='<%# Bind("gc_data_text")%>' EditModes="Design" Width="100%" Height="150" StripFormattingOnPaste="All" >
                                   <Tools >
                                        <telerik:EditorToolGroup Tag="Formatting">
                                            <telerik:EditorTool Name="Bold" />
                                            <telerik:EditorTool Name="Italic" />
                                            <telerik:EditorTool Name="Underline" />
                                        </telerik:EditorToolGroup>
                                    </Tools>
                              </telerik:RadEditor>
                         </InsertItemTemplate>
                    </telerik:GridTemplateColumn>
            </Columns>
        </MasterTableView>
        <ClientSettings AllowRowsDragDrop="True" >
            <ClientEvents OnRowSelected="onSelecting" OnRowCreated="onRowCreated" OnRowSelecting="goalRowClick" />
        </ClientSettings>
    </telerik:RadGrid>
                </asp:Panel>
            </telerik:RadPane>

    <telerik:RadSplitBar ID="RadSplitBar2" runat="server" />
            <telerik:RadPane ID="EndPane" runat="server" Width="22px" Scrolling="None" >
                    <telerik:RadSlidingZone ID="rslToolbox" runat="server" Width="22px" ClickToOpen="true" SlideDirection="Left" >
                        <telerik:RadSlidingPane ID="rslpClipboard" Title="Clipboard" runat="server" Width="500px" MinWidth="100" TabView="ImageOnly" IconUrl="~/images/clip.png">
                             <telerik:RadListBox ID="lbClipboard" runat="server" Width="100%" CheckBoxes="true"  AutoPostBack="false" EnableViewState="false" OnClientSelectedIndexChanged="doCheck" OnClientContextMenu="clipboardContextMenu"/>

                                <telerik:RadContextMenu runat="server" ID="ctxClipboard" EnableRoundedCorners="true" EnableShadows="true" OnClientItemClicked="cpMenuClick">
                                    <Items>
                                        <telerik:RadMenuItem Text="Paste Selected" ImageUrl="~/images/clone.png" Value="pasteSelected" />
                                        <telerik:RadMenuItem Text="Select All" ImageUrl="~/images/checkbox_checked.png" Value="selectAll" />
                                        <telerik:RadMenuItem Text="Unselect All" ImageUrl="~/images/checkbox_unchecked.png" Value="unSelectAll" />
                                        <telerik:RadMenuItem Text="Clear Clipboard" ImageUrl="~/images/RedX_large.gif" Value="clearList" />
                                    </Items>
                                </telerik:RadContextMenu>
                        </telerik:RadSlidingPane>  
                        
                </telerik:RadSlidingZone>
            </telerik:RadPane>
</telerik:RadSplitter>
