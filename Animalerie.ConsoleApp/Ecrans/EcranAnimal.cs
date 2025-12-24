using Animalerie.BLL.Services.Interfaces;
using Animalerie.Domain.Patterns;
using Tools.ConsoleApp.Input;

namespace Animalerie.ConsoleApp.Ecrans
{
    public class EcranAnimal
    {
        private readonly IAnimalService _animalService;

        public EcranAnimal(IAnimalService animalService)
        {
            _animalService = animalService;
        }

        public void AjouterAnimal()
        {
            Console.Clear();
            Console.WriteLine("Ajout d'un animal:");

            var errors = Inputs.ReadString(
                prompt: "Identifiant (ou tapez 'quit') : ",
                validators: [InputValidator.Match(AnimalPatterns.ID)],
                userInput: out string id,
                exitCondition: InputExitCondition.IsQuitCommand,
                displayError: Inputs.DisplayErrors                
            );

            if (errors.Contains(Inputs.QUIT_ERROR))
            {
                Console.WriteLine("Saisie annulée.");
                return;
            }

            // TODO : Récupérer les autres informations de l'animal
            Console.WriteLine("ID: " + id);
        }
    }
}
