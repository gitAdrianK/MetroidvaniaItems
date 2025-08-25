namespace MetroidvaniaItems.Behaviours
{
    using JumpKing;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Controller;
    using JumpKing.Level;

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
            var pressedPadState = ControllerManager.instance.GetPressedPadState();
            if (!behaviourContext.BodyComp.IsOnGround || pressedPadState.jump)
            {
                // Close the menu for pretty much all actions unrelated to the menu
                ModEntry.IsInMenu = false;
                return true;
            }

            var data = ModEntry.DataItems;
            if (pressedPadState.down)
            {
                if (!ModEntry.IsInMenu)
                {
                    data.Hovering = data.Active;
                }
                else
                {
                    data.Active = data.Hovering;
                    Game1.instance.contentManager.audio.menu.Select.PlayOneShot();
                }

                ModEntry.IsInMenu = !ModEntry.IsInMenu;
            }

            if (!ModEntry.IsInMenu)
            {
                return true;
            }

            var next = data.GetNeighbors(data.Hovering);
            if (pressedPadState.left)
            {
                data.Hovering = next[0];
            }
            else if (pressedPadState.right)
            {
                data.Hovering = next[2];
            }

            return true;
        }
    }
}
