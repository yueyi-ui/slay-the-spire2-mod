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
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaKickCombo : WeijiaCard
    {
        public WeijiaKickCombo() : base(1, CardType.Attack, CardRarity.Common, TargetType.AnyEnemy) { }

        protected override IEnumerable<DynamicVar> CanonicalVars => new DynamicVar[] { new DamageVar(4m, ValueProp.Move) };

        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            int hits = 2;
                        if (WeijiaBits.Get(this.Owner) >= 3)
                hits = 3;
            await DamageCmd.Attack(base.DynamicVars.Damage.BaseValue).WithHitCount(hits).FromCard(this).Targeting(play.Target).Execute(ctx);
        }

        protected override void OnUpgrade()
        {
            base.DynamicVars.Damage.UpgradeValueBy(2m);
        }
    }
}


