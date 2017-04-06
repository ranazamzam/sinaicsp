<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="admLocks.ascx.vb" Inherits="Sinai_eIEP.admLocks" %>
<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT vw_list_iep.iep_uuid, display_name, subject_text, locked_by, locked_by_uuid FROM vw_list_iep INNER JOIN (SELECT iep_uuid, locked_by, locked_by_uuid FROM iep WHERE is_active = 1 AND dbo.isLocked(iep_uuid, locked_by) = 1) lock ON lock.iep_uuid = vw_list_iep.iep_uuid"
        DeleteCommand="UPDATE iep SET locked_by = NULL, locked_by_uuid = NULL, locked_by_date = NULL WHERE iep_uuid = @iep_uuid">
    </asp:SqlDataSource>

    <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" CellSpacing="0" GridLines="None" DataSourceID="SqlDataSource1" AllowAutomaticDeletes="True">
        <MasterTableView AutoGenerateColumns="False" DataKeyNames="iep_uuid" DataSourceID="SqlDataSource1" PageSize="20" CommandItemDisplay="Top">
        <Columns>
            <telerik:GridButtonColumn ConfirmText="Unlock CSP?" ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn1" ItemStyle-Width="25px">
                    <HeaderStyle Width="20px"></HeaderStyle>
                    <ItemStyle CssClass="MyImageButton"></ItemStyle>
                </telerik:GridButtonColumn>
            <telerik:GridBoundColumn DataField="iep_uuid" DataType="System.Guid" HeaderText="iep_uuid" SortExpression="iep_uuid" UniqueName="iep_uuid" Visible="false">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="display_name" HeaderText="Student Name" SortExpression="display_name" UniqueName="display_name" ItemStyle-Width="300px" ReadOnly="True">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="subject_text" HeaderText="Subject" UniqueName="subject_text" ItemStyle-Width="375px" ReadOnly="True" SortExpression="subject_text">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="locked_by" HeaderText="Locked By" UniqueName="locked_by" ItemStyle-Width="300px" SortExpression="locked_by">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="locked_by_uuid" DataType="System.Guid" FilterControlAltText="Filter locked_by_uuid column" HeaderText="locked_by_uuid" SortExpression="locked_by_uuid" UniqueName="locked_by_uuid"  Visible="false">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
        </Columns>
    </MasterTableView>
</telerik:RadGrid>
