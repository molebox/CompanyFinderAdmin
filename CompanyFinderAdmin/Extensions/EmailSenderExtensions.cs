using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CompanyFinderAdmin.Services;

namespace CompanyFinderAdmin.Services
{
    /// <summary>
    /// Extention email sender
    /// </summary>
    public static class EmailSenderExtensions
    {
        /// <summary>
        /// Send email confirm boilerplate
        /// </summary>
        /// <param name="emailSender"></param>
        /// <param name="email"></param>
        /// <param name="link"></param>
        /// <returns></returns>
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirm your email",
                $"Please confirm your account by clicking this link: <a href='{HtmlEncoder.Default.Encode(link)}'>link</a>");
        }
    }
}
