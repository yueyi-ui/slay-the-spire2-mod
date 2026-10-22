using System;
using System.Collections.Concurrent;
using HarmonyLib;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Players;

namespace WeijiaMod.Powers
{
    /// <summary>
    /// 比特资源系统。
    /// 比特是可在战斗中积累和消耗的资源（类似能量），跨回合保留；
    /// 战斗结束时清空。
    /// 卡牌有比特费用时，比特不足则不可打出（由 BitCostRegistry + CanPlay 补丁处理）。
    /// </summary>
    public static class WeijiaBits
    {
        private static readonly ConcurrentDictionary<ulong, int> _bits = new ConcurrentDictionary<ulong, int>();

        public static event Action<Player, int, int> BitsChanged;

        public static int Get(Player player)
        {
            if (player == null) return 0;
            return _bits.TryGetValue(player.NetId, out int val) ? val : 0;
        }

        public static void Gain(Player player, int amount)
        {
            if (player == null || amount == 0) return;
            int old = Get(player);
            _bits.AddOrUpdate(player.NetId, amount, (_, oldVal) => oldVal + amount);
            int newVal = Get(player);
            if (old != newVal)
                BitsChanged?.Invoke(player, old, newVal);
        }

        public static int Spend(Player player, int amount)
        {
            if (player == null) return 0;
            ulong id = player.NetId;
            lock (_bits)
            {
                int old = Get(player);
                int current = old;
                int actual = current >= amount ? amount : current;
                _bits[id] = current - actual;
                int newVal = Get(player);
                if (old != newVal)
                    BitsChanged?.Invoke(player, old, newVal);
                return actual;
            }
        }

        public static void Set(Player player, int amount)
        {
            if (player == null) return;
            int old = Get(player);
            _bits[player.NetId] = amount;
            int newVal = Get(player);
            if (old != newVal)
                BitsChanged?.Invoke(player, old, newVal);
        }

        /// <summary>
        /// 清空所有玩家的比特（战斗结束时调用）。
        /// </summary>
        public static void ClearAll()
        {
            _bits.Clear();
        }
    }

    /// <summary>
    /// 比特生命周期管理：战斗结束时清空比特，防止残留到下一场战斗。
    /// </summary>
    [HarmonyPatch]
    public static class BitLifecycle
    {
        [HarmonyPatch(typeof(CombatManager), "OnCombatEnd")]
        [HarmonyPostfix]
        private static void OnCombatEnd()
        {
            WeijiaBits.ClearAll();
        }
    }
}
