using DataGridViewProject.Forms;
using DatabaseStorage;
using Manager;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DataGridViewProject
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Создаем хранилище базы данных
            var storage = new TourDatabaseStorage();

            // Настройка Serilog
            var serilogger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Debug()
                .WriteTo.File("logs/tour-manager-.log",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.Seq("http://localhost:5341",
                    apiKey: "56XFeiZz8fsIEahlmmt8")
                .CreateLogger();

            // Создаем фабрику логгеров и подключаем Serilog
            var loggerFactory = new LoggerFactory()
                .AddSerilog(serilogger);

            // Создаем менеджер туров
            var tourManager = new TourManager(storage, loggerFactory.CreateLogger<TourManager>());

            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm(tourManager));
        }
    }
}