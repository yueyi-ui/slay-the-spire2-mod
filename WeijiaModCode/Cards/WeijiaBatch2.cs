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
    // 适应 - 造成伤害。获得 1(2) 层适应（每回合获得格挡）。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaAdapt : WeijiaCard
    {
        public WeijiaAdapt() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(8m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await PowerCmd.Apply<AdaptPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(4m); }
    }

    // 重击 - 造成伤害。获得 1 点比特。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaBigShot : WeijiaCard
    {
        public WeijiaBigShot() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(16m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            WeijiaBits.Gain(this.Owner, 1);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(6m); }
    }

    // 连击 - 造成 3(4) 次伤害。若持有比特 >= 3，额外 1 次。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaCombo : WeijiaCard
    {
        public WeijiaCombo() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(4m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            int hits = 3;
            if (WeijiaBits.Get(this.Owner) >= 3) hits = 4;
            if (IsUpgraded) hits += 1;
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(hits).FromCard(this).Targeting(play.Target).Execute(ctx);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(2m); }
    }

    // 亡命之徒 - 造成伤害。获得 1 层坚韧。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaDesperado : WeijiaCard
    {
        public WeijiaDesperado() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(10m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await PowerCmd.Apply<RuggedPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(4m); }
    }

    // 瓦解 - 造成伤害。若目标有青肿，消耗所有青肿并每层造成 8 点额外伤害。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaDissolve : WeijiaCard
    {
        public WeijiaDissolve() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(6m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal extra = 0m;
            var bruise = play.Target.Powers.OfType<BruisePower>().FirstOrDefault();
            if (bruise != null && bruise.Amount > 0m)
            {
                // 固定消耗最多 3 层青肿
                decimal consume = Math.Min(bruise.Amount, 3m);
                decimal perLayer = IsUpgraded ? 8m : 6m;
                extra = consume * perLayer;
                int draws = IsUpgraded ? Convert.ToInt32(consume) : Convert.ToInt32(consume / 2m);
                await PowerCmd.Apply<BruisePower>(ctx, play.Target, -consume, this.Owner.Creature, this, false);
                if (draws > 0)
                    await CardPileCmd.Draw(ctx, draws, this.Owner.Creature.Player);
            }
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue + extra).FromCard(this).Targeting(play.Target).Execute(ctx);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    // 俯冲 - 造成伤害。获得 1 点比特。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaDive : WeijiaCard
    {
        public WeijiaDive() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(7m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            WeijiaBits.Gain(this.Owner, 1);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    // 虚弱 - 造成伤害。施加 1 层青肿。获得 1 点比特。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaEnervate : WeijiaCard
    {
        public WeijiaEnervate() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(6m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await PowerCmd.Apply<BruisePower>(ctx, play.Target, 1m, this.Owner.Creature, this, false);
            WeijiaBits.Gain(this.Owner, 1);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    // 最后冲刺 - 造成伤害。获得 2 点比特。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaFinalCanter : WeijiaCard
    {
        public WeijiaFinalCanter() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(14m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            WeijiaBits.Gain(this.Owner, 2);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(4m); }
    }

    // 闪光粉 - 对所有敌人造成 2 次伤害。获得 1 点比特。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaFlashPowder : WeijiaCard
    {
        public WeijiaFlashPowder() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(5m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(2).FromCard(this).TargetingAllOpponents(this.CombatState).Execute(ctx);
            WeijiaBits.Gain(this.Owner, 1);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(2m); }
    }

    // 来自彼岸 - 造成伤害。获得 1 层适应和 1 层专注。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaFromBeyond : WeijiaCard
    {
        public WeijiaFromBeyond() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(18m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await PowerCmd.Apply<WeijiaMod.Powers.FocusPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(6m); }
    }

    // 险棋 - 造成伤害。对自己造成 2 点伤害。获得 1 层专注。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaGambit : WeijiaCard
    {
        public WeijiaGambit() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(8m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await DamageCmd.Attack(2m).FromCard(this).Targeting(this.Owner.Creature).Execute(ctx);
            await PowerCmd.Apply<WeijiaMod.Powers.FocusPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(4m); }
    }

    // 黄金子弹 - 造成伤害。固定消耗 1 比特，每持有 1 点比特（消耗前）额外 +4(6) 伤。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaGoldenBullet : WeijiaCard
    {
        public WeijiaGoldenBullet() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) {
            WeijiaMod.UI.BitCostRegistry.SetBitCost(this, 1); }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(8m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            // 比特费用（CanPlay 已确保足够）
            WeijiaBits.Spend(this.Owner, 1);
            int bits = WeijiaBits.Get(this.Owner);
            decimal perBit = IsUpgraded ? 6m : 4m;
            decimal extra = bits * perBit;
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue + extra).FromCard(this).Targeting(play.Target).Execute(ctx);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }
    // 积怨 - 造成伤害。若目标已有青肿，额外造成 5 点伤害。施加 1 层青肿。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaGrudge : WeijiaCard
    {
        public WeijiaGrudge() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(7m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal bonus = 0m;
            var bruise = play.Target.Powers.OfType<BruisePower>().FirstOrDefault();
            if (bruise != null && bruise.Amount > 0m) bonus = 5m;
            decimal bruiseLayers = (bruise != null) ? bruise.Amount : 0m;
            int energyThreshold = IsUpgraded ? 2 : 3;
            if (bruiseLayers >= energyThreshold)
            {
                var player = this.Owner as Player;
                if (player != null)
                    await PlayerCmd.GainEnergy(1m, player);
            }
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue + bonus).FromCard(this).Targeting(play.Target).Execute(ctx);
            await PowerCmd.Apply<BruisePower>(ctx, play.Target, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    // 大口径 - 造成伤害。施加 1 层青肿。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaHighCaliber : WeijiaCard
    {
        public WeijiaHighCaliber() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(12m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).TargetingAllOpponents(this.CombatState).Execute(ctx);
            decimal layers = IsUpgraded ? 3m : 2m;
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
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(4m); }
    }

    // 正午 - 对所有敌人造成伤害。若持有专注，额外 1 次。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaHighNoon : WeijiaCard
    {
        public WeijiaHighNoon() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(10m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            int hits = 2;
            var focus = this.Owner.Creature.Powers.OfType<FocusPower>().FirstOrDefault();
            if (focus != null && focus.Amount > 0m) hits = 3;
            if (IsUpgraded) hits += 1;
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(hits).FromCard(this).TargetingAllOpponents(this.CombatState).Execute(ctx);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(4m); }
    }

    // 马格南 - 造成伤害。若持有比特，消耗 1 层获得 6(8) 额外伤害。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaMagnum : WeijiaCard
    {
        public WeijiaMagnum() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) {
            WeijiaMod.UI.BitCostRegistry.SetBitCost(this, 1); }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(22m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            // 比特费用（CanPlay 已确保足够）
            WeijiaBits.Spend(this.Owner, 1);
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(6m); }
    }
    // 维护 - 造成伤害。获得 1 层适应。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaMaintenance : WeijiaCard
    {
        public WeijiaMaintenance() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(6m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await PowerCmd.Apply<AdaptPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    // 恶意 - 造成伤害。获得 1 点比特。若已有比特，额外获得 1 点。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaMalice : WeijiaCard
    {
        public WeijiaMalice() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(8m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
                        decimal bitGain = 1m;
            if (WeijiaBits.Get(this.Owner) > 0) bitGain = 2m;
            WeijiaBits.Gain(this.Owner, (int)bitGain);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    // 午夜 - 造成伤害。施加 1 层青肿。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaMidnight : WeijiaCard
    {
        public WeijiaMidnight() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(16m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await PowerCmd.Apply<BruisePower>(ctx, play.Target, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(6m); }
    }

    // 炼狱 - 造成伤害。若持有专注，获得 1 层适应。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaPurgatory : WeijiaCard
    {
        public WeijiaPurgatory() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(14m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            var focus = this.Owner.Creature.Powers.OfType<WeijiaMod.Powers.FocusPower>().FirstOrDefault();
            if (focus != null && focus.Amount > 0m)
                await PowerCmd.Apply<AdaptPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(4m); }
    }

    // 缓刑 - 获得格挡。获得 1(2) 层专注。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaReprieve : WeijiaCard
    {
        public WeijiaReprieve() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new BlockVar(10m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal extra = 0m;
            var focus = this.Owner.Creature.Powers.OfType<WeijiaMod.Powers.FocusPower>().FirstOrDefault();
            int focusThreshold = IsUpgraded ? 2 : 3;
            decimal bonusBlock = IsUpgraded ? 8m : 5m;
            if (focus != null && focus.Amount >= focusThreshold)
                extra = bonusBlock;
            await CreatureCmd.GainBlock(this.Owner.Creature, base.DynamicVars.Block.BaseValue + extra, ValueProp.Move, null);
            await PowerCmd.Apply<WeijiaMod.Powers.FocusPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Block.UpgradeValueBy(4m); }
    }

    // 暗影斗篷 - 获得格挡。获得 1 层坚韧。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaShadowCloak : WeijiaCard
    {
        public WeijiaShadowCloak() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new BlockVar(15m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await CreatureCmd.GainBlock(this.Owner.Creature, base.DynamicVars.Block.BaseValue, ValueProp.Move, null);
            await PowerCmd.Apply<RuggedPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Block.UpgradeValueBy(5m); }
    }

    // 决战 - 获得大量格挡。若持有比特，每层额外获得 2 点格挡。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaShowdown : WeijiaCard
    {
        public WeijiaShowdown() : base(2, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new BlockVar(18m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal extra = 0m;
            if (WeijiaBits.Get(this.Owner) > 0) extra = WeijiaBits.Get(this.Owner) * 2m;
            await CreatureCmd.GainBlock(this.Owner.Creature, base.DynamicVars.Block.BaseValue + extra, ValueProp.Move, null);
        }
        protected override void OnUpgrade() { base.DynamicVars.Block.UpgradeValueBy(6m); }
    }

    // 快照 - 获得格挡。抽 1 张牌。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaSnapshot : WeijiaCard
    {
        public WeijiaSnapshot() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new BlockVar(8m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal block = base.DynamicVars.Block.BaseValue;
            await CreatureCmd.GainBlock(this.Owner.Creature, block, ValueProp.Move, null);
            await CardPileCmd.Draw(ctx, 1, this.Owner.Creature.Player);
            int energyThreshold = IsUpgraded ? 8 : 10;
            if (block >= energyThreshold)
            {
                var player = this.Owner as Player;
                if (player != null)
                    await PlayerCmd.GainEnergy(1m, player);
            }
        }
        protected override void OnUpgrade() { base.DynamicVars.Block.UpgradeValueBy(3m); }
    }

    // 稳健打击 - 造成伤害。获得 1 层坚韧。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaSteadyStrike : WeijiaCard
    {
        public WeijiaSteadyStrike() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(8m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await PowerCmd.Apply<RuggedPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    // 美德 - 获得格挡。获得 1 层专注。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaVirtue : WeijiaCard
    {
        public WeijiaVirtue() : base(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new BlockVar(9m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await CreatureCmd.GainBlock(this.Owner.Creature, base.DynamicVars.Block.BaseValue, ValueProp.Move, null);
            await PowerCmd.Apply<WeijiaMod.Powers.FocusPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Block.UpgradeValueBy(4m); }
    }

    // 大开眼界 - 对所有敌人造成 2 次伤害。若持有比特，伤害+2。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaWideOpen : WeijiaCard
    {
        public WeijiaWideOpen() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AllEnemies) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(7m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal dmg = base.DynamicVars.Damage.BaseValue;
            if (WeijiaBits.Get(this.Owner) > 0) dmg += 2m;
            if (IsUpgraded) dmg += 2m;
            await DamageCmd.Attack(dmg).WithHitCount(2).FromCard(this).TargetingAllOpponents(this.CombatState).Execute(ctx);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(2m); }
    }

    // 凝聚 - 造成伤害。获得 1 点比特。若比特 >= 3，获得 1 层专注。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaCoalescence : WeijiaCard
    {
        public WeijiaCoalescence() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(6m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            WeijiaBits.Gain(this.Owner, 1);
            if (WeijiaBits.Get(this.Owner) >= 3)
                await PowerCmd.Apply<WeijiaMod.Powers.FocusPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    // 诅咒武器 - 造成伤害。对自己造成 3 点伤害。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaCursedWeapon : WeijiaCard
    {
        public WeijiaCursedWeapon() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(18m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            await DamageCmd.Attack(3m).FromCard(this).Targeting(this.Owner.Creature).Execute(ctx);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(6m); }
    }

    // 死人之手 - 2费攻击。固定消耗 2 比特，造成高额伤害。
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaDeadMansHand : WeijiaCard
    {
        public WeijiaDeadMansHand() : base(2, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) {
            WeijiaMod.UI.BitCostRegistry.SetBitCost(this, 2); }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(23m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            // 比特费用（CanPlay 已确保足够）
            WeijiaBits.Spend(this.Owner, 2);
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(5m); }
    }
}
