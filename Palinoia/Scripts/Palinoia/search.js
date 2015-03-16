//class control reference
//search object DDL     class = "searchObject(1-4)
//search operator       class = "searchOperator(1-4)
//search value          class = "searchValue(1-4)
//search row connector  class = "searchConnector(2-4)

var skipUnloadClear = false;

$(function () {
    $("#datepicker1").datepicker();
    $("#datepicker2").datepicker();
    $("#datepicker3").datepicker();
    $("#datepicker4").datepicker();
});

$(document).ready(function () {
    initializeSearchControls();
    showSearchResultTrees();
});

$(window).bind('beforeunload', function () {
    if (!skipUnloadClear) {
        clearSearchCookies();
    }
});

jQuery(function ($) {
    var form = $('form'), oldSubmit = form[0].onsubmit;
    form[0].onsubmit = null;

    $('form').submit(function () {
        // reset the onbeforeunload
        window.onbeforeunload = null;

        // run what actually was on
        if (oldSubmit)
            oldSubmit.call(this);
    });
});

function showSearchResultTrees() {
    var searchTypeID = $("[id$='hdnSearchTypeID']").val();
    //defects searh type = 3 and uses grid for search results instead of jstree
    if (searchTypeID != "3") {
        var fullTreeDIV = $('.treeFull');
        var searchResultsTreeDIV = $('.treeSearchResults');
        if (searchResultsTreeDIV != null) {
            var searchResults = $("[id$='hdnSearchResults']").val();
            if (searchResults == null || searchResults == "") {
                $("#TreePanelDIV").show();
                $("#searchResultTreePanelDIV").hide();
            }
            else if (fullTreeDIV != null) {
                $("#TreePanelDIV").hide();
                $("#searchResultTreePanelDIV").show();
            }
        }
    }
}

function loadSearchControls() {
    //ROW 1
    var SO1 = $.cookie("SO1");
    var OP1 = $.cookie("OP1");
    var ST1 = $.cookie("ST1");
    var DV1 = $.cookie("DV1");
    var CAL1 = $.cookie("CAL1");
    var CN2 = $.cookie("CN2");
    showSearchControls(SO1, 1);
    $("[id$='hdnSearchObjectDDL1']").val(SO1);
    $("[id$='hdnOperator1']").val(OP1);
    $("[id$='hdnTextValue1']").val(ST1);
    $("[id$='hdnCalendarValue1']").val(CAL1);
    $("[id$='hdnDDLValue1']").val(DV1);
    $("[id$='hdnConnector2']").val(CN2);
    $("select[id$=ddlSearchObject1]").val(SO1);
    $("select[id$=ddlOperator1]").val(OP1);
    $(".txtSearchValue1").val(ST1);
    $(".calTextValue1").val(CAL1);
    $("select[id$=ddlSearchValue1]").val(DV1);
    $("select[id$=ddlConnector2]").val(CN2);
    //ROW 2
    var SO2 = $.cookie("SO2");
    var OP2 = $.cookie("OP2");
    var ST2 = $.cookie("ST2");
    var DV2 = $.cookie("DV2");
    var CAL2 = $.cookie("CAL2");
    var CN3 = $.cookie("CN3");
    showSearchControls(SO2, 2);
    $("[id$='hdnSearchObjectDDL2']").val(SO2);
    $("[id$='hdnOperator2']").val(OP2);
    $("[id$='hdnTextValue2']").val(ST2);
    $("[id$='hdnCalendarValue2']").val(CAL2);
    $("[id$='hdnDDLValue2']").val(DV2);
    $("[id$='hdnConnector3']").val(CN3);
    $("select[id$=ddlSearchObject2]").val(SO2);
    $("select[id$=ddlOperator2]").val(OP2);
    $(".txtSearchValue2").val(ST2);
    $(".calTextValue2").val(CAL2);
    $("select[id$=ddlSearchValue2]").val(DV2);
    $("select[id$=ddlConnector3]").val(CN3);
    //ROW 3
    var SO3 = $.cookie("SO3");
    var OP3 = $.cookie("OP3");
    var ST3 = $.cookie("ST3");
    var DV3 = $.cookie("DV3");
    var CAL3 = $.cookie("CAL3");
    var CN4 = $.cookie("CN4");
    showSearchControls(SO3, 3);
    $("[id$='hdnSearchObjectDDL3']").val(SO3);
    $("[id$='hdnOperator3']").val(OP3);
    $("[id$='hdnTextValue3']").val(ST3);
    $("[id$='hdnCalendarValue3']").val(CAL3);
    $("[id$='hdnDDLValue3']").val(DV3);
    $("[id$='hdnConnector4']").val(CN4);
    $("select[id$=ddlSearchObject3]").val(SO3);
    $("select[id$=ddlOperator3]").val(OP3);
    $(".txtSearchValue3").val(ST3);
    $(".calTextValue3").val(CAL3);
    $("select[id$=ddlSearchValue3]").val(DV3);
    $("select[id$=ddlConnector4]").val(CN4);
    //ROW 4
    var SO4 = $.cookie("SO4");
    var OP4 = $.cookie("OP4");
    var ST4 = $.cookie("ST4");
    var DV4 = $.cookie("DV4");
    var CAL4 = $.cookie("CAL4");
    showSearchControls(SO4, 4);
    $("[id$='hdnSearchObjectDDL4']").val(SO4);
    $("[id$='hdnOperator4']").val(OP4);
    $("[id$='hdnTextValue4']").val(ST4);
    $("[id$='hdnCalendarValue4']").val(CAL4);
    $("[id$='hdnDDLValue4']").val(DV4);
    $("select[id$=ddlSearchObject4]").val(SO4);
    $("select[id$=ddlOperator4]").val(OP4);
    $(".txtSearchValue4").val(ST4);
    $(".calTextValue4").val(CAL4);
    $("select[id$=ddlSearchValue4]").val(DV4);
    //basic search
    var BV = $.cookie("BV");
    $("[id$='hdnBasicSearchValue']").val(BV);
    $(".txtBasicSearchValue").val(BV);
    //search toggle
    var ST = $.cookie("ST");
    $("[id$='hdnSearchToggle']").val(ST);
}

function clearSearchCookies() {
    $.removeCookie("SO1");
    $.removeCookie("SO2");
    $.removeCookie("SO3");
    $.removeCookie("SO4");
    $.removeCookie("CN2");
    $.removeCookie("CN3");
    $.removeCookie("CN4");
    $.removeCookie("ST1");
    $.removeCookie("ST2");
    $.removeCookie("ST3");
    $.removeCookie("ST4");
    $.removeCookie("SC1");
    $.removeCookie("SC2");
    $.removeCookie("SC3");
    $.removeCookie("SC4");
    $.removeCookie("SD1");
    $.removeCookie("SD2");
    $.removeCookie("SD3");
    $.removeCookie("SD4");
    $.removeCookie("OP1");
    $.removeCookie("OP2");
    $.removeCookie("OP3");
    $.removeCookie("OP4");
    $.removeCookie("CAL1");
    $.removeCookie("CAL2");
    $.removeCookie("CAL3");
    $.removeCookie("CAL4");
    $.removeCookie("DV1");
    $.removeCookie("DV2");
    $.removeCookie("DV3");
    $.removeCookie("DV4");
    $.removeCookie("BV");
    $.removeCookie("ST");
    $("[id$='hdnSearchObjectDDL1']").val("0");
    $("[id$='hdnOperator1']").val("0");
    $("[id$='hdnTextValue1']").val("");
    $("[id$='hdnCalendarValue1']").val("");
    $("[id$='hdnDDLValue1']").val("");
    $("[id$='hdnConnector2']").val("0");
    $("[id$='hdnSearchObjectDDL2']").val("0");
    $("[id$='hdnOperator2']").val("0");
    $("[id$='hdnTextValue2']").val("");
    $("[id$='hdnCalendarValue2']").val("");
    $("[id$='hdnDDLValue2']").val("");
    $("[id$='hdnConnector3']").val("0");
    $("[id$='hdnSearchObjectDDL3']").val("0");
    $("[id$='hdnOperator3']").val("0");
    $("[id$='hdnTextValue3']").val("");
    $("[id$='hdnCalendarValue3']").val("");
    $("[id$='hdnDDLValue3']").val("");
    $("[id$='hdnConnector4']").val("0");
    $("[id$='hdnSearchObjectDDL4']").val("0");
    $("[id$='hdnOperator4']").val("0");
    $("[id$='hdnTextValue4']").val("");
    $("[id$='hdnCalendarValue4']").val("");
    $("[id$='hdnDDLValue4']").val("");
    $("[id$='hdnBasicSearchValue']").val("");
    $('input[id$=hdnSearchResults]').val("");
    $("[id$='hdnSearchToggle']").val("");
    //reset search value controls
    showSearchControls(0, 1);
    showSearchControls(0, 2);
    showSearchControls(0, 3);
    showSearchControls(0, 4);
    //reset search object ddls
    $("select[id$=ddlSearchObject1]").val("0");
    $("select[id$=ddlSearchObject2]").val("0");
    $("select[id$=ddlSearchObject3]").val("0");
    $("select[id$=ddlSearchObject4]").val("0");
    //reset operator ddls
    $("select[id$=ddlOperator1]").val("0");
    $("select[id$=ddlOperator2]").val("0");
    $("select[id$=ddlOperator3]").val("0");
    $("select[id$=ddlOperator4]").val("0");
    //reset connector ddls
    $("select[id$=ddlConnector2]").val("0");
    $("select[id$=ddlConnector3]").val("0");
    $("select[id$=ddlConnector4]").val("0");
    //clear edit mode for doc manager
    $('input[id$=hdnChapterEditMode]').val("");
}

function connectorDLL2_Changed(e) {
    var connector2DDL = $(".searchConnector2");
    //store selected value in hidden field
    var selectedConnector = connector2DDL.find(":selected").val();
    $("[id$='hdnConnector2']").val(selectedConnector);
    //turn on/off associated search row
    var connector3DDL = $(".searchConnector3");
    var object2 = $(".searchObject2");
    var operator2 = $(".searchOperator2");
    var txtvalue2 = $(".txtSearchValue2");
    var ddlValue2 = $(".ddlSearchValue2");
    var calValue2 = $(".calTextValue2");
    if (connector2DDL[0].selectedIndex > 0) {

        object2[0].removeAttribute("disabled");
        operator2[0].removeAttribute("disabled");
        txtvalue2[0].removeAttribute("disabled");
        ddlValue2[0].removeAttribute("disabled");
        calValue2[0].removeAttribute("disabled");
        connector3DDL[0].removeAttribute("disabled");
    }
    else {
        object2[0].setAttribute("disabled", true);
        operator2[0].setAttribute("disabled", true);
        txtvalue2[0].setAttribute("disabled", true);
        ddlValue2[0].setAttribute("disabled", true);
        calValue2[0].setAttribute("disabled", true);
        connector3DDL[0].setAttribute("disabled", true);
    }
}

function connectorDLL3_Changed() {
    var connector3DDL = $(".searchConnector3");
    //store selected value in hidden field
    var selectedConnector = connector3DDL.find(":selected").val();
    $("[id$='hdnConnector3']").val(selectedConnector);
    //turn on/off associated search row
    var connector4DDL = $(".searchConnector4");
    var object3 = $(".searchObject3");
    var operator3 = $(".searchOperator3");
    var value3 = $(".txtSearchValue3");
    if (connector3DDL[0].selectedIndex == 1 || connector3DDL[0].selectedIndex == 2) {
        object3[0].removeAttribute("disabled");
        operator3[0].removeAttribute("disabled");
        value3[0].removeAttribute("disabled");
        connector4DDL[0].removeAttribute("disabled");
    }
    else {
        object3[0].setAttribute("disabled", true);
        operator3[0].setAttribute("disabled", true);
        value3[0].setAttribute("disabled", true);
        connector4DDL[0].setAttribute("disabled", true);
    }
}

function connectorDLL4_Changed() {
    var connector4DDL = $(".searchConnector4");
    //store selected value in hidden field
    var selectedConnector = connector4DDL.find(":selected").val();
    $("[id$='hdnConnector4']").val(selectedConnector);
    //turn on/off associated search row
    var object4 = $(".searchObject4");
    var operator4 = $(".searchOperator4");
    var value4 = $(".txtSearchValue4");
    if (connector4DDL[0].selectedIndex == 1 || connector4DDL[0].selectedIndex == 2) {
        object4[0].removeAttribute("disabled");
        operator4[0].removeAttribute("disabled");
        value4[0].removeAttribute("disabled");
    }
    else {
        object4[0].setAttribute("disabled", true);
        operator4[0].setAttribute("disabled", true);
        value4[0].setAttribute("disabled", true);
    }
}

function getDataTypeForSearchObject(searchObjectID) {
    var dataTypeID = 0;
    projectID = $('input[id$=hdnProjectID]').val();
    $.webMethod({ 'methodName': 'getDataTypeForSearchObject', 'parameters': { 'searchObjectID': searchObjectID, 'projectID': projectID },
        success: function (response) {
            dataTypeID = response;
        }
    });
    return dataTypeID;
}

function getSearchObjectValuesForDDL(searchObjectID) {
    var listItems;
    projectID = $('input[id$=hdnProjectID]').val();
    $.webMethod({ 'methodName': 'getSearchObjectValuesForDDL', 'parameters': { 'searchObjectID': searchObjectID, 'projectID': projectID },
        success: function (response) {
            listItems = JSON.parse(response);
        }
    });
    return listItems;
}

function showSearchControls(selectedSearchObjectID, rowNumber) {
    var searchValueDDL, searchValueTextbox, searchValueCalendar, dataTypeID;
    if (selectedSearchObjectID == "" || selectedSearchObjectID == null) {
        dataTypeID = -1;
    }
    else {
        dataTypeID = getDataTypeForSearchObject(selectedSearchObjectID);
    }
    switch(rowNumber) {
        case (1):
            searchValueDDL = $("select[id$=ddlSearchValue1]");
            searchValueTextbox = $(".txtSearchValue1");
            searchValueCalendar = $(".calTextValue1");
            $('input[id$=hdnDataType1]').val(dataTypeID);
            break;
        case(2):
            searchValueDDL = $("select[id$=ddlSearchValue2]");
            searchValueTextbox = $(".txtSearchValue2");
            searchValueCalendar = $(".calTextValue2");
            $('input[id$=hdnDataType2]').val(dataTypeID);
            break;
        case(3):
            searchValueDDL = $("select[id$=ddlSearchValue3]");
            searchValueTextbox = $(".txtSearchValue3");
            searchValueCalendar = $(".calTextValue3");
            $('input[id$=hdnDataType3]').val(dataTypeID);
            break;
        case(4):
            searchValueDDL = $("select[id$=ddlSearchValue4]");
            searchValueTextbox = $(".txtSearchValue4");
            searchValueCalendar = $(".calTextValue4");
            $('input[id$=hdnDataType4]').val(dataTypeID);
            break;
    }
    //empty all row value controls before showing
    searchValueDDL.empty();
    searchValueTextbox.val("");
    searchValueCalendar.val("");
    switch (dataTypeID) {
        //blank
        case (-1):
            searchValueCalendar.hide();
            searchValueDDL.hide();
            searchValueDDL.empty();
            searchValueTextbox.hide();
        //ID
        case (0):
            searchValueCalendar.hide();
            searchValueTextbox.show();
            searchValueDDL.empty();
            searchValueDDL.hide();
            break;
        //pk XREF
        case (1):
            searchValueCalendar.hide();
            searchValueTextbox.hide();
            searchValueDDL.show();
            var items = getSearchObjectValuesForDDL(selectedSearchObjectID);
            searchValueDDL.empty().append($("<option></option>").val("0").html("Select one"));
            $.each(items, function() {
                searchValueDDL.append($("<option></option>").val(this['ID']).html(this['TEXT']));
            });
            break;
        //string
        case(2):
            searchValueCalendar.hide();
            searchValueTextbox.show();
            searchValueDDL.hide();
            searchValueDDL.empty();
            break;
        //date
        case(3):
            searchValueCalendar.show();
            searchValueTextbox.hide();
            searchValueDDL.hide();
            searchValueDDL.empty();
            break;
        //bool
        case (4):
            searchValueCalendar.hide();
            searchValueTextbox.hide();
            searchValueDDL.show();
            searchValueDDL.empty();
            searchValueDDL.append($("<option></option>").val("0").html("Select one"));
            searchValueDDL.append($("<option></option>").val("1").html("true"));
            searchValueDDL.append($("<option></option>").val("2").html("false"));
            searchValueDDL.val("0");
            break;
    }
}

function lbSearch_Click() {
    //determine if basic or advance search is showing
    var BasicSearchDIV = $("#divBasicSearch");
    var AdvancedSearchDIV = $("#divAdvancedSearch");
    var searchLink = $(".searchLink");
    if (searchLink[0].text == "Basic Search") {
        searchLink.text("Advanced Search");
        AdvancedSearchDIV.hide();
        BasicSearchDIV.show();
        clearSearchCookies();
    }
    else if (searchLink[0].text == "Advanced Search") {
        searchLink.text("Basic Search");
        BasicSearchDIV.hide();
        AdvancedSearchDIV.show();
    }
}

function btnGridBasicSearch_Click() {
    $('input[id$=hdnSearchToggle]').val("basic");
    saveSearchInfo();
    return true;    
}

function btnGridAdvancedSearch_Click() {
    $('input[id$=hdnSearchToggle]').val("advanced");
    saveSearchInfo();
    return true;
}

function getAdvancedSearchResults() {
    var projectID, searchType, ClientSearchData, searchResults;
    saveSearchInfo();
    projectID = $('input[id$=hdnProjectID]').val();
    searchType = $('input[id$=hdnSearchTypeID]').val();
    ClientSearchData = getAdvancedSearchDataJSON();
    var searchResultsTreeDIV = $('.treeSearchResults');
    $.webMethod({ 'methodName': 'getAdvancedSearchResults', 'parameters': { 'data': ClientSearchData, 'projID': projectID, 'searchType': searchType },
        success: function (response) {
            searchResults = response;
        }
    });
    $("[id$='hdnSearchResults']").val(searchResults);
    return searchResults;
}

function getBasicSearchResults() {
    var projectID, searchType, ClientSearchData, searchResults, searchResultsTreeDIV;
    saveSearchInfo();
    projectID = $('input[id$=hdnProjectID]').val();
    searchType = $('input[id$=hdnSearchTypeID]').val();
    ClientSearchData = getBasicSearchDataJSON();
    searchResultsTreeDIV = $('.treeSearchResults');
    $.webMethod({ 'methodName': 'getBasicSearchResults', 'parameters': { 'data': ClientSearchData, 'projID': projectID, 'searchType': searchType },
        success: function (response) {
            searchResults = response;
        }
    });
    $("[id$='hdnSearchResults']").val(searchResults);
    return searchResults;
}

function saveSearchInfo() {
    //row 1
    var SO1 = $("[id$='hdnSearchObjectDDL1']").val();
    var OP1 = $("[id$='hdnOperator1']").val();
    var CN2 = $("[id$='hdnConnector2']").val();
    var ST1 = $("[id$='hdnTextValue1']").val();
    var CAL1 = $("[id$='hdnCalendarValue1']").val();
    var DV1 = $("[id$='hdnDDLValue1']").val();
    $.cookie('CAL1', CAL1);
    $.cookie('DV1', DV1);
    $.cookie('SO1', SO1);
    $.cookie('OP1', OP1);
    $.cookie('CN2', CN2);
    $.cookie('ST1', ST1);
    //row 2
    var SO2 = $("[id$='hdnSearchObjectDDL2']").val();
    var OP2 = $("[id$='hdnOperator2']").val();
    var CN3 = $("[id$='hdnConnector3']").val();
    var ST2 = $("[id$='hdnTextValue2']").val();
    var CAL2 = $("[id$='hdnCalendarValue2']").val();
    var DV2 = $("[id$='hdnDDLValue2']").val();
    $.cookie('CAL2', CAL2);
    $.cookie('DV2', DV2);
    $.cookie('SO2', SO2);
    $.cookie('OP2', OP2);
    $.cookie('CN3', CN3);
    $.cookie('ST2', ST2);
    //row 3
    var SO3 = $("[id$='hdnSearchObjectDDL3']").val();
    var OP3 = $("[id$='hdnOperator3']").val();
    var CN4 = $("[id$='hdnConnector4']").val();
    var ST3 = $("[id$='hdnTextValue3']").val();
    var CAL3 = $("[id$='hdnCalendarValue3']").val();
    var DV3 = $("[id$='hdnDDLValue3']").val();
    $.cookie('CAL3', CAL3);
    $.cookie('DV3', DV3);
    $.cookie('SO3', SO3);
    $.cookie('OP3', OP3);
    $.cookie('CN4', CN4);
    $.cookie('ST3', ST3);
    //row 4
    var SO4 = $("[id$='hdnSearchObjectDDL4']").val();
    var OP4 = $("[id$='hdnOperator4']").val();
    var ST4 = $("[id$='hdnTextValue4']").val();
    var CAL4 = $("[id$='hdnCalendarValue4']").val();
    var DV4 = $("[id$='hdnDDLValue4']").val();
    $.cookie('CAL4', CAL4);
    $.cookie('DV4', DV4);
    $.cookie('SO4', SO4);
    $.cookie('OP4', OP4);
    $.cookie('ST4', ST4);
    var BV = $("[id$='hdnBasicSearchValue']").val();
    $.cookie('BV', BV);
    var ST = $("[id$='hdnSearchToggle']").val();
    $.cookie('ST', ST);
}

function getBasicSearchDataJSON() {
    var ClientSearchData = [];
    ClientSearchData.push({
        "objectID": "",
        "operatorID": "",
        "textValue": $("[id$='hdnBasicSearchValue']").val(),
        "calendarValue": "",
        "ddlValue": "",
        "connector": ""
    });
    return ClientSearchData;
}

function getAdvancedSearchDataJSON() {
    var ClientSearchData = [];
    //get all connectors so we can determine if we need to get data from next row
    var CN2 = $("[id$='hdnConnector2']").val();
    var CN3 = $("[id$='hdnConnector3']").val();
    var CN4 = $("[id$='hdnConnector4']").val();
    //    ROW 1 
    ClientSearchData.push({
        "objectID": $("[id$='hdnSearchObjectDDL1']").val(),
        "operatorID": $("[id$='hdnOperator1']").val(),
        "textValue": $("[id$='hdnTextValue1']").val(),
        "calendarValue": $("[id$='hdnCalendarValue1']").val(),
        "ddlValue": $("[id$='hdnDDLValue1']").val(),
        "connector": CN2
    });
    if (CN2 != "0" && CN2 != null) {
        //ROW 2
        ClientSearchData.push({
            "objectID": $("[id$='hdnSearchObjectDDL2']").val(),
            "operatorID": $("[id$='hdnOperator2']").val(),
            "textValue": $("[id$='hdnTextValue2']").val(),
            "calendarValue": $("[id$='hdnCalendarValue2']").val(),
            "ddlValue": $("[id$='hdnDDLValue2']").val(),
            "connector": CN3
        });
    }
    if (CN3 != "0" && CN3 != null) {
        //ROW 3
        ClientSearchData.push({
            "objectID": $("[id$='hdnSearchObjectDDL3']").val(),
            "operatorID": $("[id$='hdnOperator3']").val(),
            "textValue": $("[id$='hdnTextValue3']").val(),
            "calendarValue": $("[id$='hdnCalendarValue3']").val(),
            "ddlValue": $("[id$='hdnDDLValue3']").val(),
            "connector": CN4
        });
    }
    if (CN4 != "0" && CN4 != null) {
        //ROW 4
        ClientSearchData.push({
            "objectID": $("[id$='hdnSearchObjectDDL4']").val(),
            "operatorID": $("[id$='hdnOperator4']").val(),
            "textValue": $("[id$='hdnTextValue4']").val(),
            "calendarValue": $("[id$='hdnCalendarValue4']").val(),
            "ddlValue": $("[id$='hdnDDLValue4']").val(),
            "connector": ""
        });
    }
    return ClientSearchData;
}

function getURLForSearchType(searchTypeID) {
    var url = "";
    switch (searchTypeID) {
        case ("1"):
            url = "BusinessRules.aspx/getSearchResultsForTree";
            break;
        case ("2"):
            url = "CustomerServiceMessages.aspx/getSearchResultsForTree"
            break;
        case ("3"):
            //3 = defects search.  Grid used to display results instead of tree.
            break;
        case ("4"):
            url = "TestCases.aspx/getSearchResultsForTree";
            break;
        case ("5"):
            url = "DocumentManager.aspx/getSearchResultsForTree";
            break;
    }
    return url;
}

function populateSearchResultsTree(searchResult) {
    "use strict";
    var projectID, searchResultsTreeDIV, a, defaultID, searchTypeID, searchResult, brTreeDIV, url;
    defaultID = 0;
    projectID = $('input[id$=hdnProjectID]').val();
    searchTypeID = $('input[id$=hdnSearchTypeID]').val();
    url = getURLForSearchType(searchTypeID);
    searchResultsTreeDIV = $('.treeSearchResults');
    searchResultsTreeDIV.addClass("JQueryDebug");
    searchResultsTreeDIV.jstree({
        "json_data": {
            "ajax": {
                "type": "POST",
                "dataType": "json",
                "contentType": "application/json;",
                "url": url,
                "data": function (node) {
                    if (node === -1) {

                        return '{  "nodeID" : ' + defaultID + ', "searchResult" : ' + searchResult + ', "projID" : ' + projectID + ', "searchType" : ' + searchTypeID + '}';
                    }
                    else {
                        var nid = node[0].id;
                        return '{ "nodeID" :  "' + nid + '", "searchResult" : ' + searchResult + ', "projID" : ' + projectID + ', "searchType" : ' + searchTypeID + '}';
                    }
                },
                "success": function (retval) {
                    return retval.d;
                },
                "error": function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("err: " + errorThrown);
                }
            }
        },
        "themes": {
            "theme": "default",
            "dots": true,
            "icons": true
        },
        "contextmenu": {
            "items": customMenu
        },
        "cookies": { cookie_options: { path: '/'} },
        "plugins": ["themes", "json_data", "ui", "contextmenu", "cookies"]
    })
        .bind("select_node.jstree", function (NODE, REF_NODE) {
            a = $.jstree._focused().get_selected();
            onSRNodeSelected(a[0]);
        });
}

function onSRNodeSelected(node) {
    "use strict";
    if (!(typeof (node) == 'undefined' || node == null)) {
        var nodeArray, brID, csmID, projectID, sectionID, parentNode, parentNodeArray;
        //get section id from parent node
        parentNode = node.parentNode.parentNode;
        parentNodeArray = parentNode.id.split("_");
        nodeArray = node.id.split("_");
        if (nodeArray[0] == "BR") {
            brID = nodeArray[1];
            projectID = $('input[id$=hdnProjectID]').val();
            $.webMethod({ 'methodName': 'GetBusinessTextByRuleID', 'parameters': { 'projID': projectID, 'brID': brID },
                success: function (response) {
                    var RuleText = $(".ObjText");
                    RuleText[0].innerHTML = response;
                }
            });
        }
        else if (nodeArray[0] == "CSM") {
            csmID = nodeArray[1];
            projectID = $('input[id$=hdnProjectID]').val();
            $.webMethod({ 'methodName': 'GetCSMTextByID', 'parameters': { 'projID': projectID, 'csmID': csmID },
                success: function (response) {
                    var CSMText = $(".ObjText");
                    CSMText[0].innerHTML = response;
                }
            });
        }
        else {
            var RuleText = $(".ObjText");
            RuleText[0].innerHTML = "";

        }
    }
}

function initializeSearchControls() {
    var searchDIV = $(".searchDIV");
    if ($(".searchDIV").is(':visible')) {
        loadSearchControls();
        var searchTypeID = $("[id$='hdnSearchTypeID']").val();
        var basicSearchButton = $('input[id$=btnBasicSearch]');
        basicSearchButton.addClass("JQueryDebug");
        if (searchTypeID == "3") {
            basicSearchButton.click(btnGridBasicSearch_Click);
        }
        else {
            basicSearchButton.click(btnTreeBasicSearch_Click);
        }
        var clearBasicSearchButton = $('input[id$=btnClearBasicSearch]');
        clearBasicSearchButton.addClass("JQueryDebug");
        clearBasicSearchButton.click(clearSearchCookies);
        var clearAdvancedSearchButton = $('input[id$=btnClearAdvancedSearch]');
        clearAdvancedSearchButton.addClass("JQueryDebug");
        clearAdvancedSearchButton.click(clearSearchCookies);
        var gridAdvancedSearchButton = $('input[id$=btnGridAdvancedSearch]');
        gridAdvancedSearchButton.addClass("JQueryDebug");
        gridAdvancedSearchButton.click(btnGridAdvancedSearch_Click);
        var treeAdvancedSearchButton = $('input[id$=btnTreeAdvancedSearch]');
        treeAdvancedSearchButton.addClass("JQueryDebug");
        treeAdvancedSearchButton.click(btnTreeAdvancedSearch_Click);
        var searchLink = $(".searchLink");
        searchLink.click(lbSearch_Click);
        searchLink.addClass("JQueryDebug");
        //show basic or advanced search
        var BasicSearchDIV = $("#divBasicSearch");
        var AdvancedSearchDIV = $("#divAdvancedSearch");
        var searchObject1DDL = $("select[id$=ddlSearchObject1]");
        var advCookieExists = $.cookie("SO1");
        var searchToggle = $("[id$='hdnSearchToggle']").val();
        if (searchToggle == "basic" || searchToggle == "" || searchToggle == null) {
            searchLink[0].text = "Advanced Search";
            BasicSearchDIV.show();
            AdvancedSearchDIV.hide();
        }
        else if (searchToggle == "advanced") {
            searchLink[0].text = "Basic Search";
            BasicSearchDIV.hide();
            AdvancedSearchDIV.show();
            loadSearchControls();
        }
        //set up connector ddl events
        //var connector2DDL = $(".searchConnector2");
        var connector2DDL = $("select[id$=ddlConnector2]");

        connector2DDL.addClass("JQueryDebug");
        connector2DDL.change(connectorDLL2_Changed);
        var connector3DDL = $(".searchConnector3");
        connector3DDL.addClass("JQueryDebug");
        connector3DDL.change(connectorDLL3_Changed);
        var connector4DDL = $(".searchConnector4");
        connector4DDL.addClass("JQueryDebug");
        connector4DDL.change(connectorDLL4_Changed);
        //disable all advanced search rows except the first.
        connectorDLL2_Changed();
        connectorDLL3_Changed();
        connectorDLL4_Changed();
        searchObject1DDL.addClass("JQueryDebug");
        searchObject1DDL.change(function (e) {
            var selectedSearchObjectID = searchObject1DDL.find(":selected").val();
            $("[id$='hdnSearchObjectDDL1']").val(selectedSearchObjectID);
            showSearchControls(selectedSearchObjectID, 1);
        });
        var searchObject2DDL = $("select[id$=ddlSearchObject2]");
        searchObject2DDL.addClass("JQueryDebug");
        searchObject2DDL.change(function (e) {
            var selectedSearchObjectID = searchObject2DDL.find(":selected").val();
            $("[id$='hdnSearchObjectDDL2']").val(selectedSearchObjectID);
            showSearchControls(selectedSearchObjectID, 2);
        });
        var searchObject3DDL = $("select[id$=ddlSearchObject3]");
        searchObject3DDL.addClass("JQueryDebug");
        searchObject3DDL.change(function (e) {
            var selectedSearchObjectID = searchObject3DDL.find(":selected").val();
            $("[id$='hdnSearchObjectDDL3']").val(selectedSearchObjectID);
            showSearchControls(selectedSearchObjectID, 3);
        });
        var searchObject4DDL = $("select[id$=ddlSearchObject4]");
        searchObject4DDL.addClass("JQueryDebug");
        searchObject4DDL.change(function (e) {
            var selectedSearchObjectID = searchObject4DDL.find(":selected").val();
            $("[id$='hdnSearchObjectDDL4']").val(selectedSearchObjectID);
            showSearchControls(selectedSearchObjectID, 4);
        });
        var searchOperator1DDL = $("select[id$=ddlOperator1]");
        searchOperator1DDL.addClass("JQueryDebug");
        searchOperator1DDL.change(function (e) {
            var selectedSearchOperator = searchOperator1DDL.find(":selected").val();
            $("[id$='hdnOperator1']").val(selectedSearchOperator);
        });
        var searchOperator2DDL = $("select[id$=ddlOperator2]");
        searchOperator2DDL.addClass("JQueryDebug");
        searchOperator2DDL.change(function (e) {
            var selectedSearchOperator = searchOperator2DDL.find(":selected").val();
            $("[id$='hdnOperator2']").val(selectedSearchOperator);
        });
        var searchOperator3DDL = $("select[id$=ddlOperator3]");
        searchOperator3DDL.addClass("JQueryDebug");
        searchOperator3DDL.change(function (e) {
            var selectedSearchOperator = searchOperator3DDL.find(":selected").val();
            $("[id$='hdnOperator3']").val(selectedSearchOperator);
        });
        var searchOperator4DDL = $("select[id$=ddlOperator4]");
        searchOperator4DDL.addClass("JQueryDebug");
        searchOperator4DDL.change(function (e) {
            var selectedSearchOperator = searchOperator4DDL.find(":selected").val();
            $("[id$='hdnOperator4']").val(selectedSearchOperator);
        });
        var searchValue1 = $("input[id$=txtValue1]");
        searchValue1.change(function (e) { $("[id$='hdnTextValue1']").val(searchValue1.val()); });
        searchValue1.addClass("JQueryDebug");
        //searchValue1.show();
        var searchValue2 = $("input[id$=txtValue2]");
        searchValue2.change(function (e) { $("[id$='hdnTextValue2']").val(searchValue2.val()); });
        searchValue2.addClass("JQueryDebug");
        var searchValue3 = $("input[id$=txtValue3]");
        searchValue3.change(function (e) { $("[id$='hdnTextValue3']").val(searchValue3.val()); });
        searchValue3.addClass("JQueryDebug");
        var searchValue4 = $("input[id$=txtValue4]");
        searchValue4.change(function (e) { $("[id$='hdnTextValue4']").val(searchValue4.val()); });
        searchValue4.addClass("JQueryDebug");
        searchValueDDL1 = $("select[id$=ddlSearchValue1]");
        searchValueDDL1.addClass("JQueryDebug");
        searchValueDDL1.change(function (e) { $("[id$='hdnDDLValue1']").val(searchValueDDL1.find(":selected").val()) });
        searchValueDDL2 = $("select[id$=ddlSearchValue2]");
        searchValueDDL2.addClass("JQueryDebug");
        searchValueDDL2.change(function (e) { $("[id$='hdnDDLValue2']").val(searchValueDDL2.find(":selected").val()) });
        searchValueDDL3 = $("select[id$=ddlSearchValue3]");
        searchValueDDL3.addClass("JQueryDebug");
        searchValueDDL3.change(function (e) { $("[id$='hdnDDLValue3']").val(searchValueDDL3.find(":selected").val()) });
        searchValueDDL4 = $("select[id$=ddlSearchValue4]");
        searchValueDDL4.addClass("JQueryDebug");
        searchValueDDL4.change(function (e) { $("[id$='hdnDDLValue4']").val(searchValueDDL4.find(":selected").val()) });
        searchValueCalendar1 = $(".calTextValue1");
        searchValueCalendar1.change(function (e) { $("[id$='hdnCalendarValue1']").val(searchValueCalendar1.val()) });
        searchValueCalendar2 = $(".calTextValue2");
        searchValueCalendar2.change(function (e) { $("[id$='hdnCalendarValue2']").val(searchValueCalendar2.val()) });
        searchValueCalendar3 = $(".calTextValue3")
        searchValueCalendar3.change(function (e) { $("[id$='hdnCalendarValue3']").val(searchValueCalendar3.val()) });
        searchValueCalendar4 = $(".calTextValue4");
        searchValueCalendar4.change(function (e) { $("[id$='hdnCalendarValue4']").val(searchValueCalendar4.val()) });
        var basicSearchValue = $("input[id$=txtBasicSearch]");
        basicSearchValue.addClass("JQueryDebug");
        basicSearchValue.change(function (e) { $("[id$='hdnBasicSearchValue']").val(basicSearchValue.val()); });
        //defects searh type = 3 and uses grid for search results instead of jstree
        if (searchTypeID != "3") {
            //check search toggle and perform search 
            var ST = $("[id$='hdnSearchToggle']").val();
            if (ST == "basic") {
                btnTreeBasicSearch_Click()
            }
            else if (ST == "advanced") {
                btnTreeAdvancedSearch_Click()
            }
        }
    }
}

function btnTreeAdvancedSearch_Click() {
    $("[id$='hdnSearchToggle']").val("advanced");
    var searchResult = JSON.stringify(getAdvancedSearchResults());
    populateSearchResultsTree(searchResult);
    if (searchResult == "[]") { $("[id$='hdnSearchResults']").val("0"); }
    showSearchResultTrees();
    //clear text display panel
    var objTextArea = $(".ObjText");
    if (objTextArea[0] != null) {
        objTextArea[0].innerHTML = "";
    }
}

function btnTreeBasicSearch_Click() {
    $("[id$='hdnSearchToggle']").val("basic");
    var searchResult = JSON.stringify(getBasicSearchResults());
    populateSearchResultsTree(searchResult);
    if (searchResult == "[]") { $("[id$='hdnSearchResults']").val("0"); }
    showSearchResultTrees();
    //clear text display panel
    var objTextArea = $(".ObjText");
    if (objTextArea[0] != null) {
        objTextArea[0].innerHTML = "";
    }
}

String.prototype.replaceAll = function (search, replace) {
    //if replace is null, return original string otherwise it will
    //replace search string with 'undefined'.
    if (!replace)
        return this;

    return this.replace(new RegExp('[' + search + ']', 'g'), replace);
};

