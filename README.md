# Projet SGBD - BIDA2 - Animalerie

Ce projet a été réalisé dans le cadre du cours de Projet de Gestion de Bases de Données (SGBD).
Il s'agit d'une application de gestion pour une animalerie, permettant de gérer les animaux et les personnes de contact.

## Détails de l'application

L'application a été développée en utilisant PostgreSQL et .NET 8.0.

### Structure de l'application

L'application est structurée en 6 dossiers principaux (dont 5 projets .NET) :

- **Database**: dossier qui contient le script SQL pour la création de la base de données PostgreSQL (table, procédure stockée...).
- **Tools.ConsoleApp**: projet qui regroupe des outils pour les applications console.
- **Tools.Database**: projet qui contient des méthodes d'extension pour la class `DbConnection` et facilitant les interactions avec la base de données.
- **Animalerie.DAL**: projet qui contient toutes les interactions avec la base de données, y compris les entités et les migrations.
- **Animalerie.BLL**: projet qui contient la logique métier de l'application et utilise le DAL pour interagir avec la base de données.
- **Animalerie.ConsoleApp**: projet qui contient l'application console principale permettant d'interagir avec l'utilisateur et d'utiliser la BLL pour gérer les animaux et les personnes de contact.

## Pré-requis

- PostgreSQL installé et en cours d'exécution.
- .NET 8.0 SDK installé.

## Instructions d'installation

1. Cloner le dépôt GitHub.
2. Exécuter les script SQL `creer_tables_trigger_procedure_function_views.sql` et `inserts.sql` dans le dossier `Database` pour créer la base de données.
3. Ouvrir le fichier de solution dans un IDE compatible avec .NET (comme Visual Studio ou JetBrains Rider).
