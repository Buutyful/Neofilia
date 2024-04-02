using Microsoft.EntityFrameworkCore;
using Neofilia.Domain;

namespace Neofilia.Server.Data.Repository;

//basic implementation, TODO: implement it properly
public class LocalRepository(ApplicationDbContext context) : IRepository<Local>
{
    private readonly ApplicationDbContext _context = context;

    public async Task Add(Local entity)
    {
        await _context.Locals.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        await _context.Locals.Where(l => l.Id.Value == id)
                             .ExecuteDeleteAsync();
    }

    public async Task<List<Local>> Get()
    {
        var locals = await _context.Locals.ToListAsync();
        return locals;
    }

    public async Task<Local> GetById(int id)
    {
        var local = await _context.Locals.FindAsync(id);
        return local;
    }

    public async Task Update(int id, Local entity)
    {
        await _context.Locals.Where(l => l.Id.Value == id)
                             .ExecuteUpdateAsync(setters => setters
                             .SetProperty(l => l.Name, entity.Name)
                             .SetProperty(l => l.Address, entity.Address)
                             .SetProperty(l => l.EventStartsAt, entity.EventStartsAt)
                             .SetProperty(l => l.EventEndsAt, entity.EventEndsAt)
                             .SetProperty(l => l.Menus, entity.Menus)
                             .SetProperty(l => l.Tables, entity.Tables));
    }
}
