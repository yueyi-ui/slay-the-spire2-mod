using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
using BaseLib.Abstracts;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using WeijiaMod.Character;
using WeijiaMod.Powers;

namespace WeijiaMod.Cards
{
    /// <summary>
    /// 能量奔涌 - 1费技能。获得[1]能量，抽1张牌。
    /// 若持有比特，额外获得[1]能量。
    /// 升级：能量+1。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaEnergySurge : WeijiaCard
    {
        public WeijiaEnergySurge() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            var player = this.Owner as Player;
            if (player != null)
            {
                decimal energy = 1m;
                if (WeijiaBits.Get(this.Owner) > 0) energy += 1m;
                if (IsUpgraded) energy += 1m;
                await PlayerCmd.GainEnergy(energy, player);
            }
            await CardPileCmd.Draw(ctx, 1, this.Owner.Creature.Player);
        }

        protected override void OnUpgrade() { }
    }

    /// <summary>
    /// 专注思维 - 1费能力。每回合开始抽1张牌。
    /// 若持有专注，改为抽2张。
    /// 升级：1费→0费。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaFocusedMind : WeijiaCard
    {
        public WeijiaFocusedMind() : base(1, CardType.Power, CardRarity.Rare, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await PowerCmd.Apply<FocusedMindPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }

        protected override void OnUpgrade() { base.EnergyCost.UpgradeBy(-1); }
    }

    /// <summary>
    /// 缩小弹 - 1费攻击。造成6(9)点伤害，施加1层缩小。
    /// 若目标有青肿，额外施加1层缩小。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaTinyBullet : WeijiaCard
    {
        public WeijiaTinyBullet() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(6m, ValueProp.Move) };

        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);

            decimal shrinkLayers = 1m;
            var bruise = play.Target.Powers.OfType<BruisePower>().FirstOrDefault();
            if (bruise != null && bruise.Amount > 0m) shrinkLayers += 1m;
            if (IsUpgraded) shrinkLayers += 1m;
            await PowerCmd.Apply<MegaCrit.Sts2.Core.Models.Powers.ShrinkPower>(ctx, play.Target, shrinkLayers, this.Owner.Creature, this, false);
        }

        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    /// <summary>
    /// 背水一战 - 0费技能。消耗。每消耗1张手牌获得2格挡和1点比特。
    /// 升级：每消耗1张手牌获得3格挡和1点比特。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaLastStand : WeijiaCard
    {
        public WeijiaLastStand() : base(0, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            var player = this.Owner.Creature.Player;
            var hand = PileType.Hand.GetPile(player).Cards.ToList();
            if (hand == null || hand.Count == 0) return;

            var cardsToExhaust = hand.Where(c => c != this).ToList();
            int count = 0;
            foreach (var card in cardsToExhaust)
            {
                await CardCmd.Exhaust(ctx, card, false, false);
                count++;
            }

            decimal blockPerCard = IsUpgraded ? 3m : 2m;
            if (count > 0)
            {
                await CreatureCmd.GainBlock(this.Owner.Creature, blockPerCard * count, ValueProp.Move, null);
                WeijiaBits.Gain(this.Owner, count);
            }
        }

        protected override void OnUpgrade() { }
    }

    /// <summary>
    /// 快速换弹 - 1费技能。获得2点比特，抽1张牌。
    /// 若持有坚韧，额外获得1点比特。
    /// 升级：额外抽1张牌。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaQuickReload : WeijiaCard
    {
        public WeijiaQuickReload() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            int bits = 2;
            var rugged = this.Owner.Creature.Powers.OfType<RuggedPower>().FirstOrDefault();
            if (rugged != null && rugged.Amount > 0m) bits += 1;
            WeijiaBits.Gain(this.Owner, bits);

            int draws = 1;
            if (IsUpgraded) draws += 1;
            await CardPileCmd.Draw(ctx, draws, this.Owner.Creature.Player);
        }

        protected override void OnUpgrade() { }
    }

    /// <summary>
    /// 连射 - 1费攻击。造成8(11)点伤害。
    /// 下回合开始时获得[2]能量。
    /// 升级：下回合获得[3]能量。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaDoubleTap : WeijiaCard
    {
        public WeijiaDoubleTap() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(8m, ValueProp.Move) };

        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);

            decimal nextEnergy = IsUpgraded ? 3m : 2m;
            await PowerCmd.Apply<DoubleTapPower>(ctx, this.Owner.Creature, nextEnergy, this.Owner.Creature, this, false);
        }

        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }
}