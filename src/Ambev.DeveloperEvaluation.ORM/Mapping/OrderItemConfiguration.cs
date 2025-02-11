using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambev.DeveloperEvaluation.ORM.Mapping
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<Domain.Entities.OrderItem>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.OrderItem> builder)
        {
            builder.ToTable("OrderItems");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

            builder.Property(u => u.ProductCode).IsRequired().HasMaxLength(50);
            builder.Property(u => u.ProductDescription).IsRequired().HasMaxLength(50);
            builder.Property(u => u.Quantity).IsRequired();
            builder.Property(u => u.UnitPrice).IsRequired();
            builder.Property(u => u.Discount).IsRequired();
            builder.Property(u => u.OrderId).IsRequired();

            builder.HasOne(u => u.Order).WithMany(u => u.Items).HasForeignKey(u => u.OrderId);
        }
    }
}