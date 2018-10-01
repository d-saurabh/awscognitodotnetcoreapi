using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace cognitocoreapi.Models
{
     public class ChangePasswordModel
    {
        [Required]
        [Display(Name = "Token")]
        [JsonProperty("token")]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        [JsonProperty("old_password")]
        public string OldPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        [JsonProperty("new_password")]
        public string NewPassword { get; set; }
    }
}