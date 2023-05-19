using Poliglot.Source.Database;

namespace Poliglot.Source.Text;

public class WordInContext
{
    // db items
    public WordDbItem wordDbItem { get; }
    public SentenceDbItem sentenceDbItem { get; }

    // accessors
    public string Original { get => wordDbItem.Word; set => wordDbItem.Word = value; }

    public string Context { get => sentenceDbItem.Sentence; set => sentenceDbItem.Sentence = value; }

    public States State { get => wordDbItem.State; set => wordDbItem.State = value; }

    public DateTime? RepeatTime { get => wordDbItem.RepeatTime; set => wordDbItem.RepeatTime = value; }

    public string Note { get => wordDbItem.Note; set => wordDbItem.Note = value; }

    public string SentenceNote { get => sentenceDbItem.Note; set => sentenceDbItem.Note = value; }

    public bool WordBlocked { get => wordDbItem.Blocked; set => wordDbItem.Blocked = value; }

    public bool SentenceBlocked { get => sentenceDbItem.Blocked; set => sentenceDbItem.Blocked = value; }

    public WordInContext(WordDbItem wordDbItem, SentenceDbItem sentenceDbItem)
    {
        this.wordDbItem = wordDbItem;
        this.sentenceDbItem = sentenceDbItem;

        Original = wordDbItem.Word;
        Context = sentenceDbItem.Sentence;
        State = wordDbItem.State;
        RepeatTime = wordDbItem.RepeatTime;
        Note = wordDbItem.Note;
    }

    public void Answered(bool correct)
    {
        if (correct)
        {
            // if recognised from the first attempt - mark as known
            if (RepeatTime == null)
                State = States.Known;
            else
                State += 1;
        }
        else
            State = States.New;

        RepeatTime = DateTime.UtcNow;
    }

    public static bool operator ==(WordInContext x, WordInContext y) => string.Equals(x?.Original, y?.Original);
    public static bool operator !=(WordInContext x, WordInContext y) => !(x == y);
    public override bool Equals(object obj) => Original == obj.ToString();
    public override int GetHashCode() => Original.GetHashCode();
    public override string ToString() => Original;
}
