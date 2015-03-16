
$(document).ready(function () {
    "use strict";
    initializeControls();
});

function initializeControls() {
    "use strict";
    var closeButton = $('input[id$=btnClose]');
    closeButton.addClass("JQueryDebug");
    closeButton.click(function () { closeButton_click(); });
    var clearErrorsButton = $('input[id$=btnClearErrors]');
    clearErrorsButton.click(function () { clearErrors_click(); });
    //initialize dialog
    $('#dlgErrorDetail').dialog({
        autoOpen: false,
        modal: true,
        width: 600,
        height: 650,
        open: function () { }
    });
}

function clearErrors_click() {
    $.webMethod({ 'methodName': 'clearErrors', 'parameters': { },
        success: function (response) {
            __doPostBack('', '');
        }
    });
    return false;
}

function showDialog(visible) {
    if (visible) {
        $('#dlgErrorDetail').dialog('open');
        $('#dlgErrorDetail').parent().appendTo($("form:first"));
    }
    else {
        $('#dlgErrorDetail').dialog('close');
    }
}

function showErrorDetails(errorID) {
    var error, projectName, userName;
    $.webMethod({ 'methodName': 'getErrorDetails', 'parameters': { 'strErrorID': errorID },
        success: function (response) {
            error = response;
        }
    });
    if (error != null) {
        $.webMethod({ 'methodName': 'getProjectName', 'parameters': { 'strProjectID': error.ProjectID },
            success: function (response) {
                projectName = response;
            }
        });
        $.webMethod({ 'methodName': 'getUserName', 'parameters': { 'strUserID': error.UserID },
            success: function (response) {
                userName = response;
            }
        });

        $("input:asp(txtDate)").val(error.DisplayDate);
        $("input:asp(txtProject)").val(projectName);
        $("input:asp(txtUser)").val(userName);
        $('.SourceTextbox').val(error.Source);
        $('.InnerExceptionTextbox').val(error.InnerException); ;
        $('.MessageTextbox').val(error.Message);
        $('.StackTraceTextbox').val(error.StackTrace);
        showDialog(true);
    }
}

function closeButton_click() {
    showDialog(false);
}