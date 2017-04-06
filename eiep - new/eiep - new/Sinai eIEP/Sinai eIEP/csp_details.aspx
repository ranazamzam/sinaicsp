<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="csp_details.aspx.vb" Inherits="Sinai_eIEP.csp_details" %>
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
    <form id="form1" runat="server">
        
        <asp:Table ID="tblHeading" runat="server" Width="100%" >
            <asp:TableRow>
                <asp:TableCell Width="50%"><img class="auto-style1" src="images/SINAI%20Schools%20logo.jpeg" height="70px" /></asp:TableCell>
                <asp:TableCell Width="48%" HorizontalAlign="Right"><a runat="server" href="/admin.aspx" target="_blank" style="display:none" id="admin_link">Admin</a></asp:TableCell>
                <asp:TableCell Width="2%">&nbsp;</asp:TableCell>
            </asp:TableRow>
        </asp:Table>
        <telerik:RadScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="30" >
            <Services>
                <asp:ServiceReference Path="~/cspUpdate.asmx" />
            </Services>
        </telerik:RadScriptManager>

        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" ></telerik:RadAjaxLoadingPanel>

        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false" Skin="Office2007" >
        </telerik:RadSkinManager>

        <telerik:RadCodeBlock runat="server" ID="RadCodeBlock1">
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


                function onRowCreated(sender, args) {
                    var strGrade

                    if ($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_editItems().length == 0) {
                        strGrade = args.get_gridDataItem().get_element().cells[7].innerHTML
                        args.get_gridDataItem().get_element().cells[7].innerHTML = strGrade.substring(0, ((Math.abs(strGrade.indexOf(' - ')) + strGrade.indexOf(' - ')) / 2) || strGrade.length)
                        strGrade = args.get_gridDataItem().get_element().cells[8].innerHTML
                        args.get_gridDataItem().get_element().cells[8].innerHTML = strGrade.substring(0, ((Math.abs(strGrade.indexOf(' - ')) + strGrade.indexOf(' - ')) / 2) || strGrade.length)
                        strGrade = args.get_gridDataItem().get_element().cells[9].innerHTML
                        args.get_gridDataItem().get_element().cells[9].innerHTML = strGrade.substring(0, ((Math.abs(strGrade.indexOf(' - ')) + strGrade.indexOf(' - ')) / 2) || strGrade.length)
                    }
                    if (args.get_gridDataItem().getDataKeyValue("iep_data_parent_uuid") != "") {
                        args.get_gridDataItem().get_element().cells[5].innerHTML = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + args.get_gridDataItem().get_element().cells[5].innerHTML
                    } else if (args.get_itemIndexHierarchical() == "0") {
                        args.get_gridDataItem().get_element().cells[2].innerHTML = "&nbsp;"
                    }
                }

                function OnColumnCreated(sender, args) {
                    var column = args.get_column();
                    var d = new Date();
                    var intYear = d.getFullYear() - 2000;

                    if (d.getMonth() < 7) {
                        intYear = intYear - 1
                    }

                    var student_name = document.getElementById("lblStudent").innerHTML;
                    student_name = student_name.substring(0, student_name.indexOf(" "))

                    switch (column.get_uniqueName()) {
                        case "iep_data_text":
                            column.get_element().innerHTML = student_name + " will:"; //setting new HeaderText
                            break;
                        case "iep_data_grade_1_id":
                            column.get_element().innerHTML = "11/" + intYear.toString();
                            break;
                        case "iep_data_grade_2_id":
                            column.get_element().innerHTML = "2/" + (intYear + 1).toString();
                            break;
                        case "iep_data_grade_3_id":
                            column.get_element().innerHTML = "6/" + (intYear + 1).toString();
                            break;
                    }
                }

                function SetEditorContent(content, key, eMode) {
                    //set content to RadEditor on the main page from RadWindow
                    $find("<%= reTransfer.ClientID%>").set_html(content);
                    
                    switch (eMode) {
                        case "Comments":
                            Sinai_eIEP.cspUpdate.updateCommentsNotes('<%= Me.Request.QueryString("iep_uuid")%>', 'iep_comments', $find("<%= reComments.ClientID%>").get_html(true))
                            //$find("<%= RadAjaxManager1.ClientID%>").ajaxRequest("iep_comments");
                            break;
                        case "FNotes":
                            Sinai_eIEP.cspUpdate.updateCommentsNotes('<%= Me.Request.QueryString("iep_uuid")%>', 'iep_feb_notes', $find("<%= reFNotes.ClientID%>").get_html(true))
                            //$find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("iep_feb_notes");
                            break;
                        case "JNotes":
                            Sinai_eIEP.cspUpdate.updateCommentsNotes('<%= Me.Request.QueryString("iep_uuid")%>', 'iep_june_notes', $find("<%= reJNotes.ClientID%>").get_html(true))
                            //$find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("iep_june_notes");
                            break;
                        case "SubGoal":
                            try {
                                var list = $find("<%= lbClipboard.ClientID%>")
                                var items = list.get_items();
                                var item = new Telerik.Web.UI.RadListBoxItem();
                                //list.trackChanges();
                                item.set_text(content);
                                item.set_value(content);
                                items.add(item);
                                list.commitChanges();
                            }
                            catch (err) {
                                console.log("Clipboard error: " + err)
                            }
                            $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("doSubGoal");
                            $find("<%= rgCspGoals.ClientID%>").get_masterTableView().rebind();
                            break;
                        default:
                            try {
                                var list = $find("<%= lbClipboard.ClientID%>")
                                var items = list.get_items();
                                var item = new Telerik.Web.UI.RadListBoxItem();
                                //list.trackChanges();
                                item.set_text(content);
                                item.set_value(content);
                                items.add(item);
                            }
                            catch(err){
                                console.log("Clipboard error: " + err)
                            }
                            $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("InsertUpdateIepData");
                            $find("<%= rgCspGoals.ClientID%>").get_masterTableView().rebind();
                    }
                }

                function SetDialogContent(oWnd) {
                    var contentWindow = oWnd.get_contentFrame().contentWindow;
                    if (contentWindow && contentWindow.setContent) {
                        window.setTimeout(function () {
                            //pass and set the content from the mane page to RadEditor in RadWindow
                            contentWindow.setContent(editorContent, "<%= Me.Request.QueryString("iep_uuid")%>", editMode);
                        }, 100);
                    }
                }

                function onWindowClose(oWnd, args) {
                    var contentWindow = oWnd.get_contentFrame().contentWindow;
                    if (contentWindow.isDirty()) {
                        args.set_cancel(!confirm("Are you sure you want to close without saving?"));
                    }
                }

                function onCommand(sender, args) {
                    
                    switch (args.get_commandName()) {
                        case "EditGoal":
                            editedRow = args.get_commandArgument();
                            //if ($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_editItems().length > 1) { updateTimer = setTimeout(function () { $find("rgCspGoals").get_masterTableView().updateItem($find("rgCspGoals").get_masterTableView().get_dataItems()[0].get_element()); }, 100); }
                            editorContent = $find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_text"); //get RadEditor content
                            document.getElementById("iep_data_uuid").value = $find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_uuid")
                            editMode = 'Edit'
                            $find("<%= DialogWindow.ClientID %>").show(); //open RadWindow
                            args.set_cancel(true);
                            break;
                        case "ClipboardCopy":
                            editedRow = args.get_commandArgument();
                            var list = $find("<%= lbClipboard.ClientID%>")
                            var items = list.get_items();
                            var item = new Telerik.Web.UI.RadListBoxItem();
                            //list.trackChanges();
                            item.set_text($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_text"));
                            item.set_value($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_text"));
                            items.add(item);
                            list.commitChanges();
                            args.set_cancel(true);
                            break;
                        case "SubGoal":
                            editedRow = args.get_commandArgument();
                            $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("HandleSubItem|" + $find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_uuid"));
                            $find("<%= rgCspGoals.ClientID%>").get_masterTableView().rebind()
                            args.set_cancel(true);
                            break;
                    }
                }

                function SaveAll() {
                    $find("<%= RadAjaxLoadingPanel1.ClientID%>").show("<%= rgCspGoals.ClientID%>");
                    $find("<%= RadAjaxLoadingPanel1.ClientID%>").show("<%= reComments.ClientID%>");
                    $find("<%= RadAjaxLoadingPanel1.ClientID%>").show("<%= reFNotes.ClientID%>");
                    $find("<%= RadAjaxLoadingPanel1.ClientID%>").show("<%= reJNotes.ClientID%>");
                    if ($find("<%= rgComments.ClientID%>").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_comments") != $find("<%= reComments.ClientID%>").get_html(true)) {
                        //$find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("iep_comments");
                        Sinai_eIEP.cspUpdate.updateCommentsNotes('<%= Me.Request.QueryString("iep_uuid")%>', 'iep_comments', $find("<%= reComments.ClientID%>").get_html(true))
                    }
                    if ($find("<%= rgComments.ClientID%>").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_feb_notes") != $find("<%= reFNotes.ClientID%>").get_html(true)) {
                        //$find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("iep_feb_notes");
                        Sinai_eIEP.cspUpdate.updateCommentsNotes('<%= Me.Request.QueryString("iep_uuid")%>', 'iep_feb_notes', $find("<%= reFNotes.ClientID%>").get_html(true))
                    }
                    if ($find("<%= rgComments.ClientID%>").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_june_notes") != $find("<%= reJNotes.ClientID%>").get_html(true)) {
                        //$find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("iep_june_notes");
                        Sinai_eIEP.cspUpdate.updateCommentsNotes('<%= Me.Request.QueryString("iep_uuid")%>', 'iep_june_notes', $find("<%= reJNotes.ClientID%>").get_html(true))
                    }
                    if ($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_editItems().length > 1) {
                        //$find("rgCspGoals").get_masterTableView().updateItem($find("rgCspGoals").get_masterTableView().get_dataItems()[0].get_element());
                        $find("<%= rgCspGoals.ClientID%>").get_masterTableView().cancelAll();
                    }
                    
                    var cbItems = $find("<%= lbClipboard.ClientID%>").get_items();
                    var strItems = '';

                    cbItems.forEach(function (item) {
                        strItems = strItems + '~' + item.get_value() + '|||' + item.get_text();
                    });

                    $find("<%= reClipboardTransfer.ClientID%>").set_html(strItems);
                }

                function onToolBarClientButtonClicking(sender, args) {
                    var button = args.get_item(); 

                    switch (button.get_commandName()) {
                        case "CancelAll":
                            args.set_cancel(true);
                            SaveAll();
                            $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("doClose");
                            break;
                        case "EditAll":
                            if ($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_dataItems().length > 0) {
                                $find("<%= rgCspGoals.ClientID%>").get_masterTableView().editAllItems();
                            }
                            args.set_cancel(true);
                            break;
                        case "Sequence":
                            if ($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_editItems().length > 0) {
                                //$find("rgCspGoals").get_masterTableView().updateItem($find("rgCspGoals").get_masterTableView().get_dataItems()[0].get_element());
                                $find("<%= rgCspGoals.ClientID%>").get_masterTableView().cancelAll();
                            }
                            args.set_cancel(true);
                            break;
                        case "ExportPDF":
                            window.open('GeneratePDF.aspx?iep_uuid=<%=Request.QueryString("iep_uuid") %>')
                            args.set_cancel(true);
                            break;
                        case "NewSubGoal":
                            newGoal = true;
                            if ($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_selectedItems().length > 1) {
                                alert('This action may not be completed when more than one goal is selected.');
                                args.set_cancel(true);
                                return;
                                break;
                            }
                            if ($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_dataItems().length == 0) {
                                alert('This action may not be completed if there are no parent goals.');
                                args.set_cancel(true);
                                return;
                                break;
                            }
                            document.getElementById("iep_data_uuid").value = "";
                            //if ($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_editItems().length > 1) { updateTimer = setTimeout(function () { $find("rgCspGoals").get_masterTableView().updateItem($find("rgCspGoals").get_masterTableView().get_dataItems()[0].get_element()); }, 100); }
                            editMode = 'SubGoal';
                            editorContent = "";
                            $find("<%= DialogWindow.ClientID %>").reload();
                            $find("<%= DialogWindow.ClientID %>").show(); //open RadWindow
                            args.set_cancel(true);
                            break;
                        case "NewGoal":
                            newGoal = true;
                            if ($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_selectedItems().length > 1) {
                                alert('This action may not be completed when more than one goal is selected.');
                                args.set_cancel(true);
                                return;
                                break;
                            }
                            document.getElementById("iep_data_uuid").value = "";
                            //if ($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_editItems().length > 1) { updateTimer = setTimeout(function () { $find("rgCspGoals").get_masterTableView().updateItem($find("rgCspGoals").get_masterTableView().get_dataItems()[0].get_element()); }, 100); }
                            editMode = 'Goal';
                            editorContent = "";
                            $find("<%= DialogWindow.ClientID %>").reload();
                            $find("<%= DialogWindow.ClientID %>").show(); //open RadWindow
                            args.set_cancel(true);
                            break;
                        case "Clone":
                            var list = $find("<%= lbClipboard.ClientID%>")
                            var items = list.get_items();
                            var item = new Telerik.Web.UI.RadListBoxItem();

                            item.set_text(document.getElementById("lblStudent").innerHTML + ' - ' + document.getElementById("lblSubject").innerHTML);
                            item.set_value("<%= Request.QueryString("iep_uuid")%>");
                            items.add(item);
                            list.commitChanges();
                            args.set_cancel(true);
                            break;
                        case "Copy":
                            var oSelected = $find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_selectedItems()

                            if (oSelected.length > 0) {
                                var list = $find("<%= lbClipboard.ClientID%>")
                                var items = list.get_items();
                                //list.trackChanges();
                                for (i = 0; i < oSelected.length; i++) {
                                    var item = new Telerik.Web.UI.RadListBoxItem();
                                    item.set_text($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_dataItems()[oSelected[i].get_itemIndexHierarchical()].getDataKeyValue("iep_data_text"));
                                    item.set_value($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_dataItems()[oSelected[i].get_itemIndexHierarchical()].getDataKeyValue("iep_data_text"));
                                    items.add(item);
                                }
                                list.commitChanges();
                            }
                            args.set_cancel(true);
                            break;
                        case "Undo":
                            $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Undo");
                            $find("<%= rgCspGoals.ClientID%>").get_masterTableView().rebind()
                            args.set_cancel(true);
                            break;
                    }
                }

                function OnClientCommandExecuting(editor, args) {
                    var commandName = args.get_commandName();   //returns the executed command

                    if (commandName == "SaveAndClose") {
                        SetEditorContent(editor.get_html(true), "", editor.get_id().replace("re", ""));     //set the editor content on RadWindow to the editor on the parent page
                        args.set_cancel(true); //cancel the SaveAndClose command
                        alertTimer = setTimeout(function () { alert("Save complete"); }, 750);
                    }
                }

                function loadCommentsNotes(sender, arg) {
                    updateTimer = setTimeout(function () {
                        $find("<%= reComments.ClientID%>").set_html($find("<%= rgComments.ClientID%>").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_comments"));
                        $find("<%= reFNotes.ClientID%>").set_html($find("<%= rgComments.ClientID%>").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_feb_notes"));
                        $find("<%= reJNotes.ClientID%>").set_html($find("<%= rgComments.ClientID%>").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_june_notes"));
                    }, 100);
                    if ($find("<%= rgComments.ClientID%>").get_masterTableView().get_dataItems()[0].getDataKeyValue("isLocked") == "True") {
                        alert($find("<%= rgComments.ClientID%>").get_masterTableView().get_dataItems()[0].getDataKeyValue("lockMessage"));
                        window.location.href = "csp_list.aspx"
                    } else {
                        //goalTimer = setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("LockIEP"); }, 250);
                        Sinai_eIEP.cspUpdate.doLock('<%= Me.Request.QueryString("iep_uuid")%>')
                    }
                }

                function onDragStart(sender, args) {

                    var isChild = (args.getDataKeyValue("iep_data_parent_uuid") != "");
                    var idxDrag = args.get_itemIndexHierarchical();
                    var masterTable = $find("<%=rgCspGoals.ClientID%>").get_masterTableView();

                    if ($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_editItems().length > 1) {
                        args.set_cancel(true);
                    }

                    if (idxDrag != masterTable.get_dataItems().length - 1) {
                        if (!isChild && (masterTable.get_dataItems()[parseInt(idxDrag) + 1].getDataKeyValue("iep_data_parent_uuid") != "")) {
                            //args.set_cancel(true);
                        }
                    }
                }

                function goalRowClick(sender, args) {
                    var intItemNum = parseInt(args.get_itemIndexHierarchical()) + 1;

                    if ($find("rgCspGoals").get_masterTableView().get_dataItems()[intItemNum] != null) {
                        if ($find("rgCspGoals").get_masterTableView().get_dataItems()[intItemNum - 1].getDataKeyValue("iep_data_parent_uuid") == "" && $find("rgCspGoals").get_masterTableView().get_dataItems()[intItemNum].getDataKeyValue("iep_data_parent_uuid") != "") {
                            while ($find("rgCspGoals").get_masterTableView().get_dataItems()[intItemNum].getDataKeyValue("iep_data_parent_uuid") != "") {
                                $find("rgCspGoals").get_masterTableView().get_dataItems()[intItemNum].set_selected(true);
                                intItemNum += 1;
                                if ($find("rgCspGoals").get_masterTableView().get_dataItems()[intItemNum] == null) {
                                    return;
                                }
                            }
                        }
                    }
                }
                
                function ctxGoals(sender, args) {
                    if ($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_selectedItems().length == 0)
                        return;

                    switch (args.get_item().get_value()) {
                        case "copySelected":
                            var oSelected = $find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_selectedItems()

                            if (oSelected.length > 0) {
                                var list = $find("<%= lbClipboard.ClientID%>")
                                var items = list.get_items();
                                oSelected = $find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_dataItems();
                                //list.trackChanges();
                                for (i = 0; i < oSelected.length; i++) {
                                    if (oSelected[i].get_selected()) {
                                        var item = new Telerik.Web.UI.RadListBoxItem();
                                        var sPrefix = ''

                                        if ($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_dataItems()[oSelected[i].get_itemIndexHierarchical()].get_element().cells[2].innerHTML.indexOf("arrow_left") > -1) {
                                            sPrefix = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' //'\t\t\t\t\t\t\t\t\t\t'
                                        }

                                        item.set_text(sPrefix + $find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_dataItems()[oSelected[i].get_itemIndexHierarchical()].getDataKeyValue("iep_data_text"));
                                        item.set_value(sPrefix + $find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_dataItems()[oSelected[i].get_itemIndexHierarchical()].getDataKeyValue("iep_data_text"));
                                        items.add(item);
                                    }
                                }
                                list.commitChanges();
                            }
                            break;
                        case "deleteSelected":
                            $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("deleteSelected");
                            $find("rgCspGoals").get_masterTableView().rebind();
                            break;
                        case "doSubGoal":
                            $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("doBulkSubGoal");
                            $find("rgCspGoals").get_masterTableView().rebind();
                            break;
                        case "doGoal":
                            $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("doBulkGoal");
                            $find("rgCspGoals").get_masterTableView().rebind();
                            break;
                    }
                }

                function RowContextMenu(sender, eventArgs) {
                    var menu = $find("<%=ctxMenu.ClientID%>");
                    var evt = eventArgs.get_domEvent();

                    if ($find("<%= rgCspGoals.ClientID%>").get_masterTableView().get_editItems().length >= 1) {
                        return;
                    }
                    if (evt.target.tagName == "INPUT" || evt.target.tagName == "A") {
                        return;
                    }
                    menu.show(evt);
                }

                var myList;

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
                    var isGc = false;

                    if (myList.get_id().slice(-3) == 'Cat') {
                        isGc = true;
                    }

                    if (checkedItems.length == 0) {
                        return
                    }

                    document.getElementById("iep_data_uuid").value = ""

                    for (i = 0; i < checkedItems.length; i++) {
                        if (isGc) {
                            strItems = strItems + '~' + checkedItems[i].get_text();
                        }
                        else {
                            strItems = strItems + '~' + sPrefix + checkedItems[i].get_value();
                        }
                    }

                    console.log(strItems)

                    $find("<%= reTransfer.ClientID%>").set_html(strItems);
                    $find("<%= RadAjaxLoadingPanel1.ClientID%>").show("<%= rgCspGoals.ClientID%>")
                    $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("ClipboardPaste");
                    goalTimer = setTimeout(checkStatus, 2500);
                }

                function checkStatus() {
                    $find("<%= rgCspGoals.ClientID%>").get_masterTableView().rebind();
                    $find("<%= RadAjaxLoadingPanel1.ClientID%>").hide("<%= rgCspGoals.ClientID%>")
                }

                function gc_pasteItem(sender, args) {
                    var content = 'gc_' + sender.get_selectedItem().get_value()

                    document.getElementById("iep_data_uuid").value = ""
                    $find("<%= reTransfer.ClientID%>").set_html(content);
                    $find("<%= RadAjaxLoadingPanel1.ClientID%>").show("<%= rgCspGoals.ClientID%>")
                    $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("InsertUpdateIepData");
                    goalTimer = setTimeout(checkStatus, 2500);
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

                function onTabSelecting(sender, args) {

                    if (args.get_tab().get_pageViewID()) {
                        args.get_tab().set_postBack(false);
                    }
                }

                function setDateInitiated(sender, args) {
                    //console.log(sender)
                    //console.log(sender.get_id())
                    //console.log(sender.get_value())
                    //console.log(sender.get_parent().getDataKeyValue("iep_data_uuid"))
                    //console.log(Sinai_eIEP.cspUpdate.HelloWorld(sender, sender.get_value()))

                    Sinai_eIEP.cspUpdate.setDateInitiated(sender.get_parent().getDataKeyValue("iep_data_uuid"), sender.get_value())
                }

                function setGradeValue(sender, args) {
                    //console.log(sender)
                    //console.log(sender.get_id())
                    //console.log(sender.get_value())
                    //console.log(sender.get_parent().getDataKeyValue("iep_data_uuid"))
                    //console.log(Sinai_eIEP.cspUpdate.HelloWorld(sender, sender.get_value()))

                    Sinai_eIEP.cspUpdate.setGradeValue(sender.get_id(), sender.get_parent().getDataKeyValue("iep_data_uuid"), sender.get_value())
                }

                function gcChange(sender, args) {

                   

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

        <table id="Table3" cellspacing="1" cellpadding="1" width="80%" border="0">
            <tr>
                <td width="25%">
                    <strong>Student:</strong>&nbsp;<asp:Label ID="lblStudent" runat="server" />
                </td>
                <td width="25%">
                    <strong>School:</strong>&nbsp;<asp:Label ID="lblSchool" runat="server" ></asp:Label>
                </td>
                <td width="25%">
                    <strong>Teacher:</strong>&nbsp;<asp:Label ID="lblTeacher" runat="server" ></asp:Label>
                </td>
                <td width="25%">
                    <strong>Year:</strong>&nbsp;<asp:Label ID="lblYr" runat="server" ></asp:Label>
                </td>
            </tr>
            <tr>
                <td width="50%" colspan="2">
                    <strong>Subject:</strong>&nbsp;<asp:Label ID="lblSubject" runat="server" ></asp:Label>
                </td>
                <td width="50%" colspan="2">
                    <strong>Materials:</strong>&nbsp;<asp:Label ID="lblMats" runat="server" ></asp:Label>
                </td>
            </tr>    
            <tr>
                <td width="100%" colspan="4">&nbsp;</td>
            </tr>
                <tr>
                <td width="50%" colspan="2">
                    <telerik:RadButton ID="rbtnPrev" runat="server" Text="Prev"  AutoPostBack="true" OnClientClicking="SaveAll" />&nbsp;
                    <telerik:RadButton ID="rbtnNext" runat="server" Text="Next"  AutoPostBack="true" OnClientClicking="SaveAll" />
                </td>
                <td width="50%" colspan="2">&nbsp;</td>
            </tr>
        </table>

        <div id="sqlStrings">
            <asp:SqlDataSource ID="sqlCspHeader" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
                SelectCommand="SELECT [iep_uuid], [school_id], [display_name], [school_name], [iep_materials], [usr_display_name], [iep_year], [iep_comments], [iep_feb_notes], [iep_june_notes], [subject_text] FROM [dbo].[vw_list_iep] WHERE iep_uuid = @iep_uuid">
                <SelectParameters>
                    <asp:QueryStringParameter Name="iep_uuid" QueryStringField="iep_uuid" />
                </SelectParameters>
            </asp:SqlDataSource>
        
            <asp:SqlDataSource ID="sqlCspGoals" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
                SelectCommand="SELECT * FROM vw_get_iep_data WHERE iep_uuid = @iep_uuid ORDER BY iep_data_sequence, iep_sub_data_sequence">
                <SelectParameters>
                    <asp:QueryStringParameter Name="iep_uuid" QueryStringField="iep_uuid" />
                </SelectParameters>
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="sqlCspGrades" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
                SelectCommand="SELECT NULL AS grade_id, @school_id AS school_id, '' AS grade_text, '' AS grade_desc UNION SELECT grade_id, school_id, grade_text, grade_desc FROM grades WHERE school_id = @school_id">
                <SelectParameters><asp:ControlParameter  ControlID="hdnSchoolId" Name="school_id" PropertyName="Value"  /></SelectParameters> 
            </asp:SqlDataSource>

            <asp:SqlDataSource ID="sqlCspComments" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
                SelectCommand="SELECT iep_uuid, iep_comments, iep_feb_notes, iep_june_notes, locked_by, dbo.lockMessage(iep_uuid) AS lockMessage, dbo.isLocked(iep_uuid, @usr_login) AS isLocked FROM iep WHERE iep_uuid = @iep_uuid">
                    <SelectParameters>
                        <asp:QueryStringParameter Name="iep_uuid" QueryStringField="iep_uuid" />
                        <asp:ControlParameter ControlID="usr_login" Name="usr_login" PropertyName="Value" />
                    </SelectParameters>
            </asp:SqlDataSource>

            <asp:ObjectDataSource ID="odsCspInitDates" runat="server" TypeName="DDLists" SelectMethod="getInitDatesYr">
                <SelectParameters>
                    <asp:ControlParameter ControlID="lblYr" Name="strYearStart" PropertyName="Text" />
                </SelectParameters>
            </asp:ObjectDataSource>

            <asp:ObjectDataSource ID="odsGCCategories" runat="server" TypeName="DDLists" SelectMethod="getGCCategories">
            </asp:ObjectDataSource>
        </div>
        <asp:HiddenField ID="hdnSchoolId" runat="server" />
        <asp:HiddenField ID="iep_data_uuid" runat="server" />
        <asp:HiddenField ID="usr_login" runat="server" />

        <br />
        <telerik:RadSplitter ID="rspCspDetails" runat="server" BorderStyle="None" Height="800px" Width="100%" BorderSize="0" PanesBorderSize="0" >
            <telerik:RadPane runat="server" ID="rpMain" >
                <asp:Panel ID="mainPanel" runat="server" style="width:98%;margin-left:auto;margin-right:auto" Enabled="true">
                    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" RequestQueueSize="30" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
                        <AjaxSettings>
                            <telerik:AjaxSetting AjaxControlID="rgCspGoals" >
                                <UpdatedControls>
                                    <telerik:AjaxUpdatedControl ControlID="rgCspGoals" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                                </UpdatedControls>
                            </telerik:AjaxSetting>
                        </AjaxSettings>
                    </telerik:RadAjaxManager>
                    
                    <telerik:RadGrid ID="rgCspGoals" runat="server"  AllowMultiRowSelection="True" AllowMultiRowEdit="True" CellSpacing="1" DataSourceID="sqlCspGoals" ClientSettings-ClientEvents-OnRowContextMenu="RowContextMenu">
                        <AlternatingItemStyle BackColor="White" />
                        <MasterTableView EditMode="InPlace" CommandItemDisplay="Top" AutoGenerateColumns="False" DataKeyNames="iep_data_uuid, iep_data_parent_uuid, iep_data_text" ClientDataKeyNames="iep_uuid, iep_data_uuid, iep_data_parent_uuid, iep_data_text" CommandItemSettings-AddNewRecordText="New Goal" DataSourceID="sqlCspGoals" >
                            <Columns>
                                <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" >
                                    <HeaderStyle Width="15px"></HeaderStyle>
                                </telerik:GridClientSelectColumn>
                                <telerik:GridButtonColumn ConfirmText="Delete this goal?" ButtonType="ImageButton" CommandName="Delete" Text="Delete" UniqueName="DeleteColumn1" ShowInEditForm="false">
                                    <HeaderStyle Width="15px"></HeaderStyle>
                                    <ItemStyle CssClass="MyImageButton"></ItemStyle>
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" CommandName="SubGoal" UniqueName="SubGoal" ImageUrl="images/arrow_right.gif" Visible="true">
                                    <HeaderStyle Width="15px"></HeaderStyle>
                                    <ItemStyle CssClass="MyImageButton"></ItemStyle>
                                </telerik:GridButtonColumn>
                                <telerik:GridBoundColumn DataField="iep_data_sequence" DataType="System.Byte" FilterControlAltText="Filter iep_data_sequence column" HeaderText="" SortExpression="iep_data_sequence" UniqueName="iep_data_sequence" Visible="false" ReadOnly="true" >
                                    <ItemStyle Width="10" />
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="iep_sub_data_sequence" DataType="System.Byte" FilterControlAltText="Filter iep_sub_data_sequence column" HeaderText="" SortExpression="iep_sub_data_sequence" UniqueName="iep_sub_data_sequence" Visible="false" ReadOnly="true" >
                                    <ItemStyle Width="10" />
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="EditCommandColumn1" CommandName="EditGoal" Text="Edit goal text" ShowInEditForm="true" ImageUrl="images/Edit.gif">
                                    <HeaderStyle Width="15px"></HeaderStyle>
                                    <ItemStyle CssClass="MyImageButton"></ItemStyle>
                                </telerik:GridButtonColumn>
                                <telerik:GridButtonColumn ButtonType="ImageButton" UniqueName="ClipboardCopy" CommandName="ClipboardCopy" Text="Copy to clipboard" ShowInEditForm="true" ImageUrl="images/clone.png">
                                    <HeaderStyle Width="15px"></HeaderStyle>
                                    <ItemStyle CssClass="MyImageButton"></ItemStyle>
                                </telerik:GridButtonColumn>
                                <telerik:GridBoundColumn DataField="iep_uuid" DataType="System.Guid" FilterControlAltText="Filter iep_uuid column" HeaderText="iep_uuid" SortExpression="iep_uuid" UniqueName="iep_uuid" Visible="false">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="iep_data_uuid" DataType="System.Guid" FilterControlAltText="Filter iep_data_uuid column" HeaderText="iep_data_uuid" ReadOnly="True" SortExpression="iep_data_uuid" UniqueName="iep_data_uuid" Visible="false">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="iep_data_parent_uuid" DataType="System.Guid" FilterControlAltText="Filter iep_data_parent_uuid column" HeaderText="iep_data_parent_uuid" SortExpression="iep_data_parent_uuid" UniqueName="iep_data_parent_uuid" Visible="false">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridBoundColumn DataField="iep_data_text" FilterControlAltText="Filter iep_data_text column" HeaderText="Goal" SortExpression="iep_data_text" UniqueName="iep_data_text" ReadOnly="true">
                                    <ColumnValidationSettings>
                                        <ModelErrorMessage Text="" />
                                    </ColumnValidationSettings>
                                </telerik:GridBoundColumn>
                                <telerik:GridDropDownColumn UniqueName="iep_date_initiated" HeaderText="Date Initiated" ColumnEditorID="GridDropDownListColumnEditor4" HeaderStyle-HorizontalAlign="Center" DataField="iep_date_initiated" ListTextField="init_date" ListValueField="init_date" DataSourceID="odsCspInitDates" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle Width="60px"></ItemStyle>
                                </telerik:GridDropDownColumn>
                                <telerik:GridDropDownColumn UniqueName="iep_data_grade_1_id" HeaderText="Grade" ColumnEditorID="GridDropDownListColumnEditor1" HeaderStyle-HorizontalAlign="Center" ListTextField="grade_desc" ListValueField="grade_id" DataSourceID="sqlCspGrades" DataField="iep_data_grade_1_id" ItemStyle-HorizontalAlign="Center" >
                                    <ItemStyle Width="50px"></ItemStyle>
                                </telerik:GridDropDownColumn>
                                <telerik:GridDropDownColumn UniqueName="iep_data_grade_2_id" HeaderText="Grade" ColumnEditorID="GridDropDownListColumnEditor2" HeaderStyle-HorizontalAlign="Center" ListTextField="grade_desc" ListValueField="grade_id" DataSourceID="sqlCspGrades" DataField="iep_data_grade_2_id" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle Width="50px"></ItemStyle>
                                </telerik:GridDropDownColumn>
                                <telerik:GridDropDownColumn UniqueName="iep_data_grade_3_id" HeaderText="Grade" ColumnEditorID="GridDropDownListColumnEditor3" HeaderStyle-HorizontalAlign="Center" ListTextField="grade_desc" ListValueField="grade_id" DataSourceID="sqlCspGrades" DataField="iep_data_grade_3_id" ItemStyle-HorizontalAlign="Center">
                                    <ItemStyle Width="50px"></ItemStyle>
                                </telerik:GridDropDownColumn>
                            </Columns>
                            <CommandItemTemplate>
                                <telerik:RadToolBar ID="RadToolBar1" runat="server"  AutoPostBack="true" OnClientButtonClicking="onToolBarClientButtonClicking" >
                                    <Items>
                                        <telerik:RadToolBarButton runat="server" CommandName="NewGoal" Text="New Goal" ImageUrl="images/AddRecord.gif" Enabled="<%# enableEdit%>"></telerik:RadToolBarButton>
                                        <telerik:RadToolBarButton runat="server" CommandName="NewSubGoal" Text="New Sub Goal" ImageUrl="images/AddRecord.gif" Enabled="<%# enableEdit%>"></telerik:RadToolBarButton>
                                        <telerik:RadToolBarButton runat="server" CommandName="ExportPDF" Text="PDF" ImageUrl="images/pdf.png" ></telerik:RadToolBarButton>
                                        <telerik:RadToolBarButton runat="server" CommandName="EditAll" Text="Initiate/Rate Mode" ImageUrl="images/Edit.gif" Enabled="<%# enableEdit%>"></telerik:RadToolBarButton>
                                        <telerik:RadToolBarButton runat="server" CommandName="Sequence" Text="Position Mode" ImageUrl="images/seq.png" Enabled="<%# enableEdit%>"></telerik:RadToolBarButton>
                                        <telerik:RadToolBarButton runat="server" CommandName="Clone" Text="Clone" ImageUrl="images/clone.png" ></telerik:RadToolBarButton>
                                        <telerik:RadToolBarButton runat="server" CommandName="Undo" Text="Undo" ImageUrl="images/undo.png" Enabled="<%# enableEdit%>"></telerik:RadToolBarButton>
                                        <telerik:RadToolBarButton runat="server" CommandName="CancelAll" Text="Close" ImageUrl="images/RedX.gif"></telerik:RadToolBarButton>
                                    </Items>
                                </telerik:RadToolBar>
                            </CommandItemTemplate>
                        </MasterTableView>
                        <ClientSettings EnablePostBackOnRowClick="false" AllowRowsDragDrop="true" >
                            <ClientEvents OnCommand="onCommand" OnRowCreated="onRowCreated" OnColumnCreated="OnColumnCreated" OnRowDragStarted="onDragStart" OnRowSelecting="goalRowClick" />
                            <Selecting AllowRowSelect="True" EnableDragToSelectRows="false"></Selecting>
                        </ClientSettings>
                        <ItemStyle BackColor="White" />
                        <PagerStyle Mode="NextPrev" />
                    </telerik:RadGrid>

                    <telerik:GridDropDownListColumnEditor ID="GridDropDownListColumnEditor4" runat="server" DropDownStyle-Width="60px"/>
                    <telerik:GridDropDownListColumnEditor ID="GridDropDownListColumnEditor1" runat="server" DropDownStyle-Width="50px"/>
                    <telerik:GridDropDownListColumnEditor ID="GridDropDownListColumnEditor2" runat="server" DropDownStyle-Width="50px"/>
                    <telerik:GridDropDownListColumnEditor ID="GridDropDownListColumnEditor3" runat="server" DropDownStyle-Width="50px"/>

                    <br />
                         <h2>Comments</h2>
                        <telerik:RadEditor ID="reComments" Runat="server" EditModes="Design" Width="100%" Height="290" OnClientCommandExecuting="OnClientCommandExecuting" StripFormattingOnPaste="All" Enabled="true" >
                            <ExportSettings></ExportSettings>
                            <Tools >
                                <telerik:EditorToolGroup Tag="Formatting">
                                    <telerik:EditorTool Name="Bold" />
                                    <telerik:EditorTool Name="Italic" />
                                    <telerik:EditorTool Name="Underline" />
                                    <telerik:EditorTool Name="SaveAndClose" />
                                </telerik:EditorToolGroup>
                            </Tools>
                            <Content></Content>
                            <TrackChangesSettings CanAcceptTrackChanges="False"></TrackChangesSettings>
                        </telerik:RadEditor>
                         <br />
                         <h2>February Notes</h2>
                        <telerik:RadEditor ID="reFNotes" Runat="server" EditModes="Design" Width="100%" Height="290" OnClientCommandExecuting="OnClientCommandExecuting" StripFormattingOnPaste="All">
                            <ExportSettings></ExportSettings>
                            <Tools >
                                <telerik:EditorToolGroup Tag="Formatting">
                                    <telerik:EditorTool Name="Bold" />
                                    <telerik:EditorTool Name="Italic" />
                                    <telerik:EditorTool Name="Underline" />
                                    <telerik:EditorTool Name="SaveAndClose" />
                                </telerik:EditorToolGroup>
                            </Tools>
                            <Content>
                            </Content>
                            <TrackChangesSettings CanAcceptTrackChanges="False"></TrackChangesSettings>
                        </telerik:RadEditor>

                         <br />
                         <h2>June Notes</h2>
                        <telerik:RadEditor ID="reJNotes" Runat="server" EditModes="Design" Width="100%" Height="290" OnClientCommandExecuting="OnClientCommandExecuting" StripFormattingOnPaste="All">
                            <ExportSettings></ExportSettings>
                            <Tools >
                                <telerik:EditorToolGroup Tag="Formatting">
                                    <telerik:EditorTool Name="Bold" />
                                    <telerik:EditorTool Name="Italic" />
                                    <telerik:EditorTool Name="Underline" />
                                    <telerik:EditorTool Name="SaveAndClose" />
                                </telerik:EditorToolGroup>
                            </Tools>
                            <Content>
                            </Content>
                            <TrackChangesSettings CanAcceptTrackChanges="False"></TrackChangesSettings>
                        </telerik:RadEditor>
                    <div style="display:none">
                        <telerik:RadEditor ID="reClipboardTransfer" Runat="server" EditModes="Design" Width="100%" Height="100" Visible="true" OnClientLoad="doClipboardInit" />
                    </div>

                    <telerik:RadWindow OnClientShow="SetDialogContent" OnClientPageLoad="SetDialogContent" NavigateUrl="Test2.aspx" runat="server" Behaviors="Close"
                        ID="DialogWindow" VisibleStatusbar="false" Width="400px" Modal="true" Height="350px" OnClientBeforeClose="onWindowClose">
                    </telerik:RadWindow>
             
                    <telerik:RadContextMenu runat="server" ID="ctxMenu" EnableRoundedCorners="true" EnableShadows="true" OnClientItemClicked="ctxGoals" >
                        <Items>
                            <telerik:RadMenuItem Text="Delete Selected" ImageUrl="images/RedX_large.gif" Value="deleteSelected" />
                            <telerik:RadMenuItem Text="Copy Selected" ImageUrl="images/clone.png" Value="copySelected"  />
                            <telerik:RadMenuItem Text="Convert to Subgoal" ImageUrl="images/arrow_right.gif" Value="doSubGoal"  />
                            <telerik:RadMenuItem Text="Convert to Goal" ImageUrl="images/arrow_left.gif" Value="doGoal"  />
                        </Items>
                    </telerik:RadContextMenu>
                </asp:Panel>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitBar2" runat="server" />
            <telerik:RadPane ID="EndPane" runat="server" Width="22px" Scrolling="None" >
                    <telerik:RadSlidingZone ID="rslToolbox" runat="server" Width="22px" ClickToOpen="true" SlideDirection="Left" >
                        <telerik:RadSlidingPane ID="rslpClipboard" Title="Clipboard" runat="server" Width="500px" MinWidth="100" TabView="ImageOnly" IconUrl="~/images/clip.png">
                             <telerik:RadListBox ID="lbClipboard" runat="server" Width="100%" CheckBoxes="true"  AutoPostBack="false" EnableViewState="false" OnClientSelectedIndexChanged="doCheck" OnClientContextMenu="clipboardContextMenu" />

                                <telerik:RadContextMenu runat="server" ID="ctxClipboard" EnableRoundedCorners="true" EnableShadows="true" OnClientItemClicked="cpMenuClick"  >
                                    <Items>
                                        <telerik:RadMenuItem Text="Paste Selected" ImageUrl="images/clone.png" Value="pasteSelected" />
                                        <telerik:RadMenuItem Text="Select All" ImageUrl="images/checkbox_checked.png" Value="selectAll" />
                                        <telerik:RadMenuItem Text="Unselect All" ImageUrl="images/checkbox_unchecked.png" Value="unSelectAll" />
                                        <telerik:RadMenuItem Text="Clear Clipboard" ImageUrl="images/RedX_large.gif" Value="clearList" />
                                    </Items>
                                </telerik:RadContextMenu>
                        </telerik:RadSlidingPane>  
                        
                        <telerik:RadSlidingPane ID="rslpGoalCatalog" Title="Goal Catalog" runat="server" Width="500px" MinWidth="100" TabView="ImageOnly" IconUrl="~/images/1463731389_catalog.png">
                            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
                                SelectCommand="SELECT gc_data_uuid, CASE ISNULL(gc_sub_data_sequence, 0) WHEN 0 THEN gc_data_text ELSE '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + gc_data_text END AS gc_data_text FROM gc_data WHERE gc_subject_uuid = @gc_subject_uuid ORDER BY gc_data_sequence, gc_sub_data_sequence">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="cmbGoalCatalog" Name="gc_subject_uuid" PropertyName="SelectedValue" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            Subject: <telerik:RadComboBox runat="server" ID="cmbGoalCatalog" DataSourceID="odsGCCategories" DataTextField="gc_category_name" DataValueField="gc_category_uuid" OnClientSelectedIndexChanged="gcChange" AutoPostBack="true" ></telerik:RadComboBox>
                            <telerik:RadListBox ID="lbGoalCat" runat="server" CheckBoxes="true"  AutoPostBack="false" EnableViewState="false" OnClientSelectedIndexChanged="doCheck" OnClientContextMenu="clipboardContextMenu" Width="100%" DataSourceID="SqlDataSource1" DataTextField="gc_data_text" DataValueField="gc_data_uuid" />
     
                            
                    </telerik:RadSlidingPane>  
                    <telerik:RadSlidingPane ID="rslpRatings" Title="Ratings" runat="server" Width="500px" MinWidth="100" TabView="ImageOnly" IconUrl="~/images/checkbox_checked.png">
                        <telerik:RadGrid ID="rgRatings" runat="server" CellSpacing="0" DataSourceID="sqlCspGrades" GridLines="None">
                            <MasterTableView AutoGenerateColumns="False" DataSourceID="sqlCspGrades" AllowPaging="false" AllowSorting="false">
                                <Columns>
                                    <telerik:GridBoundColumn DataField="grade_text" FilterControlAltText="Filter grade_text column" HeaderText="Rating" ReadOnly="True" SortExpression="grade_text" UniqueName="grade_text">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                    <telerik:GridBoundColumn DataField="grade_desc" FilterControlAltText="Filter grade_desc column" HeaderText="Description" ReadOnly="True" SortExpression="grade_desc" UniqueName="grade_desc">
                                        <ColumnValidationSettings>
                                            <ModelErrorMessage Text="" />
                                        </ColumnValidationSettings>
                                    </telerik:GridBoundColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </telerik:RadSlidingPane>
                </telerik:RadSlidingZone>
            </telerik:RadPane>

        </telerik:RadSplitter>


        <div style="display:none">
            <telerik:RadEditor ID="reTransfer" Runat="server" EditModes="Design" Width="100%" Height="100" Visible="true" />
        </div>

        <div style="display:none">
            <telerik:RadGrid ID="rgComments" runat="server" DataSourceID="sqlCspComments" CellSpacing="0" GridLines="None">
                    <MasterTableView DataKeyNames="iep_uuid" DataSourceID="sqlCspComments" ClientDataKeyNames="iep_uuid, iep_comments, iep_feb_notes, iep_june_notes, isLocked, lockMessage" AutoGenerateColumns="False">
                        <Columns>
                            <telerik:GridBoundColumn DataField="iep_uuid" DataType="System.Guid" FilterControlAltText="Filter iep_uuid column" HeaderText="iep_uuid" ReadOnly="True" SortExpression="iep_uuid" UniqueName="iep_uuid">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="iep_comments" FilterControlAltText="Filter iep_comments column" HeaderText="iep_comments" SortExpression="iep_comments" UniqueName="iep_comments">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="iep_feb_notes" FilterControlAltText="Filter iep_feb_notes column" HeaderText="iep_feb_notes" SortExpression="iep_feb_notes" UniqueName="iep_feb_notes">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="iep_june_notes" FilterControlAltText="Filter iep_june_notes column" HeaderText="iep_june_notes" SortExpression="iep_june_notes" UniqueName="iep_june_notes">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="lockMessage" FilterControlAltText="Filter lockedMessage column" HeaderText="lockedMessage" SortExpression="lockedMessage" UniqueName="lockedMessage">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn DataField="isLocked" FilterControlAltText="Filter isLocked column" HeaderText="isLocked" SortExpression="isLocked" UniqueName="isLocked">
                                <ColumnValidationSettings>
                                    <ModelErrorMessage Text="" />
                                </ColumnValidationSettings>
                            </telerik:GridBoundColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings><ClientEvents OnRowCreated="loadCommentsNotes" /></ClientSettings>
                </telerik:RadGrid>
        </div>
                
    </form>
</body>
</html>
