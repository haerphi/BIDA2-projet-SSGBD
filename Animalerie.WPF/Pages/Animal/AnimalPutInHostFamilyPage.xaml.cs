using Animalerie.BLL.Services.Interfaces;
using Animalerie.WPF.Pages.Interfaces;
using Animalerie.WPF.ViewModels.Animals;
using Animalerie.WPF.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
namespace Animalerie.WPF.Pages.Animal
{
    /// <summary>
    /// Interaction logic for AnimalPutInHostFamilyPage.xaml
    /// </summary>
    public partial class AnimalPutInHostFamilyPage : Page, ICanCheckDirty
    {
        public bool IsDirty
        {
            get
            {
                if (DataContext is ViewModelBase vm)
                {
                    return vm.IsDirty;
                }
                return false;
            }
        }

        public AnimalPutInHostFamilyPage(string animalId)
        {
            InitializeComponent();

            var animalService = App.ServiceProvider.GetRequiredService<IAnimalService>();
            var contactService = App.ServiceProvider.GetRequiredService<IContactService>();

           var vm = new AnimalPutInHostFamilyViewModel(animalId, animalService, contactService);

            vm.RequestClose += () =>
            {
                if (this.NavigationService.CanGoBack)
                    this.NavigationService.GoBack();
            };

            this.DataContext = vm;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is AnimalPutInHostFamilyViewModel vm)
            {
                vm.LoadData();
            }
        }
    }
}
