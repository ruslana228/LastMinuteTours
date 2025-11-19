using Entities.Models;
using Services.Contracts;

namespace Services
{
    /// <summary>
    /// Класс, который предоставляет методы для добавления, обновления, удаления и получения туров, а также для подсчёта статистики.
    /// </summary>
    public class InMemoryTourService : ITourService
    {
        private readonly List<TourModel> _items;

        /// <summary>
        /// Инициализация нового экземпляра, загрузка начальных данных
        /// </summary>
        public InMemoryTourService()
        {
            _items = new List<TourModel>();
            InitializeData();
        }

        private void InitializeData()
        {
            _items.Add(new TourModel
            {
                Id = Guid.NewGuid(),
                Direction = Direction.Turkey,
                DepartureDate = DateOnly.Parse("20.10.2025"),
                NumberNights = 5,
                CostPerVacationer = 45000.00m,
                NumberVacationers = 2,
                AvailabilityWiFi = true,
                Surcharges = 0.00m,
            });

            _items.Add(new TourModel
            {
                Id = Guid.NewGuid(),
                Direction = Direction.Spain,
                DepartureDate = DateOnly.Parse("15.11.2025"),
                NumberNights = 7,
                CostPerVacationer = 68000.00m,
                NumberVacationers = 2,
                AvailabilityWiFi = true,
                Surcharges = 3200.00m,
            });

            _items.Add(new TourModel
            {
                Id = Guid.NewGuid(),
                Direction = Direction.Italy,
                DepartureDate = DateOnly.Parse("05.12.2025"),
                NumberNights = 6,
                CostPerVacationer = 72000.00m,
                NumberVacationers = 3,
                AvailabilityWiFi = true,
                Surcharges = 4100.50m,
            });

            _items.Add(new TourModel
            {
                Id = Guid.NewGuid(),
                Direction = Direction.France,
                DepartureDate = DateOnly.Parse("12.01.2026"),
                NumberNights = 8,
                CostPerVacationer = 89000.00m,
                NumberVacationers = 2,
                AvailabilityWiFi = false,
                Surcharges = 0.00m,
            });

            _items.Add(new TourModel
            {
                Id = Guid.NewGuid(),
                Direction = Direction.Shushary,
                DepartureDate = DateOnly.Parse("25.10.2025"),
                NumberNights = 2,
                CostPerVacationer = 5000.00m,
                NumberVacationers = 4,
                AvailabilityWiFi = false,
                Surcharges = 500.00m,
            });
        }

        /// <summary>
        /// Возврат списка всех туров
        /// </summary>
        public List<TourModel> GetAll() => new List<TourModel>(_items);

        /// <summary>
        /// Возврат тура по его идентификатору
        /// </summary>
        public TourModel GetById(Guid id) => _items.FirstOrDefault(t => t.Id == id);

        /// <summary>
        /// Добавление нового тура в список
        /// </summary>
        public void Add(TourModel tour) => _items.Add(tour);

        /// <summary>
        /// Метод для обновления существующего тура в списке по его идентификатору
        /// </summary>
        public void Update(TourModel tour)
        {
            var existingTour = _items.FirstOrDefault(t => t.Id == tour.Id);
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
        }

        /// <summary>
        /// Метод для удаления тура из списка по его идентификатору
        /// </summary>
        public void Delete(Guid id)
        {
            var tour = _items.FirstOrDefault(t => t.Id == id);
            if (tour != null)
                _items.Remove(tour);
        }

        /// <summary>
        /// Возвращает общее количество туров
        /// </summary>
        public int GetTotalToursCount() => _items.Count;

        /// <summary>
        /// Возвращает общую стоимость всех туров (включая доплаты)
        /// </summary>
        public decimal GetTotalCostAllTours() => _items.Sum(t => (t.CostPerVacationer * t.NumberVacationers) + t.Surcharges);

        /// <summary>
        /// Возвращает количество туров, у которых есть доплаты
        /// </summary>
        public int GetToursWithSurchargesCount() => _items.Count(t => t.Surcharges > 0);

        /// <summary>
        /// Возвращает общую сумму всех доплат по всем турам
        /// </summary>
        public decimal GetTotalSurcharges() => _items.Sum(t => t.Surcharges);
    }
}