<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="admUsers.ascx.vb" Inherits="Sinai_eIEP.admUsers" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT usrs.*, school_name FROM usrs LEFT OUTER JOIN schools ON usrs.school_id = schools.school_id WHERE usrs.is_active <> @is_active AND (usrs.school_id = @school_id OR @school_id = 0) AND (usr_uuid = @usr_uuid OR @usr_uuid = '00000000-0000-0000-0000-000000000000')"
        UpdateCommand="UPDATE [dbo].[usrs] SET [usr_login] = @usr_login, [usr_display_name] = @usr_display_name, [role_id] = @role_id, [usr_title] = @usr_title, [school_id] = CASE @school_id WHEN 0 THEN NULL ELSE @school_id END WHERE [usr_uuid] = @usr_uuid"
        DeleteCommand="UPDATE [dbo].[usrs] SET [is_active] = [is_active] ^ 1 WHERE [usr_uuid] = @usr_uuid"
        InsertCommand="INSERT INTO [dbo].[usrs] ([usr_uuid], [usr_login], [usr_display_name], [role_id], [last_login], [is_active], [usr_title], [school_id]) VALUES (NEWID(), @usr_login, @usr_display_name, @role_id, GETDATE(), 1, @usr_title, CASE @school_id WHEN 0 THEN NULL ELSE @school_id END)">
        <SelectParameters>
            <asp:ControlParameter ControlID="cmbTeachers" Name="usr_uuid" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="cmbSchools" Name="school_id" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="chkDeleted" Name="is_active" PropertyName="Checked" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="usr_login" Type="String" />
            <asp:Parameter Name="usr_display_name" Type="String" />
            <asp:Parameter Name="role_id" Type="Int32" />
            <asp:Parameter Name="usr_title" Type="String" />
            <asp:Parameter Name="school_id" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="usr_login" Type="String" />
            <asp:Parameter Name="usr_display_name" Type="String" />
            <asp:Parameter Name="role_id" Type="Int32" />
            <asp:Parameter Name="usr_title" Type="String" />
            <asp:Parameter Name="school_id" Type="String" />
        </UpdateParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT 0 AS o_school_id, '' AS school_name UNION SELECT * FROM schools">
    </asp:SqlDataSource>

    <asp:table ID="Table1" runat="server" CellPadding="2" CellSpacing="2" Width="1000px" BorderStyle="Solid" BorderWidth="1">
        <asp:TableRow>
            <asp:TableCell Width="5%"></asp:TableCell>
            <asp:TableCell Width="25%">Name</asp:TableCell>
            <asp:TableCell Width="25%"><asp:Label runat="server" ID="lblSchoolFilter">School</asp:Label></asp:TableCell>
            <asp:TableCell Width="45%"></asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell></asp:TableCell>
            <asp:TableCell>
                <telerik:RadComboBox runat="server" ID="cmbTeachers" OnItemsRequested="cmbTeachers_ItemsRequested" ShowToggleImage ="false" EnableLoadOnDemand="true" AutoPostBack="true" MarkFirstMatch="true" Height="100px" Width="200px">
                    <Items>
                        <telerik:RadComboBoxItem Value="00000000-0000-0000-0000-000000000000" Selected="true" />
                    </Items>
                </telerik:RadComboBox>
            </asp:TableCell>
            <asp:TableCell>
                <telerik:RadComboBox runat="server" ID="cmbSchools" AutoPostBack="true" Height="100px" DataSourceID="SqlDataSource2" DataTextField="school_name" DataValueField="o_school_id">
                </telerik:RadComboBox>
            </asp:TableCell>
            <asp:TableCell>
                <asp:CheckBox ID="chkDeleted" runat="server" AutoPostBack="true" Text="Show Deleted: " TextAlign="Left" Checked="false" Visible="true" />
            </asp:TableCell>
        </asp:TableRow>
    </asp:table>

<telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" AllowSorting="True" CellSpacing="0" GridLines="None" DataSourceID="SqlDataSource1" AllowAutomaticUpdates="true" AllowAutomaticDeletes="true" AllowAutomaticInserts="true" >
    <MasterTableView AutoGenerateColumns="False" DataKeyNames="usr_uuid" DataSourceID="SqlDataSource1" EditMode="InPlace" InsertItemDisplay="Top" PageSize="15" CommandItemDisplay="Top">
        <Columns>
            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn1">
                    <HeaderStyle Width="20px"></HeaderStyle>
                    <ItemStyle CssClass="MyImageButton"></ItemStyle>
                </telerik:GridEditCommandColumn>
                <telerik:GridButtonColumn ConfirmText="Delete user?" ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn1">
                    <HeaderStyle Width="20px"></HeaderStyle>
                    <ItemStyle CssClass="MyImageButton"></ItemStyle>
                </telerik:GridButtonColumn>
            <telerik:GridBoundColumn DataField="usr_uuid" DataType="System.Guid" FilterControlAltText="Filter usr_uuid column" HeaderText="usr_uuid" ReadOnly="True" Visible="false" SortExpression="usr_uuid" UniqueName="usr_uuid">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="usr_display_name" FilterControlAltText="Filter usr_display_name column" HeaderText="User Name" SortExpression="usr_display_name" UniqueName="usr_display_name" ItemStyle-Width="300px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="usr_login" FilterControlAltText="Filter usr_login column" HeaderText="Login Name" SortExpression="usr_login" UniqueName="usr_login" ItemStyle-Width="300px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridCheckBoxColumn DataField="is_active" DataType="System.Boolean" FilterControlAltText="Filter is_active column" HeaderText="is_active" Visible="false" SortExpression="is_active" UniqueName="is_active">
            </telerik:GridCheckBoxColumn>
            <telerik:GridBoundColumn DataField="usr_title" FilterControlAltText="Filter usr_title column" HeaderText="Title" SortExpression="usr_title" UniqueName="usr_title" ItemStyle-Width="25px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="role_id" DataType="System.Byte" FilterControlAltText="Filter role_id column" HeaderText="Role" SortExpression="role_id" UniqueName="role_id" ItemStyle-Width="25px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridDropDownColumn UniqueName="school_id" DataType="System.Int32" HeaderText="School" ColumnEditorID="GridDropDownListColumnEditor1" DataField="school_id" ListTextField="school_name" ListValueField="o_school_id" DataSourceID="SqlDataSource2" ItemStyle-Width="200px">
            </telerik:GridDropDownColumn>
            <telerik:GridBoundColumn DataField="last_login" DataType="System.DateTime" FilterControlAltText="Filter last_login column" HeaderText="Last Login" SortExpression="last_login" UniqueName="last_login" ReadOnly="true" ItemStyle-Width="150px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
        </Columns>
    </MasterTableView>
</telerik:RadGrid>

<telerik:GridDropDownListColumnEditor ID="GridDropDownListColumnEditor1" runat="server" DataValueField="school_id"  />