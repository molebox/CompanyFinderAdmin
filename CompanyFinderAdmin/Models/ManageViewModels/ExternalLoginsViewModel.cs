using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace CompanyFinderAdmin.Models.ManageViewModels
{
    /// <summary>
    /// External login view model
    /// </summary>
    public class ExternalLoginsViewModel
    {
        /// <summary>
        /// Current logins list
        /// </summary>
        public IList<UserLoginInfo> CurrentLogins { get; set; }
        /// <summary>
        /// Other logins
        /// </summary>
        public IList<AuthenticationScheme> OtherLogins { get; set; }
        /// <summary>
        /// Remove button
        /// </summary>
        public bool ShowRemoveButton { get; set; }
        /// <summary>
        /// Status message
        /// </summary>
        public string StatusMessage { get; set; }
    }
}
