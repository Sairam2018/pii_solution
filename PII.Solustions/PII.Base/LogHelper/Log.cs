using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace PII.Base.LogHelper
{
    public class Log
    {
        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        public void LogError(string message)
        {
            //Logger.Write(new LogEntry() { Message = message, TimeStamp = DateTime.Now });
        }
        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="StackTrace">The stack trace.</param>
        public void LogError(string message, string stackTrace)
        {
           // Logger.Write(new LogEntry() { Message = message + " " + stackTrace, TimeStamp = DateTime.Now });
        }

        public  string LogErrorEmail(string errmsg, string stackTrace)
        {
            string timestatus = "" + DateTime.Now.ToString() + " ";

            // Retrive the Name of HOST
            string hostName = Dns.GetHostName();

            // Get the IP
            string myIP = string.Empty;
            myIP = "  " + Dns.GetHostByName(hostName).AddressList[0].ToString() + " ";

            string success = "1";

            string FromAddress = string.Empty;
            string ToAddress = string.Empty;
            string Port = string.Empty;
            string Host = string.Empty;
            FromAddress = ConfigurationManager.AppSettings["FromAddress"].ToString();
            ToAddress = ConfigurationManager.AppSettings["ToAddress"].ToString();
            Port = ConfigurationManager.AppSettings["Port"].ToString();
            Host = ConfigurationManager.AppSettings["EmailHost"].ToString();
            try
            {
                // SmtpClient smtp;
                using (SmtpClient smtp = new SmtpClient())
                {
                    var message = new MailMessage();
                    message.From = new System.Net.Mail.MailAddress(FromAddress);
                    message.To.Add(ToAddress);
                    message.Subject = "Session Serives Errors";
                    message.Body = timestatus + myIP + errmsg + " " + stackTrace;
                    message.IsBodyHtml = true;
                    smtp.Port = Convert.ToInt32(Port);
                    smtp.Host = Host;
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                success = "2";
            }
            return success;
        }
    }
}
