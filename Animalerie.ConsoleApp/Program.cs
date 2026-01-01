using Animalerie.BLL.Services;
using Animalerie.BLL.Services.Interfaces;
using Animalerie.ConsoleApp.Ecrans;
using Animalerie.ConsoleApp.Screens;
using Animalerie.DAL.Repositories;
using Animalerie.DAL.Repositories.Interfaces;
using Animalerie.Domain.CustomEnums.Database;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using System.Data;
using System.Data.Common;
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
#endregion

#region Services
services.AddSingleton<IContactService, ContactService>();
services.AddSingleton<IAnimalService, AnimalService>();
services.AddSingleton<ICompatibiliteService, CompatibiliteService>();
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