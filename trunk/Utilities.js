function trim(stringToTrim) {return stringToTrim.replace(/^\s+|\s+$/g,"");}
function ltrim(stringToTrim) {return stringToTrim.replace(/^\s+/,"");}
function rtrim(stringToTrim) {return stringToTrim.replace(/\s+$/,"");}
function calcIST_(dStr) {
    if (!dStr) return null;
    var d = (typeof (dStr) == "string" ? new Date(Date.parse(dStr)) : dStr);
    utc = d.getTime() + (d.getTimezoneOffset() * 60000);
    nd = new Date(utc + (3600000 * 5.5));
    return nd;
}
function isURL(s) { var regexp = /(ftp|http|https):\/\/(\w+:{0,1}\w*@)?(\S+)(:[0-9]+)?(\/|\/([\w#!:.?+=&%@!\-\/]))?/i; return regexp.test(s); }   
function makeTablePreamble_(style) { return "<table style='"+style+"border: 1px solid #cef;text-align: left;'>"; }
function makeTableHeader_(text) { if (typeof (text) == "object") { var retVal = ""; for (var i = 0; i < text.length; ++i) retVal += makeTableHeader_(text[i]); return retVal; } else return "<th style=\'font-weight: bold;background-color: #acf;border-bottom: 1px solid #cef;padding: 0;\'>" + text + "</th>"; }
function makeTableCell_(text, row) { if (typeof (text) == "object") { var retVal = ""; for (var i = 0; i < text.length; ++i) retVal += makeTableCell_(text[i], row); return retVal; } if (row % 2 == 0) return "<td style=\'padding: 0;background-color: #def; border-bottom: 1px solid #cef;\'>" + text + "</td>"; else return "<td style=\'padding: 0;\'>" + text + "</td>"; }
function getResultsURL(event) {
    var text = event.getContent().getText();
    var patternString = "(result|results)\\s*:\\s*([^\\s\\n\\t\\f\\v\\0]*)";
    var pattern = new RegExp(patternString, "i");
    var match = pattern.exec(text);
    if (match != null) {
        var ret = (isURL(match[2]) ? match[2] : event.getHtmlLink().getHref());
        return ret;
    }
    var patt1Str = "<a([^>]+)>\\s*(results|result)\\s*</a>";
    var patt1 = new RegExp(patt1Str, "i");
    var match = patt1.exec(text);
    if (match == null) return null;
    var patt2Str = "href\\s*=\\s*(\"([^\"]*)\"|'([^']*)'|([^'\">\\s]+))";
    var patt2 = new RegExp(patt2Str, "i");
    match = patt2.exec(match[1]);
    if (match == null) return null;
    if (typeof (match[3]) != "undefined" && isURL(match[3])) return match[3];
    if (typeof (match[2]) != "undefined" && isURL(match[2])) return match[2];
    if (typeof (match[1]) != "undefined" && isURL(match[1])) return match[1];
    return null;
};
function makeCityTitle(title, cityName) {
    var html = "";
    html += stringExists(title) ? title : "Events in";
    html += " ";
    var displayCityName = (stringExists(cityName) ? capitalize(cityName) : "India");
    html += displayCityName;
    return html;
}
function displayEvents(events, otherParameters) {
    var functionObjects = { "Event Name": getEventName, "Event Date": getEventDate, "Results Link": getResultsLink, "Recurrence": getRecurrence };
    var html = makeCityTitle(otherParameters.title,otherParameters.cityName);
    html+=makeTablePreamble_("font-size:small;");
    html += "<thead>";
    html += makeTableHeader_(otherParameters.columns);
    html += "</thead><tbody>";
    if (events.length > 0) {
        for (var i = 0; i < events.length; i++) {
            var event = events[i];
            var cells = new Array();
            for (var j = 0; j < otherParameters.columns.length; ++j) {
                cells.push(functionObjects[otherParameters.columns[j]](event));
            }
            html += "<tr>" + makeTableCell_(cells, i) + "</tr>";
        }
    }
    else {
        html = "<h2>No Events found in Indian Bridge Calendar!!!</h2>";
    }
    return html;
}
var getEventName = function (event) { return event.getTitle().getText(); }
var getEventDate = function (event) { return event.getTimes()[0].getStartTime().getDate().toDateString(); }
var getRecurrence = function (event) {
    //return getEventPeriod(event);
    return getEventPeriod(event) + " " + getEventWeek(event) + " " + getEventDay(event) + " at "+ getEventTime(event); 
}
var getResultsLink = function (event) {
    var resultsLink = getResultsURL(event);
    var resultsText = (resultsLink ? '<a target="_blank" href="' + resultsLink + '">Results</a>' : '<img title="Not Published" src="http://indianbridgefiles.appspot.com/images/publish_x.png" alt="Not Published"/>');
    return resultsText;
}
function getEvents(queryParameters, otherParameters) {
    if (typeof (queryParameters) == "undefined") queryParameters = {};
    if (typeof (otherParameters) == "undefined") otherParameters = {};
    if (typeof (otherParameters.eventName) == "undefined" && otherParameters.eventName) queryParameters.FullTextQuery = otherParameters.eventName;
    else if (typeof (otherParameters.cityName) == "undefined" && otherParameters.cityName) queryParameters.FullTextQuery = otherParameters.cityName;
    var feedUri = 'https://www.google.com/calendar/feeds/indianbridge@gmail.com/public/full';
    var calendarService = new google.gdata.calendar.CalendarService('Indian Bridge Calendar');
    var query = createGoogleQuery(feedUri,queryParameters);
    calendarService.getEventsFeed(query, function (root) { handleSuccess(root, queryParameters,otherParameters); }, function (error) { handleError(error, otherParameters); });
}
var createGoogleQuery = function (eventFeedURL, queryParameters) {
    var query = new google.gdata.calendar.CalendarEventQuery(eventFeedURL);
    if (typeof (queryParameters) == 'undefined') queryParameters = { "MaxResults": 1000 };
    for (var queryParameter in queryParameters) {
        var str = 'query.set' + queryParameter + '(queryParameters["' + queryParameter + '"])';
        eval(str);
    }
    return query;
}
function convertToGoogleDateTime(date) { return new google.gdata.DateTime(calcIST_(date)); }
function handleSuccess(root, queryParameters, otherParameters) {
    var eventEntries = root.feed.getEntries();
    var events = new Array() ;
    for (var i = 0; i < eventEntries.length; i++) {
	    var event = eventEntries[i];
	    var re1 = new RegExp(queryParameters.eventName, "i");
	    var re2 = new RegExp(queryParameters.cityName, "i");
	    if (addEvent(event,otherParameters)) events.push(event);
	}
	getDocumentElement(otherParameters.divId).innerHTML = displayEvents(events, otherParameters);
}
function addEvent(event, otherParameters) {
    var cityName = otherParameters.cityName;
    var eventName = otherParameters.eventName;
    var recurring = otherParameters.recurring;
    var retVal = true;
    if (stringExists(cityName)) {
        var re = new RegExp(cityName, "i");
        retVal = retVal && (event.getLocations()[0].getValueString().search(re) != -1);
        if (!retVal) return false;
    }
    if (stringExists(eventName)) {
        var re = new RegExp(eventName, "i");
        retVal = retVal && (event.getTitle().getText().search(re) != -1);
        if (!retVal) return false;
    }
    if (stringExists(recurring)) {
        retVal = retVal && event.getRecurrence();
    }
    return retVal;
}
function stringExists(str) {return (typeof(str)!="undefined"&&str);}
function handleError(error, otherParameters) { getDocumentElement(otherParameters.divId).innerHTML = '<h4>Error : ' + error + '</h4>'; }
function getDocumentElement(id) {
    if(stringExists(id)) {
        var element = document.getElementById(id);
        if(element) return element;
    }
    element = document.createElement("div"); 
    var divName = "dummy-div";
    element.setAttribute("id",divName);
    document.body.appendChild(element);
    return element;
}

var getEventPeriod = function (event) {
    if (event.getRecurrence() == null) return '';
    recurrenceString = trim(event.getRecurrence().getValue());
    var pos1 = recurrenceString.indexOf('FREQ=');
    if (pos1 == -1) return '';
    return recurrenceString.slice(pos1 + 5, recurrenceString.indexOf(';', pos1 + 5));
};

var days = { 'MO': 'Monday', 'TU': 'Tuesday', 'WE': 'Wednesday', 'TH': 'Thursday', 'FR': 'Friday', 'SA': 'Saturday', 'SU': 'Sunday' };
var getEventDay = function (event) {
    if (event.getRecurrence() == null) return '';
    recurrenceString = trim(event.getRecurrence().getValue());
    pos1 = recurrenceString.indexOf('BYDAY=');
    if (pos1 == -1) return '';
    var byDay = recurrenceString.slice(pos1 + 6, recurrenceString.indexOf(';', pos1 + 6));
    if (byDay.length == 2) {
        return days[byDay];
    } else {
        return days[byDay.slice(1, 3)];
    }
    return '';
};
var dayNumbers = { 'SU': 6, 'MO': 0, 'TU': 1, 'WE': 2, 'TH': 3, 'FR': 4, 'SA': 5 };
var getEventDayNumber = function (event) {
    if (event.getRecurrence() == null) return 7;
    recurrenceString = trim(event.getRecurrence().getValue());
    pos1 = recurrenceString.indexOf('BYDAY=');
    if (pos1 == -1) return '';
    var byDay = recurrenceString.slice(pos1 + 6, recurrenceString.indexOf(';', pos1 + 6));
    if (byDay.length == 2) {
        return dayNumbers[byDay];
    } else {
        return dayNumbers[byDay.slice(1, 3)];
    }
    return 7;
};

var prefix = { '1': '1st', '2': '2nd', '3': '3rd', '4': '4th', '5': '5th' };
var getEventWeek = function (event) {
    if (event.getRecurrence() == null) return '';
    recurrenceString = trim(event.getRecurrence().getValue());
    pos1 = recurrenceString.indexOf('BYDAY=');
    if (pos1 == -1) return '';
    var byDay = recurrenceString.slice(pos1 + 6, recurrenceString.indexOf(';', pos1 + 6));
    if (byDay.length == 2) return 'Every';
    else return prefix[byDay.charAt(0)];
    return '';
};
var getEventTime = function (event) {
    if (event.getRecurrence() == null) return formatTime(event.getTimes()[0].getStartTime());
    recurrenceString = trim(event.getRecurrence().getValue());
    var startSearchString = 'DTSTART;TZID=Asia/Calcutta:';
    pos1 = recurrenceString.indexOf(startSearchString);
    if (pos1 == -1) {
        startSearchString = 'DTSTART;VALUE=DATE:';
        return ((recurrenceString.indexOf(startSearchString) == -1) ? '' : 'All Day');
    } else pos1 += startSearchString.length;
    var temp = recurrenceString.slice(pos1, pos1 + 15);
    var temp1 = temp.slice(10, 11);
    var time = new Date();
    time.setFullYear(Number(recurrenceString.slice(pos1, pos1 + 4)), Number(recurrenceString.slice(pos1 + 4, pos1 + 6)), Number(recurrenceString.slice(pos1 + 6, pos1 + 8)));
    time.setHours(Number(recurrenceString.slice(pos1 + 9, pos1 + 11)));
    time.setMinutes(Number(recurrenceString.slice(pos1 + 11, pos1 + 13)));
    time.setSeconds(Number(recurrenceString.slice(pos1 + 13, pos1 + 15)));
    return time.toLocaleTimeString();

};
var formatTime = function (googleDate) {
    if (googleDate.isDateOnly()) return "All Day"
    return calcIST_(googleDate.getDate()).toLocaleTimeString();
}
function parseQueryParameters(href) {
    var queryString = {};
    href.replace(new RegExp("([^?=&]+)(=([^&]*))?", "g"), function ($0, $1, $2, $3) { queryString[$1] = $3; });
    return queryString;
}
function getCityName(href) {
    var re = new RegExp("city", "i");
    var queryString = parseQueryParameters(href);
    for (var parameter in queryString) if (parameter.search(re) != -1) return queryString[parameter];
    return "";
}
function capitalize(str) {
    return str.replace( /(^|\s)([a-z])/g , function(m,p1,p2){ return p1+p2.toUpperCase(); } );
};
