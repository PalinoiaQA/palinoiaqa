/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/// <reference path="../../jquery-ui-1.9.0.min.js" />

/*jslint browser: true*/
/*global $, jQuery, alert, confirm*/

$(document).ready(function () {
    "use strict";
    initializeControls();
    //jAlert("Controls initialized");
});


function initializeControls() {
    var failButton = $('input[id$=btnFail]');
    failButton.unbind('click');
    failButton.addClass("JQueryDebug");
    failButton.click(function (e) {
        failButton_click(e);
    });
    var passButton = $('input[id$=btnPass]');
    passButton.unbind('click');
    passButton.addClass("JQueryDebug");
    passButton.click(function (e) {
        passButton_click(e);
    });
    var cancelButton = $('input[id$=btnCancel]');
    cancelButton.unbind('click');
    cancelButton.addClass("JQueryDebug");
    cancelButton.click(function (e) {
        cancelButton_click(e);
    });
    var dlg1 = $('#dlgNotes').dialog({
        autoOpen: false,
        modal: true,
        width: 400,
        open: function () { }
    });
    var dlg2 = $('#dlgSelectBR').dialog({
        autoOpen: false,
        modal: true,
        width: 400,
        open: function () { }
    });
}

function failButton_click(e) {
    $('#dlgNotes').dialog('open');
    $('#dlgNotes').parent().appendTo($("form:first"));
}

function cancelButton_click(e) {
    $('#dlgNotes').dialog('close');
}

function saveButton_click() {
    //validate input
    var validationMessage = "";
    var failText = $('.FailNotes');
    re = new RegExp("[!@#$%&*_<>\///]");
    invalidChars = failText.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid text!";
    }
    if (failText.val().length == 0) {
        validationMessage = "Text is required!";
    }
    if (validationMessage.length > 0) {
        jAlert(validationMessage);
        return false;
    }
    else {
        //validation passed.  do we need to show the select failed br dialog?
        if ($('input[id$=hdnShowSelectBR').val() == "true") {
            $('#dlgNotes').dialog('close');
            $('#dlgSelectBR').dialog('open');
            $('#dlgSelectBR').parent().appendTo($("form:first"));
            return false;
        }
        else {
            //is there at least 1 business rule asssigned to this test step?
            var selectFailedBRDDL = $('.SelectFailedBRDDL');
            var selectedID = selectFailedBRDDL.val();
            $('input[id$=hdnFailedBusinessRuleID]').val(selectedID);
            //if not, alert user that the defect will be assigned to them
            if(selectedID == "" || selectedID == null) {
                var result = confirm("No business rule is associated with this test step.  Defect will be assigned to current user. Continue?");
                if (result) { return true; }
                else { return false; }
            }
        }
    }
}

function btnSaveFailedBRButton_Click() {
    //input of previous screen already validated
    //get selected failed business rule id from ddl and place in hidden field
    var selectFailedBRDDL = $('.SelectFailedBRDDL');
    var selectedID = selectFailedBRDDL.val();
    $('input[id$=hdnFailedBusinessRuleID]').val(selectedID);
    return true;
}

function passButton_click(e) {
    var resultLabel = $(".result");
    //update test result label to give user visual indication that currrent test passed
    resultLabel.css('color', 'green');
    resultLabel.text("PASS");
    return true;
}