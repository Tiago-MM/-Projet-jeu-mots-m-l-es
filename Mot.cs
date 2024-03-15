using System;
namespace ProjetInfo
{
    public class Mot : IEquatable<Mot>//Permet le bon fonctionnement de la méthode d'instance juste en bas
    {
        string nom;//mot
        //définir la matrice représentant la localisation du mot
        string direction;
        int ligne;
        int colonne;

        //Constructeur
        public Mot(string nom, string direction, int ligne, int colonne)
        {
            this.nom = nom;
            this.direction = direction;
            this.ligne = ligne;
            this.colonne = colonne;
        }
        //propriétés
        public string Nom
        {
            get { return this.nom; }
            set { this.nom = value; }
        }
        public string Direction
        {
            get { return this.direction; }
            set { this.direction = value; }
        }
        public int Ligne
        {
            get { return this.ligne; }
            set { this.ligne = value; }
        }
        public int Colonne
        {
            get { return this.colonne; }
            set { this.colonne = value; }
        }
        //méthode d'instance qui vérifie si le mot est bien placé au bon endroit
        public bool Equals(Mot other)
        {
            if (this.nom == other.nom && this.ligne == other.ligne
                && this.colonne == other.colonne && this.direction == other.direction)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
