using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Online_Shopping.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Online_Shopping.Data.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.Property(x => x.Phone).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Address).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Support).HasMaxLength(150).IsRequired();

        }
    }
}
