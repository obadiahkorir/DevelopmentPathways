using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevelopmentPathWays.Models
{
    [Table("Employees")]
    public class EmployeeModel
    {

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string MiddleName { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        [StringLength(50)]
        public string Gender { get; set; }

        [Required]
        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string EmailAddress { get; set; }

        [Required]
        [StringLength(50)]
        public string MobileNumber { get; set; }

        [Required]
        [StringLength(50)]
        public string Address { get; set; }

        [Required]
        [StringLength(50)]
        public string Position { get; set; }

        [Required]
        public DateTime JoiningDate { get; set; }

        [Required]
        [StringLength(50)]
        public string IDNO { get; set; }

        [Required]
        [StringLength(50)]
        public string EmployeeNo { get; set; }

        [Key]
        public int EmployeeId { get; set; }

        

    }
}