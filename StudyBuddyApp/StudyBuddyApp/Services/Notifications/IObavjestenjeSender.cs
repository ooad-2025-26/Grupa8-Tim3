using StudyBuddyApp.Models;

namespace StudyBuddyApp.Services.Notifications
{
    public interface IObavjestenjeSender
    {
        Task PosaljiAsync(Obavjestenje obavjestenje);
    }
}