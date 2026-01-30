using Animalerie.BLL.Services.Interfaces;
using Animalerie.WPF.ViewModels.Contacts;
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

namespace Animalerie.WPF.Pages
{
    /// <summary>
    /// Logique d'interaction pour ContactListPage.xaml
    /// </summary>
    public partial class ContactListPage : Page
    {
        public ContactListPage()
        {
            InitializeComponent();

            var service = App.ServiceProvider.GetRequiredService<IContactService>();

            var vm = new ContactListViewModel(service);
            vm.RequestNavigateToDetails += OnRequestNavigateToDetails;

            this.DataContext = vm;

            this.Loaded += (s, e) => vm.LoadData();
        }

        public void OnRequestNavigateToDetails(int contactId)
        {
            //this.NavigationService.Navigate(new ContactDetailsPage(contactId));
        }
    }
}
