using Entities.Models;
using Manager.Contracts;
using MemoryStorage.Contracts;
using Serilog;
using System.Diagnostics;


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
        public async Task<IReadOnlyCollection<TourModel>> GetAll(CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var result = await Storage.GetAll(cancellationToken);
                return result;
            }
            finally
            {
                sw.Stop();
                var ms = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                Log.Debug("TourManager.GetAll выполнен за {ElapsedMs:F6} мс", ms);
            }
        }


        /// <summary>
        /// Возврат тура по его идентификатору
        /// </summary>
        public async Task<TourModel?> GetById(Guid id, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                var result = await Storage.GetById(id, cancellationToken);
                return result;
            }
            finally
            {
                sw.Stop();
                var ms = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                Log.Debug("TourManager.GetById выполнен за {ElapsedMs:F6} мс", ms);
            }
        }


        /// <summary>
        /// Добавление нового тура в список
        /// </summary>
        public async Task Add(TourModel tour, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await Storage.Add(tour, cancellationToken);
            }
            finally
            {
                sw.Stop();
                var ms = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                Log.Debug("TourManager.Add выполнен за {ElapsedMs:F6} мс", ms);
            }
        }


        /// <summary>
        /// Метод для обновления существующего тура в списке по его идентификатору
        /// </summary>
        public async Task Update(TourModel tour, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await Storage.Update(tour, cancellationToken);
            }
            finally
            {
                sw.Stop();
                var ms = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                Log.Debug("TourManager.Update выполнен за {ElapsedMs:F6} мс", ms);
            }
        }


        /// <summary>
        /// Метод для удаления тура из списка по его идентификатору
        /// </summary>
        public async Task Delete(Guid id, CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                await Storage.Delete(id, cancellationToken);
            }
            finally
            {
                sw.Stop();
                var ms = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                Log.Debug("TourManager.Delete выполнен за {ElapsedMs:F6} мс", ms);
            }
        }


        /// <summary>
        /// Возвращает статистику по всем турам
        /// </summary>
        public async Task<TourStatistics> GetStatistics(CancellationToken cancellationToken = default)
        {
            var sw = Stopwatch.StartNew();
            try
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
            finally
            {
                sw.Stop();
                var ms = sw.ElapsedTicks * 1000.0 / Stopwatch.Frequency;
                Log.Debug("TourManager.GetStatistics выполнен за {ElapsedMs:F6} мс", ms);
            }
        }
    }
}
