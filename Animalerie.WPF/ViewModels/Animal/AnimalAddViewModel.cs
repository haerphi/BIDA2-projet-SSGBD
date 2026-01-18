using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using Animalerie.Domain.Patterns;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Animalerie.WPF.ViewModels
{
    public class AnimalAddViewModel : INotifyPropertyChanged
    {
        private readonly IAnimalService _animalService;
        private readonly IContactService _contactService;

        private string _id;
        public string Id { get => _id; set { _id = value; OnPropertyChanged(); } }

        private string _nom;
        public string Nom { get => _nom; set { _nom = value; OnPropertyChanged(); } }

        public IEnumerable<TypeAnimal> Types { get; }
        private TypeAnimal? _selectedType;
        public TypeAnimal? SelectedType { get => _selectedType; set { _selectedType = value; OnPropertyChanged(); } }

        public IEnumerable<SexeAnimal> Sexes { get; }
        private SexeAnimal? _selectedSexe;
        public SexeAnimal? SelectedSexe { get => _selectedSexe; set { _selectedSexe = value; OnPropertyChanged(); } }

        public IEnumerable<RaisonEntree> Raisons { get; }
        private RaisonEntree? _selectedRaison;
        public RaisonEntree? SelectedRaison { get => _selectedRaison; set { _selectedRaison = value; OnPropertyChanged(); } }

        public IEnumerable<Contact> Contacts { get; }
        private Contact _selectedContact;
        public Contact SelectedContact { get => _selectedContact; set { _selectedContact = value; OnPropertyChanged(); } }

        private DateTime? _dateNaissance;
        public DateTime? DateNaissance { get => _dateNaissance; set { _dateNaissance = value; OnPropertyChanged(); } }

        private string _couleurs;
        public string Couleurs { get => _couleurs; set { _couleurs = value; OnPropertyChanged(); } }

        private string _particularites;
        public string Particularites { get => _particularites; set { _particularites = value; OnPropertyChanged(); } }

        private string _description;
        public string Description { get => _description; set { _description = value; OnPropertyChanged(); } }

        private DateTime? _dateSterilisation;
        public DateTime? DateSterilisation { get => _dateSterilisation; set { _dateSterilisation = value; OnPropertyChanged(); } }

        private DateTime _dateEntree = DateTime.Today; // Valeur par défaut
        public DateTime DateEntree { get => _dateEntree; set { _dateEntree = value; OnPropertyChanged(); } }

        public ICommand SaveCommand { get; }
        public event PropertyChangedEventHandler? PropertyChanged;

        public AnimalAddViewModel(IAnimalService animalService, IContactService contactService)
        {
            _animalService = animalService;
            _contactService = contactService;

            Types = Enum.GetValues(typeof(TypeAnimal)).Cast<TypeAnimal>();
            Sexes = Enum.GetValues(typeof(SexeAnimal)).Cast<SexeAnimal>();
            Raisons = Enum.GetValues(typeof(RaisonEntree)).Cast<RaisonEntree>();
            Contacts = _contactService.Lister();

            SaveCommand = new RelayCommand(Sauvegarder);
        }

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void Sauvegarder(object obj)
        {
            // Validation (Logique déplacée du code-behind)
            if (!Regex.IsMatch(Id ?? "", AnimalPatterns.ID))
            {
                MessageBox.Show("L'ID ne respecte pas le format requis.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Transformation des couleurs (string -> array)
            string[] tabCouleurs = Couleurs?.Split(',').Select(c => c.Trim()).ToArray() ?? new string[0];

            var model = new Animal(Id, Nom, SelectedType ?? TypeAnimal.Chat, SelectedSexe ?? SexeAnimal.F,
                                   Particularites, Description, DateSterilisation, DateNaissance ?? DateTime.Now, tabCouleurs);

            try
            {
                _animalService.Ajouter(
                    model,
                    couleurs: tabCouleurs,
                    contactId: SelectedContact?.Id ?? 0,
                    raison: SelectedRaison ?? RaisonEntree.Abandon,
                    dateEntree: DateEntree
                );

                MessageBox.Show("Animal ajouté !");
                // Note : La navigation devrait idéalement être gérée par un service de navigation injecté
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }
    }
}
