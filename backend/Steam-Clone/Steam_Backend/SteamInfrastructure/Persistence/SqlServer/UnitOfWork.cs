using SteamDomain.Database.SqlServer;
using SteamDomain.Database.SqlServer.Context;
using SteamDomain.Interfaces.Repositories;

namespace SteamInfrastructure.Persistence.SqlServer
{
    public class UnitOfWork(SteamContext context, IUserRepository userRepository, IEmailTemplateRepository emailTemplateRepository) : IUnitOfWork
    {
        private readonly SteamContext _context = context;
        public IUserRepository userRepository { get; set; } = userRepository;
        public IEmailTemplateRepository emailTemplateRepository { get; set; } = emailTemplateRepository;

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
