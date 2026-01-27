using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.Models;
using Animalerie.WPF.Mappers;
using Animalerie.WPF.Models.Contacts;
using Animalerie.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Animalerie.WPF.ViewModels.Animals
{
    public class AnimalPutInHostFamilyViewModel : ViewModelBase
    {
        private readonly IAnimalService _animalService;
        private readonly IContactService _contactService;

        private readonly FamilleAccueil? _familleAccueil; // null si création, remplis en mode édition
        private readonly string _animalId;

        private Animal? _selectedAnimal;
        public Animal? SelectedAnimal
        {
            get => _selectedAnimal;
            set => SetProperty(ref _selectedAnimal, value);
        }

        private ContactModel? _selectedContact;
        public ContactModel? SelectedContact
        {
            get => _selectedContact;
            set => SetProperty(ref _selectedContact, value);
        }

        private DateTime _dateStart;
        public DateTime DateStart
        {
            get => _dateStart;
            set => SetProperty(ref _dateStart, value);
        }

        private DateTime? _dateEnd;
        public DateTime? DateEnd
        {
            get => _dateEnd;
            set => SetProperty(ref _dateEnd, value);
        }

        // Combobox
        public ObservableCollection<ContactModel> ContactsList { get; } = new();

        // Commandes
        public RelayCommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action RequestClose;

        private AnimalPutInHostFamilyViewModel(IAnimalService animalService, IContactService contactService)
        {
            _animalService = animalService;
            _contactService = contactService;

            SaveCommand = new RelayCommand(
                execute: _ => SaveData(),
                canExecute: _ => SelectedContact != null && SelectedAnimal != null
            );
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke());
        }

        // Constructeur en mode création
        public AnimalPutInHostFamilyViewModel(string animalId, IAnimalService animalService, IContactService contactService)
            : this(animalService, contactService)
        {
            _animalId = animalId;
            _familleAccueil = null;

            ResetForm();
        }

        // Constructeur en mode édition
        public AnimalPutInHostFamilyViewModel(int familleAccueilId, IAnimalService animalService, IContactService contactService)
            : this(animalService, contactService)
        {
            _familleAccueil = _animalService.ConsulterFamilelAccueil(familleAccueilId);

            if (_familleAccueil == null)
                throw new ArgumentException("Famille d'accueil introuvable");

            _animalId = _familleAccueil.AniId;
        }

        public void LoadData()
        {
            SelectedAnimal = _animalService.Consulter(_animalId);

            ContactsList.Clear();
            foreach (Contact contact in _contactService.Lister())
            {
                ContactsList.Add(contact.ToContactModel());
            }

            // Si mode édition, récupérer les données existantes
            if (_familleAccueil != null)
            {
                DateStart = _familleAccueil.DateDebut;
                DateEnd = _familleAccueil.DateFin;
                SelectedContact = ContactsList.FirstOrDefault(c => c.Id == _familleAccueil.ContactId);

                IsDirty = false;
            }
            else
            {
                ResetForm();
            }
        }

        private void SaveData()
        {
            if (SelectedAnimal != null && SelectedContact != null)
            {
                try
                {
                   // mode création
                    if (_familleAccueil == null)
                    {
                        _animalService.MettreEnFamilleAccueil(_animalId, SelectedContact.Id, DateStart, DateEnd);
                    }
                    else
                    {
                        _familleAccueil.ContactId = SelectedContact.Id;
                        _familleAccueil.DateDebut = DateStart;
                        _familleAccueil.DateFin = DateEnd;
                        // mode édition
                        _animalService.ModifierFamilleAccueil(_familleAccueil);
                    }

                    MessageBox.Show("Sauvegarde effectuée avec succès !");
                    IsDirty = false;
                    RequestClose?.Invoke();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Erreur : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ResetForm()
        {
            SelectedContact = null;
            DateStart = DateTime.Today;
            DateEnd = null;
            IsDirty = false;
        }
    }
}