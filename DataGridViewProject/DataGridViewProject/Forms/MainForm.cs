using Entities.Models;
using Services;
using Services.Contracts;

namespace DataGridViewProject.Forms
{
    /// <summary>
    /// Главная форма приложения для управления турами
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly ITourService _tourService; // Коллекция туров, используемая в качестве источника данных
        private readonly BindingSource bindingSource = new(); // Компонент для привязки данных между коллекцией и DataGridView

        /// <summary>
        /// Конструктор главной формы
        /// </summary>
        public MainForm()
        {
            // Инициализация сервиса
            _tourService = new InMemoryTourService();

            InitializeComponent();

            dataGridViewTours.AutoGenerateColumns = false; // Отключение автоматического создания колонок
            bindingSource.DataSource = _tourService.GetAll(); // Настройка привязки данных: связываем источник данных с BindingSource
            dataGridViewTours.DataSource = bindingSource; // Связывание BindingSource с DataGridView для отображения данных

            SetStatistics(); // Обновление статистики
        }

        /// <summary>
        /// Обработчик события форматирования ячеек DataGridView
        /// </summary>
        private void dataGridViewTours_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Проверка, что это не заголовок и строка существует
            // Заголовки имеют RowIndex = -1, поэтому их пропускаем
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            { 
                return;
            }

            // Получение ссылок на колонку и строку для которой происходит форматирование
            var col = dataGridViewTours.Columns[e.ColumnIndex];
            var row = dataGridViewTours.Rows[e.RowIndex];

            // Проверка, что строка содержит данные
            if (row.DataBoundItem == null)
            {
                return;
            }

            var tour = (TourModel)row.DataBoundItem; // Получение объекта тура, привязанного к текущей строке

            // Форматирование колонки "Направление" - преобразование enum в читаемый текст
            if (col.DataPropertyName == nameof(TourModel.Direction))
            {
                switch (tour.Direction)
                {
                    case Direction.Turkey:
                        e.Value = "Турция";
                        break;
                    case Direction.Spain:
                        e.Value = "Испания";
                        break;
                    case Direction.Italy:
                        e.Value = "Италия";
                        break;
                    case Direction.France:
                        e.Value = "Франция";
                        break;
                    case Direction.Shushary:
                        e.Value = "Шушары";
                        break;
                    default:
                        e.Value = string.Empty;
                        break;
                }
            }

            // Форматирование колонки "Наличие Wi-Fi" - преобразование bool в "Да"/"Нет"
            if (col.DataPropertyName == nameof(TourModel.AvailabilityWiFi))
            {
                e.Value = tour.AvailabilityWiFi
                    ? "Да" // Если Wi-Fi есть
                    : "Нет"; // Если Wi-Fi нет
            }

            if (col.Name == "DGTotalCost")
            {
                var totalCost = (tour.CostPerVacationer * tour.NumberVacationers) + tour.Surcharges;
                e.Value = totalCost.ToString("N2");
            }
        }


        /// <summary>
        /// Метод для вычисления и отображения общих показателей по всем турам
        /// </summary>
        private void SetStatistics()
        {
            // Вычисление общей суммы за все туры
            var totalCostAllTours = _tourService.GetTotalCostAllTours();

            toolStrpLblTotalTours.Text = $"Общее кол-во туров: {_tourService.GetTotalToursCount()}";
            toolStrpLblTotalCost.Text = $"Общая сумма за все туры: {totalCostAllTours} руб.";
            toolStrpLblToursWithSurcharges.Text = $"Кол-во туров с доплатами: {_tourService.GetToursWithSurchargesCount()}";
            toolStrpLblTotalSurcharges.Text = $"Общая сумма доплат: {_tourService.GetTotalSurcharges()}";
        }

        /// <summary>
        /// Обработчик клика по кнопке "Добавить" - добавление нового тура
        /// </summary>
        private void tlStrpBtnAdd_Click(object sender, EventArgs e)
        {
            var addForm = new TourForm(); // Создание новой формы для добавления тура

            if (addForm.ShowDialog(this) == DialogResult.OK)
            {
                _tourService.Add(addForm.CurrentTour); // Добавление тура в коллекцию

                bindingSource.ResetBindings(false); // Обновление привязки данных для отображения нового тура в таблице
                SetStatistics(); // Обновление статистики с учетом нового тура
            }
        }

        /// <summary>
        /// Обработчик клика по кнопке "Редактировать" - редактирование туров
        /// </summary>
        private void tlStrpBtnEdit_Click(object sender, EventArgs e)
        {
            // Проверка, что пользователь выбрал строку для редактирования
            if (dataGridViewTours.SelectedRows.Count == 0)
            {
                return;
            }

            var tour = (TourModel)dataGridViewTours.SelectedRows[0].DataBoundItem; // Получение выбранного тура из привязанных данных выбранной строки

            var editForm = new TourForm(tour);
            if (editForm.ShowDialog(this) == DialogResult.OK)
            {
                _tourService.Update(editForm.CurrentTour); // Обновление тура через сервис

                bindingSource.ResetBindings(false); // Обновление привязки данных
                SetStatistics(); // Обновление статистики
            }
        }
        

        /// <summary>
        /// Обработчик клика по кнопке "Удалить" - удаление тура
        /// </summary>
        private void tlStrpBtnDelete_Click(object sender, EventArgs e)
        {
            // Проверка, что пользователь выбрал строку для редактирования
            if (dataGridViewTours.SelectedRows.Count == 0)
            {
                return;
            }

            var tour = (TourModel)dataGridViewTours.SelectedRows[0].DataBoundItem; // Получение выбранного тура из привязанных данных выбранной строки

            // Проверка, что тур найден и получение подтверждение удаления
            if (MessageBox.Show($"Удалить тур '{tour.Direction}'?",
                    "Удаление тура",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _tourService.Delete(tour.Id); // Удаление тура через сервис

                bindingSource.ResetBindings(false); // Обновление привязки данных
                SetStatistics(); // Обновление статистики
            }
        }
    }
}
