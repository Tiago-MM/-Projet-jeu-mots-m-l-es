using System;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;

namespace ProjetInfo
{
    public class Plateau
    {
        int difficulté;
        List<Mot> achercher;
        string[,] matrice;

        public Plateau(int difficulté, List<Mot> achercher, string[,] matrice)
        {
            this.difficulté = difficulté;
            this.achercher = achercher;
            this.matrice = matrice;
        }
        public List<Mot> Achercher
        {
            get { return this.achercher; }
            set { this.achercher = value; }
        }
        public string[,] Matrice
        {
            get { return this.matrice; }
            set { this.matrice = value; }
        }
        //Retourne une chaîne de caractères qui décrit le plateau de jeu en fonction :
        //du niveau, la localisation du mot dans le tableau, le nombre de mots à trouver
        public string toString()
        {
            string mots = "\nLe niveau de cette grille est le niveau: " + this.difficulté + " Les dimensions de cette grille : " + this.matrice.GetLength(0) + "x" + this.matrice.GetLength(1) + " et il y a " + this.achercher.Count() + " mots à trouver" + "\n";
            return (mots);
        }
        public void ToFile(string nomfile) //Sauvegarde l'instance du plateau et les mots enregistrés
        {
            int numero = 1;
            string path = "Jeu/Parties/" + nomfile;
            string path1 = @"Jeu/Parties/" + nomfile + "/grille" + difficulté + "Joueur" + numero + ".csv";//Chemin plateau
            string path1_2 = @"Jeu/Parties/" + nomfile + "/grille" + difficulté + "Joueur" + numero + "mots.csv"; //chemin mots
            System.IO.Directory.CreateDirectory(path);
            string matricecsv = "";
            string motstexte = "";
            matricecsv += this.difficulté + ";" + this.matrice.GetLength(0) + ";" + this.matrice.GetLength(1) + ";" + this.achercher.Count() + ";\n";

            //enregistrement de tous les mots de la grille
            for (int i = 0; i < this.achercher.Count(); i++)
            {
                matricecsv += this.achercher[i].Nom + ";";
            }

            matricecsv += "\n";

            //enregistrement de tous les éléments de la grille
            for (int i = 0; i < matrice.GetLength(0); i++)
            {
                for (int j = 0; j < matrice.GetLength(1); j++)
                {
                    matricecsv += matrice[i, j] + ';';
                }
                matricecsv += '\n';
            }

            //Enregistre les mots de la grille
            for (int i = 0; i < this.achercher.Count(); i++)
            {
                if (i == this.achercher.Count() - 1)
                {
                    motstexte += this.achercher[i].Nom + ";" + this.achercher[i].Ligne + ";" + this.achercher[i].Colonne + ";" + this.achercher[i].Direction + ";";

                }
                else
                {
                    motstexte += this.achercher[i].Nom + ";" + this.achercher[i].Ligne + ";" + this.achercher[i].Colonne + ";" + this.achercher[i].Direction + ";\n";

                }
            }

            if (File.Exists(path1))//si chemin existe déjà =>le chemin correpond au fichier du joueur 1, donc dans la boucle if, le chemin change de nom
            {
                //Enregistrement de la grille pour le joueur 2
                path1 = @"Jeu/Parties/" + nomfile + "/grille" + difficulté + "Joueur" + (numero + 1) + ".csv";
                using (FileStream fs = File.Create(path1))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(matricecsv);
                    fs.Write(info, 0, info.Length);
                }
                //Enregistrement des mots à trouver pour le joueur 2
                path1_2 = @"Jeu/Parties/" + nomfile + "/grille" + difficulté + "Joueur" + (numero + 1) + "mots.csv";
                using (FileStream fs = File.Create(path1_2))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(motstexte);
                    fs.Write(info, 0, info.Length);
                }
            }
            else
            {
                //Enregistrement de la grille pour le joueur 1
                using (FileStream fs = File.Create(path1))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(matricecsv);
                    fs.Write(info, 0, info.Length);
                }
                //Enregistrement des mots à trouver pour le joueur 1
                using (FileStream fs = File.Create(path1_2))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(motstexte);
                    fs.Write(info, 0, info.Length);
                }
            }
        }
        static public Plateau ToRead(string nomfile) //Instancie un plateau à partir d'un fichier(jamais utiliser car génération aléatoire)&&Récupère grille déjà faites pour la sauvegarde
        {
            return (new Plateau(Niveau(nomfile), Tableaudemots(nomfile), Grille(nomfile)));
        }

        //Lit le niveau du Plateau pour la récupération de données lors de la sauvegarde
        static public int Niveau(string mot)
        {
            string[] lines = System.IO.File.ReadAllLines(@mot);
            string tab1 = lines[0];
            return (Convert.ToInt32(Convert.ToString(tab1[0])));
        }

        //Lit la liste des mots du Plateau pour la récupération de données lors de la sauvegarde
        public static List<Mot> Tableaudemots(string mot)
        {
            List<Mot> trouver = new List<Mot>();
            string[] lines = System.IO.File.ReadAllLines(@mot.Substring(0, mot.Length - 4) + "mots.csv"); //change le nom de fichier afin d'acceder au fichier avec les coordonnées
            for (int i = 0; i < lines.Length; i++)
            {
                string[] subs = lines[i].Split(';');
                trouver.Add(new Mot(subs[0], subs[3], Convert.ToInt32(subs[1]), Convert.ToInt32(subs[2])));
            }
            return (trouver);

        }
        //Lit la grille du Plateau pour la récupération de données lors de la sauvegarde
        public static string[,] Grille(string nomfile)
        {
            int l = 0;
            string[] lines = System.IO.File.ReadAllLines(nomfile);
            string[] tab1 = lines[0].Split(';');
            string[,] grille = new string[Convert.ToInt32(Convert.ToString(tab1[1])), Convert.ToInt32(Convert.ToString(tab1[2]))];
            int i = 0;
            int j = 0;

            foreach (string line in lines)
            {
                if (l <= 1)
                {
                    l++;
                }
                else
                {
                    foreach (char s in line)
                    {
                        if (s == ';')
                        {

                        }
                        else
                        {
                            if (j <= Convert.ToInt32(Convert.ToString(tab1[2])))
                            {
                                grille[j, i] = Convert.ToString(s);
                            }
                            i++;
                        }

                    }
                    i = 0;
                    j++;
                }
            }
            return (grille);
        }

        //Affiche la grille
        public void Afficher(List<string> motstrouves)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(00 + " ");
            Console.ForegroundColor = ConsoleColor.White;
            for (int i = 1; i <= this.matrice.GetLength(1); i++)
            {
                if (i < 10)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("0" + i + " ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(i + " ");
                }
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine();
            for (int i = 0; i < this.matrice.GetLength(0); i++)
            {
                if ((i + 1) < 10)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("0" + (i + 1) + " ");
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write(i + 1 + " ");
                }
                for (int j = 0; j < this.matrice.GetLength(1); j++)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write(this.matrice[i, j] + "  ");
                }
                Console.WriteLine();
            }
            for (int k = 0; k < achercher.Count(); k++)
            {
                if(motstrouves.Contains(achercher[k].Nom))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(this.achercher[k].Nom + " ");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.Write(this.achercher[k].Nom + " ");   //Permet de rajouter un espace entre chaque mot a rechercher
                }
            }
            Console.WriteLine();
            Console.WriteLine();
        }
        //Test si mot est éligible ou non
        public bool Test_Plateau(string mot, int ligne, int colonne, string direction)
        {
            if (achercher.Contains(new Mot(mot, direction, ligne, colonne))) { return (true); } //compare si le nouveau mot est le même que celui existant déja dans le plateau 
            else return false;
        }

        //Obtenir le placement d'une lettre à partir de son mot dans la grille
        public static int[] Obtenirplacement(int[] placementxy, List<Mot> atrouver, int quellelettre, int k)
        {
            //Cas pour est
            if (atrouver[k - 1].Direction == "E")
            {
                placementxy[1] = atrouver[k - 1].Colonne + quellelettre; //Modifie la postion colonne pour avoir la position précise de la letrre en question sur lagrille
            }
            //Cas pour sud
            else if (atrouver[k - 1].Direction == "S")
            {
                placementxy[0] = atrouver[k - 1].Ligne + quellelettre; //même principe
            }
            //etc
            else if (atrouver[k - 1].Direction == "O")
            {
                placementxy[1] = atrouver[k - 1].Colonne - quellelettre;
            }
            else if (atrouver[k - 1].Direction == "N")
            {
                placementxy[0] = atrouver[k - 1].Ligne - quellelettre;
            }
            else if (atrouver[k - 1].Direction == "SO")
            {
                placementxy[0] = atrouver[k - 1].Ligne + quellelettre;
                placementxy[1] = atrouver[k - 1].Colonne - quellelettre;
            }
            else if (atrouver[k - 1].Direction == "SE")
            {
                placementxy[0] = atrouver[k - 1].Ligne + quellelettre;
                placementxy[1] = atrouver[k - 1].Colonne + quellelettre;
            }
            else if (atrouver[k - 1].Direction == "NE")
            {
                placementxy[0] = atrouver[k - 1].Ligne - quellelettre;
                placementxy[1] = atrouver[k - 1].Colonne + quellelettre;
            }
            //Cas pour NO
            else
            {
                placementxy[0] = atrouver[k - 1].Ligne - quellelettre;
                placementxy[1] = atrouver[k - 1].Colonne - quellelettre;
            }

            return (placementxy);

        }
        public static void Placer(int[] placementxy, string direction, string[,] grille, string motaplacer)
        {
            //cas pour est
            if (direction == "E")
            {
                for (int i = placementxy[1], p = 0; p < motaplacer.Length; i++, p++)
                {
                    grille[placementxy[0], i] = Convert.ToString(motaplacer[p]); //place le mot dans la grille
                }
            }
            //etc
            else if (direction == "S")
            {
                for (int i = placementxy[0], p = 0; p < motaplacer.Length; i++, p++)
                {
                    grille[i, placementxy[1]] = Convert.ToString(motaplacer[p]);
                }
            }
            else if (direction == "O")
            {
                for (int i = placementxy[1], p = 0; p < motaplacer.Length; i--, p++)
                {
                    grille[placementxy[0], i] = Convert.ToString(motaplacer[p]);
                }
            }
            else if (direction == "N")
            {
                for (int i = placementxy[0], p = 0; p < motaplacer.Length; i--, p++)
                {
                    grille[i, placementxy[1]] = Convert.ToString(motaplacer[p]);
                }
            }
            else if (direction == "SO")
            {
                for (int i = placementxy[1], j = placementxy[0], p = 0; p < motaplacer.Length; i--, p++, j++)
                {
                    grille[j, i] = Convert.ToString(motaplacer[p]);
                }
            }
            else if (direction == "SE")
            {
                for (int i = placementxy[1], j = placementxy[0], p = 0; p < motaplacer.Length; i++, p++, j++)
                {
                    grille[j, i] = Convert.ToString(motaplacer[p]);
                }
            }
            else if (direction == "NE")
            {
                for (int i = placementxy[1], j = placementxy[0], p = 0; p < motaplacer.Length; i++, p++, j--)
                {
                    grille[j, i] = Convert.ToString(motaplacer[p]);
                }
            }
            else if (direction == "NO")
            {
                for (int i = placementxy[1], j = placementxy[0], p = 0; p < motaplacer.Length; i--, p++, j--)
                {
                    grille[j, i] = Convert.ToString(motaplacer[p]);
                }
            }
        }
        //Méthode qui génère un plateau aléatoirement 
        public static Plateau Aleatoire(string filename, int manche) //Langue(pour le dictionnaire) et manche(pour la difficulté)
        {
            //Créer une liste de mots à partir du dictionnaire
            List<string> dictionnaireFR = new List<string>();
            string[] li = System.IO.File.ReadAllLines(@filename);
            for (int i = 0; i < li.Length; i++)
            {
                string tab1 = li[i];
                string[] tab12 = tab1.Split(' ', ',');
                for (int b = 0; b < tab12.Length; b++)
                {
                    dictionnaireFR.Add(tab12[b]);
                }
            }

            //Initialisation des variables
            int nbligne = 0;
            int nbcolonne = 0;
            int nombredemotstotal = 0;
            int nbdirection = 0;
            List<Mot> atrouver = new List<Mot>(); //Liste de mots à trouver
            int k = 0;
            bool motplace = false;//des que le mot est place true
            bool boucledejaeffectuee1 = false;
            bool boucledejaeffectuee2 = false;
            bool boucledejaeffectuee12 = false;
            bool boucledejaeffectuee22 = false;
            bool boucledejaeffectuee3 = false;
            bool boucledejaeffectuee32 = false;
            bool boucledejaeffectuee4 = false;
            bool boucledejaeffectuee42 = false;
            bool boucledejaeffectuee5 = false;
            bool boucledejaeffectuee52 = false;
            bool boucledejaeffectuee6 = false;
            bool boucledejaeffectuee62 = false;
            bool boucledejaeffectuee7 = false;
            bool boucledejaeffectuee72 = false;
            bool boucledejaeffectuee8 = false;
            bool boucledejaeffectuee82 = false;
            int condition = 0;
            Random r = new Random(); //Random

            //Selon la manche, le nb de lignes, colonnes, directions et mots à trouver différent
            if (manche == 1)
            {
                nbdirection = 2;
                nombredemotstotal = 8;
                nbligne = 7;
                nbcolonne = 6;
            }
            else if (manche == 2)
            {
                nbdirection = 4;
                nombredemotstotal = 10;
                nbligne = 8;
                nbcolonne = 7;
            }
            else if (manche == 3)
            {
                nbdirection = 5;
                nombredemotstotal = 14;
                nbligne = 10;
                nbcolonne = 10;
            }
            else if (manche == 4)
            {
                nbdirection = 6;
                nombredemotstotal = 20;
                nbligne = 11;
                nbcolonne = 11;
            }
            else
            {
                nbdirection = 8;
                nombredemotstotal = 28;
                nbligne = 13;
                nbcolonne = 13;
            }
            //Création d'une matrice de string rempli de 0 selon le nombre de lignes et colonnes
            string[,] grille = new string[nbligne, nbcolonne];
            for (int i = 0; i < nbligne; i++)
            {
                for (int j = 0; j < nbcolonne; j++)
                {
                    grille[i, j] = Convert.ToString(0);
                }
            }
            //Début de la génération aléatoire
            while (atrouver.Count() != nombredemotstotal)
            {
                while (motplace != true)
                {
                    int direction = r.Next(nbdirection); //choisi la direction qu'on va jouer
                                                         //Cas pour le premier mot
                    if (k == 0) //k réprésente le nombre de mot placé
                    {
                        condition = nbdirection;
                        direction = 99;
                    }
                    else
                    {
                    }
                    //Placer un mot direction est
                    while (Convert.ToInt32(direction) == 0 && boucledejaeffectuee1 != true)
                    {
                        int lettres = 0;
                        //Tant qu'on a pas fait toutes les lettres du mot et que le mot n'est pas placé alors la boucle s'effectue
                        while (lettres != atrouver[k - 1].Nom.Length && motplace != true)
                        {
                            int quellelettre = r.Next(atrouver[k - 1].Nom.Length); //Choisis aléatoirement une lettre du mot précédent 
                            int[] placementxy = { atrouver[k - 1].Ligne, atrouver[k - 1].Colonne };//Prend la postion de placement du mot précédent, Ligne et Colonne du mot
                            int espacelibre = 0;
                            placementxy = Obtenirplacement(placementxy, atrouver, quellelettre, k);//méthode pour obtenir le placement quellelettre
                            int i = 1;
                            //Compte le nombre d'espace libre
                            while (placementxy[1] + i < grille.GetLength(1) && grille[placementxy[0], placementxy[1] + i] == "0")
                            {
                                espacelibre++;
                                i++;
                            }
                            //Cas où on peut écrire quelque chose
                            if (espacelibre >= 1)
                            {
                                string motaplacer = "q";
                                int compteur = 0;
                                //Chosi un mot qui commence par la même lettre choisi précédement et inférieur ou égal à l'espace libre, la répétion s'effecture au maximum 10000 fois
                                while (compteur <= 10000 && (atrouver[k - 1].Nom[quellelettre] != motaplacer[0] || espacelibre < motaplacer.Length))
                                {
                                    motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                    compteur++;
                                }
                                //Il n'y a pas de mot, donc ça ne place pas et ça recommence pour un autre lettre
                                if (compteur > 10000)
                                {
                                    lettres += 1;
                                }
                                else
                                {
                                    //Place le mot
                                    string direction1 = "E";
                                    Placer(placementxy, direction1, grille, motaplacer);
                                    atrouver.Add(new Mot(motaplacer, direction1, placementxy[0], placementxy[1]));//Ajoute le mot dans la liste de mot à trouver
                                    motplace = true;//Permet de finir la boucle
                                    k++;//Le nombre de mot placé augmente de 1
                                    dictionnaireFR.Remove(motaplacer);//Le mot est supprimé du dictionnaire pour éviter les répétitions
                                }
                            }
                            //Cas où il n'y a pas d'espace libre, la vérification recommence pour une autre lettre choisi aléatoirement
                            else
                            {
                                lettres += 1;
                            }
                        }
                        condition += 1;
                        boucledejaeffectuee1 = true;//Finir la boucle
                    }
                    //Même principe pour le reste mais direction différent
                    while (Convert.ToInt32(direction) == 1 && boucledejaeffectuee2 != true)
                    {
                        int lettres = 0;
                        while (lettres != atrouver[k - 1].Nom.Length && motplace != true)
                        {
                            int quellelettre = r.Next(atrouver[k - 1].Nom.Length);
                            int[] placementxy = { atrouver[k - 1].Ligne, atrouver[k - 1].Colonne };
                            int espacelibre = 0;
                            placementxy = Obtenirplacement(placementxy, atrouver, quellelettre, k);
                            int i = 1;
                            while (placementxy[0] + i < grille.GetLength(0) && grille[placementxy[0] + i, placementxy[1]] == "0")
                            {
                                espacelibre++;
                                i++;
                            }
                            if (espacelibre >= 1)
                            {
                                string motaplacer = "q";
                                int compteur = 0;
                                while (compteur <= 10000 && (atrouver[k - 1].Nom[quellelettre] != motaplacer[0] || espacelibre < motaplacer.Length))
                                {
                                    motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                    compteur++;
                                }
                                if (compteur > 10000)
                                {
                                    lettres += 1;
                                }
                                else
                                {
                                    string direction1 = "S";
                                    Placer(placementxy, direction1, grille, motaplacer);
                                    atrouver.Add(new Mot(motaplacer, direction1, placementxy[0], placementxy[1]));
                                    motplace = true;
                                    k++;
                                    dictionnaireFR.Remove(motaplacer);
                                }
                            }
                            else
                            {
                                lettres += 1;
                            }
                        }
                        condition += 1;
                        boucledejaeffectuee2 = true;
                    }
                    while (Convert.ToInt32(direction) == 2 && boucledejaeffectuee3 != true)
                    {
                        int lettres = 0;
                        while (lettres != atrouver[k - 1].Nom.Length && motplace != true)
                        {
                            int quellelettre = r.Next(atrouver[k - 1].Nom.Length);
                            int[] placementxy = { atrouver[k - 1].Ligne, atrouver[k - 1].Colonne };
                            int espacelibre = 0;
                            placementxy = Obtenirplacement(placementxy, atrouver, quellelettre, k);
                            int i = 1;
                            while (placementxy[1] - i >= 0 && grille[placementxy[0], placementxy[1] - i] == "0")
                            {
                                espacelibre++;
                                i++;
                            }

                            if (espacelibre >= 1)
                            {
                                string motaplacer = "q";
                                int compteur = 0;
                                while (compteur <= 10000 && (atrouver[k - 1].Nom[quellelettre] != motaplacer[0] || espacelibre < motaplacer.Length))
                                {
                                    motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                    compteur++;
                                }
                                if (compteur > 10000)
                                {
                                    lettres += 1;
                                }
                                else
                                {
                                    string direction1 = "O";
                                    Placer(placementxy, direction1, grille, motaplacer);
                                    atrouver.Add(new Mot(motaplacer, direction1, placementxy[0], placementxy[1]));
                                    motplace = true;
                                    k++;
                                    dictionnaireFR.Remove(motaplacer);
                                }
                            }
                            else
                            {
                                lettres += 1;
                            }
                        }
                        condition += 1;
                        boucledejaeffectuee3 = true;
                    }
                    while (Convert.ToInt32(direction) == 3 && boucledejaeffectuee4 != true)
                    {
                        int lettres = 0;
                        while (lettres != atrouver[k - 1].Nom.Length && motplace != true)
                        {
                            int quellelettre = r.Next(atrouver[k - 1].Nom.Length);
                            int[] placementxy = { atrouver[k - 1].Ligne, atrouver[k - 1].Colonne };
                            int espacelibre = 0;
                            placementxy = Obtenirplacement(placementxy, atrouver, quellelettre, k);
                            int i = 1;
                            while (placementxy[0] - i >= 0 && grille[placementxy[0] - i, placementxy[1]] == "0")
                            {
                                espacelibre++;
                                i++;
                            }
                            if (espacelibre >= 1)
                            {
                                string motaplacer = "q";
                                int compteur = 0;
                                while (compteur <= 10000 && (atrouver[k - 1].Nom[quellelettre] != motaplacer[0] || espacelibre < motaplacer.Length))
                                {
                                    motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                    compteur++;
                                }
                                if (compteur > 10000)
                                {
                                    lettres += 1;
                                }
                                else
                                {
                                    string direction1 = "N";
                                    Placer(placementxy, direction1, grille, motaplacer);
                                    atrouver.Add(new Mot(motaplacer, direction1, placementxy[0], placementxy[1]));
                                    motplace = true;
                                    k++;
                                    dictionnaireFR.Remove(motaplacer);
                                }
                            }
                            else
                            {
                                lettres += 1;
                            }
                        }
                        condition += 1;
                        boucledejaeffectuee4 = true;
                    }
                    while (Convert.ToInt32(direction) == 4 && boucledejaeffectuee5 != true)
                    {
                        int lettres = 0;
                        while (lettres != atrouver[k - 1].Nom.Length && motplace != true)
                        {
                            int quellelettre = r.Next(atrouver[k - 1].Nom.Length);
                            int[] placementxy = { atrouver[k - 1].Ligne, atrouver[k - 1].Colonne };
                            int espacelibre = 0;
                            placementxy = Obtenirplacement(placementxy, atrouver, quellelettre, k);
                            int i = 1;
                            while (placementxy[0] + i < grille.GetLength(0) && placementxy[1] - i >= 0 && grille[placementxy[0] + i, placementxy[1] - i] == "0")
                            {
                                espacelibre++;
                                i++;
                            }

                            if (espacelibre >= 1)
                            {
                                string motaplacer = "q";
                                int compteur = 0;
                                while (compteur <= 10000 && (atrouver[k - 1].Nom[quellelettre] != motaplacer[0] || espacelibre < motaplacer.Length))
                                {
                                    motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                    compteur++;
                                }
                                if (compteur > 10000)
                                {
                                    lettres += 1;
                                }
                                else
                                {
                                    string direction1 = "SO";
                                    Placer(placementxy, direction1, grille, motaplacer);
                                    atrouver.Add(new Mot(motaplacer, direction1, placementxy[0], placementxy[1]));
                                    motplace = true;
                                    k++;
                                    dictionnaireFR.Remove(motaplacer);
                                }
                            }
                            else
                            {
                                lettres += 1;
                            }
                        }
                        condition += 1;
                        boucledejaeffectuee5 = true;
                    }
                    while (Convert.ToInt32(direction) == 5 && boucledejaeffectuee6 != true)
                    {
                        int lettres = 0;
                        while (lettres != atrouver[k - 1].Nom.Length && motplace != true)
                        {
                            int quellelettre = r.Next(atrouver[k - 1].Nom.Length);
                            int[] placementxy = { atrouver[k - 1].Ligne, atrouver[k - 1].Colonne };
                            int espacelibre = 0;
                            placementxy = Obtenirplacement(placementxy, atrouver, quellelettre, k);
                            int i = 1;
                            while (placementxy[0] + i < grille.GetLength(0) && placementxy[1] + i < grille.GetLength(1) && grille[placementxy[0] + i, placementxy[1] + i] == "0")
                            {
                                espacelibre++;
                                i++;
                            }

                            if (espacelibre >= 1)
                            {
                                string motaplacer = "q";
                                int compteur = 0;
                                while (compteur <= 10000 && (atrouver[k - 1].Nom[quellelettre] != motaplacer[0] || espacelibre < motaplacer.Length))
                                {
                                    motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                    compteur++;
                                }
                                if (compteur > 10000)
                                {
                                    lettres += 1;
                                }
                                else
                                {
                                    string direction1 = "SE";
                                    Placer(placementxy, direction1, grille, motaplacer);
                                    atrouver.Add(new Mot(motaplacer, direction1, placementxy[0], placementxy[1]));
                                    motplace = true;
                                    k++;
                                    dictionnaireFR.Remove(motaplacer);
                                }
                            }
                            else
                            {
                                lettres += 1;
                            }
                        }
                        condition += 1;
                        boucledejaeffectuee6 = true;
                    }
                    while (Convert.ToInt32(direction) == 6 && boucledejaeffectuee7 != true)
                    {
                        int lettres = 0;
                        while (lettres != atrouver[k - 1].Nom.Length && motplace != true)
                        {
                            int quellelettre = r.Next(atrouver[k - 1].Nom.Length);
                            int[] placementxy = { atrouver[k - 1].Ligne, atrouver[k - 1].Colonne };
                            int espacelibre = 0;
                            placementxy = Obtenirplacement(placementxy, atrouver, quellelettre, k);
                            int i = 1;
                            while (placementxy[0] - i >= 0 && placementxy[1] + i < grille.GetLength(1) && grille[placementxy[0] - i, placementxy[1] + i] == "0")
                            {
                                espacelibre++;
                                i++;
                            }

                            if (espacelibre >= 1)
                            {
                                string motaplacer = "q";
                                int compteur = 0;
                                while (compteur <= 10000 && (atrouver[k - 1].Nom[quellelettre] != motaplacer[0] || espacelibre < motaplacer.Length))
                                {
                                    motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                    compteur++;
                                }
                                if (compteur > 10000)
                                {
                                    lettres += 1;
                                }
                                else
                                {
                                    string direction1 = "NE";
                                    Placer(placementxy, direction1, grille, motaplacer);
                                    atrouver.Add(new Mot(motaplacer, direction1, placementxy[0], placementxy[1]));
                                    motplace = true;
                                    k++;
                                    dictionnaireFR.Remove(motaplacer);
                                }
                            }
                            else
                            {
                                lettres += 1;
                            }
                        }
                        condition += 1;
                        boucledejaeffectuee7 = true;
                    }
                    while (Convert.ToInt32(direction) == 7 && boucledejaeffectuee8 != true)
                    {
                        int lettres = 0;
                        while (lettres != atrouver[k - 1].Nom.Length && motplace != true)
                        {
                            int quellelettre = r.Next(atrouver[k - 1].Nom.Length);
                            int[] placementxy = { atrouver[k - 1].Ligne, atrouver[k - 1].Colonne };
                            int espacelibre = 0;
                            placementxy = Obtenirplacement(placementxy, atrouver, quellelettre, k);
                            int i = 1;
                            while (placementxy[0] - i >= 0 && placementxy[1] - i >= 0 && grille[placementxy[0] - i, placementxy[1] - i] == "0")
                            {
                                espacelibre++;
                                i++;
                            }

                            if (espacelibre >= 1)
                            {
                                string motaplacer = "q";
                                int compteur = 0;
                                while (compteur <= 10000 && (atrouver[k - 1].Nom[quellelettre] != motaplacer[0] || espacelibre < motaplacer.Length))
                                {
                                    motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                    compteur++;
                                }
                                if (compteur > 10000)
                                {
                                    lettres += 1;
                                }
                                else
                                {
                                    string direction1 = "NO";
                                    Placer(placementxy, direction1, grille, motaplacer);
                                    atrouver.Add(new Mot(motaplacer, direction1, placementxy[0], placementxy[1]));
                                    motplace = true;
                                    k++;
                                    dictionnaireFR.Remove(motaplacer);
                                }
                            }
                            else
                            {
                                lettres += 1;
                            }
                        }
                        condition += 1;
                        boucledejaeffectuee8 = true;
                    }
                    int a = 0;
                    //Si on a testé toutes les directions possibles alors on place un mot aléatoirement sans croisement dans la grille
                    while ((condition == nbdirection && motplace != true && a < 50)) // a<50 car Cas où l'algorithme n'a pas pu placer un mot (très peu probable) Suite à cela, l'algorithme recommence à essayer de trouver un mot depuis le debut du placement précédent
                    {
                        a++;
                        direction = r.Next(nbdirection);
                        //Même principe que les boucles while vu précédement mais sans croisement
                        while (Convert.ToInt32(direction) == 0 && boucledejaeffectuee12 != true)
                        {
                            int repet = 0;
                            int[] placementxy = { r.Next(grille.GetLength(0)), r.Next(grille.GetLength(1)) };
                            while (motplace != true && repet < 10000)
                            {
                                while (grille[placementxy[0], placementxy[1]] != "0" && motplace != true)
                                {
                                    placementxy[0] = r.Next(grille.GetLength(0));
                                    placementxy[1] = r.Next(grille.GetLength(1));
                                }
                                int espacelibre = 0;
                                int i = 0;
                                while (placementxy[1] + i < grille.GetLength(1) && grille[placementxy[0], placementxy[1] + i] == "0")
                                {
                                    espacelibre++;
                                    i++;
                                }

                                if (espacelibre > 1)
                                {
                                    string motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                    int compteur = 0;
                                    while (compteur < 10000 && espacelibre < motaplacer.Length)
                                    {
                                        motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                        compteur++;
                                    }
                                    if (compteur >= 10000)
                                    {
                                        repet += 1;
                                    }
                                    else
                                    {
                                        string direction1 = "E";
                                        Placer(placementxy, direction1, grille, motaplacer);
                                        atrouver.Add(new Mot(motaplacer, direction1, placementxy[0], placementxy[1]));
                                        motplace = true;
                                        k++;
                                        dictionnaireFR.Remove(motaplacer);
                                    }
                                }
                                else
                                {
                                    repet += 1;
                                }
                            }
                            boucledejaeffectuee12 = true;
                        }
                        while (Convert.ToInt32(direction) == 1 && boucledejaeffectuee22 != true)
                        {
                            int repet = 0;
                            int[] placementxy = { r.Next(grille.GetLength(0)), r.Next(grille.GetLength(1)) };
                            while (motplace != true && repet < 10000)
                            {
                                while (grille[placementxy[0], placementxy[1]] != "0" && motplace != true)
                                {
                                    placementxy[0] = r.Next(grille.GetLength(0));
                                    placementxy[1] = r.Next(grille.GetLength(1));
                                }
                                int espacelibre = 0;
                                int i = 0;
                                while (placementxy[0] + i < grille.GetLength(0) && grille[placementxy[0] + i, placementxy[1]] == "0")
                                {
                                    espacelibre++;
                                    i++;
                                }

                                if (espacelibre > 1)
                                {
                                    string motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                    int compteur = 0;
                                    while (compteur < 10000 && espacelibre < motaplacer.Length)
                                    {
                                        motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                        compteur++;
                                    }
                                    if (compteur >= 10000)
                                    {
                                        repet += 1;
                                    }
                                    else
                                    {
                                        string direction1 = "S";
                                        Placer(placementxy, direction1, grille, motaplacer);
                                        atrouver.Add(new Mot(motaplacer, direction1, placementxy[0], placementxy[1]));
                                        motplace = true;
                                        k++;
                                        dictionnaireFR.Remove(motaplacer);
                                    }
                                }
                                else
                                {
                                    repet += 1;
                                }
                            }
                            boucledejaeffectuee22 = true;
                        }
                        while (Convert.ToInt32(direction) == 2 && boucledejaeffectuee32 != true)
                        {
                            int repet = 0;
                            int[] placementxy = { r.Next(grille.GetLength(0)), r.Next(grille.GetLength(1)) };
                            while (motplace != true && repet < 10000)
                            {
                                while (grille[placementxy[0], placementxy[1]] != "0" && motplace != true)
                                {
                                    placementxy[0] = r.Next(grille.GetLength(0));
                                    placementxy[1] = r.Next(grille.GetLength(1));
                                }
                                int espacelibre = 0;
                                int i = 0;
                                while (placementxy[1] - i >= 0 && grille[placementxy[0], placementxy[1] - i] == "0")
                                {
                                    espacelibre++;
                                    i++;
                                }

                                if (espacelibre > 1)
                                {
                                    string motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                    int compteur = 0;
                                    while (compteur < 10000 && espacelibre < motaplacer.Length)
                                    {
                                        motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                        compteur++;
                                    }
                                    if (compteur >= 10000)
                                    {
                                        repet += 1;
                                    }
                                    else
                                    {
                                        string direction1 = "O";
                                        Placer(placementxy, direction1, grille, motaplacer);
                                        atrouver.Add(new Mot(motaplacer, direction1, placementxy[0], placementxy[1]));
                                        motplace = true;
                                        k++;
                                        dictionnaireFR.Remove(motaplacer);
                                    }
                                }
                                else
                                {
                                    repet += 1;
                                }
                            }
                            boucledejaeffectuee32 = true;
                        }
                        while (Convert.ToInt32(direction) == 3 && boucledejaeffectuee42 != true)
                        {
                            int repet = 0;
                            int[] placementxy = { r.Next(grille.GetLength(0)), r.Next(grille.GetLength(1)) };
                            while (motplace != true && repet < 10000)
                            {
                                while (grille[placementxy[0], placementxy[1]] != "0" && motplace != true)
                                {
                                    placementxy[0] = r.Next(grille.GetLength(0));
                                    placementxy[1] = r.Next(grille.GetLength(1));
                                }
                                int espacelibre = 0;
                                int i = 0;
                                while (placementxy[0] - i >= 0 && grille[placementxy[0] - i, placementxy[1]] == "0")
                                {
                                    espacelibre++;
                                    i++;
                                }

                                if (espacelibre > 1)
                                {
                                    string motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                    int compteur = 0;
                                    while (compteur < 10000 && espacelibre < motaplacer.Length)
                                    {
                                        motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                        compteur++;
                                    }
                                    if (compteur >= 10000)
                                    {
                                        repet += 1;
                                    }
                                    else
                                    {
                                        string direction1 = "N";
                                        Placer(placementxy, direction1, grille, motaplacer);
                                        atrouver.Add(new Mot(motaplacer, direction1, placementxy[0], placementxy[1]));
                                        motplace = true;
                                        k++;
                                        dictionnaireFR.Remove(motaplacer);
                                    }
                                }
                                else
                                {
                                    repet += 1;
                                }
                            }
                            boucledejaeffectuee42 = true;
                        }
                        while (Convert.ToInt32(direction) == 4 && boucledejaeffectuee52 != true)
                        {
                            int repet = 0;
                            int[] placementxy = { r.Next(grille.GetLength(0)), r.Next(grille.GetLength(1)) };
                            while (motplace != true && repet < 10000)
                            {
                                while (grille[placementxy[0], placementxy[1]] != "0" && motplace != true)
                                {
                                    placementxy[0] = r.Next(grille.GetLength(0));
                                    placementxy[1] = r.Next(grille.GetLength(1));
                                }
                                int espacelibre = 0;
                                int i = 0;
                                while (placementxy[0] + i < grille.GetLength(0) && placementxy[1] - i >= 0 && grille[placementxy[0] + i, placementxy[1] - i] == "0")
                                {
                                    espacelibre++;
                                    i++;
                                }

                                if (espacelibre > 1)
                                {
                                    string motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                    int compteur = 0;
                                    while (compteur < 10000 && espacelibre < motaplacer.Length)
                                    {
                                        motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                        compteur++;
                                    }
                                    if (compteur >= 10000)
                                    {
                                        repet += 1;
                                    }
                                    else
                                    {
                                        string direction1 = "SO";
                                        Placer(placementxy, direction1, grille, motaplacer);
                                        atrouver.Add(new Mot(motaplacer, direction1, placementxy[0], placementxy[1]));
                                        motplace = true;
                                        k++;
                                        dictionnaireFR.Remove(motaplacer);
                                    }
                                }
                                else
                                {
                                    repet += 1;
                                }
                            }
                            boucledejaeffectuee52 = true;
                        }
                        while (Convert.ToInt32(direction) == 5 && boucledejaeffectuee62 != true)
                        {
                            int repet = 0;
                            int[] placementxy = { r.Next(grille.GetLength(0)), r.Next(grille.GetLength(1)) };
                            while (motplace != true && repet < 10000)
                            {
                                while (grille[placementxy[0], placementxy[1]] != "0" && motplace != true)
                                {
                                    placementxy[0] = r.Next(grille.GetLength(0));
                                    placementxy[1] = r.Next(grille.GetLength(1));
                                }
                                int espacelibre = 0;
                                int i = 0;
                                while (placementxy[0] + i < grille.GetLength(0) && placementxy[1] + i < grille.GetLength(1) && grille[placementxy[0] + i, placementxy[1] + i] == "0")
                                {
                                    espacelibre++;
                                    i++;
                                }

                                if (espacelibre > 1)
                                {
                                    string motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                    int compteur = 0;
                                    while (compteur < 100000 && espacelibre < motaplacer.Length)
                                    {
                                        motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                        compteur++;
                                    }
                                    if (compteur >= 10000)
                                    {
                                        repet += 1;
                                    }
                                    else
                                    {
                                        string direction1 = "SE";
                                        Placer(placementxy, direction1, grille, motaplacer);
                                        atrouver.Add(new Mot(motaplacer, direction1, placementxy[0], placementxy[1]));
                                        motplace = true;
                                        k++;
                                        dictionnaireFR.Remove(motaplacer);
                                    }
                                }
                                else
                                {
                                    repet += 1;
                                }
                            }
                            boucledejaeffectuee62 = true;
                        }
                        while (Convert.ToInt32(direction) == 6 && boucledejaeffectuee72 != true)
                        {
                            int repet = 0;
                            int[] placementxy = { r.Next(grille.GetLength(0)), r.Next(grille.GetLength(1)) };
                            while (motplace != true && repet < 10000)
                            {
                                while (grille[placementxy[0], placementxy[1]] != "0" && motplace != true)
                                {
                                    placementxy[0] = r.Next(grille.GetLength(0));
                                    placementxy[1] = r.Next(grille.GetLength(1));
                                }
                                int espacelibre = 0;
                                int i = 0;
                                while (placementxy[0] - i >= 0 && placementxy[1] + i < grille.GetLength(1) && grille[placementxy[0] - i, placementxy[1] + i] == "0")
                                {
                                    espacelibre++;
                                    i++;
                                }

                                if (espacelibre > 1)
                                {
                                    string motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                    int compteur = 0;
                                    while (compteur < 10000 && espacelibre < motaplacer.Length)
                                    {
                                        motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                        compteur++;
                                    }
                                    if (compteur >= 10000)
                                    {
                                        repet += 1;
                                    }
                                    else
                                    {
                                        string direction1 = "NE";
                                        Placer(placementxy, direction1, grille, motaplacer);
                                        atrouver.Add(new Mot(motaplacer, direction1, placementxy[0], placementxy[1]));
                                        motplace = true;
                                        k++;
                                        dictionnaireFR.Remove(motaplacer);
                                    }
                                }
                                else
                                {
                                    repet += 1;
                                }
                            }
                            boucledejaeffectuee72 = true;
                        }
                        while (Convert.ToInt32(direction) == 7 && boucledejaeffectuee82 != true)
                        {
                            int repet = 0;
                            int[] placementxy = { r.Next(grille.GetLength(0)), r.Next(grille.GetLength(1)) };
                            while (motplace != true && repet < 10000)
                            {
                                while (grille[placementxy[0], placementxy[1]] != "0" && motplace != true)
                                {
                                    placementxy[0] = r.Next(grille.GetLength(0));
                                    placementxy[1] = r.Next(grille.GetLength(1));
                                }
                                int espacelibre = 0;
                                int i = 0;
                                while (placementxy[0] - i >= 0 && placementxy[1] - i >= 0 && grille[placementxy[0] - i, placementxy[1] - i] == "0")
                                {
                                    espacelibre++;
                                    i++;
                                }

                                if (espacelibre > 1)
                                {
                                    string motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                    int compteur = 0;
                                    while (compteur < 10000 && espacelibre < motaplacer.Length)
                                    {
                                        motaplacer = dictionnaireFR[r.Next(dictionnaireFR.Count())];
                                        compteur++;
                                    }
                                    if (compteur >= 10000)
                                    {
                                        repet += 1;
                                    }
                                    else
                                    {
                                        string direction1 = "NO";
                                        Placer(placementxy, direction1, grille, motaplacer);
                                        atrouver.Add(new Mot(motaplacer, direction1, placementxy[0], placementxy[1]));
                                        motplace = true;
                                        k++;
                                        dictionnaireFR.Remove(motaplacer);
                                    }
                                }
                                else
                                {
                                    repet += 1;
                                }
                            }
                            boucledejaeffectuee82 = true;
                        }
                    }
                    boucledejaeffectuee12 = false;
                    boucledejaeffectuee22 = false;
                    boucledejaeffectuee32 = false;
                    boucledejaeffectuee42 = false;
                    boucledejaeffectuee52 = false;
                    boucledejaeffectuee62 = false;
                    boucledejaeffectuee72 = false;
                    boucledejaeffectuee82 = false;
                }
                condition = 0;
                boucledejaeffectuee1 = false;
                boucledejaeffectuee2 = false;
                boucledejaeffectuee3 = false;
                boucledejaeffectuee4 = false;
                boucledejaeffectuee5 = false;
                boucledejaeffectuee6 = false;
                boucledejaeffectuee7 = false;
                boucledejaeffectuee8 = false;
                motplace = false;
            }
            //Quand le joueur va jouer, la premiere colonne =1 et non 0 en C#jnxbv ,jk
            for (int i = 0; i < atrouver.Count(); i++)
            {
                atrouver[i].Ligne += 1;
                atrouver[i].Colonne += 1;
            }
            //Tous les 0 sont remplacés par des lettres aléatoirement
            string tab = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < grille.GetLength(0); i++)
            {
                for (int j = 0; j < grille.GetLength(1); j++)
                {
                    if (grille[i, j] == "0")
                    {
                        grille[i, j] = Convert.ToString(tab[r.Next(26)]);
                    }
                }
            }
            //Crééer le plateau
            return (new Plateau(manche, atrouver, grille));
        }
    }
}