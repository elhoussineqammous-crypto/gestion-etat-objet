# Suivi des changements d'état d'objets métier

Application C# (.NET 10 ) qui suit les changements d'état d'un objet métier et conserve l'**historique complet** de chaque transition : ancien état, nouvel état, date et description de l'événement.

## Prérequis

- SDK .NET 10
- Visual Studio 2026

## Exécution

```bash
dotnet build      # compiler
dotnet test       # lancer les tests unitaires
dotnet run --project SuiviEtats   # lancer la démonstration
```

Dans Visual Studio : ouvrir le fichier `.sln`, puis **F5** pour la démonstration, et **Test → Exécuter tous les tests** pour les tests.

## Règles de gestion

| ID    | Règle                                                 | Classe                       |
|-------|-------------------------------------------------------|------------------------------|
| RG-01 | Un objet est créé dans l'état `Cree`                  | `ObjetMetier` (constructeur) |
| RG-02 | La création est la 1re ligne de l'historique          | `ObjetMetier` (constructeur) |
| RG-03 | Le nom de l'objet est obligatoire                     | `ObjetMetier` (constructeur) |
| RG-04 | Seuls les passages du cycle de vie sont autorisés     | `ObjetMetier`                |
| RG-05 | Aucun changement depuis un état final                 | `ObjetMetier`                |
| RG-06 | Chaque changement enregistre états, date, description | `TransitionEtat`             |
| RG-07 | La description est obligatoire                        | `ObjetMetier`                |
| RG-08 | Pas de date antérieure au dernier changement          | `ObjetMetier`                |
| RG-09 | Un changement refusé ne modifie rien                  | `ObjetMetier`                |
| RG-10 | L'historique n'est pas modifiable de l'extérieur      | `EtatHistorique`             |
| RG-11 | L'historique complet est affichable                   | `EtatHistorique`             |

## Choix techniques

- **Machine à états** : pour chaque état, la liste des états suivants autorisés est décrite à un seul endroit. Un état sans état suivant est un état final (`Termine`, `Annule`).
- **Historique** : une `List` (ajout à la fin, ordre chronologique conservé), exposée en lecture seule. Chaque `TransitionEtat` est immuable.
- **Exception** pour un passage interdit : c'est une erreur de l'appelant, pas un cas métier normal.

## Tests

Tests xUnit  : un test par règle de gestion, dont le plus important vérifie qu'un changement refusé ne modifie rien.
