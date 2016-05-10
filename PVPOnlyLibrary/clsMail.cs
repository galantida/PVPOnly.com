using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// using System.Threading.Tasks; 4.5 only
using System.Net.Mail;

namespace PVPOnlyLibrary
{
    public class clseMail
    {
        public static void send(string from, string to, string subject, string message)
        {
            try
            {
                // get server settings
                string SMTPServer = Properties.Settings.Default.SMTPServer;
                string SMTPUserName = Properties.Settings.Default.SMTPUserName;
                string SMTPPassword = Properties.Settings.Default.SMTPPassword;

                // Create Mail Message
                System.Net.Mail.MailMessage Email = new System.Net.Mail.MailMessage(from, to, subject, message);
                Email.Priority = MailPriority.Normal;
                Email.IsBodyHtml = true;
                //TestEmail.Attachments.Add(new System.Net.Mail.Attachment(EmailAttachmentFileName));

                // Send Message
                System.Net.Mail.SmtpClient EmailServer = new System.Net.Mail.SmtpClient(SMTPServer);
                EmailServer.Credentials = new System.Net.NetworkCredential(SMTPUserName, SMTPPassword);
                EmailServer.Send(Email);
            }
            catch (Exception ex)
            {
                //EZ.LogSys.Entry(LogEntryType.SYSTEM_EXCEPTION, "Send", "clsMail", ex.Message);
            }
        }
    }
}
