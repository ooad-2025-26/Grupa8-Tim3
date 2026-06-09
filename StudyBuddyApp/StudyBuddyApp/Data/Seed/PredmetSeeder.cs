using Microsoft.EntityFrameworkCore;
using StudyBuddyApp.Models;

namespace StudyBuddyApp.Data.Seed
{
    public static class PredmetSeeder
    {
        public static async Task SeedAsync(ApplicationDbContext db)
        {
            var predmeti = new List<Predmet>();

            DodajZajednickePredmete(predmeti);

            DodajRiPredmete(predmeti);
            DodajTkPredmete(predmeti);
            DodajEePredmete(predmeti);
            DodajAiePredmete(predmeti);

            foreach (var predmet in predmeti)
            {
                bool postoji = await db.Predmeti.AnyAsync(p =>
                    p.Naziv == predmet.Naziv &&
                    p.SmjerStudija == predmet.SmjerStudija);

                if (!postoji)
                {
                    db.Predmeti.Add(predmet);
                }
            }

            await db.SaveChangesAsync();
        }

        private static Predmet P(string naziv, string oznaka, GodinaStudija godina, SmjerStudija smjer)
        {
            return new Predmet
            {
                Naziv = naziv,
                Oznaka = oznaka,
                Opis = string.Empty,
                GodinaStudija = godina,
                SmjerStudija = smjer,
                StatusPredmeta = StatusPredmeta.Aktivan
            };
        }

        private static void DodajZaSveSmjerove(List<Predmet> predmeti, string naziv, string oznaka, GodinaStudija godina)
        {
            predmeti.Add(P(naziv, oznaka, godina, SmjerStudija.RacunarstvoIInformatika));
            predmeti.Add(P(naziv, oznaka, godina, SmjerStudija.AutomatikaIElektronika));
            predmeti.Add(P(naziv, oznaka, godina, SmjerStudija.Telekomunikacije));
            predmeti.Add(P(naziv, oznaka, godina, SmjerStudija.Elektroenergetika));
        }

        private static void DodajZajednickePredmete(List<Predmet> predmeti)
        {
            DodajZaSveSmjerove(predmeti, "Inžinjerska matematika I", "IM1", GodinaStudija.Prva);
            DodajZaSveSmjerove(predmeti, "Osnove elektrotehnike", "OE", GodinaStudija.Prva);
            DodajZaSveSmjerove(predmeti, "Inženjerska fizika I", "IF1", GodinaStudija.Prva);
            DodajZaSveSmjerove(predmeti, "Linearna algebra i geometrija", "LAG", GodinaStudija.Prva);

            DodajZaSveSmjerove(predmeti, "Inžinjerska matematika II", "IM2", GodinaStudija.Prva);
            DodajZaSveSmjerove(predmeti, "Tehnike programiranja", "TP", GodinaStudija.Prva);
        }

        private static void DodajRiPredmete(List<Predmet> predmeti)
        {
            var s = SmjerStudija.RacunarstvoIInformatika;

            predmeti.Add(P("Uvod u programiranje", "UUP", GodinaStudija.Prva, s));
            predmeti.Add(P("Matematička logika i teorija izračunljivosti", "MLTI", GodinaStudija.Prva, s));
            predmeti.Add(P("Vjerovatnoća i statistika", "VS", GodinaStudija.Prva, s));
            predmeti.Add(P("Operativni sistemi", "OS", GodinaStudija.Prva, s));

            predmeti.Add(P("Algoritmi i strukture podataka", "ASP", GodinaStudija.Druga, s));
            predmeti.Add(P("Logički dizajn", "LD", GodinaStudija.Druga, s));
            predmeti.Add(P("Razvoj programskih rješenja", "RPR", GodinaStudija.Druga, s));
            predmeti.Add(P("Osnove baza podataka", "OBP", GodinaStudija.Druga, s));
            predmeti.Add(P("Diskretna matematika", "DM", GodinaStudija.Druga, s));
            predmeti.Add(P("Sistemsko programiranje", "SP", GodinaStudija.Druga, s));
            predmeti.Add(P("Numerički algoritmi", "NA", GodinaStudija.Druga, s));

            predmeti.Add(P("Računarske arhitekture", "RA", GodinaStudija.Druga, s));
            predmeti.Add(P("Osnove računarskih mreža", "ORM", GodinaStudija.Druga, s));
            predmeti.Add(P("Objektno orijentisana analiza i dizajn", "OOAD", GodinaStudija.Druga, s));
            predmeti.Add(P("Automati i formalni jezici", "AFJ", GodinaStudija.Druga, s));
            predmeti.Add(P("Razvoj mobilnih aplikacija", "RMA", GodinaStudija.Druga, s));
            predmeti.Add(P("CAD-CAM inžinjering", "CCI", GodinaStudija.Druga, s));
            predmeti.Add(P("Ugradbeni sistemi", "US", GodinaStudija.Druga, s));
            predmeti.Add(P("Digitalno procesiranje signala", "DPS", GodinaStudija.Druga, s));

            predmeti.Add(P("Web tehnologije", "WT", GodinaStudija.Treca, s));
            predmeti.Add(P("Računarska grafika", "RG", GodinaStudija.Treca, s));
            predmeti.Add(P("Osnove informacionih sistema", "OIS", GodinaStudija.Treca, s));
            predmeti.Add(P("Osnove operacionih istraživanja", "OOI", GodinaStudija.Treca, s));
            predmeti.Add(P("Verifikacija i validacija softvera", "VVS", GodinaStudija.Treca, s));
            predmeti.Add(P("Poslovni web sistemi", "PWS", GodinaStudija.Treca, s));
            predmeti.Add(P("Programski jezici i prevodioci", "PJP", GodinaStudija.Treca, s));
            predmeti.Add(P("Računarsko modeliranje i simulacije", "RMS", GodinaStudija.Treca, s));

            predmeti.Add(P("Softver inžinjering", "SI", GodinaStudija.Treca, s));
            predmeti.Add(P("Projektovanje informacionih sistema", "PIS", GodinaStudija.Treca, s));
            predmeti.Add(P("Vještačka inteligencija", "VI", GodinaStudija.Treca, s));
            predmeti.Add(P("Organizacija softverskog projekta", "OSP", GodinaStudija.Treca, s));
            predmeti.Add(P("Administracija računarskih mreža", "ARM", GodinaStudija.Treca, s));
            predmeti.Add(P("Dizajn i arhitektura softverskih sistema", "DASS", GodinaStudija.Treca, s));
            predmeti.Add(P("Projektovanje i sinteza digitalnih sistema", "PSDS", GodinaStudija.Treca, s));
        }

        private static void DodajTkPredmete(List<Predmet> predmeti)
        {
            var s = SmjerStudija.Telekomunikacije;

            predmeti.Add(P("Osnove računarstva", "OR", GodinaStudija.Prva, s));

            predmeti.Add(P("Električni krugovi I", "EK1", GodinaStudija.Prva, s));
            predmeti.Add(P("Inžinjerska fizika II", "IF2", GodinaStudija.Prva, s));
            predmeti.Add(P("Električni elementi i sklopovi", "EES", GodinaStudija.Prva, s));

            predmeti.Add(P("Teorija elektromagnetnih polja", "TEP", GodinaStudija.Druga, s));
            predmeti.Add(P("Električni krugovi II", "EK2", GodinaStudija.Druga, s));
            predmeti.Add(P("Elektronika telekomunikacija 1", "ETK1", GodinaStudija.Druga, s));
            predmeti.Add(P("Teorija informacija i izvorno kodiranje", "TIIK", GodinaStudija.Druga, s));
            predmeti.Add(P("Teorija signala", "TS", GodinaStudija.Druga, s));
            predmeti.Add(P("Operativni sistemi", "OS", GodinaStudija.Druga, s));
            predmeti.Add(P("Inženjerska ekonomika", "IEK", GodinaStudija.Druga, s));
            predmeti.Add(P("Osnove elektroenergetskih sistema", "OEES", GodinaStudija.Druga, s));

            predmeti.Add(P("Statistička teorija signala", "STS", GodinaStudija.Druga, s));
            predmeti.Add(P("Elektronika telekomunikacija II", "ETK2", GodinaStudija.Druga, s));
            predmeti.Add(P("Osnove optoelektronike", "OO", GodinaStudija.Druga, s));
            predmeti.Add(P("Antene i prostiranje talasa", "APT", GodinaStudija.Druga, s));
            predmeti.Add(P("Telekomunikacione tehnike 1", "TT1", GodinaStudija.Druga, s));
            predmeti.Add(P("Objektno orijentisana analiza i dizajn", "OOAD", GodinaStudija.Druga, s));
            predmeti.Add(P("Osnove baza podataka", "OBP", GodinaStudija.Druga, s));
            predmeti.Add(P("Osnove sistema automatskog upravljanja", "OSAU", GodinaStudija.Druga, s));

            predmeti.Add(P("Telekomunikacione tehnike II", "TT2", GodinaStudija.Treca, s));
            predmeti.Add(P("Radiotehnika", "R", GodinaStudija.Treca, s));
            predmeti.Add(P("Mobilne komunikacije", "MK", GodinaStudija.Treca, s));
            predmeti.Add(P("Kanalno kodiranje", "KK", GodinaStudija.Treca, s));
            predmeti.Add(P("Mjerenja u telekomunikacijama", "MT", GodinaStudija.Treca, s));
            predmeti.Add(P("Softverski inženjering", "SI", GodinaStudija.Treca, s));
            predmeti.Add(P("Nove generacije mreža i usluga", "NGMU", GodinaStudija.Treca, s));
            predmeti.Add(P("Teorija prometa", "TP", GodinaStudija.Treca, s));

            predmeti.Add(P("Mikrovalni komunikacijski sistemi", "MKS", GodinaStudija.Treca, s));
            predmeti.Add(P("Komutacioni sistemi", "KS", GodinaStudija.Treca, s));
            predmeti.Add(P("Komunikacijski protokoli i mreže", "KPM", GodinaStudija.Treca, s));
            predmeti.Add(P("Osnovi signalizacionih protokola", "OSP", GodinaStudija.Treca, s));
            predmeti.Add(P("Elektrotehnički materijali", "ETM", GodinaStudija.Treca, s));
            predmeti.Add(P("Organizacija i osnove upravljanja mrežom", "OOUM", GodinaStudija.Treca, s));
        }

        private static void DodajEePredmete(List<Predmet> predmeti)
        {
            var s = SmjerStudija.Elektroenergetika;

            predmeti.Add(P("Osnove računarstva", "OR", GodinaStudija.Prva, s));

            predmeti.Add(P("Električni krugovi I", "EK1", GodinaStudija.Prva, s));
            predmeti.Add(P("Inžinjerska fizika II", "IF2", GodinaStudija.Prva, s));
            predmeti.Add(P("Električni elementi i sklopovi", "EES", GodinaStudija.Prva, s));

            predmeti.Add(P("Inženjerska elektromagnetika", "IEM", GodinaStudija.Druga, s));
            predmeti.Add(P("Električni krugovi II", "EK2", GodinaStudija.Druga, s));
            predmeti.Add(P("Pouzdanost električnih elemenata i sistema", "PEES", GodinaStudija.Druga, s));
            predmeti.Add(P("Električna mjerenja", "EM", GodinaStudija.Druga, s));
            predmeti.Add(P("Osnove elektroenergetskih sistema", "OEES", GodinaStudija.Druga, s));

            predmeti.Add(P("Osnove sistema automatskog upravljanja", "OSAU", GodinaStudija.Druga, s));
            predmeti.Add(P("Osnove telekomunikacija", "OT", GodinaStudija.Druga, s));
            predmeti.Add(P("Elektrotehnički materijali", "ETM", GodinaStudija.Druga, s));
            predmeti.Add(P("Inženjerska ekonomika", "IEK", GodinaStudija.Druga, s));
            predmeti.Add(P("Osnove mehatronike", "OM", GodinaStudija.Druga, s));
            predmeti.Add(P("Objektno orijentirana analiza i dizajn", "OOAD", GodinaStudija.Druga, s));
            predmeti.Add(P("Osnove baza podataka", "OBP", GodinaStudija.Druga, s));
            predmeti.Add(P("Senzori i pretvarači", "SIP", GodinaStudija.Druga, s));

            predmeti.Add(P("Električne mašine", "EM", GodinaStudija.Treca, s));
            predmeti.Add(P("Energetska elektronika", "EE", GodinaStudija.Treca, s));
            predmeti.Add(P("Elektroenergetski sistemi", "EES", GodinaStudija.Treca, s));
            predmeti.Add(P("Tehnologija viskonaponske izolacije", "TVI", GodinaStudija.Treca, s));
            predmeti.Add(P("Praktikum iz elektroenergetike", "PE1", GodinaStudija.Treca, s));
            predmeti.Add(P("Tehnika visokog napona", "TVN", GodinaStudija.Treca, s));
            predmeti.Add(P("Komponente i tehnologije", "KT", GodinaStudija.Treca, s));
            predmeti.Add(P("Električne instalacije i mjere sigurnosti", "EIMS", GodinaStudija.Treca, s));
            predmeti.Add(P("Električni sistemi u transportu", "EST", GodinaStudija.Treca, s));

            predmeti.Add(P("Elektromotorni pogoni", "EMP", GodinaStudija.Treca, s));
            predmeti.Add(P("Električna postrojenja", "EP", GodinaStudija.Treca, s));
            predmeti.Add(P("Proizvodnja električne energije", "PEE", GodinaStudija.Treca, s));
            predmeti.Add(P("Kvalitet električne energije", "KEE", GodinaStudija.Treca, s));
            predmeti.Add(P("Održavanje električnih sistema", "OES", GodinaStudija.Treca, s));
            predmeti.Add(P("Elektrotermička konverzija energije", "ETKE", GodinaStudija.Treca, s));
            predmeti.Add(P("Upravljanje potrošnjom električne energije", "UPEE", GodinaStudija.Treca, s));
            predmeti.Add(P("Praktikum iz elektroenergetike II", "PE2", GodinaStudija.Treca, s));
        }

        private static void DodajAiePredmete(List<Predmet> predmeti)
        {
            var s = SmjerStudija.AutomatikaIElektronika;

            predmeti.Add(P("Uvod u programiranje", "UUP", GodinaStudija.Prva, s));

            predmeti.Add(P("Električni krugovi I", "EK1", GodinaStudija.Prva, s));
            predmeti.Add(P("Inžinjerska fizika II", "IF2", GodinaStudija.Prva, s));
            predmeti.Add(P("Električni elementi i sklopovi", "EES", GodinaStudija.Prva, s));

            predmeti.Add(P("Inžinjerska matematika III", "IM3", GodinaStudija.Druga, s));
            predmeti.Add(P("Električni krugovi II", "EK2", GodinaStudija.Druga, s));
            predmeti.Add(P("Električna mjerenja", "EM", GodinaStudija.Druga, s));
            predmeti.Add(P("Analogna elektronika", "AE", GodinaStudija.Druga, s));
            predmeti.Add(P("Senzori i pretvarači", "SP", GodinaStudija.Druga, s));
            predmeti.Add(P("Dinamički sistemi", "DS", GodinaStudija.Druga, s));
            predmeti.Add(P("Diskretna matematika", "DM", GodinaStudija.Druga, s));

            predmeti.Add(P("Digitalna elektronika", "DE", GodinaStudija.Druga, s));
            predmeti.Add(P("Modeliranje i simulacija", "MS", GodinaStudija.Druga, s));
            predmeti.Add(P("Linearni sistemi automatskog upravljanja", "LSAU", GodinaStudija.Druga, s));
            predmeti.Add(P("Praktikum elektrotehnike i elektronike", "PEE", GodinaStudija.Druga, s));
            predmeti.Add(P("Praktikum iz automatike i informatike", "PAI", GodinaStudija.Druga, s));
            predmeti.Add(P("Električne mašine", "EM", GodinaStudija.Druga, s));
            predmeti.Add(P("Aktuatori", "A", GodinaStudija.Druga, s));
            predmeti.Add(P("Osnove baza podataka", "OBP", GodinaStudija.Druga, s));
            predmeti.Add(P("Osnove telekomunikacija", "OT", GodinaStudija.Druga, s));
            predmeti.Add(P("Osnove optoelektronike", "OO", GodinaStudija.Druga, s));

            predmeti.Add(P("Digitalni integrirani krugovi", "DIK", GodinaStudija.Treca, s));
            predmeti.Add(P("Digitalni sistemi upravljanja", "DSU", GodinaStudija.Treca, s));
            predmeti.Add(P("Analiza signala i sistema", "ASS", GodinaStudija.Treca, s));
            predmeti.Add(P("Projektovanje logičkih sistema", "PLS", GodinaStudija.Treca, s));
            predmeti.Add(P("Praktikum automatike", "PA", GodinaStudija.Treca, s));
            predmeti.Add(P("Praktikum elektronike", "PE", GodinaStudija.Treca, s));
            predmeti.Add(P("Operativni sistemi", "OS", GodinaStudija.Treca, s));
            predmeti.Add(P("Razvoj programskih rješenja", "RPR", GodinaStudija.Treca, s));

            predmeti.Add(P("Strukture i režimi rada elektroenergetskih sistema", "SRRES", GodinaStudija.Treca, s));
            predmeti.Add(P("Mehatronika", "M", GodinaStudija.Treca, s));
            predmeti.Add(P("Energetska elektronika", "EE", GodinaStudija.Treca, s));
            predmeti.Add(P("Dinamika fluida i toplotnih sistema", "DFTS", GodinaStudija.Treca, s));
            predmeti.Add(P("Projektovanje mikroprocesorskih sistema", "PMS", GodinaStudija.Treca, s));
            predmeti.Add(P("Robotika I", "R1", GodinaStudija.Treca, s));
        }
    }
}