using Animalerie.BLL.Services.Interfaces;
using Animalerie.WPF.Pages.Interfaces;
using Animalerie.WPF.ViewModels.Animals;
using Animalerie.WPF.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Animalerie.WPF.Pages.Animal
{
    public partial class AnimalEditCompatPage : Page, ICanCheckDirty
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
        
        public AnimalEditCompatPage(string animalId)
        {
            InitializeComponent();

            var animalService = App.ServiceProvider.GetRequiredService<IAnimalService>();
            var compatService = App.ServiceProvider.GetRequiredService<ICompatibiliteService>();

            var vm = new AnimalEditCompatibiliteViewModel(animalService, compatService, animalId);

            vm.RequestClose += () =>
            {
                if (this.NavigationService.CanGoBack)
                    this.NavigationService.GoBack();
            };

            this.DataContext = vm;
        }
    }
}