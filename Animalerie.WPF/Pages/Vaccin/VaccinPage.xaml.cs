using Animalerie.BLL.Services.Interfaces;
using Animalerie.WPF.Pages.Interfaces;
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

namespace Animalerie.WPF.Pages.Vaccin
{
    /// <summary>
    /// Interaction logic for VaccinPage.xaml
    /// </summary>
    public partial class VaccinPage : Page, ICanCheckDirty
    {
        public bool IsDirty => (DataContext is ViewModelBase vm) && vm.IsDirty;

        public VaccinPage()
        {
            InitializeComponent();
            var service = App.ServiceProvider.GetRequiredService<IVaccinService>();
            this.DataContext = new ViewModels.Vaccins.VaccinViewModel(service);

            this.Loaded += (s, e) => {
                if (this.DataContext is ViewModels.Vaccins.VaccinViewModel vm) vm.LoadData();
            };
        }
    }
}
