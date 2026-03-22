using CleaNoteMd.Domain.Models.Blocks;

using Markdig.Syntax;

namespace Noeud.Infrastructure.Markdown_Parser.Converters.Blocks;

public class FencedCodeBlockConverter : IBlockConverter
{
    public MdBlock? Convert(Block block, IParserContext context)
    {
        if (block is not FencedCodeBlock fencedCodeBlock)
            return null;

        string language = fencedCodeBlock.Info ?? string.Empty;
        string rawCode = fencedCodeBlock.Lines.ToString();

        return new MdFencedCodeBlock(language, rawCode);
    }
}
