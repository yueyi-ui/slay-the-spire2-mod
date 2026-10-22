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
    /// <summary>
    /// 比特涌动 - 0费，消耗自身，获得1比特，抽1张牌。
    /// 升级：获得2比特，抽2张牌。
    /// </summary>
    [Pool(typeof(WeijiaCardPool))]
    public class WeijiaBitSurge : WeijiaCard
    {
        public WeijiaBitSurge() : base(0, CardType.Skill, CardRarity.Common, TargetType.Self) { }

        protected override async Task OnPlay(PlayerChoiceContext ctx, CardPlay play)
        {
            int bits = IsUpgraded ? 2 : 1;
            WeijiaBits.Gain(this.Owner, bits);
            await CardPileCmd.Draw(ctx, bits, this.Owner.Creature.Player);
        }

        protected override void OnUpgrade() { }
    }
}
