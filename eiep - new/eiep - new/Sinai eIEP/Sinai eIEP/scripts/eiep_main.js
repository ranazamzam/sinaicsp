var closeIEP;
var newGoal = false;
var openIEP;
var goalTimer;
var updateTimer;
var navTimer;
var tbObject;
var alertTimer;

Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
function EndRequestHandler(sender, args) {
    if (args.get_error() != undefined) {
        args.set_errorHandled(true);
        alert("An error occurred while performing the requested task.  Please try again or refresh the page.\n\n" + args.get_error());
        if ($find("RadAjaxLoadingPanel1")) {
            $find("RadAjaxLoadingPanel1").hide("RadGrid2")
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

function onInsertRow(sender, args) {
    if ((document.getElementById("hdnTest2").value != document.getElementById("hdnTest").value) && (document.getElementById("hdnTest").value == args.get_gridDataItem().getDataKeyValue("iep_uuid"))) {
        alert(1)
        $find("RadGrid1").get_masterTableView().page(6);
        alert(2)
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
    } else if (closeIEP) {
        closeIEP = false;
        goalTimer = setTimeout(function () { $find("RadAjaxManager1").ajaxRequest("UnLockIEP"); }, 150);
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
    document.getElementById("hdnTest").value = $find("RadGrid1").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_uuid");
    document.getElementById("hdnTest2").value = $find("RadGrid1").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_uuid");
    //updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().rebind() }, 200);
    document.getElementById("lblStudent").innerHTML = $find("RadGrid1").get_masterTableView().get_selectedItems()[0].get_cell("display_name").innerHTML
    document.getElementById("lblSchool").innerHTML = $find("RadGrid1").get_masterTableView().get_selectedItems()[0].get_cell("school_name").innerHTML
    document.getElementById("lblTeacher").innerHTML = $find("RadGrid1").get_masterTableView().get_selectedItems()[0].get_cell("usr_display_name").innerHTML
    document.getElementById("lblSubject").innerHTML = $find("RadGrid1").get_masterTableView().get_selectedItems()[0].get_cell("iep_subject").innerHTML
    document.getElementById("lblMats").innerHTML = $find("RadGrid1").get_masterTableView().get_selectedItems()[0].get_cell("iep_materials").innerHTML
    document.getElementById("lblYr").innerHTML = $find("RadGrid1").get_masterTableView().get_selectedItems()[0].get_cell("iep_year").innerHTML
    openIEP = true;
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
                if ($find("rgComments").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_comments") != $find("reComments").get_html(true)) {
                    $find("RadAjaxManager1").ajaxRequest("iep_comments");
                }
                if ($find("rgComments").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_feb_notes") != $find("reFNotes").get_html(true)) {
                    $find("RadAjaxManager1").ajaxRequest("iep_feb_notes");
                }
                if ($find("rgComments").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_june_notes") != $find("reJNotes").get_html(true)) {
                    $find("RadAjaxManager1").ajaxRequest("iep_june_notes");
                }
            }

            if ($find("RadGrid2").get_masterTableView().get_dataItems().length > 0) {
                //$find("RadAjaxLoadingPanel1").show("RadGrid2");
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
            if ($find("RadGrid2").get_masterTableView().get_dataItems().length > 0) {
                //updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().editAllItems(); updateTimer = setTimeout(function () { if ($find("RadGrid2").get_masterTableView().get_editItems().length < 1) { $find("RadGrid2").get_masterTableView().editAllItems(); } }, 5000) }, 100);
                $find("RadGrid2").get_masterTableView().editAllItems();
                //$find("RadAjaxLoadingPanel1").show("RadGrid2");
            }
            args.set_cancel(true);
            break;
        case "Sequence":
            //clearTimeout(updateTimer)
            if ($find("RadGrid2").get_masterTableView().get_editItems().length > 1) {
                //updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().updateEditedItems(); }, 100);
                $find("RadGrid2").get_masterTableView().updateEditedItems();
                //$find("RadAjaxLoadingPanel1").show("RadGrid2");
            }
            args.set_cancel(true);
            break;
        case "ExportPDF":
            window.open('GeneratePDF.aspx?iep_uuid=' + $find("RadGrid1").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_uuid"))
            args.set_cancel(true);
            break;
        case "NewGoal":
            newGoal = true;
            if ($find("RadGrid2").get_masterTableView().get_editItems().length > 1) { updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().updateEditedItems(); }, 100); }
            document.getElementById("iep_uuid").value = $find("RadGrid1").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_data_uuid");
            document.getElementById("iep_data_uuid").value = "";
            editorContent = "";
            $find("DialogWindow").reload();
            $find("DialogWindow").show(); //open RadWindow
            args.set_cancel(true);
            break;
        case "Clone":
            var list = $find("RadListBox1")
            var items = list.get_items();
            var item = new Telerik.Web.UI.RadListBoxItem();
            item.set_text(document.getElementById("lblStudent").innerHTML + ' - ' + document.getElementById("lblSubject").innerHTML);
            item.set_value($find("RadGrid1").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_uuid"));
            items.add(item);
            list.commitChanges();
            args.set_cancel(true);
            break;
    }
}

function loadCommentsNotes(sender, args) {
    $find("reComments").set_html($find("rgComments").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_comments"));
    $find("reFNotes").set_html($find("rgComments").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_feb_notes"));
    $find("reJNotes").set_html($find("rgComments").get_masterTableView().get_dataItems()[0].getDataKeyValue("iep_june_notes"));
    if (document.getElementById("iep_list").style["display"] != "block") {
        if ($find("rgComments").get_masterTableView().get_dataItems()[0].getDataKeyValue("isLocked") == "True") {
            alert($find("rgComments").get_masterTableView().get_dataItems()[0].getDataKeyValue("lockMessage"));
            closeIEP = true;
            onClientHiding(sender, args)
        } else {
            goalTimer = setTimeout(function () { $find("RadAjaxManager1").ajaxRequest("LockIEP"); }, 250);
        }
    }
}

function doStudentPDF(sender, args) {
    window.open('GeneratePDF.aspx?student_uuid=' + args.get_commandArgument());
}

function SetEditorContent(content, key, eMode) {
    //set content to RadEditor on the mane page from RadWindow
    document.getElementById("iep_uuid").value = $find("RadGrid1").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_uuid");
    $find("RadEditor1").set_html(content);
    //$find("RadAjaxLoadingPanel1").show("RadGrid2")
    switch (eMode) {
        case "Comments":
            $find("RadAjaxManager1").ajaxRequest("iep_comments");
            break;
        case "FNotes":
            $find("RadAjaxManager1").ajaxRequest("iep_feb_notes");
            break;
        case "JNotes":
            $find("RadAjaxManager1").ajaxRequest("iep_june_notes");
            break;
        default:
            var list = $find("RadListBox1")
            var items = list.get_items();
            var item = new Telerik.Web.UI.RadListBoxItem();
            item.set_text(content);
            item.set_value(content);
            items.add(item);
            list.commitChanges();
            $find("RadAjaxManager1").ajaxRequest("InsertUpdateIepData");
            $find("RadGrid2").get_masterTableView().rebind();
    }
    //goalTimer = setTimeout(checkStatus, 2500);
}

function checkStatus() {
    $find("RadGrid2").get_masterTableView().rebind();
    $find("RadAjaxLoadingPanel1").hide("RadGrid2")
}

function SetDialogContent(oWnd) {
    var contentWindow = oWnd.get_contentFrame().contentWindow;
    if (contentWindow && contentWindow.setContent) {
        window.setTimeout(function () {
            //pass and set the content from the mane page to RadEditor in RadWindow
            contentWindow.setContent(editorContent, $find("RadGrid1").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_uuid"), editMode);
        }, 100);
    }
}

function OnColumnCreated(sender, args) {
    if ($find("RadGrid1").get_masterTableView() == null) {
        return;
    }

    if ($find("RadGrid1").get_masterTableView().get_selectedItems()[0] != undefined) {

        var column = args.get_column();
        var d = new Date();
        var intYear = d.getFullYear() - 2000;

        if (d.getMonth() < 7) {
            intYear = intYear - 1
        }

        var student_name = $find("RadGrid1").get_masterTableView().get_selectedItems()[0].get_cell("display_name").innerHTML;
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
            editorContent = $find("RadGrid2").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_text"); //get RadEditor content
            document.getElementById("iep_data_uuid").value = $find("RadGrid2").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_uuid")
            editMode = 'Edit'
            $find("DialogWindow").show(); //open RadWindow
            args.set_cancel(true);
            break;
        case "ClipboardCopy":
            editedRow = args.get_commandArgument();
            var list = $find("RadListBox1")
            var items = list.get_items();
            var item = new Telerik.Web.UI.RadListBoxItem();
            item.set_text($find("RadGrid2").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_text"));
            item.set_value($find("RadGrid2").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_text"));
            items.add(item);
            list.commitChanges();
            args.set_cancel(true);
            break;
        case "SubGoal":
            editedRow = args.get_commandArgument();
            //updateTimer = setTimeout(function () { $find("RadAjaxManager1").ajaxRequest("HandleSubItem|" + $find("RadGrid2").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_uuid")); updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().rebind(); }, 1000); }, 100);
            $find("RadAjaxManager1").ajaxRequest("HandleSubItem|" + $find("RadGrid2").get_masterTableView().get_dataItems()[editedRow].getDataKeyValue("iep_data_uuid"));
            $find("RadGrid2").get_masterTableView().rebind()
            args.set_cancel(true);
            break;
    }
}

function onRowCreated(sender, args) {
    $find("RadAjaxLoadingPanel1").hide("RadGrid2")
    if (args.get_gridDataItem().getDataKeyValue("iep_data_parent_uuid") != "") {
        args.get_gridDataItem().get_element().cells[5].innerHTML = '&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;' + args.get_gridDataItem().get_element().cells[5].innerHTML
    } else if (args.get_itemIndexHierarchical() == "0") {
        args.get_gridDataItem().get_element().cells[2].innerHTML = "&nbsp;"
    }
}

function pasteItem(sender, args) {
    var content = $find("RadListBox1").get_selectedItem().get_value()

    document.getElementById("iep_uuid").value = $find("RadGrid1").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_uuid");
    $find("RadEditor1").set_html(content);
    $find("RadAjaxLoadingPanel1").show("RadGrid2")
    $find("RadAjaxManager1").ajaxRequest("InsertUpdateIepData");
    goalTimer = setTimeout(checkStatus, 2500);
}


function gc_pasteItem(sender, args) {
    var content = 'gc_' + sender.get_selectedItem().get_value()

    document.getElementById("iep_uuid").value = $find("RadGrid1").get_masterTableView().get_selectedItems()[0].getDataKeyValue("iep_uuid");
    $find("RadEditor1").set_html(content);
    $find("RadAjaxLoadingPanel1").show("RadGrid2")
    $find("RadAjaxManager1").ajaxRequest("InsertUpdateIepData");
    goalTimer = setTimeout(checkStatus, 2500);
}

function onDragStart(sender, args) {

    var isChild = (args.getDataKeyValue("iep_data_parent_uuid") != "");
    var idxDrag = args.get_itemIndexHierarchical();
    var masterTable = $find("RadGrid2").get_masterTableView();

    if (idxDrag != masterTable.get_dataItems().length - 1) {
        if (!isChild && (masterTable.get_dataItems()[parseInt(idxDrag) + 1].getDataKeyValue("iep_data_parent_uuid") != "")) {
            args.set_cancel(true);
        }
    }
}

function onRowDropping(sender, args) {
    if (sender.get_id() == "RadGrid2") {
        var node = args.get_destinationHtmlElement();
        if (!isChildOf('RadGrid2', node)) {
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
    if ($find("RadGrid2").get_masterTableView().get_editItems().length > 1) {
        //updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().updateEditedItems(); }, 100);
        $find("RadGrid2").get_masterTableView().updateEditedItems();
        //$find("RadAjaxLoadingPanel1").show("RadGrid2");
    }
    var masterTable = $find("RadGrid1").get_masterTableView()
    masterTable.get_dataItems()
    var newItemIndex = parseInt(masterTable.get_selectedItems()[0].get_itemIndexHierarchical()) - 1;
    if (newItemIndex < 0) {
        alert("You have reached the first CSP on this page.  Please close the CSP and go to the previous page.");
        return;
    }
    $find("RadAjaxManager1").ajaxRequest("UnLockIEP");
    clearTimeout(updateTimer);
    masterTable.clearSelectedItems();
    masterTable.get_dataItems()[newItemIndex].set_selected(true);
    //updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().rebind() }, 200);
    $find("RadGrid2").get_masterTableView().rebind()
}

function btnNext(sender, args) {
    if ($find("RadGrid2").get_masterTableView().get_editItems().length > 1) {
        //updateTimer = setTimeout(function () { $find("RadGrid2").get_masterTableView().updateEditedItems(); }, 100);
        $find("RadGrid2").get_masterTableView().updateEditedItems();
        //$find("RadAjaxLoadingPanel1").show("RadGrid2");
    }
    var masterTable = $find("RadGrid1").get_masterTableView()
    masterTable.get_dataItems()
    var newItemIndex = parseInt(masterTable.get_selectedItems()[0].get_itemIndexHierarchical()) + 1;
    if (newItemIndex >= masterTable.get_dataItems().length) {
        alert("You have reached the last CSP on this page.  Please close the CSP and go to the next page.");
        return;
    }
    $find("RadAjaxManager1").ajaxRequest("UnLockIEP");
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

function onTabSelecting(sender, args) {

    if (args.get_tab().get_pageViewID()) {
        args.get_tab().set_postBack(false);
    }
}