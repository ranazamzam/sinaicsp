<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="eiep_main.aspx.vb" Inherits="Sinai_eIEP.eiep_main" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .auto-style1 {
            width: 189px;
            height: 98px;
        }
    </style>
    <script type="text/javascript">
        window.onbeforeunload = confirmExit;
        function confirmExit() {
            $find("RadAjaxManager1").ajaxRequest("UnLockIEP");
        }
</script>

    <script type="text/javascript">

        window.history.forward();
        function noBack() { window.history.forward(); }
    </script>
</head>
<body onload="noBack();" onpageshow="if (event.persisted) noBack();" >
    <form id="form1" runat="server">

        <asp:Table ID="Table4" runat="server" Width="100%">
            <asp:TableRow>
                <asp:TableCell Width="50%"><img class="auto-style1" src="images/SINAI%20Schools%20logo.jpeg" /></asp:TableCell>
                <asp:TableCell Width="48%" HorizontalAlign="Right"><a runat="server" href="/admin.aspx" target="_blank" style="display:none" id="admin_link">Admin</a></asp:TableCell>
                <asp:TableCell Width="2%">&nbsp;</asp:TableCell>
            </asp:TableRow>
        </asp:Table>

        <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="30" >

        </asp:ScriptManager>
        <br /><br />
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            var closeIEP;
            var newGoal = false;
            var openIEP;
            var goalTimer;
            var updateTimer;
            var navTimer;
            var tbObject;
            var alertTimer;
            var sessionTimer;

            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

            function EndRequestHandler(sender, args) {
                clearTimeout(sessionTimer);
                //sessionTimer = setTimeout(function () { alert('5 minutes'); }, 300000);

                if (args.get_error() != undefined && args.get_error().httpStatusCode != 0) {
                    args.set_errorHandled(true);
                    alert("An error occurred while performing the requested task.  Please try again or refresh the page.\n\n" + args.get_error().message);
                    if ($find("<%= RadAjaxLoadingPanel1.ClientID%>")) {
                        $find("<%= RadAjaxLoadingPanel1.ClientID%>").hide("<%= RadGrid2.ClientID%>")
                    }
                    //location.reload(true);
                }
            }
            
            function onWindowClose(oWnd, args) {
                var contentWindow = oWnd.get_contentFrame().contentWindow;
                if (contentWindow.isDirty()) {
                    args.set_cancel(!confirm("Are you sure you want to close without saving?"));
                }
            }

            function selectStudent(sender, args) {
                sender.closeDropDown()

                if (sender.get_id().indexOf("ddSubjects") > 0) {
                    var txtSubject = document.getElementById(sender.get_id().replace("ddSubjects", "txtSubject"));

                    if (txtSubject != null) {
                        setTimeout(function () { if (sender.get_selectedText().indexOf('Other (specify subject name below)') >= 0) { txtSubject.style.display = "block"; } else { txtSubject.style.display = "none"; } }, 200);
                    }
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
            
            function isNumber(n) {
                return !isNaN(parseFloat(n)) && isFinite(n);
            }

            function onInsertRow(sender, args) {
                //alert($find("RadGrid1").get_masterTableView().().length)
                if ((document.getElementById("hdnPage").value != $find("RadGrid1").get_masterTableView().get_currentPageIndex() - 1) && (isNumber(document.getElementById("hdnPage").value)) && (document.getElementById("hdnPage").value != 0)) {
                    $find("RadGrid1").get_masterTableView().page(document.getElementById("hdnPage").value);
                    document.getElementById("hdnPage").value = 0;
                }                

                if ((document.getElementById("hdnTest2").value != document.getElementById("hdnTest").value) && (document.getElementById("hdnTest").value == args.get_gridDataItem().getDataKeyValue("iep_uuid"))) {
                    document.getElementById("hdnTest2").value = document.getElementById("hdnTest").value
                    $find("RadGrid1").get_masterTableView().clearSelectedItems();
                    $find("RadGrid1").get_masterTableView().get_dataItems()[args.get_itemIndexHierarchical()].set_selected(true);
                    updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().rebind() }, 200);
                }

            }

            function onClientHiding(sender, args) {
                if (openIEP) {
                    openIEP = false;
                    document.getElementById("iep_list").style["display"] = "none";
                    document.getElementById("iep_details").style["display"] = "block";
                    document.getElementById("lblStudent").focus();
                    $find("MiddlePane1").setScrollPos(0, 0);
                } else if (closeIEP) {
                    closeIEP = false;
                    goalTimer = setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("UnLockIEP"); }, 150);
                    document.getElementById("hdnTest").value = "00000000-0000-0000-0000-000000000000";
                    document.getElementById("hdnTest2").value = "00000000-0000-0000-0000-000000000000";
                    updateTimer = setTimeout(function () { $find("RadGrid1").get_masterTableView().clearSelectedItems(); updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().rebind() }, 200); }, 200);
                    //$find("RadGrid1").get_masterTableView().clearSelectedItems();
                    //$find("RadGrid2").get_masterTableView().rebind();
                    document.getElementById("iep_list").style["display"] = "block";
                    document.getElementById("iep_details").style["display"] = "none";
                }
            }

            function onSelected(sender, args) {
                document.getElementById("hdnTest").value = $find("<%= RadGrid1.ClientID%>").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_uuid");
                document.getElementById("hdnTest2").value = $find("<%= RadGrid1.ClientID%>").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_uuid");
                //updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().rebind() }, 200);
                document.getElementById("lblStudent").innerHTML = $find("<%= RadGrid1.ClientID%>").get_masterTableView().get_selectedItems()[0].get_cell("display_name").innerHTML
                document.getElementById("lblSchool").innerHTML = $find("<%= RadGrid1.ClientID%>").get_masterTableView().get_selectedItems()[0].get_cell("school_name").innerHTML
                document.getElementById("lblTeacher").innerHTML = $find("<%= RadGrid1.ClientID%>").get_masterTableView().get_selectedItems()[0].get_cell("usr_display_name").innerHTML
                document.getElementById("lblSubject").innerHTML = $find("<%= RadGrid1.ClientID%>").get_masterTableView().get_selectedItems()[0].get_cell("iep_subject").innerHTML
                document.getElementById("lblMats").innerHTML = $find("<%= RadGrid1.ClientID%>").get_masterTableView().get_selectedItems()[0].get_cell("iep_materials").innerHTML
                document.getElementById("lblYr").innerHTML = $find("<%= RadGrid1.ClientID%>").get_masterTableView().get_selectedItems()[0].get_cell("iep_year").innerHTML
                
                openIEP = true;
                onClientHiding(null, null);
            }

            function onToolBarClientButtonClicking(sender, args) {
                var button = args.get_item();

                switch (button.get_commandName()) {
                    case "CancelAll":
                        document.getElementById("lblStudent").innerHTML = "";
                        document.getElementById("lblSchool").innerHTML = "";
                        document.getElementById("lblTeacher").innerHTML = "";
                        document.getElementById("lblSubject").innerHTML = "";
                        document.getElementById("lblMats").innerHTML = "";
                        document.getElementById("lblYr").innerHTML = "";

                        if ($find("rgComments").get_masterTableView().get_dataItems().length > 0) {
                            document.getElementById("iep_uuid").value = $find("rgComments").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_uuid");
                            if ($find("<%= rgComments.ClientID%>").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_comments") != $find("<%= reComments.ClientID%>").get_html(true)) {
                                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("iep_comments");
                            }
                            if ($find("<%= rgComments.ClientID%>").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_feb_notes") != $find("<%= reFNotes.ClientID%>").get_html(true)) {
                                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("iep_feb_notes");
                            }
                            if ($find("<%= rgComments.ClientID%>").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_june_notes") != $find("<%= reJNotes.ClientID%>").get_html(true)) {
                                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("iep_june_notes");
                            }
                        }

                        if ($find("RadGrid2").get_masterTableView().get_dataItems().length > 0) {
                            //$find("<%= RadAjaxLoadingPanel1.ClientID%>").show("<%= RadGrid2.ClientID%>");
                            //updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().updateEditedItems(); }, 250);
                            $find("RadGrid2").get_masterTableView().updateEditedItems();
                            closeIEP = true;
                            args.set_cancel(true);
                        } else {
                            closeIEP = true;
                        }
                        break;
                    case "EditAll":
                        //clearTimeout(updateTimer)
                        if ($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_dataItems().length > 0) {
                            //updateTimer = setTimeout(function () { $find("<%= RadGrid2.ClientID%>").get_masterTableView().editAllItems(); updateTimer = setTimeout(function () { if ($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_editItems().length < 1) { $find("<%= RadGrid2.ClientID%>").get_masterTableView().editAllItems(); } }, 5000) }, 100);
                            $find("<%= RadGrid2.ClientID%>").get_masterTableView().editAllItems();
                            //$find("<%= RadAjaxLoadingPanel1.ClientID%>").show("<%= RadGrid2.ClientID%>");
                        }
                        args.set_cancel(true);
                        break;
                    case "Sequence":
                        //clearTimeout(updateTimer)
                        if ($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_editItems().length > 1) {
                            //updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().updateEditedItems(); }, 100);
                            $find("RadGrid2").get_masterTableView().updateEditedItems();
                            //$find("<%= RadAjaxLoadingPanel1.ClientID%>").show("<%= RadGrid2.ClientID%>");
                        }
                        args.set_cancel(true);
                        break;
                    case "ExportPDF":
                        window.open('GeneratePDF.aspx?iep_uuid=' + $find("RadGrid1").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_uuid"))
                        args.set_cancel(true);
                        break;
                    case "NewSubGoal":
                        newGoal = true;
                        if ($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_selectedItems().length > 1) {
                            alert('This action may not be completed when more than one goal is selected.');
                            args.set_cancel(true);
                            return;
                            break;
                        }
                        if ($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_dataItems().length == 0) {
                            alert('This action may not be completed if there are no parent goals.');
                            args.set_cancel(true);
                            return;
                            break;
                        }
                        if ($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_editItems().length > 1) { updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().updateEditedItems(); }, 100); }
                        document.getElementById("iep_uuid").value = document.getElementById("hdnTest").value //$find("RadGrid1").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_data_uuid");
                        document.getElementById("iep_data_uuid").value = "";
                        editMode = 'SubGoal';
                        editorContent = "";
                        $find("<%= DialogWindow.ClientID %>").reload();
                        $find("<%= DialogWindow.ClientID %>").show(); //open RadWindow
                        args.set_cancel(true);
                        break;
                    case "NewGoal":
                        newGoal = true;
                        if ($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_selectedItems().length > 1) {
                            alert('This action may not be completed when more than one goal is selected.');
                            args.set_cancel(true);
                            return;
                            break;
                        }
                        if ($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_editItems().length > 1) { updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().updateEditedItems(); }, 100); }
                        document.getElementById("iep_uuid").value = document.getElementById("hdnTest").value //$find("RadGrid1").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_data_uuid");
                        document.getElementById("iep_data_uuid").value = "";
                        editMode = 'Goal';
                        editorContent = "";
                        $find("<%= DialogWindow.ClientID %>").reload();
                        $find("<%= DialogWindow.ClientID %>").show(); //open RadWindow
                        args.set_cancel(true);
                        break;
                    case "Clone":
                        var list = $find("<%= RadListBox1.ClientID%>")
                        var items = list.get_items();
                        var item = new Telerik.Web.UI.RadListBoxItem();
                        //list.trackChanges();
                        item.set_text(document.getElementById("lblStudent").innerHTML + ' - ' + document.getElementById("lblSubject").innerHTML);
                        item.set_value($find("RadGrid1").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_uuid"));
                        items.add(item);
                        list.commitChanges();
                        args.set_cancel(true);
                        break;
                    case "Copy":
                        var oSelected = $find("<%= RadGrid2.ClientID%>").get_masterTableView().get_selectedItems()

                        if (oSelected.length > 0) {
                            var list = $find("<%= RadListBox1.ClientID%>")
                            var items = list.get_items();
                            //list.trackChanges();
                            for (i = 0; i < oSelected.length; i++) {
                                var item = new Telerik.Web.UI.RadListBoxItem();
                                item.set_text($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_dataItems()[oSelected[i].get_itemIndexHierarchical()].getDataKeyValue("iep_data_text"));
                                item.set_value($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_dataItems()[oSelected[i].get_itemIndexHierarchical()].getDataKeyValue("iep_data_text"));
                                items.add(item);
                            }
                            list.commitChanges();
                        }
                        args.set_cancel(true);
                        break;
                    case "Undo":
                        document.getElementById("iep_uuid").value = document.getElementById("hdnTest").value;
                        $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Undo");
                        //args.set_cancel(true);
                        break;
                }
            }

            function loadCommentsNotes(sender, args) {
                $find("<%= reComments.ClientID%>").set_html($find("<%= rgComments.ClientID%>").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_comments"));
                $find("<%= reFNotes.ClientID%>").set_html($find("<%= rgComments.ClientID%>").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_feb_notes"));
                $find("<%= reJNotes.ClientID%>").set_html($find("<%= rgComments.ClientID%>").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_june_notes"));
                if (document.getElementById("iep_list").style["display"] != "block") {
                    if ($find("<%= rgComments.ClientID%>").get_masterTableView().get_dataItems()[0].getDataKeyValue("isLocked") == "True") {
                        alert($find("<%= rgComments.ClientID%>").get_masterTableView().get_dataItems()[0].getDataKeyValue("lockMessage"));
                        closeIEP = true;
                        onClientHiding(sender, args)
                    } else {
                        goalTimer = setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("LockIEP"); }, 250);
                    }
                }
            }

            function doStudentPDF(sender, args) {
                window.open('GeneratePDF.aspx?student_uuid=' + args.get_commandArgument());
            }

            function SetEditorContent(content, key, eMode) {
                //set content to RadEditor on the mane page from RadWindow
                document.getElementById("iep_uuid").value = document.getElementById("hdnTest").value //$find("RadGrid1").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_uuid");
                $find("<%= RadEditor1.ClientID%>").set_html(content);
                //$find("<%= RadAjaxLoadingPanel1.ClientID%>").show("<%= RadGrid2.ClientID%>")
                switch (eMode) {
                    case "Comments":
                        $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("iep_comments");
                        break;
                    case "FNotes":
                        $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("iep_feb_notes");
                        break;
                    case "JNotes":
                        $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("iep_june_notes");
                        break;
                    case "SubGoal":
                        var list = $find("<%= RadListBox1.ClientID%>")
                        var items = list.get_items();
                        var item = new Telerik.Web.UI.RadListBoxItem();
                        //list.trackChanges();
                        item.set_text(content);
                        item.set_value(content);
                        items.add(item);
                        list.commitChanges();
                        $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("doSubGoal");
                        $find("<%= RadGrid2.ClientID%>").get_masterTableView().rebind();
                        break;
                    default:
                        var list = $find("<%= RadListBox1.ClientID%>")
                        var items = list.get_items(); 
                        var item = new Telerik.Web.UI.RadListBoxItem();
                        //list.trackChanges();
                        item.set_text(content);
                        item.set_value(content);
                        items.add(item);
                        list.commitChanges();
                        $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("InsertUpdateIepData");
                        $find("<%= RadGrid2.ClientID%>").get_masterTableView().rebind();
                }
                //goalTimer = setTimeout(checkStatus, 2500);
            }

            function checkStatus() {
                $find("<%= RadGrid2.ClientID%>").get_masterTableView().rebind();
                $find("<%= RadAjaxLoadingPanel1.ClientID%>").hide("<%= RadGrid2.ClientID%>")
            }

            function SetDialogContent(oWnd) {
                var contentWindow = oWnd.get_contentFrame().contentWindow;
                if (contentWindow && contentWindow.setContent) {
                    window.setTimeout(function () {
                        //pass and set the content from the mane page to RadEditor in RadWindow
                        contentWindow.setContent(editorContent, document.getElementById("hdnTest").value, editMode);
                    }, 100);
                }
            }

            function OnColumnCreated(sender, args) {
                if ($find("RadGrid1").get_masterTableView() == null) {
                    return;
                }

                //if(document.getElementById("hdnTest").value

                if (document.getElementById("hdnTest").value != "00000000-0000-0000-0000-000000000000") {
                    if ($find("RadGrid1").get_masterTableView().get_selectedItems().length == 0) {
                        var dataItems = $find("RadGrid1").get_masterTableView().get_dataItems()

                        for (var i = 0; i < dataItems.length; i++) {
                            if (document.getElementById("hdnTest").value == dataItems[i].getDataKeyValue("iep_uuid")) {
                                dataItems[i].set_selected(true);
                            }
                        }
                    }
                }

                if ($find("RadGrid1").get_masterTableView().get_selectedItems()[0] != undefined) {

                    var column = args.get_column();
                    var d = new Date();
                    var intYear = d.getFullYear() - 2000;

                    if (d.getMonth() < 7) {
                        intYear = intYear - 1
                    }

                    var student_name = $find("<%= RadGrid1.ClientID%>").get_masterTableView().get_selectedItems()[0].get_cell("display_name").innerHTML;
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
            }

            function onCommand(sender, args) {
                switch (args.get_commandName()) {
                    case "EditGoal":
                        editedRow = args.get_commandArgument();
                        if ($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_editItems().length > 1) { updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().updateEditedItems(); }, 100); }
                        editorContent = $find("<%= RadGrid2.ClientID%>").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_text"); //get RadEditor content
                        document.getElementById("iep_data_uuid").value = $find("<%= RadGrid2.ClientID%>").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_uuid")
                        editMode = 'Edit'
                        $find("<%= DialogWindow.ClientID %>").show(); //open RadWindow
                        args.set_cancel(true);
                        break;
                    case "ClipboardCopy":
                        editedRow = args.get_commandArgument();
                        var list = $find("<%= RadListBox1.ClientID%>")
                        var items = list.get_items();
                        var item = new Telerik.Web.UI.RadListBoxItem();
                        //list.trackChanges();
                        item.set_text($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_text"));
                        item.set_value($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_text"));
                        items.add(item);
                        list.commitChanges();
                        args.set_cancel(true);
                        break;
                    case "SubGoal":
                        editedRow = args.get_commandArgument();
                        //updateTimer = setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("HandleSubItem|" + $find("<%= RadGrid2.ClientID%>").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_uuid")); updateTimer = setTimeout(function () { $find("<%= RadGrid2.ClientID%>").get_masterTableView().rebind(); }, 1000); }, 100);
                        $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("HandleSubItem|" + $find("<%= RadGrid2.ClientID%>").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_uuid"));
                        $find("<%= RadGrid2.ClientID%>").get_masterTableView().rebind()
                        args.set_cancel(true);
                        break;
                }
            }
            
            function onRowCreated(sender, args) {
                $find("<%= RadAjaxLoadingPanel1.ClientID%>").hide("<%= RadGrid2.ClientID%>")
                var strGrade
                if ($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_editItems().length == 0) {
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

            function strip(html) {
                var tmp = document.createElement("DIV");
                tmp.innerHTML = html;
                return tmp.textContent || tmp.innerText || "";
            }

            function pasteItems() {
                var checkedItems = $find("<%= RadListBox1.ClientID%>").get_checkedItems();
                var i;
                var strItems = '';

                if (checkedItems.length == 0) {
                    return
                }

                document.getElementById("iep_uuid").value = document.getElementById("hdnTest").value //$find("RadGrid1").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_uuid");
                document.getElementById("iep_data_uuid").value = ""

                for (i = 0; i < checkedItems.length; i++) {
                    strItems = strItems + '~' + checkedItems[i].get_value();
                }

                $find("<%= RadEditor1.ClientID%>").set_html(strItems);
                $find("<%= RadAjaxLoadingPanel1.ClientID%>").show("<%= RadGrid2.ClientID%>")
                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("ClipboardPaste");
                goalTimer = setTimeout(checkStatus, 2500);
            }

            function gc_pasteItem(sender, args) {
                var content = 'gc_' + sender.get_selectedItem().get_value()

                document.getElementById("iep_uuid").value = document.getElementById("hdnTest").value //$find("RadGrid1").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_uuid");
                document.getElementById("iep_data_uuid").value = ""
                $find("<%= RadEditor1.ClientID%>").set_html(content);
                $find("<%= RadAjaxLoadingPanel1.ClientID%>").show("<%= RadGrid2.ClientID%>")
                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("InsertUpdateIepData");
                goalTimer = setTimeout(checkStatus, 2500);
            }

            function onDragStart(sender, args) {

                var isChild = (args.getDataKeyValue("iep_data_parent_uuid") != "");
                var idxDrag = args.get_itemIndexHierarchical();
                var masterTable = $find("<%=RadGrid2.ClientID%>").get_masterTableView();

                if ($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_editItems().length > 1) {
                    args.set_cancel(true);
                }

                if (idxDrag != masterTable.get_dataItems().length - 1) {
                    if (!isChild && (masterTable.get_dataItems()[parseInt(idxDrag) + 1].getDataKeyValue("iep_data_parent_uuid") != "")) {
                        //args.set_cancel(true);
                    }
                }
            }

            function onRowDropping(sender, args) {
                if (sender.get_id() == "<%=RadGrid2.ClientID%>") {
                    var node = args.get_destinationHtmlElement();
                    if (!isChildOf('<%=RadGrid2.ClientID%>', node)) {
                        //args.set_cancel(true);
                    }
                }
                else {
                    //args.set_cancel(true);
                }
            }

            function isChildOf(parentId, element) {
                while (element) {
                    if (element.id && element.id.indexOf(parentId) > -1) {
                        return true;
                    }
                    element = element.parentNode;
                }
                return false;
            }
            
            function btnPrev(sender, args) {
                if ($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_editItems().length > 1) {
                    //updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().updateEditedItems(); }, 100);
                    $find("RadGrid2").get_masterTableView().updateEditedItems();
                    //$find("<%= RadAjaxLoadingPanel1.ClientID%>").show("<%= RadGrid2.ClientID%>");
                }
                var masterTable = $find("<%=RadGrid1.ClientID%>").get_masterTableView()
                masterTable.get_dataItems()
                var newItemIndex = parseInt(masterTable.get_selectedItems()[0].get_itemIndexHierarchical()) - 1;
                if (newItemIndex < 0) {
                    if (masterTable.get_currentPageIndex() > 0) {
                        masterTable.clearSelectedItems();
                        masterTable.page("Prev");
                        document.getElementById("hdnPager").value = "Prev"
                        //updateTimer = setTimeout(function () { masterTable.get_dataItems()[1].set_selected(true); $find("RadGrid2").get_masterTableView().rebind(); }, 200);
                        return;
                    } else {
                        alert("You have reached the first CSP.");
                        return;
                    }
                }
                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("UnLockIEP");
                clearTimeout(updateTimer);
                masterTable.clearSelectedItems();
                masterTable.get_dataItems()[newItemIndex].set_selected(true);
                //updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().rebind() }, 200);
                $find("RadGrid2").get_masterTableView().rebind()
            }

            function btnNext(sender, args) {
                if ($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_editItems().length > 1) {
                    //updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().updateEditedItems(); }, 100);
                    $find("RadGrid2").get_masterTableView().updateEditedItems();
                    //$find("<%= RadAjaxLoadingPanel1.ClientID%>").show("<%= RadGrid2.ClientID%>");
                }
                var masterTable = $find("<%=RadGrid1.ClientID%>").get_masterTableView()
                masterTable.get_dataItems()
                var newItemIndex = parseInt(masterTable.get_selectedItems()[0].get_itemIndexHierarchical()) + 1;
                if (newItemIndex >= masterTable.get_dataItems().length) {
                    if (masterTable.get_currentPageIndex() < masterTable.get_pageCount() - 1) {
                        masterTable.clearSelectedItems();
                        masterTable.page("Next");
                        document.getElementById("hdnPager").value = "Next"
                        //updateTimer = setTimeout(function () { masterTable.get_dataItems()[1].set_selected(true); $find("RadGrid2").get_masterTableView().rebind(); }, 200);
                        return;
                    } else {
                        alert("You have reached the last CSP.");
                        return;
                    }
                }
                $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("UnLockIEP");
                clearTimeout(updateTimer);
                masterTable.clearSelectedItems();
                masterTable.get_dataItems()[newItemIndex].set_selected(true);
                //updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().rebind() }, 200);
                $find("RadGrid2").get_masterTableView().rebind()
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

            function goalRowClick(sender, args) {
                var intItemNum = parseInt(args.get_itemIndexHierarchical()) + 1;

                if ($find("RadGrid2").get_masterTableView().get_dataItems()[intItemNum] != null) {
                    if ($find("RadGrid2").get_masterTableView().get_dataItems()[intItemNum - 1].getDataKeyValue("iep_data_parent_uuid") == "" && $find("RadGrid2").get_masterTableView().get_dataItems()[intItemNum].getDataKeyValue("iep_data_parent_uuid") != "") {
                        while ($find("RadGrid2").get_masterTableView().get_dataItems()[intItemNum].getDataKeyValue("iep_data_parent_uuid") != "") {
                            $find("RadGrid2").get_masterTableView().get_dataItems()[intItemNum].set_selected(true);
                            intItemNum += 1;
                            if ($find("RadGrid2").get_masterTableView().get_dataItems()[intItemNum] == null) {
                                return;
                            }
                        }
                    }
                }
            }

            function cpMenuClick(sender, args) {
                switch (args.get_item().get_value()) {
                    case "pasteSelected":
                        pasteItems();
                        break;
                    case "selectAll":
                        $find("<%= RadListBox1.ClientID%>").get_items().forEach(function (itm) { itm.check(); });
                        break;
                    case "unSelectAll":
                        $find("<%= RadListBox1.ClientID%>").get_items().forEach(function (itm) { itm.uncheck(); });
                        break;
                    case "clearList":
                        $find("<%= RadListBox1.ClientID%>").get_items().clear();
                        break;
                }
            }

            function test(sender, args) {
                if ($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_selectedItems().length == 0)
                    return;

                switch (args.get_item().get_value()) {
                    case "copySelected":
                        var oSelected = $find("<%= RadGrid2.ClientID%>").get_masterTableView().get_selectedItems()

                        if (oSelected.length > 0) {
                            var list = $find("<%= RadListBox1.ClientID%>")
                            var items = list.get_items();
                            oSelected = $find("<%= RadGrid2.ClientID%>").get_masterTableView().get_dataItems();
                            //list.trackChanges();
                            for (i = 0; i < oSelected.length; i++) {
                                if (oSelected[i].get_selected()) {
                                    var item = new Telerik.Web.UI.RadListBoxItem();
                                    var sPrefix = ''

                                    if ($find("<%= RadGrid2.ClientID%>").get_masterTableView().get_dataItems()[oSelected[i].get_itemIndexHierarchical()].get_element().cells[2].innerHTML.indexOf("arrow_left") > -1) {
                                        sPrefix = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' //'\t\t\t\t\t\t\t\t\t\t'
                                    }

                                    item.set_text(sPrefix + $find("<%= RadGrid2.ClientID%>").get_masterTableView().get_dataItems()[oSelected[i].get_itemIndexHierarchical()].getDataKeyValue("iep_data_text"));
                                    item.set_value(sPrefix + $find("<%= RadGrid2.ClientID%>").get_masterTableView().get_dataItems()[oSelected[i].get_itemIndexHierarchical()].getDataKeyValue("iep_data_text"));
                                    items.add(item);
                                }
                            }
                            list.commitChanges();
                        }
                        break;
                    case "deleteSelected":
                        $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("deleteSelected");
                        $find("RadGrid2").get_masterTableView().rebind();
                        break;
                    case "doSubGoal":
                        $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("doBulkSubGoal");
                        $find("RadGrid2").get_masterTableView().rebind();
                        break;
                    case "doGoal":
                        $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("doBulkGoal");
                        $find("RadGrid2").get_masterTableView().rebind();
                        break;
                }
            }

            function RowContextMenu(sender, eventArgs) {
                var menu = $find("<%=ctxMenu.ClientID%>");
                var evt = eventArgs.get_domEvent();
                if (evt.target.tagName == "INPUT" || evt.target.tagName == "A") {
                    return;
                }
                menu.show(evt);
            }

            function clipboardContextMenu(sender, eventArgs) {
                var menu = $find("<%=ctxClipboard.ClientID%>");
                var evt = eventArgs.get_domEvent();
                menu.show(evt);
            }

            function onTabSelecting(sender, args) {

                if (args.get_tab().get_pageViewID()) {
                    args.get_tab().set_postBack(false);
                }
            }

            function onMasterTableLoad(sender, args) {
                if (document.getElementById("hdnPager").value == "Next" && $find("RadGrid1").get_masterTableView().get_dataItems().length >= 1) {
                    $find("RadGrid1").get_masterTableView().get_dataItems()[0].set_selected(true);
                    updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().rebind() }, 200);
                    document.getElementById("hdnPager").value = ""
                }

                if (document.getElementById("hdnPager").value == "Prev" && $find("RadGrid1").get_masterTableView().get_dataItems().length >= 1) {
                    $find("RadGrid1").get_masterTableView().get_dataItems()[$find("RadGrid1").get_masterTableView().get_dataItems().length - 1].set_selected(true);
                    updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().rebind() }, 200);
                    document.getElementById("hdnPager").value = ""
                }
            }

        </script>
    </telerik:RadCodeBlock>
        <telerik:RadWindow OnClientShow="SetDialogContent" OnClientPageLoad="SetDialogContent"
          NavigateUrl="Test2.aspx" runat="server" Behaviors="Close"
          ID="DialogWindow" VisibleStatusbar="false" Width="400px" Modal="true" Height="350px" OnClientBeforeClose="onWindowClose">
        </telerik:RadWindow>
             
        <telerik:RadContextMenu runat="server" ID="ctxMenu" EnableRoundedCorners="true" EnableShadows="true" OnClientItemClicked="test">
            <Items>
                <telerik:RadMenuItem Text="Delete Selected" ImageUrl="images/RedX_large.gif" Value="deleteSelected" />
                <telerik:RadMenuItem Text="Copy Selected" ImageUrl="images/clone.png" Value="copySelected" />
                <telerik:RadMenuItem Text="Convert to Subgoal" ImageUrl="images/arrow_right.gif" Value="doSubGoal" />
                <telerik:RadMenuItem Text="Convert to Goal" ImageUrl="images/arrow_left.gif" Value="doGoal" />
            </Items>
        </telerik:RadContextMenu>

        <telerik:RadContextMenu runat="server" ID="ctxClipboard" EnableRoundedCorners="true" EnableShadows="true" OnClientItemClicked="cpMenuClick">
            <Items>
                <telerik:RadMenuItem Text="Paste Selected" ImageUrl="images/clone.png" Value="pasteSelected" />
                <telerik:RadMenuItem Text="Select All" ImageUrl="images/checkbox_checked.png" Value="selectAll" />
                <telerik:RadMenuItem Text="Unselect All" ImageUrl="images/checkbox_unchecked.png" Value="unSelectAll" />
                <telerik:RadMenuItem Text="Clear Clipboard" ImageUrl="images/RedX_large.gif" Value="clearList" />
            </Items>
        </telerik:RadContextMenu>

        <asp:HiddenField runat="server" ID="iep_uuid" />
        <asp:HiddenField runat="server" ID="iep_selected" Value=""/>
        <asp:HiddenField runat="server" ID="hdnAddNew" value="0"/>
        <asp:HiddenField runat="server" ID="hdnPager" />
        <asp:HiddenField runat="server" ID="hdnPage"/>

      <telerik:RadSplitter ID="RadSplitter1" runat="server" Height="800" BorderStyle="None" Width="100%" >
          <telerik:RadPane ID="MiddlePane1" runat="server" Scrolling="Y">
          
            <asp:Panel runat="server" Height="750" ID="RadEditor" style="display:none">
                <telerik:RadEditor ID="RadEditor1" Runat="server" EditModes="Design" Width="100%" Height="100" Visible="true">
                </telerik:RadEditor>
                <telerik:RadGrid ID="rgComments" runat="server" DataSourceID="SqlDataSource3" CellSpacing="0" GridLines="None">
                    <MasterTableView DataKeyNames="iep_uuid" DataSourceID="SqlDataSource3" ClientDataKeyNames="iep_uuid, iep_comments, iep_feb_notes, iep_june_notes, isLocked, lockMessage" AutoGenerateColumns="False">
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
            </asp:Panel>
        <asp:HiddenField runat="server" ID="iep_data_uuid" Value=""/>
        
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest" RequestQueueSize="30">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="cmbSchools">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="cmbClass"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="cmbSchools"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmbClass">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="cmbClass"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmbTeachers">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="cmbTeachers"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="cmbStudents">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="cmbStudents"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid1" >
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="hdnTest" />
                    <telerik:AjaxUpdatedControl ControlID="hdnPage" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="rgComments"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid2" >
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid3"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="rgComments"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="tbGoalCatalogue">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="tbGoalCatalogue"></telerik:AjaxUpdatedControl>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1" LoadingPanelID="LoadingPanel1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadMultiPage1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadMultiPage1"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
           </AjaxSettings>
        </telerik:RadAjaxManager>

        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" Skin="Default" OnClientHiding="onClientHiding"></telerik:RadAjaxLoadingPanel>
            
        <asp:Panel runat="server" Height="750" ID="iep_list" >
            <asp:table runat="server" CellPadding="2" CellSpacing="2">
                <asp:TableRow>
                    <asp:TableCell Width="20%"></asp:TableCell>
                    <asp:TableCell Width="12%">Teacher</asp:TableCell>
                    <asp:TableCell Width="12%">Student</asp:TableCell>
                    <asp:TableCell Width="12%">Subject</asp:TableCell>
                    <asp:TableCell Width="10%">Class</asp:TableCell>
                    <asp:TableCell Width="14%"><asp:Label runat="server" ID="lblSchoolFilter">School</asp:Label></asp:TableCell>
                    <asp:TableCell Width="20%"></asp:TableCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableCell></asp:TableCell>
                    <asp:TableCell>
                        <telerik:RadComboBox OnItemsRequested="cmbTeachers_ItemsRequested" runat="server" ID="cmbTeachers" ShowToggleImage ="false" EnableLoadOnDemand="true" AutoPostBack="true" MarkFirstMatch="true" Height="100px" Width="200px">
                            <Items>
                                <telerik:RadComboBoxItem Value="00000000-0000-0000-0000-000000000000" Selected="true" />
                            </Items>
                        </telerik:RadComboBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <telerik:RadComboBox OnItemsRequested="cmbStudents_ItemsRequested" runat="server" ID="cmbStudents" ShowToggleImage ="false" EnableLoadOnDemand="true" AutoPostBack="true" MarkFirstMatch="true" Height="100px" Width="200px">
                            <Items>
                                <telerik:RadComboBoxItem Value="00000000-0000-0000-0000-000000000000" Selected="true" />
                            </Items>
                        </telerik:RadComboBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <telerik:RadComboBox OnItemsRequested="cmbSubjects_ItemsRequested" runat="server" ID="cmbSubjects" ShowToggleImage ="false" EnableLoadOnDemand="true" AutoPostBack="true" MarkFirstMatch="true" Height="100px" Width="200px">
                            <Items>
                                <telerik:RadComboBoxItem Value="00000000-0000-0000-0000-000000000000" Selected="true" />
                            </Items>
                        </telerik:RadComboBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <telerik:RadComboBox runat="server" ID="cmbClass" AutoPostBack="true" Height="100px" DataSourceID="SqlDataSource6" DataTextField="current_placement" DataValueField="val">
                            <Items>
                                <telerik:RadComboBoxItem Value="0" Selected="true" />
                            </Items>
                        </telerik:RadComboBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <telerik:RadComboBox runat="server" ID="cmbSchools" AutoPostBack="true" Height="100px" DataSourceID="SqlDataSource4" DataTextField="school_name" DataValueField="school_id">
                            <Items>
                                <telerik:RadComboBoxItem Value="0" Selected="true" />
                            </Items>
                        </telerik:RadComboBox>
                    </asp:TableCell>
                    <asp:TableCell>
                        <asp:CheckBox ID="chkDeleted" runat="server" AutoPostBack="true" Text="Show Deleted: " TextAlign="Left" Checked="false" Visible="false" />
                    </asp:TableCell>
                </asp:TableRow>
            </asp:table>
            
            <telerik:RadGrid ID="RadGrid1" runat="server" AllowPaging="True" AllowSorting="False" OnItemInserted="RadGrid1_ItemInserted" OnUpdateCommand="RadGrid1_UpdateCommand" OnInsertCommand="RadGrid1_InsertCommand" CellSpacing="0" OnDeleteCommand="RadGrid1_DeleteCommand"
                GridLines="None" PageSize="20" DataSourceID="SqlDataSource1" ShowStatusBar="True" >
        <MasterTableView CommandItemDisplay="Top" AutoGenerateColumns="False" DataKeyNames="iep_uuid, usr_uuid, school_id" DataSourceID="SqlDataSource1" ClientDataKeyNames="iep_uuid, school_id" CommandItemSettings-AddNewRecordText="New CSP">
            <GroupHeaderTemplate>
                <asp:Label runat="server" ID="Label1" Text='<%# Eval("display_name")%>' />
                <telerik:RadButton runat="server" OnClientClicked="doStudentPDF" CommandArgument='<%# Eval("student_uuid")%>' Text="PDF" />
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

            <EditFormSettings UserControlName="eiep_header.ascx" EditFormType="WebUserControl">
                <EditColumn UniqueName="EditCommandColumn1" ></EditColumn>
            </EditFormSettings>
        </MasterTableView>

        <ClientSettings EnablePostBackOnRowClick="true" >
            <Selecting AllowRowSelect="true" />
            <ClientEvents OnRowSelected="onSelected" OnRowCreated="onInsertRow" OnMasterTableViewCreated="onMasterTableLoad" />
        </ClientSettings>
        <PagerStyle Mode="NextPrev" />
    </telerik:RadGrid>
            
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="getIepList_New @usr_login, @student_uuid, @teacher_uuid, @school_id, @is_active, @subject_text, @class_placement">
        <SelectParameters>
            <asp:ControlParameter ControlID="usr_login" Name="usr_login" PropertyName="Value" />
            <asp:ControlParameter ControlID="cmbTeachers" Name="teacher_uuid" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="cmbStudents" Name="student_uuid" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="cmbSchools" Name="school_id" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="cmbClass" Name="class_placement" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="cmbSubjects" Name="subject_text" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="chkDeleted" Name="is_active" PropertyName="Checked" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="getSchoolList @usr_login">
        <SelectParameters>
            <asp:ControlParameter ControlID="usr_login" Name="usr_login" PropertyName="Value" />
        </SelectParameters>
    </asp:SqlDataSource>
                
    <asp:HiddenField ID="usr_login" runat="server" />

        </asp:Panel>
              <asp:HiddenField runat="server" ID="hdnTest"  />
              <asp:HiddenField runat="server" ID="hdnTest2" />
              <!--<SelectParameters><asp:ControlParameter ControlID="RadGrid1" Name="iep_uuid" PropertyName="SelectedValue" /></SelectParameters> -->
    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT * FROM vw_get_iep_data WHERE iep_uuid = @iep_uuid ORDER BY iep_data_sequence, iep_sub_data_sequence">
        <SelectParameters><asp:ControlParameter ControlID="hdnTest" Name="iep_uuid" PropertyName="Value" /></SelectParameters>
    </asp:SqlDataSource>

        <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT iep_uuid, iep_comments, iep_feb_notes, iep_june_notes, locked_by, dbo.lockMessage(iep_uuid) AS lockMessage, dbo.isLocked(iep_uuid, @usr_login) AS isLocked FROM iep WHERE iep_uuid = @iep_uuid">
            <SelectParameters>
                <asp:ControlParameter ControlID="hdnTest" Name="iep_uuid" PropertyName="Value" />
                <asp:ControlParameter ControlID="usr_login" Name="usr_login" PropertyName="Value" />
            </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSource5" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT NULL AS grade_id, @school_id AS school_id, '' AS grade_text, '' AS grade_desc UNION SELECT grade_id, school_id, grade_text, grade_desc FROM grades WHERE school_id = @school_id">
        <SelectParameters><asp:ControlParameter  ControlID="RadGrid1" Name="school_id" PropertyName="SelectedValues[school_id]"  /></SelectParameters> 
    </asp:SqlDataSource>
              
    <asp:SqlDataSource ID="SqlDataSource6" runat="server" ConnectionString="server=dsrcvjfaa9.database.windows.net;database=eiep;uid=eiep_admin;password=Klonimus55;"
        SelectCommand="SELECT '' AS current_placement, '0' AS val UNION SELECT DISTINCT current_placement, current_placement FROM students WHERE is_active = 1 AND (school_id = @school_id OR @school_id = 0) AND (school_id = dbo.getUserSchoolId(@usr_login) OR dbo.getUserSchoolId(@usr_login) IS NULL) AND NOT current_placement IS NULL">
        <SelectParameters>
            <asp:ControlParameter ControlID="cmbSchools" Name="school_id" PropertyName="SelectedValue" />
            <asp:ControlParameter ControlID="usr_login" Name="usr_login" PropertyName="Value" />
        </SelectParameters> 
    </asp:SqlDataSource>

        <asp:ObjectDataSource ID="ObjDataSource1" runat="server" TypeName="DDLists" SelectMethod="getGrades" >
            <SelectParameters>
                <asp:ControlParameter  ControlID="RadGrid1" Name="intSchoolId" PropertyName="SelectedValues[school_id]"  />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ObjDataSource2" runat="server" TypeName="DDLists" SelectMethod="getInitDates" ></asp:ObjectDataSource>

         <asp:Panel runat="server" Height="750" ID="iep_details"  style="display:none">
             
             <table id="Table3" cellspacing="1" cellpadding="1" width="300" border="0">
                <tr>
                    <td width="100">
                        <strong>Student:</strong>
                    </td>
                    <td>
                        <asp:Label ID="lblStudent" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>School:</strong>
                    </td>
                    <td>
                        <asp:Label ID="lblSchool" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Teacher:</strong>
                    </td>
                    <td>
                        <asp:Label ID="lblTeacher" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Subject:</strong>
                    </td>
                    <td>
                        <asp:Label ID="lblSubject" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Materials:</strong>
                    </td>
                    <td>
                       <asp:Label ID="lblMats" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <strong>Year:</strong>
                    </td>
                    <td>
                       <asp:Label ID="lblYr" runat="server" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;</td>
                    <td>
                        &nbsp;</td>
                </tr>
                 <tr>
                    <td><telerik:RadButton ID="rbtnPrev" runat="server" Text="Prev" OnClientClicked="btnPrev" AutoPostBack="false" /></td>
                    <td><telerik:RadButton ID="rbtnNext" runat="server" Text="Next" OnClientClicked="btnNext" AutoPostBack="false" /></td>
                </tr>
            </table>
                          <telerik:RadGrid ID="RadGrid2" runat="server" ClientSettings-ClientEvents-OnRowContextMenu="RowContextMenu" AllowMultiRowSelection="true" AllowMultiRowEdit="true" OnUpdateCommand="RadGrid2_UpdateCommand" OnItemCreated="RadGrid2_ItemCreated" OnDeleteCommand="RadGrid2_DeleteCommand" CellSpacing="1" GridLines="Both" DataSourceID="SqlDataSource2" OnRowDrop="RadGrid2_RowDrop">
                              <AlternatingItemStyle BackColor="White" />
                            
        <MasterTableView EditMode="InPlace" CommandItemDisplay="Top" AutoGenerateColumns="False" DataKeyNames="iep_data_uuid, iep_data_parent_uuid, iep_data_text" ClientDataKeyNames="iep_uuid, iep_data_uuid, iep_data_parent_uuid, iep_data_text" CommandItemSettings-AddNewRecordText="New Goal" DataSourceID="SqlDataSource2" >
            <Columns>
                <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn">
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
                <telerik:GridDropDownColumn UniqueName="iep_date_initiated" HeaderText="Date Initiated" ColumnEditorID="GridDropDownListColumnEditor4" HeaderStyle-HorizontalAlign="Center" DataField="iep_date_initiated" ListTextField="init_date" ListValueField="init_date" DataSourceID="ObjDataSource2">
                    <ItemStyle Width="60px"></ItemStyle>
                </telerik:GridDropDownColumn>
                <telerik:GridDropDownColumn UniqueName="iep_data_grade_1_id" HeaderText="Grade" ColumnEditorID="GridDropDownListColumnEditor1" HeaderStyle-HorizontalAlign="Center" ListTextField="grade_desc" ListValueField="grade_id" DataSourceID="SqlDataSource5" DataField="iep_data_grade_1_id">
                    <ItemStyle Width="50px"></ItemStyle>
                </telerik:GridDropDownColumn>
                <telerik:GridDropDownColumn UniqueName="iep_data_grade_2_id" HeaderText="Grade" ColumnEditorID="GridDropDownListColumnEditor2" HeaderStyle-HorizontalAlign="Center" ListTextField="grade_desc" ListValueField="grade_id" DataSourceID="SqlDataSource5" DataField="iep_data_grade_2_id">
                    <ItemStyle Width="50px"></ItemStyle>
                </telerik:GridDropDownColumn>
                <telerik:GridDropDownColumn UniqueName="iep_data_grade_3_id" HeaderText="Grade" ColumnEditorID="GridDropDownListColumnEditor3" HeaderStyle-HorizontalAlign="Center" ListTextField="grade_desc" ListValueField="grade_id" DataSourceID="SqlDataSource5" DataField="iep_data_grade_3_id">
                    <ItemStyle Width="50px"></ItemStyle>
                </telerik:GridDropDownColumn>
            </Columns>
            <CommandItemTemplate>
                <telerik:RadToolBar ID="RadToolBar1" runat="server" OnClientButtonClicking="onToolBarClientButtonClicking" AutoPostBack="true" >
                    <Items>
                        <telerik:RadToolBarButton runat="server" CommandName="NewGoal" Text="New Goal" ImageUrl="images/AddRecord.gif"></telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="NewSubGoal" Text="New Sub Goal" ImageUrl="images/AddRecord.gif"></telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="ExportPDF" Text="PDF" ImageUrl="images/pdf.png"></telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="EditAll" Text="Initiate/Rate Mode" ImageUrl="images/Edit.gif"></telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Sequence" Text="Position Mode" ImageUrl="images/seq.png"></telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Clone" Text="Clone" ImageUrl="images/clone.png"></telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="Undo" Text="Undo" ImageUrl="images/undo.png"></telerik:RadToolBarButton>
                        <telerik:RadToolBarButton runat="server" CommandName="CancelAll" Text="Close" ImageUrl="images/RedX.gif"></telerik:RadToolBarButton>
                    </Items>
                </telerik:RadToolBar>
            </CommandItemTemplate>
        </MasterTableView>
        <ClientSettings EnablePostBackOnRowClick="false" AllowRowsDragDrop="True" >
            <ClientEvents OnCommand="onCommand" OnRowCreated="onRowCreated" OnColumnCreated="OnColumnCreated" OnRowDragStarted="onDragStart" OnRowSelecting="goalRowClick"  />
            <Selecting AllowRowSelect="True" EnableDragToSelectRows="false"></Selecting>
        </ClientSettings>
                              <ItemStyle BackColor="White" />
        <PagerStyle Mode="NextPrev" />
    </telerik:RadGrid>

             <br />
             <h2>Comments</h2>
            <telerik:RadEditor ID="reComments" Runat="server" EditModes="Design" Width="100%" Height="290" OnClientCommandExecuting="OnClientCommandExecuting" StripFormattingOnPaste="All">
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

             <telerik:GridDropDownListColumnEditor ID="GridDropDownListColumnEditor1" runat="server" DropDownStyle-Width="50px"/>
             <telerik:GridDropDownListColumnEditor ID="GridDropDownListColumnEditor2" runat="server" DropDownStyle-Width="50px"/>
             <telerik:GridDropDownListColumnEditor ID="GridDropDownListColumnEditor3" runat="server" DropDownStyle-Width="50px"/>
             <telerik:GridDropDownListColumnEditor ID="GridDropDownListColumnEditor4" runat="server" DropDownStyle-Width="60px"/>
                        
                    </asp:Panel>
    </telerik:RadPane>       
          <telerik:RadSplitBar ID="RadSplitBar2" runat="server">
          </telerik:RadSplitBar>
          <telerik:RadPane ID="EndPane" runat="server" Width="22px" Scrolling="None">
              <telerik:RadSlidingZone ID="Radslidingzone1" runat="server" Width="22px" ClickToOpen="true"
                    SlideDirection="Left">
                    <telerik:RadSlidingPane ID="Radslidingpane5" Title="Clipboard" runat="server" Width="500px" MinWidth="100">
                         <telerik:RadListBox ID="RadListBox1" runat="server" Width="100%" CheckBoxes="true" Skin="Default" OnClientSelectedIndexChanged="doCheck" OnClientContextMenu="clipboardContextMenu">
                           
                        </telerik:RadListBox>
                         
                    </telerik:RadSlidingPane>
                    <telerik:RadSlidingPane ID="Radslidingpane1" Title="Goal Catalogue" runat="server" Width="500px" MinWidth="100">
                         
                        <div>
                            <telerik:RadTabStrip runat="server" ID="tbGoalCatalogue" Align="Justify" OnClientTabSelecting="onTabSelecting" MultiPageID="RadMultiPage1" Skin="Default" SelectedIndex="0" OnTabClick="tbGoalCatalogue_TabClick">
                                <Tabs>
                                    <telerik:RadTab PageViewID="8B9C57E7-CDA0-47E5-88EF-F511974462F9" Text="Hebrew Reading"></telerik:RadTab>
                                    <telerik:RadTab PageViewID="8850A5A0-62F5-4061-910A-F7B6C3E0EF8D" Text="Fundations - Level 1"></telerik:RadTab>
                                    <telerik:RadTab PageViewID="6FDC4EBA-9787-4780-AED0-4C91F1527E3A" Text="Fundations - Level 2" IsBreak="true"></telerik:RadTab>
                                    <telerik:RadTab PageViewID="597D3E03-A1DD-4E31-B2E0-15C9C3B9F18B" Text="Chumash - Basic Level"></telerik:RadTab>
                                    <telerik:RadTab PageViewID="471BD7B5-D7FD-4B43-BA92-A528FE15F06B" Text="Chumash - Level 2"></telerik:RadTab>
                                </Tabs>
                            </telerik:RadTabStrip>
                            <telerik:RadMultiPage runat="server" ID="RadMultiPage1" SelectedIndex="0" OnPageViewCreated="RadMultiPage1_PageViewCreated">
                                
                            </telerik:RadMultiPage>
                        </div>
                         
                    </telerik:RadSlidingPane>
                  <telerik:RadSlidingPane ID="Radslidingpane2" Title="Ratings" runat="server" Width="500px" MinWidth="100">
                        
                         
                      <telerik:RadGrid ID="RadGrid3" runat="server" CellSpacing="0" DataSourceID="SqlDataSource5" GridLines="None">
                          <MasterTableView AutoGenerateColumns="False" DataSourceID="SqlDataSource5" AllowPaging="false" AllowSorting="false">
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
        

    </form>

</body>

</html>
