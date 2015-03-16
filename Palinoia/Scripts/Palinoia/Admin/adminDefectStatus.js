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

function addDefectStatusButton_click(e) {
    $("input:asp(txtAddDefectStatus)").val("");
    var activeCheckbox = $(':checkbox');
    activeCheckbox.prop('checked', true);
    showDialog(true);
    $('input[id$=hdnDefectStatusID]').val("0");
    setTimeout("$('input:asp(txtAddDefectStatus)').focus();", 100);
}

function showDialog(visible) {
    if (visible) {
        $('#dlgDefectStatus').dialog('open');
        $('#dlgDefectStatus').parent().appendTo($("form:first"));
    }
    else {
        $('#dlgDefectStatus').dialog('close');
    }
}

function saveDefectStatusButton_click(e) {
    "use strict";
    var validationMessage, invalidChars, DefectStatusText, addDefectStatusTextbox, re, retVal;
    validationMessage = "";
    addDefectStatusTextbox = $("input:asp(txtAddDefectStatus)");
    re = new RegExp("[!@#$%&*_<>\///]");
    invalidChars = addDefectStatusTextbox.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text!";
    }
    DefectStatusText = addDefectStatusTextbox.val();
    if (DefectStatusText.length === 0) {
        validationMessage = "Defect Status is required.";
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

function cancelDefectStatusButton_click() {
    "use strict";
    var addDefectStatusTextbox;
    addDefectStatusTextbox = $("input:asp(txtAddDefectStatus)");
    addDefectStatusTextbox.val("");
    showDialog(false);
}

function initializeControls() {
    "use strict";
    var saveDefectStatusButton, addDefectStatusTextbox,
        cancelDefectStatusButton, retVal, addDefectStatusButton;
    saveDefectStatusButton = $('input[id$=btnSaveDefectStatus]');
    saveDefectStatusButton.click(function (e) {
        saveDefectStatusButton_click(e);
    });
    saveDefectStatusButton.addClass("JQueryDebug");
    addDefectStatusTextbox = $("input:asp(txtAddDefectStatus)");
    addDefectStatusTextbox.addClass("JQueryDebug");
    addDefectStatusButton = $('input[id$=btnAddDefectStatus]');
    addDefectStatusButton.addClass("JQueryDebug");
    addDefectStatusButton.click(function (e) { addDefectStatusButton_click(e) });
    cancelDefectStatusButton = $('input[id$=btnCancel]');
    cancelDefectStatusButton.addClass("JQueryDebug");
    cancelDefectStatusButton.click(function () { cancelDefectStatusButton_click(); });
    //initialize dialog
    $('#dlgDefectStatus').dialog({
        autoOpen: false,
        modal: true,
        width: 450,
        open: function () { }
    });
}