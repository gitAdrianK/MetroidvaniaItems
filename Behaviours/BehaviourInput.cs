namespace MetroidvaniaItems.Behaviours
{
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Controller;
    using JumpKing.Level;
    using Patches;

    public class BehaviourInput : IBlockBehaviour
    {
        public bool IsPlayerOnBlock { get; set; } = false;
        public float BlockPriority => 2.0f;

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (!behaviourContext.BodyComp.IsOnGround)
            {
                // Close the menu on jump, things get fucky otherwise
                ModEntry.IsInMenu = false;
                return true;
            }

            if (PatchControllerManager.CustomPadInstance.GetPressed().OpenCloseItemsMenu)
            {
                ModEntry.IsInMenu = !ModEntry.IsInMenu;
            }

            if (!ModEntry.IsInMenu)
            {
                return true;
            }

            var pressedPadState = ControllerManager.instance.GetPressedPadState();
            var next = ModEntry.DataItems.GetActiveNeighbors();
            if (pressedPadState.left)
            {
                ModEntry.DataItems.Active = next[0];
            }

            if (pressedPadState.right)
            {
                ModEntry.DataItems.Active = next[2];
            }

            return true;
        }
    }
}
