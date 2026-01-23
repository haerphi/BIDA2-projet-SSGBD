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

        // Commandes
        public ICommand EditCompatCommand { get; }

        // Events
        public event Action<string> RequestNavigateToEditCompat;

        public AnimalDetailsViewModel(IAnimalService animalService, string animalId)
        {
            _animalService = animalService;
            _animalId = animalId;

            EditCompatCommand = new RelayCommand(_ => EditCompat());

            LoadData();
        }

        private void LoadData()
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
            }
        }

        // Command methods
        private void EditCompat()
        {
            RequestNavigateToEditCompat?.Invoke(_animalId);
        }
    }
}
