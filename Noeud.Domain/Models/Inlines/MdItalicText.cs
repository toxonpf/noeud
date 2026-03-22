namespace CleaNoteMd.Domain.Models.Inlines;

public class MdItalicText : MdInLine
{
    public MdItalicText(string text)
    {
        Text = text;
    }

    public string Text { get; set; } = string.Empty;
}
