using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animalerie.Domain.Models.Listing
{
    public class ContactFilters
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? RegistreNational { get; set; }
        public string? Email { get; set; }
    }
}
