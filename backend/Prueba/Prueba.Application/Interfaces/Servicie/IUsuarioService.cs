using Prueba.Application.Models.Dtos;
using Prueba.Application.Models.Request.Users;
using Prueba.Application.Models.Responses;

namespace Prueba.Application.Interfaces.Servicie
{
    public interface IUsuarioService
    {
        /* GenericResponse<UserDto> CreateUser(CreateUsersRequest model);
         GenericResponse<UserDto> UpdateUser(Guid id, UpdateUserRequest model);
         bool DeleteUser(Guid id);
         List<UserDto> GetAllUsers();
         GenericResponse<UserDto> GetUserById(Guid id);
        */

        public Task<GenericResponse<UsuarioDto>> Create(CreateUsersRequest model);
        public Task<GenericResponse<UsuarioDto>> Update(Guid collaboratorId, UpdateUserRequest model);
        public GenericResponse<List<UsuarioDto>> Get(FilterUserRequest model);
        public Task<GenericResponse<UsuarioDto>> Get(Guid collaboratorId);
        public Task<GenericResponse<bool>> Delete(Guid collaboratorId);
    }
}
