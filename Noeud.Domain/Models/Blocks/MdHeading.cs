namespace CleaNoteMd.Domain.Models.Blocks;

public class MdHeading : MdLeafBlock
{
    public MdHeading(int level)
    {
        Level = level;
    }

    public int Level { get; set; }
}
