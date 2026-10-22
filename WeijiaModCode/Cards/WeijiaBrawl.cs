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
    /// 混战 - 对所有敌人造成伤害。若持有比特，额外造成 1 次。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaBrawl : WeijiaCard
    {
        public WeijiaBrawl() : base(1, CardType.Attack, CardRarity.Common, TargetType.AllEnemies) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(6m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            int hits = WeijiaBits.Get(this.Owner) > 0 ? 2 : 1;
            if (IsUpgraded) hits += 1;
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(hits).FromCard(this).TargetingAllOpponents(this.CombatState).Execute(ctx);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(2m); }
    }
}


