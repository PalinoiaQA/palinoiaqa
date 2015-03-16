/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/*jslint browser: true*/
/*global $, jQuery, alert, confirm*/

//set Global control variables
//var cancelForm = false;

function setMode(mode) {
    "use strict";
    var editDIV, treeDIV, saveAddCSMButton, saveEditCSMButton, searchDIV;
    editDIV = $("#CSMEditDIV");
    treeDIV = $("#CSMTree");
    searchDIV = $(".searchDIV");
    saveAddCSMButton = $('input[id$=btnSaveAddCSM]');
    saveEditCSMButton = $('input[id$=btnSaveEditCSM]');
    if (mode === "add") {
        editDIV.show();
        treeDIV.hide();
        $("#PageTitle").text("Add Customer Service Message");
        saveAddCSMButton.show();
        saveEditCSMButton.hide();
        searchDIV.hide();
    } else if (mode === "edit") {
        editDIV.show();
        treeDIV.hide();
        $("#PageTitle").text("Edit Customer Service Message");
        saveAddCSMButton.hide();
        saveEditCSMButton.show();
        searchDIV.hide();
    } else {
        treeDIV.show();
        editDIV.hide();
        searchDIV.show();
        $("#PageTitle").text("Customer Service Messages");
    }
}

function CSMNameSectionSelected(e) {
    "use strict";
    var CSMSectionDDL, selectedSection, projectID;
    CSMSectionDDL = $("select[id$=ddlCSMNameSection]");
    selectedSection = CSMSectionDDL.find(":selected").val();
    $('input[id$=hdnSectionID').val(selectedSection);
    projectID = $('input[id$=hdnProjectID').val();
    $.webMethod({ 'methodName': 'getNextCSMNumberBySection', 'parameters': { 'secID': selectedSection, 'projID': projectID },
        success: function (response) {
            $('input[id$=txtCSMName4]').val(response);
        }
    });
}

function addCSM(nodeID) {
    "use strict";
    var CSMSectionDDL, CSMNameTextbox1, activeCheckbox, nodeArray, sectionID, CSMStatusDDL;
    nodeArray = nodeID.split("_");
    sectionID = nodeArray[1];
    setMode("add");
    CSMSectionDDL = $("select[id$=ddlCSMNameSection]").change(function (event) {
        CSMNameSectionSelected(event);
    });
    $("select[id$=ddlCSMNameSection]").val(sectionID);
    CSMNameSectionSelected()
    CSMNameTextbox1 = $("input:asp(txtCSMName1)");
    CSMNameTextbox1.val("CSM");
    activeCheckbox = $('#chkActive');
    activeCheckbox.addClass("JQueryDebug");
    activeCheckbox.attr('checked', true);
    CSMStatusDDL = $("select[id$=ddlCSMStatus]");
    CSMStatusDDL.prop('selectedIndex', 2); //<<-- hard coded value for "New"  problems if deleted by admin/user
}

function saveCSMButton_click(e) {
    "use strict";
    var validationMessage, CSMSectionDDL, selectedSection;
    validationMessage = "";
    //pull latest section from ddl
    CSMSectionDDL = $("select[id$=ddlCSMNameSection]");
    selectedSection = CSMSectionDDL.find(":selected").val();
    $('input[id$=hdnSectionID').val(selectedSection);
    //to-do: add validation code here
}

$(document).ready(function () {
    "use strict";
    initializeControls();
    populateCSMsTree();
    var editor = CKEDITOR;
    editor.config.height = 330;
});



function cancelCSMButton_click() {
    "use strict";
    setMode("");
    return false;
}

function initializeControls() {
    "use strict";
    var saveAddCSMButton, saveEditCSMButton, businessRuleStatusDDL, addCSMNameTextbox, addCSMTextTextbox, cancelEditCSMButton;
    saveAddCSMButton = $('input[id$=btnSaveAddCSM]');
    saveAddCSMButton.click(function (e) {
        saveCSMButton_click(e);
    });
    saveAddCSMButton.addClass("JQueryDebug");
    saveEditCSMButton = $('input[id$=btnSaveEditCSM]');
    saveEditCSMButton.click(function (e) {
        saveCSMButton_click(e);
    });
    saveEditCSMButton = $(":asp(btnSaveEditCSM)");
    saveEditCSMButton.click(saveCSMButton_click);
    saveEditCSMButton.addClass("JQueryDebug");
    businessRuleStatusDDL = $("select:asp(ddlCSMStatus)");
    businessRuleStatusDDL.addClass("JQueryDebug");
    saveEditCSMButton.addClass("JQueryDebug");
    addCSMNameTextbox = $("input:asp(txtCSMName)");
    addCSMNameTextbox.addClass("JQueryDebug");
    addCSMTextTextbox = $("input:asp(txtCSMText)");
    addCSMTextTextbox.addClass("JQueryDebug");
    cancelEditCSMButton = $('input[id$=btnEditCSMCancel]');
    cancelEditCSMButton.addClass("JQueryDebug");
    cancelEditCSMButton.click(cancelCSMButton_click);
}

function customMenu(node) {
    "use strict";
    var indexBR, indexSEC, disableAdd, disableEdit, disableDelete, items, retVal, nodeID;
    indexBR = node[0].id.indexOf("CSM_");
    indexSEC = node[0].id.indexOf("SEC_");
    disableEdit = $('input[id$=hdnDisableEdit').val();
    disableDelete = $('input[id$=hdnDisableDelete').val();
    disableAdd = $('input[id$=hdnDisableAdd]').val();
    if (indexSEC > -1) {
        if (disableAdd === "false") {
            items = {
                editItem: {
                    label: "Add CSM",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        addCSM(nodeID);
                    }
                }
            };
            retVal = items;
        }
    }
    else if (indexBR > -1) {
        // The default set of all items
        if (disableEdit == "false" && disableDelete == "false") {
            items = {
                editItem: { // The "edit" menu item
                    label: "Edit CSM",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        editCSM(nodeID);
                    }
                },
                deleteItem: {
                    label: "Delete CSM",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        deleteCSM(nodeID);
                    }
                }
            };
            retVal = items;
        } else if (disableEdit == "false" && disableDelete == "true") {
            items = {
                editItem: { // The "edit" menu item
                    label: "Edit CSM",
                    action: function (NODE, REF_NODE) {
                        nodeID = $(NODE).attr("id");
                        editCSM(nodeID);
                    }
                }
            };
            retVal = items;
        } else if (disableEdit == "true" && disableDelete == "false") {
            items = {
                deleteItem: {
                    label: "Delete CSM",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        deleteCSM(nodeID);
                    }
                }
            };
            retVal = items;
        }

    }
    else {
        retVal = false;
    }
    return retVal;
}

function populateCSMsTree() {
    "use strict";
    var projectID, treeDIV, a, defaultID;
    defaultID = 0;
    //the treeDIV will contain the tree
    projectID = $('input[id$=hdnProjectID').val();
    //var projectID = 1;
    treeDIV = $('.treeCSMs');
    treeDIV.addClass("JQueryDebug");
    treeDIV.jstree({
        "json_data": {
            "ajax": {
                "type": "POST",
                "dataType": "json",
                "contentType": "application/json;",
                "url": "CustomerServiceMessages.aspx/GetCSMsForTree",
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

function onNodeSelected(node) {
    "use strict";
    if (!(typeof (node) == 'undefined' || node == null)) {
        var nodeArray, csmID, projectID;
        nodeArray = node.id.split("_");
        if (nodeArray[0] == "CSM") {
            csmID = nodeArray[1];
            projectID = $('input[id$=hdnProjectID]').val();
            $.webMethod({ 'methodName': 'GetCSMTextByID', 'parameters': { 'projID': projectID, 'csmID': csmID },
                success: function (response) {
                    var CSMText = $(".ObjText");
                    CSMText[0].innerHTML = response;
                }
            });
        }
        else {
            var CSMText = $(".ObjText");
            CSMText[0].innerHTML = "";
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

function deleteCSM(nodeID) {
    "use strict";
    if (verifyDelete()) {
        var projID, nodeArray, brID, deleteCSMButton;
        projID = $('input[id$=hdnProjectID').val();
        nodeArray = nodeID.split("_");
        brID = nodeArray[1];
        $('input[id$=hdnCSMID').val(brID);
        deleteCSMButton = $('input[id$=btnDeleteCSM');
        deleteCSMButton.click();
    }
}

function editCSM(nodeID) {
    "use strict";
    var projID, nodeArray, csmID, editCSMButton;
    projID = $('input[id$=hdnProjectID').val();
    nodeArray = nodeID.split("_");
    csmID = nodeArray[1];
    $('input[id$=hdnCSMID').val(csmID);
    editCSMButton = $('input[id$=btnEditCSM');
    editCSMButton.click();
}

function populateCSMEditFields(viewCSM) {
    "use strict";
    var editCSMNameTextbox, editor;
    editCSMNameTextbox = $("input:asp(txtCSMName)");
    editCSMNameTextbox.val(viewCSM.Name);
    $('input[id$=hdnSectionID').val(viewCSM.SectionID);
    editor = $("#CKEditor1");
}

