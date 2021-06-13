using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Online_Shopping.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Configurations
{
    public class SliderConfiguration : IEntityTypeConfiguration<Slider>
    {
        public void Configure(EntityTypeBuilder<Slider> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Text).HasMaxLength(350);
            builder.Property(x => x.RedirectUrl).HasMaxLength(350);
            builder.Property(x => x.Photo).HasMaxLength(100);
        }
    }
}
