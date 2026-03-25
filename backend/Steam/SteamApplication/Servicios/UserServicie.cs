using SteamApplication.Interfaces;
using SteamApplication.Models.Request.Users;

namespace SteamApplication.Servicios
{
    public class UserService : IUserService
    {
        private static List<CreateUsersRequest> users = new List<CreateUsersRequest>();

        public List<CreateUsersRequest> GetAll()
        {
            return users;
        }

        public CreateUsersRequest? GetById(int id)
        {
            return users.FirstOrDefault(u => u.UserId == id);
        }

        public CreateUsersRequest Create(CreateUsersRequest user)
        {
            user.UserId = users.Count + 1;
            user.CreatedAt = DateTime.Now;
            users.Add(user);
            return user;
        }

        public bool Update(int id, CreateUsersRequest updatedUser)
        {
            var user = users.FirstOrDefault(u => u.UserId == id);
            if (user == null) return false;

            user.UserName = updatedUser.UserName;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;

            return true;
        }

        public bool Delete(int id)
        {
            var user = users.FirstOrDefault(u => u.UserId == id);
            if (user == null) return false;

            users.Remove(user);
            return true;
        }

    }
}
