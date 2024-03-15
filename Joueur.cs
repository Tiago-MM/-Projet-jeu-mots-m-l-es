using System;
using System.Collections.Generic;
namespace ProjetInfo
{
    public class Joueur
    {
        string nom; //nom du joueur
        List<string> nbmot; //nombre de mot trouvé
        int score; //Score

        public Joueur(string nom, List<string> nbmot, int score)
        {
            this.nom = nom;
            this.nbmot = nbmot;
            this.score = score;
        }
        public string Nom
        {
            get { return this.nom; }
            set { this.nom = value; }
        }
        public int Score
        {
            get { return this.score; }
            set { this.score = value; }
        }
        public List<string> Nbmot
        {
            get { return this.nbmot; }
            set { this.nbmot = value; }
        }

        //méthode qui renvoie un string avec le nombre de mots trouvés
        public void Add_Mot(string mot)
        {
            nbmot.Add(mot);
        }

        // affichage des paramètres à chaque début de manche : nom du joueur en cours, nombre de mot trouvé, score
        public string toString()
        {
            return ("Nom: " + this.nom + "\n" + "Nombre de mot trouvé: " + this.nbmot.Count + "\n" + "Score actuel : " + this.score);
        }

        //méthode qui additionne le nombre de lettres trouvés qui définit le score à chaque mot trouvé
        public void Add_Score(int val)
        {
            this.score += val;
        }
    }
}
