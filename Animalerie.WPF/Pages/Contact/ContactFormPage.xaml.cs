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

namespace Animalerie.WPF.Pages.Contact
{
    /// <summary>
    /// Interaction logic for ContactFormPage.xaml
    /// </summary>
    public partial class ContactFormPage : Page, ICanCheckDirty
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

        public ContactFormPage()
        {
            InitializeComponent();

            var contactService = App.ServiceProvider.GetRequiredService<IContactService>();

            var vm = new ViewModels.Contacts.ContactFormViewModel(contactService);

            vm.RequestClose += () =>
            {
                if (this.NavigationService.CanGoBack)
                    this.NavigationService.GoBack();
            };

            this.DataContext = vm;
            this.Loaded += ContactFormPage_Loaded;
        }

        public ContactFormPage(int contactId)
        {
            InitializeComponent();

            var contactService = App.ServiceProvider.GetRequiredService<IContactService>();

            var vm = new ViewModels.Contacts.ContactFormViewModel(contactService, contactId);

            vm.RequestClose += () =>
            {
                if (this.NavigationService.CanGoBack)
                    this.NavigationService.GoBack();
            };

            this.DataContext = vm;
            this.Loaded += ContactFormPage_Loaded;
        }

        private void ContactFormPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is ViewModels.Contacts.ContactFormViewModel vm)
            {
                vm.LoadData();
            }
        }
    }
}
