using DatabaseLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.ViewModels
{
    /// <summary>
    /// View model for the admin skills and details view. Used when viewing all the skills and details in the db
    /// </summary>
    public class SkillsDetailsFocusListViewModel
    {
        /// <summary>
        /// IEnummeration of the skill sets
        /// </summary>
        public IEnumerable<SkillSet> SkillSets { get; set; }
        /// <summary>
        /// IEnummeration of the details
        /// </summary>
        public IEnumerable<SkillDetail> SkillDetails { get; set; }
        /// <summary>
        /// IEnummeration of the focuses
        /// </summary>
        public IEnumerable<Focus> Focuses { get; set; }
        /// <summary>
        /// Page info opbject
        /// </summary>
        public PagingInfoViewModel PagingInfo { get; set; }
    }
}
