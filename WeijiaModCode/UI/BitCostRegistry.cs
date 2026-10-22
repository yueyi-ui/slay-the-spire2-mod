using System.Collections.Generic;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Cards;
using WeijiaMod.Powers;

namespace WeijiaMod.UI
{
    /// <summary>
    /// 比特费用系统：给卡牌设置比特费用。
    /// - 卡面左上角显示比特图标+数字
    /// - 比特不足时卡牌不可打出（CanPlay 补丁检查）
    /// - 打出卡牌时由 OnPlay 中的 WeijiaBits.Spend 扣除
    /// 参考 Regent 星标机制。
    /// </summary>
    public static class BitCostRegistry
    {
        private static readonly Dictionary<string, int> _bitCosts = new();

        /// <summary>
        /// 设置卡牌的比特费用。
        /// </summary>
        public static void SetBitCost(CardModel card, int cost)
        {
            _bitCosts[card.Id.Entry] = cost;
        }

        /// <summary>
        /// 获取卡牌的比特费用，无则返回 0。
        /// </summary>
        public static int GetBitCost(CardModel card)
        {
            if (card == null) return 0;
            return _bitCosts.TryGetValue(card.Id.Entry, out int cost) ? cost : 0;
        }

        /// <summary>
        /// 卡牌是否有比特费用。
        /// </summary>
        public static bool HasBitCost(CardModel card)
        {
            return GetBitCost(card) > 0;
        }

        private static Texture2D _bitIcon;

        /// <summary>
        /// 比特图标（阴阳圆形图案）。
        /// </summary>
        public static Texture2D GetBitIcon()
        {
            if (_bitIcon != null) return _bitIcon;
            _bitIcon = ResourceLoader.Load<Texture2D>("res://images/powers/bit_icon.png", null, ResourceLoader.CacheMode.Reuse);
            return _bitIcon;
        }
    }

    /// <summary>
    /// 比特不足时卡牌不可打出（参考 Regent 星标机制）。
    /// 通过 TargetMethod 精确 patch CanPlay(out UnplayableReason, out AbstractModel)。
    /// </summary>
    [HarmonyPatch]
    public static class BitCostCanPlay
    {
        private static System.Reflection.MethodBase TargetMethod()
        {
            return typeof(CardModel).GetMethod("CanPlay",
                new[] { typeof(UnplayableReason).MakeByRefType(), typeof(AbstractModel).MakeByRefType() });
        }

        [HarmonyPostfix]
        private static void CheckBitCost(CardModel __instance, ref bool __result, ref UnplayableReason reason)
        {
            try
            {
                if (!__result) return; // 已不可打
                int bitCost = BitCostRegistry.GetBitCost(__instance);
                if (bitCost <= 0) return;

                var owner = __instance.Owner;
                if (owner == null || owner.PlayerCombatState == null) return;

                // 检查比特是否足够
                if (WeijiaBits.Get(owner) < bitCost)
                {
                    __result = false;
                    reason |= UnplayableReason.StarCostTooHigh;
                }
            }
            catch (System.Exception)
            {
                // 检查失败不阻止打出
            }
        }
    }

    /// <summary>
    /// 在卡面左上角显示比特费用（位于能量费用下方，参考星标费用位置）。
    /// </summary>
    [HarmonyPatch]
    public static class BitCostVisuals
    {
        [HarmonyPatch(typeof(NCard), "UpdateStarCostVisuals")]
        [HarmonyPostfix]
        private static void OnUpdateStarCostVisuals(NCard __instance)
        {
            try
            {
                var model = __instance.Model;
                if (model == null) return;

                int bitCost = BitCostRegistry.GetBitCost(model);
                if (bitCost <= 0) return;

                // 在星标位置显示比特（不干扰原星标逻辑）
                var starLabel = __instance.GetNodeOrNull<Label>("%StarLabel");
                var starIcon = __instance.GetNodeOrNull<TextureRect>("%StarIcon");
                if (starLabel == null) return;

                // 卡没有星标费用时，用比特费用填充显示
                if (model.CanonicalStarCost < 0 && model.CurrentStarCost < 0)
                {
                    starLabel.Text = bitCost.ToString();
                    starLabel.Visible = true;
                    if (starIcon != null)
                    {
                        starIcon.Visible = true;
                        var icon = BitCostRegistry.GetBitIcon();
                        if (icon != null) starIcon.Texture = icon;
                    }
                }
            }
            catch (System.Exception)
            {
                // 显示失败不影响卡牌功能
            }
        }
    }
}
