using Animalerie.BLL.Services;
using Animalerie.BLL.Services.Interfaces;
using Animalerie.ConsoleApp.Ecrans;
using Animalerie.ConsoleApp.Screens;
using Animalerie.DAL.Repositories;
using Animalerie.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Tools.Database;

string connectionString = @"Host=localhost;Username=devuser;Password=devpassword;Database=refugeanimaux";

IServiceCollection services = new ServiceCollection();

#region Database
services.AddSingleton<AnimalerieDBContext>(sp => AnimalerieDBContext.Build(connectionString));
#endregion

#region Repositories
services.AddSingleton<IContactRepository, ContactRepository>();
services.AddSingleton<IAnimalRepository, AnimalRepository>();
services.AddSingleton<ICompatibiliteRepository, CompatibiliteRepository>();
services.AddSingleton<IAdoptionRepository, AdoptionRepository>();
#endregion

#region Services
services.AddSingleton<IContactService, ContactService>();
services.AddSingleton<IAnimalService, AnimalService>();
services.AddSingleton<ICompatibiliteService, CompatibiliteService>();
services.AddSingleton<IAdoptionService, AdoptionService>();
#endregion

#region Ecrans
services.AddTransient<EcranPrincipal>();
services.AddTransient<EcranAnimal>();
#endregion

var serviceProvider = services.BuildServiceProvider();

var dbContext = serviceProvider.GetRequiredService<AnimalerieDBContext>();
var menu = serviceProvider.GetRequiredService<EcranPrincipal>();

dbContext.Connection.EnsureValidConnection();

menu.Display();