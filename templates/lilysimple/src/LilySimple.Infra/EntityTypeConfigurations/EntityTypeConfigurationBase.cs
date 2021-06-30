using LilySimple.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace LilySimple.EntityTypeConfigurations
{
    public class EntityTypeConfigurationBase<TModel>
        : IEntityTypeConfiguration<TModel>
        where TModel : ModelBase
    {
        public void Configure(EntityTypeBuilder<TModel> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .HasColumnName("id");

            builder.Property(m => m.IsDeleted)
                .HasColumnName("is_deleted")
                .HasDefaultValue(false);

            builder.Property(m => m.InsertedAt)
                .HasColumnName("inserted_at")
                .ValueGeneratedOnAdd();

            builder.Property(m => m.UpdatedAt)
                .HasColumnName("updated_at")
                .ValueGeneratedOnAddOrUpdate();
        }
    }
}
