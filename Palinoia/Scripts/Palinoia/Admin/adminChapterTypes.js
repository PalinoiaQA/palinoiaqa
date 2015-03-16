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

function addChapterTypeButton_click(e) {
    $("input:asp(txtAddChapterType)").val("");
    var activeCheckbox = $(':checkbox');
    activeCheckbox.prop('checked', true);
    showDialog(true);
    $('input[id$=hdnChapterTypeID]').val("0");
    setTimeout("$('input:asp(txtAddChapterType)').focus();", 100);
    return false;
}

function showDialog(visible) {
    if (visible) {
        $('#dlgChapterType').dialog('open');
        $('#dlgChapterType').parent().appendTo($("form:first"));
    }
    else {
        $('#dlgChapterType').dialog('close');
    }
}

function saveChapterTypeButton_click(e) {
    "use strict";
    var validationMessage, invalidChars, chapterTypeText, addChapterTypeTextbox, re, retVal;
    validationMessage = "";
    addChapterTypeTextbox = $("input:asp(txtAddChapterType)");
    re = new RegExp("[!@#$%&*_<>\///]");
    invalidChars = addChapterTypeTextbox.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text!";
    }
    chapterTypeText = addChapterTypeTextbox.val();
    if (chapterTypeText.length === 0) {
        validationMessage = "Chapter Type is required.";
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

function cancelChapterTypeButton_click() {
    "use strict";
    var addChapterTypeTextbox;
    addChapterTypeTextbox = $("input:asp(txtAddChapterType)");
    //clear Status textbox of all text to prevent dangerous script error
    addChapterTypeTextbox.val("");
    showDialog(false);
}

function initializeControls() {
    "use strict";
    var saveAddChapterTypeButton, saveEditChapterTypeButton, addChapterTypeTextbox,
        cancelChapterTypeButton, addChapterTypeButton, retVal;
    saveAddChapterTypeButton = $('input[id$=btnSaveChapterType]');
    saveAddChapterTypeButton.click(function (e) {
        saveChapterTypeButton_click(e);
    });
    saveAddChapterTypeButton.addClass("JQueryDebug");
    saveEditChapterTypeButton = $('input[id$=btnSaveEditChapterType]');
    saveEditChapterTypeButton.click(function (e) {
        saveChapterTypeButton_click(e);
    });
    saveEditChapterTypeButton.addClass("JQueryDebug");
    addChapterTypeTextbox = $("input:asp(txtAddChapterType)");
    addChapterTypeTextbox.addClass("JQueryDebug");
    cancelChapterTypeButton = $('input[id$=btnCancel]');
    cancelChapterTypeButton.addClass("JQueryDebug");
    cancelChapterTypeButton.click(function () { cancelChapterTypeButton_click(); });
    addChapterTypeButton = $(":asp(btnAddChapterType)");
    addChapterTypeButton.addClass("JQueryDebug");
    addChapterTypeButton.click(function (e) { addChapterTypeButton_click(e); });
    //initialize dialog
    $('#dlgChapterType').dialog({
        autoOpen: false,
        modal: true,
        width: 450,
        open: function () { }
    });
}