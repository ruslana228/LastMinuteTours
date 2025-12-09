using Entities.Models;
using MemoryStorage.Contracts;
using Microsoft.EntityFrameworkCore;

namespace DatabaseStorage
{
    /// <summary>
    /// Хранилище туров в виде базы данных.
    /// </summary>
    public class TourDatabaseStorage : ITourStorage
    {
        /// <summary>
        /// Добавляет новый тур в базу данных.
        /// </summary>
        public async Task Add(TourModel tour, CancellationToken cancellationToken = default)
        {
            using var database = new TourDatabaseContext();
            database.Tours.Add(tour);
            await database.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Возвращает все туры из базы данных.
        /// </summary>
        public async Task<IReadOnlyCollection<TourModel>> GetAll(CancellationToken cancellationToken = default)
        {
            using var database = new TourDatabaseContext();
            var tours = await database.Tours.AsNoTracking().ToListAsync(cancellationToken);
            return tours.AsReadOnly();
        }

        /// <summary>
        /// Возвращает тур по его уникальному идентификатору.
        /// </summary>
        public async Task<TourModel?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            using var database = new TourDatabaseContext();
            var tour = await database.Tours
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
            return tour;
        }

        /// <summary>
        /// Обновляет существующий тур в базе данных.
        /// </summary>
        public async Task Update(TourModel tour, CancellationToken cancellationToken = default)
        {
            using var database = new TourDatabaseContext();
            database.Tours.Update(tour);
            await database.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Удаляет тур из базы данных по его идентификатору.
        /// </summary>
        public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            using var database = new TourDatabaseContext();
            var tour = await database.Tours
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

            if (tour != null)
            {
                database.Tours.Remove(tour);
                await database.SaveChangesAsync(cancellationToken);
            }
        }

    }
}
