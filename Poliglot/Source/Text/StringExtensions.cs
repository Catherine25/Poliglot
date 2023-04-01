namespace Poliglot.Source.Text;

public static class StringExtensions
{
    public static string StartWithUpperCase(this string str)
    {
        if (str.Length == 0)
            return string.Empty;
        else if (str.Length == 1)
            return char.ToUpper(str[0]) + string.Empty;
        else
            return char.ToUpper(str[0]) + str.Substring(1);
    }
}
