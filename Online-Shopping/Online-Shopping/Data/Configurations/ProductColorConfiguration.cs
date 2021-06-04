using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Online_Shopping.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Configurations
{
    public class ProductColorConfiguration : IEntityTypeConfiguration<ProductColor>
    {
        public void Configure(EntityTypeBuilder<ProductColor> builder)
        {
            builder.HasOne(x => x.Color).WithMany(x => x.ProductColors).HasForeignKey(x => x.ColorId);
            builder.HasOne(x => x.Product).WithMany(x => x.ProductColors).HasForeignKey(x => x.ProductId);
        }
    }
}
