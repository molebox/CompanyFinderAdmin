using System;

namespace CompanyFinderAdmin.Models
{
    /// <summary>
    /// Boilerplate error view model
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public string RequestId { get; set; }
        /// <summary>
        /// Show id
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}