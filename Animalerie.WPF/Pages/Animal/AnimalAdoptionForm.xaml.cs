using Animalerie.BLL.Services.Interfaces;
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
    /// Logique d'interaction pour AnimalAdoptionForm.xaml
    /// </summary>
    public partial class AnimalAdoptionForm : Page, ICanCheckDirty
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

        public AnimalAdoptionForm(string animalId)
        {
            InitializeComponent();
            var animalService = App.ServiceProvider.GetRequiredService<IAnimalService>();
            var contactService = App.ServiceProvider.GetRequiredService<IContactService>();
            var adoptionService = App.ServiceProvider.GetRequiredService<IAdoptionService>();

            var vm = new AnimalAdoptionFormViewModel(animalService, contactService, adoptionService, animalId);
            
            vm.RequestClose += () =>
            {
                if (this.NavigationService.CanGoBack)
                    this.NavigationService.GoBack();
            };

            this.DataContext = vm;
            this.Loaded += (s, e) => vm.LoadData();

        }

        private void AnimalAdoptionForm_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is AnimalAdoptionFormViewModel vm)
            {
                vm.LoadData();
            }

        }
    }
}
