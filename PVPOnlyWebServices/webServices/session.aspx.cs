using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PVPOnlyLibrary;


namespace PVPOnlyWebServices
{
    public partial class session : System.Web.UI.Page
    {
        string sessionUserName = "anonymous";

        protected void Page_Load(object sender, EventArgs e)
        {
            /****** IP address spam protection ******/
            clsAuthentication auth = new clsAuthentication(Request.UserHostAddress); // load new or existing auth
            auth.addTime(15); // increase IP address login timer. 
            if (auth.isBanned(120)) // check if beyond threshhold
            {
                string description = "Your IP address '" + Request.UserHostAddress + "' has been temporarily banned for too many anonymous requests.";
                clsEventLog.entry(EntryType.Protected, "IP Address Banned", description, "session.aspx");
                sendResponse("Error", description);
                Response.End(); // sendResponse should end but just in case
            }


            /****** Session Support ******/
            clsAccount myAccount = (clsAccount)Session["myAccount"]; // load a session if there is one
            if (myAccount != null) sessionUserName = myAccount.eMail;

            /****** web services ******/
            string command = Request.QueryString["command"];
            if (command == null) command = "";
            
            /****** Session Support ******/
            // record request
            clsEventLog.entry(EntryType.Request, command + " - (" + sessionUserName + "@" + Request.UserHostAddress + ")", Request.Url.ToString(), clsUtil.fileNameFromPath(Request.PhysicalPath));


            // process command
            switch (command.ToLower())
            {
                case "login":
                    {
                        // validate parameters
                        string userName = Request.QueryString["username"];
                        if (userName == null)
                        {
                            sendResponse("Error", "'userName' is a required parameter for login.");
                            break;
                        }

                        string password = Request.QueryString["password"];
                        if (password == null)
                        {
                            sendResponse("Error", "'password' is a required parameter for login.");
                            break;
                        }

                        /****** account spam protection ******/
                        auth = new clsAuthentication(userName); // load new or existing auth
                        auth.addTime(15); // increase ban timer. 
                        if (auth.isBanned(120)) // check if banned beyond threshhold
                        {
                            string description = "Account '" + userName + "' is temporarily banned for too many logon attempts.";
                            clsEventLog.entry(EntryType.Protected, "Account Temporarily Banned", description, "session.aspx");
                            sendResponse("Error", description);
                            Response.End(); // sendResponse should end but just in case
                        }

                        // get account by username (email address)
                        myAccount = clsAccount.getByUserName(userName);

                        if (myAccount != null)
                        {
                            if (myAccount.Password == password)
                            {
                                // credentials were good so reset ban
                                auth.reset(Request.UserHostAddress);
                                auth.reset(userName);
                                

                                // check account status
                                if (myAccount.Confirmed == false) {
                                    // unconfirmed account
                                    Session["unconfirmedAccount"] = myAccount; // save the account so if they do confirm we don't have to make them login again
                                    sendResponse("Unconfirmed Account", "This account has not yet been confirmed.");
                                }
                                else 
                                {
                                    // logon successful
                                    Session["myAccount"] = myAccount; // save the account so we know who is using this session
                                    sendResponse("Logon Successful", "Successfull logon for account '" + myAccount.eMail + "'.", mySessionJSON(myAccount));
                                }
                            }
                            else
                            {
                                // invalid password
                                sendResponse("Error", "Invalid Logon.");
                                clsEventLog.entry(EntryType.Warning, "Invalid Logon Attempt", "Login attempt with incorrect password for userName '" + userName + "' and password '" + password + "' from IPAddress '" + Request.UserHostAddress + "'.", "session.aspx");
                            }
                        }
                        else
                        {
                            // invalid account
                            sendResponse("Error", "Invalid Logon.");
                            clsEventLog.entry(EntryType.Warning, "Invalid Logon Attempt", "Logon attempt for non-existant account with userName '" + userName + "' and password '" + password + "' from IPAddress '" + Request.UserHostAddress + "'.", "session.aspx");
                        }
                        break;
                    }
                case "forgotpassword":
                    {
                        // validate parameters
                        string emailAddress = Request.QueryString["emailaddress"];
                        if (emailAddress == null)
                        {
                            sendResponse("Error", "'userName' is a required parameter to retrieve a forgotten password.");
                            break;
                        }

                        clsAccount account = clsAccount.getByUserName(emailAddress);

                        if (account != null)
                        {
                            // send email address
                            clseMail.send("mailsystem@pvponly.com", account.eMail, "Forgot Password", account.Password);
                            clsEventLog.entry(EntryType.SendEmail, "Forgot password email sent to '" + account.eMail + "'.", "Forgot password email requested by (" + sessionUserName + "@" + Request.UserHostAddress + ").", "session.aspx");
                        }
                        else
                        {
                            clsEventLog.entry(EntryType.Warning, "Invalid Forgot Password request ignored for '" + emailAddress + "'", "Invalid forgot password request ignored for email account '" + emailAddress + "' from (" + sessionUserName + "@" + Request.UserHostAddress + ").", "session.aspx");
                        }
                        sendResponse("Forgot Password Email Request", "If the account '" + emailAddress + "' exists in our system an email containing the password will be sent to that address.");
                        break;
                    }
                case "registration":
                    {
                        // validate parameters
                        string emailAddress = Request.QueryString["emailaddress"];
                        if (emailAddress == null)
                        {
                            sendResponse("Error", "'userName' is a required parameter for registration.");
                            break;
                        }

                        string password = Request.QueryString["password"];
                        if (password == null)
                        {
                            sendResponse("Error", "'password' is a required parameter for registration.");
                            break;
                        }

                        string callSign = Request.QueryString["callSign"];
                        if (callSign == null)
                        {
                            sendResponse("Error", "'callsign' is a required parameter for registration.");
                            break;
                        }

                        clsAccount account = clsAccount.getByUserName(emailAddress);

                        if (account != null)
                        {
                            // account already exists
                            sendResponse("Error", "An account already exists for email address '" + emailAddress + "'.");
                        }
                        else
                        {
                            // create account
                            account = new clsAccount();
                            account.eMail = emailAddress;
                            account.Password = password;
                            account.CallSign = callSign;
                            account.Confirmed = false;
                            account.Confirmation = new Random().Next(10000, 99999).ToString(); // add a confirmation number
                            account.save();

                            // send registration email
                            clseMail.send("mailsystem@pvponly.com", account.eMail, "Address Confirmation", account.Confirmation);
                            clsEventLog.entry(EntryType.SendEmail, "Address Confirmation email sent to '" + account.eMail + "'.", "Registration of a new account by (" + sessionUserName + "@" + Request.UserHostAddress + ").", "session.aspx");

                            // send response to user
                            sendResponse("New User Registation.", "New user account was created for '" + callSign + "' using the unconfirmed email address '" + account.eMail + "'.");
                        }
                        break;
                    }
                case "resendaddressconfirmation":
                    {
                        // validate parameters
                        clsAccount account = (clsAccount)Session["unconfirmedAccount"];
                        

                        // send email address
                        clseMail.send("mailsystem@pvponly.com", account.eMail, "Address Confirmation", account.Confirmation);
                        clsEventLog.entry(EntryType.SendEmail, "Address Confirmation email sent to '" + account.eMail + "'.", "Address Confirmation email requested by (" + sessionUserName + "@" + Request.UserHostAddress + ").", "session.aspx");


                        // either way send the same response. Don't want to give away what addresses are in our system
                        sendResponse("Address Confirmation Email Request", "If the account '" + account.eMail + "' exists in our system a Address Confirmation email containing your confirmation code will be sent to that address.");
                        break;
                    }
                case "confirmation":
                    {
                        // This uses a confirmation code to locate and confirm an account
                        int iCode;
                        if (Int32.TryParse(Request.QueryString["code"].ToString(), out iCode))
                        {
                            clsAccount account = clsAccount.getByConfirmationCode(iCode);
                            if (account != null)
                            {
                                // confirm account
                                account.Confirmed = true;
                                account.save();
                                clsEventLog.entry(EntryType.Action, "Account email address confirmed '" + account.eMail + "'.", "Account email address confirmed '" + account.eMail + "' by (" + sessionUserName + "@" + Request.UserHostAddress + ").", "session.aspx");

                                // send response
                                if (Session["unconfirmedAccount"] != null)
                                {
                                    // if user previously logged in return account info
                                    Session["myAccount"] = Session["unconfirmedAccount"];
                                    sendResponse("Session Located", "The account '" + account.eMail + "' has been confirmed with code '" + iCode.ToString() + "'. Continuing logon...", mySessionJSON((clsAccount)Session["myAccount"]));
                                }
                                else
                                {
                                    // user confirmed but was not logged in 
                                    sendResponse("Account Confirmed", "The account '" + account.eMail + "' has been confirmed with code '" + iCode.ToString() + "'.");
                                }
                            }
                            else sendResponse("Error", "Invalid confirmation code.");  
                        }
                        else sendResponse("Error", "A numeric confirmation code is a required parameter."); 
                        break;
                    }
                case "mysession":
                    {
                        if (myAccount != null) sendResponse("Session Located", "Session retreived.", mySessionJSON(myAccount));
                        else sendResponse("Error", "Unable to locate session.");
                        break;
                    }
                case "logoff":
                    {
                        Session["myAccount"] = null;
                        sendResponse("Logoff successful", "Logoff successful.");
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
            Response.Write("{");
            Response.Write("\"summary\":\"" + summary + "\"");
            Response.Write(",\"description\":\"" + description + "\"");
            Response.Write(",\"generated\":\"" + DateTime.Now + "\"");
            Response.Write(",\"obj\":" + obj);
            Response.Write("}");
            Response.End();
        }


        // session object creator - right now it is just the account object but could expand
        private string mySessionJSON(clsAccount account)
        {
            // create a profile object
            string json = "{";

            // session information
            json += "\"session\":";
            json += "{";
            json += "\"IPAddress\":\"" + Request.UserHostAddress + "\"";
            json += "},";

            // add account information to profile
            json += "\"account\":";
            json += account.JSON();

            // close the profile object
            json += "}";

            return json;
        }
    }
}