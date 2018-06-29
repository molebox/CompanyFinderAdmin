using MailKit.Net.Pop3;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.Email
{
    /// <summary>
    /// Email service for sending emails
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly IEmailConfiguration _emailConfiguration;
        /// <summary>
        /// Constructor injecting the email configuration into the class
        /// </summary>
        /// <param name="emailConfiguration"></param>
        public EmailService(IEmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }
        /// <summary>
        /// List of recived emails
        /// </summary>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public List<EmailMessage> ReceiveEmail(int maxCount = 10)
        {
            using (var emailClient = new Pop3Client())
            {
                emailClient.Connect(_emailConfiguration.PopServer, _emailConfiguration.PopPort, true);

                emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                emailClient.Authenticate(_emailConfiguration.PopUsername, _emailConfiguration.PopPassword);

                List<EmailMessage> emails = new List<EmailMessage>();
                for (int i = 0; i < emailClient.Count && i < maxCount; i++)
                {
                    var message = emailClient.GetMessage(i);
                    var emailMessage = new EmailMessage
                    {
                        Content = !string.IsNullOrEmpty(message.HtmlBody) ? message.HtmlBody : message.TextBody,
                        Subject = message.Subject
                    };
                    emailMessage.ToAddresses.AddRange(message.To.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                    emailMessage.FromAddresses.AddRange(message.From.Select(x => (MailboxAddress)x).Select(x => new EmailAddress { Address = x.Address, Name = x.Name }));
                }

                return emails;
            }
        }
        /// <summary>
        /// Send an email
        /// </summary>
        /// <param name="emailMessage"></param>
        public async Task<bool> Send(EmailMessage emailMessage)
        {
            var message = new MimeMessage();
            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            //message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.From.Add( new MailboxAddress(_emailConfiguration.SmtpUsername));


            message.Subject = emailMessage.Subject;
            //We will say we are sending HTML. But there are options for plaintext etc. 
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.Content
            };

            //Be careful that the SmtpClient class is the one from Mailkit not the framework!
            using (var emailClient = new SmtpClient())
            {
                try
                {
                    //// TEMPORARY SOCKET FOR TESTING WITH 2 SECONDS TIMEOUT

                            //Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                            //IAsyncResult result = socket.BeginConnect(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, null, null);

                            //bool success = result.AsyncWaitHandle.WaitOne(2000, true);

                            //if (socket.Connected)
                            //{
                            //    socket.EndConnect(result);
                            //}
                            //else
                            //{
                            //    socket.Close();
                            //    return false;
                            //}

                    //// END OF TEMPORARY SOCKET

                    emailClient.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    emailClient.Timeout = 5000;
                    //The last parameter here is to use SSL (Which you should!)
                    await emailClient.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.SmtpPort, true);
                    

                 
                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate(_emailConfiguration.SmtpUsername, _emailConfiguration.SmtpPassword);

                    await emailClient.SendAsync(message);

                    emailClient.Disconnect(true);

                    return true;
                }
                catch (SocketException)
                {
                    emailClient.Disconnect(true);
                    return false;
              
                }

                
            }
                
                           
        }
    }
}
