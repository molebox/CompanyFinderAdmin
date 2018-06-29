using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.Models.AccountViewModels
{
    /// <summary>
    /// Boilerplate login view model two factor
    /// </summary>
    public class LoginWith2faViewModel
    {
        /// <summary>
        /// Two factor code
        /// </summary>
        [Required]
        [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Text)]
        [Display(Name = "Authenticator code")]
        public string TwoFactorCode { get; set; }
        /// <summary>
        /// Remember machine
        /// </summary>
        [Display(Name = "Remember this machine")]
        public bool RememberMachine { get; set; }
        /// <summary>
        /// Remember me
        /// </summary>
        public bool RememberMe { get; set; }
    }
}
