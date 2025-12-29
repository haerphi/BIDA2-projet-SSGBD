using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animalerie.Domain.Patterns
{
    public static class DatePatterns
    {
        // pattern dd/MM/yyyy or dd-MM-yyyy or dd.MM.yyyy or dd MM yyyy
        public const string DATE_DDMMYYYY = @"^(0[1-9]|[12][0-9]|3[01])[\/\-. ](0[1-9]|1[0-2])[\/\-. ](19|20)\d\d$";
    }
}
