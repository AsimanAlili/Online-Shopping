using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Online_Shopping.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Configurations
{
    public class DiscountConfiguration : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(150).IsRequired();
            builder.Property(x => x.SubTitle).HasMaxLength(150);
            builder.Property(x => x.SaleTitle).HasMaxLength(150);
            builder.Property(x => x.Photo).HasMaxLength(100);
            builder.Property(x => x.RedirectUrl).HasMaxLength(250);

        }
    }
}
