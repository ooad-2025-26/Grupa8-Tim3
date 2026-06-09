using StudyBuddyApp.Models;

namespace StudyBuddyApp.Services.Sessions
{
    public class SesijaUcenjaBuilder
    {
        private readonly SesijaUcenja _sesija = new SesijaUcenja();

        public SesijaUcenjaBuilder PostaviNaziv(string naziv)
        {
            _sesija.Naziv = naziv;
            return this;
        }

        public SesijaUcenjaBuilder PostaviOpis(string opis)
        {
            _sesija.Opis = opis;
            return this;
        }

        public SesijaUcenjaBuilder PostaviDatumVrijeme(DateTime datumVrijeme)
        {
            _sesija.DatumVrijeme = DateTime.SpecifyKind(datumVrijeme, DateTimeKind.Utc);
            return this;
        }

        public SesijaUcenjaBuilder PostaviTrajanje(int trajanje)
        {
            _sesija.Trajanje = trajanje;
            return this;
        }

        public SesijaUcenjaBuilder PostaviLokaciju(int lokacijaId)
        {
            _sesija.LokacijaId = lokacijaId;
            return this;
        }

        public SesijaUcenjaBuilder PostaviPredmet(int predmetId)
        {
            _sesija.PredmetId = predmetId;
            return this;
        }

        public SesijaUcenjaBuilder PostaviKreatora(string kreatorId)
        {
            _sesija.KreatorId = kreatorId;
            return this;
        }

        public SesijaUcenjaBuilder PostaviMaksimalanBrojUcesnika(int broj)
        {
            _sesija.MaksimalanBrojUcesnika = broj;
            _sesija.BrojSlobodnihMjesta = broj;
            return this;
        }

        public SesijaUcenjaBuilder PostaviStatus(StatusSesije statusSesije)
        {
            _sesija.StatusSesije = statusSesije;
            return this;
        }

        public SesijaUcenja Kreiraj()
        {
            return _sesija;
        }
    }
}