using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.Email
{
    /// <summary>
    /// Interface for the smtp credentials
    /// </summary>
    public interface IEmailConfiguration
    {
        /// <summary>
        /// SmtpServer
        /// </summary>
        string SmtpServer { get; }
        /// <summary>
        /// SmtpPort
        /// </summary>
        int SmtpPort { get; }
        /// <summary>
        /// SmtpUsername
        /// </summary>
        string SmtpUsername { get; set; }
        /// <summary>
        /// SmtpPassword
        /// </summary>
        string SmtpPassword { get; set; }
        /// <summary>
        /// PopServer
        /// </summary>
        string PopServer { get; }
        /// <summary>
        /// PopPort
        /// </summary>
        int PopPort { get; }
        /// <summary>
        /// PopUsername
        /// </summary>
        string PopUsername { get; }
        /// <summary>
        /// PopPassword
        /// </summary>
        string PopPassword { get; }
    }
}
