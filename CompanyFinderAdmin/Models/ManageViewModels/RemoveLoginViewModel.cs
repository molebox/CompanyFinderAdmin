using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.Models.ManageViewModels
{
    /// <summary>
    /// Boilerplate Remove login view model
    /// </summary>
    public class RemoveLoginViewModel
    {
        /// <summary>
        /// Login provider
        /// </summary>
        public string LoginProvider { get; set; }
        /// <summary>
        /// Provider key
        /// </summary>
        public string ProviderKey { get; set; }
    }
}
