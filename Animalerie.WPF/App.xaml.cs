using Animalerie.BLL.Services;
using Animalerie.BLL.Services.Interfaces;
using Animalerie.DAL.Repositories;
using Animalerie.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;

namespace Animalerie.WPF
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public App()
        {
            IServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            string connectionString = @"Host=localhost;Username=devuser;Password=devpassword;Database=refugeanimaux";

            #region Database
            services.AddSingleton<AnimalerieDBContext>(sp => AnimalerieDBContext.Build(connectionString));
            #endregion

            #region Repositories
            services.AddSingleton<IContactRepository, ContactRepository>();
            services.AddSingleton<IAnimalRepository, AnimalRepository>();
            services.AddSingleton<ICompatibiliteRepository, CompatibiliteRepository>();
            services.AddSingleton<IAdoptionRepository, AdoptionRepository>();
            services.AddSingleton<IVaccinRepository, VaccinRepository>();
            #endregion

            #region Services
            services.AddSingleton<IContactService, ContactService>();
            services.AddSingleton<IAnimalService, AnimalService>();
            services.AddSingleton<ICompatibiliteService, CompatibiliteService>();
            services.AddSingleton<IAdoptionService, AdoptionService>();
            services.AddSingleton<IVaccinService, VaccinService>();
            #endregion

            services.AddTransient<MainWindow>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
