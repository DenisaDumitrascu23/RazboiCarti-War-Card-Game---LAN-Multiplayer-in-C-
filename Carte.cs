using System;

namespace RazboiCarti
{
    /// Clasă abstractă care descrie o carte de joc
    /// Nu putem crea direct obiecte de tip Carte, ci doar cărți concrete (ex: ClasaCarte)
    internal class Carte
    {
        // Datele unei cărți trebuie protejate (incapsulare), deci din exterior pot fi doar citite, nu modificate
        public string Nume { get; private set; }
        public int Valoare { get; private set; }
        public string NumeFisier { get; private set; }

        /// Constructor protejat. Este apelat doar din clasele derivate. Verificăm datele pentru a evita obiecte invalide
        public Carte(string nume, int valoare, string numeFisier)
        {
            // Verificăm dacă numele este valid
            if (string.IsNullOrWhiteSpace(nume))
                throw new ArgumentException("Numele cărții nu poate fi gol.");

            // Verificăm dacă valoarea este corectă
            // (1–10, J=11, Q=12, K=13, A=14)
            if (valoare < 1 || valoare > 14)
                throw new ArgumentOutOfRangeException(
                    "Valoarea cărții trebuie să fie între 1 și 14.");

            // Verificăm dacă fișierul imaginii este valid
            if (string.IsNullOrWhiteSpace(numeFisier))
                throw new ArgumentException("Numele fișierului nu poate fi gol.");

            Nume = nume;
            Valoare = valoare;
            NumeFisier = numeFisier;
        }

        /*public string GetNume()
        {
            return Nume;
        }*/

        public int GetValoare()
        {
            return Valoare;
        }

        public string GetNumeFisier()
        {
            return NumeFisier;
        }

   
        /// Compară această carte cu o altă carte.
        /// Returnează:
        ///  1  -> cartea curentă este mai mare
        ///  0  -> cărțile sunt egale
        /// -1  -> cartea curentă este mai mică
   
        /// Metoda este virtuală pentru a putea fi modificată în clasele derivate dacă regulile jocului se schimbă
        public int Compara(Carte altaCarte) //altaCarte este cartea cu care se face comparatia
        {
            if (altaCarte == null) //daca nu exista alta carte, jucatorul actual primeste puctul
                return 1;

            if (Valoare > altaCarte.Valoare)
                return 1; 

            if (Valoare < altaCarte.Valoare)
                return -1;

            return 0;
        }
    }
} 
