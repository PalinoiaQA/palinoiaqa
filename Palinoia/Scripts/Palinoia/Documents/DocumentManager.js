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
    var config = {};
    var html = '';
    var editor = CKEDITOR.appendTo('editor', config, html);
    editor.config.height = 360;
    initializeControls();
    populateDocumentTree();
});

function setMode(mode) {
    "use strict";
    var editDIV, treeDIV, saveAddBusinessRuleButton, saveEditBusinessRuleButton, searchDIV;
    editDIV = $("#chapterEditDIV");
    treeDIV = $("#documentTreesDIV");
    searchDIV = $(".searchDIV");
    if (mode === "add") {
        editDIV.show();
        treeDIV.hide();
        searchDIV.hide();
        $("#PageTitle").text("Add Chapter");
        clearChapterText();
    } else if (mode === "edit") {
        editDIV.show();
        treeDIV.hide();
        searchDIV.hide();
        $("#PageTitle").text("Edit Chapter");
        setChapterControls();
    } else {
        treeDIV.show();
        searchDIV.show();
        editDIV.hide();
        $("#PageTitle").text("Document Manager");
    }
}

function initializeControls() {
    var documentTitleTextbox = $('.docTitle');
    documentTitleTextbox.addClass("JQueryDebug");
    var documentDescriptionTextbox = $('.docDescription');
    documentDescriptionTextbox.addClass("JQueryDebug");
    var addDocumentButton = $('input[id$=btnAddDocument]');
    addDocumentButton.addClass("JQueryDebug");
    addDocumentButton.unbind('click');
    addDocumentButton.click(function (e) {
        addDocumentButton_click(e);
    });
    var documentTypeDDL = $("select[id$=ddlDocumentType]");
    documentTypeDDL.addClass("JQueryDebug");
    documentTypeDDL.change(function (e) {
        storeSelectedIndex(e);
    });
    var newDocumentSaveButton = $('input[id$=btnNewDocumentSave]');
    newDocumentSaveButton.unbind('click');
    newDocumentSaveButton.click(function (e) {
        return newDocumentSaveButton_click(e);
    });
    var editDocumentSaveButton = $('input[id$=btnEditDocumentSave]');
    newDocumentSaveButton.unbind('click');
    newDocumentSaveButton.click(function (e) {
        return editDocumentSaveButton_click(e);
    });
    var newDocumentCancelButton = $('input[id$=btnNewDocumentCancel]');
    newDocumentCancelButton.unbind('click');
    newDocumentCancelButton.click(function (e) {
        return newDocumentCancelButton_click(e);
    });
    var dlg = $('#dlgAddDocument').dialog({
        autoOpen: false,
        modal: true,
        width: 400,
        open: function () { }
    });
    dlg.parent().appendTo(jQuery("form:first"));
    var saveChapterButton, addChapterButton, saveAddImage, cancelButton, doneButton,
        addBusinessRuleButton, saveEditBusinessRuleButton,
        saveAddBusinessRuleButton, openBRWindowButton, openCSMWindowButton;
    saveChapterButton = $(":asp(btnSaveChapter)");
    saveChapterButton.addClass("JQueryDebug");
    saveChapterButton.unbind('click');
    saveChapterButton.click(function (e) {
        return saveChapterButton_click(e);
    });
    addChapterButton = $(":asp(btnAddChapter)");
    addChapterButton.addClass("JQueryDebug");
    addChapterButton.unbind('click');
    addChapterButton.click(function (e) {
        return addChapterButton_click(e);
    });
    saveAddImage = $(":asp(btnSaveAddImage)");
    saveAddImage.addClass("JQueryDebug");
    saveAddImage.unbind('click');
    saveAddImage.click(function (e) {
        return saveAddImage_click(e);
    });
    cancelButton = $(":asp(btnAddImageCancel)");
    cancelButton.addClass("JQueryDebug");
    cancelButton.unbind('click');
    cancelButton.click(function (e) {
        return cancelButton_click(e);
    });
    doneButton = $(":asp(btnDone)");
    doneButton.addClass("JQueryDebug");
    doneButton.click(doneButton_Click);
    var dlg1 = $('#dlgAddImage').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        open: function () { }
    });
    dlg1.parent().appendTo(jQuery("form:first"));
    var editMode = $('input[id$=hdnChapterEditMode]').val();
    setMode(editMode);
}

function addDocumentButton_click(e) {
    $('input[id$=hdnDocumentID]').val("0");
    $('input[id$=hdnDocumentTypeID]').val("0");
    $('#dlgAddDocument').dialog('open');
    $('#dlgAddDocument').parent().appendTo($("form:first"));
}

function validateDocumentTextControls() {
    var validationMessage = "";
    var docTitleTextBox = $('input[id$=txtDocumentTitle]');
    var docDescriptionTextBox = $('.docDescription');
    //validate input
    re = new RegExp("[!@#$%&*_<>\///]");
    invalidChars = docTitleTextBox.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text in Document Title!";
    }
    else {
        invalidChars = docDescriptionTextBox.val().match(re);
        if (invalidChars) {
            validationMessage = "Invalid Text in Document Description!";
        }
    }
    if (docTitleTextBox.val().length == 0) {
        validationMessage = "Document title is required.";
    }
    if (docDescriptionTextBox.val().length == 0) {
        validationMessage = "Document description is required.";
    }
    var documentTypeDDL = $("select[id$=ddlDocumentType]");
    if (documentTypeDDL.val() == "select one") {
        validationMessage = "A selection is required for document type.";
    }
    //check for errors
    if (validationMessage.length > 0) {
        alert(validationMessage);
        return false;
    }
    else {
        return true;
    }
}

function validateChapterTextControls() {
    var validationMessage = "";
    var chapterNameTextBox = $('input[id$=txtChapterName]');
    //validate input
    re = new RegExp("[!@#$%&*_<>\///]");
    invalidChars = chapterNameTextBox.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text in Chapter Title!";
    }
    if (chapterNameTextBox.val().length == 0) {
        validationMessage = "Chapter title is required.";
    }
    //check for errors
    if (validationMessage.length > 0) {
        alert(validationMessage);
        return false;
    }
    else {
        return true;
    }
}

function editDocumentSaveButton_click(e) {
    if (validateDocumentTextControls()) {
        var id = $('input[id$=hdnDocumentID]').val();
        var title = $('input[id$=txtDocumentTitle]').val();
        var desc = $('.docDescription').val();
        var typeID = $('select[id$=ddlDocumentType]').val();
        var newDoc = new EditDocumentDetails(id, title, "", typeID, desc);
        saveEditDocumentDetails(newDoc);
        $('#dlgAddDocument').dialog('close');
    }
}

function newDocumentSaveButton_click(e) {
    if(validateDocumentTextControls()) { //no errors, go ahead with save
        var id = "0";
        var title = $('input[id$=txtDocumentTitle]').val();
        var desc = $('.docDescription').val();
        var typeID = $('select[id$=ddlDocumentType]').val();
        var newDoc = new EditDocumentDetails(id, title, "", typeID, desc);
        saveEditDocumentDetails(newDoc);
        $('#dlgAddDocument').dialog('close');
    }
}

function newDocumentCancelButton_click(e) {
    $('#dlgAddDocument').dialog('close');
}

function storeSelectedIndex(e) {
    var docTypeID = e.srcElement.selectedIndex;
    $('input[id$=hdnDocumentTypeID]').val(docTypeID);
}

function populateDocumentTree() {
    "use strict";
    var projectID, treeDIV, a, defaultID;
    defaultID = 0;
    //the treeDIV will contain the tree
    projectID = $('input[id$=hdnProjectID]').val();
    treeDIV = $('.treeFull');
    treeDIV.addClass("JQueryDebug");
    treeDIV.jstree({
        "json_data": {
            "ajax": {
                "type": "POST",
                "dataType": "json",
                "contentType": "application/json;",
                "url": "DocumentManager.aspx/GetDocumentsForTree",
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
        "types": {
            valid_children: ["Document", "Section", "DocumentType", "Root"],
            "types": {
                "Document": {
                    "start_drag": false
                },
                "Section": {
                    "start_drag": false
                },
                "DocumentType": {
                    "start_drag": false
                },
                "Root": {
                    "start_drag": false
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
                //only allow moves that reorder the chapters
                "check_move": function (m) {
                    var p = this._get_parent(m.o);
                    if (!p) return false;
                    p = p == -1 ? this.get_container() : p;
                    if (p === m.np) return true;
                    if (p[0] && m.np[0] && p[0] === m.np[0]) return true;
                    return false;
                }
            }
        },
        "contextmenu": {
            "items": customMenu,
            "select_node": true
        },
        "cookies": { cookie_options: { path: '/'} },
        "plugins": ["themes", "json_data", "crrm", "contextmenu", "dnd", "ui", "types", "cookies"]
    })
        .bind("move_node.jstree", function (e, data) {
            var moveNodeID, parentID, chapters, documentID;
            var idList = new Array();
            moveNodeID = data.rslt.o[0].id;
            parentID = data.rslt.cr[0].id;
            var childNodes = $('#DocumentTree li[id=' + parentID + ']  li');
            $.each(childNodes, function (idx, node) {
                var idArray1 = node.id.split("_");
                idList[idx] = idArray1[1];
            });
            var idArray2 = parentID.split("_");
            documentID = idArray2[1];
            saveChapterOrder(documentID, idList);
        })
        .bind("select_node.jstree", function (NODE, REF_NODE) {
            //a = $.jstree._focused().get_selected();
            //alert("NodeID: " + a[0].id);
        });
    }

    function saveChapterOrder(documentID, childIDList) {
        //call server webmethod to save new chapter order for document
        var pcID = $('[id$=hdnProjectID]').val();
        var docID = $('[id$=hdnDocumentID]').val();
        var userID = $('input[id$=hdnUserID]').val();
        $.webMethod({ 'methodName': 'saveChapterOrder', 'parameters': { 'projID': pcID, 'userID': userID, 'docID': documentID, 'children': childIDList },
            success: function (result) {
                //do nothing
            },
            "error": function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Error Saving Document: " + errorThrown);
            }
        });
    }

    function customMenu(node) {
        "use strict";
        var rootIndex, documentIndex, descIndex, dtypeIndex, tcDocType, sectionIndex, parentIndex,
            parentNode, disableDocumentEdit, disableDocumentAdd, disableDocumentDelete, testCaseIndex,
            disableDocumentView, items, retVal, chapterIndex, disableChapterEdit, disableChapterAdd,
            disableChapterView, disableChapterDelete;
        tcDocType = false;
        rootIndex = node[0].id.indexOf("ROOT_");
        documentIndex = node[0].id.indexOf("DOC_");
        descIndex = node[0].id.indexOf("DES_");
        dtypeIndex = node[0].id.indexOf("DTYP_");
        chapterIndex = node[0].id.indexOf("CHP_");
        testCaseIndex = node[0].id.indexOf("TCD_");
        if(dtypeIndex > -1) {
            var id = node[0].id;
            var idArray = id.split('_');
            var id2 = idArray[1];
            if(id2 == "3") {
                tcDocType = true;
            }
        }
        if (rootIndex > -1 || sectionIndex > -1 || dtypeIndex > -1) {
            //USER RIGHT CLICKED A SECTION NODE, DOCUMENT TYPE NODE OR THE ROOT NODE
            disableDocumentAdd = $('input[id$=hdnDisableDocumentAdd]').val();
            // add all items
            items = {
                addItem: {
                    label: "Add Document",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        addDocumentAction(nodeID);
                    }
                },
            }
            //remove items for node types
            if ((rootIndex > -1 || dtypeIndex > -1) && tcDocType == false) { 

            }
            else { 
                // user right clicked a section node or the Test Case doc type
                delete items.addItem;
            }
            //apply features to any items remaining
            if (disableDocumentAdd == "true") {
                if (items.addItem != null) {
                    delete items.addItem;
                }
            }
            retVal = items;
        }
        else if (documentIndex > -1) {
            //USER RIGHT CLICKED A DOCUMENT NODE
            storeDocumentID(node);
            disableDocumentEdit = $('input[id$=hdnDisableDocumentEdit]').val();
            disableDocumentDelete = $('input[id$=hdnDisableDocumentDelete]').val();
            disableDocumentAdd = $('input[id$=hdnDisableDocumentAdd]').val();
            disableDocumentView = $('input[id$=hdnDisableDocumentView]').val();
            //add all items
            items = {
                addItem: {
                    label: "Add Chapter",
                    action: function (NODE, REF_NODE) {
                        addChapterAction(NODE);
                    }
                },
                editItem: {
                    label: "Edit Document",
                    action: function (NODE, REF_NODE) {
                        editDocumentAction(NODE);
                    }
                },
                deleteItem: {
                    label: "Delete Document",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        deleteDocumentAction(nodeID);
                    }
                },
                viewItem: {
                    label: "View Document",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        viewDocumentAction(nodeID);
                    }
                }
            }
            //apply features
            if (disableDocumentAdd == "true") {
                if (items.addItem != null) {
                    delete items.addItem;
                }
            }
            if (disableDocumentEdit == "true") {
                if (items.editItem != null) {
                    delete items.editItem;
                }
            }
            if (disableDocumentDelete == "true") {
                if (items.deleteItem != null) {
                    delete items.deleteItem;
                }
            }
            if (disableDocumentView == "true") {
                if (items.viewItem != null) {
                    delete items.viewItem;
                }
            }
            retVal = items;
        }
        else if (testCaseIndex > -1) {
            //USER RIGHT CLICKED A TESTCASE NODE
            disableDocumentView = $('input[id$=hdnDisableDocumentView]').val();
            items = {
                viewItem: {
                    label: "View Document",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        viewTCDocumentAction(nodeID);
                    }
                }
            }
            //apply features
            if (disableDocumentView == "true") {
                if (items.viewItem != null) {
                    delete items.viewItem;
                }
            }
            retVal = items;
        }
        else if (chapterIndex > -1) {
            //USER RIGHT CLICKED A CHAPTER NODE
            disableChapterEdit = $('input[id$=hdnDisableChapterEdit]').val();
            disableChapterDelete = $('input[id$=hdnDisableChapterDelete]').val();
            disableChapterAdd = $('input[id$=hdnDisableChapterAdd]').val();
            disableChapterView = $('input[id$=hdnDisableChapterView]').val();
            items = {
                editItem: {
                    label: "Edit Chapter",
                    action: function (NODE, REF_NODE) {
                        editChapterAction(NODE);
                    }
                },
                deleteItem: {
                    label: "Delete Chapter",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        deleteChapterAction(nodeID);
                    }
                },
                viewItem: {
                    label: "View Chapter",
                    action: function (NODE, REF_NODE) {
                        var nodeID = $(NODE).attr("id");
                        viewChapterAction(nodeID);
                    }
                }
            }
            //determine target node of right click
            if (chapterIndex > -1) {
                delete items.addItem;
            }
            //apply features
            if (disableChapterEdit == "true") {
                if (items.editItem != null) {
                    delete items.editItem;
                }
            }
            if (disableChapterDelete == "true") {
                if (items.deleteItem != null) {
                    delete items.deleteItem;
                }
            }
            if (disableChapterView == "true") {
                if (items.viewItem != null) {
                    delete items.viewItem;
                }
            }
            retVal = items;
        }
        return retVal;
    }

    function storeDocumentID(node) {
        var nodeArray;
        //check if node a document node
        nodeArray = node[0].id.split("_");
        objType = nodeArray[0];
        objID = nodeArray[1];
        //if doc node, store doc id in hidden field
        if (objType == "DOC") {
            $('[id$=hdnDocumentID]').val(objID);
        }
    }

    function addDocumentAction(nodeID) {
        saveSearchInfo();
        //clear hidden doc id field
        $('input[id$=hdnDocumentID]').val("0");
        $('input[id$=hdnDocumentTypeID]').val("0");
        $('#dlgAddDocument').dialog('open');
        $('#dlgAddDocument').parent().appendTo($("form:first"));
    }

    function editDocumentAction(NODE) {
        var nodeID, documentIndex, descriptionIndex, editDocumentButton, editDocumentDetails;
        // pull id from NODE
        nodeID = $(NODE).attr("id");
        //verity that this is a section node
        documentIndex = nodeID.indexOf("DOC_");
        descriptionIndex = nodeID.indexOf("DES_");
        if ((documentIndex > -1) || (descriptionIndex > -1)) {
            //get document id from node id
            nodeArray = nodeID.split("_");
            documentID = nodeArray[1];
            //populate hidden fields for test case add
            $('input[id$=hdnDocumentID]').val(documentID);
            var editDocumentDetails = loadEditDocumentDetails();
            loadEditDocumentControls(editDocumentDetails);
        }
    }

    function loadEditDocumentControls(editDocumentDetails) {
        $('input[id$=txtDocumentTitle]').val(editDocumentDetails.title);
        $('.docDescription').val(editDocumentDetails.desc);
        $('select[id$=ddlDocumentType]').val(editDocumentDetails.typeID);
        $("span.ui-dialog-title").text('Edit Document');
        $('#dlgAddDocument').dialog('open');
        $('#dlgAddDocument').parent().appendTo($("form:first"));
    }

    function deleteDocumentAction(nodeID) {
        var nodeID, documentIndex, descriptionIndex, editDocumentButton
        //verity that this is a document node
        documentIndex = nodeID.indexOf("DOC_");
        if ((documentIndex > -1)) {
            var verifyDelete = confirm("Are you sure you want to delete this document?");
            if (verifyDelete) {
                //get document id from node id
                nodeArray = nodeID.split("_");
                documentID = nodeArray[1];
                //populate hidden fields for test case add
                $('input[id$=hdnDocumentID]').val(documentID);
                var deleteDocumentButton = $('input[id$=btnDeleteDocument]');
                deleteDocumentButton.click();
            }
        }
    }

    function viewDocumentAction(nodeID) {
        var nodeID, documentIndex, descriptionIndex, editDocumentButton, projectID
        //verity that this is a document node
        documentIndex = nodeID.indexOf("DOC_");
        descriptionIndex = nodeID.indexOf("DES_");
        if ((documentIndex > -1) || (descriptionIndex > -1)) {
            //get document id from node id
            nodeArray = nodeID.split("_");
            documentID = nodeArray[1];
            $('input[id$=hdnDocumentID]').val(documentID);
            projectID = $('[id$=hdnProjectID]').val();
            //get URL for ShowODF
            $.webMethod({ 'methodName': 'getURLForViewDocument', 'parameters': { 'pid': projectID, 'did': documentID, 'cid': 0, 'tcid':0 },
                success: function (result) {
                    window.open(result);
                },
                "error": function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error Saving Document: " + errorThrown);
                }
            });
        }
    }

    function viewTCDocumentAction(nodeID) {
        var nodeID, documentIndex, descriptionIndex, editDocumentButton, projectID, testCaseID
        //verity that this is a document node
        testCaseIndex = nodeID.indexOf("TCD_");
        if (testCaseIndex > -1) {
            //get document id from node id
            nodeArray = nodeID.split("_");
            testCaseID = nodeArray[1];
            $('input[id$=hdnTestCaseID]').val(testCaseID);
            projectID = $('[id$=hdnProjectID]').val();
            //get URL for ShowODF
            $.webMethod({ 'methodName': 'getURLForViewDocument', 'parameters': { 'pid': projectID, 'did':0, 'cid':0, 'tcid':testCaseID  },
                success: function (result) {
                    window.open(result);
                },
                "error": function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error Saving Document: " + errorThrown);
                }
            });
        }
    }

    function viewChapterAction(nodeID) {
        var viewChapterButton, chapterID, documentIndex, nodeArray, projectID;
        documentIndex = nodeID.indexOf("CHP_");
        if (documentIndex > -1) {
            //get chapter id from node id
            nodeArray = nodeID.split("_");
            chapterID = nodeArray[1];
            $('input[id$=hdnChapterID]').val(chapterID);
            projectID = $('[id$=hdnProjectID]').val();
            documentID = $('[id$=hdnDocumentID]').val();
            //get URL for ShowODF
            $.webMethod({ 'methodName': 'getURLForViewDocument', 'parameters': { 'pid': projectID, 'did': documentID, 'cid': chapterID, 'tcid':0 },
                success: function (result) {
                    window.open(result);
                },
                "error": function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error viewing chapter: " + errorThrown);
                }
            });
        }
    }

    function addChapterAction(NODE) {
        saveSearchInfo();
        var nodeID, documentIndex, documentID, nodeArray
        // pull id from NODE
        nodeID = $(NODE).attr("id");
        //verity that this is a section node
        documentIndex = nodeID.indexOf("DOC_");
        if (documentIndex > -1) {
            //get document id from node id
            nodeArray = nodeID.split("_");
            documentID = nodeArray[1];
            //populate hidden fields for doc id
            $('input[id$=hdnDocumentID]').val(documentID);
            $('input[id$=hdnChapterID]').val("0");
            setMode("add");
        }
    }

    function deleteChapterAction(nodeID) {
        var nodeID, chapterIndex, deleteChapterButton
        //verify that this is a chapter node
        chapterIndex = nodeID.indexOf("CHP_");
        if ((chapterIndex > -1)) {
            var verifyDelete = confirm("Are you sure you want to delete this chapter?");
            if (verifyDelete) {
                //get chapter id from node id
                nodeArray = nodeID.split("_");
                chapterID = nodeArray[1];
                //populate hidden field  
                $('input[id$=hdnChapterID]').val(chapterID);
                deleteChapterButton = $('input[id$=btnDeleteChapter]');
                deleteChapterButton.click();
            }
        }
    }

    function editChapterAction(NODE) {
        saveSearchInfo();
        var nodeID, documentIndex, descriptionIndex, editChapterButton
        // pull id from NODE
        nodeID = $(NODE).attr("id");
        //verity that this is a section node
        documentIndex = nodeID.indexOf("CHP_");
        if (documentIndex > -1) {
            //get document id from node id
            nodeArray = nodeID.split("_");
            chapterID = nodeArray[1];
            //populate hidden fields for test case add
            $('input[id$=hdnChapterID]').val(chapterID);
            // call button click to trigger postback
            setMode("edit");
        }
    }

    function EditDocumentDetails(id, title, type, typeID, desc) {
        this.desc = desc;
        this.id = id;
        this.title = title;
        this.type = type;
        this.typeID = typeID;
    }

    function loadEditDocumentDetails() {
        var editDocumentDetails;
        var pcID = $('[id$=hdnProjectID]').val();
        var docID = $('[id$=hdnDocumentID]').val();
        $.webMethod({ 'methodName': 'getEditDetailsForDocument', 'parameters': { 'projID': pcID, 'docID': docID },
            success: function (response) {
                editDocumentDetails = response;
            },
            "error": function (XMLHttpRequest, textStatus, errorThrown) {
                alert("err: " + errorThrown);
            }
        });
        return editDocumentDetails;
    }

    function saveEditDocumentDetails(details) {
        var pcID = $('[id$=hdnProjectID]').val();
        var docID = $('[id$=hdnDocumentID]').val();
        var userID = $('input[id$=hdnUserID]').val();
        $.webMethod({ 'methodName': 'saveEditDocumentDetails', 'parameters': { 'projID': pcID, 'userID': userID, 'details': details },
            success: function (result) {
                populateDocumentTree();
            },
            "error": function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Error Saving Document: " + errorThrown);
            }
        });
    }

    function onNodeSelected(node) {

    }

    function doneButton_Click() {
        $('input[id$=hdnChapterEditMode]').val("");
        $('input[id$=hdnChapterID]').val("");
        setMode("");
    }

    function saveAddImage_click(e) {
        var retValue = false;
        var projectID = $('input[id$=hdnProjectID]').val();
        var fileUploadControl = $('.FileUpload');
        var fileName = fileUploadControl[0].value;
        $.webMethod({ 'methodName': 'getImageHTML', 'parameters': { 'pid': projectID, 'fn': fileName },
            success: function (response) {
                var data = response;
                insertHTML(data);
                saveChapterButton_click(e);
                retValue = true;
            }
        });
        return retValue;
    }

    function cancelButton_click(e) {
        $('#dlgAddImage').dialog('close');
    }

    function saveChapterButton_click(e) {
        if (validateChapterTextControls()) {
            var editor2;
            for (instance in CKEDITOR.instances) {
                editor2 = CKEDITOR.instances[instance];
            }
            var userID = $('input[id$=hdnUserID]').val();
            var documentID = $('input[id$=hdnDocumentID]').val();
            var chapterID = $('input[id$=hdnChapterID]').val();
            var projectID = $('input[id$=hdnProjectID]').val();
            var title = $('input[id$=txtChapterName]').val();
            var text = editor2.getData();
            //TODO: add text validation here
            $.webMethod({ 'methodName': 'saveChapterText', 'parameters': { 'did': documentID,
                'cid': chapterID,
                'pid': projectID,
                'title': title,
                'text': text,
                'uid': userID
            },
                success: function (response) {
                    $('input[id$=hdnChapterID]').val(response);
                    if (chapterID == "0") {
                        window.location.reload(true);
                    }
                }
            });
        }
        return false;
    }

    function insertHTML(data) {
        var editor2;
        for (instance in CKEDITOR.instances) {
            editor2 = CKEDITOR.instances[instance];
        }
        if (editor2 != null) {
            editor2.focus();
            editor2.insertHtml(data);
        }
    }

    function setChapterControls() {
        var editor2, chapterTitleTextbox;
        for (var instance in CKEDITOR.instances) {
            editor2 = CKEDITOR.instances[instance];
        }
        chapterTitleTextbox = $('input[id$=txtChapterName]');
        var chapterID = $('input[id$=hdnChapterID]').val();
        var projectID = $('input[id$=hdnProjectID]').val();
        $.webMethod({ 'methodName': 'getChapterText', 'parameters': { 'cid': chapterID, 'pid': projectID },
            success: function (text) {
                editor2.setData(text);
            }
        });
        $.webMethod({ 'methodName': 'getChapterTitle', 'parameters': { 'cid': chapterID, 'pid': projectID },
            success: function (title) {
                chapterTitleTextbox.val(title);
            }
        });
    }

    function clearChapterText() {
        var editor2;
        for (var instance in CKEDITOR.instances) {
            editor2 = CKEDITOR.instances[instance];
        }
        editor2.setData("");
        //clear chapter title
        $('input[id$=txtChapterName]').val("");
    }

    function showAddImageDialog() {
        $('#dlgAddImage').dialog('open');
        var descriptionTextbox = $('.imageDescText');
        descriptionTextbox.val("");
        $('#dlgAddImage').parent().appendTo($("form:first"));
    }