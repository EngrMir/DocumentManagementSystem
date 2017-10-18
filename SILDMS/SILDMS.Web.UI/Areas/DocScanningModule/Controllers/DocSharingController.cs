using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using SILDMS.Utillity;

namespace SILDMS.Web.UI.Areas.DocScanningModule.Controllers
{
    public class DocSharingController : Controller
    {
        //
        // GET: /DocScanningModule/DocShare/
        public ActionResult SendMail(String toEmail, string ccAddress, string bccAddress, string Subj, string Message)
        {
            // HttpPostedFile httpPostedFileBase2 = System.Web.HttpContext.Current.Request.Files[0];

            //Reading sender Email credential from web.config file
            HostAdd = ConfigurationManager.AppSettings["Host"].ToString();
            FromEmailid = ConfigurationManager.AppSettings["FromMail"].ToString();
            Password = ConfigurationManager.AppSettings["Password"].ToString();
            // ToEmail = "shalim@squaregroup.com";
            //creating the object of MailMessage
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress(FromEmailid); //From Email Id
            mailMessage.Subject = Subj; //Subject of Email
            mailMessage.Body = Message; //body or message of Email
            mailMessage.IsBodyHtml = true;
            string[] toMuliId = toEmail.Split(',');
            foreach (string toEMailId in toMuliId)
            {
                if (!string.IsNullOrEmpty(toEMailId))
                {
                    mailMessage.To.Add(new MailAddress(toEMailId)); //adding multiple TO Email Id
                }
            }

            // ccAddress = "jain@squaregroup.com";
            string[] CCId = ccAddress.Split(',');
            foreach (string ccEmail in CCId)
            {
                if (!string.IsNullOrEmpty(ccEmail))
                {
                    mailMessage.CC.Add(new MailAddress(ccEmail)); //Adding Multiple CC email Id
                }
            }

            // bccAddress = "shalim@squaregroup.com";
            string[] bccid = bccAddress.Split(',');
            foreach (string bccEmailId in bccid)
            {
                if (!string.IsNullOrEmpty(bccEmailId))
                {
                    mailMessage.Bcc.Add(new MailAddress(bccEmailId)); //Adding Multiple BCC email Id
                }
            }


            if (TempData["Attachment"] != null)
            {
                HttpPostedFile httpPostedFileBase = (HttpPostedFile)TempData["Attachment"];
                var attachment = new Attachment(httpPostedFileBase.InputStream, httpPostedFileBase.FileName);
                mailMessage.Attachments.Add(attachment);
            }

            SmtpClient smtp = new SmtpClient(); // creating object of smptpclient
            smtp.Host = HostAdd; //host of emailaddress for example smtp.gmail.com etc
            //network and security related credentials
            smtp.EnableSsl = true;
            NetworkCredential networkCred = new NetworkCredential();
            networkCred.UserName = mailMessage.From.Address;
            networkCred.Password = Password;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = networkCred;
            smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);





            ServicePointManager.ServerCertificateValidationCallback = (s, certificate, chain, sslPolicyErrors) => true;

            try
            {
                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Response.Write(ex);
            }
            return null;

        }

        [HttpPost]
        [Authorize]
        public string UploadHandler()
        {
            HttpPostedFile httpPostedFileBase = System.Web.HttpContext.Current.Request.Files[0];
            if (httpPostedFileBase != null)
            {
                string[] file = httpPostedFileBase.FileName.Split('.');
                if (file.Length > 0)
                {
                    TempData["Attachment"] = httpPostedFileBase;
                    return httpPostedFileBase.FileName;
                }
            }


            else
            {
                return "1";
            }
            return "0";
        }


        public string HostAdd { get; set; }

        public string FromEmailid { get; set; }

        public string ToEmail { get; set; }

        public string Password { get; set; }
    }
}