using CleaNoteMd.Domain.Models.Blocks;

using Markdig.Syntax;


namespace Noeud.Infrastructure.Markdown_Parser.Converters;

public interface IBlockConverter
{
    MdBlock? Convert(Block block, IParserContext context);
}
