-- INSERTIONS NÉCESSAIRES

-- Insertion des Rôles (basé sur votre TYPE role_nom)
INSERT INTO ROLE (nom) VALUES
('benevole'), 
('adoptant'), 
('candidat'), 
('famille_accueil');

-- Insertion des Vaccins
INSERT INTO VACCIN (nom) VALUES 
('Rage'), 
('Parvovirose'), 
('Leucose féline'), 
('Coryza');

-- Insertion des types de Compatibilité
INSERT INTO COMPATIBILITE (type) VALUES 
('Enfants de moins de 6 ans'), 
('Chien'), 
('Chat'), 
('Appartement');

-- QUELQUES DATAS
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

CALL ps_ajouter_contact(
        'Pines', -- p_nom
        'Mabel', -- p_prenom
        '90.12.31-456.79', -- p_registre_national
        '618 Gopher Road', -- p_rue
        '97001', -- p_cp
        'Gravity Falls', -- p_localite
        '0612345678', -- p_gsm
        NULL, -- p_telephone
        'pines.mabel@example.com'
     );

CALL ps_ajouter_contact(
        'Cypher', -- p_nom
        'Bill', -- p_prenom
        '00.00.00-000.00', -- p_registre_national
        '618 Gopher Road', -- p_rue
        '97001', -- p_cp
        'Gravity Falls', -- p_localite
        '0612345678', -- p_gsm
        NULL, -- p_telephone
        'cypher.bill@example.com'
     );

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

CALL ps_modifier_compatibilite_animal(
        '25122200000',
        1,
        FALSE,
        'Mauvaise expérience avec des enfants turbulents'
     );

CALL ps_mettre_animal_en_famille_accueil(
        '25122200000', -- p_ani_id
        1, -- p_contact_id
        current_timestamp - interval '5 days'
     );