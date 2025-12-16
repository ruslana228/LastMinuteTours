using Entities.Models;

namespace Manager.Contracts
{
    public interface ITourManager
    {
        /// <summary>
        /// Возвращает список всех туров
        /// </summary>
        Task<IReadOnlyCollection<TourModel>> GetAll(CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает тур по его идентификатору
        /// </summary>
        Task<TourModel?> GetById(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Добавляет новый тур
        /// </summary>
        Task Add(TourModel tour, CancellationToken cancellationToken);

        /// <summary>
        /// Обновляет существующий тур
        /// </summary>
        Task Update(TourModel tour, CancellationToken cancellationToken);

        /// <summary>
        /// Удаляет тур по его идентификатору
        /// </summary>
        Task Delete(Guid id, CancellationToken cancellationToken);

        /// <summary>
        /// Возвращает статистику по всем турам
        /// </summary>
        Task<TourStatistics> GetStatistics(CancellationToken cancellationToken);
    }
}
