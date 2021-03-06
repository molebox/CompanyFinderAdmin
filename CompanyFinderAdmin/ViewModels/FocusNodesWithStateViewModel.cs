﻿using CompanyFinderAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CompanyFinderAdmin.ViewModels
{
    /// <summary>
    /// Focus view model with state
    /// </summary>
    public class FocusNodesWithStateViewModel
    {
        /// <summary>
        /// Node Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Node name
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Parent id, can be null
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// Order number in the table
        /// </summary>
        public int OrderNumber { get; set; }
        /// <summary>
        /// List of children nodes
        /// </summary>
        public virtual List<FocusNodesWithStateViewModel> Children { get; set; }
        /// <summary>
        /// State of the node
        /// </summary>
        public State State { get; set; }
    }
}
