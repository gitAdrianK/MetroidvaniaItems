namespace MetroidvaniaItems.Menus
{
    using BehaviorTree;
    using EntityComponent;
    using EntityComponent.BT;
    using JumpKing.Controller;
    using static Preferences;

    public class MenuBindButton : EntityBTNode
    {
        private EBinding Button { get; }
        private int OrderIndex { get; }

        public MenuBindButton(Entity entity, EBinding button, int orderIndex) : base(entity)
        {
            this.Button = button;
            this.OrderIndex = orderIndex;
        }

        protected override BTresult MyRun(TickData data)
        {
            var pad = ControllerManager.instance.GetMain();

            if (!pad.IsValid || !pad.IsConnected)
            {
                return BTresult.Failure;
            }

            var pressedButtons = pad.GetPad().GetPressedButtons();
            if (pressedButtons.Length == 0)
            {
                return BTresult.Running;
            }

            var binds = ModEntry.Preferences.KeyBindings[this.Button];
            ModEntry.Preferences.KeyBindings[this.Button] = this.Poll(binds, pressedButtons[0]);
            return BTresult.Success;
        }

        private int[] Poll(int[] array, int num)
        {
            if (array == null)
            {
                array = new int[this.OrderIndex + 1];
                for (var i = 0; i < array.Length; i++)
                {
                    array[i] = i == this.OrderIndex ? num : -1;
                }
            }
            else if (array.Length > this.OrderIndex)
            {
                array[this.OrderIndex] = num;
            }
            else if (array.Length - 1 < this.OrderIndex)
            {
                var array2 = new int[this.OrderIndex + 1];
                for (var j = 0; j < array2.Length; j++)
                {
                    if (j < array.Length)
                    {
                        array2[j] = array[j];
                    }
                    else if (j == this.OrderIndex)
                    {
                        array2[j] = num;
                    }
                    else
                    {
                        array2[j] = -1;
                    }
                }
                array = array2;
            }
            for (var k = 0; k < array.Length; k++)
            {
                for (var l = array.Length - 1; l > k; l--)
                {
                    if (array[k] != array[l])
                    {
                        continue;
                    }

                    var array3 = new int[array.Length - 1];
                    for (var m = 0; m < array.Length; m++)
                    {
                        if (m < l)
                        {
                            array3[m] = array[m];
                        }
                        else if (m > l)
                        {
                            array3[m - 1] = array[m];
                        }
                    }
                    array = array3;
                }
            }
            return array;
        }
    }
}
