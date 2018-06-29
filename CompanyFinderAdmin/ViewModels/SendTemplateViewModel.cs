using CompanyFinderAdmin.Email;
using DatabaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.ViewModels
{
    /// <summary>
    /// Send template view model
    /// </summary>
    public class SendTemplateViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SendTemplateViewModel()
        {
            CompanyList = new List<TemporaryCompanyTemplate>();
        }
        /// <summary>
        /// Company Name
        /// </summary>
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        /// <summary>
        /// Company Email
        /// </summary>
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Company Email")]
        public string Email { get; set; }
        /// <summary>
        /// Email object
        /// </summary>
        public EmailMessage EmailMessage { get; set; }
        /// <summary>
        /// The from email address that will be shown to the receiver
        /// </summary>
        public string FromEmail { get; set; }
        /// <summary>
        /// The Email subject
        /// </summary>
        public string EmailSubject { get; set; }
        /// <summary>
        /// The Email subject
        /// </summary>
        public string EmailContent { get; set; }
        /// <summary>
        /// Email Address to send to
        /// </summary>
        public EmailAddress EmailAddressToSend { get; set; }
        /// <summary>
        /// Unique Url for template link
        /// </summary>
        public string UniqueUrl { get; set; }

        /// <summary>
        /// A list of the companies that the template has been sent to
        /// </summary>
        public List<TemporaryCompanyTemplate> CompanyList { get; set; }
        /// <summary>
        /// Paging info view model object
        /// </summary>
        public PagingInfoViewModel PagingInfo { get; set; }
        /// <summary>
        /// The companies unique guid
        /// </summary>
        public Guid CompanyGuid { get; set; }
        /// <summary>
        /// The template link that is emailed to the company
        /// </summary>
        public string Link { get; set; }
        /// <summary>
        /// Company recruitment web address
        /// </summary>
        [Display(Name = "Recruitment Address")]
        public string RecruitmentWebAddress { get; set; }
    }
}
