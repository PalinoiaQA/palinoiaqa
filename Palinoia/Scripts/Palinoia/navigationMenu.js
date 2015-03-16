/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/// <reference path="../../jquery.json-2.4.min.js" />
/// <reference path="../../JSTree/jquery.jstree.js" />
/// <reference path="../../jquery-ui-1.9.0.min.js" />
var first = false;

$(document).ready(function () {
    populateNavigationMenu();
    var treeDIV = $('.treeNavigation');
    treeDIV.addClass("JQueryDebug");
});

function pageLoad(sender, args) {

}

function populateNavigationMenu() {
    "use strict";
    var treeDIV,
        projectID,
        a,
        defaultID,
        userID,
        defaultID = 0,
        userID = $('input[id$=hdnUserID]').val(),
        projectID = $('input[id$=hdnProjectID]').val(),
        root = window.location.host,
        webMethod = "http://" + root + "/UI/NavigationMenu.asmx/GetNavigationMenuItems";
    //the treeDIV will contain the tree
    treeDIV = $('.treeNavigation');
    //do not load tree is no user id or project id
    if (userID != "0" && projectID != "0") {
        treeDIV.jstree({
            "json_data": {
                "ajax": {
                    "type": "POST",
                    "dataType": "json",
                    "contentType": "application/json;",
                    "url": webMethod,
                    "data": function (node) {
                        if (node === -1) {
                            if (userID != "0") {
                                $(".leftCol").show();
                                return '{  "nodeID" : ' + defaultID + ', "userID" : "' + userID + '"}';

                            }
                            else {
                                $(".leftCol").hide();
                            }
                        }
                        else {
                            $(".leftCol").show();
                            var nid = node[0].id;
                            return '{  "nodeID" :  "' + nid + '", "userID" : "' + userID + '" }';
                        }
                    },
                    "success": function (retval) {

                        return retval.d;
                    },
                    "error": function (xhr, status, error) {
                        alert("responseText=" + xhr.responseText +
                      "\n textStatus=" + status + "\n errorThrown=" + error + "\n webmethod=" + webMethod + "\n userid: " + userID + "\n projectid: " + projectID);
                    }
                }
            },
            "ui": {
                "initially_select": ["0"]
            },
            "themes": {
                "theme": "classic",
                "dots": true,
                "icons": false
            },
            "cookies": { cookie_options: { path: '/'} },
            "plugins": ["themes", "json_data", "crrm", "ui", "cookies"]
        })
            .bind("select_node.jstree", function (NODE, data) {
                if (!first) {
                    first = true;
                    //alert(data.rslt.obj[0].id);
                    return;
                }
                else {
                    var fullID = data.rslt.obj[0].id;
                    var idArray = fullID.split("_");
                    var name = idArray[0];
                    var id = idArray[1];
                    if (name == "ITM") {
                        NavigateToPage(id);
                    }
                    if (name == "0") {
                        NavigateToPage("1");
                        return;
                    }
                }
            });
        var selectedNode = treeDIV.jstree("get_selected").attr('id');
        if (selectedNode == "") {
            treeDIV.jstree("select_node", "0");
        }
    }
    else {
        // remove border since tree is not loaded with elements
        treeDIV.css("border", "none");
    }
    var mainDIV = $(".main");
    mainDIV.addClass("JQueryDebug");
}

function NavigateToPage(pageID) {
    var root = window.location.host;
    var webMethod = "http://" + root + "/UI/NavigationMenu.asmx/GetNavigationURL";
    var parameters = "{'id':'" + pageID + "'}";
         $.ajax({
            type: "POST",
            url: webMethod,
            data: parameters,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                window.location.href = msg.d;
            },
            error: function (e) {
                alert("Error navigating to page");
            }
        });
    }