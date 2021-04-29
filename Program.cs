using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using jeu_du_pendu;

namespace jeu_du_pendu
{
    class Program
    {

        static void AfficherMot(string mot, List<char> lettres)
        {
            // il faut faire afficher _ _ _ _ par rapport au nombre de lettres du mot donné
            for(int i = 0; i < mot.Length; i++)
            {
                char lettre = mot[i];
                
                if(lettres.Contains(lettre))
                {
                    Console.Write($"{lettre} ");
                }
                else
                {
                    Console.Write("_ ");
                }
            }
            
            Console.WriteLine();

        }

        static bool ToutesLettresDevinees(string mot, List<char> lettres)
        {
            // true -> toutes les lettres ont été trouvées -> gagné
            //false
            
           foreach(var lettre in lettres)
            {
                mot = mot.Replace(lettre.ToString(), "");
            }
            if(mot == "")
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        static char DemanderUneLettre(string message = "Rentrez une lettre : ")
        {

            while(true)
            {
                Console.Write(message);
                string reponse = Console.ReadLine();
                if(reponse.Length == 1)
                {
                    reponse = reponse.ToUpper();
                    return reponse[0];
                }
                Console.WriteLine("ERREUR : Vous devez rentrer une lettre");
            }

        }

        static void DevinerMot(string mot)
        {

            var lettresDevinees = new List<char>();
            var lettresExclu = new List<char>();
            const int NB_VIES = 6;
            int viesRestantes = NB_VIES;




            while (viesRestantes > 0)
            {
                Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestantes]);
                Console.WriteLine();

                AfficherMot(mot, lettresDevinees);
                Console.WriteLine();
                var lettre = DemanderUneLettre();
                Console.Clear();

                if (mot.Contains(lettre))
                {
                    Console.WriteLine("Cette lettre est dans le mot");
                    lettresDevinees.Add(lettre);
                    if(ToutesLettresDevinees(mot, lettresDevinees))
                    {
                        break;
                    }
                }
                else
                {
                    if (!lettresExclu.Contains(lettre))
                    {
                        lettresExclu.Add(lettre);
                        viesRestantes--;
                        Console.WriteLine($"Nombre de vies restantes : {viesRestantes}");
                    }
                    else
                    {
                        Console.WriteLine($"Nombre de vies restantes : {viesRestantes}");
                    }
                    
                    
                }
                if(lettresExclu.Count > 0)
                {
                    Console.WriteLine($"Lettres qui ne sont pas dans le mot : {String.Join(", ", lettresExclu)}");
                }
                
                Console.WriteLine();
            }

            Console.WriteLine(Ascii.PENDU[NB_VIES - viesRestantes]);

            if (viesRestantes == 0)
            {
                Console.WriteLine($"Vous avez perdu, il fallait trouver: {mot}");
            }
            else
            {
                AfficherMot(mot, lettresDevinees);
                Console.WriteLine();

                Console.WriteLine("GAGNE !");
            }
        }

        static string[] ChargerLesMots(string monfichier)
        {
            try
            {
                return File.ReadAllLines(monfichier);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"ERREUR de lecture du fichier : {monfichier} ({ex.Message})");
            }
            return null;
        }

        static bool DemanderDeRejouer()
        {
            char reponse = DemanderUneLettre("Voulez vous rejouer (o/n) : ");

            if ((reponse == 'o') || (reponse == 'O'))
            {
                return true;
            }
            else if ((reponse == 'n') || (reponse == 'N'))
            {
                return false;
            }
            else
            {
                Console.WriteLine("ERREUR : Vous devez répondre oui (o) ou non (n)");
                return DemanderDeRejouer();
            }
        }

        static void Main(string[] args)
        {
            var mots = ChargerLesMots("mots.txt");
  
                if ((mots == null) || (mots.Length == 0))
                {
                    Console.WriteLine("La liste de mots est vide");
                }
                else
                {

                    while(true)
                    {
                        Random r = new Random();
                        int i = r.Next(mots.Length);
                        string mot = mots[i].Trim().ToUpper();
                        DevinerMot(mot);
                        if(!DemanderDeRejouer())
                        {
                            break;
                        }
                        Console.Clear();
                    }
                    Console.WriteLine("Merci et à bientôt.");
                }   
        }
    }
}
