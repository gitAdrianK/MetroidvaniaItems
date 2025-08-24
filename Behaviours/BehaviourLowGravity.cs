namespace MetroidvaniaItems.Behaviours
{
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;

    public class BehaviourLowGravity : IBlockBehaviour
    {
        // Basically copied from JumpKingPlus LowGravityBlockBehaviour
        private const float LowGravGravityMultiplier = 0.75f;
        private const float LowGravXMoveMultiplier = 1.1f;
        private const float LowGravXMoveMultiplierOnGround = 0.825f;
        private const float LowGravYMoveMultiplier = 1.1f;
        public bool IsPlayerOnBlock { get; set; } = false;
        public float BlockPriority => 2.0f;

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
        {
            var modifier = 1.0f;
            if (ModEntry.DataItems.Active == ModItems.LowGravity)
            {
                modifier = behaviourContext.BodyComp.IsOnGround
                    ? LowGravXMoveMultiplierOnGround
                    : LowGravXMoveMultiplier;
            }

            return inputXVelocity * modifier;
        }

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            var bodyComp = behaviourContext.BodyComp;
            var isLowGravity = ModEntry.DataItems.Active == ModItems.LowGravity;

            var modifier = isLowGravity ? LowGravYMoveMultiplier : 1.0f;

            var newYVelocity = inputYVelocity * modifier;

            if (isLowGravity
                && bodyComp.IsOnGround
                && bodyComp.Velocity.Y > 0)
            {
                bodyComp.Position.Y += 1;
            }

            return newYVelocity;
        }

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext)
            => inputGravity * (ModEntry.DataItems.Active == ModItems.LowGravity ? LowGravGravityMultiplier : 1.0f);

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext) => true;
    }
}
