using LastMinuteTours.Forms;
using LastMinuteTours.Models;
using System.Windows.Forms;

namespace LastMinuteTours
{
    public partial class MainForm : Form
    {
        private readonly List<TourModel> items;
        private readonly BindingSource bindingSource = new();

        public MainForm()
        {
            items = new List<TourModel>();
            items.Add(new TourModel
            {
                Id = Guid.NewGuid(),
                Direction = Models.Direction.Turkey,
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
                Direction = Models.Direction.Spain,
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
                Direction = Models.Direction.Italy,
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
                Direction = Models.Direction.France,
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
                Direction = Models.Direction.Shushary,
                DepartureDate = DateOnly.Parse("25.10.2025"),
                NumberNights = 2,
                CostPerVacationer = 5000.00m,
                NumberVacationers = 4,
                AvailabilityWiFi = false,
                Surcharges = 500.00m,
            });
            InitializeComponent();
            dataGridViewTours.AutoGenerateColumns = false;

            bindingSource.DataSource = items;
            dataGridViewTours.DataSource = bindingSource;

            SetStatistics();
        }

        private void dataGridViewTours_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Проверяем, что это не заголовок и строка существует
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            var col = dataGridViewTours.Columns[e.ColumnIndex];
            var row = dataGridViewTours.Rows[e.RowIndex];

            // Проверяем, что DataBoundItem не null
            if (row.DataBoundItem == null)
                return;

            var tour = (TourModel)dataGridViewTours.Rows[e.RowIndex].DataBoundItem;

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

            if (col.DataPropertyName == nameof(TourModel.AvailabilityWiFi))
            {
                e.Value = tour.AvailabilityWiFi
                    ? "Да"
                    : "Нет";
            }
        }


        private void SetStatistics()
        {
            toolStrpLblTotalTours.Text = $"Общее кол-во туров: {items.Count}";
            toolStrpLblTotalCost.Text = $"Общая сумма за все туры: {items.Sum(t => t.TotalCost)} руб.";
            toolStrpLblToursWithSurcharges.Text = $"Кол-во туров с доплатами: {items.Count(t => t.Surcharges > 0)}";
            toolStrpLblTotalSurcharges.Text = $"Общая сумма доплат: {items.Sum(t => t.Surcharges)}";
        }

        private void tlStrpBtnAdd_Click(object sender, EventArgs e)
        {
            var addForm = new TourForm();
            if (addForm.ShowDialog(this) == DialogResult.OK)
            {
                items.Add(addForm.CurrentTour);
                bindingSource.ResetBindings(false);
                SetStatistics();
            }
        }

        private void tlStrpBtnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridViewTours.SelectedRows.Count == 0)
            {
                return;
            }

            var tour = (TourModel)dataGridViewTours.SelectedRows[0].DataBoundItem;

            var editForm = new TourForm(tour);
            if (editForm.ShowDialog(this) == DialogResult.OK)
            {
                var selectedTour = items.FirstOrDefault(x => x.Id == editForm.CurrentTour.Id);
                if (selectedTour != null)
                {
                    selectedTour.Direction = editForm.CurrentTour.Direction;
                    selectedTour.DepartureDate = editForm.CurrentTour.DepartureDate;
                    selectedTour.NumberNights = editForm.CurrentTour.NumberNights;
                    selectedTour.CostPerVacationer = editForm.CurrentTour.CostPerVacationer;
                    selectedTour.NumberVacationers = editForm.CurrentTour.NumberVacationers;
                    selectedTour.AvailabilityWiFi = editForm.CurrentTour.AvailabilityWiFi;
                    selectedTour.Surcharges = editForm.CurrentTour.Surcharges;
                    bindingSource.ResetBindings(false);
                    SetStatistics();
                }
            }
        }

        private void tlStrpBtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewTours.SelectedRows.Count == 0)
            {
                return;
            }

            var tour = (TourModel)dataGridViewTours.SelectedRows[0].DataBoundItem;
            var selectedTour = items.FirstOrDefault(x => x.Id == tour.Id);
            if (selectedTour != null &&
                MessageBox.Show($"Удалить тур '{tour.Direction}'?", 
                "Удаление тура", 
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                items.Remove(selectedTour);
                bindingSource.ResetBindings(false );
                SetStatistics();
            }
        }
    }
}
