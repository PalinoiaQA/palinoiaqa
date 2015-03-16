/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/*jslint browser: true*/
/*global $, jQuery, jAlert, confirm*/

$(document).ready(function () {
    "use strict";
    initializeControls();
});

function initializeControls() {
    "use strict";
    var viewBRbySectionReportButton;
    viewBRbySectionReportButton = $('input[id$=btnBRbySection]');
    viewBRbySectionReportButton.click(function (e) {
        viewBRbySectionButton_click(e);
    });
    var viewBRbyStatusReportButton;
    viewBRbyStatusReportButton = $('input[id$=btnBRbyStatus]');
    viewBRbyStatusReportButton.click(function (e) {
        viewBRbyStatusButton_click(e);
    });
}

function viewBRbySectionButton_click(e) {
    var sectionDDL, selectedSection;
    //make sure that a section is selected in ddl
    sectionDDL = $(".sectionDDL");
    selectedSection = sectionDDL.find(":selected").val();
    $('input[id$=hdnSectionID]').val(selectedSection);
    if (selectedSection == "0") {
        jAlert("Section is required");
        e.preventDefault();
        return false;
    }
    else {
        return true;
    }
}

function viewBRbyStatusButton_click(e) {
    var sectionDDL, selectedStatus;
    //make sure that a section is selected in ddl
    statusDDL = $(".statusDDL");
    selectedStatus = statusDDL.find(":selected").val();
    $('input[id$=hdnStatusID]').val(selectedStatus);
    if (selectedStatus == "0") {
        jAlert("Status is required");
        e.preventDefault();
        return false;
    }
    else {
        return true;
    }
}