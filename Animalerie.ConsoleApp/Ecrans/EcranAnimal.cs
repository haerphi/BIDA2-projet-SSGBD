using Animalerie.BLL.Services.Interfaces;
using Animalerie.ConsoleApp.Ecrans.Utils;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.CustomEnums.ListingFilters;
using Animalerie.Domain.Models;
using Animalerie.Domain.Models.Listing;
using Animalerie.Domain.Patterns;
using Tools.ConsoleApp.Input;

namespace Animalerie.ConsoleApp.Ecrans
{
    public class EcranAnimal
    {
        private readonly IAnimalService _animalService;
        private readonly IContactService _contactService;

        public EcranAnimal(IAnimalService animalService, IContactService contactService)
        {
            _animalService = animalService;
            _contactService = contactService;
        }

        public void Ajouter()
        {
            Console.Clear();
            Console.WriteLine("Ajout d'un animal:");

            //Animal animal2 = new Animal(
            //    id:  "29122500000",
            //    nom: "White",
            //    type: TypeAnimal.Chien,
            //    sexe: SexeAnimal.M,
            //    particularites: "",
            //    description: "",
            //    dateSterilisation: null,
            //    dateNaissance: DateTime.Now.AddDays(-2),
            //    couleurs: ["Crème", "Blanc"],
            //    dateDeces: null
            //    );
            //_animalService.Ajouter(
            //    animal2,
            //    couleurs: ["Crème", "Blanc"],
            //    contactId: 1,
            //    raison: RaisonEntree.Errant,
            //    dateEntree: DateTime.Now
            //);


            //return;

            // ID
            string defaultId = DateTime.Now.ToString("yyMMdd") + "00000"; // TODO (si temps) récupérer l dernière ID

            var errors = Inputs.ReadString(
                prompt: $"Identifiant (ou tapez 'quit') - défaut  : {defaultId} \n>",
                validators: [InputValidator.Match(AnimalPatterns.ID)],
                userInput: out string inputId,
                defaultValue: defaultId,
                exitCondition: InputExitCondition.IsQuitCommand,
                displayError: Inputs.DisplayErrors
            );

            if (errors.Contains(Inputs.QUIT_ERROR))
            {
                Console.WriteLine("Saisie annulée.");
                return;
            }

            // Nom
            errors = Inputs.ReadString(
                prompt: "Nom de l'animal (ou laisser vide pour annuler) : \n>",
                validators: [InputValidator.IsNotEmpty()],
                userInput: out string inputName,
                exitCondition: InputExitCondition.IsEmptyInput,
                displayError: Inputs.DisplayErrors
            );

            if (errors.Contains(Inputs.QUIT_ERROR))
            {
                Console.WriteLine("Saisie annulée.");
                return;
            }

            // Type
            Console.WriteLine("Animal types:");
            Display.EnumOptions<TypeAnimal>();

            errors = Inputs.ReadEnum<TypeAnimal>(
                prompt: "Type d'animal (ou tapez 'quit') : \n",
                validators: [],
                userInput: out TypeAnimal? inputType,
                defaultValue: null,
                exitCondition: InputExitCondition.IsQuitCommand,
                displayError: Inputs.DisplayErrors
            );

            if (errors.Contains(Inputs.QUIT_ERROR))
            {
                Console.WriteLine("Saisie annulée.");
                return;
            }

            // Sexe
            Console.WriteLine("Animal sexes:");
            Display.EnumOptions<SexeAnimal>();

            errors = Inputs.ReadEnum<SexeAnimal>(
                prompt: "Sexe de l'animal (ou tapez 'quit') : \n>",
                validators: [],
                userInput: out SexeAnimal? inputSexe,
                defaultValue: null,
                exitCondition: InputExitCondition.IsQuitCommand,
                displayError: Inputs.DisplayErrors
            );

            if (errors.Contains(Inputs.QUIT_ERROR))
            {
                Console.WriteLine("Saisie annulée.");
                return;
            }

            // Date naissance
            errors = Inputs.ReadDateTime(
                prompt: "Date de naissance de l'animal au format dd/mm/yyyy (ou tapez 'quit') : \n>",
                preValidators: [InputValidator.Match(DatePatterns.DATE_DDMMYYYY)],
                validators: [],
                userInput: out DateTime? inputBirthDate,
                exitCondition: InputExitCondition.IsQuitCommand,
                displayError: Inputs.DisplayErrors
            );

            if (errors.Contains(Inputs.QUIT_ERROR))
            {
                Console.WriteLine("Saisie annulée.");
                return;
            }

            // Couleurs
            List<string> inputCouleurs = new List<string>();
            do
            {
                errors = Inputs.ReadString(
                    prompt: "Ajouter une couleur à l'animal (ou laisser vide pour terminer) : \n>",
                    validators: [InputValidator.IsNotEmpty()],
                    userInput: out string couleur,
                    exitCondition: InputExitCondition.IsEmptyInput,
                    displayError: Inputs.DisplayErrors
                );
                if (!string.IsNullOrEmpty(couleur))
                {
                    inputCouleurs.Add(couleur);
                }
                else if (inputCouleurs.Count == 0 && errors.Contains(Inputs.QUIT_ERROR))
                {
                    Console.WriteLine("L'animal doit avoir au moins une couleur.");
                }
            } while (inputCouleurs.Count == 0 || !errors.Contains(Inputs.QUIT_ERROR));

            // particularités   
            errors = Inputs.ReadString(
                prompt: "Particularités de l'animal (ou laisser vide pour annuler) : \n>",
                validators: [],
                userInput: out string inputParticularites,
                defaultValue: string.Empty,
                exitCondition: InputExitCondition.IsEmptyInput,
                displayError: Inputs.DisplayErrors
            );

            // description
            errors = Inputs.ReadString(
                prompt: "Description de l'animal (ou laisser vide pour annuler) : \n>",
                validators: [],
                userInput: out string inputDescription,
                defaultValue: string.Empty,
                exitCondition: InputExitCondition.IsEmptyInput,
                displayError: Inputs.DisplayErrors
            );

            // date stérilisation
            errors = Inputs.ReadDateTime(
                prompt: "Date de stérilisation de l'animal au format dd/mm/yyyy (ou laisser vide si non stérilisé) : \n>",
                preValidators: [InputValidator.Match(DatePatterns.DATE_DDMMYYYY)],
                validators: [],
                userInput: out DateTime? inputDateSterilisation,
                exitCondition: InputExitCondition.IsEmptyInput,
                displayError: Inputs.DisplayErrors
            );

            // raison entrée
            Console.WriteLine("Raisons d'entrée:");
            Display.EnumOptions<RaisonEntree>();
            errors = Inputs.ReadEnum<RaisonEntree>(
                prompt: "Raison d'entrée de l'animal (ou tapez 'quit') - défaut érrant : \n>",
                validators: [],
                userInput: out RaisonEntree? inputRaison,
                defaultValue: RaisonEntree.Errant,
                exitCondition: InputExitCondition.IsQuitCommand,
                displayError: Inputs.DisplayErrors
            );

            if (errors.Contains(Inputs.QUIT_ERROR))
            {
                Console.WriteLine("Saisie annulée.");
                return;
            }

            // date de entrée
            errors = Inputs.ReadDateTime(
                prompt: "Date d'entrée de l'animal au format dd/mm/yyyy (ou tapez 'quit') - défaut aujourd'hui : \n>",
                preValidators: [InputValidator.Match(DatePatterns.DATE_DDMMYYYY)],
                validators: [],
                userInput: out DateTime? inputDateEntree,
                defaultValue: DateTime.Now,
                exitCondition: InputExitCondition.IsQuitCommand,
                displayError: Inputs.DisplayErrors
            );
            if (errors.Contains(Inputs.QUIT_ERROR))
            {
                Console.WriteLine("Saisie annulée.");
                return;
            }

            // date de décès
            errors = Inputs.ReadDateTime(
                prompt: "Date de décès de l'animal au format dd/mm/yyyy (ou laisser vide si vivant) : \n>",
                preValidators: [InputValidator.Match(DatePatterns.DATE_DDMMYYYY)],
                validators: [],
                userInput: out DateTime? inputDateDeces,
                exitCondition: InputExitCondition.IsEmptyInput,
                displayError: Inputs.DisplayErrors
            );

            // contact ID
            IEnumerable<Contact> contacts = _contactService.Lister();
            Console.WriteLine("Contacts existants:");
            foreach (var contact in contacts)
            {
                Console.WriteLine($"\t{contact.Id} - {contact.Nom} {contact.Prenom}");
            }
            IEnumerable<int?> contactIds = contacts.Select(c => (int?)c.Id); // TODO (si temps) corriger ce "int?"

            errors = Inputs.ReadInt(
                prompt: "Identifiant du contact associé à l'animal (ou tapez 'quit') : \n>",
                validators: [InputValidator.IsPositive(), InputValidator.IsIn(contactIds)],
                userInput: out int? inputContactId,
                exitCondition: InputExitCondition.IsQuitCommand,
                displayError: Inputs.DisplayErrors
            );

            if (errors.Contains(Inputs.QUIT_ERROR))
            {
                Console.WriteLine("Saisie annulée.");
                return;
            }

            // RECAP
            Console.WriteLine(
                $"Récapitulatif de l'animal à ajouter:\n" +
                $"- ID: {inputId}\n" +
                $"- Nom: {inputName}\n" +
                $"- Type: {inputType}\n" +
                $"- Sexe: {inputSexe}\n" +
                $"- Date de naissance: {inputBirthDate?.ToString("dd/MM/yyyy")}\n" +
                $"- Couleurs: {string.Join(", ", inputCouleurs)}\n" +
                $"- Particularités: {inputParticularites}\n" +
                $"- Description: {inputDescription}\n" +
                $"- Date de stérilisation: {(inputDateSterilisation.HasValue ? inputDateSterilisation.Value.ToString("dd/MM/yyyy") : "Non stérilisé")}\n" +
                $"- Raison d'entrée: {inputRaison}\n" +
                $"- Date d'entrée: {inputDateEntree?.ToString("dd/MM/yyyy")}\n" +
                $"- Date de décès: {(inputDateDeces.HasValue ? inputDateDeces.Value.ToString("dd/MM/yyyy") : "Vivant")}\n" +
                $"- Contact ID: {inputContactId}\n"
            );
            Inputs.ReadConfirmation(
                prompt: "Confirmer l'ajout de cet animal ? (O/n) : \n>",
                userInput: out bool isConfirmed,
                defaultValue: true
            );
            if (!isConfirmed)
            {
                Console.WriteLine("Ajout annulé.");
                return;
            }

            Animal animal = new Animal(
                id: inputId,
                nom: inputName,
                type: inputType.Value,
                sexe: inputSexe.Value,
                particularites: inputParticularites,
                description: inputDescription,
                dateSterilisation: inputDateSterilisation,
                dateNaissance: inputBirthDate.Value,
                couleurs: inputCouleurs.ToArray(),
                dateDeces: inputDateDeces
                );
            _animalService.Ajouter(
                animal,
                couleurs: inputCouleurs.ToArray(),
                contactId: inputContactId.Value,
                raison: inputRaison.Value,
                dateEntree: inputDateEntree.Value
            );
        }

        public void Lister()
        {
            Console.Clear();
            bool displayListAgain = false;
            AnimalFilters filters = new AnimalFilters();
            do
            {
                IEnumerable<Animal> animaux = _animalService.Lister(filters);

                Console.WriteLine("Liste des animeaux");
                string header = $"| {"Id",-11} | {"Nom",-12} | {"Type",-10} | {"Sexe",-5} | {"Naissance",-13} | Status";
                Console.WriteLine(new string('-', header.Length)); // Ligne de séparation
                Console.WriteLine(header);
                Console.WriteLine(new string('-', header.Length));
                foreach (var animal in animaux)
                {
                    Console.WriteLine(animal.ToStringTableau());
                }

                Inputs.ReadConfirmation(
                    prompt: "Voulez-vous filter la liste d'animaux ? (O/n) : \n>",
                    userInput: out displayListAgain,
                    defaultValue: true
                );

                if (displayListAgain)
                {
                    // Nom
                    Inputs.ReadString(
                        prompt: "Filtrer par nom (ou laisser vide pour ne pas filtrer) : \n>",
                        validators: [],
                        userInput: out string? inputName,
                        defaultValue: string.Empty,
                        exitCondition: InputExitCondition.IsEmptyInput,
                        displayError: Inputs.DisplayErrors
                    );
                    filters.Nom = inputName;

                    // Type
                    Display.EnumOptions<TypeAnimal>();
                    Inputs.ReadEnum<TypeAnimal>(
                        prompt: "Filtrer par type d'animal (ou laisser vide pour ne pas filtrer) : \n>",
                        validators: [],
                        userInput: out TypeAnimal? inputType,
                        defaultValue: null,
                        exitCondition: InputExitCondition.IsEmptyInput,
                        displayError: Inputs.DisplayErrors
                    );
                    filters.Type = inputType;

                    // Sexe
                    Display.EnumOptions<SexeAnimal>();
                    Inputs.ReadEnum<SexeAnimal>(
                        prompt: "Filtrer par sexe d'animal (ou laisser vide pour ne pas filtrer) : \n>",
                        validators: [],
                        userInput: out SexeAnimal? inputSexe,
                        defaultValue: null,
                        exitCondition: InputExitCondition.IsEmptyInput,
                        displayError: Inputs.DisplayErrors
                    );
                    filters.Sexe = inputSexe;

                    // Status
                    Console.WriteLine("Statuts disponibles:");
                    Display.EnumOptions<AnimalStatus>();
                    Inputs.ReadEnum<AnimalStatus>(
                        prompt: "Filtrer par statut d'animal (ou laisser vide pour ne pas filtrer) : \n>",
                        validators: [],
                        userInput: out AnimalStatus? inputStatus,
                        defaultValue: null,
                        exitCondition: InputExitCondition.IsEmptyInput,
                        displayError: Inputs.DisplayErrors
                    );
                    filters.AnimalStatus = inputStatus;
                }

            } while (displayListAgain);
        }

        public void Consulter()
        {
            Console.Clear();

            Console.WriteLine("Consultation d'un animal:");
            var errors = Inputs.ReadString(
                prompt: "Entrez l'identifiant de l'animal à consulter (ou tapez 'quit') : \n>",
                validators: [InputValidator.Match(AnimalPatterns.ID)],
                userInput: out string inputId,
                exitCondition: InputExitCondition.IsQuitCommand,
                displayError: Inputs.DisplayErrors
            );

            if (errors.Contains(Inputs.QUIT_ERROR))
            {
                Console.WriteLine("Consultation annulée.");
                return;
            }

            Animal? animal = _animalService.Consulter(inputId);
            if (animal is null)
            {
                Console.WriteLine($"Aucun animal trouvé avec l'identifiant {inputId}.");
                return;
            }

            Console.WriteLine("Détails de l'animal:");
            Console.WriteLine(animal.ToString());

            Console.WriteLine("Compatibilité de l'animal:");
            IEnumerable<AniCompatibilite> compatibilites = _animalService.ListCompatibilites(animal.Id);
            foreach (var comp in compatibilites)
            {
                Console.WriteLine($"\t- {comp.CompType}:  {comp.Valeur}, Description: {comp.Description}, UpdatedAt: {comp.UpdatedAt}");
            }
        }
    }
}
