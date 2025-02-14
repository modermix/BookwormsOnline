using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BookwormsOnline.Models
{
    public class User
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        [MinLength(15), MaxLength(255)]
        public string CreditCardNo { get; set; } = string.Empty;

        [MinLength(8), MaxLength(8)]
        public string MobileNo { get; set; } = string.Empty;

        [MaxLength(150)]
        public string BillingAddress { get; set; } = string.Empty;

        [MaxLength(150)]
        public string ShippingAddress { get; set; } = string.Empty;

        [MaxLength(50)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(100), JsonIgnore]
        public string Password { get; set; } = string.Empty;


        [Column(TypeName = "datetime")]
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime UpdatedAt { get; set; }

        [MaxLength(100)]
        public string? ProfileImage { get; set; }

    }
}