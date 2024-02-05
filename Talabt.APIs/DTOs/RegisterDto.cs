using System.ComponentModel.DataAnnotations;

namespace Talabt.APIs.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string DisplayName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("(?=^.{6,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&amp;*()_+}{&quot;:;'?/&gt;.&lt;,])(?!.*\\s).*$",
       ErrorMessage = "Password Must Have 1 UpperCase , 1 LowerCase , 1 Number , 1 Non Alphanumeric And At Least 6 Chars")]
        public string Password { get; set; }
    }
}
