using System.Text.RegularExpressions;

public static class TextCleaner
{
    public static string Clean(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return "";

        text = Regex.Replace(text, "<.*?>", "");
        text = Regex.Replace(text, @"\s+", " ");

        return text.Trim();
    }
}
