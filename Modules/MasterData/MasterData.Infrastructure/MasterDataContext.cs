using BuildingBlocks.Application.Outbox;
using MasterData.Domain.Entities.Item;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MasterData.Infrastructure;

public class MasterDataContext(DbContextOptions<MasterDataContext> options, ILoggerFactory? loggerFactory = null)
    : DbContext(options)
{
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<Item> Items { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        if (loggerFactory != null)
        {
            optionsBuilder.UseLoggerFactory(loggerFactory);
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MasterDataContext).Assembly);
    }
}