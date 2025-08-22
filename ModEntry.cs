namespace MetroidvaniaItems
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics.CodeAnalysis;
    using System.IO;
    using System.Reflection;
    using Behaviours;
    using Blocks;
    using Data;
    using EntityComponent;
    using HarmonyLib;
    using JetBrains.Annotations;
    using JumpKing;
    using JumpKing.Mods;
    using JumpKing.PauseMenu;
    using JumpKing.PauseMenu.BT;
    using JumpKing.Player;
    using Models;
    using static ModItems.ItemType;
#if DEBUG
    using System.Diagnostics;
#endif

    [JumpKingMod(Identifier)]
    public static class ModEntry
    {
        private const string Identifier = "Zebra.MetroidvaniaItems";
        private const string HarmonyIdentifier = Identifier + ".Harmony";
        private const string SettingsFile = Identifier + ".Settings.xml";

        private static string PreferencesPath { get; set; }
        public static Preferences Preferences { get; private set; }
        public static DataItems DataItems { get; private set; }

        public static bool IsInMenu { get; set; }

        // TODO: This has to be tested, menus don't show up unless uploaded to steam
        [UsedImplicitly]
        [SuppressMessage("Style", "IDE0060:Remove unused parameter", Justification = "Required for JK")]
        [PauseMenuItemSetting]
        [MainMenuItemSetting]
        public static TextButton BindSettings(object factory, GuiFormat format)
            => new TextButton("Bind Key(s)", ModelMenuOptions.CreateSaveStatesBindControls(factory));

        /// <summary>
        ///     Called by Jump King before the level loads
        /// </summary>
        [UsedImplicitly]
        [BeforeLevelLoad]
        public static void BeforeLevelLoad()
        {
#if DEBUG
            _ = Debugger.Launch();
#endif

            var harmony = new Harmony(HarmonyIdentifier);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            var dllPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ??
                          throw new InvalidOperationException();

            PreferencesPath = Path.Combine(dllPath, SettingsFile);
            Preferences = Preferences.LoadFromFile(PreferencesPath);
            Preferences.PropertyChanged += SaveToFile;

            ModResources.LoadDefaultTextures();
        }

        /// <summary>
        ///     Called by Jump King when the Level Starts
        /// </summary>
        [UsedImplicitly]
        [OnLevelStart]
        public static void OnLevelStart()
        {
            var contentManager = Game1.instance.contentManager;
            var level = contentManager.level;
            if (level is null)
            {
                return;
            }

            var file = Path.Combine(contentManager.root, "props", "metroidvania", "items.xml");
            if (!File.Exists(file))
            {
                return;
            }

            var entityManager = EntityManager.instance;
            var player = entityManager.Find<PlayerEntity>();
            if (player is null)
            {
                return;
            }

            DataItems = DataItems.ReadFromFile();
            ModResources.LoadCustomTextures();
            var loadedTypes = ModEntities.LoadEntities(player);

            var body = player.m_body;
            // Once again "cheesing" the player behaviour modifiers detected message by registering
            // behaviours as block behaviours.
            _ = body.RegisterBlockBehaviour<BlockInput>(new BehaviourInput());
            foreach (var type in loadedTypes)
            {
                switch (type)
                {
                    case None:
                        // None means none.
                        break;
                    case DoubleJump:
                        _ = body.RegisterBlockBehaviour<BlockDoubleJump>(new BehaviourDoubleJump(player));
                        break;
                    case LowGravity:
                        _ = body.RegisterBlockBehaviour<BlockLowGravity>(new BehaviourLowGravity());
                        break;
                    case HighGravity:
                        _ = body.RegisterBlockBehaviour<BlockHighGravity>(new BehaviourHighGravity());
                        break;
                    case Umbrella:
                        // I'm Mary Poppins y'all
                        _ = body.RegisterBlockBehaviour<BlockUmbrella>(new BehaviourUmbrella());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        /// <summary>
        ///     Called by Jump King when the Level Ends
        /// </summary>
        [UsedImplicitly]
        [OnLevelEnd]
        public static void OnLevelEnd() => DataItems.SaveToFile();

        private static void SaveToFile(object sender, PropertyChangedEventArgs args)
            => Preferences.SaveToFile(PreferencesPath);
    }
}
