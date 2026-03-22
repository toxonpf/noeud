using CleaNoteMd.Domain.Models.Inlines;

using Markdig.Syntax.Inlines;

namespace Noeud.Infrastructure.Markdown_Parser.Converters.Inlines;

public class LiteralInlineConverter : IInlineConverter
{
    public MdInLine? Convert(Inline inline, IParserContext context)
    {
        if (inline is not LiteralInline literalInline)
            return null;
        string plainText = literalInline.Content.ToString();

        var myPlainText = new MdPlainText(plainText);

        return myPlainText;
    }
}
