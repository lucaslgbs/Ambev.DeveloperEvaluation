using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class OrderConfiguration : IEntityTypeConfiguration<Domain.Entities.Order>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Order> builder)
        {
            builder.ToTable("Orders");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(u => u.Branch).IsRequired().HasMaxLength(100);
            builder.Property(u => u.OrderNumber).IsRequired().HasMaxLength(30);
            builder.Property(u => u.Customer).IsRequired().HasMaxLength(100);
            builder.Property(u => u.OrderDate).IsRequired();
            builder.Property(u => u.IsCancelled).IsRequired();

        }
    }
}
