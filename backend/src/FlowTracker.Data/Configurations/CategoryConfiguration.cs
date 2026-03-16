using FlowTracker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("Categories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(c => c.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(c => c.Icon)
                .IsRequired()
                .HasMaxLength(100)
                .HasDefaultValue("default-icon");

            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(250);

            builder.HasIndex(c => new { c.Name, c.Type, c.UserId }).IsUnique();
        }
    }
}