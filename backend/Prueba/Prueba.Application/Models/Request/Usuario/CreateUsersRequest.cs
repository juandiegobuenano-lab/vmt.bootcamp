using Prueba.Shared.Constants;
using System.ComponentModel.DataAnnotations;


namespace Prueba.Application.Models.Request.Users
{
    public class CreateUsersRequest
    {
        /*[Required(ErrorMessage = ValidationConstants.Required)]
        [MinLength(10, ErrorMessage = ValidationConstants.MinLength)]
        [MaxLength(150, ErrorMessage = ValidationConstants.MaxLength)]
        public string FullName { get; set; } = null!; */

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
