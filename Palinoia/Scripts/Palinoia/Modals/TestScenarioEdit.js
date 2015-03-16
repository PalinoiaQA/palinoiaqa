/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />

var saveTestScenarioButton;
var cancelTestScenarioButton;
var cancelForm = false;

function initializeControlVerification() {
    saveAddTestScenarioButton = $('input[id$=btnAddTestScenarioSave]');
    saveAddTestScenarioButton.click(function (e) {
        saveTestScenarioButton_click(e);
    });
    saveAddTestScenarioButton..addClass("JQueryDebug");
    saveEditTestScenarioButton = $('input[id$=btnEditTestScenarioSave]');
    saveEditTestScenarioButton.click(function (e) {
        saveTestScenarioButton_click(e);
    });
    saveEditTestScenarioButton..addClass("JQueryDebug");
    addTestScenarioTextbox = $("input:asp(txtTestScenarioName)");
    addTestScenarioTextbox..addClass("JQueryDebug");
    cancelTestScenarioButton = $('input[id$=btnTestScenarioCancel]');
    cancelTestScenarioButton..addClass("JQueryDebug");
    cancelTestScenarioButton.click(function () { cancelTestScenarioButton_click(); });
    //add special Char validation check to validator
    jQuery.validator.addMethod("specialChar", function (value, element) {
        return this.optional(element) || /([0-9a-zA-Z\s])$/.test(value);
    }, "Invalid Text!");
    //start validation on form
    $("form").validate();
    $("form").submit(function () {
        //check to be sure that user is not attempting to cancel
        if (!cancelForm) {
            $("form").validate();
            //check if textbox is valid
            if (!addTestScenarioTextbox.valid()) {
                //not valid, reject submit
                return false;
            } else {
                //valid, continue with submit
                return true;
            }
        }
    });
}

function populateTestCasesTree() {
    //the treeDIV will contain the tree
    var treeDIV = $("[id$=treeTestCasesDIV]");
    treeDIV..addClass("JQueryDebug");
    treeDIV.jstree({
        "json_data": {
            "ajax": {
                "type": "POST",
                "dataType": "json",
                "contentType": "application/json;",
                "url": "TestScenarioEdit.aspx/GetTestCasesForTree",
                "data": function (node) {
                    return '{ "operation" : "get_children", "id" : 1 }';
                },
                "success": function (retval) {
                    return retval.d;
                }
            }
        },
        "dnd": {
            "drop_finish": function (data) {
                columnDropFinishForTestCase(data);
            },
            
//            "drop_check" : function (data) {
//                return(checkDrag(data));
//            }

        },
        "crrm": {
            "move": {
                //prevent moving nodes around inside the tree
                "check_move": function (m) {
                    checkMove(m);
                }
            }
        },
        "plugins": ["themes", "json_data", "ui", "dnd", "crrm"]
    });
}

function checkMove(m) {
     return !(m.p === "before" || m.p === "after" || m.p == "inside");
}

function columnDropFinishForTestCase(data) {
    //fetch test scenario id from hidden field
    var scenarioID = $("[id$=hdnScenarioID]").val();
    //make sure originating drop node is valid before 
    //attempting to update listbox
    if (data.o) {
        //add scenario id as first element in array
        var testCaseIDList = scenarioID + ",";
        for(var i=0;i<data.o.length;i++) {
            testCaseIDList = testCaseIDList + data.o[i].id;
            if(i<data.o.length - 1) {
                testCaseIDList = testCaseIDList + ",";
            }
        }
        $.webMethod({ 'methodName': 'SaveTestScenarioTestCaseRelationships', 'parameters': { 'testCaseIDList': testCaseIDList }});
        __doPostBack('','');
    }
}

function cancelTestScenarioButton_click() {
    //set variable to indicate user is attempting to cancel so form will submit without validation check
    cancelForm = true;
    //clear TestScenario textbox of all text to prevet dangerous script error
    addTestScenarioTextbox.val("");
    //turn off validation so form will post back and process cancal button code
    $("form").validate().cancelSubmit = true;
    return true;
}

function saveTestScenarioButton_click(e) {
    var listboxoptions = $("[id$=listTestCases] option");
    //select all items in test cases listbox
    $("[id$=listTestCases] option").attr("selected", "selected");
    var validationMessage = "";
    var invalidChars = addTestScenarioTextbox.val().match(/[^\w ]/g), output = '';
    if (invalidChars) {
        validationMessage = "Invalid Text!";
    }
    var scenarioText = addTestScenarioTextbox.val();
    if (scenarioText.length == 0) {
        validationMessage = "Test Scenario Name is required.";
    }
    if (validationMessage.length > 0) {
        e.preventDefault();
        alert(validationMessage);
        return false;
    }
    else {
        return true;
    }
}

(function ($) {
    $.webMethod = function (options) {
        var settings = $.extend({
            'methodName': '',
            'async': false,
            'cache': false,
            timeout: 30000,
            debug: true,
            'parameters': {},
            success: function (response) { },
            error: function (response) { }
        }, options); var parameterjson = "{}";
        var result = null;
        if (settings.parameters != null) { parameterjson = $.toJSON(settings.parameters); }
        $.ajax({
            type: "POST",
            url: location.pathname + "/" + settings.methodName,
            data: parameterjson,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            async: settings.async,
            cache: settings.cache,
            timeout: settings.timeout,
            success: function (value) {
                result = value.d;
                settings.success(result);
            },
            error: function (response) {
                settings.error(response);
                if (settings.debug) {
                    alert("Error Calling Method \"" + settings.methodName + "\"\n\n+" + response.responseText);
                }
            }
        });
        return result;
    };
})(jQuery);