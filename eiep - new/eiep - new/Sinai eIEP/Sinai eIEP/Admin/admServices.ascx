<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="admServices.ascx.vb" Inherits="Sinai_eIEP.admServices" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        var oMaster
        var updateTimer

        function doInsert(sender, args) {
            //alert(args.get_commandArgument());
            document.getElementById("<%=hdnServicesParent.ClientID%>").value = args.get_commandArgument()
            oMaster.showInsertItem();
        }

        function doHide(sender, args) {
            oMaster = sender.get_masterTableView()
        }

        function doValidate(startDate) {
            if (document.getElementById(startDate).value == '') {
                alert('Please enter a service start date.');
            }
        }

    </script>
</telerik:RadCodeBlock>

<asp:HiddenField ID="hdnStudentUUID" runat="server" />
<asp:HiddenField ID="hdnServicesParent" runat="server" />

<asp:SqlDataSource ID="SqlDataSource1" OnUpdating="SqlDataSource1_Updating" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
    SelectCommand="SELECT [services_uuid], [student_uuid], [services_name] AS [services_name_group], [services_name], [provider_name], [service_model], [num_students], [session_length], [weekly_sessions], [session_start_date], [session_end_date], [services_parent] FROM [dbo].[student_services] WHERE [student_uuid] = @student_uuid ORDER BY [services_name], [services_parent], CONVERT(SMALLDATETIME, REPLACE([session_start_date], '/', '/1/'))"
    DeleteCommand="DELETE FROM [dbo].[student_services] WHERE [services_uuid] = @services_uuid"
    InsertCommand="INSERT INTO [dbo].[student_services] ([services_uuid], [student_uuid], [services_name], [provider_name], [service_model], [num_students], [session_length], [weekly_sessions], [session_start_date], [session_end_date], [services_parent]) VALUES(NEWID(), @student_uuid, @services_name, @provider_name, @service_model, @num_students, @session_length, @weekly_sessions, @session_start_date, @session_end_date, @services_parent)"
    UpdateCommand="UPDATE [dbo].[student_services] SET [services_name] = @services_name, [provider_name] = @provider_name, [service_model] = @service_model, [num_students] = @num_students, [session_length] = @session_length, [weekly_sessions] = @weekly_sessions, [session_start_date] = @session_start_date, [session_end_date] = @session_end_date WHERE [services_uuid] = @services_uuid"
    >
    <SelectParameters>
        <asp:ControlParameter ControlID="hdnStudentUUID" Name="student_uuid" PropertyName="Value" />
    </SelectParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hdnStudentUUID" Name="student_uuid" PropertyName="Value" />
        <asp:Parameter Name="services_name" Type="String" />
        <asp:Parameter Name="provider_name" Type="String" />
        <asp:Parameter Name="service_model" Type="String" />
        <asp:Parameter Name="num_students" Type="Int16" />
        <asp:Parameter Name="session_length" Type="String" />
        <asp:Parameter Name="weekly_sessions" Type="String" />
        <asp:Parameter Name="session_start_date" Type="String" />
        <asp:Parameter Name="session_end_date" Type="String" />
        <asp:ControlParameter ControlID="hdnServicesParent" Name="services_parent" PropertyName="Value" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="services_name" Type="String" />
        <asp:Parameter Name="provider_name" Type="String" />
        <asp:Parameter Name="service_model" Type="String" />
        <asp:Parameter Name="num_students" Type="Int16" />
        <asp:Parameter Name="session_length" Type="String" />
        <asp:Parameter Name="weekly_sessions" Type="String" />
        <asp:Parameter Name="session_start_date" Type="String" />
        <asp:Parameter Name="session_end_date" Type="String" />
    </UpdateParameters>
</asp:SqlDataSource>

<telerik:RadGrid ID="RadGrid1" runat="server" CellSpacing="0" DataSourceID="SqlDataSource1" GridLines="None" Width="995px" Height="250px" AllowAutomaticInserts="True" AllowAutomaticUpdates="True" AllowAutomaticDeletes="True" Enabled="false">
    <ClientSettings ClientEvents-OnRowCreated="doHide">
        <Scrolling AllowScroll="True" UseStaticHeaders="True" />
    </ClientSettings>
    <MasterTableView DataSourceID="SqlDataSource1" AutoGenerateColumns="False" DataKeyNames="services_uuid, services_name_group, services_parent" EditMode="InPlace" InsertItemDisplay="Top" CommandItemDisplay="Top">
        <GroupHeaderTemplate>
                <asp:Label runat="server" ID="Label1" Text='<%# Eval("services_name_group")%>' />
                <telerik:RadButton ID="RadButton1" runat="server" OnClientClicked="doInsert" AutoPostBack="false" CommandArgument='<%# Eval("services_parent")%>' Text="Modify Services" />
            </GroupHeaderTemplate>
            <GroupByExpressions>
                <telerik:GridGroupByExpression>
                    <SelectFields>
                        <telerik:GridGroupByField FieldAlias="services_name_group" FieldName="services_name_group" ></telerik:GridGroupByField>
                        <telerik:GridGroupByField FieldAlias="services_parent" FieldName="services_parent" ></telerik:GridGroupByField>
                    </SelectFields>
                    <GroupByFields>
                        <telerik:GridGroupByField FieldName="services_name_group" SortOrder="Ascending"></telerik:GridGroupByField>
                        <telerik:GridGroupByField FieldName="services_parent"></telerik:GridGroupByField>
                    </GroupByFields>
                </telerik:GridGroupByExpression>
            </GroupByExpressions>
        <Columns>
            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn1">
                <HeaderStyle Width="10px"></HeaderStyle>
                <ItemStyle CssClass="MyImageButton"></ItemStyle>
            </telerik:GridEditCommandColumn>
            <telerik:GridButtonColumn ConfirmText="Delete service?" ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn1">
                <HeaderStyle Width="10px"></HeaderStyle>
                <ItemStyle CssClass="MyImageButton"></ItemStyle>
            </telerik:GridButtonColumn>
            
            <telerik:GridBoundColumn DataField="services_uuid" DataType="System.Guid" FilterControlAltText="Filter services_uuid column" HeaderText="services_uuid" ReadOnly="True" SortExpression="services_uuid" UniqueName="services_uuid" Visible="false">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="student_uuid" DataType="System.Guid" FilterControlAltText="Filter student_uuid column" HeaderText="student_uuid" SortExpression="student_uuid" UniqueName="student_uuid" Visible="false">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="services_name" FilterControlAltText="Filter services_name column" HeaderText="Services" SortExpression="services_name" UniqueName="services_name" HeaderStyle-Width="125px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
<HeaderStyle Width="75px"></HeaderStyle>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="provider_name" FilterControlAltText="Filter provider_name column" HeaderText="Provider" SortExpression="provider_name" UniqueName="provider_name" HeaderStyle-Width="225px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
<HeaderStyle Width="75px"></HeaderStyle>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="service_model" FilterControlAltText="Filter service_model column" HeaderText="Model" SortExpression="service_model" UniqueName="service_model" HeaderStyle-Width="30px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridNumericColumn DataField="num_students" DataType="System.Byte" FilterControlAltText="Filter num_students column" HeaderText="# Students" SortExpression="num_students" UniqueName="num_students" HeaderStyle-Width="30px" NumericType="Number" MaxLength="3">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridNumericColumn>
            <telerik:GridBoundColumn DataField="session_length" FilterControlAltText="Filter session_length column" HeaderText="Length of Session" SortExpression="session_length" UniqueName="session_length" HeaderStyle-Width="45px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="weekly_sessions" FilterControlAltText="Filter weekly_sessions column" HeaderText="Weekly Sessions" SortExpression="weekly_sessions" UniqueName="weekly_sessions" HeaderStyle-Width="30px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridDropDownColumn DataField="session_start_date" ColumnEditorID="ddSessionStart" ListTextField="init_date" ListValueField="init_date" DataSourceID="ObjDataSource2" FilterControlAltText="Filter session_start_date column" HeaderText="Session Start" SortExpression="session_start_date" UniqueName="session_start_date" HeaderStyle-Width="25px" ItemStyle-Width="20px">
                <ColumnValidationSettings EnableRequiredFieldValidation="true">
                    
                </ColumnValidationSettings>
            </telerik:GridDropDownColumn>
            <telerik:GridDropDownColumn DataField="session_end_date" ColumnEditorID="ddSessionEnd" ListTextField="init_date" ListValueField="init_date" DataSourceID="ObjDataSource2" FilterControlAltText="Filter session_end_date column" HeaderText="Session End" SortExpression="session_end_date" UniqueName="session_end_date" HeaderStyle-Width="25px" ItemStyle-Width="20px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridDropDownColumn>
        </Columns>
    </MasterTableView>
</telerik:RadGrid>

<asp:ObjectDataSource ID="ObjDataSource2" runat="server" TypeName="DDLists" SelectMethod="getInitDates" ></asp:ObjectDataSource>

<telerik:GridDropDownListColumnEditor ID="ddSessionStart" runat="server" DropDownStyle-Width="60px"/>
<telerik:GridDropDownListColumnEditor ID="ddSessionEnd" runat="server" DropDownStyle-Width="60px"/>
             