using CleaNoteMd.Domain.Models.Inlines;

using Markdig.Syntax.Inlines;

namespace Noeud.Infrastructure.Markdown_Parser.Converters;

public interface IInlineConverter
{
    MdInLine Convert(Inline inline, IParserContext context);
}
