/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/*jslint browser: true*/
/*global $, jQuery, alert, confirm*/

var savingBusinessRule = false;

function setMode(mode) {
    "use strict";
    var editDIV, treeDIV, saveAddBusinessRuleButton, saveEditBusinessRuleButton, searchDIV;
    editDIV = $("#BusinessRuleEditDIV");
    treeDIV = $("#BusinessRuleTreeDIV");
    saveAddBusinessRuleButton = $('input[id$=btnSaveAddBusinessRule]');
    saveEditBusinessRuleButton = $('input[id$=btnSaveEditBusinessRule]');
    searchDIV = $(".searchDIV");
    if (mode === "add") {
        editDIV.show();
        treeDIV.hide();
        searchDIV.hide();
        $('input[id$=hdnBusinessRuleID]').val("");
        $("#PageTitle").text("Add Business Rule");
        saveAddBusinessRuleButton.show();
        saveEditBusinessRuleButton.hide();
    } else if (mode === "edit") {
        editDIV.show();
        treeDIV.hide();
        searchDIV.hide();
        $("#PageTitle").text("Edit Business Rule");
        saveAddBusinessRuleButton.hide();
        saveEditBusinessRuleButton.show();
    } else {
        treeDIV.show();
        searchDIV.show();
        editDIV.hide();
        $('input[id$=hdnBusinessRuleID]').val("");
        $("#PageTitle").text("Business Rules");
    }
}

function saveBusinessRuleButton_click(e) {
    "use strict";
    savingBusinessRule = true;
    var validationMessage, BRSectionDDL, selectedSection, addDefectCheckbox, saveAddBusinessRuleButton, cancelBusinessRuleButton;
    //disable save button to prevent double clicks waiting for postback
    cancelBusinessRuleButton = $('input[id$=btnCancel]');
    saveAddBusinessRuleButton = $('input[id$=btnSaveAddBusinessRule]');
    saveAddBusinessRuleButton.hide();
    cancelBusinessRuleButton.hide();
    validationMessage = "";
    //check to see if Add Defect is checked but no owner specified
    addDefectCheckbox = $('#chkAddDefect');
    var addDefectChecked = addDefectCheckbox.is(':checked');
    var defectOwnerID = $('input[id$=hdnDefectOwnerID]').val();
    if (addDefectChecked == true && defectOwnerID == "") {
        showDialog(true);
        e.preventDefault();
        //return false;
    }
    else {
        //pull latest section from ddl
        BRSectionDDL = $("select[id$=ddlBRNameSection]");
        selectedSection = BRSectionDDL.find(":selected").val();
        $('input[id$=hdnSectionID').val(selectedSection);
        //to-do: add validation code here
    }
}

function cancelBusinessRuleButton_click() {
    "use strict";
    setMode("");
    return false;
}

function initializeControls() {
    "use strict";
    var saveAddBusinessRuleButton, businessRuleStatusDDL, saveEditBusinessRuleButton, addBusinessRuleNameTextbox,
        cancelBusinessRuleButton, addBusinessRuleTextTextbox, retVal, addDefectCheckbox, saveDefectOwnerButton,
        cancelDefectOwnerButton, defectOwnerDDL, activeCheckbox, subSectionTextbox;
    saveAddBusinessRuleButton = $('input[id$=btnSaveAddBusinessRule]');
    saveAddBusinessRuleButton.click(function (e) {
        saveBusinessRuleButton_click(e);
    });
    saveAddBusinessRuleButton.addClass("JQueryDebug");
    saveEditBusinessRuleButton = $('input[id$=btnSaveEditBusinessRule]');
    saveEditBusinessRuleButton.click(function (e) {
        saveBusinessRuleButton_click(e);
    });
    saveEditBusinessRuleButton = $(":asp(btnSaveEditBusinessRule)");
    saveEditBusinessRuleButton.click(saveBusinessRuleButton_click);
    saveEditBusinessRuleButton.addClass("JQueryDebug");
    saveAddBusinessRuleButton = $(":asp(btnSaveAddBusinessRule)");
    saveAddBusinessRuleButton.click(saveBusinessRuleButton_click);
    saveAddBusinessRuleButton.addClass("JQueryDebug");
    cancelBusinessRuleButton = $('input[id$=btnCancel]');
    cancelBusinessRuleButton.addClass("JQueryDebug");
    cancelBusinessRuleButton.click(function () { cancelBusinessRuleButton_click(); });
    businessRuleStatusDDL = $("select:asp(ddlBusinessRuleStatus)");
    businessRuleStatusDDL.addClass("JQueryDebug");
    addBusinessRuleNameTextbox = $("input:asp(txtBusinessRuleName)");
    addBusinessRuleNameTextbox.addClass("JQueryDebug");
    addBusinessRuleTextTextbox = $("input:asp(txtBusinessRuleText)");
    addBusinessRuleTextTextbox.addClass("JQueryDebug");
    subSectionTextbox = $("input:asp(txtBRName3)");
    subSectionTextbox.focusout(subSectionChanged);
    activeCheckbox = $('#chkActive');
    activeCheckbox.addClass("JQueryDebug");
    //defect owner controls
    addDefectCheckbox = $('#chkAddDefect');
    addDefectCheckbox.addClass("JQueryDebug");
    addDefectCheckbox.change(toggleAddDefect);
    saveDefectOwnerButton = $('input[id$=btnSaveDefectOwner]');
    saveDefectOwnerButton.addClass("JQueryDebug");
    saveDefectOwnerButton.click(saveDefectOwnerButton_click);
    cancelDefectOwnerButton = $('input[id$=btnCancelDefectOwner]');
    cancelDefectOwnerButton.addClass("JQueryDebug");
    cancelDefectOwnerButton.click(cancelDefectOwnerButton_click);
    defectOwnerDDL = $(".defectOwnerDDL");
    defectOwnerDDL.addClass("JQueryDebug");
    $('#dlgDefectOwner').dialog({
        autoOpen: false,
        modal: true,
        width: 450,
        open: function () { }
    });
    var brSectionDDL = $(".BRNameSectionDDL");
    brSectionDDL.change(sectionChanged);
}

function sectionChanged() {
    subSectionTextbox = $("input:asp(txtBRName3)");
    subSectionTextbox.val("1");
    subSectionChanged();
}

function subSectionChanged() {
    var subSectionTextbox, sectionID, subSectionID, projectID, BRSectionDDL, selectedSection;
    subSectionTextbox = $("input:asp(txtBRName3)");
    subSectionID = subSectionTextbox.val();
    projectID = $('input[id$=hdnProjectID]').val();
    BRSectionDDL = $("select[id$=ddlBRNameSection]");
    selectedSection = BRSectionDDL.find(":selected").val();
    $.webMethod({ 'methodName': 'getNextBRNumberForSubSection', 'parameters': { 'secID': selectedSection, 'subSectID': subSectionID, 'projID': projectID },
        success: function (response) {
            lastBRTextbox = $("input:asp(txtBRName4)");
            lastBRTextbox.val(response);
        }
    });
}

function saveDefectOwnerButton_click() {
    var defectOwnerDDL = $("select[id$=ddlDefectOwner]")
    var defectOwnerID = defectOwnerDDL.val();
    $('input[id$=hdnDefectOwnerID]').val(defectOwnerID);
    showDialog(false);
    if (savingBusinessRule == true) {
        //initiate Save again
        //are we editing or adding new?
        var editBusinessRuleID = $('input[id$=hdnBusinessRuleID]').val();
        if(editBusinessRuleID != "" && editBusinessRuleID != null) {
            $('input[id$=btnSaveEditBusinessRule]').click();
            $('input[id$=btnSaveEditBusinessRule]').attr("disabled", "disabled");
        }
        else {
            $('input[id$=btnSaveAddBusinessRule]').click();
        }
    }
}

function toggleAddDefect() {
    addDefectCheckbox = $('#chkAddDefect');
    if (addDefectCheckbox.is(':checked')) {
        showDialog(true);
    }
    else {
        $('input[id$=hdnDefectOwnerID]').val('');
    }
}

function cancelDefectOwnerButton_click() {
    showDialog(false);
}

function showDialog(visible) {
    if (visible) {
        $('#dlgDefectOwner').dialog('open');
        $('#dlgDefectOwner').parent().appendTo($("form:first"));
    }
    else {
        $('#dlgDefectOwner').dialog('close');
    }
}

function onNodeSelected(node) {
    "use strict";
    if (!(typeof (node) == 'undefined' || node == null)) {
        var nodeArray, brID, projectID, sectionID, parentNode, parentNodeArray;
        //get section id from parent node
        parentNode = node.parentNode.parentNode;
        parentNodeArray = parentNode.id.split("_");
        if (parentNodeArray[0] == "SEC") {
            sectionID = parentNodeArray[1];
            $('input[id$=hdnSectionID]').val(sectionID);
        }
        nodeArray = node.id.split("_");
        if (nodeArray[0] == "BR") {
            brID = nodeArray[1];
            projectID = $('input[id$=hdnProjectID]').val();
            $.webMethod({ 'methodName': 'GetBusinessTextByRuleID', 'parameters': { 'projID': projectID, 'brID': brID },
                success: function (response) {
                    var RuleText = $(".ObjText");
                    RuleText[0].innerHTML = response;
                }
            });
        }
        else {
            var RuleText = $(".ObjText");
            RuleText[0].innerHTML = "";
        }
    }
}

function checkMove(m) {
    "use strict";
    return !(m.p === "before" || m.p === "after" || m.p === "inside");
}

function verifyDelete() {
    "use strict";
    var result = confirm('Are you sure you want to delete this record?');
    return result;
}

function deleteBusinessRule(nodeID) {
    "use strict";
    if (verifyDelete()) {
        var projID, nodeArray, brID, deleteBRButton;
        projID = $('input[id$=hdnProjectID]').val();
        nodeArray = nodeID.split("_");
        brID = nodeArray[1];
        $('input[id$=hdnBusinessRuleID').val(brID);
        deleteBRButton = $('input[id$=btnDeleteBR]');
        deleteBRButton.click();
    }
}

function editBusinessRule(nodeID) {
    "use strict";
    var projID, nodeArray, brID, editBRButton;
    projID = $('input[id$=hdnProjectID]').val();
    nodeArray = nodeID.split("_");
    brID = nodeArray[1];
    $('input[id$=hdnBusinessRuleID]').val(brID);
    editBRButton = $('input[id$=btnEditBR]');
    editBRButton.click();
}

function populateBREditFields(viewBusinessRule) {
    "use strict";
    var editBusinessRuleNameTextbox, editor;
    editBusinessRuleNameTextbox = $("input:asp(txtBusinessRuleName)");
    editBusinessRuleNameTextbox.val(viewBusinessRule.Name);
    $('input[id$=hdnSectionID]').val(viewBusinessRule.SectionID);
}

function customMenu(node) {
    "use strict";
    var indexBR, indexSEC, disableEdit, disableDelete, disableAdd, items, retVal;
    indexBR = node[0].id.indexOf("BR_");
    indexSEC = node[0].id.indexOf("SEC_");
    disableEdit = $('input[id$=hdnDisableEdit]').val();
    disableDelete = $('input[id$=hdnDisableDelete]').val();
    disableAdd = $('input[id$=hdnDisableAdd]').val();
    if (indexSEC > -1) {
        if (disableAdd === "false") {
            items = {
                editItem: {
                    label: "Add Business Rule",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        addBusinessRule(nodeID);
                    }
                }
            };
            retVal = items;
        }
    }
    else if (indexBR > -1) {
        // The default set of all items
        if (disableEdit === "false" && disableDelete === "false") {
            items = {
                editItem: { // The "edit" menu item
                    label: "Edit Business Rule",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        editBusinessRule(nodeID);
                    }
                },
                deleteItem: {
                    label: "Delete Business Rule",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        deleteBusinessRule(nodeID);
                    }
                }
            };
            retVal = items;
        } else if (disableEdit === "false" && disableDelete === "true") {
            items = {
                editItem: { // The "edit" menu item
                    label: "Edit Business Rule",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        editBusinessRule(nodeID);
                    }
                }
            };
            retVal = items;
        } else if (disableEdit === "true" && disableDelete === "false") {
            items = {
                deleteItem: {
                    label: "Delete Business Rule",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        deleteBusinessRule(nodeID);
                    }
                }
            };
            retVal = items;
        }

    } else {
        retVal = false;
    }
    return retVal;
}

function populateBusinessRulesTree() {
    "use strict";
    var projectID, treeDIV, a, defaultID;
    defaultID = 0;
    //the treeDIV will contain the tree
    projectID = $('input[id$=hdnProjectID]').val();
    //var projectID = 1;
    treeDIV = $('.treeFull');
    treeDIV.addClass("JQueryDebug");
    treeDIV.jstree({
        "json_data": {
            "ajax": {
                "type": "POST",
                "dataType": "json",
                "contentType": "application/json;",
                "url": "BusinessRules.aspx/GetBusinessRulesForTree",
                "data": function (node) {
                    if (node === -1) {
                        return '{  "nodeID" : ' + defaultID + ', "projID" : ' + projectID + '}';
                    }
                    else {
                        var nid = node[0].id;
                        return '{  "nodeID" :  "' + nid + '" , "projID" : ' + projectID + '}';
                    }
                },
                "success": function (retval) {
                    return retval.d;
                }
            }
        },
        "themes": {
            "theme": "classic",
            "dots": true,
            "icons": true
        },
        "contextmenu": {
            "items": customMenu,
            "select_node": true
        },
        "cookies": { cookie_options: { path: '/'} },
        "plugins": ["themes", "json_data", "ui", "contextmenu", "cookies"]
    })
        .bind("select_node.jstree", function (NODE, REF_NODE) {
            a = $.jstree._focused().get_selected();
            onNodeSelected(a[0]);
        });
}

function brNameSectionSelected(e) {
    "use strict";
    var BRSectionDDL, selectedSection, projectID;
    BRSectionDDL = $("select[id$=ddlBRNameSection]");
    selectedSection = BRSectionDDL.find(":selected").val();
    $('input[id$=hdnSectionID').val(selectedSection);
    projectID = $('input[id$=hdnProjectID]').val();
    $.webMethod({ 'methodName': 'getNextBRNumberBySection', 'parameters': { 'secID': selectedSection, 'projID': projectID },
        success: function (response) {
            $('input[id$=txtBRName4]').val(response);
        }
    });
}

function addBusinessRule(nodeID) {
    "use strict";
    var BRSectionDDL, BRStatusDDL, BRNameTextbox1, activeCheckbox, sectionID, nodeArray, addDefectCheckbox, subSectionBRTextbox;
    nodeArray = nodeID.split("_");
    sectionID = nodeArray[1];
    setMode("add");
    BRSectionDDL = $("select[id$=ddlBRNameSection]").change(function (event) {
        brNameSectionSelected(event);
    });
    BRStatusDDL = $("select[id$=ddlBusinessRuleStatus]");
    BRStatusDDL.prop('selectedIndex', 2); //<<-- hard coded value for "New"  problems if deleted by admin/user
    //BRStatusDDL.val('New');
    $("select[id$=ddlBRNameSection]").val(sectionID);
    brNameSectionSelected();
    BRNameTextbox1 = $("input:asp(txtBRName1)");
    BRNameTextbox1.val("BR");
    activeCheckbox = $('#chkActive');
    activeCheckbox.attr('checked', true);
    addDefectCheckbox = $('#chkAddDefect');
    addDefectCheckbox.attr('checked', true);
    addDefectCheckbox.attr("disabled", "disabled");
    //default BR name subsection (textbox 3) to 1
    subSectionBRTextbox = $("input:asp(txtBRName3)");
    subSectionBRTextbox.val("1");
}

$(document).ready(function () {
    "use strict";
    var addDefectCheckbox;
    initializeControls();
    populateBusinessRulesTree();
    addDefectCheckbox = $('#chkAddDefect');
    addDefectCheckbox.attr('checked', false);
    var editor = CKEDITOR;
    editor.config.height = 360;
});