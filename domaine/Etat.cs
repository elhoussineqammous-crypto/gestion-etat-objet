using System;
using System.Collections.Generic;
using System.Text;

namespace gestion_etat_objet.domaine
{
    public enum Etat
    {
        Aucun,      // uniquement pour la ligne de création (pas d'état avant)
        Cree,       // état initial
        EnCours,
        Suspendu,
        Termine,    // état final
        Annule
    }
}
