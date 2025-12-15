using DataGridViewProject.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Manager.Contracts;
using Entities.Models;

namespace DataGridViewProject.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ITourManager tourManager;

        /// <summary>
        /// Инициализирует контроллер
        /// </summary>
        public HomeController(ITourManager tourManager)
        {
            this.tourManager = tourManager;
        }

        /// <summary>
        /// Отображает главную страницу со списком туров и статистикой
        /// </summary>
        public async Task<IActionResult> Index()
        {
            var tours = await tourManager.GetAll();
            var statistics = await tourManager.GetStatistics();

            var model = new IndexViewModel
            {
                Tours = tours.ToList(),
                Statistics = statistics
            };

            return View(model);
        }

        /// <summary>
        /// Отображает страницу подтверждения удаления выбранного тура
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var tour = await tourManager.GetById(id);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        /// <summary>
        /// Выполняет удаление тура после подтверждения пользователем
        /// </summary>
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await tourManager.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Отображает форму редактирования выбранного тура
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var tour = await tourManager.GetById(id);
            if (tour == null)
            {
                return NotFound();
            }

            return View(tour);
        }

        /// <summary>
        /// Принимает изменения тура из формы и сохраняет их
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Edit(TourModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await tourManager.Update(model);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Отображает пустую форму для добавления нового тура
        /// </summary>
        [HttpGet]
        public IActionResult Create()
        {
            var model = new TourModel()
            {
                // Устанавливаем текущую дату по умолчанию
                DepartureDate = DateOnly.FromDateTime(DateTime.Today)
            };

            return View(model);
        }

        /// <summary>
        /// Принимает данные нового тура из формы и добавляет его в хранилище
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create(TourModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await tourManager.Add(model);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Отображает страницу с политикой конфиденциальности
        /// </summary>
        public IActionResult Privacy()
        {
            return View();
        }

        /// <summary>
        /// Отображает страницу ошибки с информацией о текущем запросе
        /// </summary>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}