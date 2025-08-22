namespace MetroidvaniaItems.Behaviours
{
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using static ModItems;

    public class BehaviourHighGravity : IBlockBehaviour
    {
        // Basically copied from ExpansionBlocks, similar behaviour is important right?
        private const float HighGravYMoveMultiplier = 1.5f;
        private const float HighGravXMoveMultiplier = 0.9f;
        public bool IsPlayerOnBlock { get; set; } = false;
        public float BlockPriority => 2.0f;

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
            => inputXVelocity * (ModEntry.DataItems.Active == ItemType.HighGravity ? HighGravXMoveMultiplier : 1.0f);

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext)
            => inputGravity * (ModEntry.DataItems.Active == ItemType.HighGravity ? HighGravYMoveMultiplier : 1.0f);

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext) => true;
    }
}
