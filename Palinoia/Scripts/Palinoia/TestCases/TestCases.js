/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/// <reference path="../../jquery.json-2.4.min.js" />
/// <reference path="../../JSTree/jquery.jstree.js" />
/// <reference path="../../jquery-ui-1.9.0.min.js" />

/*jslint browser: true*/
/*global $, jQuery, alert, confirm*/

//set global variables
var CANCELFORM = false;
var $activeRow;

$(document).ready(function () {
    "use strict";
    initializeControls();
    populateTestCasesTree();
    populatePreConditionsDisplayListbox()
    checkEnableDisableRemoveRelatedBusinessRuleButton();
});

function initializeControls() {
    var testCaseEditMode, testStepEditMode, refreshButton;
    refreshButton = $('input[id$=btnRefresh]');
    refreshButton.addClass("JQueryDebug");
    refreshButton.click(function(e) {
        refreshAction(e);
    });
    var testCaseNameTextbox = $('input[id$=txtTestCaseName]');
    testCaseNameTextbox.addClass("JQueryDebug");
    var saveTestCaseButton = $('input[id$=btnTestCaseSave]');
    saveTestCaseButton.unbind('click');
    saveTestCaseButton.click(function (e) {
        return saveTestCaseButton_click(e);
    });
    saveTestCaseButton.addClass("JQueryDebug");
    var doneButton = $('input[id$=btnDone]');
    doneButton.addClass("JQueryDebug");
    doneButton.click(function (e) {
        doneAction(e);
    });
    var sectionsDDL = $("select[id$=ddlSections]");
    sectionsDDL.addClass("JQueryDebug");
    sectionsDDL.change(function (e) {
        var selectedSectionID = sectionsDDL.find(":selected").val();
        $("[id$='hdnSectionID']").val(selectedSectionID);
    });
    testCaseEditMode = $('input[id$=hdnTestCaseMode]').val();
    setTestCaseEditMode(testCaseEditMode);
    initializePreConditionControls();
    initializeTestStepControls();
}

function initializeTestStepControls() {
    //TEST STEP ###################################################
    var addTestStepButton;
    addTestStepButton = $('input[id$=btnAddTestStep]');
    addTestStepButton.addClass("JQueryDebug");
    addTestStepButton.unbind('click');
    addTestStepButton.click(function (e) {
        addTestStepButton_click(e);
    });
    var dlg3 = $('#dlgTestStep').dialog({
        autoOpen: false,
        modal: true,
        width: 700,
        open: function () { }
    });
    dlg3.parent().appendTo(jQuery("form:first"));
    if ($("[id$='hdnSelectedTestStepID']").val() == "0" || $("[id$='hdnSelectedTestStepID']").val() == "") {
        $('input[id$=btnMoveUp]').attr("disabled", "disabled");
        $('input[id$=btnMoveDown]').attr("disabled", "disabled");
    }
    var testStepGrid = $('.testStepGrid tr');
    testStepGrid.unbind('click');
    testStepGrid.click(function () {
        $('.testStepGrid tr').not(this).removeClass('selectedRow');
        $(this).toggleClass('selectedRow');
        $("[id$='hdnSelectedTestStepID']").val($(this).children().eq(0).text());
        var seqNum = $(this).children().eq(1).text();
        var seqNumber = parseInt(seqNum, 10);
        var totalTestSteps = testStepGrid.length - 1;
        //don't enable move up button for first record
        if (seqNumber > 1) {
            $('input[id$=btnMoveUp]').removeAttr("disabled");
        }
        else {
            $('input[id$=btnMoveUp]').attr("disabled", "disabled");
        }
        //don't enable move down button for last record or if only 1 record total
        if (seqNum < totalTestSteps) {
            $('input[id$=btnMoveDown]').removeAttr("disabled");
        }
        else {
            $('input[id$=btnMoveDown]').attr("disabled", "disabled");
        }
    });
    $('.testStepGrid').addClass("JQueryDebug");
    var testStepTextbox = $('.TestStepText');
    testStepTextbox.focusout(getRelatedBusinessRules);
    testStepTextbox.addClass("JQueryDebug");
    var testStepDDL = $('.TestStepDDL');
    testStepDDL.addClass("JQueryDebug");
    testStepDDL.change(function (e) {
        testStepDDL_change(e);
    });
    var moveUpButton = $('input[id$=btnMoveUp]');
    moveUpButton.addClass("JQueryDebug");
    var moveDownButton = $('input[id$=btnMoveDown]');
    moveDownButton.addClass("JQueryDebug");
    var testStepSelectButton = $('input[id$=btnTestStepSelect]');
    testStepSelectButton.addClass("JQueryDebug");
    testStepSelectButton.click(function (e) {
        btnTestStepSelect_click(e);
    });
    var testStepNewButton = $('input[id$=btnTestStepNew]');
    testStepNewButton.addClass("JQueryDebug");
    testStepNewButton.click(function (e) {
        btnTestStepNew_click(e);
    });
    var testStepCancelButton = $('input[id$=btnTestStepCancel]');
    testStepCancelButton.addClass("JQueryDebug");
    testStepCancelButton.click(function (e) {
        btnTestStepCancel_click(e);
    });
    var saveTestStepButton = $('input[id$=btnTestStepSave]');
    saveTestStepButton.addClass("JQueryDebug");
    saveTestStepButton.unbind('click');
    saveTestStepButton.click(function (e) {
        btnTestStepSave_click(e);
    });
    var relatedBusinessRulesListBox = $('.jstree-drop');
    relatedBusinessRulesListBox.addClass("JQueryDebug");
    var removeRelatedBusinessRuleButton = $('input[id$=btnRemoveRelatedBusinessRule]');
    removeRelatedBusinessRuleButton.addClass("JQueryDebug");
    removeRelatedBusinessRuleButton.click(function (e) {
        btnRemoveRelatedBusinessRule_click(e);
    });
    var testStepNotesTextbox = $('.NotesText');
    testStepNotesTextbox.addClass("JQueryDebug");
}

function initializePreConditionControls() {
    //PRE CONDITIONS ###################################################
    var editPreCButton;
    editPreCButton = $('input[id$=btnEditPreConditions]');
    editPreCButton.addClass("JQueryDebug");
    editPreCButton.click(function (e) {
        editPreCButton_click(e);
    });
    var dlg1 = $('#dlgPreCondition').dialog({
        autoOpen: false,
        modal: true,
        width: 800,
        open: function () { }
    });
    dlg1.parent().appendTo(jQuery("form:first"));

    var preConditionCancelButton = $('input[id$=btnPreCCancel]');
    preConditionCancelButton.addClass("JQueryDebug");
    preConditionCancelButton.click(function (e) {
        btnPreCCancel_click(e);
    });
    var savePreCButton = $('input[id$=btnSavePreCon]');
    savePreCButton.addClass("JQueryDebug");
    savePreCButton.unbind('click');
    savePreCButton.click(function (e) {
        btnPreCSave_click();
    });
    var moveTCUpButton = $('input[id$=btnMoveTCUp]');
    moveTCUpButton.addClass("JQueryDebug");
    moveTCUpButton.unbind('click');
    moveTCUpButton.click(function (e) {
        MoveTCUp_click();
    });
    var moveTCDownButton = $('input[id$=btnMoveTCDown]');
    moveTCDownButton.addClass("JQueryDebug");
    moveTCDownButton.unbind('click');
    moveTCDownButton.click(function (e) {
        MoveTCDown_click();
    });
    var removePreCTestCaseButton = $('input[id$=btnRemovePreCTestCase]');
    removePreCTestCaseButton.addClass("JQueryDebug");
    removePreCTestCaseButton.unbind('click');
    removePreCTestCaseButton.click(function (e) {
        btnRemovePreCTestCase_click();
    });
    var listboxPreConditions = $('.listboxPreConditionsDisplay');
    listboxPreConditions.addClass('JQueryDebug');
}

function getRelatedBusinessRules() {
    var relatedBusinessRulesListBox = $('.jstree-drop.relatedBusinessRuleListbox');
    var name = $('.TestStepText').val();
    var pcID = $('[id$=hdnProjectID]').val();
    $.webMethod({ 'methodName': 'getRelatedBusinessRulesForTSName', 'parameters': { 'projID': pcID, 'tsName': name },
        success: function (text) {
            if (text != "") {
                var arrPCList = text.split(",");
                for (var i = 0; i < arrPCList.length; i = i + 1) {
                    dropBusinessRule(arrPCList[i]);
                }
            }
        }
    });
}

function checkEnableDisableRemoveRelatedBusinessRuleButton() {
    var relatedBRListBox = $('.jstree-drop');
    var brCount = relatedBRListBox.children().length;
    var removeRelatedBusinessRuleButton = $('input[id$=btnRemoveRelatedBusinessRule]');
    if (brCount == 0) {
        removeRelatedBusinessRuleButton.attr("disabled", "disabled");
    }
    else {
        removeRelatedBusinessRuleButton.removeAttr("disabled");
    }
}

function refreshAction(e) {
    $('input[id$=hdnTestCaseMode]').val("");
    initializeControls();
    populateTestCasesTree();
}

function doneAction(e) {
    $('input[id$=hdnTestCaseID]').val("");
    $("[id$='hdnSectionID']").val("");
    setTestCaseEditMode("add");
    //TODO: empty all test step grids and clear textbox
    var listboxPreConditions = $('.listboxPreConditionsDisplay');
    listboxPreConditions.addClass('JQueryDebug');
    listboxPreConditions.empty();
    var testStepGrid = $('.testStepGrid');
    testStepGrid.select("tr:not(:first-child)").html("");
    var testCaseNameTextbox = $('.TestCaseName');
    testCaseNameTextbox.val("");
    var sectionsDDL = $('.SectionsDDL');
    sectionsDDL.empty();
}

function testStepDDL_change(e) {
    var testStepDDL = $('.TestStepDDL');
    var selectedID = testStepDDL.val();
    $('input[id$=hdnTestStepID]').val(selectedID);    
}

function addTestStepButton_click(e) {
    //clear hidden fields
    $('input[id$=hdnTestStepID]').val("0");
    $('input[id$=hdnOriginalTestStepID]').val("0");
    $('#dlgTestStep').dialog('open');
    $('#dlgTestStep').parent().appendTo($("form:first"));
    //setTestStepEditMode("add");
    populateBusinessRulesTree();
    clearRelatedBusinessRulesListbox();
    var testStepTextbox = $('.TestStepText');
    testStepTextbox.val("");
    var testStepNotesTextbox = $('.NotesText');
    testStepNotesTextbox.val("");
}

//this function is called by the server when user
//clicks edit in test steps grid.  Controls loaded
//from server for existing test step/test case 
//relationship
function editTestStepLink_click() {
    $('#dlgTestStep').dialog('open');
    $('#dlgTestStep').parent().appendTo($("form:first"));
    //setTestStepEditMode("edit");
    populateBusinessRulesTree();
}

function showEditPreConditionModal(preConditionText) {
    setPreCModalEditMode("edit");
    //set up dialog and display
    var dlg = $('#dlgPreCondition').dialog({
        autoOpen: false,
        modal: true,
        width: 550,
        open: function () { }
    });
    dlg.parent().appendTo(jQuery("form:first"));
    $('#dlgPreCondition').dialog('open');
}

function testRunnerAction(nodeID) {
    var nodeArray, tcID, sectionID;
    //get test case id and populate hidden field
    tcIndex = nodeID.indexOf("TC_");
    if (tcIndex > -1) {
        nodeArray = nodeID.split("_");
        tcID = nodeArray[1];
        $('input[id$=hdnTestCaseID]').val(tcID);
        var settings = 'scrollbars=no,resizable=yes,status=no,location=no,toolbar=no,menubar=no,';
        settings = settings + 'width=450,height=200,left=100,top=100';
        testRunnerURL = '../../../UI/TestCases/TestRunner.aspx?tcid=' + tcID;
        var testRunner = open('', 'TestRunner', settings);
        //if popup is not already open
        if (testRunner.location == "about:blank") {
            testRunner.location.href = testRunnerURL;
            testRunner.focus();
        }
        //else, just set focus to existing popup window
        else {
            testRunner.focus();
        }
    }
}

function editTreeAction(node) {
    var nodeID, nodeArray, tcID, sectionID, parentID, editTCButton;
    nodeID = $(node).attr("id");
    parentID = node[0].parentNode.parentNode.id;
    setTestCaseEditMode("edit");
    //get test case id and populate hidden field
    nodeArray = nodeID.split("_");
    tcID = nodeArray[1];
    nodeArray = parentID.split("_");
    sectionID = nodeArray[1];
    $('input[id$=hdnTestCaseID]').val(tcID);
    $('input[id$=hdnSectionID]').val(sectionID);
    //showTestCaseEditControls();
    //generate server postback
    editTCButton = $('input[id$=btnEditTC]');
    editTCButton.click();
}

function setTestCaseEditMode(mode) {
    $("#hdnTestCaseMode").val(mode);
    if (mode == "add") {
        $("#TestCasesTreeDIV").show();
        $("#EditTestCaseDIV").hide();
        //$('.beforeSaveEditDIV').hide();
        //$('.afterSaveEditDIV').hide();
        $(".searchDIV").show();
        $('input[id$=hdnTestCaseID]').val("");
        
    }
    else if (mode == "edit") {
        $("#TestCasesTreeDIV").hide();
        $("#EditTestCaseDIV").show();
        //$('.beforeSaveEditDIV').show();
        $(".searchDIV").hide();
        
    }
    else if (mode == "") {
        $("#TestCasesTreeDIV").show();
        $("#EditTestCaseDIV").hide();
        //$('.beforeSaveEditDIV').hide();
        //$('.afterSaveEditDIV').hide();
        $(".searchDIV").show();
        $('input[id$=hdnTestCaseID]').val("");
    }
}

function deleteTreeAction(nodeID) {
    var tcIndex, tsIndex;
    tcIndex = nodeID.indexOf("TC_");
    if (tcIndex > -1) {
        var result = confirm("Are you sure you want to delete this test case?");
        if (result) {
            //get test case id from nodeID and store in hidden variable
            var deleteID, arrNodeID;
            arrNodeID = nodeID.split("_");
            deleteID = arrNodeID[1];
            $('input[id$=hdnDeleteID]').val(deleteID);
            $('input[id$=hdnTestCaseID]').val("");
            //click hidden button for server postback
            var deleteTCButton = $('input[id$=btnDeleteTC]');
            deleteTCButton.click();
            populateTestCasesTree();
        }
    }
}

function addTestCaseAction(nodeID) {
    var sectionIndex, nodeArray, sectionID;
    //verity that this is a section node
    sectionIndex = nodeID.indexOf("SEC_");
    if (sectionIndex > -1) {
        //get section id from node id
        nodeArray = nodeID.split("_");
        sectionID = nodeArray[1];
        //populate hidden fields for test case add
        $('input[id$=hdnTestCaseID]').val("0");
        $('input[id$=hdnSectionID]').val(sectionID);
        //clear test case name textbox
        $('input[id$=txtTestCaseName]').val("");
        //set edit mode to show edit controls
        setTestCaseEditMode("edit");
        //hide edit controls
        hideTestCaseEditControlsUntilSave();
        //click hidden button to call server
        addTCButton = $('input[id$=btnAddTC]');
        addTCButton.click();
    }
}

function btnTestStepSelect_click(e) {
    setTestStepEditMode("edit");
}

function btnTestStepNew_click(e) {
    $('input[id$=hdnTestStepID]').val("0");
    setTestStepEditMode("add");

}

function btnPreCNew_click(e) {
    $("[id$=ddlPreConditions]").addClass("JQueryDebug");
    $("[id$=ddlPreConditions]").val('0');
    $("[id$=divEditPreC]").show();
    $("[id$=divNewPreC]").hide();
    $("[id$=divPreCSelectButton]").show();
    $("[id$=divPreCNewButton]").hide();
}

function btnPreCCancel_click(e) {
    $('#dlgPreCondition').dialog('close');
}

function btnTestStepCancel_click(e) {
    $('#dlgTestStep').dialog('close');
}

function saveTestCaseButton_click(e) {
    var validationMessage = "";
    var TCText = $('input[id$=txtTestCaseName]');
    //validate input
    re = new RegExp("[!$%&*_<>\///]");
    invalidChars = TCText.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text!";
    }
    if (TCText.val().length == 0) {
        validationMessage = "Test Case name is required.";
    }
    //check for errors
    if (validationMessage.length > 0) {
        alert(validationMessage);
        return false
    }
    else { //no errors, go ahead with save
        showTestCaseEditControls();
        return true;
    }
}

function btnPreCSave_click() {
    var hdnPCIDs = $('input[id$=hdnPreConditionIDs]');
    //clear hidden field before populating from listbox
    hdnPCIDs.val("");
    //get all pre condition test case ids and populate hidden field
    //so they can be picked up on the server
    var preConditionsListBox = $('.jstree-drop.listboxPreConditions');
    var pcids = preConditionsListBox.find('option').map(function () {
        return $(this).val();
    });
    var idList = '';
    var itemCounter = pcids.length;
    for (var i = 0; i < pcids.length; i++) {
        itemCounter--;
        idList += pcids[i];
        if (itemCounter > 0) {
            idList += ",";
        }
    }
    hdnPCIDs.val(idList);
    return true;
}

function btnRemoveRelatedBusinessRule_click(e) {
    //remove selected item(s) from listbox
    var relatedBusinessRulesListBox = $('.jstree-drop.relatedBusinessRuleListbox');
    relatedBusinessRulesListBox.find('option:selected').remove();
    checkEnableDisableRemoveRelatedBusinessRuleButton();
}

function btnTestStepSave_click(e) {
    var saveTestStepButton = $('input[id$=btnSaveTestStep]');
    saveTestStepButton.addClass("JQueryDebug");
    var hdnRBR = $('input[id$=hdnRelatedBusinessRules]');
    //get all related business rule ids and populate hidden field
    //so they can be picked up on the server
    var relatedBusinessRulesListBox = $('.jstree-drop.relatedBusinessRuleListbox');
    var aBR = relatedBusinessRulesListBox.find('option').map(function () {
        return $(this).val();
    });
    var idList = '';
    var itemCounter = aBR.length;
    for (var i = 0; i < aBR.length; i++) {
        itemCounter--;
        idList += aBR[i];
        if (itemCounter > 0) {
            idList += ",";
        }
    } 
    hdnRBR.val(idList);
    //validate user input
    var validationMessage = "";
    var tsText = $('input[id$=txtTestStep]');
    var notesText = $('.NotesText');
    var tsDDL = $('.TestStepDDL');
    if (tsText.is(":visible")) {
        //validate input
        re = new RegExp("[!$%&*_<>\///]");
        invalidChars = tsText.val().match(re);
        if (invalidChars) {
            validationMessage = "Invalid test step text!";
        }
        else {
            invalidChars = notesText.val().match(re);
            if (invalidChars) {
                validationMessage = "Invalid notes text!";
            }
        }
        if (tsText.val().length == 0) {
            validationMessage = "Test step text is required.";
        }
    }
    else if (tsDDL.is(":visible")) {
        var selectedIndex = $(".TestStepDDL option:selected").index();
        if(selectedIndex === 0) {
            validationMessage ="Dropdown selection required";
        }
    }
    //check for errors
    if (validationMessage.length > 0) {
        alert(validationMessage);
    }
    else { //no errors, go ahead with save
        saveTestStepButton.click();
    }
}

function customMenu(node) {
    "use strict";
    var sectionIndex, tcIndex, tsIndex, disableTCEdit, disableTCDelete, disableTCAdd, items, retVal;
    tcIndex = node[0].id.indexOf("TC_");
    tsIndex = node[0].id.indexOf("TS_");
    sectionIndex = node[0].id.indexOf("SEC_");
    disableTCEdit = $('input[id$=hdnDisableTCEdit]').val();
    disableTCDelete = $('input[id$=hdnDisableTCDelete]').val();
    disableTCAdd = $('input[id$=hdnDisableTCAdd]').val();
    if (tcIndex > -1) { // user right clicked a test case node
        if (disableTCEdit === "false" && disableTCDelete === "false") {
            items = {
                editItem: { 
                    label: "Edit",
                    action: function (NODE, REF_NODE) {
                        editTreeAction(NODE);
                    }
                },
                deleteItem: {
                    label: "Delete",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        deleteTreeAction(nodeID);
                    }
                },
                runTestItem: {
                label: "Run Test",
                action: function (NODE, REF_NODE) {
                    var nodeID = $(NODE).attr("id");
                    testRunnerAction(nodeID);
                }
            }
            };
            retVal = items;
        } else if (disableTCEdit === "false" && disableTCDelete === "true") {
            items = {
                editItem: { // The "edit" menu item
                    label: "Edit",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        editBusinessRule(nodeID);
                    }
                }
            };
            retVal = items;
        } else if (disableTCEdit === "true" && disableTCDelete === "false") {
            items = {
                deleteItem: {
                    label: "Delete",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        deleteBusinessRule(nodeID);
                    }
                }
            };
            retVal = items;
        }

    } else if (tsIndex > -1) { //user right clicked a test step node
        if (disableTCEdit === "false" && disableTCDelete === "false") {

        }
    } else if (sectionIndex > -1) {
        if (disableTCAdd === "false") {
            items = {
                editItem: {
                    label: "Add",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        addTestCaseAction(nodeID);
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

function populateTestCasesTree() {
    "use strict";
    var treeDIV,
        projectID,
        a, 
        defaultID;
    defaultID = 0;
    projectID = $('input[id$=hdnProjectID]').val();
    //the treeDIV will contain the tree
    treeDIV = $('.treeFull');
    treeDIV.addClass("JQueryDebug");
    treeDIV.jstree({
        "json_data": {
            "ajax": {
                "type": "POST",
                "dataType": "json",
                "contentType": "application/json;",
                "url": "TestCases.aspx/GetTestCasesForTree",
                "data": function (node) {
                    if(node === -1) {
                        
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
        "types": {
            "types": {
                "pass": {
                    "icon": {
                        "image": "C:\ palinoia\Palinoia\Scripts\JSTree\themes\testcase\tick_16.png"
                    }
                },
                "folder": {
                    "icon": {
                        "image": "folder.png"
                    }
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
        "plugins": ["themes", "json_data", "crrm", "contextmenu", "dnd", "ui", "types", "cookies"]
    })
    .bind("select_node.jstree", function (NODE, REF_NODE) {
        a = $.jstree._focused().get_selected();
        //onNodeSelected(a[0]);
    });
}

function populateBusinessRulesTree() {
    "use strict";
    var projectID, treeDIV, a, defaultID;
    defaultID = 0;
    //the treeDIV will contain the tree
    projectID = $('input[id$=hdnProjectID').val();
    //var projectID = 1;
    treeDIV = $('.treeBusinessRules');
    treeDIV.addClass("JQueryDebug");
    treeDIV.jstree({
        "json_data": {
            "ajax": {
                "type": "POST",
                "dataType": "json",
                "contentType": "application/json;",
                "url": "TestCases.aspx/GetBusinessRulesForTree",
                "data": function (node) {
                    if(node === -1) {
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
        "crrm": {
            "move": {
                //prevent moving nodes around inside the tree
                "check_move": function (m) {
                    return false;
                }
            }
        },
        "dnd": {
            "drop_check": function (data) {
                return businessRuleDropCheck(data);
            },
            "drop_finish": function (data) {
                businessRuleDrop(data);
            }
        },
        "ui": { select_multiple_modifier: false,
                select_limit: 1 },
        "plugins": ["themes", "json_data", "ui", "crrm", "dnd"]
    })
//        .bind("select_node.jstree", function (NODE, REF_NODE) {
//            a = $.jstree._focused().get_selected();
//            alert("NodeID: " + a[0].id);
//        });
    }

    function clearRelatedBusinessRulesListbox() {
        var relatedBusinessRulesListBox = $('.jstree-drop');
        relatedBusinessRulesListBox.empty();
    }

    function hideTestCaseEditControlsUntilSave() {
        //$('.afterSaveEditDIV').hide();
//        $('input[id$=btnEditPreConditions]').hide();
//        $('input[id$=btnAddTestStep]').hide();
//        $('input[id$=btnAddPostCondition]').hide();
//        $('input[id$=btnMoveUp]').hide();
//        $('input[id$=btnMoveDown]').hide();
    }

    function showTestCaseEditControls() {
        //$('.beforeSaveEditDIV').show();
        //$('.afterSaveEditDIV').show();
//        $('input[id$=btnEditPreConditions]').show();
//        $('input[id$=btnAddTestStep]').show();
//        $('input[id$=btnAddPostCondition]').show();
//        $('input[id$=btnMoveUp]').show();
//        $('input[id$=btnMoveDown]').show();
    }

    function businessRuleDrop(data) {
        //get business rule and id from node data
        var nodeID, businessRule, businessRuleID;
        nodeID = data.o[0].id;
        arrNodeID = nodeID.split('_');
        businessRuleID = arrNodeID[1];
        dropBusinessRule(businessRuleID);
    }

    function businessRuleDropCheck(data) {
        var nodeID = data.o[0].id;
        var index = nodeID.indexOf("BR_");
        if (index != -1) {
            return true;
        }
        else {
            return false;
        }
    }

    function editPreCButton_click() {
        var pcID = $('[id$=hdnProjectID]').val();
        var tcID = $('[id$=hdnTestCaseID]').val();
        //clear hidden pc id field, it is populated when save pc is clicked
        var hdnPCIDs = $('input[id$=hdnPreConditionIDs]');
        hdnPCIDs.val("");
        //clear listbox before added option from db
        var listboxPreConditionsDisplay = $('select[id$=preConditionsDisplay]');
        var listboxPreConditions = $('.jstree-drop');
        listboxPreConditionsDisplay.empty();
        listboxPreConditions.empty();
        //display dialog
        $('#dlgPreCondition').dialog('open');
        $('#dlgPreCondition').parent().appendTo($("form:first"));
        populatePreCTestCasesTree();
        $.webMethod({ 'methodName': 'getPreConditionsForTestCase', 'parameters': { 'projID': pcID, 'tcID': tcID },
            success: function (text) {
                if (text != "") {
                    var arrPCList = text.split(",");
                    for (var i = 0; i < arrPCList.length; i = i + 2) {
                        listboxPreConditions.append('<option value=' + arrPCList[i + 1] + '>' + arrPCList[i] + '</option>');
                        listboxPreConditionsDisplay.append('<option value=' + arrPCList[i + 1] + '>' + arrPCList[i] + '</option>');
                    }
                }
            }
        });
    }

    function populatePreCTestCasesTree() {
        "use strict";
        var treeDIV,
            projectID,
            a,
            defaultID;
        projectID = $('input[id$=hdnProjectID]').val();
        defaultID = 0;
        //the treeDIV will contain the tree
        treeDIV = $('.treeTestCases2');
        treeDIV.addClass("JQueryDebug");
        treeDIV.jstree({
            "json_data": {
                "ajax": {
                    "type": "POST",
                    "dataType": "json",
                    "contentType": "application/json;",
                    "url": "TestCases.aspx/GetTestCasesForTree",
                    "data": function (node) {
                        if(node === -1) {
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
            "crrm": {
                "move": {
                    //prevent moving nodes around inside the tree
                    "check_move": function (m) {
                        return false;
                    }
                }
            },
            "dnd": {
                "drop_check": function (data) {
                    return testCaseDropCheck(data);
                },
                "drop_finish": function (data) {
                    testCaseDrop(data);
                }
            },
            "ui" : { select_multiple_modifier : false },
            "plugins": ["themes", "json_data", "crrm", "dnd", "ui"]
        })
    //.bind("select_node.jstree", function (NODE, REF_NODE) {
    //    a = $.jstree._focused().get_selected();
    //    alert("NodeID: " + a[0].id);
    //});
}

function testCaseDrop(data) {
    //get business rule and id from node data
    var nodeID, testCase, testCaseID;
    nodeID = data.o[0].id;
    arrNodeID = nodeID.split('_');
    testCaseID = arrNodeID[1];
    testCase = data.o[0].innerText;
    var listboxPreConditions = $('select[id$=listboxPreConditions]');
    //add new option to listbox
    listboxPreConditions.append('<option value="' + testCaseID + '">' + testCase + '</option>');
    checkEnableDisableRemovePreCTestCaseButton();
}

function testCaseDropCheck(data) {
    var nodeID = data.o[0].id;
    var index = nodeID.indexOf("TC_");
    if (index != -1) {
        return true;
    }
    else {
        return false;
    }
}

function btnRemovePreCTestCase_click() {
    //remove selected item(s) from listbox
    var listboxPreConditions = $('select[id$=listboxPreConditions]');
    listboxPreConditions.find('option:selected').remove();
    checkEnableDisableRemovePreCTestCaseButton();
}

function checkEnableDisableRemovePreCTestCaseButton() {
    var listboxPreConditions = $('.jstree-drop.listboxPreConditions');
    var brCount = listboxPreConditions.length;
    var removePreCTestCaseButton = $('input[id$=btnRemovePreCTestCase]');
    if (brCount == 0) {
        removePreCTestCaseButton.attr("disabled", "disabled");
    }
    else {
        removePreCTestCaseButton.removeAttr("disabled");
    }
}

function MoveTCUp_click() {
    var listboxPreConditions = $('.jstree-drop.listboxPreConditions'); 
    var direction = -1;
    moveTC(listboxPreConditions[0], direction);
}

function MoveTCDown_click() {
    var listboxPreConditions = $('.jstree-drop.listboxPreConditions');
    var direction = 1;
    moveTC(listboxPreConditions[0], direction);
}

function moveTC(el, dir) {
    var max = el.options.length;
    //  the selected option
    var i1 = el.selectedIndex;
    var o1 = el.options[i1];

    //  the option to switch places with
    var i2 = (i1 + dir) % max;
    if (i2 < 0) i2 = max - 1;
    var o2 = el.options[i2];
    //  temporarily move o1 to very end
    var tmp = el.options[max] = new Option(o1.text, o1.value);
    //  move o2 to o1
    el.options[i1] = new Option(o2.text, o2.value);
    //  move temp o1 to o2
    el.options[i2] = new Option(tmp.text, tmp.value);
    //  remove temp o1
    el.options.length = max;
    el.selectedIndex = i2;
}

function populatePreConditionsDisplayListbox() {
    var pcID = $('[id$=hdnProjectID]').val();
    var tcID = $('[id$=hdnTestCaseID]').val();
    //clear listbox before added option from db
    var listboxPreConditionsDisplay = $('select[id$=preConditionsDisplay]');
    listboxPreConditionsDisplay.empty();
    $.webMethod({ 'methodName': 'getPreConditionsForTestCase', 'parameters': { 'projID': pcID, 'tcID': tcID },
        success: function (text) {
            if (text != "") {
                var arrPCList = text.split(",");
                for (var i = 0; i < arrPCList.length; i = i + 2) {
                    listboxPreConditionsDisplay.append('<option value=' + arrPCList[i + 1] + '>' + arrPCList[i] + '</option>');
                }
            }
        }
    });
}

function dropBusinessRule(brID) {
    if (brID != "") {
        var pcID = $('[id$=hdnProjectID]').val();
        $.webMethod({ 'methodName': 'getBusinessRuleNameForID', 'parameters': { 'projID': pcID, 'brID': brID },
            success: function (name) {
                if (name != "" && name != null) {
                    var relatedBusinessRulesListBox = $('.jstree-drop.relatedBusinessRuleListbox');
                    //check for duplicates
                    if (relatedBusinessRulesListBox[0].innerHTML.indexOf(">" + name + "<") > -1) {
                        jAlert("The Business Rule and Test Step are already related.");
                    }
                    else {
                        //add new option to listbox
                        relatedBusinessRulesListBox.append('<option value="' + brID + '">' + name + '</option>');
                    }
                    checkEnableDisableRemoveRelatedBusinessRuleButton();
                }
            }
        });
    }
}


