using SteamDomain.Interfaces.Repositories;

namespace SteamDomain.Database.SqlServer
{
    public interface IUnitOfWork
    {
        IUserRepository userRepository { get; set; }
        IEmailTemplateRepository emailTemplateRepository { get; set; }
        Task SaveChangesAsync();
    }



}

