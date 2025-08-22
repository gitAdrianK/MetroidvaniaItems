namespace MetroidvaniaItems.Behaviours
{
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using static ModItems;

    public class BehaviourUmbrella : IBlockBehaviour
    {
        public bool IsPlayerOnBlock { get; set; } = false;
        public float BlockPriority => 2.0f;

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext)
            => inputGravity * (ModEntry.DataItems.Active == ItemType.Umbrella
                               && behaviourContext.BodyComp.Velocity.Y >= 0.0f
                ? 0.25f
                : 1.0f);

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext) => true;
    }
}
