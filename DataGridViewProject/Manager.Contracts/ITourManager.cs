using Entities.Models;

namespace Manager.Contracts
{
    public interface ITourManager
    {
        /// <summary>
        /// Возвращает список всех туров
        /// </summary>
        Task<IReadOnlyCollection<TourModel>> GetAll(CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает тур по его идентификатору
        /// </summary>
        Task<TourModel?> GetById(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Добавляет новый тур
        /// </summary>
        Task Add(TourModel tour, CancellationToken cancellationToken = default);

        /// <summary>
        /// Обновляет существующий тур
        /// </summary>
        Task Update(TourModel tour, CancellationToken cancellationToken = default);

        /// <summary>
        /// Удаляет тур по его идентификатору
        /// </summary>
        Task Delete(Guid id, CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает общее количество туров
        /// </summary>
        Task<int> GetTotalToursCount(CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает общую стоимость всех туров (включая доплаты)
        /// </summary>
        Task<decimal> GetTotalCostAllTours(CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает количество туров, у которых есть доплаты
        /// </summary>
        Task<int> GetToursWithSurchargesCount(CancellationToken cancellationToken = default);

        /// <summary>
        /// Возвращает общую сумму всех доплат по всем турам
        /// </summary>
        Task<decimal> GetTotalSurcharges(CancellationToken cancellationToken = default);
    }
}
