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
            vm.RequestNavigateToPutInHostFamily += OnRequestNavigateToPutInHostFamily;
            vm.RequestNavigateToEditHostFamily += OnRequestNavigateToEditHostFamily;
            vm.RequestNavigateToAdoptionForm += OnRequestNavigateToAdoptionForm;
            vm.RequestNavigateToEditAdoptionForm += OnRequestNavigateToEditAdoptionForm;
            vm.RequestNavigateToUpdateVaccination += OnRequestNavigateToUpdateVaccination;
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

        public void OnRequestNavigateToPutInHostFamily(string animalId)
        {
            this.NavigationService.Navigate(new AnimalPutInHostFamilyPage(animalId));
        }
        public void OnRequestNavigateToEditHostFamily(int familleId)
        {
            this.NavigationService.Navigate(new AnimalPutInHostFamilyPage(familleId));
        }
        public void OnRequestNavigateToAdoptionForm(string animalId)
        {
            this.NavigationService.Navigate(new AnimalAdoptionForm(animalId));
        }

        public void OnRequestNavigateToEditAdoptionForm(int adoptionId)
        {
            this.NavigationService.Navigate(new AnimalAdoptionForm(adoptionId));
        }

        public void OnRequestNavigateToUpdateVaccination(string animalId)
        {
            this.NavigationService.Navigate(new AnimalEditVaccinationPage(animalId));
        }
    }
}
