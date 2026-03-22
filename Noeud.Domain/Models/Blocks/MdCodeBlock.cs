namespace CleaNoteMd.Domain.Models.Blocks;

// Это специальный блок, он не наслудует Inlines, потому что внутри просто текст
public class MdCodeBlock : MdLeafBlock
{
    public MdCodeBlock(string rawContent)
    {
        RawContent = rawContent;
    }

    public string RawContent { get; set; }
}
