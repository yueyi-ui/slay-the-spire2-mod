using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WeijiaMod.Character;
using WeijiaMod.Powers;
using BaseLib.Utils;
using BaseLib.Abstracts;

namespace WeijiaMod.Cards
{
    /// <summary>
    /// 快速拔枪 - 造成伤害，抽1张牌。若持有比特，额外抽1张牌。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaQuickDraw : WeijiaCard
    {
        public WeijiaQuickDraw() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(8m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            int draws = 1;
            if (WeijiaBits.Get(this.Owner) > 0) draws = 2;
            if (IsUpgraded) draws += 1;
            await CardPileCmd.Draw(ctx, draws, this.Owner.Creature.Player);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    /// <summary>
    /// 爆头 - 造成伤害，消耗目标最多 2 层青肿，每层青肿造成额外伤害并抽1张牌。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaHeadshot : WeijiaCard
    {
        public WeijiaHeadshot() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(8m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal bonus = 0m;
            var bruise = play.Target.Powers.OfType<BruisePower>().FirstOrDefault();
            if (bruise != null && bruise.Amount > 0m)
            {
                // 固定消耗最多 2 层青肿
                decimal consume = Math.Min(bruise.Amount, 2m);
                decimal perLayer = IsUpgraded ? 6m : 4m;
                bonus = consume * perLayer;
                await PowerCmd.Apply<BruisePower>(ctx, play.Target, -consume, this.Owner.Creature, this, false);
            }
            int draws = IsUpgraded ? 2 : 1;
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue + bonus).FromCard(this).Targeting(play.Target).Execute(ctx);
            await CardPileCmd.Draw(ctx, draws, this.Owner.Creature.Player);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    /// <summary>
    /// 精准射击 - 造成伤害，施加青肿。若已有专注，额外施加青肿。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaCalledShot : WeijiaCard
    {
        public WeijiaCalledShot() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(5m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            decimal layers = IsUpgraded ? 3m : 2m;
            var focus = this.Owner.Creature.Powers.OfType<FocusPower>().FirstOrDefault();
            if (focus != null && focus.Amount > 0m) layers += 1m;
            await PowerCmd.Apply<BruisePower>(ctx, play.Target, layers, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(2m); }
    }

    /// <summary>
    /// 神枪手 - 造成2次伤害，获得1点比特。若持有比特，改为3次伤害。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaDeadeye : WeijiaCard
    {
        public WeijiaDeadeye() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(5m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            int hits = WeijiaBits.Get(this.Owner) > 0 ? 3 : 2;
            if (IsUpgraded) hits += 1;
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(hits).FromCard(this).Targeting(play.Target).Execute(ctx);
            WeijiaBits.Gain(this.Owner, 1);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(2m); }
    }

    /// <summary>
    /// 怒视 - 0费。造成伤害，施加青肿，抽1张牌。若目标已有青肿，额外施加1层。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaGlare : WeijiaCard
    {
        public WeijiaGlare() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(3m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            decimal layers = IsUpgraded ? 2m : 1m;
            var bruise = play.Target.Powers.OfType<BruisePower>().FirstOrDefault();
            if (bruise != null && bruise.Amount > 0m) layers += 1m;
            await PowerCmd.Apply<BruisePower>(ctx, play.Target, layers, this.Owner.Creature, this, false);
            await CardPileCmd.Draw(ctx, 1, this.Owner.Creature.Player);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(2m); }
    }

    /// <summary>
    /// 枪托猛击 - 0费。造成伤害，获得格挡，抽牌。若持有坚韧，格挡翻倍。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaPistolWhip : WeijiaCard
    {
        public WeijiaPistolWhip() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(4m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal dmg = IsUpgraded ? 6m : 4m;
            decimal block = IsUpgraded ? 6m : 4m;
            int draws = IsUpgraded ? 2 : 1;
            var rugged = this.Owner.Creature.Powers.OfType<RuggedPower>().FirstOrDefault();
            if (rugged != null && rugged.Amount > 0m) block *= 2m;
            await DamageCmd.Attack(dmg).FromCard(this).Targeting(play.Target).Execute(ctx);
            await CreatureCmd.GainBlock(this.Owner.Creature, block, ValueProp.Move, null);
            await CardPileCmd.Draw(ctx, draws, this.Owner.Creature.Player);
        }
        protected override void OnUpgrade() { }
    }

    /// <summary>
    /// 作弊 - 0费。造成伤害，获得1点比特。若目标有青肿，额外获得1点比特。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaCheat : WeijiaCard
    {
        public WeijiaCheat() : base(0, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(3m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            int bits = 1;
            var bruise = play.Target.Powers.OfType<BruisePower>().FirstOrDefault();
            if (bruise != null && bruise.Amount > 0m) bits = 2;
            if (IsUpgraded) bits += 1;
            WeijiaBits.Gain(this.Owner, bits);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    /// <summary>
    /// 贪婪 - 造成伤害，固定消耗 1 点比特，获得1点比特。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaCovet : WeijiaCard
    {
        public WeijiaCovet() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) {
            WeijiaMod.UI.BitCostRegistry.SetBitCost(this, 1); }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(12m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            // 比特费用（CanPlay 已确保足够）
            WeijiaBits.Spend(this.Owner, 1);
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            WeijiaBits.Gain(this.Owner, 1);
            if (IsUpgraded) await CardPileCmd.Draw(ctx, 1, this.Owner.Creature.Player);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }
    /// <summary>
    /// 狙击 - 造成伤害。若目标有青肿，每层青肿额外造成3点伤害（不消耗）。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaSnipe : WeijiaCard
    {
        public WeijiaSnipe() : base(2, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(15m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal bonus = 0m;
            var bruise = play.Target.Powers.OfType<BruisePower>().FirstOrDefault();
            if (bruise != null && bruise.Amount > 0m) bonus = bruise.Amount * 3m;
            if (IsUpgraded) bonus += 5m;
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue + bonus).FromCard(this).Targeting(play.Target).Execute(ctx);
            WeijiaBits.Gain(this.Owner, 1);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(5m); }
    }

    /// <summary>
    /// 瞄准 - 施加青肿，抽1张牌。若持有专注，额外施加1层青肿。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaTakeAim : WeijiaCard
    {
        public WeijiaTakeAim() : base(1, CardType.Skill, CardRarity.Common, TargetType.AnyEnemy) { }
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal layers = 2m;
            var focus = this.Owner.Creature.Powers.OfType<FocusPower>().FirstOrDefault();
            if (focus != null && focus.Amount > 0m) layers += 1m;
            if (IsUpgraded) layers += 1m;
            await PowerCmd.Apply<BruisePower>(ctx, play.Target, layers, this.Owner.Creature, this, false);
            await CardPileCmd.Draw(ctx, 1, this.Owner.Creature.Player);
        }
        protected override void OnUpgrade() { base.EnergyCost.UpgradeBy(-1); }
    }

    /// <summary>
    /// 佯攻 - 获得格挡，获得1层专注。若持有比特，额外获得1层专注。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaFeint : WeijiaCard
    {
        public WeijiaFeint() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new BlockVar(5m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await CreatureCmd.GainBlock(this.Owner.Creature, base.DynamicVars.Block.BaseValue, ValueProp.Move, null);
            decimal focusLayers = 1m;
            if (WeijiaBits.Get(this.Owner) > 0) focusLayers = 2m;
            if (IsUpgraded) focusLayers += 1m;
            await PowerCmd.Apply<FocusPower>(ctx, this.Owner.Creature, focusLayers, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Block.UpgradeValueBy(3m); }
    }

    /// <summary>
    /// 走火 - 造成伤害，对自己造成伤害。获得1层坚韧。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaMisfire : WeijiaCard
    {
        public WeijiaMisfire() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(10m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            decimal selfDmg = IsUpgraded ? 1m : 2m;
            await DamageCmd.Attack(selfDmg).FromCard(this).Targeting(this.Owner.Creature).Execute(ctx);
            await PowerCmd.Apply<RuggedPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    /// <summary>
    /// 乱射 - 对所有敌人造成2次伤害。获得比特，抽牌。若持有专注，额外1次伤害。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaSprayPray : WeijiaCard
    {
        public WeijiaSprayPray() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(3m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            int hits = 2;
            var focus = this.Owner.Creature.Powers.OfType<FocusPower>().FirstOrDefault();
            if (focus != null && focus.Amount > 0m) hits = 3;
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(hits).FromCard(this).TargetingAllOpponents(this.CombatState).Execute(ctx);
            int bits = IsUpgraded ? 2 : 1;
            int draws = IsUpgraded ? 2 : 1;
            WeijiaBits.Gain(this.Owner, bits);
            await CardPileCmd.Draw(ctx, draws, this.Owner.Creature.Player);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(1m); }
    }

    /// <summary>
    /// 追踪弹 - 造成伤害。若目标有青肿，额外造成伤害并获得1点比特。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaTrackingShot : WeijiaCard
    {
        public WeijiaTrackingShot() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(6m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal bonus = 0m;
            var bruise = play.Target.Powers.OfType<BruisePower>().FirstOrDefault();
            if (bruise != null && bruise.Amount > 0m)
            {
                bonus = IsUpgraded ? 5m : 3m;
                WeijiaBits.Gain(this.Owner, 1);
            }
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue + bonus).FromCard(this).Targeting(play.Target).Execute(ctx);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    /// <summary>
    /// 优势 - 造成伤害，获得1层专注。若持有比特，伤害+3。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaVantage : WeijiaCard
    {
        public WeijiaVantage() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy)
        {
        }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(6m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal dmg = base.DynamicVars.Damage.BaseValue;
            if (WeijiaBits.Get(this.Owner) > 0) dmg += 3m;
            await DamageCmd.Attack(dmg).FromCard(this).Targeting(play.Target).Execute(ctx);
            await PowerCmd.Apply<FocusPower>(ctx, this.Owner.Creature, 1m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    /// <summary>
    /// 斗殴 - 对所有敌人造成2次伤害。若持有比特，额外1次伤害。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaRoughhouse : WeijiaCard
    {
        public WeijiaRoughhouse() : base(2, CardType.Attack, CardRarity.Common, TargetType.AllEnemies) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(10m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            int hits = 2;
            if (WeijiaBits.Get(this.Owner) > 0) hits = 3;
            if (IsUpgraded) hits += 1;
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(hits).FromCard(this).TargetingAllOpponents(this.CombatState).Execute(ctx);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(4m); }
    }

    /// <summary>
    /// 躲藏 - 获得格挡。若持有坚韧，格挡+5。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaHoleUp : WeijiaCard
    {
        public WeijiaHoleUp() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new BlockVar(8m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal block = base.DynamicVars.Block.BaseValue;
            var rugged = this.Owner.Creature.Powers.OfType<RuggedPower>().FirstOrDefault();
            if (rugged != null && rugged.Amount > 0m) block += 5m;
            if (IsUpgraded) block += 3m;
            await CreatureCmd.GainBlock(this.Owner.Creature, block, ValueProp.Move, null);
        }
        protected override void OnUpgrade() { base.DynamicVars.Block.UpgradeValueBy(3m); }
    }

    /// <summary>
    /// 掩护 - 获得格挡，获得1点比特。若持有专注，额外获得1点比特。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaTakeCover : WeijiaCard
    {
        public WeijiaTakeCover() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new BlockVar(6m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await CreatureCmd.GainBlock(this.Owner.Creature, base.DynamicVars.Block.BaseValue, ValueProp.Move, null);
            int bits = 1;
            var focus = this.Owner.Creature.Powers.OfType<FocusPower>().FirstOrDefault();
            if (focus != null && focus.Amount > 0m) bits = 2;
            if (IsUpgraded) bits += 1;
            WeijiaBits.Gain(this.Owner, bits);
        }
        protected override void OnUpgrade() { base.DynamicVars.Block.UpgradeValueBy(3m); }
    }

    /// <summary>
    /// 低调行事 - 获得格挡。若持有专注，每层专注+2格挡。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaLowProfile : WeijiaCard
    {
        public WeijiaLowProfile() : base(1, CardType.Skill, CardRarity.Common, TargetType.Self) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new BlockVar(5m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal extra = 0m;
            var focus = this.Owner.Creature.Powers.OfType<FocusPower>().FirstOrDefault();
            if (focus != null && focus.Amount > 0m) extra = focus.Amount * 2m;
            if (IsUpgraded) extra += 3m;
            await CreatureCmd.GainBlock(this.Owner.Creature, base.DynamicVars.Block.BaseValue + extra, ValueProp.Move, null);
        }
        protected override void OnUpgrade() { base.DynamicVars.Block.UpgradeValueBy(3m); }
    }

    /// <summary>
    /// 护甲 - 获得格挡，获得1层坚韧。若持有适应，额外获得1层坚韧。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaBodyArmor : WeijiaCard
    {
        public WeijiaBodyArmor() : base(2, CardType.Skill, CardRarity.Common, TargetType.Self) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new BlockVar(12m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await CreatureCmd.GainBlock(this.Owner.Creature, base.DynamicVars.Block.BaseValue, ValueProp.Move, null);
            decimal rugged = 1m;
            var adapt = this.Owner.Creature.Powers.OfType<AdaptPower>().FirstOrDefault();
            if (adapt != null && adapt.Amount > 0m) rugged = 2m;
            if (IsUpgraded) rugged += 1m;
            await PowerCmd.Apply<RuggedPower>(ctx, this.Owner.Creature, rugged, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.DynamicVars.Block.UpgradeValueBy(4m); }
    }
}