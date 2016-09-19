#r "System.Drawing"

using System.Drawing;

[AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
sealed class OCRAttribute : Attribute
{
    public string ImageKind { get; set; }

    public Rectangle Rectangle { get; private set; }

    public string ValuePattern { get; set; }

    public OCRAttribute(string imageKind, int left, int top, int width, int height, string valuePattern = null)
    {
        this.ImageKind = imageKind;
        this.Rectangle = new Rectangle(left, top, width, height);
        this.ValuePattern = valuePattern;
    }
}