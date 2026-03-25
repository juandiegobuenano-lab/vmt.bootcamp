using SteamApplication.Models.Request.Users;

namespace SteamApplication.Interfaces
{
    public interface IUserService
    {
        List<CreateUsersRequest> GetAll();
        CreateUsersRequest? GetById(int id);
        CreateUsersRequest Create(CreateUsersRequest user);
        bool Update(int id, CreateUsersRequest user);
        bool Delete(int id);
    }
}
