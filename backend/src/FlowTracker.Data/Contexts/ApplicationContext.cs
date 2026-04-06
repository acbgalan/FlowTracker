using FlowTracker.Data.Configurations;
using FlowTracker.Data.Entities;
using FlowTracker.Shared.Enums;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FlowTracker.Data.Contexts
{
    public class ApplicationContext : IdentityDbContext<User>
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<SavingGoal> SavingGoals { get; set; }
        public DbSet<SavingLog> SavingLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>().HasData(
                // Fixed Expenses (Needs)
                new Category { Id = 1, Name = "Vivienda", Type = TransactionType.Expense, Description = "Alquiler, hipoteca, comunidad." },
                new Category { Id = 2, Name = "Servicios", Type = TransactionType.Expense, Description = "Luz, agua, gas, internet, basura." },
                new Category { Id = 3, Name = "Supermercado", Type = TransactionType.Expense, Description = "Comida, productos de limpieza, aseo personal." },
                new Category { Id = 4, Name = "Transporte", Type = TransactionType.Expense, Description = "Gasolina, transporte público, parking, mantenimiento coche." },
                new Category { Id = 5, Name = "Salud", Type = TransactionType.Expense, Description = "Farmacia, seguro médico, dentista." },
                // Variable Expenses (Lifestyle)
                new Category { Id = 6, Name = "Restauración", Type = TransactionType.Expense, Description = "Restaurantes, bares, café, comida a domicilio." },
                new Category { Id = 7, Name = "Ocio", Type = TransactionType.Expense, Description = "Cine, conciertos, suscripciones (Netflix, Spotify, DAZN)." },
                new Category { Id = 8, Name = "Compras", Type = TransactionType.Expense, Description = "Ropa, calzado, accesorios, electrónica." },
                new Category { Id = 9, Name = "Viajes", Type = TransactionType.Expense, Description = "Hoteles, vuelos, escapadas de fin de semana." },
                new Category { Id = 10, Name = "Cuidado Personal", Type = TransactionType.Expense, Description = "Gimnasio, peluquería, estética." },
                // Income
                new Category { Id = 11, Name = "Nómina/Salario", Type = TransactionType.Income, Description = "El ingreso principal." },
                new Category { Id = 12, Name = "Freelance", Type = TransactionType.Income, Description = "Trabajos extra o proyectos secundarios." },
                new Category { Id = 13, Name = "Regalos", Type = TransactionType.Income, Description = "Dinero recibido por cumpleaños o eventos." },
                new Category { Id = 14, Name = "Inversiones", Type = TransactionType.Income, Description = "Dividendos, intereses bancarios, cripto." },
                new Category { Id = 15, Name = "Venta", Type = TransactionType.Income, Description = "Dinero por vender artículos usados (Wallapop, Vinted)." },
                // Special Categories(Saving and Investing)
                new Category { Id = 16, Name = "Ahorro e inversión", Type = TransactionType.Saving, Description = "Dinero que se mueve de la cuenta corriente a una hucha." }
            );

            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
            modelBuilder.ApplyConfiguration(new BudgetConfiguration());
            modelBuilder.ApplyConfiguration(new SavingGoalConfiguration());
            modelBuilder.ApplyConfiguration(new SavingLogConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
        }
    }
}
