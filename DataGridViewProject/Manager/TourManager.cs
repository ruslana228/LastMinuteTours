using Entities.Models;
using Manager.Contracts;
using MemoryStorage.Contracts;

namespace Manager
{
    public class TourManager : ITourManager
    {
        private ITourStorage Storage { get; }

        public TourManager(ITourStorage storage)
        {
            Storage = storage;
        }

        /// <summary>
        /// Возврат списка всех туров
        /// </summary>
        public Task<IReadOnlyCollection<TourModel>> GetAll(CancellationToken cancellationToken = default) => Storage.GetAll(cancellationToken);
        

        /// <summary>
        /// Возврат тура по его идентификатору
        /// </summary>
        public Task<TourModel?> GetById(Guid id, CancellationToken cancellationToken = default) => Storage.GetById(id, cancellationToken);
        

        /// <summary>
        /// Добавление нового тура в список
        /// </summary>
        public Task Add(TourModel tour, CancellationToken cancellationToken = default) => Storage.Add(tour, cancellationToken);
        

        /// <summary>
        /// Метод для обновления существующего тура в списке по его идентификатору
        /// </summary>
        public Task Update(TourModel tour, CancellationToken cancellationToken = default) => Storage.Update(tour, cancellationToken);
        

        /// <summary>
        /// Метод для удаления тура из списка по его идентификатору
        /// </summary>
        public Task Delete(Guid id, CancellationToken cancellationToken = default) => Storage.Delete(id, cancellationToken);


        /// <summary>
        /// Возвращает статистику по всем турам
        /// </summary>
        public async Task<TourStatistics> GetStatistics(CancellationToken cancellationToken = default)
        {
            var tours = await Storage.GetAll(cancellationToken);

            // Расчет статистики в бизнес-логике
            var totalToursCount = tours.Count;
            var totalCostAllTours = tours.Sum(t => (t.CostPerVacationer * t.NumberVacationers) + t.Surcharges);
            var toursWithSurchargesCount = tours.Count(t => t.Surcharges > 0);
            var totalSurcharges = tours.Sum(t => t.Surcharges);

            return new TourStatistics
            {
                TotalToursCount = totalToursCount,
                TotalCostAllTours = totalCostAllTours,
                ToursWithSurchargesCount = toursWithSurchargesCount,
                TotalSurcharges = totalSurcharges
            };
        }
    }
}
