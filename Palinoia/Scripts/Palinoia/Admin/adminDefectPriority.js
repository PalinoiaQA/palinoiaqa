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

function addDefectPriorityButton_click(e) {
    $("input:asp(txtAddDefectPriority)").val("");
    var activeCheckbox = $(':checkbox');
    activeCheckbox.prop('checked', true);
    showDialog(true);
    $('input[id$=hdnDefectPriorityID]').val("0");
    setTimeout("$('input:asp(txtAddDefectPriority)').focus();", 100);
    return false;
}

function showDialog(visible) {
    if (visible) {
        $('#dlgDefectPriority').dialog('open');
        $('#dlgDefectPriority').parent().appendTo($("form:first"));
    }
    else {
        $('#dlgDefectPriority').dialog('close');
    }
}

function saveDefectPriorityButton_click(e) {
    "use strict";
    var validationMessage, invalidChars, DefectPriorityText, addDefectPriorityTextbox, re, retVal;
    validationMessage = "";
    addDefectPriorityTextbox = $("input:asp(txtAddDefectPriority)");
    re = new RegExp("[!@#$%&*_<>\///]");
    invalidChars = addDefectPriorityTextbox.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text!";
    }
    DefectPriorityText = addDefectPriorityTextbox.val();
    if (DefectPriorityText.length === 0) {
        validationMessage = "Defect Priority is required.";
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

function cancelDefectPriorityButton_click() {
    "use strict";
    var addDefectPriorityTextbox;
    addDefectPriorityTextbox = $("input:asp(txtAddDefectPriority)");
    addDefectPriorityTextbox.val("");
    showDialog(false);
}

function initializeControls() {
    "use strict";
    var saveDefectPriorityButton, addDefectPriorityTextbox,
        cancelDefectPriorityButton, addDefectPriorityButton;
    saveDefectPriorityButton = $('input[id$=btnSaveDefectPriority]');
    saveDefectPriorityButton.click(function (e) {
        saveDefectPriorityButton_click(e);
    });
    saveDefectPriorityButton.addClass("JQueryDebug");
    addDefectPriorityTextbox = $("input:asp(txtAddDefectPriority)");
    addDefectPriorityTextbox.addClass("JQueryDebug");
    addDefectPriorityButton = $('input[id$=btnAddDefectPriority]');
    addDefectPriorityButton.addClass("JQueryDebug");
    addDefectPriorityButton.click(function (e) { addDefectPriorityButton_click(e) });
    cancelDefectPriorityButton = $('input[id$=btnCancel]');
    cancelDefectPriorityButton.addClass("JQueryDebug");
    cancelDefectPriorityButton.click(function () { cancelDefectPriorityButton_click(); });
    //initialize dialog
    $('#dlgDefectPriority').dialog({
        autoOpen: false,
        modal: true,
        width: 450,
        open: function () { }
    });
}