namespace CleaNoteMd.Domain.Models.Blocks;

public class MdFencedCodeBlock : MdBlock
{
    public MdFencedCodeBlock(string? language, string? rawContent)
    {
        Language = language;
        RawContent = rawContent;
    }


    public string Language { get; set; } = string.Empty;
    public string RawContent { get; set; } = string.Empty;
}
