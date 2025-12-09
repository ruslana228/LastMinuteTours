namespace Constants
{
    /// <summary>
    /// Константы для валидации туров
    /// </summary>
    public static class TourConstants
    {
        /// <summary>
        /// Минимальное количество ночей
        /// </summary>
        public const int NumberNightsMin = 1;

        /// <summary>
        /// Максимальное количество ночей
        /// </summary>
        public const int NumberNightsMax = 30;

        /// <summary>
        /// Минимальная стоимость за отдыхающего
        /// </summary>
        public const double CostPerVacationerMin = 0.01;

        /// <summary>
        /// Максимальная стоимость за отдыхающего
        /// </summary>
        public const double CostPerVacationerMax = 100000.0;

        /// <summary>
        /// Минимальное количество отдыхающих
        /// </summary>
        public const int NumberVacationersMin = 1;

        /// <summary>
        /// Максимальное количество отдыхающих
        /// </summary>
        public const int NumberVacationersMax = 10;

        /// <summary>
        /// Минимальная сумма доплат
        /// </summary>
        public const double SurchargesMin = 0.00;

        /// <summary>
        /// Максимальная сумма доплат
        /// </summary>
        public const double SurchargesMax = 100000.0;
    }
}