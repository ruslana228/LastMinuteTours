namespace DataGridViewProject.Web.Models
{
    /// <summary>
    /// Модель для отображения информации об ошибке
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Уникальный идентификатор запроса, в котором произошла ошибка
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Указывает, следует ли отображать идентификатор запроса
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
