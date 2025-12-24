using Animalerie.BLL.Services;
using Animalerie.BLL.Services.Interfaces;
using Animalerie.ConsoleApp.Ecrans;
using Animalerie.ConsoleApp.Screens;
using Animalerie.DAL.Repositories;
using Animalerie.DAL.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

string connectionString = @"Host=localhost;Username=devuser;Password=devpass;Database=RefugeAnimaux";

IServiceCollection services = new ServiceCollection();
services.AddSingleton<AnimalerieDBContext>(sp => AnimalerieDBContext.Build(connectionString));

#region Repositories
services.AddSingleton<IContactRepository, ContactRepository>();
services.AddSingleton<IAnimalRepository, AnimalRepository>();
#endregion

#region Services
services.AddSingleton<IContactService, ContactService>();
services.AddSingleton<IAnimalService, AnimalService>();
#endregion


#region Ecrans
services.AddTransient<EcranPrincipal>();
services.AddTransient<EcranAnimal>();
#endregion


var serviceProvider = services.BuildServiceProvider();

var dbContext = serviceProvider.GetRequiredService<AnimalerieDBContext>();
var menu = serviceProvider.GetRequiredService<EcranPrincipal>();

menu.Display();