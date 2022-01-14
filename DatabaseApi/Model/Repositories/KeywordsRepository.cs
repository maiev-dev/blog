using DatabaseApi.Model.Context;
using DatabaseApi.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseApi.Model.Repositories;

public class KeywordsRepository : IRepository<Keyword>
{
    private ApplicationContext _context;

    public KeywordsRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<Keyword>> GetAllAsync()
    {
        return await _context.Keywords.ToListAsync();
    }

    public async Task<Keyword?> FindAsync(Guid id)
    {
        return await _context.Keywords.FindAsync(id);
    }

    public async Task CreateAsync(Keyword element)
    {
        await _context.Keywords.AddAsync(element);
    }

    public async Task UpdateAsync(Keyword element)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Keyword element)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}