-- Insertion des Rôles (basé sur votre TYPE role_nom)
INSERT INTO ROLE (nom) VALUES
('benevole'), 
('adoptant'), 
('candidat'), 
('Famille_accueil');

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
