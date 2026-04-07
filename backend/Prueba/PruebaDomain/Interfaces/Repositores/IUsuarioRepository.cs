using PruebaDomain.Database.SqlServer.Entities;


namespace PruebaDomain.Interfaces.Repositories
{
    public interface IUsuarioRepository
    {
        Task<Usuario> Create(Usuario user);
        Task<Usuario?> Get(Guid id);
        IQueryable<Usuario> Queryable();
        Task<bool> IfExists(Guid id);
        Task<Usuario> Update(Usuario id);


    }
}
