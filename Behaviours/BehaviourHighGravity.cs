namespace MetroidvaniaItems.Behaviours
{
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using static ModItems;

    public class BehaviourHighGravity : IBlockBehaviour
    {
        private const float HighGravGravityMultiplier = 1.25f;
        private const float HighGravXMoveMultiplier = 1.1f;
        private const float HighGravXMoveMultiplierOnGround = 0.825f;
        private const float HighGravYMoveMultiplier = 1.1f;
        public bool IsPlayerOnBlock { get; set; } = false;
        public float BlockPriority => 2.0f;

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
        {
            var modifier = 1.0f;
            if (ModEntry.DataItems.Active == ItemType.HighGravity)
            {
                modifier = behaviourContext.BodyComp.IsOnGround
                    ? HighGravXMoveMultiplierOnGround
                    : HighGravXMoveMultiplier;
            }

            return inputXVelocity * modifier;
        }

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            var bodyComp = behaviourContext.BodyComp;
            var isLowGravity = ModEntry.DataItems.Active == ItemType.HighGravity;

            var modifier = isLowGravity ? HighGravYMoveMultiplier : 1f;

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
            => inputGravity * (ModEntry.DataItems.Active == ItemType.HighGravity ? HighGravGravityMultiplier : 1.0f);

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext) => true;
    }
}
