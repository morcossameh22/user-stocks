using Stocks.Core.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Stocks.Core.Entities;

namespace Stocks.Infrastructure.DbContext
{
  /* The ApplicationDbContext class is used for managing the database context and includes a DbSet for
  StockEntity. */
  public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
  {
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public ApplicationDbContext()
    {
    }

    public virtual DbSet<StockEntity> Stocks { get; set; }

    /// <summary>
    /// This is an override method for configuring the model builder.
    /// </summary>
    /// <param name="ModelBuilder">ModelBuilder is a class in Entity Framework Core that is used to
    /// create a model for a database. It provides a set of methods to configure the model, including
    /// defining entities, relationships, and constraints.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);
    }
  }
}

