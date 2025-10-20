using System.Windows.Forms;
using LastMinuteTours.Infrostructure;
using LastMinuteTours.Models;

namespace LastMinuteTours.Forms
{
    /// <summary>
    /// Форма для добавления и редактирования информации о туре
    /// </summary>
    public partial class TourForm : Form
    {
        /// <summary>
        /// Объект тура, с которым работает форма
        /// </summary>
        private readonly TourModel targetTour;

        /// <summary>
        /// Конструктор формы тура
        /// </summary>
        public TourForm(TourModel? sourceTour = null)
        {
            InitializeComponent();

            if (sourceTour != null) // Режим редактирования
            {
                targetTour = new TourModel
                {
                    Id = sourceTour.Id,
                    Direction = sourceTour.Direction,
                    DepartureDate = sourceTour.DepartureDate,
                    NumberNights = sourceTour.NumberNights,
                    CostPerVacationer = sourceTour.CostPerVacationer,
                    NumberVacationers = sourceTour.NumberVacationers,
                    AvailabilityWiFi = sourceTour.AvailabilityWiFi,
                    Surcharges = sourceTour.Surcharges,
                };
                this.Text = "Редактирование тура";
                buttonSave.Text = "Сохранить";
            }
            else // Режим добавления
            {
                targetTour = new TourModel
                {
                    Id = Guid.NewGuid(),
                    Direction = Direction.Unknown,
                    DepartureDate = DateOnly.FromDateTime(DateTime.Now),
                    NumberNights = 0,
                    CostPerVacationer = 0.00m,
                    NumberVacationers = 0,
                    AvailabilityWiFi = false,
                    Surcharges = 0.00m,
                };
                this.Text = "Добавление тура";
                buttonSave.Text = "Добавить";
            }

            // Настройка эл-тов управления и привязки данных
            comboBoxDirection.DataSource = Enum.GetValues(typeof(Direction));
            var dateTimePickerBinding = new Binding("Value", targetTour, "DepartureDate");
            dateTimePickerBinding.Format += new ConvertEventHandler(DateOnlyToDateTime!);
            dateTimePickerBinding.Parse += new ConvertEventHandler(DateTimeTodateOnly!);
            dateTimePickerDepartureDate.DataBindings.Add(dateTimePickerBinding);

            // Привязка с errorProvider
            comboBoxDirection.AddBinding(x => x.SelectedItem, targetTour, x => x.Direction);
            numericUpDownNumberNights.AddBinding(x => x.Value, targetTour, x => x.NumberNights, errorProvider1);
            textBoxCostPerVacationer.AddBinding(x => x.Text, targetTour, x => x.CostPerVacationer, errorProvider1);
            numericUpDownNumberVacationers.AddBinding(x => x.Value, targetTour, x => x.NumberVacationers, errorProvider1);
            textBoxSurcharges.AddBinding(x => x.Text, targetTour, x => x.Surcharges, errorProvider1);
            checkBoxAvailabilityWiFiYes.AddBinding(x => x.Checked, targetTour, x => x.AvailabilityWiFi);

            // Обработчик изменения направления для валидации
            comboBoxDirection.SelectedIndexChanged += (s, e) => ValidateForm();

            // Изначально блокируем кнопку сохранения
            UpdateSaveButtonState();
        }

        /// <summary>
        /// Метод для преобразования DateOnly в DateTime для отображения в DateTimePicker
        /// </summary>
        private void DateOnlyToDateTime(object sender, ConvertEventArgs e)
        {
            // Проверка, что целевой тип - DateTime и исходное значение - DateOnly
            if (e.DesiredType == typeof(DateTime) && e.Value is DateOnly)
            {
                var dateOnly = (DateOnly)e.Value;
                e.Value = new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day); // Преобразуем DateOnly в DateTime с временем 00:00:00
            }
        }

        /// <summary>
        /// Метод для преобразования DateTime в DateOnly для сохранения в модели
        /// </summary>
        private void DateTimeTodateOnly(object sender, ConvertEventArgs e)
        {
            // Проверка, что целевой тип - DateOnly и исходное значение - DateTime
            if (e.DesiredType == typeof(DateOnly) && e.Value is DateTime)
            {
                e.Value = DateOnly.FromDateTime((DateTime)e.Value); // Преобразуем DateTime в DateOnly, отбрасывая время
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Сохранить/Добавить"
        /// </summary>
        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Принудительно вызываем валидацию всех контролов
            ValidateChildren(ValidationConstraints.Enabled);

            // Проверка валидности всей модели данных
            if (!targetTour.IsValid())
            {
                MessageBox.Show("Исправьте ошибки в форме перед сохранением.", "Ошибка валидации",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Если все данные валидны, устанавливаем результат OK и закрываем форму
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Отмена"
        /// </summary>
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // Закрываем форму с результатом Cancel
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        /// <summary>
        /// Валидация выбора направления тура
        /// </summary>
        private void comboBoxDirection_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ValidateForm();
        }

        /// <summary>
        /// Валидация даты вылета
        /// </summary>
        private void dateTimePickerDepartureDate_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ValidateForm();
        }

        /// <summary>
        /// Валидация количества ночей
        /// </summary>
        private void numericUpDownNumberNights_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ValidateForm();
        }

        /// <summary>
        /// Валидация стоимости за отдыхающего
        /// </summary>
        private void textBoxCostPerVacationer_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ValidateForm();
        }

        /// <summary>
        /// Валидация количества отдыхающих
        /// </summary>
        private void numericUpDownNumberVacationers_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ValidateForm();
        }

        /// <summary>
        /// Валидация суммы доплат
        /// </summary>
        private void textBoxSurcharges_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ValidateForm();
        }

        /// <summary>
        /// Метод для проверки валидности всей формы
        /// </summary>
        private void ValidateForm()
        {
            UpdateSaveButtonState();
        }

        /// <summary>
        /// Обновление состояния кнопки сохранения
        /// </summary>
        private void UpdateSaveButtonState()
        {
            // Проверяем, что все поля валидны
            bool isValid = targetTour.IsValid() &&
                          targetTour.Direction != Direction.Unknown; // Дополнительная проверка для направления

            buttonSave.Enabled = isValid; // Включаем или отключаем кнопку сохранения в зависимости от валидности

            if (!isValid)
            {
                // Дополнительная проверка для направления
                if (targetTour.Direction == Direction.Unknown)
                {
                    errorProvider1.SetError(comboBoxDirection, "Выберите направление тура");
                }
                else
                {
                    errorProvider1.SetError(comboBoxDirection, string.Empty);
                }
            }
        }

        /// <summary>
        /// Свойство для доступа к текущему состоянию тура
        /// </summary>
        public TourModel CurrentTour => targetTour;
    }
}
