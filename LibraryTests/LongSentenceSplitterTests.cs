namespace LibraryTests;

public class LongSentenceSplitterTests
{
    [Test]
    public void ProduceWords_NoSeparator_SameResult()
    {
        var matches = TestGetMatches("test", Delimeters.Comma, 1);

        Assert.That(matches.Count(), Is.EqualTo(1));
        Assert.That(matches.Single(), Is.EqualTo("test"));
    }

    [Test]
    public void ProduceWords_DifferentSeparator_SameResult()
    {
        var matches = TestGetMatches("test, test", Delimeters.Semicolon, 1);

        Assert.That(matches.Count(), Is.EqualTo(1));
        Assert.That(matches.Single(), Is.EqualTo("test, test"));
    }

    [Test]
    public void ProduceWords_TooShortWord_SameResult()
    {
        var matches = TestGetMatches("test", Delimeters.Comma, 10);

        Assert.That(matches.Count(), Is.EqualTo(1));
        Assert.That(matches.Single(), Is.EqualTo("test"));
    }

    [Test]
    public void ProduceWords_SingleDelimeter_Success()
    {
        var matches = TestGetMatches("testOne testTwo, testThree; testFour", Delimeters.Comma, 10);

        Assert.That(matches.Count(), Is.EqualTo(2));
        Assert.That(matches.First(), Is.EqualTo("testOne testTwo,"));
    }

    private IEnumerable<string> TestGetMatches(string sentence, char separator, int maxLength)
    {
        var longSentenceSplitter = new LongSentenceSplitter(sentence, maxLength);
        var matches = longSentenceSplitter.SplitSentenceBy(separator);
        return matches;
    }
}