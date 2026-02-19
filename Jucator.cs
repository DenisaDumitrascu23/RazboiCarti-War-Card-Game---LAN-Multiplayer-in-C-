using System;
using System.Collections.Generic; // colecții generice din biblioteca standard .NET (list, quene, stack...)

namespace RazboiCarti
{
    /// Clasă abstractă pentru un jucător
    internal abstract class Jucator
    {
        // Numele este util în general, chiar dacă în UI afișezi "Jucătorul 1/2"
        private string nume;

        // Lista de cărți este privată ca să nu poată fi modificată haotic din exterior
        private List<Carte> carti;

        // Scorul (runde câștigate) este privat: crește doar prin CastigaRunda()
        private int rundeCastigate;

        /// Constructor: fiecare jucător pornește cu 0 runde câștigate și o listă goală de cărți
        protected Jucator(string nume)
        {
            if (string.IsNullOrWhiteSpace(nume))
                nume = "Jucător";

            this.nume = nume;
            this.carti = new List<Carte>();
            this.rundeCastigate = 0;
        }

        /// Returnează numele jucătorului
        /*public string GetNume()
        {
            return nume;
        }*/

        /// Returnează lista de cărți a jucătorului
        internal List<Carte> GetCarti()
        {
            return carti;
        }

        /// Returnează câte runde a câștigat jucătorul
        internal int GetRundeCastigate()
        {
            return rundeCastigate;
        }

        /// Adaugă o carte la jucător
        /// Este folosită la distribuirea cărților din pachet
        internal void AdaugaCarte(Carte carte)
        {
            if (carte != null)
                carti.Add(carte);
        }

        /// Crește scorul jucătorului cu 1 rundă câștigată
        /// Punem creșterea aici ca să nu modificăm direct rundeCastigate din alte clase
        internal void CastigaRunda()
        {
            rundeCastigate++;
        }

        /// Metodă abstractă
        /// Metoda trebuie să scoată cartea din listă, ca să nu se repete
        internal abstract Carte JoacaCarte();
    }
}
