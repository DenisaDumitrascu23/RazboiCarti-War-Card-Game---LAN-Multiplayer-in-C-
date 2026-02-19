using System;                               // Tipuri de bază din .NET
using System.Threading;                    // Mutex (sincronizare între instanțe)
using System.Windows.Forms;                // WinForms (Form, MessageBox)
using Microsoft.VisualBasic;               // InputBox pentru introducerea IP-ului

namespace RazboiCarti                      // Namespace-ul aplicației
{
    static class Program
    {
        [STAThread]                      // necesar pentru WinForms
        static void Main()               // punctul de intrare (OBLIGATORIU)
        {
            new ProgramRunner().Ruleaza(); // logica e în clasa non-static
        }
    }

    /// Clasă normală (NON-static) care conține toată logica aplicației
    class ProgramRunner
    {
        //CONSTANTE MUTEX - un mecanism al sistemului de operare care permite ca o singură instanță a aplicației să „dețină” un rol (Host sau Client) la un moment dat

        // Nume unic pentru mutex-ul de Host (prima instanță)
        private const string MutexHost = "RazboiCarti_MUTEX_HOST_UNIC";

        // Nume unic pentru mutex-ul de Client (a doua instanță)
        private const string MutexClient = "RazboiCarti_MUTEX_CLIENT_UNIC";

        //VARIABILE DE INSTANȚĂ

        private Mutex mutexHost;            // Mutex pentru Host (Jucătorul 1)
        private Mutex mutexClient;          // Mutex pentru Client (Jucătorul 2)

        //METODA PRINCIPALĂ A APLICAȚIEI

        public void Ruleaza()
        {
            // Setări standard pentru aplicații WinForms
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Afișăm un mesaj la pornire ca utilizatorul să aleagă rolul
            // YES  -> Host (Jucătorul 1)
            // NO   -> Client (Jucătorul 2)
            DialogResult rezultat = MessageBox.Show(
                "Vrei să fii Host (Jucătorul 1)?",
                "Alegere rol",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            //CAZ HOST
            if (rezultat == DialogResult.Yes)
            {
                // Încercăm să obținem mutex-ul de Host
                // Mutex-ul garantează că pe ACELAȘI joc nu pot exista doi Host
                if (!IncearcaPornireHost())
                {
                    // Dacă mutex-ul nu poate fi obținut, înseamnă că există deja un Host
                    MessageBox.Show(
                        "Există deja un Host pornit pe acest PC.",
                        "Eroare",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error
                    );
                    return; // Ieșim din aplicație
                }

                // Pornim aplicația ca Host (Jucătorul 1)
                // true  = Host
                // IP nu este necesar pentru Host
                Application.Run(new Form1(true, ""));

                // La închiderea aplicației eliberăm mutex-urile
                ElibereazaMutex();
                return;
            }

            // CAZ CLIENT

            // Încercăm să obținem mutex-ul de Client
            // Acesta garantează că pe același joc nu pot exista doi Client
            if (!IncearcaPornireClient())
            {
                MessageBox.Show(
                    "Există deja un Client pornit pe acest PC.",
                    "Eroare",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return; // Ieșim din aplicație
            }

            // Cerem utilizatorului IP-ul Host-ului
            // (ex: 127.0.0.1 pe același PC sau IP-ul din LAN)
            string ip = CereIpHost();

            // Dacă utilizatorul a anulat sau a introdus text gol
            if (string.IsNullOrWhiteSpace(ip))
            {
                MessageBox.Show("Conectare anulată.");
                ElibereazaMutex(); // Eliberăm mutex-ul de Client
                return;
            }

            // Pornim aplicația ca Client (Jucătorul 2)
            // false = Client
            // IP-ul este necesar pentru conectare
            Application.Run(new Form1(false, ip.Trim()));

            // La închiderea aplicației eliberăm mutex-urile
            ElibereazaMutex();
        }

        //METODE AJUTĂTOARE

        private bool IncearcaPornireHost()             // Verifică dacă putem fi Host
        {
            bool hostNou;                              // true = prima instanță
            mutexHost = new Mutex(true, MutexHost, out hostNou); // Creează mutex Host
            return hostNou;                            // Returnează rezultatul
        }

        private bool IncearcaPornireClient()           // Verifică dacă putem fi Client
        {
            bool clientNou;                            // true = nu există alt client
            mutexClient = new Mutex(true, MutexClient, out clientNou); // Creează mutex Client
            return clientNou;                          // Returnează rezultatul
        }

        private string CereIpHost()                    // Cere IP-ul Host-ului
        {
            return Interaction.InputBox(
                "Introdu IP-ul Jucătorului 1 (Host):\n\n" +
                "- Pe același PC: 127.0.0.1\n" +
                "- In LAN: (Network and internet -> Wi-Fi -> IPv4)",
                "Conectare Jucătorul 2",
                "127.0.0.1"                            // IP implicit
            );
        }

        private void ElibereazaMutex()                 // Eliberează mutex-urile la ieșire
        {
            try
            {
                if (mutexClient != null)              // Dacă mutex-ul Client există
                {
                    mutexClient.ReleaseMutex();       // Eliberăm mutex-ul
                    mutexClient.Dispose();            // Eliberăm resursele
                    mutexClient = null;               // Curățăm referința
                }
            }
            catch { }                                 // Ignorăm erorile

            try
            {
                if (mutexHost != null)                // Dacă mutex-ul Host există
                {
                    mutexHost.ReleaseMutex();         // Eliberăm mutex-ul
                    mutexHost.Dispose();              // Eliberăm resursele
                    mutexHost = null;                 // Curățăm referința
                }
            }
            catch { }                                 // Ignorăm erorile
        }
    }
}
