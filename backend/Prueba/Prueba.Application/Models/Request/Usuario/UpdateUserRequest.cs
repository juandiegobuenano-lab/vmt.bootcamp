using Prueba.Shared.Constants;
using System.ComponentModel.DataAnnotations;

namespace Prueba.Application.Models.Request.Users
{
    public class UpdateUserRequest
    {

        [Required(ErrorMessage = ValidationConstants.Required)]
        [MinLength(5, ErrorMessage = ValidationConstants.MinLength)]
        public string DisplayName { get; set; }
        [Required(ErrorMessage = ValidationConstants.Required)]
        [MinLength(5, ErrorMessage = ValidationConstants.MinLength)]
        public string Username { get; set; }

        [Required(ErrorMessage = ValidationConstants.Required)]
        [MaxLength(200, ErrorMessage = ValidationConstants.MaxLength)]
        public string Description { get; set; }
    }
}
