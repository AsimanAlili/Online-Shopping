using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Online_Shopping.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Configurations
{
    public class ProductSizeConfiguration : IEntityTypeConfiguration<ProductSize>
    {
        public void Configure(EntityTypeBuilder<ProductSize> builder)
        {
            builder.HasOne(x => x.Size).WithMany(x => x.ProductSizes).HasForeignKey(x => x.SizeId);
            builder.HasOne(x => x.Product).WithMany(x => x.ProductSizes).HasForeignKey(x => x.ProductId);

        }
    }
}
