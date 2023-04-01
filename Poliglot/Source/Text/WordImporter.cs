﻿using Poliglot.Source.Storage;

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

    public async Task<WordBank> ImportInto(WordBank wordBank)
    {
        // load unknown words
        string text = await loader.Load(TextFileName);

        // split to sentences
        var sentences = textProcessor.ExtractSentences(text);

        // get words in context
        IEnumerable<Word> newWordsInContext = sentences
            .SelectMany(s => textProcessor.ExtractWords(s)
            .Select(w => new Word(w, s)));

        // remove known words
        IEnumerable<Word> notSavedWords = newWordsInContext
            .Where(w => wordBank.Words
            .All(w2 => w.Original != w2.Original));

        // save new words changes
        var newWords = new List<Word>();
        newWords.AddRange(wordBank.Words);
        newWords.AddRange(notSavedWords);
        wordBank.Words = newWords;

        return wordBank;
    }
}
