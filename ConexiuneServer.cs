using System;                               // Tipuri de bază (Action, Exception etc.)
using System.IO;                            // Citire și scriere de date (StreamReader/Writer)
using System.Net;                           // Lucru cu adrese IP
using System.Net.Sockets;                  // Comunicare prin rețea (TCP)
using System.Threading;                    // Thread-uri (execuție paralelă)

namespace RazboiCarti                      
{
    /// ConexiuneServer = Host (Jucătorul 1)
    /// Se ocupă DOAR de comunicarea în rețea
    internal class ConexiuneServer            // Clasa care reprezintă serverul (host)
    {
        private int port;                   // Portul pe care este serverul 
        private TcpListener listener;       // Obiect care așteaptă conexiuni de la clienți
        private TcpClient client;           // Clientul conectat (Jucătorul 2)
        private StreamReader reader;        // Citește mesaje primite de la client
        private StreamWriter writer;        // Trimite mesaje către client
        private Thread threadAccept;        // Thread care așteaptă conectarea clientului
        private Thread threadCitire;        // Thread care citește mesajele clientului
        private bool ruleaza;               // Spune dacă serverul este pornit

        /// Mesaje primite de la client (Jucătorul 2)
        public event Action<string> MesajPrimit; // Eveniment declanșat când vine un mesaj

        /// Eveniment: se declanșează când clientul s-a conectat
        public event Action ClientConectat; // Eveniment pentru UI (client conectat)

        public ConexiuneServer(int port)    // Constructorul serverului
        {
            this.port = port;               // Salvează portul primit
        }

        /// Pornește serverul
        public void Porneste()              // Metodă apelată pentru a porni host-ul
        {
            if (ruleaza)                    // Dacă serverul rulează deja
                return;                     // Ieșim ca să nu îl pornim din nou

            ruleaza = true;                 // Marcăm serverul ca activ

            listener = new TcpListener(IPAddress.Any, port); // Creează listener pe orice IP
            listener.Start();               // Începe să caute conexiuni

            threadAccept = new Thread(AcceptaClient); // Creează thread-ul de acceptare
            threadAccept.IsBackground = true;          // Rulează în fundal
            threadAccept.Start();                      // Pornește thread-ul
        }

        /// Spune dacă există un client conectat acum
        /*public bool EsteClientConectat()    // Metodă de verificare conexiune
        {
            if (client == null)             // Dacă nu există client
                return false;               // Nu este nimeni conectat

            return client.Connected;        // Returnează starea conexiunii
        }*/

        /// Așteaptă conectarea clientului
        private void AcceptaClient()        // Metodă rulată pe thread separat
        {
            try                             // Încearcă să accepte un client
            {
                client = listener.AcceptTcpClient(); // Așteaptă până se conectează cineva

                NetworkStream stream = client.GetStream(); // Obține fluxul de date
                reader = new StreamReader(stream);         // Creează cititorul de mesaje
                writer = new StreamWriter(stream);         // Creează scriitorul de mesaje
                writer.AutoFlush = true;                   // Trimite mesajele imediat

                if (ClientConectat != null) // Dacă cineva ascultă evenimentul
                    ClientConectat();       // Anunță că s-a conectat clientul

                threadCitire = new Thread(CitesteMesaje); // Creează thread-ul de citire
                threadCitire.IsBackground = true;         // Thread de fundal
                threadCitire.Start();                     // Pornește citirea mesajelor
            }
            catch                           // Dacă apare o eroare
            {
                // Server oprit sau eroare de rețea
            }
        }

        /// Citește mesajele primite de la client
        private void CitesteMesaje()        // Metodă rulată pe thread separat
        {
            try                             // Încearcă citirea mesajelor
            {
                while (ruleaza)             // Cât timp serverul este activ
                {
                    string mesaj = reader.ReadLine(); // Așteaptă un mesaj de la client

                    if (mesaj == null)      // Dacă clientul s-a deconectat
                        break;              // Ieșim din buclă

                    if (MesajPrimit != null)// Dacă există abonați la eveniment
                        MesajPrimit(mesaj); // Trimite mesajul către aplicație (UI)
                }
            }
            catch                           // Dacă apare o eroare
            {
                // Conexiune întreruptă
            }
        }

        /// Trimite un mesaj către client
        public void Trimite(string mesaj)   // Metodă pentru trimiterea mesajelor
        {
            try                             // Încearcă trimiterea
            {
                if (writer != null)         // Dacă writer-ul există
                    writer.WriteLine(mesaj);// Trimite mesajul către client
            }
            catch                           // Dacă nu se mai poate trimite
            {
                // Client deconectat
            }
        }

        /// Oprește serverul
        public void Opreste()               // Metodă de oprire a serverului
        {
            ruleaza = false;                // Oprește thread-ul de citire

            try
            {
                if (client != null)         // Dacă există client
                    client.Close();         // Închide conexiunea cu clientul
            }
            catch { }                       // Ignoră erorile

            try
            {
                if (listener != null)       // Dacă listener-ul există
                    listener.Stop();        // Oprește ascultarea pe port
            }
            catch { }                       // Ignoră erorile
        }
    }
}
