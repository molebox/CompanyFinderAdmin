using DatabaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.ViewModels
{
    /// <summary>
    /// Company Editor view model for creating and editing companies in the db
    /// </summary>
    public class CompanyEditorViewModel
    {
        /// <summary>
        /// The company id
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// The company name
        /// </summary>
        [Required]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }
        /// <summary>
        /// The contact person
        /// </summary>
        [Required]
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        /// <summary>
        /// The contact email
        /// </summary>
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }
        /// <summary>
        /// The company biography
        /// </summary>
        [Required]
        [Display(Name = "Biography")]
        public string Bio { get; set; }
        /// <summary>
        /// The company website address
        /// </summary>
        [Required]
        [DataType(DataType.Url)]
        public string Website { get; set; }
        /// <summary>
        /// The contact phone number
        /// </summary>
        [Required]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid Phone number")]
        [Display(Name = "Phone Number")]
        public string Phone { get; set; }
        /// <summary>
        /// Company recruitment web address
        /// </summary>
        [Display(Name = "Recruitment Address")]
        public string RecruitmentWebAddress { get; set; }

        /// <summary>
        /// Company object to hold the actual company
        /// </summary>
        public Companies Company { get; set; }
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

    }
}
