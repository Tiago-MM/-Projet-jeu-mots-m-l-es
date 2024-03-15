using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
namespace ProjetInfo
{
    public class Dictionnaire
    {
        //constructeur
        //Liste de mots du dico en sous liste en fonction nombre de lettres, ordre alphabétique
        List<List<string>> mots; //Liste de mots du dico
        string langue; //choix de la Langue du dico

        public Dictionnaire(List<List<string>> mots, string langue)
        {
            this.mots = mots;
            this.langue = langue;
        }

        //lis le fichier et création d'un dictionnaire=conversion de chaque ligne en liste et se rajoute dans la liste de liste
        public static Dictionnaire Creationdictionnaire(string filename, string langue)
        {
            List<List<string>> dictionnaireFR = new List<List<string>>();
            string[] li = System.IO.File.ReadAllLines(@filename);
            for (int i = 0; i < li.Length; i++)
            {
                dictionnaireFR.Add(li[i].Split(' ', ',').ToList());
            }
            return (new Dictionnaire(dictionnaireFR, langue));
        }
        //affichage d'un string retournant le nombre de mots du dictionnaire en fonction du nombre de lettres,
        //la langue choisie et le nombre total de lettres dans une liste de mots qui pour max mots.count()=m
        public string toString()
        {
            int b = 0;
            Console.WriteLine("La langue de ces mots : " + this.langue);
            Thread.Sleep(500); //attente de traitement
            for (int i = 0; i < mots.Count(); i++)
            {
                Console.WriteLine("Il y a " + mots[i].Count() + " mots de " + mots[i][0].Count() + " lettres");
                Thread.Sleep(100);
                b += mots[i].Count();
            }
            return ("Soit un total de " + b + " mots");
        }

        //retourne un boleen pour verifier si le mot indiqué existe dans le dictionnaire donné 
        public bool RechDichoRecursif(string mot, int p = 0) //compteur p 
        {
            if (p != this.mots.Count && this.mots[p].Contains(mot))//this.mot = liste de liste et cela va prendre la premiere liste : this.mot[0] et vérifie si p=taille liste et si la premiere liste p contient bien le mot
            {
                return (true);
            }
            else if (p == this.mots.Count)//quand on rentre dans le else, on a déjà regardé toutes les sous listes donc le mot n'est pas dans le dico
            {
                return false;
            }
            return (RechDichoRecursif(mot, p + 1));//recursif : augmenter p et cela va commencer à la deuxième sous-liste jusqu'a le n-ieme liste jusqu'à ce qu'on trouve le mot 
        }
    }
}
