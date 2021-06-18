using LilySimple.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.EntityTypeConfigurations
{
    public class UserRoleEntityTypeConfiguration :
       EntityTypeConfigurationBase<UserRole>,
       IEntityTypeConfiguration<UserRole>
    {
        public new void Configure(EntityTypeBuilder<UserRole> builder)
        {
            base.Configure(builder);

            builder.ToTable("user_role");

            builder.Property(m => m.UserId)
                .HasColumnName("user_id")
                .IsRequired();

            builder.Property(m => m.RoleId)
                .HasColumnName("role_id")
                .IsRequired();
        }
    }
}
