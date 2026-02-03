using BuildingBlocks.Application.Outbox;
using BuildingBlocks.Infrastructure.InternalCommand;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Item.Infrastructure;

public class MasterDataContext(DbContextOptions options, ILoggerFactory loggerFactory) : DbContext(options)
{
    public DbSet<MasterData.Domain.Item.Item> Items { get; set; }
    public DbSet<InternalCommand> InternalCommands { get; set; }
    
    public DbSet<OutboxMessage> OutboxMessages { get; set; }

    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}