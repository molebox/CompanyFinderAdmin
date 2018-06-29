using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.Models.ManageViewModels
{
    /// <summary>
    /// Boilerplate Recovery code view model
    /// </summary>
    public class ShowRecoveryCodesViewModel
    {
        /// <summary>
        /// Recovery codes
        /// </summary>
        public string[] RecoveryCodes { get; set; }
    }
}
