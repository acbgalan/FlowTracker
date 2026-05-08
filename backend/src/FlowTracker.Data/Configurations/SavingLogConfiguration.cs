using FlowTracker.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace FlowTracker.Data.Configurations
{
    public class SavingLogConfiguration : IEntityTypeConfiguration<SavingLog>
    {
        public void Configure(EntityTypeBuilder<SavingLog> builder)
        {
            builder.ToTable("SavingLogs");

            builder.HasKey(x => x.Id);

            builder.HasOne(s => s.SavingGoal)
                .WithMany(s => s.SavingLogs)
                .HasForeignKey(s => s.SavingGoalId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Transaction)
                .WithMany(t => t.SavingLogs)
                .HasForeignKey(s => s.TransactionId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(s => s.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

            builder.Property(s => s.Type)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(20);

            builder.Property(s => s.Date)
                .IsRequired();
        }
    }
}
