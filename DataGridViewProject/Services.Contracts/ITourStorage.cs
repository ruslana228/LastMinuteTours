using Entities.Models;

namespace MemoryStorage.Contracts
{
    /// <summary>
    ///  Интерфейс, определяющий контракт для сервиса управления турами
    /// </summary>
    public interface ITourStorage
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
    }
}