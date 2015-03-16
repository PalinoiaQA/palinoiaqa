/// <reference path="../../jquery.validate-vsdoc.js" />
/// <reference path="../../jquery-1.8.3-vsdoc.js" />
/*jslint browser: true*/
/*global $, jQuery, alert, confirm*/

//$(document).ready(function () {
//    startTimeoutMonitor();
//});
var sessionTimeoutWarning;
var sessionTimeout;
var timeOnPageLoad;

// Include this function before you use any selectors that rely on it
if ($.expr.createPseudo) {
    jQuery.expr[':'].asp = $.expr.createPseudo(function (id) {
        "use strict";
        return function (elem) {
            return elem.id && elem.id.match(id + "$");
        };
    });
} else {
    jQuery.expr[':'].asp = function (elem, i, match) {
        "use strict";
        return !!(elem.id && elem.id.match(match[3] + "$"));
    };
}

function messageToClient(message) {
    "use strict";
    alert(message);
}

(function ($) {
    "use strict";
    $.webMethod = function (options) {
        var settings, result, parameterjson;
        settings = $.extend({
            'methodName': '',
            'async': false,
            'cache': false,
            timeout: 30000,
            debug: true,
            'parameters': {},
            success: function (response) { },
            error: function (response) { }
        }, options);
        parameterjson = "{}";
        result = null;
        if (settings.parameters !== null) {
            parameterjson = $.toJSON(settings.parameters);
        }
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
}(jQuery));

(function ($) {
    "use strict";
    $.asycWebMethod = function (options) {
        var settings, result, parameterjson;
        settings = $.extend({
            'methodName': '',
            'async': true,
            'cache': false,
            timeout: 30000,
            debug: true,
            'parameters': {},
            success: function (response) { },
            error: function (response) { }
        }, options);
        parameterjson = "{}";
        result = null;
        if (settings.parameters !== null) {
            parameterjson = $.toJSON(settings.parameters);
        }
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
} (jQuery));



