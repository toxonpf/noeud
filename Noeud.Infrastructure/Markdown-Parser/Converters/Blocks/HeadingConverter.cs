using CleaNoteMd.Domain.Models.Blocks;

using Markdig.Syntax;

namespace Noeud.Infrastructure.Markdown_Parser.Converters.Blocks;


public class HeadingConverter : IBlockConverter
{
    public MdBlock? Convert(Block block, IParserContext context)
    {
        if (block is not HeadingBlock headingBlock)
            return null;

        var myHeading = new MdHeading(headingBlock.Level);
        if (headingBlock.Inline != null)
        {
            foreach (var inline in headingBlock.Inline)
            {
                var convertedInline = context.ConvertInLine(inline);
                if (convertedInline != null)
                {
                    myHeading.Inlines.Add(convertedInline);
                }
            }
        }
        return myHeading;
    }
}
