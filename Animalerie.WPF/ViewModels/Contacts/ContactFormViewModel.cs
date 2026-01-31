using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using Animalerie.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
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

        private ObservableCollection<Role> _roles = new();
        public ObservableCollection<Role> Roles
        {
            get => _roles;
            set => SetProperty(ref _roles, value);
        }

        private List<Role> _allRoles = new();
        public List<Role> AllRoles
        {
            get => _allRoles;
            set => SetProperty(ref _allRoles, value);
        }

        public ICommand ToggleRoleCommand { get; }
        public ICommand ValiderAdoptionCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action RequestClose;


        public ContactFormViewModel(IContactService contactService)
        {
            _contactService = contactService;

            ToggleRoleCommand = new RelayCommand(p => ToggleRole((Role)p!));
            ValiderAdoptionCommand = new RelayCommand(ExecuteValider, CanExecuteValider);
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke());


            ResetForm();
        }

        // Mode édition
        public ContactFormViewModel(IContactService contactService, int contactId) : this(contactService)
        {
            this.contactId = contactId;
            _exitingContact = _contactService.Consulter(contactId, true);
        }

        public void LoadData()
        {
            AllRoles = _contactService.ListerRoles().ToList();

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
                var contactRoles = AllRoles.Where(ar =>
                                    _exitingContact.Roles.Any(er => er.RolId == ar.Id)
                                ).ToList();

                Roles = new ObservableCollection<Role>(contactRoles);

                IsDirty = false;
            }
        }

        private void ToggleRole(Role role)
        {
            // Comparaison par ID car ce sont des entités de base de données
            var existingRole = Roles.FirstOrDefault(r => r.Id == role.Id);
            if (existingRole != null)
                Roles.Remove(existingRole);
            else
                Roles.Add(role);

            IsDirty = true;
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
                _exitingContact.Roles = Roles.Select(r => new PersonneRole { RolId = r.Id, Nom = r.Nom }).ToList();
                _contactService.MettreAJour(_exitingContact);
            }
            else
            {
                // Mode création
                var newContact = new Contact(0, Nom, Prenom, Rue, Cp, Localite, RegistreNational, Gsm, Telephone, Email);
                newContact.Roles = Roles.Select(r => new PersonneRole { RolId = r.Id, Nom = r.Nom }).ToList();
                _contactService.Ajouter(newContact);
            }
            IsDirty = false;
            RequestClose?.Invoke();
        }
    }
}
