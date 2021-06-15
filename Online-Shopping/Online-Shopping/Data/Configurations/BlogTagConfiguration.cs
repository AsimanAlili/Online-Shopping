using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Online_Shopping.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Configurations
{
    public class BlogTagConfiguration : IEntityTypeConfiguration<BlogTag>
    {
        public void Configure(EntityTypeBuilder<BlogTag> builder)
        {
            builder.HasOne(x => x.Blog).WithMany(x => x.BlogTags).HasForeignKey(x => x.BlogId);
            builder.HasOne(x => x.Tag).WithMany(x => x.BlogTags).HasForeignKey(x => x.TagId);
        }
    }
}
