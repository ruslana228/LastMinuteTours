using Entities.Models;
using Manager.Contracts;

namespace DataGridViewProject.Web.Models
{
    /// <summary>
    /// Модель главной страницы управления турами
    /// </summary>
    public class IndexViewModel
    {
        /// <summary>
        /// Таблица с данными о турах
        /// </summary>
        public List<TourModel> Tours { get; set; } = [];

        /// <summary>
        /// Статистика по турам
        /// </summary>
        public TourStatistics Statistics { get; set; } = new();
    }
}
