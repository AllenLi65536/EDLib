using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace EDLib
{
    /// <summary>
    /// Send e-mail via dzmail01.kgi.com
    /// </summary>
    /// <example>
    /// <code>
    /// MailService ms = new MailService();
    /// ms.SendMail("kgiBulletin@kgi.com", "內網公告", new string[] { "judy.lu@kgi.com" }, null, new string[] { "allen.li@kgi.com", "andrea.chang@kgi.com" }, "Hello", "ㄋ好", false, null);
    /// </code>
    /// </example>
    public class MailService
    {
        /// <summary>
        /// smtp server
        /// </summary>
        private readonly string smtpServer;

        /// <summary>
        /// smtp server port default 25
        /// </summary>
        private readonly int smtpPort;

        /// <summary>
        /// Mail Account
        /// </summary>
        private readonly string mailAccount;

        /// <summary>
        /// Mail Password
        /// </summary>
        private readonly string mailPwd;

        /// <summary>
        /// Initiate MailService 
        /// </summary>
        /// <param name="smtpServer">SMTP server, default: dzmail01.kgi.com</param>
        /// <param name="smtpPort">SMTP port, default: 25</param>
        /// <param name="mailAccount">mail account: not needed for KGI</param>
        /// <param name="mailPwd">mail password: not needed for KGI</param>
        public MailService(string smtpServer = "dzmail01.kgi.com", int smtpPort = 25, string mailAccount = null, string mailPwd = null) {
            this.smtpServer = smtpServer;
            this.smtpPort = smtpPort;
            this.mailAccount = mailAccount;
            this.mailPwd = mailPwd;
        }

        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="mailFrom">Sender's E-mail Address</param>
        /// <param name="senderName">Sender's name</param>
        /// <param name="mailTos">Receivers' E-mail Addresses</param>
        /// <param name="ccs">Carbon copy to E-mail Addresses</param>
        /// <param name="bccs">Blind carbon copy to E-mail Addresses</param>
        /// <param name="mailSub">Subject</param>
        /// <param name="mailBody">Body of the mail</param>
        /// <param name="isBodyHtml">Is body in html format</param>
        /// <param name="filePaths">Paths to files to be attached.</param>
        /// <param name="priority">Mail priority. High = 2, Low = 1, Normal = 0</param>
        /// <param name="deleteFileAttachment">Delete the attached files or not</param>
        /// <returns>Successful or not</returns>
        public bool SendMail(string mailFrom, string senderName, string[] mailTos, string[] ccs, string[] bccs, string mailSub, string mailBody, bool isBodyHtml, string[] filePaths, MailPriority priority = MailPriority.Normal, bool deleteFileAttachment = false) {

            try {

                //Initialize a new mail message object
                using (MailMessage mail = new MailMessage()) {
                    mail.Subject = mailSub;
                    mail.Body = mailBody;
                    mail.IsBodyHtml = isBodyHtml;
                    mail.Priority = priority;

                    //Sender's address and name
                    if (!string.IsNullOrEmpty(senderName))
                        mail.From = new MailAddress(mailFrom, senderName);
                    else
                        mail.From = new MailAddress(mailFrom);

                    //Receivers' addresses
                    if (mailTos != null)
                        foreach (string mailTo in mailTos)
                            if (!string.IsNullOrEmpty(mailTo.Trim()))
                                mail.To.Add(new MailAddress(mailTo.Trim()));

                    //CC
                    if (ccs != null)
                        foreach (string cc in ccs)
                            if (!string.IsNullOrEmpty(cc.Trim()))
                                mail.CC.Add(new MailAddress(cc.Trim()));

                    //BCC                
                    if (bccs != null)
                        foreach (string bcc in bccs)
                            if (!string.IsNullOrEmpty(bcc.Trim()))
                                mail.Bcc.Add(new MailAddress(bcc.Trim()));

                    //Attachments 
                    if (filePaths != null)
                        foreach (string filePath in filePaths)
                            if (!string.IsNullOrEmpty(filePath.Trim()))
                                mail.Attachments.Add(new Attachment(filePath.Trim()));


                    using (SmtpClient client = new SmtpClient(smtpServer, smtpPort)) {
                        if (!string.IsNullOrEmpty(mailAccount) && !string.IsNullOrEmpty(mailPwd)) //Send with ID and password
                            client.Credentials = new NetworkCredential(mailAccount, mailPwd);//ID & password
                        client.Send(mail);//Send mail
                    }

                    //Release read locks
                    if (mail.Attachments != null)
                        for (int i = 0; i < mail.Attachments.Count; i++)
                            mail.Attachments[i].Dispose();
                }

                //Delete attachments
                if (deleteFileAttachment && filePaths != null)
                    foreach (string filePath in filePaths)
                        if (!string.IsNullOrEmpty(filePath.Trim()))
                            File.Delete(filePath.Trim());

                return true;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

    }
}

