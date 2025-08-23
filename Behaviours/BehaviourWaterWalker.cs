namespace MetroidvaniaItems.Behaviours
{
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;

    public class BehaviourWaterWalker : IBlockBehaviour
    {
        public static bool IsSolid { get; private set; }
        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            var advCollisionInfo = behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo;
            if (advCollisionInfo == null)
            {
                return true;
            }

            IsSolid = true;
            var playerRect = behaviourContext.BodyComp.GetHitbox();
            foreach (var block in advCollisionInfo.GetCollidedBlocks<WaterBlock>())
            {
                _ = block.Intersects(playerRect, out var collision);
                if (collision.Size.X <= 0 && collision.Size.Y <= 0)
                {
                    continue;
                }

                IsSolid = false;
                return true;
            }

            return true;
        }

        public float BlockPriority => 2.0f;
        public bool IsPlayerOnBlock { get; set; }
    }
}
