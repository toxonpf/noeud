using CleaNoteMd.Domain.Models.Blocks;

using Markdig.Syntax;

namespace Noeud.Infrastructure.Markdown_Parser.Converters.Blocks;

public class QuoteConverter : IBlockConverter
{
    public MdBlock? Convert(Block block, IParserContext context)
    {
        if (block is not QuoteBlock quoteBlock)
            return null;

        var myQuote = new MdQuote();

        foreach (var innerBlock in quoteBlock)
        {
            var convertedInlineBlock = context.ConvertBlock(innerBlock);

            if (convertedInlineBlock != null)
            {
                myQuote.ChildrenMd.Add(convertedInlineBlock);
            }
        }

        return myQuote;
    }
}
