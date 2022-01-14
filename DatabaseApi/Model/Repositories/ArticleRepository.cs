using DatabaseApi.Model.Context;
using DatabaseApi.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseApi.Model.Repositories;

public class ArticleRepository : IRepository<Article>
{
    private readonly ApplicationContext _context;

    public ArticleRepository(ApplicationContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Article>> GetAllAsync()
    {
        var res = _context.Articles.Select(a => new Article(a.Creator, 
                                                                    a.Header,
                                                                    a.Abstract, 
                                                                    a.Text)
        {
            Keywords = _context.Keywords.Where(k => k.ArticleId == a.Id).ToList(),
            Id = a.Id
        });
        return await res.ToListAsync();
    }

    public async Task<Article?> FindAsync(Guid id)
    {
        return await _context.Articles.FindAsync(id);
    }

    public async Task CreateAsync(Article element)
    {
        await _context.Articles.AddAsync(element);
    }

    public Task UpdateAsync(Article element)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Article element)
    {
        throw new NotImplementedException();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync(); 
    }
}