using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UserManagement.Models;

namespace UserManagement.Data;

public class DataContext : DbContext, IDataContext
{
    public DataContext() => Database.EnsureCreated();

    public DataContext(DbContextOptions options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder options) { }

    protected override void OnModelCreating(ModelBuilder model)
    {
        model.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Log> Logs { get; set; }

    public IQueryable<TEntity> GetAll<TEntity>() where TEntity : class
        => base.Set<TEntity>();

    public async Task Create<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class
    {
        await base.AddAsync(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task Update<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class
    {
        base.Update(entity);
        await SaveChangesAsync(cancellationToken); 
    }

    public async Task Delete<TEntity>(TEntity entity, CancellationToken cancellationToken) where TEntity : class
    {
        base.Remove(entity);
        await SaveChangesAsync(cancellationToken);
    }

    public async Task<TEntity?> Get<TEntity>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken) where TEntity : class
    {
       return await base.Set<TEntity>().FirstOrDefaultAsync(predicate,cancellationToken);
    }
}
