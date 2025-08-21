namespace MetroidvaniaItems.Models
{
    using System.Collections.Generic;
    using BehaviorTree;
    using EntityComponent;
    using EntityComponent.BT;
    using HarmonyLib;
    using JumpKing;
    using JumpKing.PauseMenu;
    using JumpKing.PauseMenu.BT;
    using JumpKing.PauseMenu.BT.Actions;
    using JumpKing.PauseMenu.BT.Actions.BindController;
    using JumpKing.Util;
    using LanguageJK;
    using Menus;
    using Microsoft.Xna.Framework;
    using IDrawable = JumpKing.Util.IDrawable;

    // The whole keybindings UI is taken from the save states mod, thanks :D

    public static class ModelMenuOptions
    {
        private static Traverse Drawables { get; set; }

        private static List<IDrawable> MenuFactoryDrawables
        {
            get => Drawables.GetValue<List<IDrawable>>();
            set => Drawables.SetValue(value);
        }

        internal static BTsimultaneous CreateSaveStatesBindControls(object factory)
        {
            var traverse = Traverse.Create(factory);
            Drawables = traverse.Field("m_drawables");

            var entity = traverse.Field("m_entity").GetValue<Entity>();
            var guiLeft = traverse.Field("CONTROLS_GUI_FORMAT_LEFT").GetValue<GuiFormat>();
            var guiRight = traverse.Field("CONTROLS_GUI_FORMAT_RIGHT").GetValue<GuiFormat>();

            var menuSelector = new MenuSelector(guiLeft);

            var btsimultaneous = new BTsimultaneous();
            btsimultaneous.AddChild(menuSelector);
            MenuFactoryDrawables.Add(menuSelector);

            // left
            var menuFontSmall = Game1.instance.contentManager.font.MenuFontSmall;
            menuSelector.AddChild(new TextButton(language.MENUFACTORY_INPUT_SCAN_FOR_DEVICES,
                new GetSlimDevices(), menuFontSmall));
            menuSelector.AddChild(new SelectDevice(entity));
            var count = MenuFactoryDrawables.Count;
            var child = MakeBindController(0, entity);
            var child2 = MakeBindController(1, entity);
            menuSelector.AddChild(new TextButton(language.MENUFACTORY_INPUT_BIND_PRIMARY, child,
                menuFontSmall));
            menuSelector.AddChild(new TextButton(language.MENUFACTORY_INPUT_BIND_SECONDARY, child2,
                menuFontSmall));

            var btsequencor = new BTsequencor();
            btsequencor.AddChild(new MenuBindDefault(entity));
            btsequencor.AddChild(new SetBBKeyNode<bool>(entity, "BBKEY_UNSAVED_CHANGED", true));
            menuSelector.AddChild(new TextButton(language.MENUFACTORY_INPUT_DEFAULT, btsequencor,
                menuFontSmall));

            var btsequencor2 = new BTsequencor();
            btsequencor2.AddChild(new MenuSaveBind(entity));
            btsequencor2.AddChild(new SetBBKeyNode<bool>(entity, "BBKEY_UNSAVED_CHANGED", true));
            menuSelector.AddChild(new SaveNotifier(entity,
                new TextButton(language.MENUFACTORY_SAVE, btsequencor2, menuFontSmall)));

            menuSelector.Initialize();
            menuSelector.GetBounds();

            // right
            var displayFrame = new DisplayFrame(guiRight, BTresult.Running);
            displayFrame.AddChild(new MenuBindDisplay(entity,
                Preferences.EBinding.Menu));
            displayFrame.Initialize();

            var drawables = MenuFactoryDrawables;
            drawables.Insert(count, displayFrame);
            MenuFactoryDrawables = drawables;

            btsimultaneous.AddChild(new StaticNode(displayFrame, BTresult.Failure));
            return btsimultaneous;
        }

        private static IBTnode MakeBindController(int orderIndex, Entity entity)
        {
            var format = new GuiFormat
            {
                anchor_bounds = new Rectangle(0, 0, 480, 360),
                anchor = new Vector2(1f, 1f) / 2f,
                all_margin = 16,
                element_margin = 8,
                all_padding = 16
            };

            var child = new BindCatchSave(entity);
            var child2 = new MenuBindDefault(entity);
            var menuSelector = new MenuSelector(format) { AllowEscape = false };
            var child3 = new MenuSelectorBack(menuSelector);
            var btsequencor = new BTsequencor();
            btsequencor.AddChild(child2);
            btsequencor.AddChild(new SetBBKeyNode<bool>(entity, "BBKEY_UNSAVED_CHANGED", true));
            btsequencor.AddChild(child3);
            var timerAction = new TimerAction(language.MENUFACTORY_REVERTS_IN, 5, Color.Gray, btsequencor);
            menuSelector.AddChild(new TextInfo(language.MENUFACTORY_KEEPCHANGES, Color.Gray));
            menuSelector.AddChild(timerAction);
            menuSelector.AddChild(new TextButton(language.MENUFACTORY_NO, btsequencor));
            menuSelector.AddChild(new TextButton(language.MENUFACTORY_YES, child3));
            menuSelector.SetNodeForceRun(timerAction);
            menuSelector.Initialize(false);

            var drawables = MenuFactoryDrawables;
            drawables.Add(menuSelector);
            MenuFactoryDrawables = drawables;

            var btsequencor2 = new BTsequencor();
            btsequencor2.AddChild(child);
            btsequencor2.AddChild(new WaitUntilNoMenuInput());
            btsequencor2.AddChild(MakeBindButtonMenu(Preferences.EBinding.Menu, format, orderIndex, entity));
            btsequencor2.AddChild(new WaitUntilNoInput(entity));
            btsequencor2.AddChild(menuSelector);

            var btselector = new BTselector();
            btselector.AddChild(btsequencor2);
            btselector.AddChild(new PlaySFX(Game1.instance.contentManager.audio.menu.MenuFail));
            return btselector;
        }

        private static BindButtonFrame MakeBindButtonMenu(Preferences.EBinding button, GuiFormat format, int orderIndex, Entity entity)
        {
            var btsequencor = new BTsequencor();
            btsequencor.AddChild(new WaitUntilNoInput(entity));
            btsequencor.AddChild(new MenuBindButton(entity, button, orderIndex));
            btsequencor.AddChild(new SetBBKeyNode<bool>(entity, "BBKEY_UNSAVED_CHANGED", true));
            var bindButtonFrame = new BindButtonFrame(format, btsequencor);
            bindButtonFrame.AddChild(new TextButton(Util.ParseString(language.MENUFACTORY_PRESS_BUTTON, button), btsequencor));
            bindButtonFrame.Initialize();

            var draw = MenuFactoryDrawables;
            draw.Add(bindButtonFrame);
            MenuFactoryDrawables = draw;

            return bindButtonFrame;
        }
    }
}
