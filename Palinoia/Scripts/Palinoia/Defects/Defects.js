/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/*jslint browser: true*/
/*global $, jQuery, jAlert, confirm*/

$(document).ready(function () {
    "use strict";
    initializeControls();

});

$(function () {
    $("#txtCreated").datepicker();
    $("#txtCompleted").datepicker();
});

function setMode(mode) {
    "use strict";
    var editDIV, gridDIV, saveAddDefectButton, saveEditDefectButton, searchDIV;
    editDIV = $("#defectDetailsDIV");
    gridDIV = $("#searchResultsDIV");
    searchDIV = $(".searchDIV");
    if (mode === "add") {
        editDIV.show();
        gridDIV.hide();
        searchDIV.hide();
        $('input[id$=hdnDefectID]').val("0");
        $("#PageTitle").text("Add Defect");

    } else if (mode === "edit") {
        $(".CommentControl").show();
        $(".calDateCreated").show();
        $(".IDControl").show();
        $(".calDateCreated").val($('input[id$=hdnDateCreated]').val());
        $(".calDateCompleted").val($('input[id$=hdnDateCompleted]').val()); 
        editDIV.show();
        gridDIV.hide();
        searchDIV.hide();
        $("#PageTitle").text("Edit Defect");
        
    } else {
        gridDIV.show();
        searchDIV.show();
        editDIV.hide();
        $('input[id$=hdnDefectID]').val("");
        $("#PageTitle").text("Defects");
    }
}

function initializeControls() {
    var saveCommentButton = $('input[id$=btnSaveComment]');
    saveCommentButton.addClass("JQueryDebug");
    saveCommentButton.click(function (e) {
        btnSaveComment_click(e);
    });
    var saveButton = $('input[id$=btnSave]');
    saveButton.addClass("JQueryDebug");
    saveButton.click(function (e) {
        return btnSave_click(e);
    });
    var closeButton = $('input[id$=btnClose]');
    closeButton.addClass("JQueryDebug");
    //closeButton.click(btnClose_Click); 

    //determine if search div or defect details div should be showing
    var defectID = $('input[id$=hdnDefectID]').val();
    if (defectID == null) {
        showDefectDetails("False");
    }
    else {
        if (defectID > 0) {
            showDefectDetails("True");
        }
        else {
            showDefectDetails("False");
        }
    }
    var addDefectButton = $('input[id$=btnAddDefect]');
    addDefectButton.addClass("JQueryDebug");
    addDefectButton.click(btnAddDefect_Click);
    $(".calDateCompleted").change(function (e) { $("[id$='hdnDateCompleted']").val($(".calDateCompleted").val()); jAlert("dateCompleted change: " + $(".calDateCompleted").val()); });
    $(".calDateCreated").change(function (e) { $("[id$='hdnDateCreated']").val($("#txtCreated").val()); jAlert("dateCreated change: " + $("#txtCreated").val()); });
    //initialize comments
    var addCommentButton = $('input[id$=btnAddComment]');
    addCommentButton.addClass("JQueryDebug");
    addCommentButton.click(btnAddComment_Click);
    var cancelButton = $('input[id$=btnCancel]');
    cancelButton.addClass("JQueryDebug");
    cancelButton.click(cancelButton_Click);
    //initialize dialog
    $('#dlgAddComment').dialog({
        autoOpen: false,
        modal: true,
        width: 450,
        height: 350,
        open: function () { }
    });
    var editor = CKEDITOR;
    editor.config.height = 100;
}

function btnAddDefect_Click() {
    $(".CommentControl").hide();
    $(".calDateCreated").hide();
    $(".IDControl").hide();
    clearDefectDetailsControls();
    setMode("add");
}

function btnClose_Click() {
    //showDefectDetails("False");
    setMode("");
    var defectID = $('input[id$=hdnDefectID]').val("0");
    return false;
}

function clearDefectDetailsControls() {
    $('input[id$=txtName]').val("");
    $(".descriptionControl").addClass("JQueryDebug");
    $(".descriptionControl").val("");
    $('input[id$=txtCompleted]').val("");
    $(".detailDDL").val("0");
    $(".detailDDL").addClass("JQueryDebug");
}

function showDefectDetails(visible) {
    var searchDIV = $(".searchDIV");
    var searchResultsDIV = $("#searchResultsDIV");
    var detailsDIV = $("#defectDetailsDIV");
    if (visible == "True") {
        detailsDIV.show();
        searchDIV.hide();
        searchResultsDIV.hide();
    }
    else {
        detailsDIV.hide();
        searchDIV.show();
        searchResultsDIV.show();
    }
}

function btnAddComment_Click() {
    showDialog(true);
}

function cancelButton_Click() {
    showDialog(false);
}

function showDialog(visible) {
    if (visible) {
        var commentTextbox = $(".addCommentTextbox");
        commentTextbox.addClass("JQueryDebug");
        commentTextbox.val("");
        $('#dlgAddComment').dialog('open');
        $('#dlgAddComment').parent().appendTo($("form:first"));
        commentTextbox.focus();
    }
    else {
        $('#dlgAddComment').dialog('close');
    }
}

function btnSaveComment_click(e) {
    var invalidChars, re, validationMessage, addCommentTextbox, retVal;
    validationMessage = "";
    re = new RegExp("[!@$%&*_<>\///]");
    addCommentTextbox = $(".addCommentTextbox");
    if (addCommentTextbox.val().length == 0) {
        validationMessage = "Comment text is required!";
    }
    else {
        invalidChars = addCommentTextbox.val().match(re);
        if (invalidChars) {
            validationMessage = "Invalid Text!";
        }
    }
    if (validationMessage.length > 0) {
        e.preventDefault();
        jAlert(validationMessage);
        retVal = false;
    } else {
        retVal = true;
    }
    return retVal;
}

function validateDefect() {
    var invalidCharacters = "";
    var valid = true;
    var ownerDDL = $(".ddlOwner");
    var priorityDDL = $(".ddlPriority");
    var statusDDL = $(".ddlStatus");
    var typeDDL = $(".ddlType");
    var re = new RegExp("[!@#$%&*_<>\///]");
    //var descText = $(".descriptionControl").val();
    var descText = CKEDITOR.instances.MainContent_CKEditor1.getData();
    var defectName = $('input[id$=txtName]').val();
    //validate required fields
    if($('input[id$=txtName]').val().length == 0) {
        jAlert("Name is a required field.");
        valid = false;
    }
    else if (descText.length == 0 || descText == "") {
        jAlert("Description is required.");
        valid = false;
    }
    else if (ownerDDL.val() == "0") {
        jAlert("Owner is a required field.");
        valid = false;
    }
    else if (priorityDDL.val() == "0") {
        jAlert("Priority is a required field.");
        valid = false;
    }
    else if (typeDDL.val() == "0") {
        jAlert("Type is a required field.");
        valid = false;
    }
    else if (statusDDL.val() == "0") {
        jAlert("Status is a required field.");
        valid = false;
    }
    //validate text
    if(valid) {
        if (defectName.match(re)) {
            jAlert("Invalid Text in defect name!");
            valid = false;
        }
        //Can't validate description this was since it contains html code
//        if(descText.match(re)) {
//            jAlert("Invalid Text in defect description!");
//            valid = false;
//        }
    }
    return valid;
}

function btnSave_click(e) {
    return validateDefect();
}

