﻿using ErrorOr;
using Microsoft.EntityFrameworkCore;
using Neofilia.Domain;
using Neofilia.Domain.Common.Errors;
using static Neofilia.Domain.Local;

namespace Neofilia.Server.Data.Repository;

//TODO: implement it properly
public class LocalRepository(ApplicationDbContext context) : ILocalRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task Add(Local entity)
    {
        await _context.Locals.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var localId = new LocalId(id);
        await _context.Locals.Where(l => l.Id == localId)
                             .ExecuteDeleteAsync();
    }

    public async Task<List<Local>> Get()
    {
        var locals = await _context.Locals.ToListAsync();

        return locals is null ? [] : locals;
    }

    public async Task<ErrorOr<Local>> GetById(int id)
    {
        var localId = new LocalId(id);
        var local = await _context.Locals.FindAsync(localId);

        return local is null ? 
            Errors.LocalErrors.LocalNotFound(id) :
            local;
    }

    public async Task Update(int id, Local entity)
    {        
        var localId = new LocalId(id);
        await _context.Locals.Where(l => l.Id == localId)
                             .ExecuteUpdateAsync(setters => setters
                             .SetProperty(l => l.Name, entity.Name)
                             .SetProperty(l => l.Email, entity.Email)
                             .SetProperty(l => l.Address.Street, entity.Address.Street)
                             .SetProperty(l => l.Address.CivilNumber, entity.Address.CivilNumber)
                             .SetProperty(l => l.Address.PhoneNumber, entity.Address.PhoneNumber)
                             .SetProperty(l => l.EventStartsAt, entity.EventStartsAt)
                             .SetProperty(l => l.EventEndsAt, entity.EventEndsAt));
    }

    public List<Table> GetLocalTables(int localId)
    {
        var id = new LocalId(localId);
        var tables =  _context.Locals.Where(l => l.Id == id)
                                     .Include(l => l.Tables)
                                     .SelectMany(l => l.Tables)
                                     .ToList();
        return tables is null ?
            [] : tables;
    }
    public async Task<List<Local>> GetLocalsWithTables()
    {
        var locals = await Get();

        var localsWithTables = locals.Select(l =>
        {
            var tables = GetLocalTables(l.Id.Value);
            l.AddTables(tables);
            return l;
        });
        return localsWithTables.ToList();
    }
}
