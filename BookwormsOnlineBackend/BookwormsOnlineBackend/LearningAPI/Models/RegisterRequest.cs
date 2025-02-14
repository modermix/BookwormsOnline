using System.ComponentModel.DataAnnotations;

namespace BookwormsOnline.Models
{
    public class RegisterRequest
    {
        [Required, MinLength(3), MaxLength(50)]
        // Regular expression to enforce name format
        [RegularExpression(@"^[a-zA-Z '-,.]+$",
            ErrorMessage = "Only allow letters, spaces and characters: ' - , .")]
        public string FirstName { get; set; } = string.Empty;

        [Required, MinLength(3), MaxLength(50)]
        // Regular expression to enforce name format
        [RegularExpression(@"^[a-zA-Z '-,.]+$",
            ErrorMessage = "Only allow letters, spaces and characters: ' - , .")]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(50)]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(8), MaxLength(50)]
        // Regular expression to enforce password complexity
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*[0-9]).{8,}$",
            ErrorMessage = "At least 1 letter and 1 number")]
        public string Password { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? ProfileImage { get; set; }

        [Required, MinLength(8), MaxLength(255)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Credit Card No must be numeric.")]
        public string CreditCardNo { get; set; } = string.Empty;

        [Required, MinLength(8), MaxLength(8)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Mobile No must be exactly 8 digits.")]
        public string MobileNo { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string BillingAddress { get; set; } = string.Empty;

        [Required, MaxLength(255)]
        public string ShippingAddress { get; set; } = string.Empty;

    }
}
