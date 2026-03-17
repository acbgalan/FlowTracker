using System;
using System.Collections.Generic;
using System.Text;
using FlowTracker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowTracker.Data.Configurations
{
    public class BudgetConfiguration : IEntityTypeConfiguration<Budget>
    {
        public void Configure(EntityTypeBuilder<Budget> builder)
        {
            builder.ToTable("Budgets", b =>
            {
                b.HasCheckConstraint("CK_Budgets_Month", "[Month] >= 1 AND [Month] <= 12");
                b.HasCheckConstraint("CK_Budgets_Year", "[Year] >= 2000 AND [Year] <= 2100");
            });

            builder.HasKey(b => b.Id);

            builder.HasOne(b => b.Category)
                .WithMany(c => c.Budgets)
                .HasForeignKey(b => b.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.User)
                .WithMany(u => u.Budgets)
                .HasForeignKey(b => b.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(b => b.LimitAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(b => b.Month)
                .IsRequired();

            builder.Property(b => b.Year)
                .IsRequired();

            builder.HasIndex(b => new { b.UserId, b.CategoryId, b.Month, b.Year }).IsUnique();
        }
    }
}
