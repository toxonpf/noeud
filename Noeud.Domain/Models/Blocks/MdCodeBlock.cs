namespace CleaNoteMd.Domain.Models.Blocks;

// Это специальный блок, он не наслудует Inlines, потому что внутри просто текст
public class MdCodeBlock : MdLeafBlock
{
    public MdCodeBlock(string text)
    {
        Text = text;
    }

    public string Text { get; set; }
}
