using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.Models;
using Animalerie.WPF.Mappers;
using Animalerie.WPF.Models.Animals;
using Animalerie.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Animalerie.WPF.ViewModels.Animals
{
    internal class AnimalDetailsViewModel : ViewModelBase
    {
        private readonly IAnimalService _animalService;
        private readonly string _animalId;

        private AnimalDetailsModel? _selectedAnimal;
        public AnimalDetailsModel? SelectedAnimal
        {
            get => _selectedAnimal;
            set => SetProperty(ref _selectedAnimal, value);
        }

        public ObservableCollection<AnimalCompatibiliteModel> Compatibilites { get; } = new();
        public ObservableCollection<FamilleAccueilModel> FamilleAccueils { get; } = new();
        public ObservableCollection<AdoptionModel> Adoptions { get; } = new();
        public ObservableCollection<Vaccination> Vaccinations { get; } = new();

        // Commandes
        public ICommand EditCompatCommand { get; }
        public ICommand PutInHostfamilyCommand { get; }
        public ICommand EditFamilleAccueilCommand { get; }
        public ICommand CreateAdoptionDemandCommand { get; }
        public ICommand UpdateAdoptionDemandCommand { get; }
        public ICommand UpdateVaccinationCommand { get; }

        // Events
        public event Action<string> RequestNavigateToEditCompat = null!;
        public event Action<string> RequestNavigateToPutInHostFamily = null!;
        public event Action<int> RequestNavigateToEditHostFamily = null!;
        public event Action<string> RequestNavigateToAdoptionForm = null!;
        public event Action<int> RequestNavigateToEditAdoptionForm = null!;
        public event Action<string> RequestNavigateToUpdateVaccination = null!;

        public AnimalDetailsViewModel(IAnimalService animalService, string animalId)
        {
            _animalService = animalService;
            _animalId = animalId;

            EditCompatCommand = new RelayCommand(_ => EditCompat());
            PutInHostfamilyCommand = new RelayCommand(_ => PutInHostfamily());
            EditFamilleAccueilCommand = new RelayCommand(param => EditFamilleAccueil(param));
            CreateAdoptionDemandCommand = new RelayCommand(_ => CreateAdoptionDemand());
            UpdateAdoptionDemandCommand = new RelayCommand(param => UpdateAdoptionDemand(param));
            UpdateVaccinationCommand = new RelayCommand(_ => UpdateVaccinnation());

            // l'appel de LoadData() est appelé depuis la page lors du chargement
        }

        public void LoadData()
        {
            SelectedAnimal = _animalService.Consulter(_animalId).ToAnimalDetailsModel();

            if (SelectedAnimal != null)
            {
                IEnumerable<AnimalCompatibiliteModel> comps = _animalService.ListCompatibilites(_animalId).Select(ac => ac.ToAnimalCompatibiliteModel());
                Compatibilites.Clear();
                foreach (AnimalCompatibiliteModel c in comps)
                {
                    Compatibilites.Add(c);
                }

                IEnumerable<FamilleAccueilModel> familles = _animalService.ListerFamillesAccueil(_animalId, true).Select(fa => fa.ToFamilleAccueilModel());
                FamilleAccueils.Clear();
                foreach (FamilleAccueilModel f in familles)
                {
                    FamilleAccueils.Add(f);
                }

                IEnumerable<AdoptionModel> adoptions = _animalService.ListerAdoptions(_animalId, true).Select(ad => ad.ToAdoptionModel());
                Adoptions.Clear();
                foreach (AdoptionModel ad in adoptions)
                {
                    Adoptions.Add(ad);
                }

                IEnumerable<Vaccination> vaccins = _animalService.ListerVaccination(_animalId);
                Vaccinations.Clear();
                foreach (Vaccination v in vaccins)
                {
                    Vaccinations.Add(v);
                }
            }
        }

        // Command methods
        private void EditCompat()
        {
            RequestNavigateToEditCompat?.Invoke(_animalId);
        }

        private void PutInHostfamily()
        {
            RequestNavigateToPutInHostFamily?.Invoke(_animalId);
        }

        private void EditFamilleAccueil(object? param)
        {
            if (param is FamilleAccueilModel fa)
            {
                RequestNavigateToEditHostFamily?.Invoke(fa.Id);
            }
        }

        private void CreateAdoptionDemand()
        {
            RequestNavigateToAdoptionForm?.Invoke(_animalId);
        }

        private void UpdateAdoptionDemand(object? param)
        {
            if (param is AdoptionModel ad)
            {
                RequestNavigateToEditAdoptionForm?.Invoke(ad.Id);
            }
        }

        private void UpdateVaccinnation()
        {
            RequestNavigateToUpdateVaccination?.Invoke(_animalId);
        }
    }
}
