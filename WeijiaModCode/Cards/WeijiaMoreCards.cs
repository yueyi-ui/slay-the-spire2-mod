using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BaseLib.Utils;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.ValueProps;
using WeijiaMod.Character;
using WeijiaMod.Powers;

namespace WeijiaMod.Cards
{
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaDetermination : WeijiaCard
    {
        public WeijiaDetermination() : base(2, CardType.Power, CardRarity.Rare, TargetType.Self) { }
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(2m).FromCard(this).Targeting(this.Owner.Creature).Execute(ctx);
            await PowerCmd.Apply<FocusPower>(ctx, this.Owner.Creature, 3m, this.Owner.Creature, this, false);
        }
        protected override void OnUpgrade() { base.EnergyCost.UpgradeBy(-1); }
    }

    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaBlackWind : WeijiaCard
    {
        public WeijiaBlackWind() : base(2, CardType.Attack, CardRarity.Rare, TargetType.AllEnemies) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(12m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            decimal dmg = base.DynamicVars.Damage.BaseValue;
            if (WeijiaBits.Get(this.Owner) >= 2) dmg += 6m;
            if (IsUpgraded) dmg += 4m;
            await DamageCmd.Attack(dmg).FromCard(this).TargetingAllOpponents(this.CombatState).Execute(ctx);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(4m); }
    }

    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaManifest : WeijiaCard
    {
        public WeijiaManifest() : base(1, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(8m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            WeijiaBits.Gain(this.Owner, 1);
            await CardPileCmd.Draw(ctx, 1, this.Owner.Creature.Player);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(3m); }
    }

    /// <summary>
    /// 亲属卡 - 找亲戚要生活费！战斗中获得家庭支援。
    /// 未升级: 获得 20 金币。若金币少于 10，获得 100 金币。
    /// 升级后: 获得 25 金币。若金币少于 20，获得 100 金币。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaFamilyCard : WeijiaCard
    {
        private decimal _goldAmount = 20m;
        private decimal _poorThreshold = 10m;

        public WeijiaFamilyCard() : base(1, CardType.Power, CardRarity.Uncommon, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            var player = this.Owner as Player;
            if (player != null)
            {
                decimal finalGold = player.Gold < _poorThreshold ? 100m : _goldAmount;
                await PlayerCmd.GainGold(finalGold, player);
            }
        }

        protected override void OnUpgrade()
        {
            _goldAmount = 25m;
            _poorThreshold = 20m;
            base.EnergyCost.UpgradeBy(-1);
        }
    }
}

