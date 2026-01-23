using Animalerie.Domain.Models;
using Animalerie.WPF.ViewModels.Base;

namespace Animalerie.WPF.ViewModels.Animals
{
    public class AnimalEditCompatibiliteItemViewModel : ViewModelBase
    {
        public int CompatibiliteId { get; }
        public string TypeName { get; }

        private bool _valeur;
        public bool Valeur
        {
            get => _valeur;
            set => SetProperty(ref _valeur, value);
        }

        private string? _description;
        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public AnimalEditCompatibiliteItemViewModel(int id, string typeName, bool valeur, string? description)
        {
            CompatibiliteId = id;
            TypeName = typeName;
            Valeur = valeur;
            Description = description;

            IsDirty = false;
        }

        public bool Equals(AniCompatibilite ac)
        {
            if (ac == null) return false;
            return ac.Compatibilite.Id == CompatibiliteId
                && ac.Valeur == Valeur
                && ac.Description == Description;
        }
    }
}
