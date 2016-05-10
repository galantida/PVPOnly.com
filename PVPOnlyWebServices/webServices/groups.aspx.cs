using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PVPOnlyLibrary;


namespace PVPOnlyWebServices
{
    public partial class groups : System.Web.UI.Page
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
            clsEventLog.entry(EntryType.Request, command + " - (" + sessionUserName + "@" + Request.UserHostAddress + ")", Request.Url.ToString(), clsUtil.fileNameFromPath(Request.PhysicalPath));


            // process command
            switch (command.ToLower())
            {
                case "mygroups":
                    {
                        // return the groups that the logged on user is a member of
                        List<clsGroup> groups = clsGroup.getByAccountID(myAccount.ID);
                        sendResponse(command, "List of groups you belong to.", clsGroup.JSON(groups));
                        break;
                    }
                case "getmembers":
                    {
                        // return all the members of a particular group
                        
                        // validate parameters
                        int groupID;
                        string groupIDs = Request.QueryString["groupID"];
                        if (!int.TryParse(groupIDs, out groupID))
                        {
                            sendResponse("Error", "A numeric 'groupID' parameter is required.");
                            break;
                        }

                        string JSON = clsGroup.getMembers(groupID);
                        sendResponse(command, "All members of group (" + groupID + ").", JSON);
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
    }
}