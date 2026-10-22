using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BaseLib.Utils;
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
    /// 版号迭代 - 1费攻击。造成伤害，消耗 1 比特，每持有 1 点比特额外造成 3 点伤害（消耗前）。
    /// 参考 Regent 星标卡：固定消耗 + 基于资源量的加成。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaVersionBump : WeijiaCard
    {
        public WeijiaVersionBump() : base(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
        {
            WeijiaMod.UI.BitCostRegistry.SetBitCost(this, 1);
        }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(6m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            // 比特费用（CanPlay 已确保足够）
            WeijiaBits.Spend(this.Owner, 1);
            // 比特费用由游戏自动扣除（CustomResource 系统）
            int bits = WeijiaBits.Get(this.Owner);
            decimal extra = bits * 3m;
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue + extra).FromCard(this).Targeting(play.Target).Execute(ctx);
            int draws = IsUpgraded ? 2 : 1;
            await CardPileCmd.Draw(ctx, draws, this.Owner.Creature.Player);
        }
        protected override void OnUpgrade()
        {
            base.DynamicVars.Damage.UpgradeValueBy(3m);
        }
    }
}
