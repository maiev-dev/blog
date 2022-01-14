using DatabaseApi.Model.Context;
using DatabaseApi.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseApi.Model.Repositories;

public class ApiUserRightsRepository : IRepository<ApiUserRight>
{
    private ApplicationContext _context;

    public ApiUserRightsRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ApiUserRight>> GetAllAsync()
    {
        return await _context.ApiUserRights.ToListAsync();
    }

    public async Task<ApiUserRight?> FindAsync(Guid id)
    {
        return await _context.ApiUserRights.FindAsync(id);
    }

    public async Task CreateAsync(ApiUserRight element)
    {
        await _context.ApiUserRights.AddAsync(element);
    }

    public Task UpdateAsync(ApiUserRight element)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(ApiUserRight element)
    {
        var userRight = await _context.ApiUsers.FindAsync(element.Id);
        if (userRight is not null)
        {
            _context.ApiUsers.Remove(userRight);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}