using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Neofilia.Domain;

namespace Neofilia.Server.Data.Repository;

//basic implementation, TODO: implement it properly
public class TableRepository(ApplicationDbContext context) : ITableRepository
{
    private readonly ApplicationDbContext _context = context;
    public async Task Add(Table entity)
    {
        await _context.Tables.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        await _context.Tables.Where(t => t.Id.Value == id)
                             .ExecuteDeleteAsync();
    }

    public async Task<List<Table>> Get()
    {
        var tables = await _context.Tables.ToListAsync();
        return tables is null ? [] : tables;
    }

    public async Task<ErrorOr<Table>> GetById(int id)
    {
        var table = await _context.Tables.FindAsync(id);
        return table is null ?
            Error.NotFound() :
            table;
    }

    public async Task Update(int id, Table entity)
    {
        await _context.Tables.Where(t => t.Id.Value == id)
                             .ExecuteUpdateAsync(setters => setters
                             .SetProperty(t => t.TableNumber, entity.TableNumber)
                             .SetProperty(t => t.Reward, entity.Reward));
    }
}
