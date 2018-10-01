using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cognitocoreapi.Models
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [JsonProperty("email")]
        public string Email { get; set; }
    }
}