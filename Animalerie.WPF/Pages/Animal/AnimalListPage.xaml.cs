using Animalerie.BLL.Services.Interfaces;
using Animalerie.WPF.ViewModels;
using Animalerie.WPF.ViewModels.Animals;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Animalerie.WPF.Pages
{
    public partial class AnimalListPage : Page
    {
        public AnimalListPage()
        {
            InitializeComponent();

            var service = App.ServiceProvider.GetRequiredService<IAnimalService>();

            // Création et assignation du ViewModel
            this.DataContext = new AnimalListViewModel(service);
        }
    }
}