using Entities.Models;
using MemoryStorage.Contracts;

namespace MemoryStorage
{
    /// <summary>
    /// Класс, который предоставляет методы для добавления, обновления, удаления и получения туров, а также для подсчёта статистики.
    /// </summary>
    public class InMemoryTourStorage : ITourManager
    {
        private readonly List<TourModel> items;

        /// <summary>
        /// Инициализация нового экземпляра, загрузка начальных данных
        /// </summary>
        public InMemoryTourStorage()
        {
            items = new List<TourModel>();
        }

        /// <summary>
        /// Возврат списка всех туров
        /// </summary>
        public Task<IReadOnlyCollection<TourModel>> GetAll(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyCollection<TourModel>>(items);
        }

        /// <summary>
        /// Возврат тура по его идентификатору
        /// </summary>
        public Task<TourModel?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var tour = items.FirstOrDefault(t => t.Id == id);
            return Task.FromResult(tour);
        }

        /// <summary>
        /// Добавление нового тура в список
        /// </summary>
        public Task Add(TourModel tour, CancellationToken cancellationToken = default)
        {
            items.Add(tour);
            return Task.CompletedTask;
        }

        /// <summary>
        /// Метод для обновления существующего тура в списке по его идентификатору
        /// </summary>
        public Task Update(TourModel tour, CancellationToken cancellationToken = default)
        {
            var existingTour = items.FirstOrDefault(t => t.Id == tour.Id);

            if (existingTour != null)
            {
                existingTour.Direction = tour.Direction;
                existingTour.DepartureDate = tour.DepartureDate;
                existingTour.NumberNights = tour.NumberNights;
                existingTour.CostPerVacationer = tour.CostPerVacationer;
                existingTour.NumberVacationers = tour.NumberVacationers;
                existingTour.AvailabilityWiFi = tour.AvailabilityWiFi;
                existingTour.Surcharges = tour.Surcharges;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Метод для удаления тура из списка по его идентификатору
        /// </summary>
        public Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var tour = items.FirstOrDefault(t => t.Id == id);

            if (tour != null)
            {
                items.Remove(tour);
            }

            return Task.CompletedTask;

        }

        /// <summary>
        /// Возвращает общее количество туров
        /// </summary>
        public Task<int> GetTotalToursCount(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(items.Count);
        }

        /// <summary>
        /// Возвращает общую стоимость всех туров (включая доплаты)
        /// </summary>
        public Task<decimal> GetTotalCostAllTours(CancellationToken cancellationToken = default)
        {
            var total = items.Sum(t => t.CostPerVacationer * t.NumberVacationers + t.Surcharges);

            return Task.FromResult(total);
        }

        /// <summary>
        /// Возвращает количество туров, у которых есть доплаты
        /// </summary>
        public Task<int> GetToursWithSurchargesCount(CancellationToken cancellationToken = default)
        {
            var count = items.Count(t => t.Surcharges > 0);

            return Task.FromResult(count);
        }

        /// <summary>
        /// Возвращает общую сумму всех доплат по всем турам
        /// </summary>
        public Task<decimal> GetTotalSurcharges(CancellationToken cancellationToken = default)
        {
            var total = items.Sum(t => t.Surcharges);

            return Task.FromResult(total);
        }
    }
}