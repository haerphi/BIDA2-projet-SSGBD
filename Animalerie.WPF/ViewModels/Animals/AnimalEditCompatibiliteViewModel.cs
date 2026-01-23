using Animalerie.BLL.Services;
using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.Models;
using Animalerie.WPF.Mappers;
using Animalerie.WPF.Models.Animals;
using Animalerie.WPF.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Animalerie.WPF.ViewModels.Animals
{
    public class AnimalEditCompatibiliteViewModel : ViewModelBase
    {
        private readonly IAnimalService _animalService;
        private readonly ICompatibiliteService _compatibiliteService;
        private readonly string _animalId;
        private List<Compatibilite> _allCompatibilites = [];
        private List<AniCompatibilite> _aniCompatibilites = [];

        public ObservableCollection<AnimalEditCompatibiliteItemViewModel> CompatibiliteList { get; } = new();

        // Commands
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event Action RequestClose;

        public AnimalEditCompatibiliteViewModel(IAnimalService animalService, ICompatibiliteService compatibiliteService, string animalId)
        {
            _animalService = animalService;
            _compatibiliteService = compatibiliteService;
            _animalId = animalId;

            SaveCommand = new RelayCommand(_ => SaveData());
            CancelCommand = new RelayCommand(_ => RequestClose?.Invoke());

            LoadData();
        }

        private void LoadData()
        {
            foreach (var item in CompatibiliteList)
            {
                item.PropertyChanged -= OnItemPropertyChanged;
            }
            CompatibiliteList.Clear();

            Animal animal = _animalService.Consulter(_animalId);
            _allCompatibilites = _compatibiliteService.Lister().ToList();
            _aniCompatibilites = _animalService.ListCompatibilites(_animalId).ToList();

            foreach (var compatibilite in _allCompatibilites)
            {
                AniCompatibilite? existing = _aniCompatibilites.FirstOrDefault(e => e.Compatibilite.Id == compatibilite.Id);

                AnimalEditCompatibiliteItemViewModel itemVm = new AnimalEditCompatibiliteItemViewModel(
                    compatibilite.Id,
                    compatibilite.Type,
                    existing != null ? existing.Valeur : false,
                    existing != null ? existing.Description : string.Empty
                );
                itemVm.PropertyChanged += OnItemPropertyChanged; // utiliser pour la propagation de l'état "IsDirty"

                CompatibiliteList.Add(itemVm);
            }
        }

        private void SaveData()
        {
            int saved = 0;
            foreach (var item in CompatibiliteList)
            {
                if (item.IsDirty)
                {
                    AniCompatibilite? ac = _aniCompatibilites.FirstOrDefault(a => a.Compatibilite.Id == item.CompatibiliteId);
                    if (ac is null || !item.Equals(ac))
                    {
                        _animalService.ModifierCompatibilite(
                            aniId: _animalId,
                            compId: item.CompatibiliteId,
                            valeur: item.Valeur,
                            desc: item.Description
                        );
                        saved++;
                    }
                }
            }

            System.Diagnostics.Debug.WriteLine("Sauvegarde effectuée pour " + saved + " éléments.");

            RequestClose?.Invoke();
        }
    }
}
