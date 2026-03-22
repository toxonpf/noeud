namespace CleaNoteMd.Domain.Models.Inlines;

public class MdBoldText : MdInLine
{
    public MdBoldText(string text)
    {
        Text = text;
    }

    public string Text { get; set; } = string.Empty;
}
