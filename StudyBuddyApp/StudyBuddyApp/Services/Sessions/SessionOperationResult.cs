namespace StudyBuddyApp.Services.Sessions
{
    public class SessionOperationResult
    {
        public bool Uspjesno { get; private set; }
        public string? Poruka { get; private set; }

        private SessionOperationResult(bool uspjesno, string? poruka)
        {
            Uspjesno = uspjesno;
            Poruka = poruka;
        }

        public static SessionOperationResult Uspjeh(string poruka)
        {
            return new SessionOperationResult(true, poruka);
        }

        public static SessionOperationResult Greska(string poruka)
        {
            return new SessionOperationResult(false, poruka);
        }
    }
}