using Entities.Models;
using MemoryStorage.Contracts;

namespace MemoryStorage
{
    /// <summary>
    /// Класс, который предоставляет методы для добавления, обновления, удаления и получения туров, а также для подсчёта статистики.
    /// </summary>
    public class InMemoryTourStorage : ITourStorage
    {
        private readonly List<TourModel> items;

        /// <summary>
        /// Инициализация нового экземпляра
        /// </summary>
        public InMemoryTourStorage()
        {
            items = new List<TourModel>();
        }

        /// <summary>
        /// Возврат списка всех туров
        /// </summary>
        public async Task<IReadOnlyCollection<TourModel>> GetAll(CancellationToken cancellationToken) 
            => await Task.FromResult<IReadOnlyCollection<TourModel>>(items.AsReadOnly());

        /// <summary>
        /// Возврат тура по его идентификатору
        /// </summary>
        public async Task<TourModel?> GetById(Guid id, CancellationToken cancellationToken) 
            => await Task.FromResult(items.FirstOrDefault(t => t.Id == id));

        /// <summary>
        /// Добавление нового тура в список
        /// </summary>
        public async Task Add(TourModel tour, CancellationToken cancellationToken)
        {
            items.Add(tour);
            await Task.CompletedTask;
        }

        /// <summary>
        /// Метод для обновления существующего тура в списке по его идентификатору
        /// </summary>
        public async Task Update(TourModel tour, CancellationToken cancellationToken)
        {
            var existingTour = items.FirstOrDefault(t => t.Id == tour.Id);

            if (existingTour == null)
            {
                return;
            }

            existingTour.Direction = tour.Direction;
            existingTour.DepartureDate = tour.DepartureDate;
            existingTour.NumberNights = tour.NumberNights;
            existingTour.CostPerVacationer = tour.CostPerVacationer;
            existingTour.NumberVacationers = tour.NumberVacationers;
            existingTour.AvailabilityWiFi = tour.AvailabilityWiFi;
            existingTour.Surcharges = tour.Surcharges;

            await Task.CompletedTask;
        }

        /// <summary>
        /// Метод для удаления тура из списка по его идентификатору
        /// </summary>
        public async Task Delete(Guid id, CancellationToken cancellationToken)
        {
            var existingTour = items.FirstOrDefault(t => t.Id == id);

            if (existingTour == null)
            {
                return;
            }

            items.Remove(existingTour);

            await Task.CompletedTask;
        }
    }
}