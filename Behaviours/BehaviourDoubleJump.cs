namespace MetroidvaniaItems.Behaviours
{
    using System;
    using JumpKing;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using JumpKing.Player;

    public class BehaviourDoubleJump : IBlockBehaviour
    {
        public bool IsPlayerOnBlock { get; set; }
        public float BlockPriority => 2.0f;

        private bool DoubleJumpFlag { get; set; }
        private float DoubleJumpVelocity  { get; set; }

        private InputComponent Input { get; set; }
        private PlayerEntity Player { get; }

        public BehaviourDoubleJump(PlayerEntity player)
            => this.Player = player ?? throw new ArgumentNullException(nameof(player));

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            var bodyComp = behaviourContext.BodyComp;
            if (behaviourContext.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return true;
            }

            if (ModEntry.DataItems.Active == ModItems.ItemType.DoubleJump && this.Player.m_body.IsOnGround)
            {
                this.DoubleJumpFlag = true;
                this.DoubleJumpVelocity = 0f;
            }
            else if (ModEntry.DataItems.Active != ModItems.ItemType.DoubleJump && this.Player.m_body.IsOnGround)
            {
                this.DoubleJumpFlag = false;
            }

            if (!this.DoubleJumpFlag)
            {
                return true;
            }

            this.DoubleJumpVelocity = Math.Min(bodyComp.Velocity.Y, this.DoubleJumpVelocity);
            this.Input = this.Player.GetComponent<InputComponent>();
            if (!(bodyComp.Velocity.Y > -1.0f) || !this.Input.GetState().jump || this.Player.m_body.IsOnGround)
            {
                return true;
            }

            if (this.Input.GetState().right)
            {
                bodyComp.Velocity.X = PlayerValues.SPEED;
            }
            else if (this.Input.GetState().left)
            {
                bodyComp.Velocity.X = -PlayerValues.SPEED;
            }
            else
            {
                bodyComp.Velocity.X = 0f;
            }
            bodyComp.Velocity.Y = this.DoubleJumpVelocity;
            this.DoubleJumpFlag = false;
            this.DoubleJumpVelocity = 0f;
            return true;
        }
    }
}
