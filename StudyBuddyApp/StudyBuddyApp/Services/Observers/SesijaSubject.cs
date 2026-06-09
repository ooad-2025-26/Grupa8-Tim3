namespace StudyBuddyApp.Services.Observers
{
    public class SesijaSubject
    {
        private readonly IEnumerable<ISesijaObserver> _observeri;

        public SesijaSubject(IEnumerable<ISesijaObserver> observeri)
        {
            _observeri = observeri;
        }

        public async Task ObavijestiObservereAsync(SesijaPromjenaInfo promjena)
        {
            if (!promjena.ImaPromjena)
            {
                return;
            }

            foreach (var observer in _observeri)
            {
                await observer.AzurirajAsync(promjena);
            }
        }
    }
}