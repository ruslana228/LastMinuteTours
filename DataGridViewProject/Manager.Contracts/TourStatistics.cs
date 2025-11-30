namespace Manager.Contracts
{
    /// <summary>
    /// Модель статистики по турам
    /// </summary>
    public class TourStatistics
    {
        /// <summary>
        /// Общее количество туров
        /// </summary>
        public int TotalToursCount { get; set; }

        /// <summary>
        /// Общая стоимость всех туров (включая доплаты)
        /// </summary>
        public decimal TotalCostAllTours { get; set; }

        /// <summary>
        /// Количество туров, у которых есть доплаты
        /// </summary>
        public int ToursWithSurchargesCount { get; set; }

        /// <summary>
        /// Общая сумма всех доплат по всем турам
        /// </summary>
        public decimal TotalSurcharges { get; set; }
    }
}