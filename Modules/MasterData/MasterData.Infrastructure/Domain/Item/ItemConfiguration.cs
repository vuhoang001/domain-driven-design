using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Item.Infrastructure.Domain.Item;

public class ItemConfiguration : IEntityTypeConfiguration<global::MasterData.Domain.Item.Item>
{
    public void Configure(EntityTypeBuilder<global::MasterData.Domain.Item.Item> builder)
    {
        // Table
        builder.ToTable("Items", schema: "dbo");

        // Primary Key
        builder.HasKey(x => x.Id);

        // Id - ItemId (strongly-typed Guid)
        builder.Property(x => x.Id)
            .HasColumnName("Id")
            .HasColumnType("uniqueidentifier")
            .ValueGeneratedNever();

        // ItemName
        builder.Property(x => x.ItemName)
            .HasColumnName("ItemName")
            .HasColumnType("nvarchar(255)")
            .IsRequired()
            .HasMaxLength(255);

        // ItemDesc - optional
        builder.Property(x => x.ItemDesc)
            .HasColumnName("ItemDesc")
            .HasColumnType("nvarchar(max)")
            .IsRequired(false);

        // Price - FIX: explicitly set column type to decimal(18,2)
        builder.Property(x => x.Price)
            .HasColumnName("Price")
            .HasColumnType("decimal(18,2)")
            .IsRequired()
            .HasPrecision(18, 2);
    }
}