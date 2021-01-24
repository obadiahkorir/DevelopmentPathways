using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DevelopmentPathWays.Models
{
    public class DepartmentModel
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        [StringLength(50)]
        public string DepartmentCode { get; set; }

        [Required]
        [StringLength(50)]
        public string DepartmentName { get; set; }
    }
}