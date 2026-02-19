using System;                     // Tipuri de bază (Random, Exception etc.)
using System.Collections.Generic; // List<T>
using System.IO;                  // Lucru cu fișiere și directoare (Directory, Path)
using System.Windows.Forms;       // Application, MessageBox (WinForms)

namespace RazboiCarti
{
    /// Clasa care reprezintă pachetul de cărți al jocului
    /// Se ocupă de:
    /// - încărcarea cărților din folderul Images
    /// - crearea obiectelor Carte
    /// - amestecarea pachetului
    internal class ClasaPachet
    {
        // Lista internă care conține toate cărțile din pachet
        // Este privată pentru a respecta încapsularea
        public List<Carte> carti;

        // Calea către folderul Images din aplicație
        public string folderImagini;

        // Generator de numere aleatoare, folosit la amestecarea cărților
        private Random rnd = new Random();

        /// Constructorul clasei ClasaPachet
        /// Este apelat automat când creăm un pachet nou
        public ClasaPachet()
        {
            // Inițializăm lista de cărți (pachetul este gol la început)
            carti = new List<Carte>();

            // Construim calea completă către folderul Images
            // Application.StartupPath = folderul unde rulează aplicația
            folderImagini = Path.Combine(Application.StartupPath, "Images");

            // Încărcăm toate cărțile din folderul Images
            IncarcaCartiDinFolder();

            // Amestecăm pachetul după ce a fost încărcat
            Shuffle();
        }

        /// Încarcă toate fișierele .png din folderul Images
        /// Fiecare fișier valid devine o carte unică
        private void IncarcaCartiDinFolder()
        {
            // Verificăm dacă folderul Images există
            if (!Directory.Exists(folderImagini))
            {
                // Dacă nu există, afișăm un mesaj de eroare
                MessageBox.Show("Folderul Images nu există");
                return; // Ieșim din metodă
            }

            // Luăm toate fișierele cu extensia .png din folder
            string[] fisiere = Directory.GetFiles(folderImagini, "*.png");

            // Parcurgem fiecare fișier găsit
            for (int i = 0; i < fisiere.Length; i++)
            {
                // Calea completă către fișierul curent
                string caleFisier = fisiere[i];

                // Numele fișierului fără extensie
                // Ex: "10_Inimi.png" -> "10_Inimi"
                string numeFisier = Path.GetFileNameWithoutExtension(caleFisier);

                // Ignorăm imaginea de spate a cărții
                if (numeFisier == "Spate_Carte")
                    continue;

                // Împărțim numele după caracterul "_"
                // Ex: "10_Inimi" -> ["10", "Inimi"]
                string[] parti = numeFisier.Split('_');

                // Dacă formatul nu este corect, ignorăm fișierul
                if (parti.Length != 2)
                    continue;

                // Prima parte reprezintă valoarea cărții (2–10, J, Q, K, A)
                string textValoare = parti[0];

                int valoareCarte;

                // Încercăm să transformăm valoarea din text în int
                // Dacă nu reușim, fișierul este ignorat
                if (!IncearcaCitireValoare(textValoare, out valoareCarte))
                    continue;

                // Creăm obiectul Carte folosind datele obținute
                // Fiecare fișier corespunde unei cărți unice
                Carte carte = new Carte(numeFisier, valoareCarte, caleFisier);

                // Adăugăm cartea în lista pachetului
                carti.Add(carte);
            }
        }

        /// Transformă textul valorii unei cărți în valoare numerică
        /// Returnează true dacă transformarea reușește
        /// Returnează false dacă valoarea este invalidă
        private bool IncearcaCitireValoare(string textValoare, out int valoare)
        {
            // Inițializăm valoarea cu 0
            valoare = 0;

            // Cazuri speciale pentru figurile din pachet
            if (textValoare == "J") { valoare = 11; return true; }
            if (textValoare == "Q") { valoare = 12; return true; }
            if (textValoare == "K") { valoare = 13; return true; }
            if (textValoare == "A") { valoare = 14; return true; }

            // Pentru cărțile numerice (2–10)
            // TryParse returnează true dacă conversia reușește
            return int.TryParse(textValoare, out valoare);
        }

        /// Amestecă lista de cărți folosind un algoritm de tip Fisher–Yates
        internal void Shuffle()
        {
            // Dacă sunt 0 sau 1 cărți, nu este nevoie de amestecare
            if (carti.Count < 2)
                return;

            int n = carti.Count;

            // Parcurgem lista de la final spre început
            while (n > 1)
            {
                n--;

                // Alegem un index aleator între 0 și n
                int k = rnd.Next(n + 1);

                // Schimbăm cartea de la poziția k cu cea de la poziția n
                Carte temp = carti[k];
                carti[k] = carti[n];
                carti[n] = temp;
            }
        }

        /// Extrage o carte din pachet
        /// Cartea extrasă este eliminată din listă
        /*internal Carte ExtrageCarte()
        {
            // Dacă pachetul este gol, nu avem ce extrage
            if (carti.Count == 0)
                return null;

            // Luăm prima carte din listă
            Carte carte = carti[0];

            // O eliminăm din pachet pentru a nu putea fi extrasă din nou
            carti.RemoveAt(0);

            // Returnăm cartea extrasă
            return carte;
        }*/

        /// Returnează lista de cărți din pachet
        /// Este folosită la distribuirea inițială în ClasaMeci
        internal List<Carte> GetCarti()
        {
            // Returnăm lista internă de cărți
            return carti;
        }
    }
}
