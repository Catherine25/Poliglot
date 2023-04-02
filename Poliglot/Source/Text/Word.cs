namespace Poliglot.Source.Text;

public class Word
{
    public string Original { get; set; }
    public string Context { get; set; }
    public States State { get; set; }
    public DateTime? RepeatTime { get; set; }
    public string Note { get; set; }

    public Word(string original, string context, States state = States.New)
    {
        Original = original;
        Context = context;
        State = state;
    }

    public bool ReadyToForRepeating() =>
        State != States.Known &&
        (RepeatTime == null ||
        DateTime.Now > RepeatTime);

    public void Answered(bool correct)
    {
        if (correct)
        {
            State += 1;

            if (State == States.Seen)
                RepeatTime = DateTime.Now.AddDays(1);
            else if (State == States.Studying)
                RepeatTime = DateTime.Now.AddDays(3);
            else if (State == States.Recognized)
                RepeatTime = DateTime.Now.AddDays(7);
        }
        else
        {
            State = States.New;
            RepeatTime = DateTime.UtcNow.AddMinutes(1);
        }
    }

    public static bool operator ==(Word x, Word y) => string.Equals(x?.Original, y?.Original);
    public static bool operator !=(Word x, Word y) => !(x == y);
    public override bool Equals(object obj) => Original == obj.ToString();
    public override int GetHashCode() => Original.GetHashCode();
    public override string ToString() => Original;
}
