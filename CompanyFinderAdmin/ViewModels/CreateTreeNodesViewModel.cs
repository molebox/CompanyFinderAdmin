using CompanyFinderAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.ViewModels
{
    /// <summary>
    /// View Model for creating the treeview nodes
    /// </summary>
    public class CreateTreeNodesViewModel
    {
        /// <summary>
        /// List of tree nodes
        /// </summary>
        public List<TreeNodes> TreeNodesList { get; set; }
        /// <summary>
        /// The new tree node
        /// </summary>
        public TreeNodes TreeNodes { get; set; }
        ///// <summary>
        ///// The focus nodes
        ///// </summary>
        //public FocusNodes FocusNodes { get; set; }
        ///// <summary>
        ///// List of the focus nodes
        ///// </summary>
        //public List<FocusNodes> FocusNodesList { get; set; }


       
    }
}
