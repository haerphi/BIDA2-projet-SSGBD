using Animalerie.WPF.ViewModels.Base;

namespace Animalerie.WPF.ViewModels.Compaitibilites
{
    internal class CompatibiliteEditItemViewModel: ViewModelBase
    {
        public int Id { get; }

        private string _type;
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public CompatibiliteEditItemViewModel(int id, string type)
        {
            Id = id;
            Type = type;

            IsDirty = false;
        }
    }
}
