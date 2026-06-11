using System;
using System.Collections.Generic;
using System.Reflection;
using Godot;
using HarmonyLib;
using HermitMod.Cards;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Logging;  // 明确用这个 Logger
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Rewards;
using MegaCrit.Sts2.Core.Runs;

namespace HermitMod
{
    [ModInitializer("Initialize")]
    [ScriptPath("res://HermitModCode/MainFile.cs")]
    public partial class MainFile : Node  // 加 partial
    {
        // 明确用 MegaCrit 的 Logger，避免和 Godot.Logger 冲突
        public static MegaCrit.Sts2.Core.Logging.Logger Logger { get; }
            = new MegaCrit.Sts2.Core.Logging.Logger("HermitMod", LogType.Generic);

        public static void Initialize()
        {
            Harmony harmony = new Harmony("HermitMod");
            Assembly executingAssembly = Assembly.GetExecutingAssembly();
            foreach (Type type in executingAssembly.GetTypes())
            {
                try
                {
                    List<HarmonyMethod> fromType = HarmonyMethodExtensions.GetFromType(type);
                    if (fromType != null && fromType.Count > 0)
                    {
                        harmony.CreateClassProcessor(type).Patch();
                    }
                }
                catch (Exception ex)
                {
                    Logger.Info("Skipping patch " + type.Name + ": " + ex.Message, 1);
                }
            }
            MainFile.Logger.Info("The Hermit mod loaded successfully!", 1);
        }
        public const string ModId = "HermitMod";
    }
}