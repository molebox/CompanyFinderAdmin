using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.Email
{
    /// <summary>
    /// Emial service interface
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Method to send an email
        /// </summary>
        /// <param name="emailMessage"></param>
        Task<bool> Send(EmailMessage emailMessage);
        /// <summary>
        /// List of recieved emails
        /// </summary>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        List<EmailMessage> ReceiveEmail(int maxCount = 10);
    }
}
