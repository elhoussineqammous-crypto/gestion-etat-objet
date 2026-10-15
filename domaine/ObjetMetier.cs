using System;
using System.Collections.Generic;
using System.Text;

namespace gestion_etat_objet.domaine
{
    public class ObjetMetier
    {
        private readonly EtatHistorique _historique = new EtatHistorique();

        public string Nom { get; }

        // private set : l'état ne change QUE par ChangerEtat, donc toujours avec une trace dans l'historique.
        public Etat EtatCourant { get; private set; }

        public IReadOnlyList<TransitionEtat> Historique => _historique.Transitions;

        // Un état final est un état sans aucun état suivant.
        public bool EstTermine => EtatsSuivants(EtatCourant).Length == 0;

        // Création avec la date du jour.
        public ObjetMetier(string nom) : this(nom, DateTime.Now)
        {
        }

        // Création avec une date choisie (utile pour les tests).
        public ObjetMetier(string nom, DateTime dateCreation)
        {
            if (string.IsNullOrWhiteSpace(nom))
                throw new ArgumentException("Le nom est obligatoire.", nameof(nom));

            Nom = nom;
            EtatCourant = Etat.Cree;

            // La création est la première ligne de l'historique.
            _historique.Ajouter(new TransitionEtat(Etat.Aucun, Etat.Cree, dateCreation, "Création"));
        }

        // Changement d'état avec la date du jour.
        public void ChangerEtat(Etat nouvelEtat, string description)
        {
            ChangerEtat(nouvelEtat, description, DateTime.Now);
        }

        // Changement d'état avec une date choisie (utile pour les tests).
        public void ChangerEtat(Etat nouvelEtat, string description, DateTime date)
        {
            // 1. On vérifie tout AVANT de modifier quoi que ce soit.
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("La description est obligatoire.", nameof(description));

            if (!EtatsSuivants(EtatCourant).Contains(nouvelEtat))
                throw new InvalidOperationException($"Passage impossible de {EtatCourant} à {nouvelEtat}.");

            if (date < _historique.Derniere.Date)
                throw new InvalidOperationException("La date ne peut pas être antérieure au dernier changement.");

            // 2. On enregistre la transition, PUIS on change l'état.
            _historique.Ajouter(new TransitionEtat(EtatCourant, nouvelEtat, date, description));
            EtatCourant = nouvelEtat;
        }

        public string AfficherHistorique()
        {
            return _historique.Afficher();
        }

        // Le cycle de vie : pour chaque état, les états vers lesquels on peut aller.
        // Termine et Annule n'ont aucune sortie : ce sont les états finaux.
        private static Etat[] EtatsSuivants(Etat etat)
        {
            switch (etat)
            {
                case Etat.Cree:
                    return new[] { Etat.EnCours, Etat.Annule };
                case Etat.EnCours:
                    return new[] { Etat.Suspendu, Etat.Termine, Etat.Annule };
                case Etat.Suspendu:
                    return new[] { Etat.EnCours, Etat.Annule };
                default:
                    return new Etat[0];
            }
        }
    }
}
