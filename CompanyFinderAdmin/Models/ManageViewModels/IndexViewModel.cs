using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.Models.ManageViewModels
{
    /// <summary>
    /// Boilerplate index view model
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// Username
        /// </summary>
        public string Username { get; set; }
        /// <summary>
        /// Email confirm
        /// </summary>
        public bool IsEmailConfirmed { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        /// <summary>
        /// Phone number
        /// </summary>
        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Status message
        /// </summary>
        public string StatusMessage { get; set; }
    }
}
