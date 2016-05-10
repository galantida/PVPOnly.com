
// *************************************************************************************************
// The purpose of this file is to facilitate properly formatted communications with the server.
// Goals one place for server names and paths
// simplify and standardize communications with web services
// *************************************************************************************************

// report version
console.log("=== included wsInterface.js version 1.5 ===");

// decide which service to use base on the domain in the URL
var wsURL = "";
switch (window.location.host) {
    case "localhost:52688":
        {
            wsURL = "http://localhost:52690/webServices/";
            break;
        }
    case "davidgalanti.com":
        {
            wsURL = "http://davidgalanti.com/pvponlybeta/webServices/";
            break;
        }
    default:
        {
            // temporarily disabled
            alert("Error: Unrecognised deployment location '" + window.location.host + "'");
            break;
        }
}
console.log("Domain detected as '" + window.location.host + "' which retrieves its data from '" + wsURL + "'");

// ***********************************************************************************************************************
// requestJSONInfo - multithreaded handler for requesting information in the form of JSON objects
// usage - Pass a one word command and a javascript object with need parameters as properties
// 
function requestJSONInfo(serviceName, commandName, params, callBack) {

    // build parameter string
    var paramString = serviceName + ".aspx?command=" + commandName;

    // parameters
    for (var key in params) { // loop through properties
        if (params.hasOwnProperty(key)) {
            paramString += "&" + key + "=" + params[key];
        }
    }

    // add no cache to the end
    paramString += "&nocache=" + Math.random();

    // display param string
    console.log("JSON Request: (" + serviceName + " " + commandName + " - " + wsURL + paramString); // display the request in the console

    // prepare service request
    var xhr = new XMLHttpRequest(); // communication object    
    xhr.withCredentials = false;
    // you must set access control and allow credentials on web service server HTTP response headers as well.
    // Access-Control-Allow-Credentials = true
    // Access-Control-Allow-Headers = Content-Type
    // Access-Control-Allow-Methods = GET,POST,OPTIONS
    // Access-Control-Allow-Origin = http://tcgportal1:1212

    xhr.open("POST", wsURL + paramString, true); // create a post in the com object
    xhr.timeout = 30000; // 30000 = 30 seconds
    xhr.setRequestHeader("Content-type", "application/x-www-form-urlencoded"); // set the header
    xhr.onreadystatechange = function () { receiveJSONInfo(serviceName, commandName, xhr, callBack) };
    try {
        xhr.send(encodeURI(paramString)); // send
    } catch (err) {
        // alert error;
    }
}

// ***********************************************************************************************************************
// receiveJSONInfo - multi-threaded handler for JSON object responses received from the server
// usage - this function is only called from "requestJSONInfo". just make sure to add a case statement to redirect
// responses to their appropriate pages
// 
function receiveJSONInfo(serviceName, commandName, xhr, callBack) {
    if (xhr.readyState == 4) {
        // report connection failure
        if (xhr.status != 200) {
            console.log("ERROR: failed to execute (" + serviceName + " - " + commandName + ") on server (" + wsURL + ") returned status (" + xhr.status + ").");
            // alert error could go here
            alert("Can not contact server.");
        }
        else {
            try {
                var JSONObject = JSON.parse(xhr.responseText);
            }
            catch (err) {
                // report invalid JSON response error
                console.log("ERROR: JSON parse error '" + err.message + "'");
                console.log("response='" + xhr.responseText + "'");
                alert("Invalid response from the server.");
            }

            // stamp receved
            JSONObject.received = Date.now();

            // log the response from the server
            console.log("JSON Response: (" + serviceName + " " + commandName + ") - " + JSON.stringify(JSONObject) + "\n\n"); // display the response in the console

            if (JSONObject.summary == "Error") {
                switch (JSONObject.description)
                {
                    case "Logon expired.":
                        {
                            loadContent("authentication");
                        }
                    default:
                        {
                            console.log("Unhandled Error");
                        }
                }
            }

            // execute callback
            if (typeof callBack === 'function') callBack(JSONObject);
            else console.log("ERROR: a Function with the name  '" + callBack + "(xhr)' does not exists to handle the server response.\n\n");
        }
    } else {
        // commented out since this is normal
        //console.log("ERROR: Server responded but is in a state other then ready. xhr.readyState='" + xhr.readyState + "'\n\n");
    }
}
