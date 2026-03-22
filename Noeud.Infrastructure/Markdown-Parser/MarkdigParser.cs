using CleaNoteMd.Domain.Models.Blocks;
using CleaNoteMd.Domain.Models.Inlines;

using Noeud.Application;

using Markdig;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

using Noeud.Infrastructure.Markdown_Parser.Converters;
using Noeud.Infrastructure.Markdown_Parser.Converters.Blocks;
using Noeud.Infrastructure.Markdown_Parser.Converters.Inlines;

namespace Noeud.Infrastructure.Markdown_Parser;

public class MarkdigParser : IMarkdownParser, IParserContext
{
    private readonly Dictionary<Type, IBlockConverter> _blockConverters;
    private readonly Dictionary<Type, IInlineConverter> _inlineConverters;

    public MarkdigParser()
    {
        _blockConverters = new Dictionary<Type, IBlockConverter>
        {
            { typeof(HeadingBlock), new HeadingConverter() },
            { typeof(ParagraphBlock), new ParagraphConverter() },
            { typeof(ThematicBreakBlock), new ThematicBreakConverter() },
            { typeof(CodeBlock), new CodeBlockConverter() },
            { typeof(FencedCodeBlock), new FencedCodeBlockConverter() },
            { typeof(QuoteBlock), new QuoteConverter() }
        };

        _inlineConverters = new Dictionary<Type, IInlineConverter>
        {
            { typeof(LiteralInline), new LiteralInlineConverter() },
            { typeof(EmphasisInline), new EmphasisInlineConverter() }
        };
    }

    public IEnumerable<MdBlock> Parse(string rawText)
    {
        var pipeline = new MarkdownPipelineBuilder().Build();
        var document = Markdig.Markdown.Parse(rawText, pipeline);
        var resultBlocks = new List<MdBlock>();

        foreach (var markdigBlock in document)
        {
            var ownBlock = ConvertBlock(markdigBlock);
            if (ownBlock != null) resultBlocks.Add(ownBlock);
        }

        return resultBlocks;
    }

    public MdBlock? ConvertBlock(Block block)
    {
        var blockType = block.GetType();

        if (_blockConverters.TryGetValue(blockType, out var converter))
        {
            return converter.Convert(block, this);
        }

        if (block is LeafBlock leafBlock && leafBlock.Inline != null)
        {
            var fallBlockParagraph = new MdParagraph();
            foreach (var inline in leafBlock.Inline)
            {
                var convertedInline = ConvertInLine(inline);
                if (convertedInline != null)
                {
                    fallBlockParagraph.Inlines.Add(convertedInline);
                }
            }

            return fallBlockParagraph;
        }

        return null;
    }

    public MdInLine? ConvertInLine(Inline inline)
    {
        var inlineType = inline.GetType();

        if (_inlineConverters.TryGetValue(inlineType, out var converter))
        {
            return converter.Convert(inline, this);
        }

        if (inline is ContainerInline containerInline)
        {
            string fallbackText = string.Empty;

            foreach (var child in containerInline)
            {
                if (child is LiteralInline lit)
                {
                    fallbackText += lit.Content.ToString();
                }
            }

            if (!string.IsNullOrWhiteSpace(fallbackText))
            {
                return new MdPlainText(fallbackText);
            }
        }

        else if (inline is CodeInline codeInline)
        {
            return new MdPlainText(codeInline.Content);
        }

        return null;
    }
}
