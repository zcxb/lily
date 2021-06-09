using LilySimple.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.EntityTypeConfigurations
{
    public class AlbumEntityTypeConfiguration :
        EntityTypeConfigurationBase<Album>,
        IEntityTypeConfiguration<Album>
    {
        public new void Configure(EntityTypeBuilder<Album> builder)
        {
            builder.ToTable("albums");

            builder.Property(m => m.Name)
                .HasColumnName("name")
                .IsRequired()
                .HasMaxLength(127);
        }
    }
}
