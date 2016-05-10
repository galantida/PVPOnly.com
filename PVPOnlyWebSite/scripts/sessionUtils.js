// validate session
var mySession;

function checkSession() {

    mySession = getObjectLS("mySession");

    if (mySession == null) {
        // request remote session info
        console.log("No local session, requesting remote session...");
        requestJSONInfo("session", "mySession", {}, sessionResponse);
    }
    else {
        console.log("Local session still active, logged on as '" + mySession.account.eMail + "'");
        //console.log("confirming remote session...");
        //requestJSONInfo("session", "mySession", {}, sessionResponse);
    }
}


function sessionResponse(response) {
    if (response.summary != "Error") {
        // remote session retreived
        mySession = response.obj; // save in the ram
        setObjectLS("mySession", mySession); // save it to local disk (cookie)
        console.log("Remote session retreived as '" + mySession.account.eMail + "'. saving locally...");
        loadContent("tabBrowser");
    }
    else {
        // no remote session
        console.log("No remote session either, sending user to authentication page...");
        loadContent("authentication");
    }
}

function logonRequest(userName, password) {
    // send request
    var params = { "userName": userName, "password": password };
    requestJSONInfo("session", "login", params, logonResponse);
}

function logonResponse(obj) {
    if (obj.summary != "Error") {
        if (obj.summary == "Unconfirmed Account") {
            loadContent("confirmation"); // unconfirmed
        }
        else {
            console.log("Logon successful.");
            sessionResponse(obj)
            loadContent("tabBrowser"); // start page
        }
    }
    else {
        console.log("logon failed.");
        displayMessage(obj.description, "red");
    }
}

function logoffRequest() {
    mySession = null;
    setObjectLS("mySession", null);
    requestJSONInfo("session", "logoff", {}, logoffResponse);
}

function logoffResponse(obj) {
    if (obj.summary != "Error") {
        console.log("Log off successful.");
        loadContent("authentication");
        //window.dispatchEvent(new Event('resize'));
    }
    else {
        alert("Log off failed." + obj.description)
    }
}

checkSession();


