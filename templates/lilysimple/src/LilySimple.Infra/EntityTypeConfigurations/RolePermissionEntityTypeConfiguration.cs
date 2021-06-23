using LilySimple.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.EntityTypeConfigurations
{
    public class RolePermissionEntityTypeConfiguration :
         EntityTypeConfigurationBase<RolePermission>,
         IEntityTypeConfiguration<RolePermission>
    {
        public new void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            base.Configure(builder);

            builder.ToTable("role_permission");

            builder.Property(m => m.RoleId)
                .HasColumnName("role_id")
                .IsRequired();

            builder.Property(m => m.PermissionId)
                .HasColumnName("permission_id")
                .IsRequired();
        }
    }
}
