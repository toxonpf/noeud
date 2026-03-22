using CleaNoteMd.Domain.Models.Inlines;

using Markdig.Syntax.Inlines;

namespace Noeud.Infrastructure.Markdown_Parser.Converters.Inlines;

public class EmphasisInlineConverter : IInlineConverter
{
    public MdInLine? Convert(Inline inline, IParserContext context)
    {
        if (inline is not EmphasisInline emphasisInline)
            return null;

        string innerText = string.Empty;
        foreach (var childInline in emphasisInline)
        {
            if (childInline is LiteralInline lit)
                innerText += lit.Content.ToString();
        }

        if (emphasisInline.DelimiterCount == 1)
        {
            var myMdItalicText = new MdItalicText(innerText);
            return myMdItalicText;
        }
        else
        {
            var myMdBoldText = new MdBoldText(innerText);
            return myMdBoldText;
        }
    }
}
