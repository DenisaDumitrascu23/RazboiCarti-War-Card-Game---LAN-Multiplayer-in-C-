using System;                         // Tipuri de bază (Action, Exception etc.)
using System.IO;                      // Lucru cu stream-uri (StreamReader, StreamWriter)
using System.Net.Sockets;             // Comunicare prin rețea (TcpClient)
using System.Threading;               // Lucru cu thread-uri (cu asta mesajele se citesc in fundal, interfata ramane activa si jucatorii pot apadsa in acelasi timp si pe butoane 

namespace RazboiCarti                 
{
    /// ConexiuneClient = Client (Jucătorul 2)
    internal class ConexiuneClient       // Clasa care gestionează conexiunea clientului
    {
        private string ip;             // IP-ul hostului
        private int port;              // Portul hostului
        private TcpClient client;      // Client TCP pentru conexiune
        private StreamReader reader;   // Citește date de la host
        private StreamWriter writer;   // Trimite date către host
        private Thread threadCitire;   // Thread separat pentru citirea mesajelor
        private bool ruleaza;           // Indică dacă conexiunea este activă

        public event Action<string> MesajPrimit; // Eveniment declanșat când vine un mesaj

        public ConexiuneClient(string ip, int port) // Constructorul clasei
        {
            this.ip = ip;              // Salvează IP-ul primit
            this.port = port;          // Salvează portul primit
        }

        /// Încearcă să se conecteze la Host
        public bool Conecteaza()        // Metodă de conectare la host
        {
            try                          // Încearcă conectarea
            {
                client = new TcpClient();           // Creează clientul TCP
                client.Connect(ip, port);           // Se conectează la host

                NetworkStream stream = client.GetStream(); // Obține fluxul de date
                reader = new StreamReader(stream);         // Creează cititorul de date

                writer = new StreamWriter(stream);         // Creează scriitorul de date
                writer.AutoFlush = true;                   // Trimite automat datele

                ruleaza = true;                            // Marchează conexiunea activă

                threadCitire = new Thread(CitesteMesaje);  // Creează thread-ul de citire
                threadCitire.IsBackground = true;          // Rulează în fundal
                threadCitire.Start();                      // Pornește thread-ul

                return true;                               // Conectare reușită
            }
            catch                                         // Dacă apare o eroare
            {
                ruleaza = false;                           // Oprește rularea
                return false;                              // Conectare eșuată
            }
        }

        /// Spune dacă acest client este conectat acum
        /*public bool EsteConectat()     // Verifică starea conexiunii
        {
            if (client == null) return false; // Dacă nu există client, nu e conectat
            return client.Connected;          // Returnează starea conexiunii
        }*/

        private void CitesteMesaje()   // Metodă rulată în thread
        {
            try                          // Încearcă citirea mesajelor
            {
                while (ruleaza)         // Rulează cât timp conexiunea e activă
                {
                    string mesaj = reader.ReadLine(); // Citește un mesaj de la host

                    if (mesaj == null)  // Dacă conexiunea s-a închis
                        break;          // Iese din buclă

                    MesajPrimit?.Invoke(mesaj); // Anunță aplicația că a venit un mesaj
                }
            }
            catch                       // Dacă apare o eroare
            {
                // conexiune pierdută
            }

            ruleaza = false;            // Oprește conexiunea
        }

        public void Trimite(string mesaj) // Trimite un mesaj către host
        {
            try                          // Încearcă trimiterea
            {
                if (writer != null)     // Dacă writer-ul există
                    writer.WriteLine(mesaj); // Trimite mesajul
            }
            catch                       // Dacă hostul e oprit
            {
                // host oprit
            }
        }

        public void Opreste()           // Oprește conexiunea
        {
            ruleaza = false;            // Oprește citirea mesajelor

            try { client?.Close(); } catch { } // Închide conexiunea TCP
        }
    }
}
