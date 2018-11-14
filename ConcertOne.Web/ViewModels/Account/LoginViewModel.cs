using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace ConcertOne.Web.ViewModels.Account
{
    public class LoginViewModel
    {
        [JsonProperty( "EmailAddress" )]
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [JsonProperty( "Password" )]
        [Required]
        [MinLength( 8 )]
        public string Password { get; set; }
    }
}
