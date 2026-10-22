using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WeijiaMod.Character;
using WeijiaMod.Powers;

namespace WeijiaMod.Cards
{
    /// <summary>
    /// 青肿收割 - 1费，消耗自身，对所有敌人施加1(2)层青肿，抽2(3)张牌。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaBruiseHarvest : WeijiaCard
    {
        public WeijiaBruiseHarvest() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal layers = IsUpgraded ? 2m : 1m;
            int draws = IsUpgraded ? 3 : 2;

            var combatState = this.CombatState;
            if (combatState != null)
            {
                var enemies = combatState.Creatures
                    .Where(e => e.IsAlive && e.Side != this.Owner.Creature.Side);

                foreach (var target in enemies)
                {
                    await PowerCmd.Apply<BruisePower>(ctx, target, layers, this.Owner.Creature, this, false);
                }
            }

            await CardPileCmd.Draw(ctx, draws, this.Owner.Creature.Player);
        }

        protected override void OnUpgrade() { }
    }
}
