using Animalerie.WPF.ViewModels.Base;

namespace Animalerie.WPF.ViewModels.Vaccins
{
    internal class VaccinEditItemViewModel : ViewModelBase
    {
        public int Id { get; }

        private string _nom;
        public string Nom
        {
            get => _nom;
            set => SetProperty(ref _nom, value);
        }

        public VaccinEditItemViewModel(int id, string nom)
        {
            Id = id;
            Nom = nom;

            IsDirty = false;
        }
    }
}
