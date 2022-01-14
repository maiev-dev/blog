using DatabaseApi.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace DatabaseApi.Model.Context;

public class ApplicationContext : DbContext
{
    public DbSet<ApiUser> ApiUsers { get; set; }
    public DbSet<ApiUserRight> ApiUserRights { get; set; }
    public DbSet<Article> Articles { get; set; }
    public DbSet<Keyword> Keywords { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) 
    :base(options)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }
}