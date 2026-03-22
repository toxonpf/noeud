using CleaNoteMd.Domain.Models.Blocks;

using Markdig.Syntax;

namespace Noeud.Infrastructure.Markdown_Parser.Converters.Blocks;

public class CodeBlockConverter : IBlockConverter
{
    public MdBlock? Convert(Block block, IParserContext context)
    {
        if (block is not CodeBlock codeBlock)
            return null;

        var myCodeBlock = new MdCodeBlock(codeBlock.Lines.ToString());
        return myCodeBlock;
    }
}
