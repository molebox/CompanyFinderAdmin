using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.Email
{
    /// <summary>
    /// Class to describe an email
    /// </summary>
    public class EmailMessage
    {
        /// <summary>
        /// Constructor to initialize the lists of email addresses
        /// </summary>
        public EmailMessage()
        {
            ToAddresses = new List<EmailAddress>();
            FromAddresses = new List<EmailAddress>();
        }

        /// <summary>
        /// List of to addresses
        /// </summary>
        public List<EmailAddress> ToAddresses { get; set; }
        /// <summary>
        /// List of from addresses
        /// </summary>
        public List<EmailAddress> FromAddresses { get; set; }
        /// <summary>
        /// Email subject
        /// </summary>
        public string Subject { get; set; }
        /// <summary>
        /// Email content
        /// </summary>
        public string Content { get; set; }
    }
}
