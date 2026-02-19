namespace RazboiCarti                 // Namespace-ul aplicației
{
    /// ControlLanJoc = „starea” jocului pentru sincronizare
    /// Clasa NU face logică de joc, doar ține minte stări
    /// - dacă J1 a apăsat
    /// - dacă J2 a apăsat
    /// - dacă J1/J2 vor restart
    internal class ControlLanJoc        // Clasă de control pentru sincronizarea LAN
    {
        // true dacă J1 a apăsat butonul pentru rundă
        private bool apasatJ1;         // stare internă pentru Jucătorul 1

        // true dacă J2 a apăsat butonul pentru rundă
        private bool apasatJ2;         // stare internă pentru Jucătorul 2

        // true dacă J1 dorește restart
        //private bool restartJ1;        // stare internă pentru restart J1

        // true dacă J2 dorește restart
        //private bool restartJ2;        // stare internă pentru restart J2

        // RUNDĂ 
        /// Setat de Form1 când J1 apasă „Începe runda J1”
        public bool ApasatJ1           // Proprietate publică pentru starea J1
        {
            get { return apasatJ1; }   // Returnează dacă J1 a apăsat
            set { apasatJ1 = value; }  // Setează apăsarea lui J1
        }

        /// Setat de Form1 când J2 apasă „Începe runda J2”
        public bool ApasatJ2           // Proprietate publică pentru starea J2
        {
            get { return apasatJ2; }   // Returnează dacă J2 a apăsat
            set { apasatJ2 = value; }  // Setează apăsarea lui J2
        }

        /// Verifică dacă ambii jucători sunt gata să joace runda
        public bool SuntAmandoiGata()  // Metodă de verificare sincronizare
        {
            return apasatJ1 && apasatJ2; // true doar dacă ambii au apăsat
        }

        /// După terminarea rundei, resetăm stările
        public void Reseteaza()        // Pregătește următoarea rundă
        {
            apasatJ1 = false;          // J1 nu mai este „gata”
            apasatJ2 = false;          // J2 nu mai este „gata”
        }

        //RESTART
        /*public bool RestartJ1          // Proprietate pentru cererea de restart J1
        {
            get { return restartJ1; }  // Returnează dacă J1 vrea restart
            set { restartJ1 = value; } // Setează cererea lui J1
        }

        public bool RestartJ2          // Proprietate pentru cererea de restart J2
        {
            get { return restartJ2; }  // Returnează dacă J2 vrea restart
            set { restartJ2 = value; } // Setează cererea lui J2
        } 

        /// Jocul repornește DOAR dacă ambii au cerut restart
        public bool SuntAmandoiPentruRestart() // Verifică acordul ambilor jucători
        {
            return restartJ1 && restartJ2;     // true doar dacă ambii vor restart
        }

        /// După restart, resetăm cererile
        public void ReseteazaRestart()  // Curăță stările de restart
        {
            restartJ1 = false;          // J1 nu mai cere restart
            restartJ2 = false;          // J2 nu mai cere restart
        } */
    }
}
