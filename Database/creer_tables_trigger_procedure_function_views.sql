/*
    DROP DATABASE RefugeAnimaux WITH (FORCE)
    CREATE DATABASE RefugeAnimaux;
*/

-- ===========================================================================================================================================================
-- TYPES

CREATE TYPE type_animal AS ENUM ('chat', 'chien');
CREATE TYPE sexe_animal AS ENUM ('m', 'f');
CREATE TYPE role_nom AS ENUM ('benevole', 'adoptant', 'candidat', 'famille_accueil');
CREATE TYPE raison_entree AS ENUM ('abandon', 'errant', 'deces_proprietaire', 'saisie', 'retour_adoption', 'retour_famille_accueil');
CREATE TYPE raison_sortie AS ENUM ('adoption', 'retour_proprietaire', 'deces_animal', 'famille_accueil');
CREATE TYPE statut_adoption AS ENUM ('demande', 'acceptee', 'rejet_environnement', 'rejet_comportement');

-- ===========================================================================================================================================================
-- TABLES

CREATE TABLE CONTACT
(
    id                SERIAL PRIMARY KEY,
    nom               VARCHAR(100) NOT NULL CHECK (LENGTH(nom) >= 2),
    prenom            VARCHAR(100) NOT NULL CHECK (LENGTH(prenom) >= 2),
    rue               VARCHAR(255),
    cp                VARCHAR(10),
    localite          VARCHAR(100),
    registre_national CHAR(15)     NOT NULL,
    gsm               VARCHAR(20),
    telephone         VARCHAR(20),
    email             VARCHAR(100),

    CONSTRAINT chk_moyen_contact CHECK (gsm IS NOT NULL OR telephone IS NOT NULL OR email IS NOT NULL),
    CONSTRAINT uq_email UNIQUE (email),
    CONSTRAINT uq_registre_national UNIQUE (registre_national)
);

CREATE TABLE ROLE
(
    id  SERIAL PRIMARY KEY,
    nom role_nom UNIQUE NOT NULL
);

CREATE TABLE VACCIN
(
    id  SERIAL PRIMARY KEY,
    nom VARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE COMPATIBILITE
(
    id   SERIAL PRIMARY KEY,
    type VARCHAR(100) UNIQUE NOT NULL
);

CREATE TABLE ANIMAL
(
    id                 CHAR(11) PRIMARY KEY,
    nom                VARCHAR(100) NOT NULL,
    type               type_animal  NOT NULL,
    sexe               sexe_animal  NOT NULL,
    particularites     TEXT,
    description        TEXT,
    date_sterilisation TIMESTAMPTZ,
    date_naissance     TIMESTAMPTZ  NOT NULL CHECK (date_naissance <= CURRENT_DATE),
    date_deces         TIMESTAMPTZ,
    deleted_at         TIMESTAMPTZ,
    CONSTRAINT chk_id CHECK (id ~ '^[0-9]{11}$'),
    CONSTRAINT chk_sterilisation CHECK (
        date_sterilisation IS NULL OR date_sterilisation > date_naissance
        ),
    CONSTRAINT chk_deces CHECK (date_deces IS NULL OR date_deces > date_naissance)
);

CREATE TABLE ANIMAL_COULEUR
(
    couleur VARCHAR(50),
    ani_id  CHAR(11) REFERENCES ANIMAL (id),
    PRIMARY KEY (couleur, ani_id)
);

CREATE TABLE ANI_ENTREE
(
    id         SERIAL PRIMARY KEY,
    raison     raison_entree                   NOT NULL,
    date       TIMESTAMPTZ                     NOT NULL,
    ani_id     CHAR(11) REFERENCES ANIMAL (id) NOT NULL,
    contact_id INT REFERENCES CONTACT (id)     NOT NULL
);

CREATE TABLE ANI_SORTIE
(
    id         SERIAL PRIMARY KEY,
    raison     raison_sortie                   NOT NULL,
    date       TIMESTAMPTZ                     NOT NULL,
    ani_id     CHAR(11) REFERENCES ANIMAL (id) NOT NULL,
    contact_id INT REFERENCES CONTACT (id)     NOT NULL
);

CREATE TABLE ADOPTION
(
    id           SERIAL PRIMARY KEY,
    statut       statut_adoption                 NOT NULL,
    date_demande TIMESTAMPTZ                     NOT NULL,
    ani_id       CHAR(11) REFERENCES ANIMAL (id) NOT NULL,
    adoptant_id  INT REFERENCES CONTACT (id)     NOT NULL
);

CREATE TABLE FAMILLE_ACCUEIL
(
    id                 SERIAL PRIMARY KEY,
    date_debut         TIMESTAMPTZ                     NOT NULL,
    date_fin           TIMESTAMPTZ,
    ani_id             CHAR(11) REFERENCES ANIMAL (id) NOT NULL,
    famille_accueil_id INT REFERENCES CONTACT (id)     NOT NULL,
    CONSTRAINT chk_dates_accueil CHECK (date_fin IS NULL OR date_fin >= date_debut)
);

CREATE TABLE VACCINATION
(
    id     SERIAL PRIMARY KEY,
    date   TIMESTAMPTZ,
    ani_id CHAR(11) REFERENCES ANIMAL (id) NOT NULL,
    vac_id INT REFERENCES VACCIN (id)      NOT NULL,
    CONSTRAINT uq_vaccin_animal_date UNIQUE (ani_id, vac_id, date)
);

CREATE TABLE ANI_COMPATIBILITE
(
    valeur      BOOLEAN                           NOT NULL,
    description TEXT,
    comp_id     INT REFERENCES COMPATIBILITE (id) NOT NULL,
    ani_id      CHAR(11) REFERENCES ANIMAL (id)   NOT NULL,
    updated_at  TIMESTAMPTZ DEFAULT CURRENT_TIMESTAMP,
    PRIMARY KEY (comp_id, ani_id)
);

CREATE TABLE PERSONNE_ROLE
(
    pers_id INT REFERENCES CONTACT (id),
    rol_id  INT REFERENCES ROLE (id),
    PRIMARY KEY (pers_id, rol_id)
);


-- ===========================================================================================================================================================
-- TRIGGER

-- met à jour updated_at ANI_COMPATIBILITE
CREATE OR REPLACE FUNCTION fn_update_at_ANI_COMPATIBILITE()
    RETURNS TRIGGER AS
$$
BEGIN
    NEW.updated_at = CURRENT_TIMESTAMP;
    RETURN NEW;
END
$$ LANGUAGE plpgsql;
CREATE TRIGGER trg_update_at_ANI_COMPATIBILITE
    BEFORE UPDATE
    ON ANI_COMPATIBILITE
    FOR EACH ROW
EXECUTE FUNCTION fn_update_at_ANI_COMPATIBILITE();


-- Si date_deces IS NOT NULL, l'animal ne peut plus avoir de nouvelles entrees/sorties, vaccination
CREATE OR REPLACE FUNCTION fn_check_animal_status()
    RETURNS TRIGGER AS
$$
BEGIN
    -- Empêcher toute modification/insertion si l'animal est decede (sauf pour la date de decès elle-même)
    IF EXISTS (SELECT 1 FROM ANIMAL WHERE id = NEW.ani_id AND date_deces IS NOT NULL) THEN
        RAISE EXCEPTION 'Action impossible : l''animal est marque comme decede.';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Application du trigger
CREATE TRIGGER trg_check_post_mortem_entree
    BEFORE INSERT
    ON ANI_ENTREE
    FOR EACH ROW
EXECUTE FUNCTION fn_check_animal_status();
CREATE TRIGGER trg_check_post_mortem_sortie
    BEFORE INSERT
    ON ANI_SORTIE
    FOR EACH ROW
EXECUTE FUNCTION fn_check_animal_status();
CREATE TRIGGER trg_check_post_mortem_vaccin
    BEFORE INSERT
    ON VACCINATION
    FOR EACH ROW
EXECUTE FUNCTION fn_check_animal_status();

-- Un animal ne peut pas avoir plusieurs accueils actifs simultanement (un seul ACCUEIL avec date_fin = NULL à la fois)
CREATE OR REPLACE FUNCTION fn_check_accueil_actif()
    RETURNS TRIGGER AS
$$
BEGIN
    IF EXISTS (SELECT 1
               FROM FAMILLE_ACCUEIL
               WHERE ani_id = NEW.ani_id
                 AND date_fin IS NULL) THEN
        RAISE EXCEPTION 'Cet animal est dejà dans une famille d''accueil active.';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Application du trigger
CREATE TRIGGER trg_famille_accueil_unique
    BEFORE INSERT
    ON FAMILLE_ACCUEIL
    FOR EACH ROW
EXECUTE FUNCTION fn_check_accueil_actif();

-- Trigger pour la sortie par adoption
CREATE OR REPLACE FUNCTION fn_check_sortie_adoption()
    RETURNS TRIGGER AS
$$
BEGIN
    IF NEW.raison = 'adoption' THEN
        IF NOT EXISTS (SELECT 1
                       FROM ADOPTION
                       WHERE ani_id = NEW.ani_id
                         AND statut = 'acceptee') THEN
            RAISE EXCEPTION 'Impossible de sortir l''animal pour adoption sans un dossier avec statut "acceptee".';
        END IF;
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Application du trigger
CREATE TRIGGER trg_check_sortie_adoption
    BEFORE INSERT
    ON ANI_SORTIE
    FOR EACH ROW
EXECUTE FUNCTION fn_check_sortie_adoption();

-- Trigger pour la vaccination (doublon le même jour)
CREATE OR REPLACE FUNCTION fn_check_vaccin_doublon()
    RETURNS TRIGGER AS
$$
BEGIN
    IF EXISTS (SELECT 1
               FROM VACCINATION
               WHERE ani_id = NEW.ani_id
                 AND vac_id = NEW.vac_id
                 AND date = NEW.date) THEN
        RAISE EXCEPTION 'L''animal ne peut pas recevoir le même vaccin deux fois le même jour.';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Application du trigger
CREATE TRIGGER trg_no_duplicate_vaccin_day
    BEFORE INSERT
    ON VACCINATION
    FOR EACH ROW
EXECUTE FUNCTION fn_check_vaccin_doublon();

-- Trigger pour la vaccination (date avant naissance)
CREATE OR REPLACE FUNCTION fn_check_vaccin_date_naissance()
    RETURNS TRIGGER AS
$$
DECLARE
    date_naissance TIMESTAMPTZ;
BEGIN
    SELECT a.date_naissance INTO date_naissance FROM ANIMAL a WHERE a.id = NEW.ani_id;
    IF NEW.date < date_naissance THEN
        RAISE EXCEPTION 'La date de vaccination ne peut pas être anterieure à la date de naissance de l''animal.';
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

-- Application du trigger
CREATE TRIGGER trg_check_vaccin_date_naissance
    BEFORE INSERT
    ON VACCINATION
    FOR EACH ROW
EXECUTE FUNCTION fn_check_vaccin_date_naissance();

-- Trigger soft delete animal
CREATE OR REPLACE FUNCTION fn_soft_delete_animal()
    RETURNS TRIGGER AS
$$
BEGIN
    UPDATE ANIMAL
    SET deleted_at = CURRENT_DATE
    WHERE id = OLD.id;
    RETURN OLD;
END;
$$ LANGUAGE plpgsql;

-- Application du trigger
CREATE TRIGGER trg_soft_delete_animal
    BEFORE DELETE
    ON ANIMAL
    FOR EACH ROW
EXECUTE FUNCTION fn_soft_delete_animal();

-- =====================================================================================================
-- PROCEDURES AND FUNCTIONS

-- ajouter une personne de contact
CREATE OR REPLACE PROCEDURE ps_ajouter_contact(
    p_nom VARCHAR,
    p_prenom VARCHAR,
    p_registre_national CHAR,
    p_rue VARCHAR DEFAULT NULL,
    p_cp VARCHAR DEFAULT NULL,
    p_localite VARCHAR DEFAULT NULL,
    p_gsm VARCHAR DEFAULT NULL,
    p_telephone VARCHAR DEFAULT NULL,
    p_email VARCHAR DEFAULT NULL
) AS
$$
BEGIN
    INSERT INTO CONTACT (nom, prenom, rue, cp, localite, registre_national, gsm, telephone, email)
    VALUES (p_nom, p_prenom, p_rue, p_cp, p_localite, p_registre_national, p_gsm, p_telephone, p_email);
END;
$$ LANGUAGE plpgsql;


-- Modifier les coordonnees de la personne de contact
CREATE OR REPLACE PROCEDURE ps_modifier_contact(
    p_contact_id INT,
    p_nom VARCHAR,
    p_prenom VARCHAR,
    p_registre_national CHAR,
    p_rue VARCHAR DEFAULT NULL,
    p_cp VARCHAR DEFAULT NULL,
    p_localite VARCHAR DEFAULT NULL,
    p_gsm VARCHAR DEFAULT NULL,
    p_telephone VARCHAR DEFAULT NULL,
    p_email VARCHAR DEFAULT NULL
) AS
$$
BEGIN
    UPDATE CONTACT c
    SET nom               = COALESCE(p_nom, c.nom),
        prenom            = COALESCE(p_prenom, c.prenom),
        registre_national = COALESCE(p_registre_national, c.registre_national),
        rue               = p_rue,
        cp                = p_cp,
        localite          = p_localite,
        gsm               = p_gsm,
        telephone         = p_telephone,
        email             = p_email
    WHERE c.id = p_contact_id;
END;
$$ LANGUAGE plpgsql;

-- Ajouter un role à un contact
CREATE OR REPLACE PROCEDURE ps_ajouter_role_contact(
    p_contact_id INT,
    p_rol_id INT
) AS
$$
BEGIN
    IF NOT (EXISTS (SELECT 1
                    FROM PERSONNE_ROLE
                    WHERE pers_id = p_contact_id
                      AND rol_id = p_rol_id)) THEN
        INSERT INTO PERSONNE_ROLE (pers_id, rol_id)
        VALUES (p_contact_id, p_rol_id);
    END IF;
END;
$$ LANGUAGE plpgsql;

-- Retirer un role à un contact
CREATE OR REPLACE PROCEDURE ps_retirer_role_contact(
    p_contact_id INT,
    p_rol_id INT
) AS
$$
BEGIN
    DELETE
    FROM PERSONNE_ROLE
    WHERE pers_id = p_contact_id
      AND rol_id = p_rol_id;
END;
$$ LANGUAGE plpgsql;

-- Ajouter un animal avec ses couleurs
CREATE OR REPLACE PROCEDURE ps_ajouter_animal(
    p_contact_id INT,
    p_id CHAR(11),
    p_nom VARCHAR,
    p_type type_animal,
    p_sexe sexe_animal,
    p_date_naissance TIMESTAMPTZ,
    p_couleurs VARCHAR[],
    p_particularites TEXT DEFAULT NULL,
    p_description TEXT DEFAULT NULL,
    p_date_sterilisation TIMESTAMPTZ DEFAULT NULL,
    p_raison raison_entree DEFAULT 'abandon',
    p_entree_date TIMESTAMPTZ DEFAULT CURRENT_DATE
) AS
$$
DECLARE
    couleur VARCHAR;
BEGIN
    INSERT INTO ANIMAL (id, nom, type, sexe, date_naissance, particularites, description, date_sterilisation)
    VALUES (p_id, p_nom, p_type, p_sexe, p_date_naissance, p_particularites, p_description, p_date_sterilisation);

    INSERT INTO ANI_ENTREE (contact_id, raison, date, ani_id)
    VALUES (p_contact_id, p_raison, p_entree_date, p_id);

    IF p_couleurs IS NOT NULL THEN
        FOREACH couleur IN ARRAY p_couleurs
            LOOP
                INSERT INTO ANIMAL_COULEUR (couleur, ani_id)
                VALUES (couleur, p_id);
            END LOOP;
    END IF;

EXCEPTION
    WHEN OTHERS THEN
        RAISE NOTICE 'Erreur lors de l''ajout de l''animal. Annulation des changements. Erreur: %', SQLERRM;
        ROLLBACK;
        RAISE;
END;
$$ LANGUAGE plpgsql;

-- Supprimer un animal (soft delete)
CREATE OR REPLACE PROCEDURE ps_supprimer_animal(
    p_id CHAR(11)
) AS
$$
BEGIN
    UPDATE ANIMAL
    SET deleted_at = CURRENT_DATE
    WHERE id = p_id;
END;
$$ LANGUAGE plpgsql;

-- Ajouter/Modifier une compatibilite sur un animal
CREATE OR REPLACE PROCEDURE ps_modifier_compatibilite_animal(
    p_ani_id CHAR(11),
    p_comp_id INT,
    p_valeur BOOLEAN,
    p_description TEXT DEFAULT NULL
) AS
$$
BEGIN
    -- Si la compatibilite existe dejà
    IF EXISTS (SELECT 1
               FROM ANI_COMPATIBILITE
               WHERE ani_id = p_ani_id
                 AND comp_id = p_comp_id) THEN
        -- Mettre à jour la compatibilite existante
        UPDATE ANI_COMPATIBILITE
        SET valeur      = p_valeur,
            description = p_description
        WHERE ani_id = p_ani_id
          AND comp_id = p_comp_id;
    ELSE
        -- Inserer une nouvelle compatibilite
        INSERT INTO ANI_COMPATIBILITE (ani_id, comp_id, valeur, description)
        VALUES (p_ani_id, p_comp_id, p_valeur, p_description);
    END IF;
END;
$$ LANGUAGE plpgsql;

-- Mettre animal en famille d'accueil
CREATE OR REPLACE PROCEDURE ps_mettre_animal_en_famille_accueil(
    p_ani_id CHAR(11),
    p_famille_accueil_id INT,
    p_date_fin TIMESTAMPTZ DEFAULT NULL
) AS
$$
BEGIN
    -- check si l'animal n'est pas dejà en famille d'accueil
    IF EXISTS (SELECT 1
               FROM FAMILLE_ACCUEIL
               WHERE ani_id = p_ani_id
                 AND (date_fin IS NULL OR date_fin >= CURRENT_DATE)) THEN
        RAISE EXCEPTION 'Cet animal est dejà dans une famille d''accueil.';
    END IF;

    INSERT INTO FAMILLE_ACCUEIL (ani_id, famille_accueil_id, date_debut, date_fin)
    VALUES (p_ani_id, p_famille_accueil_id, CURRENT_DATE, p_date_fin);

    -- sortie de l'animal
    INSERT INTO ANI_SORTIE (ani_id, contact_id, raison, date)
    VALUES (p_ani_id, p_famille_accueil_id, 'famille_accueil', CURRENT_DATE);

    -- si date_fin est fournie, encoder la rentree de l'animal
    IF p_date_fin IS NOT NULL THEN
        INSERT INTO ANI_ENTREE (ani_id, contact_id, raison, date)
        VALUES (p_ani_id, p_famille_accueil_id, 'retour_famille_accueil', p_date_fin);
    END IF;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE PROCEDURE ps_modifier_date_fin_famille_accueil(
    p_accueil_id int,
    p_date_fin TIMESTAMPTZ DEFAULT CURRENT_DATE
) AS
$$
DECLARE
    v_ani_id   CHAR(11);
    v_date_debut TIMESTAMPTZ;
    v_date_fin TIMESTAMPTZ;
BEGIN
    -- vérifier que l'accueil existe
    IF NOT EXISTS (SELECT 1
                   FROM FAMILLE_ACCUEIL
                   WHERE id = p_accueil_id) THEN
        RAISE EXCEPTION 'L''accueil avec l''ID % n''existe pas.', p_accueil_id;
    END IF;

    -- Récupérer l'ani_id et la date de fin actuelle
    SELECT ani_id, date_fin, date_debut
    INTO v_ani_id, v_date_fin, v_date_debut
    FROM FAMILLE_ACCUEIL
    WHERE id = p_accueil_id;

    -- vérifier que la nouvelle date de fin est postérieure à la date de début
    IF p_date_fin IS NOT NULL AND p_date_fin < v_date_debut THEN
        RAISE EXCEPTION 'La date de fin ne peut pas être antérieure à la date de début de l''accueil.';
    END IF;

    -- Mettre à jour la date de fin dans l'accueil
    UPDATE FAMILLE_ACCUEIL
    SET date_fin = p_date_fin
    WHERE id = p_accueil_id;

    -- si la date de fin était NULL et qu'on fournit une date de fin, enregistrer la rentrée de l'animal
    IF v_date_fin IS NULL AND p_date_fin IS NOT NULL THEN
        -- Enregistrer la rentrée de l'animal
        INSERT INTO ANI_ENTREE (ani_id, contact_id, raison, date)
        VALUES (v_ani_id, (SELECT famille_accueil_id FROM FAMILLE_ACCUEIL WHERE id = p_accueil_id),
                'retour_famille_accueil', p_date_fin);
    END IF;

    -- si la date de fin était déjà définie
    IF v_date_fin IS NOT NULL THEN
        -- Supprimer l'entrée existante
        DELETE
        FROM ANI_ENTREE
        WHERE ani_id = v_ani_id
          AND contact_id = (SELECT famille_accueil_id FROM FAMILLE_ACCUEIL WHERE id = p_accueil_id)
          AND raison = 'retour_famille_accueil'
          AND date = v_date_fin;

        -- si la date de fin est donnée, recrée l'entrée
        IF p_date_fin IS NOT NULL THEN
            -- Insérer la nouvelle entrée avec la date mise à jour
            INSERT INTO ANI_ENTREE (ani_id, contact_id, raison, date)
            VALUES (v_ani_id, (SELECT famille_accueil_id FROM FAMILLE_ACCUEIL WHERE id = p_accueil_id),
                    'retour_famille_accueil', p_date_fin);
        END IF;
    END IF;
END;
$$ LANGUAGE plpgsql;

-- Lister les familles d’accueil par où l’animal est passe
CREATE OR REPLACE FUNCTION fn_lister_familles_accueil_animal(
    p_ani_id CHAR(11)
)
    RETURNS TABLE
            (
                id                 INT,
                date_debut         TIMESTAMPTZ,
                date_fin           TIMESTAMPTZ,
                ani_id             CHAR(11),
                famille_accueil_id INT
            )
AS
$$
BEGIN
    RETURN QUERY
        SELECT fa.*
        FROM FAMILLE_ACCUEIL fa
        WHERE fa.ani_id = p_ani_id
        ORDER BY fa.date_debut DESC;
END;
$$ LANGUAGE plpgsql;

-- Lister les animaux accueillis par une famille d’accueil
CREATE OR REPLACE FUNCTION fn_lister_animaux_famille_accueil(
    p_famille_accueil_id INT
)
    RETURNS TABLE
            (
                id          INT,
                ani_id      CHAR(11),
                nom_animal  VARCHAR,
                type_animal type_animal,
                date_debut  TIMESTAMPTZ,
                date_fin    TIMESTAMPTZ
            )
AS
$$
BEGIN
    RETURN QUERY
        SELECT fa.id,
               fa.ani_id,
               a.nom,
               a.type,
               fa.date_debut,
               fa.date_fin
        FROM FAMILLE_ACCUEIL fa
                 JOIN ANIMAL a ON fa.ani_id = a.id
        WHERE fa.famille_accueil_id = p_famille_accueil_id
        ORDER BY fa.date_debut DESC;
END;
$$ LANGUAGE plpgsql;

-- Ajouter une adoption
CREATE OR REPLACE PROCEDURE ps_ajouter_adoption(
    p_ani_id CHAR(11),
    p_adoptant_id INT
) AS
$$
BEGIN
    -- check si l'animal est decede
    IF EXISTS (SELECT 1
               FROM ANIMAL
               WHERE id = p_ani_id
                 AND date_deces IS NOT NULL) THEN
        RAISE EXCEPTION 'Impossible de creer une adoption pour un animal decede.';
    END IF;

    -- check (s'il n'y a pas de sortie) OU (si il n'y a pas d'entree après la dernière sortie et que la dernière sortie est une adoption)
    IF EXISTS (SELECT 1
               FROM ANIMAL a
               WHERE a.id = p_ani_id
                 AND (
                   NOT EXISTS (SELECT 1
                               FROM ANI_SORTIE s
                               WHERE s.ani_id = a.id)
                       OR EXISTS (SELECT 1
                                  FROM ANI_SORTIE s
                                  WHERE s.ani_id = a.id
                                    AND s.date >= (SELECT MAX(e.date)
                                                   FROM ANI_ENTREE e
                                                   WHERE e.ani_id = a.id)
                                    AND s.raison = 'adoption')
                   ))
    THEN

        RAISE EXCEPTION 'Impossible de creer une adoption : l''animal n''est pas actuellement disponible pour adoption.';
    ELSE
        INSERT INTO ADOPTION (ani_id, adoptant_id, statut, date_demande)
        VALUES (p_ani_id, p_adoptant_id, 'demande', CURRENT_DATE);
    END IF;
END;
$$ LANGUAGE plpgsql;

-- Modifier le statut d’une adoption
CREATE OR REPLACE PROCEDURE ps_modifier_statut_adoption(
    p_adoption_id INT,
    p_nouveau_statut statut_adoption
) AS
$$
BEGIN
    UPDATE ADOPTION
    SET statut = p_nouveau_statut
    WHERE id = p_adoption_id;

    -- Si l'adoption est acceptee, enregistrer la sortie de l'animal
    IF p_nouveau_statut = 'acceptee' THEN
        INSERT INTO ANI_SORTIE (ani_id, contact_id, raison, date)
        SELECT ani_id, adoptant_id, 'adoption', CURRENT_DATE
        FROM ADOPTION
        WHERE id = p_adoption_id;
    END IF;
END;
$$ LANGUAGE plpgsql;

-- Ajouter un vaccin (date du vaccin, nom du vaccin, fait ou non fait) à un animal
CREATE OR REPLACE PROCEDURE ps_ajouter_vaccination_animal(
    p_ani_id CHAR(11),
    p_vac_id int,
    p_date TIMESTAMPTZ
) AS
$$
BEGIN
    -- check si l'animal est decede
    IF EXISTS (SELECT 1
               FROM ANIMAL
               WHERE id = p_ani_id
                 AND date_deces IS NOT NULL) THEN
        RAISE EXCEPTION 'Impossible d''ajouter une vaccination pour un animal decede.';
    END IF;


    INSERT INTO VACCINATION (ani_id, vac_id, date)
    VALUES (p_ani_id, p_vac_id, p_date);
END;
$$ LANGUAGE plpgsql;

-- Supprimer un vaccin (d'un animal)
CREATE OR REPLACE PROCEDURE ps_supprimer_vaccination_animal(
    p_ani_id CHAR(11),
    p_vac_id int
) AS
$$
BEGIN
    DELETE
    FROM VACCINATION
    WHERE ani_id = p_ani_id
      AND vac_id = p_vac_id;
END;
$$ LANGUAGE plpgsql;

-- Fonction animal status
CREATE OR REPLACE FUNCTION fn_animal_status(
    p_ani_id CHAR(11)
) RETURNS VARCHAR AS
$$
DECLARE
    v_statut VARCHAR;
BEGIN
    SELECT CASE
               WHEN a.date_deces IS NOT NULL
                   THEN 'decede' || '|' || TO_CHAR(a.date_deces, 'DD/MM/YYYY')

               WHEN NOT EXISTS (SELECT 1
                                FROM ANI_SORTIE s
                                WHERE s.ani_id = a.id AND s.date <= CURRENT_TIMESTAMP
                                  AND s.date >= (SELECT MAX(e.date) FROM ANI_ENTREE e WHERE e.ani_id = a.id AND e.date <= CURRENT_TIMESTAMP))
                   THEN 'present'

               ELSE (SELECT s.raison || '|' || TO_CHAR(s.date, 'DD/MM/YYYY')
                     FROM ANI_SORTIE s
                     WHERE s.ani_id = a.id AND s.date <= CURRENT_TIMESTAMP
                     ORDER BY s.date DESC
                     LIMIT 1)
               END
    INTO v_statut
    FROM ANIMAL a
    WHERE a.id = p_ani_id;

    RETURN v_statut; -- On retourne la variable
END;
$$ LANGUAGE plpgsql;

-- Animal compatibilité
CREATE OR REPLACE FUNCTION fn_animal_compatibilite(
    p_ani_id CHAR(11)
)
    RETURNS TABLE
            (
                comp_type   VARCHAR,
                valeur      BOOLEAN,
                description TEXT,
                updated_at  TIMESTAMPTZ
            )
AS
$$
BEGIN
    RETURN QUERY
        SELECT c.type,
               ac.valeur,
               ac.description,
               ac.updated_at
        FROM ANI_COMPATIBILITE ac
                 JOIN COMPATIBILITE c ON ac.comp_id = c.id
        WHERE ac.ani_id = p_ani_id;
END;
$$ LANGUAGE plpgsql;
-- ===========================================================================================================================================================
-- VIEWS

-- Lister les animaux et leur statut (present ou leur dernière sortie)
CREATE OR REPLACE VIEW vue_animaux AS
SELECT a.*,
       fn_animal_status(a.id) AS Status
FROM ANIMAL a
WHERE a.deleted_at IS NULL;

-- Lister les animaux avec leurs compatibilités
CREATE OR REPLACE VIEW vue_animaux_compatibilites AS
SELECT c.type, ac.*
FROM ani_compatibilite ac
         JOIN compatibilite c ON ac.comp_id = c.id;