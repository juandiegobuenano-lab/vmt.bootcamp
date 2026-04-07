using Prueba.Application.Helpers;
using Prueba.Application.Interfaces.Servicie;
using Prueba.Application.Models.Dtos;
using Prueba.Application.Models.Request.Users;
using Prueba.Application.Models.Responses;
using Prueba.Shared.Constants;
using PruebaDomain.Database.SqlServer.Entities;
using PruebaDomain.Exceptions;
using PruebaDomain.Interfaces.Repositories;

namespace Prueba.Application.Servicios
{
    public class UsuarioServicie : IUsuarioService
    {
        private readonly IUsuarioRepository repository;

        public UsuarioServicie(IUsuarioRepository repository)
        {
            this.repository = repository;
        }

        // private static List<UserDto> _data = new();

        public async Task<GenericResponse<UsuarioDto>> Create(CreateUsersRequest model)
        {
            var create = await repository.Create(new Usuario
            {
                UserId = Guid.NewGuid(),
                DisplayName = model.DisplayName,
                Username = model.Username,
                Description = model.Description,
                CreatedAt = DateTime.UtcNow,
            });

            return ResponseHelper.Create(Map(create));
        }

        public async Task<GenericResponse<bool>> Delete(Guid id)
        {
            var user = await GetUserById(id);

            // 🔥 eliminación lógica (opcional)
            // user.DeletedAt = DateTimeHelper.UtcNow();

            await repository.Update(user);

            return ResponseHelper.Create(true);
        }

        public GenericResponse<List<UsuarioDto>> Get(FilterUserRequest model)
        {
            var queryable = repository.Queryable();

            // Filtrado de nombre de usuario
            if (!string.IsNullOrWhiteSpace(model.Username))
            {
                queryable = queryable.Where(x => x.Username.Contains(model.Username ?? ""));
            }

            // Realizar paginación y realizar consulta
            var users = queryable.Skip(model.Offset).Take(model.Limit).ToList();

            // Mapear colaboradores
            List<UsuarioDto> mapped = new List<UsuarioDto>();
            foreach (var user in users)
            {
                mapped.Add(Map(user));
            }

            return ResponseHelper.Create(mapped);
        }

        public async Task<GenericResponse<UsuarioDto>> Get(Guid id)
        {
            var collaborator = await GetUserById(id);
            return ResponseHelper.Create(Map(collaborator));
        }

        public async Task<GenericResponse<UsuarioDto>> Update(Guid id, UpdateUserRequest model)
        {
            var user = await GetUserById(id);

            user.Username = model.Username ?? user.Username;

            //user.UpdateAt = DateTimeHelper.UtcNow();

            var update = await repository.Update(user);

            return ResponseHelper.Create(Map(update));
        }

        private async Task<Usuario> GetUserById(Guid id)
        {
            return await repository.Get(id)
                 ?? throw new NotFoundException(ResponseConstants.USER_NOT_EXISTS);
        }

        private static UsuarioDto Map(Usuario user)
        {
            return new UsuarioDto
            {
                UserId = user.UserId,
                Username = user.Username,
                DisplayName = user.DisplayName,
                CreatedAt = user.CreatedAt,
                Description = user.Description ?? string.Empty,
                StatusType = user.StatusType == 1,
            };
        }

        /*
        public List<UserDto> GetAllUsers()
        {
            return _data;
        }

        public GenericResponse<UserDto> GetUserById(Guid id)
        {
            var user = _data.FirstOrDefault(x => x.UserId == id);

            return new GenericResponse<UserDto>
            {
                Message = user != null ? "OK" : "No encontrado",
                Data = user
            };
        }

        public GenericResponse<UserDto> UpdateUser(Guid id, UpdateUserRequest model)
        {
            var user = _data.FirstOrDefault(x => x.UserId == id);

            if (user == null)
                return new GenericResponse<UserDto> { Message = "No encontrado" };

            user.FullName = model.FullName;
            user.Email = model.Email;
            user.Username = model.Username;

            return new GenericResponse<UserDto>
            {
                Message = "Actualizado",
                Data = user
            };
        }

        public bool DeleteUser(Guid id)
        {
            var user = _data.FirstOrDefault(x => x.UserId == id);
            if (user == null) return false;

            _data.Remove(user);
            return true;
        }
        */
    }
}
