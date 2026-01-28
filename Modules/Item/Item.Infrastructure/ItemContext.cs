using BuildingBlocks.Infrastructure.InternalCommand;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Item.Infrastructure;

public class ItemContext(DbContextOptions options, ILoggerFactory loggerFactory) : DbContext(options)
{
    public DbSet<Domain.Item.Item> Items { get; set; }
    public DbSet<InternalCommand> InternalCommands { get; set; }
    
    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}