using Animalerie.BLL.Services.Interfaces;
using Animalerie.WPF.Pages.Animal;
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
            var vm = new AnimalListViewModel(service);

            vm.RequestNavigateToDetails += OnRequestNavigateToDetails;

            this.DataContext = vm;
        }

        public void OnRequestNavigateToDetails(string animalId)
        {
            this.NavigationService.Navigate(new AnimalDetailsPage(animalId));
        }
    }
}