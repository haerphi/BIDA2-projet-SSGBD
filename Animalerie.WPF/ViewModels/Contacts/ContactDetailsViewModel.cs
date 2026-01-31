using Animalerie.BLL.Services.Interfaces;
using Animalerie.WPF.Mappers;
using Animalerie.WPF.Models.Contacts;
using Animalerie.WPF.ViewModels.Base;
using System.Windows.Input;

namespace Animalerie.WPF.ViewModels.Contacts
{
    internal class ContactDetailsViewModel: ViewModelBase
    {
        private readonly IContactService _contactService;
        private readonly int _contactId;

        private ContactModel? _selectedContact;
        public ContactModel? SelectedContact
        {
            get => _selectedContact;
            set => SetProperty(ref _selectedContact, value);
        }

        // Commandes
        public ICommand EditContactCommand { get; }

        // Events
        public event Action<int> RequestNavigateToEditContact = null!;

        public ContactDetailsViewModel(IContactService contactService, int contactId)
        {
            _contactService = contactService;
            _contactId = contactId;

            EditContactCommand = new RelayCommand(_ => EditContact());
        }

        public void LoadData()
        {
            SelectedContact = _contactService.Consulter(_contactId).ToContactModel();
        }

        // Command methods
        private void EditContact()
        {
            RequestNavigateToEditContact?.Invoke(_contactId);
        }
    }
}
