/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/// <reference path="../../jquery-ui-1.9.0.min.js" />

/*jslint browser: true*/
/*global $, jQuery, jAlert, confirm*/

//set Global control variables
var CANCELFORM = false;

$(document).ready(function () {
    "use strict";
    initializeControls();
});

function reloadPage() {
    window.location.reload();
}

function addProjectButton_click(e) {
    $("input:asp(txtNewProjectName)").val("");
    showDialog(true);
    $('input[id$=hdnProjectID]').val("0");
    return false;
}

function saveProjectButton_click(e) {
    "use strict";
    var validationMessage, addProjectTextbox, invalidChars, projectText, re, retVal;
    validationMessage = "";
    re = new RegExp("[!@#$%&*_<>\///]");
    addProjectTextbox = $("input:asp(txtNewProjectName)");
    invalidChars = addProjectTextbox.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text!";
    }
    projectText = addProjectTextbox.val();
    if (projectText.length === 0) {
        validationMessage = "Project Name is required.";
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

function showDialog(visible) {
    if (visible) {
        $('#dlgProject').dialog('open');
        $('#dlgProject').parent().appendTo($("form:first"));
    }
    else {
        $('#dlgProject').dialog('close');
    }
}

function cancelProjectButton_click() {
    "use strict";
    $('#dlgProject').dialog('close');
}

function initializeControls() {
    "use strict";
    var addProjectButton, saveProjectButton, addProjectTextbox, cancelProjectButton, retVal;
    addProjectButton = $(":asp(btnNewProject)");
    addProjectButton.addClass("JQueryDebug");
    addProjectButton.click(function (e) { return addProjectButton_click(e); });
    saveProjectButton = $('input[id$=btnSaveProject]');
    saveProjectButton.click(function (e) {
        saveProjectButton_click(e);
    });
    saveProjectButton.addClass("JQueryDebug");
    addProjectTextbox = $("input:asp(txtNewProjectName)");
    addProjectTextbox.addClass("JQueryDebug");
    cancelProjectButton = $('input[id$=btnCancel]');
    cancelProjectButton.addClass("JQueryDebug");
    cancelProjectButton.click(function () { cancelProjectButton_click(); });
    //initialize dialog
    $('#dlgProject').dialog({
        autoOpen: false,
        modal: true,
        width: 450,
        open: function () { }
    });
}