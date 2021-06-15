using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Online_Shopping.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Configurations
{
    public class BlogConfiguration : IEntityTypeConfiguration<Blog>
    {
        public void Configure(EntityTypeBuilder<Blog> builder)
        {
            builder.Property(x => x.Title).HasMaxLength(250).IsRequired();
            builder.Property(x => x.FullName).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Desc).HasMaxLength(3000);
            builder.Property(x => x.Photo).HasMaxLength(100);
            builder.HasOne(x => x.BlogCategory).WithMany(x => x.Blogs);
        }
    }
}
