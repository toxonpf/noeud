using CleaNoteMd.Domain.Models.Blocks;
using CleaNoteMd.Domain.Models.Inlines;

using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Noeud.Infrastructure.Markdown_Parser.Converters;

public interface IParserContext
{
    MdBlock? ConvertBlock(Block block);
    MdInLine? ConvertInLine(Inline inline);
}
