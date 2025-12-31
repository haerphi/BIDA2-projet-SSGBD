using Animalerie.ConsoleApp.Ecrans;
using System.ComponentModel;

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
                Console.WriteLine("3.  Consulter un animal (Détails)");
                Console.WriteLine("4.  Supprimer un animal (Archive)");
                Console.WriteLine("5.  Gérer les informations (Compatibilité, Vaccins)");
                Console.WriteLine("6.  Lister les animaux présents au refuge");
                Console.WriteLine("7.  Gestion des Contacts (Ajout/Modif/Consulter)");
                Console.WriteLine("8.  Gestion des Familles d'accueil (Passage/Attribution)");
                Console.WriteLine("9.  Gestion des Adoptions (Demande/Statut)");
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

                if (continuer)
                {
                    Console.WriteLine("\nAppuyez sur une touche pour revenir au menu...");
                    Console.ReadKey();
                }
            }
        }

    }
}
