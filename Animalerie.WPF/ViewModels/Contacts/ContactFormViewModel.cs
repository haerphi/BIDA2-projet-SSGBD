using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.Models;
using Animalerie.WPF.ViewModels.Base;
using System.Windows.Input;

namespace Animalerie.WPF.ViewModels.Contacts
{
    internal class ContactFormViewModel : ViewModelBase
    {
        private readonly IContactService _contactService;

        private readonly int? contactId; // mode édition
        private Contact? _exitingContact; // mode édition

        // Champs du formulaire
        private string _nom = string.Empty;
        public string Nom
        {
            get => _nom;
            set => SetProperty(ref _nom, value);
        }

        private string _prenom = string.Empty;
        public string Prenom
        {
            get => _prenom;
            set => SetProperty(ref _prenom, value);
        }

        private string? _rue;
        public string? Rue
        {
            get => _rue;
            set => SetProperty(ref _rue, value);
        }

        private string? _cp;
        public string? Cp
        {
            get => _cp;
            set => SetProperty(ref _cp, value);
        }

        private string? _localite;
        public string? Localite
        {
            get => _localite;
            set => SetProperty(ref _localite, value);
        }

        private string _registreNational = string.Empty;
        public string RegistreNational
        {
            get => _registreNational;
            set => SetProperty(ref _registreNational, value);
        }

        private string? _gsm;
        public string? Gsm
        {
            get => _gsm;
            set => SetProperty(ref _gsm, value);
        }

        private string? _telephone;
        public string? Telephone
        {
            get => _telephone;
            set => SetProperty(ref _telephone, value);
        }

        private string? _email;
        public string? Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public ICommand ValiderAdoptionCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action RequestClose;


        public ContactFormViewModel(IContactService contactService)
        {
            _contactService = contactService;

            ValiderAdoptionCommand = new RelayCommand(ExecuteValider, CanExecuteValider);
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke());


            ResetForm();
        }

        // Mode édition
        public ContactFormViewModel(IContactService contactService, int contactId) : this(contactService)
        {
            this.contactId = contactId;
            _exitingContact = _contactService.Consulter(contactId);
        }

        public void LoadData()
        {
            if (_exitingContact is not null)
            {
                Nom = _exitingContact.Nom;
                Prenom = _exitingContact.Prenom;
                Rue = _exitingContact.Rue;
                Cp = _exitingContact.Cp;
                Localite = _exitingContact.Localite;
                RegistreNational = _exitingContact.RegistreNational;
                Gsm = _exitingContact.Gsm;
                Telephone = _exitingContact.Telephone;
                Email = _exitingContact.Email;

                IsDirty = false;
            }
        }

        private void ResetForm()
        {
            Nom = string.Empty;
            Prenom = string.Empty;
            Rue = string.Empty;
            Cp = string.Empty;
            Localite = string.Empty;
            RegistreNational = string.Empty;
            Gsm = string.Empty;
            Telephone = string.Empty;
            Email = string.Empty;

            IsDirty = false;
        }

        private bool CanExecuteValider(object? parameter)
        {
            return !string.IsNullOrWhiteSpace(Nom)
                && !string.IsNullOrWhiteSpace(Prenom)
                && !string.IsNullOrWhiteSpace(RegistreNational);
        }

        private void ExecuteValider(object? parameter)
        {
            if (contactId.HasValue && _exitingContact is not null)
            {
                // Mode édition
                _exitingContact.Nom = Nom;
                _exitingContact.Prenom = Prenom;
                _exitingContact.Rue = Rue;
                _exitingContact.Cp = Cp;
                _exitingContact.Localite = Localite;
                _exitingContact.RegistreNational = RegistreNational;
                _exitingContact.Gsm = Gsm;
                _exitingContact.Telephone = Telephone;
                _exitingContact.Email = Email;
                //_contactService.Modifier(_exitingContact);
            }
            else
            {
                // Mode création
                var newContact = new Contact(0, Nom, Prenom, Rue, Cp, Localite, RegistreNational, Gsm, Telephone, Email);
                _contactService.Ajouter(newContact);
            }
            IsDirty = false;
            RequestClose?.Invoke();
        }
    }
}
