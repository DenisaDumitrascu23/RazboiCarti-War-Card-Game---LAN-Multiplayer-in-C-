using System;                          // Tipuri de bază (Action, EventArgs etc.)
using System.Drawing;                  // Image, Graphics etc.
using System.IO;                       // Path, File
using System.Windows.Forms;            // WinForms (Form, Button, MessageBox)

namespace RazboiCarti                  // Namespace-ul proiectului
{
    public partial class Form1 : Form   // Fereastra principală WinForms
    {
        private const int PORT = 5000;  // Portul folosit de Host/Client pentru conexiune

        // ROLE / STARE
        private bool esteJucatorul1;    // true=Host(J1), false=Client(J2)
        private bool jocTerminat;       // devine true când jocul s-a terminat

        // Restart: jocul repornește doar dacă apasă amândoi
        private bool jucatorul1VreaRestart; // J1 a cerut restart
        private bool jucatorul2VreaRestart; // J2 a cerut restart

        // RETEA
        private ConexiuneServer server; // serverul (există doar la Host)
        private ConexiuneClient client; // clientul (există doar la Client)

        // JOC
        private ClasaMeci meci;         // logica jocului (există doar la Host)
        private ControlLanJoc controlLan; // sincronizare apăsări (există doar la Host)

        // IMAGINI
        private string folderImagini;   // calea către folderul Images
        private string caleSpateCarte;  // calea către imaginea de spate a cărții

        public Form1(bool esteHost, string ipHost) // constructor: primește rol + IP (dacă e client)
        {
            InitializeComponent();      // creează controalele din designer

            esteJucatorul1 = esteHost;  // stabilește dacă această fereastră e Host sau Client

            jocTerminat = false;        // la început jocul nu e terminat
            jucatorul1VreaRestart = false; // nimeni nu cere restart la început
            jucatorul2VreaRestart = false; // nimeni nu cere restart la început

            folderImagini = Path.Combine(Application.StartupPath, "Images"); // folderul cu imagini
            caleSpateCarte = Path.Combine(folderImagini, "Spate_Carte.png"); // imaginea spatelui

            this.Load += Form1_Load;    // când se încarcă fereastra, rulează Form1_Load

            if (esteJucatorul1)         // dacă e Host
                PornesteCaJucatorul1(); // pornește server + inițializează jocul
            else                        // altfel e Client
                PornesteCaJucatorul2(ipHost); // se conectează la server
        }

        // EVENIMENT: Load (interfață inițială)
        private void Form1_Load(object sender, EventArgs e) // se execută când fereastra apare
        {
            PuneSpateleCartilor();      // pune imaginea de spate la ambele cărți

            if (esteJucatorul1)         // dacă suntem Host
                ActualizeazaScorLaJucatorul1(); // scor real din obiectul meci
            else                        // dacă suntem Client
                PuneScorInitialLaJucatorul2();  // scor inițial (fără meci local)
        }

        // PORNIRE J1 (HOST)
        private void PornesteCaJucatorul1() // inițializare Host
        {
            meci = new ClasaMeci();     // creează jocul (pachet, jucători etc.)
            controlLan = new ControlLanJoc(); // creează starea de sincronizare

            server = new ConexiuneServer(PORT); // creează serverul pe portul ales

            server.MesajPrimit += Jucatorul1_A_PrimitMesaj; // când vine mesaj de la client, îl tratăm aici

            server.Porneste();          // pornește ascultarea (accept client pe thread separat)

            AfiseazaMesaj("Jucătorul 1 este gata. Se așteaptă conectarea Jucătorului 2."); // mesaj UI

            butonRundaJ1.Enabled = false; // nu poate începe până nu se conectează J2
            butonRundaJ2.Enabled = false; // J2 nu folosește butonul lui J1
            butonRestart.Enabled = false; // restart apare doar la final
        }

        // PORNIRE J2 (CLIENT)
        private void PornesteCaJucatorul2(string ipHost)
        {
            // Creăm obiectul de tip client, cu IP-ul host-ului și portul comun
            client = new ConexiuneClient(ipHost, PORT);

            // Ne abonăm la evenimentul MesajPrimit
            // De fiecare dată când host-ul trimite ceva, metoda Jucatorul2_A_PrimitMesaj va fi apelată
            client.MesajPrimit += Jucatorul2_A_PrimitMesaj;

            // Încercăm conectarea la Host
            // Această metodă returnează true DOAR dacă serverul există și conexiunea reușește
            bool conectat = client.Conecteaza();

            // Dacă NU s-a reușit conectarea
            if (!conectat)
            {
                // Afișăm un mesaj clar pentru utilizator
                MessageBox.Show(
                    "Nu mă pot conecta la Host.\n\n" +
                    "Pornește mai întâi Jucătorul 1 (Host), apoi pornește Jucătorul 2.",
                    "Conectare eșuată",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );

                // Închidem aplicația Client
                // Astfel evităm să rămână într-o stare „fals conectată”
                Close();
                return; // ieșim din metodă
            }

            // DOAR dacă conectarea a reușit:
            // trimitem un mesaj Host-ului ca să știe că J2 este conectat
            client.Trimite("CONECTAT");

            // Afișăm mesaj în interfața Clientului
            AfiseazaMesaj("Jucătorul 2 este conectat. Jocul poate începe.");

            // Clientul NU folosește butonul J1
            butonRundaJ1.Enabled = false;

            // Restart-ul va fi disponibil doar la finalul jocului
            butonRestart.Enabled = false;

            // Permitem Clientului să apese butonul lui de rundă
            butonRundaJ2.Enabled = true;
        }

        // BUTON: INCEPE RUNDA J1 (doar în fereastra Host)
        private void butonRundaJ1_Click(object sender, EventArgs e) // click pe butonul J1
        {
            if (!esteJucatorul1) return; // dacă nu suntem host, ignorăm
            if (jocTerminat) return;     // dacă jocul s-a terminat, ignorăm

            if (!meci.AreCarti())        // dacă nu mai sunt cărți
            {
                FinalizeazaJocLaJucatorul1(); // termină jocul
                return;                   // ieșim
            }

            controlLan.ApasatJ1 = true;  // marcăm că J1 a apăsat (este gata)

            AfiseazaMesaj("Jucătorul 1 este pregătit. Se așteaptă acțiunea Jucătorului 2."); // UI

            server.Trimite("MESAJ|Jucătorul 1 este pregătit. Se așteaptă acțiunea Jucătorului 2."); // mesaj la J2

            if (controlLan.SuntAmandoiGata()) // dacă și J2 a apăsat deja
                JoacaRundaLaJucatorul1();     // host joacă runda
        }

        // BUTON: INCEPE RUNDA J2 (doar în fereastra Client)
        private void butonRundaJ2_Click(object sender, EventArgs e) // click pe butonul J2
        {
            if (esteJucatorul1) return;  // dacă suntem host, ignorăm
            if (jocTerminat) return;     // dacă jocul e terminat, ignorăm

            client.Trimite("GATA");      // anunță host-ul că J2 e pregătit

            AfiseazaMesaj("Jucătorul 2 este pregătit. Se așteaptă acțiunea Jucătorului 1."); // UI

            butonRundaJ2.Enabled = false; // blocăm până primim STARE de la host (runda s-a jucat)
        }

        // BUTON: RESTART (apare doar la final; repornește doar dacă apasă amândoi)
        private void butonRestart_Click(object sender, EventArgs e) // click pe restart
        {
            if (!jocTerminat) return;    // restart permis doar după final

            if (esteJucatorul1)          // dacă suntem Host (J1)
            {
                jucatorul1VreaRestart = true; // J1 confirmă restart
                butonRestart.Enabled = false; // dezactivăm butonul după apăsare

                AfiseazaMesaj("Jucătorul 1 a solicitat reînceperea jocului. Se așteaptă confirmarea Jucătorului 2."); // UI

                server.Trimite("MESAJ|Jucătorul 1 dorește reînceperea jocului. Apasă „Restart joc” pentru a confirma."); // cere confirmare la J2

                if (jucatorul2VreaRestart) // dacă J2 confirmase deja
                    RepornesteJoculLaJucatorul1(); // repornește jocul

                return;                   // ieșim (am tratat cazul host)
            }

            jucatorul2VreaRestart = true; // dacă suntem Client, J2 confirmă restart
            butonRestart.Enabled = false; // dezactivăm după apăsare

            AfiseazaMesaj("Jucătorul 2 a solicitat reînceperea jocului. Se așteaptă confirmarea Jucătorului 1."); // UI

            client.Trimite("RESTART");    // trimite la host cererea de restart
        }

        // J1 PRIMEȘTE MESAJE DE LA J2 (Host side)
        private void Jucatorul1_A_PrimitMesaj(string mesaj) // se apelează când server primește un mesaj
        {
            if (!IsHandleCreated) return; // dacă fereastra nu e pregătită, ieșim

            BeginInvoke((Action)(() =>    // mutăm execuția pe thread-ul UI (WinForms)
            {
                if (mesaj == "CONECTAT")  // mesaj: J2 s-a conectat
                {
                    AfiseazaMesaj("Jucătorul 2 s-a conectat. Jocul poate începe."); // UI
                    butonRundaJ1.Enabled = true; // acum J1 poate apăsa runda
                    return;               // ieșim
                }

                if (jocTerminat)          // dacă jocul e terminat, tratăm doar restart
                {
                    if (mesaj == "RESTART") // J2 cere restart
                    {
                        jucatorul2VreaRestart = true; // memorăm cererea

                        AfiseazaMesaj("Jucătorul 2 dorește reînceperea jocului. Apasă „Restart joc” pentru a confirma."); // UI

                        if (!jucatorul1VreaRestart) // dacă J1 nu a apăsat încă
                            butonRestart.Enabled = true; // îi lăsăm butonul activ

                        if (jucatorul1VreaRestart)  // dacă și J1 a confirmat
                            RepornesteJoculLaJucatorul1(); // repornim jocul
                    }

                    return;               // ieșim (nu mai tratăm runde)
                }

                if (mesaj == "GATA")      // mesaj: J2 e pregătit pentru rundă
                {
                    controlLan.ApasatJ2 = true; // marcăm apăsarea lui J2

                    AfiseazaMesaj("Jucătorul 2 este pregătit. Se așteaptă acțiunea Jucătorului 1."); // UI

                    if (controlLan.SuntAmandoiGata()) // dacă și J1 apăsase
                        JoacaRundaLaJucatorul1();     // host joacă runda
                }
            }));
        }

        // RUNDA SE JOACA DOAR LA J1 (Host side)
        private void JoacaRundaLaJucatorul1() // logica de rundă (doar host)
        {
            if (!meci.AreCarti())          // dacă nu mai sunt cărți
            {
                FinalizeazaJocLaJucatorul1(); // finalizează jocul
                return;                     // ieșim
            }

            meci.JoacaRunda();              // host extrage și compară cărțile + actualizează scoruri

            imagineJucator1.Image = Image.FromFile(meci.UltimaCarteJ1.GetNumeFisier()); // arată cartea J1
            imagineJucator2.Image = Image.FromFile(meci.UltimaCarteJ2.GetNumeFisier()); // arată cartea J2

            int rezultat = ((Carte)meci.UltimaCarteJ1).Compara(meci.UltimaCarteJ2); // compară cele 2 cărți

            string mesajRunda;              // textul care se afișează după rundă
            if (rezultat > 0) mesajRunda = "Runda a fost câștigată de Jucătorul 1.";      // J1 câștigă
            else if (rezultat < 0) mesajRunda = "Runda a fost câștigată de Jucătorul 2."; // J2 câștigă
            else mesajRunda = "Runda s-a încheiat la egalitate.";                         // egal

            AfiseazaMesaj(mesajRunda);      // afișează mesajul rundei

            ActualizeazaScorLaJucatorul1(); // actualizează scorurile în UI (Host)

            string mesajStare =             // construim pachetul STARE pentru client
                "STARE|" +                  // prefix ca să știe clientul că urmează date
                Path.GetFileName(meci.UltimaCarteJ1.GetNumeFisier()) + "|" + // nume fișier carte J1
                Path.GetFileName(meci.UltimaCarteJ2.GetNumeFisier()) + "|" + // nume fișier carte J2
                meci.Jucator1.GetRundeCastigate() + "|" +                    // runde câștigate J1
                meci.Jucator2.GetRundeCastigate() + "|" +                    // runde câștigate J2
                meci.Jucator1.GetCarti().Count + "|" +                       // cărți rămase J1
                meci.Jucator2.GetCarti().Count + "|" +                       // cărți rămase J2
                mesajRunda;                                                  // mesajul rundei

            server.Trimite(mesajStare);     // trimitem starea către client (J2)

            controlLan.Reseteaza();         // resetăm apăsările pentru runda următoare

            if (!meci.AreCarti())           // după rundă poate s-au terminat cărțile
                FinalizeazaJocLaJucatorul1(); // finalizăm jocul dacă e cazul
        }

        // SCOR LA J1 (Host side)
        private void ActualizeazaScorLaJucatorul1() // update UI cu date reale din meci
        {
            cartiRamaseJ1.Text = "Cărțile rămase Jucătorului 1: " + meci.Jucator1.GetCarti().Count; // cărți rămase J1
            cartiRamaseJ2.Text = "Cărțile rămase Jucătorului 2: " + meci.Jucator2.GetCarti().Count; // cărți rămase J2
            rundeJ1.Text = "Runde câștigate de Jucătorul 1: " + meci.Jucator1.GetRundeCastigate();  // runde J1
            rundeJ2.Text = "Runde câștigate de Jucătorul 2: " + meci.Jucator2.GetRundeCastigate();  // runde J2
        }

        // J2 PRIMEȘTE MESAJE DE LA J1 (Client side)
        private void Jucatorul2_A_PrimitMesaj(string mesaj) // se apelează când client primește un mesaj
        {
            if (!IsHandleCreated) return;  // dacă fereastra nu e pregătită, ieșim

            BeginInvoke((Action)(() =>     // mutăm execuția pe thread-ul UI
            {
                if (mesaj.StartsWith("MESAJ|")) // mesaj de tip text simplu pentru label
                {
                    AfiseazaMesaj(mesaj.Substring(6)); // scoatem "MESAJ|" și afișăm restul
                    return;                // ieșim
                }

                if (mesaj.StartsWith("STARE|")) // mesaj de tip STARE (date complete rundă)
                {
                    string[] p = mesaj.Split('|'); // separăm bucățile după |

                    imagineJucator1.Image = Image.FromFile(Path.Combine(folderImagini, p[1])); // carte J1
                    imagineJucator2.Image = Image.FromFile(Path.Combine(folderImagini, p[2])); // carte J2

                    rundeJ1.Text = "Runde câștigate de Jucătorul 1: " + p[3]; // scor runde J1
                    rundeJ2.Text = "Runde câștigate de Jucătorul 2: " + p[4]; // scor runde J2

                    cartiRamaseJ1.Text = "Cărțile rămase Jucătorului 1: " + p[5]; // cărți rămase J1
                    cartiRamaseJ2.Text = "Cărțile rămase Jucătorului 2: " + p[6]; // cărți rămase J2

                    AfiseazaMesaj(p[7]);  // afișează mesajul rundei primit de la host

                    butonRundaJ2.Enabled = true; // acum J2 poate apăsa pentru runda următoare
                    return;                // ieșim
                }

                if (mesaj.StartsWith("FINAL|")) // mesaj: joc terminat
                {
                    jocTerminat = true;    // marcăm jocul ca terminat

                    AfiseazaMesaj(mesaj.Substring(6)); // afișăm mesajul final

                    butonRundaJ1.Enabled = false; // nu mai sunt runde
                    butonRundaJ2.Enabled = false; // nu mai sunt runde

                    butonRestart.Enabled = true;  // la client restart devine posibil

                    jucatorul1VreaRestart = false; // resetăm cererile de restart
                    jucatorul2VreaRestart = false; // resetăm cererile de restart
                    return;                // ieșim
                }

                if (mesaj == "RESET")      // mesaj: host a repornit jocul
                {
                    jocTerminat = false;   // jocul nu mai e terminat

                    jucatorul1VreaRestart = false; // reset cereri restart
                    jucatorul2VreaRestart = false; // reset cereri restart

                    PuneSpateleCartilor();          // punem din nou spatele cărților
                    PuneScorInitialLaJucatorul2();   // reset scor UI

                    AfiseazaMesaj("Jocul poate începe."); // mesaj UI

                    butonRundaJ2.Enabled = true;    // J2 poate începe din nou

                    butonRestart.Enabled = false;   // restart dispare până la final
                    return;                // ieșim
                }
            }));
        }

        // SCOR INITIAL (J2)
        private void PuneScorInitialLaJucatorul2() // valori inițiale afișate la client
        {
            cartiRamaseJ1.Text = "Cărțile rămase Jucătorului 1: 26"; // presupunem 26-26 la start
            cartiRamaseJ2.Text = "Cărțile rămase Jucătorului 2: 26"; // presupunem 26-26 la start
            rundeJ1.Text = "Runde câștigate de Jucătorul 1: 0";      // 0 runde la start
            rundeJ2.Text = "Runde câștigate de Jucătorul 2: 0";      // 0 runde la start
        }

        // FINAL JOC (J1)
        private void FinalizeazaJocLaJucatorul1() // doar host decide finalul
        {
            jocTerminat = true;            // marcăm finalul jocului

            jucatorul1VreaRestart = false; // reset cereri restart
            jucatorul2VreaRestart = false; // reset cereri restart

            string mesajFinal;             // mesaj final (câștigător/egal)
            if (meci.Jucator1.GetRundeCastigate() > meci.Jucator2.GetRundeCastigate()) // J1 are mai multe runde
                mesajFinal = "Jocul s-a încheiat. Câștigător: Jucătorul 1.";           // J1 câștigă
            else if (meci.Jucator2.GetRundeCastigate() > meci.Jucator1.GetRundeCastigate()) // J2 are mai multe runde
                mesajFinal = "Jocul s-a încheiat. Câștigător: Jucătorul 2.";           // J2 câștigă
            else
                mesajFinal = "Jocul s-a încheiat la egalitate.";                       // egal

            AfiseazaMesaj(mesajFinal);     // afișăm mesajul final la host

            butonRundaJ1.Enabled = false;  // dezactivăm runde
            butonRundaJ2.Enabled = false;  // dezactivăm runde

            butonRestart.Enabled = true;   // host poate cere restart

            server.Trimite("FINAL|" + mesajFinal); // trimitem finalul către client
        }

        // RESTART REAL (doar la J1, după ce apasă amândoi)
        private void RepornesteJoculLaJucatorul1() // doar host resetează jocul
        {
            jucatorul1VreaRestart = false; // reset cereri restart
            jucatorul2VreaRestart = false; // reset cereri restart

            meci = new ClasaMeci();        // recreăm meciul (pachet nou / distribuire nouă)
            controlLan = new ControlLanJoc(); // reset starea apăsărilor
            jocTerminat = false;           // jocul nu mai e terminat

            PuneSpateleCartilor();         // reset imagini
            ActualizeazaScorLaJucatorul1();// reset scor real la host

            AfiseazaMesaj("Jocul poate începe."); // mesaj UI

            butonRundaJ1.Enabled = true;   // host poate începe
            butonRundaJ2.Enabled = false;  // host nu folosește butonul J2

            butonRestart.Enabled = false;  // restart dispare până la final

            server.Trimite("RESET");       // anunțăm clientul să reseteze UI-ul
        }

        // IMAGINI
        private void PuneSpateleCartilor() // setează imaginile cu spatele cărții
        {
            imagineJucator1.Image = Image.FromFile(caleSpateCarte); // spate la J1
            imagineJucator2.Image = Image.FromFile(caleSpateCarte); // spate la J2
        }

        // METODA MICA PENTRU MESAJ (UI)
        private void AfiseazaMesaj(string mesaj) // afișează textul în label
        {
            textCastigator.Text = mesaj;   // pune mesajul pe ecran
        }

        // ÎNCHIDERE
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                if (server != null) server.Opreste(); // oprește serverul dacă există
                if (client != null) client.Opreste(); // oprește clientul dacă există
            }
            catch { }

            base.OnFormClosing(e);
        }
    }
}
