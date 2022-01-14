using DatabaseApi.Model.Context;
using DatabaseApi.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseApi.Model.Repositories;

public class ApiUsersRepository : IRepository<ApiUser>
{
    private readonly ApplicationContext _context;
    public ApiUsersRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<ApiUser>> GetAllAsync()
    {
        return await _context.ApiUsers.ToListAsync();
    }

    public async Task<ApiUser?> FindAsync(Guid id)
    {
        return await _context.ApiUsers.FindAsync(id);
    }

    public async Task CreateAsync(ApiUser element)
    {
        await _context.ApiUsers.AddAsync(element);
    }

    public async Task UpdateAsync(ApiUser element)
    {
        var user = await _context.ApiUsers.FindAsync(element.Id);
       
        if (user is not null)
        {
            user = user with
            {
                ApiKey = element.ApiKey
            };
        }
    }

    public async Task DeleteAsync(ApiUser element)
    {
        var user = await _context.ApiUsers.FindAsync(element.Id);
        if (user is not null)
        {
            _context.ApiUsers.Remove(user);
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}