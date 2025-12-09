using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace DatabaseStorage
{
    /// <summary>
    /// Контекст базы данных для управления турами
    /// </summary>
    public class TourDatabaseContext : DbContext
    {
        /// <summary>
        /// Сущность <see cref="TourModel"/>.
        /// </summary>
        public DbSet<TourModel> Tours { get; set; }

        /// <summary>
        /// Создаёт экземпляр <see cref="TourDatabaseContext"/>.
        /// </summary>
        public TourDatabaseContext() => Database.EnsureCreated();

        /// <summary>
        /// Конфигурация подключения к базе данных
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\mssqllocaldb;Database=TourDatabase;Trusted_Connection=True;");

        /// <summary>
        /// Конфигурация модели данных
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TourModel>(entity =>
            {
                // Настройка первичного ключа
                entity.HasKey(e => e.Id);

                // Настройка свойства Id
                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                // Настройка перечисления Direction
                entity.Property(e => e.Direction)
                    .IsRequired()
                    .HasConversion<int>();

                // Настройка даты вылета
                entity.Property(e => e.DepartureDate)
                    .IsRequired()
                    .HasConversion(
                        v => v.ToDateTime(TimeOnly.MinValue),
                        v => DateOnly.FromDateTime(v));

                // Настройка числовых свойств
                entity.Property(e => e.NumberNights)
                    .IsRequired();

                entity.Property(e => e.NumberVacationers)
                    .IsRequired();

                // Настройка денежных свойств
                entity.Property(e => e.CostPerVacationer)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                entity.Property(e => e.Surcharges)
                    .IsRequired()
                    .HasColumnType("decimal(18,2)");

                // Настройка булевого свойства
                entity.Property(e => e.AvailabilityWiFi)
                    .IsRequired();

                // Добавление индексов для часто используемых полей
                entity.HasIndex(e => e.Direction);
                entity.HasIndex(e => e.DepartureDate);
            });
        }
    }
}
