using Poliglot.Source.Database;
using Poliglot.Source.Storage;

namespace Poliglot.Source.Text;

public class WordImporter
{
    private const string TextFileName = "Text.txt";

    private readonly Loader loader;
    private readonly TextProcessor textProcessor;

    public WordImporter (
        Loader loader,
        TextProcessor textProcessor)
    {
        this.loader = loader;
        this.textProcessor = textProcessor;
    }

    public async Task<IEnumerable<(string word, string sentence)>> ImportInto(IEnumerable<WordDbItem> wordDbItems, IEnumerable<SentenceDbItem> sentenceDbItems)
    {
        // load unknown words
        string text = await loader.Load(TextFileName);

        // split to sentences
        var sentences = textProcessor.ExtractSentences(text);

        // get words in context
        IEnumerable<(string word, string sentence)> newWordsInContext = sentences
            .SelectMany(sentence => textProcessor.ExtractWords(sentence)
            .Select(word => (word, sentence)))
            .DistinctBy(x => x.word);

        // remove known words
        IEnumerable<(string word, string sentence)> notSavedWords = newWordsInContext
            .Where(w => wordDbItems
            .All(w2 => w.word != w2.Word));

        // remove known sentences
        IEnumerable<(string word, string sentence)> allowedWords = notSavedWords
            .Where(w => !sentenceDbItems.Any(s => s.Blocked || w.sentence == s.Sentence));

        return notSavedWords;
    }
}
