using Microsoft.EntityFrameworkCore;
using PruebaDomain.Database.SqlServer.Context;
using PruebaDomain.Database.SqlServer.Entities;
using PruebaDomain.Interfaces.Repositories;


namespace Prueba.Infrastructure.Persistence.SqlServer.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly PruebaContext _context;

        public UsuarioRepository(PruebaContext context)
        {
            _context = context;
        }

        public async Task<Usuario> Create(Usuario user)
        {
            await _context.Usuarios.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<Usuario?> Get(Guid id)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(x => x.UserId == id);
        }

        public IQueryable<Usuario> Queryable()
        {
            return _context.Usuarios.AsQueryable();
        }

        public async Task<bool> IfExists(Guid id)
        {
            return await _context.Usuarios
                .AnyAsync(x => x.UserId == id);
        }

        public async Task<Usuario> Update(Usuario user)
        {
            _context.Usuarios.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}
