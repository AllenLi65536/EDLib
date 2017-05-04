using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace EDLib
{
    public class SendMail
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
        /// 完整的寄信功能
        /// </summary>
        /// <param name="mailFrom">寄信人E-mail Address</param>
        /// <param name="senderName">寄信人名稱</param>
        /// <param name="mailTos">收信人E-mail Address</param>
        /// <param name="ccs">副本E-mail Address</param>
        /// <param name="mailSub">主旨</param>
        /// <param name="mailBody">信件內容</param>
        /// <param name="isBodyHtml">是否採用HTML格式</param>
        /// <param name="filePaths">附檔在WebServer檔案總管路徑</param>
        /// <param name="deleteFileAttachment">是否刪除在WebServer上的附件</param>
        /// <returns>是否成功</returns>
        static public bool MailSend(string mailFrom, string senderName, string[] mailTos, string[] ccs, string mailSub, string mailBody, bool isBodyHtml, string[] filePaths, bool deleteFileAttachment) {

            try {

                //if (string.IsNullOrEmpty(MailFrom)) //※有些公司的Mail Server會規定寄信人的Domain Name要是該Mail Server的Domain Name
                //    MailFrom = "allen.li@kgi.com";

                //建立MailMessage物件
                MailMessage mms = new MailMessage();
                //指定一位寄信人MailAddress
                if (!string.IsNullOrEmpty(senderName))
                    mms.From = new MailAddress(mailFrom, senderName);
                else
                    mms.From = new MailAddress(mailFrom);
                //信件主旨
                mms.Subject = mailSub;
                //信件內容
                mms.Body = mailBody;
                //信件內容 是否採用Html格式
                mms.IsBodyHtml = isBodyHtml;

                //加入信件的收信人(們)address
                if (mailTos != null)
                    for (int i = 0; i < mailTos.Length; i++)
                        if (!string.IsNullOrEmpty(mailTos[i].Trim()))
                            mms.To.Add(new MailAddress(mailTos[i].Trim()));

                //加入信件的副本(們)address
                if (ccs != null)
                    for (int i = 0; i < ccs.Length; i++)
                        if (!string.IsNullOrEmpty(ccs[i].Trim()))
                            mms.CC.Add(new MailAddress(ccs[i].Trim()));

                //加入信件的夾帶檔案
                if (filePaths != null) //有夾帶檔案
                    for (int i = 0; i < filePaths.Length; i++)
                        if (!string.IsNullOrEmpty(filePaths[i].Trim())) {
                            Attachment file = new Attachment(filePaths[i].Trim());
                            mms.Attachments.Add(file);
                        }

                using (SmtpClient client = new SmtpClient(smtpServer, smtpPort)) {
                    if (!string.IsNullOrEmpty(mailAccount) && !string.IsNullOrEmpty(mailPwd)) //.config有帳密的話
                        client.Credentials = new NetworkCredential(mailAccount, mailPwd);//寄信帳密
                    client.Send(mms);//寄出一封信
                }

                //釋放每個附件，才不會Lock住
                if (mms.Attachments != null && mms.Attachments.Count > 0)
                    for (int i = 0; i < mms.Attachments.Count; i++) {
                        mms.Attachments[i].Dispose();
                        //mms.Attachments[i] = null;
                    }

                #region 要刪除附檔
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

