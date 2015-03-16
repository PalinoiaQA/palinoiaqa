/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/// <reference path="../../jquery-ui-1.9.0.min.js" />
/// <reference path="../../jquery.json-2.4.min.js" />

/*jslint browser: true*/
/*global $, jQuery, jAlert, confirm*/

function saveStatusButton_click(e) {
    "use strict";
    var validationMessage, invalidChars, statusText, addStatusTextbox, retVal, re, colorValue, colorTextbox;
    validationMessage = "";
    re = new RegExp("[!@#$%&*_<>\///]");
    addStatusTextbox = $("input:asp(txtAddStatus)");
    colorTextbox = $("input:asp(txtColor)");
    invalidChars = addStatusTextbox.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text!";
    }
    statusText = addStatusTextbox.val();
    if (statusText.length === 0) {
        validationMessage = "Status is required.";
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

function cancelStatusButton_click() {
    "use strict";
    var addStatusTextbox;
    addStatusTextbox = $("input:asp(txtAddStatus)");
    //clear Status textbox of all text to prevet dangerous script error
    addStatusTextbox.val("");
    showDialog(false);
}

$(document).ready(function () {
    "use strict";
    initializeControls();
    
});

function addStatusButton_click(e) {
    var activeCheckbox = $(':checkbox');
    activeCheckbox.prop('checked', true);
    showDialog(true);
    $('input[id$=hdnStatusID]').val("0");
    $('input:asp(txtAddStatus)').val("");
    $('input:asp(txtAddStatus)').addClass("JQueryDebug");
    setTimeout("$('input:asp(txtAddStatus)').focus();", 100);
    $('#chooseColor').val("#000000");
    return false;
}

function showDialog(visible) {
    if (visible) {
        $('#dlgStatus').dialog('open');
        $('#dlgStatus').parent().appendTo($("form:first"));
        $("input:asp(txtAddStatus)").focus();
    }
    else {
        $('#dlgStatus').dialog('close');
    }
}

function initializeControls() {
    "use strict";
    var saveStatusButton, addStatusTextbox, cancelStatusButton, addStatusButton, 
        retVal, colorTextbox, previewColorDIV, previewColor;
    saveStatusButton = $('input[id$=btnSaveStatus]');
    saveStatusButton.click(function (e) {
        saveStatusButton_click(e);
    });
    saveStatusButton.addClass("JQueryDebug");
    addStatusTextbox = $("input:asp(txtAddStatus)");
    addStatusTextbox.addClass("JQueryDebug");
    cancelStatusButton = $('input[id$=btnCancel]');
    cancelStatusButton.addClass("JQueryDebug");
    cancelStatusButton.click(function () { cancelStatusButton_click(); });
    addStatusButton = $(":asp(btnAddStatus)");
    addStatusButton.addClass("JQueryDebug");
    addStatusButton.click(function (e) { return addStatusButton_click(e); });
    //color picker controls
    $('#chooseColor').colorPicker();
    $('#chooseColor').val("#" + $("[id$='hdnColor']").val());
    $('#chooseColor').change();
    $('#chooseColor').change(function(){
        $("[id$='hdnColor']").val($('#chooseColor').val());
    });
    
    //initialize dialog
    $('#dlgStatus').dialog({
        autoOpen: false,
        modal: true,
        width: 500,
        open: function () { }
    });
}
