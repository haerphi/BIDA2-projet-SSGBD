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
                Console.WriteLine("2.  Consulter un animal (Détails)");
                Console.WriteLine("3.  Supprimer un animal (Archive)");
                Console.WriteLine("4.  Gérer les informations (Compatibilité, Vaccins)");
                Console.WriteLine("5.  Lister les animaux présents au refuge");
                Console.WriteLine("6.  Gestion des Contacts (Ajout/Modif/Consulter)");
                Console.WriteLine("7.  Gestion des Familles d'accueil (Passage/Attribution)");
                Console.WriteLine("8.  Gestion des Adoptions (Demande/Statut)");
                Console.WriteLine("0.  Quitter");
                Console.Write("\nVotre choix : ");

                string? choix = Console.ReadLine();

                try
                {
                    switch (choix)
                    {
                        case "1":
                            _ecranAnimal.AjouterAnimal();
                            break;
                        //case "2":
                        //    MenuConsulterAnimal(service);
                        //    break;
                        //case "3":
                        //    MenuSupprimerAnimal(service);
                        //    break;
                        //case "4":
                        //    MenuInfosAnimal(service);
                        //    break;
                        //case "5":
                        //    ListerAnimaux(service);
                        //    break;
                        //case "6":
                        //    MenuContacts(service);
                        //    break;
                        //case "7":
                        //    MenuFamillesAccueil(service);
                        //    break;
                        //case "8":
                        //    MenuAdoptions(service);
                        //    break;
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
