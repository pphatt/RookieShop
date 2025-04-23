using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace HeadphoneStore.Application.DependencyInjection.Extensions;

public static class StringExtension
{
    // remove accents from 'é' to 'e' or 'crème brûlée' to 'creme brulee'
    public static string RemoveAccents(this string text)
    {
        var normalizedString = text.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder(capacity: normalizedString.Length);

        for (int i = 0; i < normalizedString.Length; i++)
        {
            char c = normalizedString[i];
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder
            .ToString()
            .Normalize(NormalizationForm.FormC);
    }

    public static string Slugify(this string phrase)
    {
        if (string.IsNullOrWhiteSpace(phrase))
            return string.Empty;

        string output = phrase.RemoveAccents().ToLower();
        output = Regex.Replace(output, @"[^A-Za-z0-9\s-]", "");
        output = Regex.Replace(output, @"\s+", " ").Trim();
        output = Regex.Replace(output, @"\s", "-");

        return output;
    }

    public static string FormatPascalCaseString(this string input)
    {
        var words = Regex.Matches(input, @"[A-Z][a-z]*")
                         .Cast<Match>()
                         .Select(m => m.Value)
                         .ToArray();

        if (words.Length == 0) return input;

        words[0] = CapitalizeFirstLetter(words[0]);

        for (int i = 1; i < words.Length; i++)
        {
            words[i] = words[i].ToLower();
        }

        return string.Join(" ", words);
    }

    private static string CapitalizeFirstLetter(string word)
    {
        if (string.IsNullOrEmpty(word)) return word;

        return char.ToUpper(word[0]) + word.Substring(1).ToLower();
    }
}
