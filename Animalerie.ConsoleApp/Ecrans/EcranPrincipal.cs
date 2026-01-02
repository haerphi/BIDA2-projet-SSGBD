using Animalerie.ConsoleApp.Ecrans;
using System.ComponentModel;
using Tools.ConsoleApp.Input;

namespace Animalerie.ConsoleApp.Screens
{
    internal class EcranPrincipal
    {
        private readonly EcranAnimal _ecranAnimal;

        public EcranPrincipal(EcranAnimal ecranAnimal)
        {
            _ecranAnimal = ecranAnimal;
        }

        public void Display()
        {
            bool continuer = true;

            while (continuer)
            {
                Console.Clear();
                Console.WriteLine("=== GESTION DU REFUGE ANIMAUX ===");
                Console.WriteLine("1.  Ajouter un animal");
                Console.WriteLine("2.  Lister les animaux");
                Console.WriteLine("3.  Consulter un animal (Détails/Modif)");
                Console.WriteLine("4.  Ajouter un contact");
                Console.WriteLine("5.  Lister les contacts");
                Console.WriteLine("6.  Consulter un contact (Détails/Modif)");

                Console.WriteLine("0.  Quitter");
                Console.Write("\nVotre choix : ");

                string? choix = Console.ReadLine();

                try
                {
                    switch (choix)
                    {
                        case "1":
                            _ecranAnimal.Ajouter();
                            break;
                        case "2":
                            _ecranAnimal.Lister();
                            break;
                        case "3":
                            _ecranAnimal.Consulter();
                            break;
                        case "0":
                            continuer = false;
                            break;
                        default:
                            Console.WriteLine("Choix invalide.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"\nErreur : {ex.Message}");
                    Console.ResetColor();
                }
            }
        }

    }
}
