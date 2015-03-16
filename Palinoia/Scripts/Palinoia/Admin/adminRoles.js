/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/*jslint browser: true*/
/*global $, jQuery, jAlert, confirm*/

$(document).ready(function () {
    "use strict";
    initializeControls();
});

function addRoleButton_click(e) {
    //turn off set features button because this is a new role
    //and does not yet have a role id
    var setFeaturesButton = $('input[id$=btnSetFeatures]');
    setFeaturesButton.hide();
    $("input:asp(txtAddRole)").val("");
    var activeCheckbox = $(':checkbox');
    activeCheckbox.prop('checked', true);
    showDialog(true);
    $('input[id$=hdnRoleID]').val("0");
    return false;
}

function showDialog(visible) {
    if (visible) {
        $('#dlgRole').dialog('open');
        $('#dlgRole').parent().appendTo($("form:first"));
    }
    else {
        $('#dlgRole').dialog('close');
    }
}

function saveRoleButton_click(e) {
    "use strict";
    var validationMessage, invalidChars, roleText, addRoleTextbox, re, retVal;
    addRoleTextbox = $("input:asp(txtAddRole)");
    validationMessage = "";
    re = new RegExp("[!@#$%&*_<>\///]");
    invalidChars = addRoleTextbox.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text!";
    }
    roleText = addRoleTextbox.val();
    if (roleText.length === 0) {
        validationMessage = "Role Name is required.";
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

function cancelRoleButton_click() {
    "use strict";
    showDialog(false);
}

function setFeaturesButton_click(e) {
    "use strict";
}

function initializeControls() {
    "use strict";
    var setFeaturesButton, saveAddRoleButton, saveEditRoleButton, 
        addRoleButton, addRoleTextbox, cancelRoleButton, retVal;
    setFeaturesButton = $('input[id$=btnSetFeatures]');
    setFeaturesButton.click(function (e) {
        setFeaturesButton_click(e);
    });
    setFeaturesButton.addClass("JQueryDebug");
    saveAddRoleButton = $('input[id$=btnSaveAddRole]');
    saveAddRoleButton.click(function (e) {
        saveRoleButton_click(e);
    });
    saveAddRoleButton.addClass("JQueryDebug");
    saveEditRoleButton = $('input[id$=btnSaveRole]');
    saveEditRoleButton.click(function (e) {
        saveRoleButton_click(e);
    });
    saveEditRoleButton.addClass("JQueryDebug");
    addRoleTextbox = $("input:asp(txtAddRole)");
    addRoleTextbox.addClass("JQueryDebug");
    cancelRoleButton = $('input[id$=btnCancel]');
    cancelRoleButton.addClass("JQueryDebug");
    cancelRoleButton.click(function () { cancelRoleButton_click(); });
    addRoleButton = $(":asp(btnAddRole)");
    addRoleButton.addClass("JQueryDebug");
    addRoleButton.click(function (e) { addRoleButton_click(e); });
    //initialize dialog
    $('#dlgRole').dialog({
        autoOpen: false,
        modal: true,
        width: 475,
        open: function () { }
    });
}