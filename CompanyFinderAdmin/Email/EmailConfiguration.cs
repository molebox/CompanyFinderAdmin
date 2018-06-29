using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.Email
{
    /// <summary>
    /// Email config for getting the smtp data
    /// </summary>
    public class EmailConfiguration : IEmailConfiguration
    {
        /// <summary>
        /// SmtpServer
        /// </summary>
        public string SmtpServer { get; set; }
        /// <summary>
        /// SmtpPort
        /// </summary>
        public int SmtpPort { get; set; }
        /// <summary>
        /// SmtpUsername
        /// </summary>
        public string SmtpUsername { get; set; }
        /// <summary>
        /// SmtpPassword
        /// </summary>
        public string SmtpPassword { get; set; }
        /// <summary>
        /// PopServer
        /// </summary>
        public string PopServer { get; set; }
        /// <summary>
        /// PopPort
        /// </summary>
        public int PopPort { get; set; }
        /// <summary>
        /// PopUsername
        /// </summary>
        public string PopUsername { get; set; }
        /// <summary>
        /// PopPassword
        /// </summary>
        public string PopPassword { get; set; }
    }
}
