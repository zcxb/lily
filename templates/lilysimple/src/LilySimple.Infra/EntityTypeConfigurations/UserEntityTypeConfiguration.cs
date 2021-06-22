using LilySimple.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.EntityTypeConfigurations
{
    public class UserEntityTypeConfiguration :
        EntityTypeConfigurationBase<User>,
        IEntityTypeConfiguration<User>
    {
        public new void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.ToTable("user");

            builder.Property(m => m.UserName)
                .HasColumnName("username")
                .IsRequired()
                .HasMaxLength(127);

            builder.Property(m => m.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired()
                .HasMaxLength(64);
        }
    }
}
