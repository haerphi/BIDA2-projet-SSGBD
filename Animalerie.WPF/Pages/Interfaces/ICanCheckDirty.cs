using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animalerie.WPF.Pages.Interfaces
{
    public interface ICanCheckDirty
    {
        bool IsDirty { get; }
    }
}
