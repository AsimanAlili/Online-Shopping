using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Online_Shopping.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(x => x.Name).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Desc).HasMaxLength(2000);
            builder.Property(x => x.Specification).HasMaxLength(2000);
            builder.Property(x => x.Slug).HasMaxLength(200);
            builder.HasOne(x => x.SubCategory).WithMany(x => x.Products);
            builder.HasOne(x => x.Brand).WithMany(x => x.Products);

        }
    }
}
