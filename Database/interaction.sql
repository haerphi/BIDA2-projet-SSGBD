-- ajouter une personne de contact
CALL ps_ajouter_contact(
        'Pines', -- p_nom
        'Dipper', -- p_prenom
        '90.12.31-456.78', -- p_registre_national
        '618 Gopher Road', -- p_rue
        '97001', -- p_cp
        'Gravity Falls', -- p_localite
        '0612345678', -- p_gsm
        NULL, -- p_telephone
        'pines.dipper@example.com'
     );

-- Modifier les coordonnées de la personne de contact
CALL ps_modifier_contact(
        1, -- p_contact_id
        'Pines', -- p_nom
        'Dipper', -- p_prenom
        '90.12.31-456.78', -- p_registre_national
        '618 Gopher Road', -- p_rue
        '97001', -- p_cp
        'Gravity Falls', -- p_localite
        NULL, -- p_gsm
        NULL, -- p_telephone
        'pines.dipper@example.com'
     );

-- Liste des contacts
SELECT *
FROM contact;

-- Ajouter un rôle à une personne de contact
CALL ps_ajouter_role_contact(
        1, -- p_contact_id
        2 -- p_rol_nom
     );

CALL ps_ajouter_role_contact(
        1, -- p_contact_id
        3 -- p_rol_nom
     );

CALL ps_ajouter_role_contact(
        1, -- p_contact_id
        4 -- p_rol_nom
     );

-- Supprimer un rôle à une personne de contact
CALL ps_retirer_role_contact(
        1, -- p_contact_id
        3 -- p_rol_nom
     );

-- list des contacts avec leurs rôles
SELECT c.id, c.nom, c.prenom, r.nom AS role
FROM contact c
         JOIN personne_role pr ON c.id = pr.pers_id
         JOIN role r ON pr.rol_id = r.id
ORDER BY c.id;

-- Ajouter un animal
CALL ps_ajouter_animal(
        1, -- p_contact_id
        '25122200000', -- p_id (yymmddxxxxx)
        'Rex', -- p_nom
        'chien', -- p_type (de type type_animal)
        'm', -- p_sexe (de type sexe_animal)
        '2023-05-15', -- p_date_naissance
        ARRAY ['Noir', 'Blanc'] -- p_couleurs (tableau de VARCHAR)
     );

CALL ps_ajouter_animal(
        1, -- p_contact_id
        '25122200001', -- p_id (yymmddxxxxx)
        'Cerber', -- p_nom
        'chien', -- p_type (de type type_animal)
        'm', -- p_sexe (de type sexe_animal)
        '2023-05-15', -- p_date_naissance
        ARRAY ['Noir', 'Rouge'] -- p_couleurs (tableau de VARCHAR)
     );

-- TODO Consulter un animal (`select`)
SELECT *
FROM animal;
SELECT *
FROM ani_entree;

-- Supprimer un animal
CALL ps_supprimer_animal(
        '25122200000' -- p_id (yymmddxxxxx)
     );

-- Ajouter/Modifier une compatibilité sur un animal
CALL ps_modifier_compatibilite_animal(
        '25122200000',
        1,
        TRUE,
        NULL
     );

CALL ps_modifier_compatibilite_animal(
        '25122200000',
        1,
        FALSE,
        'Mauvaise expérience avec des enfants turbulents'
     );

-- Lister les compatibilités d’un animal
SELECT c.type, ac.valeur, ac.description
FROM ani_compatibilite ac
         JOIN compatibilite c ON ac.comp_id = c.id
WHERE ac.ani_id = '25122200000';

-- Lister tout les animaux
SELECT *
FROM vue_animaux;

-- Lister les animaux présents au refuge
SELECT *
FROM vue_animaux
    WHERE status = 'present';



-- Ajouter une nouvelle famille d’accueil à un animal (la date d’arrivée et la personne de contact sont obligatoires)
CALL ps_ajouter_famille_accueil_animal(
        '25122200000', -- p_ani_id
        1 -- p_contact_id
     );

CALL ps_modifier_date_fin_famille_accueil(
        '25122200000', -- p_ani_id
        current_timestamp -- p_date_fin
     );

-- Lister les familles d’accueil par où l’animal est passé
SELECT *
FROM fn_lister_familles_accueil_animal('25122200000');

-- Lister les animaux accueillis par une famille d’accueil
SELECT *
FROM fn_lister_animaux_famille_accueil(1);

-- Ajouter une adoption
CALL ps_ajouter_adoption(
        '25122200000', -- p_ani_id
        1 -- p_contact_id
     );

-- Modifier le statut d’une adoption
CALL ps_modifier_statut_adoption(
        1, -- p_adoption_id
        'acceptee' -- p_nouveau_statut (de type statut_adoption)
     );

-- Lister les adoptions et leur statut
SELECT * FROM adoption;
SELECT * FROM ani_sortie;

-- Ajouter un vaccin (date du vaccin, nom du vaccin, fait ou non fait) à un animal
CALL ps_ajouter_vaccination_animal(
        '25122200000', -- p_ani_id
        1, -- p_vaccin_id
        current_timestamp -- p_date_vaccin
     );

CALL ps_ajouter_vaccination_animal(
        '25122200000', -- p_ani_id
        1, -- p_vaccin_id
        NULL -- p_date_vaccin (non fait)
     );

-- Supprimer un vaccin (d'un animal)
CALL ps_supprimer_vaccination_animal(
        '25122200000', -- p_ani_id
        1 -- p_vaccin_id
     );

-- Lister les vaccins
SELECT * FROM vaccin;

-- Lister les vaccinations d’un animal
SELECT *
FROM vaccination
WHERE ani_id = '25122200000';