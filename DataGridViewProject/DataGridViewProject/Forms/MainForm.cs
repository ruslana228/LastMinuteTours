using Entities.Models;
using Manager.Contracts;

namespace DataGridViewProject.Forms
{
    /// <summary>
    /// Главная форма приложения для управления турами
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly ITourManager tourManager; // Сервис для работы с турами
        private readonly BindingSource bindingSource = new(); // Компонент для привязки данных между коллекцией и DataGridView

        /// <summary>
        /// Конструктор главной формы
        /// </summary>
        public MainForm(ITourManager tourManager)
        {
            // Инициализация сервиса
            //var storage = new InMemoryTourStorage();
            //tourManager = new TourManager(storage);

            this.tourManager = tourManager;
            InitializeComponent();

            dataGridViewTours.AutoGenerateColumns = false; // Отключение автоматического создания колонок
        }
        
        private async void MainForm_Load(object sender, EventArgs e)
        {
            // Загрузка начальных данных
            await InitializeDataAsync();

            // Настройка привязки данных после загрузки
            var tours = await tourManager.GetAll(CancellationToken.None);
            bindingSource.DataSource = tours;
            dataGridViewTours.DataSource = bindingSource; // Связывание BindingSource с DataGridView для отображения данных

            SetStatistics(); // Обновление статистики
        }

        // Асинхронная инициализация начальных данных
        private async Task InitializeDataAsync()
        {
            var existingTours = await tourManager.GetAll(CancellationToken.None);

            if (existingTours.Count > 0)
            {
                return; // Данные уже есть, не добавляем снова
            }

            var tours = new List<TourModel>
            {
                new TourModel
                {
                    Id = Guid.NewGuid(),
                    Direction = Direction.Turkey,
                    DepartureDate = DateOnly.Parse("20.10.2025"),
                    NumberNights = 5,
                    CostPerVacationer = 45000.00m,
                    NumberVacationers = 2,
                    AvailabilityWiFi = true,
                    Surcharges = 0.00m,
                },
                new TourModel
                {
                    Id = Guid.NewGuid(),
                    Direction = Direction.Spain,
                    DepartureDate = DateOnly.Parse("15.11.2025"),
                    NumberNights = 7,
                    CostPerVacationer = 68000.00m,
                    NumberVacationers = 2,
                    AvailabilityWiFi = true,
                    Surcharges = 3200.00m,
                },
                new TourModel
                {
                    Id = Guid.NewGuid(),
                    Direction = Direction.Italy,
                    DepartureDate = DateOnly.Parse("05.12.2025"),
                    NumberNights = 6,
                    CostPerVacationer = 72000.00m,
                    NumberVacationers = 3,
                    AvailabilityWiFi = true,
                    Surcharges = 4100.50m,
                },
                new TourModel
                {
                    Id = Guid.NewGuid(),
                    Direction = Direction.France,
                    DepartureDate = DateOnly.Parse("12.01.2026"),
                    NumberNights = 8,
                    CostPerVacationer = 89000.00m,
                    NumberVacationers = 2,
                    AvailabilityWiFi = false,
                    Surcharges = 0.00m,
                },
                new TourModel
                {
                    Id = Guid.NewGuid(),
                    Direction = Direction.Shushary,
                    DepartureDate = DateOnly.Parse("25.10.2025"),
                    NumberNights = 2,
                    CostPerVacationer = 5000.00m,
                    NumberVacationers = 4,
                    AvailabilityWiFi = false,
                    Surcharges = 500.00m,
                }
            };

            foreach (var tour in tours)
            {
                await tourManager.Add(tour, CancellationToken.None);
            }
        }

        // Обработчик события форматирования ячеек DataGridView
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

        // Метод для вычисления и отображения общих показателей по всем турам
        private async void SetStatistics()
        {
            var statistics = await tourManager.GetStatistics(CancellationToken.None);

            toolStrpLblTotalTours.Text = $"Общее кол-во туров: {statistics.TotalToursCount}";
            toolStrpLblTotalCost.Text = $"Общая сумма за все туры: {statistics.TotalCostAllTours:N2} руб.";
            toolStrpLblToursWithSurcharges.Text = $"Кол-во туров с доплатами: {statistics.ToursWithSurchargesCount}";
            toolStrpLblTotalSurcharges.Text = $"Общая сумма доплат: {statistics.TotalSurcharges:N2} руб.";
        }

        // Обработчик клика по кнопке "Добавить" - добавление нового тура
        private async void tlStrpBtnAdd_Click(object sender, EventArgs e)
        {
            var addForm = new TourForm(); // Создание новой формы для добавления тура
            if (addForm.ShowDialog(this) == DialogResult.OK)
            {
                await tourManager.Add(addForm.CurrentTour, CancellationToken.None); // Добавление тура через менеджер

                bindingSource.DataSource = await tourManager.GetAll(CancellationToken.None); // Обновление привязки данных для отображения нового тура в таблице
                bindingSource.ResetBindings(false);
                SetStatistics(); // Обновление статистики с учетом нового тура
            }
        }

        // Обработчик клика по кнопке "Редактировать" - редактирование туров
        private async void tlStrpBtnEdit_Click(object sender, EventArgs e)
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
                await tourManager.Update(editForm.CurrentTour, CancellationToken.None); // Обновление тура через сервис

                bindingSource.DataSource = await tourManager.GetAll(CancellationToken.None); // Обновление привязки данных
                bindingSource.ResetBindings(false);
                SetStatistics(); // Обновление статистики
            }
        }

        // Обработчик клика по кнопке "Удалить" - удаление тура
        private async void tlStrpBtnDelete_Click(object sender, EventArgs e)
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
                await tourManager.Delete(tour.Id, CancellationToken.None); // Удаление тура через сервис

                bindingSource.DataSource = await tourManager.GetAll(CancellationToken.None); // Обновление привязки данных
                bindingSource.ResetBindings(false);
                SetStatistics(); // Обновление статистики
            }
        }
    }
}