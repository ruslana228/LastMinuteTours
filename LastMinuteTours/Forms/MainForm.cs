using LastMinuteTours.Models;

namespace LastMinuteTours.Forms
{
    /// <summary>
    /// Главная форма приложения для управления турами
    /// </summary>
    public partial class MainForm : Form
    {
        private readonly List<TourModel> items; // Коллекция туров, используемая в качестве источника данных

        private readonly BindingSource bindingSource = new(); // Компонент для привязки данных между коллекцией и DataGridView

        /// <summary>
        /// Конструктор главной формы
        /// </summary>
        public MainForm()
        {
            // Инициализация коллекции
            items = new List<TourModel>();

            // Добавление туров
            items.Add(new TourModel
            {
                Id = Guid.NewGuid(),
                Direction = Direction.Turkey,
                DepartureDate = DateOnly.Parse("20.10.2025"),
                NumberNights = 5,
                CostPerVacationer = 45000.00m,
                NumberVacationers = 2,
                AvailabilityWiFi = true,
                Surcharges = 0.00m,
            });

            items.Add(new TourModel
            {
                Id = Guid.NewGuid(),
                Direction = Direction.Spain,
                DepartureDate = DateOnly.Parse("15.11.2025"),
                NumberNights = 7,
                CostPerVacationer = 68000.00m,
                NumberVacationers = 2,
                AvailabilityWiFi = true,
                Surcharges = 3200.00m,
            });

            items.Add(new TourModel
            {
                Id = Guid.NewGuid(),
                Direction = Direction.Italy,
                DepartureDate = DateOnly.Parse("05.12.2025"),
                NumberNights = 6,
                CostPerVacationer = 72000.00m,
                NumberVacationers = 3,
                AvailabilityWiFi = true,
                Surcharges = 4100.50m,
            });

            items.Add(new TourModel
            {
                Id = Guid.NewGuid(),
                Direction = Direction.France,
                DepartureDate = DateOnly.Parse("12.01.2026"),
                NumberNights = 8,
                CostPerVacationer = 89000.00m,
                NumberVacationers = 2,
                AvailabilityWiFi = false,
                Surcharges = 0.00m,
            });

            items.Add(new TourModel
            {
                Id = Guid.NewGuid(),
                Direction = Direction.Shushary,
                DepartureDate = DateOnly.Parse("25.10.2025"),
                NumberNights = 2,
                CostPerVacationer = 5000.00m,
                NumberVacationers = 4,
                AvailabilityWiFi = false,
                Surcharges = 500.00m,
            });

            InitializeComponent();

            dataGridViewTours.AutoGenerateColumns = false; // Отключение автоматического создания колонок

            bindingSource.DataSource = items; // Настройка привязки данных: связываем источник данных с BindingSource
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
            var totalCostAllTours = items.Sum(t => (t.CostPerVacationer * t.NumberVacationers) + t.Surcharges);

            toolStrpLblTotalTours.Text = $"Общее кол-во туров: {items.Count}";
            toolStrpLblTotalCost.Text = $"Общая сумма за все туры: {totalCostAllTours} руб.";
            toolStrpLblToursWithSurcharges.Text = $"Кол-во туров с доплатами: {items.Count(t => t.Surcharges > 0)}";
            toolStrpLblTotalSurcharges.Text = $"Общая сумма доплат: {items.Sum(t => t.Surcharges)}";
        }

        /// <summary>
        /// Обработчик клика по кнопке "Добавить" - добавление нового тура
        /// </summary>
        private void tlStrpBtnAdd_Click(object sender, EventArgs e)
        {
            var addForm = new TourForm(); // Создание новой формы для добавления тура

            if (addForm.ShowDialog(this) == DialogResult.OK)
            {
                items.Add(addForm.CurrentTour); // Добавление тура в коллекцию

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
                var selectedTour = items.FirstOrDefault(x => x.Id == editForm.CurrentTour.Id); // Поиск тура в коллекции по идентификатору
                if (selectedTour != null)
                {
                    // Обновление свойств выбранного тура данными из формы редактирования
                    selectedTour.Direction = editForm.CurrentTour.Direction;
                    selectedTour.DepartureDate = editForm.CurrentTour.DepartureDate;
                    selectedTour.NumberNights = editForm.CurrentTour.NumberNights;
                    selectedTour.CostPerVacationer = editForm.CurrentTour.CostPerVacationer;
                    selectedTour.NumberVacationers = editForm.CurrentTour.NumberVacationers;
                    selectedTour.AvailabilityWiFi = editForm.CurrentTour.AvailabilityWiFi;
                    selectedTour.Surcharges = editForm.CurrentTour.Surcharges;

                    bindingSource.ResetBindings(false); // Обновление привязки данных для отображения нового тура в таблице
                    SetStatistics(); // Обновление статистики с учетом нового тура
                }
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
            var selectedTour = items.FirstOrDefault(x => x.Id == tour.Id); // Поиск тура в коллекции по идентификатору
            
            // Проверка, что тур найден и получение подтверждение удаления
            if (selectedTour != null &&
                MessageBox.Show($"Удалить тур '{tour.Direction}'?",
                "Удаление тура",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                items.Remove(selectedTour); // Удаление тура из коллекции

                bindingSource.ResetBindings(false); // Обновление привязки данных для отображения нового тура в таблице
                SetStatistics(); // Обновление статистики с учетом нового тура
            }
        }
    }
}
