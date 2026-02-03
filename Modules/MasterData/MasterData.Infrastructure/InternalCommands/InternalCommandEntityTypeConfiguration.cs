using BuildingBlocks.Infrastructure.InternalCommand;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Item.Infrastructure.InternalCommands;

public class InternalCommandEntityTypeConfiguration : IEntityTypeConfiguration<InternalCommand>
{
    public void Configure(EntityTypeBuilder<InternalCommand> builder)
    {
        builder.Property(x => x.Error)
            .HasColumnType("nvarchar(max)");
    }
}