using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.ViewModels
{
    /// <summary>
    /// Template login view model 
    /// </summary>
    public class TemplateLoginViewModel
    {
        /// <summary>
        /// The admin id
        /// </summary>
        public int TemplateAccessID { get; set; }
        /// <summary>
        /// The Admin username
        /// </summary>
        [Required]
        [Display(Name = "Username")]
        public string CompanyAccessName { get; set; }
        /// <summary>
        /// The admin password
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string CompanyAccessPassword { get; set; }
    }
}
