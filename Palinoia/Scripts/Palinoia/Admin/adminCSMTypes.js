/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/*jslint browser: true*/
/*global $, jQuery, jAlert, confirm*/

$(document).ready(function () {
    "use strict";
    initializeControls();
});

function saveCSMTypeButton_click(e) {
    "use strict";
    var validationMessage, invalidChars, csmTypeText, addCSMTypeTextbox, re, retVal;
    validationMessage = "";
    addCSMTypeTextbox = $("input:asp(txtAddCSMType)");
    re = new RegExp("[!@#$%&*_<>\///]");
    invalidChars = addCSMTypeTextbox.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text!";
    }
    csmTypeText = addCSMTypeTextbox.val();
    if (csmTypeText.length === 0) {
        validationMessage = "CSMType is required.";
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

function addCSMTypeButton_click(e) {
    addCSMTypeTextbox = $("input:asp(txtAddCSMType)");
    addCSMTypeTextbox.text = "";
    var activeCheckbox = $(':checkbox');
    activeCheckbox.prop('checked', true);
    showDialog(true);
    $('input[id$=hdnCSMTypeID]').val("0");
    return false;   
}

function showDialog(visible) {
    if (visible) {
        $('#dlgCSMTypes').dialog('open');
        $('#dlgCSMTypes').parent().appendTo($("form:first"));
    }
    else {
        $('#dlgCSMTypes').dialog('close');
    }
}

function cancelCSMTypeButton_click(e) {
    "use strict";
    var addCSMTypeTextbox;
    addCSMTypeTextbox = $("input:asp(txtAddCSMType)");
    //clear Status textbox of all text to prevent dangerous script error
    addCSMTypeTextbox.val("");
    //turn off validation so form will post back and process cancel button code
    $("form").validate().cancelSubmit = true;
    showDialog(false);
}

function initializeControls() {
    "use strict";
    var saveCSMTypeButton, addCSMTypeTextbox, 
        cancelCSMTypeButton, retVal, addCSMTypeButton;
    saveCSMTypeButton = $('input[id$=btnSaveCSMType]');
    saveCSMTypeButton.click(function (e) {
        saveCSMTypeButton_click(e);
    });
    saveCSMTypeButton.addClass("JQueryDebug");
    addCSMTypeTextbox = $("input:asp(txtAddCSMType)");
    addCSMTypeTextbox.addClass("JQueryDebug");
    cancelCSMTypeButton = $('input[id$=btnCancel]');
    cancelCSMTypeButton.addClass("JQueryDebug");
    cancelCSMTypeButton.click(function (e) { cancelCSMTypeButton_click(e); });
    addCSMTypeButton = $(":asp(btnAddCSMType)");
    addCSMTypeButton.addClass("JQueryDebug");
    addCSMTypeButton.click(function (e) { addCSMTypeButton_click(e); });
    //initialize dialog
    $('#dlgCSMTypes').dialog({
        autoOpen: false,
        modal: true,
        width: 450,
        open: function () { }
    });
}





