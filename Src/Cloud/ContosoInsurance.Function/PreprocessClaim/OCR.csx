#r "System.IO"
#r "System.Runtime"
#r "System.Threading.Tasks"

#load "..\Shared\Settings.csx"

using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

public static class OCR
{
    public static async Task UpdateAsync<T>(T t, string imageKind, Stream imageStream)
        where T : class
    {
        var client = new VisionServiceClient(Settings.MSVisionServiceSubscriptionKey);
        var result = await client.RecognizeTextAsync(imageStream);
        await Task.Run(() => UpdateProperties(t, imageKind, result));
    }

    private static void UpdateProperties<T>(T t, string imageKind, OcrResults result)
    {
        var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (!property.SetMethod.IsPublic) continue;

            var attribute = property.GetCustomAttribute<OCRAttribute>(true);
            if (attribute == null) continue;
            if (attribute.ImageKind != imageKind) continue;

            var text = result.GetText(attribute.Rectangle);
            if (!string.IsNullOrEmpty(attribute.ValuePattern))
                text = Regex.Match(text, attribute.ValuePattern).Value;

            object value = GetPropertyValue(property.PropertyType, text);
            if (value != null)
                property.SetMethod.Invoke(t, new[] { value });
        }
    }

    private static object GetPropertyValue(Type propertyType, string text)
    {
        if (propertyType != typeof(string))
        {
            var converter = TypeDescriptor.GetConverter(propertyType);
            if (converter != null)
            {
                if (converter.CanConvertFrom(typeof(string)))
                    return converter.ConvertFromString(text);
            }
            return null;
        }
        else
            return text;
    }
}

#region Extensions

public static string GetText(this OcrResults result, Rectangle rect)
{
    List<Word> words = new List<Word>();

    foreach (var region in result.Regions)
        foreach (var line in region.Lines)
            foreach (var word in line.Words)
                if (rect.Contains(word.Rectangle))
                    words.Add(word);

    return string.Join(" ", words.Select(i => i.Text));
}

public static string GetText(this OcrResults result, System.Drawing.Rectangle rect)
{
    return GetText(result, new Rectangle
    {
        Left = rect.Left,
        Top = rect.Top,
        Width = rect.Width,
        Height = rect.Height
    });
}

public static bool Contains(this Rectangle rect, Rectangle other)
{
    return rect.Top < other.Top
        && rect.Left < other.Left
        && (rect.Left + rect.Width) > (other.Left + other.Width)
        && (rect.Top + rect.Height) > (other.Top + other.Height);
}

#endregion