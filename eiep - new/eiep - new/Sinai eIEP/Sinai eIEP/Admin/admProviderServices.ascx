<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="admProviderServices.ascx.vb" Inherits="Sinai_eIEP.admProviderServices" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<script type="text/javascript">

    function doInsert(sender, args) {
        document.getElementById("<%=hdnServicesParent.ClientID%>").value = args.get_commandArgument()
        $find("AdminProviderServices_rgServices").get_masterTableView().showInsertItem();
    }

    function addStudent(sender, args) {
        var cmbStudents = $find(sender.get_id().replace("btnAddStudent", "cmbStudents"));
        var oStudentList = $find(sender.get_id().replace("btnAddStudent", "lvStudents"));

        if (!oStudentList.findItemByValue(cmbStudents.get_value()) && cmbStudents.get_text() != '') {
            var item = new Telerik.Web.UI.RadListBoxItem();
            item.set_text(cmbStudents.get_text());
            item.set_value(cmbStudents.get_value());
            oStudentList.trackChanges();
            oStudentList.get_items().add(item);
            item.select();
            oStudentList.commitChanges();
        }
    }
     
    function doValidate(startDate) {
        if (document.getElementById(startDate).value == '') {
            alert('Please enter a service start date.');
        }
    }

</script>

<asp:SqlDataSource ID="sqlProviders" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT * FROM service_providers UNION SELECT '00000000-0000-0000-0000-000000000000', '' ORDER BY provider_name">
</asp:SqlDataSource>
<asp:SqlDataSource ID="sqlServices" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT services.[services_uuid], services.[services_parent], [provider_uuid], [services_name], [service_model], [num_students], [session_length], [weekly_sessions], [session_start_date], [session_end_date], [provider_name], [services_name] AS [services_name_group], (SELECT LEFT(student_list, LEN(student_list) - 1) FROM (
SELECT student_first_name + N' ' + student_last_name + N', ' FROM services_students INNER JOIN students ON students.student_uuid = services_students.student_uuid WHERE services_uuid = services.services_uuid FOR XML PATH('')
) C (student_list)) AS student_list FROM services INNER JOIN service_providers ON services.[provider_uuid] = service_providers.[providers_uuid] 
    INNER JOIN (SELECT DISTINCT services_students.services_uuid FROM services_students INNER JOIN students ON services_students.student_uuid = students.student_uuid WHERE school_year = @school_year AND (school_id = @school_id OR @school_id = 0) AND (students.student_uuid = @student_uuid OR @student_uuid = '00000000-0000-0000-0000-000000000000')) AS sy ON sy.services_uuid = services.services_uuid
    WHERE service_providers.[providers_uuid] = @provider_uuid ORDER BY [services_name], [services_parent], CONVERT(SMALLDATETIME, REPLACE([session_start_date], '/', '/1/'))"
        DeleteCommand="DELETE FROM services WHERE services_uuid = @services_uuid">
        <SelectParameters>
            <asp:ControlParameter ControlID="cmbProviders" PropertyName="SelectedValue" Name="provider_uuid" />
            <asp:ControlParameter ControlID="cmbYear" PropertyName="SelectedValue" Name="school_year" />
            <asp:ControlParameter ControlID="cmbSchools" PropertyName="SelectedValue" Name="school_id" />
            <asp:ControlParameter ControlID="cmbStudentFilter" PropertyName="SelectedValue" Name="student_uuid" />
        </SelectParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="sqlSchools" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
            SelectCommand="SELECT school_id, school_name FROM schools UNION SELECT 0, '' ORDER BY 2">
            
        </asp:SqlDataSource>

<asp:HiddenField ID="hdnServicesParent" runat="server" />

<asp:Panel runat="server" ID="pnlServices">
    <asp:table ID="Table1" runat="server" CellPadding="2" CellSpacing="2" Width="1000px" BorderStyle="Solid" BorderWidth="1">
        <asp:TableRow>
            <asp:TableCell Width="2%"></asp:TableCell>
            <asp:TableCell Width="98%" ColumnSpan="3">
                Provider: <telerik:RadComboBox ID="cmbProviders" runat="server" DataSourceID="sqlProviders" OnSelectedIndexChanged="cmbProviders_SelectedIndexChanged" DataTextField="provider_name" DataValueField="providers_uuid" DropDownAutoWidth="Enabled" AutoPostBack="true" />
                &nbsp;<telerik:RadTextBox runat="server" ID="txtNewProvider" />
                &nbsp;<telerik:RadButton runat="server" ID="btnNewProvider" Text="Add" OnClick="btnNewProvider_Click" />
            </asp:TableCell>
        </asp:TableRow>
        <asp:TableRow>
            <asp:TableCell Width="2%"></asp:TableCell>
            <asp:TableCell Width="32%">
                School Year: 
                    <telerik:RadComboBox runat="server" ID="cmbYear" AutoPostBack="true" Height="100px" >
                        <Items>
                            <telerik:RadComboBoxItem Value="2016 - 2017" Text="2016 - 2017" Selected="true" />
                            <telerik:RadComboBoxItem Value="2015 - 2016" Text="2015 - 2016" />
                            <telerik:RadComboBoxItem Value="2014 - 2015" Text="2014 - 2015" />
                            <telerik:RadComboBoxItem Value="2013 - 2014" Text="2013 - 2014" />
                        </Items>
                    </telerik:RadComboBox>
            </asp:TableCell>
            <asp:TableCell Width="32%">
                School:
                <telerik:RadComboBox runat="server" ID="cmbSchools" AutoPostBack="true" Height="100px" DataSourceID="sqlSchools" DataTextField="school_name" DataValueField="school_id" >
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Selected="true" />
                        </Items>
                    </telerik:RadComboBox>
             </asp:TableCell>
            <asp:TableCell Width="33%">
                Student:
                <telerik:RadComboBox ID="cmbStudentFilter" runat="server" OnItemsRequested="cmbStudentFilter_ItemsRequested" ShowToggleImage ="false" EnableLoadOnDemand="true" AutoPostBack="true" MarkFirstMatch="true" Height="100px" Width="195px">
                    <Items>
                            <telerik:RadComboBoxItem Value="00000000-0000-0000-0000-000000000000" Selected="true" />
                        </Items>
                </telerik:RadComboBox>
                </asp:TableCell>
        </asp:TableRow>
    </asp:table>

    <telerik:RadGrid ID="rgServices" runat="server" CellSpacing="0" Enabled="true" DataSourceID="sqlServices" OnItemDataBound="rgServices_ItemDataBound" OnInsertCommand="rgServices_InsertCommand" OnUpdateCommand="rgServices_UpdateCommand" AllowAutomaticDeletes="True" MasterTableView-EditMode="EditForms">
        <MasterTableView AutoGenerateColumns="False" CommandItemDisplay="Top" DataKeyNames="services_uuid, services_parent, services_name" >
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
                <telerik:GridBoundColumn DataField="services_name" FilterControlAltText="Filter services_name column" HeaderText="Services" SortExpression="services_name" UniqueName="services_name" ItemStyle-Width="350px" Visible="false">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                    <HeaderStyle Width="75px"></HeaderStyle>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="student_list" FilterControlAltText="Filter student_list column" HeaderText="Students" SortExpression="student_list" UniqueName="student_list" ItemStyle-Width="350px">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                    <HeaderStyle Width="75px"></HeaderStyle>
                </telerik:GridBoundColumn>
                <telerik:GridBoundColumn DataField="provider_name" FilterControlAltText="Filter provider_name column" HeaderText="Provider" SortExpression="provider_name" UniqueName="provider_name" HeaderStyle-Width="225px" Visible="false">
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
                <telerik:GridDropDownColumn DataField="session_start_date" ColumnEditorID="ddSessionStart" ListTextField="init_date" ListValueField="init_date" DataSourceID="odsDateList" FilterControlAltText="Filter session_start_date column" HeaderText="Session Start" SortExpression="session_start_date" UniqueName="session_start_date" HeaderStyle-Width="25px" ItemStyle-Width="20px">
                    <ColumnValidationSettings EnableRequiredFieldValidation="true">
                    
                    </ColumnValidationSettings>
                </telerik:GridDropDownColumn>
                <telerik:GridDropDownColumn DataField="session_end_date" ColumnEditorID="ddSessionEnd" ListTextField="init_date" ListValueField="init_date" DataSourceID="odsDateList" FilterControlAltText="Filter session_end_date column" HeaderText="Session End" SortExpression="session_end_date" UniqueName="session_end_date" HeaderStyle-Width="25px" ItemStyle-Width="20px">
                    <ColumnValidationSettings>
                        <ModelErrorMessage Text="" />
                    </ColumnValidationSettings>
                </telerik:GridDropDownColumn>
            </Columns>
            <EditFormSettings EditFormType="Template">
                <FormTemplate>

                    <asp:SqlDataSource ID="sqlStudentList" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
                            SelectCommand="SELECT students.student_uuid, student_first_name + N' ' + student_last_name AS student_name FROM services INNER JOIN services_students ON services.services_uuid = services_students.services_uuid INNER JOIN students ON students.student_uuid = services_students.student_uuid WHERE services.services_uuid = @services_uuid">
                            <SelectParameters>
                                <asp:ControlParameter ControlID="hdnServicesUuid" PropertyName="Value" Name="services_uuid" />
                            </SelectParameters>
                    </asp:SqlDataSource>

                    <asp:TextBox ID="txtService" runat="server" Text='<%# Bind("services_name")%>' Width="525px"></asp:TextBox>
                    <asp:DropDownList ID="ddlModel" runat="server" SelectedValue='<%# Bind("service_model")%>' DataSource='<%# (New String() {"Pullout", "Push-in"})%>' AppendDataBoundItems="True">
                                    <asp:ListItem Selected="True" Text="" Value=""></asp:ListItem>
                                </asp:DropDownList>
                    <telerik:RadNumericTextBox ID="txtNumStudents" runat="server" MaxValue="25" MinValue="1" DbValue='<%# Bind("num_students")%>' DataType="System.Byte" NumberFormat-DecimalDigits="0" Width="70px"></telerik:RadNumericTextBox>
                    <asp:TextBox ID="txtLength" runat="server" Text='<%# Bind("session_length")%>' Width="70px"></asp:TextBox>
                    <asp:TextBox ID="txtNumSessions" runat="server" Text='<%# Bind("weekly_sessions")%>' Width="70px"></asp:TextBox>
                    <asp:DropDownList ID="ddlStart" runat="server" SelectedValue='<%# Bind("session_start_date")%>' DataSourceID="odsDateList" DataTextField="init_date" DataValueField="init_date" Width="60px"></asp:DropDownList>
                    <asp:DropDownList ID="ddlEnd" runat="server" SelectedValue='<%# Bind("session_end_date")%>' DataSourceID="odsDateList" DataTextField="init_date" DataValueField="init_date" Width="60px"></asp:DropDownList>
                    <asp:HiddenField ID="hdnServicesUuid" runat="server" Value='<%# Bind("services_uuid")%>' />
                    <asp:HiddenField ID="hdnServicesParentUuid" runat="server" Value='<%# Bind("services_parent")%>' />

                    <br />
                    <h3>Students</h3>
                    <telerik:RadComboBox ID="cmbStudents" runat="server" OnItemsRequested="cmbStudents_ItemsRequested" ShowToggleImage ="false" EnableLoadOnDemand="true" AutoPostBack="true" MarkFirstMatch="true" Height="100px" Width="195px"></telerik:RadComboBox>
                    <telerik:RadButton runat="server" ID="btnAddStudent" Text="Add Student"  AutoPostBack="false" OnClientClicked="addStudent">
                        <Icon PrimaryIconCssClass="rbAdd" PrimaryIconLeft="4" PrimaryIconTop="4"></Icon>
                    </telerik:RadButton>
                            
                    <br />
                    <telerik:RadListBox ID="lvStudents" runat="server" DataSourceID="sqlStudentList" DataTextField="student_name" DataValueField="student_uuid" AllowDelete="true" ButtonSettings-ShowDelete="true" Width="225px" Height="150px" >
          
                    </telerik:RadListBox>
                    <br /><br />
                    <asp:Button ID="btnUpdate" Text='<%# IIf((TypeOf(Container) is GridEditFormInsertItem), "Insert", "Update") %>' runat="server" CommandName='<%# IIf((TypeOf(Container) is GridEditFormInsertItem), "PerformInsert", "Update")%>'></asp:Button>&nbsp;
                    <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CommandName="Cancel"></asp:Button>
                    <br />
                </FormTemplate>
            </EditFormSettings>
        </MasterTableView>
    </telerik:RadGrid>

<asp:ObjectDataSource ID="odsDateList" runat="server" TypeName="DDLists" SelectMethod="getInitDates" ></asp:ObjectDataSource>

<telerik:GridDropDownListColumnEditor ID="ddSessionStart" runat="server" DropDownStyle-Width="60px"/>
<telerik:GridDropDownListColumnEditor ID="ddSessionEnd" runat="server" DropDownStyle-Width="60px"/>

</asp:Panel>