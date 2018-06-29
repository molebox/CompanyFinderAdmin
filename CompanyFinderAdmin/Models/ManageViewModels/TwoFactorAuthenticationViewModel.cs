using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.Models.ManageViewModels
{
    /// <summary>
    /// Boilerplate two factor view model
    /// </summary>
    public class TwoFactorAuthenticationViewModel
    {
        /// <summary>
        /// Has auth
        /// </summary>
        public bool HasAuthenticator { get; set; }
        /// <summary>
        /// Recovery left
        /// </summary>
        public int RecoveryCodesLeft { get; set; }
        /// <summary>
        /// is enabled
        /// </summary>
        public bool Is2faEnabled { get; set; }
    }
}
