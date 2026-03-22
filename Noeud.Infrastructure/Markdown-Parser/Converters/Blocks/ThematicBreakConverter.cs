using CleaNoteMd.Domain.Models.Blocks;

using Markdig.Syntax;

namespace Noeud.Infrastructure.Markdown_Parser.Converters.Blocks;

public class ThematicBreakConverter : IBlockConverter
{
    public MdBlock? Convert(Block block, IParserContext context)
    {
        if (block is not ThematicBreakBlock)
            return null;

        return new MdHorizontalLine();
    }
}
