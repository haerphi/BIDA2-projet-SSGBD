using Animalerie.BLL.Services;
using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.CustomEnums.ListingFilters;
using Animalerie.Domain.Models.Listing;
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
using System.Windows.Threading;

namespace Animalerie.WPF.Pages
{
    public partial class AnimalListPage : Page
    {
        private readonly IAnimalService _animalService;
        private DispatcherTimer _debounceTimer;

        public AnimalListPage()
        {
            InitializeComponent();
            _animalService = App.ServiceProvider.GetRequiredService<IAnimalService>();

            _debounceTimer = new DispatcherTimer();
            _debounceTimer.Interval = TimeSpan.FromMilliseconds(500);
            _debounceTimer.Tick += (s, e) =>
            {
                _debounceTimer.Stop();
                AppliquerFiltres();
            };

            ChargerEnums();
            AppliquerFiltres();
        }

        private void ChargerEnums()
        {
            cbFilterType.ItemsSource = Enum.GetValues(typeof(TypeAnimal));
            cbFilterSexe.ItemsSource = Enum.GetValues(typeof(SexeAnimal));
            cbFilterStatus.ItemsSource = Enum.GetValues(typeof(AnimalStatus));
        }

        private void AppliquerFiltres()
        {
            AnimalFilters filters = new AnimalFilters
            {
                Nom = string.IsNullOrWhiteSpace(txtFilterNom.Text) ? null : txtFilterNom.Text,
                Type = (TypeAnimal?)cbFilterType.SelectedItem,
                Sexe = (SexeAnimal?)cbFilterSexe.SelectedItem,
                AnimalStatus = (AnimalStatus?)cbFilterStatus.SelectedItem
            };

            var resultats = _animalService.Lister(filters);

            dgAnimaux.ItemsSource = resultats;
        }

        private void FilterChanged(object sender, EventArgs e)
        {
            if (sender is TextBox)
            {
                _debounceTimer.Stop();
                _debounceTimer.Start();
            }
            else
            {
                AppliquerFiltres();
            }
        }

        private void ResetFilters_Click(object sender, RoutedEventArgs e)
        {
            _debounceTimer.Stop(); // Arrêt du timer au cas où une recherche serait en attente

            txtFilterNom.Text = string.Empty;
            cbFilterType.SelectedItem = null;
            cbFilterSexe.SelectedItem = null;
            cbFilterStatus.SelectedItem = null;
            AppliquerFiltres();
        }
    }
}