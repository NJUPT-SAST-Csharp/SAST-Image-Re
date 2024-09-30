namespace WebApp.Utils;

public static class StringExtentions
{
    public const string ValidCharacters =
        "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_";

    public static bool IsValid(this string str)
    {
        foreach (char c in str)
        {
            if (ValidCharacters.Contains(c) == false)
                return false;
        }

        return true;
    }

    public static bool IsDigit(this string str)
    {
        return int.TryParse(str, out _);
    }
}
