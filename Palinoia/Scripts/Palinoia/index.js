/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/*jslint browser: true*/
/*global $, jQuery, alert, confirm*/

$(document).ready(function () {
    initializeControls();
});

function initializeControls() {
    var demoBtn1, demoBtn2;
    demoBtn1 = $('input[id$=btnDemo1]');
    demoBtn1.click(function (e) {
        demoButtonClick(e);
    });
    demoBtn2 = $('input[id$=btnDemo2]');
    demoBtn2.click(function (e) {
        demoButtonClick(e);
    });
}

function demoButtonClick(e) {
    window.open("UI/default.aspx");
}