using Animalerie.BLL.Services.Interfaces;
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

namespace Animalerie.WPF.Pages.Contact
{
    /// <summary>
    /// Interaction logic for ContactDetailsPage.xaml
    /// </summary>
    public partial class ContactDetailsPage : Page
    {
        public ContactDetailsPage(int contactId)
        {
            InitializeComponent();
            var service = App.ServiceProvider.GetRequiredService<IContactService>();
            var vm = new ViewModels.Contacts.ContactDetailsViewModel(service, contactId);

            vm.RequestNavigateToEditContact += OnRequestNavigateToEditContact;

            this.DataContext = vm;

            this.Loaded += ContactDetailsPage_Loaded;
        }

        private void ContactDetailsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is ViewModels.Contacts.ContactDetailsViewModel vm)
            {
                vm.LoadData();
            }
        }

        public void OnRequestNavigateToEditContact(int contactId)
        {
            this.NavigationService.Navigate(new ContactFormPage(contactId));
        }
    }
}
