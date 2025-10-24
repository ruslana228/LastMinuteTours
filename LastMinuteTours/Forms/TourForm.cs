using LastMinuteTours.Infrostructure;
using LastMinuteTours.Models;
using System.ComponentModel.DataAnnotations;

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
            comboBoxDirection.AddBinding(x => x.SelectedItem, targetTour, x => x.Direction, errorProvider1);
            numericUpDownNumberNights.AddBinding(x => x.Value, targetTour, x => x.NumberNights, errorProvider1);
            textBoxCostPerVacationer.AddBinding(x => x.Text, targetTour, x => x.CostPerVacationer, errorProvider1);
            numericUpDownNumberVacationers.AddBinding(x => x.Value, targetTour, x => x.NumberVacationers, errorProvider1);
            textBoxSurcharges.AddBinding(x => x.Text, targetTour, x => x.Surcharges, errorProvider1);
            checkBoxAvailabilityWiFiYes.AddBinding(x => x.Checked, targetTour, x => x.AvailabilityWiFi);
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
            // Очищаем все предыдущие ошибки
            errorProvider1.Clear();

            // Создаем контекст валидации для целевого тура
            var context = new ValidationContext(targetTour);
            var results = new List<ValidationResult>();

            // Выполняем валидацию всего объекта
            var isValid = Validator.TryValidateObject(targetTour, context, results, true);

            // Дополнительная проверка направления (не должно быть Unknown)
            if (targetTour.Direction == Direction.Unknown)
            {
                isValid = false;
                results.Add(new ValidationResult("Выберите направление тура", new[] { nameof(TourModel.Direction) }));
            }

            if (isValid)
            {
                // Если все данные валидны, устанавливаем результат OK и закрываем форму
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                // Если есть ошибки валидации, отображаем их через ErrorProvider
                foreach (var validationResult in results)
                {
                    foreach (var memberName in validationResult.MemberNames)
                    {
                        // Сопоставляем имя свойства с соответствующим контролом
                        Control? control = memberName switch
                        {
                            nameof(TourModel.Direction) => comboBoxDirection,
                            nameof(TourModel.DepartureDate) => dateTimePickerDepartureDate,
                            nameof(TourModel.NumberNights) => numericUpDownNumberNights,
                            nameof(TourModel.CostPerVacationer) => textBoxCostPerVacationer,
                            nameof(TourModel.NumberVacationers) => numericUpDownNumberVacationers,
                            nameof(TourModel.Surcharges) => textBoxSurcharges,
                            _ => null
                        };

                        // Устанавливаем сообщение об ошибке для соответствующего контрола
                        if (control != null)
                        {
                            errorProvider1.SetError(control, validationResult.ErrorMessage);
                        }
                    }
                }

                // Показываем общее сообщение о необходимости исправить ошибки
                MessageBox.Show("Исправьте ошибки в форме перед сохранением.", "Ошибка валидации",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
        /// Свойство для доступа к текущему состоянию тура
        /// </summary>
        public TourModel CurrentTour => targetTour;
    }
}
