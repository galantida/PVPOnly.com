
/****************************************************
                    General Functions
*****************************************************/

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
    var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
        results = regex.exec(location.search);
    return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
}

function toJSDateTime(jSONDateTime) {
    return new Date(jSONDateTime.year + "/" + jSONDateTime.month + "/" + jSONDateTime.day + " " + jSONDateTime.hour + ":" + jSONDateTime.minute + ":" + jSONDateTime.second);
}

function displayDateTime(DT) {
    //DT = new Date("3/19/1969 22:45")
    // Monday March 31st, 2014 at 10:45 PM
    var result = "";
    result += getWeekDay(DT.getDay());
    result += " " + getMonth(DT.getMonth());
    result += " " + DT.getDate() + ",";
    result += " " + DT.getFullYear() + " at ";
    if (DT.getHours() < 12) result += DT.getHours() + ":" + displayPadding(DT.getMinutes()) + " AM";
    else result += (DT.getHours() - 12) + ":" + displayPadding(DT.getMinutes()) + " PM";
    return result;
}

function getWeekDay(number) {
    var WeekDays = new Array("Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday");
    return WeekDays[number];
}

function getMonth(number) {
    var Months = new Array("January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December");
    return Months[number];
}



/****************************************************
                Display functions
*****************************************************/
function displayPadding(number) {
    if (number < 10) return "0" + number;
    else return number;
}

function displayFriendly(text) {
    return text.replace("'", "&#39;");
}

function HTMLEncode(value) {
    //create a in-memory div, set it's inner text(which jQuery automatically encodes)
    //then grab the encoded contents back out.  The div never exists on the page.
    return $('<div/>').text(value).html();
}

function HTMLDecode(value) {
    return $('<div/>').html(value).text();
}


/****************************************************
                Local Storage Functions
*****************************************************/

function setStringLS(name, value) {
    localStorage.setItem(name, value);
}

function getStringLS(name) {
    var value = null;
    value = localStorage.getItem(name);
    if (value == "null") value = null;
    return value;
}

function setDateLS(name, value) {
    setStringLS(name, value);
}

function getDateLS(name) {
    return new Date(getStringLS(name));
}

function setObjectLS(name, obj) {
    setStringLS(name, JSON.stringify(obj));
}

function getObjectLS(name) {
    var objString = getStringLS(name);
    var obj = null;
    try {
        obj = JSON.parse(objString);
    }
    catch (err) {
        console.log("ERROR: Failed to parse objectString '" + objString + "'");
    }
    return obj;
}

function checkLS(name) {
    if ((getStringLS(name) == "") || (getStringLS(name) == null)) return false;
    else return true;
}
