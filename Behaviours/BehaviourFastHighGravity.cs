namespace MetroidvaniaItems.Behaviours
{
    using System;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Patches;

    public class BehaviourFastHighGravity : IBlockBehaviour
    {
        public bool IsPlayerOnBlock { get; set; }

        public float BlockPriority => 2.0f;

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            if (ModEntry.DataMetroidvania.Active != ModItems.FastHighGravity)
            {
                return inputYVelocity;
            }

            var bodyComp = behaviourContext.BodyComp;

            bodyComp.Velocity.Y = Math.Max(bodyComp.Velocity.Y, AdjustYVelocity());
            if (bodyComp.Velocity.Y > 0.0f)
            {
                bodyComp.Velocity.Y = Math.Min(bodyComp.Velocity.Y, 10.0f);
            }

            return inputYVelocity;
        }

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext)
            => inputGravity * (ModEntry.DataMetroidvania.Active == ModItems.FastHighGravity ? 1.15f : 1.0f);

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext) => true;

        private static float AdjustYVelocity()
        {
            switch (PatchJumpStateMyRun.JumpFrames)
            {
                case 0:
                    return 0.000f;
                case 1:
                    return -0.700f;
                case 2:
                    return -0.950f;
                case 3:
                    return -1.200f;
                case 4:
                    return -1.450f;
                case 5:
                    return -1.700f;
                case 6:
                    return -1.950f;
                case 7:
                    return -2.200f;
                case 8:
                    return -2.450f;
                case 9:
                    return -2.700f;
                case 10:
                    return -2.950f;
                case 11:
                    return -3.200f;
                case 12:
                    return -3.450f;
                case 13:
                    return -3.700f;
                case 14:
                    return -3.950f;
                case 15:
                    return -4.200f;
                case 16:
                    return -4.450f;
                case 17:
                    return -4.698f;
                case 18:
                    return -4.950f;
                case 19:
                    return -5.200f;
                case 20:
                    return -5.450f;
                case 21:
                    return -5.655f;
                case 22:
                    return -5.950f;
                case 23:
                    return -6.203f;
                case 24:
                    return -6.462f;
                case 25:
                    return -6.715f;
                case 26:
                    return -6.970f;
                case 27:
                    return -7.228f;
                default:
                    return -7.435f;
            }
        }
    }
}
