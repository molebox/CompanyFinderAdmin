using CompanyFinderAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.ViewModels
{
    /// <summary>
    /// tree nodes with state view model
    /// </summary>
    public class TreeNodesWithStateViewModel
    {
        /// <summary>
        /// The name of the node
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// The tree node id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The nodes parents id, can be null. The root node is null
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// The order of the node levels
        /// </summary>
        public int OrderNumber { get; set; }

        /// <summary>
        /// List of the children nodes
        /// </summary>
        public virtual List<TreeNodesWithStateViewModel> Children { get; set; }
        /// <summary>
        /// State of the node
        /// </summary>
        public State State { get; set; }
    }
}
