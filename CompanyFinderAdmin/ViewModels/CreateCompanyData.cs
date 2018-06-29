using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.ViewModels
{
    /// <summary>
    /// ViewModel for the created company info
    /// </summary>
    public class CreateCompanyData
    {
        /// <summary>
        /// The company id
        /// </summary>
        public int CompanyId { get; set; }
        /// <summary>
        /// The company name
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// The contact person
        /// </summary>
        public string ContactPerson { get; set; }
        /// <summary>
        /// The contact email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// The company biography
        /// </summary>
        public string Bio { get; set; }
        /// <summary>
        /// The company website address
        /// </summary>
        public string Website { get; set; }
        /// <summary>
        /// The contact phone number
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// Company recruitment web address
        /// </summary>
        public string RecruitmentWebAddress { get; set; }
        /// <summary>
        /// List of the checked nodes ids
        /// </summary>
        public List<int> CheckedRolesNodes { get; set; }
        /// <summary>
        /// List of the checked nodes ids
        /// </summary>
        public List<int> CheckedFocusNodes { get; set; }
    }
}
