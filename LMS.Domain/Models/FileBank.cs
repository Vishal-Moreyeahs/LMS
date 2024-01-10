using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LMS.Domain.Models.Common;

namespace LMS.Domain.Models
{
    public class FileBank : BaseEntityClass
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public string Format { get; set; }
        public int Company_Id { get; set; }
        public string? Size { get; set; }
        public byte[] Path { get; set; }

        public virtual Company Company { get; set; } = null!;

    }
}
