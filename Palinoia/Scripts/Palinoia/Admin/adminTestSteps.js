/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/// <reference path="../../jquery-ui-1.9.0.min.js" />

/*jslint browser: true*/
/*global $, jQuery, jAlert, confirm*/

//set global variables
var CANCELFORM = false;

$(document).ready(function () {
    "use strict";
    initializeControls();
});

function initializeControls() {
    addTestStepButton = $('input[id$=btnAddTestStep]');
    addTestStepButton.addClass("JQueryDebug");
    addTestStepButton.click(addTestStepButton_click);
    saveTestStepButton = $('input[id$=btnSaveTestStep]');
    saveTestStepButton.addClass("JQueryDebug");
    cancelTestStepButton = $('input[id$=btnCancel]');
    cancelTestStepButton.addClass("JQueryDebug");
    cancelTestStepButton.click(btnCancel_click);
    var dlg1 = $('#dlgTestStep').dialog({
        autoOpen: false,
        modal: true,
        width: 450,
        open: function () { }
    });
    dlg1.parent().appendTo(jQuery("form:first"));
}

function showClientEditTestStepModal(TestStepText) {
    var pcID = $('[id$=hdnTestStepID]').val();
    //$('input[id$=txtPreCText]').val(TestStepText);
    //setPreCModalEditMode("edit");
    var dlg = $('#dlgTestStep').dialog({
        autoOpen: false,
        modal: true,
        width: 450,
        open: function () { }
    });
    dlg.parent().appendTo(jQuery("form:first"));
    $('#dlgTestStep').dialog('open');
}

function addTestStepButton_click(e) {
    $('#dlgTestStep').dialog('open');
    $('input[id$=txtAddTestStep]').val("");
    $(':checkbox').prop('checked', true);
    $('[id$=hdnTestStepID]').val(0);
}

function btnCancel_click(e) {
    $('#dlgTestStep').dialog('close');
}