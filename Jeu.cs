using System;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Text;
using System.Linq;
using System.Collections.Generic;
namespace ProjetInfo
{
    public class Jeu
    {
        List<Joueur> joueurs;
        List<Plateau> plateaux;
        Dictionnaire structure;
        public Jeu(List<Joueur> joueurs, List<Plateau> plateaux, Dictionnaire structure)
        {       
            this.joueurs = joueurs;
            this.plateaux = plateaux;
            this.structure = structure;
        }
        //Donne le temps limite pour jouer lors de la manche n
        //Niveau 1 : 1 minute, Niveau 2 : 2 minutes etc...
        public static DateTime Limite(DateTime seconde, int manche)
        {
            DateTime limite;
            if (seconde.Minute + manche >= 60) //éviter un problème d'affichage de l'heure par exemple quand on a 16h59 => 16h60 mais bien 17h00 lors du début du chrono
            {
                limite = new DateTime(seconde.Year, seconde.Month, seconde.Day, seconde.Hour + 1, seconde.Minute + manche - 60, seconde.Second, seconde.Millisecond);
            }
            else
            {
                limite = new DateTime(seconde.Year, seconde.Month, seconde.Day, seconde.Hour, seconde.Minute + 1, seconde.Second, seconde.Millisecond);
            }
            return (limite);
        }
        //Programme de sauvegarde de données
        public static void Sauvegarde(string filename, string donnees)
        {
            //Cas où il existe alors suprimer et écraser(utile pour une sauvegarde d'une sauvegarde)
            if (File.Exists(@"Jeu/Parties_en_cours/" + filename + "/donnees.csv"))
            {
                File.Delete(@"Jeu/Parties_en_cours/" + filename + "/donnees.csv");
            }
            string path = "Jeu/Parties_en_cours/" + filename; //chemin pour créer le dossier
            string path1 = @"Jeu/Parties_en_cours/" + filename + "/donnees.csv"; //chemin du fichier csv où il y a les données
            System.IO.Directory.CreateDirectory(path);//Création d'un dossier
            using (FileStream fs = File.Create(path1))//Créer un fichier donnees.csv
            {
                byte[] info = new UTF8Encoding(true).GetBytes(donnees);
                fs.Write(info, 0, info.Length);  //écrit dans le fichier les données
            }
        }
        //Saisie sécurisée quand le joueur écrit le nom de la partie car
        //Certains caractères comme "'!`ne sont pas possibles pour la création d'un dossier
        public static bool Saisiesecu(string nomfichier)
        {
            nomfichier = nomfichier.ToLower();
            int a = 0;
            char[] tab = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', '-', '_', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            for (int i = 0; i < nomfichier.Length; i++)
            {
                if (tab.Contains(nomfichier[i])) //Vérification pour chaque caractère
                {
                    a += 0;
                }
                else
                {
                    a += 1;
                }
            }
            return (a > 0);//Return true ou false
        }

        //La partie commence
        static public void Play()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("© copyright tous droits réservés");
            Console.ForegroundColor = ConsoleColor.White;
            Thread.Sleep(3000);
            Console.Clear();
            //Console.WriteLine("Ce jeu a été créé par CHEVREAU Emilie et MACHADO MONTEIRO Tiago pour un projet d'informatique.");
            Thread.Sleep(3000);
            Console.Clear();
            Console.WriteLine("Bienvenue");
            Console.WriteLine();
            Thread.Sleep(3000);
            //initialisation des variables
            //////////////////////////////////////////
            string emplacement = "";
            string donnees = "";
            int manche = 1;
            int scoremaxi = 0;
            int scoremaxi2 = 0;
            string langue = "";
            string filenamelangue = "";
            TimeSpan tempsaddi1 = new TimeSpan(0, 0, 0);
            TimeSpan tempsaddi2 = new TimeSpan(0, 0, 0);
            List<string> listedemotstrouverJ1 = new List<string>();
            List<string> listedemotstrouverJ2 = new List<string>();
            Joueur Joueur1 = new Joueur(null, null, 0);
            Joueur Joueur2 = new Joueur(null, null, 0);
            Dictionnaire dictionnairedujeu = new Dictionnaire(null, null);
            List<Joueur> joueurs = new List<Joueur>();
            List<Plateau> plateauxdujeu = new List<Plateau>();
            Jeu jeunumero1 = new Jeu(null, null, null);
            Stopwatch chronojoueur1 = new Stopwatch();
            Stopwatch chronojoueur2 = new Stopwatch();
            Random r = new Random();
            //////////////////////////////////////////////////////////////////////////
            Console.WriteLine("Souhaitez vous jouer une nouvelle partie ? (Si \"oui\"=>Nouvelle partie  Si \"non\"=>Partie sauvegardée)");
            string réponse = Console.ReadLine().ToLower(); //demande au joueur si il veut commencer une nouvelle partie ou non
            while (réponse != "non" && réponse != "oui")
            {
                Console.WriteLine("Resaisir");
                réponse = Console.ReadLine().ToLower();     //Saisie sécurisée lors de la réponse des joueurs
            }

            //Cas d'une nouvelle partie
            if (réponse == "oui")
            {
                Console.WriteLine("Comment souhaitez vous appeler la partie ?");
                emplacement = Console.ReadLine();
                while (Directory.Exists(@"Jeu/Parties/" + emplacement) || Saisiesecu(emplacement)) //Vérifie si il existe déjà une partie avec le même nom et saisie sécurisée
                {
                    Console.WriteLine("Veuillez choisir un autre nom");
                    emplacement = Console.ReadLine();
                }
                manche = 1;
                string nom1, nom2; //Initialisation des variables noms
                Console.WriteLine("Joueur 1, quel est votre nom ?");
                nom1 = Convert.ToString(Console.ReadLine());
                Console.WriteLine("Joueur 2, quel est votre nom ?");
                nom2 = Convert.ToString(Console.ReadLine());
                //création des deux joueurs
                Joueur1 = new Joueur(nom1, listedemotstrouverJ1, 0);
                Joueur2 = new Joueur(nom2, listedemotstrouverJ2, 0);
                langue = "français";
                filenamelangue = "Jeu/mots11.txt";
                dictionnairedujeu = Dictionnaire.Creationdictionnaire(filenamelangue, langue);//créer le dictionnaire utilisé pour le jeu
                for (int i = 0; i < 10; i++)
                {
                    Console.Write('\n');
                }
                Thread.Sleep(1000);
                Console.WriteLine(dictionnairedujeu.toString());//décrit le dictionnaire
                Console.Write('\n');
                Thread.Sleep(1500);
                Console.WriteLine(Joueur1.toString());//décrit le joueur1
                Console.Write('\n');
                Console.WriteLine(Joueur2.toString());//décrit le joueur2
                Thread.Sleep(5000);
                joueurs = new List<Joueur>() { Joueur1,Joueur2};
                plateauxdujeu = new List<Plateau>();
                jeunumero1 = new Jeu(joueurs, plateauxdujeu, dictionnairedujeu);//Création d'un jeu
                chronojoueur1 = new Stopwatch();//Chrono
                chronojoueur2 = new Stopwatch();
            }

            else //Cas pour une partie sauvegardée
            {
                string partiesauv = "";
                Console.WriteLine("Quel était le nom de votre partie ?");
                partiesauv = Console.ReadLine();
                while (Directory.Exists(@"Jeu/Parties_en_cours/" + partiesauv) == false) //Vérifie s'il existe déjà une partie avec le même nom
                {
                    Console.WriteLine("Veuillez saisir un autre nom");
                    partiesauv = Console.ReadLine();
                }
                emplacement = partiesauv;

                string[] lines = System.IO.File.ReadAllLines(@"Jeu/Parties_en_cours/" + partiesauv + "/donnees.csv");//Récupération des données
                string[] tab1 = lines[0].Split(';');//Récupère ligne par ligne
                string[] tab2 = lines[1].Split(';');
                string[] tab3 = lines[2].Split(';');
                string[] tab4 = lines[3].Split(';');
                string[] tab5 = lines[4].Split(';');
                string[] tab7 = lines[6].Split(';');
                string[] tab8 = lines[7].Split(';');
                string[] tab9 = lines[8].Split(';');
                if (tab3.Length == 1)//Problème de format à cause des espaces et des ;
                {

                }
                else//Si il n'y a pas de soucis
                {
                    for (int i = 0; i < tab3.Length; i++)
                    {
                        listedemotstrouverJ1.Add(tab3[i]); //Les mots trouvés par le joeur1 lors de sa partie sont récupérés
                    }
                }
                //Même principe
                if (tab5.Length == 1)
                {

                }
                else
                {
                    for (int i = 0; i < tab5.Length; i++)
                    {
                        listedemotstrouverJ2.Add(tab5[i]);
                    }
                }
                //Récupère les plateauxc qui ont été joués
                for (int i = 1; i <= manche; i++)
                {
                    plateauxdujeu.Add(Plateau.ToRead("Jeu/Parties/" + emplacement + "/grille" + i + "Joueur1.csv"));
                    plateauxdujeu.Add(Plateau.ToRead("Jeu/Parties/" + emplacement + "/grille" + i + "Joueur2.csv"));
                }
                //Récréer les jouers
                Joueur1 = new Joueur(tab2[0], listedemotstrouverJ1, Convert.ToInt32(tab2[1]));
                Joueur2 = new Joueur(tab4[0], listedemotstrouverJ2, Convert.ToInt32(tab4[1]));
                filenamelangue = tab7[0];//langue
                langue = tab7[1];
                dictionnairedujeu = Dictionnaire.Creationdictionnaire(filenamelangue, langue);//Dictionnaire
                tempsaddi1 = new TimeSpan((Convert.ToInt32(tab8[0])), Convert.ToInt32(tab8[1]), Convert.ToInt32(tab8[2]));
                tempsaddi2 = new TimeSpan((Convert.ToInt32(tab9[0])), Convert.ToInt32(tab9[1]), Convert.ToInt32(tab9[2])); //Récupère le temps
                joueurs = new List<Joueur>() { Joueur1, Joueur2 };
                jeunumero1 = new Jeu(joueurs, plateauxdujeu, dictionnairedujeu);//Recréer le jeu
                manche = Convert.ToInt32(tab1[0]);
            }

            while (manche < 6)
            {
                scoremaxi = 0;//Score maximum des joueurs lors de la manche n
                scoremaxi2 = 0;
                for (int i = 0; i < 10; i++)
                {
                    Console.Write('\n');
                }
                Console.WriteLine("La manche " + manche + " commence !!");
                Thread.Sleep(4000);
                for (int i = 0; i < 10; i++)
                {
                    Console.Write('\n');
                }
                Console.WriteLine("C'est le tour de " + Joueur1.Nom);
                int role = 0;//Variable pour le jeu tour par tour
                //Manche n pour le joueur 1
                while (role == 0)
                {
                    //Donne l'exemple avant le début de la manche 1
                    if (manche == 1)
                    {
                        Console.WriteLine("Lorsque vous trouvez un mot, saisissez le mot, le point d'ancrage(ligne/colonne) et sa direction");
                        Thread.Sleep(2000);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Exemple : chevreau 2 6 E");
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(4000);
                    }
                    Plateau test1;//Initialisation du plateau pour cette manche
                    test1 = Plateau.Aleatoire(filenamelangue, manche);//Génère aléatoirement le plateau
                    //Compte le score maximale que peut atteindre le joueur
                    for (int i = 0; i < test1.Achercher.Count; i++)
                    {
                        scoremaxi += test1.Achercher[i].Nom.Length;
                    }
                    plateauxdujeu.Add(test1);//Permet d'ajouter le plateau dans le jeu en cours
                    test1.ToFile(emplacement);//Sauvegarde le plateau dans un dossier avec le nom de la partie
                    Console.WriteLine(test1.toString());//Décrit le Plateau
                    Thread.Sleep(3000);
                    chronojoueur1.Start();//Chrono du joueur 1 commence
                    List<string> motstrouves = new List<string>();//Pour afficher les mots trouvés du plateau en couleur
                    test1.Afficher(motstrouves); //Affiche le plateau du joueur 1 et les mots à chercher
                    DateTime seconde = DateTime.Now;
                    DateTime limite = Limite(seconde, manche);//Défini le temps de jeu pour la manche n
                    //Le joueur commence à jouer
                    while (DateTime.Now < limite && Joueur1.Score != scoremaxi)//S'il dépasse le temps limite ou qu'il atteint le score maximum la boucle s'arr^te
                    {
                        bool erreur = true;
                        string mot = Console.ReadLine().ToUpper();
                        string[] subs = mot.Split(' ', '.');//Divise ce que le joueur à écrit en différents éléments dans tableau                       
                        while (subs.Length != 4 || erreur) //Saisie sécurisé (4 correspond au mot, ligne, colonne, direction)(le mot passe obligatoirement ici)
                        {
                            try
                            {
                                //if n°1
                                if (subs.Length != 4)
                                {
                                    //Dans ce cas, le programme reprend dans la boucle if n°2
                                }
                                else
                                {
                                    //Ici il y a un problème de format donc catch
                                    Convert.ToInt32(subs[1]);
                                    Convert.ToInt32(subs[2]);
                                    erreur = false;
                                }
                            }
                            catch (FormatException) //Si il y a une erreur de format alors le joueur doit à nouveau saisir un mot puis boucle if n°2
                            {
                            }
                            //if n°2
                            if (subs.Length != 4 || erreur)
                            {
                                //Redemande au joueur d'écrire à nouveau le mot
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("FAUX");
                                Console.ForegroundColor = ConsoleColor.White;
                                mot = Console.ReadLine().ToUpper();
                                subs = mot.Split(' ', '.');
                            }
                        }
                        //Vérifie si le mot écrit correct, bien positionner etc
                        if (test1.Test_Plateau(subs[0], Convert.ToInt32(subs[1]), Convert.ToInt32(subs[2]), subs[3]) && dictionnairedujeu.RechDichoRecursif(subs[0]) && !Joueur1.Nbmot.Contains(subs[0]))
                        {
                            Joueur1.Add_Score(subs[0].Length);
                            Joueur1.Add_Mot(subs[0]);
                            motstrouves.Add(subs[0]);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Votre score vaut désormais :" + Joueur1.Score);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine();
                            test1.Afficher(motstrouves);
                        }
                        //Cas où il se trompe
                        else 
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("FAUX");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    chronojoueur1.Stop();//Son chrono s'arrête
                    role = 1;//Joueur2
                }
                for (int i = 0; i < 10; i++)
                {
                    Console.Write('\n');
                }
                Console.WriteLine("C'est le tour de " + Joueur2.Nom);
                //Même principe pour le Joueur 2
                while (role == 1)
                {
                    if (manche == 1)
                    {
                        Console.WriteLine("Lorsque vous trouvez un mot, saisissez le mot, le point d'ancrage(ligne/colonne) et sa direction séparé par des espaces");
                        Thread.Sleep(2000);
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Exemple : influence 9 3 NO");
                        Console.ForegroundColor = ConsoleColor.White;
                        Thread.Sleep(3000);
                    }
                    Plateau test1;
                    test1 = Plateau.Aleatoire(filenamelangue, manche);
                    for (int i = 0; i < test1.Achercher.Count; i++)
                    {
                        scoremaxi2 += test1.Achercher[i].Nom.Length;
                    }
                    while (scoremaxi2 != scoremaxi)//Pour qu'il n'y ai pas d'inégalités entre les deux joueurs, les deux joueurs doivent avoir le même score maximum pour chaque partie
                    {
                        test1 = Plateau.Aleatoire(filenamelangue, manche);
                        scoremaxi2 = 0;
                        for (int i = 0; i < test1.Achercher.Count; i++)
                        {
                            scoremaxi2 += test1.Achercher[i].Nom.Length;
                        }
                    }
                    plateauxdujeu.Add(test1);
                    test1.ToFile(emplacement);
                    Console.WriteLine(test1.toString());
                    Thread.Sleep(3000);
                    chronojoueur2.Start();
                    List<string> motstrouves = new List<string>();
                    test1.Afficher(motstrouves); 
                    DateTime seconde = DateTime.Now;
                    DateTime limite = Limite(seconde, manche);
                    while (DateTime.Now < limite && Joueur2.Score != scoremaxi2)
                    {
                        bool erreur = true;
                        string mot = Console.ReadLine().ToUpper();
                        string[] subs = mot.Split(' ', '.');
                        while (subs.Length != 4 || erreur) //Saisie sécurisé
                        {
                            try
                            {
                                if (subs.Length != 4)
                                {

                                }
                                else
                                {
                                    Convert.ToInt32(subs[1]);
                                    Convert.ToInt32(subs[2]);
                                    erreur = false;
                                }
                            }
                            catch (FormatException) //Si il y a une erreur de format alors le joueur doit à nouveau saisir un mot
                            {
                            }
                            if (subs.Length != 4 || erreur)
                            {
                                Console.ForegroundColor = ConsoleColor.Red;
                                Console.WriteLine("FAUX");
                                Console.ForegroundColor = ConsoleColor.White;
                                mot = Console.ReadLine().ToUpper();
                                subs = mot.Split(' ', '.');
                            }
                        }
                        if (test1.Test_Plateau(subs[0], Convert.ToInt32(subs[1]), Convert.ToInt32(subs[2]), subs[3]) && dictionnairedujeu.RechDichoRecursif(subs[0]) && !Joueur2.Nbmot.Contains(subs[0]))
                        {
                            Joueur2.Add_Score(subs[0].Length);
                            Joueur2.Add_Mot(subs[0]);
                            motstrouves.Add(subs[0]);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Votre score vaut désormais :" + Joueur2.Score);
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.WriteLine();
                            test1.Afficher(motstrouves);
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("FAUX");
                            Console.ForegroundColor = ConsoleColor.White;
                        }
                    }
                    chronojoueur2.Stop();
                    role = 0;//Joueur 1
                }
                for (int i = 0; i < 10; i++)
                {
                    Console.Write('\n');
                }
                Console.WriteLine("Timer du joueur 1 : " + (chronojoueur1.Elapsed + tempsaddi1));//Temps additionnel dans le cas d'une sauvegarde
                Console.WriteLine(Joueur1.toString());
                Console.Write('\n');
                Console.WriteLine("Timer du joueur 1 : " + (chronojoueur2.Elapsed + tempsaddi2)); //Affiche le temps que le joueur a fait
                Console.WriteLine(Joueur2.toString());
                Thread.Sleep(5000);
                //Supprime le dossier du jeu se trouvant dans la partie en cours (cas où il y a eu une sauvegarde)
                if (manche == 5)
                {
                    Console.WriteLine("FINI !!!");
                    string chemin = @"Jeu/Parties_en_cours/" + emplacement;
                    if (Directory.Exists(chemin))
                    {
                        string[] files = Directory.GetFiles(chemin); //Tous les noms des fichiers de ce dossier sont enregistrés dans un tableau
                        foreach (string file in files) //pour chaque élément dans files
                        {
                            File.SetAttributes(file, FileAttributes.Normal);
                            File.Delete(file);//Supprimer les fichiers
                        }
                        Directory.Delete(chemin);
                    }
                }
                //Autres manches
                else
                {
                    //Cas si on veut sauvegarder ou non
                    Console.WriteLine("Souhaitez vous sauvegarder la partie en cours et reprendre prochainement ?");
                    string ouiounon = Console.ReadLine().ToLower();
                    while (ouiounon != "oui" && ouiounon != "non")
                    {
                        Console.WriteLine("Resaisir");
                        ouiounon = Console.ReadLine(); //Saisie sécurisée
                    }
                    if (ouiounon == "non")
                    {
                        Console.WriteLine("Fin de la manche " + manche + '\n' + "La manche " + (manche + 1) + " va commencer...");
                    }
                    else
                    {   //Sauvegarde de toutes les variables dans un fichier csv                                                   
                        string motsjoueur1 = "";
                        string motsjoueur2 = "";
                        for (int i = 0; i < Joueur1.Nbmot.Count(); i++) //Sauvegarde tous les mots trouvés
                        {
                            motsjoueur1 += Joueur1.Nbmot[i] + ";";
                        }
                        for (int i = 0; i < Joueur2.Nbmot.Count(); i++)
                        {
                            motsjoueur2 += Joueur2.Nbmot[i] + ";";
                        }
                        //Garde le chrono des joueurs
                        tempsaddi1 = (chronojoueur1.Elapsed + tempsaddi1);
                        tempsaddi2 = (chronojoueur2.Elapsed + tempsaddi2);
                        donnees += (manche + 1) + ";" + "\n" + Joueur1.Nom + ";" + Joueur1.Score + ";\n" + motsjoueur1 + "\n" + Joueur2.Nom + ";" + Joueur2.Score + ";\n" + motsjoueur2 + "\n" + emplacement + ";\n" + filenamelangue + ";" + langue + ";\n" + tempsaddi1.Hours + ";" + tempsaddi1.Minutes + ";" + tempsaddi1.Seconds + ";\n" + tempsaddi2.Hours + ";" + tempsaddi2.Minutes + ";" + tempsaddi2.Seconds + ";";
                        Jeu.Sauvegarde(emplacement, donnees);//utilise la méthode sauvegarde
                        manche = 98;//Ensuite manche+=1 =99 => Partie sauvegardée
                    }
                }
                Thread.Sleep(4000);
                manche++;
            }
            Console.ForegroundColor = ConsoleColor.Green;
            if (Joueur1.Score > Joueur2.Score && manche != 99)
            {
                Console.WriteLine("Le joueur " + Joueur1.Nom + " a gagné !!!");
                Thread.Sleep(5000);
            }
            else if (Joueur1.Score < Joueur2.Score && manche != 99)
            {
                Console.WriteLine("Le joueur " + Joueur2.Nom + " a gagné !!!");
                Thread.Sleep(5000);
            }
            else if (manche == 99)
            {
                Console.Write("La partie a bien été sauvegardé");
                Thread.Sleep(5000);
            }
            else //Ici on vérifie celui qui a été le plus rapide
            {
                if ((chronojoueur1.Elapsed + tempsaddi1) > (chronojoueur2.Elapsed + tempsaddi2))
                {
                    Console.WriteLine("Le joueur " + Joueur2.Nom + " a gagné !!!");
                    Thread.Sleep(5000);
                }
                else if ((chronojoueur1.Elapsed + tempsaddi1) < (chronojoueur2.Elapsed + tempsaddi2))
                {
                    Console.WriteLine("Le joueur " + Joueur1.Nom + " a gagné !!!");
                    Thread.Sleep(5000);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Match nul !");
                    Thread.Sleep(5000);
                }
            }
        }
    }
}
