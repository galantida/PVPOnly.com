using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PVPOnlyLibrary;


namespace PVPOnlyWebServices
{
    public partial class events : System.Web.UI.Page
    {
        string sessionUserName = "anonymous";

        protected void Page_Load(object sender, EventArgs e)
        {

            /****** Session Support ******/
            clsAccount myAccount = (clsAccount)Session["myAccount"]; // load a session if there is one
            if (myAccount != null) sessionUserName = myAccount.eMail;
            else sendResponse("Error", "Logon expired."); 
            

            /****** web services ******/
            string command = Request.QueryString["command"];
            if (command == null) command = "";
            

            /****** Session Support ******/
            // record request
            clsEventLog.entry(EntryType.Request, command + " - (" + sessionUserName + "@" + Request.UserHostAddress + ")", Request.Url.ToString(), clsUtil.fileNameFromPath(Request.PhysicalPath));


            // process command
            switch (command.ToLower())
            {
                case "myevents":
                    {
                        // returns event information connected with a particular user
                        sendResponse("myEvents", "myEvents",clsAttendee.getByAccountID(myAccount.ID));
                        break;
                    }
                case "getevent":
                    {
                        // validate parameters
                        int eventID = getNumericParameter("eventID", true);

                        // return a particular event
                        sendResponse("getEvent", "getEvent", clsEvent.get(eventID));
                        break;
                    }
                case "geteventattendees":
                    {
                        // validate parameters
                        int eventID = getNumericParameter("eventID", true);

                        // return a particular event
                        sendResponse("getAttendees", "getAttendees", clsAttendee.getByEventID(eventID));
                        break;
                    }
                case "updateevent":
                    {
                         // validate parameters
                        int eventID = getNumericParameter("id");

                        clsAttendee attendee = new clsAttendee(eventID, myAccount.ID);

                        if ((!attendee.Coordinator) && (!myAccount.Admin)) sendResponse("Error", "You must be one of the coordinators for this meeting to run this command.");

                        // validate parameters
                        string name = getStringParameter("name");

                        clsEvent curEvent = new clsEvent(eventID);
                        curEvent.Name = name;

                        if (curEvent.save() > 0)
                        {
                            // save success
                            sendResponse("updateEvent", "Updated the event values of a particular event.", curEvent.JSON());
                        }
                        else
                        {
                            // save failure
                            sendResponse("ERROR", "No records affected.", curEvent.JSON());
                        }
                        break;
                    }
                case "setattendeersvp":
                    {
                        // verify access
                        siteAdmin(myAccount);

                        // validate parameters
                        int attendeeID = getNumericParameter("attendeeID", true);
                        string RSVP = getStringParameter("RSVP", true);

                        // return a particular event
                        sendResponse("setAttendeeRSVP", "Updated the RSVP value of a particular attendee.", clsAttendee.updateRSVP(attendeeID, clsAttendee.getRSVP(RSVP)));
                        break;
                    }
                case "setattendeeattendance":
                    {
                        // verify access
                        siteAdmin(myAccount);

                        // validate parameters
                        int attendeeID = getNumericParameter("attendeeID", true);
                        string attendance = getStringParameter("attendance", true);

                        // return a particular event
                        sendResponse("setAttendeeAttendance", "Updated the attendance value of a particular attendee.", clsAttendee.updateAttendance(attendeeID, clsAttendee.getAttendance(attendance)));
                        break;
                    }
                default:
                    {
                        // handling invalid commands
                        if (command == "") sendResponse("Error", "Command is a required parameter.");
                        else sendResponse("Error", "'" + command + "' is not a valid command.");
                        break;
                    }
            }
        }

        // standard place for all responses to be sent from
        private void sendResponse(string summary, string description, string obj = "null")
        {
            clsEventLog.entry(EntryType.Response, summary + " (" + sessionUserName + " @ " + Request.UserHostAddress + ")", description, "session.aspx");
            Response.Write("{");
            Response.Write("\"summary\":\"" + summary + "\"");
            Response.Write(",\"description\":\"" + description + "\"");
            Response.Write(",\"generated\":\"" + DateTime.Now + "\"");
            Response.Write(",\"obj\":" + obj);
            Response.Write("}");
            Response.End();
        }

        private string getStringParameter(string name, bool required = false)
        {
            string value = Request.QueryString[name];
            if ((required == true) && (value == null)) sendResponse("Error", "'" + name + "' parameter is required.");
            return value;
        }

        private int getNumericParameter(string name, bool required = false)
        {
            // validate parameters
            int value;
            string svalue = Request.QueryString[name];
            if (!int.TryParse(svalue, out value))
            {
                if (required == true) sendResponse("Error", "' A numeric '" + name + "' parameter is required.");
            }
            return value;
        }

        private bool siteAdmin(clsAccount account)
        {
            if (account.Admin != true) sendResponse("Error", "Administration access is required to run this command.");
            return true;
        }
    }
}