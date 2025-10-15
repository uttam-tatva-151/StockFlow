using StockFlow.Repository.Entities;
using StockFlow.Repository.Entities.Configurations;
using Microsoft.EntityFrameworkCore;
using StockFlow.Repository.Entities.DbSeed;

namespace StockFlow.Repository.Contexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    #region DBSet
    public virtual DbSet<UserAuth> UserAuths { get; set; }
    #endregion

    #region Methods
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.ApplyConfiguration(new UserAuthConfiguration());
        modelBuilder.ApplyConfiguration(new RoleConfiguration());
        modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

        base.OnModelCreating(modelBuilder);
        // InitialAuthModuleSeed.Seed(modelBuilder);
    }

    #endregion
}
