using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.Models;
using Animalerie.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace Animalerie.WPF.ViewModels.Compaitibilites
{
    internal class CompatibiliteViewModel : ViewModelBase
    {
        private readonly ICompatibiliteService _compatibiliteService;

        public ObservableCollection<CompatibiliteEditItemViewModel> Compatibilites { get; } = new();

        private string _newCompatibiliteType;
        public string NewCompatibiliteType
        {
            get => _newCompatibiliteType;
            set => SetProperty(ref _newCompatibiliteType, value);
        }

        // Commandes
        public ICommand SaveItemCommand { get; }
        public ICommand AddItemCommand { get; }

        public CompatibiliteViewModel(ICompatibiliteService compatibiliteService)
        {
            _compatibiliteService = compatibiliteService;
            SaveItemCommand = new RelayCommand(param => SaveItem(param));
            AddItemCommand = new RelayCommand(AddItem, CanAddItem);
        }

        public void LoadData()
        {
            foreach (var item in Compatibilites)
            {
                item.PropertyChanged -= OnItemPropertyChanged;
            }
            Compatibilites.Clear();

            List<Compatibilite> compatibilites = _compatibiliteService.Lister().ToList();
            foreach (var compatibilite in compatibilites)
            {
                CompatibiliteEditItemViewModel itemVm = new CompatibiliteEditItemViewModel(
                    compatibilite.Id,
                    compatibilite.Type
                );
                itemVm.PropertyChanged += OnItemPropertyChanged;
                Compatibilites.Add(itemVm);
            }
        }

        private void SaveItem(object? param)
        {
            if (param is CompatibiliteEditItemViewModel item && item.IsDirty)
            {
                Compatibilite compatibilite = new Compatibilite(item.Id, item.Type);
                _compatibiliteService.Modifier(compatibilite);

                MessageBox.Show("Compatibilité mis à jour avec succès !");
                item.IsDirty = false;
                IsDirty = Compatibilites.Any(i => i.IsDirty);
            }
        }

        private bool CanAddItem(object? param)
        {
            return !string.IsNullOrWhiteSpace(NewCompatibiliteType);
        }

        private void AddItem(object? param)
        {
            Compatibilite nouvelleCompat = new Compatibilite(0, NewCompatibiliteType);

            _compatibiliteService.Ajouter(nouvelleCompat);

            NewCompatibiliteType = string.Empty;

            MessageBox.Show("Compatibilité ajouté avec succès !");
            LoadData();
        }
    }
}
