using Poliglot.Source.Text;

namespace Poliglot.Source.Database;

public class WordDbItem : DbItem
{
    public string Word { get; set; }
    public int SentenceId { get; set; }
    public States State { get; set; }
    public DateTime? RepeatTime { get; set; }
    public string Note { get; set; }
    public bool Blocked { get; set; }

    public bool ReadyToForRepeating()
    {
        // if already learned
        if (State == States.Known)
            return false;

        // if new
        if (State == States.New)
            return true;

        var mapping = new Dictionary<States, DateTime>()
        {
            { States.Seen, RepeatTime.Value.AddMinutes(1) },
            { States.Studying, RepeatTime.Value.AddDays(1) },
            { States.Recognized, RepeatTime.Value.AddDays(7) },
        };

        return DateTime.Now < mapping[State];
    }
}