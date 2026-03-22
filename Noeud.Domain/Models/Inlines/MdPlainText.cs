namespace CleaNoteMd.Domain.Models.Inlines;

// Самый обычный текст без ничего
public class MdPlainText : MdInLine
{
    public MdPlainText(string text)
    {
        Text = text;
    }


    public string Text { get; set; } = string.Empty;
}
