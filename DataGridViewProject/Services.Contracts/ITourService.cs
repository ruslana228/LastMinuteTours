using Entities.Models;

namespace Services.Contracts
{
    /// <summary>
    ///  Интерфейс, определяющий контракт для сервиса управления турами
    /// </summary>
    public interface ITourService
    {
        /// <summary>
        /// Возвращает список всех туров
        /// </summary>
        List<TourModel> GetAll();

        /// <summary>
        /// Возвращает тур по его идентификатору
        /// </summary>
        TourModel GetById(Guid id);

        /// <summary>
        /// Добавляет новый тур
        /// </summary>
        void Add(TourModel tour);

        /// <summary>
        /// Обновляет существующий тур
        /// </summary>
        void Update(TourModel tour);

        /// <summary>
        /// Удаляет тур по его идентификатору
        /// </summary>
        void Delete(Guid id);

        /// <summary>
        /// Возвращает общее количество туров
        /// </summary>
        int GetTotalToursCount();

        /// <summary>
        /// Возвращает общую стоимость всех туров (включая доплаты)
        /// </summary>
        decimal GetTotalCostAllTours();

        /// <summary>
        /// Возвращает количество туров, у которых есть доплаты
        /// </summary>
        int GetToursWithSurchargesCount();

        /// <summary>
        /// Возвращает общую сумму всех доплат по всем турам
        /// </summary>
        decimal GetTotalSurcharges();
    }
}