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

        public AnimalPutInHostFamilyViewModel(string animalId, IAnimalService animalService, IContactService contactService)
        {
            _animalId = animalId;
            _animalService = animalService;
            _contactService = contactService;

            SaveCommand = new RelayCommand(
                execute: _ => SaveData(),
                canExecute: _ => SelectedContact != null && SelectedAnimal != null
            );
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke());

            ResetForm();
        }

        public void LoadData()
        {
            _selectedAnimal = _animalService.Consulter(_animalId);
            OnPropertyChanged(nameof(SelectedAnimal));

            ContactsList.Clear();
            foreach (Contact contact in _contactService.Lister())
            {
                ContactsList.Add(contact.ToContactModel());
            }
        }

        private void SaveData()
        {
            if (SelectedAnimal != null && SelectedContact != null)
            {
                try
                {
                    _animalService.MettreEnFamilleAccueil(_animalId, SelectedContact.Id, DateStart, DateEnd);

                    MessageBox.Show("Mise en place effectuée avec succès !");
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
