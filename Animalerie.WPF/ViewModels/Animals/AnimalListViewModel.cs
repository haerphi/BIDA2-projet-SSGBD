using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.CustomEnums.ListingFilters;
using Animalerie.Domain.Models.Listing;
using Animalerie.WPF.Mappers;
using Animalerie.WPF.Models.Animals;
using Animalerie.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Animalerie.WPF.ViewModels.Animals
{
    public class AnimalListViewModel : ViewModelBase
    {
        private readonly IAnimalService _animalService;
        private CancellationTokenSource? _searchCancellationToken;

        // Collections pour les Combobox
        public IEnumerable<TypeAnimal> TypesAvailable { get; }
        public IEnumerable<SexeAnimal> SexesAvailable { get; }
        public IEnumerable<AnimalStatus> StatusAvailable { get; }

        // Collection pour la DataGrid
        private IEnumerable<AnimalListingModel> _animaux;
        public IEnumerable<AnimalListingModel> Animaux
        {
            get => _animaux;
            set => SetProperty(ref _animaux, value);
        }

        // Propriétés de Filtres (Binding TwoWay)
        private string? _filterNom;
        public string? FilterNom
        {
            get => _filterNom;
            set
            {
                if (SetProperty(ref _filterNom, value))
                {
                    // Debounce logic pour la recherche textuelle
                    ApplyFiltersWithDebounce();
                }
            }
        }

        private TypeAnimal? _filterType;
        public TypeAnimal? FilterType
        {
            get => _filterType;
            set { if (SetProperty(ref _filterType, value)) LoadAnimals(); }
        }

        private SexeAnimal? _filterSexe;
        public SexeAnimal? FilterSexe
        {
            get => _filterSexe;
            set { if (SetProperty(ref _filterSexe, value)) LoadAnimals(); }
        }

        private AnimalStatus? _filterStatus;
        public AnimalStatus? FilterStatus
        {
            get => _filterStatus;
            set { if (SetProperty(ref _filterStatus, value)) LoadAnimals(); }
        }

        // Commandes
        public ICommand ResetFiltersCommand { get; }
        public ICommand ShowDetailsCommand { get; }

        // Events
        public event Action<string>? RequestNavigateToDetails;

        public AnimalListViewModel(IAnimalService animalService)
        {
            _animalService = animalService;

            // Initialisation des listes d'enums
            TypesAvailable = Enum.GetValues(typeof(TypeAnimal)).Cast<TypeAnimal>();
            SexesAvailable = Enum.GetValues(typeof(SexeAnimal)).Cast<SexeAnimal>();
            StatusAvailable = Enum.GetValues(typeof(AnimalStatus)).Cast<AnimalStatus>();

            ResetFiltersCommand = new RelayCommand(_ => ResetFilters());
            ShowDetailsCommand = new RelayCommand(param => OpenDetails(param));

            // Chargement initial
            LoadAnimals();
        }

        private void LoadAnimals()
        {
            var filters = new AnimalFilters
            {
                Nom = string.IsNullOrWhiteSpace(FilterNom) ? null : FilterNom,
                Type = FilterType,
                Sexe = FilterSexe,
                AnimalStatus = FilterStatus
            };

            Animaux = _animalService.Lister(filters).Select(a => a.ToAnimalListingModel());
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
                    LoadAnimals();
                }
            }
            catch (TaskCanceledException)
            {
                // Ignorer l'annulation
            }
        }

        private void ResetFilters()
        {
            _filterNom = string.Empty;
            _filterType = null;
            _filterSexe = null;
            _filterStatus = null;

            //// notifie la vue que tout a changé
            OnPropertyChanged(nameof(FilterNom));
            OnPropertyChanged(nameof(FilterType));
            OnPropertyChanged(nameof(FilterSexe));
            OnPropertyChanged(nameof(FilterStatus));

            LoadAnimals();
        }

        private void OpenDetails(object? param)
        {
            if(param is AnimalListingModel a)
            {
                RequestNavigateToDetails?.Invoke(a.Id);
            }
        }
    }
}