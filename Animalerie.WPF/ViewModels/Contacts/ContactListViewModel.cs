using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.Models.Listing;
using Animalerie.WPF.Mappers;
using Animalerie.WPF.Models.Contacts;
using Animalerie.WPF.ViewModels.Base;
using System.Windows.Input;

namespace Animalerie.WPF.ViewModels.Contacts
{
    internal class ContactListViewModel : ViewModelBase
    {
        private readonly IContactService _contactService;
        private CancellationTokenSource? _searchCancellationToken;

        // Collections pour la DataGrid
        private IEnumerable<ContactModel> _contacts;
        public IEnumerable<ContactModel> Contacts
        {
            get => _contacts;
            set => SetProperty(ref _contacts, value);
        }

        // Propriétés de Filtres (Binding TwoWay)
        private string? _filterFirstname;
        public string? FilterFirstname
        {
            get => _filterFirstname;
            set
            {
                if (SetProperty(ref _filterFirstname, value))
                {
                    // Debounce logic pour la recherche textuelle
                    ApplyFiltersWithDebounce();
                }
            }
        }

        private string? _filterLastname;
        public string? FilterLastname
        {
            get => _filterLastname;
            set
            {
                if (SetProperty(ref _filterLastname, value))
                {
                    // Debounce logic pour la recherche textuelle
                    ApplyFiltersWithDebounce();
                }
            }
        }

        // Commandes
        public ICommand ResetFiltersCommand { get; }
        public ICommand ShowDetailsCommand { get; }

        // Events
        public event Action<int>? RequestNavigateToDetails;

        public ContactListViewModel(IContactService contactService)
        {
            _contactService = contactService;

            ResetFiltersCommand = new RelayCommand(ResetFilters);
            ShowDetailsCommand = new RelayCommand(param => OnShowDetails(param));
        }

        public void LoadData()
        {
            // Apply filters
            var filters = new ContactFilters
            {
                Firstname = FilterFirstname,
                Lastname = FilterLastname
            };

            Contacts = _contactService.Lister(filters, true).Select(c => c.ToContactModel());
        }

        private async void ApplyFiltersWithDebounce()
        {
            try
            {
                _searchCancellationToken?.Cancel();
                _searchCancellationToken = new CancellationTokenSource();
                var token = _searchCancellationToken.Token;

                await Task.Delay(500, token); // Attendre 500ms

                if (!token.IsCancellationRequested)
                {
                    LoadData();
                }
            }
            catch (TaskCanceledException)
            {
                // Ignorer l'annulation
            }
        }

        private void ResetFilters(object? obj = null)
        {
            FilterFirstname = null;
            FilterLastname = null;

            // notifie la vue que tout a changé
            OnPropertyChanged(nameof(FilterFirstname));
            OnPropertyChanged(nameof(FilterLastname));

            LoadData();
        }

        private void OnShowDetails(object? parameter)
        {
            if (parameter is ContactModel contact)
            {
                RequestNavigateToDetails?.Invoke(contact.Id);
            }
        }
    }
}
