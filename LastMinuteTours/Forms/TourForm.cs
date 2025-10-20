using System.Windows.Forms;
using LastMinuteTours.Infrostructure;
using LastMinuteTours.Models;

namespace LastMinuteTours.Forms
{
    public partial class TourForm : Form
    {
        private readonly TourModel targetTour;
        public TourForm(TourModel? sourceTour = null)
        {
            InitializeComponent();
            if (sourceTour != null)
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
            else
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
            comboBoxDirection.DataSource = Enum.GetValues(typeof(Direction));
            var dateTimePickerBinding = new Binding("Value", targetTour, "DepartureDate");
            dateTimePickerBinding.Format += new ConvertEventHandler(DateOnlyToDateTime!);
            dateTimePickerBinding.Parse += new ConvertEventHandler(DateTimeTodateOnly!);
            dateTimePickerDepartureDate.DataBindings.Add(dateTimePickerBinding);

            comboBoxDirection.AddBinding(x => x.SelectedItem, targetTour, x => x.Direction);
            numericUpDownNumberNights.AddBinding(x => x.Value, targetTour, x => x.NumberNights);
            textBoxCostPerVacationer.AddBinding(x => x.Text, targetTour, x => x.CostPerVacationer);
            numericUpDownNumberVacationers.AddBinding(x => x.Value, targetTour, x => x.NumberVacationers);
            textBoxSurcharges.AddBinding(x => x.Text, targetTour, x => x.Surcharges);
            checkBoxAvailabilityWiFiYes.AddBinding(x => x.Checked, targetTour, x => x.AvailabilityWiFi);
        }

        private void DateOnlyToDateTime(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(DateTime) && e.Value is DateOnly)
            {
                var dateOnly = (DateOnly)e.Value;
                e.Value = new DateTime(dateOnly.Year, dateOnly.Month, dateOnly.Day);
            }
        }

        private void DateTimeTodateOnly(object sender, ConvertEventArgs e)
        {
            if (e.DesiredType == typeof(DateOnly) && e.Value is DateTime)
            {
                e.Value = DateOnly.FromDateTime((DateTime)e.Value);
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            // Устанавливаем результат DialogResult.OK
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // Закрываем форму с результатом Cancel
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public TourModel CurrentTour => targetTour;
    }
}
