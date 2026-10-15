using gestion_etat_objet.domaine;

namespace gestion_etat_objet.Tests
{
    public class ObjetMetierTests
    {
        // Date fixe : les tests donnent toujours le même résultat.
        private static readonly DateTime Debut = new DateTime(2026, 10, 1, 9, 0, 0);

        [Fact]
        public void Un_Nouvel_Objet_Est_Cree_Avec_Une_Ligne_D_Historique()
        {
            var objet = new ObjetMetier("Dossier", Debut);

            Assert.Equal(Etat.Cree, objet.EtatCourant);
            Assert.Single(objet.Historique);
            Assert.True(objet.Historique[0].EstCreation);
        }

        [Fact]
        public void Un_Nom_Vide_Est_Refuse()
        {
            Assert.Throws<ArgumentException>(() => new ObjetMetier(" ", Debut));
        }

        [Fact]
        public void Chaque_Changement_Est_Enregistre_Avec_Date_Et_Description()
        {
            var objet = new ObjetMetier("Dossier", Debut);

            objet.ChangerEtat(Etat.EnCours, "Prise en charge", Debut.AddHours(1));

            var derniere = objet.Historique[1];
            Assert.Equal(Etat.Cree, derniere.AncienEtat);
            Assert.Equal(Etat.EnCours, derniere.NouvelEtat);
            Assert.Equal(Debut.AddHours(1), derniere.Date);
            Assert.Equal("Prise en charge", derniere.Description);
        }

        [Fact]
        public void Un_Passage_Interdit_Est_Refuse_Sans_Rien_Modifier()
        {
            var objet = new ObjetMetier("Dossier", Debut);

            Assert.Throws<InvalidOperationException>(() => objet.ChangerEtat(Etat.Termine, "Trop tôt", Debut.AddHours(1)));

            Assert.Equal(Etat.Cree, objet.EtatCourant);
            Assert.Single(objet.Historique);
        }

        [Fact]
        public void Aucun_Changement_Apres_Un_Etat_Final()
        {
            var objet = new ObjetMetier("Dossier", Debut);
            objet.ChangerEtat(Etat.Annule, "Demande retirée", Debut.AddHours(1));

            Assert.True(objet.EstTermine);
            Assert.Throws<InvalidOperationException>(() => objet.ChangerEtat(Etat.EnCours, "Reprise", Debut.AddHours(2)));
            Assert.Equal(2, objet.Historique.Count);
        }

        [Fact]
        public void Une_Description_Vide_Est_Refusee()
        {
            var objet = new ObjetMetier("Dossier", Debut);

            Assert.Throws<ArgumentException>(() => objet.ChangerEtat(Etat.EnCours, "", Debut.AddHours(1)));
            Assert.Single(objet.Historique);
        }

        [Fact]
        public void Une_Date_Anterieure_Au_Dernier_Changement_Est_Refusee()
        {
            var objet = new ObjetMetier("Dossier", Debut);

            Assert.Throws<InvalidOperationException>(() => objet.ChangerEtat(Etat.EnCours, "Prise en charge", Debut.AddHours(-1)));
            Assert.Single(objet.Historique);
        }

        [Fact]
        public void L_Historique_Garde_Les_Allers_Retours_Dans_L_Ordre()
        {
            var objet = new ObjetMetier("Dossier", Debut);
            objet.ChangerEtat(Etat.EnCours, "Prise en charge", Debut.AddHours(1));
            objet.ChangerEtat(Etat.Suspendu, "Attente client", Debut.AddHours(2));
            objet.ChangerEtat(Etat.EnCours, "Reprise", Debut.AddHours(3));

            Assert.Equal(4, objet.Historique.Count);
            Assert.Equal(Etat.Suspendu, objet.Historique[2].NouvelEtat);
            Assert.Equal(Etat.EnCours, objet.Historique[3].NouvelEtat);
        }

        [Fact]
        public void L_Historique_Ne_Peut_Pas_Etre_Modifie_De_L_Exterieur()
        {
            var objet = new ObjetMetier("Dossier", Debut);
            var commeListe = (IList<TransitionEtat>)objet.Historique;

            Assert.Throws<NotSupportedException>(() => commeListe.Clear());
            Assert.Single(objet.Historique);
        }
    }
}