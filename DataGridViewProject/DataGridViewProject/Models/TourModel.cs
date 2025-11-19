using System.ComponentModel.DataAnnotations;

namespace DataGridViewProject.Models
{
    /// <summary>
    /// Модель тура
    /// </summary>
    public class TourModel
    {
        private const int NumberNightsMin = 1; // Минимальное количество ночей
        private const int NumberNightsMax = 30; // Максимальное количество ночей
        private const double CostPerVacationerMin = 0.01; // Минимальная стоимость за отдыхающего
        private const double CostPerVacationerMax = 100000.0; // Максимальная стоимость за отдыхающего
        private const int NumberVacationersMin = 1; // Минимальное количество отдыхающих
        private const int NumVacationersMax = 10; // Максимальное количество отдыхающих
        private const double SurchargesMin = 0.00; // Минимальная сумма доплат
        private const double SurchargesMax = 100000.0; // Максимальная сумма доплат

        /// <summary>
        /// Индентификатор тура
        /// </summary>
        public Guid Id { get; set; }

        /// <inheritdoc cref="Models.Direction"/>
        [Required(ErrorMessage = "Выберите направление тура")]
        public Direction Direction { get; set; }

        /// <summary>
        /// Дата вылета
        /// </summary>
        public DateOnly DepartureDate { get; set; }

        /// <summary>
        /// Количество ночей
        /// </summary>
        [Range(NumberNightsMin, NumberNightsMax, ErrorMessage = "Кол-во ночей должно быть от 1 до 30")]
        public int NumberNights { get; set; }

        /// <summary>
        /// Стоимость за отдыхающего (руб)
        /// </summary>
        [Range(CostPerVacationerMin, CostPerVacationerMax, ErrorMessage = "Стоимость должна быть больше 0, но меньше 100000 руб.")]
        public decimal CostPerVacationer { get; set; }

        /// <summary>
        /// Количество отдыхающих
        /// </summary>
        [Range(NumberVacationersMin, NumVacationersMax, ErrorMessage = "Кол-во отдыхающих должно быть от 1 до 10")]
        public int NumberVacationers { get; set; }

        /// <summary>
        /// Наличие Wi-Fi
        /// </summary>
        public bool AvailabilityWiFi { get; set; }

        /// <summary>
        /// Доплаты (руб)
        /// </summary>
        [Range(SurchargesMin, SurchargesMax, ErrorMessage = "Доплаты должны быть в диапазоне от 0 до 100000")]
        public decimal Surcharges { get; set; }
    }
}
