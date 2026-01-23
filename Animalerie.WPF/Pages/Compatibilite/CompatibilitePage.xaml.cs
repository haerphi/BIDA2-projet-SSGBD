using Animalerie.BLL.Services.Interfaces;
using Animalerie.WPF.Pages.Interfaces;
using Animalerie.WPF.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Controls;

namespace Animalerie.WPF.Pages.Compatibilite
{
    /// <summary>
    /// Interaction logic for CompatibilitePage.xaml
    /// </summary>
    public partial class CompatibilitePage : Page, ICanCheckDirty
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

        public CompatibilitePage()
        {
            InitializeComponent();
            var service = App.ServiceProvider.GetRequiredService<ICompatibiliteService>();
            var vm = new ViewModels.Compaitibilites.CompatibiliteViewModel(service);

            this.DataContext = vm;

            this.Loaded += CompatibilitePage_Loaded;
        }

        private void CompatibilitePage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (this.DataContext is ViewModels.Compaitibilites.CompatibiliteViewModel vm)
            {
                vm.LoadData();
            }
        }
    }
}
