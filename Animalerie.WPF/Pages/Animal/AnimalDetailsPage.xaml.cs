using Animalerie.BLL.Services.Interfaces;
using Animalerie.WPF.ViewModels.Animals;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Animalerie.WPF.Pages.Animal
{
    /// <summary>
    /// Logique d'interaction pour AnimalDetailsPage.xaml
    /// </summary>
    public partial class AnimalDetailsPage : Page
    {
        public AnimalDetailsPage(string animalId)
        {
            InitializeComponent();
            var service = App.ServiceProvider.GetRequiredService<IAnimalService>();
            var vm = new AnimalDetailsViewModel(service, animalId);

            vm.RequestNavigateToEditCompat += OnRequestNavigateToEditCompat;

            this.DataContext = vm;

            this.Loaded += AnimalDetailsPage_Loaded;
        }

        private void AnimalDetailsPage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.DataContext is AnimalDetailsViewModel vm)
            {
                vm.LoadData();
            }
        }

        public void OnRequestNavigateToEditCompat(string animalId)
        {
            this.NavigationService.Navigate(new AnimalEditCompatPage(animalId));
        }
    }
}
