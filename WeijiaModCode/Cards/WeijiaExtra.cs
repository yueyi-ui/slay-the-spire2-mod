using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WeijiaMod.Character;
using WeijiaMod.Powers;

namespace WeijiaMod.Cards
{
    /// <summary>
    /// 跆拳道踢击 - 踢腿连击！造成多次伤害。若目标有青肿，额外1次。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaTaekwondoKick : WeijiaCard
    {
        public WeijiaTaekwondoKick() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(5m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            int hits = 2;
            var bruise = play.Target.Powers.OfType<BruisePower>().FirstOrDefault();
            if (bruise != null && bruise.Amount > 0m) hits = 3;
            if (IsUpgraded) hits += 1;
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(hits).FromCard(this).Targeting(play.Target).Execute(ctx);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(2m); }
    }

    /// <summary>
    /// 辩论胜利 - 激烈的辩论让你思路更清晰，抽牌并获得专注。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaDebateVictory : WeijiaCard
    {
        public WeijiaDebateVictory() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await CardPileCmd.Draw(ctx, 2, this.Owner.Creature.Player);
            await CardPileCmd.Draw(ctx, 2, this.Owner.Creature.Player);
            await PowerCmd.Apply<FocusPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.EnergyCost.UpgradeBy(-1); }
    }

    /// <summary>
    /// 女娲守护 - 以女娲之名守护自身，获得大量格挡和比特。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaNuwaGuard : WeijiaCard
    {
        public WeijiaNuwaGuard() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new BlockVar(15m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await CreatureCmd.GainBlock(this.Owner.Creature, base.DynamicVars.Block.BaseValue, ValueProp.Move, null);
            WeijiaBits.Gain(this.Owner, 2);
        }
        protected override void OnUpgrade() { base.DynamicVars.Block.UpgradeValueBy(5m); }
    }
}
