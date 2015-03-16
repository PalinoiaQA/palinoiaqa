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

function addDefectTypeButton_click(e) {
    $("input:asp(txtAddDefectType)").val("");
    var activeCheckbox = $(':checkbox');
    activeCheckbox.prop('checked', true);
    showDialog(true);
    $('input[id$=hdnDefectTypeID]').val("0");
    setTimeout("$('input:asp(txtAddDefectType)').focus();", 100);
}

function showDialog(visible) {
    if (visible) {
        $('#dlgDefectType').dialog('open');
        $('#dlgDefectType').parent().appendTo($("form:first"));
    }
    else {
        $('#dlgDefectType').dialog('close');
    }
}

function saveDefectTypeButton_click(e) {
    "use strict";
    var validationMessage, invalidChars, DefectTypeText, addDefectTypeTextbox, re, retVal;
    validationMessage = "";
    addDefectTypeTextbox = $("input:asp(txtAddDefectType)");
    re = new RegExp("[!@#$%&*_<>\///]");
    invalidChars = addDefectTypeTextbox.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text!";
    }
    DefectTypeText = addDefectTypeTextbox.val();
    if (DefectTypeText.length === 0) {
        validationMessage = "Defect Type is required.";
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

function cancelDefectTypeButton_click() {
    "use strict";
    var addDefectTypeTextbox;
    addDefectTypeTextbox = $("input:asp(txtAddDefectType)");
    addDefectTypeTextbox.val("");
    showDialog(false);
}

function initializeControls() {
    "use strict";
    var saveDefectTypeButton, addDefectTypeTextbox,
        cancelDefectTypeButton, retVal, addDefectTypeButton;
    saveDefectTypeButton = $('input[id$=btnSaveDefectType]');
    saveDefectTypeButton.click(function (e) {
        saveDefectTypeButton_click(e);
    });
    saveDefectTypeButton.addClass("JQueryDebug");
    addDefectTypeTextbox = $("input:asp(txtAddDefectType)");
    addDefectTypeTextbox.addClass("JQueryDebug");
    addDefectTypeButton = $('input[id$=btnAddDefectType]');
    addDefectTypeButton.addClass("JQueryDebug");
    addDefectTypeButton.click(function (e) { addDefectTypeButton_click(e) });
    cancelDefectTypeButton = $('input[id$=btnCancel]');
    cancelDefectTypeButton.addClass("JQueryDebug");
    cancelDefectTypeButton.click(function () { cancelDefectTypeButton_click(); });
    //initialize dialog
    $('#dlgDefectType').dialog({
        autoOpen: false,
        modal: true,
        width: 450,
        open: function () { }
    });
}