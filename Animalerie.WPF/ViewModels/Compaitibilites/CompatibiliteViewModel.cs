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

        // Commandes
        public ICommand SaveItemCommand { get; }

        public CompatibiliteViewModel(ICompatibiliteService compatibiliteService)
        {
            _compatibiliteService = compatibiliteService;
            SaveItemCommand = new RelayCommand(param => SaveItem(param));
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
    }
}
