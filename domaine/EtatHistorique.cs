using System;
using System.Collections.Generic;
using System.Text;

namespace gestion_etat_objet.domaine
{
    public class EtatHistorique
    {
        
        private readonly List<TransitionEtat> _transitions = new List<TransitionEtat>();

        // Lecture seule : personne ne peut modifier ou effacer l'historique de l'extérieur.
        public IReadOnlyList<TransitionEtat> Transitions => _transitions.AsReadOnly();

        public TransitionEtat Derniere => _transitions[_transitions.Count - 1];

        public void Ajouter(TransitionEtat transition)
        {
            ArgumentNullException.ThrowIfNull(transition);
            _transitions.Add(transition);
        }

        // Affiche l'historique complet : date, ancien état -> nouvel état, description.
        public string Afficher()
        {
            var texte = new StringBuilder();

            foreach (var transition in _transitions)
            {
                string ancien = transition.EstCreation ? "[création]" : transition.AncienEtat.ToString();
                texte.AppendLine($"{transition.Date:dd/MM/yyyy HH:mm}  {ancien,-10} -> {transition.NouvelEtat,-10} {transition.Description}");
            }

            return texte.ToString();
        }
    }
}
