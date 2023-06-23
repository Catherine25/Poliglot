namespace LibraryTests;

public class NodeTests
{
    [Test]
    public void SplitBy_NoDelimeter_SameResult()
    {
        var node = new Node("test", 1);

        node.SplitBy(Delimeters.Comma);

        var data = node.Construct();

        Assert.That(data.Count(), Is.EqualTo(1));
        Assert.That(data.Single(), Is.EqualTo("test"));
    }
}
