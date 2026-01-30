using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using Animalerie.WPF.Mappers;
using Animalerie.WPF.Models.Animals;
using Animalerie.WPF.Models.Contacts;
using Animalerie.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Animalerie.WPF.ViewModels.Animals
{
    internal class AnimalAdoptionFormViewModel : ViewModelBase
    {
        private readonly IAnimalService _animalService;
        private readonly IContactService _contactService;
        private readonly IAdoptionService _adoptionService;
        private readonly string? _animalId;

        private readonly int? _adoptionId; // mode édition
        private AdoptionModel? _existingAdoption; // mode édition

        private AnimalDetailsModel? _selectedAnimal;
        public AnimalDetailsModel? SelectedAnimal
        {
            get => _selectedAnimal;
            set => SetProperty(ref _selectedAnimal, value);
        }

        private ObservableCollection<ContactModel> _contacts = new();
        public ObservableCollection<ContactModel> Contacts
        {
            get => _contacts;
            set => SetProperty(ref _contacts, value);
        }

        private ContactModel? _selectedContact;
        public ContactModel? SelectedContact
        {
            get => _selectedContact;
            set => SetProperty(ref _selectedContact, value);
        }

        private string _note = string.Empty;
        public string Note
        {
            get => _note;
            set => SetProperty(ref _note, value);
        }

        public IEnumerable<StatutAdoption> StatutOptions => Enum.GetValues(typeof(StatutAdoption)).Cast<StatutAdoption>();

        private StatutAdoption _selectedStatut;
        public StatutAdoption SelectedStatut
        {
            get => _selectedStatut;
            set => SetProperty(ref _selectedStatut, value);
        }

        public ICommand ValiderAdoptionCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action RequestClose;

        private AnimalAdoptionFormViewModel(IAnimalService animalService, IContactService contactService, IAdoptionService adoptionService)
        {
            _animalService = animalService;
            _contactService = contactService;
            _adoptionService = adoptionService;

            ValiderAdoptionCommand = new RelayCommand(ExecuteValider, CanExecuteValider);
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke());
        }

        // Mode Création
        public AnimalAdoptionFormViewModel(IAnimalService animalService, IContactService contactService, IAdoptionService adoptionService, string animalId)
            : this(animalService, contactService, adoptionService)
        {
            _animalId = animalId;
            _adoptionId = null;
        }

        // Mode Édition
        public AnimalAdoptionFormViewModel(IAnimalService animalService, IContactService contactService, IAdoptionService adoptionService, int adoptionId)
            : this(animalService, contactService, adoptionService)
        {
            _adoptionId = adoptionId;

            _existingAdoption = _adoptionService.Consulter(adoptionId)?.ToAdoptionModel();
            if (_existingAdoption == null)
            {
                throw new ArgumentException("Adoption introuvable");
            }

            _animalId = _existingAdoption.AniId;
        }

        public void LoadData()
        {
            SelectedAnimal = _animalService.Consulter(_animalId).ToAnimalDetailsModel();
            var liste = _contactService.Lister().Select(c => c.ToContactModel());
            Contacts = new ObservableCollection<ContactModel>(liste);

            if (_existingAdoption != null)
            {
                // Remplissage des champs en mode édition
                Note = _existingAdoption.Note ?? string.Empty;
                SelectedContact = Contacts.FirstOrDefault(c => c.Id == _existingAdoption.ContactId);
                SelectedStatut = _existingAdoption.Statut;
                IsDirty = false;
            }

            IsDirty = false;
        }

        private bool CanExecuteValider(object? obj)
        {
            return SelectedContact != null && SelectedAnimal != null;
        }

        private void ExecuteValider(object? obj)
        {
            try
            {
                if (_existingAdoption is null)
                {
                    // mode création
                    _adoptionService.Ajouter(SelectedAnimal!.Id, SelectedContact!.Id, Note, SelectedStatut);
                }
                else
                {
                    // mode édition
                    _adoptionService.Modifier(_existingAdoption.Id, SelectedStatut, Note);
                }
                IsDirty = false;
                MessageBox.Show("Demande d'adoption enregistrée !");

                RequestClose?.Invoke();
            }
            catch (Exception ex)
            {
                // Remplace le Console.WriteLine de l'erreur
                System.Windows.MessageBox.Show($"Erreur lors de la demande d'adoption: {ex.Message}");
            }
        }
    }
}
