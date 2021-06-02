/* 
 ***** Author       : Jijeesh & Ajay    *****
 ***** Start Date   : 11-Sep-2013       *****
 ***** Organization : Vijaya Bank       *****
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Xml;
using System.Data.OleDb;
using System.Data;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;

namespace Alert
{
    class clsAlert
    {

        public static string SendSms(string MobileNo, string Message)
        {
            string srequest = "";
            string sreader = "";
            try
            {
                //Reading the http stream and for the return code
                srequest = @"http://172.16.241.41:88/SendSMS.aspx?userid=smssiuser1&password=smssi1@4321&mobileno=" + MobileNo + "&sendername=VIJBNK&category=1&sendernumber=1103&smsc=SISMS&message=" + Message;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(srequest);
                //IWebProxy prox = new WebProxy("vb22k", 8080);
                //prox.Credentials = new NetworkCredential("ajay", "123");
                // Set  limits on resources used by this request
                request.MaximumAutomaticRedirections = 4;
                request.MaximumResponseHeadersLength = 4;
                // Set credentials to use for this request.
                // request.Credentials = CredentialCache.DefaultCredentials;
                // request.Proxy = prox;
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                //Console.WriteLine("Content length is {0}", response.ContentLength);
                //Console.WriteLine("Content type is {0}", response.ContentType);
                // Get the stream associated with the response.
                Stream receiveStream = response.GetResponseStream();

                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);

                //Console.WriteLine("Response stream received.");
                //Console.WriteLine(readStream.ReadToEnd());

                sreader = readStream.ReadToEnd();
                response.Close();
                readStream.Close();
                //Console.WriteLine("SMS sent successfully");
                //Console.Read();
            }
            catch (Exception ex)
            {
                sreader = ex.Message;
                clsEvent.LogExceptionEvent(sreader);
                if (ex.Message == "Unable to connect to the remote server")
                {
                    sreader = "SMSSVRDN";
                }
            }
            return sreader;

        }

        public static void sendCautionEmail(string sFileName)
        {
            string sDtNow = DateTime.Now.ToString();
            string frmMail = "alerts@vijayabank.co.in";
            string sToEmail = "cbsmem18@vijayabank.co.in";
            string sCcEmail1 = "cbsprog6@vijayabank.co.in";
            //string ccEmail2 = "dgmplanning@vijayabank.co.in";

            //string ccEmail = "mbd.pension@vijayabank.co.in";
            string sSubject = "!!! CAUTION EMAIL ALERT FOR CPSMS Project !!!";
            //email Body
            string sEmailBody = "<HTML>";
            sEmailBody += "<body bgcolor='#E0FFFF'></p>";
            sEmailBody = "<p align='left'><font face='calibri' color='#0000FF' size='3'>Dear Sir, </p>";
            sEmailBody += "<p></p><p align='left'><font color='red'>CAUTION !!! </font> CPSMS XML file named as: <b> '" + sFileName + "'.xml </b> on <b>'" + sDtNow + "' not able to process </b></p>";

            sEmailBody += "<p align='left'>Kindly check application immediatly and resolve.</p>";
            sEmailBody += "<br><br><p align='left'>Thanks & Regards,</p></br></br>";
            sEmailBody += "<br>Vijaya Bank-CPSMS Application,</br>";
            sEmailBody += "<br>Dept. of Information Technology,</br>";
            sEmailBody += "<br>Head Office, Bangalore.</br>";
            sEmailBody += "<p><br><br>This is a system generated email.</br></br></p></font>";

            string body = sEmailBody;
            sToEmail = sToEmail.Trim();

            try
            {

                SmtpClient sMail = new SmtpClient("webmail.vijayabank.co.in");//exchange or smtp server goes here.
                sMail.DeliveryMethod = SmtpDeliveryMethod.Network;
                sMail.Credentials = new NetworkCredential("alerts", "login@123");

                MailMessage Mail = new MailMessage();
                MailAddress from = new MailAddress(frmMail);
                Mail.From = from;
                Mail.To.Add(sToEmail);
                Mail.CC.Add(sCcEmail1);
                //Mail.CC.Add(ccEmail2);
                Mail.Subject = sSubject;
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, "text/html");
                Mail.AlternateViews.Add(htmlView);

                //Mail.Body = body;

                sMail.Send(Mail); //mail sent

                //sendMailLbl.Text = "Request Send Successfully";
                Console.WriteLine("Email sent successfully");

            }
            catch (FormatException ex)
            {
                clsEvent.LogExceptionEvent("Mail Not Send. Invalid EmailID! " + ex.Message);
            }
            catch (SmtpFailedRecipientsException ex)
            {
                clsEvent.LogExceptionEvent("Mail Not Send. " + ex.Message + " - " + ex.FailedRecipient.ToString());
            }
        }

    }
}
