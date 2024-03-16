# -Projet-jeu-mots-m-l-es

Nous avons créé un programme en POO permettant de générer un jeu de de mots mêlés pours deux joueurs. Pour cela, nous avons créé 5 classes différentes (mot, dictionnaire, jeu, joueur et plateau). Nous avons généré aléatoirement les grilles pour chaque niveau et une méthode permettant de sauvegarder ou non la partie de jeu en cours. Cela sauvegarde les grilles, les mots trouvés et les paramètres des joueurs.

CLASSE DICTIONNAIRE : Cette classe permet dans un premier temps d’afficher tous les mots du dictionnaire en fonction de leur lettre et si le mot rentré par le joueur existe bien dans le fichier texte contenant tous les mots avec la méthode RechDicoRecursif().

CLASSE JOUEUR : Elle indique les paramètres du joueur avec leur nom, leur score et Addmot() qui permet de compter les mots trouvés.

CLASSE MOT: est caractérisée par le mot, lignes, colonnes et directions tous les mots du plateau enregistrés (excepté les tableaux créés aléatoirement) ont un fichier annexe dans le debug où il y a tous les mots, lignes, colonne et directions.

CLASSE PLATEAU : permet de lire, créer, afficher et sauvegarder les grilles se caractérisant par la matrice, les mots et la difficulté. Elle contient la méthode Aleatoire() permettant de générer aléatoirement un plateau. Selon la difficulté, on choisit une direction aléatoirement puis on regarde la localisation d’une lettre choisie aléatoirement du mot placé précédemment (boucle while + ObtenirPlacement()). Selon la direction, on regarde les emplacements libres et on regarde les mots avec la taille inférieure ou égale à l’espace libre et une lettre en commun avec la lettre précédemment choisie. On écrit le mot avec Placer(). Dans le cas où on ne peut pas écrire le mot, on regarder les autres lettres et les autres directions aléatoirement. Si on ne peut toujours pas, le mot est placé aléatoirement sans croisement sur la grille.

CLASSE JEU: permet de concrètement de jouer avec la méthode Play(). Elle affiche les données du joueur avec le nom, le score, le nombre de mots trouvés, le timer et si sa partie a été sauvegardée. Concernant la sauvegarde, à chaque nouvelle manche le joueur a choix de sauvegarder ou non sa partie en cours pour pouvoir la reprendre plus tard. S’il sauvegarde, il trouvera toutes ses données de jeu dans un fichier dans le debug avec la méthode Sauvegarde(). Toutefois, à la fin du jeu, le fichier contenant sa sauvegarde sera automatiquement supprimé.
