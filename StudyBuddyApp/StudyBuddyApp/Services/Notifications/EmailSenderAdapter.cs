using StudyBuddyApp.Models;

namespace StudyBuddyApp.Services.Notifications
{
    public class EmailSenderAdapter : IObavjestenjeSender
    {
        private readonly EksterniEmailServis _emailServis;

        public EmailSenderAdapter(EksterniEmailServis emailServis)
        {
            _emailServis = emailServis;
        }

        public async Task PosaljiAsync(Obavjestenje obavjestenje)
        {
            if (obavjestenje.Korisnik == null || string.IsNullOrWhiteSpace(obavjestenje.Korisnik.Email))
            {
                return;
            }

            await _emailServis.SendEmailAsync(
                obavjestenje.Korisnik.Email,
                obavjestenje.Naslov,
                obavjestenje.Sadrzaj
            );
        }
    }
}