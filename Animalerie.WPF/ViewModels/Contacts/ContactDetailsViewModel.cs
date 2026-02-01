using Animalerie.BLL.Services;
using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.WPF.Mappers;
using Animalerie.WPF.Models.Animals;
using Animalerie.WPF.Models.Contacts;
using Animalerie.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Animalerie.WPF.ViewModels.Contacts
{
    internal class ContactDetailsViewModel : ViewModelBase
    {
        private readonly IContactService _contactService;
        private readonly int _contactId;

        private ContactModel? _selectedContact;
        public ContactModel? SelectedContact
        {
            get => _selectedContact;
            set => SetProperty(ref _selectedContact, value);
        }
        public ObservableCollection<AdoptionModel> Adoptions { get; } = new();

        // Commandes
        public ICommand EditContactCommand { get; }
        public ICommand UpdateAdoptionDemandCommand { get; }

        // Events
        public event Action<int> RequestNavigateToEditContact = null!;
        public event Action<int> RequestNavigateToEditAdoptionForm = null!;

        public ContactDetailsViewModel(IContactService contactService, int contactId)
        {
            _contactService = contactService;
            _contactId = contactId;

            EditContactCommand = new RelayCommand(_ => EditContact());
            UpdateAdoptionDemandCommand = new RelayCommand(param => UpdateAdoptionDemand(param));
        }

        public void LoadData()
        {
            SelectedContact = _contactService.Consulter(_contactId, true).ToContactModel();

            if (SelectedContact is not null)
            {
                IEnumerable<AdoptionModel> adoptions = _contactService.ListerAdoptions(_contactId, true).Select(ad => ad.ToAdoptionModel());
                Adoptions.Clear();
                foreach (AdoptionModel ad in adoptions)
                {
                    Adoptions.Add(ad);
                }
            }
        }

        // Command methods
        private void EditContact()
        {
            RequestNavigateToEditContact?.Invoke(_contactId);
        }

        private void UpdateAdoptionDemand(object? param)
        {
            if (param is AdoptionModel ad)
            {
                RequestNavigateToEditAdoptionForm?.Invoke(ad.Id);
            }
        }
    }
}
