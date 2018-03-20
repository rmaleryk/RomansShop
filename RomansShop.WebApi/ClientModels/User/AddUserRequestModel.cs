using System.ComponentModel.DataAnnotations;
using RomansShop.Domain.Entities;

namespace RomansShop.WebApi.ClientModels.User
{
    public class AddUserRequestModel
    {
        [Required(ErrorMessage = "The field 'FullName' is required.")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "The field 'Email' is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "The field 'Password' is required.")]
        public string Password { get; set; }
    }
}