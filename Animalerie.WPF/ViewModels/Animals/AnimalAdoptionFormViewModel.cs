using Animalerie.BLL.Services.Interfaces;
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
    internal class AnimalAdoptionFormViewModel: ViewModelBase
    {
        private readonly IAnimalService _animalService;
        private readonly IContactService _contactService;
        private readonly IAdoptionService _adoptionService;
        private readonly string _animalId;

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

        public ICommand ValiderAdoptionCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action RequestClose;

        public AnimalAdoptionFormViewModel(IAnimalService animalService, IContactService contactService, IAdoptionService adoptionService, string animalId)
        {
            _animalService = animalService;
            _contactService = contactService;
            _adoptionService = adoptionService;
            _animalId = animalId;

            ValiderAdoptionCommand = new RelayCommand(ExecuteValider, CanExecuteValider);
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke());

        }

        public void LoadData()
        {
            SelectedAnimal = _animalService.Consulter(_animalId).ToAnimalDetailsModel();
            var liste = _contactService.Lister().Select(c => c.ToContactModel());
            Contacts = new ObservableCollection<ContactModel>(liste);

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
                _adoptionService.Ajouter(SelectedAnimal!.Id, SelectedContact!.Id, Note);
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
