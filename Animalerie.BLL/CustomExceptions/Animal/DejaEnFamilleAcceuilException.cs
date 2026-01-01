using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animalerie.BLL.CustomExceptions.Animal
{
    public class DejaEnFamilleAcceuilException : Exception
    {
        public DejaEnFamilleAcceuilException() : base("Cet animal est dejà dans une famille d'accueil.")
        {
        }

        public DejaEnFamilleAcceuilException(string message) : base(message)
        {
        }
    }
}
