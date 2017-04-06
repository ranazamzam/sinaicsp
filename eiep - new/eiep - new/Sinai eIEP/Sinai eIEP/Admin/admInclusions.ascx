<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="admInclusions.ascx.vb" Inherits="Sinai_eIEP.admInclusions" %>
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<asp:HiddenField ID="hdnStudentUUID" runat="server" />

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
    SelectCommand="SELECT [inclusion_uuid], [student_uuid], [subject_name], [num_classes], [teacher_name], [session_start_date], [session_end_date] FROM [dbo].[student_inclusions] WHERE [student_uuid] = @student_uuid ORDER BY [subject_name]"
    InsertCommand="INSERT INTO [dbo].[student_inclusions] ([inclusion_uuid], [student_uuid], [subject_name], [num_classes], [teacher_name], [session_start_date], [session_end_date]) VALUES(NEWID(), @student_uuid, @subject_name, @num_classes, @teacher_name, @session_start_date, @session_end_date)"
    DeleteCommand="DELETE FROM [dbo].[student_inclusions] WHERE [inclusion_uuid] = @inclusion_uuid"
    UpdateCommand="UPDATE [dbo].[student_inclusions] SET [subject_name] = @subject_name, [num_classes] = @num_classes, [teacher_name] = @teacher_name, [session_start_date] = @session_start_date, [session_end_date] = @session_end_date WHERE [inclusion_uuid] = @inclusion_uuid">
    <SelectParameters>
        <asp:ControlParameter ControlID="hdnStudentUUID" Name="student_uuid" PropertyName="Value" />
    </SelectParameters>
    <InsertParameters>
        <asp:ControlParameter ControlID="hdnStudentUUID" Name="student_uuid" PropertyName="Value" />
        <asp:Parameter Name="subject_name" Type="String" />
        <asp:Parameter Name="num_classes" Type="String" />
        <asp:Parameter Name="teacher_name" Type="String" />
        <asp:Parameter Name="session_start_date" Type="String" />
        <asp:Parameter Name="session_end_date" Type="String" />
    </InsertParameters>
    <UpdateParameters>
        <asp:Parameter Name="subject_name" Type="String" />
        <asp:Parameter Name="num_classes" Type="String" />
        <asp:Parameter Name="teacher_name" Type="String" />
        <asp:Parameter Name="session_start_date" Type="String" />
        <asp:Parameter Name="session_end_date" Type="String" />
    </UpdateParameters>
</asp:SqlDataSource>

<telerik:RadGrid ID="RadGrid1" runat="server" CellSpacing="0" DataSourceID="SqlDataSource1" GridLines="None" Width="995px" Height="250px" AllowAutomaticInserts="true" AllowAutomaticUpdates="true" AllowAutomaticDeletes="true" >
    <ClientSettings>
        <Scrolling AllowScroll="True" UseStaticHeaders="True" />
    </ClientSettings>
    <MasterTableView DataSourceID="SqlDataSource1" AutoGenerateColumns="False" DataKeyNames="inclusion_uuid" EditMode="InPlace" InsertItemDisplay="Top" CommandItemDisplay="Top">
        <Columns>
            <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn1">
                <HeaderStyle Width="10px"></HeaderStyle>
                <ItemStyle CssClass="MyImageButton"></ItemStyle>
            </telerik:GridEditCommandColumn>
            <telerik:GridButtonColumn ConfirmText="Delete inclusion?" ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn1">
                <HeaderStyle Width="10px"></HeaderStyle>
                <ItemStyle CssClass="MyImageButton"></ItemStyle>
            </telerik:GridButtonColumn>
            <telerik:GridBoundColumn DataField="inclusion_uuid" DataType="System.Guid" FilterControlAltText="Filter inclusion_uuid column" HeaderText="inclusion_uuid" ReadOnly="True" SortExpression="inclusion_uuid" UniqueName="inclusion_uuid" Visible="false">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="student_uuid" DataType="System.Guid" FilterControlAltText="Filter student_uuid column" HeaderText="student_uuid" SortExpression="student_uuid" UniqueName="student_uuid" Visible="false">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="subject_name" FilterControlAltText="Filter subject_name column" HeaderText="Subject" SortExpression="subject_name" UniqueName="subject_name" HeaderStyle-Width="75px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="num_classes" FilterControlAltText="Filter num_classes column" HeaderText="Classes" SortExpression="num_classes" UniqueName="num_classes" HeaderStyle-Width="25px" ItemStyle-Width="25px" >
                <ItemStyle Width="10px" />
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridBoundColumn DataField="teacher_name" FilterControlAltText="Filter teacher_name column" HeaderText="Teacher" SortExpression="teacher_name" UniqueName="teacher_name" HeaderStyle-Width="75px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
                </ColumnValidationSettings>
            </telerik:GridBoundColumn>
            <telerik:GridDropDownColumn DataField="session_start_date" ColumnEditorID="ddSessionStart" ListTextField="init_date" ListValueField="init_date" DataSourceID="ObjDataSource2" FilterControlAltText="Filter session_start_date column" HeaderText="Session Start" SortExpression="session_start_date" UniqueName="session_start_date" HeaderStyle-Width="25px" ItemStyle-Width="20px">
                <ColumnValidationSettings>
                    <ModelErrorMessage Text="" />
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
             