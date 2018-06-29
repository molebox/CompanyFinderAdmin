using CompanyFinderAdmin.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.ViewModels
{
    /// <summary>
    /// View model to hold the created tree view
    /// </summary>
    public class NodesToSend
    {
        /// <summary>
        /// List of tree nodes
        /// </summary>       
        public List<TreeNodes> TreeNodesList { get; set; }
        /// <summary>
        /// List of tree nodes
        /// </summary>       
        public List<FocusNodes> FocusNodesList { get; set; }

    }
}
