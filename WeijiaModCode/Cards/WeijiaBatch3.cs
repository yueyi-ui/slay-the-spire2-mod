using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WeijiaMod.Character;
using WeijiaMod.Powers;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Creatures;
using BaseLib.Utils;
using BaseLib.Abstracts;

namespace WeijiaMod.Cards
{
    // 死或生 - 造成大量伤害。获得 1 层专注。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaDeadOrAlive : WeijiaCard
    {
        public WeijiaDeadOrAlive() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(24m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await PowerCmd.Apply<WeijiaMod.Powers.FocusPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(6m); }
    }

    // 永恒形态 - 能力。获得 2(3) 层专注。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaEternalForm : WeijiaCard
    {
        public WeijiaEternalForm() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self) { }
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await PowerCmd.Apply<WeijiaMod.Powers.FocusPower>(ctx, this.Owner.Creature, 3m, this.Owner.Creature, this, false);
            await PowerCmd.Apply<EternalFormPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.EnergyCost.UpgradeBy(-1); }
    }

    // 风暴之眼 - 对所有敌人造成 2 次伤害。若持有比特，额外 1 次。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaEyeOfTheStorm : WeijiaCard
    {
        public WeijiaEyeOfTheStorm() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(10m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            int hits = 2;
            if (WeijiaBits.Get(this.Owner) > 0) hits = 3;
            if (IsUpgraded) hits += 1;
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(hits).FromCard(this).TargetingAllOpponents(this.CombatState).Execute(ctx);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    // 致命欲望 - 造成伤害。失去 3 点生命。获得 2(3) 点比特。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaFatalDesire : WeijiaCard
    {
        public WeijiaFatalDesire() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(12m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await DamageCmd.Attack(3m).FromCard(this).Targeting(this.Owner.Creature).Execute(ctx);
            WeijiaBits.Gain(this.Owner, 2);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(4m); }
    }

    // 满载弹药 - 造成伤害。获得 3 点比特。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaFullyLoaded : WeijiaCard
    {
        public WeijiaFullyLoaded() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(10m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            int bits = IsUpgraded ? 3 : 2;
            int draws = IsUpgraded ? 2 : 1;
            WeijiaBits.Gain(this.Owner, bits);
            await CardPileCmd.Draw(ctx, draws, this.Owner.Creature.Player);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(4m); }
    }

    // 完形 - 能力。获得 1(2) 层适应和 1(2) 层专注。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaGestalt : WeijiaCard
    {
        public WeijiaGestalt() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self) { }
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await PowerCmd.Apply<AdaptPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
            await PowerCmd.Apply<WeijiaMod.Powers.FocusPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.EnergyCost.UpgradeBy(-1); }
    }

    // 鬼影 - 获得大量格挡。获得 1 层坚韧。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaGhostlyPresence : WeijiaCard
    {
        public WeijiaGhostlyPresence() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new BlockVar(20m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await CreatureCmd.GainBlock(this.Owner.Creature, base.DynamicVars.Block.BaseValue, ValueProp.Move, null);
            await PowerCmd.Apply<RuggedPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Block.UpgradeValueBy(6m); }
    }

    // 英勇虚张 - 造成伤害。获得 2 层专注。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaHeroicBravado : WeijiaCard
    {
        public WeijiaHeroicBravado() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(18m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await PowerCmd.Apply<WeijiaMod.Powers.FocusPower>(ctx, this.Owner.Creature, 2m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(6m); }
    }

    // 恐怖 - 对所有敌人造成伤害。获得 1 点比特。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaHorror : WeijiaCard
    {
        public WeijiaHorror() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(15m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(2).FromCard(this).TargetingAllOpponents(this.CombatState).Execute(ctx);
            WeijiaBits.Gain(this.Owner, 1);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(5m); }
    }

    // 末日降临 - 造成毁灭性伤害。若比特 >= 3，额外获得 1 层专注。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaImpendingDoom : WeijiaCard
    {
        public WeijiaImpendingDoom() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(30m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
                        if (WeijiaBits.Get(this.Owner) >= 3)
                await PowerCmd.Apply<WeijiaMod.Powers.FocusPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(8m); }
    }

    // 扣动扳机 - 造成伤害。获得 1 层适应。抽 1 张牌。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaItchyTrigger : WeijiaCard
    {
        public WeijiaItchyTrigger() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(10m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await PowerCmd.Apply<AdaptPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
            await CardPileCmd.Draw(ctx, 1, this.Owner.Creature.Player);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(4m); }
    }

    // 独狼 - 造成伤害。获得 1 层坚韧。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaLoneWolf : WeijiaCard
    {
        public WeijiaLoneWolf() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(22m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await PowerCmd.Apply<RuggedPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(6m); }
    }

    // 抽签运气 - 造成伤害。抽 2 张牌。未升级时消耗。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaLuckOfTheDraw : WeijiaCard
    {
        public WeijiaLuckOfTheDraw() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(8m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await CardPileCmd.Draw(ctx, 2, this.Owner.Creature.Player);
            await CardPileCmd.Draw(ctx, 2, this.Owner.Creature.Player);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(4m); }
    }

    // 纪念物 - 获得大量格挡。获得 1 层专注。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaMementoCard : WeijiaCard
    {
        public WeijiaMementoCard() : base(2, CardType.Skill, CardRarity.Rare, TargetType.Self) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new BlockVar(20m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await CreatureCmd.GainBlock(this.Owner.Creature, base.DynamicVars.Block.BaseValue, ValueProp.Move, null);
            await PowerCmd.Apply<WeijiaMod.Powers.FocusPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Block.UpgradeValueBy(6m); }
    }

    // 毫无保留 - 造成伤害。固定消耗 2 比特，每持有 1 点比特（消耗前）额外 +6(8) 伤。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaNoHoldsBarred : WeijiaCard
    {
        public WeijiaNoHoldsBarred() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) {
            WeijiaMod.UI.BitCostRegistry.SetBitCost(this, 2); }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(12m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            // 比特费用（CanPlay 已确保足够）
            WeijiaBits.Spend(this.Owner, 2);
            int bits = WeijiaBits.Get(this.Owner);
            decimal perBit = IsUpgraded ? 8m : 6m;
            decimal extra = bits * perBit;
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue + extra).FromCard(this).Targeting(play.Target).Execute(ctx);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(4m); }
    }
    // 压倒力量 - 造成巨大伤害。失去 5(3) 点生命。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaOverwhelmingPower : WeijiaCard
    {
        public WeijiaOverwhelmingPower() : base(3, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(35m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await DamageCmd.Attack(5m).FromCard(this).Targeting(this.Owner.Creature).Execute(ctx);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(9m); }
    }

    // 回旋踢 - 造成伤害。施加 2(3) 层青肿。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaRoundhouseKick : WeijiaCard
    {
        public WeijiaRoundhouseKick() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(20m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await PowerCmd.Apply<BruisePower>(ctx, play.Target, 2m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(6m); }
    }
}
