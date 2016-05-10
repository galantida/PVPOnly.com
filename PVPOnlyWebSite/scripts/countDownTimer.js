
var CDTimer;
var CDTimerDestination;
var CDTimerDiv
var CDTimerIncrement;
function initCDTimer(divName, destination, increment) {
    CDTimerDiv = document.getElementById(divName);
    CDTimerDestination = destination;
    CDTimerIncrement = increment;
    console.log("Timer Init");
}

function startCDTimer() {
    CDTimer = window.setInterval("updateCDTimerDisplay()", CDTimerIncrement);
    console.log("Timer Start");
}

function stopCDTimer() {
    window.clearInterval(CDTimer);
    console.log("Timer Stop");
}

function updateCDTimerDisplay() {
    console.log("Timer Update");

    // shortcuts
    var tenth = 100;
    var second = tenth * 10;
    var minute = second * 60;
    var hour = minute * 60;
    var day = hour * 24;

    // calculate time difference
    var dif = Math.abs(Date.now() - CDTimerDestination);

    // calucate digits
    var CDTDays = Math.floor(dif / day);
    var CDTHours = Math.floor((dif % day) / hour);
    var CDTMinutes = Math.floor((dif % hour) / minute);
    var CDTSeconds = Math.floor((dif % minute) / second);
    var CDTTenths = Math.floor((dif % second) / tenth);

    
    var fullResult = zeroPad(CDTDays, 2) + " " + zeroPad(CDTHours, 2) + "-" + zeroPad(CDTMinutes, 2) + "-" + zeroPad(CDTSeconds, 2) + "-" + CDTTenths;

    // standard display
    //CDTimerDiv.innerHTML = fullResult;

    // fancy image display
    imgDisplay(fullResult);
}

function imgDisplay(numberString) {
    var scale = .5;
    var width = 84; // digit width
    var height = 125; // digit height
    var sourceDigits = 12; // modify this if you add more digits

    width = width * scale;
    height = height * scale;
    CDTimerDiv.innerHTML = "";
    for (var digit=0; digit<numberString.length; digit++) {
        // add a digit element to the container
        CDTimerDiv.innerHTML += "<img id='digit" + digit + "' src='../images/misc/digits.jpg' style='position:absolute;width:" + (sourceDigits * width) + "px;height:" + height + "px'/>"
		
        // display appropriate digit 
        var digitElement = document.getElementById("digit" + digit);
        var c = numberString.substring(digit, digit + 1);
        var value = 0;
        if (isNumeric(c)) value = Number(c);
        else {
            // special characters
            if (c == "-") value = 11;
            else value = 10; // blank
        }
        digitElement.style.clip = "rect(0px, " + ((value+1) * width) + "px, " + height + "px, " + (value * width) + "px)";

        // position
        digitElement.style.left = ((digit * width) + (value * -width)) + "px";
        digitElement.style.top = "0px";
    }
}

// return a string with the number prepadded with 0 zeros
function zeroPad(numericValue, length) 
{
    var stringValue = "00000" + numericValue;
    stringValue = stringValue.substr(stringValue.length - length);
    return stringValue;
}

function isNumeric(n) {
    return !isNaN(parseFloat(n)) && isFinite(n);
}

