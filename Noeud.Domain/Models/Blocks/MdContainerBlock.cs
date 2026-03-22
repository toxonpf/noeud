namespace CleaNoteMd.Domain.Models.Blocks;

// Блок от которого наследуются все блоки контейнеры
// Позволяет создать вложенность -> Quote Tabulation etc.
public abstract class MdContainerBlock : MdBlock
{
    public List<MdBlock> ChildrenMd { get; set; } = new();
}
