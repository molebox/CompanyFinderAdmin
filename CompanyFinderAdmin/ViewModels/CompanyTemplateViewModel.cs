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
    /// Company template view model
    /// </summary>
    public class CompanyTemplateViewModel
    {
        /// <summary>
        /// Company Id
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// Company Name
        /// </summary>
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        /// <summary>
        /// Contact Person
        /// </summary>
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        /// <summary>
        /// Company Email
        /// </summary>
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Recuitment Contact Email")]
        public string Email { get; set; }
        /// <summary>
        /// Company website
        /// </summary>
        [Display(Name = "Company Website")]
        public string Website { get; set; }
        /// <summary>
        /// Company Recruitment website address
        /// </summary>
        [Display(Name = "Recuitment Website Address")]
        public string RecruitmentWebAddress { get; set; }
        /// <summary>
        /// Company Biography
        /// </summary>
        [Display(Name = "Company Biography")]
        public string Bio { get; set; }
        /// <summary>
        /// Company Phone Number
        /// </summary>
        //[Range(0, 9)]
        //[DataType(DataType.PhoneNumber)]
        //[RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        [Display(Name = "Company Phone Number")]
        public string Phone { get; set; }
        /// <summary>
        /// Any other information the company wantes to give
        /// </summary>
        [Display(Name = "Other Information")]
        public string OtherNotes { get; set; }
        /// <summary>
        /// Is template Submitted
        /// </summary>
        public bool IsSubmitted { get; set; }
        /// <summary>
        /// Is tmeplate submitted
        /// </summary>
        public bool Locked { get; set; }

        /// <summary>
        /// List of the associated skillsets
        /// </summary>
        public List<SkillSet> SkillSetsList { get; set; }
        /// <summary>
        /// List of the associated details
        /// </summary>
        public List<SkillDetail> SkillDetailsList { get; set; }
        /// <summary>
        /// List of all the focuses
        /// </summary>
        public List<Focus> FocusList { get; set; }
        /// <summary>
        /// Email object
        /// </summary>
        public EmailMessage EmailToSend { get; set; }
        /// <summary>
        /// Email address to send to
        /// </summary>
        public EmailAddress EmailAddressToSend { get; set; }
        /// <summary>
        /// Templates unique guid
        /// </summary>
        public Guid UniqueUrl { get; set; }
    }
}