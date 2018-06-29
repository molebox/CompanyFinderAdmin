using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DatabaseLibrary.Models
{
    public partial class CompanySkills
    {
        [Key]
        public int CompSkillId { get; set; }
        public int CompanyId { get; set; }
        public int SkillId { get; set; }


        public Companies Company { get; set; }

        public SkillSet SkillSet { get; set; }

    }
}
