using FlowTracker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Configurations
{
    public class SavingGoalConfiguration : IEntityTypeConfiguration<SavingGoal>
    {
        public void Configure(EntityTypeBuilder<SavingGoal> builder)
        {
            builder.ToTable("SavingGoals");

            builder.HasKey(s => s.Id);

            builder.HasOne(s => s.User)
                .WithMany(u => u.SavingGoals)
                .HasForeignKey(s => s.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(s => s.TargetAmount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(s => s.Deadline)
                .IsRequired(false)
                .HasColumnType("date");
        }
    }
}