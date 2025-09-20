using Microsoft.EntityFrameworkCore;
using Model.DomainModels.ProductAggregates;

namespace Model;
/// <summary>
/// Provides the database context for a multi-page ASP.NET Core application.
/// </summary>
/// <remarks>
/// The context is configured via dependency injection and exposes
/// <see cref="DbSet{TEntity}"/> properties for each entity type.
/// </remarks>
public class SinglePageApplicationContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="SinglePageApplicationContext"/> class
    /// using the supplied options.
    /// </summary>
    /// <param name="options">The options used to configure the context.</param>
    public SinglePageApplicationContext(DbContextOptions<SinglePageApplicationContext> options)
        : base(options)
    {
    }
    /// <summary>
    /// Represents the collection of <see cref="Product"/> entities in the database.
    /// </summary>
    public DbSet<Product> Products { get; set; }
}

