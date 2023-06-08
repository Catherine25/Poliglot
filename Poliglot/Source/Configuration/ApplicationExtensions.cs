namespace Poliglot.Source.Configuration;

public static class Settings
{
    public static Color GetPrimaryColor()
    {
        return GetColorByName("Primary");
    }

    public static Color GetSecondaryColor()
    {
        return GetColorByName("Secondary");
    }

    private static Color GetColorByName(string name)
    {
        var dictionary = Application.Current.Resources.MergedDictionaries.First();
        var color = dictionary[name];
        return (Color)color;
    }
}
