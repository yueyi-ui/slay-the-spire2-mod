using System.Collections.Generic;
using System.Linq;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Factories;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Runs;

namespace HermitMod.Patches
{
    [HarmonyPatch(typeof(CardFactory), "CreateForReward")]
    public class PrismaticCardRewardPatch
    {
        static void Postfix(
            Player player,
            int count,
            CardCreationOptions options,
            ref IEnumerable<CardCreationResult> __result)
        {
            // 检查玩家是否有 OldLocket 遗物
            bool hasLocket = player?.Relics?.Any(r => r.Id.ToString() == "HERMITMOD-OLD_LOCKET") ?? false;
            if (!hasLocket) return;

            var resultList = __result?.ToList();
            if (resultList == null || resultList.Count == 0) return;

            // 获取其他角色卡牌（非基础、非诅咒）
            var otherCards = GetOtherCharacterCards();
            if (otherCards.Count == 0) return;

            // 替换最后一张卡牌为其他角色卡牌
            int replaceIndex = resultList.Count - 1;
            int randomIdx = GD.RandRange(0, otherCards.Count - 1);
            resultList[replaceIndex] = new CardCreationResult(otherCards[randomIdx]);

            __result = resultList;
        }

        private static List<CardModel> GetOtherCharacterCards()
        {
            var cards = new List<CardModel>();

            foreach (var character in ModelDb.AllCharacters)
            {
                if (character == null) continue;
                if (character.Id.ToString() == "HERMITMOD") continue;

                var pool = character.CardPool;
                if (pool == null) continue;

                foreach (var card in pool.AllCards)
                {
                    if (card != null &&
                        card.Rarity != CardRarity.Basic &&
                        card.Rarity != CardRarity.Curse)
                    {
                        cards.Add(card);
                    }
                }
            }

            return cards;
        }
    }
}