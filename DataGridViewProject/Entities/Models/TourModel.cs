using System.ComponentModel.DataAnnotations;
using Constants;

namespace Entities.Models
{
    /// <summary>
    /// Модель тура
    /// </summary>
    public class TourModel
    {
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
        [Range(TourConstants.NumberNightsMin, TourConstants.NumberNightsMax, ErrorMessage = "Кол-во ночей должно быть от 1 до 30")]
        public int NumberNights { get; set; }

        /// <summary>
        /// Стоимость за отдыхающего (руб)
        /// </summary>
        [Range(TourConstants.CostPerVacationerMin, TourConstants.CostPerVacationerMax, ErrorMessage = "Стоимость должна быть больше 0, но меньше 100000 руб.")]
        public decimal CostPerVacationer { get; set; }

        /// <summary>
        /// Количество отдыхающих
        /// </summary>
        [Range(TourConstants.NumberVacationersMin, TourConstants.NumberVacationersMax, ErrorMessage = "Кол-во отдыхающих должно быть от 1 до 10")]
        public int NumberVacationers { get; set; }

        /// <summary>
        /// Наличие Wi-Fi
        /// </summary>
        public bool AvailabilityWiFi { get; set; }

        /// <summary>
        /// Доплаты (руб)
        /// </summary>
        [Range(TourConstants.SurchargesMin, TourConstants.SurchargesMax, ErrorMessage = "Доплаты должны быть в диапазоне от 0 до 100000")]
        public decimal Surcharges { get; set; }
    }
}
