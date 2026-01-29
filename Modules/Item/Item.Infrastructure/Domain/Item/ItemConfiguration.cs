using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Item.Infrastructure.Domain.Item;

public class ItemConfiguration : IEntityTypeConfiguration<global::Item.Domain.Item.Item>
{
    public void Configure(EntityTypeBuilder<global::Item.Domain.Item.Item> builder)
    {
        
    }
}