using System.ComponentModel.DataAnnotations;
using Constants;
using Entities.Models;

namespace DataGridViewProject.Web.Models
{
    /// <summary>
    /// Модель тура для добавления/редактирования
    /// </summary>
    public class TourViewModel
    {
        /// <summary>
        /// Идентификатор тура
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <inheritdoc cref="Entities.Models.Direction"/>
        [Display(Name = "Направление тура")]
        [Required(ErrorMessage = "{0} обязательно для заполнения")]
        public Direction Direction { get; set; }

        /// <summary>
        /// Дата вылета
        /// </summary>
        [Display(Name = "Дата вылета")]
        [Required(ErrorMessage = "{0} обязательна для заполнения")]
        [DataType(DataType.Date)]
        public DateOnly DepartureDate { get; set; }

        /// <summary>
        /// Количество ночей
        /// </summary>
        [Display(Name = "Количество ночей")]
        [Required(ErrorMessage = "{0} обязательно для заполнения")]
        [Range(TourConstants.NumberNightsMin, TourConstants.NumberNightsMax,
               ErrorMessage = "{0} должно быть от {1} до {2}")]
        public int NumberNights { get; set; }

        /// <summary>
        /// Стоимость за отдыхающего (руб)
        /// </summary>
        [Display(Name = "Стоимость за отдыхающего")]
        [Required(ErrorMessage = "{0} обязательна для заполнения")]
        [Range(TourConstants.CostPerVacationerMin, TourConstants.CostPerVacationerMax,
               ErrorMessage = "{0} должна быть от {1} до {2} руб.")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal CostPerVacationer { get; set; }

        /// <summary>
        /// Количество отдыхающих
        /// </summary>
        [Display(Name = "Количество отдыхающих")]
        [Required(ErrorMessage = "{0} обязательно для заполнения")]
        [Range(TourConstants.NumberVacationersMin, TourConstants.NumberVacationersMax,
               ErrorMessage = "{0} должно быть от {1} до {2}")]
        public int NumberVacationers { get; set; }

        /// <summary>
        /// Наличие Wi-Fi
        /// </summary>
        [Display(Name = "Наличие Wi-Fi")]
        public bool AvailabilityWiFi { get; set; }

        /// <summary>
        /// Доплаты (руб)
        /// </summary>
        [Display(Name = "Доплаты")]
        [Range(TourConstants.SurchargesMin, TourConstants.SurchargesMax,
               ErrorMessage = "{0} должны быть от {1} до {2} руб.")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = true)]
        public decimal Surcharges { get; set; } = 0.00m;

        /// <summary>
        /// Общая стоимость
        /// </summary>
        [Display(Name = "Общая стоимость")]
        [DataType(DataType.Currency)]
        [DisplayFormat(DataFormatString = "{0:N2}")]
        public decimal TotalCost => (CostPerVacationer * NumberVacationers) + Surcharges;
    }
}