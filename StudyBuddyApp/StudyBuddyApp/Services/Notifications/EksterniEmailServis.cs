namespace StudyBuddyApp.Services.Notifications
{
    public class EksterniEmailServis
    {
        public Task SendEmailAsync(string primalac, string naslov, string sadrzaj)
        {
            // Simulacija eksternog email servisa - u stvarnoj aplikaciji ovdje bi se pozivao pravi email provider.

            Console.WriteLine($"Email poslan korisniku: {primalac}");
            Console.WriteLine($"Naslov: {naslov}");
            Console.WriteLine($"Sadržaj: {sadrzaj}");

            return Task.CompletedTask;
        }
    }
}