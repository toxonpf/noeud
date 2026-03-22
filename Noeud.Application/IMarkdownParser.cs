using CleaNoteMd.Domain.Models.Blocks;

namespace Noeud.Application;

public interface IMarkdownParser
{
    IEnumerable<MdBlock> Parse(string rawText);
}

