using System;
using System.Collections.Generic;
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
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaShortFuse : WeijiaCard
    {
        public WeijiaShortFuse() : base(0, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy) { }
        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(5m, ValueProp.Move) };
        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).FromCard(this).Targeting(play.Target).Execute(ctx);
            int bits = IsUpgraded ? 2 : 1;
            WeijiaBits.Gain(this.Owner, bits);
        }
        protected override void OnUpgrade() { base.DynamicVars.Damage.UpgradeValueBy(2m); }
    }
}


