namespace MetroidvaniaItems.Blocks
{
    using JetBrains.Annotations;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    [UsedImplicitly]
    public class BlockLowGravity : BoxBlock
    {
        public BlockLowGravity(Rectangle pCollider) : base(pCollider)
        {
        }
    }
}
