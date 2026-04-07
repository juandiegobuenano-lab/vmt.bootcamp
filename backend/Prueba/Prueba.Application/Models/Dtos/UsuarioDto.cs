namespace Prueba.Application.Models.Dtos
{
    public class UsuarioDto
    {

        public Guid UserId { get; set; }
        public string Description { get; set; }

        public string DisplayName { get; set; }
        public string Username { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool StatusType { get; set; }


    }
}
