using System; // Tipuri de bază din .NET (Exception, etc.)
using System.Linq;

namespace RazboiCarti
{
    /// Clasa care reprezintă logica principală a jocului „Război”
    /// După ștergerea Meci.cs, această clasă conține TOT ce ținea de meci:
    /// - jucători
    /// - pachet
    /// - runde
    /// - ultimele cărți jucate
    internal class ClasaMeci
    {
        // Pachetul de cărți folosit în joc
        // Compoziție: meciul ARE un pachet
        private ClasaPachet pachet;

        // Jucătorii jocului
        // Sunt privați pentru a respecta încapsularea
        private Jucator jucator1;
        private Jucator jucator2;

        // Ultimele cărți jucate într-o rundă
        // Sunt salvate pentru a fi afișate în interfață (Form1)
        private Carte ultimaCarteJ1;
        private Carte ultimaCarteJ2;

        // Proprietate publică doar pentru citire
        // Form1 poate citi jucătorul 1, dar nu îl poate modifica
        internal Jucator Jucator1 { get { return jucator1; } }

        // Proprietate publică doar pentru citire pentru jucătorul 2
        internal Jucator Jucator2 { get { return jucator2; } }

        // Proprietate read-only pentru ultima carte a Jucătorului 1
        internal Carte UltimaCarteJ1 { get { return ultimaCarteJ1; } }

        // Proprietate read-only pentru ultima carte a Jucătorului 2
        internal Carte UltimaCarteJ2 { get { return ultimaCarteJ2; } }

        // Metodă privată pentru setarea jucătorilor
        // Este apelată DOAR în constructor
        // Astfel nu permitem schimbarea jucătorilor din exterior
        private void SetJucatori(Jucator j1, Jucator j2)
        {
            jucator1 = j1; // setăm referința către jucătorul 1
            jucator2 = j2; // setăm referința către jucătorul 2
        }

        // Metodă privată pentru salvarea ultimelor cărți jucate
        // Este folosită după fiecare rundă
        private void SetUltimeleCarti(Carte c1, Carte c2)
        {
            ultimaCarteJ1 = c1; // salvăm cartea jucată de J1
            ultimaCarteJ2 = c2; // salvăm cartea jucată de J2
        }

        /// Constructorul clasei ClasaMeci
        /// Este apelat când începe un joc nou
        public ClasaMeci()
        {
            // Creăm jucătorul 1 (Host)
            Jucator j1 = new ClasaJucator("Jucătorul 1");

            // Creăm jucătorul 2 (Client)
            Jucator j2 = new ClasaJucator("Jucătorul 2");

            // Setăm jucătorii în meci
            SetJucatori(j1, j2);

            // Creăm pachetul de cărți
            // Constructorul pachetului:
            // - încarcă imaginile
            // - creează cărțile
            // - amestecă pachetul
            pachet = new ClasaPachet();

            // Distribuim cărțile către cei doi jucători
            DistribuieCarti();
        }

        /// Distribuie cărțile alternativ:
        /// prima carte la J1, a doua la J2 etc.
        /// Astfel fiecare primește jumătate din pachet
        private void DistribuieCarti()
        {
            // Luăm lista completă de cărți din pachet
            var toateCartile = pachet.GetCarti();

            int i = 0; // index pentru parcurgerea listei

            // Parcurgem toate cărțile
            while (i < toateCartile.Count)
            {
                // Adăugăm o carte la jucătorul 1
                if (i < toateCartile.Count)
                {
                    Jucator1.AdaugaCarte(toateCartile[i]);
                    i++;
                }

                // Adăugăm o carte la jucătorul 2
                if (i < toateCartile.Count)
                {
                    Jucator2.AdaugaCarte(toateCartile[i]);
                    i++;
                }
            }

            // Cărțile nu sunt copiate, ci mutate ca referințe
            // Nu există duplicate
        }
        /// Verifică dacă ambii jucători mai au cărți
        /// Jocul se termină când unul rămâne fără cărți
        public bool AreCarti()
        {
            return Jucator1.GetCarti().Count > 0
                && Jucator2.GetCarti().Count > 0;
        }

        /// Joacă o rundă completă de joc
        public void JoacaRunda()
        {
            // Dacă nu mai sunt cărți, nu mai jucăm runde
            if (!AreCarti())
                return;

            // Jucătorul 1 joacă o carte (este scoasă din lista lui)
            Carte carteJ1 = Jucator1.JoacaCarte();

            // Jucătorul 2 joacă o carte
            Carte carteJ2 = Jucator2.JoacaCarte();

            // Salvăm ultimele cărți pentru UI
            SetUltimeleCarti(carteJ1, carteJ2);

            // Dacă ceva a mers prost și o carte este null, oprim runda
            if (carteJ1 == null || carteJ2 == null)
                return;

            // Comparăm valorile cărților
            // Metoda Compara este definită în Carte
            int rezultat = carteJ1.Compara(carteJ2);

            // Dacă J1 are cartea mai mare, câștigă runda
            if (rezultat > 0)
                Jucator1.CastigaRunda();

            // Dacă J2 are cartea mai mare, câștigă runda
            else if (rezultat < 0)
                Jucator2.CastigaRunda();

            // Dacă este egalitate, nimeni nu primește punct
        }
    }
}
