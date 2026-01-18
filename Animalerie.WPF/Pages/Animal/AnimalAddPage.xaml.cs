using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using Animalerie.Domain.Patterns;
using Animalerie.WPF.Interfaces;
using Animalerie.WPF.ViewModels;
using Animalerie.WPF.ViewModels.Base;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Animalerie.WPF.Pages
{
    public partial class AnimalAddPage : Page, ICanCheckDirty
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

        public AnimalAddPage()
        {
            InitializeComponent();

            var animalService = App.ServiceProvider.GetRequiredService<IAnimalService>();
            var contactService = App.ServiceProvider.GetRequiredService<IContactService>();

            DataContext = new AnimalAddViewModel(animalService, contactService);
        }
    }
}