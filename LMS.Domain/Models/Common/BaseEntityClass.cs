using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Models.Common
{
    public class BaseEntityClass
    {
        [Required]
        public bool IsActive { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedDate { get; set;} = DateTime.UtcNow;
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set;}
    }
}
