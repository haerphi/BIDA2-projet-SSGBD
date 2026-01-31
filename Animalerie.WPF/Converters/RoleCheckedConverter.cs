using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows.Data;

namespace Animalerie.WPF.Converters
{
    public class RoleCheckedConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is Role currentRole && values[1] is ObservableCollection<Role> selectedRoles)
            {
                return selectedRoles.Any(r => r.Id == currentRole.Id);
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}