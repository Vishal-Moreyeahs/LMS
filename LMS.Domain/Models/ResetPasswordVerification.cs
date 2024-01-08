using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.Domain.Models
{
    public class ResetPasswordVerification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ResetTokenId { get; set; }
        public string GeneratedToken { get; set; }
        public DateTime? GeneratedDate { get; set; }
        public bool VerificationStatus { get; set; }
        public int UserId { get; set; }
    }
}
