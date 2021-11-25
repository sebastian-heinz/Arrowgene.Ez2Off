namespace Arrowgene.Ez2Off.Common.Models
{
    public class Audit
    {
        public bool Incident { get; set; }
        public Incident ScoreIncident { get; set; }
        public Incident ComboIncident { get; set; }
        public Incident AllComboIncident { get; set; }

        public Audit()
        {
        }

        public void Reset()
        {
            Incident = false;
            ScoreIncident = null;
            ComboIncident = null;
            AllComboIncident = null;
        }
    }
}