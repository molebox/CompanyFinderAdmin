using DatabaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.ViewModels
{
    /// <summary>
    /// View model to return a list of companies for the search match
    /// </summary>
    public class CompanyListViewModel
    {
        /// <summary>
        /// IEnummerable of companies
        /// </summary>
        public IEnumerable<Companies> Companies { get; set; }
        /// <summary>
        /// Paging info view model object
        /// </summary>
        public PagingInfoViewModel PagingInfo { get; set; }
        /// <summary>
        /// List of the searched skills
        /// </summary>
        public List<SkillSet> SelectedSkillsFromSearch { get; set; }
        /// <summary>
        /// List of the search details
        /// </summary>
        public List<SkillDetail> SelectedDetailsFromSearch { get; set; }
    
    }
}
