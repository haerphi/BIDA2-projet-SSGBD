using Animalerie.BLL.Services.Interfaces;
using Animalerie.ConsoleApp.Ecrans.Utils;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.CustomEnums.ListingFilters;
using Animalerie.Domain.Models;
using Animalerie.Domain.Models.Listing;
using Animalerie.Domain.Patterns;
using Tools.ConsoleApp.Input;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Animalerie.ConsoleApp.Ecrans
{
    public class EcranAnimal
    {
        private readonly IAnimalService _animalService;
        private readonly IContactService _contactService;
        private readonly ICompatibiliteService _compatibiliteService;
        private readonly IAdoptionService _adoptionService;

        public EcranAnimal(IAnimalService animalService, IContactService contactService, ICompatibiliteService compatibiliteService, IAdoptionService adoptionService)
        {
            _animalService = animalService;
            _contactService = contactService;
            _compatibiliteService = compatibiliteService;
            _adoptionService = adoptionService;
        }

        public void Ajouter()
        {
            Console.Clear();
            Console.WriteLine("Ajout d'un animal:");

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

            try
            {
                _animalService.Ajouter(
                    animal,
                    couleurs: inputCouleurs.ToArray(),
                    contactId: inputContactId.Value,
                    raison: inputRaison.Value,
                    dateEntree: inputDateEntree.Value
                );
                Console.WriteLine("Animal ajouté avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de l'ajout de l'animal: {ex.Message}");
            }
            Inputs.Pause();
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
                    prompt: "Voulez-vous filter la liste d'animaux ? (o/N) : \n>",
                    userInput: out displayListAgain,
                    defaultValue: false
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

        public void Consulter(string? animalId = null)
        {
            Console.Clear();

            Console.WriteLine("Consultation d'un animal:");
            if (animalId is null)
            {
                var errors = Inputs.ReadString(
                    prompt: "Entrez l'identifiant de l'animal à consulter (ou tapez 'quit') : \n>",
                    validators: [InputValidator.Match(AnimalPatterns.ID)],
                    userInput: out animalId,
                    exitCondition: InputExitCondition.IsQuitCommand,
                    displayError: Inputs.DisplayErrors
                );

                if (errors.Contains(Inputs.QUIT_ERROR) || animalId is null)
                {
                    Console.WriteLine("Consultation annulée.");
                    return;
                }
            }

            Animal? animal = _animalService.Consulter(animalId);
            if (animal is null)
            {
                Console.WriteLine($"Aucun animal trouvé avec l'identifiant {animalId}.");
                return;
            }

            Console.WriteLine("Détails de l'animal:");
            Console.WriteLine(animal.ToString());

            Console.WriteLine("Compatibilité de l'animal:");
            IEnumerable<AniCompatibilite> aniCompatibilites = _animalService.ListCompatibilites(animal.Id);
            foreach (var aniComp in aniCompatibilites)
            {
                Console.WriteLine($"\t- {aniComp.Compatibilite.Type}:  {aniComp.Valeur}, Description: {aniComp.Description}, UpdatedAt: {aniComp.UpdatedAt}");
            }
            Console.WriteLine(new string('-', 5));
            MenuDetails(animalId);
        }

        public void MenuDetails(string animalId)
        {
            bool quitter = false;

            Console.WriteLine("Actions:");
            Console.WriteLine("\t1. Modifier les compatibilités");
            Console.WriteLine("\t2. Lister les familles d'accueil");
            Console.WriteLine("\t3. Mettre en familles d'accueil");
            Console.WriteLine("\t4. Faire une demande d'adoption");
            Console.WriteLine("\t5. Voir les demandes d'adoption");
            Console.WriteLine("\t6. Enregistrer une entrée  [Non implémenté]");
            Console.WriteLine("\t7. Enregistrer une sortie  [Non implémenté]");
            Console.WriteLine("\t0. Retour au menu principal");
            var errors = Inputs.ReadInt(
                prompt: "Choisissez une action : \n>",
                validators: [InputValidator.Range(0, 7)],
                userInput: out int? inputChoice,
                displayError: (IEnumerable<string> errors) => Console.WriteLine("Choix invalide")
            );

            switch (inputChoice)
            {
                case 1:
                    ModifierCompatibilites(animalId);
                    break;
                case 2:
                    ListerFamillesAccueil(animalId);
                    break;
                case 3:
                    MettreEnFamilleAccueil(animalId);
                    break;
                case 4:
                    FaireDemandeAdoption(animalId);
                    break;
                case 5:
                    ListerDemanderAdoption(animalId);
                    break;
                case 6:
                    EnregistrerEntree(animalId);
                    break;
                case 7:
                case 0:
                    quitter = true;
                    break;
            }

            if (!quitter)
            {
                Consulter(animalId);
            }
        }

        public void ModifierCompatibilites(string? animalId = null)
        {
            Console.WriteLine("Modification des compatibilités d'un animal:");

            if (string.IsNullOrEmpty(animalId))
            {
                var errors = Inputs.ReadString(
                    prompt: "Entrez l'identifiant de l'animal à modifier (ou tapez 'quit') : \n>",
                    validators: [InputValidator.Match(AnimalPatterns.ID)],
                    userInput: out animalId,
                    exitCondition: InputExitCondition.IsQuitCommand,
                    displayError: Inputs.DisplayErrors
                );

                if (errors.Contains(Inputs.QUIT_ERROR) || animalId is null)
                {
                    Console.WriteLine("Consultation annulée.");
                    return;
                }
            }

            Animal? animal = _animalService.Consulter(animalId);
            if (animal is null)
            {
                Console.WriteLine($"Aucun animal trouvé avec l'identifiant {animalId}.");
                return;
            }

            List<Compatibilite> allCompatibilites = _compatibiliteService.Lister().ToList();
            List<AniCompatibilite> aniCompatibilites = _animalService.ListCompatibilites(animal.Id).ToList();
            Console.WriteLine("Quels compatibilités souhaitez-vous modofier?");
            foreach (var comp in allCompatibilites)
            {
                AniCompatibilite? aniComp = aniCompatibilites.FirstOrDefault(c => c.Compatibilite.Id == comp.Id);

                if (aniComp is not null)
                {
                    Console.WriteLine($"\t{comp.Id}. {comp.Type}: actuel = {aniComp.Valeur}, description = {aniComp.Description}");
                }
                else
                {
                    Console.WriteLine($"\t{comp.Id}. {comp.Type}: actuel = non défini");
                }
            }

            bool continuerDeModifier = false;
            do
            {
                var errors = Inputs.ReadInt(
                    prompt: "Entrez l'ID de la compatibilité à modifier (ou tapez '0' pour quitter) : \n>",
                    validators: [InputValidator.IsIn(allCompatibilites.Select(c => (int?)c.Id).Append(0))], // TODO (si temps) corriger ce "int?"
                    userInput: out int? inputCompId,
                    displayError: Inputs.DisplayErrors
                );

                if (inputCompId is null || inputCompId == 0)
                {
                    Console.WriteLine("Modification des compatibilités annulée.");
                    continuerDeModifier = false;
                }
                else
                {
                    Compatibilite compatibilite = allCompatibilites.First(c => c.Id == inputCompId);

                    // Valeur
                    errors = Inputs.ReadConfirmation(
                        prompt: $"Est-ce que {animal.Nom} est compatible avec {compatibilite.Type} ? (o/n ou laisser vide pour ne pas indiquer de compatibilité) : \n>",
                        userInput: out bool inputValeur,
                        exitCondition: InputExitCondition.IsEmptyInput,
                        displayError: Inputs.DisplayErrors
                    );
                    // Description
                    errors = Inputs.ReadString(
                        prompt: $"Entrez une description pour cette compatibilité (ou laisser vide pour aucune) : \n>",
                        validators: [],
                        userInput: out string? inputDescription,
                        defaultValue: null,
                        exitCondition: InputExitCondition.IsEmptyInput,
                        displayError: Inputs.DisplayErrors
                    );
                    // Appliquer la modification
                    _animalService.ModifierCompatibilite(
                        aniId: animal.Id,
                        compId: (int)inputCompId,
                        valeur: inputValeur,
                        desc: inputDescription
                    );
                    // Continuer ?
                    Inputs.ReadConfirmation(
                        prompt: "Voulez-vous modifier une autre compatibilité ? (o/N) : \n>",
                        userInput: out continuerDeModifier,
                        defaultValue: false,
                        displayError: Inputs.DisplayErrors
                    );
                }
            } while (continuerDeModifier);
        }

        public void ListerFamillesAccueil(string? animalId = null)
        {
            Console.WriteLine("Liste des familles d'accueil:");

            if (string.IsNullOrEmpty(animalId))
            {
                var errors = Inputs.ReadString(
                    prompt: "Entrez l'identifiant de l'animal à modifier (ou tapez 'quit') : \n>",
                    validators: [InputValidator.Match(AnimalPatterns.ID)],
                    userInput: out animalId,
                    exitCondition: InputExitCondition.IsQuitCommand,
                    displayError: Inputs.DisplayErrors
                );

                if (errors.Contains(Inputs.QUIT_ERROR) || animalId is null)
                {
                    Console.WriteLine("Consultation annulée.");
                    return;
                }
            }

            IEnumerable<FamilleAccueil> familles = _animalService.ListerFamillesAccueil(animalId, true);

            Console.WriteLine($"{"ID",-5} | {"Contact",-12} | {"Date début",-18} | {"Date fin",-18}");

            foreach (var famille in familles)
            {
                Console.WriteLine($"{famille.Id,-5} | {famille.Contact!.Nom,-12} | {famille.DateDebut,-18} | {famille.DateFin,-18}");
            }

            Inputs.Pause("\nAppuyez sur une touche pour revenir au menu de l'animal...");
        }

        public void MettreEnFamilleAccueil(string? animalId = null)
        {
            Console.WriteLine("Mettre un animal en famille d'accueil:");
            if (string.IsNullOrEmpty(animalId))
            {
                var errorsA = Inputs.ReadString(
                    prompt: "Entrez l'identifiant de l'animal à modifier (ou tapez 'quit') : \n>",
                    validators: [InputValidator.Match(AnimalPatterns.ID)],
                    userInput: out animalId,
                    exitCondition: InputExitCondition.IsQuitCommand,
                    displayError: Inputs.DisplayErrors
                );
                if (errorsA.Contains(Inputs.QUIT_ERROR) || animalId is null)
                {
                    Console.WriteLine("Consultation annulée.");
                    return;
                }
            }

            Animal? animal = _animalService.Consulter(animalId);
            if (animal is null)
            {
                Console.WriteLine($"Aucun animal trouvé avec l'identifiant {animalId}.");
                return;
            }

            // contact ID
            IEnumerable<Contact> contacts = _contactService.Lister();
            Console.WriteLine("Contacts existants:");
            foreach (var contact in contacts)
            {
                Console.WriteLine($"\t{contact.Id} - {contact.Nom} {contact.Prenom}");
            }
            IEnumerable<int?> contactIds = contacts.Select(c => (int?)c.Id); // TODO (si temps) corriger ce "int?"
            var errors = Inputs.ReadInt(
                prompt: "Identifiant du contact à mettre en famille d'accueil (ou tapez 'quit') : \n>",
                validators: [InputValidator.IsPositive(), InputValidator.IsIn(contactIds)],
                userInput: out int? inputContactId,
                exitCondition: InputExitCondition.IsQuitCommand,
                displayError: Inputs.DisplayErrors
            );

            if (errors.Contains(Inputs.QUIT_ERROR) || inputContactId is null)
            {
                Console.WriteLine("Saisie annulée.");
                return;
            }

            // Date de fin
            errors = Inputs.ReadDateTime(
                prompt: "Date de fin de la famille d'accueil au format dd/mm/yyyy (ou laisser vide si indéterminée) : \n>",
                preValidators: [InputValidator.Match(DatePatterns.DATE_DDMMYYYY)],
                validators: [],
                userInput: out DateTime? inputDateFin,
                exitCondition: InputExitCondition.IsEmptyInput,
                displayError: Inputs.DisplayErrors
            );

            try
            {
                _animalService.MettreEnFamilleAccueil(
                    animalId: animal.Id,
                    contactId: inputContactId.Value,
                    dateDebut: DateTime.Now,
                    dateFin: inputDateFin
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la mise en famille d'accueil: {ex.Message}");
            }
            Inputs.Pause();
        }

        public void FaireDemandeAdoption(string? animalId = null)
        {
            if (string.IsNullOrEmpty(animalId))
            {
                var errorsA = Inputs.ReadString(
                    prompt: "Entrez l'identifiant de l'animal à adopter (ou tapez 'quit') : \n>",
                    validators: [InputValidator.Match(AnimalPatterns.ID)],
                    userInput: out animalId,
                    exitCondition: InputExitCondition.IsQuitCommand,
                    displayError: Inputs.DisplayErrors
                );

                if (errorsA.Contains(Inputs.QUIT_ERROR) || animalId is null)
                {
                    Console.WriteLine("Consultation annulée.");
                    return;
                }
            }

            Animal? animal = _animalService.Consulter(animalId);
            if (animal is null)
            {
                Console.WriteLine($"Aucun animal trouvé avec l'identifiant {animalId}.");
                return;
            }

            Console.WriteLine("Faire une demande d'adoption:");
            // contact ID
            IEnumerable<Contact> contacts = _contactService.Lister();
            Console.WriteLine("Contacts existants:");
            foreach (var contact in contacts)
            {
                Console.WriteLine($"\t{contact.Id} - {contact.Nom} {contact.Prenom}");
            }
            IEnumerable<int?> contactIds = contacts.Select(c => (int?)c.Id); // TODO (si temps) corriger ce "int?"
            var errors = Inputs.ReadInt(
                prompt: "Identifiant du contact qui fait la demande d'adoption (ou tapez 'quit') : \n>",
                validators: [InputValidator.IsPositive(), InputValidator.IsIn(contactIds)],
                userInput: out int? inputContactId,
                exitCondition: InputExitCondition.IsQuitCommand,
                displayError: Inputs.DisplayErrors
            );
            if (errors.Contains(Inputs.QUIT_ERROR) || inputContactId is null)
            {
                Console.WriteLine("Saisie annulée.");
                return;
            }
            // Note dans la demande
            errors = Inputs.ReadString(
                prompt: "Note à ajouter à la demande d'adoption (ou laisser vide pour aucune) : \n>",
                validators: [],
                userInput: out string? inputNote,
                defaultValue: string.Empty,
                exitCondition: InputExitCondition.IsEmptyInput,
                displayError: Inputs.DisplayErrors
            );

            try
            {
                _adoptionService.Ajouter(
                    animal.Id,
                    inputContactId.Value,
                    inputNote
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erreur lors de la demande d'adoption: {ex.Message}");
            }
            Inputs.Pause();
        }

        public void ListerDemanderAdoption(string? animalId = null)
        {
            if (string.IsNullOrEmpty(animalId))
            {
                var errorsA = Inputs.ReadString(
                    prompt: "Entrez l'identifiant de l'animal à adopter (ou tapez 'quit') : \n>",
                    validators: [InputValidator.Match(AnimalPatterns.ID)],
                    userInput: out animalId,
                    exitCondition: InputExitCondition.IsQuitCommand,
                    displayError: Inputs.DisplayErrors
                );

                if (errorsA.Contains(Inputs.QUIT_ERROR) || animalId is null)
                {
                    Console.WriteLine("Consultation annulée.");
                    return;
                }
            }

            Animal? animal = _animalService.Consulter(animalId);
            if (animal is null)
            {
                Console.WriteLine($"Aucun animal trouvé avec l'identifiant {animalId}.");
                return;
            }

            bool changerStatut = false;
            do
            {
                Console.Clear();
                Console.WriteLine("Liste des demande d'adoptions:");
                IEnumerable<Adoption> demandes = _animalService.ListerAdoptions(animal.Id);
                Console.WriteLine(Adoption.TableauEntete());
                Console.WriteLine(new string('-', Adoption.TableauEntete().Length));
                foreach (var demande in demandes)
                {
                    Console.WriteLine(demande.ToStringTableau());
                }
                IEnumerable<int?> demandeIds = demandes.Select(d => (int?)d.Id); // TODO (si temps) corriger ce "int?"
                var errors = Inputs.ReadInt(
                    prompt: "Entrez l'ID de la demande d'adoption à modifier (ou laisser vide pour annuler) : \n>",
                    validators: [InputValidator.IsIn(demandeIds)],
                    userInput: out int? inputDemandeId,
                    displayError: Inputs.DisplayErrors,
                    exitCondition: InputExitCondition.IsEmptyInput
                );

                if (inputDemandeId is not null)
                {
                    changerStatut = true;
                    // afficher la demande sélectionnée
                    Adoption demande = demandes.First(d => d.Id == inputDemandeId);
                    Console.WriteLine("Demande sélectionnée:");
                    Console.WriteLine(demande);
                    Console.WriteLine(new string('-', 10));

                    // Nouveau statut
                    Console.WriteLine("Statuts disponibles:");
                    Display.EnumOptions<StatutAdoption>();

                    errors = Inputs.ReadEnum<StatutAdoption>(
                        prompt: "Nouveau statut de la demande d'adoption (ou laisser vide pour ne pas modifier) : \n>",
                        validators: [],
                        userInput: out StatutAdoption? inputStatut,
                        displayError: Inputs.DisplayErrors,
                        exitCondition: InputExitCondition.IsEmptyInput,
                        defaultValue: null
                    );

                    errors = Inputs.ReadString(
                        prompt: "Note à ajouter à la demande d'adoption (ou laisser vide pour aucune) : \n>",
                        validators: [],
                        userInput: out string? inputNote,
                        defaultValue: string.Empty,
                        exitCondition: InputExitCondition.IsEmptyInput,
                        displayError: Inputs.DisplayErrors
                    );

                    errors = Inputs.ReadConfirmation(
                        prompt: "Confirmer la modification de la demande d'adoption ? (O/n) : \n>",
                        userInput: out changerStatut,
                        defaultValue: true
                    );
                    if (changerStatut)
                    {
                        try
                        {
                            _adoptionService.Modifier(demande.Id,
                                 inputStatut ?? demande.Statut,
                                 inputNote
                            );
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erreur lors de la modification du statut de la demande d'adoption: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Modification annulée.");
                    }

                    Inputs.Pause();
                }
                else
                {
                    changerStatut = false;
                }
            } while (changerStatut);
        }

        public void EnregistrerEntree(string? animalId = null)
        {
            if (string.IsNullOrEmpty(animalId))
            {
                var errors = Inputs.ReadString(
                    prompt: "Entrez l'identifiant de l'animal à modifier (ou tapez 'quit') : \n>",
                    validators: [InputValidator.Match(AnimalPatterns.ID)],
                    userInput: out animalId,
                    exitCondition: InputExitCondition.IsQuitCommand,
                    displayError: Inputs.DisplayErrors
                );

                if (errors.Contains(Inputs.QUIT_ERROR) || animalId is null)
                {
                    Console.WriteLine("Consultation annulée.");
                    return;
                }
            }

            Animal? animal = _animalService.Consulter(animalId);
            if (animal is null)
            {
                Console.WriteLine($"Aucun animal trouvé avec l'identifiant {animalId}.");
                return;
            }

            if (animal.Status!.StartsWith("famille_accueil"))
            {
                // verifier si la date de fin est déjà définie
                FamilleAccueil? currentAccueil = _animalService.FamilleAccueilActuelle(animal.Id, true);

                if (currentAccueil is not null)
                {
                    Console.WriteLine("L'animal est actuellement en famille d'accueil.");
                    Console.WriteLine($"\t Contact: {currentAccueil.Contact!.Nom} (ID: {currentAccueil.Contact.Id})\n" +
                        $"\t Du {currentAccueil.DateDebut.ToString("dd/MM/yyyy")}\n" +
                        $"\t Au {currentAccueil.DateFin?.ToString("dd/MM/yyyy") ?? "indéterminé"}");

                    // proposer de clôturer la famille d'accueil
                    var errors = Inputs.ReadConfirmation(
                        prompt: "Voulez-vous modifier la date de fin? (o/N) : \n>",
                        userInput: out bool mettreAJourDateDeFin,
                        defaultValue: false,
                        displayError: Inputs.DisplayErrors
                    );

                    if (mettreAJourDateDeFin)
                    {
                        errors = Inputs.ReadDateTime(
                            prompt: "Date de fin de la famille d'accueil au format dd/mm/yyyy (ou laisser vide si indéterminée) : \n>",
                            preValidators: [InputValidator.Match(DatePatterns.DATE_DDMMYYYY)],
                            validators: [],
                            userInput: out DateTime? inputDateFin,
                            exitCondition: InputExitCondition.IsEmptyInput,
                            defaultValue: null,
                            displayError: Inputs.DisplayErrors
                        );

                        try
                        {
                            _animalService.ModifierDateFinFamilleAccueil(
                                accueilId: currentAccueil.Id,
                                dateFin: inputDateFin
                            );
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erreur lors de la mise à jour de la date de fin de la famille d'accueil: {ex.Message}");
                        }
                    }
                }
                else
                {
                    throw new Exception($"Probleme avec l'animal {animal.Id} et son status {animal.Status}");
                }
            }
            // TODO GERER LES AUTRES STATUS SI BESOIN
        }
    }
}
