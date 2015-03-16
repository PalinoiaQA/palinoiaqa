/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/*jslint browser: true*/
/*global $, jQuery, jAlert, confirm*/

//set Global control variables
var CANCELFORM = false;

function addResponseTypeButton_click(e) {
    $("input:asp(txtAddResponseType)").val("");
    var activeCheckbox = $(':checkbox');
    activeCheckbox.prop('checked', true);
    showDialog(true);
    $('input[id$=hdnEditResponseTypeID]').val("0");
    return false;
}

function showDialog(visible) {
    if (visible) {
        $('#dlgResponseType').dialog('open');
        $('#dlgResponseType').parent().appendTo($("form:first"));
    }
    else {
        $('#dlgResponseType').dialog('close');
    }
}

function saveResponseTypeButton_click(e) {
    'use strict';
    var validationMessage, invalidChars, addResponseTypeTextbox, responseTypeText, re, retVal;
    validationMessage = "";
    re = new RegExp("[!@#$%&*_<>\///]");
    addResponseTypeTextbox = $("input:asp(txtAddResponseType)");
    invalidChars = addResponseTypeTextbox.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text!";
    }
    responseTypeText = addResponseTypeTextbox.val();
    if (responseTypeText.length === 0) {
        validationMessage = "CMS Response Type is required.";
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

function cancelResponseTypeButton_click() {
    'use strict';
    showDialog(false);
    //var addResponseTypeTextbox;
    //addResponseTypeTextbox = $("input:asp(txtAddResponseType)");
    //set variable to indicate user is attempting to cancel so form will submit without validation check
    //CANCELFORM = true;
    //clear response type textbox of all text to prevet dangerous script error
    //addResponseTypeTextbox.val("");
    //turn off validation so form will post back and process cancal button code
    //$("form").validate().cancelSubmit = true;
    //return true;
}

function initializeControls() {
    'use strict';
    var saveResponseTypeButton, addResponseTypeTextbox, cancelResponseTypeButton, retVal, addResponseTypeButton;
    saveResponseTypeButton = $('input[id$=btnSaveCSMResponseType]');
    saveResponseTypeButton.click(function (e) {
        saveResponseTypeButton_click(e);
    });
    saveResponseTypeButton.addClass("JQueryDebug");
    addResponseTypeTextbox = $("input:asp(txtAddResponseType)");
    addResponseTypeTextbox.addClass("JQueryDebug");
    cancelResponseTypeButton = $('input[id$=btnCancel]');
    cancelResponseTypeButton.addClass("JQueryDebug");
    cancelResponseTypeButton.click(function () { cancelResponseTypeButton_click(); });
    addResponseTypeButton = $(":asp(btnAddResponseType)");
    addResponseTypeButton.addClass("JQueryDebug");
    addResponseTypeButton.click(function (e) { return addResponseTypeButton_click(e); });
    //initialize dialog
    $('#dlgResponseType').dialog({
        autoOpen: false,
        modal: true,
        width: 450,
        open: function () { }
    });
//    //add special Char validation check to validator
//    jQuery.validator.addMethod("specialChar", function (value, element) {
//        return this.optional(element) || /([0-9a-zA-Z\s])$/.test(value);
//    }, "Invalid Text!");
//    //start validation on form
//    $("form").validate();
//    $("form").submit(function () {
//        //check to be sure that user is not attempting to cancel
//        if (!CANCELFORM) {
//            $("form").validate();
//            //check if textbox is valid
//            if (!addResponseTypeTextbox.valid()) {
//                //not valid, reject submit
//                retVal = false;
//            } else {
//                //valid, continue with submit
//                retVal = true;
//            }
//            return retVal;
//        }
//    });
}

$(document).ready(function () {
    'use strict';
    initializeControls();
});