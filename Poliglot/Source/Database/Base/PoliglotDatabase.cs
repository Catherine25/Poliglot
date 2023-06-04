using Poliglot.Source.Text;
using SQLite;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Poliglot.Source.Database.Base;

public class PoliglotDatabase
{
    private SQLiteAsyncConnection Database;

    public PoliglotDatabase()
    {
    }

    async Task Init()
    {
        if (Database is not null)
            return;

        string databasePath = DatabaseConfiguration.DatabasePath;
        Debug.WriteLine("database path is " + databasePath);
        Database = new SQLiteAsyncConnection(databasePath, DatabaseConfiguration.Flags);

        _ = await Database.CreateTableAsync<SentenceDbItem>();
        _ = await Database.CreateTableAsync<WordDbItem>();
    }

    public async Task<bool> Any<T>() where T : DbItem, new()
    {
        await Init();
        return await Database.Table<T>().CountAsync() != 0;
    }

    public async Task<List<WordInContext>> GetWordsReadyForRepeating(string currentContext = null)
    {
        await Init();

        var words = await Database.Table<WordDbItem>().ToListAsync();
        Debug.WriteLine($"{words.Count} words loaded");
        Debug.WriteLine($"With state 'New': {words.Count(x => x.State == States.New)}");

        var wordsNotBlocked = words.Where(w => !w.Blocked);
        Debug.WriteLine($"{wordsNotBlocked.Count()} words are not blocked");

        var wordsToStudy = wordsNotBlocked.Where(w => w.ReadyToForRepeating()); // word can be studied
        Debug.WriteLine($"{wordsToStudy.Count()} words are ready for repeating");

        var mostKnownWords = wordsToStudy.OrderByDescending(w => w.State); // first repeat most known

        var sentences = await GetItemsAsync<SentenceDbItem>();
        Debug.WriteLine($"{sentences.Count} sentences loaded");

        return mostKnownWords
            .Select(wordItem => (wordItem, sentenceItem: sentences.SingleOrDefault(s => s.Id == wordItem.SentenceId)))
            .OrderBy(m => m.sentenceItem.Sentence != currentContext) // show words with different sentence from the previous one to not spoil the word to the user
            .Select(x => new WordInContext(x.wordItem, x.sentenceItem))
            .ToList();
    }

    public async Task<List<T>> GetItemsAsync<T>() where T : DbItem, new()
    {
        await Init();
        return await Database.Table<T>().ToListAsync();
    }

    public async Task<T> GetItemAsync<T>(Expression<Func<T, bool>> expr) where T : DbItem, new()
    {
        await Init();
        return await Database.Table<T>().Where(expr).FirstOrDefaultAsync();
    }

    public async Task<T> GetItemAsync<T>(int id) where T : DbItem, new()
    {
        await Init();
        return await Database.Table<T>().Where(i => i.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> SaveItemAsync<T>(T item) where T : DbItem, new()
    {
        await Init();
        if (item.Id != 0)
            return await Database.UpdateAsync(item);
        else
            return await Database.InsertAsync(item);
    }

    public async Task<int> DeleteItemAsync<T>(T item) where T : DbItem, new()
    {
        await Init();
        return await Database.DeleteAsync(item);
    }
}
