using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LastMinuteTours.Models
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
        public Direction Direction { get; set; }

        /// <summary>
        /// Дата вылета
        /// </summary>
        public DateOnly DepartureDate { get; set; }

        /// <summary>
        /// Количество ночей
        /// </summary>
        public int NumberNights { get; set; }

        /// <summary>
        /// Стоимость за отдыхающего (руб)
        /// </summary>
        public decimal CostPerVacationer { get; set; }

        /// <summary>
        /// Количество отдыхающих
        /// </summary>
        public int NumberVacationers { get; set; }

        /// <summary>
        /// Наличие Wi-Fi
        /// </summary>
        public bool AvailabilityWiFi { get; set; }

        /// <summary>
        /// Доплаты (руб)
        /// </summary>
        public decimal Surcharges { get; set; }

        /// <summary>
        /// Общая стоимость
        /// </summary>
        public decimal TotalCost 
        { 
            get { return (CostPerVacationer * NumberVacationers) + Surcharges; }
        }
    }
}
