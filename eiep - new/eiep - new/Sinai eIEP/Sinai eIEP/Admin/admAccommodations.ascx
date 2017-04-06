<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="admAccommodations.ascx.vb" Inherits="Sinai_eIEP.admAccommodations" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:HiddenField ID="hdnStudentUUID" runat="server" />

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
    SelectCommand="SELECT [accommodations_uuid], [student_uuid], [accommodation_details] FROM [dbo].[student_accommodations] WHERE [student_uuid] = @student_uuid ORDER BY [accommodation_details]"
    InsertCommand="INSERT INTO [dbo].[student_accommodations] ([accommodations_uuid], [student_uuid], [accommodation_details]) VALUES(NEWID(), @student_uuid, @accommodation_details)"
    DeleteCommand="DELETE FROM [dbo].[student_accommodations] WHERE [accommodations_uuid] = @accommodations_uuid"
    UpdateCommand="UPDATE [dbo].[student_accommodations] SET [accommodation_details] = @accommodation_details WHERE [accommodations_uuid] = @accommodations_uuid">
    <SelectParameters>
        <asp:ControlParameter ControlID="hdnStudentUUID" Name="student_uuid" PropertyName="Value" />
    </SelectParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hdnStudentUUID" Name="student_uuid" PropertyName="Value" />
        <asp:Parameter Name="accommodation_details" Type="String" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="accommodation_details" Type="String" />
    </UpdateParameters>
</asp:SqlDataSource>

<telerik:RadGrid ID="RadGrid1" runat="server" CellSpacing="0" DataSourceID="SqlDataSource1" GridLines="None" Width="995" Height="250px" AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AllowAutomaticDeletes="true" >
    <ClientSettings>
        <Scrolling AllowScroll="True" UseStaticHeaders="True" />
    </ClientSettings>
    <MasterTableView DataSourceID="SqlDataSource1" AutoGenerateColumns="False" DataKeyNames="accommodations_uuid" EditMode="InPlace" InsertItemDisplay="Top" CommandItemDisplay="Top">
        <Columns>
            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn1">
                <HeaderStyle Width="25px"></HeaderStyle>
                <ItemStyle CssClass="MyImageButton"></ItemStyle>
            </telerik:GridEditCommandColumn>
            <telerik:GridButtonColumn ConfirmText="Delete accommodation?" ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn1">
               <HeaderStyle Width="25px"></HeaderStyle>
                <ItemStyle CssClass="MyImageButton"></ItemStyle>
            </telerik:GridButtonColumn>
            <telerik:GridBoundColumn DataField="accommodations_uuid" DataType="System.Guid" FilterControlAltText="Filter accommodations_uuid column" HeaderText="accommodations_uuid" ReadOnly="True" SortExpression="accommodations_uuid" UniqueName="accommodations_uuid" Visible="false">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="student_uuid" DataType="System.Guid" FilterControlAltText="Filter student_uuid column" HeaderText="student_uuid" SortExpression="student_uuid" UniqueName="student_uuid" Visible="false">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="accommodation_details" FilterControlAltText="Filter accommodation_details column" HeaderText="Accommodation" SortExpression="accommodation_details" UniqueName="accommodation_details">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
        </Columns>
    </MasterTableView>
</telerik:RadGrid>             