using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.Models;
using Animalerie.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Animalerie.WPF.ViewModels.Animals
{
    public class AnimalEditVaccinationViewModel : ViewModelBase
    {
        private readonly IAnimalService _animalService;
        private readonly IVaccinService _vaccinService;
        private readonly string _animalId;

        // Propriétés pour l'UI
        public ObservableCollection<Vaccination> Vaccinations { get; } = new();
        public ObservableCollection<Vaccin> AvailableVaccins { get; } = new();

        private Vaccin? _selectedVaccin;
        public Vaccin? SelectedVaccin
        {
            get => _selectedVaccin;
            set => SetProperty(ref _selectedVaccin, value);
        }

        private DateTime _newVaccinDate = DateTime.Now;
        public DateTime NewVaccinDate
        {
            get => _newVaccinDate;
            set => SetProperty(ref _newVaccinDate, value);
        }

        // Commandes
        public ICommand AddVaccinationCommand { get; }
        public ICommand DeleteVaccinationCommand { get; }
        public ICommand CloseCommand { get; }

        // events
        public event Action? RequestClose;

        public AnimalEditVaccinationViewModel(IAnimalService animalService, IVaccinService vaccinService, string animalId)
        {
            _animalService = animalService;
            _vaccinService = vaccinService;
            _animalId = animalId;

            AddVaccinationCommand = new RelayCommand(_ => AddVaccination(), _ => SelectedVaccin != null);
            DeleteVaccinationCommand = new RelayCommand(param => DeleteVaccination(param));
            CloseCommand = new RelayCommand(_ => RequestClose?.Invoke());

            LoadData();
        }

        public void LoadData()
        {
            // Charger les vaccinations de l'animal
            Vaccinations.Clear();
            foreach (var v in _animalService.ListerVaccination(_animalId))
            {
                Vaccinations.Add(v);
            }

            // Charger le référentiel des vaccins existants
            AvailableVaccins.Clear();
            foreach (var v in _vaccinService.Lister())
            {
                AvailableVaccins.Add(v);
            }

            IsDirty = false;
        }

        private void AddVaccination()
        {
            if (SelectedVaccin != null)
            {
                _animalService.VaccinerAnimal(_animalId, SelectedVaccin.Id, NewVaccinDate);
                LoadData();
            }
        }

        private void DeleteVaccination(object? param)
        {
            if (param is int vaccinationId)
            {
                _animalService.SupprimerVaccination(vaccinationId);
                LoadData();
            }
        }
    }
}
