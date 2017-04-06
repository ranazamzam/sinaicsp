<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="eiep_header.ascx.vb" Inherits="Sinai_eIEP.eiep_header" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="getTeacherList @usr_login, '%', @school_id">
        <SelectParameters>
            <asp:ControlParameter ControlID="usr_login" Name="usr_login" PropertyName="Value" />
            <asp:ControlParameter ControlID="cmbSchool" Name="school_id" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="getStudentList_New @usr_login, '%', @school_id, NULL">
        <SelectParameters>
            <asp:ControlParameter ControlID="usr_login" Name="usr_login" PropertyName="Value" />
            <asp:ControlParameter ControlID="cmbSchool" Name="school_id" PropertyName="SelectedValue" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="getSchoolList @usr_login, 0">
        <SelectParameters>
            <asp:ControlParameter ControlID="usr_login" Name="usr_login" PropertyName="Value" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT * FROM subjects ORDER BY subject_order">
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT * FROM usrs">
    </asp:SqlDataSource>
    
    <asp:HiddenField ID="usr_login" runat="server" />

<table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="1" rules="none"
    style="border-collapse: collapse">
    <tr class="EditFormHeader">
        <td colspan="2">
            <b>IEP Details</b>
        </td>
    </tr>
    <tr>
        <td colspan="2">
        </td>
    </tr>
    <tr>
        <td>
            <table id="Table3" cellspacing="1" cellpadding="1" width="500" border="0">
                <tr>
                    <td>
                        <strong>School:</strong>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="cmbSchool" AutoPostBack="true" runat="server" Width="275" DataSourceID="SqlDataSource3" DataTextField="school_name" DataValueField="school_id" Enabled="<%# (TypeOf DataItem Is Telerik.Web.UI.GridInsertionObject)%>"></telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Student:</strong>
                    </td>
                    <td>
                        <telerik:RadDropDownTree ID="ddStudent" runat="server" Width="275" DropDownSettings-Height="500" DataSourceID="SqlDataSource1" DataFieldParentID="placement_uuid" DataFieldID="student_uuid" DataTextField="student_name" Enabled="<%# cmbSchool.Enabled%>" DataValueField="student_uuid" OnClientEntryAdded="selectStudent"></telerik:RadDropDownTree>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Teacher:</strong>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="cmbAllTeachers" runat="server" Width="275"  DataSourceID="SqlDataSource4" DataTextField="usr_display_name" DataValueField="usr_uuid" Visible="false"></telerik:RadComboBox>
                        <telerik:RadComboBox ID="cmbTeacher" runat="server" Width="275"  DataSourceID="SqlDataSource2" DataTextField="teacher_name" DataValueField="usr_uuid"></telerik:RadComboBox>
                        <telerik:RadButton runat="server" ID="btnAddTeacher" Text="Add" AutoPostBack="false" OnClientClicked="addTeacher">
                                   <Icon PrimaryIconCssClass="rbAdd" PrimaryIconLeft="4" PrimaryIconTop="4"></Icon>
                        </telerik:RadButton>
                        <br />
                        <telerik:RadListBox ID="lbTeachers" runat="server" Height="100px" Width="275px" OnClientItemDoubleClicking="onDblClick"></telerik:RadListBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Subject:</strong>
                    </td>
                    <td>
                        <telerik:RadDropDownTree ID="ddSubjects" runat="server" Width="275" TextMode="FullPath" DropDownSettings-Height="375" DataSourceID="SqlDataSource5" DataFieldParentID="subject_category" DataFieldID="subject_uuid" DataTextField="subject_name" Enabled="true" DataValueField="subject_uuid" OnClientEntryAdded="selectStudent"></telerik:RadDropDownTree>
                        <asp:TextBox ID="txtSubject" runat="server" Width="275px" Text='<%# DataBinder.Eval(Container, "DataItem.iep_subject")%>' TabIndex="2" style="display:none"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Materials:</strong>
                    </td>
                    <td>
                       <asp:TextBox ID="txtMaterials" runat="server" Width="275px" Text='<%# DataBinder.Eval(Container, "DataItem.iep_materials")%>' TabIndex="2" ></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Year:</strong>
                    </td>
                    <td>
                       <asp:TextBox ID="txtYear" runat="server" Width="275px" TabIndex="2" Enabled="false"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
        </td>
    </tr>
    <tr>
        <td align="right" colspan="2">
            <asp:Button ID="btnUpdate" Text="Update" runat="server" CommandName="Update" Visible='<%# Not (TypeOf DataItem Is Telerik.Web.UI.GridInsertionObject) %>'>
            </asp:Button>
            <asp:Button ID="btnInsert" Text="Insert" runat="server" CommandName="PerformInsert"
                Visible='<%# (TypeOf DataItem Is Telerik.Web.UI.GridInsertionObject) %>'></asp:Button>
            &nbsp;
            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                CommandName="Cancel"></asp:Button>
        </td>
    </tr>
</table>