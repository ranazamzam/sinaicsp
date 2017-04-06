<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="csp_list.aspx.vb" Inherits="Sinai_eIEP.csp_list" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .infoIcon, .notificationContent
        {
             display: inline-block; 
             zoom: 1;
             *display: inline;
        }
           
        .infoIcon
        {
             width: 32px;
             height: 32px;
             margin: 0 10px ;
             vertical-align: top;
        }
 
        .notificationContent
        {
             width: 160px;
             vertical-align: bottom;
        }
       
    </style>
</head>
<body>
    <form id="csp_list" runat="server">
        <asp:Table ID="tblHeading" runat="server" Width="100%" >
            <asp:TableRow>
                <asp:TableCell Width="50%"><img class="auto-style1" src="images/SINAI%20Schools%20logo.jpeg" height="70px" /></asp:TableCell>
                <asp:TableCell Width="48%" HorizontalAlign="Right"><a runat="server" href="/admin.aspx" target="_blank" style="display:none" id="admin_link">Admin</a></asp:TableCell>
                <asp:TableCell Width="2%">&nbsp;</asp:TableCell>
            </asp:TableRow>
        </asp:Table>

        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false" Skin="Office2007" >
        </telerik:RadSkinManager>

        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="30" >

        </asp:ScriptManager>

        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <script type="text/javascript">

                var timeLeftCounter = null;
                var seconds = <%= timeoutWarningLength.ToString%>
                
                function pageLoad() {
                    var xmlPanel = $find("<%= RadNotification1.ClientID %>")._xmlPanel;
                    xmlPanel.set_enableClientScriptEvaluation(true);
                };

                function stopTimer(timer) {
                    clearInterval(this[timer]);
                    this[timer] = null;
                };

                function resetTimer(timer, func, interval) {
                    this.stopTimer(timer);
                    this[timer] = setInterval(Function.createDelegate(this, func), interval);
                };

                function OnClientShowing(sender, args) {
                    resetTimer("timeLeftCounter", UpdateTimeLabel, 1000);
                }
                                
                //update the text in the label in RadNotification
                //this could also be done automatically by using UpdateInterval. However, this will cause callbacks [which is the second best solution than javascript] on every second that is being count
                function UpdateTimeLabel(toReset) {
                    var sessionExpired = (seconds == 0);
                    if (sessionExpired) {
                        stopTimer("timeLeftCounter");
                        //redirect to session expired page - simply take the url which RadNotification sent from the server to the client as value
                        window.location.href = $find("<%= RadNotification1.ClientID %>").get_value();
                    }
                    else {
                        var timeLbl = $get("timeLbl");
                        timeLbl.innerHTML = seconds--;
                    }
                }

                function ContinueSession() {
                    var notification = $find("<%= RadNotification1.ClientID %>");
                    //we need to contact the server to restart the Session - the fastest way is via callback
                    //calling update() automatically performs the callback, no need for any additional code or control
                    notification.update();
                    notification.hide();

                    //resets the showInterval for the scenario where the Notification is not disposed (e.g. an AJAX request is made)
                    //You need to inject a call to the ContinueSession() function from the code behind in such a request
                    var showIntervalStorage = notification.get_showInterval(); //store the original value
                    notification.set_showInterval(0); //change the timer to avoid untimely showing, 0 disables automatic showing
                    notification.set_showInterval(showIntervalStorage); //sets back the original interval which will start counting from its full value again

                    seconds = <%= timeoutWarningLength.ToString%>
                    stopTimer("timeLeftCounter");
                }

                // Header functions

                function selectStudent(sender, args) {
                    //console.log(sender)
                    sender.closeDropDown()

                    if (sender.get_id().indexOf("ddSubjects") > 0) {
                        var tbSubject = document.getElementById(sender.get_id().replace("ddSubjects", "tbSubject_Div"));

                        if (tbSubject != null) {
                            setTimeout(function () { if (sender.get_selectedText().indexOf('Other (specify subject name below)') >= 0) { tbSubject.style.display = "block"; } else { tbSubject.style.display = "none"; } }, 200);
                        }
                    }
                }

                function yearChange(sender, args) {
                    if ($find("cmbStudents").get_selectedItem() != null) {
                        $find("cmbStudents").get_selectedItem().set_checked()
                    }
                }

                function addTeacher(sender, args) {
                    var cmbTeacher = $find(sender.get_id().replace("btnAddTeacher", "cmbTeacher"));
                    var lbTeachers = $find(sender.get_id().replace("btnAddTeacher", "lbTeachers"));

                    if (!lbTeachers.findItemByValue(cmbTeacher.get_value())) {
                        var item = new Telerik.Web.UI.RadListBoxItem();
                        item.set_text(cmbTeacher.get_text());
                        item.set_value(cmbTeacher.get_value());
                        lbTeachers.trackChanges();
                        lbTeachers.get_items().add(item);
                        item.select();
                        lbTeachers.commitChanges();
                    }
                }

                function onDblClick(sender, args) {
                    var comboItem = sender.get_selectedItem();

                    if (comboItem) {
                        sender.trackChanges();
                        sender.get_items().remove(comboItem);
                        sender.commitChanges();
                    }
                }

                var hoveredItem;
                var myTeachers;

                function cpTeachers(sender, eventArgs) {
                    if (hoveredItem != null && hoveredItem != undefined) {
                        var menu = $find("<%=ctxTeachers.ClientID%>");
                        var evt = eventArgs.get_domEvent();
                        myTeachers = $find(sender._element.id);
                        var items = myTeachers.get_items();
                        items.forEach(function (item) {
                            item.unselect();
                        });
                        hoveredItem.select();
                        menu.show(evt);
                    }
                }

                function getHover(sender, args) {
                    hoveredItem = args.get_item();
                }

                function cpMenuTeachersClick(sender, args) {
                    myTeachers.deleteItem(myTeachers.get_selectedItem())
                }

                function doStudentPDF(sender, args) {
                    window.open('GeneratePDF.aspx?student_uuid=' + args.get_commandArgument());
                }
            </script>
        </telerik:RadCodeBlock>

        <asp:UpdatePanel runat="server" id="updNotify" UpdateMode="Always">
            <ContentTemplate>
                <telerik:RadNotification ID="RadNotification1" runat="server" Position="Center" Width="240" Height="100" OnCallbackUpdate="OnCallbackUpdate" OnClientShowing="OnClientShowing" 
                  LoadContentOn="PageLoad" AutoCloseDelay="60000" Title="Continue Your Session"  TitleIcon="" EnableRoundedCorners="true"
                  ShowCloseButton="false" KeepOnMouseOver="false">
                  <ContentTemplate>
                       <div class="infoIcon">
                            <img src="images/infoIcon.jpg" alt="info icon" /></div>
                       <div class="notificationContent">
                            Time remaining:&nbsp; <span id="timeLbl">300</span>
                            <telerik:RadButton ID="continueSession" runat="server"  Text="Continue Your Session"
                                 Style="margin-top: 10px;" AutoPostBack="false" OnClientClicked="ContinueSession">
                            </telerik:RadButton>
                       </div>
                  </ContentTemplate>
                </telerik:RadNotification>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="usr_login" runat="server" />

        <telerik:RadContextMenu runat="server" ID="ctxTeachers" EnableRoundedCorners="true" EnableShadows="true" OnClientItemClicked="cpMenuTeachersClick"  >
            <Items>
                <telerik:RadMenuItem Text="Delete Selected" ImageUrl="images/RedX_large.gif" Value="deleteSelected" />
            </Items>
        </telerik:RadContextMenu>

        <!-- Sql Connections -->
        <asp:SqlDataSource ID="sqlTeachers" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
            SelectCommand="getSchoolList @usr_login">
            <SelectParameters>
                <asp:ControlParameter ControlID="usr_login" Name="usr_login" PropertyName="Value" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="sqlClasses" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
            SelectCommand="SELECT '' AS current_placement, '0' AS val UNION SELECT DISTINCT current_placement, current_placement FROM students WHERE is_active = 1 AND (school_id = @school_id OR @school_id = 0) AND (school_id = dbo.getUserSchoolId(@usr_login) OR dbo.getUserSchoolId(@usr_login) IS NULL) AND NOT current_placement IS NULL">
            <SelectParameters>
                <asp:ControlParameter ControlID="cmbSchools" Name="school_id" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="usr_login" Name="usr_login" PropertyName="Value" />
            </SelectParameters> 
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="sqlCspList" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
            SelectCommand="getIepList_New @usr_login, @student_uuid, @teacher_uuid, @school_id, @is_active, @subject_text, @class_placement, @school_year">
            <SelectParameters>
                <asp:ControlParameter ControlID="usr_login" Name="usr_login" PropertyName="Value" />
                <asp:ControlParameter ControlID="cmbTeachers" Name="teacher_uuid" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="cmbStudents" Name="student_uuid" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="cmbSchools" Name="school_id" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="cmbClass" Name="class_placement" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="cmbSubjects" Name="subject_text" PropertyName="SelectedValue" />
                <asp:ControlParameter ControlID="chkDeleted" Name="is_active" PropertyName="Checked" />
                <asp:ControlParameter ControlID="cmbYear" Name="school_year" PropertyName="SelectedValue" />
            </SelectParameters>
        </asp:SqlDataSource>

       <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"  ></telerik:RadAjaxLoadingPanel>

        <!--Filters-->
        
        <asp:table ID="Table1" runat="server" CellPadding="2" CellSpacing="2">
            <asp:TableRow>
                <asp:TableCell Width="3%"></asp:TableCell>
                <asp:TableCell Width="12%">Teacher</asp:TableCell>
                <asp:TableCell Width="12%">Student</asp:TableCell>
                <asp:TableCell Width="12%">Subject</asp:TableCell>
                <asp:TableCell Width="10%">Class</asp:TableCell>
                <asp:TableCell Width="10%"><asp:Label runat="server" ID="lblSchoolFilter">School</asp:Label></asp:TableCell>
                <asp:TableCell Width="7%"></asp:TableCell>
                <asp:TableCell Width="34%" HorizontalAlign="Left">School Year</asp:TableCell>
            </asp:TableRow>
            <asp:TableRow>
                <asp:TableCell></asp:TableCell>
                <asp:TableCell>
                    <telerik:RadComboBox OnItemsRequested="cmbTeachers_ItemsRequested" OnLoad="cmbTeachers_Load" OnDataBound="cmbTeachers_DataBound" runat="server" ID="cmbTeachers" ShowToggleImage ="false" EnableLoadOnDemand="true" AutoPostBack="true" MarkFirstMatch="true" Height="100px" Width="200px" >
                        <Items>
                            <telerik:RadComboBoxItem Value="00000000-0000-0000-0000-000000000000" Selected="true" />
                        </Items>
                    </telerik:RadComboBox>
                </asp:TableCell>
                <asp:TableCell>
                    <telerik:RadComboBox OnItemsRequested="cmbStudents_ItemsRequested" OnLoad="cmbStudents_Load" OnDataBound="cmbStudents_DataBound" runat="server" ID="cmbStudents" ShowToggleImage ="false" EnableLoadOnDemand="true" AutoPostBack="true" MarkFirstMatch="true" Height="100px" Width="200px" >
                        <Items>
                            <telerik:RadComboBoxItem Value="00000000-0000-0000-0000-000000000000" Selected="true" />
                        </Items>
                    </telerik:RadComboBox>
                </asp:TableCell>
                <asp:TableCell>
                    <telerik:RadComboBox OnItemsRequested="cmbSubjects_ItemsRequested" OnLoad="cmbSubjects_Load" OnDataBound="cmbSubjects_DataBound" runat="server" ID="cmbSubjects" ShowToggleImage ="false" EnableLoadOnDemand="true" AutoPostBack="true" MarkFirstMatch="true" Height="100px" Width="200px" >
                        <Items>
                            <telerik:RadComboBoxItem Value="00000000-0000-0000-0000-000000000000" Selected="true" />
                        </Items>
                    </telerik:RadComboBox>
                </asp:TableCell>
                <asp:TableCell>
                    <telerik:RadComboBox runat="server" ID="cmbClass" AutoPostBack="true" Height="100px" DataSourceID="sqlClasses" DataTextField="current_placement" DataValueField="val" >
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Selected="true" />
                        </Items>
                    </telerik:RadComboBox>
                </asp:TableCell>
                <asp:TableCell>
                    <telerik:RadComboBox runat="server" ID="cmbSchools" AutoPostBack="true" Height="100px" OnLoad="cmbSchools_Load" DataSourceID="sqlTeachers" DataTextField="school_name" DataValueField="school_id" >
                        <Items>
                            <telerik:RadComboBoxItem Value="0" Selected="true" />
                        </Items>
                    </telerik:RadComboBox>
                </asp:TableCell>
                <asp:TableCell>
                    <asp:CheckBox ID="chkDeleted" runat="server" AutoPostBack="true" Text="Show Deleted: " TextAlign="Left" Checked="false" Visible="false" />
                </asp:TableCell>
                <asp:TableCell>
                    <telerik:RadComboBox runat="server" ID="cmbYear" AutoPostBack="true" Height="100px" OnSelectedIndexChanged="cmbYear_SelectedIndexChanged"  >
                        <Items>
                            <telerik:RadComboBoxItem Value="2016 - 2017" Text="2016 - 2017" Selected="true" />
                            <telerik:RadComboBoxItem Value="2015 - 2016" Text="2015 - 2016" />
                            <telerik:RadComboBoxItem Value="2014 - 2015" Text="2014 - 2015" />
                            <telerik:RadComboBoxItem Value="2013 - 2014" Text="2013 - 2014" />
                        </Items>
                    </telerik:RadComboBox>
                    <!--<asp:CheckBox ID="chkShowPrevYear" runat="server" AutoPostBack="true" Text="Show Previous Year: " TextAlign="Left" Checked="false" />-->
                </asp:TableCell>
            </asp:TableRow>
        </asp:table>

        <!-- Data grid -->
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" RequestQueueSize="30">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgCspList" >
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgCspList" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadGrid ID="rgCspList" runat="server" AllowPaging="True" CellSpacing="0" GridLines="None" PageSize="20" DataSourceID="sqlCspList" ShowStatusBar="True"  Width="90%" HorizontalAlign="Center" >
            <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="False" DataKeyNames="iep_uuid, usr_uuid, school_id" DataSourceID="sqlCspList" ClientDataKeyNames="iep_uuid, school_id" CommandItemSettings-AddNewRecordText="New CSP">
                <GroupHeaderTemplate>
                    <asp:Label runat="server" ID="Label1" Text='<%# Eval("display_name")%>' />
                    <telerik:RadButton ID="RadButton1" runat="server" OnClientClicked="doStudentPDF" CommandArgument='<%# Eval("student_uuid")%>'  Text="PDF" />
                </GroupHeaderTemplate>
                <GroupByExpressions>
                    <telerik:GridGroupByExpression>
                        <SelectFields>
                            <telerik:GridGroupByField FieldAlias="sort_name" FieldName="sort_name" ></telerik:GridGroupByField>
                            <telerik:GridGroupByField FieldAlias="display_name" FieldName="display_name" ></telerik:GridGroupByField>
                            <telerik:GridGroupByField FieldAlias="student_uuid" FieldName="student_uuid" ></telerik:GridGroupByField>
                        </SelectFields>
                        <GroupByFields>
                            <telerik:GridGroupByField FieldName="sort_name" SortOrder="Ascending"></telerik:GridGroupByField>
                        </GroupByFields>
                    </telerik:GridGroupByExpression>
                </GroupByExpressions>

                <CommandItemSettings AddNewRecordText="New CSP" />
            
                <Columns>
                    <telerik:GridEditCommandColumn ButtonType="ImageButton" UniqueName="EditCommandColumn1">
                        <HeaderStyle Width="20px"></HeaderStyle>
                        <ItemStyle CssClass="MyImageButton"></ItemStyle>
                    </telerik:GridEditCommandColumn>
                    <telerik:GridButtonColumn ConfirmText="Delete CSP?" ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn1">
                        <HeaderStyle Width="20px"></HeaderStyle>
                        <ItemStyle CssClass="MyImageButton"></ItemStyle>
                    </telerik:GridButtonColumn>
                    <telerik:GridBoundColumn DataField="iep_uuid" DataType="System.Guid" FilterControlAltText="Filter iep_uuid column" HeaderText="iep_uuid" ReadOnly="True" SortExpression="iep_uuid" UniqueName="iep_uuid" Visible="false">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="student_uuid" FilterControlAltText="Filter student_uuid column" HeaderText="student_uuid" SortExpression="student_uuid" UniqueName="student_uuid" Visible="false">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="display_name" FilterControlAltText="Filter Student column" HeaderText="Student" SortExpression="student_last_name" UniqueName="display_name" ReadOnly="true">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="sort_name" FilterControlAltText="Filter Student column" HeaderText="Student" SortExpression="student_last_name" UniqueName="sort_name" Visible="false" ReadOnly="true">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="student_last_name" FilterControlAltText="Filter student_last_name column" HeaderText="student_last_name" SortExpression="student_last_name" UniqueName="student_last_name" Visible="false">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="school_id" FilterControlAltText="Filter school_id column" HeaderText="school_id" SortExpression="school_id" UniqueName="school_id" Visible="false">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="school_name" FilterControlAltText="Filter school_name column" HeaderText="School" SortExpression="school_name" UniqueName="school_name" ReadOnly="true">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="subject_text" FilterControlAltText="Filter iep_subject column" HeaderText="Subject" SortExpression="subject_text" UniqueName="iep_subject" ReadOnly="true">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="iep_materials" FilterControlAltText="Filter iep_materials column" HeaderText="Materials" SortExpression="iep_materials" UniqueName="iep_materials" Visible="true">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="usr_display_name" FilterControlAltText="Filter usr_display_name column" HeaderText="Teacher" SortExpression="usr_display_name" UniqueName="usr_display_name">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="usr_uuid" HeaderText="usr_uuid" SortExpression="usr_uuid" UniqueName="usr_uuid" Visible="false">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="teacher_2_uuid" HeaderText="teacher_2_uuid" SortExpression="teacher_2_uuid" UniqueName="teacher_2_uuid" Visible="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="teacher_3_uuid" HeaderText="teacher_3_uuid" SortExpression="teacher_3_uuid" UniqueName="teacher_3_uuid" Visible="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="teacher_4_uuid" HeaderText="teacher_4_uuid" SortExpression="teacher_4_uuid" UniqueName="teacher_4_uuid" Visible="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="teacher_5_uuid" HeaderText="teacher_5_uuid" SortExpression="teacher_5_uuid" UniqueName="teacher_5_uuid" Visible="false">
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="iep_year" DataType="System.Int16" FilterControlAltText="Filter iep_year column" HeaderText="School Year" SortExpression="iep_year" UniqueName="iep_year" ReadOnly="true">
                        <ColumnValidationSettings>
                            <ModelErrorMessage Text="" />
                        </ColumnValidationSettings>
                    </telerik:GridBoundColumn>
                    <telerik:GridCheckBoxColumn DataField="is_active" DataType="System.Boolean" FilterControlAltText="Filter is_active column" HeaderText="is_active" SortExpression="is_active" UniqueName="is_active" Visible="false">
                    </telerik:GridCheckBoxColumn>
                </Columns>

                <EditFormSettings UserControlName="csp_header.ascx" EditFormType="WebUserControl">
                    <EditColumn UniqueName="EditCommandColumn1" ></EditColumn>
                </EditFormSettings>
            </MasterTableView>

            <ClientSettings EnablePostBackOnRowClick="true" >
                <Selecting AllowRowSelect="true" />
            </ClientSettings>
            <PagerStyle Mode="NextPrev" />
        </telerik:RadGrid>

    </form>
</body>
</html>
