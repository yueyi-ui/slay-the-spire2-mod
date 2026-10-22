using System;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using WeijiaMod.Character;
using WeijiaMod.Powers;

namespace WeijiaMod.Cards
{
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaDivineFusion : WeijiaCard
    {
        public WeijiaDivineFusion() : base(0, CardType.Skill, CardRarity.Rare, TargetType.None)
        {
            WeijiaMod.UI.BitCostRegistry.SetBitCost(this, 3);
        }

        protected override void OnUpgrade()
        {
        }

        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            var playerCreature = this.Owner.Creature;
            var combatState = this.CombatState;
            if (combatState == null) return;

            // 比特费用（CanPlay 已确保足够）
            WeijiaBits.Spend(this.Owner, 3);

            decimal duration = IsUpgraded ? 2m : 1m;

            var enemies = combatState.Creatures
                .Where(e => e.IsAlive && e.Side != playerCreature.Side);

            foreach (var target in enemies)
            {
                await PowerCmd.Apply<WeakPower>(ctx, target, duration, playerCreature, this, false);
                await PowerCmd.Apply<VulnerablePower>(ctx, target, duration, playerCreature, this, false);
                await PowerCmd.Apply<MegaCrit.Sts2.Core.Models.Powers.ShrinkPower>(ctx, target, duration, playerCreature, this, false);
            }
        }
    }
}
