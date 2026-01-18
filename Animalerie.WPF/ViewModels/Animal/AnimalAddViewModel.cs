using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using Animalerie.Domain.Patterns;
using Animalerie.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Animalerie.WPF.ViewModels
{
    public class AnimalAddViewModel : ViewModelBase
    {
        private readonly IAnimalService _animalService;
        private readonly IContactService _contactService;

        // Propriétés pour les champs de saisie
        private string _id;
        public string Id { get => _id; set => SetProperty(ref _id, value: value); }

        private string _nom;
        public string Nom { get => _nom; set => SetProperty(ref _nom, value: value); }

        private TypeAnimal _selectedType;
        public TypeAnimal SelectedType { get => _selectedType; set => SetProperty(ref _selectedType, value: value); }

        private SexeAnimal _selectedSexe;
        public SexeAnimal SelectedSexe { get => _selectedSexe; set => SetProperty(ref _selectedSexe, value: value); }

        private DateTime? _dateNaissance;
        public DateTime? DateNaissance { get => _dateNaissance; set => SetProperty(ref _dateNaissance, value: value); }

        private string _couleursInput;
        public string CouleursInput { get => _couleursInput; set => SetProperty(ref _couleursInput, value: value); }

        private string _particularites;
        public string Particularites { get => _particularites; set => SetProperty(ref _particularites, value: value); }

        private string _description;
        public string Description { get => _description; set => SetProperty(ref _description, value: value); }

        private DateTime? _dateSterilisation;
        public DateTime? DateSterilisation { get => _dateSterilisation; set => SetProperty(ref _dateSterilisation, value: value); }

        private RaisonEntree _selectedRaison;
        public RaisonEntree SelectedRaison { get => _selectedRaison; set => SetProperty(ref _selectedRaison, value: value); }

        private DateTime _dateEntree;
        public DateTime DateEntree { get => _dateEntree; set => SetProperty(ref _dateEntree, value: value); }

        private Contact _selectedContact;
        public Contact SelectedContact { get => _selectedContact; set => SetProperty(ref _selectedContact, value: value); }

        // Listes pour les ComboBox
        public IEnumerable<TypeAnimal> TypesList { get; }
        public IEnumerable<SexeAnimal> SexesList { get; }
        public IEnumerable<RaisonEntree> RaisonsList { get; }
        public ObservableCollection<Contact> ContactsList { get; }

        // Commandes
        public RelayCommand SaveCommand { get; }

        public AnimalAddViewModel(IAnimalService animalService, IContactService contactService)
        {
            _animalService = animalService;
            _contactService = contactService;

            TypesList = Enum.GetValues(typeof(TypeAnimal)).Cast<TypeAnimal>();
            SexesList = Enum.GetValues(typeof(SexeAnimal)).Cast<SexeAnimal>();
            RaisonsList = Enum.GetValues(typeof(RaisonEntree)).Cast<RaisonEntree>();

            var contacts = _contactService.Lister();
            ContactsList = new ObservableCollection<Contact>(contacts);

            ResetForm();

            SaveCommand = new RelayCommand(ExecuteSave);

            IsDirty = false;
        }

        private void ExecuteSave(object? obj)
        {
            // Validation
            if (!Regex.IsMatch(Id ?? "", AnimalPatterns.ID))
            {
                MessageBox.Show("L'ID ne respecte pas le format requis.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Traitement
            string[] couleurs = CouleursInput?.Split(',').Select(c => c.Trim()).ToArray() ?? Array.Empty<string>();

            Animal model = new Animal(
                    Id,
                    Nom,
                    SelectedType,
                    SelectedSexe,
                    Particularites,
                    Description,
                    DateSterilisation,
                    DateNaissance ?? DateTime.Now,
                    couleurs
                );

            try
            {
                _animalService.Ajouter(
                    model,
                    couleurs: couleurs,
                    contactId: SelectedContact?.Id ?? 0,
                    raison: SelectedRaison,
                    dateEntree: DateEntree
                );

                MessageBox.Show("Animal ajouté avec succès !");
                ResetForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }

        private void ResetForm()
        {
            Id = string.Empty;
            Nom = string.Empty;
            CouleursInput = string.Empty;
            Particularites = string.Empty;
            Description = string.Empty;
            DateNaissance = null;
            DateSterilisation = null;
            DateEntree = DateTime.Today;
            SelectedContact = null;
            SelectedType = default(TypeAnimal);
            SelectedSexe = default(SexeAnimal);
            SelectedRaison = default(RaisonEntree);

            IsDirty = false;
        }
    }
}
