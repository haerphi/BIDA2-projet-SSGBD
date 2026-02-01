using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.Models;
using Animalerie.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Animalerie.WPF.ViewModels.Vaccins
{
    internal class VaccinViewModel : ViewModelBase
    {
        private readonly IVaccinService _vaccinService;

        public ObservableCollection<VaccinEditItemViewModel> Vaccins { get; } = new();

        private string _newVaccinNom;
        public string NewVaccinNom
        {
            get => _newVaccinNom;
            set => SetProperty(ref _newVaccinNom, value);
        }

        public ICommand SaveItemCommand { get; }
        public ICommand AddItemCommand { get; }

        public VaccinViewModel(IVaccinService vaccinService)
        {
            _vaccinService = vaccinService;
            SaveItemCommand = new RelayCommand(param => SaveItem(param));
            AddItemCommand = new RelayCommand(AddItem, CanAddItem);
        }

        public void LoadData()
        {
            foreach (var item in Vaccins)
            {
                item.PropertyChanged -= OnItemPropertyChanged;
            }
            Vaccins.Clear();

            var vaccins = _vaccinService.Lister().ToList();
            foreach (var v in vaccins)
            {
                var itemVm = new VaccinEditItemViewModel(v.Id, v.Nom);
                itemVm.PropertyChanged += OnItemPropertyChanged;
                Vaccins.Add(itemVm);
            }
        }

        private void SaveItem(object? param)
        {
            if (param is VaccinEditItemViewModel item && item.IsDirty)
            {
                Vaccin vaccin = new Vaccin()
                {
                    Id = item.Id,
                    Nom = item.Nom
                };
                _vaccinService.MettreAJour(vaccin);

                MessageBox.Show("Vaccin mis à jour avec succès !");
                item.IsDirty = false;
                IsDirty = Vaccins.Any(i => i.IsDirty);
            }
        }

        private bool CanAddItem(object? param) => !string.IsNullOrWhiteSpace(NewVaccinNom);

        private void AddItem(object? param)
        {
            Vaccin nouveauVaccin = new Vaccin() { Nom = NewVaccinNom };
            _vaccinService.Ajouter(nouveauVaccin);

            NewVaccinNom = string.Empty;
            MessageBox.Show("Vaccin ajouté avec succès !");
            LoadData();
        }
    }
}
