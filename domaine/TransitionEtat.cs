using System;
using System.Collections.Generic;
using System.Text;

namespace gestion_etat_objet.domaine
{
    public class TransitionEtat
    {
        // Propriétés sans "set" : une fois créée, une transition ne change plus (on ne réécrit pas le passé).
        public Etat AncienEtat { get; }
        public Etat NouvelEtat { get; }
        public DateTime Date { get; }
        public string Description { get; }

        public TransitionEtat(Etat ancienEtat, Etat nouvelEtat, DateTime date, string description)
        {
            AncienEtat = ancienEtat;
            NouvelEtat = nouvelEtat;
            Date = date;
            Description = description;
        }

        public bool EstCreation => AncienEtat == Etat.Aucun;
    }
}
