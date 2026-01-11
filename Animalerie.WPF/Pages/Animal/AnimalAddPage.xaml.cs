using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using Animalerie.Domain.Patterns;
using Animalerie.WPF.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace Animalerie.WPF.Pages
{
    public partial class AnimalAddPage : Page, ICanCheckDirty
    {
        public bool IsDirty { get; private set; } = false;

        private readonly IAnimalService _animalService;
        private readonly IContactService _contactService;

        public AnimalAddPage(IAnimalService animalService, IContactService contactService)
        {
            InitializeComponent();
            _animalService = animalService;
            _contactService = contactService;

            RemplirChamps();
        }

        public AnimalAddPage()
            : this(
                  App.ServiceProvider.GetRequiredService<IAnimalService>(),
                  App.ServiceProvider.GetRequiredService<IContactService>()
              )
        {            
        }

        private void RemplirChamps()
        {
            cbType.ItemsSource = Enum.GetValues(typeof(TypeAnimal));
            cbSexe.ItemsSource = Enum.GetValues(typeof(SexeAnimal));
            cbRaison.ItemsSource = Enum.GetValues(typeof(RaisonEntree));

            cbContact.ItemsSource = _contactService.Lister();

            dpEntree.SelectedDate = DateTime.Today;

            txtId.TextChanged += MarkAsDirty;
            txtNom.TextChanged += MarkAsDirty;
            cbType.SelectionChanged += MarkAsDirty;
        }

        private void MarkAsDirty(object sender, EventArgs e)
        {
            IsDirty = true;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (!Regex.IsMatch(txtId.Text, AnimalPatterns.ID))
            {
                MessageBox.Show("L'ID ne respecte pas le format requis. (JJMMAAXXXXX, jour_mois_année_nombre)", "Erreur de validation", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string[] couleurs = txtCouleurs.Text.Split(',').Select(c => c.Trim()).ToArray();
            Animal model = new Animal(
                    txtId.Text,
                    txtNom.Text,
                    (TypeAnimal)cbType.SelectedItem,
                    (SexeAnimal)cbSexe.SelectedItem,
                    txtPartic.Text,
                    txtDesc.Text,
                    dpSteril.SelectedDate,
                    dpNaissance.SelectedDate ?? DateTime.Now,
                    couleurs
                );

            try
            {
                _animalService.Ajouter(
                    model,
                    couleurs: couleurs,
                    contactId: ((Contact)cbContact.SelectedItem).Id,
                    raison: (RaisonEntree)cbRaison.SelectedItem,
                    dateEntree: (DateTime)dpEntree.SelectedDate!
                );
                IsDirty = false;
                MessageBox.Show("Animal ajouté avec succès !");
                NavigationService.Navigate(new AnimalListPage());
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }
    }
}