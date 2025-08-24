namespace MetroidvaniaItems
{
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
    using JumpKing.Player;
#if DEBUG
    using System.Diagnostics;
#endif

    [JumpKingMod(Identifier)]
    public static class ModEntry
    {
        private const string Identifier = "Zebra.MetroidvaniaItems";
        private const string HarmonyIdentifier = Identifier + ".Harmony";

        public static DataItems DataItems { get; private set; }

        public static bool IsInMenu { get; set; }

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

            var xmlFile = Path.Combine(contentManager.root, "metroidvania", "items.xml");
            if (!File.Exists(xmlFile))
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
            var loadedTypes = ModEntities.LoadEntities(xmlFile, player);

            var body = player.m_body;
            // Once again "cheesing" the player behaviour modifiers detected message by registering
            // behaviours as block behaviours.
            _ = body.RegisterBlockBehaviour<BlockInput>(new BehaviourInput());
            foreach (var type in loadedTypes)
            {
                // ReSharper disable once ConvertIfStatementToSwitchStatement
                if (type == ModItems.DoubleJump)
                {
                    _ = body.RegisterBlockBehaviour<BlockDoubleJump>(new BehaviourDoubleJump(player));
                }
                else if (type == ModItems.LowGravity)
                {
                    _ = body.RegisterBlockBehaviour<BlockLowGravity>(new BehaviourLowGravity());
                }
                else if (type == ModItems.HighGravity)
                {
                    _ = body.RegisterBlockBehaviour<BlockHighGravity>(new BehaviourHighGravity());
                }
                else if (type == ModItems.SlowFall)
                {
                    // I'm Mary Poppins y'all
                    _ = body.RegisterBlockBehaviour<BlockSlowFall>(new BehaviourSlowFall());
                }
                else if (type == ModItems.SolidWater)
                {
                    _ = body.RegisterBlockBehaviour<BlockSolidWater>(new BehaviourSolidWater());
                }
            }
        }

        /// <summary>
        ///     Called by Jump King when the Level Ends
        /// </summary>
        [UsedImplicitly]
        [OnLevelEnd]
        public static void OnLevelEnd() => DataItems.SaveToFile();
    }
}
