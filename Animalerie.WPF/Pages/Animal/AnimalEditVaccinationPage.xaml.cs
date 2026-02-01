using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.Models;
using Animalerie.WPF.Pages.Interfaces;
using Animalerie.WPF.ViewModels.Animals;
using Animalerie.WPF.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Animalerie.WPF.Pages.Animal
{
    /// <summary>
    /// Interaction logic for AnimalEditVaccinationPage.xaml
    /// </summary>
    public partial class AnimalEditVaccinationPage : Page, ICanCheckDirty
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

        public AnimalEditVaccinationPage(string animalId)
        {
            InitializeComponent();

            var animalService = App.ServiceProvider.GetRequiredService<IAnimalService>();
            var vaccinService = App.ServiceProvider.GetRequiredService<IVaccinService>();

            var vm = new AnimalEditVaccinationViewModel(animalService, vaccinService, animalId);

            vm.RequestClose += () =>
            {
                if (this.NavigationService.CanGoBack)
                    this.NavigationService.GoBack();
            };

            this.DataContext = vm;
        }
    }
}
