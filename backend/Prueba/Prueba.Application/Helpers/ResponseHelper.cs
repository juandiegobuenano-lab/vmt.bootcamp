using Prueba.Application.Models.Responses;

namespace Prueba.Application.Helpers
{
    public static class ResponseHelper
    {
        // Hacer que 'errors' sea opcional con valor por defecto null
        public static GenericResponse<T> Create<T>(T data, List<string>? errors = null, string message = "Solicitud realizada correctamente")
        {
            var response = new GenericResponse<T>
            {
                Data = data,
                Message = message,
                Success = true,
                Errors = errors ?? new List<string>()
            };
            return response;
        }

        public static GenericResponse<T> Fail<T>(string message)
        {
            return new GenericResponse<T>
            {
                Message = message,
                Success = false,
                Errors = new List<string>()
            };
        }
    }
}
