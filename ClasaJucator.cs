using System;

namespace RazboiCarti
{
    /// Implementare concretă a unui jucător. Jucătorul joacă o carte aleator din lista lui
    internal class ClasaJucator : Jucator
    {
        private Random rnd = new Random();

        public ClasaJucator(string nume) : base(nume) //constructorul clasei ClasaJucator apelata cand creez un jucator nou
        {
        }

        /// Alege o carte aleator din lista jucătorului și o scoate din listă, astfel, aceeași carte nu poate apărea de două ori în același joc
        internal override Carte JoacaCarte() //in clasa de baza metoda a fost declarata virtual, iar clasa derivata foloseste override pentru a o modifica
        { //override foloseste la polimorfism 
            var carti = GetCarti();

            // Dacă nu mai are cărți, întoarcem null. ClasaMeci verifică AreCarti() înainte de a juca runda
            if (carti.Count == 0)
                return null;

            // Alegem un index aleator între 0 și (Count - 1)
            int index = rnd.Next(carti.Count);

            // Luăm cartea de la acel index
            Carte carteAleasa = carti[index];

            // Scoatem cartea din listă.
            // Acesta este pasul cheie ca să NU se repete cartea
            carti.RemoveAt(index);

            return carteAleasa;
        }
    }
}
