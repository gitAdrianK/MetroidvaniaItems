namespace MetroidvaniaItems.Blocks
{
    using JetBrains.Annotations;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    [UsedImplicitly]
    public class BlockSolidWater : BoxBlock
    {
        public BlockSolidWater(Rectangle pCollider) : base(pCollider)
        {
        }
    }
}
