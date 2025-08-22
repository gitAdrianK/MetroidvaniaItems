namespace MetroidvaniaItems.Menus
{
    using BehaviorTree;
    using EntityComponent;
    using EntityComponent.BT;

    public class MenuSaveBind : EntityBTNode
    {
        public MenuSaveBind(Entity entity) : base(entity)
        {
        }

        protected override BTresult MyRun(TickData data)
        {
            ModEntry.Preferences.ForceUpdate();
            return BTresult.Success;
        }
    }
}
