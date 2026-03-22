using CleaNoteMd.Domain.Models.Blocks;

using Markdig.Syntax;

namespace Noeud.Infrastructure.Markdown_Parser.Converters.Blocks;

public class ParagraphConverter : IBlockConverter
{
    public MdBlock? Convert(Block block, IParserContext context)
    {
        if (block is not ParagraphBlock paragraphBlock)
            return null;

        var myParagraph = new MdParagraph();
        if (paragraphBlock.Inline != null)
        {
            foreach (var inline in paragraphBlock.Inline)
            {
                var convertedInline = context.ConvertInLine(inline);
                if (convertedInline != null)
                {
                    myParagraph.Inlines.Add(convertedInline);
                }
            }
        }
        return myParagraph;
    }
}
