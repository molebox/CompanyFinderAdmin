using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CompanyFinderAdmin.Models.ManageViewModels
{
    /// <summary>
    /// Boilerplate enable auth
    /// </summary>
    public class EnableAuthenticatorViewModel
    {
        /// <summary>
        /// Code
        /// </summary>
            [Required]
            [StringLength(7, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Verification Code")]
            public string Code { get; set; }
        /// <summary>
        /// Sharedkey
        /// </summary>
            [BindNever]
            public string SharedKey { get; set; }
        /// <summary>
        /// Auth url
        /// </summary>
            [BindNever]
            public string AuthenticatorUri { get; set; }
    }
}
