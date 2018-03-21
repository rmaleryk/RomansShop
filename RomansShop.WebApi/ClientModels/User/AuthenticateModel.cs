using System.ComponentModel.DataAnnotations;

namespace RomansShop.WebApi.ClientModels.User
{
    public class AuthenticateModel
    {
        [Required(ErrorMessage = "The field 'Email' is required.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The field 'Password' is required.")]
        public string Password { get; set; }
    }
}
