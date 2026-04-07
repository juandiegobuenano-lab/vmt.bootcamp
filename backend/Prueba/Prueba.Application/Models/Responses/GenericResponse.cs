using Prueba.Shared.Helpers;

namespace Prueba.Application.Models.Responses
{
    public class GenericResponse<T>
    {
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public DateTime TimeStamp { get; } = DateTimeHelper.UtcNow(); // Sirve para indicar la fecha y hora en que se generó la respuesta, se establece por defecto a la fecha y hora actual en formato UTC

        public T Data { get; set; }
        public bool Success { get; set; } = true;
        public DateTime DeletedAt { get; internal set; } = DateTimeHelper.UtcNow();
    }
}