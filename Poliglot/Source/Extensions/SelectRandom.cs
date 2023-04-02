namespace Poliglot.Source.Extensions;

public static class IEnumerableExtensions
{
    private static readonly Random random = new();

    public static T SelectRandom<T>(this IEnumerable<T> data)
    {
        int amount = data.Count();
        return data.ElementAt(random.Next(amount));
    }
}
