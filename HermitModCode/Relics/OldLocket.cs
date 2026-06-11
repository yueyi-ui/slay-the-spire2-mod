using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using HermitMod.Powers;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;

namespace HermitMod.Relics
{
    public sealed class OldLocket : HermitRelic
    {
        public override RelicRarity Rarity => RelicRarity.Starter;

        private bool _firstTurn;

        public override Task BeforeCombatStart()
        {
            this._firstTurn = true;
            return Task.CompletedTask;
        }

        public override async Task BeforeSideTurnStart(PlayerChoiceContext choiceContext, CombatSide side, CombatState combatState)
        {
            if (side == CombatSide.Player && this._firstTurn)
            {
                this._firstTurn = false;
                await CommonActions.Apply < StrengthPower > (choiceContext, this.Owner.Creature, null, 1m);
                await CommonActions.Apply < DexterityPower > (choiceContext, this.Owner.Creature, null, 1m);
                await CommonActions.Apply < BitPower > (choiceContext, this.Owner.Creature, null, 1m);
            }
        }

        public override Task AfterCombatEnd(CombatRoom _)
        {
            this._firstTurn = true;
            return Task.CompletedTask;
        }

        // 棱彩宝石效果：修改卡牌奖励的卡池，包含所有角色卡池
        public override CardCreationOptions ModifyCardRewardCreationOptions(Player player, CardCreationOptions options)
        {
            // 收集所有角色的卡池
            var allPools = new List<CardPoolModel>();

            foreach (var character in ModelDb.AllCharacters)
            {
                if (character == null) continue;

                var pool = character.CardPool;
                if (pool != null && !allPools.Contains(pool))
                {
                    allPools.Add(pool);
                }
            }

            // 如果没有其他角色卡池，返回原选项
            if (allPools.Count <= 1) return options;

            // 使用 WithCardPools 设置合并后的卡池
            return options.WithCardPools(allPools);
        }
    }
}