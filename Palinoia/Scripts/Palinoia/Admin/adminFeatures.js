/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/*jslint browser: true*/
/*global $, jQuery, jAlert, confirm*/

function saveFeatureButton_click(e) {
    'use strict';
    var validationMessage, invalidChars, featureText, addFeatureTextbox, re, retVal;
    validationMessage = "";
    re = new RegExp("[!@#$%&*_<>\///]");
    addFeatureTextbox = $("input:asp(txtAddFeature)");
    invalidChars = addFeatureTextbox.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text!";
    }
    featureText = addFeatureTextbox.val();
    if (featureText.length === 0) {
        validationMessage = "Feature is required.";
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

function showDialog(visible) {
    if (visible) {
        $('#dlgFeature').dialog('open');
        $('#dlgFeature').parent().appendTo($("form:first"));
    }
    else {
        $('#dlgFeature').dialog('close');
    }
}

function addFeatureButton_click(e) {
    $("input:asp(txtAddFeature)").val("");
    var activeCheckbox = $(':checkbox');
    activeCheckbox.prop('checked', true);
    showDialog(true);
    $('input[id$=hdnFeatureID]').val("0");
}

function cancelFeatureButton_click() {
    'use strict';
    $("input:asp(txtAddFeature)").val("");
    showDialog(false);
}

function initializeControls() {
    'use strict';
    var saveFeatureButton, addFeatureTextbox, 
        addFeatureButton, cancelFeatureButton, retVal;
    saveFeatureButton = $('input[id$=btnSaveFeature]');
    saveFeatureButton.click(function (e) {
        saveFeatureButton_click(e);
    });
    saveFeatureButton.addClass("JQueryDebug");
    addFeatureTextbox = $("input:asp(txtAddFeature)");
    addFeatureTextbox.addClass("JQueryDebug");
    cancelFeatureButton = $('input[id$=btnCancel]');
    cancelFeatureButton.addClass("JQueryDebug");
    cancelFeatureButton.click(function () { cancelFeatureButton_click(); });
    addFeatureButton = $(":asp(btnAddFeature)");
    addFeatureButton.addClass("JQueryDebug");
    addFeatureButton.click(function (e) { addFeatureButton_click(e); });
    //initialize dialog
    $('#dlgFeature').dialog({
        autoOpen: false,
        modal: true,
        width: 450,
        open: function () { }
    });
}

$(document).ready(function () {
    'use strict';
    initializeControls();
});