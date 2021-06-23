using LilySimple.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.EntityTypeConfigurations
{
    public class RoleEntityTypeConfiguration :
         EntityTypeConfigurationBase<Role>,
         IEntityTypeConfiguration<Role>
    {
        public new void Configure(EntityTypeBuilder<Role> builder)
        {
            base.Configure(builder);

            builder.ToTable("role");

            builder.Property(m => m.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(m => m.IsReserved)
                .HasColumnName("is_reserved")
                .IsRequired()
                .HasDefaultValue(false);
        }
    }
}
