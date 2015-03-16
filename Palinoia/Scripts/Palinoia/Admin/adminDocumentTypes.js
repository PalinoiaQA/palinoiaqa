/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/*jslint browser: true*/
/*global $, jQuery, jAlert, confirm*/

//set Global control variables
var CANCELFORM = false;

$(document).ready(function () {
    "use strict";
    initializeControls();
});

function addDocumentTypeButton_click(e) {
    $("input:asp(txtAddDocumentType)").val("");
    var activeCheckbox = $(':checkbox');
    activeCheckbox.prop('checked', true);
    showDialog(true);
    $('input[id$=hdnDocumentTypeID]').val("0");
    setTimeout("$('input:asp(txtAddDocumentType)').focus();", 100);
}

function showDialog(visible) {
    if (visible) {
        $('#dlgDocumentType').dialog('open');
        $('#dlgDocumentType').parent().appendTo($("form:first"));
    }
    else {
        $('#dlgDocumentType').dialog('close');
    }
}

function saveDocumentTypeButton_click(e) {
    "use strict";
    var validationMessage, invalidChars, documentTypeText, addDocumentTypeTextbox, re, retVal;
    validationMessage = "";
    addDocumentTypeTextbox = $("input:asp(txtAddDocumentType)");
    re = new RegExp("[!@#$%&*_<>\///]");
    invalidChars = addDocumentTypeTextbox.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text!";
    }
    documentTypeText = addDocumentTypeTextbox.val();
    if (documentTypeText.length === 0) {
        validationMessage = "Document Type is required.";
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

function cancelDocumentTypeButton_click() {
    "use strict";
    var addDocumentTypeTextbox;
    addDocumentTypeTextbox = $("input:asp(txtAddDocumentType)");
    addDocumentTypeTextbox.val("");
    showDialog(false);
}

function initializeControls() {
    "use strict";
    var saveDocumentTypeButton, addDocumentTypeTextbox, 
        cancelDocumentTypeButton, retVal, addDocumentTypeButton;
    saveDocumentTypeButton = $('input[id$=btnSaveDocumentType]');
    saveDocumentTypeButton.click(function (e) {
        saveDocumentTypeButton_click(e);
    });
    saveDocumentTypeButton.addClass("JQueryDebug");
    addDocumentTypeTextbox = $("input:asp(txtAddDocumentType)");
    addDocumentTypeTextbox.addClass("JQueryDebug");
    addDocumentTypeButton = $('input[id$=btnAddDocumentType]');
    addDocumentTypeButton.addClass("JQueryDebug");
    addDocumentTypeButton.click(function(e) { addDocumentTypeButton_click(e) });
    cancelDocumentTypeButton = $('input[id$=btnCancel]');
    cancelDocumentTypeButton.addClass("JQueryDebug");
    cancelDocumentTypeButton.click(function () { cancelDocumentTypeButton_click(); });
    //initialize dialog
    $('#dlgDocumentType').dialog({
        autoOpen: false,
        modal: true,
        width: 450,
        open: function () { }
    });
}