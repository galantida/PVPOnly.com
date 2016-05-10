using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PVPOnlyLibrary;


namespace PVPOnlyWebServices
{
    public partial class messages : System.Web.UI.Page
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
                case "mymessages":
                    {
                        sendResponse("myMessages", "myMessages", clsMessage.getByToAccountID(myAccount.ID));
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
        private void sendResponse(string summary, string description, string obj = "null") {
            clsEventLog.entry(EntryType.Response, summary + " (" + sessionUserName + " @ " + Request.UserHostAddress + ")", description, "session.aspx");
            Response.Write("{\"summary\":\"" + summary + "\",\"description\":\"" + description + "\",\"obj\":" + obj + "}");
            Response.End();
        }
    }
}