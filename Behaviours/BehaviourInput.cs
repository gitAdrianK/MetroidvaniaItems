namespace MetroidvaniaItems.Behaviours
{
    using System;
    using Data;
    using JumpKing;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Controller;
    using JumpKing.Level;
    using JumpKing.XnaWrappers;
    using Util;

    public class BehaviourInput : IBlockBehaviour
    {
        private JKSound Select { get; } = Game1.instance.contentManager.audio.menu.Select;
        private JKSound CursorMove { get; } = Game1.instance.contentManager.audio.menu.CursorMove;

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
            var data = ModEntry.DataMetroidvania;

            switch (data.MenuState)
            {
                case MenuState.Closed:
                case MenuState.Previous:
                    this.HandleClosed(data, pressedPadState);
                    break;
                case MenuState.Select:
                    this.HandleSelect(behaviourContext, data, pressedPadState);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return true;
        }

        private void HandleClosed(DataMetroidvania data, PadState pressedPadState)
        {
            if (pressedPadState.down)
            {
                data.MenuState = MenuState.Select;
                data.Hovering = data.Active;
                return;
            }

            if (pressedPadState.up)
            {
                this.DoQuickSwitch(data);
            }
        }

        private void DoQuickSwitch(DataMetroidvania data)
        {
            data.MenuDuration = 1.0f;
            data.MenuState = MenuState.Previous;
            data.Active = data.Active == ModItems.None ? data.Previous : ModItems.None;

            this.Select.PlayOneShot();
        }

        private void HandleSelect(BehaviourContext behaviourContext, DataMetroidvania data, PadState pressedPadState)
        {
            if (!behaviourContext.BodyComp.IsOnGround || pressedPadState.jump)
            {
                // Close the menu when jumping or in air in general
                data.MenuState = MenuState.Closed;
                return;
            }

            if (pressedPadState.up)
            {
                this.DoQuickSwitch(data);
                return;
            }

            if (pressedPadState.down)
            {
                data.MenuState = MenuState.Closed;
                data.Active = data.Hovering;
                if (data.Hovering != ModItems.None)
                {
                    data.Previous = data.Hovering;
                }

                this.Select.PlayOneShot();
                return;
            }

            var next = data.GetNeighbours(data.Hovering);
            if (pressedPadState.left)
            {
                data.Hovering = next[0];
                this.CursorMove.PlayOneShot();
            }
            else if (pressedPadState.right)
            {
                data.Hovering = next[2];
                this.CursorMove.PlayOneShot();
            }
        }
    }
}
