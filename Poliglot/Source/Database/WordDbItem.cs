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

    public bool ReadyToForRepeating() =>
       State != States.Known &&
       (RepeatTime == null ||
       DateTime.Now > RepeatTime);
}