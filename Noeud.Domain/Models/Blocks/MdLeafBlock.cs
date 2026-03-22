using CleaNoteMd.Domain.Models.Inlines;

namespace CleaNoteMd.Domain.Models.Blocks;

// Блок от которого наследуются все блоки содержащие в себе
// текст с форматированием -> Bold Italic ect.
// но никак не вложенные в него другие блоки, только текст
public abstract class MdLeafBlock : MdBlock
{
    public List<MdInLine> Inlines { get; set; } = new();
}
