<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="admStudentInfo.ascx.vb" Inherits="Sinai_eIEP.admStudentInfo" %>

<%@ Register assembly="Telerik.Web.UI" namespace="Telerik.Web.UI" tagprefix="telerik" %>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT [student_uuid], [student_first_name], [student_last_name], [current_placement], [is_active], [students].[school_id], [school_name], [date_of_birth], [student_sex], [student_classification], [native_language], [student_grade], [parent_primary], [parent_secondary], [home_address], [home_city], [home_state], [home_phone], [father_cell], [mother_cell] FROM students LEFT OUTER JOIN schools ON students.school_id = schools.school_id WHERE students.is_active <> @is_active AND (students.school_id = @school_id OR @school_id = 0) AND (student_uuid = @student_uuid OR @student_uuid = '00000000-0000-0000-0000-000000000000')"
        UpdateCommand="UPDATE [dbo].[students] SET [student_first_name] = @student_first_name, [student_last_name] = @student_last_name, [current_placement] = @current_placement, [school_id] = @school_id, [date_of_birth] = @date_of_birth, [student_sex] = @student_sex, [student_classification] = @student_classification, [native_language] = @native_language, [student_grade] = @student_grade, [parent_primary] = @parent_primary, [parent_secondary] = @parent_secondary, [home_address] = @home_address, [home_city] = @home_city, [home_state] = @home_state, [home_phone] = @home_phone, [father_cell] = @father_cell, [mother_cell] = @mother_cell WHERE [student_uuid] = @student_uuid"
        DeleteCommand="UPDATE [dbo].[students] SET [is_active] = [is_active] ^ 1 WHERE [student_uuid] = @student_uuid"
        InsertCommand="INSERT INTO [dbo].[students] ([student_uuid], [student_first_name], [student_last_name], [is_active], [current_placement], [school_id]) VALUES (NEWID(), @student_first_name, @student_last_name, 1, @current_placement, @school_id)">
        <SelectParameters>
            <asp:ControlParameter ControlID="cmbStudents" Name="student_uuid" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="cmbSchools" Name="school_id" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="chkDeleted" Name="is_active" PropertyName="Checked" />
        </SelectParameters>
        <InsertParameters>
            <asp:Parameter Name="student_first_name" Type="String" />
            <asp:Parameter Name="student_last_name" Type="String" />
            <asp:Parameter Name="current_placement" Type="String" />
            <asp:Parameter Name="school_id" Type="Int32" />
        </InsertParameters>
        <UpdateParameters>
            <asp:Parameter Name="student_first_name" Type="String" />
            <asp:Parameter Name="student_last_name" Type="String" />
            <asp:Parameter Name="current_placement" Type="String" />
            <asp:Parameter Name="school_id" Type="Int32" />
            <asp:Parameter Name="date_of_birth" Type="DateTime" />
            <asp:Parameter Name="student_sex" Type="String" />
            <asp:Parameter Name="student_classification" Type="String" />
            <asp:Parameter Name="native_language" Type="String" />
            <asp:Parameter Name="student_grade" Type="Int32" />
            <asp:Parameter Name="parent_primary" Type="String" />
            <asp:Parameter Name="parent_secondary" Type="String" />
            <asp:Parameter Name="home_address" Type="String" />
            <asp:Parameter Name="home_city" Type="String" />
            <asp:Parameter Name="home_state" Type="String" />
            <asp:Parameter Name="home_phone" Type="String" />
            <asp:Parameter Name="father_cell" Type="String" />
            <asp:Parameter Name="mother_cell" Type="String" />
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
                <telerik:RadComboBox OnItemsRequested="cmbStudents_ItemsRequested" runat="server" ID="cmbStudents" ShowToggleImage ="false" EnableLoadOnDemand="true" AutoPostBack="true" MarkFirstMatch="true" Height="100px" Width="200px">
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
    <MasterTableView AutoGenerateColumns="False" DataKeyNames="student_uuid" DataSourceID="SqlDataSource1" EditMode="EditForms" InsertItemDisplay="Top" PageSize="15" CommandItemDisplay="Top">
        <Columns>
            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn1">
                    <HeaderStyle Width="20px"></HeaderStyle>
                    <ItemStyle CssClass="MyImageButton"></ItemStyle>
                </telerik:GridEditCommandColumn>
                <telerik:GridButtonColumn ConfirmText="Delete student?" ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn1">
                    <HeaderStyle Width="20px"></HeaderStyle>
                    <ItemStyle CssClass="MyImageButton"></ItemStyle>
                </telerik:GridButtonColumn>
            <telerik:GridBoundColumn DataField="student_uuid" DataType="System.Guid" FilterControlAltText="Filter student_uuid column" HeaderText="student_uuid" ReadOnly="True" Visible="false" SortExpression="student_uuid" UniqueName="student_uuid">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="student_first_name" FilterControlAltText="Filter student_first_name column" HeaderText="First Name" SortExpression="student_first_name" UniqueName="student_first_name" ItemStyle-Width="300px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="student_last_name" FilterControlAltText="Filter student_last_name column" HeaderText="Last Name" SortExpression="student_last_name" UniqueName="student_last_name" ItemStyle-Width="300px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="current_placement" FilterControlAltText="Filter current_placement column" HeaderText="Class" SortExpression="current_placement" UniqueName="current_placement" ItemStyle-Width="200px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridCheckBoxColumn DataField="is_active" DataType="System.Boolean" FilterControlAltText="Filter is_active column" HeaderText="is_active" Visible="false" SortExpression="is_active" UniqueName="is_active">
            </telerik:GridCheckBoxColumn>
            <telerik:GridDropDownColumn UniqueName="school_id" DataType="System.Int32" HeaderText="School" ColumnEditorID="GridDropDownListColumnEditor1" DataField="school_id" ListTextField="school_name" ListValueField="o_school_id" DataSourceID="SqlDataSource2" ItemStyle-Width="200px">
            </telerik:GridDropDownColumn>
        </Columns>
        <EditFormSettings EditFormType="Template">
            <FormTemplate>
                <asp:Table ID="Table1" runat="server" Width="750px">
                    <asp:TableRow>
                        <asp:TableCell>First Name: <asp:TextBox ID="TextBox7" runat="server" Text='<%# Bind("student_first_name")%>'></asp:TextBox></asp:TableCell>
                        <asp:TableCell>School: 
                            <asp:DropDownList ID="ddlTOC" runat="server" SelectedValue='<%# Bind("school_id")%>' DataSourceID="SqlDataSource2" DataValueField="o_school_id" DataTextField="school_name" TabIndex="7" AppendDataBoundItems="True">
                                <asp:ListItem Selected="True" Text="Select" Value=""></asp:ListItem>
                            </asp:DropDownList>
                        </asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Last Name: <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("student_last_name")%>'></asp:TextBox></asp:TableCell>
                        <asp:TableCell>Class: <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("current_placement")%>'></asp:TextBox></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>DOB: <telerik:RadDatePicker ID="RadDatePicker1" runat="server" DbSelectedDate='<%# Bind("date_of_birth") %>'></telerik:RadDatePicker></asp:TableCell>
                        <asp:TableCell>Grade: <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("student_grade")%>'></asp:TextBox></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Primary Parent: <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("parent_primary")%>'></asp:TextBox></asp:TableCell>
                        <asp:TableCell>Secondary Parent: <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("parent_secondary")%>'></asp:TextBox></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Address: <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("home_address")%>'></asp:TextBox></asp:TableCell>
                        <asp:TableCell>City: <asp:TextBox ID="TextBox8" runat="server" Text='<%# Bind("home_city")%>'></asp:TextBox></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>State: <asp:TextBox ID="TextBox9" runat="server" Text='<%# Bind("home_state")%>'></asp:TextBox></asp:TableCell>
                        <asp:TableCell>Phone: <asp:TextBox ID="TextBox10" runat="server" Text='<%# Bind("home_phone")%>'></asp:TextBox></asp:TableCell>
                    </asp:TableRow>
                    <asp:TableRow>
                        <asp:TableCell>Mother Cell: <asp:TextBox ID="TextBox11" runat="server" Text='<%# Bind("mother_cell")%>'></asp:TextBox></asp:TableCell>
                        <asp:TableCell>Father Cell: <asp:TextBox ID="TextBox12" runat="server" Text='<%# Bind("father_cell")%>'></asp:TextBox></asp:TableCell>
                    </asp:TableRow>
                </asp:Table>
                <asp:Button ID="btnUpdate" Text='<%# IIf((TypeOf(Container) is GridEditFormInsertItem), "Insert", "Update") %>' runat="server" CommandName='<%# IIf((TypeOf(Container) is GridEditFormInsertItem), "PerformInsert", "Update")%>'></asp:Button>&nbsp;
                <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False" CommandName="Cancel"></asp:Button>
            </FormTemplate>
        </EditFormSettings>
    </MasterTableView>
</telerik:RadGrid>

<telerik:GridDropDownListColumnEditor ID="GridDropDownListColumnEditor1" runat="server" DataValueField="school_id"  />