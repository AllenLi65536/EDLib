﻿using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace EDLib
{
    /// <summary>
    /// Send e-mail via dzmail01.kgi.com
    /// </summary>
    public static class SendMail
    {
        /// <summary>
        /// smtp server
        /// </summary>
        private static readonly string smtpServer = "dzmail01.kgi.com";

        /// <summary>
        /// smtp server port default 25
        /// </summary>
        private static readonly int smtpPort = 25;

        /// <summary>
        /// Mail Account
        /// </summary>
        private static readonly string mailAccount = null;

        /// <summary>
        /// Mail Password
        /// </summary>
        private static readonly string mailPwd = null;

        /// <summary>
        /// Send mail
        /// </summary>
        /// <param name="mailFrom">Sender's E-mail Address</param>
        /// <param name="senderName">Sender's name</param>
        /// <param name="mailTos">Receivers' E-mail Addresses</param>
        /// <param name="ccs">CC to E-mail Addresses</param>
        /// <param name="mailSub">Subject</param>
        /// <param name="mailBody">Body of the mail</param>
        /// <param name="isBodyHtml">Is body in html format</param>
        /// <param name="filePaths">Paths to files to be attached.</param>
        /// <param name="deleteFileAttachment">Delete the attached files or not</param>
        /// <returns>Successful or not</returns>
        static public bool MailSend(string mailFrom, string senderName, string[] mailTos, string[] ccs, string mailSub, string mailBody, bool isBodyHtml, string[] filePaths, bool deleteFileAttachment) {

            try {

                //if (string.IsNullOrEmpty(MailFrom)) //※有些公司的Mail Server會規定寄信人的Domain Name要是該Mail Server的Domain Name
                //    MailFrom = "allen.li@kgi.com";

                //A new mail message object
                MailMessage mms = new MailMessage();
                //Sender's address and name
                if (!string.IsNullOrEmpty(senderName))
                    mms.From = new MailAddress(mailFrom, senderName);
                else
                    mms.From = new MailAddress(mailFrom);
                //Mail subject
                mms.Subject = mailSub;
                //Mail body
                mms.Body = mailBody;
                //Mail body is html or not
                mms.IsBodyHtml = isBodyHtml;

                //Receivers' addresses
                if (mailTos != null)
                    for (int i = 0; i < mailTos.Length; i++)
                        if (!string.IsNullOrEmpty(mailTos[i].Trim()))
                            mms.To.Add(new MailAddress(mailTos[i].Trim()));

                //CC
                if (ccs != null)
                    for (int i = 0; i < ccs.Length; i++)
                        if (!string.IsNullOrEmpty(ccs[i].Trim()))
                            mms.CC.Add(new MailAddress(ccs[i].Trim()));

                //Attachments 
                if (filePaths != null) 
                    for (int i = 0; i < filePaths.Length; i++)
                        if (!string.IsNullOrEmpty(filePaths[i].Trim())) {
                            Attachment file = new Attachment(filePaths[i].Trim());
                            mms.Attachments.Add(file);
                        }

                using (SmtpClient client = new SmtpClient(smtpServer, smtpPort)) {
                    if (!string.IsNullOrEmpty(mailAccount) && !string.IsNullOrEmpty(mailPwd)) //Send with ID and password
                        client.Credentials = new NetworkCredential(mailAccount, mailPwd);//ID & password
                    client.Send(mms);//Send mail
                }

                //Release read locks
                if (mms.Attachments != null && mms.Attachments.Count > 0)
                    for (int i = 0; i < mms.Attachments.Count; i++) {
                        mms.Attachments[i].Dispose();
                        //mms.Attachments[i] = null;
                    }

                #region Delete attachments
                if (deleteFileAttachment && filePaths != null && filePaths.Length > 0)
                    foreach (string filePath in filePaths)
                        File.Delete(filePath.Trim());
                #endregion

                return true;
            } catch (Exception ex) {
                Console.WriteLine(ex);
                return false;
            }
        }

    }
}

