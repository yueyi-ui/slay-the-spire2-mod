using System;
using HarmonyLib;
using MegaCrit.Sts2.Core.Logging;

namespace WeijiaMod
{
    [ModInitializer("Init")]
    public class Entry
    {
        public static void Init()
        {
            try
            {
                Harmony harmony = new Harmony("sts2.weijia.mod");
                harmony.PatchAll();
                Log.Debug("Weijia initialized!", 2);
            }
            catch (Exception ex)
            {
                Log.Debug("Weijia init error: " + ex.Message, 1);
            }
        }
    }
}
