using Entities.Models;
using Manager.Contracts;
using MemoryStorage.Contracts;

namespace Manager
{
    public class TourManager(MemoryStorage.Contracts.ITourManager storage): Contracts.ITourManager
    {
        private MemoryStorage.Contracts.ITourManager Storage { get; } = storage;

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
        /// Возвращает общее количество туров
        /// </summary>
        public Task<int> GetTotalToursCount(CancellationToken cancellationToken = default) => Storage.GetTotalToursCount();
        

        /// <summary>
        /// Возвращает общую стоимость всех туров (включая доплаты)
        /// </summary>
        public Task<decimal> GetTotalCostAllTours(CancellationToken cancellationToken = default) => Storage.GetTotalCostAllTours();
        

        /// <summary>
        /// Возвращает количество туров, у которых есть доплаты
        /// </summary>
        public Task<int> GetToursWithSurchargesCount(CancellationToken cancellationToken = default) => Storage.GetToursWithSurchargesCount();
        

        /// <summary>
        /// Возвращает общую сумму всех доплат по всем турам
        /// </summary>
        public Task<decimal> GetTotalSurcharges(CancellationToken cancellationToken = default) => Storage.GetTotalSurcharges();
    }
}
