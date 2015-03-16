/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/*jslint browser: true*/
/*global $, jQuery, jAlert, confirm*/


$(document).ready(function () {
    "use strict";
    initializeControls();
});

function addSectionButton_click(e) {
    $("input:asp(txtSectionName)").val("");
    $("input:asp(txtSectionAbbreviation)").val("");
    var activeCheckbox = $(':checkbox');
    activeCheckbox.prop('checked', true);
    $('input[id$=hdnSectionID]').val("0");
    showDialog(true);
}

function showDialog(visible) {
    if (visible) {
        $('#dlgSection').dialog('open');
        $('#dlgSection').parent().appendTo($("form:first"));
    }
    else {
        $('#dlgSection').dialog('close');
    }
}

function saveSectionButton_click(e) {
    "use strict";
    var validationMessage, addSectionNameTextbox, invalidChars, sectionName, sectionAbbreviation, re,
        addSectionAbbreviationTextbox, retVal;
    validationMessage = "";
    addSectionNameTextbox = $("input:asp(txtSectionName)");
    addSectionAbbreviationTextbox = $("input:asp(txtSectionAbbreviation)");
    re = new RegExp("[!@#$%&*_<>\///]");
    invalidChars = addSectionNameTextbox.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text!";
    }
    re = new RegExp("[!@#$%&*_<>\///]");
    invalidChars = addSectionAbbreviationTextbox.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text!";
    }
    sectionName = addSectionNameTextbox.val();
    sectionAbbreviation = addSectionAbbreviationTextbox.val();
    if (sectionName.length === 0) {
        validationMessage = "Section name is required.";
    }
    if (sectionAbbreviation.length === 0) {
        validationMessage = "Section abbreviation is required.";
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

function cancelSectionButton_click() {
    "use strict";
    $("input:asp(txtSectionName)").val("");
    $("input:asp(txtSectionAbbreviation)").val("");
    showDialog(false);
}

function initializeControls() {
    "use strict";
    var saveSectionButton, addSectionNameTextbox, addSectionAbbreviationTextbox,
        addSectionButton, cancelSectionButton, retVal;
    saveSectionButton = $('input[id$=btnSaveSection]');
    saveSectionButton.click(function (e) {
        saveSectionButton_click(e);
    });
    saveSectionButton.addClass("JQueryDebug");
    addSectionNameTextbox = $("input:asp(txtSectionName)");
    addSectionNameTextbox.addClass("JQueryDebug");
    addSectionAbbreviationTextbox = $("input:asp(txtSectionAbbreviation)");
    addSectionAbbreviationTextbox.addClass("JQueryDebug");
    cancelSectionButton = $('input[id$=btnCancel]');
    cancelSectionButton.addClass("JQueryDebug");
    cancelSectionButton.click(function () { cancelSectionButton_click(); });
    addSectionButton = $(":asp(btnAddSection)");
    addSectionButton.addClass("JQueryDebug");
    addSectionButton.click(function (e) { addSectionButton_click(e); });
    //initialize dialog
    $('#dlgSection').dialog({
        autoOpen: false,
        modal: true,
        width: 450,
        open: function () { }
    });
}