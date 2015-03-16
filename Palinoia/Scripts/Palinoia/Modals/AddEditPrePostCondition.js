/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/*jslint browser: true*/
/*global $, jQuery, alert, confirm*/

$(document).ready(function () {
    "use strict";
    var newPCButton, savePCButton;
    newPCButton = $(":asp(btnNew)");
    newPCButton.click(newPCButton_click);
    newPCButton.addClass("JQueryDebug");
    savePCButton = $(":asp(btnSave)");
    savePCButton.click(savePCButton_click);
    savePCButton.addClass("JQueryDebug");
    setEditMode();
});

function setEditMode() {
    var editMode, objMode, existingPCDIV, newPCDIV, pageTitle, title;
    editMode = $('input[id$=hdnEditMode').val();
    objMode = $('input[id$=hdnObjMode').val();
    existingPCDIV = $("#divExistingPC");
    newPCDIV = $("#divNewPC");
    pageTitle = $("#lblPCTitle");
    if (editMode == "New") {
        existingPCDIV.hide();
        newPCDIV.show();
        title = title + "New ";
    }
    else {
        title = title + "Edit ";
        existingPCDIV.show();
        newPCDIV.hide();
    }
    if (objMode == "Pre") {
        title = title + "Pre Conditions";
    }
    else {
        title = title + "Post Conditions";
    }
    pageTitle.text = title;
}

function newPCButton_click() {
    hdnEditMode = $('input[id$=hdnEditMode');
    hdnEditMode.val("New");
    setEditMode();
}

function savePCButton_click() {

}



