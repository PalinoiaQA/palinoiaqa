/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/*jslint browser: true*/
/*global $, jQuery, jAlert, confirm*/

$(document).ready(function () {
    "use strict";
    initializeControls();
});

function addUserButton_click(e) {
    $("input:asp(txtFirstName)").val("");
    $("input:asp(txtLastName)").val("");
    $("input:asp(txtMiddleInitial)").val("");
    $("input:asp(txtEmail)").val("");
    $("input:asp(txtPassword)").val("");
    var activeCheckbox = $(':checkbox');
    activeCheckbox.prop('checked', true);
    showDialog(true);
    $('input[id$=hdnUserID]').val("0");
    return false;
}

function showDialog(visible) {
    if (visible) {
        $('#dlgUser').dialog('open');
        $('#dlgUser').parent().appendTo($("form:first"));
    }
    else {
        $('#dlgUser').dialog('close');
    }
}

function saveUserButton_click(e) {
    "use strict";
    var validationMessage, invalidChars, fname, lname, email, userFirstNameTextbox,
        userLastNameTextbox, userMiddleInitialTextbox, userEmailTextbox, re, retVal;
    userFirstNameTextbox = $("input:asp(txtFirstName)");
    userLastNameTextbox = $("input:asp(txtLastName)");
    userMiddleInitialTextbox = $("input:asp(txtMiddleInitial)");
    userEmailTextbox = $("input:asp(txtEmail)");
    validationMessage = "";
    re = new RegExp("[!@#$%&*_<>\///]");
    invalidChars = userFirstNameTextbox.val().match(re);
    if (invalidChars) {
        validationMessage = "Invalid Text!";
    }
    fname = userFirstNameTextbox.val();
    if (fname.length === 0) {
        validationMessage = "First name is required.";
    }
    lname = userLastNameTextbox.val();
    if (lname.length === 0) {
        validationMessage = "Last name is required.";
    }
    email = userEmailTextbox.val();
    if (email.length === 0) {
        validationMessage = "Email is required.";
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

function cancelUserButton_click() {
    "use strict";
    var userFirstNameTextbox, userLastNameTextbox, userMiddleInitialTextbox, userRoleDDL, userEmailTextbox;
    //clear all dialog text boxes
    $("input:asp(txtFirstName)").val("");
    $("input:asp(txtLastName)").val("");
    $("input:asp(txtMiddleInitial)").val("");
    $("input:asp(txtEmail)").val("");
    $("select:asp(ddlUserRole)").val("");
    showDialog(false);
}

function initializeControls() {
    "use strict";
    var userFirstNameTextbox, userLastNameTextbox, userMiddleInitialTextbox, userEmailTextbox, userPasswordTextbox,
        userRoleDDL, saveAddUserButton, saveEditUserButton, cancelUserButton, retVal, addUserButton, saveUserButton;
    userFirstNameTextbox = $("input:asp(txtFirstName)");
    userFirstNameTextbox.addClass("JQueryDebug");
    userLastNameTextbox = $("input:asp(txtLastName)");
    userLastNameTextbox.addClass("JQueryDebug");
    userMiddleInitialTextbox = $("input:asp(txtMiddleInitial)");
    userMiddleInitialTextbox.addClass("JQueryDebug");
    userEmailTextbox = $("input:asp(txtEmail)");
    userEmailTextbox.addClass("JQueryDebug");
    userPasswordTextbox = $("input:asp(txtPassword)");
    userPasswordTextbox.addClass("JQueryDebug");
    userRoleDDL = $("select:asp(ddlUserRole)");
    userRoleDDL.addClass("JQueryDebug");
    saveUserButton = $('input[id$=btnSaveUser]');
    saveUserButton.click(function (e) {
        saveUserButton_click(e);
    });
    saveUserButton.addClass("JQueryDebug");
    cancelUserButton = $('input[id$=btnCancel]');
    cancelUserButton.addClass("JQueryDebug");
    cancelUserButton.click(function () { cancelUserButton_click(); });
    addUserButton = $(":asp(btnAddUser)");
    addUserButton.click(function (e) { return addUserButton_click(e); });
    addUserButton.addClass("JQueryDebug");
    //initialize dialog
    $('#dlgUser').dialog({
        autoOpen: false,
        modal: true,
        width: 450,
        open: function () { }
    });
}