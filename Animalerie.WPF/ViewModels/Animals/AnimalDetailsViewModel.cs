using Animalerie.BLL.Services.Interfaces;
using Animalerie.WPF.Mappers;
using Animalerie.WPF.Models.Animals;
using Animalerie.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Security.Permissions;
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

        // Commandes
        public ICommand EditCompatCommand { get; }
        public ICommand PutInHostfamilyCommand { get; }
        public ICommand EditFamilleAccueilCommand { get; }
        public ICommand CreateAdoptionDemandCommand { get; }

        // Events
        public event Action<string> RequestNavigateToEditCompat;
        public event Action<string> RequestNavigateToPutInHostFamily;
        public event Action<int> RequestNavigateToEditHostFamily;
        public event Action<string> RequestNavigateToAdoptionForm;

        public AnimalDetailsViewModel(IAnimalService animalService, string animalId)
        {
            _animalService = animalService;
            _animalId = animalId;

            EditCompatCommand = new RelayCommand(_ => EditCompat());
            PutInHostfamilyCommand = new RelayCommand(_ => PutInHostfamily());
            EditFamilleAccueilCommand = new RelayCommand(param => EditFamilleAccueil(param));
            CreateAdoptionDemandCommand = new RelayCommand(_ => CreateAdoptionDemand());

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
    }
}
