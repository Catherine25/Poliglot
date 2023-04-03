namespace Poliglot.Source.Statistics;

public class StatisticsBank
{
    public IEnumerable<DayEntry> Entries { get; set; }

    public DayEntry TodayEntry()
    {
        var entry = Entries.FirstOrDefault(e => e.Date.Date == Today);

        if (entry == null)
        {
            entry = new DayEntry() { Date = Today, WordCount = 0 };

            var entries = Entries.ToList();
            entries.Add(entry);
            Entries = entries;
        }

        return entry;
    }

    public int WordCompleted()
    {
        Entries.FirstOrDefault(e => e.Date.Date == Today).WordCount++;

        return Entries.FirstOrDefault(e => e.Date == Today).WordCount;
    }

    private static DateTime Today => DateTime.Now.Date;
}
