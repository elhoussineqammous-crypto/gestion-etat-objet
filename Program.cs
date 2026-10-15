using gestion_etat_objet.domaine;

namespace gestion_etat_objet
{
    internal class Program
    {
        public static void Main()
        {
            var debut = new DateTime(2026, 10, 1, 9, 0, 0);

            // Exemple d'objet métier : un dossier de crédit
            var dossier = new ObjetMetier("Dossier de crédit n°42", debut);

            dossier.ChangerEtat(Etat.EnCours, "Dossier pris en charge par un conseiller", debut.AddHours(2));
            dossier.ChangerEtat(Etat.Suspendu, "En attente des justificatifs du client", debut.AddDays(1));
            dossier.ChangerEtat(Etat.EnCours, "Justificatifs reçus, reprise de l'étude", debut.AddDays(3));
            dossier.ChangerEtat(Etat.Termine, "Crédit accordé", debut.AddDays(4));
            

            // Tentative interdite : on ne sort pas d'un état final
            try
            {
                dossier.ChangerEtat(Etat.EnCours, "Réouverture du dossier", debut.AddDays(5));
            }
            catch (InvalidOperationException erreur)
            {
                Console.WriteLine($"Refusé : {erreur.Message}");
            }

            Console.WriteLine();
            Console.WriteLine($"{dossier.Nom} (état actuel : {dossier.EtatCourant})");
            Console.Write(dossier.AfficherHistorique());
        }
    }
}
