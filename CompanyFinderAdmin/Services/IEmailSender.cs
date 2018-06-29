using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.Services
{
    /// <summary>
    /// Boilerplate email interface
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Default emailing
        /// </summary>
        /// <param name="email"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        Task SendEmailAsync(string email, string subject, string message);
    }
}
